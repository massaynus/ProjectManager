namespace Windows_CLient.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices.WindowsRuntime;
    using TT = System.Threading.Tasks;

    public partial class Task
    {
        public int TaskID { get; set; }
        public int? Project { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Priority { get; set; }
        public int? Difficulty { get; set; }
        public DateTime? DeadLine { get; set; }
        public int? Stack { get ; set ;  }
        public bool? isBooked { get; set; }
        public bool? isComplete { get; set; }
        [JsonIgnore]
        public bool HasIssues { get => Issues.Count > 0; }
        public int? DoneBy { get; set; }
        public virtual ICollection<Issue> Issues { get; set; }
        public Stack Stack1 { get; set; }

    }
}
