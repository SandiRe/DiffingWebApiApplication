using DiffingWebApiApplication.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace DiffingWebApiApplication.Tests
{
    public class DiffingWebApiApplicationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }



        [Test]
        public async Task LeftValue_SendingDataToIndex1_OperationSuccessfull()
        {
            await using var application = new DiffingApplication();

            var client = application.CreateClient();
            var putResponse = await client.PutAsJsonAsync("/v1/diff/1/left", new DiffingData("AAAAAA=="));

            Assert.Equals(putResponse.StatusCode, HttpStatusCode.Created);

            var getResponse = await client.GetFromJsonAsync<DiffingData>("/v1/diff/1/left");

            Assert.Equals(putResponse.StatusCode, HttpStatusCode.OK);
            Assert.Equals(getResponse.Data, "AAAAAA==");
            
            //var difference = await client.GetFromJsonAsync<DiffingResultData>("/v1/diff/1", default, );

            //var difference = await client.GetAsync("/v1/diff/1");

            //Assert.Equals(difference.StatusCode, HttpStatusCode.NotFound);
        }


        /*
        [Fact]
        public async Task DeleteTodos()
        {
            await using var application = new TodoApplication();

            var client = application.CreateClient();
            var response = await client.PostAsJsonAsync("/todos", new Todo { Title = "I want to do this thing tomorrow" });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var todos = await client.GetFromJsonAsync<List<Todo>>("/todos");

            var todo = Assert.Single(todos);
            Assert.Equal("I want to do this thing tomorrow", todo.Title);
            Assert.False(todo.IsComplete);

            response = await client.DeleteAsync($"/todos/{todo.Id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            response = await client.GetAsync($"/todos/{todo.Id}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        */







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