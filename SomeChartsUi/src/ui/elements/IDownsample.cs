namespace SomeChartsUi.ui.elements;

/// <summary>level of detail using with <see cref="RenderableBase"/> </summary>
public interface IDownsample {
	/// <summary>more value = less elements</summary>
	public float downsampleMultiplier { get; set; }

	/// <summary>element scale using in downsample</summary>
	public float elementScale { get; set; }
}