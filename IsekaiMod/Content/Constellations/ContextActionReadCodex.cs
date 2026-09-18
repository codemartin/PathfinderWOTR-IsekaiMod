using System;
using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Constellations
{
	public class ContextActionReadCodex : ContextAction
	{
		public override string GetCaption()
		{
			return "Reads the Codex of the Reincarnated Otherworlder, granting wisdom and Cosmic Coins";
		}

		public override void RunAction()
		{
			try
			{
				UnitEntityData unitEntityData = BlueprintSafetyExtensions.SafeGetMainCharacter();
				if (unitEntityData != null && CodexOfReincarnation.CodexKnowledgeBuff != null)
				{
					unitEntityData.Descriptor.Buffs.AddBuff(CodexOfReincarnation.CodexKnowledgeBuff, unitEntityData, new TimeSpan(24, 0, 0));
				}
				if (!TimelineManager.Data.CompletedSideQuests.Contains("CodexFirstReadBonus"))
				{
					TimelineManager.Data.CompletedSideQuests.Add("CodexFirstReadBonus");
					TimelineManager.Save();
					DivineTokens.AddCoins(500, "The Grand Arbiter");
					EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
					{
						h.HandleLogMessage("<color=#FFD700><b>[Codex of the Reincarnated Otherworlder]</b></color> You commune with the cosmic chronicle. <color=#00FFFF>+500 Cosmic Coins</color> received from <b>The Grand Arbiter</b>! You gain a 24-hour bonus to Knowledge, Lore, and Will saves.");
					});
				}
				else
				{
					EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
					{
						h.HandleLogMessage("<color=#FFD700><b>[Codex of the Reincarnated Otherworlder]</b></color> You commune with the cosmic chronicle. Your 24-hour insight bonus to Knowledge, Lore, and Will saves has been refreshed.");
					});
				}
				List<PastCycleRecord> pastCycles = TimelineManager.PastCycles;
				if (pastCycles != null && pastCycles.Count > 0)
				{
					EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
					{
						h.HandleLogMessage($"<color=#F5C542><b>[Past Incarnations Chronicle - {pastCycles.Count} Record(s)]</b></color>");
					});
					foreach (PastCycleRecord cycle in pastCycles)
					{
						string legInfo = "";
						if (!string.IsNullOrEmpty(cycle.PrimaryLegacy))
						{
							legInfo = " | Legacies: " + cycle.PrimaryLegacy + (string.IsNullOrEmpty(cycle.SecondaryLegacy) ? "" : (", " + cycle.SecondaryLegacy));
						}
						EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
						{
							h.HandleLogMessage($" - <b>Loop {cycle.CycleNumber}</b>: {cycle.CharacterName} ({cycle.Archetype}, {cycle.MythicPath}){legInfo} -> Ending: {cycle.EndingAchieved}");
						});
					}
				}
				List<string> pastLegs = TimelineManager.GetUnlockedPastLegacies();
				if (pastLegs != null && pastLegs.Count > 0)
				{
					EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
					{
						h.HandleLogMessage("<color=#00FFFF><b>[Continuity of Legacies]</b></color> You carry the soul memories of your past reincarnations: " + string.Join(", ", pastLegs) + ".");
					});
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ContextActionReadCodex: " + ex);
			}
		}
	}
}
