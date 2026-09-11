using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class ManaShield
	{
		private static readonly Sprite Icon_Shield = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e4450dd9c06dc034fb7c0c08abcc202b")).m_Icon;

		public static void Add()
		{
			SpecialPowerSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "ManaShield", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Special Power - Mana Shield");
				bp.SetDescription(Main.IsekaiContext, "Your spiritual mana weaves an invisible protective envelope around your body. \nBenefit: You gain DR 10/- against all physical damage, spell resistance equal to 12 + your character level, and a +4 shield bonus to AC.\nRequires character level 7.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Shield;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.BonusValue;
					c.m_StepLevel = 12;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 7;
				});
				bp.ReapplyOnLevelUp = true;
			}));
		}
	}
}
