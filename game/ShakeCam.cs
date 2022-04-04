using Godot;
using System;

public class ShakeCam : Camera2D
{
    public float Decay = 0.8f;
    public Vector2 MaxOffset = new Vector2(100, 75);
    public float MaxRoll = 0.1f;
    public float Trauma = 0.5f;
    private int TraumaPower = 2;

    private OpenSimplexNoise Noise = new OpenSimplexNoise();
    private int noiseY = 0;

    public override void _Ready() 
    {
        Noise.Seed = (int)GD.Randi();
        Noise.Period = 4;
        Noise.Octaves = 2;
        GD.Print("camera ready");
    }

    public override void _Process(float delta)
    {
        if (Trauma > 0) {
            Trauma = Mathf.Max(Trauma - Decay * delta, 0);
            Shake();
        }
    }

    public void Shake() {
        noiseY++;
        var amount = Mathf.Pow(Trauma, TraumaPower);
        Rotation = MaxRoll * amount * Noise.GetNoise2d(Noise.Seed, noiseY);
        var offsetVect = new Vector2(MaxOffset.x * amount * Noise.GetNoise2d(Noise.Seed * 2, noiseY), MaxOffset.y * amount * Noise.GetNoise2d(Noise.Seed * 3, noiseY));
        Offset = offsetVect;
    }
}