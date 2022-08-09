using MathStuff;
using MathStuff.vectors;

namespace SomeChartsUi.utils.vectors; 

public static class VectorUtils {
    public static float2 SinCos(this float a) => new(MathF.Sin(a), MathF.Cos(a));

    public static float2[] SinCos(this float[] angles) {
        int len = angles.Length;
        float2[] vectors = new float2[len];

        for (int i = 0; i < len; i++)
            vectors[i] = angles[i].SinCos();
        
        return vectors;
    }
}