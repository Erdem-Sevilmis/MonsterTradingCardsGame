using SWE1.MessageServer.Core.Request;
using HttpMethod = SWE1.MessageServer.Core.Request.HttpMethod;

namespace SWE1.MessageServer.API.RouteCommands
{
    internal interface IRouteParser
    {
        bool IsMatch(string resourcePath, string routePattern);
        Dictionary<string, string> ParseParameters(string resourcePath, string routePattern);
    }
}
