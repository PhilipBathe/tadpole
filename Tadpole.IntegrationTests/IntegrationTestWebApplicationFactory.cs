using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Tadpole.IntegrationTests
{
    public class IntegrationTestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup: class
    {
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(
                    new Dictionary<string, string>
                    {
                        ["ConnectionStrings:TadpoleConnection"] = Config.TestDatabaseConnectionString
                    });
            });
        }
    }

    public static class Config
    {
        //TODO: move this into config file
        public static string TestDatabaseConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Tadpole.Test.Database;Trusted_Connection=True;MultipleActiveResultSets=true";
    }

}
