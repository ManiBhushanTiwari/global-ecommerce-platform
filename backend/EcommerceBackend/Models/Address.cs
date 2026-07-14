namespace EcommerceBackend.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public bool IsDefault { get; set; }

        public virtual User User { get; set; } = null!;
    }

}
