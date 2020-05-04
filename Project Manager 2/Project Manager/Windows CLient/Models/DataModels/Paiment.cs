namespace Windows_CLient.Models
{
    using System;
    public partial class Paiment
    {
        public int PaymentID { get; set; }
        public int? Project { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }
    }
}
