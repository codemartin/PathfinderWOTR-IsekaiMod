using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker;
using Kingmaker.Blueprints.Classes;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestRespecChecks
{
	internal static string TryEngineRespec(UnitEntityData unit, out LevelUpConfig config)
	{
		config = null;
		SelfTestRespecWindowCapture.Captured = null;
		SelfTestRespecWindowCapture.Armed = true;
		try
		{
			unit.Respec(delegate
			{
			});
		}
		catch (Exception ex)
		{
			string text = (from l in (ex.StackTrace ?? "").Split('\n')
				select l.Trim()).FirstOrDefault((string l) => l.Length > 0) ?? "(no frame)";
			return ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message) + " at " + SelfTestReport.Clean(text);
		}
		finally
		{
			SelfTestRespecWindowCapture.Armed = false;
		}
		config = SelfTestRespecWindowCapture.Captured;
		SelfTestRespecWindowCapture.Captured = null;
		if (config == null)
		{
			return "UnitHelper.Respec returned without asking to open the respec window";
		}
		if (config.Mode != LevelUpState.CharBuildMode.Respec)
		{
			return $"the window Respec asked for is in {config.Mode} mode, not Respec";
		}
		return null;
	}

	internal static UnitEntityData RespecUnitLikeTheEngine(UnitEntityData original)
	{
		UnitEntityData unitEntityData;
		using (ContextData<UnitEntityData.DoNotCreateItems>.Request())
		{
			using (ContextData<UnitEntityData.ChargenUnit>.Request())
			{
				unitEntityData = Game.Instance.CreateUnitVacuum(original.Blueprint);
			}
		}
		SelfTestUnit.EnsureArmor(unitEntityData);
		unitEntityData.PreviewOf = original;
		unitEntityData.Progression.AdvanceExperienceTo(original.Descriptor.Progression.Experience, log: false);
		if (unitEntityData.Progression.CharacterLevel < 1)
		{
			foreach (ModifiableValueAttributeStat attribute in unitEntityData.Stats.Attributes)
			{
				attribute.BaseValue = 10;
			}
			unitEntityData.Progression.DropLevelPlans(mythic: false);
			unitEntityData.Progression.DropLevelPlans(mythic: true);
		}
		return unitEntityData;
	}

	internal static BlueprintCharacterClass FirstClassOf(UnitEntityData unit)
	{
		try
		{
			return unit.Descriptor.Progression.ClassesOrder.FirstOrDefault((BlueprintCharacterClass c) => c != null && !c.IsMythic);
		}
		catch
		{
			return null;
		}
	}

	internal static SelfTestChecks.Outcome LiveRespec()
	{
		List<string> list = new List<string>();
		UnitEntityData unitEntityData = Game.Instance?.Player?.MainCharacter.Value;
		if (unitEntityData == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The loaded game has no main character to respec.");
		}
		BlueprintCharacterClass blueprintCharacterClass = FirstClassOf(unitEntityData);
		if (blueprintCharacterClass == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The main character has no class levels, so there is nothing to respec.");
		}
		int characterLevel = unitEntityData.Descriptor.Progression.CharacterLevel;
		int experience = unitEntityData.Descriptor.Progression.Experience;
		string text = unitEntityData.Descriptor.Progression.ExperienceTable?.name ?? "(none)";
		int count = Game.Instance.Player.Party.Count;
		List<string> before = SaveStateFingerprint.Of(SelfTestSaveChecks.ModStorages());
		list.Add($"Before: {unitEntityData.CharacterName}, level {characterLevel} ({experience} XP on {text}), first class {blueprintCharacterClass.name}, party of {count}.");
		string text2 = TryEngineRespec(unitEntityData, out var config);
		if (text2 != null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The engine's UnitHelper.Respec did not reach the respec window in a loaded game: " + text2 + ".", list);
		}
		UnitEntityData unit = config.Unit;
		if (unit?.PreviewOf.Entity != unitEntityData)
		{
			list.Add("The respec unit Respec built does not point back at the main character through PreviewOf.");
		}
		SelfTestLevelUpWindow.Plan plan = new SelfTestLevelUpWindow.Plan
		{
			Class = blueprintCharacterClass,
			Mode = LevelUpState.CharBuildMode.Respec,
			OnCommit = config.OnCommit,
			OnStop = config.OnStop
		};
		plan.BeforeFill = delegate(LevelUpController controller, SelfTestLevelUpWindow.Result result2)
		{
			if (controller.State.Mode != LevelUpState.CharBuildMode.Respec)
			{
				result2.Problems.Add($"save-respec: the level-up state is in {controller.State.Mode} mode, not Respec.");
			}
		};
		SelfTestLevelUpWindow.Result result = SelfTestLevelUpWindow.DriveLevel(unit, plan, "save-respec, level 1 in Respec mode");
		List<string> list2 = new List<string>();
		list2.AddRange(result.Problems);
		list2.AddRange(result.Lost.Select((KeyValuePair<string, string> l) => l.Value));
		list2.AddRange(result.Refused.Select((KeyValuePair<string, string> r) => r.Value));
		if (!result.Committed)
		{
			list2.Add("The Respec-mode level was not committed, so RespecOnCommit never ran.");
			list.AddRange(list2);
			return SelfTestChecks.Outcome.Of(passed: false, "The respec could not be completed.", list);
		}
		if (Game.Instance.Player.MainCharacter.Value != unitEntityData)
		{
			list2.Add("After the respec the main character is a different unit.");
		}
		int characterLevel2 = unitEntityData.Descriptor.Progression.CharacterLevel;
		int experience2 = unitEntityData.Descriptor.Progression.Experience;
		string text3 = unitEntityData.Descriptor.Progression.ExperienceTable?.name ?? "(none)";
		int count2 = Game.Instance.Player.Party.Count;
		list.Add(string.Format("After: level {0} ({1} XP on {2}), first class {3}, party of {4}.", characterLevel2, experience2, text3, FirstClassOf(unitEntityData)?.name ?? "(none)", count2));
		if (characterLevel2 != 1)
		{
			list2.Add($"After the respec the main character is level {characterLevel2}, expected 1 (the rest is levelled again from the kept experience).");
		}
		if (experience2 != experience)
		{
			list2.Add($"The respec changed the main character's experience from {experience} to {experience2}.");
		}
		if (!string.Equals(text, text3, StringComparison.Ordinal))
		{
			list2.Add("The respec kept the first class but switched the experience table from " + text + " to " + text3 + " (sweep 03, finding 3).");
		}
		if (count2 != count)
		{
			list2.Add($"The party had {count} members before the respec and {count2} after it.");
		}
		if (unitEntityData.PreviewOf.Entity != null)
		{
			list2.Add("The main character still has PreviewOf set after the swap.");
		}
		foreach (string item in SaveStateFingerprint.Differences(before, SaveStateFingerprint.Of(SelfTestSaveChecks.ModStorages())))
		{
			list2.Add("A record the mod keeps in the save changed across the respec: " + item);
		}
		list.AddRange(list2);
		string summary = "The main character taken through the engine's own respec (UnitHelper.Respec, the Respec-mode level through the window path," + $" RespecOnCommit's party swap) and rebuilt at level 1 as {blueprintCharacterClass.name}; {list2.Count} problems." + " The main character stays level 1 for the rest of this session; nothing is saved after this check.";
		return SelfTestChecks.Outcome.Of(list2.Count == 0, summary, list);
	}
}
