using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.data;
using SomeChartsUi.themes.colors;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.layers.render;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.mesh.construction;

namespace SomeChartsUi.elements.charts.heatmap; 

public class HeatmapChart : RenderableBase, IDownsample {
    public IChart2DData<float>? values;
    public Gradient? gradient;
    public IChart2DData<indexedColor>? colors;

    public bool smooth;

    public float downsampleMultiplier { get; set; } = 0.5f;
    public float elementScale { get; set; } = 100;

    private readonly int _meshesPerAxis;
    private readonly Mesh[] _meshes;

    public HeatmapChart(ChartsCanvas owner, int meshesPerAxis) : base(owner) {
        _meshesPerAxis = meshesPerAxis;
        _meshes = new Mesh[_meshesPerAxis * _meshesPerAxis];
        for (int i = 0; i < _meshesPerAxis * _meshesPerAxis; i++)
            _meshes[i] = canvas.factory.CreateMesh();
    }
    
    public override void Render(RenderLayerId pass) {
        if (pass == RenderLayerId.transparent) foreach (Mesh m in _meshes) DrawMesh(material, m);
    }

    protected override void GenerateMesh() {
        if (colors == null && (values == null || gradient == null)) throw new("(Values and gradient) or color must have value in heatmap");

        int2 length = values?.GetLength() ?? colors!.GetLength();
        
        int2 downsample = GetDownsample(downsampleMultiplier);
        (float2 startPos, float2 endPos) culledPositions = GetStartEndPos(float2.zero, length * elementScale);
        (float2 start, int2 count) = GetStartCountIndexes(culledPositions, elementScale * new int2(1 << downsample.x, 1 << downsample.y));
        int2 startIndex = new((int)(start.x / elementScale), (int)(start.y / elementScale));
        if (count.x <= 1 || count.y <= 1) return;

        int partCount = (count.x / _meshesPerAxis + 1) * (count.y / _meshesPerAxis + 1);
        
        float2 scale = elementScale * (1 << downsample.x);
        int2 c = new(count.x / _meshesPerAxis, count.y / _meshesPerAxis);
        float2 offset = c * scale;

        Parallel.For(0, _meshesPerAxis * _meshesPerAxis, () => new color[partCount], (i, _, chartColors) => {
            
            int x = i % _meshesPerAxis;
            int y = i / _meshesPerAxis;

            _meshes[x * _meshesPerAxis + y].Clear();
            GetColors(startIndex + new int2(x * (c.x << downsample.x), y * (c.y << downsample.y)), c + 1, downsample.x, chartColors);
            _meshes[x * _meshesPerAxis + y].AddCellsGrid(start + new float2(x * offset.x, y * offset.y), scale, c, chartColors, smooth, true);
            
            return chartColors;
        }, _ => { });
        
        foreach (Mesh m in _meshes) m.OnModified();
    }

    private unsafe void GetColors(int2 start, int2 count, int downsample, color[] dest) {
        int c = count.x * count.y;
        if (colors != null) {
            indexedColor* colorBuffer = stackalloc indexedColor[c];
            colors.GetValues(start, count, downsample, colorBuffer);

            for (int i = 0; i < c; i++)
                dest[i] = colorBuffer[i].GetColor();
            return;
        }


        float* buffer = stackalloc float[c];
        values!.GetValues(start, count, downsample, buffer);

        for (int i = 0; i < c; i++)
            dest[i] = gradient!.Eval(buffer[i]);
    }
}