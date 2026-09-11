using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds
{
	internal class Speedrunner
	{
		public static void Add()
		{
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundSpeedrunner", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Speedrunner");
				bp.SetBackgroundDescription(Main.IsekaiContext, "The Speedrunner treats movement through reality as an optimization puzzle, navigating physical space with frame-perfect precision. She gains a +10 foot bonus to base movement speed, a +4 competence bonus to Initiative, a +2 competence bonus to Reflex saves, adds Mobility and Trickery to her class skills with a +4 competence bonus to each, and uses Intelligence instead of Dexterity for Mobility checks.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.Initiative;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillMobility;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillThievery;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillMobility;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillThievery;
				});
				bp.AddComponent(delegate(ReplaceStatBaseAttribute c)
				{
					c.TargetStat = StatType.SkillMobility;
					c.BaseAttributeReplacement = StatType.Intelligence;
					c.ReplaceIfHigher = true;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillMobility;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillThievery;
					c.Value = 4;
				});
			}));
		}
	}
}
