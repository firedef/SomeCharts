using System;
using System.Collections.Generic;
using System.IO;
using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.data;
using SomeChartsUi.elements;
using SomeChartsUi.elements.charts.graph;
using SomeChartsUi.elements.charts.line;
using SomeChartsUi.elements.charts.pie;
using SomeChartsUi.elements.charts.scatter;
using SomeChartsUi.elements.other;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui.elements;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils.shaders;
using SomeChartsUiAvalonia.controls.opengl;
using SomeChartsUiAvalonia.controls.skia;
using SomeChartsUiAvalonia.impl.opengl.shaders;

namespace SomeChartsAvaloniaExamples.elements;

public static class ElementsExamples {

	private static int rectCount;
	public static void RunTest() {
		AvaloniaRunUtils.RunAfterStart(() => {
			AvaloniaGlChartsCanvas canvas = AvaloniaRunUtils.AddGlCanvas();

			TestRenderable r = new(canvas.canvas);
			canvas.AddElement(r);
			r.MarkDirty();
			r.isDynamic = true;

			r = new(canvas.canvas);
			r.transform = new(new(220, 220), float2.one, 0);
			canvas.AddElement(r);
			r.MarkDirty();
			r.isDynamic = true;
		});

		AvaloniaRunUtils.RunAvalonia();
	}

	public static void RunTeapot() {
		AvaloniaRunUtils.RunAfterStart(() => {
			AvaloniaGlChartsCanvas canvas = AvaloniaRunUtils.AddGlCanvas();

			MeshRenderer r = new(canvas.canvas, Path.GetFullPath("data/monkey.obj"));
			r.transform = new(new(0, -100), float3.one * 3200, 0);
			canvas.AddElement(r);
			r.MarkDirty();
			r.material = new(GlShaders.diffuse);
			//r.material.SetProperty("lightCol", new float3(1,0,0));
			r.material.SetProperty("shininess", 32f);
			Texture tex = canvas.canvas.factory.CreateTexture(Path.GetFullPath("data/checker.png"));
			r.material.SetProperty("texture0", tex);
			MeshRenderer r2 = r;
		});

		AvaloniaRunUtils.RunAvalonia();
	}
	
	public static void RunGraph() {
		AvaloniaRunUtils.RunAfterStart(() => {
			AvaloniaGlChartsCanvas canvas = AvaloniaRunUtils.AddGlCanvas();

			GraphChart chart = new(canvas.canvas);
			canvas.AddElement(chart);

			int c = 800;
			Random rnd = new();

			float2[] positions = new float2[c];
			indexedColor[] colors = new indexedColor[c];
			int[][] connections = new int[c][];

			for (int i = 0; i < c; i++) {
				positions[i] = new(rnd.NextSingle() * 50_000, rnd.NextSingle() * 50_000);
				colors[i] = rnd.NextSingle() > .8 ? theme.bad_ind : theme.normal_ind;
			}
			
			for (int i = 0; i < c; i++) {
				List<int> curConnections = new();
				
				for (int j = i; j < c; j++) {
					float dist = (positions[i] - positions[j]).length;
					if (dist < 2000) curConnections.Add(j);
				}

				connections[i] = curConnections.ToArray();
			}

			chart.nodePosition = new ArrayChartData<float2>(positions);
			chart.connections = new ArrayChartManagedData<int[]>(connections);
			chart.nodeColor = new ArrayChartData<indexedColor>(colors);
			chart.isDynamic = true;
		});

		AvaloniaRunUtils.RunAvalonia();
	}
	
	public static void RunRectPack() {
		AvaloniaRunUtils.RunAfterStart(() => {
			AvaloniaGlChartsCanvas canvas = AvaloniaRunUtils.AddGlCanvas();

			const int c = 8;
			RectPackVisualization[] packs = new RectPackVisualization[c];
			for (int i = 0; i < c; i++) {
				packs[i] = new(canvas.canvas);
				packs[i].transform = new(new(3000 * i, 0));
				canvas.AddElement(packs[i]);
			}

			color[] cols = {color.softRed, color.softPurple, color.softBlue, color.softCyan, color.softGreen};
			Random rnd = new();

			packs[0].beforeRender += () => {
				//if (rnd.NextDouble() < .5) return;
				float w = (float)rnd.NextDouble() * 24 + 20;
				float h = (float)rnd.NextDouble() * 32 + 10;
				color col = cols[2];

				for (int i = 0; i < c; i++)
					if (packs[i].AddRect(new(0, 0, w, h), col)) {
						Console.WriteLine(rectCount++);
						break;
					}
				//if (!r.AddRect(new(0, 0, w, h), c)) Console.WriteLine("failed");
			};

			//Console.WriteLine($"{sw.ElapsedMilliseconds}ms: {i}");
		});

		AvaloniaRunUtils.RunAvalonia();
	}

