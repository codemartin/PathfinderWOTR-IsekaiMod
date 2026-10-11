using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds;

internal class DemonicCultivator
{
	public static void Add()
	{
		IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundDemonicCultivator", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Demonic Cultivator");
			bp.SetBackgroundDescription(Main.IsekaiContext, "Cultivating forbidden techniques in the demonic sects of another world, the Demonic Cultivator refined her body in poison baths and spirit fire until neither could harm her. Poison-Tempered Body: she is immune to poison. She also gains fire resistance 10, a +2 competence bonus on Fortitude saves and to Combat Maneuver Bonus, adds Athletics and Mobility to the list of her class skills with a +4 competence bonus on both, and uses the higher of Strength and Dexterity for Athletics and Mobility checks.");
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Poison;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Poison;
			});
			bp.AddComponent(delegate(AddDamageResistanceEnergy c)
			{
				c.Type = DamageEnergyType.Fire;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.AdditionalCMB;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillAthletics;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillMobility;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillAthletics;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillMobility;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillAthletics;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillMobility;
			});
			bp.AddComponent(delegate(ReplaceStatBaseAttribute c)
			{
				c.TargetStat = StatType.SkillAthletics;
				c.BaseAttributeReplacement = StatType.Dexterity;
				c.ReplaceIfHigher = true;
			});
			bp.AddComponent(delegate(ReplaceStatBaseAttribute c)
			{
				c.TargetStat = StatType.SkillMobility;
				c.BaseAttributeReplacement = StatType.Strength;
				c.ReplaceIfHigher = true;
			});
		}));
	}
}
