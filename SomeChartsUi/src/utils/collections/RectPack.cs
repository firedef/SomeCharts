using MathStuff;
using MathStuff.vectors;

namespace SomeChartsUi.utils.collections; 

public class RectPack {
	public float2 size;
	public float2 padding;
	public List<rect> shelfs = new(); // x y w h

	public RectPack(float2 size, float2 padding) {
		this.size = size;
		this.padding = padding;
	}

	public float2 Pack(float2 s) {
		int c = shelfs.Count;

		s += padding * 2;
		
		if (c == 0) {
			rect shelf = new(0, 0, s.x, s.y);
			shelfs.Add(shelf);
			return padding;
		}

		float totalShelfHeight = 0;

		int bestShelf = -1;
		float bestShelfH = float.MaxValue;
		for (int i = 0; i < c; i++) {
			float freeWidth = size.x - shelfs[i].width;
			float h = shelfs[i].height;
			totalShelfHeight += h;
			
			if (freeWidth < s.x || h < s.y) continue;
			if (h > bestShelfH) continue;
			bestShelf = i;
			bestShelfH = h;
		}

		if (c > 0)
		{
			float h = shelfs[c - 1].height;
			float freeWidth = size.x - shelfs[c - 1].width;
			float freeHeight = size.y - h;
			if (freeWidth >= s.x && freeHeight >= s.y && h < bestShelfH) {
				bestShelf = c - 1;
				bestShelfH = h;
			}  
		}

		float freeShelfHeight = size.y - totalShelfHeight;

		if ((bestShelfH > s.y * 1.5f || bestShelfH < s.y * .75f) && freeShelfHeight > s.y) { // add new shelf
			float2 pos = new(0, totalShelfHeight);
			rect shelf = new(0, totalShelfHeight, s.x, s.y);
			shelfs.Add(shelf);
			return pos + padding;
		}

		if (bestShelf == -1) return new(-1, -1);
		
		if (bestShelf == c - 1) {
			rect shelf = shelfs[bestShelf];
			float2 pos = new(shelf.right, shelf.bottom);
			shelf.width += s.x;
			shelf.height = math.max(shelf.height, s.y);
			shelfs[bestShelf] = shelf;
			return pos + padding;
		}

		{
			rect shelf = shelfs[bestShelf];
			float2 pos = new(shelf.right, shelf.bottom);
			shelf.width += s.x;
			shelfs[bestShelf] = shelf;
			return pos + padding;
		}
	}
}