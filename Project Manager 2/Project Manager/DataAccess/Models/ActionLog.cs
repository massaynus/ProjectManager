namespace DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ActionLog")]
    public partial class ActionLog
    {
        [Key]
        public int ActionID { get; set; }

        [Required]
        [StringLength(35)]
        public string UserName { get; set; }

        [Required]
        [StringLength(60)]
        public string UserFullName { get; set; }

        [Required]
        [StringLength(35)]
        public string ActionName { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string ActionDATA { get; set; }

        public DateTime? RequestDate { get; set; }

        [StringLength(8)]
        public string ActionMethod { get; set; }
    }
}
