namespace SomeChartsUi.themes.colors; 

public struct color {
	public const int sizeOf = 4;
		
		public static readonly color white = new(1f, 1f, 1f);
		public static readonly color black = new(0f, 0f, 0f);
		public static readonly color transparent = new(0f, 0f, 0f, 0f);
		public static readonly color red = new(1f, 0f, 0f);
		public static readonly color green = new(0f, 1f, 0f);
		public static readonly color blue = new(0f, 0f, 1f);

		public static readonly color yellow = new(1f, 1f, 0f);
		public static readonly color purple = new(1f, 0f, 1f);
		public static readonly color cyan = new(0f, 1f, 1f);

		public static readonly color softRed = new(1f, .5f, .5f);
		public static readonly color softGreen = new(.5f, 1f, .5f);
		public static readonly color softBlue = new(.5f, .5f, 1f);
		public static readonly color softYellow = new(1f, 1f, .5f);
		public static readonly color softPurple = new(1f, .5f, 1f);
		public static readonly color softCyan = new(.5f, 1f, 1f);

		public static readonly color steelBlue = "#242A3D";
		public static readonly color steelBlueLight = "#3f4966";
		public static readonly color steelBlueLighter = "#515e87";
		public static readonly color steelBlueLightest = "#5e71ad";
		public static readonly color steelBlueLightest2 = "#667dc4";
		public static readonly color steelBlueLightest3 = "#7995ed";
		public static readonly color steelBlueLightest4 = "#abbfff";
		public static readonly color steelBlueLightest5 = "#bfcfff";

		private const float _oneDiv255 = 1 / 255f;
		public uint raw;

		public color(uint raw) => this.raw = raw;

		public color(byte r, byte g, byte b, byte a = 255) : this((uint)((a << 24) | (r << 16) | (g << 8) | b)) { }

		public color(float r, float g, float b, float a = 1) : this() {
			aF = a;
			rF = r;
			gF = g;
			bF = b;
		}

		public byte a {
			get => (byte)((raw >> 24) & 0xff);
			set => raw = (raw & 0x00_ff_ff_ff) | ((uint)value << 24);
		}

		public byte r {
			get => (byte)((raw >> 16) & 0xff);
			set => raw = (raw & 0xff_00_ff_ff) | ((uint)value << 16);
		}

		public byte g {
			get => (byte)((raw >> 8) & 0xff);
			set => raw = (raw & 0xff_ff_00_ff) | ((uint)value << 8);
		}

		public byte b {
			get => (byte)(raw & 0xff);
			set => raw = (raw & 0xff_ff_ff_00) | value;
		}

		public float aF {
			get => a * _oneDiv255;
			set => a = (byte)(value >= 1 ? 255 : value <= 0 ? 0 : value * 255);
		}

		public float rF {
			get => r * _oneDiv255;
			set => r = (byte)(value >= 1 ? 255 : value <= 0 ? 0 : value * 255);
		}

		public float gF {
			get => g * _oneDiv255;
			set => g = (byte)(value >= 1 ? 255 : value <= 0 ? 0 : value * 255);
		}

		public float bF {
			get => b * _oneDiv255;
			set => b = (byte)(value >= 1 ? 255 : value <= 0 ? 0 : value * 255);
		}

		public float value => Math.Max(r, Math.Max(g, b)) * _oneDiv255;
		public float minValue => Math.Min(r, Math.Min(g, b)) * _oneDiv255;
		public float chroma => value - minValue;
		public float lightness => (value + minValue) * .5f;

		public float hue {
			get {
				float fR = rF;
				float fG = gF;
				float fB = bF;
				float minV = minValue;
				float maxV = value;
				float chromaV = maxV - minV;
				float h = 0.0f;

				if (!(Math.Abs(chromaV) > .001)) return h * 360;
				float num4 = ((maxV - fR) / 6.0f + chromaV * .5f) / chromaV;
				float num5 = ((maxV - fG) / 6.0f + chromaV * .5f) / chromaV;
				float num6 = ((maxV - fB) / 6.0f + chromaV * .5f) / chromaV;
				h = Math.Abs(fR - maxV) >= .001
					? Math.Abs(fG - maxV) >= .001
						? 0.6666667f + num5 - num4
						: 0.33333334f + num4 - num6
					: num6 - num5;
				if (h < 0) ++h;
				if (h > 1) --h;
				return h;
			}
		}

		public float saturation {
			get {
				float minV = minValue;
				float maxV = value;
				float chromaV = maxV - minV;
				float s = 0.0f;
				float l = (maxV + minV) * .5f;
				if (Math.Abs(chromaV) > .001) s = l >= 0.5 ? chromaV / (2f - maxV - minV) : chromaV / (maxV + minV);
				return s;
			}
		}

		public (float h, float s, float l) hsl {
			get {
				float fR = rF;
				float fG = gF;
				float fB = bF;
				float minV = minValue;
				float maxV = value;
				float chromaV = maxV - minV;
				float h = 0.0f;
				float s = 0.0f;
				float l = (maxV + minV) * .5f;
				if (!(Math.Abs(chromaV) > .001)) return (h, s, l);

				s = l >= 0.5 ? chromaV / (2f - maxV - minV) : chromaV / (maxV + minV);
				float num4 = ((maxV - fR) / 6.0f + chromaV * .5f) / chromaV;
				float num5 = ((maxV - fG) / 6.0f + chromaV * .5f) / chromaV;
				float num6 = ((maxV - fB) / 6.0f + chromaV * .5f) / chromaV;
				h = Math.Abs(fR - maxV) >= .001
					? Math.Abs(fG - maxV) >= .001
						? 0.6666667f + num5 - num4
						: 0.33333334f + num4 - num6
					: num6 - num5;
				if (h < 0) ++h;
				if (h > 1) --h;

				return (h, s, l);
			}
		}

