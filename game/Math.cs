using Godot;
using System;
using ldjam50;
using System.Collections.Generic;

public static class Math {
    public static Vector2[] LerpAngle(int count, float start, float end) {
        // Special case count 1 (which shoots in the middle instead of start)
        if (count == 1) {
            return new Vector2[]{ Vector2.Right.Rotated(start + (end - start) / 2) };
        }

        var angles = new Vector2[count];
        for (int i = 0; i < count; i++) {
            angles[i] = Vector2.Right.Rotated(start + (end - start) / (count - 1) * i);
        }
        return angles;
    }

    public static Vector2[] FanAngle(int count, float angle, float spread) {
        if (count == 1) {
            return new Vector2[]{ Vector2.Right.Rotated(angle) };
        }

        return LerpAngle(count, angle - spread / 2, angle + spread / 2);
    }
}