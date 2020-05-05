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

        [Required]
        [StringLength(70)]
        public string SenderFullName { get; set; }

        [Required]
        [StringLength(70)]
        public string RecieverFullName { get; set; }

        public decimal Amount { get; set; }

        public bool? isSalary { get; set; }

        public bool? isProjectPaiement { get; set; }

        public int? ProjectID { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public virtual Project Project { get; set; }
    }
}
