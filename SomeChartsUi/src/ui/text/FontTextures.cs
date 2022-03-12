namespace SomeChartsUi.ui.text;

public abstract class FontTextures {
	public List<FontTextureAtlas> atlases = new();
	public uint resolution;
	public float lineHeight;
	public HashSet<uint> charMap = new();

	//TODO: add ligatures
	public (FontCharData ch, int atlas) GetGlyph(uint character) {
		int atlasCount = atlases.Count;
		for (int i = 0; i < atlasCount; i++) {
			int charCount = atlases[i].characters.Count;
			for (int j = 0; j < charCount; j++)
				if (atlases[i].characters[j].glyph == character)
					return (atlases[i].characters[j], i);
		}

		return Add(character);
	}

	protected abstract (FontCharData ch, int atlas) Add(uint u);

	protected abstract FontTextureAtlas CreateAtlas();


	public uint ToCharacter(char c0, char c1) => ((uint)c1 << 16) + c0;
	public bool ContainsCharacter(uint ch) => charMap.Contains(ch);
}