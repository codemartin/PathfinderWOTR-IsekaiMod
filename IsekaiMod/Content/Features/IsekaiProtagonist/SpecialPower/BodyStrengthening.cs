using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class BodyStrengthening
	{
		private static readonly Sprite Icon_IronBody = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("198fcc43490993f49899ed086fe723c1")).m_Icon;

		public static void Add()
		{
			SpecialPowerSelection.AddToDefenseSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BodyStrengthening", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Body Strengthening");
				bp.SetDescription(Main.IsekaiContext, "You gain {g|Encyclopedia:Damage_Reduction}DR{/g}/- equal to your character level.\nRequires character level 5.");
				((BlueprintUnitFact)bp).m_Icon = Icon_IronBody;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 5;
				});
				// Context recalculation updates the character-level rank without
				// removing and recreating the physical damage resistance fact.
				bp.ReapplyOnLevelUp = false;
			}));
		}
	}
}
