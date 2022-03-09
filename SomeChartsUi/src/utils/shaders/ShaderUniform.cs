namespace SomeChartsUi.utils.shaders;

public readonly record struct ShaderUniform {
	public readonly int location = -1;
	public readonly string name;
	public readonly int size;
	public readonly int type;

	public ShaderUniform(string name, int location, int type, int size) {
		this.name = name;
		this.location = location;
		this.type = type;
		this.size = size;
	}

	public override string ToString() => $"(loc:{location}, name:'{name}', type:{type}, size:{size})";
}