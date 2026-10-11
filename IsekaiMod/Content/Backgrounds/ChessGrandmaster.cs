using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds;

internal class ChessGrandmaster
{
	public static void Add()
	{
		BlueprintFeature UncannyDodge = BlueprintTools.GetBlueprint<BlueprintFeature>("3c08d842e802c3e4eb19d15496145709");
		IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundChessGrandmaster", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Chess Grandmaster");
			bp.SetBackgroundDescription(Main.IsekaiContext, "Accustomed to evaluating tactical board states dozens of moves in advance, the Chess Grandmaster anticipates enemy flanking vectors and ambush lines before they can strike. Thinking Ahead: she adds her Intelligence bonus to Initiative as an insight bonus, on top of her Dexterity. She also cannot be caught flat-footed, gains a +2 insight bonus to Armor Class, and adds Perception and Knowledge (World) to her class skills with a +4 competence bonus to both.");
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
			bp.AddComponent(delegate(DerivativeStatBonus c)
			{
				c.BaseStat = StatType.Intelligence;
				c.DerivativeStat = StatType.Initiative;
				c.Descriptor = ModifierDescriptor.Insight;
			});
			bp.AddComponent(delegate(RecalculateOnStatChange c)
			{
				c.Stat = StatType.Intelligence;
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
