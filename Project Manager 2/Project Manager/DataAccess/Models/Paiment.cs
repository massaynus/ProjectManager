namespace DataAccess.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Paiment
    {
        [Key]
        public int PaymentID { get; set; }

        [Column(TypeName = "text")]
        public string PaymentDescription { get; set; }

        [Required]
        [StringLength(70)]
        public string SenderFullName { get; set; }

        public int? SenderID { get; set; }

        [Required]
        [StringLength(70)]
        public string RecieverFullName { get; set; }

        public int? RecieverID { get; set; }

        public decimal Amount { get; set; }

        public bool? isSalary { get; set; }

        public bool? isProjectPaiement { get; set; }

        public int? ProjectID { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [JsonIgnore]
        public virtual Project Project { get; set; }

        [JsonIgnore]
        public virtual User RecieverUser { get; set; }

        [JsonIgnore]
        public virtual User SenderUser { get; set; }
    }
}
