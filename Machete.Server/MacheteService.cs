#region License

// 
//  Copyright 2013 Steven Thuriot
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Machete.Contracts;

namespace Machete.Server
{
    public class MacheteService : IMacheteService
    {
        public IEnumerable<Response> Slash(Request request)
        {
            if (request.Questions == null || request.Questions.Count == 0)
                return Enumerable.Empty<Response>();


            var results = new BlockingCollection<Response>();

            //TODO: Optimize so instances get reused: 
            //var groupedQuestions = request.Questions.GroupBy(x => x.Type);

            var tasks = request.Questions.Select(question => 
                Task.Run(() =>
                {
                    var result = ResolveExpression(question)();

                    var response = new Response(result);

                    results.Add(response);
                })).ToList();

            Task.WhenAll(tasks).ContinueWith(x => HandleCompletion(tasks, results));

            return results.GetConsumingEnumerable();
        }

        private static void HandleCompletion(IEnumerable<Task> tasks, BlockingCollection<Response> results)
        {
            foreach (var task in tasks.Where(x => x.IsFaulted && x.Exception != null))
            {
// ReSharper disable PossibleNullReferenceException
                task.Exception.Handle(x => true);
// ReSharper restore PossibleNullReferenceException

                var response = new Response("Task failed: " + task.Exception) { Succeeded = false };
                results.Add(response);
            }

            results.CompleteAdding();
        }

        private static Func<dynamic> ResolveExpression(Question question)
        {
            var type = Type.GetType(question.Type);

            if (type == null)
                throw new NotSupportedException("The type '" + question.Type + "' is unknown on the server.");



            var constructor = type.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public,
                null,
                CallingConventions.HasThis,
                new Type[] { },
                new ParameterModifier[0]);

            var constructorCallExpression = Expression.New(constructor, new Expression[] { });

            var constants = question.Arguments.Select(Expression.Constant).Cast<Expression>().ToArray();
            var callExpression = Expression.Call(constructorCallExpression, question.Call, null, constants);

            var expression = Expression.Lambda<Func<dynamic>>(callExpression);

            //TODO: Cache expressions.
            return expression.Compile();
        }
    }
}
