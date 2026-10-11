using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds;

internal class EnlightenedSage
{
	public static void Add()
	{
		IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundEnlightenedSage", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Enlightened Sage");
			bp.SetBackgroundDescription(Main.IsekaiContext, "Years of meditation on a distant mountain opened the Enlightened Sage's inner eye; she no longer needs the outer ones to see. Third Eye: she ignores all concealment and is immune to blindness. She also adds {g|Encyclopedia:Lore_Nature}Lore (Nature){/g}, {g|Encyclopedia:Lore_Religion}Lore (Religion){/g}, {g|Encyclopedia:Knowledge_World}Knowledge (World){/g}, {g|Encyclopedia:Knowledge_Arcana}Knowledge (Arcana){/g}, and {g|Encyclopedia:Perception}Perception{/g} to the list of her class {g|Encyclopedia:Skills}skills{/g}, and uses the higher of {g|Encyclopedia:Intelligence}Intelligence{/g} and {g|Encyclopedia:Wisdom}Wisdom{/g} for all five of those skills.");
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Blindness;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Blindness;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Blindness;
			});
			bp.AddComponent<IgnoreConcealment>();
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillKnowledgeWorld;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillKnowledgeArcana;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillLoreReligion;
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
				c.Skill = StatType.SkillKnowledgeWorld;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillKnowledgeArcana;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillLoreReligion;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillLoreNature;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillPerception;
			});
			bp.AddComponent(delegate(ReplaceStatBaseAttribute c)
			{
				c.TargetStat = StatType.SkillKnowledgeWorld;
				c.BaseAttributeReplacement = StatType.Wisdom;
				c.ReplaceIfHigher = true;
			});
			bp.AddComponent(delegate(ReplaceStatBaseAttribute c)
			{
				c.TargetStat = StatType.SkillKnowledgeArcana;
				c.BaseAttributeReplacement = StatType.Wisdom;
				c.ReplaceIfHigher = true;
			});
			bp.AddComponent(delegate(ReplaceStatBaseAttribute c)
			{
				c.TargetStat = StatType.SkillLoreReligion;
				c.BaseAttributeReplacement = StatType.Intelligence;
				c.ReplaceIfHigher = true;
			});
			bp.AddComponent(delegate(ReplaceStatBaseAttribute c)
			{
				c.TargetStat = StatType.SkillLoreNature;
				c.BaseAttributeReplacement = StatType.Intelligence;
				c.ReplaceIfHigher = true;
			});
			bp.AddComponent(delegate(ReplaceStatBaseAttribute c)
			{
				c.TargetStat = StatType.SkillPerception;
				c.BaseAttributeReplacement = StatType.Intelligence;
				c.ReplaceIfHigher = true;
			});
		}));
	}
}
