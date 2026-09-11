using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.TrainingEpisode
{
	internal class Tenacious
	{
		private static readonly Sprite Icon_DextrousDuelist = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("b701196306bb4674bb902c9f1160180f")).m_Icon;

		public static void Add()
		{
			TrainingEpisodeSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "Tenacious", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Tenacious");
				bp.SetDescription(Main.IsekaiContext, "You gain immunity to stunned, staggered, slowed, entangled, petrified, paralysis, and movement impairing effects.");
				((BlueprintUnitFact)bp).m_Icon = Icon_DextrousDuelist;
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Slowed;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Staggered;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Stunned;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.CantMove;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.MovementBan;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Entangled;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Paralyzed;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Petrified;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Staggered | SpellDescriptor.Stun | SpellDescriptor.Paralysis | SpellDescriptor.Petrified | SpellDescriptor.MovementImpairing;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Staggered | SpellDescriptor.Stun | SpellDescriptor.Paralysis | SpellDescriptor.Petrified | SpellDescriptor.MovementImpairing;
				});
				((BlueprintUnitFact)bp).m_Icon = Icon_DextrousDuelist;
			}));
		}
	}
}
