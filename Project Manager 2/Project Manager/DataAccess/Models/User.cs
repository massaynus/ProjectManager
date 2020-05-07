namespace DataAccess.Models
{
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
            Paiments = new HashSet<Paiment>();
            Paiments1 = new HashSet<Paiment>();
            Projects = new HashSet<Project>();
            Tasks = new HashSet<Task>();
            User1 = new HashSet<User>();
            User11 = new HashSet<User>();
        }

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Address> Addresses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Issue> Issues { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Paiment> Paiments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Paiment> Paiments1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Project> Projects { get; set; }

        public virtual Role Role1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> Tasks { get; set; }

        public virtual Team Team1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> User1 { get; set; }

        public virtual User User2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> User11 { get; set; }

        public virtual User User3 { get; set; }
    }
}
