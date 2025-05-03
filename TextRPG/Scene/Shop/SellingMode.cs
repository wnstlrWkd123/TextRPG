public class SellingMode
{
    private Status status;
    private Inventory inventory;
    private FittingMode fittingMode;

    private bool flag = false;

    public SellingMode(Status existingStatus, Inventory existingInventory, FittingMode existingFittingMode)
    {
        status = existingStatus;
        inventory = existingInventory;
        fittingMode = existingFittingMode;
    }

    private void SellItem(Item item)
    {
        if (inventory.equippedItems.Contains(item))
        {
            fittingMode.OffEquip(item);
        }

        status.Gold += (int)(item.ItemPrice * 0.85f);
        inventory.inventoryItems.Remove(item);
    }

    public ScreenState ScreenSellingMode()
    {
        Console.Clear();
        ShowSellingMode();
        return SellingModeHandle();
    }

    private void ShowSellingMode()
    {
        Console.WriteLine("[보유 골드]");
        Console.WriteLine(status.Gold + " G\n");
        Console.WriteLine("[아이템 목록]");
        if (!inventory.inventoryItems.Any())
        {
            Console.WriteLine("판매 가능한 아이템이 없습니다.");
        }
        else
        {
            for (int i = 0; i < inventory.inventoryItems.Count; ++i)
            {
                Item item = inventory.inventoryItems[i];

                Console.WriteLine($"- {i + 1} {item.ItemName} | {item.ItemEffect.ToString()} +{item.ItemAbility} | {item.ItemDescription} | {(int)(item.ItemPrice * 0.85f)}");
            }
        }
        Console.WriteLine("\n0. 아이템 판매 취소");
        if (flag)
        {
            flag = false;
            Console.WriteLine("\n아이템을 판매 하였습니다.");
        }
        Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
    }

    private ScreenState SellingModeHandle()
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

                if (itemIndex >= 0 && itemIndex < inventory.inventoryItems.Count)
                {
                    SellItem(inventory.inventoryItems[itemIndex]);
                    flag = true;

                    return ScreenState.SellingMode;
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