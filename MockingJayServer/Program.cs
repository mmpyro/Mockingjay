using CustomLogger;
using MockingJayRoutes;
using System;
using System.Net;
using MockingJay.Controllers;
using static System.Console;
using MockingJay;
using MockingJay.Validation;

namespace MockingJayServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger logger = new GenericLogger();
            const string url = "http://localhost:51111/mockingJay/";
            try
            {
                var app = new MockingJayApp(new MockEngine(), new ComplexValidator(
                                                                new UrlValidator(),
                                                                new NullResponseValidator(),
                                                                new StatusValidator()));

                var route = new RouteManager();
                route.Add($"{url}register", new RegisterController(app));
                route.Add($"{url}remove/all", new RemoveAllController(app));
                route.Add($"{url}remove", new RemoveController(app));
                route.Add("*", new FillController(app));

                using (var server = new HttpServer(url,
                    new HttpListener(), route , logger))
                {
                    server.StartListening();
                    WriteLine("To stop server press any key...");
                    ReadKey();
                }
            }
            catch(Exception ex)
            {
                logger?.Fatal(ex);
                ReadKey();
            }
        }
    }
}
