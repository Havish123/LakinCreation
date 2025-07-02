namespace AashaGifts.Web.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public string UploadedPhotoPath { get; set; } = default!;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending";
        public string? UserId { get; set; }
    }
}
