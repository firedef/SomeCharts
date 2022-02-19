using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using SomeChartsAvaloniaExamples.elements;

namespace SomeChartsAvaloniaExamples {
	class Program {
		[STAThread]
		public static void Main(string[] args) {
			ElementsExamples.RunGl();
		}
	}
}