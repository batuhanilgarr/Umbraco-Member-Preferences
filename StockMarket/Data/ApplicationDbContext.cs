using Microsoft.EntityFrameworkCore;

namespace StockMarket.Data {
    using Models;

    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {}

        public DbSet<UserPreferences> UserPreferences { get; set; }
    }

}
