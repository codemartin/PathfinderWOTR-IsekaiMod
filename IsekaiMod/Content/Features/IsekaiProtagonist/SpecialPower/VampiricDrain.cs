using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class VampiricDrain
	{
		private static readonly Sprite Icon_VampiricTouch = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("0dff842f06edace43baf8a2f44207045")).m_Icon;

		public static void Add()
		{
			SpecialPowerSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "VampiricDrain", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Special Power - Vampiric Drain");
				bp.SetDescription(Main.IsekaiContext, "Your attacks thirst for the vital essence of your foes. \nBenefit: Whenever you strike a living creature with a weapon attack, you drain their vitality, healing for 1d6 plus half your character level in hit points.\nRequires character level 5.");
				((BlueprintUnitFact)bp).m_Icon = Icon_VampiricTouch;
				bp.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
				{
					c.OnlyHit = true;
					c.Action = Helpers.CreateActionList(new ContextActionOnContextCaster
					{
						Actions = Helpers.CreateActionList(new ContextActionHealTarget
						{
							Value = new ContextDiceValue
							{
								DiceType = DiceType.D6,
								DiceCountValue = 1,
								BonusValue = Values.CreateContextRankValue(AbilityRankType.Default)
							}
						})
					});
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Div2;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 5;
				});
			}));
		}
	}
}
