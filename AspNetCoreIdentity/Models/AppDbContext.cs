using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AspNetCoreIdentity.Models
{
    public class AppDbContext : IdentityDbContext<AppUser,AppRole,string>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

    }


    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Data Source=THENQLV;Initial Catalog=IdentityDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            return new AppDbContext(optionsBuilder.Options);

        }
    }

}
