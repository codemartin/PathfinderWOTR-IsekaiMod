using System;
using System.Linq;
using HarmonyLib;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility.TabletopTweaksBase;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.Utilities;
using UnityModManagerNet;

namespace IsekaiMod.Utilities;

internal class ModSupport
{
	[HarmonyPatch(typeof(BlueprintsCache), "Init")]
	private static class BlueprintsCache_Init_Patch
	{
		private static bool Initialized;

		[HarmonyAfter(new string[] { })]
		public static void Postfix()
		{
			if (!Initialized)
			{
				Initialized = true;
				if (IsTableTopTweakBaseEnabled)
				{
					SafeModSupportStep("TabletopTweaks-Base", AddTabletopTweaksBaseSupport);
				}
				if (IsMysticalMayhemEnabled)
				{
					SafeModSupportStep("MysticalMayhem", AddMysticalMayhemSupport);
				}
				if (IsTomeOfTheFirebirdEnabled)
				{
					SafeModSupportStep("TomeOfTheFirebird", AddTomeOfTheFirebirdSupport);
				}
				if (IsExpandedContentEnabled)
				{
					SafeModSupportStep("ExpandedContent", AddExpandedContentSupport);
				}
				if (IsSpellbookMergeEnabled)
				{
					SafeModSupportStep("SpellbookMerge", AddSpellbookMergeSupport);
				}
			}
		}

		public static void AddTabletopTweaksBaseSupport()
		{
			Main.Log("TabletopTweaks-Base " + InstalledVersion("TabletopTweaks-Base") + " Support Enabled.");
			AutoRime.Add();
			AutoBurning.Add();
			AutoFlaring.Add();
			AutoPiercing.Add();
			AutoSolidShadows.Add();
			AutoEncouraging.Add();
			AutoIntensified.Add();
			AutoElemental.Add();
			BlueprintAbility blueprint = BlueprintTools.GetBlueprint<BlueprintAbility>("fdc6aa23a730426ba4a70a41b76a8fe2");
			if (blueprint != null)
			{
				QuickStudyComponent component = blueprint.GetComponent<QuickStudyComponent>();
				if (component != null && !component.CharacterClass.Contains(IsekaiProtagonistClass.GetReference()))
				{
					component.CharacterClass = component.CharacterClass.AddToArray(IsekaiProtagonistClass.GetReference());
				}
			}
		}

		public static void AddMysticalMayhemSupport()
		{
			Main.Log("Mystical Mayhem " + InstalledVersion("MysticalMayhem") + " Support Enabled.");
			if (!Main.IsekaiContext.AddedContent.MergeIsekaiSpellList)
			{
				BlueprintAbility blueprint = BlueprintTools.GetBlueprint<BlueprintAbility>("d0cd103b15494866b0444c1a961bc40f");
				BlueprintSpellList blueprintSpellList = IsekaiProtagonistSpellList.Get();
				if (blueprint == null)
				{
					Main.Log("WARNING: Mystical Mayhem Meteor Swarm (d0cd103b15494866b0444c1a961bc40f) not found; skipping.");
				}
				else if (blueprintSpellList?.SpellsByLevel == null || blueprintSpellList.SpellsByLevel.Length <= 9)
				{
					Main.Log("WARNING: Isekai Protagonist spell list has no level 9 entry; skipping Meteor Swarm.");
				}
				else
				{
					blueprintSpellList.SpellsByLevel[9].m_Spells.Add(blueprint.ToReference<BlueprintAbilityReference>());
				}
			}
		}

		public static void AddTomeOfTheFirebirdSupport()
		{
			Main.Log("Tome of the Firebird " + InstalledVersion("TomeOfTheFirebird") + " detected (load order only; no integration).");
		}

		public static void AddExpandedContentSupport()
		{
			Main.Log("Expanded Content " + InstalledVersion("ExpandedContent") + " Support Enabled.");
			AddExpandedContentSpells(IsekaiProtagonistSpellList.Get());
		}

		public static void AddSpellbookMergeSupport()
		{
			Main.Log("Spellbook Merge " + InstalledVersion("SpellbookMerge") + " Support Enabled.");
		}

