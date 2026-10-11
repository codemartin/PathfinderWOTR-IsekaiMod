using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds;

internal class ModernSoldier
{
	public static void Add()
	{
		IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundModernSoldier", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Modern Soldier");
			bp.SetBackgroundDescription(Main.IsekaiContext, "Trained in modern military discipline, marksmanship, and tactical doctrine, the Modern Soldier brings professional combat doctrine to the battlefield. Marksman Doctrine: she gains a +2 competence bonus on ranged attack rolls. She also gains proficiency with all martial weapons, a +2 competence bonus to Initiative, a +2 competence bonus on Fortitude saves, and adds Athletics and Perception to her class skills, gaining a +3 competence bonus to Athletics and a +1 competence bonus to Perception.");
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { StaticReferences.Proficiencies.MartialWeaponProficiency.ToReference<BlueprintUnitFactReference>() };
			});
			bp.AddComponent(delegate(AttackTypeAttackBonus c)
			{
				c.Type = WeaponRangeType.Ranged;
				c.AttackBonus = 1;
				c.Value = 2;
				c.Descriptor = ModifierDescriptor.Competence;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.Initiative;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillAthletics;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillPerception;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillAthletics;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillPerception;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillAthletics;
				c.Value = 3;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillPerception;
				c.Value = 1;
			});
		}));
	}
}
