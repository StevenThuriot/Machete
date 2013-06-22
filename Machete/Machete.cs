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
using System.Collections.Generic;
using ImpromptuInterface;
using Machete.Contracts;

namespace Machete
{
    public static class Machete
    {
        public static IDisposable Scope
        {
            get { return new MacheteContext(); }
        }

        public static IDisposable CreateScope()
        {
            return new MacheteContext();
        }

        public static IDisposable CreateScope(Func<IMacheteClientService> serviceCreator)
        {
            return new MacheteContext(serviceCreator);
        }

        public static IDisposable Current
        {
            get { return MacheteContext.CurrentContext; }
        }

        public static T CreateService<T>() where T : class
        {
            var currentContext = MacheteContext.CurrentContext;

            if (currentContext == null)
                throw new NotSupportedException("Machete must have an active scope to work in.");

            var bundler = currentContext.GetOrCreateBundler<T>();
            var impromptuBundler = bundler.ActLike<T>();

            return impromptuBundler;
        }

        public static IEnumerable<Response> Slash()
        {
            var currentContext = MacheteContext.CurrentContext;

            if (currentContext == null)
                throw new NotSupportedException("Machete must have an active scope to work in.");

            return currentContext.Slash();
        }
    }
}