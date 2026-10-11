using IsekaiMod.Config;
using Kingmaker;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace IsekaiMod.Components;

[TypeId("d80265e4a2ec40988c60eb2241910c97")]
public class PrerequisiteIsMainCharacter : Prerequisite
{
	public bool CompanionOnly;

	public bool RespectsIsekaiSettings;

	internal static bool IsCharacterBuildMode(LevelUpState.CharBuildMode mode)
	{
		if (mode != LevelUpState.CharBuildMode.CharGen)
		{
			return mode == LevelUpState.CharBuildMode.Respec;
		}
		return true;
	}

	internal static bool CheckCharacterBuild(bool isEmployee, bool isLoreCompanion, bool companionOnly)
	{
		if (isEmployee | isLoreCompanion)
		{
			return companionOnly;
		}
		return !companionOnly;
	}

	internal static bool GateEnabled(bool respectsIsekaiSettings, bool companionOnly, bool excludeCompanions, bool allowMainCharacterRetinue)
	{
		if (!respectsIsekaiSettings)
		{
			return true;
		}
		if (!companionOnly)
		{
			return excludeCompanions;
		}
		return !allowMainCharacterRetinue;
	}

	internal static bool Decide(bool respectsIsekaiSettings, bool companionOnly, bool excludeCompanions, bool allowMainCharacterRetinue, bool isCompanion)
	{
		if (!GateEnabled(respectsIsekaiSettings, companionOnly, excludeCompanions, allowMainCharacterRetinue))
		{
			return true;
		}
		if (!companionOnly)
		{
			return !isCompanion;
		}
		return isCompanion;
	}

	private bool GateEnabledNow()
	{
		AddedContent addedContent = Main.IsekaiContext?.AddedContent;
		if (addedContent == null)
		{
			return true;
		}
		return GateEnabled(RespectsIsekaiSettings, CompanionOnly, addedContent.ExcludeCompanionsFromIsekaiClass, addedContent.AllowMainCharacterRetinue);
	}

	private bool AlreadyHasTheClass(UnitDescriptor unit)
	{
		if (unit?.Progression == null)
		{
			return false;
		}
		if (base.OwnerBlueprint is BlueprintCharacterClass characterClass)
		{
			return unit.Progression.GetClassLevel(characterClass) > 0;
		}
		return false;
	}

	public override bool CheckInternal(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state)
	{
		if (!GateEnabledNow())
		{
			return true;
		}
		if (AlreadyHasTheClass(unit))
		{
			return true;
		}
		if (state != null && IsCharacterBuildMode(state.Mode))
		{
			return CheckCharacterBuild(state.IsEmployee, state.IsLoreCompanion, CompanionOnly);
		}
		if (Game.Instance?.Player?.MainCharacter.Value == null)
		{
			return !CompanionOnly;
		}
		if (unit?.Unit == null)
		{
			return !CompanionOnly;
		}
		if (!CompanionOnly)
		{
			return unit.Unit.IsMainCharacter;
		}
		return !unit.Unit.IsMainCharacter;
	}

	public override string GetUITextInternal(UnitDescriptor unit)
	{
		if (!CompanionOnly)
		{
			return "Is Main Character";
		}
		return "Is Companion or Mercenary";
	}
}
