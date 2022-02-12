using SomeChartsUi.ui.elements;

namespace SomeChartsUi.data;

public abstract class ChartProperty<T> {
	public abstract T Get(RenderableBase owner);
	
	public static implicit operator ChartProperty<T>(T v) => new ChartPropertyValue<T>(v);
	public static implicit operator ChartProperty<T>(Func<RenderableBase, T> v) => new ChartPropertyFunc<T>(v);
}

public class ChartPropertyValue<T> : ChartProperty<T> {
	public T value;
	public ChartPropertyValue(T value) => this.value = value;
	public override T Get(RenderableBase owner) => value;

	public static implicit operator ChartPropertyValue<T>(T v) => new(v);
}

public class ChartPropertyFunc<T> : ChartProperty<T> {
	public Func<RenderableBase, T> value;
	public ChartPropertyFunc(Func<RenderableBase, T> value) => this.value = value;
	public override T Get(RenderableBase owner) => value(owner);
}