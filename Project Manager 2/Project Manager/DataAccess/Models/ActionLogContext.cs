namespace DataAccess.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ActionLogContext : DbContext
    {
        public ActionLogContext()
            : base("name=Local_DB_Model")
        {
        }

        public virtual DbSet<ActionLog> ActionLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<ActionLog>()
                .Property(e => e.ActionMethod)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
