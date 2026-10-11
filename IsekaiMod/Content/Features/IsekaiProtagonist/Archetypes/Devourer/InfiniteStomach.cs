using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;

internal class InfiniteStomach
{
	private static readonly Sprite Icon_Stomach = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ad5ed5ea4ec52334a94e975a64dad336"))?.m_Icon;

	public static void Add()
	{
		BlueprintBuff InfiniteStomachVitalityBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "InfiniteStomachVitalityBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Infinite Stomach: Stored Vitality");
			bp.SetDescription(Main.IsekaiContext, "Biomass stored in the Slime's pocket dimension while resting. Grants 20 temporary hit points, which last until they are spent.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Stomach;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(TemporaryHitPointsFromAbilityValue c)
			{
				c.Value = 20;
				c.RemoveWhenHitPointsEnd = true;
			});
		});
		ContextActionApplyBuff ApplyVitality = new ContextActionApplyBuff
		{
			m_Buff = InfiniteStomachVitalityBuff.ToReference<BlueprintBuffReference>(),
			Permanent = true,
			DurationValue = Values.Duration.Zero,
			AsChild = false
		};
		Helpers.CreateBlueprint(Main.IsekaiContext, "InfiniteStomachFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Infinite Stomach");
			bp.SetDescription(Main.IsekaiContext, "At 7th level, the Slime's body houses an infinite spatial pocket dimension. Enemy ranged and projectile attacks suffer a 20% miss chance as incoming attacks are swallowed directly into the void. Additionally, you gain Spell Resistance equal to 11 + your character level, and 20 temporary hit points that are restored whenever you rest.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Stomach;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddConcealment c)
			{
				c.Descriptor = ConcealmentDescriptor.Displacement;
				c.Concealment = Concealment.Partial;
				c.CheckWeaponRangeType = true;
				c.RangeType = WeaponRangeType.Ranged;
			});
			bp.AddComponent(delegate(AddSpellResistance c)
			{
				c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
			});
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.Default;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				c.m_Progression = ContextRankProgression.BonusValue;
				c.m_StepLevel = 11;
			});
			bp.AddComponent(delegate(AddRestTrigger c)
			{
				c.Action = Helpers.CreateActionList(ApplyVitality);
			});
			bp.AddComponent(delegate(AddFactContextActions c)
			{
				c.Activated = Helpers.CreateActionList(ApplyVitality);
				c.Deactivated = Helpers.CreateActionList(new ContextActionRemoveBuff
				{
					m_Buff = InfiniteStomachVitalityBuff.ToReference<BlueprintBuffReference>()
				});
				c.NewRound = Helpers.CreateActionList();
			});
		});
	}
}
