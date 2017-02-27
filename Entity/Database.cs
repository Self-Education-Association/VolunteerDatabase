namespace VolunteerDatabase.Entity
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Database : DbContext
    {
        public Database()
            : base("name=Database")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Volunteer>().Property(v => v.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Volunteer>().HasKey(v => v.UID).Property(v => v.UID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<AppUser>().HasMany(u => u.Roles).WithMany(r => r.Users);
            modelBuilder.Entity<AppUser>().HasRequired(u => u.Organization).WithMany(o => o.Members);
            modelBuilder.Entity<AppUser>().HasMany(u => u.AddedLogRecords).WithOptional(r=> r.Adder);
            modelBuilder.Entity<AppUser>().HasMany(u => u.TargetedBy).WithOptional(t => t.TargetAppUser);
           
            modelBuilder.Entity<AppUser>().HasMany(u => u.Project).WithMany(o => o.Managers);
            modelBuilder.Entity<Project>().HasMany(u => u.Volunteer).WithMany(r => r.Project);
            modelBuilder.Entity<Project>().HasMany(p => p.TargetedBy).WithOptional(t => t.TargetProject);

            modelBuilder.Entity<Volunteer>().HasMany(v => v.BlackListRecords).WithRequired(b => b.Volunteer);
            modelBuilder.Entity<Volunteer>().HasMany(v => v.Project).WithMany(p => p.Volunteer);
            modelBuilder.Entity<Volunteer>().HasMany(v => v.TargetedBy).WithOptional(t => t.TargetVolunteer);

            modelBuilder.Entity<BlackListRecord>().HasKey(b => b.UID).Property(v => v.UID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<BlackListRecord>().HasRequired(b => b.Organization).WithMany(o => o.BlackListRecords).WillCascadeOnDelete(false);
            modelBuilder.Entity<BlackListRecord>().HasRequired(b => b.Adder).WithMany(a => a.BlackListRecords);
            modelBuilder.Entity<BlackListRecord>().HasRequired(b => b.Project).WithMany(p => p.BlackListRecords);

            //modelBuilder.Entity<LogRecord>().HasKey(l => l.Id).Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

        public virtual DbSet<AppUser> Users { get; set; }

        public virtual DbSet<AppRole> Roles { get; set; }

        public virtual DbSet<Volunteer> Volunteers { get; set; }

        public virtual DbSet<BlackListRecord> BlackListRecords { get; set; }

        public virtual DbSet<Organization> Organizations { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<LogRecord> LogRecords { get; set; }

    }
}