using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist;

public class CosmicThreatScaling : IUnitCombatHandler, ISubscriber, IGlobalSubscriber, IPartyCombatHandler
{
	private static bool _initialized = false;

	private static CosmicThreatScaling _instance;

	internal const int BonusPerBuff = 2;

	internal const int FirstThreatLevel = 10;

	internal const int LastThreatLevel = 40;

	internal const int LevelsPerStep = 5;

	internal const StatType HitPointBonusStat = StatType.TemporaryHitPoints;

	internal const int RampantHitPoints = 100;

	internal const string TrueSeeingBuff = "09b4b69169304474296484c74aa12027";

	internal const string FreedomOfMovementBuff = "1533e782fca42b84ea370fc1dcbf4fc1";

	internal const int MythicThreatBonusPerRank = 1;

	internal const int RetributionBonusPerStack = 2;

	internal const int RetributionHitPointsPerStack = 100;

	internal const int RetributionStackCap = 50;

	internal const int RetributionMilestoneStacks = 5;

	internal const int CheatsPerExtraProtagonist = 10;

	internal const int CheatsPerRetributionStack = 3;

	internal static readonly string[] CheatPrefixes = new string[11]
	{
		"ItemPrimordialAmbrosia", "ItemNectar", "ManaWellspring", "RealityPiercer", "CosmicChronoSurge", "GachaJackpot", "AegisUndying", "DimensionalSanctuary", "CosmicSupremeBeing", "CosmicAutoQuicken",
		"InstakillFeature"
	};

