using DiffingWebApiApplication.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace DiffingWebApiApplication.Tests
{
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