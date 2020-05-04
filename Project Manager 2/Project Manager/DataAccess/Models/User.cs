namespace DataAccess.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Addresses = new HashSet<Address>();
            Issues = new HashSet<Issue>();
            Projects = new HashSet<Project>();
            Tasks = new HashSet<Task>();
        }

        public int UserID { get; set; }

        [StringLength(35)]
        public string UserName { get; set; }

        [JsonIgnore]
        [StringLength(160)]
        public string Password { get; set; }

        [StringLength(35)]
        public string FirstName { get; set; }

        [StringLength(35)]
        public string LastName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BirthDtae { get; set; }

        [StringLength(15)]
        public string GSM { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(23)]
        public string RIB { get; set; }

        public int? Role { get; set; }

        public int? Team { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Address> Addresses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Issue> Issues { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Project> Projects { get; set; }

        public virtual Role Role1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> Tasks { get; set; }

        public virtual Team Team1 { get; set; }
    }
}
