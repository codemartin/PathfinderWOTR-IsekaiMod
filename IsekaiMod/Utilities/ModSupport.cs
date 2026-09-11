using System.Linq;
using HarmonyLib;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
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

namespace IsekaiMod.Utilities
{
	internal class ModSupport
	{
		[HarmonyPatch(typeof(BlueprintsCache), "Init")]
		private static class BlueprintsCache_Init_Patch
		{
			private static bool Initialized;

			[HarmonyAfter(new string[] { "ExpandedContent", "MysticalMayhem", "SpellbookMerge", "KineticistElementsExpanded", "TabletopTweaks-Base", "TomeOfTheFirebird" })]
			public static void Postfix()
			{
				if (Initialized)
				{
					return;
				}
				Initialized = true;
				if (Main.IsekaiContext.AddedContent.Isekai.IsDisabled("Isekai Protagonist"))
				{
					return;
				}
				if (IsTableTopTweakBaseEnabled)
				{
					Main.Log("TabletopTweaks-Base Support Enabled.");
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
				if (IsMysticalMayhemEnabled)
				{
					Main.Log("Mystical Mayhem Support Enabled.");
					if (!Main.IsekaiContext.AddedContent.MergeIsekaiSpellList)
					{
						BlueprintAbility blueprint2 = BlueprintTools.GetBlueprint<BlueprintAbility>("d0cd103b15494866b0444c1a961bc40f");
						if (blueprint2 != null)
						{
							IsekaiProtagonistSpellList.Get().SpellsByLevel[9].m_Spells.Add(blueprint2.ToReference<BlueprintAbilityReference>());
						}
					}
				}
				if (IsTomeOfTheFirebirdEnabled)
				{
					Main.Log("Tome of the Firebird Support Enabled.");
				}
				if (IsExpandedContentEnabled)
				{
					Main.Log("Expanded Content Support Enabled.");
					AddExpandedContentSpells(IsekaiProtagonistSpellList.Get());
					AddExpandedContentDrakes(IsekaiProtagonistClass.Get());
				}
				if (IsSpellbookMergeEnabled)
				{
					Main.Log("Spellbook Merge Support Enabled.");
					BlueprintFeatureSelectMythicSpellbook blueprint3 = BlueprintTools.GetBlueprint<BlueprintFeatureSelectMythicSpellbook>("2b7027ee76cb4c58b2cff0475bc69fbb");
					BlueprintFeatureSelectMythicSpellbook blueprint4 = BlueprintTools.GetBlueprint<BlueprintFeatureSelectMythicSpellbook>("83385d9f4d714e4e94618703be762a20");
					BlueprintFeatureSelectMythicSpellbook blueprint5 = BlueprintTools.GetBlueprint<BlueprintFeatureSelectMythicSpellbook>("f3ff8515355e4738b128c3d01483f1ca");
					BlueprintFeatureSelectMythicSpellbook blueprint6 = BlueprintTools.GetBlueprint<BlueprintFeatureSelectMythicSpellbook>("c4ef6975167d4cf5acbfd66b60e63f9c");
					BlueprintSpellbook[] array = new BlueprintSpellbook[5]
					{
						IsekaiProtagonistSpellbook.Get(),
						GodEmperorSpellbook.Get(),
						MastermindSpellbook.Get(),
						OverlordSpellbook.Get(),
						ShadowMonarchSpellbook.Get()
					};
					foreach (BlueprintSpellbook spellBook in array)
					{
						TTCoreExtensions.RegisterForMythicSpellbook(blueprint3, spellBook);
						TTCoreExtensions.RegisterForMythicSpellbook(blueprint4, spellBook);
						TTCoreExtensions.RegisterForMythicSpellbook(blueprint5, spellBook);
						TTCoreExtensions.RegisterForMythicSpellbook(blueprint6, spellBook);
					}
				}
			}

			public static void AddExpandedContentSpells(BlueprintSpellList spellList)
			{
				if (!Main.IsekaiContext.AddedContent.MergeIsekaiSpellList && spellList != null)
				{
					RegisterModSpell(spellList, "5a20c33fce1c4d90b0d8e71d7918d699", 1);
					RegisterModSpell(spellList, "ff9b4a7437d44c5fa29a7573a63728f5", 1);
					RegisterModSpell(spellList, "f8774451760a427ab4694d10581cfda6", 1);
					RegisterModSpell(spellList, "490cc69049be462eafecf69d7030b07a", 1);
					RegisterModSpell(spellList, "56b8f0304a704a67b3c35cbe8c774854", 2);
					RegisterModSpell(spellList, "accc5584b62e4e73aa0a693f725ddf60", 2);
					RegisterModSpell(spellList, "bad01be5ec684dc39019269c6eff4d6f", 2);
					RegisterModSpell(spellList, "e023af1af9c147549a8e7bd246967861", 2);
					RegisterModSpell(spellList, "fe43fadb91b040b38718e88dd5744413", 2);
					RegisterModSpell(spellList, "a848a5aeb1be4bbdbdf79041c5890098", 3);
					RegisterModSpell(spellList, "31ed0a88513246afac5b0bea60a728a9", 3);
					RegisterModSpell(spellList, "a49ee6f1ec6744a6b16e3476a504e2a9", 3);
					RegisterModSpell(spellList, "9f8ab280738a4578a294ccb8f0b25fa7", 3);
					RegisterModSpell(spellList, "6eff7010684143c5bcd47120718c75ef", 3);
					RegisterModSpell(spellList, "e28f4633c0a2425d8895adf20cb22f8f", 3);
					RegisterModSpell(spellList, "7a7877faca0c4e98a5452d29967677e6", 4);
					RegisterModSpell(spellList, "80189142f7c640f39195defdc9777b27", 4);
					RegisterModSpell(spellList, "98f9c960637f4934bc4cca02c45cb3bc", 4);
					RegisterModSpell(spellList, "2bba038472a64f67b235674c7e27d90c", 4);
					RegisterModSpell(spellList, "a1f0d4c3ce2c4c2eb705b18861f14708", 5);
					RegisterModSpell(spellList, "ff31ae1abe3c418db7842dcc76eca7ee", 5);
					RegisterModSpell(spellList, "a4fd2673b9d44b8c8d20714b2ee51df6", 6);
					RegisterModSpell(spellList, "3385edd23aad4795861425acfa798d64", 6);
					RegisterModSpell(spellList, "a8be30ddf37042d5b56ffaa8eae976d6", 7);
					RegisterModSpell(spellList, "dafdc0eef4374785aa827bf5b2059bf0", 9);
				}
			}

			private static void RegisterModSpell(BlueprintSpellList spellList, string guid, int level)
			{
				BlueprintAbility blueprint = BlueprintTools.GetBlueprint<BlueprintAbility>(guid);
				if (blueprint != null)
				{
					TTCoreExtensions.RegisterSpell(spellList, blueprint, level);
				}
			}

			public static void AddExpandedContentDrakes(BlueprintCharacterClass characterClass)
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
		}

		public static bool IsExpandedContentEnabled => IsModEnabled("ExpandedContent");

		public static bool IsMysticalMayhemEnabled => IsModEnabled("MysticalMayhem");

		public static bool IsSpellbookMergeEnabled => IsModEnabled("SpellbookMerge");

		public static bool IsExpandedElementEnabled => IsModEnabled("KineticistElementsExpanded");

		public static bool IsTableTopTweakBaseEnabled => IsModEnabled("TabletopTweaks-Base");

		public static bool IsTomeOfTheFirebirdEnabled => IsModEnabled("TomeOfTheFirebird");

		protected static bool IsModEnabled(string modName)
		{
			return UnityModManager.modEntries.Where((UnityModManager.ModEntry mod) => mod.Info.Id.Equals(modName) && mod.Enabled && !mod.ErrorOnLoading).Any();
		}
	}
}
