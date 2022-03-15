using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.data;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui.canvas;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.layers.render;
using SomeChartsUi.ui.text;

namespace SomeChartsUi.elements.charts.pie;

public class PieChart : RenderableBase, IDownsample {
	private const int _maxQuality = 1024;
	
	public IChartData<float> values = null!;
	public IChartManagedData<string> names = null!;
	public IChartData<indexedColor> colors = null!;

	public Font font;
	private TextMesh _textMesh;

	/// <summary>z-axis rotation of chart, in radians</summary>
	public float rotation;

	/// <summary>hole size</summary>
	public float innerScale = 400;

	/// <summary>pie size</summary>
	public float outerScale = 800;
	
	public bool drawLabels = true;

	/// <summary>length of lines using for labels <br/><br/>depends on outerScale</summary>
	public float labelLineLength = 2;

	/// <summary>get outerScale for each segment separately</summary>
	public Func<int, float>? getOuterScale;
	
	/// <summary>get innerScale for each segment separately</summary>
	public Func<int, float>? getInnerScale;

	public indexedColor labelColor = theme.default5_ind;
	
	public float downsampleMultiplier { get; set; } = .5f;
	public float elementScale { get; set; } = 100;
	
	public PieChart(ChartsCanvas owner) : base(owner) {
		_textMesh = canvas.factory.CreateTextMesh(this);
		font = canvas.GetDefaultFont();
	}


	protected override unsafe void GenerateMesh() {
		int len = values.GetLength();
		if (len < 1) return;

		if (!IsVisible()) return;
		
		mesh!.Clear();
		_textMesh.ClearMeshes();

		if (rotation >= MathF.PI * 2) rotation -= MathF.PI * 2;
		if (rotation < 0) rotation += MathF.PI * 2;

		// * sample values
		float* pieValues = stackalloc float[len];
		indexedColor* pieColors = stackalloc indexedColor[len];
		values.GetValues(0, len, 0, pieValues);
		colors.GetValues(0, len, 0, pieColors);

		// * calculate quality (side count)
		int quality = (int)Math.Clamp(outerScale * canvasScale.avg, 8, _maxQuality);

		// * calculate mesh data
		float valueSum = 0;
		int sideCountSum = 0;
		ushort* sideCount = stackalloc ushort[len];
		int iCount = 0;

		for (int i = 0; i < len; i++)
			valueSum += pieValues[i];
		for (int i = 0; i < len; i++) {
			float percent = pieValues[i] / valueSum;
			sideCount[i] = (ushort)Math.Max(percent * quality, 2);
			sideCountSum += sideCount[i];
			iCount += (sideCount[i] - 1) * 6;
		}

		int vCount = sideCountSum * 2;

		// * allocate mesh data
		mesh.vertices.EnsureCapacity(vCount);
		mesh.indexes.EnsureCapacity(iCount);
		mesh.vertices.count = vCount;
		mesh.indexes.count = iCount;

		// * fill vertices and indexes
		int vPos = 0;
		int iPos = 0;
		float rotOffset = rotation;
		for (int i = 0; i < len; i++) {
			float curOutScale = getOuterScale?.Invoke(i) ?? outerScale;
			float curInScale = getInnerScale?.Invoke(i) ?? innerScale;
			float rot = pieValues[i] / valueSum * MathF.PI * 2;

			color col = pieColors[i].GetColor();
			ushort curSideCount = sideCount[i];
			float step = rot / (curSideCount - 1);

			for (int v = 0; v < curSideCount; v++) {
				float currentAngle = step * v + rotOffset;
				float sin = MathF.Sin(currentAngle);
				float cos = MathF.Cos(currentAngle);
				int vPosCur = vPos + (v << 1);

				mesh.vertices[vPosCur+0] = new(new(sin * curInScale, cos * curInScale), float3.front, float2.zero, col);
				mesh.vertices[vPosCur+1] = new(new(sin * curOutScale, cos * curOutScale), float3.front, float2.zero, col);
			}

			for (int v = 0; v < curSideCount - 1; v++) {
				int vPosCur = vPos + (v << 1);
				int iPosCur = iPos + v * 6;

				mesh.indexes[iPosCur + 0] = (ushort)(vPosCur);
				mesh.indexes[iPosCur + 1] = (ushort)(vPosCur+1);
				mesh.indexes[iPosCur + 2] = (ushort)(vPosCur+3);
				mesh.indexes[iPosCur + 3] = (ushort)(vPosCur);
				mesh.indexes[iPosCur + 4] = (ushort)(vPosCur+3);
				mesh.indexes[iPosCur + 5] = (ushort)(vPosCur+2);
			}
			
			if (drawLabels && curSideCount > 2) {
				float lineLen1Mul = outerScale * 3;
				float lineLen2Mul = outerScale;
				float midAngle = rotOffset + rot * .5f;
				if (midAngle > MathF.PI * 2) midAngle -= MathF.PI * 2;
				
				float sin = MathF.Sin(midAngle);
				float cos = MathF.Cos(midAngle);
				float lineLen = lineLen1Mul * .5f * labelLineLength;
				
				float lineLen2 = MathF.Cos(midAngle) * lineLen2Mul;
				float line2Angle = MathF.PI * .25f;
				if (midAngle >= MathF.PI) {
					lineLen = -lineLen;
					line2Angle = -line2Angle;
				}
				if (midAngle is >= MathF.PI * .5f and <= MathF.PI * .5f * 3) line2Angle = -line2Angle;
				
				float2 lineStart = new(sin * curOutScale, cos * curOutScale);
				float2 line2 = new(MathF.Sin(line2Angle) * lineLen2, MathF.Cos(line2Angle) * lineLen2);
				float2 lineEnd = new(lineStart.x + line2.x, lineStart.y + line2.y);
				float2 lineEnd2 = new(lineStart.x + lineLen - curOutScale * labelLineLength * MathF.CopySign(MathF.Sin(midAngle), lineLen), lineStart.y + line2.y);
				float2 labelPos = new(lineEnd2.x, lineEnd2.y + 4);

				float thickness = 1f / canvasScale.avg;
				AddLine(mesh, lineStart, lineEnd, thickness, col.WithAlpha(255));
				AddLine(mesh, lineEnd, lineEnd2, thickness, col.WithAlpha(255));

				string name = string.Format(names.GetValue(i), pieValues[i], pieValues[i] / valueSum * 100, i);
				_textMesh.GenerateMesh(name, font, 16 / canvasScale.avg, labelColor.GetColor(), new(labelPos));
			}
			
			vPos += curSideCount * 2;
			iPos += (curSideCount - 1) * 6;
			rotOffset += rot;
		}

		mesh.OnModified();
	}

	public override void Render(RenderLayerId pass) {
		if (!IsVisible()) return;
		if (pass == RenderLayerId.opaque && IsVisible()) DrawMesh(material);
		if (pass == RenderLayerId.ui && IsVisible()) _textMesh.Draw();
	}
	
	private bool IsVisible() {
		float approximateSize = outerScale;
		if (drawLabels) approximateSize += (outerScale + 1) * labelLineLength;
		return IsVisibleWithTransform(float2.zero, approximateSize);
	}
}