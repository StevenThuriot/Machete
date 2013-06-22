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

using System.Collections.Generic;
using System.Dynamic;

namespace Machete
{
    internal class Bundler : DynamicObject
    {
        public string ServiceType { get; private set; }

        public IEnumerable<CallInfo> Calls
        {
            get { return _Calls.AsReadOnly(); }
        }

        private readonly List<CallInfo> _Calls;

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