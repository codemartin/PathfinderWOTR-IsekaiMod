using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Components;
using IsekaiMod.Config;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;

internal class BlessingOfTheMythic
{
	private static BlueprintFeatureReference[] SafeGetSelectionFeatures(string guid)
	{
		return BlueprintTools.GetBlueprint<BlueprintFeatureSelection>(guid)?.m_AllFeatures ?? new BlueprintFeatureReference[0];
	}

	private static BlueprintFeatureReference SafeGetFeatureRef(string guid)
	{
		return BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(guid);
	}

	public static void Configure()
	{
		BlueprintCharacterClassReference isekaiClassRef = IsekaiProtagonistClass.GetReference();
		BlueprintFeature MythicAeonSpellsKnown = Helpers.CreateBlueprint(Main.IsekaiContext, "AeonSpellsKnown", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Mythic Aeon Spells");
			bp.SetDescription(Main.IsekaiContext, "Gain the spells of an Aeon.");
			bp.Ranks = 1;
			bp.ReapplyOnLevelUp = false;
			bp.IsClassFeature = true;
			bp.HideInUI = true;
			if (SpellTools.SpellList.AeonSpellMythicList?.SpellsByLevel != null && isekaiClassRef != null)
			{
				SpellLevelList[] spellsByLevel = SpellTools.SpellList.AeonSpellMythicList.SpellsByLevel;
				foreach (SpellLevelList spellLevelList in spellsByLevel)
				{
					if (spellLevelList?.m_Spells != null)
					{
						foreach (BlueprintAbilityReference spell in spellLevelList.m_Spells)
						{
							if (spell != null)
							{
								AddKnownSpellUnlessDlcRestricted.AddTo(bp, isekaiClassRef, spellLevelList.SpellLevel, spell);
							}
						}
					}
				}
			}
		});
		BlueprintFeature MythicAngelSpellsKnown = Helpers.CreateBlueprint(Main.IsekaiContext, "AngelSpellsKnown", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Mythic Angel Spells");
			bp.SetDescription(Main.IsekaiContext, "Gain the spells of an Angel. The Angel list includes 10th-level spells: only a Mastermind of level 21 or higher can learn and cast them. Other Isekai spellbooks stop at 9th level and never learn them.");
			bp.Ranks = 1;
			bp.ReapplyOnLevelUp = false;
			bp.IsClassFeature = true;
			bp.HideInUI = true;
			if (SpellTools.SpellList.AngelMythicSpelllist?.SpellsByLevel != null && isekaiClassRef != null)
			{
				SpellLevelList[] spellsByLevel = SpellTools.SpellList.AngelMythicSpelllist.SpellsByLevel;
				foreach (SpellLevelList spellLevelList in spellsByLevel)
				{
					if (spellLevelList?.m_Spells != null)
					{
						foreach (BlueprintAbilityReference spell2 in spellLevelList.m_Spells)
						{
							if (spell2 != null)
							{
								AddKnownSpellUnlessDlcRestricted.AddTo(bp, isekaiClassRef, spellLevelList.SpellLevel, spell2);
							}
						}
					}
				}
			}
		});
		BlueprintFeature MythicAzataSpellsKnown = Helpers.CreateBlueprint(Main.IsekaiContext, "AzataSpellsKnown", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Mythic Azata Spells");
			bp.SetDescription(Main.IsekaiContext, "Gain the spells of an Azata.");
			bp.Ranks = 1;
			bp.ReapplyOnLevelUp = false;
			bp.IsClassFeature = true;
			bp.HideInUI = true;
			if (SpellTools.SpellList.AzataMythicSpellsSpelllist?.SpellsByLevel != null && isekaiClassRef != null)
			{
				SpellLevelList[] spellsByLevel = SpellTools.SpellList.AzataMythicSpellsSpelllist.SpellsByLevel;
				foreach (SpellLevelList spellLevelList in spellsByLevel)
				{
					if (spellLevelList?.m_Spells != null)
					{
						foreach (BlueprintAbilityReference spell3 in spellLevelList.m_Spells)
						{
							if (spell3 != null)
							{
								AddKnownSpellUnlessDlcRestricted.AddTo(bp, isekaiClassRef, spellLevelList.SpellLevel, spell3);
							}
						}
					}
				}
			}
		});
		BlueprintFeature MythicDemonSpellsKnown = Helpers.CreateBlueprint(Main.IsekaiContext, "DemonSpellsKnown", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Mythic Demon Spells");
			bp.SetDescription(Main.IsekaiContext, "Gain the spells of a Demon.");
			bp.Ranks = 1;
			bp.ReapplyOnLevelUp = false;
			bp.IsClassFeature = true;
			bp.HideInUI = true;
			if (SpellTools.SpellList.DemonSpelllist?.SpellsByLevel != null && isekaiClassRef != null)
			{
				SpellLevelList[] spellsByLevel = SpellTools.SpellList.DemonSpelllist.SpellsByLevel;
				foreach (SpellLevelList spellLevelList in spellsByLevel)
				{
					if (spellLevelList?.m_Spells != null)
					{
						foreach (BlueprintAbilityReference spell4 in spellLevelList.m_Spells)
						{
							if (spell4 != null)
							{
								AddKnownSpellUnlessDlcRestricted.AddTo(bp, isekaiClassRef, spellLevelList.SpellLevel, spell4);
							}
						}
					}
				}
			}
		});
		BlueprintFeature MythicLichSpellsKnown = Helpers.CreateBlueprint(Main.IsekaiContext, "LichSpellsKnown", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Mythic Lich Spells");
			bp.SetDescription(Main.IsekaiContext, "Gain the spells of a Lich. The Lich list includes 10th-level spells: only a Mastermind of level 21 or higher can learn and cast them. Other Isekai spellbooks stop at 9th level and never learn them.");
			bp.Ranks = 1;
			bp.ReapplyOnLevelUp = false;
			bp.IsClassFeature = true;
			bp.HideInUI = true;
			if (SpellTools.SpellList.LichMythicSpelllist?.SpellsByLevel != null && isekaiClassRef != null)
			{
				SpellLevelList[] spellsByLevel = SpellTools.SpellList.LichMythicSpelllist.SpellsByLevel;
				foreach (SpellLevelList spellLevelList in spellsByLevel)
				{
					if (spellLevelList?.m_Spells != null)
					{
						foreach (BlueprintAbilityReference spell5 in spellLevelList.m_Spells)
						{
							if (spell5 != null)
							{
								AddKnownSpellUnlessDlcRestricted.AddTo(bp, isekaiClassRef, spellLevelList.SpellLevel, spell5);
							}
						}
					}
				}
			}
		});
		BlueprintFeature MythicTricksterSpellsKnown = Helpers.CreateBlueprint(Main.IsekaiContext, "TricksterSpellsKnown", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Mythic Trickster Spells");
			bp.SetDescription(Main.IsekaiContext, "Gain the spells of a Trickster.");
			bp.Ranks = 1;
			bp.ReapplyOnLevelUp = false;
			bp.IsClassFeature = true;
			bp.HideInUI = true;
			if (SpellTools.SpellList.TricksterSpelllistMythic?.SpellsByLevel != null && isekaiClassRef != null)
			{
				SpellLevelList[] spellsByLevel = SpellTools.SpellList.TricksterSpelllistMythic.SpellsByLevel;
				foreach (SpellLevelList spellLevelList in spellsByLevel)
				{
					if (spellLevelList?.m_Spells != null)
					{
						foreach (BlueprintAbilityReference spell6 in spellLevelList.m_Spells)
						{
							if (spell6 != null)
							{
								AddKnownSpellUnlessDlcRestricted.AddTo(bp, isekaiClassRef, spellLevelList.SpellLevel, spell6);
							}
						}
					}
				}
			}
		});
		BlueprintFeatureSelection TricksterSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "BlessingOfTheTrickster", delegate(BlueprintFeatureSelection bp)
		{
			bp.SetName(Main.IsekaiContext, "Trickster Mythic Class Feature");
			bp.SetDescription(Main.IsekaiContext, "A feat worthy of a Trickster.");
			bp.Ranks = 1;
			bp.IsClassFeature = true;
			bp.IgnorePrerequisites = false;
			List<BlueprintFeatureReference> list = new List<BlueprintFeatureReference>();
			list.AddRange(SafeGetSelectionFeatures("4fbc563529717de4d92052048143e0f1"));
			list.AddRange(SafeGetSelectionFeatures("5cd96c3460844fc458dc3e1656dafa42"));
			list.AddRange(SafeGetSelectionFeatures("446f4a8b32019f5478a8dfeddac74710"));
			bp.m_AllFeatures = list.Where((BlueprintFeatureReference f) => f != null).Distinct().ToArray();
			bp.m_Features = bp.m_AllFeatures;
		});
		BlueprintFeatureSelection AzataSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "BlessingOfTheAzata", delegate(BlueprintFeatureSelection bp)
		{
			bp.SetName(Main.IsekaiContext, "Azata Mythic Class Feature");
			bp.SetDescription(Main.IsekaiContext, "Let me show you something fun!");
			bp.Ranks = 1;
			bp.IsClassFeature = true;
			bp.IgnorePrerequisites = false;
			bp.m_AllFeatures = (from f in SafeGetSelectionFeatures("8a30e92cd04ff5b459ba7cb03584fda0")
				where f != null
				select f).Distinct().ToArray();
			bp.m_Features = bp.m_AllFeatures;
		});
		BlueprintFeatureSelection LichSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "BlessingOfTheLich", delegate(BlueprintFeatureSelection bp)
		{
			bp.SetName(Main.IsekaiContext, "Lich Mythic Class Feature");
			bp.SetDescription(Main.IsekaiContext, "What?\nA bit of undeath never hurt anyone...");
			bp.Ranks = 1;
			bp.IsClassFeature = true;
			bp.IgnorePrerequisites = false;
			List<BlueprintFeatureReference> list = new List<BlueprintFeatureReference>();
			list.AddRange(SafeGetSelectionFeatures("1f646b820a37d3d4a8ab116a24ee0022"));
			BlueprintFeatureReference blueprintFeatureReference = SafeGetFeatureRef("9703d79082dc75e4aaaa4387b9c95229");
			if (blueprintFeatureReference != null)
			{
				list.Add(blueprintFeatureReference);
			}
			BlueprintFeatureReference blueprintFeatureReference2 = SafeGetFeatureRef("eea98a8c70c68ff489967c6f9cf1876c");
			if (blueprintFeatureReference2 != null)
			{
				list.Add(blueprintFeatureReference2);
			}
			bp.m_AllFeatures = list.Where((BlueprintFeatureReference f) => f != null).Distinct().ToArray();
			bp.m_Features = bp.m_AllFeatures;
		});
		BlueprintFeatureSelection AngelSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "BlessingOfTheAngel", delegate(BlueprintFeatureSelection bp)
		{
			bp.SetName(Main.IsekaiContext, "Angel Mythic Class Feature");
			bp.SetDescription(Main.IsekaiContext, "Behold the blessing of the Angel.");
			bp.Ranks = 1;
			bp.IsClassFeature = true;
			bp.IgnorePrerequisites = false;
			List<BlueprintFeatureReference> list = new List<BlueprintFeatureReference>();
			list.AddRange(SafeGetSelectionFeatures("bdbc41e2bad92a640bd58acf74e2af8b"));
			BlueprintFeatureReference[] array2 = new BlueprintFeatureReference[4]
			{
				SafeGetFeatureRef("bdddaa78f8795024081f2d1eb8b4bd78"),
				SafeGetFeatureRef("dd9648afaaba516488b6aeb8ff86b70a"),
				SafeGetFeatureRef("e4d0a00fb70cd3f4384db0687ef88964"),
				SafeGetFeatureRef("7a6080461eaa278428fe3f12df75c8d0")
			};
			foreach (BlueprintFeatureReference blueprintFeatureReference in array2)
			{
				if (blueprintFeatureReference != null)
				{
					list.Add(blueprintFeatureReference);
				}
			}
			list.AddRange(SafeGetSelectionFeatures("e0ce40968bf0007408b11089a10f36cf"));
			bp.m_AllFeatures = list.Where((BlueprintFeatureReference f) => f != null).Distinct().ToArray();
			bp.m_Features = bp.m_AllFeatures;
		});
		BlueprintFeatureSelection blueprintFeatureSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "BlessingOfTheMythic", delegate(BlueprintFeatureSelection bp)
		{
			bp.SetName(Main.IsekaiContext, "Mythic Class Feature");
			bp.SetDescription(Main.IsekaiContext, "So I am an angel, is that really a reason not to cast demonic spells? \nOr to deny myself some of those sweet abilities of the trickster?\nSome of you Aeons are far too unflexible.\nSpeaking of Aeons, you got some great spells as well, didn't you?\nLet me quickly learn them...\nRequires character level 10. The mythic spell packs additionally require at least one level of Isekai Protagonist, because the spells are added to that spellbook.\nSource: Isekai Reimagined");
			bp.Ranks = 1;
			bp.IsClassFeature = true;
			bp.IgnorePrerequisites = false;
			bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 10;
			});
			bp.m_AllFeatures = new BlueprintFeatureReference[10]
			{
				MythicAeonSpellsKnown.ToReference<BlueprintFeatureReference>(),
				MythicAngelSpellsKnown.ToReference<BlueprintFeatureReference>(),
				MythicAzataSpellsKnown.ToReference<BlueprintFeatureReference>(),
				MythicDemonSpellsKnown.ToReference<BlueprintFeatureReference>(),
				MythicLichSpellsKnown.ToReference<BlueprintFeatureReference>(),
				MythicTricksterSpellsKnown.ToReference<BlueprintFeatureReference>(),
				TricksterSelection.ToReference<BlueprintFeatureReference>(),
				AzataSelection.ToReference<BlueprintFeatureReference>(),
				LichSelection.ToReference<BlueprintFeatureReference>(),
				AngelSelection.ToReference<BlueprintFeatureReference>()
			};
			bp.m_Features = bp.m_AllFeatures;
		});
		BlueprintFeature[] array = new BlueprintFeature[6] { MythicAeonSpellsKnown, MythicAngelSpellsKnown, MythicAzataSpellsKnown, MythicDemonSpellsKnown, MythicLichSpellsKnown, MythicTricksterSpellsKnown };
		foreach (BlueprintFeature blueprintFeature in array)
		{
			if (blueprintFeature != null && isekaiClassRef != null)
			{
				blueprintFeature.AddComponent(delegate(PrerequisiteClassLevel c)
				{
					c.m_CharacterClass = isekaiClassRef;
					c.Level = 1;
				});
			}
		}
		if (blueprintFeatureSelection != null && isekaiClassRef != null && Main.IsekaiContext.AddedContent.RestrictMythicOPAbility)
		{
			blueprintFeatureSelection.AddComponent(delegate(PrerequisiteClassLevel c)
			{
				c.m_CharacterClass = isekaiClassRef;
				c.Level = 1;
			});
		}
		if (blueprintFeatureSelection != null && ContentOffering.ShouldOfferBlessingOfTheMythic(ContentOffering.IsOffered(OfferedContent.MythicClassFeature), ContentOffering.IsOffered(OfferedContent.IsekaiProtagonist)))
		{
			OverpoweredAbilitySelection.AddToNonMythicSelection(blueprintFeatureSelection);
			SpecialPowerSelection.AddToNonMythicSelection(blueprintFeatureSelection);
			FeatTools.Selections.MythicAbilitySelection?.AddToSelection(blueprintFeatureSelection);
			FeatTools.Selections.ExtraMythicAbilityMythicFeat?.AddToSelection(blueprintFeatureSelection);
		}
	}
}
