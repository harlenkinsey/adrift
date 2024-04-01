using Godot;
using System.Collections.Generic;

public partial class InventoryManager : CanvasLayer
{

	List<ItemSlot> Inventory = new List<ItemSlot>();

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

		foreach (HBoxContainer row in Rows)
		{
			foreach (Node slot in row.GetChildren())
			{
				Inventory.Add((ItemSlot)slot);
			}
		}
	}

	public override void _Process(double delta)
	{
	}
}
