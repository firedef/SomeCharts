using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.data;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.layers.render;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.mesh.construction;
using SomeChartsUi.utils.mesh.construction.line;

namespace SomeChartsUi.elements.charts.graph; 

public class GraphChart : RenderableBase {
    public bool drawConnections = true;
    public bool drawNodes = true;

    public IChartData<float> nodeSize = new ConstChartData<float>(25f);
    public IChartData<float> constNodeSize = new ConstChartData<float>(5f);
    public IChartData<indexedColor> nodeColor = new ConstChartData<indexedColor>(theme.normal_ind);
    public IChartData<float2> nodePosition;
    public IChartManagedData<int[]> connections;
    
    private readonly List<Mesh> _meshes = new();

    public LineConstructor lineConstructor = new BasicLine();

    public GraphChart(ChartsCanvas owner) : base(owner) {
        _meshes.Add(mesh!);
    }
    
    public override void Render(RenderLayerId pass) {
        if (pass == RenderLayerId.transparent) foreach (Mesh m in _meshes) DrawMesh(material, m);
    }

    protected override void GenerateMesh() {
        foreach (Mesh m in _meshes) 
            m.Clear();

        int len = nodePosition.GetLength();
        if (len < 1) return;

        int meshIndex = -1;
        NextMesh(ref meshIndex);

        float2[] positions = new float2[len];
        nodePosition.GetValues(0, len, 0, positions);

        for (int i = 0; i < len; i++) {
            float2 p = positions[i];
            float s = nodeSize.GetValue(i) + constNodeSize.GetValue(i) / canvas.transform.scale.animatedValue.x;
            color col = nodeColor.GetValue(i).GetColor();
            
            if (drawConnections) {
                int[] nodeConnections = connections.GetValue(i);
                if (nodeConnections.Length > 0) {
                    color lineCol = col.WithAlpha(80);

                    foreach (int connection in nodeConnections) {
                        float2 p1 = positions[connection];
                        lineConstructor.Construct(_meshes[meshIndex], p, p1, null, lineCol, canvas, -0.01f);
                    }
                }
            }
            
            if (drawNodes)
                _meshes[meshIndex].AddPolygon(p, s, 4, col);
            
            if (_meshes[meshIndex].vertices.count > 60_000)
                NextMesh(ref meshIndex);
        }
        
        foreach (Mesh m in _meshes) m.OnModified();
    }

    private void NextMesh(ref int index) {
        index++;
        if (index >= _meshes.Count) _meshes.Add(canvas.factory.CreateMesh());
    }
}