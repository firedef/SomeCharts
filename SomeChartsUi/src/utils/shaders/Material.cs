namespace SomeChartsUi.utils.shaders;

public class Material {
	public List<MaterialProperty> properties = new();
	public Shader shader;

	public bool depthTest = true;

	public Material(Shader shader) => this.shader = shader;
	public Material(Shader shader, params MaterialProperty[] properties) : this(shader) => this.properties = properties.ToList();
	public Material(Shader shader, params (string name, object val)[] properties) : this(shader) => this.properties = properties.Select(v => new MaterialProperty(v.name, v.val)).ToList();

	public void SetProperty<T>(string name, T v) {
		int index = properties.FindIndex(p => p.name == name);
		if (index == -1) properties.Add(new(name, v!));
		else properties[index].value = v!;
	}
}

public class MaterialProperty {
	public readonly string name;
	public object value;

	public MaterialProperty(string name, object value) {
		this.name = name;
		this.value = value;
	}
}