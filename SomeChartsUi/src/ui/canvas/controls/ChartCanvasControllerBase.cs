using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.ui.canvas.controls; 

public abstract class ChartCanvasControllerBase : ICanvasUpdate {
	protected ChartsCanvas owner;

	protected ChartCanvasControllerBase(ChartsCanvas owner) => this.owner = owner;

	public abstract void OnUpdate(float deltatime);
	
	//protected void Move(float2 dir) => owner
}