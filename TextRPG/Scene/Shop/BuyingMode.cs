public class BuyingMode
{
    private Status status;
    private Inventory inventory;
    private Shop shop;

    private bool flag = false;

    public BuyingMode(Status existingStatus, Inventory existingInventory, Shop existingShop)
    {
        status = existingStatus;
        inventory = existingInventory;
        shop = existingShop;
    }

    private BuyResult BuyItem(Item item)
    {
        if (shop.purchasedItems.Contains(item))
            return BuyResult.AlreadyPurchased;

        if (status.Gold < item.ItemPrice)
            return BuyResult.NotEnoughGold;

        shop.purchasedItems.Add(item);
        inventory.inventoryItems.Add(item);
        status.Gold -= item.ItemPrice;

        return BuyResult.Success;
    }

    public ScreenState ScreenBuyingMode()
    {
        Console.Clear();
        ShowBuyingMode();
        return BuyingModeHandle();
    }

    private void ShowBuyingMode()
    {
        Console.WriteLine("[보유 골드]");
        Console.WriteLine(status.Gold + " G\n");
        Console.WriteLine("[아이템 목록]");
        for (int i = 0; i < shop.shopItems.Count; ++i)
        {
            Item item = shop.shopItems[i];
            string priceDisplay = shop.purchasedItems.Contains(item) ? "구매완료" : $"{item.ItemPrice} G";

            Console.WriteLine($"- {i + 1} {item.ItemName} | {item.ItemEffect.ToString()} +{item.ItemAbility} | {item.ItemDescription} | {priceDisplay}");
        }
        Console.WriteLine("\n0. 아이템 구매 취소");
        if (flag)
        {
            flag = false;
            Console.WriteLine("\n아이템을 구매 하였습니다.");
        }
        Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
    }

    private ScreenState BuyingModeHandle()
    {
        while (true)
        {
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int selectedIndex) && input == selectedIndex.ToString())
            {
                if (selectedIndex == 0)
                {
                    return ScreenState.Shop;
                }

                int itemIndex = selectedIndex - 1;

                if (itemIndex >= 0 && itemIndex < shop.shopItems.Count)
                {
                    BuyResult buyResult = BuyItem(shop.shopItems[itemIndex]);

                    switch (buyResult)
                    {
                        case BuyResult.AlreadyPurchased:
                            Console.WriteLine("이미 구매한 아이템입니다.");
                            break;
                        case BuyResult.NotEnoughGold:
                            Console.WriteLine("골드가 부족합니다.");
                            break;
                        case BuyResult.Success:
                            flag = true;
                            return ScreenState.BuyingMode;
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요!");
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요!");
            }
        }
    }
}