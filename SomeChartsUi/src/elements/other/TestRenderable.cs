using SomeChartsUi.themes.colors;
using SomeChartsUi.ui.elements;
using SomeChartsUi.utils.rects;
using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.elements.other; 

public class TestRenderable : RenderableBase {
	protected override void Render() {
		rect r = new(-100, -100, 200, 200);
		float2[] points = {r.leftBottom, r.leftTop, r.rightTop, r.rightBottom};
		color[] colors = {color.red, color.softRed, color.blue, color.softBlue};
		ushort[] indexes = {0, 1, 2, 0, 2, 3};
		DrawVertices(points, null, colors, indexes);
		//Console.WriteLine("render");
	}
}