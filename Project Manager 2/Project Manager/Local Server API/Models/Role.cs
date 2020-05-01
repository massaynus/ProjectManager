namespace Local_Server_API.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Role")]
    public partial class Role
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Role()
        {
            User = new HashSet<User>();
        }

        public int RoleID { get; set; }

        [StringLength(25)]
        public string RoleName { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> User { get; set; }

        public const string Manager = "Manager";
        public const string TeamLeader = "TeamLeader";
        public const string Member = "Member";
        public const string Client = "Client";
        public const string Developper = "Developper";
    }
}
