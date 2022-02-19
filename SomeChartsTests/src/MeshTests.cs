using System;
using System.Collections.Generic;
using NUnit.Framework;
using SomeChartsUi.themes.colors;
using SomeChartsUi.utils.collections;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.vectors;

namespace SomeChartsTests; 

[TestFixture]
public class MeshTests {
	[Test]
	public void TestMesh() {
		Vertex[] verts = {
			new(new(00, 00, 0), new(0, 0), color.softRed),
			new(new(00, 10, 0), new(0, 1), color.softPurple),
			new(new(10, 10, 0), new(1, 1), color.softBlue),
			new(new(10, 00, 0), new(1, 0), color.softRed)
		};

		ushort[] indexes = {0, 1, 2, 0, 2, 3};

		using Mesh m = new(verts, indexes);
		m.vertices.ResetChanges();
		m.indexes.ResetChanges();
		
		List<Range> ch = m.vertices.GetChanges();
		Assert.IsEmpty(ch);
		
		verts[0].position.x = 0;
		m.SetVertices(verts);
		ch = m.vertices.GetChanges();
		Assert.IsEmpty(ch);

		verts[0].position.x = 5;
		m.SetVertices(verts);
		ch = m.vertices.GetChanges();
		Assert.AreEqual(ch.Count, 1);
	}
}