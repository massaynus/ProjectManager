namespace Windows_CLient.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public partial class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDtae { get; set; }
        public string Sexe { get; set; }
        public string GSM { get; set; }
        public string Email { get; set; }
        public string RIB { get; set; }
        public int Role { get; set; }
        public int? Team { get; set; }
        public bool isAccountActive { get; set; }
        public int? Leader { get; set; }
        public int? Manager { get; set; }
        public virtual Role Role1 { get; set; }
        public virtual Team Team1 { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Issue> Issues { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }


        
        [JsonIgnore]
        public string FullName { get => $"{LastName} {FirstName}"; }

        [JsonIgnore]
        public int TasksCompletedCount { get => Tasks.Count(T => T.DoneBy == UserID && T.isComplete == true); }

        [JsonIgnore]
        public int TasksBookedCount { get => Tasks.Count(T => T.DoneBy == UserID && !(T.isComplete ?? false)); }

        [JsonIgnore]
        public int IssuesCount { get => Issues.Count(I => I.Issuer == UserID); }
    }
}
