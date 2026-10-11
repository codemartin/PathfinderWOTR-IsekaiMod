using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IsekaiMod.Config;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Localization;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Deities;

internal static class IsekaiDeitySelection
{
	internal static readonly string[] ModDeityBlueprints = new string[6] { "YogSothothFeature", "BlackButterflyFeature", "MilaniFeature", "LanternKingFeature", "BesmaraFeature", "ChaldiraFeature" };

	private static readonly Dictionary<string, string> VanillaSponsorGuids = new Dictionary<string, string>
	{
		{ "300e212868bca984687c92bcb66d381b", "Cayden Cailean" },
		{ "88d5da04361b16746bf5b65795e0c38c", "Iomedae" },
		{ "a3a5ccc9c670e6f4ca4a686d23b89900", "Asmodeus" },
		{ "2c0a3b9971327ba4d9d85354d16998c1", "Desna" },
		{ "458750bc214ab2e44abdeae404ab22e9", "Pharasma" },
		{ "c7531715a3f046d4da129619be63f44c", "Calistria" },
		{ "6262cfce7c31626458325ca0909de997", "Nethys" },
		{ "8f49a5d8528a82c44b8c117a89f6b68c", "Gorum" }
	};

	private static readonly Dictionary<string, string> ModSponsorBlueprints = new Dictionary<string, string>
	{
		{ "BesmaraFeature", "Besmara" },
		{ "LanternKingFeature", "The Lantern King" },
		{ "ChaldiraFeature", "Chaldira" },
		{ "MilaniFeature", "Milani" },
		{ "BlackButterflyFeature", "Black Butterfly" }
	};

	private static readonly Dictionary<string, string> VanillaOuterCourtGuids = new Dictionary<string, string> { { "812f6c07148088e41a9ac94b56ac2fc8", "Urgathoa" } };

	private static readonly Dictionary<string, string> ModOuterCourtBlueprints = new Dictionary<string, string> { { "YogSothothFeature", "Yog-Sothoth" } };

