using Godot;
using System;

public partial class ItemSlot : Panel
{
	public TextureRect IconTextureRect;
	public TextureRect RarityTextureRect;
	public Label CountLabel;
	public ProgressBar DurabilityProgressBar;

	public InventoryData IV;
	public int SlotID;

	public override void _Ready()
	{
		IV = GetNode<InventoryData>("/root/InventoryData");

		IconTextureRect = (TextureRect)FindChild("IconTextureRect");
		RarityTextureRect = (TextureRect)FindChild("QualityTextureRect");
		CountLabel = (Label)FindChild("CountLabel");
		DurabilityProgressBar = (ProgressBar)FindChild("DurabilityProgressBar");
	}

	public void UpdateSlot(TextureRect texture, string quality, int count, int durability)
	{
		IconTextureRect = texture;
		RarityTextureRect.Modulate = IV.Quality[quality];
		CountLabel.Text = count.ToString();
		DurabilityProgressBar.Value = durability;
	}
}
