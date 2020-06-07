namespace Windows_CLient.Models
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public partial class Team
    {
        public int TeamID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<TeamStack> TeamStacks { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
