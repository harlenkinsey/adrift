using Godot;

public class Inventory
{
    public Item[] Items;

    public Inventory(int size)
    {
        Items = new Item[size];
    }

    public Item GetItem(int index)
    {
        return Items[index];
    }
}

public class Item
{
    public int ItemId;
    public string Name;
    public Texture2D Icon;
    public string Quality;
    public int Count;
    public int Durability;
    public int MaxStack;
    public bool Stackable;
    public bool Durable;

    public Item(int id, string name, Texture2D icon, string quality, int count, int durability, int maxStack, bool stackable, bool durable)
    {
        ItemId = id;
        Name = name;
        Icon = icon;
        Quality = quality;
        Count = count;
        Durability = durability;
        MaxStack = maxStack;
        Stackable = stackable;
        Durable = durable;
    }
}