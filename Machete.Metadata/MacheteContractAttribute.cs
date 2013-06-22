﻿#region License

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

namespace Machete.Metadata
{
    [AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public class MacheteContractAttribute : Attribute
    {
        private readonly string _ServiceAssemblyQualifiedName;

        public string ServiceAssemblyQualifiedName
        {
            get { return _ServiceAssemblyQualifiedName; }
        }

        public MacheteContractAttribute(Type serviceType)
            : this(serviceType.AssemblyQualifiedName)
        {
        }

        public MacheteContractAttribute(string serviceAssemblyQualifiedName)
        {
            _ServiceAssemblyQualifiedName = serviceAssemblyQualifiedName;
        }
    }
}
