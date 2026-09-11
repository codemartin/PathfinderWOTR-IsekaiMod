using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds
{
	internal class SoftwareEngineer
	{
		public static void Add()
		{
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundSoftwareEngineer", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Software Engineer");
				bp.SetBackgroundDescription(Main.IsekaiContext, "Accustomed to debugging Byzantine codebases, reverse-engineering architectures, and analyzing algorithmic logic, the Software Engineer views magical wards, intricate runic systems, and dimensional locks as poorly documented code to manipulate. She gains a +2 competence bonus on caster level checks to overcome spell resistance, a +2 competence bonus on saving throws against enchantment and illusion effects, adds Knowledge (Arcana), Knowledge (World), Trickery, and Use Magic Device to her class skills, and uses Intelligence instead of Charisma or Dexterity for Use Magic Device and Trickery checks.");
				bp.AddComponent(delegate(SpellPenetrationBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
				bp.AddComponent(delegate(SavingThrowBonusAgainstSchool c)
				{
					c.ModifierDescriptor = ModifierDescriptor.Competence;
					c.School = SpellSchool.Enchantment;
					c.Value = 2;
				});
				bp.AddComponent(delegate(SavingThrowBonusAgainstSchool c)
				{
					c.ModifierDescriptor = ModifierDescriptor.Competence;
					c.School = SpellSchool.Illusion;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillKnowledgeArcana;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillKnowledgeWorld;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillThievery;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillUseMagicDevice;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillKnowledgeArcana;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillKnowledgeWorld;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillThievery;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillUseMagicDevice;
				});
				bp.AddComponent(delegate(ReplaceStatBaseAttribute c)
				{
					c.TargetStat = StatType.SkillUseMagicDevice;
					c.BaseAttributeReplacement = StatType.Intelligence;
					c.ReplaceIfHigher = true;
				});
				bp.AddComponent(delegate(ReplaceStatBaseAttribute c)
				{
					c.TargetStat = StatType.SkillThievery;
					c.BaseAttributeReplacement = StatType.Intelligence;
					c.ReplaceIfHigher = true;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillKnowledgeArcana;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillKnowledgeWorld;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillThievery;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillUseMagicDevice;
					c.Value = 3;
				});
			}));
		}
	}
}
