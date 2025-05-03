public class BootStrapper
{
    public MainMenu mainMenu;
    public Status status;
    public Items items;
    public Inventory inventory;
    public FittingMode fittingMode;
    public Shop shop;
    public BuyingMode buyingMode;
    public SellingMode sellingMode;
    public RestMode restMode;

    public BootStrapper()
    {
        mainMenu = new MainMenu();
        status = new Status();
        items = new Items();
        inventory = new Inventory();
        fittingMode = new FittingMode(status, inventory);
        shop = new Shop(status, items.items);
        buyingMode = new BuyingMode(status, inventory, shop);
        sellingMode = new SellingMode(status, inventory, fittingMode);
        restMode = new RestMode(status);
    }
}