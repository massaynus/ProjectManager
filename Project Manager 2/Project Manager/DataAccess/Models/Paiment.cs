namespace DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Paiment
    {
        [Key]
        public int PaymentID { get; set; }

        public int? Project { get; set; }

        public decimal? Amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public virtual Project Project1 { get; set; }
    }
}
