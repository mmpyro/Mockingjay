using System;
using System.Threading;
using CustomLogger;
using System.Threading.Tasks;
using System.Net;
using MockingJayRoutes;

namespace MockingJayServer
{
    public class HttpServer : IDisposable
    {
        private readonly ILogger _logger;
        private readonly RouteManager _route;
        private readonly HttpListener _listener;

        private static bool isRunning = false;
        private static object locker = new object();
        private static CancellationTokenSource source;
        private static CancellationToken token;
        

        public HttpServer(string address, HttpListener listener, RouteManager route, ILogger logger)
        {
            _logger = logger;
            _listener = listener;
            _route = route;
            _listener.Prefixes.Add(address);
        }

        public void StartListening()
        {
            lock (locker)
            {
                if(isRunning == false)
                {
                    _listener?.Start();
                    isRunning = true;
                    source = new CancellationTokenSource();
                    token = source.Token;
                    StartThread();
                }
            }
        }

        private void StartThread()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        var context = _listener.GetContext();
                        Log(context.Request);
                        _route.Resolve(new HttpContext(context));
                    }
                }
                catch { }
            }, TaskCreationOptions.LongRunning);
        }

        private void Log(HttpListenerRequest request)
        {
            _logger.Info($"Received: {request.Url.AbsoluteUri}, Method: {request.HttpMethod}");
        }

        public void Dispose()
        {
            lock (locker)
            {
                if (isRunning)
                {
                    source.Cancel();
                    _listener?.Close();
                    isRunning = false;
                }
            }
        }
    }
}