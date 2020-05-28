namespace Windows_CLient.Models
{
    public partial class Issue
    {
        public int IssueID { get; set; }
        public int? Task { get; set; }
        public int? Issuer { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? isSolved { get; set; }

        public Task IssueTask { get; set; } = null;
        public User IssuerObj { get; set; } = null;
    }
}
