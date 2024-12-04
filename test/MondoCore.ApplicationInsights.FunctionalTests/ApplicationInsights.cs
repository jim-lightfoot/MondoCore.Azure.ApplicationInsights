using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AppInsights = MondoCore.Azure.ApplicationInsights.ApplicationInsights;
using MondoCore.Log;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MondoCore.ApplicationInsights.FunctionalTests
{
    [TestClass]
    [TestCategory("Functional Tests")]
    public class ApplicationInsightsTests
    {
        private static string _correlationId = Guid.NewGuid().ToString().ToLower();

        [TestMethod]
        public async Task ApplicationInsights_WriteException()
        {
            var log = CreateAppInsights();
            var ex  = new System.Exception("Test Exception");

            ex.Source = "ApplicationInsights_WriteException";
             
            await log.WriteError(ex, properties: new { Make = "Chevy", Model = "Camaro", Year = 1969 }, correlationId: _correlationId);
        }

        [TestMethod]
        public async Task ApplicationInsights_WriteEvent()
        {
            var log = CreateAppInsights();
             
            await log.WriteEvent("Test Event", properties: new { Make = "Chevy", Model = "Camaro", Year = 1969 }, correlationId: _correlationId);
        }

        [TestMethod]
        public async Task ApplicationInsights_WriteError()
        {
            using(var log = SetupRequest("WriteError", true))
            { 
                log.SetProperty("Model", "Corvette");

                await log.WriteError(new Exception("Bob's hair is on fire"), properties: new {Make = "Chevy", Engine = new { Cylinders = 8, Displacement = 350, Piston = new { RodMaterial = "Chrome Moly", Material = "Stainless Steel", Diameter = 9200 } } } );
                await log.WriteError(new Exception("Fred's hair is on fire"), properties: new {Make = "Chevy" });
                await log.WriteError(new Exception("Wilma's hair is on fire"), properties: new {Make = "Chevy" });
            }
        }

        [TestMethod]
        public async Task ApplicationInsights_WriteError_dotted()
        {
            using(var log = SetupRequest("WriteError", false))
            { 
                log.SetProperty("Model", "Corvette");

                await log.WriteError(new Exception("Bob Dot's hair is on fire"), properties: new {Make = "Chevy", Engine = new { Cylinders = 8, Displacement = 350, Piston = new { RodMaterial = "Chrome Moly", Material = "Stainless Steel", Diameter = 9200 } } });
                await log.WriteError(new Exception("Fred Dot's hair is on fire"), properties: new {Make = "Chevy" });
                await log.WriteError(new Exception("Wilma Dot's hair is on fire"), properties: new {Make = "Chevy" });
            }
        }

        [TestMethod]
        public async Task ApplicationInsights_WriteError2()
        {
            using(var log = SetupRequest("WriteError2", true))
            { 
                log.SetProperty("Make", "Chevy");
                log.SetProperty("Model", "Corvette");

                await log.WriteEvent("John's feet are wet");
                await log.WriteError(new Exception("George's hair is on fire"));
                await log.WriteError(new Exception("Franks's hair is on fire"));
                await log.WriteError(new Exception("Naomi's hair is on fire"), Telemetry.LogSeverity.Critical);
            }
        }

        [TestMethod]
        public async Task ApplicationInsights_WriteError3()
        {
            using(var log = SetupRequest("WriteError3", true))
            { 
                 await log.WriteEvent("John's hair is on fire", properties: new {Make = "Chevy", Model = "Corvette" });
                 await log.WriteError(new Exception("Linda's hair is on fire"), properties: new {Make = "Chevy", Model = "Corvette" });
            }
        }

        [TestMethod]
        public async Task ApplicationInsights_WriteError_lowercase()
        {
            using(var log = SetupRequest("WriteError_lowercase", true))
            { 
                var make  = "Chevy";
                var model = "Camaro";
                var color = "Black";

                 await log.WriteEvent("Alexander's hair is on fire", properties: new {make, model, color});
                 await log.WriteError(new Exception("Melissa's hair is on fire"), properties: new {make, model, color});
            }
        }

        [TestMethod]
        public async Task ApplicationInsights_WriteAvailability()
        {
            using(var log = SetupRequest("WriteError3", true))
            { 
                 await log.WriteAvailability(new AvailabilityTelemetry
                 {
                   Message       = "My app is available",
                   Success       = true,
                   Properties    = new { Make = "Chevy", Model = "Silverado"},
                   Metrics       = new Dictionary<string, double> { {"Length", 128.0} },
                   TestId        = Guid.NewGuid().ToString(),
                   TestName      = "ApplicationInsights_WriteAvailability",
                   Sequence      = "1",
                   RunLocation   = "HobbitTown",
                   Duration      = new TimeSpan(100)
                 });
            }
        }

        #region Private

        private IRequestLog SetupRequest(string operationName, bool childrenAsJson = false)
        {
            var baseLog = new MondoCore.Log.Log();

            baseLog.Register(CreateAppInsights(childrenAsJson));

            return new RequestLog(baseLog, operationName, _correlationId);
        }

        private ILog CreateAppInsights(bool childrenAsJson = false)
        { 
            var config = TestConfiguration.Load();
            var tConfig = new TelemetryConfiguration { ConnectionString = config.InstrumentationKey };

            return new AppInsights(tConfig, childrenAsJson);
        }

        #endregion
    }

    internal static class TestConfiguration
    {
        public static Configuration Load()
        { 
            var path = Assembly.GetCallingAssembly().Location;
                path = path[..path.IndexOf("\\bin")];
            var json = File.ReadAllText(Path.Combine(path, "localhost.json"));

            return JsonConvert.DeserializeObject<Configuration>(json);
        }
    }

    internal class Configuration
    {
        public string InstrumentationKey        { get; set; } 
        public string ConnectionString          { get; set; }
        public string DataLakeConnectionString  { get; set; }
        public string ConfigConnectionString    { get; set; }

        public string KeyVaultUri               { get; set; }
        public string KeyVaultTenantId          { get; set; }
        public string KeyVaultClientId          { get; set; }
        public string KeyVaultClientSecret      { get; set; }     
    }    
}
