using Avalonia.OpenGL;

namespace SomeChartsUiAvalonia.controls.gl;

public class GlExtrasInterface : GlInterfaceBase<GlInterface.GlContextInfo>
{
	public GlExtrasInterface(GlInterface gl) : base(gl.GetProcAddress, gl.ContextInfo) { }

    [GlEntryPoint("glDeleteVertexArrays")]
    public GlDeleteVertexArrays DeleteVertexArrays { get; } = null!;
    public unsafe delegate void GlDeleteVertexArrays(int count, int[] buffers);

    [GlEntryPoint("glBindVertexArray")]
    public GlBindVertexArray BindVertexArray { get; } = null!;
    public unsafe delegate void GlBindVertexArray(int array);

    [GlEntryPoint("glGenVertexArrays")]
    public GlGenVertexArrays GenVertexArrays { get; } = null!;
    public unsafe delegate void GlGenVertexArrays(int count, int[] arrays);

    [GlEntryPoint("glBufferSubData")]
    public GlBufferSubData BufferSubData { get; } = null!;
    public unsafe delegate void GlBufferSubData(int buffer, int offset, int size, void* data);

    [GlEntryPoint("glCullFace")]
    public GlCullFace CullFace { get; } = null!;
    public unsafe delegate void GlCullFace(int face);

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

    [GlEntryPoint("glPolygonMode")]
    public GlPolygonMode PolygonMode { get; } = null!;
    public unsafe delegate void GlPolygonMode(int face, int mode);

}