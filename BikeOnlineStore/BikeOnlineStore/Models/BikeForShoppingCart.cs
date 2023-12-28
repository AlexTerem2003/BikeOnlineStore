namespace BikeOnlineStore.Models
{
    public class BikeForShoppingCart
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; } = 0;

        public int PriceMultiplyQuantity { get; set; }

        public string? CoverImagePath { get; set; }
    }
}
