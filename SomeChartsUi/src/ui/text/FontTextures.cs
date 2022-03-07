namespace SomeChartsUi.ui.text; 

public abstract class FontTextures {
	public List<FontTextureAtlas> atlases = new();

	public (FontCharData ch, int atlas) GetGlyph(string character) {
		int atlasCount = atlases.Count;
		for (int i = 0; i < atlasCount; i++) {
			int charCount = atlases[i].characters.Count;
			for (int j = 0; j < charCount; j++)
				if (atlases[i].characters[j].glyph == character) return (atlases[i].characters[j], i);
		}
		
		return Add(character);
	}

	protected abstract (FontCharData ch, int atlas) Add(string character);

	protected abstract FontTextureAtlas CreateAtlas();
}