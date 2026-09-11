using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds
{
	internal class DemonicCultivator
	{
		public static void Add()
		{
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundDemonicCultivator", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Demonic Cultivator");
				bp.SetBackgroundDescription(Main.IsekaiContext, "The Demonic Cultivator adds Athletics and Mobility to the list of her class skills and uses the higher of Strength and Dexterity while attempting Athletics and Mobility Checks.");
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
}
