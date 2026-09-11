using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.TrainingEpisode
{
	internal class InnerPower
	{
		private static readonly Sprite Icon_BurningRenewal = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("7cf2a6bf35c422e4ea219fcc2eb564f5")).m_Icon;

		public static void Add()
		{
			TrainingEpisodeSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "InnerPower", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Inner Power");
				bp.SetDescription(Main.IsekaiContext, "You gain immunity to shaken, frightened, cowering, fear, death effects, {g|Encyclopedia:Ability_Scores}ability score{/g} drain, energy drain, and negative levels.");
				((BlueprintUnitFact)bp).m_Icon = Icon_BurningRenewal;
				bp.AddComponent(delegate(AddImmunityToAbilityScoreDamage c)
				{
					c.Drain = true;
				});
				bp.AddComponent<AddImmunityToEnergyDrain>();
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Shaken;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Frightened;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Cowering;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Shaken | SpellDescriptor.Frightened | SpellDescriptor.Death | SpellDescriptor.StatDebuff | SpellDescriptor.NegativeLevel;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Shaken | SpellDescriptor.Frightened | SpellDescriptor.Death | SpellDescriptor.StatDebuff | SpellDescriptor.NegativeLevel;
				});
			}));
		}
	}
}
