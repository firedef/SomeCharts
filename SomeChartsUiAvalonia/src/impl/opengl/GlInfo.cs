using System;
using Avalonia.OpenGL;

namespace SomeChartsUiAvalonia.impl.opengl;

public static class GlInfo {
	public static GlExtrasInterface? glExt;
	public static GlInterface? gl;
	public static GlVersion? version;

	public static void CheckError(string part) {
		int err;
		while ((err = gl!.GetError()) != GlConsts.GL_NO_ERROR)
			Console.WriteLine(part + ": " + err);
	}
}