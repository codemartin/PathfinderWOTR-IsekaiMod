using System;

namespace IsekaiMod.Utilities;

public static class IsekaiPalette
{
	public const string InkPurpleHex = "#5B2A86";

	public const string InkBlueHex = "#133E71";

	public const string InkGainHex = "#004604";

	public const string InkLossHex = "#7A1010";

	public const string InkMutedHex = "#3F3F3F";

	public const string GlowPurpleHex = "#C9B3FF";

	public const string GlowBlueHex = "#AEC8FF";

	public const string GlowGainHex = "#9BE8A0";

	public const string GlowLossHex = "#FFAFA8";

	public const string GlowMutedHex = "#B9BEC9";

	public const string Purple = "<color=#5B2A86>";

	public const string Blue = "<color=#133E71>";

	public const string Gain = "<color=#004604>";

	public const string Loss = "<color=#7A1010>";

	public const string Muted = "<color=#3F3F3F>";

	public const string GlowPurple = "<color=#C9B3FF>";

	public const string GlowBlue = "<color=#AEC8FF>";

	public const string GlowGain = "<color=#9BE8A0>";

	public const string GlowLoss = "<color=#FFAFA8>";

	public const string GlowMuted = "<color=#B9BEC9>";

	public const string OverlayPanelHex = "#0A0D17";

	public const string OverlayArchiveHex = "#1E202C";

	public const string OverlayOverBrightSceneHex = "#282B33";

	public static readonly string[] ParchmentBackgrounds = new string[4] { "#FFFFFF", "#FFF5C6", "#C3C1B9", "#CBAE6E" };

	public static readonly string[] OverlayBackgrounds = new string[3] { "#0A0D17", "#1E202C", "#282B33" };

	public static readonly string[] InkHexes = new string[5] { "#5B2A86", "#133E71", "#004604", "#7A1010", "#3F3F3F" };

	public static readonly string[] GlowHexes = new string[5] { "#C9B3FF", "#AEC8FF", "#9BE8A0", "#FFAFA8", "#B9BEC9" };

	public const double MinimumContrast = 4.5;

	public static string ToGlow(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return text;
		}
		if (text.IndexOf("<color=#", StringComparison.Ordinal) < 0)
		{
			return text;
		}
		for (int i = 0; i < InkHexes.Length; i++)
		{
			text = text.Replace(InkHexes[i], GlowHexes[i]);
		}
		return text;
	}

	public static double RelativeLuminance(string hex)
	{
		if (hex == null)
		{
			throw new ArgumentNullException("hex");
		}
		if (hex.Length != 7 || hex[0] != '#')
		{
			throw new ArgumentException("Not a #RRGGBB colour: " + hex, "hex");
		}
		double num = Channel(Convert.ToInt32(hex.Substring(1, 2), 16));
		double num2 = Channel(Convert.ToInt32(hex.Substring(3, 2), 16));
		double num3 = Channel(Convert.ToInt32(hex.Substring(5, 2), 16));
		return 0.2126 * num + 0.7152 * num2 + 0.0722 * num3;
	}

	public static double ContrastRatio(string first, string second)
	{
		double val = RelativeLuminance(first);
		double val2 = RelativeLuminance(second);
		double num = Math.Max(val, val2);
		double num2 = Math.Min(val, val2);
		return (num + 0.05) / (num2 + 0.05);
	}

	private static double Channel(int value)
	{
		double num = (double)value / 255.0;
		if (!(num <= 0.04045))
		{
			return Math.Pow((num + 0.055) / 1.055, 2.4);
		}
		return num / 12.92;
	}
}
