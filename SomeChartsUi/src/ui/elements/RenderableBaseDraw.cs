using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.elements;
using SomeChartsUi.utils.mesh;

namespace SomeChartsUi.ui.elements;

public abstract partial class RenderableBase {
	/// <summary>draws connected lines</summary>
	/// <param name="m">target-mesh</param>
	/// <param name="linePoints">points of lines <br/>the length is lineCount + 1</param>
	/// <param name="lineColors">point colors of lines <br/>the length is lineCount + 1</param>
	/// <param name="thickness">thickness of lines</param>
	/// <param name="len">line count</param>
	/// <param name="alphaMul">alpha color multiplier</param>
	protected unsafe void AddConnectedLines(Mesh m, float2* linePoints, color* lineColors, float thickness, int len, float alphaMul = 1) {
		int vCount = len * 4;
		int iCount = len * 6;

		m.vertices.EnsureFreeSpace(vCount);
		m.indexes.EnsureFreeSpace(iCount);

		for (int i = 0; i < len; i++) {
			float2 p0 = linePoints[i];
			float2 p1 = linePoints[i + 1];
			color c0 = lineColors[i];
			c0.aF *= alphaMul;
			
			float2 offset = Rot90DegFastWithLen(p0 - p1, thickness);
			
			m.AddRect(
				new(p0.x - offset.x, p0.y - offset.y), 
				new(p0.x + offset.x, p0.y + offset.y),
				new(p1.x + offset.x, p1.y + offset.y),
				new(p1.x - offset.x, p1.y - offset.y),
				c0);
		}
	}
	
	/// <summary>draw multiple square points</summary>
	/// <param name="m">target-mesh</param>
	/// <param name="elementPoints">point positions</param>
	/// <param name="elementColors">point colors</param>
	/// <param name="size">point size</param>
	/// <param name="len">point count</param>
	protected unsafe void AddPoints(Mesh m, float2* elementPoints, color* elementColors, float size, int len) {
		int vCount = len * 4;
		int iCount = len * 6;
		
		m.vertices.EnsureFreeSpace(vCount);
		m.indexes.EnsureFreeSpace(iCount);

		int curVIndex = m.vertices.count;
		for (int i = 0; i < len; i++) {
			float2 p0 = elementPoints[i];
			color c0 = elementColors[i];
			
			m.AddRect(
				new(p0.x - size, p0.y - size),
				new(p0.x - size, p0.y + size),
				new(p0.x + size, p0.y + size),
				new(p0.x + size, p0.y - size),
				c0);

			curVIndex += 4;
		}
	}

