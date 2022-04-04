using Godot;
using System;

public class ShakeCam : Camera2D
{
    public float decay = 0.8f;
    public Vector2 maxOffset = new Vector2(100, 75);
    public float maxRoll = 0.1f;
    float trauma = 100.0f;
    int traumaPower = 2;

    private OpenSimplexNoise noise = new OpenSimplexNoise();
    private int noiseY = 0;

    public override void _Ready() 
    {
        noise.Seed = (int)GD.Randi();
        noise.Period = 4;
        noise.Octaves = 2;
        GD.Print("camera ready");
    }

    public override void _Process(float delta)
    {
        if (trauma > 0) {
            trauma = Mathf.Max(trauma - decay * delta, 0);
            Shake();
        }
    }

    public void Shake() {
        GD.Print("shake");
        noiseY++;
        var amount = Mathf.Pow(trauma, traumaPower);
        Rotation = maxRoll * amount * noise.GetNoise2d(noise.Seed, noiseY);
        var offsetVect = new Vector2(maxOffset.x * amount * noise.GetNoise2d(noise.Seed * 2, noiseY), maxOffset.y * amount * noise.GetNoise2d(noise.Seed * 3, noiseY));
        Offset = offsetVect;
    }
}