	public static void Add()
	{
		Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiDeitySelection", delegate(BlueprintFeatureSelection bp)
		{
			bp.SetName(Main.IsekaiContext, "Isekai");
			bp.SetDescription(Main.IsekaiContext, "Gods from another world.");
			bp.m_AllFeatures = new BlueprintFeatureReference[0];
			bp.Groups = new FeatureGroup[1] { FeatureGroup.Deities };
			bp.Group = FeatureGroup.Deities;
		});
	}

	public static void AddToSelection(BlueprintFeature deityFeature)
	{
		if (deityFeature != null)
		{
			BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiDeitySelection")?.AddToSelection(deityFeature);
		}
	}

	public static void PatchDeitySelection()
	{
		if (ContentOffering.IsOffered(OfferedContent.IsekaiDeities))
		{
			OfferModDeities();
			HideForeignDuplicates();
			Dictionary<int, HashSet<BlueprintFeatureSelection>> dictionary = new Dictionary<int, HashSet<BlueprintFeatureSelection>>();
			HashSet<BlueprintFeatureSelection> value = new HashSet<BlueprintFeatureSelection> { FeatTools.Selections.DeitySelection };
			dictionary.Add(1, value);
			HashSet<BlueprintFeature> knownDeities = new HashSet<BlueprintFeature>();
			HashSet<string> knownNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			FlattenAndDeduplicateSelection(dictionary, knownDeities, knownNames, 1);
			CleanDeitySelection(FeatTools.Selections.DeitySelection);
		}
	}

	private static List<BlueprintFeature> ModDeities()
	{
		List<BlueprintFeature> list = new List<BlueprintFeature>();
		string[] modDeityBlueprints = ModDeityBlueprints;
		foreach (string name in modDeityBlueprints)
		{
			BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, name);
			if (modBlueprint != null)
			{
				list.Add(modBlueprint);
			}
		}
		return list;
	}

	private static void OfferModDeities()
	{
		BlueprintFeatureSelection deitySelection = FeatTools.Selections.DeitySelection;
		if (deitySelection == null)
		{
			return;
		}
		HashSet<BlueprintGuid> hashSet = new HashSet<BlueprintGuid>(from e in AllEntries(deitySelection)
			select e.AssetGuid);
		foreach (BlueprintFeature item in ModDeities())
		{
			if (!hashSet.Contains(item.AssetGuid))
			{
				deitySelection.AddToSelection(item);
			}
		}
	}

	private static IEnumerable<BlueprintFeature> ChildrenOf(BlueprintFeature feature)
	{
		if (!(feature is BlueprintFeatureSelection { m_AllFeatures: not null, m_AllFeatures: var allFeatures }))
		{
			yield break;
		}
		for (int i = 0; i < allFeatures.Length; i++)
		{
			BlueprintFeature blueprintFeature = allFeatures[i]?.Get();
			if (blueprintFeature != null)
			{
				yield return blueprintFeature;
			}
		}
	}

	private static IEnumerable<BlueprintFeature> AllEntries(BlueprintFeatureSelection root)
	{
		HashSet<BlueprintFeature> seen = new HashSet<BlueprintFeature>();
		Stack<BlueprintFeature> stack = new Stack<BlueprintFeature>();
		stack.Push(root);
		while (stack.Count > 0)
		{
			BlueprintFeature blueprintFeature = stack.Pop();
			if (!seen.Add(blueprintFeature))
			{
				continue;
			}
			foreach (BlueprintFeature child in ChildrenOf(blueprintFeature))
			{
				yield return child;
				stack.Push(child);
			}
		}
	}

	internal static string NormalizeDeityName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return "";
		}
		string text = name.Replace("★", " ").Trim();
		text = string.Join(" ", text.Split(new char[4] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
		if (text.StartsWith("The ", StringComparison.OrdinalIgnoreCase))
		{
			text = text.Substring(4).Trim();
		}
		return text.ToLowerInvariant();
	}

	internal static string NormalizeDeityKey(string assetName)
	{
		if (string.IsNullOrEmpty(assetName))
		{
			return "";
		}
		string text = assetName.Trim();
		if (text.EndsWith("Feature", StringComparison.Ordinal))
		{
			text = text.Substring(0, text.Length - "Feature".Length);
		}
		if (text.StartsWith("The", StringComparison.Ordinal))
		{
			text = text.Substring(3);
		}
		StringBuilder stringBuilder = new StringBuilder(text.Length);
		string text2 = text;
		foreach (char c in text2)
		{
			if (char.IsLetterOrDigit(c))
			{
				stringBuilder.Append(char.ToLowerInvariant(c));
			}
		}
		return stringBuilder.ToString();
	}

	internal static List<KeyValuePair<T, T>> PlanDuplicateHiding<T>(T root, Func<T, IEnumerable<T>> childrenOf, Func<T, bool> isSelection, Func<T, string> nameOf, Func<T, bool> isOurs, int maxDepth = 10, Func<string, string> normalize = null) where T : class
	{
		normalize = normalize ?? new Func<string, string>(NormalizeDeityName);
		List<KeyValuePair<T, T>> list = new List<KeyValuePair<T, T>>();
		HashSet<T> hashSet = new HashSet<T>();
		List<T> list2 = new List<T> { root };
		for (int i = 0; i < maxDepth; i++)
		{
			if (list2.Count <= 0)
			{
				break;
			}
			List<T> list3 = new List<T>();
			foreach (T item in list2)
			{
				if (item == null || !hashSet.Add(item))
				{
					continue;
				}
				foreach (T item2 in childrenOf(item) ?? Enumerable.Empty<T>())
				{
					if (item2 != null)
					{
						list.Add(new KeyValuePair<T, T>(item, item2));
						if (isSelection(item2))
						{
							list3.Add(item2);
						}
					}
				}
			}
			list2 = list3;
		}
		HashSet<string> hashSet2 = new HashSet<string>(StringComparer.Ordinal);
		foreach (KeyValuePair<T, T> item3 in list)
		{
			if (!isSelection(item3.Value) && isOurs(item3.Value))
			{
				string text = normalize(nameOf(item3.Value));
				if (text.Length > 0)
				{
					hashSet2.Add(text);
				}
			}
		}
		List<KeyValuePair<T, T>> list4 = new List<KeyValuePair<T, T>>();
		foreach (KeyValuePair<T, T> item4 in list)
		{
			T value = item4.Value;
			if (!isSelection(value) && !isOurs(value) && hashSet2.Contains(normalize(nameOf(value))))
			{
				list4.Add(item4);
			}
		}
		return list4;
	}

	private static void HideForeignDuplicates()
	{
		BlueprintFeatureSelection deitySelection = FeatTools.Selections.DeitySelection;
		if (deitySelection == null)
		{
			return;
		}
		HashSet<BlueprintGuid> ours = new HashSet<BlueprintGuid>(from d in ModDeities()
			select d.AssetGuid);
		foreach (KeyValuePair<BlueprintFeature, BlueprintFeature> item in PlanDuplicateHiding(deitySelection, ChildrenOf, (BlueprintFeature f) => f is BlueprintFeatureSelection, (BlueprintFeature bp) => bp.name, (BlueprintFeature f) => ours.Contains(f.AssetGuid), 10, NormalizeDeityKey))
		{
			BlueprintFeatureSelection blueprintFeatureSelection = item.Key as BlueprintFeatureSelection;
			BlueprintFeature value = item.Value;
			if (blueprintFeatureSelection == null)
			{
				continue;
			}
			BlueprintGuid id = value.AssetGuid;
			if (blueprintFeatureSelection.m_AllFeatures != null)
			{
				blueprintFeatureSelection.m_AllFeatures = blueprintFeatureSelection.m_AllFeatures.Where((BlueprintFeatureReference r) => r == null || r.Guid != id).ToArray();
			}
			if (blueprintFeatureSelection.m_Features != null)
			{
				blueprintFeatureSelection.m_Features = blueprintFeatureSelection.m_Features.Where((BlueprintFeatureReference r) => r == null || r.Guid != id).ToArray();
			}
			Main.IsekaiContext.Logger.Log($"[Deity Duplicate] Hid {value.SafeGetName()} ({id}) from the {blueprintFeatureSelection.name} picker: the Isekai deity of that name is offered instead. The blueprint is kept, so a character who already worships it keeps it.");
		}
	}

	private static void CleanDeitySelection(BlueprintFeatureSelection selection)
	{
		if (selection == null || selection.m_AllFeatures == null)
		{
			return;
		}
		List<BlueprintFeatureReference> list = new List<BlueprintFeatureReference>();
		BlueprintFeatureReference[] allFeatures = selection.m_AllFeatures;
		foreach (BlueprintFeatureReference blueprintFeatureReference in allFeatures)
		{
			if (blueprintFeatureReference != null && blueprintFeatureReference.Get() != null)
			{
				list.Add(blueprintFeatureReference);
			}
		}
		selection.m_AllFeatures = list.ToArray();
		if (selection.m_Features == null)
		{
			return;
		}
		List<BlueprintFeatureReference> list2 = new List<BlueprintFeatureReference>();
		allFeatures = selection.m_Features;
		foreach (BlueprintFeatureReference blueprintFeatureReference2 in allFeatures)
		{
			if (blueprintFeatureReference2 != null && blueprintFeatureReference2.Get() != null)
			{
				list2.Add(blueprintFeatureReference2);
			}
		}
		selection.m_Features = list2.ToArray();
	}

	public static void MarkConstellationSponsors()
	{
		LocalizedString sponsorBadge = SponsorBadges.BadgeParagraph("ConstellationSponsorBadge.Description", "<color=#5B2A86><b>[★ Active Constellation Sponsor ★]</b></color>\nThis deity is an active VIP participant in the Multiversal Constellation Channel. Choosing them as your personal sponsor yields live commentary banter, +25% bonus Cosmic Coins from their donations, and direct access to their Avatar ascension path.\n\n");
		foreach (KeyValuePair<string, string> vanillaSponsorGuid in VanillaSponsorGuids)
		{
			BlueprintFeature blueprint = BlueprintTools.GetBlueprint<BlueprintFeature>(vanillaSponsorGuid.Key);
			if (blueprint != null)
			{
				ApplySponsorMarker(blueprint, vanillaSponsorGuid.Value, sponsorBadge);
			}
		}
		if (ContentOffering.IsOffered(OfferedContent.IsekaiDeities))
		{
			foreach (KeyValuePair<string, string> modSponsorBlueprint in ModSponsorBlueprints)
			{
				BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, modSponsorBlueprint.Key);
				if (modBlueprint != null)
				{
					ApplySponsorMarker(modBlueprint, modSponsorBlueprint.Value, sponsorBadge);
				}
			}
		}
		LocalizedString sponsorBadge2 = SponsorBadges.BadgeParagraph("OuterCourtSponsorBadge.Description", "<color=#7A1010><b>[★ Outer Court Sponsor ★]</b></color>\nThis deity holds a seat, but sits outside the Thirteen. The Outer Court pays for subject matter rather than for conduct: the Key and the Gate for the loop and the world you came from, the Pallid Princess for appetite, corpses and undeath. No alignment you choose will ever please or offend either of them.\nChoosing one as your personal sponsor yields their live commentary and +25% bonus Cosmic Coins from their own donations, but while you sit with them each of the Thirteen keeps back a quarter of every donation it sends you, and the Outer Court grants no Avatar ascension path.\n\n");
		foreach (KeyValuePair<string, string> vanillaOuterCourtGuid in VanillaOuterCourtGuids)
		{
			BlueprintFeature blueprint2 = BlueprintTools.GetBlueprint<BlueprintFeature>(vanillaOuterCourtGuid.Key);
			if (blueprint2 != null)
			{
				ApplySponsorMarker(blueprint2, vanillaOuterCourtGuid.Value, sponsorBadge2);
			}
		}
		if (ContentOffering.IsOffered(OfferedContent.IsekaiDeities))
		{
			foreach (KeyValuePair<string, string> modOuterCourtBlueprint in ModOuterCourtBlueprints)
			{
				BlueprintFeature modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, modOuterCourtBlueprint.Key);
				if (modBlueprint2 != null)
				{
					ApplySponsorMarker(modBlueprint2, modOuterCourtBlueprint.Value, sponsorBadge2);
				}
			}
		}
		BlueprintFeature blueprint3 = BlueprintTools.GetBlueprint<BlueprintFeature>("92c0d2da0a836ce418a267093c09ca54");
		if (blueprint3 != null)
		{
			LocalizedString badge = SponsorBadges.BadgeParagraph("GodDefierBadge.Description", "<color=#5B2A86><b>[★ The Free Agent / God Defier Path ★]</b></color>\nRejects divine sponsorship entirely. All watching deities treat your arrogance with cynical amusement or outrage, but staying true to this defiance unlocks the legendary <b>God Defier</b> transcendence path!\n\n");
			SponsorBadges.Apply(blueprint3, "Atheism", badge);
		}
	}

	private static void ApplySponsorMarker(BlueprintFeature deity, string canonicalName, LocalizedString sponsorBadge)
	{
		SponsorBadges.Apply(deity, canonicalName, sponsorBadge);
	}

	private static bool IsKnownSelection(BlueprintFeatureSelection selection, Dictionary<int, HashSet<BlueprintFeatureSelection>> knownselections)
	{
		foreach (int key in knownselections.Keys)
		{
			foreach (BlueprintFeatureSelection item in knownselections.Get(key))
			{
				if (item.AssetGuid == selection.AssetGuid)
				{
					return true;
				}
			}
		}
		return false;
	}

	private static void FlattenAndDeduplicateSelection(Dictionary<int, HashSet<BlueprintFeatureSelection>> knownselections, HashSet<BlueprintFeature> knownDeities, HashSet<string> knownNames, int level)
	{
		HashSet<BlueprintFeatureSelection> hashSet = knownselections.Get(level);
		HashSet<BlueprintFeatureSelection> hashSet2 = new HashSet<BlueprintFeatureSelection>();
		foreach (BlueprintFeatureSelection item in hashSet)
		{
			if (item?.m_AllFeatures == null)
			{
				continue;
			}
			List<BlueprintFeatureBase> list = new List<BlueprintFeatureBase>();
			BlueprintFeatureReference[] allFeatures = item.m_AllFeatures;
			for (int i = 0; i < allFeatures.Length; i++)
			{
				BlueprintFeature blueprintFeature = allFeatures[i]?.Get();
				if (blueprintFeature == null)
				{
					continue;
				}
				if (blueprintFeature is BlueprintFeatureSelection blueprintFeatureSelection)
				{
					if (IsKnownSelection(blueprintFeatureSelection, knownselections))
					{
						list.Add(blueprintFeatureSelection);
					}
					else
					{
						hashSet2.Add(blueprintFeatureSelection);
					}
				}
				else
				{
					if (blueprintFeature == null)
					{
						continue;
					}
					BlueprintFeature blueprintFeature2 = blueprintFeature;
					if (knownDeities.Contains(blueprintFeature2))
					{
						list.Add(blueprintFeature2);
						continue;
					}
					knownDeities.Add(blueprintFeature2);
					string text = blueprintFeature2.SafeGetName().Replace("★", "").Trim();
					if (!string.IsNullOrEmpty(text) && !knownNames.Add(text))
					{
						Main.IsekaiContext.Logger.Log($"[Deity Conflict] Two deities are called '{text}'; keeping both, including {blueprintFeature2.AssetGuid}.");
					}
				}
			}
			foreach (BlueprintFeatureBase item2 in list)
			{
				if (item2 is BlueprintFeature blueprintFeature3)
				{
					item.RemoveFeatures(blueprintFeature3);
				}
				else if (item2 is BlueprintFeatureSelection blueprintFeatureSelection2)
				{
					item.RemoveFeatures(blueprintFeatureSelection2);
				}
			}
		}
		if (hashSet2.Count != 0)
		{
			level++;
			if (level > 10)
			{
				Main.IsekaiContext.Logger.LogError("Deity List Patching exceeded recursion limit, safely aborting.");
				return;
			}
			knownselections.Add(level, hashSet2);
			FlattenAndDeduplicateSelection(knownselections, knownDeities, knownNames, level);
		}
	}
}
