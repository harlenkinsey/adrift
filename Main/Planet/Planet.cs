using Godot;
using System;

public partial class Planet : Node3D
{
	[Export]
	FastNoiseLite[] NoiseMaps;
	[Export]
	public float Radius = 1f;
	[Export]
	public float Amplitude = 1f;
	[Export]
	public int Resolution = 10;
	[Export]
	public float MinHeight = 1f;
	[Export]
	public bool UseFirstLayerAsMask = true;
	[Export]
	public GradientTexture1D PlanetTexture;

	public float minHeight = 9999f;
	public float maxHeight = 0f;

	public override void _Ready()
	{
		OnDataChanged();
	}

	public void OnDataChanged()
	{
		minHeight = 9999f;
		maxHeight = 0f;

		Godot.Collections.Array<Node> children = GetChildren();

		for (int i = 0; i < 6; i++)
		{
			PlanetMeshFace face = (PlanetMeshFace)children[i];
			face.RegenerateMesh();
		}
	}

	public Vector3 PointOnPlanet(Vector3 pointOnSphere)
	{

		float elevation = 0f;

		float baseElevation = NoiseMaps[0].GetNoise3Dv(pointOnSphere * 100f);
		baseElevation = (baseElevation + 1f) / 2f * Amplitude;
		baseElevation = Mathf.Max(0f, baseElevation - MinHeight);

		float mask = UseFirstLayerAsMask ? baseElevation : 1f;

		foreach (FastNoiseLite noiseMap in NoiseMaps)
		{
			float levelElevation = noiseMap.GetNoise3Dv(pointOnSphere * 100f);
			levelElevation = (levelElevation + 1f) / 2f * Amplitude;
			levelElevation = Mathf.Max(0f, levelElevation - MinHeight) * mask;
			elevation += levelElevation;
		}

		return pointOnSphere * Radius * (elevation + 1f);
	}

}
