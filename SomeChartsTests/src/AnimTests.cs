using NUnit.Framework;
using SomeChartsUi.ui.canvas.animation;

namespace SomeChartsTests;

[TestFixture]
public class AnimTests {
	[TestCase(0, ExpectedResult = 0)]
	[TestCase(1, ExpectedResult = 10)]
	[TestCase(10, ExpectedResult = 10)]
	[TestCase(.5f, ExpectedResult = 5)]
	[TestCase(.2f, ExpectedResult = 2)]
	public float TestAnimVariable(float t) {
		CanvasAnimVariable<float> anim = new();
		anim.currentValue = 10;
		anim.OnUpdate(t);

		return anim.animatedValue;
	}
}