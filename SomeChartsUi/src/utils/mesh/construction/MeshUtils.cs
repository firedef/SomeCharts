using System.Buffers;
using System.Runtime.InteropServices;
using MathStuff;
using MathStuff.vectors;
using SomeChartsUi.elements;
using SomeChartsUi.utils.vectors;

namespace SomeChartsUi.utils.mesh.construction; 

public static class MeshUtils {
    /// <summary>draws connected lines</summary>
    /// <param name="m">target-mesh</param>
    /// <param name="linePoints">points of lines <br/>the length is lineCount + 1</param>
    /// <param name="lineColors">point colors of lines <br/>the length is lineCount + 1</param>
    /// <param name="thickness">thickness of lines</param>
    /// <param name="len">line count</param>
    /// <param name="alphaMul">alpha color multiplier</param>
    public static unsafe void AddConnectedLines(this Mesh m, float2* linePoints, color* lineColors, float thickness, int len, float alphaMul = 1) {
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
    public static unsafe void AddPoints(this Mesh m, float2* elementPoints, color* elementColors, float size, int len) {
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
    public static void AddStraightLines(this Mesh m, float2[] positions, float length, color lineColor, float thickness, Orientation orientation, float zPos = 0) {
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

    public static void AddLine(this Mesh m, float2 p0, float2 p1, float thickness, color col, float z = 0) {
        float2 offset = Rot90DegFastWithLen(p0 - p1, thickness);
		
        m.AddRect(
            new(p1.x - offset.x, p1.y - offset.y, z), 
            new(p1.x + offset.x, p1.y + offset.y, z),
            new(p0.x + offset.x, p0.y + offset.y, z),
            new(p0.x - offset.x, p0.y - offset.y, z),
            col);
    }

    private static readonly float2[][] _polygonPoints = MakePolygonPoints();

    private static float2[][] MakePolygonPoints() {
        const int c = 16;
        float2[][] points = new float2[c][];

        points[0] = Array.Empty<float2>();
        points[1] = Array.Empty<float2>();
        points[2] = Array.Empty<float2>();

        for (int i = 3; i < c; i++) {
            float2[] curPoints = new float2[i];
            float angleStep = MathF.PI * 2 / i;

            for (int j = 0; j < i; j++)
                curPoints[j] = (angleStep * j).SinCos();

            points[i] = curPoints;
        }

        return points;
    }

    public static void AddPolygon(this Mesh m, float2 p, float radius, int sides, color col) {
        if (sides < 3) return;

        int vOffset = m.vertices.count;
        int vCount = sides;
        int iCount = (sides - 2) * 3;
        m.vertices.EnsureFreeSpace(vCount);
        m.indexes.EnsureFreeSpace(iCount);
		
        if (sides < 16) {
            for (int i = 0; i < sides; i++) {
                float2 sincos = _polygonPoints[sides][i];
                m.AddVertex(new(p + sincos * radius, float3.front, float2.zero, col));
            }
        }
        else {
            float angleStep = MathF.PI * 2 / sides;
            for (int i = 0; i < sides; i++) {
                float2 sincos = (angleStep * i).SinCos();
                m.AddVertex(new(p + sincos * radius, float3.front, float2.zero, col));
            }
        }

        for (int i = 2; i < sides; i++) {
            m.AddIndex(vOffset);
            m.AddIndex(vOffset + i - 1);
            m.AddIndex(vOffset + i);
        }
    }

    public static unsafe void AddCellsGrid(this Mesh m, float2 start, float2 cellSize, int2 cellCount, color[] colors, bool smooth = true, bool haveBorderColors = false) {
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

    public static Random rnd = new();

    /// <summary>rotate vector by 90 degrees and set length</summary>
    private static float2 Rot90DegFastWithLen(float2 p, float len) {
        len *= FastInverseSquareRoot(p.x * p.x + p.y * p.y);
        return new(-p.y * len, p.x * len);
    }

    /// <summary>1 / sqrt(num)</summary>
    private static unsafe float FastInverseSquareRoot(float num) {
        float x2 = num * .5f;
        float y = num;
        long i = *(long*)&y;
        i = 0x5f3759df - (i >> 1);
        y = *(float*)&i;
        y *= 1.5f - x2 * y * y;
        return y;
    }

    /// <summary>(0,1) for vertical and (1,0) for horizontal</summary>
    public static float2 GetOrientationVector(Orientation orientation) => (orientation & Orientation.vertical) != 0 ? new(0, 1) : new(1, 0);// vertical : horizontal

    private static T[] Rent<T>(int size) => ArrayPool<T>.Shared.Rent(size);
    private static unsafe T* RentMem<T>(int size) where T : unmanaged => (T*)NativeMemory.Alloc((nuint)((long)size * sizeof(T)));
    private static void Free<T>(T[] arr) => ArrayPool<T>.Shared.Return(arr);
    private static unsafe void FreeMem<T>(T* arr) where T : unmanaged => NativeMemory.Free(arr);
}