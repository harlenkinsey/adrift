using Godot;
using System.Collections.Generic;

public partial class InventoryData : Node
{
    public Dictionary<int, Item> Items = new Dictionary<int, Item>()
    {
        { 0, new Item(0, "WoodPickaxe", (Texture2D)ResourceLoader.Load("res://Main/Player/UI/Inventory/ItemPlaceholder.png"), "Common", 1, 100, 1, false, true) }
    };
    public Dictionary<string, Color> Quality = new Dictionary<string, Color>()
    {
        {"Common", new Color(1, 1, 1, 0.43f)},
        {"Uncommon", new Color(0, 1, 0, 0.43f)},
        {"Rare", new Color(0, 1, 0, 0.43f)},
        {"Epic", new Color(1, 0.647059f, 0, 0.43f)},
        {"Legendary", new Color(1, 0, 0, 0.43f)},
        {"Ascendant", new Color(0, 0.501961f, 0.501961f, 0.43f)}
    };
}
