using System;
using System.Collections.Generic;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	public class CosmicThreatScaling : IUnitCombatHandler, ISubscriber, IGlobalSubscriber
	{
		private static bool _initialized = false;

		private static CosmicThreatScaling _instance;

		private static bool _incursionSpawnedForCurrentCombat = false;

		private static readonly Dictionary<string, BlueprintUnit> _incursionUnits = new Dictionary<string, BlueprintUnit>();

		private static BlueprintBuff _threatBuff;

		private static BlueprintBuff _cheatBuff;

		private static BlueprintBuff _dynamicBuff;

		private static BlueprintBuff _retributionBuff;

		private static BlueprintBuff _milestoneBuff;

		private static BlueprintCharacterClass _isekaiClass;

		public static CosmicThreatScaling Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new CosmicThreatScaling();
				}
				return _instance;
			}
		}

		public static void Add()
		{
			Sprite Icon_Threat = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("247a4068296e8be42890143f451b4b45"))?.m_Icon;
			BlueprintBuff threatBuff = TTCoreExtensions.CreateBuff("CosmicThreatScalingBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Cosmic Threat Scaling");
				bp.SetDescription(Main.IsekaiContext, "To challenge an otherworldly protagonist whose power defies reality, this creature has been adapted by cosmic destiny with enhanced combat reflexes, armor class, and saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Threat;
				bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Simple,
						Value = 6
					};
				});
			});
			BlueprintBuff cheatBuff = TTCoreExtensions.CreateBuff("RampantCheatingCurseBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Cosmic Retribution: Rampant Cheating");
				bp.SetDescription(Main.IsekaiContext, "Outraged by the presence of multiple reality-breaking otherworlders or excessive otherworldly divine enhancements, cosmic balance forces this creature to swell with planar fury: gaining +4 to attack, +4 to AC, +4 to saving throws, an extra attack per round, +100 HP, and +10 spell resistance.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Threat;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.HitPoints;
					c.Value = 100;
				});
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
					c.Haste = true;
				});
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Simple,
						Value = 10
					};
				});
			});
			BlueprintBuff dynamicBuff = TTCoreExtensions.CreateBuff("CosmicDynamicMythicThreatBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Cosmic Dynamic Mythic Threat");
				bp.SetDescription(Main.IsekaiContext, "This creature scales dynamically with the Commander's Mythic Rank, gaining untyped bonuses to attack rolls, armor class, and saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Threat;
				bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
				bp.Ranks = 20;
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Multiplier = 1;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AC;
					c.Multiplier = 1;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveFortitude;
					c.Multiplier = 1;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveReflex;
					c.Multiplier = 1;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveWill;
					c.Multiplier = 1;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.TargetBuffRank;
					c.m_Buff = bp.ToReference<BlueprintBuffReference>();
				});
			});
			BlueprintBuff retributionBuff = TTCoreExtensions.CreateBuff("CosmicRetributionBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Cosmic Retribution: Planar Fury");
				bp.SetDescription(Main.IsekaiContext, "Outraged by the presence of reality-breaking otherworlders and excessive divine enhancements, cosmic balance empowers this creature with scaling combat fury.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Threat;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
				bp.Ranks = 99;
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Multiplier = 2;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AC;
					c.Multiplier = 2;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveFortitude;
					c.Multiplier = 2;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveReflex;
					c.Multiplier = 2;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveWill;
					c.Multiplier = 2;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.HitPoints;
					c.Multiplier = 100;
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Rank,
						ValueRank = AbilityRankType.Default
					};
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.TargetBuffRank;
					c.m_Buff = bp.ToReference<BlueprintBuffReference>();
				});
			});
			BlueprintBuff milestoneBuff = TTCoreExtensions.CreateBuff("CosmicRetributionMilestoneBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Cosmic Retribution: Apex Equilibrium");
				bp.SetDescription(Main.IsekaiContext, "At extreme cheat levels, reality equilibrium grants this creature True Seeing, Freedom of Movement, and an extra attack per round.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Threat;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
					c.Haste = true;
				});
				BlueprintBuff blueprint = BlueprintTools.GetBlueprint<BlueprintBuff>("09b4b69169304474296484c74aa12027");
				BlueprintBuff blueprint2 = BlueprintTools.GetBlueprint<BlueprintBuff>("1533e782fca42b84ea370fc1dcbf4fc1");
				List<BlueprintUnitFactReference> facts = new List<BlueprintUnitFactReference>();
				if (blueprint != null)
				{
					facts.Add(blueprint.ToReference<BlueprintUnitFactReference>());
				}
				if (blueprint2 != null)
				{
					facts.Add(blueprint2.ToReference<BlueprintUnitFactReference>());
				}
				if (facts.Count > 0)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = facts.ToArray();
					});
				}
			});
			_threatBuff = threatBuff;
			_cheatBuff = cheatBuff;
			_dynamicBuff = dynamicBuff;
			_retributionBuff = retributionBuff;
			_milestoneBuff = milestoneBuff;
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

		public void HandleUnitJoinCombat(UnitEntityData unit)
		{
			try
			{
				UnitEntityData unitEntityData = BlueprintSafetyExtensions.SafeGetMainCharacter();
				if (unitEntityData == null || unit == null || !unit.IsEnemy(unitEntityData))
				{
					return;
				}
				if (_isekaiClass == null)
				{
					_isekaiClass = IsekaiProtagonistClass.Get();
				}
				int num = 0;
				int num2 = 0;
				List<UnitEntityData> list = Game.Instance?.Player?.Party;
				if (list != null)
				{
					foreach (UnitEntityData item in list)
					{
						if (item?.Descriptor?.Progression == null)
						{
							continue;
						}
						if (_isekaiClass != null && item.Descriptor.Progression.GetClassLevel(_isekaiClass) > 0)
						{
							num++;
						}
						foreach (Feature feature in item.Descriptor.Progression.Features)
						{
							string text = feature.Blueprint?.name;
							if (!string.IsNullOrEmpty(text) && (text.StartsWith("ItemPrimordialAmbrosia") || text.StartsWith("ItemNectar") || text.StartsWith("ManaWellspring") || text.StartsWith("RealityPiercer") || text.StartsWith("CosmicChronoSurge") || text.StartsWith("GachaJackpot") || text.StartsWith("AegisUndying") || text.StartsWith("DimensionalSanctuary") || text.StartsWith("CosmicSupremeBeing") || text.StartsWith("CosmicAutoQuicken") || text.StartsWith("InstakillFeature")))
							{
								num2 += feature.GetRank();
							}
						}
					}
				}
				int num3 = num2 + Math.Max(0, (num - 1) * 10);
				if (num > 1 || num2 >= 5)
				{
					if (_cheatBuff == null)
					{
						_cheatBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "RampantCheatingCurseBuff");
					}
					if (_cheatBuff != null && unit.Descriptor != null && !unit.Descriptor.HasFact(_cheatBuff))
					{
						unit.Descriptor.AddFact(_cheatBuff);
					}
					ConstellationChatManager.WarnRampantCheating((num > 1) ? num : (num2 / 2));
				}
				int num4 = Math.Max(1, Main.IsekaiContext.AddedContent.CosmicThreatDifficultyMultiplier);
				int num5 = num3 / 3 * num4;
				if (num5 > 50)
				{
					num5 = 50;
				}
				if (num5 > 0 && unit.Descriptor != null)
				{
					if (_retributionBuff == null)
					{
						_retributionBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "CosmicRetributionBuff");
					}
					if (_retributionBuff != null)
					{
						for (int i = unit.Descriptor.Buffs.GetBuff(_retributionBuff)?.Rank ?? 0; i < num5; i++)
						{
							unit.Descriptor.Buffs.AddBuff(_retributionBuff, unit, null);
						}
					}
					if (num5 >= 5)
					{
						if (_milestoneBuff == null)
						{
							_milestoneBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "CosmicRetributionMilestoneBuff");
						}
						if (_milestoneBuff != null && !unit.Descriptor.HasFact(_milestoneBuff))
						{
							unit.Descriptor.AddFact(_milestoneBuff);
						}
					}
				}
				if (!_incursionSpawnedForCurrentCombat && Main.IsekaiContext.AddedContent.EnablePlanarIncursions && (num3 >= 15 || Main.IsekaiContext.AddedContent.EnableIsekaiEncounterMultiplier))
				{
					_incursionSpawnedForCurrentCombat = true;
					SpawnPlanarIncursion(unit, unitEntityData, num3);
				}
				if (!Main.IsekaiContext.AddedContent.EnableCosmicThreatScaling)
				{
					return;
				}
				if ((unitEntityData.Progression?.CharacterLevel ?? 0) >= 10)
				{
					if (_threatBuff == null)
					{
						_threatBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "CosmicThreatScalingBuff");
					}
					if (_threatBuff != null && unit.Descriptor != null && !unit.Descriptor.HasFact(_threatBuff))
					{
						unit.Descriptor.AddFact(_threatBuff);
					}
				}
				int num6 = unitEntityData.Progression?.MythicLevel ?? 0;
				if (num6 <= 0)
				{
					return;
				}
				if (_dynamicBuff == null)
				{
					_dynamicBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "CosmicDynamicMythicThreatBuff");
				}
				if (_dynamicBuff != null && unit.Descriptor != null)
				{
					for (int j = unit.Descriptor.Buffs.GetBuff(_dynamicBuff)?.Rank ?? 0; j < num6; j++)
					{
						unit.Descriptor.Buffs.AddBuff(_dynamicBuff, unit, null);
					}
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in CosmicThreatScaling.HandleUnitJoinCombat: " + ex);
			}
		}

		private static void SpawnPlanarIncursion(UnitEntityData targetEnemy, UnitEntityData player, int totalCheats)
		{
			try
			{
				if (Game.Instance?.LoadedAreaState?.MainState == null)
				{
					return;
				}
				int num = player.Progression?.CharacterLevel ?? 1;
				string text = ((num <= 8) ? "a6dceccaf84af1a42843e58412265388" : ((num <= 14) ? "dbe2e6ca69a41984383093c9ae7bd159" : ((num > 19) ? "995fd185611986c4ca02fcac280b03fa" : "d13545bd34bf2c3438f909417b9c5314")));
				if (!_incursionUnits.TryGetValue(text, out var value) || value == null)
				{
					value = BlueprintTools.GetBlueprint<BlueprintUnit>(text);
					if (value != null)
					{
						_incursionUnits[text] = value;
					}
				}
				if (value == null)
				{
					return;
				}
				int num2 = 2;
				if (num >= 15 || totalCheats >= 25)
				{
					num2 = 3;
				}
				if (num >= 20 || totalCheats >= 40)
				{
					num2 = 4;
				}
				if (Main.IsekaiContext.AddedContent.EnableIsekaiEncounterMultiplier)
				{
					num2 += 2;
				}
				ConstellationChatManager.WarnPlanarIncursion(num2);
				Vector3 position = targetEnemy.Position;
				for (int i = 0; i < num2; i++)
				{
					float x = ((i % 2 == 0) ? 4f : (-4f)) * (float)(1 + i / 2);
					float z = ((i < 2) ? 4f : (-4f));
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
				Main.IsekaiContext.Logger.LogError("Error in SpawnPlanarIncursion: " + ex);
			}
		}

		public void HandleUnitLeaveCombat(UnitEntityData unit)
		{
			if (Game.Instance?.Player != null && !Game.Instance.Player.IsInCombat)
			{
				_incursionSpawnedForCurrentCombat = false;
				ConstellationChatManager.ResetCombatWarning();
			}
		}
	}
}
