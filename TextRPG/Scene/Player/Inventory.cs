public class Inventory
{
    public List<Item> inventoryItems = new List<Item>();
    public List<Item> equippedItems = new List<Item>();
    public Item? equippedPowerItem = null;
    public Item? equippedArmorItem = null;
    public Item? equippedHpItem = null;

    public ScreenState ScreenInventory()
    {
        Console.Clear();
        ShowInventory();
        return InventoryHandle();
    }

    private void ShowInventory()
    {
        Console.WriteLine("[아이템 목록]");
        if (!inventoryItems.Any())
        {
            Console.WriteLine("인벤토리가 비어있습니다.");
        }
        else
        {
            for (int i = 0; i < inventoryItems.Count; ++i)
            {
                Item item = inventoryItems[i];
                string? equippedDisplay = equippedItems.Contains(item) ? "[E]" : null;

                Console.WriteLine($"- {i + 1} {equippedDisplay}{item.ItemName} | {item.ItemEffect.ToString()} +{item.ItemAbility} | {item.ItemDescription}");
            }
        }
        Console.WriteLine("\n1. 장착모드\n");
        Console.WriteLine("0. 나가기");
        Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
    }

    private ScreenState InventoryHandle()
    {
        while (true)
        {
            string? input = Console.ReadLine();

            switch (input)
            {
                case "0":
                    return ScreenState.MainMenu;
                case "1":
                    return ScreenState.FittingMode;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요!");
                    break;
            }
        }
    }
}