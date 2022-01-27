using SomeChartsUi.ui.canvas.animation;
using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.ui.canvas; 

public class ChartCanvasTransform {
	public CanvasAnimVariable<float2> position = new(float2.zero);
	public CanvasAnimVariable<float2> zoom = new(float2.one);
}