using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;

internal static class PortedAuras
{
	internal static readonly StatType[] AcAttackDamageSaves = new StatType[6]
	{
		StatType.AC,
		StatType.AdditionalAttackBonus,
		StatType.AdditionalDamage,
		StatType.SaveFortitude,
		StatType.SaveReflex,
		StatType.SaveWill
	};

	internal static void ConfigureAllyAura(BlueprintAbilityAreaEffect area, BlueprintBuff buff, int feet)
	{
		area.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
		area.SpellResistance = false;
		area.AggroEnemies = false;
		area.AffectEnemies = false;
		area.Shape = AreaEffectShape.Cylinder;
		area.Size = new Feet(feet);
		area.AddUnconditionalAuraEffect(buff.ToReference<BlueprintBuffReference>());
	}
}
