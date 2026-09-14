using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds
{
	internal class WebNovelAuthor
	{
		public static void Add()
		{
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundWebNovelAuthor", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Web Novel Author");
				bp.SetBackgroundDescription(Main.IsekaiContext, "Having drafted hundreds of serialized chapters, clichéd tropes, and surreal plot twists, the Web Novel Author's mind is completely desensitized to cognitive madness. She gains immunity to {g|Encyclopedia:Condition}daze{/g} and {g|Encyclopedia:Condition}confusion{/g} effects, and adds {g|Encyclopedia:Knowledge_World}Knowledge (World){/g} and {g|Encyclopedia:Lore_Religion}Lore (Religion){/g} to her class {g|Encyclopedia:Skills}skills{/g}, with a +4 competence bonus on both.");
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Dazed;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Confusion;
				});
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Daze | SpellDescriptor.Confusion;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Daze | SpellDescriptor.Confusion;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillKnowledgeWorld;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillLoreReligion;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillKnowledgeWorld;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillLoreReligion;
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
			}));
		}
	}
}
