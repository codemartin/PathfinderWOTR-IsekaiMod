using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds;

internal class HighschoolStudent
{
	public static void Add()
	{
		IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundHighschoolStudent", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Highschool Student");
			bp.SetBackgroundDescription(Main.IsekaiContext, "Transported mid-growth spurt, straight from gym class, the Highschool Student arrives in Golarion with a body that has not finished growing. Growing Body: she is immune to ability score damage and drain. She also gains a +1 bonus to Strength, Dexterity and Constitution, a +2 competence bonus on Fortitude saves, and adds Athletics and Mobility to her class skills.");
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Trait;
				c.Stat = StatType.Strength;
				c.Value = 1;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Trait;
				c.Stat = StatType.Dexterity;
				c.Value = 1;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Trait;
				c.Stat = StatType.Constitution;
				c.Value = 1;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddImmunityToAbilityScoreDamage c)
			{
				c.Drain = true;
				c.StatTypes = new StatType[0];
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
		}));
	}
}
