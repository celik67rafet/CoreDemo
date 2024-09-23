using Microsoft.EntityFrameworkCore;

namespace BlogApiDemo.DataAccessLayer
{
    public class Context: DbContext
    {
        protected override void OnConfiguring( DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlServer("server=MYPC;database=CoreBlogApiDb; integrated security=true; TrustServerCertificate=True;");
            //trustservercertificate biz ekledik true, migration esnasında gelen hatayı engelledi.
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
