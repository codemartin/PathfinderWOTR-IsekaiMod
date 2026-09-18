using System;
using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace IsekaiMod.Content.Arenas
{
	public class CosmicBarrenBosses : IUnitHandler, ISubscriber, IGlobalSubscriber, IUnitSpawnHandler, IAreaHandler
	{
		private static bool _initialized = false;

		private static CosmicBarrenBosses _instance;

		private static readonly Dictionary<string, int> _trackedBossRewards = new Dictionary<string, int>();

		public static CosmicBarrenBosses Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new CosmicBarrenBosses();
				}
				return _instance;
			}
		}

		public static void Add()
		{
			Sprite Icon_Boss = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
			BlueprintAbility BossAct1Ability = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonBarrenBossAct1Ability", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Barren Anomaly: The Outer World Stray (Act 1)");
				bp.SetDescription(Main.IsekaiContext, "Summons an otherworldly stray who reincarnated into Kenabres with unfamiliar firearm sorcery. Defeating them awards 300 Cosmic Coins.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Boss;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionSpawnBarrenBoss a)
					{
						a.BossType = BarrenBossType.Act1OuterWorldStray;
					});
				});
			});
			BlueprintAbility BossAct2Ability = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonBarrenBossAct2Ability", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Barren Anomaly: Chronos Exile (Act 2)");
				bp.SetDescription(Main.IsekaiContext, "Summons a temporal exile wandering the desolation of the Worldwound. Defeating them awards 600 Cosmic Coins.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Boss;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionSpawnBarrenBoss a)
					{
						a.BossType = BarrenBossType.Act2ChronosExile;
					});
				});
			});
			BlueprintAbility BossAct3Ability = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonBarrenBossAct3Ability", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Barren Anomaly: The Transdimensional Behemoth (Act 3)");
				bp.SetDescription(Main.IsekaiContext, "Summons a colossal entity drawn through planar fissures. Defeating them awards 1,000 Cosmic Coins.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Boss;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionSpawnBarrenBoss a)
					{
						a.BossType = BarrenBossType.Act3TransdimensionalBehemoth;
					});
				});
			});
			BlueprintAbility BossAct4Ability = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonBarrenBossAct4Ability", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Barren Anomaly: Abyssal Void Dragon (Act 4)");
				bp.SetDescription(Main.IsekaiContext, "Summons a cosmic dragon mutated by the Midnight Fane and the Abyss. Defeating them awards 1,500 Cosmic Coins.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Boss;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionSpawnBarrenBoss a)
					{
						a.BossType = BarrenBossType.Act4AbyssalVoidDragon;
					});
				});
			});
			BlueprintAbility BossAct5Ability = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonBarrenBossAct5Ability", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Barren Anomaly: Cosmic Arbiter of Ruin (Act 5)");
				bp.SetDescription(Main.IsekaiContext, "Summons the ultimate multiversal arbiter arriving to pass judgment upon your reincarnated soul. Defeating them awards 2,500 Cosmic Coins.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Boss;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionSpawnBarrenBoss a)
					{
						a.BossType = BarrenBossType.Act5CosmicArbiterOfRuin;
					});
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicBarrenEncountersFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Cosmic Barren Encounters");
				bp.SetDescription(Main.IsekaiContext, "Grants the ability to summon 5 challenging optional cosmic boss encounters across Acts 1 through 5, awarding massive Cosmic Coins.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Boss;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[5]
					{
						BossAct1Ability.ToReference<BlueprintUnitFactReference>(),
						BossAct2Ability.ToReference<BlueprintUnitFactReference>(),
						BossAct3Ability.ToReference<BlueprintUnitFactReference>(),
						BossAct4Ability.ToReference<BlueprintUnitFactReference>(),
						BossAct5Ability.ToReference<BlueprintUnitFactReference>()
					};
				});
			});
			Init();
		}

		public static void Init()
		{
			if (!_initialized)
			{
				_initialized = true;
				EventBus.Subscribe(Instance);
			}
		}

		public void HandleUnitDeath(UnitEntityData unit)
		{
			OnUnitDie(unit);
		}

		public void HandleUnitDestroyed(UnitEntityData unit)
		{
		}

		public void HandleUnitSpawned(UnitEntityData unit)
		{
		}

		public void OnAreaDidLoad()
		{
			_trackedBossRewards.Clear();
		}

		public void OnAreaBeginUnloading()
		{
			_trackedBossRewards.Clear();
		}

		public static void SpawnBoss(BarrenBossType bossType)
		{
			try
			{
				BlueprintArea currentlyLoadedArea = Game.Instance.CurrentlyLoadedArea;
				if (currentlyLoadedArea != null && currentlyLoadedArea.IsGlobalMap)
				{
					EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
					{
						h.HandleLogMessage("<color=#DC143C><b>[Cosmic Encounter]</b></color> Cosmic entities cannot manifest while traversing the Worldwound overland. Enter a localized area or the Planar Coliseum first!");
					});
					return;
				}
				UnitEntityData unitEntityData = ((Game.Instance?.Player != null) ? Game.Instance.Player.MainCharacter.Value : null);
				if (unitEntityData == null)
				{
					return;
				}
				Game instance = Game.Instance;
				if (instance != null && instance.Player?.IsInCombat == true)
				{
					EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
					{
						h.HandleLogMessage("<color=#DC143C><b>[Cosmic Anomaly]</b></color> You cannot summon a cosmic anomaly while already engaged in battle!");
					});
					return;
				}
				int num = 300;
				string id = "6114adae73a1338498412b3c63c946f8";
				switch (bossType)
				{
				case BarrenBossType.Act1OuterWorldStray:
					num = 300;
					id = "6114adae73a1338498412b3c63c946f8";
					break;
				case BarrenBossType.Act2ChronosExile:
					num = 600;
					id = "2e4a9f6fa84651148bc065c5330817fe";
					break;
				case BarrenBossType.Act3TransdimensionalBehemoth:
					num = 1000;
					id = "b8972cfe36e3cd945bbd2c4c320d5237";
					break;
				case BarrenBossType.Act4AbyssalVoidDragon:
					num = 1500;
					id = "370039781f3a4004ab4e3ac6c7032573";
					break;
				case BarrenBossType.Act5CosmicArbiterOfRuin:
					num = 2500;
					id = "3b4b1b05783044f0b75f6f3afd9e6d7f";
					break;
				}
				BlueprintUnit unitBp = BlueprintTools.GetBlueprint<BlueprintUnit>(id);
				if (unitBp == null || Game.Instance?.LoadedAreaState?.MainState == null)
				{
					return;
				}
				Vector3 vector = unitEntityData.Position + unitEntityData.OrientationDirection * 8f;
				if (NavMesh.SamplePosition(vector, out var hit, 15f, -1))
				{
					vector = hit.position;
				}
				UnitEntityData unitEntityData2 = Game.Instance.EntityCreator.SpawnUnit(unitBp, vector, Quaternion.identity, Game.Instance.LoadedAreaState.MainState);
				if (unitEntityData2 != null)
				{
					unitEntityData2.SwitchFactions(BlueprintRoot.Instance.Cheats.Enemy.Faction, resetAttackFactions: true);
					unitEntityData2.CombatState?.Engage(unitEntityData);
					_trackedBossRewards[unitEntityData2.UniqueId] = num;
					EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
					{
						h.HandleLogMessage("<color=#DC143C><b>[Cosmic Anomaly]</b></color> <b>" + unitBp.CharacterName + "</b> has emerged from the planar fissure!");
					});
					Main.IsekaiContext.Logger.Log($"[Cosmic Barren Bosses] Spawned {bossType} (Reward: {num} Cosmic Coins).");
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in SpawnBoss: " + ex);
			}
		}

		public void OnUnitDie(UnitEntityData unit)
		{
			if (!(unit == null) && _trackedBossRewards.TryGetValue(unit.UniqueId, out var reward))
			{
				_trackedBossRewards.Remove(unit.UniqueId);
				DivineTokens.AddCoins(reward, "Barren Anomaly Victory");
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage($"<color=#FFD700><b>[Cosmic Anomaly Defeated]</b></color> You triumphed over <b>{unit.CharacterName}</b> and earned <b>+{reward} Cosmic Coins</b>!");
				});
				Main.IsekaiContext.Logger.Log($"[Cosmic Barren Bosses] Optional boss defeated! Awarded {reward} Cosmic Coins.");
			}
		}
	}
}
