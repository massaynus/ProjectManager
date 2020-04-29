namespace Local_Server_API.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Task")]
    public partial class Task
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Task()
        {
            Issue = new HashSet<Issue>();
        }

        public int TaskID { get; set; }

        public int? Project { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public int? Priority { get; set; }

        public int? Difficulty { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DeadLine { get; set; }

        public int? Stack { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Issue> Issue { get; set; }

        [JsonIgnore]
        public virtual Project Project1 { get; set; }

        public virtual Stack Stack1 { get; set; }
    }
}
