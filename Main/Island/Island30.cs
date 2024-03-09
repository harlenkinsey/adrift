using Godot;
using System;

public partial class Island30 : Node3D
{

	MeshDataTool MDT = new MeshDataTool();
	[Export]
	FastNoiseLite[] NoiseMaps;
	MeshInstance3D IslandMeshInstance;

	public override void _Ready()
	{

		IslandMeshInstance = (MeshInstance3D)GetChild(0);
		ArrayMesh islandMesh = (ArrayMesh)IslandMeshInstance.Mesh;
		MDT.CreateFromSurface(islandMesh, 0);

		for (int i = 0; i < MDT.GetVertexCount(); i++)
		{
			Vector3 vertexNormalized = MDT.GetVertex(i).Normalized();
			Vector3 vertexLoud = ApplyNoise(vertexNormalized);

			if (MDT.GetVertex(i).Y < 0f)
			{
				MDT.SetVertex(i, vertexLoud);
			}
		}

		// regenerates normals
		// MDT.GetFaceNormal() does calculations behind the scenes
		for (int i = 0; i < MDT.GetFaceCount(); i++)
		{
			// Gets face's vertices' indexes
			int vertex = MDT.GetFaceVertex(i, 0);
			int vertex2 = MDT.GetFaceVertex(i, 1);
			int vertex3 = MDT.GetFaceVertex(i, 2);

			Vector3 normal = MDT.GetFaceNormal(i);

			MDT.SetVertexNormal(vertex, normal);
			MDT.SetVertexNormal(vertex2, normal);
			MDT.SetVertexNormal(vertex3, normal);
		}

		islandMesh.ClearSurfaces();
		MDT.CommitToSurface(islandMesh);
		MDT.Clear();

		// Adds mesh to instance and creates collision shape
		IslandMeshInstance.Mesh = islandMesh;

		IslandMeshInstance.CreateTrimeshCollision();

		// Used to set textures based on vertex height
		//PlanetMaterial.SetShaderParameter("minHeight", MinHeight);
		//PlanetMaterial.SetShaderParameter("maxHeight", MaxHeight);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public Vector3 ApplyNoise(Vector3 point)
	{
		float cumulativeElevation = 0f;

		for (int i = 1; i < NoiseMaps.Length; i++)
		{
			FastNoiseLite currentMap = NoiseMaps[i];
			float levelElevation = Mathf.Clamp(currentMap.GetNoise3Dv(point * 100f), 0f, 2f) + 1f;
			cumulativeElevation += levelElevation;
		}

		return point * cumulativeElevation;
	}
}
