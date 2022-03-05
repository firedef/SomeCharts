using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.OpenGL;
using Avalonia.Threading;
using SomeChartsUiAvalonia.controls.gl;
using SomeChartsUiAvalonia.controls.skia;

namespace SomeChartsAvaloniaExamples; 

public static class AvaloniaRunUtils {
	/// <summary>must been called before RunAvalonia()</summary>
	public static void RunAvaloniaCloseThread(int msDelay) =>
		RunAfterStart(async () => {
			await Task.Delay(msDelay);
			ClassicDesktopStyleApplicationLifetime lt = (ClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!;
			lt.Shutdown();
			lt.Dispose();
		});

	public static void RunAfterStart(Action a) => Dispatcher.UIThread.InvokeAsync(async () => {
		await Task.Delay(200);
		a();
	});
	
	public static void RunAfterStart(Func<Task> a) => Dispatcher.UIThread.InvokeAsync(async () => {
		await Task.Delay(200);
		await a();
	});

	/// <summary>must been called from main thread after RunAvaloniaCloseThread()</summary>
	public static void RunAvalonia() => RunAvalonia(Array.Empty<string>());

	public static AvaloniaChartsCanvas AddCanvas() {
		AvaloniaChartsCanvas canvas = new();
		App.mainWindow.Content = canvas;
		return canvas;
	}
	
	public static AvaloniaGlChartsCanvas AddGlCanvas() {
		AvaloniaGlChartsCanvas canvas = new();
		App.mainWindow.Content = canvas;
		return canvas;
	}
	
	[STAThread]
	private static void RunAvalonia(params string[] args) => BuildAvaloniaApp()
	   .StartWithClassicDesktopLifetime(args);

	private static AppBuilder BuildAvaloniaApp() =>
		AppBuilder.Configure<App>()
		          .UsePlatformDetect()
		          .With(new X11PlatformOptions{UseDeferredRendering = false, GlProfiles = new List<GlVersion> {new(GlProfileType.OpenGL, 4, 0)}})
		          .LogToTrace();
}