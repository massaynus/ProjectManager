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
        [Column(Order = 0)]
        public int ActionID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(35)]
        public string UserName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(60)]
        public string UserFullName { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(35)]
        public string ActionName { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "text")]
        public string ActionDATA { get; set; }

        public DateTime? RequestDate { get; set; }
    }
}
