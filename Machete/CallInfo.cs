using System.Collections.Generic;
using System.Linq;

namespace Machete
{
    internal class CallInfo
    {
        public string MethodName { get; private set; }
        public IEnumerable<object> Arguments { get; private set; }

        public CallInfo(string methodName, IEnumerable<object> arguments)
        {
            MethodName = methodName;
            Arguments = arguments.ToList().AsReadOnly();
        }
    }
}