using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace IsekaiMod.Utilities.SelfTest;

public static class EngineExceptionRules
{
	public sealed class Known
	{
		public string Id { get; }

		public string ThrowSite { get; }

		public bool ProductBug { get; }

		public string Reason { get; }

		public bool SelfTestOnly => !ProductBug;

		public Known(string id, string throwSite, bool productBug, string reason)
		{
			Id = id;
			ThrowSite = throwSite;
			ProductBug = productBug;
			Reason = reason;
		}
	}

	public static readonly IReadOnlyList<Known> KnownExceptions = new Known[3]
	{
		new Known("HARNESS-TOYBOX-POLYMORPH-VIEW", "ToyBox.BagOfPatches.Misc+Polymorph_RestoreView_Patch.Postfix", productBug: false, "ToyBox's Polymorph.RestoreView postfix reads the unit's view; the self-test's blank units have none. A unit in play always has a view."),
		new Known("HARNESS-NAKED-UNIT-ARMOR-SLOT", "Kingmaker.Items.Slots.ItemSlot.get_Item", productBug: false, "TabletopTweaks-Core's MythicMediumArmorFocusEndurance reads Body.Armor.Armor in OnTurnOn without asking whether a slot is filled; a throw-away unit that took Armor Focus (Medium) and lost or never had its armor throws. A unit in play wears armor."),
		new Known("HARNESS-AIR-ACTIVATE-TRIGGER", "Kingmaker.UnitLogic.Mechanics.Actions.ContextActionAddFeature.RunAction", productBug: false, "Vanilla SecondaryElementAir's ActivateTrigger runs ContextActionAddFeature with no context when equipment is put on a throw-away unit that already holds the Air element fact; a unit in play equips through the game's own context.")
	};

	private static readonly Regex ModFramePattern = new Regex("(?<![A-Za-z0-9_.])IsekaiMod\\.", RegexOptions.CultureInvariant);

	public const string SelfTestNamespace = "IsekaiMod.Utilities.SelfTest.";

	private static readonly Regex MonoPrefix = new Regex("^\\s*at\\s+", RegexOptions.CultureInvariant);

	private static readonly Regex WrapperPrefix = new Regex("^\\(wrapper [^)]*\\)\\s*(MonoMod\\.Utils\\.DynamicMethodDefinition\\.)?", RegexOptions.CultureInvariant);

	private static readonly Regex PatchSuffix = new Regex("_Patch\\d+$", RegexOptions.CultureInvariant);

	private static readonly Regex IdentifierToken = new Regex("[A-Za-z_][A-Za-z0-9_]*|[0-9a-f]{32}", RegexOptions.CultureInvariant);

	public static string MethodOf(string frame)
	{
		if (string.IsNullOrWhiteSpace(frame))
		{
			return "";
		}
		string input = MonoPrefix.Replace(frame.Trim(), "");
		input = WrapperPrefix.Replace(input, "");
		int num = input.IndexOf('(');
		if (num >= 0)
		{
			input = input.Substring(0, num);
		}
		int num2 = input.IndexOf(" [0x", StringComparison.Ordinal);
		if (num2 >= 0)
		{
			input = input.Substring(0, num2);
		}
		input = input.Trim();
		return PatchSuffix.Replace(input, "");
	}

	public static bool IsModFrame(string frame)
	{
		if (!string.IsNullOrEmpty(frame))
		{
			return ModFramePattern.IsMatch(frame);
		}
		return false;
	}

	public static bool IsSelfTestFrame(string frame)
	{
		if (!string.IsNullOrEmpty(frame))
		{
			return frame.IndexOf("IsekaiMod.Utilities.SelfTest.", StringComparison.Ordinal) >= 0;
		}
		return false;
	}

	public static string ThrowSiteOf(IEnumerable<string> throwFrames)
	{
		foreach (string item in throwFrames ?? Enumerable.Empty<string>())
		{
			string text = MethodOf(item);
			if (text.Length > 0)
			{
				return text;
			}
		}
		return "";
	}

	public static string FirstModFrame(IEnumerable<string> frames)
	{
		foreach (string item in frames ?? Enumerable.Empty<string>())
		{
			if (IsModFrame(item))
			{
				return MethodOf(item);
			}
		}
		return null;
	}

	public static bool IsExceptionBody(bool flagged, IEnumerable<string> frames)
	{
		if (flagged)
		{
			return true;
		}
		foreach (string item in frames ?? Enumerable.Empty<string>())
		{
			if (!string.IsNullOrWhiteSpace(item))
			{
				return item.TrimStart().StartsWith("at ", StringComparison.Ordinal);
			}
		}
		return false;
	}

	public static bool NamesModBlueprint(string text, ICollection<string> names, ICollection<string> ids)
	{
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		foreach (Match item in IdentifierToken.Matches(text))
		{
			if ((names != null && names.Contains(item.Value)) || (ids != null && ids.Contains(item.Value)))
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsModException(IEnumerable<string> frames, string text, ICollection<string> names, ICollection<string> ids)
	{
		if (FirstModFrame(frames) == null)
		{
			return NamesModBlueprint(text, names, ids);
		}
		return true;
	}

	public static Known Match(string throwSite, IEnumerable<string> allFrames)
	{
		return Match(throwSite, allFrames, KnownExceptions);
	}

	public static Known Match(string throwSite, IEnumerable<string> allFrames, IEnumerable<Known> allowList)
	{
		if (string.IsNullOrEmpty(throwSite))
		{
			return null;
		}
		bool flag = (allFrames ?? Enumerable.Empty<string>()).Any(IsSelfTestFrame);
		foreach (Known item in allowList ?? Enumerable.Empty<Known>())
		{
			if (string.Equals(item.ThrowSite, throwSite, StringComparison.Ordinal) && (!item.SelfTestOnly || flag))
			{
				return item;
			}
		}
		return null;
	}

	public static string KnownIssueLine(Known known, string what)
	{
		return "KNOWN ISSUE " + known.Id + ": " + what + " (" + known.Reason + ")";
	}
}
