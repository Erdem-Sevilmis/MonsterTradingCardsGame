using SWE1.MessageServer.Core.Request;

namespace SWE1.MessageServer.Core.Routing
{
    public interface IRouter
    {
        IRouteCommand? Resolve(RequestContext request);
    }
}