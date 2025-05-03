public class MainMenu
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