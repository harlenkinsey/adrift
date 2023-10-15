using Godot;
using System;

public partial class PlanetMeshFace : MeshInstance3D
{
	[Export]
	Planet Planet;
	[Export]
	Vector3 Normal;
	public void RegenerateMesh()
	{
		var surfaceArray = new Godot.Collections.Array();
		surfaceArray.Resize((int)Mesh.ArrayType.Max);

		Vector3[] vertexArray = new Vector3[1];
		Vector2[] uvArray = new Vector2[1];
		Vector3[] normalArray = new Vector3[1];
		int[] indexArray = new int[1];

		int resolution = Planet.Resolution;

		int numVertices = resolution * resolution;
		int numIndices = (resolution - 1) * (resolution - 1) * 6;

		Array.Resize(ref vertexArray, numVertices);
		Array.Resize(ref uvArray, numVertices);
		Array.Resize(ref normalArray, numVertices);
		Array.Resize(ref indexArray, numIndices);

		int triIndex = 0;
		Vector3 axisA = new Vector3(Normal.Y, Normal.Z, Normal.X);
		Vector3 axisB = Normal.Cross(axisA);

		for (int y = 0; y < resolution; y++)
		{
			for (int x = 0; x < resolution; x++)
			{

				int i = x + y * resolution;
				Vector2 percent = new Vector2(x, y) / (resolution - 1);

				Vector3 pointOnUnitCube = Normal + (percent.X - 0.5f) * 2.0f * axisA + (percent.Y - 0.5f) * 2.0f * axisB;
				Vector3 pointOnUnitSphere = pointOnUnitCube.Normalized();
				Vector3 pointOnPlanet = Planet.PointOnPlanet(pointOnUnitSphere);

				float pointLength = pointOnPlanet.Length();
				if (pointLength < Planet.minHeight)
				{
					Planet.minHeight = pointLength;
				}
				if (pointLength > Planet.maxHeight)
				{
					Planet.maxHeight = pointLength;
				}

				vertexArray[i] = pointOnPlanet;

				if (x != resolution - 1 && y != resolution - 1)
				{
					indexArray[triIndex + 2] = i;
					indexArray[triIndex + 1] = i + resolution + 1;
					indexArray[triIndex] = i + resolution;

					indexArray[triIndex + 5] = i;
					indexArray[triIndex + 4] = i + 1;
					indexArray[triIndex + 3] = i + resolution + 1;
					triIndex += 6;
				}
			}
		}

		for (int a = 0; a < indexArray.Length; a += 3)
		{
			int b = a + 1;
			int c = a + 2;

			Vector3 ab = vertexArray[indexArray[b]] - vertexArray[indexArray[a]];
			Vector3 bc = vertexArray[indexArray[c]] - vertexArray[indexArray[b]];
			Vector3 ca = vertexArray[indexArray[a]] - vertexArray[indexArray[c]];

			Vector3 crossAbBc = ab.Cross(bc) * -1f;
			Vector3 crossBcCa = bc.Cross(ca) * -1f;
			Vector3 crossCaAb = ab.Cross(ab) * -1f;

			normalArray[indexArray[a]] += crossAbBc + crossBcCa + crossCaAb;
			normalArray[indexArray[b]] += crossAbBc + crossBcCa + crossCaAb;
			normalArray[indexArray[c]] += crossAbBc + crossBcCa + crossCaAb;
		}

		for (int i = 0; i < normalArray.Length; i++)
		{
			normalArray[i] = normalArray[i].Normalized();
		}

		surfaceArray[(int)Mesh.ArrayType.Vertex] = vertexArray;
		surfaceArray[(int)Mesh.ArrayType.Normal] = normalArray;
		surfaceArray[(int)Mesh.ArrayType.TexUV] = uvArray;
		surfaceArray[(int)Mesh.ArrayType.Index] = indexArray;

		CallDeferred("UpdateMesh", surfaceArray);
	}

	public void UpdateMesh(Godot.Collections.Array arrays)
	{
		ArrayMesh mesh = new ArrayMesh();
		mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);
		this.Mesh = mesh;

		CreateTrimeshCollision();

		ShaderMaterial shaderMaterial = (ShaderMaterial)MaterialOverride;
		shaderMaterial.SetShaderParameter("minHeight", Planet.minHeight);
		shaderMaterial.SetShaderParameter("maxHeight", Planet.maxHeight);
		shaderMaterial.SetShaderParameter("heightColor", Planet.PlanetTexture);
	}
}
