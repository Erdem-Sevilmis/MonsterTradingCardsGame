using SWE1.MessageServer.Core.Request;

namespace SWE1.MessageServer.Core.Client
{
    public interface IClient
    {
        public RequestContext? ReceiveRequest();
        public void SendResponse(Response.Response response);
        public void Close();
    }
}