	internal const BlueprintBuff.Flags ThreatBuffFlags = BlueprintBuff.Flags.HiddenInUi;

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
		TTCoreExtensions.CreateBuff("CosmicThreatScalingBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Cosmic Threat Scaling");
			bp.SetDescription(Main.IsekaiContext, "To challenge an otherworldly protagonist whose power defies reality, this creature has been adapted by cosmic destiny with enhanced combat reflexes, armor class, and saving throws.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Threat;
			bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
			bp.Stacking = StackingType.Stack;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AC;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
		});
		TTCoreExtensions.CreateBuff("RampantCheatingCurseBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Cosmic Retribution: Rampant Cheating");
			bp.SetDescription(Main.IsekaiContext, "Outraged by the presence of multiple reality-breaking otherworld transmigrants in the same party, cosmic balance forces this creature to swell with planar fury for the rest of this fight: +4 to attack, +4 to AC, +4 to saving throws, an extra attack per round and +100 temporary hit points.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Threat;
			bp.m_Flags = (BlueprintBuff.Flags)0;
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
				c.Stat = StatType.TemporaryHitPoints;
				c.Value = 100;
			});
			bp.AddComponent(delegate(BuffExtraAttack c)
			{
				c.Number = 1;
				c.Haste = false;
			});
		});
		TTCoreExtensions.CreateBuff("CosmicDynamicMythicThreatBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Cosmic Dynamic Mythic Threat");
			bp.SetDescription(Main.IsekaiContext, "This creature scales dynamically with the Commander's Mythic Rank, gaining untyped bonuses to attack rolls, armor class, and saving throws. It gains +1 to each for every mythic rank.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Threat;
			bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
			bp.Stacking = StackingType.Stack;
			AddCombatBonuses(bp, 1, 0);
		});
		TTCoreExtensions.CreateBuff("CosmicRetributionBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Cosmic Retribution: Planar Fury");
			bp.SetDescription(Main.IsekaiContext, "Outraged by the presence of reality-breaking otherworlders and excessive divine enhancements, cosmic balance empowers this creature with scaling combat fury. Each stack grants +2 to attack rolls, armor class and saving throws and +100 temporary hit points.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Threat;
			bp.m_Flags = (BlueprintBuff.Flags)0;
			bp.Stacking = StackingType.Stack;
			AddCombatBonuses(bp, 2, 100);
		});
		BlueprintBuff trueSeeing = BlueprintTools.GetBlueprint<BlueprintBuff>("09b4b69169304474296484c74aa12027");
		BlueprintBuff freedom = BlueprintTools.GetBlueprint<BlueprintBuff>("1533e782fca42b84ea370fc1dcbf4fc1");
		TTCoreExtensions.CreateBuff("CosmicRetributionMilestoneBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Cosmic Retribution: Apex Equilibrium");
			bp.SetDescription(Main.IsekaiContext, "At extreme cheat levels, reality equilibrium grants this creature True Seeing, Freedom of Movement, and an extra attack per round.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Threat;
			bp.m_Flags = (BlueprintBuff.Flags)0;
			bp.AddComponent(delegate(BuffExtraAttack c)
			{
				c.Number = 1;
				c.Haste = false;
			});
			List<BlueprintUnitFactReference> facts = new List<BlueprintUnitFactReference>();
			if (trueSeeing != null)
			{
				facts.Add(trueSeeing.ToReference<BlueprintUnitFactReference>());
			}
			if (freedom != null)
			{
				facts.Add(freedom.ToReference<BlueprintUnitFactReference>());
			}
			if (facts.Count > 0)
			{
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = facts.ToArray();
				});
			}
		});
		Init();
	}

	private static void AddCombatBonuses(BlueprintBuff bp, int bonus, int hitPoints)
	{
		StatType[] array = new StatType[5]
		{
			StatType.AdditionalAttackBonus,
			StatType.AC,
			StatType.SaveFortitude,
			StatType.SaveReflex,
			StatType.SaveWill
		};
		foreach (StatType stat in array)
		{
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = stat;
				c.Value = bonus;
			});
		}
		if (hitPoints > 0)
		{
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.TemporaryHitPoints;
				c.Value = hitPoints;
			});
		}
	}

	internal static bool IsCheat(string featureName)
	{
		if (string.IsNullOrEmpty(featureName))
		{
			return false;
		}
		string[] cheatPrefixes = CheatPrefixes;
		foreach (string value in cheatPrefixes)
		{
			if (featureName.StartsWith(value, StringComparison.Ordinal))
			{
				return true;
			}
		}
		return false;
	}

	internal static int CheatScore(int cheatRanks, int protagonistsInParty)
	{
		return Math.Max(0, cheatRanks) + Math.Max(0, protagonistsInParty - 1) * 10;
	}

	internal static int RetributionStacks(bool enabled, int cheatScore, int configuredMultiplier)
	{
		if (!enabled)
		{
			return 0;
		}
		return Math.Min(50, Math.Max(0, cheatScore) / 3 * ThreatStacks(configuredMultiplier));
	}

	internal static int MythicThreatStacks(bool enabled, int mythicLevel)
	{
		if (!enabled)
		{
			return 0;
		}
		return Math.Max(0, mythicLevel);
	}

	private static int PartyCheatRanks()
	{
		int num = 0;
		List<UnitEntityData> list = Game.Instance?.Player?.Party;
		if (list == null)
		{
			return 0;
		}
		foreach (UnitEntityData item in list)
		{
			FeatureCollection featureCollection = item?.Descriptor?.Progression?.Features;
			if (featureCollection == null)
			{
				continue;
			}
			foreach (Feature item2 in featureCollection)
			{
				if (item2?.Blueprint != null && IsCheat(item2.Blueprint.name))
				{
					num += Math.Max(1, item2.GetRank());
				}
			}
		}
		return num;
	}

	private static void SetStacks(UnitEntityData unit, BlueprintBuff buff, int wanted)
	{
		if (buff != null)
		{
			List<Buff> list = BuffsOf(unit, buff);
			for (int i = list.Count; i < wanted; i++)
			{
				unit.Descriptor.Buffs.AddBuff(buff, unit, null);
			}
			for (int j = wanted; j < list.Count; j++)
			{
				list[j].Remove();
			}
		}
	}

	internal static int ThreatStacks(int configuredMultiplier)
	{
		return Math.Min(5, Math.Max(1, configuredMultiplier));
	}

	internal static int ThreatBonusForLevel(int level)
	{
		if (level < 10)
		{
			return 0;
		}
		int num = Math.Min(level, 40);
		return 2 * (1 + (num - 10) / 5);
	}

	internal static int ThreatBuffCount(bool enabled, int partyLevel, int configuredMultiplier)
	{
		if (!enabled)
		{
			return 0;
		}
		return ThreatBonusForLevel(partyLevel) / 2 * ThreatStacks(configuredMultiplier);
	}

	internal static int TotalBonus(int buffCount)
	{
		return Math.Max(0, buffCount) * 2;
	}

	internal static bool RampantApplies(bool enabled, int protagonistsInParty)
	{
		if (enabled)
		{
			return protagonistsInParty > 1;
		}
		return false;
	}

	public static void Init()
	{
		if (!_initialized)
		{
			_initialized = true;
			EventBus.Subscribe(Instance);
		}
	}

	private static List<Buff> BuffsOf(UnitEntityData unit, BlueprintBuff blueprint)
	{
		List<Buff> list = unit?.Descriptor?.Buffs?.RawFacts;
		if (list == null || blueprint == null)
		{
			return new List<Buff>();
		}
		return list.Where((Buff b) => b != null && b.Blueprint == blueprint).ToList();
	}

	public void HandleUnitJoinCombat(UnitEntityData unit)
	{
		try
		{
			if (CampaignEncounterGuard.Excludes(unit) || unit.Descriptor == null || !CampaignEncounterGuard.IsPartysEnemy(unit))
			{
				return;
			}
			int num = CampaignEncounterGuard.ProtagonistsInParty();
			BlueprintBuff modBlueprint = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "RampantCheatingCurseBuff");
			if (modBlueprint != null)
			{
				List<Buff> list = BuffsOf(unit, modBlueprint);
				if (RampantApplies(Main.IsekaiContext.AddedContent.EnableRampantCheating, num))
				{
					if (list.Count == 0)
					{
						unit.Descriptor.Buffs.AddBuff(modBlueprint, unit, null);
					}
					ConstellationChatManager.WarnRampantCheating(num);
				}
				else
				{
					foreach (Buff item in list)
					{
						item.Remove();
					}
				}
			}
			BlueprintBuff modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "CosmicThreatScalingBuff");
			bool enabled = CampaignEncounterGuard.AppliesToParty(Main.IsekaiContext.AddedContent.EnableCosmicThreatScaling, num);
			if (modBlueprint2 != null)
			{
				int num2 = ThreatBuffCount(enabled, CampaignEncounterGuard.PartyLevel(), Main.IsekaiContext.AddedContent.CosmicThreatDifficultyMultiplier);
				List<Buff> list2 = BuffsOf(unit, modBlueprint2);
				for (int i = list2.Count; i < num2; i++)
				{
					unit.Descriptor.Buffs.AddBuff(modBlueprint2, unit, null);
				}
				for (int j = num2; j < list2.Count; j++)
				{
					list2[j].Remove();
				}
			}
			BlueprintBuff modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "CosmicDynamicMythicThreatBuff");
			int valueOrDefault = (Game.Instance?.Player?.MainCharacter.Value?.Progression?.MythicLevel).GetValueOrDefault();
			SetStacks(unit, modBlueprint3, MythicThreatStacks(enabled, valueOrDefault));
			BlueprintBuff modBlueprint4 = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "CosmicRetributionBuff");
			BlueprintBuff modBlueprint5 = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "CosmicRetributionMilestoneBuff");
			bool num3 = CampaignEncounterGuard.AppliesToParty(Main.IsekaiContext.AddedContent.EnableRampantCheating, num);
			int num4 = RetributionStacks(num3, num3 ? CheatScore(PartyCheatRanks(), num) : 0, Main.IsekaiContext.AddedContent.CosmicThreatDifficultyMultiplier);
			SetStacks(unit, modBlueprint4, num4);
			SetStacks(unit, modBlueprint5, (num4 >= 5) ? 1 : 0);
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error in CosmicThreatScaling.HandleUnitJoinCombat: " + ex);
		}
	}

	public void HandleUnitLeaveCombat(UnitEntityData unit)
	{
		try
		{
			if (!(unit != null))
			{
				return;
			}
			BlueprintBuff modBlueprint = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "RampantCheatingCurseBuff");
			foreach (Buff item in BuffsOf(unit, modBlueprint))
			{
				item.Remove();
			}
			BlueprintBuff modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "CosmicThreatScalingBuff");
			foreach (Buff item2 in BuffsOf(unit, modBlueprint2))
			{
				item2.Remove();
			}
			string[] array = new string[3] { "CosmicDynamicMythicThreatBuff", "CosmicRetributionBuff", "CosmicRetributionMilestoneBuff" };
			foreach (string name in array)
			{
				BlueprintBuff modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, name);
				foreach (Buff item3 in BuffsOf(unit, modBlueprint3))
				{
					item3.Remove();
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error in CosmicThreatScaling.HandleUnitLeaveCombat: " + ex);
		}
	}

	public void HandlePartyCombatStateChanged(bool inCombat)
	{
		if (!inCombat)
		{
			ConstellationChatManager.ResetCombatWarning();
		}
	}
}
