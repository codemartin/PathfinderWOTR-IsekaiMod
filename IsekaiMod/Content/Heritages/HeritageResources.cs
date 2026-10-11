using Kingmaker.Blueprints;

namespace IsekaiMod.Content.Heritages;

internal static class HeritageResources
{
	internal static BlueprintAbilityResource.Amount ByCharacterLevel(int baseValue, int startingLevel, int startingIncrease, int levelStep, int perStep)
	{
		return new BlueprintAbilityResource.Amount
		{
			BaseValue = baseValue,
			IncreasedByLevel = false,
			IncreasedByStat = false,
			IncreasedByLevelStartPlusDivStep = true,
			StartingLevel = startingLevel,
			StartingIncrease = startingIncrease,
			LevelStep = levelStep,
			PerStepIncrease = perStep,
			MinClassLevelIncrease = 0,
			OtherClassesModifier = 1f,
			m_Class = new BlueprintCharacterClassReference[0],
			m_Archetypes = new BlueprintArchetypeReference[0],
			m_ClassDiv = new BlueprintCharacterClassReference[0],
			m_ArchetypesDiv = new BlueprintArchetypeReference[0]
		};
	}

	internal static BlueprintAbilityResource.Amount Fixed(int amount)
	{
		return new BlueprintAbilityResource.Amount
		{
			BaseValue = amount,
			IncreasedByLevel = false,
			IncreasedByStat = false,
			m_Class = new BlueprintCharacterClassReference[0],
			m_Archetypes = new BlueprintArchetypeReference[0],
			m_ClassDiv = new BlueprintCharacterClassReference[0],
			m_ArchetypesDiv = new BlueprintArchetypeReference[0]
		};
	}
}
