using Godot;
using System;

public partial class WaterMeshFace : MeshInstance3D
{
	[Export]
	Water Water;
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

		int resolution = Water.Resolution;

		int numVertices = resolution * resolution;

		/* 
			- Each square of the mesh is made up of two triangles, hence 6 vertices.
			- To prevent triangles from generating on the edges of the face, subtract 1.
		*/
		int numIndices = (resolution - 1) * (resolution - 1) * 6;

		Array.Resize(ref vertexArray, numVertices);
		Array.Resize(ref uvArray, numVertices);
		Array.Resize(ref normalArray, numVertices);

		/*
			- Indicies represent the location of each triangle's vertices
		*/
		Array.Resize(ref indexArray, numIndices);

		int triIndex = 0;
		Vector3 axisA = new Vector3(Normal.Y, Normal.Z, Normal.X);

		/*
			- The cross product is an operation on two vectors that results in a new vector with a direction that is perpendicular (at 90 degrees) to both.
		*/
		Vector3 axisB = Normal.Cross(axisA);

		/*
			- Traverses each vertex on the face, normalizes the vertex (to create a sphere), and adjusts the vertex's height based on noise (to create terrain).
		*/
		for (int y = 0; y < resolution; y++)
		{
			for (int x = 0; x < resolution; x++)
			{
				int i = x + y * resolution;
				Vector2 percent = new Vector2(x, y) / (resolution - 1);

				Vector3 pointOnUnitCube = Normal + (percent.X - 0.5f) * 2.0f * axisA + (percent.Y - 0.5f) * 2.0f * axisB;
				Vector3 pointOnUnitSphere = pointOnUnitCube.Normalized();

				vertexArray[i] = pointOnUnitSphere * Water.Radius;
				uvArray[i] = percent;


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

		/*
			- Iterates over the face's triangles (again) to calculate their normals and add them to the normalArray
		*/
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
			normalArray[i] = normalArray[i].Normalized(); // Normal magnitude (length) must be between 0 and 1
		}

		surfaceArray[(int)Mesh.ArrayType.Vertex] = vertexArray;
		surfaceArray[(int)Mesh.ArrayType.Normal] = normalArray;
		surfaceArray[(int)Mesh.ArrayType.TexUV] = uvArray;
		surfaceArray[(int)Mesh.ArrayType.Index] = indexArray;

		CallDeferred("UpdateMesh", surfaceArray); // To reduce the chance of errors
	}

	public void UpdateMesh(Godot.Collections.Array arrays)
	{
		ArrayMesh mesh = new ArrayMesh();
		mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);
		this.Mesh = mesh;
	}
}