		public static void AddExpandedContentSpells(BlueprintSpellList spellList)
		{
			if (!Main.IsekaiContext.AddedContent.MergeIsekaiSpellList)
			{
				BlueprintAbility blueprint = BlueprintTools.GetBlueprint<BlueprintAbility>("accc5584b62e4e73aa0a693f725ddf60");
				BlueprintAbility blueprint2 = BlueprintTools.GetBlueprint<BlueprintAbility>("e28f4633c0a2425d8895adf20cb22f8f");
				BlueprintAbility blueprint3 = BlueprintTools.GetBlueprint<BlueprintAbility>("f8774451760a427ab4694d10581cfda6");
				BlueprintAbility blueprint4 = BlueprintTools.GetBlueprint<BlueprintAbility>("bad01be5ec684dc39019269c6eff4d6f");
				BlueprintAbility blueprint5 = BlueprintTools.GetBlueprint<BlueprintAbility>("490cc69049be462eafecf69d7030b07a");
				BlueprintAbility blueprint6 = BlueprintTools.GetBlueprint<BlueprintAbility>("e023af1af9c147549a8e7bd246967861");
				BlueprintAbility blueprint7 = BlueprintTools.GetBlueprint<BlueprintAbility>("ff31ae1abe3c418db7842dcc76eca7ee");
				BlueprintAbility blueprint8 = BlueprintTools.GetBlueprint<BlueprintAbility>("98f9c960637f4934bc4cca02c45cb3bc");
				BlueprintAbility blueprint9 = BlueprintTools.GetBlueprint<BlueprintAbility>("2bba038472a64f67b235674c7e27d90c");
				BlueprintAbility blueprint10 = BlueprintTools.GetBlueprint<BlueprintAbility>("dafdc0eef4374785aa827bf5b2059bf0");
				BlueprintAbility blueprint11 = BlueprintTools.GetBlueprint<BlueprintAbility>("fe43fadb91b040b38718e88dd5744413");
				BlueprintAbility blueprint12 = BlueprintTools.GetBlueprint<BlueprintAbility>("a8be30ddf37042d5b56ffaa8eae976d6");
				BlueprintAbility blueprint13 = BlueprintTools.GetBlueprint<BlueprintAbility>("80189142f7c640f39195defdc9777b27");
				BlueprintAbility blueprint14 = BlueprintTools.GetBlueprint<BlueprintAbility>("a49ee6f1ec6744a6b16e3476a504e2a9");
				BlueprintAbility blueprint15 = BlueprintTools.GetBlueprint<BlueprintAbility>("56b8f0304a704a67b3c35cbe8c774854");
				BlueprintAbility blueprint16 = BlueprintTools.GetBlueprint<BlueprintAbility>("31ed0a88513246afac5b0bea60a728a9");
				BlueprintAbility blueprint17 = BlueprintTools.GetBlueprint<BlueprintAbility>("ff9b4a7437d44c5fa29a7573a63728f5");
				BlueprintAbility blueprint18 = BlueprintTools.GetBlueprint<BlueprintAbility>("5a20c33fce1c4d90b0d8e71d7918d699");
				BlueprintAbility blueprint19 = BlueprintTools.GetBlueprint<BlueprintAbility>("a848a5aeb1be4bbdbdf79041c5890098");
				BlueprintAbility blueprint20 = BlueprintTools.GetBlueprint<BlueprintAbility>("7a7877faca0c4e98a5452d29967677e6");
				BlueprintAbility blueprint21 = BlueprintTools.GetBlueprint<BlueprintAbility>("a4fd2673b9d44b8c8d20714b2ee51df6");
				BlueprintAbility blueprint22 = BlueprintTools.GetBlueprint<BlueprintAbility>("9f8ab280738a4578a294ccb8f0b25fa7");
				BlueprintAbility blueprint23 = BlueprintTools.GetBlueprint<BlueprintAbility>("3385edd23aad4795861425acfa798d64");
				BlueprintAbility blueprint24 = BlueprintTools.GetBlueprint<BlueprintAbility>("6eff7010684143c5bcd47120718c75ef");
				BlueprintAbility blueprint25 = BlueprintTools.GetBlueprint<BlueprintAbility>("a1f0d4c3ce2c4c2eb705b18861f14708");
				RegisterExpandedContentSpell(spellList, blueprint18, 1);
				RegisterExpandedContentSpell(spellList, blueprint17, 1);
				RegisterExpandedContentSpell(spellList, blueprint3, 1);
				RegisterExpandedContentSpell(spellList, blueprint5, 1);
				RegisterExpandedContentSpell(spellList, blueprint15, 2);
				RegisterExpandedContentSpell(spellList, blueprint, 2);
				RegisterExpandedContentSpell(spellList, blueprint4, 2);
				RegisterExpandedContentSpell(spellList, blueprint6, 2);
				RegisterExpandedContentSpell(spellList, blueprint11, 2);
				RegisterExpandedContentSpell(spellList, blueprint19, 3);
				RegisterExpandedContentSpell(spellList, blueprint16, 3);
				RegisterExpandedContentSpell(spellList, blueprint14, 3);
				RegisterExpandedContentSpell(spellList, blueprint22, 3);
				RegisterExpandedContentSpell(spellList, blueprint24, 3);
				RegisterExpandedContentSpell(spellList, blueprint2, 3);
				RegisterExpandedContentSpell(spellList, blueprint20, 4);
				RegisterExpandedContentSpell(spellList, blueprint13, 4);
				RegisterExpandedContentSpell(spellList, blueprint8, 4);
				RegisterExpandedContentSpell(spellList, blueprint9, 4);
				RegisterExpandedContentSpell(spellList, blueprint25, 5);
				RegisterExpandedContentSpell(spellList, blueprint7, 5);
				RegisterExpandedContentSpell(spellList, blueprint21, 6);
				RegisterExpandedContentSpell(spellList, blueprint23, 6);
				RegisterExpandedContentSpell(spellList, blueprint12, 7);
				RegisterExpandedContentSpell(spellList, blueprint10, 9);
			}
		}

