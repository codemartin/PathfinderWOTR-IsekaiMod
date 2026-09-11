using System.Collections.Generic;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.Deathsnatcher
{
	internal class DeathsnatcherUndeadMaster
	{
		private static readonly Sprite Icon_MasteryOfFlesh = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("921ed6a6751d71140b4e75ab7bcb9890")).m_Icon;

		public static void Add()
		{
			BlueprintAbility modBlueprint = BlueprintTools.GetModBlueprint<BlueprintAbility>(Main.IsekaiContext, "DeathsnatcherCommandUndeadAbility");
			BlueprintAbility modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintAbility>(Main.IsekaiContext, "DeathsnatcherAnimateDeadAbility");
			BlueprintAbilityResource DeathsnatcherCreateUndeadResource = BlueprintTools.GetModBlueprint<BlueprintAbilityResource>(Main.IsekaiContext, "DeathsnatcherCreateUndeadResource");
			BlueprintAbilityResource DeathsnatcherFingerOfDeathResource = BlueprintTools.GetModBlueprint<BlueprintAbilityResource>(Main.IsekaiContext, "DeathsnatcherFingerOfDeathResource");
			BlueprintFeature bp = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherUndeadMaster", delegate(BlueprintFeature blueprintFeature)
			{
				blueprintFeature.SetName(Main.IsekaiContext, "Undead Master");
				blueprintFeature.SetDescription(Main.IsekaiContext, "At 20th level, the Deathsnatcher becomes a master of the undead.\nCommand Undead has unlimited uses.\nAnimate Dead has unlimited uses.\nCreate Undead has 2 additional uses per day.\nFinger of Death has 2 additional uses per day.");
				((BlueprintUnitFact)blueprintFeature).m_Icon = Icon_MasteryOfFlesh;
				blueprintFeature.AddComponent(delegate(IncreaseResourceAmount c)
				{
					c.m_Resource = DeathsnatcherCreateUndeadResource.ToReference<BlueprintAbilityResourceReference>();
					c.Value = 2;
				});
				blueprintFeature.AddComponent(delegate(IncreaseResourceAmount c)
				{
					c.m_Resource = DeathsnatcherFingerOfDeathResource.ToReference<BlueprintAbilityResourceReference>();
					c.Value = 2;
				});
			});
			modBlueprint.GetComponent<AbilityResourceLogic>().ResourceCostDecreasingFacts = new List<BlueprintUnitFactReference> { bp.ToReference<BlueprintUnitFactReference>() };
			modBlueprint2.GetComponent<AbilityResourceLogic>().ResourceCostDecreasingFacts = new List<BlueprintUnitFactReference> { bp.ToReference<BlueprintUnitFactReference>() };
		}
	}
}
