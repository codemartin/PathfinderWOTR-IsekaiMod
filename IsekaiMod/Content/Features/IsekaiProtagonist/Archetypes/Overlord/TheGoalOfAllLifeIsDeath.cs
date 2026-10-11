using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;

internal class TheGoalOfAllLifeIsDeath
{
	private static readonly Sprite Icon_TGOALID = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("b24583190f36a8442b212e45226c54fc"))?.m_Icon;

	public static void Add()
	{
		BlueprintAbilityResource TGOALIDResource = Helpers.CreateBlueprint(Main.IsekaiContext, "TGOALIDResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = new BlueprintAbilityResource.Amount
			{
				BaseValue = 3,
				IncreasedByLevel = false,
				IncreasedByStat = false
			};
		});
		BlueprintBuff TGOALIDBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "TGOALIDBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "The Goal of All Life Is Death (Active)");
			bp.SetDescription(Main.IsekaiContext, "The supreme eclipse clock of the Sovereign of Death has begun ticking down. For 1 round per character level you gain a +20 bonus on caster level checks to overcome spell resistance and a +4 bonus to the DC of all your spells, and your death effects ignore immunity to death effects.");
			((BlueprintUnitFact)bp).m_Icon = Icon_TGOALID;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(SpellPenetrationBonus c)
			{
				c.Value = 20;
				c.Descriptor = ModifierDescriptor.UntypedStackable;
			});
			bp.AddComponent(delegate(IncreaseAllSpellsDC c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Value = 4;
			});
			bp.AddComponent(delegate(IgnoreSpellImmunity c)
			{
				c.SpellDescriptor = SpellDescriptor.Death;
			});
		});
		BlueprintAbility TGOALIDAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "TGOALIDAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "The Goal of All Life Is Death");
			bp.SetDescription(Main.IsekaiContext, "Three times per day as a swift action, the Overlord activates the supreme secret art of supreme necromancy. For 1 round per character level you gain a +20 bonus on caster level checks to overcome spell resistance and a +4 bonus to the DC of all your spells, and your death effects ignore immunity to death effects.");
			((BlueprintUnitFact)bp).m_Icon = Icon_TGOALID;
			bp.Type = AbilityType.Supernatural;
			bp.Range = AbilityRange.Personal;
			bp.CanTargetSelf = true;
			bp.ActionType = UnitCommand.CommandType.Swift;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
			bp.AddComponent(delegate(AbilityResourceLogic c)
			{
				c.m_RequiredResource = TGOALIDResource.ToReference<BlueprintAbilityResourceReference>();
				c.m_IsSpendResource = true;
				c.Amount = 1;
			});
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new ContextActionApplyBuff
				{
					m_Buff = TGOALIDBuff.ToReference<BlueprintBuffReference>(),
					Permanent = false,
					DurationValue = new ContextDurationValue
					{
						Rate = DurationRate.Rounds,
						DiceType = DiceType.Zero,
						DiceCountValue = 0,
						BonusValue = Values.CreateContextRankValue(AbilityRankType.Default)
					},
					IsNotDispelable = true
				});
			});
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.Default;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
			});
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "TheGoalOfAllLifeIsDeathFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "The Goal of All Life Is Death");
			bp.SetDescription(Main.IsekaiContext, "At 12th level, the Overlord masters the ultimate trump card skill: The Goal of All Life Is Death. Three times per day, for 1 round per character level, you gain a +20 bonus on caster level checks to overcome spell resistance and a +4 bonus to the DC of all your spells, and your death effects ignore immunity to death effects.");
			((BlueprintUnitFact)bp).m_Icon = Icon_TGOALID;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddAbilityResources c)
			{
				c.m_Resource = TGOALIDResource.ToReference<BlueprintAbilityResourceReference>();
				c.RestoreAmount = true;
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { TGOALIDAbility.ToReference<BlueprintUnitFactReference>() };
			});
		});
	}
}
