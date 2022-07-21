using MathStuff;
using SomeChartsUi.themes.palettes;
using SomeChartsUi.themes.themes;

namespace SomeChartsUi.themes.colors; 

public class Gradient {
    private GradientPoint[] _points = Array.Empty<GradientPoint>();

    public Gradient() { }
    public Gradient(params GradientPoint[] colors) => _points = colors.OrderBy(v => v.time).ToArray();
    public Gradient(params (float t, indexedColor c)[] colors) => _points = colors.Select(v => new GradientPoint(v.t, v.c)).OrderBy(v => v.time).ToArray();
    
    public Gradient(params indexedColor[] colors) {
        float step = 1f / colors.Length;
        _points = colors.Select((v, i) => new GradientPoint(i * step, v)).OrderBy(v => v.time).ToArray();
    }

    /// <summary>evaluate at specific time (0-1)</summary>
    public color Eval(float t, palette? p = null) {
        int len = _points.Length;
        if (len == 0) return theme.globalTheme.black;
        if (len == 1 || t <= _points[0].time) return _points[0].col.GetColor(p);
        if (t >= _points[^1].time) return _points[^1].col.GetColor(p);

        int pos = 0;
        for (; pos < len; pos++) 
            if (_points[pos].time >= t) break;

        float normalizedTime = (t - _points[pos - 1].time) / (_points[pos].time - _points[pos - 1].time);
        return indexedColor.Lerp(_points[pos - 1].col, _points[pos].col, normalizedTime, p);
    }

    public int FindCeil(float t) {
        int len = _points.Length;
        if (len == 0) return -1;
        if (len == 1 || t <= _points[0].time) return 0;
        if (t >= _points[^1].time) return len - 1;
        
        int pos = 0;
        for (; pos < len; pos++) 
            if (_points[pos].time >= t) break;
        return pos;
    }

    public int FindNearest(float t) {
        int i = FindCeil(t);
        if (i == 0) return i;

        float t0 = _points[i - 1].time;
        float t1 = _points[i].time;

        return math.abs(t0 - t) > math.abs(t1 - t) ? i : i - 1;
    }

    public void Add(GradientPoint p) => _points = _points.Append(p).OrderBy(v => v.time).ToArray();

    public void Remove(GradientPoint p) {
        List<GradientPoint> l = _points.ToList();
        l.Remove(p);
        _points = l.ToArray();
    }

    public void Clear() => _points = Array.Empty<GradientPoint>();

    public void Replace(int at, GradientPoint p) {
        _points[at] = p;
        _points = _points.OrderBy(v => v.time).ToArray();
    }

    public color this[float t] => Eval(t);
    public color this[float t, palette? p] => Eval(t, p);

    public static Gradient FromColor(indexedColor col, float startAlpha = 0f, float endAlpha = 1f) => new((startAlpha, col), (endAlpha, col));
}

public readonly record struct GradientPoint(float time, indexedColor col) {
    public readonly float time = time;
    public readonly indexedColor col = col;
}