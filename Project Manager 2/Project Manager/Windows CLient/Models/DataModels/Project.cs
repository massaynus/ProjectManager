namespace Windows_CLient.Models
{
    using System;
    using System.Collections.Generic;
    public partial class Project
    {
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Owner { get; set; }
        public int? Team { get; set; }
        public decimal? Budget { get; set; }
        public int? Priority { get; set; }
        public int? Complexity { get; set; }
        public DateTime? StartinDate { get; set; }
        public DateTime? DueDate { get; set; }
        public virtual ICollection<Paiment> Paiments { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
