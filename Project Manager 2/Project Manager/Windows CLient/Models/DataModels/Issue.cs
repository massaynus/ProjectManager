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
    }
}
