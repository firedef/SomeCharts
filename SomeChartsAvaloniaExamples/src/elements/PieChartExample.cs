using SomeChartsUi.data;
using SomeChartsUi.elements.charts.pie;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUiAvalonia.controls.opengl;

namespace SomeChartsAvaloniaExamples.elements; 

public static class PieChartExample {
	private static readonly float[] _series = {1, 16, 9, 33, 2, 3, 2};
	private static readonly indexedColor[] _colors = {
		theme.accent0_ind,
		theme.default3_ind, 
		theme.default4_ind, 
		theme.default5_ind,
		theme.normal_ind,
		theme.default6_ind,
		theme.default7_ind,
	};

	public static void Run() {
		// call RunAfterStart before running avalonia
		// it will execute after application open
		// RunAvalonia() blocks current thread
		AvaloniaRunUtils.RunAfterStart(AddElements);
		AvaloniaRunUtils.RunAvalonia();
	}

	private static void AddElements() {
		AvaloniaGlChartsCanvas canvas = AvaloniaRunUtils.AddGlCanvas();
		
		IChartData<float> values = new ArrayChartData<float>(_series);
		IChartData<indexedColor> colors = new ArrayChartData<indexedColor>(_colors);

		// pie chart provides additional data to names
		// {0} - value
		// {1} - percent (100 is 100%)
		// {2} - element id
		// to truncate output of percent you can add ':0.00'
		IChartManagedData<string> names = new FuncChartManagedData<string>(i => $"#{i}: {{1:0.00}}%", 1);

		PieChart chart = canvas.AddPieChart(values, colors, names);
		chart.isDynamic = true;
	}
}