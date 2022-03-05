using MathStuff;

namespace SomeChartsUi.themes.themes; 

public partial record theme {
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
	
	public const ushort default0_ind = 0;
	public const ushort default1_ind = 1;
	public const ushort default2_ind = 2;
	public const ushort default3_ind = 3;
	public const ushort default4_ind = 4;
	public const ushort default5_ind = 5;
	public const ushort default6_ind = 6;
	public const ushort default7_ind = 7;
	public const ushort default8_ind = 8;
	public const ushort default9_ind = 9;
	public const ushort default10_ind = 10;
	public const ushort default11_ind = 11;
	
	// * other common colors
	public color good = "#9effad";
	public color normal = "#ffe59e";
	public color bad = "#ffa39e";
	
	public const ushort good_ind = 12;
	public const ushort normal_ind = 13;
	public const ushort bad_ind = 14;
	
	// * accent colors
	public color accent0 = "#9effdd"; // default H1,H0 text
	public color accent1 = "#357860"; // default H0 text bg
	public color accent2 = "#13362a";
	
	public const ushort accent0_ind = 15;
	public const ushort accent1_ind = 16;
	public const ushort accent2_ind = 17;
	
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
	
	public const ushort red_ind = 51;
	public const ushort green_ind = 52;
	public const ushort blue_ind = 53;
	public const ushort yellow_ind = 54;
	public const ushort purple_ind = 55;
	public const ushort cyan_ind = 56;
	public const ushort darkRed_ind = 57;
	public const ushort darkGreen_ind = 58;
	public const ushort darkBlue_ind = 59;
	public const ushort darkYellow_ind = 60;
	public const ushort darkPurple_ind = 61;
	public const ushort darkCyan_ind = 62;
	public const ushort white_ind = 63;
	public const ushort black_ind = 64;
	public const ushort gray_ind = 65;
	public const ushort darkGray_ind = 66;
	
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
	
	public const ushort orange_ind = 67;
	public const ushort lightGreen_ind = 68;
	public const ushort magenta_ind = 69;
	public const ushort purple2_ind = 70;
	public const ushort aquamarine_ind = 71;
	public const ushort lightBlue_ind = 72;
	public const ushort darkOrange_ind = 73;
	public const ushort darkLightGreen_ind = 74;
	public const ushort darkMagenta_ind = 75;
	public const ushort darkPurple2_ind = 76;
	public const ushort darkAquamarine_ind = 77;
	public const ushort darkLightBlue_ind = 78;

	public color this[int i] {
		get => i switch {
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
		set {
			switch (i) {
				case 000: default0 = value; return;
				case 001: default1 = value; return;
				case 002: default2 = value; return;
				case 003: default3 = value; return;
				case 004: default4 = value; return;
				case 005: default5 = value; return;
				case 006: default6 = value; return;
				case 007: default7 = value; return;
				case 008: default8 = value; return;
				case 009: default9 = value; return;
				case 010: default10 = value; return;
				case 011: default11 = value; return;
				case 012: good = value; return;
				case 013: normal = value; return;
				case 014: bad = value; return;
				case 015: accent0 = value; return;
				case 016: accent1 = value; return;
				case 017: accent2 = value; return;
				case 051: red = value; return;
				case 052: green = value; return;
				case 053: blue = value; return;
				case 054: yellow = value; return;
				case 055: purple = value; return;
				case 056: cyan = value; return;
				case 057: darkRed = value; return;
				case 058: darkGreen = value; return;
				case 059: darkBlue = value; return;
				case 060: darkYellow = value; return;
				case 061: darkPurple = value; return;
				case 062: darkCyan = value; return;
				case 063: white = value; return;
				case 064: black = value; return;
				case 065: gray = value; return;
				case 066: darkGray = value; return;
				case 067: orange = value; return;
				case 068: lightGreen = value; return;
				case 069: magenta = value; return;
				case 070: purple2 = value; return;
				case 071: aquamarine = value; return;
				case 072: lightBlue = value; return;
				case 073: darkOrange = value; return;
				case 074: darkLightGreen = value; return;
				case 075: darkMagenta = value; return;
				case 076: darkPurple2 = value; return;
				case 077: darkAquamarine = value; return;
				case 078: darkLightBlue = value; return;
				default:  throw new ArgumentOutOfRangeException(nameof(i), i, null);
			}
		}
	}
}