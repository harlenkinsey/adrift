using Godot;
using System;

public partial class Planet : Node3D
{
	MeshDataTool MDT = new MeshDataTool();
	[Export]
	FastNoiseLite[] NoiseMaps;
	public float Radius = 20f;
	public float Amplitude = 10f;
	public int Resolution = 200;
	public string PlanetType = "Terrestrial";
	public float SeaLevel = 0.2f; // 0.1f
	[Export]
	public ShaderMaterial PlanetMaterial;
	public MeshInstance3D ChildMeshInstance;

	private float MinHeight = 9999f;
	private float MaxHeight = 0f;

	public override void _Ready()
	{
		//Scales the planet's gravity field based on its radius
		Area3D GravityField = (Area3D)GetChild(0);
		GravityField.Transform = new Transform3D(Basis.FromScale(Vector3.One * Radius), GravityField.Transform.Origin);

		// Initializes the planet's water mesh based on Radius, Resolution, and waterLevel
		float waterLevel = 1.01f;
		Water water = (Water)GetChild(1);
		water.Radius = Radius * waterLevel;
		water.Resolution = Resolution;
		water.Initialize();

		// Generates planet's mesh
		BoxMesh newChild = new BoxMesh();
		newChild.SubdivideDepth = Resolution;
		newChild.SubdivideWidth = Resolution;
		newChild.SubdivideHeight = Resolution;

		var mesh = new ArrayMesh();
		mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, newChild.GetMeshArrays());
		MDT.CreateFromSurface(mesh, 0);

		// 1. normalizes vertices to make mesh a sphere
		// 2. applies noise to vertex position to create terrain
		for (int i = 0; i < MDT.GetVertexCount(); i++)
		{
			Vector3 vertexNormalized = MDT.GetVertex(i).Normalized();
			Vector3 vertexLoud = ApplyNoise(vertexNormalized);

			MDT.SetVertex(i, vertexLoud);

			// sets Min- and MaxHeight based on vertex length
			float vertexLength = vertexLoud.Length();
			if (vertexLength < MinHeight)
			{
				MinHeight = vertexLength;
			}
			else if (vertexLength > MaxHeight)
			{
				MaxHeight = vertexLength;
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

		mesh.ClearSurfaces();
		MDT.CommitToSurface(mesh);
		MDT.Clear();

		// Adds mesh to instance and creates collision shape
		MeshInstance3D meshInstance = new MeshInstance3D();
		meshInstance.Mesh = mesh;
		meshInstance.MaterialOverride = PlanetMaterial;
		ChildMeshInstance = meshInstance;
		AddChild(meshInstance);

		meshInstance.CreateTrimeshCollision();

		// Used to set textures based on vertex height
		PlanetMaterial.SetShaderParameter("minHeight", MinHeight);
		PlanetMaterial.SetShaderParameter("maxHeight", MaxHeight);
	}

	public Vector3 ApplyNoise(Vector3 point)
	{
		float cumulativeElevation = 0f;

		float initialNoise = NoiseMaps[0].GetNoise3Dv(point * 100f);
		float initialNoiseClamped = Mathf.Clamp(initialNoise, 0f, 1f);

		for (int i = 1; i < NoiseMaps.Length; i++)
		{
			FastNoiseLite currentMap = NoiseMaps[i];
			float levelElevation = Mathf.Clamp(currentMap.GetNoise3Dv(point * 100f), 0f, 1f);
			cumulativeElevation += levelElevation;
		}

		return point * (Radius + cumulativeElevation * Amplitude);
	}

	public int GetFaceIndexFromPosition(Vector3 position)
	{
		for (int i = 0; i < MDT.GetFaceCount(); i++)
		{
			// only checks the faces first vertex for efficiency
			int vertex = MDT.GetFaceVertex(i, 0);

			GD.Print(MDT.GetVertex(vertex));

			GD.Print(position.DistanceTo(MDT.GetVertex(vertex)));

			if (position.DistanceTo(MDT.GetVertex(vertex)) <= 0.1f)
			{
				return i;
			}
		}

		return 0;
	}

	public void CollapseFaceAtPosition(Vector3 position)
	{
		ArrayMesh newMeshArray = new ArrayMesh();
		var oldMeshArrays = new Godot.Collections.Array();
		oldMeshArrays.Resize((int)Mesh.ArrayType.Max);

		newMeshArray.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, ChildMeshInstance.Mesh.SurfaceGetArrays(0));
		MDT.CreateFromSurface(newMeshArray, 0);

		int faceIndex = -1;
		// Gets face index based on position
		// Only checks face first vertex to improve efficiency
		for (int i = 0; i < MDT.GetFaceCount(); i++)
		{
			int vertex1 = MDT.GetFaceVertex(i, 0);
			int vertex2 = MDT.GetFaceVertex(i, 1);
			int vertex3 = MDT.GetFaceVertex(i, 2);

			if (
			position.DistanceTo(MDT.GetVertex(vertex1)) <= 0.05f ||
			position.DistanceTo(MDT.GetVertex(vertex2)) <= 0.05f ||
			position.DistanceTo(MDT.GetVertex(vertex3)) <= 0.05f
			)
			{
				faceIndex = i;
				GD.Print(i);
				break;
			}
		}

		if (faceIndex != -1)
		{
			int vertex1Index = MDT.GetFaceVertex(faceIndex, 0);
			int vertex2Index = MDT.GetFaceVertex(faceIndex, 1);
			int vertex3Index = MDT.GetFaceVertex(faceIndex, 2);

			Vector3 vertex1Position = MDT.GetVertex(vertex1Index);
			Vector3 vertex2Position = MDT.GetVertex(vertex2Index);
			Vector3 vertex3Position = MDT.GetVertex(vertex3Index);

			MDT.SetVertex(vertex1Index, vertex1Position * 0.95f);
			MDT.SetVertex(vertex2Index, vertex2Position * 0.95f);
			MDT.SetVertex(vertex3Index, vertex3Position * 0.95f);

			newMeshArray.ClearSurfaces();
			MDT.CommitToSurface(newMeshArray);
			ChildMeshInstance.Mesh = newMeshArray;
			ChildMeshInstance.GetChild(0).QueueFree();
			ChildMeshInstance.CreateTrimeshCollision();
			MDT.Clear();
		}
		else
		{
			MDT.Clear();
		}
	}
}
