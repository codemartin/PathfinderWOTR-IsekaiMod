using System;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Crusade;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Kingdom;
using Kingmaker.Kingdom.Armies;
using Kingmaker.PubSubSystem;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue;

[TypeId("b427201f553e4a30af884cbfaa94ed0f")]
public class ContextActionMobilizeSubclassArmy : IsekaiContextAction
{
	public string UnitGuid;

	public int Count;

	public string ArmyName;

	public string ClaimKey;

	internal const string WarCampLevyClaim = "WarCampLevy";

	internal static string LastPickClaimed;

	public override string GetCaption()
	{
		return $"Mobilizes {Count} {ArmyName} into Crusade Recruit Pool";
	}

	protected override void RunSafely()
	{
		if (string.IsNullOrEmpty(UnitGuid) || Count <= 0)
		{
			return;
		}
		if (!KingdomState.Founded)
		{
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(ConstellationText.Localized("CrusadeMobilization.NoArmy", "<color=#004604><b>[Crusade Mobilization]</b></color> The crusade has no army to reinforce yet."));
			});
			return;
		}
		if (!string.IsNullOrEmpty(ClaimKey))
		{
			if (!ConstellationChatManager.TryClaimSceneReward(ClaimKey))
			{
				LastPickClaimed = null;
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage(LevyAlreadyTakenMessage(Count, ArmyName));
				});
				return;
			}
			LastPickClaimed = ClaimKey;
		}
		BlueprintUnit blueprint = BlueprintTools.GetBlueprint<BlueprintUnit>(UnitGuid);
		if (blueprint == null)
		{
			return;
		}
		string unitName = (string.IsNullOrEmpty(blueprint.CharacterName) ? ArmyName : blueprint.CharacterName);
		string message = Mobilize(ArmyRecruitsManager.Instance, blueprint, Count, unitName, IsekaiKingdomProjects.AutoCrusade());
		if (message != null)
		{
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(message);
			});
		}
	}

	internal static string Mobilize(ArmyRecruitsManager recruits, BlueprintUnit unit, int count, string unitName, bool autoCrusade)
	{
		if (recruits == null || unit == null || count <= 0)
		{
			return null;
		}
		recruits.IncreasePool(unit, count);
		if (!autoCrusade)
		{
			return ConstellationText.LocalizedFormat("Constellation.CrusadeMobilization.Joined", "<color=#004604><b>[Crusade Mobilization]</b></color> <b>{0} {1}</b> have joined your recruitment pool!", count, unitName);
		}
		return ConstellationText.LocalizedFormat("Constellation.CrusadeMobilization.Joined", "<color=#004604><b>[Crusade Mobilization]</b></color> <b>{0} {1}</b> have joined your recruitment pool, but no army can be recruited from it while the crusade is managed automatically.", count, unitName);
	}

	internal static string LevyAlreadyTakenMessage(int count, string unitName)
	{
		if (count <= 0 || string.IsNullOrEmpty(unitName))
		{
			return ConstellationText.Localized("Constellation.CrusadeMobilization.LevyTaken", "<color=#3F3F3F><b>[Crusade Mobilization]</b></color> The crusade has already taken its levy from you, so these volunteers were sent back rather than counted twice. Nothing was added to your recruitment pool.");
		}
		return ConstellationText.LocalizedFormat("Constellation.CrusadeMobilization.LevyTaken", "<color=#3F3F3F><b>[Crusade Mobilization]</b></color> The crusade has already taken its levy from you, so <b>{0} {1}</b> were sent back rather than counted twice. Nothing was added to your recruitment pool.", count, unitName);
	}

	internal static bool LevyStillOwed(bool alreadyClaimed, string lastPickClaimed, string claimKey)
	{
		if (string.IsNullOrEmpty(claimKey))
		{
			return true;
		}
		if (alreadyClaimed)
		{
			return string.Equals(lastPickClaimed, claimKey, StringComparison.Ordinal);
		}
		return true;
	}
}
