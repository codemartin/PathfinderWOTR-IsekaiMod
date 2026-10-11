using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds;

internal class TabletopRPGPlayer
{
	public static void Add()
	{
		IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundTabletopRPGPlayer", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Tabletop RPG Player");
			bp.SetBackgroundDescription(Main.IsekaiContext, "Possessing an encyclopedic understanding of monster stat blocks, magical resistance curves, and rulebook interactions, the Tabletop RPG Player anticipates enemy tactics with scholarly precision. Roll Twice: she rolls every skill check twice and takes the better result. She also gains a +2 competence bonus on Will saving throws, an additional +2 bonus on saving throws against mind-affecting effects, and adds Knowledge (World), Knowledge (Arcana), Lore (Religion), and Lore (Nature) to her class skills, using the higher of Charisma and her usual attribute for checks with those skills.");
			bp.AddComponent(delegate(ModifyD20 c)
			{
				c.Rule = RuleType.SkillCheck;
				c.RollsAmount = 1;
				c.TakeBest = true;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
			bp.AddComponent(delegate(SavingThrowBonusAgainstDescriptor c)
			{
				c.ModifierDescriptor = ModifierDescriptor.Competence;
				c.SpellDescriptor = SpellDescriptor.MindAffecting;
				c.Value = 2;
				c.Bonus = 0;
			});
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
			bp.AddComponent(delegate(ReplaceStatBaseAttribute c)
			{
				c.TargetStat = StatType.SkillKnowledgeWorld;
				c.BaseAttributeReplacement = StatType.Charisma;
				c.ReplaceIfHigher = true;
			});
			bp.AddComponent(delegate(ReplaceStatBaseAttribute c)
			{
				c.TargetStat = StatType.SkillKnowledgeArcana;
				c.BaseAttributeReplacement = StatType.Charisma;
				c.ReplaceIfHigher = true;
			});
			bp.AddComponent(delegate(ReplaceStatBaseAttribute c)
			{
				c.TargetStat = StatType.SkillLoreReligion;
				c.BaseAttributeReplacement = StatType.Charisma;
				c.ReplaceIfHigher = true;
			});
			bp.AddComponent(delegate(ReplaceStatBaseAttribute c)
			{
				c.TargetStat = StatType.SkillLoreNature;
				c.BaseAttributeReplacement = StatType.Charisma;
				c.ReplaceIfHigher = true;
			});
		}));
	}
}
