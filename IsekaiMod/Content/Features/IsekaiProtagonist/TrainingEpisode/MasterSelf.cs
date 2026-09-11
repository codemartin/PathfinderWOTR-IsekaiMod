using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.TrainingEpisode
{
	internal class MasterSelf
	{
		private static readonly Sprite Icon_Serenity = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("d316d3d94d20c674db2c24d7de96f6a7")).m_Icon;

		public static void Add()
		{
			TrainingEpisodeSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "MasterSelf", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Master Self-Control");
				bp.SetDescription(Main.IsekaiContext, "You gain immunity to dazed, dazzled, sleep, confusion, charm, emotion, compulsion, and mind-affecting effects.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Serenity;
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Dazed;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Dazzled;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Confusion;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Sleeping;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion | SpellDescriptor.Emotion | SpellDescriptor.Charm | SpellDescriptor.Daze | SpellDescriptor.Confusion | SpellDescriptor.Sleep | SpellDescriptor.NegativeEmotion;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion | SpellDescriptor.Emotion | SpellDescriptor.Charm | SpellDescriptor.Daze | SpellDescriptor.Confusion | SpellDescriptor.Sleep | SpellDescriptor.NegativeEmotion;
				});
			}));
		}
	}
}
