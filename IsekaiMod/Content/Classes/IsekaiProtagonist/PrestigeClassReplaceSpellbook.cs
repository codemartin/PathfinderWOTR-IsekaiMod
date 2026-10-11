using System;
using System.Collections.Generic;
using IsekaiMod.Components;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist;

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

		public string prestigeFeatureName;

		public string displayName;

		public BlueprintCharacterClassReference characterClassReference;

		public BlueprintSpellbookReference spellbookReference;

		public BlueprintArchetypeReference includedArchetype;

		public BlueprintArchetypeReference[] excludedArchetypes;

		public bool isDivine;
	}

	public const string IsekaiPrestigeSpellbookSelectionName = "EpicPrestigeSpellbookSelection";

	public const int ReplaceSpellbookRanks = 20;

	public const string NoSpellbookFeatureName = "EpicPrestigeReplaceSpellbookNone";

	private static readonly string[] PrestigeFeatureNames = new string[5] { "EpicPrestigeReplaceSpellbookIsekai", "EpicPrestigeReplaceSpellbookGodEmperor", "EpicPrestigeReplaceSpellbookMastermind", "EpicPrestigeReplaceSpellbookOverlord", "EpicPrestigeReplaceSpellbookShadowMonarch" };

	internal const string MysticTheurgeArcaneName = "MysticTheurgeArcane";

	internal const string MysticTheurgeDivineName = "MysticTheurgeDivine";

	private static ReplaceSpellbookData[] IsekaiSpellbookChoices()
	{
		return new ReplaceSpellbookData[5]
		{
			new ReplaceSpellbookData
			{
				name = "Isekai",
				prestigeFeatureName = PrestigeFeatureNames[0],
				displayName = "Isekai Protagonist",
				characterClassReference = IsekaiProtagonistClass.GetReference(),
				spellbookReference = IsekaiProtagonistSpellbook.GetReference(),
				includedArchetype = null,
				excludedArchetypes = new BlueprintArchetypeReference[4]
				{
					GodEmperorArchetype.GetReference(),
					MastermindArchetype.GetReference(),
					OverlordArchetype.GetReference(),
					ShadowMonarchArchetype.GetReference()
				}
			},
			new ReplaceSpellbookData
			{
				name = "GodEmperor",
				prestigeFeatureName = PrestigeFeatureNames[1],
				displayName = "God Emperor",
				characterClassReference = IsekaiProtagonistClass.GetReference(),
				spellbookReference = GodEmperorSpellbook.GetReference(),
				includedArchetype = GodEmperorArchetype.GetReference(),
				isDivine = true,
				excludedArchetypes = new BlueprintArchetypeReference[3]
				{
					MastermindArchetype.GetReference(),
					OverlordArchetype.GetReference(),
					ShadowMonarchArchetype.GetReference()
				}
			},
			new ReplaceSpellbookData
			{
				name = "Mastermind",
				prestigeFeatureName = PrestigeFeatureNames[2],
				displayName = "Mastermind",
				characterClassReference = IsekaiProtagonistClass.GetReference(),
				spellbookReference = MastermindSpellbook.GetReference(),
				includedArchetype = MastermindArchetype.GetReference(),
				excludedArchetypes = new BlueprintArchetypeReference[3]
				{
					GodEmperorArchetype.GetReference(),
					OverlordArchetype.GetReference(),
					ShadowMonarchArchetype.GetReference()
				}
			},
			new ReplaceSpellbookData
			{
				name = "Overlord",
				prestigeFeatureName = PrestigeFeatureNames[3],
				displayName = "Overlord",
				characterClassReference = IsekaiProtagonistClass.GetReference(),
				spellbookReference = OverlordSpellbook.GetReference(),
				includedArchetype = OverlordArchetype.GetReference(),
				excludedArchetypes = new BlueprintArchetypeReference[3]
				{
					GodEmperorArchetype.GetReference(),
					MastermindArchetype.GetReference(),
					ShadowMonarchArchetype.GetReference()
				}
			},
			new ReplaceSpellbookData
			{
				name = "ShadowMonarch",
				prestigeFeatureName = PrestigeFeatureNames[4],
				displayName = "Shadow Monarch",
				characterClassReference = IsekaiProtagonistClass.GetReference(),
				spellbookReference = ShadowMonarchSpellbook.GetReference(),
				includedArchetype = ShadowMonarchArchetype.GetReference(),
				excludedArchetypes = new BlueprintArchetypeReference[3]
				{
					GodEmperorArchetype.GetReference(),
					MastermindArchetype.GetReference(),
					OverlordArchetype.GetReference()
				}
			}
		};
	}

	public static string[] IsekaiPrestigeSpellbookBlueprintNames()
	{
		string[] array = new string[PrestigeFeatureNames.Length + 2];
		array[0] = "EpicPrestigeSpellbookSelection";
		array[1] = "EpicPrestigeReplaceSpellbookNone";
		Array.Copy(PrestigeFeatureNames, 0, array, 2, PrestigeFeatureNames.Length);
		return array;
	}

	public static BlueprintFeatureSelection GetIsekaiPrestigeSpellbookSelection()
	{
		return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "EpicPrestigeSpellbookSelection");
	}

	public static void CreateIsekaiPrestigeSpellbookSelection()
	{
		ReplaceSpellbookData[] array = IsekaiSpellbookChoices();
		List<BlueprintFeatureReference> features = new List<BlueprintFeatureReference>();
		ReplaceSpellbookData[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			ReplaceSpellbookData replaceSpellbookData = array2[i];
			if (replaceSpellbookData.characterClassReference == null || replaceSpellbookData.spellbookReference == null)
			{
				continue;
			}
			ReplaceSpellbookData data = replaceSpellbookData;
			BlueprintFeatureReplaceSpellbook blueprintFeatureReplaceSpellbook = Helpers.CreateBlueprint(Main.IsekaiContext, data.prestigeFeatureName, delegate(BlueprintFeatureReplaceSpellbook bp)
			{
				bp.SetName(Main.IsekaiContext, data.displayName);
				bp.SetDescription(Main.IsekaiContext, "Your levels in this prestige class advance your " + data.displayName + " spellbook.");
				bp.Groups = new FeatureGroup[0];
				bp.HideNotAvailibleInUI = true;
				bp.HideInCharacterSheetAndLevelUp = false;
				bp.Ranks = 20;
				bp.IsClassFeature = true;
				bp.m_Spellbook = data.spellbookReference;
				bp.AddComponent(delegate(PrerequisiteClassSpellLevel c)
				{
					c.Group = Prerequisite.GroupType.All;
					c.m_CharacterClass = data.characterClassReference;
					c.RequiredSpellLevel = 1;
				});
				if (data.includedArchetype != null)
				{
					bp.AddComponent(delegate(PrerequisiteArchetypeLevel c)
					{
						c.Group = Prerequisite.GroupType.All;
						c.m_CharacterClass = data.characterClassReference;
						c.m_Archetype = data.includedArchetype;
						c.Level = 1;
					});
				}
				if (data.excludedArchetypes != null)
				{
					BlueprintArchetypeReference[] excludedArchetypes = data.excludedArchetypes;
					foreach (BlueprintArchetypeReference archetypeReference in excludedArchetypes)
					{
						if (archetypeReference != null)
						{
							bp.AddComponent(delegate(PrerequisiteNoArchetype c)
							{
								c.Group = Prerequisite.GroupType.All;
								c.m_CharacterClass = data.characterClassReference;
								c.m_Archetype = archetypeReference;
							});
						}
					}
				}
			});
			if (blueprintFeatureReplaceSpellbook != null)
			{
				features.Add(blueprintFeatureReplaceSpellbook.ToReference<BlueprintFeatureReference>());
			}
		}
		BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "EpicPrestigeReplaceSpellbookNone", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "No Spellbook");
			bp.SetDescription(Main.IsekaiContext, "You have no Isekai spellbook to advance, so these levels advance none. This is what an archetype whose spellbook is switched off in the mod settings takes.");
			bp.Groups = new FeatureGroup[0];
			bp.HideNotAvailibleInUI = true;
			bp.Ranks = 20;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(PrerequisiteHasNoClassSpellbook c)
			{
				c.Group = Prerequisite.GroupType.All;
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
			});
		});
		if (blueprintFeature != null)
		{
			features.Add(blueprintFeature.ToReference<BlueprintFeatureReference>());
		}
		Helpers.CreateBlueprint(Main.IsekaiContext, "EpicPrestigeSpellbookSelection", delegate(BlueprintFeatureSelection bp)
		{
			bp.SetName(Main.IsekaiContext, "Spellbook Progression");
			bp.SetDescription(Main.IsekaiContext, "Choose which of your Isekai Protagonist spellbooks this prestige class advances. Only the book your archetype actually grants is available.");
			bp.Group = FeatureGroup.ReplaceSpellbook;
			bp.Groups = new FeatureGroup[0];
			bp.IsClassFeature = true;
			bp.HideNotAvailibleInUI = true;
			bp.m_AllFeatures = features.ToArray();
			bp.m_Features = features.ToArray();
		});
	}

	public static void Patch()
	{
		CreateIsekaiPrestigeSpellbookSelection();
		PrestigeSpellbookData[] obj = new PrestigeSpellbookData[8]
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
		ReplaceSpellbookData[] array = IsekaiSpellbookChoices();
		PrestigeSpellbookData[] array2 = obj;
		foreach (PrestigeSpellbookData prestigeSpellbookData in array2)
		{
			ReplaceSpellbookData[] array3 = array;
			for (int j = 0; j < array3.Length; j++)
			{
				AddPrestigeSpellbook(array3[j], prestigeSpellbookData);
			}
		}
	}

	internal static string MysticTheurgeCounterpartName(string prestigeName, string bookName)
	{
		if (prestigeName == "MysticTheurgeArcane")
		{
			return "MysticTheurgeDivine" + bookName;
		}
		if (prestigeName == "MysticTheurgeDivine")
		{
			return "MysticTheurgeArcane" + bookName;
		}
		return null;
	}

	internal static bool IsOfferedIn(string prestigeName, bool bookIsDivine)
	{
		if (prestigeName == "MysticTheurgeDivine")
		{
			return bookIsDivine;
		}
		return !bookIsDivine;
	}

	private static void AddPrestigeSpellbook(ReplaceSpellbookData replaceSpellbookData, PrestigeSpellbookData prestigeSpellbookData)
	{
		if (replaceSpellbookData.characterClassReference == null || replaceSpellbookData.spellbookReference == null || prestigeSpellbookData.selection == null)
		{
			return;
		}
		string name = prestigeSpellbookData.name + replaceSpellbookData.name;
		string counterpartName = MysticTheurgeCounterpartName(prestigeSpellbookData.name, replaceSpellbookData.name);
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
			if (counterpartName != null)
			{
				bp.AddComponent(delegate(PrerequisiteNoFeature c)
				{
					c.Group = Prerequisite.GroupType.All;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, counterpartName);
				});
			}
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
		if (blueprintFeatureReplaceSpellbook != null && IsOfferedIn(prestigeSpellbookData.name, replaceSpellbookData.isDivine))
		{
			prestigeSpellbookData.selection.AddToSelection(blueprintFeatureReplaceSpellbook);
		}
	}
}
