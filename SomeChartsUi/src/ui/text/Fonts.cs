namespace SomeChartsUi.ui.text; 

public static class Fonts {
	public static List<InstalledFontData> availableFonts;
	public static HashSet<string> loadedPaths = new();

	static Fonts() {
		FetchAvailableFonts();
	}

	private static void FetchAvailableFonts() {
		availableFonts = new();

		List<string> paths = new();
		paths.Add(Environment.GetFolderPath(Environment.SpecialFolder.Fonts));

		// Linux additional paths for fonts
		// '~/.fonts' already added using environment special folder
		if (Environment.OSVersion.Platform == PlatformID.Unix) {
			paths.Add("/usr/share/fonts");
			paths.Add("/usr/local/share/fonts");
		}

		foreach (string path in paths) 
			FetchAvailableFontsFromPath(path);
	}

	public static void FetchAvailableFontsFromPath(string path) {
		if (!Directory.Exists(path)) return;
		if (loadedPaths.Contains(path)) return;
		
		string[] ttfPaths = Directory.GetFiles(path, "*.ttf", SearchOption.AllDirectories);
		string[] otfPaths = Directory.GetFiles(path, "*.otf", SearchOption.AllDirectories);

		AddFonts(ttfPaths);
		AddFonts(otfPaths);

		loadedPaths.Add(path);
	}
	
	public static void AddFonts(string[] paths) {
		foreach (string path in paths) AddFont(path);
	}

	public static void AddFont(string path) {
		string name = Path.GetFileName(path)[..^4];
		AddFont(name, path);
	}

	public static void AddFont(string name, string path) {
		if (availableFonts.FindIndex(v => string.Equals(v.path, path, StringComparison.CurrentCultureIgnoreCase)) != -1) return;
		availableFonts.Add(new(name, path));
	}

	public static string? TryFindExact(string name) => availableFonts.FirstOrDefault(v => string.Equals(v.name, name, StringComparison.CurrentCultureIgnoreCase))?.path;

	public static string? TryFind(string name) {
		string? result = TryFindExact(name);
		if (result != null || name.Contains('-')) return result;

		result = TryFindExact($"{name}-regular");
		if (result != null) return result;

		return availableFonts.FirstOrDefault(v => string.Equals(v.name.Split('-', 2)[0], name, StringComparison.CurrentCultureIgnoreCase))?.path;
	}
	
	public static FontFamily GetFamily(string name) {
		name = name.Split('-', 2)[0];
		FontFamily family = new();
		
		IEnumerable<InstalledFontData> fonts = availableFonts.Where(v => string.Equals(v.name.Split('-', 2)[0], name, StringComparison.CurrentCultureIgnoreCase));
		foreach ((string fName, string fPath) in fonts) {
			FontFamilyItem item = new(fName, fPath);
			family.fonts.Add(item);
		}

		return family;
	}
}

public record InstalledFontData(string name, string path) {
	public string name = name;
	public string path = path;
}