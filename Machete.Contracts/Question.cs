using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Machete.Contracts
{
    [DataContract]
    public class Question
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Call { get; set; }

        [DataMember]
        public IEnumerable<object> Arguments { get; set; }
    }
}
