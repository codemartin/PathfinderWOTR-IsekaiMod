using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal class OtherworldlyStamina
	{
		private static readonly Sprite Icon_Bravery = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("f6388946f9f472f4585591b80e9f2452")).m_Icon;

		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "OtherworldlyStamina", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Otherworldly Stamina");
				bp.SetDescription(Main.IsekaiContext, "At 13th Level, you become immune to fatigue and exhaustion.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Bravery;
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Fatigued;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Exhausted;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Fatigue | SpellDescriptor.Exhausted;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Fatigue | SpellDescriptor.Exhausted;
				});
			});
		}
	}
}
