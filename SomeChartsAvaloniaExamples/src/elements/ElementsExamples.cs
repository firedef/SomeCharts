using System;
using System.IO;
using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.data;
using SomeChartsUi.elements;
using SomeChartsUi.elements.charts.line;
using SomeChartsUi.elements.other;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui.elements;
using SomeChartsUi.utils.shaders;
using SomeChartsUiAvalonia.controls.gl;
using SomeChartsUiAvalonia.controls.skia;
using SomeChartsUiAvalonia.utils;

namespace SomeChartsAvaloniaExamples.elements; 

public static class ElementsExamples {
	public static void RunTest() {
		AvaloniaRunUtils.RunAfterStart(() => {
			AvaloniaGlChartsCanvas canvas = AvaloniaRunUtils.AddGlCanvas();

			TestRenderable r = new(canvas.canvas);
			canvas.AddElement(r);
			r.GenerateMesh();
			r.isDynamic = true;
			
			r = new(canvas.canvas);
			r.transform = new(new(220, 220), float2.one, 0);
			canvas.AddElement(r);
			r.GenerateMesh();
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
			r.GenerateMesh();
			r.material = new(GlShaders.diffuse);
			//r.material.SetProperty("lightCol", new float3(1,0,0));
			r.material.SetProperty("shininess", 32f);
			Texture tex = canvas.canvas.factory.CreateTexture(Path.GetFullPath("data/checker.png"));
			r.material.SetProperty("texture0", tex);
			MeshRenderer r2 = r;
		});
		
		AvaloniaRunUtils.RunAvalonia();
	}

	private static int rectCount = 0;
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
			
			color[] cols = { color.softRed, color.softPurple, color.softBlue, color.softCyan, color.softGreen };
			Random rnd = new();

			packs[0].beforeRender += () => {
				//if (rnd.NextDouble() < .5) return;
				float w = (float)rnd.NextDouble() * 24 + 20;
				float h = (float)rnd.NextDouble() * 32 + 10;
				color col = cols[2];

				for (int i = 0; i < c; i++) {
					if (packs[i].AddRect(new(0, 0, w, h), col)) {
						Console.WriteLine(rectCount++);
						break;
					}
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

			Label l = new("amogus", canvas.canvas);
			canvas.AddElement(l);
			l.color = new("#FF8855");
			l.GenerateMesh();
			
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
			canvas.AddElement(new Ruler(canvas.canvas)  {
				orientation = Orientation.vertical, 
				names = new FuncChartManagedData<string>(i => i.ToString(), -1), 
				stickRange = new(0, 0, 10_000, 0),
				length = 1_000,
				lineLength = 10_000,
			});
			canvas.AddElement(new Ruler(canvas.canvas)  {
				orientation = Orientation.horizontal, 
				names = new FuncChartManagedData<string>(i => i.ToString(), -1), 
				stickRange = new(0, 0, 0, 10_000),
				length = 1_000,
				lineLength = 10_000,
			});

			Func<RenderableBase, RenderableTransform> trFunc = _ => {
				float2 screenSize = canvas.screenSize;
				return new(screenSize- new float2(200,100), 1, float3.zero, TransformType.screenSpace);
			};
			canvas.AddElement(new DebugLabel(canvas.canvas) {textScale = 16/*, transform = trFunc*/}, "top");
		});
		
		AvaloniaRunUtils.RunAvalonia();
	}
	
	public static void RunLineChart() {
		AvaloniaRunUtils.RunAfterStart(() => {
			AvaloniaChartsCanvas canvas = AvaloniaRunUtils.AddCanvas();
			const int rulerOffset = 1_000_000;

			canvas.AddElement(new Ruler(canvas.canvas) {
				drawLabels = true,
				orientation = Orientation.horizontal,
				length = rulerOffset,
				names = new FuncChartManagedData<string>(i => i.ToString(), 1),
				stickRange = new(0, 0, 0, rulerOffset),
			});
			
			canvas.AddElement(new Ruler(canvas.canvas) {
				drawLabels = true,
				orientation = Orientation.vertical,
				length = rulerOffset,
				names = new FuncChartManagedData<string>(i => (i * 100).ToString(), 1),
				stickRange = new(0, 0, rulerOffset, 0),
			});
			
			IChartData<float> data = new FuncChartData<float>(i => MathF.Sin(i * .1f) * 1000, 2048);
			IChartData<indexedColor> colors = new ConstChartData<indexedColor>(new(theme.good_ind));
			canvas.AddElement(new LineChart(data, colors, canvas.canvas));
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