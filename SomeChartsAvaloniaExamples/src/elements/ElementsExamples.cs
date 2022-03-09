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

			const string txt = @"
Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec sollicitudin purus quis molestie euismod. Morbi eget purus eget ex blandit interdum. Nam finibus tortor eu erat pulvinar pretium. Donec id bibendum ante. Suspendisse sed orci et felis euismod lacinia eget et diam. Phasellus pellentesque egestas arcu, congue dictum erat faucibus in. Proin venenatis scelerisque ligula, at euismod nisl varius vitae. In sit amet commodo orci. Quisque sodales ipsum mi. Vestibulum tristique libero et rutrum vestibulum. In ut massa luctus, rhoncus elit vel, suscipit tortor. Donec massa nibh, tempor tempus condimentum a, finibus id nunc. Integer tincidunt nulla vitae viverra tincidunt. Ut at dui quis libero elementum lacinia.

Proin ultricies tempus quam, eget mollis erat efficitur malesuada. Quisque tempus massa commodo placerat porta. Fusce vel placerat lorem. Nullam rhoncus enim vitae purus elementum, id fermentum nulla faucibus. In a vulputate magna, non sodales tortor. Sed sed faucibus dui. Mauris tincidunt hendrerit vestibulum. Fusce pretium vel lectus sit amet congue. Nunc rhoncus, mi sed sodales luctus, felis orci ultricies nisi, vel tincidunt magna arcu sit amet nibh. Nulla ullamcorper vel enim id ultricies. In hac habitasse platea dictumst. Sed nec sapien congue, scelerisque elit vel, fringilla purus. Mauris risus metus, congue vitae lacus ut, pharetra imperdiet augue. Integer vel luctus sem. Etiam ipsum magna, dignissim at velit eget, convallis placerat felis.

Phasellus commodo turpis vitae urna egestas, vel tristique felis lobortis. Nullam eu dolor non risus rhoncus feugiat ut ac est. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Vivamus commodo fringilla urna, eget aliquam sem porta sed. Proin porta tempus congue. Proin pretium neque vitae aliquam iaculis. Nam commodo nulla nec molestie hendrerit. Suspendisse maximus justo id lorem viverra, sit amet scelerisque massa laoreet. In id consequat nibh. Duis vitae dolor orci. Morbi sed faucibus mauris. Donec sodales accumsan sodales. Vivamus dui sapien, sodales sit amet metus ac, tempor scelerisque odio. Sed laoreet eu leo sed varius. Suspendisse sit amet velit sagittis, tempus orci sit amet, placerat dui.

Aliquam erat volutpat. Ut posuere eget ligula vitae egestas. Vivamus sed imperdiet urna. Nunc quis ipsum a enim pulvinar lacinia ac ac eros. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Nunc rutrum erat urna, id euismod mauris hendrerit vel. Nulla cursus nunc dolor, vel hendrerit elit tempus in.

Sed id pellentesque odio. Nam nec sodales eros. In mollis enim arcu, eu tincidunt libero eleifend ac. Pellentesque nec est eget magna varius porttitor eget vitae velit. Nam tincidunt eget sapien vel maximus. Nam non nibh felis. Proin venenatis, purus vel rhoncus euismod, urna enim interdum dui, vel laoreet sem lacus eu sapien. Etiam sagittis tortor magna, et bibendum lectus consequat sed.

Ut vulputate nunc et diam blandit, in interdum lorem auctor. Vivamus blandit mauris et placerat dictum. Ut viverra feugiat lacus, quis facilisis eros dignissim id. Aenean euismod tristique cursus. Duis porttitor diam feugiat gravida porttitor. Cras condimentum euismod libero pulvinar dictum. Quisque efficitur rutrum urna, pulvinar commodo justo eleifend at. Phasellus scelerisque aliquam mi et ornare. Sed ultricies ornare augue et rutrum. Nulla pulvinar arcu eu faucibus imperdiet. Proin eu ex dictum, efficitur eros id, ullamcorper arcu. Cras elementum, velit vitae scelerisque lobortis, felis justo accumsan sem, id commodo velit nulla ut lorem. Morbi bibendum elementum lorem condimentum gravida.

