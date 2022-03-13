using System;
using System.Collections.Generic;
using MathStuff;
using MathStuff.vectors;
using NUnit.Framework;
using SomeChartsUi.utils;

namespace SomeChartsTests.math; 

[TestFixture]
public class MathTests {
	[TestCaseSource(nameof(GetAabbSources))]
	public bool TestAabbIntersect(rect a, rect b, float2 offset) => Geometry.Intersects(a, b, offset);

	// a, b, offset
	public static IEnumerable<object> GetAabbSources() {
		yield return new TestCaseData(new rect(-100, -200, 50, 100), new rect(-80, -190, 80, 90), float2.zero).Returns(true);
		yield return new TestCaseData(new rect(-80, -190, 80, 90), new rect(-100, -200, 50, 100), float2.zero).Returns(true);
		yield return new TestCaseData(new rect(-80, -190, 80, 90), new rect(-80, -190, 80, 90), float2.zero).Returns(true);
		yield return new TestCaseData(new rect(-80, -190, 80, 90), new rect(-80, -400, 80, 90), float2.zero).Returns(false);
		yield return new TestCaseData(new rect(-280, -190, 80, 90), new rect(-80, -400, 80, 90), float2.zero).Returns(false);
		yield return new TestCaseData(new rect(-280, -190, 80, 90), new rect(-80, -400, 80, 200), float2.zero).Returns(false);
		yield return new TestCaseData(new rect(-280, -190, 160, 180), new rect(-80, -400, 400, 400), float2.zero).Returns(true);
		yield return new TestCaseData(new rect(-280, -190, 80, 90), new rect(-80, -400, 200, 10), float2.zero).Returns(false);
		yield return new TestCaseData(new rect(0, 0, 10, 20), new rect(0, 50, 10, 20), float2.zero).Returns(false);
		yield return new TestCaseData(new rect(0, 0, 10, 80), new rect(0, 50, 10, 20), float2.zero).Returns(true);
		yield return new TestCaseData(new rect(0, 0, 10, 80), new rect(0, 50, 10, 20), new float2(1000,1000)).Returns(false);
		yield return new TestCaseData(new rect(0, 0, 10, 80), new rect(0, 50, 10, 20), new float2(-1000,-1000)).Returns(false);
		yield return new TestCaseData(new rect(0, 0, 10, 80), new rect(0, 50, 10, 20), new float2(0,55)).Returns(false);
		yield return new TestCaseData(new rect(0, 0, 20, 160), new rect(0, 50, 20, 40), new float2(0,45)).Returns(true);
		yield return new TestCaseData(new rect(0, 0, 10, 80), new rect(0, 50, 10, 20), new float2(0,10)).Returns(true);
		yield return new TestCaseData(new rect(0, 0, 10, 20), new rect(0, -82, 10, 20), new float2(0,0)).Returns(false);
		yield return new TestCaseData(new rect(0, 0, 20, 20), new rect(-10, -10, 40, 40), new float2(0,0)).Returns(true);
		yield return new TestCaseData(new rect(0, 0, 20, 20), new rect(-10, -10, 25, 25), new float2(0,0)).Returns(true);
	}
	
	// a, b, aR, bR
	public static IEnumerable<object> GetObbSources() {
		const float rad = MathF.PI / 180;
		yield return new TestCaseData(new rect(-100, -200, 50, 100), new rect(-80, -190, 80, 90), 0, 0).Returns(true);
		yield return new TestCaseData(new rect(-100, -200, 50, 100), new rect(-80, -190, 80, 90), 180 * rad, 0).Returns(true);
		yield return new TestCaseData(new rect(-100, -200, 50, 100), new rect(-200, -200, 80, 90), 0, 0).Returns(true);
		yield return new TestCaseData(new rect(-100, -200, 50, 20), new rect(-100, -300, 80, 20), 0, 0).Returns(false);
		yield return new TestCaseData(new rect(-100, -200, 50, 20), new rect(-100, -300, 80, 20), 0, 90 * rad).Returns(true);
	}
}