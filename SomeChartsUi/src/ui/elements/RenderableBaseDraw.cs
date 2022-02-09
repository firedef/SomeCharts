using SomeChartsUi.elements;
using SomeChartsUi.themes.colors;
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

	protected unsafe void DrawVerticalLines(float2 offset, float space, float length, int count, color lineColor, float thickness) {
		(float start, float end) = GetStartEndPos(offset.x, offset.x + count * space, Orientation.horizontal);
		if (start > end - .01f) return;
		
		count = (int) Math.Floor((end - start) / space) + 1;
		if (count < 1) return;
		offset.x += MathF.Ceiling(start / space) * space;
		
		int vCount = count * 4;
		int iCount = count * 6;

		float2* points = stackalloc float2[vCount];
		color* colors = stackalloc color[vCount];
		ushort* indexes = stackalloc ushort[iCount];
		
		for (int i = 0; i < count; i++) {
			float2 p0 = new(offset.x + i * space, offset.y);
			float2 p1 = p0 + new float2(0, length);
			
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
	
	protected unsafe void DrawHorizontalLines(float2 offset, float space, float length, int count, color lineColor, float thickness) {
		(float start, float end) = GetStartEndPos(offset.y, offset.y + count * space, Orientation.vertical);
		if (start > end - .01f) return;
		
		count = (int) Math.Floor((end - start) / space) + 1;
		if (count < 2) return;
		offset.y += MathF.Ceiling(start / space) * space;
		
		int vCount = count * 4;
		int iCount = count * 6;

		float2* points = stackalloc float2[vCount];
		color* colors = stackalloc color[vCount];
		ushort* indexes = stackalloc ushort[iCount];
		
		for (int i = 0; i < count; i++) {
			float2 p0 = new(offset.x, offset.y + i * space);
			float2 p1 = p0 + new float2(length, 0);
			
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

}