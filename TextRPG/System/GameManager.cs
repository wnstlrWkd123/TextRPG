public class GameManager
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