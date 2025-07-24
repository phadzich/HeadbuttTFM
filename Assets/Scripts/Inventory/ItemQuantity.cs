[System.Serializable]
public class ShopItem
{
    public Item item;
    public int quantity;
    public ResourceRequirement price;

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