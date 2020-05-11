namespace DataAccess.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Project")]
    public partial class Project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Project()
        {
            Paiments = new HashSet<Paiment>();
            Tasks = new HashSet<Task>();
        }

        public int ProjectID { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Description { get; set; }

        public int Owner { get; set; }

        public int? Team { get; set; }

        public decimal? Budget { get; set; }

        public int Priority { get; set; }

        public int Complexity { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartinDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DueDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Paiment> Paiments { get; set; }

        [JsonIgnore]
        public virtual User ProjectOwner { get; set; }

        [JsonIgnore]
        public virtual Team Team1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
