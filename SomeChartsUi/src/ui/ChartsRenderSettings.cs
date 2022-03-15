namespace SomeChartsUi.ui;

public enum PolygonMode {
	fill,
	line,
	points
}

public static class ChartsRenderSettings {
	public static bool useDefaultMat = false;
	public static bool debugTextMat = false;
	public static float textThickness = 0.4f;
	public static PolygonMode polygonMode = PolygonMode.fill;
	public static int textQuality = 1;
	public static bool postProcessing = true;
}