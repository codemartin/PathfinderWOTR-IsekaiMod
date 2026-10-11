using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds;

internal class BetaTester
{
	public static void Add()
	{
		IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundBetaTester", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Beta Tester");
			bp.SetBackgroundDescription(Main.IsekaiContext, "The Beta Tester played this world before it launched, and remembers every trap, ambush and cheap death that was patched out of the final release. Patch Notes: she gains a +2 insight bonus on all saving throws. She also has a +4 competence bonus to initiative and adds {g|Encyclopedia:Lore_Nature}Lore (Nature){/g}, {g|Encyclopedia:Lore_Religion}Lore (Religion){/g}, {g|Encyclopedia:Knowledge_World}Knowledge (World){/g}, {g|Encyclopedia:Knowledge_Arcana}Knowledge (Arcana){/g}, and {g|Encyclopedia:Perception}Perception{/g} to the list of her class {g|Encyclopedia:Skills}skills{/g}.");
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.Initiative;
				c.Value = 4;
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
				c.Skill = StatType.SkillLoreNature;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillLoreReligion;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillPerception;
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
				c.Skill = StatType.SkillLoreNature;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillLoreReligion;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillPerception;
			});
		}));
	}
}
