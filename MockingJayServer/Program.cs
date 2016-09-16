using CustomLogger;
using MockingJayRoutes;
using System;
using System.Net;
using MockingJay.Controllers;
using static System.Console;
using MockingJay;
using MockingJay.Validation;
using MockingJayRoutes.helpers;

namespace MockingJayServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger logger = new GenericLogger();
            string hostName = Dns.GetHostName();
            string url = CreateUrl(args);
            try
            {
                var app = new MockingJayApp(new MockEngine(), new ComplexValidator(
                                                                new UrlValidator(),
                                                                new NullResponseValidator(),
                                                                new StatusValidator()));

                var route = new RouteManager();
                route.Add($"{url}mockingJay/register", new RegisterController(app));
                route.Add($"{url}mockingJay/remove/all", new RemoveAllController(app));
                route.Add($"{url}mockingJay/remove", new RemoveController(app));
                route.Add($"{url}mockingJay/configurations", new GetAllConfigurationController(app, 
                                                                                        new ParserFactory(),
                                                                                        new PageBuilder<Configuration>()));
                route.Add($"{url}mockingJay/requests", new GetAllRequestsController(app, new ParserFactory() 
                                                                                        ,new PageBuilder<Request>()));
                route.Add("*", new FillController(app));

                using (var server = new HttpServer(url,
                    new HttpListener(), route , logger, new ReportGenerator()))
                {
                    server.StartListening();
                    logger.Info($"Server listen on: {url}");
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

        private static string CreateUrl(string[] args)
        {
            if(args.Length == 1 && args[0] == "-h")
            {
                string hostName = Dns.GetHostName();
                return $"http://{hostName}:51111/";
            }
            return "http://localhost:51111/";
        }
    }
}