Nulla consectetur mi ante, eu blandit erat finibus eget. Nunc vel vulputate enim, at gravida risus. Nam fringilla nulla lacus, sed tempor lectus feugiat fringilla. Vivamus ultrices massa non tortor varius placerat. Mauris in massa eu quam condimentum ornare. Nulla facilisi. Cras convallis metus eu lorem pretium varius. Vestibulum at sapien sit amet lectus ultricies ullamcorper. Etiam a mi magna. Etiam a turpis non elit varius convallis quis vitae sem. Sed consectetur, leo nec feugiat ornare, libero tortor accumsan nisi, non fringilla augue risus laoreet dui. Curabitur vehicula turpis a lectus facilisis, nec facilisis massa interdum. In tempor consequat enim, eu efficitur lacus rutrum ac.

Integer et libero nisl. Nam nec sagittis mauris. Nunc eget condimentum ligula. Praesent vitae ipsum in metus finibus pellentesque. Phasellus pretium porta viverra. Praesent efficitur libero ut viverra volutpat. Phasellus sed ex quam. Nam euismod vulputate purus eget dictum.

Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Vivamus placerat sed nunc in vestibulum. Mauris luctus, sem ut pellentesque molestie, enim eros finibus nibh, ut accumsan ipsum mi eget urna. Vivamus elit elit, dictum id orci nec, eleifend rhoncus lacus. Etiam faucibus a nibh in aliquam. Sed dignissim libero ut fringilla blandit. Etiam vestibulum orci ut nunc pharetra, id aliquet urna lobortis. Sed semper consequat ligula non luctus. Pellentesque sit amet nulla molestie, auctor mi sit amet, sollicitudin neque. Morbi id neque blandit, finibus risus ac, rutrum nunc. Donec viverra mauris rhoncus elit iaculis venenatis. In eget lorem ut neque semper rhoncus vel quis sem. Integer eros augue, rhoncus et luctus ut, aliquet quis nisi. Curabitur nisi lectus, laoreet in lobortis et, condimentum non purus. Aenean malesuada magna ut bibendum placerat.

Vestibulum rhoncus est at porta ultricies. Morbi fringilla hendrerit leo gravida ultricies. Ut vel vehicula odio. Integer venenatis id lorem at lacinia. Nulla rhoncus lectus mauris, ac semper enim bibendum nec. Donec eget pulvinar ligula. Duis maximus varius ante, sed euismod elit euismod a. Suspendisse ac euismod sapien. Integer vel feugiat neque, sed tincidunt enim. Nam pellentesque, nibh non lacinia interdum, nulla justo dignissim est, non interdum erat enim sit amet velit. Phasellus tincidunt tempus lorem, vel malesuada augue porttitor a. Vivamus justo lorem, ullamcorper a diam in, vestibulum vehicula quam. Aliquam eget ex maximus, lobortis felis quis, pretium odio. Quisque aliquet est quis ligula efficitur eleifend. Aliquam eu diam blandit, facilisis urna sit amet, rhoncus velit. Nam tempor, turpis ut ultricies suscipit, mi nulla vestibulum mauris, eu tincidunt mi neque quis elit.

Aenean ipsum lorem, sagittis ac pellentesque ut, lobortis et nulla. Praesent sed tortor sed libero porttitor lacinia. Phasellus aliquet justo id rutrum ullamcorper. Aliquam at rutrum mi. Aenean ligula dolor, aliquet non iaculis vitae, consectetur sed dui. Pellentesque porta, magna non porttitor congue, tortor dui pharetra nisl, non maximus erat ligula eget dolor. Nulla laoreet urna dui, sit amet bibendum eros pretium a. Duis justo purus, eleifend in pretium non, pretium gravida urna. Phasellus feugiat venenatis nulla a bibendum. Nam suscipit purus felis, quis sollicitudin metus aliquam in.

Quisque hendrerit nulla quis massa vehicula viverra sit amet a erat. Nam maximus ligula non enim volutpat varius. Aliquam dolor orci, consequat at sapien at, fringilla viverra quam. Integer blandit ex lacus, nec tincidunt nisl tristique ac. Quisque a congue diam, sed tincidunt dolor. Donec pulvinar porta mattis. Nam nec dui placerat, congue erat nec, vulputate tellus. Nunc sed erat massa. Etiam tincidunt enim vel orci congue molestie. Vivamus justo risus, laoreet vitae porttitor nec, interdum maximus ipsum. Morbi risus ligula, placerat sit amet magna et, pulvinar eleifend nisl. Vestibulum vel augue dapibus, faucibus metus non, egestas odio. Donec rutrum, nunc in luctus accumsan, urna turpis eleifend augue, quis lacinia eros dolor ac mauris. Pellentesque mollis aliquet arcu, id dapibus lorem interdum id.

