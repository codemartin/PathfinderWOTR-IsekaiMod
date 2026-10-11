using System;
using IsekaiMod.Content.Quests;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("5f90f7955c2645719e207200ddaa1e23")]
public class ActionReportArbiterRequirements : IsekaiGameAction
{
	public override string GetCaption()
	{
		return "Report what the Grand Arbiter still waits for";
	}

	protected override void RunSafely()
	{
		try
		{
			IsekaiTransmigrationQuest.AreAllSideQuestsCompleted(out var missingQuests);
			DimensionalRiftIncursions.ReportArbiterRequirements(missingQuests);
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[BossSites] Could not report the Arbiter's requirements: " + ex);
		}
	}
}
