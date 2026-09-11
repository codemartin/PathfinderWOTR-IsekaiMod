using System;
using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Deities
{
	internal static class IsekaiDeitySelection
	{
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

		public static void PatchDeitySelection()
		{
			Dictionary<int, HashSet<BlueprintFeatureSelection>> dictionary = new Dictionary<int, HashSet<BlueprintFeatureSelection>>();
			HashSet<BlueprintFeatureSelection> value = new HashSet<BlueprintFeatureSelection> { FeatTools.Selections.DeitySelection };
			dictionary.Add(1, value);
			HashSet<BlueprintFeature> knownDeities = new HashSet<BlueprintFeature>();
			HashSet<string> knownNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			FlattenAndDeduplicateSelection(dictionary, knownDeities, knownNames, 1);
			CleanDeitySelection(FeatTools.Selections.DeitySelection);
			MarkConstellationSponsors();
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

		private static void MarkConstellationSponsors()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>
			{
				{ "300e212868bca984687c92bcb66d381b", "Cayden Cailean" },
				{ "88d5da04361b16746bf5b65795e0c38c", "Iomedae" },
				{ "a3a5ccc9c670e6f4ca4a686d23b89900", "Asmodeus" },
				{ "2c0a3b9971327ba4d9d85354d16998c1", "Desna" },
				{ "458750bc214ab2e44abdeae404ab22e9", "Pharasma" },
				{ "c7531715a3f046d4da129619be63f44c", "Calistria" },
				{ "6262cfce7c31626458325ca0909de997", "Nethys" },
				{ "8f49a5d8528a82c44b8c117a89f6b68c", "Gorum" },
				{ "470d824767d34e2e9a59b6d70681af7a", "Besmara" },
				{ "ed7ba1da81834e55be5530ecbc2b1464", "The Lantern King" },
				{ "47a505b2df6a4401826d708ca6723223", "Chaldira" },
				{ "68fb3a3c9b744951b14ea9f26bf5d1f8", "Milani" },
				{ "1f29c6dc4aa940aa8c16ca61c470fe4c", "Black Butterfly" }
			};
			foreach (KeyValuePair<string, string> item in dictionary)
			{
				BlueprintFeature blueprintFeature = BlueprintSafetyExtensions.SafeGetBlueprint<BlueprintFeature>(item.Key);
				if (blueprintFeature != null)
				{
					ApplySponsorMarker(blueprintFeature, item.Value, "<color=#F5C542><b>[★ Active Constellation Sponsor ★]</b></color>\nThis deity is an active VIP participant in the Multiversal Constellation Channel. Choosing them as your personal sponsor yields live commentary banter, +25% bonus Cosmic Coins from their donations, and direct access to their Avatar ascension path.\n\n");
				}
			}
			if (FeatTools.Selections.DeitySelection?.m_AllFeatures != null)
			{
				BlueprintFeatureReference[] allFeatures = FeatTools.Selections.DeitySelection.m_AllFeatures;
				for (int i = 0; i < allFeatures.Length; i++)
				{
					BlueprintFeature blueprintFeature2 = allFeatures[i]?.Get();
					if (blueprintFeature2 == null)
					{
						continue;
					}
					string text = blueprintFeature2.SafeGetName();
					foreach (KeyValuePair<string, string> item2 in dictionary)
					{
						if (text.IndexOf(item2.Value, StringComparison.OrdinalIgnoreCase) >= 0)
						{
							ApplySponsorMarker(blueprintFeature2, item2.Value, "<color=#F5C542><b>[★ Active Constellation Sponsor ★]</b></color>\nThis deity is an active VIP participant in the Multiversal Constellation Channel. Choosing them as your personal sponsor yields live commentary banter, +25% bonus Cosmic Coins from their donations, and direct access to their Avatar ascension path.\n\n");
						}
					}
				}
			}
			BlueprintFeature blueprintFeature3 = BlueprintSafetyExtensions.SafeGetBlueprint<BlueprintFeature>("92c0d2da0a836ce418a267093c09ca54");
			if (blueprintFeature3 != null)
			{
				string text2 = blueprintFeature3.SafeGetDescription();
				if (!text2.Contains("[★ The Free Agent / God Defier Path ★]"))
				{
					blueprintFeature3.SetDescription(Main.IsekaiContext, "<color=#F5C542><b>[★ The Free Agent / God Defier Path ★]</b></color>\nRejects divine sponsorship entirely. All watching deities treat your arrogance with cynical amusement or outrage, but staying true to this defiance unlocks the legendary <b>God Defier</b> transcendence path!\n\n" + text2);
				}
				string text3 = blueprintFeature3.SafeGetName("Atheism");
				if (!text3.Contains("★"))
				{
					blueprintFeature3.SetName(Main.IsekaiContext, "★ " + text3);
				}
			}
		}

		private static void ApplySponsorMarker(BlueprintFeature deity, string canonicalName, string sponsorBadge)
		{
			string text = deity.SafeGetDescription();
			if (!text.Contains("[★ Active Constellation Sponsor ★]"))
			{
				deity.SetDescription(Main.IsekaiContext, sponsorBadge + text);
			}
			if (!deity.SafeGetName(canonicalName).Contains("★"))
			{
				deity.SetName(Main.IsekaiContext, "★ " + canonicalName);
			}
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
						string text = blueprintFeature2.SafeGetName().Replace("★", "").Trim();
						if (knownDeities.Contains(blueprintFeature2))
						{
							list.Add(blueprintFeature2);
							continue;
						}
						if (!string.IsNullOrEmpty(text) && knownNames.Contains(text))
						{
							Main.IsekaiContext.Logger.Log($"[Deity Conflict Prevention] Filtered duplicate deity '{text}' ({blueprintFeature2.AssetGuid}) from another mod.");
							list.Add(blueprintFeature2);
							continue;
						}
						knownDeities.Add(blueprintFeature2);
						if (!string.IsNullOrEmpty(text))
						{
							knownNames.Add(text);
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

		public static void AddToSelection(BlueprintFeature deityFeature)
		{
			if (deityFeature == null)
			{
				return;
			}
			BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiDeitySelection")?.AddToSelection(deityFeature);
			string text = deityFeature.SafeGetName().Replace("★", "").Trim();
			bool flag = false;
			if (FeatTools.Selections.DeitySelection?.m_AllFeatures != null)
			{
				BlueprintFeatureReference[] allFeatures = FeatTools.Selections.DeitySelection.m_AllFeatures;
				for (int i = 0; i < allFeatures.Length; i++)
				{
					BlueprintFeature blueprintFeature = allFeatures[i]?.Get();
					if (blueprintFeature != null)
					{
						if (blueprintFeature.AssetGuid == deityFeature.AssetGuid)
						{
							flag = true;
							break;
						}
						string a = blueprintFeature.SafeGetName().Replace("★", "").Trim();
						if (!string.IsNullOrEmpty(text) && string.Equals(a, text, StringComparison.OrdinalIgnoreCase))
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (!flag)
			{
				FeatTools.Selections.DeitySelection.AddToSelection(deityFeature);
			}
		}
	}
}
