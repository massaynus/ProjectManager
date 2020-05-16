namespace Windows_CLient.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices.WindowsRuntime;
    using TT = System.Threading.Tasks;

    public partial class Task
    {
        private int? stackid;

        public int TaskID { get; set; }
        public int? Project { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Priority { get; set; }
        public int? Difficulty { get; set; }
        public DateTime? DeadLine { get; set; }
        public int? Stack { get => stackid; set { stackid = value; TT.Task.Run(getStack); } }
        public bool? isBooked { get; set; }
        public bool? isComplete { get; set; }
        public int? DoneBy { get; set; }
        public virtual ICollection<Issue> Issues { get; set; }
        public Stack StackObject { get; set; }

        private async TT.Task<bool> getStack()
        {
            if (stackid is null) return false;
            var response = await APIClient.client.GetAsync(APIClient.API_HOST + $"Stacks/{stackid}");
            if (response.IsSuccessStatusCode)
            {
                var ResJSON = await response.Content.ReadAsStringAsync();
                StackObject = JsonConvert.DeserializeObject<Stack>(ResJSON);
                return true;
            }
            return false;
        }
    }
}
