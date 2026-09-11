using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist
{
	internal class PrestigeClassReplaceSpellbook
	{
		private struct PrestigeSpellbookData
		{
			public string name;

			public LocalizedString description;

			public int requiredLevel;

			public FeatureGroup[] featureGroups;

			public BlueprintFeatureSelection selection;
		}

		private struct ReplaceSpellbookData
		{
			public string name;

			public string displayName;

			public BlueprintCharacterClassReference characterClassReference;

			public BlueprintSpellbookReference spellbookReference;

			public BlueprintArchetypeReference includedArchetype;

			public BlueprintArchetypeReference[] excludedArchetypes;
		}

		public static void Patch()
		{
			PrestigeSpellbookData[] array = new PrestigeSpellbookData[8]
			{
				new PrestigeSpellbookData
				{
					name = "Loremaster",
					description = StaticReferences.Strings.ReplaceSpellbookDescription.Loremaster,
					requiredLevel = 3,
					featureGroups = new FeatureGroup[1] { FeatureGroup.MythicAdditionalProgressions },
					selection = FeatTools.Selections.LoremasterSpellbookSelection
				},
				new PrestigeSpellbookData
				{
					name = "HellknightSignifier",
					description = StaticReferences.Strings.ReplaceSpellbookDescription.HellknightSignifier,
					requiredLevel = 3,
					featureGroups = new FeatureGroup[1] { FeatureGroup.HellknightSigniferSpellbook },
					selection = FeatTools.Selections.HellknightSigniferSpellbook
				},
				new PrestigeSpellbookData
				{
					name = "ArcaneTrickster",
					description = StaticReferences.Strings.ReplaceSpellbookDescription.ArcaneTrickster,
					requiredLevel = 2,
					featureGroups = new FeatureGroup[1] { FeatureGroup.ArcaneTricksterSpellbook },
					selection = FeatTools.Selections.ArcaneTricksterSpellbookSelection
				},
				new PrestigeSpellbookData
				{
					name = "MysticTheurgeArcane",
					description = StaticReferences.Strings.ReplaceSpellbookDescription.MysticTheurgeArcane,
					requiredLevel = 2,
					featureGroups = new FeatureGroup[1] { FeatureGroup.MysticTheurgeArcaneSpellbook },
					selection = FeatTools.Selections.MysticTheurgeArcaneSpellbookSelection
				},
				new PrestigeSpellbookData
				{
					name = "MysticTheurgeDivine",
					description = StaticReferences.Strings.ReplaceSpellbookDescription.MysticTheurgeDivine,
					requiredLevel = 2,
					featureGroups = new FeatureGroup[1] { FeatureGroup.MysticTheurgeDivineSpellbook },
					selection = FeatTools.Selections.MysticTheurgeDivineSpellbookSelection
				},
				new PrestigeSpellbookData
				{
					name = "DragonDisciple",
					description = StaticReferences.Strings.ReplaceSpellbookDescription.DragonDisciple,
					requiredLevel = 2,
					featureGroups = new FeatureGroup[1] { FeatureGroup.DragonDiscipleSpellbook },
					selection = FeatTools.Selections.DragonDiscipleSpellbookSelection
				},
				new PrestigeSpellbookData
				{
					name = "EldritchKnight",
					description = StaticReferences.Strings.ReplaceSpellbookDescription.EldritchKnight,
					requiredLevel = 2,
					featureGroups = new FeatureGroup[1] { FeatureGroup.EldritchKnightSpellbook },
					selection = FeatTools.Selections.EldritchKnightSpellbookSelection
				},
				new PrestigeSpellbookData
				{
					name = "WinterWitch",
					description = StaticReferences.Strings.ReplaceSpellbookDescription.WinterWitch,
					requiredLevel = 2,
					featureGroups = new FeatureGroup[0],
					selection = FeatTools.Selections.WinterWitchSpellbookSelection
				}
			};
			ReplaceSpellbookData[] array2 = new ReplaceSpellbookData[4]
			{
				new ReplaceSpellbookData
				{
					name = "Isekai",
					displayName = "Isekai Protagonist",
					characterClassReference = IsekaiProtagonistClass.GetReference(),
					spellbookReference = IsekaiProtagonistSpellbook.GetReference(),
					includedArchetype = null,
					excludedArchetypes = new BlueprintArchetypeReference[3]
					{
						GodEmperorArchetype.GetReference(),
						MastermindArchetype.GetReference(),
						OverlordArchetype.GetReference()
					}
				},
				new ReplaceSpellbookData
				{
					name = "GodEmperor",
					displayName = "God Emperor",
					characterClassReference = IsekaiProtagonistClass.GetReference(),
					spellbookReference = GodEmperorSpellbook.GetReference(),
					includedArchetype = GodEmperorArchetype.GetReference(),
					excludedArchetypes = new BlueprintArchetypeReference[2]
					{
						MastermindArchetype.GetReference(),
						OverlordArchetype.GetReference()
					}
				},
				new ReplaceSpellbookData
				{
					name = "Mastermind",
					displayName = "Mastermind",
					characterClassReference = IsekaiProtagonistClass.GetReference(),
					spellbookReference = MastermindSpellbook.GetReference(),
					includedArchetype = MastermindArchetype.GetReference(),
					excludedArchetypes = new BlueprintArchetypeReference[2]
					{
						GodEmperorArchetype.GetReference(),
						OverlordArchetype.GetReference()
					}
				},
				new ReplaceSpellbookData
				{
					name = "Overlord",
					displayName = "Overlord",
					characterClassReference = IsekaiProtagonistClass.GetReference(),
					spellbookReference = OverlordSpellbook.GetReference(),
					includedArchetype = OverlordArchetype.GetReference(),
					excludedArchetypes = new BlueprintArchetypeReference[2]
					{
						GodEmperorArchetype.GetReference(),
						MastermindArchetype.GetReference()
					}
				}
			};
			PrestigeSpellbookData[] array3 = array;
			foreach (PrestigeSpellbookData prestigeSpellbookData in array3)
			{
				ReplaceSpellbookData[] array4 = array2;
				for (int j = 0; j < array4.Length; j++)
				{
					AddPrestigeSpellbook(array4[j], prestigeSpellbookData);
				}
			}
		}

		private static void AddPrestigeSpellbook(ReplaceSpellbookData replaceSpellbookData, PrestigeSpellbookData prestigeSpellbookData)
		{
			if (replaceSpellbookData.characterClassReference == null || replaceSpellbookData.spellbookReference == null || prestigeSpellbookData.selection == null)
			{
				return;
			}
			string name = prestigeSpellbookData.name + replaceSpellbookData.name;
			BlueprintFeatureReplaceSpellbook blueprintFeatureReplaceSpellbook = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintFeatureReplaceSpellbook bp)
			{
				bp.SetName(Main.IsekaiContext, replaceSpellbookData.displayName);
				bp.SetDescription(prestigeSpellbookData.description);
				bp.Groups = prestigeSpellbookData.featureGroups;
				bp.HideInUI = true;
				bp.HideNotAvailibleInUI = true;
				bp.HideInCharacterSheetAndLevelUp = false;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.m_Spellbook = replaceSpellbookData.spellbookReference;
				bp.AddComponent(delegate(PrerequisiteClassSpellLevel c)
				{
					c.Group = Prerequisite.GroupType.All;
					c.m_CharacterClass = replaceSpellbookData.characterClassReference;
					c.RequiredSpellLevel = prestigeSpellbookData.requiredLevel;
				});
				if (replaceSpellbookData.includedArchetype != null)
				{
					bp.AddComponent(delegate(PrerequisiteArchetypeLevel c)
					{
						c.Group = Prerequisite.GroupType.All;
						c.m_CharacterClass = replaceSpellbookData.characterClassReference;
						c.m_Archetype = replaceSpellbookData.includedArchetype;
						c.Level = 1;
					});
				}
				if (replaceSpellbookData.excludedArchetypes != null)
				{
					BlueprintArchetypeReference[] excludedArchetypes = replaceSpellbookData.excludedArchetypes;
					foreach (BlueprintArchetypeReference archetypeReference in excludedArchetypes)
					{
						if (archetypeReference != null)
						{
							bp.AddComponent(delegate(PrerequisiteNoArchetype c)
							{
								c.Group = Prerequisite.GroupType.All;
								c.m_CharacterClass = replaceSpellbookData.characterClassReference;
								c.m_Archetype = archetypeReference;
							});
						}
					}
				}
			});
			if (blueprintFeatureReplaceSpellbook != null)
			{
				prestigeSpellbookData.selection.AddToSelection(blueprintFeatureReplaceSpellbook);
			}
		}
	}
}
