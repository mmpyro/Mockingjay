namespace MockingJayRoutes
{
    public interface IHttpContext
    {
        IHttpRequest Request { get; set; }
        IHttpResponse Response { get; set; }
    }
}