		private static void RegisterExpandedContentSpell(BlueprintSpellList spellList, BlueprintAbility spell, int level)
		{
			if (spellList != null)
			{
				if (spell == null)
				{
					Main.Log($"WARNING: an Expanded Content spell for Isekai spell level {level} was not found; skipping it.");
				}
				else
				{
					TTCoreExtensions.RegisterSpell(spellList, spell, level);
				}
			}
		}
	}

	public static bool IsExpandedContentEnabled => IsModEnabled("ExpandedContent");

	public static bool IsMysticalMayhemEnabled => IsModEnabled("MysticalMayhem");

	public static bool IsSpellbookMergeEnabled => IsModEnabled("SpellbookMerge");

	public static bool IsExpandedElementEnabled => IsModEnabled("KineticistElementsExpanded");

	public static bool IsTableTopTweakBaseEnabled => IsModEnabled("TabletopTweaks-Base");

	public static bool IsTomeOfTheFirebirdEnabled => IsModEnabled("TomeOfTheFirebird");

	internal static string InstalledVersion(string modId)
	{
		string text = (UnityModManager.modEntries?.FirstOrDefault((UnityModManager.ModEntry m) => m?.Info?.Id == modId))?.Info?.Version;
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		return "(unknown version)";
	}

	internal static void AddExpandedContentDrakes(BlueprintCharacterClass characterClass)
	{
		BlueprintProgression blueprint = BlueprintTools.GetBlueprint<BlueprintProgression>("925c3ece6b9446efa9100fe2cf98542e");
		if (blueprint != null)
		{
			blueprint.m_Classes = blueprint.m_Classes.AddToArray(new BlueprintProgression.ClassWithLevel
			{
				m_Class = characterClass.ToReference<BlueprintCharacterClassReference>(),
				AdditionalLevel = 0
			});
		}
		BlueprintFeatureSelection blueprint2 = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("d78cdd3dd370473ea1ee3003ea6e83f2");
		if (blueprint2 == null || blueprint2.m_AllFeatures == null)
		{
			return;
		}
		BlueprintFeatureReference[] allFeatures = blueprint2.m_AllFeatures;
		for (int i = 0; i < allFeatures.Length; i++)
		{
			BlueprintFeature blueprintFeature = allFeatures[i]?.Get();
			if (blueprintFeature != null)
			{
				IsekaiPetSelection.AddToSelection(blueprintFeature);
			}
		}
	}

	protected static bool IsModEnabled(string modName)
	{
		return UnityModManager.modEntries.Where((UnityModManager.ModEntry mod) => mod.Info.Id.Equals(modName) && mod.Enabled && !mod.ErrorOnLoading).Any();
	}

	public static bool SafeModSupportStep(string name, Action step)
	{
		if (step == null)
		{
			return false;
		}
		try
		{
			step();
			return true;
		}
		catch (Exception arg)
		{
			Main.Log($"WARNING: Mod support for {name} failed and was skipped: {arg}");
			return false;
		}
	}
}
