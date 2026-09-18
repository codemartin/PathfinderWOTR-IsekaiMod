using System;
using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	public class DimensionalReinforcements : IAreaHandler, ISubscriber, IGlobalSubscriber, IGlobalRulebookHandler<RuleDealDamage>, IRulebookHandler<RuleDealDamage>, IGlobalRulebookSubscriber
	{
		private static bool _initialized = false;

		private static DimensionalReinforcements _instance;

		private static readonly HashSet<string> _triggeredBossIds = new HashSet<string>();

		private static readonly Dictionary<BlueprintUnit, bool> _bossCheckCache = new Dictionary<BlueprintUnit, bool>();

		private static readonly Dictionary<string, BlueprintUnit> _summonCache = new Dictionary<string, BlueprintUnit>();

		private static BlueprintBuff _aegisBuff;

		public static DimensionalReinforcements Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new DimensionalReinforcements();
				}
				return _instance;
			}
		}

		public static bool HasTriggered(string uniqueId)
		{
			return _triggeredBossIds.Contains(uniqueId);
		}

		public static bool IsTrueBoss(UnitEntityData unit)
		{
			if (unit?.Blueprint == null)
			{
				return false;
			}
			if (_bossCheckCache.TryGetValue(unit.Blueprint, out var value))
			{
				return value;
			}
			int num = unit.Progression?.CharacterLevel ?? 0;
			int cR = unit.Blueprint.CR;
			string text = unit.Blueprint.name ?? "";
			value = cR >= 20 || num >= 20 || text.IndexOf("Boss", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("Leader", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("Lord", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("Areelu", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("Baphomet", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("Deskari", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("Nocticula", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("Mephistopheles", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("InevitableKolyarut", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("BarrenBoss", StringComparison.OrdinalIgnoreCase) >= 0;
			_bossCheckCache[unit.Blueprint] = value;
			return value;
		}

		public static void Add()
		{
			Sprite Icon_Threat = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("247a4068296e8be42890143f451b4b45"))?.m_Icon;
			_aegisBuff = TTCoreExtensions.CreateBuff("DimensionalAegisBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Cosmic Phase Gate: Dimensional Aegis");
				bp.SetDescription(Main.IsekaiContext, "Protected by impenetrable dimensional stasis for 1 round while reinforcements breach reality. Incoming physical and energy damage is completely negated.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Threat;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 999;
					c.BypassedByMaterial = false;
					c.BypassedByAlignment = false;
					c.BypassedByForm = false;
					c.BypassedByMagic = false;
					c.BypassedByReality = false;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Fire;
					c.Value = 999;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = 999;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Electricity;
					c.Value = 999;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Acid;
					c.Value = 999;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Sonic;
					c.Value = 999;
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

		public void OnAreaDidLoad()
		{
			_triggeredBossIds.Clear();
		}

		public void OnAreaBeginUnloading()
		{
			_triggeredBossIds.Clear();
		}

		public void OnEventAboutToTrigger(RuleDealDamage evt)
		{
			try
			{
				if (!Main.IsekaiContext.AddedContent.EnableDimensionalReinforcements || !Main.IsekaiContext.AddedContent.EnableBossPhaseGate || evt?.Target == null)
				{
					return;
				}
				UnitEntityData target = evt.Target;
				UnitEntityData unitEntityData = BlueprintSafetyExtensions.SafeGetMainCharacter();
				if (unitEntityData == null || !target.IsEnemy(unitEntityData) || _triggeredBossIds.Contains(target.UniqueId) || !IsTrueBoss(target))
				{
					return;
				}
				int maxHP = target.MaxHP;
				int hPLeft = target.HPLeft;
				if (maxHP > 0 && hPLeft > maxHP / 2)
				{
					int num = maxHP / 2;
					if (!evt.MinHPAfterDamage.HasValue || evt.MinHPAfterDamage < num)
					{
						evt.MinHPAfterDamage = num;
					}
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in DimensionalReinforcements.OnEventAboutToTrigger: " + ex);
			}
		}

		public void OnEventDidTrigger(RuleDealDamage evt)
		{
			CheckAndTriggerBossPhase(evt?.Target);
		}

		private static void CheckAndTriggerBossPhase(UnitEntityData target)
		{
			try
			{
				if (!Main.IsekaiContext.AddedContent.EnableDimensionalReinforcements || target == null)
				{
					return;
				}
				UnitEntityData unitEntityData = BlueprintSafetyExtensions.SafeGetMainCharacter();
				if (unitEntityData == null || !target.IsEnemy(unitEntityData) || _triggeredBossIds.Contains(target.UniqueId) || !IsTrueBoss(target))
				{
					return;
				}
				int maxHP = target.MaxHP;
				int hPLeft = target.HPLeft;
				if (maxHP > 0 && hPLeft <= maxHP / 2 && hPLeft > 0)
				{
					_triggeredBossIds.Add(target.UniqueId);
					CleanseCrowdControl(target);
					if (_aegisBuff == null)
					{
						_aegisBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "DimensionalAegisBuff");
					}
					if (_aegisBuff != null && target.Descriptor != null)
					{
						target.Descriptor.AddBuff(_aegisBuff, target, TimeSpan.FromSeconds(6.0));
					}
					ConstellationChatManager.WarnBossPhaseGate(target.CharacterName);
					SpawnReinforcements(target, unitEntityData);
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in DimensionalReinforcements.CheckAndTriggerBossPhase: " + ex);
			}
		}

		private static void CleanseCrowdControl(UnitEntityData unit)
		{
			if (unit?.Descriptor?.Buffs == null)
			{
				return;
			}
			try
			{
				List<Buff> list = new List<Buff>();
				foreach (Buff buff in unit.Descriptor.Buffs)
				{
					if (buff.Blueprint != null && (buff.Blueprint.SpellDescriptor & (SpellDescriptor.MindAffecting | SpellDescriptor.Stun | SpellDescriptor.Paralysis | SpellDescriptor.Blindness | SpellDescriptor.Sleep | SpellDescriptor.Petrified | SpellDescriptor.MovementImpairing)) != SpellDescriptor.None)
					{
						list.Add(buff);
					}
				}
				foreach (Buff item in list)
				{
					item.Remove();
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error cleansing crowd control on boss: " + ex);
			}
		}

		private static void SpawnReinforcements(UnitEntityData boss, UnitEntityData player)
		{
			try
			{
				if (Game.Instance?.LoadedAreaState?.MainState == null)
				{
					return;
				}
				Main.IsekaiContext.Logger.Log("[Dimensional Reinforcements] Rift opened near " + boss.CharacterName + "!");
				int num = player?.Progression?.CharacterLevel ?? boss.Progression?.CharacterLevel ?? 1;
				string text = ((num <= 8) ? "a6dceccaf84af1a42843e58412265388" : ((num <= 14) ? "dbe2e6ca69a41984383093c9ae7bd159" : ((num > 19) ? "995fd185611986c4ca02fcac280b03fa" : "d13545bd34bf2c3438f909417b9c5314")));
				if (!_summonCache.TryGetValue(text, out var value) || value == null)
				{
					value = BlueprintTools.GetBlueprint<BlueprintUnit>(text);
					if (value != null)
					{
						_summonCache[text] = value;
					}
				}
				if (value == null)
				{
					return;
				}
				Vector3 position = boss.Position;
				int num2 = (Main.IsekaiContext.AddedContent.EnableIsekaiEncounterMultiplier ? 4 : 2);
				if (Main.IsekaiContext.AddedContent.CosmicThreatDifficultyMultiplier >= 3)
				{
					num2 += 2;
				}
				for (int i = 0; i < num2; i++)
				{
					float x = ((i % 2 == 0) ? 3f : (-3f)) * (float)(1 + i / 2);
					float z = ((i < 2) ? 3f : (-3f));
					Vector3 vector = position + new Vector3(x, 0f, z);
					if (NavMesh.SamplePosition(vector, out var hit, 10f, -1))
					{
						vector = hit.position;
					}
					UnitEntityData unitEntityData = Game.Instance.EntityCreator.SpawnUnit(value, vector, Quaternion.identity, Game.Instance.LoadedAreaState.MainState);
					if (unitEntityData != null)
					{
						unitEntityData.SwitchFactions(BlueprintRoot.Instance.Cheats.Enemy.Faction, resetAttackFactions: true);
						if (player != null)
						{
							unitEntityData.CombatState?.Engage(player);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error spawning dimensional reinforcements: " + ex);
			}
		}
	}
}
