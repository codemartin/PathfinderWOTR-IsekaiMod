using System;
using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist;

public class RivalReincarnators : IUnitHandler, ISubscriber, IGlobalSubscriber, IUnitSpawnHandler, IUnitCombatHandler
{
	private static bool _initialized;

	private static RivalReincarnators _instance;

	internal const int RivalBounty = 100;

	internal const int RivalOdds = 10;

	internal const string RivalStorageName = "IsekaiMod.Rival";

	internal const string PaidKey = "Paid";

	internal const string DefeatSponsor = "The Pallid Princess";

	public static RivalReincarnators Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new RivalReincarnators();
			}
			return _instance;
		}
	}

	public static void Add()
	{
		Sprite Icon_Rival = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("247a4068296e8be42890143f451b4b45"))?.m_Icon;
		TTCoreExtensions.CreateBuff("RivalReincarnatorBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Rival Reincarnator");
			bp.SetDescription(Main.IsekaiContext, "A rival transmigrated soul from beyond the astral plane: +4 luck bonus to attack and AC and 20% concealment. Defeating it pays 100 Cosmic Coins, once.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Rival;
			bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Luck;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Luck;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddConcealment c)
			{
				c.Concealment = Concealment.Partial;
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

	internal static bool IsRivalCandidate(int cr, int partyLevel, bool summoned, bool storyBoss, bool plotProtected)
	{
		if (summoned | storyBoss | plotProtected)
		{
			return false;
		}
		if (partyLevel <= 0)
		{
			return false;
		}
		return cr >= partyLevel;
	}

	internal static bool RollsRival(string uniqueId)
	{
		if (string.IsNullOrEmpty(uniqueId))
		{
			return false;
		}
		return CampaignEncounterGuard.StableHash(uniqueId) % 10 == 0;
	}

	internal static int SettleRivalDefeat(IDictionary<string, string> rivalStorage)
	{
		if (rivalStorage == null)
		{
			return 0;
		}
		if (SaveScopedStorage.GetFlag(rivalStorage, "Paid"))
		{
			return 0;
		}
		SaveScopedStorage.SetFlag(rivalStorage, "Paid", value: true);
		return 100;
	}

	internal static string RevealLine(string name)
	{
		return ConstellationText.LocalizedFormat("Constellation.RivalReincarnator.Reveal", "<color=#7A1010><b>[Rival Reincarnator]</b></color> <b>{0}</b> is another transmigrated soul: +4 to attack and AC, 20% concealment. Defeating it pays {1} Cosmic Coins, once.", name, 100);
	}

	internal static string DefeatLine(string name, int coins)
	{
		return ConstellationText.LocalizedFormat("Constellation.RivalReincarnator.Defeat", "{0}: <i>\"Another world's soul, and it rots just like yours will. <b>{1}</b> is mine now. Take your {2} coins.\"</i>", ConstellationText.LocalizeIfKnown("<color=#7A1010><b>[The Constellation 'The Pallid Princess']</b></color>"), name, coins);
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

	public void OnUnitDie(UnitEntityData unit)
	{
		try
		{
			if (CampaignEncounterGuard.Excludes(unit))
			{
				return;
			}
			BlueprintBuff modBlueprint = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "RivalReincarnatorBuff");
			if (modBlueprint == null)
			{
				return;
			}
			UnitDescriptor descriptor = unit.Descriptor;
			if (descriptor != null && descriptor.HasFact(modBlueprint))
			{
				int num = SettleRivalDefeat(SaveScopedStorage.Ensure(unit, "IsekaiMod.Rival"));
				if (num > 0)
				{
					DivineTokens.AddClaimedCoins(num, ConstellationText.Localized("CosmicCoins.Sponsor", "Rival Reincarnator Defeat"));
					Main.IsekaiContext.Logger.Log($"[Rival Reincarnator] Defeated rival {unit.CharacterName}! Awarded {num} Cosmic Coins.");
					ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Voice, DefeatLine(unit.CharacterName, num), "The Pallid Princess", ConstellationCategory.Combat, "Rival Reincarnator");
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error in RivalReincarnators.OnUnitDie: " + ex);
		}
	}

	public void HandleUnitJoinCombat(UnitEntityData unit)
	{
		try
		{
			if (!CampaignEncounterGuard.Excludes(unit) && CampaignEncounterGuard.AppliesToParty(Main.IsekaiContext.AddedContent.EnableRivalReincarnators, CampaignEncounterGuard.ProtagonistsInParty()) && unit.Descriptor != null && CampaignEncounterGuard.IsPartysEnemy(unit))
			{
				BlueprintBuff modBlueprint = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "RivalReincarnatorBuff");
				if (modBlueprint != null && !unit.Descriptor.HasFact(modBlueprint) && SaveScopedStorage.SpawnKindOf(unit) == null && IsRivalCandidate(unit.Blueprint?.CR ?? 0, CampaignEncounterGuard.PartyLevel(), CampaignEncounterGuard.IsSummonedUnit(unit), CampaignEncounterGuard.IsBoss(unit.Blueprint), CampaignEncounterGuard.IsPlotProtected(unit)) && RollsRival(unit.UniqueId))
				{
					unit.Descriptor.AddFact(modBlueprint);
					ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Notice, RevealLine(unit.CharacterName), null, ConstellationCategory.Combat, "Rival Reincarnator");
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error in RivalReincarnators.HandleUnitJoinCombat: " + ex);
		}
	}

	public void HandleUnitLeaveCombat(UnitEntityData unit)
	{
	}
}
