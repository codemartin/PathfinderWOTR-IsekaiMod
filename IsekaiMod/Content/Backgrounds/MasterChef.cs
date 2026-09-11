using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds
{
	internal class MasterChef
	{
		public static void Add()
		{
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundMasterChef", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Master Chef");
				bp.SetBackgroundDescription(Main.IsekaiContext, "Trained in the high culinary arts with precise knife technique, biochemical flavor balancing, and metabolic nutrition, the Master Chef fortifies allies and carves ingredients with clinical mastery. She gains proficiency with daggers and kukris, +10 maximum Hit Points, a +2 competence bonus on Fortitude saves, a +1 insight bonus on all saving throws, and adds Lore (Nature) and Perception to her class skills with a +4 and +2 competence bonus respectively.");
				bp.AddComponent(delegate(AddBackgroundWeaponProficiency c)
				{
					c.Proficiency = WeaponCategory.Dagger;
					c.StackBonusType = ModifierDescriptor.Enhancement;
					c.StackBonus = 1;
				});
				bp.AddComponent(delegate(AddBackgroundWeaponProficiency c)
				{
					c.Proficiency = WeaponCategory.Kukri;
					c.StackBonusType = ModifierDescriptor.Enhancement;
					c.StackBonus = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveFortitude;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveReflex;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveWill;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.HitPoints;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillLoreNature;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillPerception;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillLoreNature;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillPerception;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillLoreNature;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillPerception;
					c.Value = 2;
				});
			}));
		}
	}
}
