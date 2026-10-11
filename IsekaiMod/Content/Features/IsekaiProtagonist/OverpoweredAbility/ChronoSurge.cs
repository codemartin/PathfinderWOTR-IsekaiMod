using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;

internal class ChronoSurge
{
	private static readonly Sprite Icon_Surge = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("4f8181e7a7f1d904fbaea64220e83379"))?.m_Icon;

	internal const int UsesPerDay = 3;

	internal const int StorePrice = 15000;

	public static void Add()
	{
		ContextActionChronoSurge.SurgeMessage = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.ChronoSurge", "<color=#133E71><b>[Chrono-Surge]</b></color> {0} warps the temporal stream! Standard, Move, and Swift actions restored!");
		BlueprintAbilityResource resource = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSurgeResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = new BlueprintAbilityResource.Amount
			{
				BaseValue = 3,
				IncreasedByLevel = false,
				IncreasedByStat = false
			};
		});
		BlueprintAbility ability = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSurgeAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Chrono-Surge: Double Turn");
			bp.SetDescription(Main.IsekaiContext, "As a free action (usable 3 times per day), warp the local temporal stream to immediately reset and refund your Standard, Move, and Swift actions. Does not desync the turn order.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Surge;
			bp.Type = AbilityType.Special;
			bp.Range = AbilityRange.Personal;
			bp.CanTargetSelf = true;
			bp.ActionType = UnitCommand.CommandType.Free;
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = ActionFlow.DoSingle<ContextActionChronoSurge>();
			});
			bp.AddComponent(delegate(AbilityResourceLogic c)
			{
				c.m_RequiredResource = resource.ToReference<BlueprintAbilityResourceReference>();
				c.m_IsSpendResource = true;
				c.Amount = 1;
			});
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicChronoSurgeFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Forbidden Cheat: Chrono-Surge");
			bp.SetDescription(Main.IsekaiContext, "Mastery over temporal flow grants you the Chrono-Surge ability, allowing you to seize a double turn 3 times per day as a free action.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Surge;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { ability.ToReference<BlueprintUnitFactReference>() };
			});
			bp.AddComponent(delegate(AddAbilityResources c)
			{
				c.m_Resource = resource.ToReference<BlueprintAbilityResourceReference>();
				c.RestoreAmount = true;
			});
		});
	}
}
