namespace Windows_CLient.Models
{
    public partial class Address
    {
        public int AddressID { get; set; }
        public int? UserID { get; set; }
        public string street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
