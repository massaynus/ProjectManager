namespace DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Issue")]
    public partial class Issue
    {
        public int IssueID { get; set; }

        public int Task { get; set; }

        public int Issuer { get; set; }

        [Required]
        [StringLength(30)]
        public string Title { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Description { get; set; }

        public bool? isSolved { get; set; }

        public virtual User User { get; set; }

        public virtual Task Task1 { get; set; }
    }
}
