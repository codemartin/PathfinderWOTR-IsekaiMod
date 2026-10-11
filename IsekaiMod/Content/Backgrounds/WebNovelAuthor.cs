using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds;

internal class WebNovelAuthor
{
	public static void Add()
	{
		IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundWebNovelAuthor", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Web Novel Author");
			bp.SetBackgroundDescription(Main.IsekaiContext, "Having drafted hundreds of serialized chapters, cliched tropes, and surreal plot twists, the Web Novel Author's mind is completely desensitized to cognitive madness. Seen Every Plot Twist: she is immune to daze and confusion. She also gains a +2 competence bonus on Will saves and Initiative, a +1 bonus to the DC of her illusion spells, and adds {g|Encyclopedia:Knowledge_World}Knowledge (World){/g}, {g|Encyclopedia:Lore_Religion}Lore (Religion){/g} and {g|Encyclopedia:Knowledge_Arcana}Knowledge (Arcana){/g} to her class {g|Encyclopedia:Skills}skills{/g}, with a +4 competence bonus on each.");
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Dazed;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Confusion;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Daze | SpellDescriptor.Confusion;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Daze | SpellDescriptor.Confusion;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.Initiative;
				c.Value = 2;
			});
			bp.AddComponent(delegate(IncreaseSpellSchoolDC c)
			{
				c.School = SpellSchool.Illusion;
				c.BonusDC = 1;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillKnowledgeWorld;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillLoreReligion;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillKnowledgeArcana;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillKnowledgeWorld;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillLoreReligion;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillKnowledgeArcana;
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
				c.Stat = StatType.SkillLoreReligion;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillKnowledgeArcana;
				c.Value = 4;
			});
		}));
	}
}
