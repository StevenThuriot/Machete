using System.Runtime.Serialization;

namespace Machete.Contracts
{
    [DataContract]
    public class Response
    {
        public Response(object response)
        {
            Answer = response;
            Succeeded = true;
        }

        [DataMember]
        public object Answer { get; set; }

        [DataMember]
        public bool Succeeded { get; set; }
    }
}
