using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using NUnit.Framework;
using SomeChartsAvaloniaExamples;
using SomeChartsUiAvalonia;
using SomeChartsUiAvalonia.utils;

namespace SomeChartsTests.avalonia; 

[TestFixture]
public class AvaloniaTests {
	[Test]
	public void TestStartupAndShutdown() {
		AvaloniaRunUtils.RunAvaloniaCloseThread(200);
		AvaloniaRunUtils.RunAvalonia();
	}
}