using Dapper;
using FlightConnections.Data.Support.FlightRoutes;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;

namespace FlightConnections.IT.Steps
{
    [Binding]
    public class BaseStep : WebApplicationFactory<Startup>
    {
        protected readonly WebApplicationFactory<Startup> Factory;
        private readonly DbFlightRoutes FlightRoutes = new DbFlightRoutes();
        protected BaseStep(WebApplicationFactory<Startup> factory)
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.json");

            Factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, conf) =>
                {
                    conf.AddJsonFile(configPath);
                });
            });

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            config.GetSection("ConnectionStrings").Bind(FlightRoutes);
        }

        public void DeleteAll()
        {
            var sqlQuery = "DELETE from Connections" +
                            " DBCC CHECKIDENT ('[Connections]', RESEED, 0)";


            SqlConnection connection = new SqlConnection(FlightRoutes.ServerConnection);
            connection.Execute(sqlQuery);
        }
    }
}
