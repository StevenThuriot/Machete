using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Machete.Contracts
{
    [DataContract]
    public class Request
    {
        public Request()
        {
            Questions = new List<Question>();
        }

        [DataMember]
        public IList<Question> Questions { get; private set; }
    }
}
