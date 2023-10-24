using Godot;

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
	public float NoiseMinHeight = 1f;
	[Export]
	public bool UseFirstLayerAsMask = true;
	[Export]
	public GradientTexture1D PlanetTexture;

	/*
		Both minHeight and maxHeight are used to set the texture color base on elevation
	*/
	public float minHeight = 9999f;
	public float maxHeight = 0f;

	public override void _Ready()
	{
		minHeight = 9999f;
		maxHeight = 0f;

		/*
			- Scales the planet's gravity field based on its radius
		*/
		Area3D GravityField = (Area3D)GetChild(6);
		GravityField.Transform = new Transform3D(Basis.FromScale(Vector3.One * Radius), GravityField.Transform.Origin);

		/*
			- Scales the planet's water mesh based on its radius and the water level
		*/
		float waterLevel = 1.06f;
		Node3D waterMesh = (Node3D)GetChild(7);
		waterMesh.Transform = new Transform3D(Basis.FromScale(Vector3.One * Radius * waterLevel), waterMesh.Transform.Origin);

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
		baseElevation = Mathf.Max(0f, baseElevation - NoiseMinHeight); // makes it so that baseElevation >= 0

		/*
			- Subsequent noise layers get filtered by the first layer
			- This makes it so that peaks from subsequent layers don't pop out of the flat portion (often the sea) of the planet
		*/
		float mask = UseFirstLayerAsMask ? baseElevation : 1f;

		foreach (FastNoiseLite noiseMap in NoiseMaps)
		{
			float levelElevation = noiseMap.GetNoise3Dv(pointOnSphere * 100f);
			levelElevation = (levelElevation + 1f) / 2f * Amplitude;
			levelElevation = Mathf.Max(0f, levelElevation - NoiseMinHeight) * mask;
			elevation += levelElevation;
		}

		return pointOnSphere * Radius * (elevation + 1f);
	}
}
