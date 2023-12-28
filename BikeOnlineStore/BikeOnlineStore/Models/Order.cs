namespace BikeOnlineStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        public DateTime OrderDateTime { get; set; }

        public string? Status { get; set; }

        public string? DeliveryAddress { get; set; }

        public int UserId { get; set; }

        public string? BikeList { get; set; }
    }
}
