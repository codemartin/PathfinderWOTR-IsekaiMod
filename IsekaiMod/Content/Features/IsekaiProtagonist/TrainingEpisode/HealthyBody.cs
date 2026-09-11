using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.TrainingEpisode
{
	internal class HealthyBody
	{
		private static readonly Sprite Icon_PurityOfBody = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9b02f77c96d6bba4daf9043eff876c76")).m_Icon;

		public static void Add()
		{
			TrainingEpisodeSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "HealthyBody", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Healthy Body");
				bp.SetDescription(Main.IsekaiContext, "You gain immunity to bleed, blindness, curses, poison, disease, sickened, and nauseated conditions.");
				((BlueprintUnitFact)bp).m_Icon = Icon_PurityOfBody;
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Sickened;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Nauseated;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Blindness;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Poison | SpellDescriptor.Disease | SpellDescriptor.Sickened | SpellDescriptor.Nauseated | SpellDescriptor.Blindness | SpellDescriptor.Curse | SpellDescriptor.Bleed;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Poison | SpellDescriptor.Disease | SpellDescriptor.Sickened | SpellDescriptor.Nauseated | SpellDescriptor.Blindness | SpellDescriptor.Curse | SpellDescriptor.Bleed;
				});
			}));
		}
	}
}
