using System;
using IsekaiMod.Content.Quests;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("7c36dbc6e423494fbf85866455c1dedf")]
public class ActionEnterBossSite : IsekaiGameAction
{
	public BossSiteKind Kind;

	public int Act;

	public override string GetCaption()
	{
		if (Kind != BossSiteKind.Rift)
		{
			return "Enter the site of Barren Anomaly " + Act;
		}
		return "Enter the site of the open Dimensional Rift act";
	}

	protected override void RunSafely()
	{
		try
		{
			if (ColiseumGates.RefuseTravel((Kind == BossSiteKind.Rift) ? "The rift site" : "The anomaly site"))
			{
				return;
			}
			int act = ((Kind == BossSiteKind.Rift) ? BossSites.CurrentAccessibleRiftAct() : Act);
			if (act == 0)
			{
				PlanarColiseumHubManager.PostLog("No rift site is open here right now.");
				return;
			}
			if (Kind == BossSiteKind.Rift && act == 6 && !IsekaiTransmigrationQuest.AreAllSideQuestsCompleted(out var missingQuests))
			{
				DimensionalRiftIncursions.ReportArbiterRequirements(missingQuests);
				return;
			}
			BlueprintAreaEnterPoint blueprintAreaEnterPoint = BossSites.SiteEnterPointFor(Kind, act);
			if (blueprintAreaEnterPoint == null)
			{
				Main.IsekaiContext.Logger.LogError($"[BossSites] No site for {Kind} {act}; the fight is called where the party stands.");
				BossSiteKind kind = Kind;
				GuardedSchedule.Next($"Calling the {kind} {act} fight where the party stands", delegate
				{
					if (kind == BossSiteKind.Rift)
					{
						DimensionalRiftIncursions.SpawnRiftBoss();
					}
					else
					{
						CosmicBarrenBosses.SpawnBoss(CosmicBarrenBosses.BossTypeForAct(act));
					}
				});
			}
			else
			{
				string currentAreaName = Game.Instance?.CurrentlyLoadedArea?.name ?? "";
				int chapter = Game.Instance?.Player?.Chapter ?? 1;
				PlanarColiseumTravel.RecordReturnPoint(ActionEnterColiseum.CurrentReturnPoint(currentAreaName, chapter));
				PlanarColiseumHubManager.ScheduleTravel(blueprintAreaEnterPoint);
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[BossSites] Error travelling to a boss site: " + ex);
		}
	}
}
