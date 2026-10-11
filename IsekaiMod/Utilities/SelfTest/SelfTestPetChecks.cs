using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using IsekaiMod.Content.Guardians;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Root;
using Kingmaker.DLC;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UI.MVVM._VM.CharGen;
using Kingmaker.UI.MVVM._VM.CharGen.Phases;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.Class;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestPetChecks
{
	private const int PetLevelCap = 20;

	internal static SelfTestChecks.Outcome PetLevelUp()
	{
		Dictionary<BlueprintFeature, AddPet> dictionary = ModPetFeatures();
		if (dictionary.Count == 0)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The mod registered no feature with an AddPet component, so no pet could be levelled.");
		}
		List<string> list = new List<string>();
		int failures = 0;
		int num = 0;
		Stopwatch stopwatch = Stopwatch.StartNew();
		foreach (KeyValuePair<BlueprintFeature, AddPet> item in dictionary)
		{
			BlueprintFeature key = item.Key;
			AddPet value = item.Value;
			string name = key.name;
			BlueprintUnit pet = value.Pet;
			if (pet == null)
			{
				failures++;
				list.Add(name + ": its AddPet names no pet unit, so the feature grants nothing.");
				continue;
			}
			AddClassLevels component = pet.GetComponent<AddClassLevels>();
			BlueprintCharacterClass blueprintCharacterClass = component?.CharacterClass;
			if (blueprintCharacterClass == null)
			{
				failures++;
				list.Add(name + ": the pet unit '" + pet.name + "' has no AddClassLevels naming a class, so the level-up window has no class to offer and cannot be finished.");
				continue;
			}
			BlueprintArchetype archetype = component.Archetypes.FirstOrDefault();
			if (!GuardianPetLevel.IsRegistered(key))
			{
				failures++;
				list.Add(name + ": it is not registered with GuardianPetLevel, so TabletopTweaks-Base's uncapped pet-level postfix still applies to it and its level will not follow its master's.");
			}
			num += LevelOnePet(name, pet, blueprintCharacterClass, archetype, value.Type, list, ref failures);
		}
		stopwatch.Stop();
		string summary = $"{dictionary.Count} mod pets, each levelled 1-{20} through the engine's own level-up" + " with the class page the player would see checked at every level" + $"; {num} levels applied in {stopwatch.Elapsed.TotalSeconds:0} s; {failures} problems.";
		return SelfTestChecks.Outcome.Of(failures == 0, summary, list);
	}

	private static int LevelOnePet(string label, BlueprintUnit petBlueprint, BlueprintCharacterClass petClass, BlueprintArchetype archetype, PetType petType, List<string> details, ref int failures)
	{
		string problem;
		using SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem);
		if (selfTestUnit == null)
		{
			failures++;
			details.Add(label + ": " + problem + ", so no master could be made for the pet.");
			return 0;
		}
		UnitEntityData unitEntityData = null;
		try
		{
			using (ContextData<UnitEntityData.ChargenUnit>.Request())
			{
				unitEntityData = Game.Instance.CreateUnitVacuum(petBlueprint);
			}
		}
		catch (Exception ex)
		{
			failures++;
			details.Add(label + ": the pet unit '" + petBlueprint.name + "' could not be created (" + ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message) + ").");
			return 0;
		}
		if (unitEntityData == null)
		{
			failures++;
			details.Add(label + ": the pet unit '" + petBlueprint.name + "' could not be created.");
			return 0;
		}
		try
		{
			selfTestUnit.Unit.Ensure<UnitPartPetMaster>().AddPet(unitEntityData, petType);
			if (!unitEntityData.IsPet)
			{
				failures++;
				details.Add(label + ": the pet could not be attached to a master, so the class page cannot be asked about.");
				return 0;
			}
			int num = 0;
			for (int i = 1; i <= 20; i++)
			{
				int num2 = 0;
				try
				{
					num2 = unitEntityData.Descriptor.Progression.CharacterLevel;
				}
				catch
				{
				}
				if (num2 < i)
				{
					List<string> list = new List<string>();
					bool num3 = TakePetLevel(unitEntityData, petClass, archetype, i, label, list);
					failures += list.Count;
					details.AddRange(list);
					if (!num3)
					{
						break;
					}
					num++;
				}
			}
			return num;
		}
		finally
		{
			try
			{
				unitEntityData.Dispose();
			}
			catch
			{
			}
		}
	}

	private static bool TakePetLevel(UnitEntityData pet, BlueprintCharacterClass petClass, BlueprintArchetype archetype, int level, string label, List<string> problems)
	{
		if (level > 1)
		{
			using (ContextData<AddClassLevels.LevelUpControllerStash>.Request().Stash())
			{
				string text = WindowProblem(pet);
				if (text != null)
				{
					problems.Add($"{label}: at pet level {level} {text}");
					return false;
				}
			}
		}
		using (ContextData<AddClassLevels.ExecutionMark>.Request().Setup(pet))
		{
			using (ContextData<AddClassLevels.LevelUpControllerStash>.Request().Stash())
			{
				LevelUpController levelUpController;
				try
				{
					levelUpController = LevelUpController.Start(pet, LevelUpState.CharBuildMode.LevelUp, instantCommit: true, ignoreSettings: true);
				}
				catch (Exception ex)
				{
					problems.Add($"{label}: opening the level {level} level-up threw {ex.GetType().Name}: {SelfTestReport.Clean(ex.Message)}");
					return false;
				}
				try
				{
					string text2 = ClassListProblem(levelUpController, petClass);
					if (text2 != null)
					{
						problems.Add($"{label}: at pet level {level} {text2}");
						return false;
					}
					if (archetype != null && pet.Descriptor.Progression.GetClassLevel(petClass) <= 0)
					{
						levelUpController.AddArchetype(petClass, archetype);
					}
					using (ContextData<IgnorePrerequisites>.Request())
					{
						if (!levelUpController.SelectClass(petClass, applyMechanics: false))
						{
							problems.Add($"{label}: pet level {level} could not select '{petClass.name}', so the level was never applied.");
							return false;
						}
					}
					levelUpController.ApplyClassMechanics();
					SelfTestProgressionChecks.FillSelections(levelUpController, level, label, problems);
					levelUpController.ApplySpellbook();
					levelUpController.ApplySkillPoints();
					SpendPoints(levelUpController);
					SelfTestProgressionChecks.FillSelections(levelUpController, level, label, problems);
				}
				catch (Exception ex2)
				{
					problems.Add($"{label}: pet level {level} threw {ex2.GetType().Name}: {SelfTestReport.Clean(ex2.Message)}");
					return false;
				}
				finally
				{
					try
					{
						if (!levelUpController.Stopped)
						{
							levelUpController.Stop();
						}
					}
					catch
					{
					}
				}
			}
		}
		int num = 0;
		try
		{
			num = pet.Descriptor.Progression.CharacterLevel;
		}
		catch
		{
		}
		if (num < level)
		{
			problems.Add($"{label}: the pet is level {num} after asking for level {level}, so the level-up did not apply.");
			return false;
		}
		return true;
	}

	private static void SpendPoints(LevelUpController controller)
	{
		int num = 0;
		while (controller.State.SkillPointsRemaining > 0 && num++ < 100)
		{
			int skillPointsRemaining = controller.State.SkillPointsRemaining;
			controller.SpendSkillPoint(StatType.SkillPerception);
			if (controller.State.SkillPointsRemaining == skillPointsRemaining)
			{
				break;
			}
		}
		num = 0;
		while (controller.State.AttributePoints > 0 && num++ < 100)
		{
			int attributePoints = controller.State.AttributePoints;
			controller.SpendAttributePoint(StatType.Strength);
			if (controller.State.AttributePoints == attributePoints)
			{
				break;
			}
		}
	}

	private static string ClassListProblem(LevelUpController controller, BlueprintCharacterClass petClass)
	{
		ProgressionRoot progressionRoot = Game.Instance?.BlueprintRoot?.Progression;
		if (progressionRoot == null)
		{
			return "the game has no progression root, so the class page could not be asked about.";
		}
		List<BlueprintCharacterClass> list = progressionRoot.AvailablePetClasses.Where((BlueprintCharacterClass c) => c != null).ToList();
		if (!list.Contains(petClass))
		{
			return "the class page does not list '" + petClass.name + "' at all: it is not in BlueprintRoot.Progression.PetClasses, which is the only list a pet's class page is built from. The page is empty, no class can be picked, and the level-up can never be finished.";
		}
		List<BlueprintCharacterClass> list2 = new List<BlueprintCharacterClass>();
		List<BlueprintCharacterClass> list3 = new List<BlueprintCharacterClass>();
		foreach (BlueprintCharacterClass item in list)
		{
			bool flag;
			try
			{
				flag = item.MeetsPrerequisites(controller.Unit, controller.State);
			}
			catch
			{
				flag = false;
			}
			if (!item.IsDlcRestricted() && (flag || !item.HideIfRestricted) && !item.HideInUI)
			{
				list2.Add(item);
				bool flag2;
				try
				{
					flag2 = CharGenClassPhaseVM.IsClassAvailable(controller, item);
				}
				catch
				{
					flag2 = false;
				}
				if (flag2)
				{
					list3.Add(item);
				}
			}
		}
		if (list2.Count == 0)
		{
			return "the class page is drawn with no class on it at all, so no class can be picked and the level-up can never be finished.";
		}
		if (!list2.Contains(petClass))
		{
			return "the class page does not draw '" + petClass.name + "': the pet's own class is filtered out " + $"(prerequisites met: {petClass.MeetsPrerequisites(controller.Unit, controller.State)}, " + $"HideIfRestricted: {petClass.HideIfRestricted}, HideInUI: {petClass.HideInUI}).";
		}
		if (!list3.Contains(petClass))
		{
			return "the class page draws '" + petClass.name + "' greyed out: CharGenClassPhaseVM.IsClassAvailable refuses it, so it cannot be picked and the level-up can never be finished.";
		}
		return null;
	}

	private static string WindowProblem(UnitEntityData pet)
	{
		CharGenVM charGenVM = null;
		LevelUpController levelUpController = null;
		try
		{
			LevelUpConfig config = LevelUpConfig.Create(pet);
			levelUpController = LevelUpController.Start(pet, LevelUpState.CharBuildMode.LevelUp, instantCommit: false, ignoreSettings: true);
			charGenVM = new CharGenVM(levelUpController, delegate
			{
			}, delegate
			{
			}, config);
		}
		catch (Exception ex)
		{
			try
			{
				if (levelUpController != null && !levelUpController.Stopped)
				{
					levelUpController.Stop();
				}
			}
			catch
			{
			}
			return "building the level-up window threw " + ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message) + ". The window is left with nothing on it and the level-up can never be finished.";
		}
		try
		{
			if (levelUpController.State.Mode != LevelUpState.CharBuildMode.LevelUp)
			{
				return null;
			}
			List<CharGenPhaseBaseVM> list = charGenVM.PhasesCollection?.ToList();
			if (list == null || list.Count == 0)
			{
				return "the level-up window is drawn with no page on it at all, so nothing can be picked and the level-up can never be finished.";
			}
			if (!list.Any((CharGenPhaseBaseVM p) => p is CharGenClassPhaseVM))
			{
				return "the level-up window has no class page, although CharGenVM.NeedClassPhase demands one for every level-up; the pages behind it can never become available. It drew: " + string.Join(", ", list.Select((CharGenPhaseBaseVM p) => (p != null) ? p.GetType().Name : "(null)")) + ".";
			}
			return null;
		}
		catch (Exception ex2)
		{
			return "reading the level-up window's pages threw " + ex2.GetType().Name + ": " + SelfTestReport.Clean(ex2.Message);
		}
		finally
		{
			try
			{
				charGenVM.Dispose();
			}
			catch
			{
			}
			try
			{
				if (!levelUpController.Stopped)
				{
					levelUpController.Stop();
				}
			}
			catch
			{
			}
		}
	}

	internal static Dictionary<BlueprintFeature, AddPet> ModPetFeatures()
	{
		Dictionary<BlueprintFeature, AddPet> dictionary = new Dictionary<BlueprintFeature, AddPet>();
		foreach (BlueprintFeature item in SelfTestProgressionChecks.ModBlueprintsOf<BlueprintFeature>().OrderBy((BlueprintFeature f) => f.name, StringComparer.Ordinal))
		{
			AddPet addPet = null;
			try
			{
				addPet = item.GetComponent<AddPet>();
			}
			catch
			{
			}
			if (addPet != null && !dictionary.ContainsKey(item))
			{
				dictionary[item] = addPet;
			}
		}
		return dictionary;
	}
}
