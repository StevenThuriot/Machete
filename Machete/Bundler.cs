using System.Collections.Generic;
using System.Dynamic;

namespace Machete
{
    internal class Bundler : DynamicObject
    {
        private readonly List<CallInfo> _Calls;

        public string ServiceType { get; private set; }

        public IEnumerable<CallInfo> Calls
        {
            get { return _Calls.AsReadOnly(); }
        }

        public Bundler(string serviceType)
        {
            ServiceType = serviceType;
            _Calls = new List<CallInfo>();
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var callInfo = new CallInfo(binder.Name, args);
            _Calls.Add(callInfo);

            result = null; //Result doesn't matter as these are dummy calls anyway
            return true;
        }
    }
}