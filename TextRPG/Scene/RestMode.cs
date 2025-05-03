public class RestMode
{
    Status status;

    private bool flag = false;

    public RestMode(Status existingStatus)
    {
        status = existingStatus;
    }

    private ScreenState Rest()
    {
        flag = true;
        status.Gold -= 500;
        status.CurrentHp = status.BaseHp + status.Hp;

        return ScreenState.Rest;
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
        Console.WriteLine("1. 휴식하기");
        Console.WriteLine("\n0. 나가기");
        if (flag)
        {
            flag = false;
            Console.WriteLine("\n휴식을 완료 했습니다.");
        }
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
                    if (status.Gold < 500)
                    {
                        Console.WriteLine("Gold 가 부족합니다.");
                        break;
                    }
                    return Rest();
                default:
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요!");
                    break;
            }
        }
    }
}