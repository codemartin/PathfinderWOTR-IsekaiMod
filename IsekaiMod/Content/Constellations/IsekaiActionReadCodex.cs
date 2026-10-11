using System;
using System.Collections.Generic;
using IsekaiMod.Content.Dialogue;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace IsekaiMod.Content.Constellations;

[TypeId("319c8d5c73d340198f95c1b1e345dd29")]
public class IsekaiActionReadCodex : IsekaiContextAction
{
	public override string GetCaption()
	{
		return "Reads the Codex of the Reincarnated Otherworlder";
	}

	protected override void RunSafely()
	{
		UnitEntityData unitEntityData = Game.Instance?.Player?.MainCharacter.Value;
		BlueprintBuff knowledgeBuff = CodexOfReincarnation.KnowledgeBuff;
		if (unitEntityData != null && knowledgeBuff != null)
		{
			unitEntityData.Descriptor.Buffs.AddBuff(knowledgeBuff, unitEntityData, TimeSpan.FromHours(24.0));
		}
		if (ConstellationChatManager.TryClaimSceneReward("Port62:CodexFirstReadTip"))
		{
			string sponsor = Port62Rewards.Seat("The Grand Arbiter");
			DivineTokens.AddClaimedCoins(500, sponsor);
			ConstellationChatManager.PostCombatLog(ConstellationText.Localized("Constellation.Codex.Read", "<color=#5B2A86><b>[Codex of the Reincarnated Otherworlder]</b></color> You commune with the cosmic chronicle. <color=#133E71>+500 Cosmic Coins</color> received from <b>The All-Seeing Eye</b>! You gain a 24-hour bonus to Knowledge, Lore, and Will saves."));
		}
		else
		{
			ConstellationChatManager.PostCombatLog(ConstellationText.Localized("Constellation.Codex.Read", "<color=#5B2A86><b>[Codex of the Reincarnated Otherworlder]</b></color> You commune with the cosmic chronicle. You gain a 24-hour bonus to Knowledge, Lore, and Will saves."));
		}
		List<PastCycleRecord> pastCycles = TimelineManager.PastCycles;
		if (pastCycles != null && pastCycles.Count > 0)
		{
			ConstellationChatManager.PostCombatLog(ConstellationText.LocalizedFormat("Constellation.Codex.Read", "<color=#5B2A86><b>[Past Incarnations Chronicle - {0} Record(s)]</b></color>", pastCycles.Count));
			foreach (PastCycleRecord item in pastCycles)
			{
				ConstellationChatManager.PostCombatLog(ConstellationText.LocalizedFormat("Constellation.Codex.Read", " - <b>Loop {0}</b>: {1} ({2}, {3}) -> Ending: {4}", item.CycleNumber, item.CharacterName, item.Archetype, item.MythicPath, item.EndingAchieved));
			}
		}
		List<string> list = CodexOfReincarnation.PastLegacies(pastCycles);
		if (list.Count > 0)
		{
			ConstellationChatManager.PostCombatLog(ConstellationText.LocalizedFormat("Constellation.Codex.Read", "<color=#133E71><b>[Continuity of Legacies]</b></color> You carry the soul memories of your past reincarnations: {0}.", string.Join(", ", list)));
		}
	}
}
