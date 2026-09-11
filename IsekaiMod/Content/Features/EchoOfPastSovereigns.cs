using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features
{
	internal class EchoOfPastSovereigns
	{
		private static BlueprintFeature echoFeature;

		public static void Add()
		{
			echoFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "EchoOfPastSovereignsFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Echo of Past Sovereigns");
				bp.SetDescription(Main.IsekaiContext, "Memories, reflexes, and cosmic authority from previous cycles echo through your soul in New Game+. You gain a +1 competence bonus to all skill checks per past loop, an uncapped +25% Cosmic Coin multiplier per loop, and otherworldly patron deities recognize your cyclical continuity across time.");
				bp.Ranks = 99;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(BuffAllSkillsBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Value = 1;
					c.Multiplier = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
					c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
					c.m_Progression = ContextRankProgression.AsIs;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.Initiative;
					c.Value = 2;
				});
			});
		}

		public static BlueprintFeature Get()
		{
			if (echoFeature != null)
			{
				return echoFeature;
			}
			return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "EchoOfPastSovereignsFeature");
		}

		public static BlueprintFeatureReference GetReference()
		{
			return BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "EchoOfPastSovereignsFeature");
		}
	}
}
