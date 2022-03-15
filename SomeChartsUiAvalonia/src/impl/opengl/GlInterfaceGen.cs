using Avalonia.OpenGL;

namespace SomeChartsUiAvalonia.impl.opengl;

public class GlExtrasInterface : GlInterfaceBase<GlInterface.GlContextInfo>
{
	public GlExtrasInterface(GlInterface gl) : base(gl.GetProcAddress, gl.ContextInfo) { }

    [GlEntryPoint("glGenVertexArrays")]
    public GlGenVertexArrays GenVertexArrays { get; } = null!;
    public unsafe delegate void GlGenVertexArrays(int count, int[] arrays);

    [GlEntryPoint("glGenTextures")]
    public GlGenTextures GenTextures { get; } = null!;
    public unsafe delegate void GlGenTextures(int count, int* textures);

    [GlEntryPoint("glDeleteVertexArrays")]
    public GlDeleteVertexArrays DeleteVertexArrays { get; } = null!;
    public unsafe delegate void GlDeleteVertexArrays(int count, int[] buffers);

    [GlEntryPoint("glDeleteTextures")]
    public GlDeleteTextures DeleteTextures { get; } = null!;
    public unsafe delegate void GlDeleteTextures(int count, int* textures);

    [GlEntryPoint("glBufferSubData")]
    public GlBufferSubData BufferSubData { get; } = null!;
    public unsafe delegate void GlBufferSubData(int buffer, int offset, int size, void* data);

    [GlEntryPoint("glTexSubImage2D")]
    public GlTexSubImage2D TexSubImage2D { get; } = null!;
    public unsafe delegate void GlTexSubImage2D(int target, int level, int xOff, int yOff, int w, int h, int format, int type, void* pixels);

    [GlEntryPoint("glBindVertexArray")]
    public GlBindVertexArray BindVertexArray { get; } = null!;
    public unsafe delegate void GlBindVertexArray(int array);

    [GlEntryPoint("glUniform1i")]
    public GlUniform1i Uniform1i { get; } = null!;
    public unsafe delegate void GlUniform1i(int location, int v);

    [GlEntryPoint("glUniform2f")]
    public GlUniform2f Uniform2f { get; } = null!;
    public unsafe delegate void GlUniform2f(int location, float x, float y);

    [GlEntryPoint("glUniform3f")]
    public GlUniform3f Uniform3f { get; } = null!;
    public unsafe delegate void GlUniform3f(int location, float x, float y, float z);

    [GlEntryPoint("glUniform4f")]
    public GlUniform4f Uniform4f { get; } = null!;
    public unsafe delegate void GlUniform4f(int location, float x, float y, float z, float w);

    [GlEntryPoint("glGetActiveUniform")]
    public GlGetActiveUniform GetActiveUniform { get; } = null!;
    public unsafe delegate void GlGetActiveUniform(int program, int index, int bufferSize, int* length, int* size, int* type, sbyte* name);

    [GlEntryPoint("glCullFace")]
    public GlCullFace CullFace { get; } = null!;
    public unsafe delegate void GlCullFace(int face);

    [GlEntryPoint("glPolygonMode")]
    public GlPolygonMode PolygonMode { get; } = null!;
    public unsafe delegate void GlPolygonMode(int face, int mode);

    [GlEntryPoint("glBlendFunc")]
    public GlBlendFunc BlendFunc { get; } = null!;
    public unsafe delegate void GlBlendFunc(int sFactor, int dFactor);

    [GlEntryPoint("glBlendEquation")]
    public GlBlendEquation BlendEquation { get; } = null!;
    public unsafe delegate void GlBlendEquation(int mode);

    [GlEntryPoint("glPixelStorei")]
    public GlPixelStorei PixelStorei { get; } = null!;
    public unsafe delegate void GlPixelStorei(int name, int param);

    [GlEntryPoint("glGenerateMipmap")]
    public GlGenerateMipmap GenerateMipmap { get; } = null!;
    public unsafe delegate void GlGenerateMipmap(int target);

    [GlEntryPoint("glDisable")]
    public GlDisable Disable { get; } = null!;
    public unsafe delegate void GlDisable(int cap);

}