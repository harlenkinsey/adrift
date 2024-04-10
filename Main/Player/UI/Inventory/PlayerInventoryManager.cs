using Godot;
using System.Collections.Generic;

public partial class PlayerInventoryManager : CanvasLayer
{
	Inventory PlayerInventory = new Inventory(12);

	List<ItemSlot> InventorySlots = new List<ItemSlot>();

	HBoxContainer Row1;
	HBoxContainer Row2;
	HBoxContainer Row3;
	HBoxContainer Row4;

	List<HBoxContainer> Rows = new List<HBoxContainer>();

	public override void _Ready()
	{
		Row1 = (HBoxContainer)FindChild("Row1");
		Row2 = (HBoxContainer)FindChild("Row2");
		Row3 = (HBoxContainer)FindChild("Row3");
		Row4 = (HBoxContainer)FindChild("Row4");

		Rows.Add(Row1);
		Rows.Add(Row2);
		Rows.Add(Row3);
		Rows.Add(Row4);

		int currentSlotID = 0;

		foreach (HBoxContainer row in Rows)
		{
			foreach (Node slot in row.GetChildren())
			{
				ItemSlot currentSlot = (ItemSlot)slot;

				InventorySlots.Add(currentSlot);
				currentSlot.SlotID = currentSlotID;
				currentSlotID += 1;
			}
		}
	}

	public override void _Process(double delta)
	{
	}
}

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
	public int Rarity;
	public int Count;
	public int Durability;
	public bool Stackable;
	public bool Durable;
}
