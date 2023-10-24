using Godot;
using System;

public partial class Water : Node3D
{
	[Export]
	public int Resolution = 200;
	[Export]
	public float Radius = 1f;

	public override void _Ready()
	{
		Godot.Collections.Array<Node> children = GetChildren();

		for (int i = 0; i < 6; i++)
		{
			WaterMeshFace face = (WaterMeshFace)children[i];
			face.RegenerateMesh();
		}
	}
}
