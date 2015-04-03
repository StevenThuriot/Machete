using System.Collections.Generic;
using System.ServiceModel;
using Machete.Contracts;

namespace Machete.Server
{
    [ServiceContract]
    public interface IMacheteService
    {
        [OperationContract]
        IEnumerable<Response> Slash(Request request);
    }
}
