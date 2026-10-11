using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Components;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Prestige;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities;

public static class RegressionSanityCheck
{
	private static readonly string[] SpellbookAdvancingPrestigeClasses = new string[7] { "TranscendentSovereignClass", "DemigodPrestigeClass", "GodhunterPrestigeClass", "WarmasterPrestigeClass", "EternalSeekerPrestigeClass", "ArchmagePrestigeClass", "ShadowSovereignPrestigeClass" };

	private static readonly string[] ExtraClassNames = new string[2] { "GuardianCompanionClass", "DeathsnatcherClass" };

	private static int DroppedLevelFeatureSlots;

	private static readonly List<KeyValuePair<string, string>> StepFailures = new List<KeyValuePair<string, string>>();

	public static void Run()
	{
		int num = 0;
		string[] array = new string[10] { "GuardianAngelCompanionUnit", "ShinigamiCompanionUnit", "LoyalDemonCompanionUnit", "AstralDevourerCompanionUnit", "TempestStarWolfCompanionUnit", "ShadowMarshallCompanionUnit", "OverlordGuardianCompanionUnit", "DivineHeraldCompanionUnit", "EnigmaticCoConspiratorCompanionUnit", "ManifestedMartialSpiritCompanionUnit" };
		foreach (string text in array)
		{
			BlueprintUnit modBlueprint = BlueprintTools.GetModBlueprint<BlueprintUnit>(Main.IsekaiContext, text);
			if (modBlueprint != null && modBlueprint.Size != Size.Medium)
			{
				Main.IsekaiContext.Logger.LogError($"[REGRESSION ASSERTION FAILED] Guardian '{text}' has Size.{modBlueprint.Size} but MUST be Size.Medium!");
				num++;
			}
		}
		BlueprintUnit modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintUnit>(Main.IsekaiContext, "ChronoSpriteCompanionUnit");
		if (modBlueprint2 != null && modBlueprint2.Size != Size.Diminutive)
		{
			Main.IsekaiContext.Logger.LogError($"[REGRESSION ASSERTION FAILED] ChronoSpriteCompanionUnit has Size.{modBlueprint2.Size} but MUST be Size.Diminutive!");
			num++;
		}
		BlueprintFeatureSelection modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiHumanHeritageSelection");
		if (modBlueprint3 != null && modBlueprint3.Group != FeatureGroup.Racial)
		{
			Main.IsekaiContext.Logger.LogError($"[REGRESSION ASSERTION FAILED] IsekaiHumanHeritageSelection.Group is {modBlueprint3.Group} but MUST be FeatureGroup.Racial!");
			num++;
		}
		BlueprintFeature modBlueprint4 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeteromorphicOverlordHeritage");
		if (modBlueprint4 != null && !modBlueprint4.GetComponents<PrerequisiteIsMainCharacter>().Any())
		{
			Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] HeteromorphicOverlordHeritage is missing PrerequisiteIsMainCharacter!");
			num++;
		}
		BlueprintFeature modBlueprint5 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SlimeReincarnateHeritage");
		if (modBlueprint5 != null && !modBlueprint5.GetComponents<PrerequisiteIsMainCharacter>().Any())
		{
			Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] SlimeReincarnateHeritage is missing PrerequisiteIsMainCharacter!");
			num++;
		}
		BlueprintCharacterClass modBlueprint6 = BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(Main.IsekaiContext, "IsekaiProtagonistClass");
		if (modBlueprint6 != null && !modBlueprint6.GetComponents<PrerequisiteIsMainCharacter>().Any())
		{
			Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] IsekaiProtagonistClass is missing PrerequisiteIsMainCharacter!");
			num++;
		}
		if (modBlueprint6 != null)
		{
			BlueprintArchetypeReference[] array2 = modBlueprint6.m_Archetypes ?? new BlueprintArchetypeReference[0];
			for (int i = 0; i < array2.Length; i++)
			{
				BlueprintArchetype blueprintArchetype = array2[i]?.Get();
				if (blueprintArchetype != null && !blueprintArchetype.GetComponents<PrerequisiteIsMainCharacter>().Any())
				{
					Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] " + blueprintArchetype.name + " is missing PrerequisiteIsMainCharacter!");
					num++;
				}
			}
		}
		BlueprintCharacterClass modBlueprint7 = BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(Main.IsekaiContext, "OtherworldRetinueClass");
		PrerequisiteIsMainCharacter prerequisiteIsMainCharacter = modBlueprint7?.GetComponents<PrerequisiteIsMainCharacter>().FirstOrDefault();
		if (modBlueprint7 != null && (prerequisiteIsMainCharacter == null || !prerequisiteIsMainCharacter.CompanionOnly))
		{
			Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] OtherworldRetinueClass is missing its companion-only PrerequisiteIsMainCharacter!");
			num++;
		}
		BlueprintArchetype modBlueprint8 = BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "DevourerArchetype");
		BlueprintArchetype modBlueprint9 = BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "OverlordArchetype");
		var array3 = new[]
		{
			new
			{
				Heritage = modBlueprint4,
				Name = "HeteromorphicOverlordHeritage",
				Archetype = "DevourerArchetype",
				Blueprint = modBlueprint8
			},
			new
			{
				Heritage = modBlueprint5,
				Name = "SlimeReincarnateHeritage",
				Archetype = "OverlordArchetype",
				Blueprint = modBlueprint9
			}
		};
		foreach (var anon in array3)
		{
			if (anon.Heritage != null)
			{
				BlueprintArchetype archetype = anon.Blueprint;
				if (archetype != null && !anon.Heritage.GetComponents<PrerequisiteNoArchetype>().Any((PrerequisiteNoArchetype p) => p.Archetype == archetype))
				{
					Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] " + anon.Name + " has no PrerequisiteNoArchetype for " + anon.Archetype + "; that archetype can take it in character creation!");
					num++;
				}
			}
		}
		string[][] array4 = ArchetypeHeritageTraits.BlueprintNames();
		foreach (string[] array5 in array4)
		{
			BlueprintFeature traits = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, array5[1]);
			if (traits != null && traits.Groups != null && traits.Groups.Contains(FeatureGroup.Racial))
			{
				Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] '" + array5[1] + "' is in FeatureGroup.Racial; an archetype granting it would give the character a second racial heritage!");
				num++;
			}
			BlueprintFeature modBlueprint10 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, array5[0]);
			BlueprintFeature heritage = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, array5[2]);
			if (modBlueprint10 != null && traits != null && heritage != null && !(modBlueprint10.GetComponents<AddFeatureIfHasFact>().Any((AddFeatureIfHasFact c) => c.Not && c.m_CheckedFact != null && c.m_CheckedFact.Guid == heritage.AssetGuid && c.m_Feature != null && c.m_Feature.Guid == traits.AssetGuid) | modBlueprint10.GetComponents<AddFeatureUnlessHasFact>().Any((AddFeatureUnlessHasFact c) => c.m_CheckedFact != null && c.m_CheckedFact.Guid == heritage.AssetGuid && c.m_Feature != null && c.m_Feature.Guid == traits.AssetGuid)))
			{
				Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] '" + array5[0] + "' does not skip '" + array5[1] + "' for a character who already has '" + array5[2] + "', so the bonuses stack.");
				num++;
			}
		}
		num += CheckArchetypesDoNotGrantHeritages();
		BlueprintFeature vampireHeritage = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiVampireHeritage");
		if (modBlueprint3 != null && vampireHeritage != null && modBlueprint3.m_AllFeatures.Any((BlueprintFeatureReference f) => f.Guid == vampireHeritage.AssetGuid))
		{
			Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] IsekaiVampireHeritage was leaked into HumanHeritageSelection!");
			num++;
		}
		ProgressionRoot progressionRoot = BlueprintRoot.Instance?.Progression;
		if (progressionRoot != null)
		{
			foreach (BlueprintCharacterClass registeredClass in TTCoreExtensions.RegisteredClasses)
			{
				if (registeredClass != null && !progressionRoot.CharacterClasses.Contains(registeredClass))
				{
					Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] Class '" + registeredClass.name + "' was registered but is missing from BlueprintRoot.Progression.CharacterClasses; it cannot be selected at level-up!");
					num++;
				}
			}
		}
		bool[] array6 = new bool[2] { true, false };
		foreach (bool flag in array6)
		{
			string text2 = CharGenSafetyPatch.PreviewPolymorphBuffGuid(flag);
			Polymorph polymorph = BlueprintTools.GetBlueprint<BlueprintBuff>(text2)?.GetComponent<Polymorph>();
			if (polymorph == null)
			{
				Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] " + (flag ? "Overlord" : "Slime") + " form polymorph buff " + text2 + " is missing or has no Polymorph component!");
				num++;
			}
			else if (!flag && polymorph.Size != Size.Small)
			{
				Main.IsekaiContext.Logger.LogError($"[REGRESSION ASSERTION FAILED] Slime form polymorph buff {text2} is {polymorph.Size}; the slime form is Small.");
				num++;
			}
		}
		num += CheckProgressionEntries();
		BlueprintFeatureSelection extraSpecialPower = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "ExtraSpecialPowerSelection");
		if (extraSpecialPower == null)
		{
			Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] ExtraSpecialPowerSelection was never created; MartialGodArchetype levels 1 and 3 are empty!");
			num++;
		}
		else
		{
			BlueprintFeatureSelection modBlueprint11 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "SecretPowerSelection");
			if (modBlueprint11 != null && !modBlueprint11.m_AllFeatures.Any((BlueprintFeatureReference f) => f != null && f.Guid == extraSpecialPower.AssetGuid))
			{
				Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] SecretPowerSelection does not offer ExtraSpecialPowerSelection; it was created before SecretPowerSelection existed!");
				num++;
			}
		}
		num += CheckPrestigeSpellbooks();
		num += CheckCosmicWishResource();
		num += DroppedLevelFeatureSlots;
		if (num != 0)
		{
			Main.IsekaiContext.Logger.LogError($"[REGRESSION ASSERTION SUMMARY] Detected {num} regression invariant violations during initialization!");
		}
	}

	private static int CheckCosmicWishResource()
	{
		BlueprintAbilityResource modBlueprint = BlueprintTools.GetModBlueprint<BlueprintAbilityResource>(Main.IsekaiContext, "CosmicWishResource");
		if (modBlueprint == null)
		{
			Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] CosmicWishResource does not exist, so the Cosmic Wish ability has no charges at all.");
			return 1;
		}
		if (modBlueprint.m_MaxAmount.BaseValue <= 0)
		{
			Main.IsekaiContext.Logger.LogError($"[REGRESSION ASSERTION FAILED] CosmicWishResource holds at most {modBlueprint.m_MaxAmount.BaseValue} charges; the 1,000 coin recharge restores into a zero-capacity pool and the wish can never be cast.");
			return 1;
		}
		return 0;
	}

	private static int CheckArchetypesDoNotGrantHeritages()
	{
		int num = 0;
		BlueprintCharacterClass modBlueprint = BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(Main.IsekaiContext, "IsekaiProtagonistClass");
		if (modBlueprint == null)
		{
			return num;
		}
		BlueprintArchetypeReference[] array = modBlueprint.m_Archetypes ?? new BlueprintArchetypeReference[0];
		for (int i = 0; i < array.Length; i++)
		{
			BlueprintArchetype blueprintArchetype = array[i]?.Get();
			if (blueprintArchetype?.AddFeatures == null)
			{
				continue;
			}
			LevelEntry[] addFeatures = blueprintArchetype.AddFeatures;
			foreach (LevelEntry levelEntry in addFeatures)
			{
				if (levelEntry?.m_Features == null)
				{
					continue;
				}
				foreach (BlueprintFeatureBaseReference feature in levelEntry.m_Features)
				{
					BlueprintFeature blueprintFeature = feature?.Get() as BlueprintFeature;
					if (blueprintFeature?.Groups != null && blueprintFeature.Groups.Contains(FeatureGroup.Racial))
					{
						Main.IsekaiContext.Logger.LogError($"[REGRESSION ASSERTION FAILED] Archetype '{blueprintArchetype.name}' grants the racial heritage '{blueprintFeature.name}' at level {levelEntry.Level}; a character who chose another heritage now has two.");
						num++;
					}
				}
			}
		}
		return num;
	}

	private static int CheckPrestigeSpellbooks()
	{
		int num = 0;
		BlueprintFeatureSelection selection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "EpicPrestigeSpellbookSelection");
		if (selection == null)
		{
			Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] The prestige spellbook selection was never created; the mod's prestige classes advance no spellbook at all!");
			return num + 1;
		}
		if (selection.m_AllFeatures == null || selection.m_AllFeatures.Length == 0)
		{
			Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] The prestige spellbook selection is empty; no Isekai spellbook can be advanced by a prestige class!");
			num++;
		}
		string[] spellbookAdvancingPrestigeClasses = SpellbookAdvancingPrestigeClasses;
		foreach (string text in spellbookAdvancingPrestigeClasses)
		{
			BlueprintCharacterClass modBlueprint = BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(Main.IsekaiContext, text);
			if (modBlueprint != null)
			{
				if (modBlueprint.m_Spellbook != null && modBlueprint.m_Spellbook.Guid != BlueprintGuid.Empty)
				{
					Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] Prestige class '" + text + "' still sets m_Spellbook; an archetype caster taking it gets a second spellbook at caster level 1 instead of advancing their own.");
					num++;
				}
				LevelEntry levelEntry = modBlueprint.m_Progression?.Get()?.LevelEntries?.FirstOrDefault((LevelEntry e) => e != null && e.Level == 1);
				if (levelEntry?.m_Features == null || !levelEntry.m_Features.Any((BlueprintFeatureBaseReference f) => f != null && f.Guid == selection.AssetGuid))
				{
					Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] Prestige class '" + text + "' does not offer the spellbook selection at level 1, so its levels advance no spellbook.");
					num++;
				}
			}
		}
		num += CheckMysticTheurgeExclusivity();
		return num + CheckRetinueArchetypeSpellbooks();
	}

	private static int CheckRetinueArchetypeSpellbooks()
	{
		int num = 0;
		BlueprintFeatureSelection modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "RetinueSpellbookSelection");
		string[][] array = OtherworldRetinuePrestige.ArchetypeReplacedBookIds();
		foreach (string[] book in array)
		{
			BlueprintFeature entry = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, book[0]);
			if (entry == null)
			{
				Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] Otherworld Retinue is missing the archetype spellbook entry '" + book[0] + "'; that companion gets a new base spellbook instead of advancing their own.");
				num++;
				continue;
			}
			if (!entry.GetComponents<PrerequisiteArchetypeLevel>().Any((PrerequisiteArchetypeLevel c) => c.m_Archetype != null && c.m_Archetype.Guid.ToString() == book[2]))
			{
				Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] Retinue spellbook entry '" + book[0] + "' is not gated on its archetype, so any companion could pick it.");
				num++;
			}
			if (modBlueprint != null && !modBlueprint.m_AllFeatures.Any((BlueprintFeatureReference f) => f != null && f.Guid == entry.AssetGuid))
			{
				Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] Retinue spellbook entry '" + book[0] + "' is not in RetinueSpellbookSelection, so it can never be chosen.");
				num++;
			}
		}
		return num;
	}

	private static int CheckMysticTheurgeExclusivity()
	{
		int num = 0;
		string[] array = new string[5] { "Isekai", "GodEmperor", "Mastermind", "Overlord", "ShadowMonarch" };
		foreach (string text in array)
		{
			string[] array2 = new string[2] { "MysticTheurgeArcane", "MysticTheurgeDivine" };
			foreach (string text2 in array2)
			{
				BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, text2 + text);
				if (modBlueprint != null)
				{
					string text3 = PrestigeClassReplaceSpellbook.MysticTheurgeCounterpartName(text2, text);
					BlueprintFeature counterpart = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, text3);
					if (counterpart != null && !modBlueprint.GetComponents<PrerequisiteNoFeature>().Any((PrerequisiteNoFeature c) => c.m_Feature != null && c.m_Feature.Guid == counterpart.AssetGuid))
					{
						Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] Mystic Theurge entry '" + text2 + text + "' does not exclude '" + text3 + "', so the same Isekai spellbook can be taken in both slots!");
						num++;
					}
				}
			}
		}
		return num;
	}

	private static int CheckProgressionEntries()
	{
		int num = 0;
		foreach (BlueprintCharacterClass item in ClassesToWalk())
		{
			BlueprintProgression blueprintProgression = item.m_Progression?.Get();
			if (blueprintProgression == null)
			{
				Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] Class '" + item.name + "' has no resolvable progression!");
				num++;
			}
			else
			{
				num += CheckLevelEntries(item.name + " -> " + blueprintProgression.name + ".LevelEntries", blueprintProgression.LevelEntries);
			}
			BlueprintArchetypeReference[] array = item.m_Archetypes ?? new BlueprintArchetypeReference[0];
			foreach (BlueprintArchetypeReference blueprintArchetypeReference in array)
			{
				BlueprintArchetype blueprintArchetype = blueprintArchetypeReference?.Get();
				if (blueprintArchetype == null)
				{
					Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] Class '" + item.name + "' has an archetype reference that does not resolve (guid '" + (blueprintArchetypeReference?.Guid.ToString() ?? "null") + "')!");
					num++;
				}
				else
				{
					num += CheckLevelEntries(item.name + " -> " + blueprintArchetype.name + ".AddFeatures", blueprintArchetype.AddFeatures);
					num += CheckLevelEntries(item.name + " -> " + blueprintArchetype.name + ".RemoveFeatures", blueprintArchetype.RemoveFeatures);
				}
			}
		}
		return num;
	}

	private static IEnumerable<BlueprintCharacterClass> ClassesToWalk()
	{
		HashSet<string> seen = new HashSet<string>();
		foreach (BlueprintCharacterClass registeredClass in TTCoreExtensions.RegisteredClasses)
		{
			if (registeredClass != null && seen.Add(registeredClass.name))
			{
				yield return registeredClass;
			}
		}
		string[] extraClassNames = ExtraClassNames;
		foreach (string name in extraClassNames)
		{
			BlueprintCharacterClass modBlueprint = BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(Main.IsekaiContext, name);
			if (modBlueprint != null && seen.Add(modBlueprint.name))
			{
				yield return modBlueprint;
			}
		}
	}

	private static int CheckLevelEntries(string what, LevelEntry[] entries)
	{
		if (entries == null)
		{
			return 0;
		}
		int num = 0;
		foreach (LevelEntry levelEntry in entries)
		{
			if (levelEntry?.m_Features == null)
			{
				continue;
			}
			for (int j = 0; j < levelEntry.m_Features.Count; j++)
			{
				BlueprintFeatureBaseReference blueprintFeatureBaseReference = levelEntry.m_Features[j];
				string text = ClassifyEntryReference(blueprintFeatureBaseReference == null, blueprintFeatureBaseReference != null && blueprintFeatureBaseReference.Guid == BlueprintGuid.Empty, blueprintFeatureBaseReference != null && blueprintFeatureBaseReference.Get() != null);
				if (text != null)
				{
					Main.IsekaiContext.Logger.LogError(string.Format("[REGRESSION ASSERTION FAILED] {0} level {1} slot {2}: {3} (guid '{4}'). The engine skips it silently, so the feature is simply never granted.", what, levelEntry.Level, j, text, blueprintFeatureBaseReference?.Guid.ToString() ?? "null"));
					num++;
					if (Main.IsekaiContext.Debug)
					{
						throw new InvalidOperationException($"{what} level {levelEntry.Level} slot {j}: {text}");
					}
				}
			}
		}
		return num;
	}

	internal static List<int> MissingFeatureSlots(IEnumerable<object> features)
	{
		List<int> list = new List<int>();
		if (features == null)
		{
			return list;
		}
		int num = 0;
		foreach (object feature in features)
		{
			if (feature == null)
			{
				list.Add(num);
			}
			num++;
		}
		return list;
	}

	public static int ReportMissingLevelFeatures(string what, int level, IEnumerable<object> features)
	{
		List<int> list = MissingFeatureSlots(features);
		foreach (int item in list)
		{
			Main.IsekaiContext.Logger.LogError($"[REGRESSION ASSERTION FAILED] {what} level {level} slot {item}: the feature did not exist when the level entry was built, so it was dropped and is never granted.");
		}
		DroppedLevelFeatureSlots += list.Count;
		return list.Count;
	}

	internal static string ClassifyEntryReference(bool referenceIsNull, bool guidIsEmpty, bool blueprintResolves)
	{
		if (referenceIsNull)
		{
			return "null reference in the entry";
		}
		if (guidIsEmpty)
		{
			return "empty GUID, so the blueprint did not exist when the entry was built";
		}
		if (!blueprintResolves)
		{
			return "GUID does not resolve to a loaded blueprint";
		}
		return null;
	}

	public static void RunAfterFinalPatch()
	{
		int num = 0;
		BlueprintSpellList modBlueprint = BlueprintTools.GetModBlueprint<BlueprintSpellList>(Main.IsekaiContext, "MastermindSpellList");
		BlueprintSpellList modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintSpellList>(Main.IsekaiContext, "IsekaiProtagonistSpellList");
		if (modBlueprint != null)
		{
			if (modBlueprint.SpellsByLevel == null || modBlueprint.SpellsByLevel.Length < 11)
			{
				Main.IsekaiContext.Logger.LogError("[REGRESSION ASSERTION FAILED] MastermindSpellList has " + ((modBlueprint.SpellsByLevel == null) ? "no" : modBlueprint.SpellsByLevel.Length.ToString()) + " spell levels; it needs 0 through 10.");
				num++;
			}
			else if (modBlueprint2?.SpellsByLevel != null)
			{
				for (int i = 0; i < modBlueprint2.SpellsByLevel.Length && i < modBlueprint.SpellsByLevel.Length; i++)
				{
					if (modBlueprint.SpellsByLevel[i] == modBlueprint2.SpellsByLevel[i])
					{
						Main.IsekaiContext.Logger.LogError($"[REGRESSION ASSERTION FAILED] MastermindSpellList level {i} is the same SpellLevelList object as the Isekai list; editing one edits both.");
						num++;
						break;
					}
				}
			}
		}
		num += BlueprintIdentifierFallback.UnregisteredNames.Count;
		BlueprintIdentifierFallback.LogSummary();
		if (num != 0)
		{
			Main.IsekaiContext.Logger.LogError($"[REGRESSION ASSERTION SUMMARY] Detected {num} post-patch regression invariant violations!");
		}
	}

	public static void RecordStepFailure(string phase, string stepName)
	{
		StepFailures.Add(new KeyValuePair<string, string>(phase ?? "?", stepName ?? "?"));
	}

	public static void LogStepFailureSummary(string phase)
	{
		string[] array = (from f in StepFailures
			where f.Key == phase
			select f.Value).ToArray();
		if (array.Length != 0)
		{
			Main.IsekaiContext.Logger.LogError(string.Format("[{0}] {1} step(s) threw and were swallowed by SafeStep: {2}. Content from those steps is missing.", phase, array.Length, string.Join(", ", array)));
		}
	}
}
