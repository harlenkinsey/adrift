using Godot;
using System;

public partial class Water : Node3D
{
	public int Resolution = 100;
	public float Radius = 1f;

	public void Initialize()
	{
		MeshInstance3D child = (MeshInstance3D)GetChild(0);
		SphereMesh waterMesh = (SphereMesh)child.Mesh;

		waterMesh.Radius = Radius;
		waterMesh.Height = Radius * 2f;
		waterMesh.RadialSegments = Resolution;
		waterMesh.Rings = Resolution / 2;
	}
}
