using Microsoft.EntityFrameworkCore;
using WebAPI_Template_Test1_MA.Models;

namespace WebAPI_Template_Test1_MA.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }

        public DbSet<Person> Persons { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>().HasData(new Person()
            {
                PersonId = 1,
                FirstName = "Mohsin",
                LastName = "Ahmed",
                Designation = "Software Engineer"
            });

            modelBuilder.Entity<Person>().HasData(new Person()
            {
                PersonId = 2,
                FirstName = "Mohammed",
                LastName = "Mohalil",
                Designation = "Software Engineer"
            });
        }

    }
}
