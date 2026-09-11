using IsekaiMod.Content.Classes.IsekaiProtagonist;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind
{
	internal class MastermindConsumeSpells
	{
		public static void Add()
		{
			BlueprintTools.GetBlueprint<BlueprintFeature>("69cfb4ab0d9812249b924b8f23d6d19f").CreateCopy(Main.IsekaiContext, "MastermindConsumeSpells").GetComponent<SpontaneousSpellConversion>()
				.m_CharacterClass = IsekaiProtagonistClass.GetReference();
		}
	}
}
