using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Numerics;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Channels;
using System.Xml.Linq;

namespace TextRPG;

internal class Program
{
    static void Main(string[] args)
    {
        GameManager gameManager = new GameManager();
        gameManager.Run();
    }
}

enum ScreenState
{
    Exit,
    Status,
    Inventory,
    Shop,
    Dungeon,
    Rest,
    MainMenu,
    FittingMode,
    BuyingMode,
    SellingMode
}

enum ItemValue
{
    Power,
    Armor,
    Hp
}

enum BuyResult
{
    AlreadyPurchased,
    NotEnoughGold,
    Success
}

internal class GameManager
{
    public void Run()
    {
        ScreenState currentScreen = ScreenState.MainMenu;

        MainMenu mainMenu = new MainMenu();
        Status status = new Status();
        Items items = new Items();
        Inventory inventory = new Inventory();
        FittingMode fittingMode = new FittingMode(status, inventory);
        Shop shop = new Shop(status, items.items);
        BuyingMode buyingMode = new BuyingMode(status, inventory, shop);
        SellingMode sellingMode = new SellingMode(status, inventory, fittingMode);
        RestMode restMode = new RestMode(status);

        Console.Write("원하시는 이름을 설정해주세요.\n>> ");
        status.PlayerName = Console.ReadLine();

        while (currentScreen != ScreenState.Exit)
        {
            switch (currentScreen)
            {
                case ScreenState.MainMenu:
                    currentScreen = mainMenu.ScreenMainMenu();
                    break;
                case ScreenState.Status:
                    currentScreen = status.ScreenStatus();
                    break;
                case ScreenState.Inventory:
                    currentScreen = inventory.ScreenInventory();
                    break;
                case ScreenState.FittingMode:
                    currentScreen = fittingMode.ScreenFittingMode();
                    break;
                case ScreenState.Shop:
                    currentScreen = shop.ScreenShop();
                    break;
                case ScreenState.BuyingMode:
                    currentScreen = buyingMode.ScreenBuyingMode();
                    break;
                case ScreenState.SellingMode:
                    currentScreen = sellingMode.ScreenSellingMode();
                    break;
                //case ScreenState.Dungeon:
                //    break;
                case ScreenState.Rest:
                    currentScreen = restMode.ScreenRestMode();
                    break;
                default:
                    Console.WriteLine("오류발생");
                    break;
            }
        }
        Console.WriteLine("게임이 종료 되었습니다.");
    }
}

internal class MainMenu
{
    public ScreenState ScreenMainMenu()
    {
        Console.Clear();
        ShowMainMenu();
        return MainMenuHandle();
    }

    private void ShowMainMenu()
    {
        Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
        Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
        Console.WriteLine("1. 상태 보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine("3. 상점");
        Console.WriteLine("4. 던전입장");
        Console.WriteLine("5. 휴식하기");
        Console.WriteLine("\n0. 게임 종료\n");
        Console.Write("원하시는 행동을 입력해주세요.\n>> ");
    }

    private ScreenState MainMenuHandle()
    {
        while (true)
        {
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int command) && input == command.ToString())
            {
                if (command >= 0 && command < 6)
                {
                    return (ScreenState)command;
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

internal class Status
{
    public int Level { get; set; } = 1;
    public string? PlayerName;
    public string Occupation = "전사";
    public int BasePower { get; set; } = 10;
    public int Power { get; set; } = 0;
    public int BaseArmor { get; set; } = 5;
    public int Armor { get; set; } = 0;
    public int BaseHp { get; set; } = 100;
    public int Hp { get; set; } = 0;
    public int CurrentHp { get; set; } = 100;
    public int Gold { get; set; } = 1234567890;

    public ScreenState ScreenStatus()
    {
        Console.Clear();
        ShowStatus();
        return StatusHandle();
    }

    private void ShowStatus()
    {
        Console.WriteLine($"Lv. {Level:D2}");
        Console.WriteLine($"{PlayerName} ( {Occupation} )");
        Console.WriteLine($"Power : {BasePower + Power} (+{Power})");
        Console.WriteLine($"Armor : {BaseArmor + Armor} (+{Armor})");
        Console.WriteLine($"Max Hp: {BaseHp + Hp} (+{Hp})");
        Console.WriteLine($"H p   : {CurrentHp}");
        Console.WriteLine($"Gold  : {Gold} G\n");
        Console.WriteLine("0. 나가기");
        Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
    }

    private ScreenState StatusHandle()
    {
        while (true)
        {
            string? input = Console.ReadLine();

            switch (input)
            {
                case "0":
                    return ScreenState.MainMenu;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요!");
                    break;
            }
        }
    }
}

internal class Item
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

internal class Items
{
    public List<Item> items = new List<Item>()
    {
            new Item("수련자 갑옷", ItemValue.Armor, 5, "수련에 도움을 주는 갑옷입니다.", 1000),
            new Item("무쇠갑옷", ItemValue.Armor, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000),
            new Item("스파르타의 갑옷", ItemValue.Armor, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500),
            new Item("낡은 검", ItemValue.Power, 2, "쉽게 볼 수 있는 낡은 검 입니다.", 600),
            new Item("청동 도끼", ItemValue.Power, 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500),
            new Item("스파르타의 창", ItemValue.Power, 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 3100),
            new Item("엑스칼리버", ItemValue.Power, 1000, "전설로만 내려오는 영웅 아서왕의 성검이다.", 1234557890),
            new Item("아테나의 방패", ItemValue.Armor, 1000, "전설로만 내려오는 여신 아테나의 방패이다.", 1234557890),
            new Item("워모그의 심장", ItemValue.Hp, 1000, "리그오브레전드에 등장하는 아이템이다.", 1234557890)
    };
}

internal class Inventory
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

internal class FittingMode
{
    private Status status;
    private Inventory inventory;

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
                        return ScreenState.FittingMode;
                    }
                    else
                    {
                        OnEquip(inventory.inventoryItems[itemIndex]);
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

internal class Shop
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

internal class BuyingMode
{
    private Status status;
    private Inventory inventory;
    private Shop shop;

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

internal class SellingMode
{
    private Status status;
    private Inventory inventory;
    private FittingMode fittingMode;

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
        Console.WriteLine("\n0. 아이템 판매 취소\n");
        Console.Write("원하시는 행동을 입력해주세요.\n>> ");
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

internal class RestMode
{
    Status status;

    public RestMode(Status existingStatus)
    {
        status = existingStatus;
    }

    private void Rest()
    {
        if (status.Gold < 500)
        {
            Console.WriteLine("Gold 가 부족합니다.");
        }
        else
        {
            status.Gold -= 500;
            status.CurrentHp = status.BaseHp + status.Hp;
            Console.WriteLine("휴식을 완료했습니다.");
        }
    }

    public ScreenState ScreenRestMode()
    {
        Console.Clear();
        ShowRestMode();
        return RestModeHandle();
    }

    private void ShowRestMode()
    {
        Console.WriteLine("휴식하기");
        Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {status.Gold} G)\n");
        Console.WriteLine("1. 휴식하기\n");
        Console.WriteLine("0. 나가기");
        Console.Write("\n원하시는 행동을 입력해주세요.\n>> ");
    }

    private ScreenState RestModeHandle()
    {
        while (true)
        {
            string? input = Console.ReadLine();

            switch (input)
            {
                case "0":
                    return ScreenState.MainMenu;
                case "1":
                    Rest();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요!");
                    break;
            }
        }
    }
}