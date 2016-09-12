namespace MockingJayRoutes
{
    public interface IHttpResponse
    {
        string ContentType { get; set; }
        int StatusCode { get; set; }

        void Close();
    }
}