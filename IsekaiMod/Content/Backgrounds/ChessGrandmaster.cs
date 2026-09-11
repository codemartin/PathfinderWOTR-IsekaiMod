using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds
{
	internal class ChessGrandmaster
	{
		public static void Add()
		{
			BlueprintFeature UncannyDodge = BlueprintTools.GetBlueprint<BlueprintFeature>("3c08d842e802c3e4eb19d15496145709");
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundChessGrandmaster", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Chess Grandmaster");
				bp.SetBackgroundDescription(Main.IsekaiContext, "Accustomed to evaluating tactical board states dozens of moves in advance, the Chess Grandmaster anticipates enemy flanking vectors and ambush lines before they can strike. She cannot be caught flat-footed, gains a +2 insight bonus to Armor Class, uses Intelligence instead of Dexterity for Initiative checks, and adds Perception and Knowledge (World) to her class skills with a +4 competence bonus to both.");
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { UncannyDodge.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				bp.AddComponent(delegate(ReplaceStatBaseAttribute c)
				{
					c.TargetStat = StatType.Initiative;
					c.BaseAttributeReplacement = StatType.Intelligence;
					c.ReplaceIfHigher = true;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillPerception;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillKnowledgeWorld;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillPerception;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillKnowledgeWorld;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillPerception;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillKnowledgeWorld;
					c.Value = 4;
				});
			}));
		}
	}
}
