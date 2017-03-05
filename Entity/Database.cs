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
            modelBuilder.Entity<AppUser>().HasMany(u => u.Roles).WithMany(r => r.Users);
            modelBuilder.Entity<AppUser>().HasRequired(u => u.Organization).WithMany(o => o.Members).WillCascadeOnDelete(false);
            modelBuilder.Entity<AppUser>().HasMany(u => u.Underlings).WithMany(u => u.Superiors);

            modelBuilder.Entity<AppUser>().HasMany(u => u.Projects).WithMany(o => o.Managers);
            modelBuilder.Entity<Project>().HasMany(u => u.Volunteers).WithMany(r => r.Project);
            modelBuilder.Entity<Project>().HasMany(u => u.CreditRecords).WithRequired(u => u.Project).WillCascadeOnDelete(true);

            modelBuilder.Entity<Volunteer>().HasKey(v => v.UID).Property(v => v.UID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Volunteer>().HasMany(v => v.BlackListRecords).WithRequired(b => b.Volunteer).WillCascadeOnDelete(true);
            modelBuilder.Entity<Volunteer>().HasMany(v => v.Project).WithMany(p => p.Volunteers);
            modelBuilder.Entity<Volunteer>().HasMany(v => v.CreditRecords).WithRequired(c => c.Participant).WillCascadeOnDelete(true);


            modelBuilder.Entity<LogRecord>().HasRequired(l => l.Adder).WithMany(a => a.AddedLogRecords).WillCascadeOnDelete(true);
            modelBuilder.Entity<LogRecord>().HasOptional(l => l.TargetAppUser).WithMany(t => t.TargetedBy);
            modelBuilder.Entity<LogRecord>().HasOptional(l => l.TargetProject).WithMany(t => t.TargetedBy);
            modelBuilder.Entity<LogRecord>().HasOptional(l => l.TargetVolunteer).WithMany(t => t.TargetedBy);

            modelBuilder.Entity<Organization>().HasMany(o => o.BlackListRecords).WithRequired(o => o.Organization);

            modelBuilder.Entity<BlackListRecord>().HasKey(b => b.UID).Property(v => v.UID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //modelBuilder.Entity<BlackListRecord>().HasRequired(b => b.Organization).WithMany(o => o.BlackListRecords);//这里为何不能WillCascedeOnDelete
            modelBuilder.Entity<BlackListRecord>().HasRequired(b => b.Adder).WithMany(a => a.BlackListRecords).WillCascadeOnDelete(true);
            modelBuilder.Entity<BlackListRecord>().HasRequired(b => b.Project).WithMany(p=>p.BlackListRecords).WillCascadeOnDelete(true);

            //modelBuilder.Entity<LogRecord>().HasKey(l => l.Id).Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

        public virtual DbSet<AppUser> Users { get; set; }

        public virtual DbSet<AppRole> Roles { get; set; }

        public virtual DbSet<Volunteer> Volunteers { get; set; }

        public virtual DbSet<BlackListRecord> BlackListRecords { get; set; }

        public virtual DbSet<Organization> Organizations { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<LogRecord> LogRecords { get; set; }

        public virtual DbSet<ApprovalRecord> ApprovalRecords { get; set; }

        public virtual DbSet<CreditRecord> CreditRecords { get; set; }

    }
}