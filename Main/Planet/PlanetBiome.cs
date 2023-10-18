using Godot;
using System;

public partial class PlanetBiome : Node
{
    public float StartHeight;
    public GradientTexture1D Gradient;

    public void Initialize(int id)
    {
        Gradient = new GradientTexture1D();
        Gradient.Gradient = new Gradient();
        

        switch(id)
        {
            case 0:
                StartHeight = 0f;

                Gradient.Gradient.AddPoint(0f, new Color(0, 255, 0));
                Gradient.Gradient.AddPoint(1f, new Color(0, 0, 255));
                break;
            
            case 1:
                StartHeight = 0.3f;

                Gradient.Gradient.AddPoint(0f, new Color(255, 255, 255));
                Gradient.Gradient.AddPoint(1f, new Color(0, 0, 0));
                break;
            
            case 2:
                StartHeight = 0.6f;

                Gradient.Gradient.AddPoint(0f, new Color(0, 0, 0));
                Gradient.Gradient.AddPoint(1f, new Color(255, 255, 0));
                break;
            
            default:
                break;
        }
    }
}
