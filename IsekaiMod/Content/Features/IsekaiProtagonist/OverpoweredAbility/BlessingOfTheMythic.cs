using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class BlessingOfTheMythic
	{
		private static BlueprintFeatureReference[] SafeGetSelectionFeatures(string guid)
		{
			return BlueprintTools.GetBlueprint<BlueprintFeatureSelection>(guid)?.m_AllFeatures ?? new BlueprintFeatureReference[0];
		}

		private static BlueprintFeatureReference SafeGetFeatureRef(string guid)
		{
			return BlueprintTools.GetBlueprint<BlueprintFeature>(guid)?.ToReference<BlueprintFeatureReference>();
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
					foreach (SpellLevelList AeonSpellLevel in spellsByLevel)
					{
						if (AeonSpellLevel?.m_Spells != null)
						{
							foreach (BlueprintAbilityReference spell in AeonSpellLevel.m_Spells)
							{
								if (spell != null)
								{
									bp.AddComponent(delegate(AddKnownSpell cp)
									{
										cp.SpellLevel = AeonSpellLevel.SpellLevel;
										cp.m_Spell = spell;
										cp.m_CharacterClass = isekaiClassRef;
									});
								}
							}
						}
					}
				}
			});
			BlueprintFeature MythicAngelSpellsKnown = Helpers.CreateBlueprint(Main.IsekaiContext, "AngelSpellsKnown", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Mythic Angel Spells");
				bp.SetDescription(Main.IsekaiContext, "Gain the spells of an Angel.");
				bp.Ranks = 1;
				bp.ReapplyOnLevelUp = false;
				bp.IsClassFeature = true;
				bp.HideInUI = true;
				if (SpellTools.SpellList.AngelMythicSpelllist?.SpellsByLevel != null && isekaiClassRef != null)
				{
					SpellLevelList[] spellsByLevel = SpellTools.SpellList.AngelMythicSpelllist.SpellsByLevel;
					foreach (SpellLevelList AngelSpellLevel in spellsByLevel)
					{
						if (AngelSpellLevel?.m_Spells != null)
						{
							foreach (BlueprintAbilityReference spell in AngelSpellLevel.m_Spells)
							{
								if (spell != null)
								{
									bp.AddComponent(delegate(AddKnownSpell cp)
									{
										cp.SpellLevel = AngelSpellLevel.SpellLevel;
										cp.m_Spell = spell;
										cp.m_CharacterClass = isekaiClassRef;
									});
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
					foreach (SpellLevelList AzataSpellLevel in spellsByLevel)
					{
						if (AzataSpellLevel?.m_Spells != null)
						{
							foreach (BlueprintAbilityReference spell in AzataSpellLevel.m_Spells)
							{
								if (spell != null)
								{
									bp.AddComponent(delegate(AddKnownSpell cp)
									{
										cp.SpellLevel = AzataSpellLevel.SpellLevel;
										cp.m_Spell = spell;
										cp.m_CharacterClass = isekaiClassRef;
									});
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
					foreach (SpellLevelList DemonSpellLevel in spellsByLevel)
					{
						if (DemonSpellLevel?.m_Spells != null)
						{
							foreach (BlueprintAbilityReference spell in DemonSpellLevel.m_Spells)
							{
								if (spell != null)
								{
									bp.AddComponent(delegate(AddKnownSpell cp)
									{
										cp.SpellLevel = DemonSpellLevel.SpellLevel;
										cp.m_Spell = spell;
										cp.m_CharacterClass = isekaiClassRef;
									});
								}
							}
						}
					}
				}
			});
			BlueprintFeature MythicLichSpellsKnown = Helpers.CreateBlueprint(Main.IsekaiContext, "LichSpellsKnown", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Mythic Lich Spells");
				bp.SetDescription(Main.IsekaiContext, "Gain the spells of a Lich.");
				bp.Ranks = 1;
				bp.ReapplyOnLevelUp = false;
				bp.IsClassFeature = true;
				bp.HideInUI = true;
				if (SpellTools.SpellList.LichMythicSpelllist?.SpellsByLevel != null && isekaiClassRef != null)
				{
					SpellLevelList[] spellsByLevel = SpellTools.SpellList.LichMythicSpelllist.SpellsByLevel;
					foreach (SpellLevelList LichSpellLevel in spellsByLevel)
					{
						if (LichSpellLevel?.m_Spells != null)
						{
							foreach (BlueprintAbilityReference spell in LichSpellLevel.m_Spells)
							{
								if (spell != null)
								{
									bp.AddComponent(delegate(AddKnownSpell cp)
									{
										cp.SpellLevel = LichSpellLevel.SpellLevel;
										cp.m_Spell = spell;
										cp.m_CharacterClass = isekaiClassRef;
									});
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
					foreach (SpellLevelList TricksterSpellLevel in spellsByLevel)
					{
						if (TricksterSpellLevel?.m_Spells != null)
						{
							foreach (BlueprintAbilityReference spell in TricksterSpellLevel.m_Spells)
							{
								if (spell != null)
								{
									bp.AddComponent(delegate(AddKnownSpell cp)
									{
										cp.SpellLevel = TricksterSpellLevel.SpellLevel;
										cp.m_Spell = spell;
										cp.m_CharacterClass = isekaiClassRef;
									});
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
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 10;
				});
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
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 10;
				});
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
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 10;
				});
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
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 10;
				});
				List<BlueprintFeatureReference> list = new List<BlueprintFeatureReference>();
				list.AddRange(SafeGetSelectionFeatures("bdbc41e2bad92a640bd58acf74e2af8b"));
				BlueprintFeatureReference[] array = new BlueprintFeatureReference[4]
				{
					SafeGetFeatureRef("bdddaa78f8795024081f2d1eb8b4bd78"),
					SafeGetFeatureRef("dd9648afaaba516488b6aeb8ff86b70a"),
					SafeGetFeatureRef("e4d0a00fb70cd3f4384db0687ef88964"),
					SafeGetFeatureRef("7a6080461eaa278428fe3f12df75c8d0")
				};
				foreach (BlueprintFeatureReference blueprintFeatureReference in array)
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
				bp.SetDescription(Main.IsekaiContext, "So I am an angel, is that really a reason not to cast demonic spells? \nOr to deny myself some of those sweet abilities of the trickster?\nSome of you Aeons are far too unflexible.\nSpeaking of Aeons, you got some great spells as well, didn't you?\nLet me quickly learn them...\nSource: Isekai Mod");
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
			if (blueprintFeatureSelection != null)
			{
				OverpoweredAbilitySelection.AddToNonMythicSelection(blueprintFeatureSelection);
				SpecialPowerSelection.AddToNonMythicSelection(blueprintFeatureSelection);
				FeatTools.Selections.MythicAbilitySelection?.AddToSelection(blueprintFeatureSelection);
				FeatTools.Selections.ExtraMythicAbilityMythicFeat?.AddToSelection(blueprintFeatureSelection);
			}
		}
	}
}
