using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using SomeChartsUiAvalonia;

namespace SomeChartsTests.avalonia; 

public static class AvaloniaTestUtils {
	/// <summary>
	/// must been called before RunAvalonia()
	/// </summary>
	public static Thread RunAvaloniaCloseThread(int msDelay) {
		Thread avaThread = new(_ => Dispatcher.UIThread.InvokeAsync(async () => {
			await Task.Delay(msDelay);
			ClassicDesktopStyleApplicationLifetime lt = (ClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!;
			lt.Shutdown();
			lt.Dispose();
		}));
		avaThread.Start();
		return avaThread;
	}

	/// <summary>
	/// must been called from main thread after RunAvaloniaCloseThread()
	/// </summary>
	public static void RunAvalonia() => RunAvalonia(Array.Empty<string>());
	
	[STAThread]
	public static void RunAvalonia(params string[] args) => BuildAvaloniaApp()
	   .StartWithClassicDesktopLifetime(args);

	public static AppBuilder BuildAvaloniaApp() =>
		AppBuilder.Configure<App>()
		          .UsePlatformDetect()
		          .LogToTrace();
}