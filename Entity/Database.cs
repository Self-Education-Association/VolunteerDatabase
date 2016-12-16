namespace VolunteerDatabase.Entity
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class Database : IdentityDbContext<AppUser>
    {
        public Database()
            : base("name=Database")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppUser>().HasRequired(a => a.Organization).WithMany(o => o.Members);
            modelBuilder.Entity<Organization>().HasRequired(o => o.Administrator);
        }

        public virtual DbSet<Organization> Organizations { get; set; }
    }
}