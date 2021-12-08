using DiffingWebApiApplication;
using DiffingWebApiApplication.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace DiffingWebApi.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }

        [Fact]
        public async Task LeftValue_SendingDataToIndex1_OperationSuccessfull()
        {
            await using DiffingApplication? application = new DiffingApplication();

            var client = application.CreateClient();
            var putResponse = await client.PutAsJsonAsync("/v1/diff/1/left", new DiffingData("AAAAAA=="));

            Assert.Equal(HttpStatusCode.Created, putResponse.StatusCode);

            var getResponse = await client.GetFromJsonAsync<DiffingData>("/v1/diff/1/left");

            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
            Assert.Equal("AAAAAA==", getResponse.Data);

            //var difference = await client.GetFromJsonAsync<DiffingResultData>("/v1/diff/1", default, );

            //var difference = await client.GetAsync("/v1/diff/1");

            //Assert.Equals(difference.StatusCode, HttpStatusCode.NotFound);
        }
    }

    class DiffingApplication : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var root = new InMemoryDatabaseRoot();

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<DiffingDb>));

                services.AddDbContext<DiffingDb>(options =>
                    options.UseInMemoryDatabase("TestingDiffingItemDatabase", root));
            });

            return base.CreateHost(builder);
        }
    }
}