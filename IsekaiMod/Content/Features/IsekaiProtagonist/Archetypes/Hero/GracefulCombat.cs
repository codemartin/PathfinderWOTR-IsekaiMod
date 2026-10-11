using IsekaiMod.Components;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;

internal class GracefulCombat
{
	private static readonly Sprite Icon_HolySword = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("bea9deffd3ab6734c9534153ddc70bde"))?.m_Icon;

	public static void Add()
	{
		Helpers.CreateBlueprint(Main.IsekaiContext, "GracefulCombat", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Graceful Combat");
			bp.SetDescription(Main.IsekaiContext, "The Hero uses the higher of their Strength and Charisma modifiers for their melee attack and damage bonus. In addition, they use the higher of their Strength and Charisma for CMB and to qualify for any feat that has a Strength prerequisite.");
			((BlueprintUnitFact)bp).m_Icon = Icon_HolySword;
			bp.AddComponent(delegate(ReplaceCombatManeuverStatIfHigher c)
			{
				c.StatType = StatType.Charisma;
			});
			bp.AddComponent(delegate(AttackStatReplacement c)
			{
				c.ReplacementStat = StatType.Charisma;
				c.SubCategory = WeaponSubCategory.Melee;
			});
			bp.AddComponent(delegate(AnyWeaponDamageStatReplacementFixed c)
			{
				c.Stat = StatType.Charisma;
				c.OnlyMelee = true;
			});
			bp.AddComponent(delegate(ReplaceStatForPrerequisites c)
			{
				c.OldStat = StatType.Strength;
				c.NewStat = StatType.Charisma;
				c.Policy = ReplaceStatForPrerequisites.StatReplacementPolicy.NewStat;
			});
		});
	}
}
