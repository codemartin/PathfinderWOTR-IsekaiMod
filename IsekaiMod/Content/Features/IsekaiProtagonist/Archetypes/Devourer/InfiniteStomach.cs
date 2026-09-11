using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer
{
	internal class InfiniteStomach
	{
		private static readonly Sprite Icon_Stomach = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ad5ed5ea4ec52334a94e975a64dad336"))?.m_Icon;

		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "InfiniteStomachFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Infinite Stomach");
				bp.SetDescription(Main.IsekaiContext, "At 7th level, the Slime's body houses an infinite spatial pocket dimension. Enemy ranged and projectile attacks suffer a 25% miss chance as incoming attacks are swallowed directly into the void. Additionally, you gain Spell Resistance equal to 11 + your character level, and gain 20 temporary hit points whenever resting.");
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
				bp.AddComponent(delegate(TemporaryHitPointsFromAbilityValue c)
				{
					c.Value = 20;
				});
			});
		}
	}
}
