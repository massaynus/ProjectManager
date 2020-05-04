namespace Windows_CLient.Models
{
    using System;
    using System.Collections.Generic;
    public partial class Task
    {
        public int TaskID { get; set; }
        public int? Project { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Priority { get; set; }
        public int? Difficulty { get; set; }
        public DateTime? DeadLine { get; set; }
        public int? Stack { get; set; }
        public bool? isBooked { get; set; }
        public bool? isComplete { get; set; }
        public int? DoneBy { get; set; }
        public virtual ICollection<Issue> Issues { get; set; }
    }
}
