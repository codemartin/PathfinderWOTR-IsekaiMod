using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using UnityEngine;

namespace IsekaiMod.Components;

[TypeId("d8ba4824778f5c3bbe2050afaf3b3e6a")]
public class PrerequisiteHasNoClassSpellbook : Prerequisite
{
	[SerializeField]
	public BlueprintCharacterClassReference m_CharacterClass;

	public BlueprintCharacterClass CharacterClass => m_CharacterClass?.Get();

	public override bool CheckInternal(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state)
	{
		if (unit == null)
		{
			return false;
		}
		BlueprintCharacterClass characterClass = CharacterClass;
		if (characterClass == null)
		{
			return false;
		}
		try
		{
			return (unit.Progression?.GetClassData(characterClass))?.Spellbook == null;
		}
		catch
		{
			return false;
		}
	}

	public override string GetUITextInternal(UnitDescriptor unit)
	{
		BlueprintCharacterClass characterClass = CharacterClass;
		if (characterClass != null)
		{
			return "You have no " + characterClass.Name + " spellbook to advance.";
		}
		return "You have no spellbook to advance.";
	}
}
