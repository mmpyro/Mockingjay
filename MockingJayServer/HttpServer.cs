using System;
using System.Threading;
using CustomLogger;
using System.Threading.Tasks;
using System.Net;
using MockingJayRoutes;
using MockingJay;

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
        private readonly ReportGenerator _reportGenerator;

        public HttpServer(string address, HttpListener listener, RouteManager route, ILogger logger, ReportGenerator reportGenerator)
        {
            _logger = logger;
            _listener = listener;
            _route = route;
            _reportGenerator = reportGenerator;
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
                        var httpContext = new HttpContext(context);
                        Log(httpContext.Request);
                        _route.Resolve(httpContext);
                    }
                }
                catch(Exception ex) {
                    if (!ex.Message.Equals("The I/O operation has been aborted because of either a thread exit or an application request"
                        , StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.Error(ex);
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        private void Log(IHttpRequest request)
        {
            _logger.Info(_reportGenerator.CreateReport(request));
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