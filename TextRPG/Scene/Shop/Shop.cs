public class Shop
{
    private Status status;
    public List<Item> shopItems;
    public List<Item> purchasedItems = new List<Item>();

    public Shop(Status existingStatus, List<Item> existingShopItems)
    {
        status = existingStatus;
        shopItems = existingShopItems;
    }

    public ScreenState ScreenShop()
    {
        Console.Clear();
        ShowShop();
        return ShopHandle();
    }

    private void ShowShop()
    {
        Console.WriteLine("[보유 골드]");
        Console.WriteLine(status.Gold + " G\n");
        Console.WriteLine("[아이템 목록]");
        foreach (Item item in shopItems)
        {
            string priceDisplay = purchasedItems.Contains(item) ? "구매완료" : $"{item.ItemPrice} G";

            Console.WriteLine($"- {item.ItemName} | {item.ItemEffect.ToString()} +{item.ItemAbility} | {item.ItemDescription} | {priceDisplay}");
        }
        Console.WriteLine("\n1. 아이템 구매");
        Console.WriteLine("2. 아이템 판매\n");
        Console.WriteLine("0. 나가기");
        Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
    }

    private ScreenState ShopHandle()
    {
        while (true)
        {
            string? input = Console.ReadLine();

            switch (input)
            {
                case "0":
                    return ScreenState.MainMenu;
                case "1":
                    return ScreenState.BuyingMode;
                case "2":
                    return ScreenState.SellingMode;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요!");
                    break;
            }
        }
    }
}