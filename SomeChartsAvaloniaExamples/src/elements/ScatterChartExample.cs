using System;
using MathStuff.vectors;
using SomeChartsUi.data;
using SomeChartsUi.elements.charts.scatter;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.utils.shaders;
using SomeChartsUiAvalonia.controls.opengl;
using SomeChartsUiAvalonia.impl.opengl.shaders;

namespace SomeChartsAvaloniaExamples.elements; 

public static class ScatterChartExample {
	// point count
	// ScatterChart handle at most 15000 points
	private const int _count = 10_000;
	private const float _bounds = 10_000;
	private const float _pointSizeMul = 80;
	private const float _pointSizeAdd = 10;
	private static Random _rnd = new();
	
	public static void Run() {
		AvaloniaRunUtils.RunAfterStart(AddElements);
		AvaloniaRunUtils.RunAvalonia();
	}

	private static void AddElements() {
		AvaloniaGlChartsCanvas canvas = AvaloniaRunUtils.AddGlCanvas();

		// generate random data
		(float3[] points, indexedColor[] colors, ScatterShape[] shapes) = GenerateRandomPoints();
		
		// values using float3
		// x,y - point position
		// z - point size
		IChartData<float3> pointsSrc = new ArrayChartData<float3>(points);
		
		IChartData<indexedColor> colorsSrc = new ArrayChartData<indexedColor>(colors);
		
		// you can skip shapes, because it's using default value (circles)
		IChartData<ScatterShape> shapesSrc = new ArrayChartData<ScatterShape>(shapes);

		// load shapes shader, so it will render triangles and circles
		// by default it uses basic shader, which render only quads
		// disable depth test for transparency (required by shape shader)
		Material mat = new(GlShaders.shapes);
		mat.depthTest = false;

		ScatterChart chart = canvas.AddScatterChart(pointsSrc, colorsSrc, shapesSrc);
		
		// set scale, so bound will render properly
		// you can disable setting drawBounds to false
		chart.scale = _bounds;
	}

	private static (float3[] points, indexedColor[] colors, ScatterShape[] shapes) GenerateRandomPoints() {
		float3[] points = new float3[_count];
		indexedColor[] colors = new indexedColor[_count];
		ScatterShape[] shapes = new ScatterShape[_count];

		for (int i = 0; i < _count; i++) {
			float x = RndPosX();
			float y = RndPosY();
			float s = RndSize();
			indexedColor col = x > _bounds * .5f ? theme.good_ind : theme.bad_ind;
			ScatterShape shape = y > _bounds * .5f ? ScatterShape.circle : ScatterShape.triangle;

			points[i] = new(x, y, s);
			colors[i] = col;
			shapes[i] = shape;
		}

		return (points, colors, shapes);
	}

	private static float RndPosX() => _rnd.NextSingle() * _bounds;
	private static float RndPosY() => _rnd.NextSingle() * _bounds;
	private static float RndSize() => _rnd.NextSingle() * _pointSizeMul + _pointSizeAdd;
}