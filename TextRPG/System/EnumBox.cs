public enum ScreenState
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

public enum ItemValue
{
    Power,
    Armor,
    Hp
}

public enum BuyResult
{
    AlreadyPurchased,
    NotEnoughGold,
    Success
}