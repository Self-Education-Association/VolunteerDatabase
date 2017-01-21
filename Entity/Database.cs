namespace VolunteerDatabase.Entity
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Database : DbContext
    {
        public Database()
            : base("name=Database")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppUser>().HasMany(u => u.Roles).WithMany(r => r.Users);
            modelBuilder.Entity<AppUser>().HasRequired(u => u.Organization).WithMany(o => o.Members);
            modelBuilder.Entity<AppUser>().HasMany(u => u.Project).WithMany(o => o.Managers);
            modelBuilder.Entity<Project>().HasMany(u => u.Volunteer).WithMany(r => r.Project);
        }

        public virtual DbSet<AppUser> Users { get; set; }

        public virtual DbSet<AppRole> Roles { get; set; }
        public virtual DbSet<Volunteer> Volunteers { get; set; }

        public virtual DbSet<Organization> Organizations { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

    }
}