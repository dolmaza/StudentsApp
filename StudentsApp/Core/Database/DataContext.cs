using Core.Database.Configurations;
using Data.Entities;
using System.Data.Entity;

namespace Core.Database
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("name=DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new StudentConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Student> Students { get; set; }
    }
}
