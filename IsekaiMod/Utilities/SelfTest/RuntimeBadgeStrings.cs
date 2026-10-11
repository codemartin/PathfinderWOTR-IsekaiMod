using System;
using System.Collections.Generic;

namespace IsekaiMod.Utilities.SelfTest;

public static class RuntimeBadgeStrings
{
	public static readonly IReadOnlyCollection<string> Owners = new HashSet<string>(StringComparer.Ordinal)
	{
		"AsmodeusFeature", "CalistriaFeature", "CaydenCaileanFeature", "DesnaFeature", "GorumFeature", "IomedaeFeature", "NethysFeature", "PharasmaFeature", "BesmaraFeature", "BlackButterflyFeature",
		"ChaldiraFeature", "LanternKingFeature", "MilaniFeature", "AtheismFeature", "UrgathoaFeature", "YogSothothFeature"
	};

	public const string NameSuffix = "SponsorName";

	public const string DescriptionSuffix = "SponsorDescription";

	public static bool IsAllowed(string simpleName)
	{
		if (string.IsNullOrEmpty(simpleName))
		{
			return false;
		}
		int num = simpleName.LastIndexOf('.');
		if (num <= 0)
		{
			return false;
		}
		string text = simpleName.Substring(num + 1);
		if (text != "SponsorName" && text != "SponsorDescription")
		{
			return false;
		}
		return ((HashSet<string>)Owners).Contains(simpleName.Substring(0, num));
	}
}
