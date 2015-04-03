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
