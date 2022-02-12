using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace SomeChartsAvaloniaExamples {
	public class App : Application {
		public static MainWindow mainWindow = null!;
		
		public override void Initialize() {
			AvaloniaXamlLoader.Load(this);
		}

		public override void OnFrameworkInitializationCompleted() {
			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
				desktop.MainWindow = mainWindow = new();
				desktop.MainWindow.Renderer.DrawFps = true;
			}

			base.OnFrameworkInitializationCompleted();
		}
	}
}