namespace BikeOnlineStore.Models
{
    public class ShoppingCartPlusBikeForShoppingCart
    {
        public ShoppingCart? shoppingCart { get; set; }

        public List<BikeForShoppingCart>? bikeForShoppingCart { get; set; }
    }
}
