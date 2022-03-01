using SomeChartsUi.ui.elements;
using SomeChartsUi.utils.vectors;

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

public static class ChartPropertyExtensions {
	public static float2 Position(this ChartProperty<RenderableTransform> v, RenderableBase o) => v.Get(o).position;
	public static float3 Rotation(this ChartProperty<RenderableTransform> v, RenderableBase o) => v.Get(o).rotation;
	public static float2 Scale(this ChartProperty<RenderableTransform> v, RenderableBase o) => v.Get(o).scale;

	public static bool TrySetPosition(this ChartProperty<RenderableTransform> v, float2 p) {
		if (v is not ChartPropertyValue<RenderableTransform> a) return false;
		a.value.position = p;
		return true;
	}
	
	public static bool TrySetRotation(this ChartProperty<RenderableTransform> v, float3 r) {
		if (v is not ChartPropertyValue<RenderableTransform> a) return false;
		a.value.rotation = r;
		return true;
	}
	
	public static bool TrySetScale(this ChartProperty<RenderableTransform> v, float2 s) {
		if (v is not ChartPropertyValue<RenderableTransform> a) return false;
		a.value.scale = s;
		return true;
	}
}