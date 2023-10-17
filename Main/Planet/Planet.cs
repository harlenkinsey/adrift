using Godot;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

public partial class Planet : Node3D
{
	[Export]
	FastNoiseLite[] NoiseMaps;
	[Export]
	public float Radius = 1f;
	[Export]
	public float Amplitude = 1f;
	[Export]
	public int Resolution = 200;
	[Export]
	public float MinHeight = 1f;
	[Export]
	public bool UseFirstLayerAsMask = true;
	public PlanetBiome[] Biomes;
	[Export]
	public FastNoiseLite BiomeNoise;
	public float BiomeAmplitude = .1f;
	public float BiomeOffset = .3f;
	public float BiomeBlend = 0.2f; // Between 0.0 and 1.0

	public float minHeight = 9999f;
	public float maxHeight = 0f;

	public override void _Ready()
	{
		InitializeBiomes(3);
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

	public ImageTexture UpdateBiomeTexture()
	{
		ImageTexture imageTexture;
		Image dynamicImage;

		int height = Biomes.Length;

		Byte[] data = new Byte[0];
		int width = Biomes[0].Gradient.Width;

		foreach (PlanetBiome biome in Biomes)
		{
			data = data.Concat(biome.Gradient.GetImage().GetData()).ToArray();
		}

		dynamicImage = Image.CreateFromData(width, height, false, Image.Format.Rgba8, data);

		imageTexture = ImageTexture.CreateFromImage(dynamicImage);
		return imageTexture;
	}

	public void InitializeBiomes(int numOfBiomes)
	{
		List<PlanetBiome> biomes = new List<PlanetBiome>();

		for (int i = 0; i < numOfBiomes; i++)
		{
			PlanetBiome newBiome = new PlanetBiome();
			newBiome.Initialize();
			biomes.Add(newBiome);
		}

		Biomes = biomes.ToArray();
	}

	public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
	{
		float heightPercent = (pointOnUnitSphere.Y + 1f) / 2f;
		heightPercent += ((BiomeNoise.GetNoise3Dv(pointOnUnitSphere * 100f) + 1f / 2f) - BiomeOffset) * BiomeAmplitude;
		float biomeIndex = 0f;
		float numberOfBiomes = Biomes.Length;

		float blendRange = BiomeBlend / 2f + 0.0001f;

		for (int i = 0; i < numberOfBiomes; i++)
		{
			float distance = heightPercent - Biomes[i].StartHeight;
			float lerpValue = Mathf.Clamp(Mathf.InverseLerp(-blendRange, blendRange, distance), 0f, 1f);
			float weight = lerpValue;
			biomeIndex *= 1f - weight;
			biomeIndex += i * weight;
		}

		return biomeIndex / Mathf.Max(1f, numberOfBiomes - 1f);
	}

}
