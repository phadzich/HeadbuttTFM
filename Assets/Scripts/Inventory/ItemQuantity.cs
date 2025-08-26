[System.Serializable]
public class ShopItem
{
    public Item item;
    public int quantity;
    public int price;

    public ShopItem Clone()
    {
        return new ShopItem
        {
            item = this.item,
            price = this.price,
            quantity = this.quantity
        };
    }
}