	public static void RunLabel() {
		AvaloniaRunUtils.RunAfterStart(() => {
			AvaloniaGlChartsCanvas canvas = AvaloniaRunUtils.AddGlCanvas();

			string txt = File.ReadAllText("data/alotoftext.txt");


			//txt = new(Enumerable.Range(20, 1000).Select(v => (char)v).ToArray());
			Label l = new(txt, canvas.canvas);
			canvas.AddElement(l);
			//l.color = "#eb4034";
			l.color = theme.accent0_ind;
			l.textScale = 1024;
			l.MarkDirty();

			//Shader postFx = GlShaders.bloom;
			//Material mat = new(postFx);
			//canvas.canvas.renderer.postProcessor = canvas.canvas.factory.CreatePostProcessor(mat);

			// l.beforeRender += () => {
			// 	float time = (float)DateTime.Now.TimeOfDay.TotalMilliseconds;
			// 	l.transform.rotation = new(0, MathF.PI * .1f * (time * .005f), 0);
			// };
		});

		AvaloniaRunUtils.RunAvalonia();
	}

	public static void RunRuler() {
		AvaloniaRunUtils.RunAfterStart(() => {
			AvaloniaChartsCanvas canvas = AvaloniaRunUtils.AddCanvas();
			canvas.AddElement(new Ruler(canvas.canvas) {
				orientation = Orientation.vertical,
				names = new FuncChartManagedData<string>(i => i.ToString(), -1),
				stickRange = new(0, 0, 10_000, 0),
				length = 1_000,
				lineLength = 10_000,
				isDynamic = true,
			});
			canvas.AddElement(new Ruler(canvas.canvas) {
				orientation = Orientation.horizontal,
				names = new FuncChartManagedData<string>(i => i.ToString(), -1),
				stickRange = new(0, 0, 0, 10_000),
				length = 1_000,
				lineLength = 10_000,
				isDynamic = true,
			});

			Func<RenderableBase, Transform> trFunc = _ => {
				float2 screenSize = canvas.screenSize;
				return new(screenSize - new float2(200, 100), 1, float3.zero, TransformType.screenSpace);
			};
			canvas.AddElement(new DebugLabel(canvas.canvas) {textScale = 16/*, transform = trFunc*/}, "top");
		});

		AvaloniaRunUtils.RunAvalonia();
	}

	public static void RunLineChart() {
		AvaloniaRunUtils.RunAfterStart(() => {
			AvaloniaGlChartsCanvas canvas = AvaloniaRunUtils.AddGlCanvas();
			const int rulerOffset = 1_000_000;

			canvas.AddElement(new Ruler(canvas.canvas) {
				drawLabels = true,
				orientation = Orientation.horizontal,
				length = rulerOffset,
				names = new FuncChartManagedData<string>(i => i.ToString(), 1),
				stickRange = new(0, 0, 0, rulerOffset),
				isDynamic = true,
			});
			
			canvas.AddElement(new Ruler(canvas.canvas) {
				drawLabels = true,
				orientation = Orientation.vertical,
				length = rulerOffset,
				names = new FuncChartManagedData<string>(i => (i * 100).ToString(), 1),
				stickRange = new(0, 0, rulerOffset, 0),
				isDynamic = true,
			});

			indexedColor[] lineColors = {theme.good_ind, theme.normal_ind, theme.bad_ind};

			for (int i = 0; i < lineColors.Length; i++) {
				int i1 = i;
				IChartData<float> data = new FuncChartData<float>(j => MathF.Sin((j + i1 * 100) * .1f) * 1000, 20480);
				IChartData<indexedColor> colors = new ConstChartData<indexedColor>(lineColors[i]);

				LineChart chart = new(data, colors, canvas.canvas) {
					isDynamic = true, 
					lineThickness = new ChartPropertyFunc<float>(r => 1 / r.canvas.transform.scale.animatedValue.x), 
					lineAlphaMul = 0.2f
				};
				canvas.AddElement(chart);
			}

			Material mat = new(GlShaders.bloom);
			mat.SetProperty("brightness", 4);
			canvas.canvas.renderer.postProcessor = canvas.canvas.factory.CreatePostProcessor(mat);
		});

		AvaloniaRunUtils.RunAvalonia();
	}
	
