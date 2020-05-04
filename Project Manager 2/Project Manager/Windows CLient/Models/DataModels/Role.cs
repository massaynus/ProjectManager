namespace Windows_CLient.Models
{
    using System.Collections.Generic;
    public partial class Role
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }

        public const string Manager = "Manager";
        public const string TeamLeader = "TeamLeader";
        public const string Member = "Member";
        public const string Client = "Client";
    }
}
