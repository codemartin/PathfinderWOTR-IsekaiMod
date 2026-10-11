using System;
using System.Collections.Generic;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using UnityEngine;

namespace IsekaiMod.Components;

[TypeId("3f7c1a52c0e84d6b9a15e2d8b74a60c1")]
public class PrerequisiteHasNoSpellbookOfClasses : Prerequisite
{
	[SerializeField]
	public BlueprintCharacterClassReference[] m_CharacterClasses;

	internal static bool HasNoBookOf(IEnumerable<BlueprintCharacterClass> classes, Func<BlueprintCharacterClass, bool> hasBook)
	{
		if (classes == null)
		{
			return true;
		}
		foreach (BlueprintCharacterClass @class in classes)
		{
			if (@class != null && hasBook(@class))
			{
				return false;
			}
		}
		return true;
	}

	public override bool CheckInternal(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state)
	{
		if (unit == null)
		{
			return false;
		}
		try
		{
			List<BlueprintCharacterClass> list = new List<BlueprintCharacterClass>();
			if (m_CharacterClasses != null)
			{
				BlueprintCharacterClassReference[] characterClasses = m_CharacterClasses;
				for (int i = 0; i < characterClasses.Length; i++)
				{
					BlueprintCharacterClass blueprintCharacterClass = characterClasses[i]?.Get();
					if (blueprintCharacterClass != null)
					{
						list.Add(blueprintCharacterClass);
					}
				}
			}
			return HasNoBookOf(list, (BlueprintCharacterClass c) => unit.Progression?.GetClassData(c)?.Spellbook != null);
		}
		catch
		{
			return false;
		}
	}

	public override string GetUITextInternal(UnitDescriptor unit)
	{
		return "You advance no spellbook";
	}
}
