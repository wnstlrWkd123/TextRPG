public class GameManager
{
    public void Run()
    {
        ScreenState currentScreen = ScreenState.MainMenu;

        BootStrapper bootStrapper = new BootStrapper();

        Console.Write("원하시는 이름을 설정해주세요.\n>> ");
        bootStrapper.status.PlayerName = Console.ReadLine();

        while (currentScreen != ScreenState.Exit)
        {
            switch (currentScreen)
            {
                case ScreenState.MainMenu:
                    currentScreen = bootStrapper.mainMenu.ScreenMainMenu();
                    break;
                case ScreenState.Status:
                    currentScreen = bootStrapper.status.ScreenStatus();
                    break;
                case ScreenState.Inventory:
                    currentScreen = bootStrapper.inventory.ScreenInventory();
                    break;
                case ScreenState.FittingMode:
                    currentScreen = bootStrapper.fittingMode.ScreenFittingMode();
                    break;
                case ScreenState.Shop:
                    currentScreen = bootStrapper.shop.ScreenShop();
                    break;
                case ScreenState.BuyingMode:
                    currentScreen = bootStrapper.buyingMode.ScreenBuyingMode();
                    break;
                case ScreenState.SellingMode:
                    currentScreen = bootStrapper.sellingMode.ScreenSellingMode();
                    break;
                //case ScreenState.Dungeon:
                //    break;
                case ScreenState.Rest:
                    currentScreen = bootStrapper.restMode.ScreenRestMode();
                    break;
                default:
                    Console.WriteLine("오류발생");
                    break;
            }
        }
        Console.WriteLine("게임이 종료 되었습니다.");
    }
}