	/// <summary>draws non-connected straight lines with uniform length</summary>
	/// <param name="m">target-mesh</param>
	/// <param name="positions">start points of lines</param>
	/// <param name="length">length of each line</param>
	/// <param name="lineColor">color of each line</param>
	/// <param name="thickness">thickness of each line</param>
	/// <param name="orientation">orientation of lines (vertical/horizontal)</param>
	protected void AddStraightLines(Mesh m, float2[] positions, float length, color lineColor, float thickness, Orientation orientation, float zPos = 0) {
		int count = positions.Length;
		if (count == 0) return;
		int vCount = count * 4;
		int iCount = count * 6;
		
		m.vertices.EnsureCapacity(vCount);
		m.indexes.EnsureCapacity(iCount);
		
		float2 vec = (orientation & Orientation.vertical) != 0 ? new(0, 1) : new(1, 0);// vertical : horizontal
		float2 offset = vec.yx * length;
		
		for (int i = 0; i < count; i++) {
			float2 p0 = positions[i];
			float2 p1 = p0 + offset;
			
			m.AddRect(
				new(p0.x - thickness, p0.y - thickness, zPos),
				new(p0.x - thickness, p1.y + thickness, zPos),
				new(p1.x + thickness, p1.y + thickness, zPos),
				new(p1.x + thickness, p0.y - thickness, zPos),
				lineColor);
		}
	}

	
/*	/// <summary>draws non-connected lines</summary>
	/// <param name="linePoints">first and second points of lines <br/>the length is 2 * lineCount</param>
	/// <param name="lineColors">first and second point colors of lines <br/>the length is 2 * lineCount</param>
	/// <param name="thickness">thickness of lines</param>
	protected unsafe void DrawLines(float2[] linePoints, color[] lineColors, float thickness) {
		int len = linePoints.Length >> 1;
		int vCount = len * 4;
		int iCount = len * 6;

		float2* points = stackalloc float2[vCount];
		color* colors = stackalloc color[vCount];
		ushort* indexes = stackalloc ushort[iCount];

		for (int i = 0; i < len; i++) {
			float2 p0 = linePoints[i * 2];
			float2 p1 = linePoints[i * 2 + 1];
			color c0 = lineColors[i * 2];
			color c1 = lineColors[i * 2 + 1];
			
			float2 offset = Rot90DegFastWithLen(p0 - p1, thickness);
			
			int curVIndex = i * 4;
			int curIIndex = i * 6;
			
			points[curVIndex + 0] = new(p1.x - offset.x, p1.y - offset.y);
			points[curVIndex + 1] = new(p1.x + offset.x, p1.y + offset.y);
			points[curVIndex + 2] = new(p0.x + offset.x, p0.y + offset.y);
			points[curVIndex + 3] = new(p0.x - offset.x, p0.y - offset.y);

			colors[curVIndex + 0] = c0;
			colors[curVIndex + 1] = c1;
			colors[curVIndex + 2] = c1;
			colors[curVIndex + 3] = c0;

			indexes[curIIndex + 0] = (ushort)(curVIndex + 0);
			indexes[curIIndex + 1] = (ushort)(curVIndex + 1);
			indexes[curIIndex + 2] = (ushort)(curVIndex + 2);
			indexes[curIIndex + 3] = (ushort)(curVIndex + 0);
			indexes[curIIndex + 4] = (ushort)(curVIndex + 2);
			indexes[curIIndex + 5] = (ushort)(curVIndex + 3);
		}

		DrawVertices(points, null, colors, indexes, vCount, iCount);
	}
*/

	/// <summary>get positions on straight line</summary>
	/// <param name="offset">offset of lines</param>
	/// <param name="space">space between lines</param>
	/// <param name="count">line count</param>
	/// <param name="orientation">line orientation (vertical/horizontal)</param>
	/// <returns>positions</returns>
	protected float2[] GetPositions(float2 offset, float space, int count, Orientation orientation) {
		// get vector (direction)
		float2 vec = (orientation & Orientation.vertical) != 0 ? new(0, 1) : new(1, 0);// vertical : horizontal
		float offsetOnMainAxis = (offset * vec).sum;

		// get start and end points
		(float start, float end) = GetStartEndPos(offsetOnMainAxis, offsetOnMainAxis + count * space, orientation);
		if (start > end - .01f) return Array.Empty<float2>();

		// get count
		count = (int)Math.Floor((end - start) / space) + 1;
		if (count < 1) return Array.Empty<float2>();

		// snap to grid
		offset += MathF.Ceiling(start / space) * space * vec;

		// calculate array fill vars
		float2 add = vec * space;

		// reverse if necessary
		if ((orientation & Orientation.reversed) != 0) {
			offset += add;
			add *= -1;
		}

		// allocate and fill array
		float2[] arr = new float2[count];
		for (int i = 0; i < count; i++)
			arr[i] = offset + add * i;

		return arr;
	}

	public void AddLine(Mesh m, float2 p0, float2 p1, float thickness, color col) {
		float2 offset = Rot90DegFastWithLen(p0 - p1, thickness);
		
		m.AddRect(
			new(p1.x - offset.x, p1.y - offset.y), 
			new(p1.x + offset.x, p1.y + offset.y),
			new(p0.x + offset.x, p0.y + offset.y),
			new(p0.x - offset.x, p0.y - offset.y),
			col);
	}
}