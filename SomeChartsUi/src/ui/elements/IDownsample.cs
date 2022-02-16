namespace SomeChartsUi.ui.elements; 

public interface IDownsample {
	/// <summary>more value = less elements</summary>
	public float downsampleMultiplier { get; set; }

	/// <summary>element scale using in downsample</summary>
	public float elementScale { get; set; }
}