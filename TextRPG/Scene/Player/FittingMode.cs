public class FittingMode
{
    private Status status;
    private Inventory inventory;

    private bool flagA = false;
    private bool flagB = false;

    public FittingMode(Status existingStatus, Inventory existingInventory)
    {
        status = existingStatus;
        inventory = existingInventory;
    }

    private void OnEquip(Item item)
    {
        inventory.equippedItems.Add(item);

        switch (item.ItemEffect)
        {
            case ItemValue.Power:
                if (inventory.equippedPowerItem != null)
                {
                    OffEquip(inventory.equippedPowerItem);
                }
                inventory.equippedPowerItem = item;
                status.Power += item.ItemAbility;
                break;
            case ItemValue.Armor:
                if (inventory.equippedArmorItem != null)
                {
                    OffEquip(inventory.equippedArmorItem);
                }
                inventory.equippedArmorItem = item;
                status.Armor += item.ItemAbility;
                break;
            case ItemValue.Hp:
                if (inventory.equippedHpItem != null)
                {
                    OffEquip(inventory.equippedHpItem);
                }
                inventory.equippedHpItem = item;
                status.Hp += item.ItemAbility;
                status.CurrentHp += item.ItemAbility;
                break;
            default:
                Console.WriteLine("오류입니다.");
                break;
        }
    }

    public void OffEquip(Item item)
    {
        inventory.equippedItems.Remove(item);

        switch (item.ItemEffect)
        {
            case ItemValue.Power:
                inventory.equippedPowerItem = null;
                status.Power -= item.ItemAbility;
                break;
            case ItemValue.Armor:
                inventory.equippedArmorItem = null;
                status.Armor -= item.ItemAbility;
                break;
            case ItemValue.Hp:
                inventory.equippedHpItem = null;
                status.Hp -= item.ItemAbility;
                status.CurrentHp -= item.ItemAbility;
                break;
            default:
                Console.WriteLine("오류입니다.");
                break;
        }
    }

    public ScreenState ScreenFittingMode()
    {
        Console.Clear();
        ShowFittingMode();
        return FittingModeHandle();
    }

    private void ShowFittingMode()
    {
        Console.WriteLine("[아이템 목록]");
        if (!inventory.inventoryItems.Any())
        {
            Console.WriteLine("인벤토리가 비어있습니다.");
        }
        else
        {
            for (int i = 0; i < inventory.inventoryItems.Count; ++i)
            {
                Item item = inventory.inventoryItems[i];
                string? equippedDisplay = inventory.equippedItems.Contains(item) ? "[E]" : null;

                Console.WriteLine($"- {i + 1} {equippedDisplay}{item.ItemName} | {item.ItemEffect.ToString()} +{item.ItemAbility} | {item.ItemDescription}");
            }
        }
        Console.WriteLine("\n0. 장착모드 해제");
        if (flagA)
        {
            flagA = false;
            Console.WriteLine("\n아이템을 장착 하였습니다.");
        }
        else if (flagB)
        {
            flagB = false;
            Console.WriteLine("\n아이템을 해제 하였습니다.");
        }
        Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
    }

    private ScreenState FittingModeHandle()
    {
        while (true)
        {
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int selectedIndex) && input == selectedIndex.ToString())
            {
                if (selectedIndex == 0)
                {
                    return ScreenState.Inventory;
                }

                int itemIndex = selectedIndex - 1;

                if (itemIndex >= 0 && itemIndex < inventory.inventoryItems.Count)
                {
                    if (inventory.equippedItems.Contains(inventory.inventoryItems[itemIndex]))
                    {
                        OffEquip(inventory.inventoryItems[itemIndex]);
                        flagB = true;
                        return ScreenState.FittingMode;
                    }
                    else
                    {
                        OnEquip(inventory.inventoryItems[itemIndex]);
                        flagA = true;
                        return ScreenState.FittingMode;
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