	public static void RunLotsOfLines() {
		AvaloniaRunUtils.RunAfterStart(() => {
			AvaloniaGlChartsCanvas canvas = AvaloniaRunUtils.AddGlCanvas();
			const int rulerOffset = 1_000_000;

			const int c = 10_000;

			for (int i = 0; i < c; i++) {
				int i1 = i;
				IChartData<float> data = new FuncChartData<float>(j => MathF.Sin((j + i1 * 100) * .1f) * 1000, 20480);
				IChartData<indexedColor> colors = new ConstChartData<indexedColor>(theme.normal_ind);

				LineChart chart = new(data, colors, canvas.canvas) {
					isDynamic = true, 
					lineThickness = new ChartPropertyFunc<float>(r => 1 / r.canvas.transform.scale.animatedValue.x), 
					lineAlphaMul = 0.2f,
					downsampleMultiplier = 5f,
				};
				canvas.AddElement(chart);
				chart.updateRareFrameSkip = 80;
			}

			Material mat = new(GlShaders.bloom);
			mat.SetProperty("brightness", 4);
			canvas.canvas.renderer.postProcessor = canvas.canvas.factory.CreatePostProcessor(mat);
		});

		AvaloniaRunUtils.RunAvalonia();
	}
	
	public static void RunPieChart() {
		AvaloniaRunUtils.RunAfterStart(() => {
			Fonts.FetchAvailableFontsFromPath("data/");
			
			AvaloniaGlChartsCanvas canvas = AvaloniaRunUtils.AddGlCanvas();
			const int rulerOffset = 1_000_000;

			int c = 10_000;
			float sc = 20;
			Random rnd = new();
			float3[] values = new float3[c];

			for (int i = 0; i < c; i++) {
				float x = rnd.NextSingle() * 10000;
				float y = rnd.NextSingle() * 10000;
				float s = rnd.NextSingle() * sc;

				values[i] = new(x, y, s);
			}
			
			IChartData<float3> data = new ArrayChartData<float3>(values);
			IChartData<indexedColor> colors = new ConstChartData<indexedColor>(theme.accent0_ind);
			IChartData<ScatterShape> shapes = new FuncChartData<ScatterShape>(i => (ScatterShape)((int)(i + DateTime.Now.TimeOfDay.TotalMilliseconds * .001) % 3), 1);

			ScatterChart scatter = new(canvas.canvas);
			scatter.values = data;
			scatter.colors = colors;
			scatter.shapes = shapes;
			scatter.isDynamic = true;
			scatter.scale = 10_000;
			//scatter.material = mat;
			
			canvas.AddElement(scatter);

			// indexedColor[] lineColors = {theme.accent0_ind, theme.default3_ind, theme.default4_ind, theme.default5_ind};
			//
			// IChartData<float> data = new FuncChartData<float>(j => MathF.Abs(MathF.Sin((j * 100) * .1f) * 100) + 10, 8);
			// IChartData<indexedColor> colors = new FuncChartData<indexedColor>(j => lineColors[j % lineColors.Length], 1);
			// IChartManagedData<string> names = new FuncChartManagedData<string>(j => $"#{j}", 1);
			//
			// PieChart pie = new(canvas.canvas);
			// pie.values = data;
			// pie.colors = colors;
			// pie.names = names;
			// pie.isDynamic = true;
			// canvas.AddElement(pie);

			//Material mat = new(GlShaders.bloom);
			//mat.SetProperty("brightness", 1);
			//canvas.canvas.renderer.postProcessor = canvas.canvas.factory.CreatePostProcessor(mat);
		});

		AvaloniaRunUtils.RunAvalonia();
	}

	public static void RunGl() {
		AvaloniaRunUtils.RunAfterStart(() => {
			AvaloniaGlChartsCanvas canvas = AvaloniaRunUtils.AddGlCanvas();
		});

		AvaloniaRunUtils.RunAvalonia();
	}
}