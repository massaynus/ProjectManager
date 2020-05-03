namespace DataAccess.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Issue")]
    public partial class Issue
    {
        public int IssueID { get; set; }

        public int? Task { get; set; }

        public int? Issuer { get; set; }

        [StringLength(30)]
        public string Title { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public bool? isSolved { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        [JsonIgnore]
        public virtual Task Task1 { get; set; }
    }
}
