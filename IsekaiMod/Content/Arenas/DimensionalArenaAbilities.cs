using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Arenas
{
	public static class DimensionalArenaAbilities
	{
		public static BlueprintAbility SummonFaeGroveAbility;

		public static BlueprintAbility SummonLostLegionsAbility;

		public static BlueprintAbility SummonMidnightColosseumAbility;

		public static BlueprintAbility SummonVoidRiftAbility;

		public static BlueprintAbility SummonMultiverseThresholdAbility;

		public static BlueprintFeature DimensionalArenaFeature;

		public static void Add()
		{
			Sprite Icon_Arena = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
			SummonFaeGroveAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonFaeGroveAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Dimensional Rift: Primal Fae Grove (Act 1)");
				bp.SetDescription(Main.IsekaiContext, "Tears open a spatial rift to the First World, initiating a 4-wave horde battle against hostile Satyrs, Redcaps, Nymphs, and a Hamadryad Queen.\nClearing all waves grants massive Experience, Cosmic Coins, and constellation favor for testing your power.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Arena;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionStartDimensionalArena a)
					{
						a.Arena = ArenaType.Act1FaeGrove;
					});
				});
			});
			SummonLostLegionsAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonLostLegionsAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Dimensional Rift: Lost Legions (Act 2)");
				bp.SetDescription(Main.IsekaiContext, "Tears open a rift to the haunted graveyard of forgotten crusaders, summoning waves of Ghouls, Wights, Skeletal Champions, and a Ghostly Warden.\nClearing all waves grants 15,000 XP and 1,200 Cosmic Coins.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Arena;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionStartDimensionalArena a)
					{
						a.Arena = ArenaType.Act2LostLegions;
					});
				});
			});
			SummonMidnightColosseumAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonMidnightColosseumAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Dimensional Rift: Midnight Colosseum (Act 3)");
				bp.SetDescription(Main.IsekaiContext, "Tears open a rift to the Abyssal gladiatorial arenas, summoning deadly Derakni, Schir executioners, Coloxus infiltrators, and Glabrezu juggernauts.\nClearing all waves grants 35,000 XP and 2,000 Cosmic Coins.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Arena;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionStartDimensionalArena a)
					{
						a.Arena = ArenaType.Act3MidnightColosseum;
					});
				});
			});
			SummonVoidRiftAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonVoidRiftAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Dimensional Rift: Abyssal Void Rift (Act 4)");
				bp.SetDescription(Main.IsekaiContext, "Tears open a rift to the deepest planar tears of the Abyss, summoning six-armed Marilith blade-masters, serpentine Vavakia dreadnoughts, Balor lords, and an axiomatic Inevitable.\nClearing all waves grants 75,000 XP and 3,500 Cosmic Coins.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Arena;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionStartDimensionalArena a)
					{
						a.Arena = ArenaType.Act4VoidRift;
					});
				});
			});
			SummonMultiverseThresholdAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonMultiverseThresholdAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Dimensional Rift: Multiverse Threshold (Act 5)");
				bp.SetDescription(Main.IsekaiContext, "Tears open an apocalyptic tear at the apex of the multiverse, facing apex mythic demons, high-tier Coloxus archmages, and supreme Inevitable Kolyarut enforcers.\nClearing all waves grants 150,000 XP and 5,000 Cosmic Coins.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Arena;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionStartDimensionalArena a)
					{
						a.Arena = ArenaType.Act5MultiverseThreshold;
					});
				});
			});
			DimensionalArenaFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "DimensionalArenaFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Dimensional Battlegrounds");
				bp.SetDescription(Main.IsekaiContext, "Grants you the ability to rip open dimensional vortexes into other planes and battlegrounds across Acts 1-5 to test your isekai powers, conquer hordes of enemies, and win Cosmic Coins.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Arena;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[5]
					{
						SummonFaeGroveAbility.ToReference<BlueprintUnitFactReference>(),
						SummonLostLegionsAbility.ToReference<BlueprintUnitFactReference>(),
						SummonMidnightColosseumAbility.ToReference<BlueprintUnitFactReference>(),
						SummonVoidRiftAbility.ToReference<BlueprintUnitFactReference>(),
						SummonMultiverseThresholdAbility.ToReference<BlueprintUnitFactReference>()
					};
				});
			});
		}
	}
}
