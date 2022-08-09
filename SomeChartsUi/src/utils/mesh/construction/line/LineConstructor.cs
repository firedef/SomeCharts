using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.ui.canvas;

namespace SomeChartsUi.utils.mesh.construction.line; 

public abstract class LineConstructor {
    public float lineThickness = 5f;
    public float constLineThickness = 0.5f;

    public abstract void Construct(Mesh m, float2 p0, float2 p1, float? thickness, color col, ChartsCanvas canvas, float z = 0);
}

public class BasicLine : LineConstructor {
    public override void Construct(Mesh m, float2 p0, float2 p1, float? thickness, color col, ChartsCanvas canvas, float z = 0) {
        thickness ??= lineThickness + constLineThickness / canvas.transform.scale.animatedValue.x;
        m.AddLine(p0, p1, thickness.Value, col, z);
    }
}

public class StepLine : LineConstructor {
    public override void Construct(Mesh m, float2 p0, float2 p1, float? thickness, color col, ChartsCanvas canvas, float z = 0) {
        thickness ??= lineThickness + constLineThickness / canvas.transform.scale.animatedValue.x;

        float xMid = (p0.x + p1.x) * .5f;
		
        m.AddLine(p0, new(xMid, p0.y), thickness.Value, col, z);
        m.AddLine(new(xMid, p0.y), new(xMid, p1.y), thickness.Value, col, z);
        m.AddLine(new(xMid, p1.y), p1, thickness.Value, col, z);
    }
}

public class CurvedLine : LineConstructor {
    public int quality = 32;
    
    public override void Construct(Mesh m, float2 p0, float2 p1, float? thickness, color col, ChartsCanvas canvas, float z = 0) {
        thickness ??= lineThickness + constLineThickness / canvas.transform.scale.animatedValue.x;
        float timeStep = 1f / quality;

        for (int i = 0; i < quality; i++) {
            float x0 = Lerp(p0.x, p1.x, i * timeStep);
            float x1 = Lerp(p0.x, p1.x, (i + 1) * timeStep);
            
            float y0 = Lerp(p0.y, p1.y, Smooth(i * timeStep));
            float y1 = Lerp(p0.y, p1.y, Smooth((i + 1) * timeStep));
            
            m.AddLine(new(x0,y0), new(x1,y1), thickness.Value, col, z);
        }
    }

    private static float Smooth(float t) => MathF.Sin(t * MathF.PI - MathF.PI * .5f) * .5f + .5f;

    private static float Lerp(float p0, float p1, float t) => p0 * (1 - t) + p1 * t;
    private static float2 Lerp(float2 p0, float2 p1, float t) => p0 * (1 - t) + p1 * t;
}
