namespace DataAccess.Models
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

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Issue> Issues { get; set; }
        public virtual DbSet<Paiment> Paiments { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Stack> Stacks { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<TeamStack> TeamStacks { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ActionLog> ActionLogs { get; set; }

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

            modelBuilder.Entity<Paiment>()
                .Property(e => e.PaymentDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Paiment>()
                .Property(e => e.SenderFullName)
                .IsUnicode(false);

            modelBuilder.Entity<Paiment>()
                .Property(e => e.RecieverFullName)
                .IsUnicode(false);

            modelBuilder.Entity<Paiment>()
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
                .HasMany(e => e.Tasks)
                .WithRequired(e => e.Project1)
                .HasForeignKey(e => e.Project)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.RoleName)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Role1)
                .HasForeignKey(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Stack>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Stack>()
                .Property(e => e.Tools)
                .IsUnicode(false);

            modelBuilder.Entity<Stack>()
                .HasMany(e => e.Tasks)
                .WithOptional(e => e.Stack1)
                .HasForeignKey(e => e.Stack);

            modelBuilder.Entity<Stack>()
                .HasMany(e => e.TeamStacks)
                .WithRequired(e => e.Stack1)
                .HasForeignKey(e => e.Stack)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Task>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .HasMany(e => e.Issues)
                .WithRequired(e => e.Task1)
                .HasForeignKey(e => e.Task)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Team>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.Projects)
                .WithOptional(e => e.Team1)
                .HasForeignKey(e => e.Team);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.TeamStacks)
                .WithRequired(e => e.Team1)
                .HasForeignKey(e => e.Team)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.Users)
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
                .Property(e => e.Sexe)
                .IsFixedLength()
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
                .HasMany(e => e.Addresses)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Issues)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.Issuer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ReceivedPaiments)
                .WithOptional(e => e.RecieverUser)
                .HasForeignKey(e => e.RecieverID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.SentPaiments)
                .WithOptional(e => e.SenderUser)
                .HasForeignKey(e => e.SenderID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Projects)
                .WithRequired(e => e.ProjectOwner)
                .HasForeignKey(e => e.Owner)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tasks)
                .WithOptional(e => e.DoneByUser)
                .HasForeignKey(e => e.DoneBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.LeadUsers)
                .WithOptional(e => e.UserLeader)
                .HasForeignKey(e => e.Leader);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ManagedUsers)
                .WithOptional(e => e.UserManager)
                .HasForeignKey(e => e.Manager);

            modelBuilder.Entity<ActionLog>()
                 .Property(e => e.UserName)
                 .IsUnicode(false);

            modelBuilder.Entity<ActionLog>()
                .Property(e => e.UserFullName)
                .IsUnicode(false);

            modelBuilder.Entity<ActionLog>()
                .Property(e => e.ActionName)
                .IsUnicode(false);

            modelBuilder.Entity<ActionLog>()
                .Property(e => e.ActionDATA)
                .IsUnicode(false);
        }
    }
}