Sed tristique nibh eget rhoncus tincidunt. Ut facilisis sem eget imperdiet ornare. Mauris ornare elit at placerat ultricies. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Suspendisse posuere lacus quis enim pellentesque vestibulum. Pellentesque odio ex, fermentum tristique nunc et, fringilla suscipit dolor. Curabitur imperdiet, tellus molestie dapibus euismod, libero ipsum aliquam diam, sit amet semper erat elit eget tortor. Praesent sit amet augue non nulla congue interdum. Sed quis egestas nulla, quis laoreet felis. Quisque non eros vel arcu faucibus eleifend. Vivamus quis maximus magna. Suspendisse pulvinar id mauris ac commodo. Integer sit amet pharetra eros, id blandit quam. Praesent ac enim non lectus tincidunt tincidunt non varius tortor.

Nunc et volutpat nibh. Aliquam vestibulum venenatis finibus. Etiam tincidunt lectus id neque mattis consectetur. Cras consectetur mauris iaculis, tristique nisl eget, rutrum odio. Aenean lorem turpis, semper consectetur ligula et, accumsan elementum dui. Duis sit amet nunc turpis. Nullam sodales ligula nec magna venenatis efficitur. Donec consectetur mi non sem sodales, a interdum tellus scelerisque. Fusce a purus id felis tempor rutrum eu vel ante. Donec sapien risus, auctor id facilisis maximus, faucibus a risus. Praesent orci nibh, aliquet aliquet nisl id, dignissim suscipit libero. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Fusce at sem pulvinar, bibendum enim et, hendrerit nisl. Integer ut eros euismod, ullamcorper sapien at, sagittis metus. Aliquam interdum sapien at ultricies lobortis.

Duis facilisis nisi metus, nec porta ipsum pharetra ac. Sed egestas justo sit amet diam elementum, in venenatis eros auctor. Phasellus convallis commodo malesuada. Vestibulum tempus ligula odio, at luctus purus fermentum at. Suspendisse potenti. Mauris bibendum urna consectetur risus feugiat, pellentesque imperdiet ex rutrum. Donec et urna vitae felis vulputate elementum. Sed est nibh, euismod vitae luctus sit amet, vehicula eu metus. Morbi a cursus est. Ut vestibulum nulla ac orci suscipit ornare. Praesent sed risus elit. Phasellus sollicitudin mi eget urna dignissim facilisis. Fusce leo lorem, ultrices vel varius eget, posuere a magna. Etiam sed viverra quam. Sed congue quam mi, eu suscipit mi blandit nec. Vestibulum leo eros, feugiat nec est vel, lobortis porttitor lectus.

Aenean suscipit odio vitae tellus placerat semper. Pellentesque interdum et orci sed pulvinar. Nulla pulvinar, metus eget volutpat molestie, risus est fringilla ligula, sed scelerisque erat lorem in dolor. In quis risus id lorem dictum porta non eget nulla. Proin pharetra tellus lacus, et lobortis turpis ultrices sit amet. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent nec purus id urna mattis elementum.";


			Label l = new(txt, canvas.canvas);
			canvas.AddElement(l);
			l.color = new("#eb4034");
			l.MarkDirty();

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
				lineLength = 10_000
			});
			canvas.AddElement(new Ruler(canvas.canvas) {
				orientation = Orientation.horizontal,
				names = new FuncChartManagedData<string>(i => i.ToString(), -1),
				stickRange = new(0, 0, 0, 10_000),
				length = 1_000,
				lineLength = 10_000
			});

			Func<RenderableBase, RenderableTransform> trFunc = _ => {
				float2 screenSize = canvas.screenSize;
				return new(screenSize - new float2(200, 100), 1, float3.zero, TransformType.screenSpace);
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
				stickRange = new(0, 0, 0, rulerOffset)
			});

			canvas.AddElement(new Ruler(canvas.canvas) {
				drawLabels = true,
				orientation = Orientation.vertical,
				length = rulerOffset,
				names = new FuncChartManagedData<string>(i => (i * 100).ToString(), 1),
				stickRange = new(0, 0, rulerOffset, 0)
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