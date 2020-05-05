namespace DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TeamStack")]
    public partial class TeamStack
    {
        [Key]
        public int Num { get; set; }

        public int Team { get; set; }

        public int Stack { get; set; }

        public virtual Stack Stack1 { get; set; }

        public virtual Team Team1 { get; set; }
    }
}
