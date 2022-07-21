using MathStuff;
using SomeChartsUi.themes.palettes;
using SomeChartsUi.themes.themes;

namespace SomeChartsUi.themes.colors; 

public class Gradient {
    public GradientPoint[] colors = Array.Empty<GradientPoint>();

    public Gradient() { }
    public Gradient(GradientPoint[] colors) => this.colors = colors;

    /// <summary>evaluate at specific time (0-1)</summary>
    public indexedColor Eval(float t, palette? p = null) {
        int len = colors.Length;
        if (len == 0) return theme.black_ind;
        if (len == 1 || t <= colors[0].time) return colors[0].col;
        if (t >= colors[^1].time) return colors[^1].col;

        int pos = 0;
        for (; pos < len; pos++) 
            if (colors[pos].time >= t) break;

        float normalizedTime = (t - colors[pos - 1].time) / (colors[pos].time - colors[pos - 1].time);
        return indexedColor.Lerp(colors[pos - 1].col, colors[pos].col, normalizedTime, p);
    }
}

public readonly record struct GradientPoint(float time, indexedColor col) {
    public readonly float time = time;
    public readonly indexedColor col = col;
}