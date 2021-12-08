using Microsoft.EntityFrameworkCore;

namespace DiffingWebApiApplication.Database
{
    public class DiffingDb : DbContext
    {
        public DiffingDb(DbContextOptions<DiffingDb> options)
            : base(options) { }

        public DbSet<DiffingItem> DiffingItems => Set<DiffingItem>();
    }
}