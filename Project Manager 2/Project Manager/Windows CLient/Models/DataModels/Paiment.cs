namespace Windows_CLient.Models
{
    using System;
    public partial class Paiment
    {
        public int PaymentID { get; set; }
        public string PaymentDescription { get; set; }
        public string SenderFullName { get; set; }
        public int? SenderID { get; set; }
        public string RecieverFullName { get; set; }
        public int? RecieverID { get; set; }
        public decimal Amount { get; set; }
        public bool? isSalary { get; set; }
        public bool? isProjectPaiement { get; set; }
        public int? ProjectID { get; set; }
        public DateTime Date { get; set; }
    }
}
