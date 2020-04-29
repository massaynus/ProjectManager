namespace Local_Server_API.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Local_DB_Model : DbContext
    {
        public Local_DB_Model()
            : base("name=Local_DB_Model")
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Issue> Issue { get; set; }
        public virtual DbSet<Paiments> Paiments { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Stack> Stack { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<TeamStack> TeamStack { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .Property(e => e.street)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.ZipCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<Issue>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Issue>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Paiments>()
                .Property(e => e.Amount)
                .HasPrecision(9, 2);

            modelBuilder.Entity<Project>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Project>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Project>()
                .Property(e => e.Budget)
                .HasPrecision(9, 2);

            modelBuilder.Entity<Project>()
                .HasMany(e => e.Paiments)
                .WithOptional(e => e.Project1)
                .HasForeignKey(e => e.Project);

            modelBuilder.Entity<Project>()
                .HasMany(e => e.Task)
                .WithOptional(e => e.Project1)
                .HasForeignKey(e => e.Project);

            modelBuilder.Entity<Role>()
                .Property(e => e.RoleName)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.User)
                .WithOptional(e => e.Role1)
                .HasForeignKey(e => e.Role);

            modelBuilder.Entity<Stack>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Stack>()
                .Property(e => e.Tools)
                .IsUnicode(false);

            modelBuilder.Entity<Stack>()
                .HasMany(e => e.Task)
                .WithOptional(e => e.Stack1)
                .HasForeignKey(e => e.Stack);

            modelBuilder.Entity<Stack>()
                .HasMany(e => e.TeamStack)
                .WithOptional(e => e.Stack1)
                .HasForeignKey(e => e.Stack);

            modelBuilder.Entity<Task>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .HasMany(e => e.Issue)
                .WithOptional(e => e.Task1)
                .HasForeignKey(e => e.Task);

            modelBuilder.Entity<Team>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.Project)
                .WithOptional(e => e.Team1)
                .HasForeignKey(e => e.Team);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.TeamStack)
                .WithOptional(e => e.Team1)
                .HasForeignKey(e => e.Team);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.User)
                .WithOptional(e => e.Team1)
                .HasForeignKey(e => e.Team);

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.GSM)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.RIB)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Issue)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.Issuer);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Project)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.Owner);
        }
    }
}
