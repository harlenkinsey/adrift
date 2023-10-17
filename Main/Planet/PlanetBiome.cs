using Godot;
using System;

public partial class PlanetBiome : Node
{
    public float StartHeight;
    public GradientTexture1D Gradient;
    public RandomNumberGenerator RNG = new RandomNumberGenerator();

    public void Initialize()
    {
        Gradient = new GradientTexture1D();
        Gradient.Gradient = new Gradient();
        StartHeight = RNG.Randf();

        Gradient.Gradient.AddPoint(0f, new Color(255, 0, 0));
        Gradient.Gradient.AddPoint(0.5f, new Color(0, 255, 0));
        Gradient.Gradient.AddPoint(1f, new Color(0, 0, 255));
    }
}
