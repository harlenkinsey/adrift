using Godot;
using System;

public partial class ItemSlot : Panel
{
	public TextureRect IconTextureRect;
	public TextureRect RarityTextureRect;
	public Label CountLabel;
	public ProgressBar DurabilityProgressBar;

	public int SlotID;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		IconTextureRect = (TextureRect)FindChild("IconTextureRect");
		RarityTextureRect = (TextureRect)FindChild("RarityTextureRect");
		CountLabel = (Label)FindChild("CountLabel");
		DurabilityProgressBar = (ProgressBar)FindChild("DurabilityProgressBar");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void UpdateSlot()
	{

	}
}
