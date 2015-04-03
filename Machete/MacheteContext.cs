using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Machete.Contracts;
using Machete.Metadata;

namespace Machete
{
    class p
    {
        static void main()
        {
            using (Machete.Scope)
            {
                var service = Machete.CreateService<IPersonalService>();

                /*int result1 = */service.Call1();
                /*var result2 = */service.Call2();
                /*var result3 = */service.Call3();

                var results = Machete.Slash();
                Response response1 = results.First();

                if (response1.Succeeded)
                {
                    var result1 = (int) response1.Answer;
                    //TODO
                }
            }
        }
    }

    internal class MacheteContext : IDisposable
    {
        private static readonly ThreadLocal<Stack<MacheteContext>> LocalScopeStack = new ThreadLocal<Stack<MacheteContext>>(() => new Stack<MacheteContext>());

        private readonly Func<dynamic> _ServiceCreator;
        private readonly Action _DisposeAction;
        private bool _Disposed;

        private readonly List<Bundler> _Bundlers = new List<Bundler>();
        

        /// <summary>
        /// Gets the scope stack.
        /// </summary>
        /// <value>
        /// The scope stack.
        /// </value>
        private static Stack<MacheteContext> ScopeStack
        {
            get { return LocalScopeStack.Value; }
        }

        public static MacheteContext CurrentContext
        {
            get { return ScopeStack.Count == 0 ? null : ScopeStack.Peek(); }
        }

        public MacheteContext()
        {
            var stack = ScopeStack;
            stack.Push(this);

            _DisposeAction = () => stack.Pop();
        }

        public MacheteContext(Func<dynamic> serviceCreator)
            : this()
        {
            _ServiceCreator = serviceCreator;
        }

        ~MacheteContext()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_Disposed) return;

            if (disposing)
            {
                _DisposeAction();
            }

            _Disposed = true;
        }

        public Bundler GetOrCreateBundler<T>()
        {
            string serviceType;

            var macheteContract = typeof (T).GetCustomAttribute<MacheteContractAttribute>(false);

            if (macheteContract != null)
            {
                serviceType = macheteContract.ServiceAssemblyQualifiedName;
            }
            else
            {
                throw new NotSupportedException("Only types marked with Machete attributes can be used.");
            }

            var bundler = _Bundlers.FirstOrDefault(x => x.ServiceType == serviceType);

            if (bundler == null)
            {
                bundler = new Bundler(serviceType);
                _Bundlers.Add(bundler);
            }

            return bundler;
        }

        public IEnumerable<Response> Slash()
        {
            //if (_ServiceCreator == null)
            //{
                //TODO: Default implementation / App.config / whatever : _ServiceCreator = () => new MacheteClient();
            //}

            using (var service = _ServiceCreator())
            {
                var request = new Request();

                foreach (var bundler in _Bundlers)
                {
                    foreach (var callInfo in bundler.Calls)
                    {
                        var question = new Question
                            {
                                Type = bundler.ServiceType,
                                Call = callInfo.MethodName,
                                Arguments = callInfo.Arguments
                            };

                        request.Questions.Add(question);
                    }
                }

                var responses = service.Slash(request);

                return responses == null
                           ? Enumerable.Empty<Response>()
                           : responses.ToList().AsReadOnly();
            }
        }
    }
}