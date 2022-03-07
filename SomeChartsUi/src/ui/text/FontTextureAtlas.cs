using MathStuff.vectors;
using SomeChartsUi.utils.collections;
using SomeChartsUi.utils.shaders;

namespace SomeChartsUi.ui.text; 

public abstract class FontTextureAtlas {
	public Texture texture;
	public List<FontCharData> characters = new();
	protected RectPack packer;

	protected FontTextureAtlas(int size = 1024) => packer = new(size, 0);

	public unsafe (int i, int x, int y) Pack(int width, int height, string character) {
		float2 size = texture.size;
		float2 pos = packer.Pack(new(width, height));
		if (pos == new float2(-1, -1)) return (-1, 0, 0);
		
		AddChar((int) pos.x, (int) pos.y, width, height, character);

		return (characters.Count - 1, (int) pos.x, (int) pos.y);
	}

	public abstract Texture CreateTexture(int width, int height);

	protected abstract void AddChar(int x, int y, int w, int h, string ch);

	public abstract unsafe void WriteToTexture(void* img, int x, int y, int width, int height, int level);
}