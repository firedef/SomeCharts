using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.elements;
using SomeChartsUi.utils.mesh;
using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.ui.elements;

public abstract partial class RenderableBase {
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

	protected unsafe void AddCellsGrid(Mesh m, float2 start, float2 cellSize, int2 cellCount, color* colors, bool smooth = true, bool haveBorderColors = false) {
		int vOffset = m.vertices.count;
		int yAxisSize = haveBorderColors ? cellCount.y + 1 : cellCount.y;

		if (!smooth) {
			m.vertices.EnsureFreeSpace(cellCount.x * cellCount.y * 4);
			m.indexes.EnsureFreeSpace(cellCount.x * cellCount.y * 6);
			
			for (int x = 0; x < cellCount.x; x++) {
				for (int y = 0; y < cellCount.y; y++) {
					color col = colors[x * yAxisSize + y];

					float2 p0 = start + new float2(x, y) * cellSize;
					float2 p1 = p0 + cellSize;

					m.AddRect(
						new(p0.x, p0.y),
						new(p0.x, p1.y),
						new(p1.x, p1.y),
						new(p1.x, p0.y),
						col);
				}
			}
			
			return;
		}
		
		m.vertices.EnsureFreeSpace((cellCount.x + 1) * (cellCount.y + 1));
		m.indexes.EnsureFreeSpace(cellCount.x * cellCount.y * 6);

		for (int y = 0; y <= cellCount.y; y++) {
			for (int x = 0; x <= cellCount.x; x++) {
				color col = colors[haveBorderColors ? (x * yAxisSize + y) : (math.min(x, cellCount.x - 1) * cellCount.y + math.min(y, cellCount.y - 1))];
				m.vertices.Add(new(start + new float2(x, y) * cellSize, float3.front, float2.zero, col));
			}
		}
		
		for (int x = 0; x < cellCount.x; x++) {
			for (int y = 0; y < cellCount.y; y++) {
				ushort i0 = (ushort)(vOffset + y * (cellCount.x + 1) + x);
				ushort i1 = (ushort)(vOffset + (y + 1) * (cellCount.x + 1) + x);
				ushort i2 = (ushort)(vOffset + (y + 1) * (cellCount.x + 1) + x + 1);
				ushort i3 = (ushort)(vOffset + y * (cellCount.x + 1) + x + 1);
				
				m.indexes.Add(i0);
				m.indexes.Add(i1);
				m.indexes.Add(i2);
				m.indexes.Add(i0);
				m.indexes.Add(i2);
				m.indexes.Add(i3);
			}
		}
	}
}