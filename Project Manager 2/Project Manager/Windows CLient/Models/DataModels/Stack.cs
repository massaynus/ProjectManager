namespace Windows_CLient.Models
{
    using System.Collections.Generic;
    public partial class Stack
    {
        public int StackID { get; set; }
        public string Name { get; set; }
        public string Tools { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<TeamStack> TeamStacks { get; set; }
    }
}
