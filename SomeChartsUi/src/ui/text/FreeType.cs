using FreeTypeSharp;
using FreeTypeSharp.Native;

namespace SomeChartsUi.ui.text;

public static class FreeType {
	public static FreeTypeLibrary ftLib = new();


	public static void CheckError(this FT_Error err) {
		if (err != FT_Error.FT_Err_Ok)
			throw new FreeTypeException(err);
	}
}