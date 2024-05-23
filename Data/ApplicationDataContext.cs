using Kill_hunger.Models;
using Microsoft.EntityFrameworkCore;

namespace Kill_hunger.Data
{
    public class ApplicationDataContext:DbContext
    {

        protected readonly IConfiguration Configuration;
        private readonly string connectionString;

        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options, IConfiguration configuration) : base(options)
        {
            connectionString = configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
        }
         protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings
            options.UseSqlServer(connectionString);
        }

        public DbSet<FileDetails> FileDetails { get; set; }
        public DbSet<User> users { get; set; }
       
    }
}
