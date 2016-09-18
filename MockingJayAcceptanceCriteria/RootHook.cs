using BoDi;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using TechTalk.SpecFlow;

namespace MockingJayAcceptanceCriteria
{
    [Binding]
    public sealed class RootHook
    {
        private readonly IObjectContainer _container;
        private string _programPath;
        private Process _process;

        public RootHook(IObjectContainer container)
        {
            _container = container;
            _programPath = ConfigurationManager.AppSettings["programPath"];
        }

        [BeforeScenario]
        public void BeforeScenarion()
        {
            _container.RegisterInstanceAs(new HttpClient());
            _process = Process.Start(new ProcessStartInfo
            {
                FileName = _programPath,
                CreateNoWindow = true,
                UseShellExecute = false
            });
            Thread.Sleep(TimeSpan.FromSeconds(0.5));
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (_process != null && _process.HasExited == false)
            {
                _process.Kill();
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }
        }
    }
}
