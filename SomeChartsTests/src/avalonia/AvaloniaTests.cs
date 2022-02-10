using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using NUnit.Framework;
using SomeChartsUiAvalonia;

namespace SomeChartsTests.avalonia; 

[TestFixture]
public class AvaloniaTests {
	[Test]
	public void TestStartupAndShutdown() {
		AvaloniaTestUtils.RunAvaloniaCloseThread(200);
		AvaloniaTestUtils.RunAvalonia();
	}
}