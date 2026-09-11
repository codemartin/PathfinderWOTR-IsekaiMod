using System.Collections.Generic;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class OmniscientMimicry
	{
		private static readonly Sprite Icon_Omniscient = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("6e109d21da9e1c44fb772a9eca2cafdd"))?.m_Icon;

		private static readonly BlueprintFeature OutflankFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("422dab7309e1ad343935f33a4d6e9f11");

		private static readonly BlueprintFeature CombatReflexesFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("0f8939ae6f220984e8fb568abbdfba95");

		private static readonly BlueprintFeature SeizeTheMomentFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("1191ef3065e6f8e4f9fbe1b7e3c0f760");

		public static void Add()
		{
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "OmniscientMimicryFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Omniscient Mimicry (Great Sage)");
				bp.SetDescription(Main.IsekaiContext, "Operating with the analytical perfection of an omniscient voice in your soul (Great Sage / Copy Ninja), you instantly analyze and counter enemy fighting styles.\nBenefit: You gain a +4 insight bonus to Armor Class, attack rolls, and saving throws. You cannot be flat-footed or surprised, and you automatically gain the Outflank, Combat Reflexes, and Seize the Moment teamwork feats.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Omniscient;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.LoseDexterityToAC;
				});
				List<BlueprintUnitFactReference> factsToAdd = new List<BlueprintUnitFactReference>();
				if (OutflankFeat != null)
				{
					factsToAdd.Add(OutflankFeat.ToReference<BlueprintUnitFactReference>());
				}
				if (CombatReflexesFeat != null)
				{
					factsToAdd.Add(CombatReflexesFeat.ToReference<BlueprintUnitFactReference>());
				}
				if (SeizeTheMomentFeat != null)
				{
					factsToAdd.Add(SeizeTheMomentFeat.ToReference<BlueprintUnitFactReference>());
				}
				if (factsToAdd.Count > 0)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = factsToAdd.ToArray();
					});
				}
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 10;
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
		}
	}
}
