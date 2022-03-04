using System;
using System.IO;
using System.Reflection;
using SomeChartsUi.data;
using SomeChartsUi.elements;
using SomeChartsUi.elements.charts.line;
using SomeChartsUi.elements.other;
using SomeChartsUi.themes.colors;
using SomeChartsUi.themes.themes;
using SomeChartsUi.ui.elements;
using SomeChartsUi.utils;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.shaders;
using SomeChartsUi.utils.vectors;
using SomeChartsUiAvalonia.controls;
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

			MeshRenderer r = new(canvas.canvas, Path.GetFullPath("data/teapot.obj"));
			r.transform = new(new(0, -100), float3.one * 32, 0);
			canvas.AddElement(r);
			r.GenerateMesh();
			r.material = new(GlShaders.diffuse);
			//r.material.SetProperty("lightCol", new float3(1,0,0));
			r.material.SetProperty("shininess", 32);
			MeshRenderer r2 = r;
			r.beforeRender += () => {
				float time = (float)DateTime.Now.TimeOfDay.TotalMilliseconds;
				r2.transform.rotation = new(MathF.PI * .1f * (time * .0005f), MathF.PI * .25f, (time * .001f) * .1f);
			};
			
			// for (int i = 0; i < 256; i++) {
			// 	MeshRenderer r1 = new(canvas.canvas, r.mesh!);
			// 	r1.transform = new(new(200 * (i % 16), 200 * (i / 16)), float3.one * 2, new(MathF.PI*.1f,MathF.PI*.25f,0));
			// 	int i1 = i;
			// 	r1.beforeRender += () => {
			// 		float time = (float)DateTime.Now.TimeOfDay.TotalMilliseconds;
			// 		r1.transform.rotation = new(MathF.PI * .1f * (i1 * .2f + time * .0005f), MathF.PI * .25f, (i1 * .5f + time * .001f) * .1f);
			// 	};
			// 	canvas.AddElement(r1);
			// 	r1.shader = GlShaders.diffuse;
			// }
			//
			//
			// r = new(canvas.canvas, Path.GetFullPath("data/cube.obj"));
			// r.transform = new(new(8000, -1000), float3.one * 500, 0);
			// canvas.AddElement(r);
			// r.GenerateMesh();
			// //r.shader = GlShaders.diffuse;
			// r.beforeRender += () => {
			// 	float time = (float)DateTime.Now.TimeOfDay.TotalMilliseconds;
			// 	r.transform.rotation = new(MathF.PI * .1f * (time * .0005f), MathF.PI * .25f, (.5f + time * .001f) * .1f);
			// };
			//
			// for (int i = 0; i < 256; i++) {
			// 	MeshRenderer r1 = new(canvas.canvas, r.mesh!);
			// 	r1.transform = new(new(8000 + 200 * (i % 16), 200 * (i / 16)), float3.one * 50, new(MathF.PI*.1f,MathF.PI*.25f,0));
			// 	int i1 = i;
			// 	r1.beforeRender += () => {
			// 		float time = (float)DateTime.Now.TimeOfDay.TotalMilliseconds;
			// 		r1.transform.rotation = new(MathF.PI * .1f * (i1 * .2f + time * .0005f), MathF.PI * .25f, (i1 * .5f + time * .001f) * .1f);
			// 	};
			// 	canvas.AddElement(r1);
			// 	r1.shader = GlShaders.diffuse;
			// }
			
			// r = new(canvas.canvas, Path.GetFullPath("data/teapot.obj"));
			// r.transform = new(new(-200, 100), float3.one * 3, 0);
			// canvas.AddElement(r);
			// r.GenerateMesh();
			// r.shader = GlShaders.diffuse;
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