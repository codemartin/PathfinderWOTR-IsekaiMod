using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds;

internal class MasterChef
{
	public static void Add()
	{
		IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundMasterChef", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Master Chef");
			bp.SetBackgroundDescription(Main.IsekaiContext, "Trained in the high culinary arts with precise knife technique, biochemical flavor balancing, and metabolic nutrition, the Master Chef has tasted everything at least once. Iron Stomach: she is immune to nausea and sickness. She also gains proficiency with daggers and kukris, +10 maximum Hit Points, a +3 competence bonus on Fortitude saves, a +2 competence bonus on Will saves, and adds Lore (Nature) and Perception to her class skills with a +4 and +2 competence bonus respectively.");
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
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Nauseated;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Sickened;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Sickened | SpellDescriptor.Nauseated;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Sickened | SpellDescriptor.Nauseated;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveFortitude;
				c.Value = 3;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
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
