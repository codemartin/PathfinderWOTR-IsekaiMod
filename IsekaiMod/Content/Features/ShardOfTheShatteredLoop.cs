using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features
{
	internal class ShardOfTheShatteredLoop
	{
		public static void Add()
		{
			Sprite Icon_Shard = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("63bb12d87f99c4748a890956e5f6b4c7"))?.m_Icon;
			Helpers.CreateBlueprint(Main.IsekaiContext, "ShardOfTheShatteredLoopFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Shard of the Shattered Loop");
				bp.SetDescription(Main.IsekaiContext, "A crystalline, iridescent fragment of Yog-Sothoth's shattered causal anchor, carried across the veil of time by an otherworlder who broke the infinite loop and chose to return as an unchained sovereign.\nGrants a +2 sacred bonus on all saving throws, a +10 ft bonus to base movement speed, and a +2 bonus to caster level checks to overcome spell resistance.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shard;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
				bp.AddComponent(delegate(SpellPenetrationBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Value = 2;
				});
				bp.Groups = new FeatureGroup[0];
				bp.ReapplyOnLevelUp = true;
			});
		}

		public static BlueprintFeature Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShardOfTheShatteredLoopFeature");
		}

		public static BlueprintFeatureReference GetReference()
		{
			return Get().ToReference<BlueprintFeatureReference>();
		}
	}
}
