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
            ReceivedPaiments = new HashSet<Paiment>();
            SentPaiments = new HashSet<Paiment>();
            Projects = new HashSet<Project>();
            Tasks = new HashSet<Task>();
            LeadUsers = new HashSet<User>();
            ManagedUsers = new HashSet<User>();
        }

        #region User Properties
        public int UserID { get; set; }

        [Required]
        [StringLength(35)]
        public string UserName { get; set; }

        [Required]
        [StringLength(160)]
        public string Password { get; set; }

        [Required]
        [StringLength(35)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(35)]
        public string LastName { get; set; }

        [Column(TypeName = "date")]
        public DateTime BirthDtae { get; set; }

        [Required]
        [StringLength(1)]
        public string Sexe { get; set; }

        [Required]
        [StringLength(15)]
        public string GSM { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        [StringLength(23)]
        public string RIB { get; set; }

        public int Role { get; set; }

        public int? Team { get; set; }

        public bool isAccountActive { get; set; }

        public int? Leader { get; set; }

        public int? Manager { get; set; }
        #endregion


        #region User External Entities

        #region User Data (Adresses / Role / Team)
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Address> Addresses { get; set; }

        public virtual Role Role1 { get; set; }

        public virtual Team Team1 { get; set; }
        #endregion

        #region User Work (Tasks / Issues)
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Issue> Issues { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> Tasks { get; set; }
        #endregion

        #region Payments & Projects

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Paiment> ReceivedPaiments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Paiment> SentPaiments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Project> Projects { get; set; }
        #endregion

        #region Hierarchy stuff
        [JsonIgnore]
        public virtual User UserManager { get; set; }

        [JsonIgnore]
        public virtual User UserLeader { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> LeadUsers { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> ManagedUsers { get; set; }
        #endregion


        #endregion
    }
}
