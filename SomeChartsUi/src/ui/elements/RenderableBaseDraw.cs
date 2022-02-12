using System.Buffers;
using SomeChartsUi.elements;
using SomeChartsUi.themes.colors;
using SomeChartsUi.ui.text;
using SomeChartsUi.utils;
using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.ui.elements; 

public abstract partial class RenderableBase {
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
			
			points[curVIndex + 0] = new(p0.x - offset.x, p0.y - offset.y);
			points[curVIndex + 1] = new(p0.x - offset.x, p1.y + offset.y);
			points[curVIndex + 2] = new(p1.x + offset.x, p1.y + offset.y);
			points[curVIndex + 3] = new(p1.x + offset.x, p0.y - offset.y);

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

	protected unsafe void DrawStraightLines(float2[] positions, float length, color lineColor, float thickness, Orientation orientation) {
		int count = positions.Length;
		if (count == 0) return;
		int vCount = count * 4;
		int iCount = count * 6;

		float2* points = stackalloc float2[vCount];
		color* colors = stackalloc color[vCount];
		ushort* indexes = stackalloc ushort[iCount];
		
		float2 vec = (orientation & Orientation.vertical) != 0 ? new(0, 1) : new(1, 0); // vertical : horizontal
		float2 offset = vec.yx * length;
		
		for (int i = 0; i < count; i++) {
			float2 p0 = positions[i];
			float2 p1 = p0 + offset;
			
			int curVIndex = i * 4;
			int curIIndex = i * 6;
			
			points[curVIndex + 0] = new(p0.x - thickness, p0.y - thickness);
			points[curVIndex + 1] = new(p0.x - thickness, p1.y + thickness);
			points[curVIndex + 2] = new(p1.x + thickness, p1.y + thickness);
			points[curVIndex + 3] = new(p1.x + thickness, p0.y - thickness);

			colors[curVIndex + 0] = lineColor;
			colors[curVIndex + 1] = lineColor;
			colors[curVIndex + 2] = lineColor;
			colors[curVIndex + 3] = lineColor;

			indexes[curIIndex + 0] = (ushort)(curVIndex + 0);
			indexes[curIIndex + 1] = (ushort)(curVIndex + 1);
			indexes[curIIndex + 2] = (ushort)(curVIndex + 2);
			indexes[curIIndex + 3] = (ushort)(curVIndex + 0);
			indexes[curIIndex + 4] = (ushort)(curVIndex + 2);
			indexes[curIIndex + 5] = (ushort)(curVIndex + 3);
		}

		DrawVertices(points, null, colors, indexes, vCount, iCount);
	}

	protected void DrawText(string[] txt, float2[] positions, FontData fontData, color color, float scale = 12) => DrawText(txt, positions, fontData, color, scale, ..);
	
	protected void DrawText(string[] txt, float2[] positions, FontData fontData, color color, float scale, Range range) {
		int count = positions.Length;
		int s = range.Start.IsFromEnd ? count - range.Start.Value : range.Start.Value;
		int e = range.End.IsFromEnd ? count - range.End.Value : range.End.Value;
		for (int i = s; i < e; i++) DrawText(txt[i], positions[i]/scale, color, fontData, scale);
	}

	protected float2[] GetPositions(float2 offset, float space, int count, Orientation orientation) {
		// get vector (direction)
		float2 vec = (orientation & Orientation.vertical) != 0 ? new(0, 1) : new(1, 0); // vertical : horizontal
		float offsetOnMainAxis = (offset * vec).sum;
		
		// get start and end points
		(float start, float end) = GetStartEndPos(offsetOnMainAxis, offsetOnMainAxis + count * space, orientation);
		if (start > end - .01f) return Array.Empty<float2>();
		
		// get count
		count = (int) Math.Floor((end - start) / space) + 1;
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
}