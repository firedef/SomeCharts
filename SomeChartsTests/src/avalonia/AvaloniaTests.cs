using NUnit.Framework;
using SomeChartsAvaloniaExamples;

namespace SomeChartsTests.avalonia; 

[TestFixture]
public class AvaloniaTests {
	[Test]
	public void TestStartupAndShutdown() {
		AvaloniaRunUtils.RunAvaloniaCloseThread(200);
		AvaloniaRunUtils.RunAvalonia();
	}
}