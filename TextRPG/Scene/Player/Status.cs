public class Status
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