		public color WithHue(float v) {
			(float h, float s, float l) hslV = hsl;
			hslV.h = v;
			return FromHsl(hslV);
		}

		public color WithSaturation(float v) {
			(float h, float s, float l) hslV = hsl;
			hslV.s = v;
			return FromHsl(hslV);
		}

		public color WithLightness(float v) {
			(float h, float s, float l) hslV = hsl;
			hslV.l = v;
			return FromHsl(hslV);
		}

		public readonly color WithAlpha(byte v) => new(r, g, b, v);
		public color WithBlue(byte v) => new(r, g, v, a);
		public color WithGreen(byte v) => new(r, v, g, a);
		public color WithRed(byte v) => new(v, r, g, a);

		public color WithInvertedLightness() {
			(float h, float s, float l) hslV = hsl;
			hslV.l = 1 - hslV.l;
			return FromHsl(hslV);
		}

		public static implicit operator (float r, float g, float b, float a)(color v) => (v.rF, v.gF, v.bF, v.aF); 
		public static implicit operator (double r, double g, double b, double a)(color v) => (v.rF, v.gF, v.bF, v.aF); 
		public static implicit operator (double r, double g, double b)(color v) => (v.rF, v.gF, v.bF); 
		public static implicit operator color((float r, float g, float b, float a) v) => new(v.r, v.g, v.b, v.a); 
		public static implicit operator color((double r, double g, double b, double a) v) => new((float)v.r, (float)v.g, (float)v.b, (float)v.a); 

		// public static implicit operator SKColor(color v) => new(v.raw);
		// public static implicit operator color(SKColor v) => new(v.Red, v.Green, v.Blue, v.Alpha);

		// public static implicit operator Color(color v) => new(v.a, v.r, v.g, v.b);
		// public static implicit operator color(Color v) => new(v.R, v.G, v.B, v.A);

		// public static implicit operator SolidColorBrush(color v) => new(v);
		// public static implicit operator color(SolidColorBrush v) => v.Color;

		public static implicit operator color(string v) {
			if (string.IsNullOrEmpty(v)) return black;
			if (v[0] == '#') v = v[1..v.Length];

			return v.Length switch {
				3 => new(Parse1_(v[0]),		   // r
				         Parse1_(v[1]),		   // g
				         Parse1_(v[2])),	   // b
				4 => new(Parse1_(v[0]),		   // r
				         Parse1_(v[1]),		   // g
				         Parse1_(v[2]),		   // b
				         Parse1_(v[3])),       // a
				6 => new(Parse2_(v[0], v[1]),  // r
				         Parse2_(v[2], v[3]),  // g
				         Parse2_(v[4], v[5])), // b
				8 => new(Parse2_(v[0], v[1]),  // r
				         Parse2_(v[2], v[3]),  // g
				         Parse2_(v[4], v[5]),  // b
				         Parse2_(v[6], v[7])), // a
				_ => black
			};

			float Parse1_(char c) => ParseChar_(c) / 15f;
			float Parse2_(char a, char b) => (ParseChar_(a) * 16 + ParseChar_(b)) / 255f;
			
			float ParseChar_(char c) {
				return c switch {
					'0' => 0,
					'1' => 1,
					'2' => 2,
					'3' => 3,
					'4' => 4,
					'5' => 5,
					'6' => 6,
					'7' => 7,
					'8' => 8,
					'9' => 9,
					'a' or 'A' => 10,
					'b' or 'B' => 11,
					'c' or 'C' => 12,
					'd' or 'D' => 13,
					'e' or 'E' => 14,
					'f' or 'F' => 15,
					_ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
				};
			}
		}

		public static color FromHsl((float h, float s, float l) v, float a = 1f) {
			float r = v.l;
			float g = v.l;
			float b = v.l;
			if (!(Math.Abs(v.s) > 0.001)) return new(r, g, b, a);
			float v2 = v.l >= 0.5 ? v.l + v.s - v.s * v.l : v.l * (1f + v.s);
			float v1 = 2f * v.l - v2;
			r = HueToRgb(v1, v2, v.h + 0.33333334f);
			g = HueToRgb(v1, v2, v.h);
			b = HueToRgb(v1, v2, v.h - 0.33333334f);
			return new(r, g, b, a);
		}

		private static float HueToRgb(float v1, float v2, float vH) {
			if (vH < 0) ++vH;
			if (vH > 1) --vH;
			if (6.0 * vH < 1) return v1 + (v2 - v1) * 6 * vH;
			if (2.0 * vH < 1) return v2;
			return 3.0 * vH < 2.0 ? v1 + (v2 - v1) * (0.6666666865348816f - vH) * 6 : v1;
		}

		public static color Lerp(color a, color b, float t) => new(
			a.rF * (1 - t) + b.rF * t, 
			a.gF * (1 - t) + b.gF * t, 
			a.bF * (1 - t) + b.bF * t, 
			a.aF * (1 - t) + b.aF * t);

		public override string ToString() => $"#{raw:x8}";
}