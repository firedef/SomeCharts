namespace SomeChartsUi.ui;

/// <summary>
/// all values is copied from avalonia Key enum
/// </summary>
public enum keycode {
	/// <summary>No key pressed.</summary>
	none = 0,
	/// <summary>The Cancel key.</summary>
	cancel = 1,
	/// <summary>The Back key.</summary>
	back = 2,
	/// <summary>The Tab key.</summary>
	tab = 3,
	/// <summary>The Linefeed key.</summary>
	lineFeed = 4,
	/// <summary>The Clear key.</summary>
	clear = 5,
	/// <summary>The Enter key.</summary>
	enter = 6,
	/// <summary>The Return key.</summary>
	@return = 6,
	/// <summary>The Pause key.</summary>
	pause = 7,
	/// <summary>The Caps Lock key.</summary>
	capital = 8,
	/// <summary>The Caps Lock key.</summary>
	capsLock = 8,
	/// <summary>The IME Hangul mode key.</summary>
	hangulMode = 9,
	/// <summary>The IME Kana mode key.</summary>
	kanaMode = 9,
	/// <summary>The IME Junja mode key.</summary>
	junjaMode = 10,// 0x0000000A
	/// <summary>The IME Final mode key.</summary>
	finalMode = 11,// 0x0000000B
	/// <summary>The IME Hanja mode key.</summary>
	hanjaMode = 12,// 0x0000000C
	/// <summary>The IME Kanji mode key.</summary>
	kanjiMode = 12,// 0x0000000C
	/// <summary>The Escape key.</summary>
	escape = 13,// 0x0000000D
	/// <summary>The IME Convert key.</summary>
	imeConvert = 14,// 0x0000000E
	/// <summary>The IME NonConvert key.</summary>
	imeNonConvert = 15,// 0x0000000F
	/// <summary>The IME Accept key.</summary>
	imeAccept = 16,// 0x00000010
	/// <summary>The IME Mode change key.</summary>
	imeModeChange = 17,// 0x00000011
	/// <summary>The space bar.</summary>
	space = 18,// 0x00000012
	/// <summary>The Page Up key.</summary>
	pageUp = 19,// 0x00000013
	/// <summary>The Page Up key.</summary>
	prior = 19,// 0x00000013
	/// <summary>The Page Down key.</summary>
	next = 20,// 0x00000014
	/// <summary>The Page Down key.</summary>
	pageDown = 20,// 0x00000014
	/// <summary>The End key.</summary>
	end = 21,// 0x00000015
	/// <summary>The Home key.</summary>
	home = 22,// 0x00000016
	/// <summary>The Left arrow key.</summary>
	left = 23,// 0x00000017
	/// <summary>The Up arrow key.</summary>
	up = 24,// 0x00000018
	/// <summary>The Right arrow key.</summary>
	right = 25,// 0x00000019
	/// <summary>The Down arrow key.</summary>
	down = 26,// 0x0000001A
	/// <summary>The Select key.</summary>
	@select = 27,// 0x0000001B
	/// <summary>The Print key.</summary>
	print = 28,// 0x0000001C
	/// <summary>The Execute key.</summary>
	execute = 29,// 0x0000001D
	/// <summary>The Print Screen key.</summary>
	printScreen = 30,// 0x0000001E
	/// <summary>The Print Screen key.</summary>
	snapshot = 30,// 0x0000001E
	/// <summary>The Insert key.</summary>
	insert = 31,// 0x0000001F
	/// <summary>The Delete key.</summary>
	delete = 32,// 0x00000020
	/// <summary>The Help key.</summary>
	help = 33,// 0x00000021
	/// <summary>The 0 key.</summary>
	d0 = 34,// 0x00000022
	/// <summary>The 1 key.</summary>
	d1 = 35,// 0x00000023
	/// <summary>The 2 key.</summary>
	d2 = 36,// 0x00000024
	/// <summary>The 3 key.</summary>
	d3 = 37,// 0x00000025
	/// <summary>The 4 key.</summary>
	d4 = 38,// 0x00000026
	/// <summary>The 5 key.</summary>
	d5 = 39,// 0x00000027
	/// <summary>The 6 key.</summary>
	d6 = 40,// 0x00000028
	/// <summary>The 7 key.</summary>
	d7 = 41,// 0x00000029
	/// <summary>The 8 key.</summary>
	d8 = 42,// 0x0000002A
	/// <summary>The 9 key.</summary>
	d9 = 43,// 0x0000002B
	/// <summary>The A key.</summary>
	a = 44,// 0x0000002C
	/// <summary>The B key.</summary>
	b = 45,// 0x0000002D
	/// <summary>The C key.</summary>
	c = 46,// 0x0000002E
	/// <summary>The D key.</summary>
	d = 47,// 0x0000002F
	/// <summary>The E key.</summary>
	e = 48,// 0x00000030
	/// <summary>The F key.</summary>
	f = 49,// 0x00000031
	/// <summary>The G key.</summary>
	g = 50,// 0x00000032
	/// <summary>The H key.</summary>
	h = 51,// 0x00000033
	/// <summary>The I key.</summary>
	I = 52,// 0x00000034
	/// <summary>The J key.</summary>
	j = 53,// 0x00000035
	/// <summary>The K key.</summary>
	k = 54,// 0x00000036
	/// <summary>The L key.</summary>
	l = 55,// 0x00000037
	/// <summary>The M key.</summary>
	m = 56,// 0x00000038
	/// <summary>The N key.</summary>
	n = 57,// 0x00000039
	/// <summary>The O key.</summary>
	o = 58,// 0x0000003A
	/// <summary>The P key.</summary>
	p = 59,// 0x0000003B
	/// <summary>The Q key.</summary>
	q = 60,// 0x0000003C
	/// <summary>The R key.</summary>
	r = 61,// 0x0000003D
	/// <summary>The S key.</summary>
	s = 62,// 0x0000003E
	/// <summary>The T key.</summary>
	T = 63,// 0x0000003F
	/// <summary>The U key.</summary>
	u = 64,// 0x00000040
	/// <summary>The V key.</summary>
	v = 65,// 0x00000041
	/// <summary>The W key.</summary>
	w = 66,// 0x00000042
	/// <summary>The X key.</summary>
	x = 67,// 0x00000043
	/// <summary>The Y key.</summary>
	y = 68,// 0x00000044
	/// <summary>The Z key.</summary>
	z = 69,// 0x00000045
	/// <summary>The left Windows key.</summary>
	lWin = 70,// 0x00000046
	/// <summary>The right Windows key.</summary>
	rWin = 71,// 0x00000047
	/// <summary>The Application key.</summary>
	apps = 72,// 0x00000048
	/// <summary>The Sleep key.</summary>
	sleep = 73,// 0x00000049
	/// <summary>The 0 key on the numeric keypad.</summary>
	numPad0 = 74,// 0x0000004A
	/// <summary>The 1 key on the numeric keypad.</summary>
	numPad1 = 75,// 0x0000004B
	/// <summary>The 2 key on the numeric keypad.</summary>
	numPad2 = 76,// 0x0000004C
	/// <summary>The 3 key on the numeric keypad.</summary>
	numPad3 = 77,// 0x0000004D
	/// <summary>The 4 key on the numeric keypad.</summary>
	numPad4 = 78,// 0x0000004E
	/// <summary>The 5 key on the numeric keypad.</summary>
	numPad5 = 79,// 0x0000004F
	/// <summary>The 6 key on the numeric keypad.</summary>
	numPad6 = 80,// 0x00000050
	/// <summary>The 7 key on the numeric keypad.</summary>
	numPad7 = 81,// 0x00000051
	/// <summary>The 8 key on the numeric keypad.</summary>
	numPad8 = 82,// 0x00000052
	/// <summary>The 9 key on the numeric keypad.</summary>
	numPad9 = 83,// 0x00000053
	/// <summary>The Multiply key.</summary>
	multiply = 84,// 0x00000054
	/// <summary>The Add key.</summary>
	add = 85,// 0x00000055
	/// <summary>The Separator key.</summary>
	separator = 86,// 0x00000056
	/// <summary>The Subtract key.</summary>
	subtract = 87,// 0x00000057
	/// <summary>The Decimal key.</summary>
	@decimal = 88,// 0x00000058
	/// <summary>The Divide key.</summary>
	divide = 89,// 0x00000059
	/// <summary>The F1 key.</summary>
	f1 = 90,// 0x0000005A
	/// <summary>The F2 key.</summary>
	f2 = 91,// 0x0000005B
	/// <summary>The F3 key.</summary>
	f3 = 92,// 0x0000005C
	/// <summary>The F4 key.</summary>
	f4 = 93,// 0x0000005D
	/// <summary>The F5 key.</summary>
	f5 = 94,// 0x0000005E
	/// <summary>The F6 key.</summary>
	f6 = 95,// 0x0000005F
	/// <summary>The F7 key.</summary>
	f7 = 96,// 0x00000060
	/// <summary>The F8 key.</summary>
	f8 = 97,// 0x00000061
	/// <summary>The F9 key.</summary>
	f9 = 98,// 0x00000062
	/// <summary>The F10 key.</summary>
	f10 = 99,// 0x00000063
	/// <summary>The F11 key.</summary>
	f11 = 100,// 0x00000064
	/// <summary>The F12 key.</summary>
	f12 = 101,// 0x00000065
	/// <summary>The F13 key.</summary>
	f13 = 102,// 0x00000066
	/// <summary>The F14 key.</summary>
	f14 = 103,// 0x00000067
	/// <summary>The F15 key.</summary>
	f15 = 104,// 0x00000068
	/// <summary>The F16 key.</summary>
	f16 = 105,// 0x00000069
	/// <summary>The F17 key.</summary>
	f17 = 106,// 0x0000006A
	/// <summary>The F18 key.</summary>
	f18 = 107,// 0x0000006B
	/// <summary>The F19 key.</summary>
	f19 = 108,// 0x0000006C
	/// <summary>The F20 key.</summary>
	f20 = 109,// 0x0000006D
	/// <summary>The F21 key.</summary>
	f21 = 110,// 0x0000006E
	/// <summary>The F22 key.</summary>
	f22 = 111,// 0x0000006F
	/// <summary>The F23 key.</summary>
	f23 = 112,// 0x00000070
	/// <summary>The F24 key.</summary>
	f24 = 113,// 0x00000071
	/// <summary>The Numlock key.</summary>
	numLock = 114,// 0x00000072
	/// <summary>The Scroll key.</summary>
	scroll = 115,// 0x00000073
	/// <summary>The left Shift key.</summary>
	leftShift = 116,// 0x00000074
	/// <summary>The right Shift key.</summary>
	rightShift = 117,// 0x00000075
	/// <summary>The left Ctrl key.</summary>
	leftCtrl = 118,// 0x00000076
	/// <summary>The right Ctrl key.</summary>
	rightCtrl = 119,// 0x00000077
	/// <summary>The left Alt key.</summary>
	leftAlt = 120,// 0x00000078
	/// <summary>The right Alt key.</summary>
	rightAlt = 121,// 0x00000079
	/// <summary>The browser Back key.</summary>
	browserBack = 122,// 0x0000007A
	/// <summary>The browser Forward key.</summary>
	browserForward = 123,// 0x0000007B
	/// <summary>The browser Refresh key.</summary>
	browserRefresh = 124,// 0x0000007C
	/// <summary>The browser Stop key.</summary>
	browserStop = 125,// 0x0000007D
	/// <summary>The browser Search key.</summary>
	browserSearch = 126,// 0x0000007E
	/// <summary>The browser Favorites key.</summary>
	browserFavorites = 127,// 0x0000007F
	/// <summary>The browser Home key.</summary>
	browserHome = 128,// 0x00000080
	/// <summary>The Volume Mute key.</summary>
	volumeMute = 129,// 0x00000081
	/// <summary>The Volume Down key.</summary>
	volumeDown = 130,// 0x00000082
	/// <summary>The Volume Up key.</summary>
	volumeUp = 131,// 0x00000083
	/// <summary>The media Next Track key.</summary>
	mediaNextTrack = 132,// 0x00000084
	/// <summary>The media Previous Track key.</summary>
	mediaPreviousTrack = 133,// 0x00000085
	/// <summary>The media Stop key.</summary>
	mediaStop = 134,// 0x00000086
	/// <summary>The media Play/Pause key.</summary>
	mediaPlayPause = 135,// 0x00000087
	/// <summary>The Launch Mail key.</summary>
	launchMail = 136,// 0x00000088
	/// <summary>The Select Media key.</summary>
	selectMedia = 137,// 0x00000089
	/// <summary>The Launch Application 1 key.</summary>
	launchApplication1 = 138,// 0x0000008A
	/// <summary>The Launch Application 2 key.</summary>
	launchApplication2 = 139,// 0x0000008B
	/// <summary>The OEM 1 key.</summary>
	oem1 = 140,// 0x0000008C
	/// <summary>The OEM Semicolon key.</summary>
	oemSemicolon = 140,// 0x0000008C
	/// <summary>The OEM Plus key.</summary>
	oemPlus = 141,// 0x0000008D
	/// <summary>The OEM Comma key.</summary>
	oemComma = 142,// 0x0000008E
	/// <summary>The OEM Minus key.</summary>
	oemMinus = 143,// 0x0000008F
	/// <summary>The OEM Period key.</summary>
	oemPeriod = 144,// 0x00000090
	/// <summary>The OEM 2 key.</summary>
	oem2 = 145,// 0x00000091
	/// <summary>The OEM Question Mark key.</summary>
	oemQuestion = 145,// 0x00000091
	/// <summary>The OEM 3 key.</summary>
	oem3 = 146,// 0x00000092
	/// <summary>The OEM Tilde key.</summary>
	oemTilde = 146,// 0x00000092
	/// <summary>The ABNT_C1 (Brazilian) key.</summary>
	abntC1 = 147,// 0x00000093
	/// <summary>The ABNT_C2 (Brazilian) key.</summary>
	abntC2 = 148,// 0x00000094
	/// <summary>The OEM 4 key.</summary>
	oem4 = 149,// 0x00000095
	/// <summary>The OEM Open Brackets key.</summary>
	oemOpenBrackets = 149,// 0x00000095
	/// <summary>The OEM 5 key.</summary>
	oem5 = 150,// 0x00000096
	/// <summary>The OEM Pipe key.</summary>
	oemPipe = 150,// 0x00000096
	/// <summary>The OEM 6 key.</summary>
	oem6 = 151,// 0x00000097
	/// <summary>The OEM Close Brackets key.</summary>
	oemCloseBrackets = 151,// 0x00000097
	/// <summary>The OEM 7 key.</summary>
	oem7 = 152,// 0x00000098
	/// <summary>The OEM Quotes key.</summary>
	oemQuotes = 152,// 0x00000098
	/// <summary>The OEM 8 key.</summary>
	oem8 = 153,// 0x00000099
	/// <summary>The OEM 3 key.</summary>
	oem102 = 154,// 0x0000009A
	/// <summary>The OEM Backslash key.</summary>
	oemBackslash = 154,// 0x0000009A
	/// <summary>
	///     A special key masking the real key being processed by an IME.
	/// </summary>
	imeProcessed = 155,// 0x0000009B
	/// <summary>
	///     A special key masking the real key being processed as a system key.
	/// </summary>
	system = 156,// 0x0000009C
	/// <summary>The DBE_ALPHANUMERIC key.</summary>
	dbeAlphanumeric = 157,// 0x0000009D
	/// <summary>The OEM ATTN key.</summary>
	oemAttn = 157,// 0x0000009D
	/// <summary>The DBE_KATAKANA key.</summary>
	dbeKatakana = 158,// 0x0000009E
	/// <summary>The OEM Finish key.</summary>
	oemFinish = 158,// 0x0000009E
	/// <summary>The DBE_HIRAGANA key.</summary>
	dbeHiragana = 159,// 0x0000009F
	/// <summary>The OEM Copy key.</summary>
	oemCopy = 159,// 0x0000009F
	/// <summary>The DBE_SBCSCHAR key.</summary>
	dbeSbcsChar = 160,// 0x000000A0
	/// <summary>The OEM Auto key.</summary>
	oemAuto = 160,// 0x000000A0
	/// <summary>The DBE_DBCSCHAR key.</summary>
	dbeDbcsChar = 161,// 0x000000A1
	/// <summary>The OEM ENLW key.</summary>
	oemEnlw = 161,// 0x000000A1
	/// <summary>The DBE_ROMAN key.</summary>
	dbeRoman = 162,// 0x000000A2
	/// <summary>The OEM BackTab key.</summary>
	oemBackTab = 162,// 0x000000A2
	/// <summary>The ATTN key.</summary>
	attn = 163,// 0x000000A3
	/// <summary>The DBE_NOROMAN key.</summary>
	dbeNoRoman = 163,// 0x000000A3
	/// <summary>The CRSEL key.</summary>
	crSel = 164,// 0x000000A4
	/// <summary>The DBE_ENTERWORDREGISTERMODE key.</summary>
	dbeEnterWordRegisterMode = 164,// 0x000000A4
	/// <summary>The DBE_ENTERIMECONFIGMODE key.</summary>
	dbeEnterImeConfigureMode = 165,// 0x000000A5
	/// <summary>The EXSEL key.</summary>
	exSel = 165,// 0x000000A5
	/// <summary>The DBE_FLUSHSTRING key.</summary>
	dbeFlushString = 166,// 0x000000A6
	/// <summary>The ERASE EOF Key.</summary>
	eraseEof = 166,// 0x000000A6
	/// <summary>The DBE_CODEINPUT key.</summary>
	dbeCodeInput = 167,// 0x000000A7
	/// <summary>The Play key.</summary>
	play = 167,// 0x000000A7
	/// <summary>The DBE_NOCODEINPUT key.</summary>
	dbeNoCodeInput = 168,// 0x000000A8
	/// <summary>The Zoom key.</summary>
	zoom = 168,// 0x000000A8
	/// <summary>The DBE_DETERMINESTRING key.</summary>
	dbeDetermineString = 169,// 0x000000A9
	/// <summary>Reserved for future use.</summary>
	noName = 169,// 0x000000A9
	/// <summary>The DBE_ENTERDLGCONVERSIONMODE key.</summary>
	dbeEnterDialogConversionMode = 170,// 0x000000AA
	/// <summary>The PA1 key.</summary>
	pa1 = 170,// 0x000000AA
	/// <summary>The OEM Clear key.</summary>
	oemClear = 171,// 0x000000AB
	/// <summary>
	///     The key is used with another key to create a single combined character.
	/// </summary>
	deadCharProcessed = 172,// 0x000000AC
	/// <summary>OSX Platform-specific Fn+Left key</summary>
	fnLeftArrow = 10001,// 0x00002711
	/// <summary>OSX Platform-specific Fn+Right key</summary>
	fnRightArrow = 10002,// 0x00002712
	/// <summary>OSX Platform-specific Fn+Up key</summary>
	fnUpArrow = 10003,// 0x00002713
	/// <summary>OSX Platform-specific Fn+Down key</summary>
	fnDownArrow = 10004// 0x00002714
}