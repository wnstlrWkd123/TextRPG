public class Item
{
    public string ItemName { get; }
    public ItemValue ItemEffect { get; }
    public int ItemAbility { get; }
    public string ItemDescription { get; }
    public int ItemPrice { get; }

    public Item(string itemName, ItemValue itemEffect, int itemAbility, string itemDescription, int itemPrice)
    {
        ItemName = itemName;
        ItemEffect = itemEffect;
        ItemAbility = itemAbility;
        ItemDescription = itemDescription;
        ItemPrice = itemPrice;
    }
}