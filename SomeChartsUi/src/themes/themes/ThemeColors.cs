using SomeChartsUi.themes.colors;

namespace SomeChartsUi.themes.themes; 

public partial class theme {
	// * default colors, using in background, text etc.
	public color default0 = "#12131c"; // default bg
	public color default1 = "#1c1e2b";
	public color default2 = "#232638";
	public color default3 = "#2e314a";
	public color default4 = "#404463";
	public color default5 = "#51567a";
	public color default6 = "#696f96";
	public color default7 = "#7b81ad";
	public color default8 = "#9399c9"; // default H5 text
	public color default9 = "#abb1de"; // default H4 text
	public color default10 = "#c4caf5";// default H3 text
	public color default11 = "#d9ddff";// default H2 text
	
	public static readonly indexedColor default0_ind = new(0);
	public static readonly indexedColor default1_ind = new(1);
	public static readonly indexedColor default2_ind = new(2);
	public static readonly indexedColor default3_ind = new(3);
	public static readonly indexedColor default4_ind = new(4);
	public static readonly indexedColor default5_ind = new(5);
	public static readonly indexedColor default6_ind = new(6);
	public static readonly indexedColor default7_ind = new(7);
	public static readonly indexedColor default8_ind = new(8);
	public static readonly indexedColor default9_ind = new(9);
	public static readonly indexedColor default10_ind = new(10);
	public static readonly indexedColor default11_ind = new(11);
	
	// * other common colors
	public color good = "#9effad";
	public color normal = "#ffe59e";
	public color bad = "#ffa39e";
	
	public static readonly indexedColor good_ind = new(12);
	public static readonly indexedColor normal_ind = new(13);
	public static readonly indexedColor bad_ind = new(14);
	
	// * accent colors
	public color accent0 = "#9effdd"; // default H1,H0 text
	public color accent1 = "#357860"; // default H0 text bg
	public color accent2 = "#13362a";
	
	public static readonly indexedColor accent0_ind = new(15);
	public static readonly indexedColor accent1_ind = new(16);
	public static readonly indexedColor accent2_ind = new(17);
	
	// * reserved indexes (18 - 50)
	
	// * basic colors
	public color red = "#f66";
	public color green = "#6f6";
	public color blue = "#66f";
	public color yellow = "#ff6";
	public color purple = "#f6f";
	public color cyan = "#6ff";
	public color darkRed = "#922";
	public color darkGreen = "#292";
	public color darkBlue = "#229";
	public color darkYellow = "#992";
	public color darkPurple = "#929";
	public color darkCyan = "#299";
	public color white = "#fff";
	public color black = "#000";
	public color gray = "#678";
	public color darkGray = "#222630";
	
	public static readonly indexedColor red_ind = new(51);
	public static readonly indexedColor green_ind = new(52);
	public static readonly indexedColor blue_ind = new(53);
	public static readonly indexedColor yellow_ind = new(54);
	public static readonly indexedColor purple_ind = new(55);
	public static readonly indexedColor cyan_ind = new(56);
	public static readonly indexedColor darkRed_ind = new(57);
	public static readonly indexedColor darkGreen_ind = new(58);
	public static readonly indexedColor darkBlue_ind = new(59);
	public static readonly indexedColor darkYellow_ind = new(60);
	public static readonly indexedColor darkPurple_ind = new(61);
	public static readonly indexedColor darkCyan_ind = new(62);
	public static readonly indexedColor white_ind = new(63);
	public static readonly indexedColor black_ind = new(64);
	public static readonly indexedColor gray_ind = new(65);
	public static readonly indexedColor darkGray_ind = new(66);
	
	// * advanced colors
	public color orange = "#fa6";
	public color lightGreen = "#af6";
	public color magenta = "#f6a";
	public color purple2 = "#a6f";
	public color aquamarine = "#6fa";
	public color lightBlue = "#6af";
	public color darkOrange = "#85a";
	public color darkLightGreen = "#582";
	public color darkMagenta = "#825";
	public color darkPurple2 = "#528";
	public color darkAquamarine = "#258";
	public color darkLightBlue = "#258";
	
	public static readonly indexedColor orange_ind = new(67);
	public static readonly indexedColor lightGreen_ind = new(68);
	public static readonly indexedColor magenta_ind = new(69);
	public static readonly indexedColor purple2_ind = new(70);
	public static readonly indexedColor aquamarine_ind = new(71);
	public static readonly indexedColor lightBlue_ind = new(72);
	public static readonly indexedColor darkOrange_ind = new(73);
	public static readonly indexedColor darkLightGreen_ind = new(74);
	public static readonly indexedColor darkMagenta_ind = new(75);
	public static readonly indexedColor darkPurple2_ind = new(76);
	public static readonly indexedColor darkAquamarine_ind = new(77);
	public static readonly indexedColor darkLightBlue_ind = new(78);

	public color this[int i] => i switch {
		000 => default0,
		001 => default1,
		002 => default2,
		003 => default3,
		004 => default4,
		005 => default5,
		006 => default6,
		007 => default7,
		008 => default8,
		009 => default9,
		010 => default10,
		011 => default11,
		
		012 => good,
		013 => normal,
		014 => bad,
		
		015 => accent0,
		016 => accent1,
		017 => accent2,
		
		051 => red,
		052 => green,
		053 => blue,
		054 => yellow,
		055 => purple,
		056 => cyan,
		057 => darkRed,
		058 => darkGreen,
		059 => darkBlue,
		060 => darkYellow,
		061 => darkPurple,
		062 => darkCyan,
		063 => white,
		064 => black,
		065 => gray,
		066 => darkGray,
		
		067 => orange,
		068 => lightGreen,
		069 => magenta,
		070 => purple2,
		071 => aquamarine,
		072 => lightBlue,
		073 => darkOrange,
		074 => darkLightGreen,
		075 => darkMagenta,
		076 => darkPurple2,
		077 => darkAquamarine,
		078 => darkLightBlue,
		_ => throw new ArgumentOutOfRangeException(nameof(i), i, null)
	};
}