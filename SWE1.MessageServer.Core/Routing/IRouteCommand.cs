namespace SWE1.MessageServer.Core.Routing
{
    public interface IRouteCommand
    {
        Response.Response Execute();
    }
}
