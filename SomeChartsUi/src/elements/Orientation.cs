namespace SomeChartsUi.elements; 

[Flags]
public enum Orientation : byte {
	vertical = 0b01,
	reversed = 0b10,
	
	leftToRight = 0,
	rightToLeft = reversed,
	bottomToTop = vertical,
	topToBottom = vertical | reversed,
	
	horizontal = 0,
}