namespace Local_Server_API.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Address")]
    public partial class Address
    {
        public int AddressID { get; set; }

        public int? UserID { get; set; }

        [StringLength(120)]
        public string street { get; set; }

        [StringLength(8)]
        public string ZipCode { get; set; }

        [StringLength(20)]
        public string City { get; set; }

        [StringLength(20)]
        public string State { get; set; }

        [StringLength(20)]
        public string Country { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
