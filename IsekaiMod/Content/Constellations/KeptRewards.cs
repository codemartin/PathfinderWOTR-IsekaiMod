using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Features;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace IsekaiMod.Content.Constellations;

internal static class KeptRewards
{
	internal enum BuffRepair
	{
		None,
		Add,
		MakePermanent
	}

	private sealed class KeptRewardsHandler : IAreaHandler, ISubscriber, IGlobalSubscriber, IUnitChangedAfterRespecHandler
	{
		public void OnAreaDidLoad()
		{
			RestoreMissing();
		}

		public void OnAreaBeginUnloading()
		{
		}

		public void HandleUnitChangedAfterRespec(UnitEntityData unit)
		{
			if (unit != null && unit == Game.Instance?.Player?.MainCharacter.Value)
			{
				RestoreMissing();
			}
		}
	}

	private static readonly Dictionary<string, BlueprintBuffReference> PermanentBuffByClaim = new Dictionary<string, BlueprintBuffReference>(StringComparer.Ordinal);

	private static readonly Dictionary<string, BlueprintFeatureReference> FeatureByClaim = new Dictionary<string, BlueprintFeatureReference>(StringComparer.Ordinal);

	private static bool s_subscribed;

	internal static BlueprintFeature KeepThroughRespec(BlueprintFeature feature)
	{
		if (feature != null)
		{
			feature.IsClassFeature = false;
		}
		return feature;
	}

	internal static BlueprintBuff KeepPermanent(BlueprintBuff buff)
	{
		if (buff == null)
		{
			return null;
		}
		buff.IsClassFeature = false;
		buff.m_Flags |= BlueprintBuff.Flags.StayOnDeath;
		return buff;
	}

	internal static bool IsPermanentDuration(int buffMinutes)
	{
		return buffMinutes <= 0;
	}

	internal static void RegisterClaimReward(string claimKey, BlueprintBuff buff, int buffMinutes, BlueprintFeature feature)
	{
		if (!string.IsNullOrEmpty(claimKey))
		{
			if (buff != null && IsPermanentDuration(buffMinutes))
			{
				KeepPermanent(buff);
				PermanentBuffByClaim[claimKey] = buff.ToReference<BlueprintBuffReference>();
			}
			if (feature != null)
			{
				KeepThroughRespec(feature);
				FeatureByClaim[claimKey] = feature.ToReference<BlueprintFeatureReference>();
			}
		}
	}

	internal static IEnumerable<KeyValuePair<string, BlueprintBuffReference>> PermanentBuffRewards()
	{
		return PermanentBuffByClaim.ToList();
	}

	internal static IEnumerable<KeyValuePair<string, BlueprintFeatureReference>> FeatureRewards()
	{
		return FeatureByClaim.ToList();
	}

	internal static BuffRepair DecideBuff(bool claimed, bool present, bool permanent)
	{
		if (!claimed)
		{
			return BuffRepair.None;
		}
		if (!present)
		{
			return BuffRepair.Add;
		}
		if (!permanent)
		{
			return BuffRepair.MakePermanent;
		}
		return BuffRepair.None;
	}

	internal static bool FeatureMissing(bool claimed, bool present)
	{
		if (claimed)
		{
			return !present;
		}
		return false;
	}

	internal static bool ShardMissing(ICollection<string> shardGrantedGames, string gameId, bool hasShard)
	{
		if (hasShard || string.IsNullOrEmpty(gameId) || shardGrantedGames == null)
		{
			return false;
		}
		return shardGrantedGames.Contains(gameId);
	}

	internal static void RestoreMissing()
	{
		try
		{
			UnitEntityData unitEntityData = Game.Instance?.Player?.MainCharacter.Value;
			if (unitEntityData?.Descriptor != null)
			{
				RestoreClaimedRewards(unitEntityData);
				RestoreShard(unitEntityData);
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error restoring kept rewards: " + ex);
		}
	}

	private static void RestoreClaimedRewards(UnitEntityData main)
	{
		foreach (KeyValuePair<string, BlueprintBuffReference> item in PermanentBuffByClaim)
		{
			BlueprintBuff blueprintBuff = item.Value?.Get();
			if (blueprintBuff != null)
			{
				Buff buff = main.Descriptor.Buffs.GetBuff(blueprintBuff);
				BuffRepair buffRepair = DecideBuff(ConstellationChatManager.SceneRewardClaimed(item.Key), buff != null, buff?.IsPermanent ?? false);
				if (buffRepair != BuffRepair.None)
				{
					main.Descriptor.Buffs.AddBuff(blueprintBuff, main, null);
					Main.IsekaiContext.Logger.Log("Kept rewards: " + ((buffRepair == BuffRepair.Add) ? "restored" : "made permanent again") + " " + blueprintBuff.name + " on " + main.CharacterName + " (" + item.Key + ").");
				}
			}
		}
		foreach (KeyValuePair<string, BlueprintFeatureReference> item2 in FeatureByClaim)
		{
			BlueprintFeature blueprintFeature = item2.Value?.Get();
			if (blueprintFeature != null)
			{
				FeatureCollection features = main.Descriptor.Progression.Features;
				if (FeatureMissing(ConstellationChatManager.SceneRewardClaimed(item2.Key), features.HasFact(blueprintFeature)))
				{
					features.AddFeature(blueprintFeature);
					Main.IsekaiContext.Logger.Log("Kept rewards: restored " + blueprintFeature.name + " to " + main.CharacterName + " (" + item2.Key + ").");
				}
			}
		}
	}

	private static void RestoreShard(UnitEntityData main)
	{
		BlueprintFeature blueprintFeature = ShardOfTheShatteredLoop.Get();
		if (blueprintFeature != null)
		{
			FeatureCollection features = main.Descriptor.Progression.Features;
			if (ShardMissing(TimelineManager.Data?.ShardGrantedGames, TimelineManager.CurrentSaveId(), features.HasFact(blueprintFeature)))
			{
				features.AddFeature(blueprintFeature);
				Main.IsekaiContext.Logger.Log("Kept rewards: restored the Shard of the Shattered Loop to " + main.CharacterName + ".");
			}
		}
	}

	public static void Init()
	{
		if (!s_subscribed)
		{
			s_subscribed = true;
			EventBus.Subscribe(new KeptRewardsHandler());
		}
	}
}
