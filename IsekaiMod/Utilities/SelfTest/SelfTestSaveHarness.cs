using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Config;
using IsekaiMod.Content.Arenas;
using Kingmaker;
using Kingmaker.Blueprints.Area;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Persistence;
using Kingmaker.GameModes;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.Globalmap.State;
using Kingmaker.Globalmap.View;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using UnityEngine;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestSaveHarness
{
	private sealed class LoadCompletionWatcher : IAreaLoadingStagesHandler, ISubscriber, IGlobalSubscriber, IDisposable
	{
		private int completions;

		internal int Completions => completions;

		internal LoadCompletionWatcher()
		{
			EventBus.Subscribe(this);
		}

		public void OnAreaScenesLoaded()
		{
		}

		public void OnAreaLoadingComplete()
		{
			completions++;
		}

		public void Dispose()
		{
			try
			{
				EventBus.Unsubscribe(this);
			}
			catch
			{
			}
		}
	}

	internal const float WorldReadyTimeoutSeconds = 420f;

	internal const float TransitionTimeoutSeconds = 300f;

	internal const float SaveWriteTimeoutSeconds = 240f;

	internal const float SettleSeconds = 3f;

	internal const string RoundTripSaveName = "IsekaiSelfTestRoundTrip";

	internal static bool LastWaitSucceeded { get; private set; }

	internal static List<string> LastWaitDetails { get; private set; } = new List<string>();

	internal static IEnumerator Run(SelfTestReport report)
	{
		if (report == null || report.Ended)
		{
			yield break;
		}
		string text = SaveHarnessSwitch.RequestedSaveForThisProcess();
		if (text == null)
		{
			report.Info("No -isekai-selftest-save was given, so the live-world checks did not run. Pass -isekai-selftest-save=<save name> (Run-GameSelfTest.ps1 -SaveName) to run them.");
			yield break;
		}
		int num = SaveHarnessSwitch.SaveTimeoutSecondsForThisProcess();
		float deadline = Time.realtimeSinceStartup + (float)num;
		report.Info($"Live-world checks requested for the save '{text}', with {num} s for the whole phase.");
		List<SaveInfo> list;
		try
		{
			Game.Instance.SaveManager.UpdateSaveListIfNeeded(force: true);
			list = Game.Instance.SaveManager.ToList();
		}
		catch (Exception ex)
		{
			report.Result("save-list", success: false, "Reading the list of saves threw " + ex.GetType().Name + ": " + ex.Message, new string[1] { ex.ToString() });
			yield break;
		}
		SaveHarnessSelection.Choice choice = SaveHarnessSelection.Select(text, list.Select((SaveInfo s) => new SaveHarnessSelection.Candidate(s?.Name, s?.FileName, s?.FolderName, s?.Area?.name)).ToList());
		if (!choice.Found)
		{
			report.Info(choice.Reason);
			yield break;
		}
		SaveInfo save = list[choice.Index];
		report.Info(choice.Reason);
		if (!SafeDlcCheck(save))
		{
			report.Info("The save '" + save.Name + "' needs downloadable content this installation does not have, so the live-world checks did not run.");
			yield break;
		}
		string expectedArea = save.Area?.name ?? "";
		string expectedCharacter = save.PlayerCharacterName ?? "";
		SelfTestRecorder.LoggedUnityExceptions(out var startup, out var duringChecks);
		int exceptionsBeforeLoad = duringChecks.Count;
		using (LoadCompletionWatcher watcher = new LoadCompletionWatcher())
		{
			IsekaiSelfTest.CurrentCheck = "save-load";
			if (!TryStart(report, "save-load", delegate
			{
				Game.Instance.LoadGameFromMainMenu(save);
			}))
			{
				yield break;
			}
			IEnumerator waited = WaitForWorld(watcher, 420f, deadline);
			while (waited.MoveNext())
			{
				yield return waited.Current;
			}
			if (!LastWaitSucceeded)
			{
				report.Result("save-load", success: false, $"The world was not up within {420f:0} s of loading '{save.Name}'.", LastWaitDetails);
				yield break;
			}
			yield return new WaitForSecondsRealtime(3f);
			SelfTestChecks.RunOne(report, new SelfTestChecks.Check
			{
				Id = "save-load",
				Run = () => SelfTestSaveChecks.WorldMatchesSave(expectedArea, expectedCharacter)
			});
			yield return null;
			SelfTestRecorder.LoggedUnityExceptions(out startup, out var duringChecks2);
			List<string> duringLoad = duringChecks2.Skip(exceptionsBeforeLoad).ToList();
			SelfTestChecks.RunOne(report, new SelfTestChecks.Check
			{
				Id = "save-load-exceptions",
				Run = () => SelfTestSaveChecks.NoExceptionsDuringLoad(duringLoad)
			});
			yield return null;
			SelfTestChecks.Check[] array = new SelfTestChecks.Check[4]
			{
				new SelfTestChecks.Check
				{
					Id = "save-mod-state",
					Run = SelfTestSaveChecks.ModStateLoaded
				},
				new SelfTestChecks.Check
				{
					Id = "save-spawned-content",
					Run = SelfTestSaveChecks.SpawnedContent
				},
				new SelfTestChecks.Check
				{
					Id = "save-broadcast",
					Run = SelfTestSaveChecks.BroadcastReachesOverlayAndArchive
				},
				new SelfTestChecks.Check
				{
					Id = "save-companion-conversations",
					Run = SelfTestSaveChecks.CompanionConversationsOpen
				}
			};
			foreach (SelfTestChecks.Check check in array)
			{
				if (report.Ended)
				{
					yield break;
				}
				IsekaiSelfTest.CurrentCheck = check.Id;
				SelfTestChecks.RunOne(report, check);
				yield return null;
			}
			IEnumerator transition = RunArenaRoundTrip(report, watcher, deadline);
			while (transition.MoveNext())
			{
				yield return transition.Current;
			}
			IEnumerator worldMap = RunWorldMapVisit(report, watcher, deadline);
			while (worldMap.MoveNext())
			{
				yield return worldMap.Current;
			}
			IEnumerator roundTrip = RunSaveRoundTrip(report, watcher, deadline);
			while (roundTrip.MoveNext())
			{
				yield return roundTrip.Current;
			}
			if (!report.Ended)
			{
				IsekaiSelfTest.CurrentCheck = "save-respec";
				SelfTestChecks.RunOne(report, new SelfTestChecks.Check
				{
					Id = "save-respec",
					Run = SelfTestRespecChecks.LiveRespec
				});
				yield return null;
			}
		}
		IsekaiSelfTest.CurrentCheck = "(live-world checks done)";
	}

	private static IEnumerator RunArenaRoundTrip(SelfTestReport report, LoadCompletionWatcher watcher, float deadline)
	{
		IsekaiSelfTest.CurrentCheck = "save-area-transition";
		AddedContent addedContent = Main.IsekaiContext.AddedContent;
		if (addedContent != null && !addedContent.EnableDimensionalArena)
		{
			report.Info("The Planar Coliseum is switched off in the settings, so the area transition was not attempted.");
			yield break;
		}
		if (PlanarColiseumHubManager.IsInColiseum())
		{
			report.Info("The save is already inside an arena, so the area transition was not attempted; make the self-test save on Golarion instead.");
			yield break;
		}
		SelfTestSaveChecks.ArenaVisit visit = new SelfTestSaveChecks.ArenaVisit
		{
			StartedAreaName = (Game.Instance?.CurrentlyLoadedArea?.name ?? ""),
			PartyBefore = SelfTestSaveChecks.PartyRoster()
		};
		BlueprintAreaEnterPoint blueprintAreaEnterPoint = null;
		try
		{
			blueprintAreaEnterPoint = PlanarColiseumArea.EnterPointForAct(PlanarColiseumArea.ArenaActForChapter(Game.Instance?.Player?.Chapter ?? 1));
		}
		catch (Exception ex)
		{
			visit.Exceptions.Add("Resolving the arena entrance threw " + ex.GetType().Name + ": " + ex.Message);
		}
		if (blueprintAreaEnterPoint == null)
		{
			report.Info("This chapter offers no arena entrance, so the area transition was not attempted.");
			yield break;
		}
		int completions = watcher.Completions;
		PlanarColiseumHubManager.ScheduleTravel(blueprintAreaEnterPoint);
		IEnumerator outward = WaitForWorld(watcher, 300f, deadline, completions);
		while (outward.MoveNext())
		{
			yield return outward.Current;
		}
		visit.Arrived = LastWaitSucceeded;
		if (!visit.Arrived)
		{
			visit.Exceptions.AddRange(LastWaitDetails);
		}
		visit.ArrivedAreaName = Game.Instance?.CurrentlyLoadedArea?.name ?? "";
		yield return new WaitForSecondsRealtime(3f);
		if (visit.Arrived)
		{
			List<UnitEntityData> units = SelfTestSaveChecks.UnitsInArea();
			visit.HubUnitsSeen = PlanarColiseumHubManager.HubUnitNames.Where((string n) => SelfTestSaveChecks.CountOf(units, n) > 0).ToList();
			visit.ServiceNpcProblems = SelfTestSaveChecks.ServiceNpcProblems();
			visit.WavePlacementProblems = SelfTestSaveChecks.WavePlacementProblems();
			try
			{
				BlueprintAbility returnAbility = PlanarColiseumHubManager.ReturnAbility;
				UnitEntityData unitEntityData = Game.Instance?.Player?.MainCharacter.Value;
				visit.HasReturnAbility = returnAbility != null && unitEntityData != null && unitEntityData.HasFact(returnAbility);
			}
			catch (Exception ex2)
			{
				visit.Exceptions.Add("Checking Return to Golarion threw " + ex2.GetType().Name + ": " + ex2.Message);
			}
			try
			{
				visit.EnvoyTradeOutcome = HubTrade.Start("AstralCoinEnvoyUnit", CosmicRelicsVendorTable.Table).ToString();
				SelfTestSaveChecks.ArenaVisit arenaVisit = visit;
				Game instance = Game.Instance;
				arenaVisit.EnvoyStock = ((instance != null && instance.Vendor?.IsTrading == true) ? (Game.Instance.Vendor.StoreItems?.Items?.Count()).GetValueOrDefault() : 0);
				SelfTestSaveChecks.ArenaVisit arenaVisit2 = visit;
				Game instance2 = Game.Instance;
				arenaVisit2.TradeProbeStarted = instance2 != null && instance2.Vendor?.IsTrading == true;
			}
			catch (Exception ex3)
			{
				visit.Exceptions.Add("Opening a trade with a hub vendor threw " + ex3.GetType().Name + ": " + ex3.Message);
			}
			yield return new WaitForSecondsRealtime(3f);
			completions = watcher.Completions;
			PlanarColiseumHubManager.ReturnToGolarion();
			IEnumerator homeward = WaitForWorld(watcher, 300f, deadline, completions);
			while (homeward.MoveNext())
			{
				yield return homeward.Current;
			}
			visit.ReturnedToGolarion = LastWaitSucceeded;
			if (!visit.ReturnedToGolarion)
			{
				visit.Exceptions.AddRange(LastWaitDetails);
			}
			visit.ReturnedAreaName = Game.Instance?.CurrentlyLoadedArea?.name ?? "";
			yield return new WaitForSecondsRealtime(3f);
			try
			{
				visit.PartyAfter = SelfTestSaveChecks.PartyRoster();
				SelfTestSaveChecks.ArenaVisit arenaVisit3 = visit;
				Game instance3 = Game.Instance;
				arenaVisit3.TradingAfterReturn = instance3 != null && instance3.Vendor?.IsTrading == true;
				visit.FullScreenAfterReturn = Game.Instance?.CurrentMode == GameModeType.FullScreenUi;
				visit.UndrawableItems = SelfTestSaveChecks.UndrawableInventoryItems();
			}
			catch (Exception ex4)
			{
				visit.Exceptions.Add("Reading the HUD state after the return threw " + ex4.GetType().Name + ": " + ex4.Message);
			}
		}
		SelfTestChecks.RunOne(report, new SelfTestChecks.Check
		{
			Id = "save-area-transition",
			Run = () => SelfTestSaveChecks.ArenaRoundTrip(visit)
		});
		yield return null;
	}

	private static IEnumerator RunWorldMapVisit(SelfTestReport report, LoadCompletionWatcher watcher, float deadline)
	{
		IsekaiSelfTest.CurrentCheck = "save-world-map";
		int chapter = (Game.Instance?.Player?.Chapter).GetValueOrDefault();
		BlueprintGlobalMap map;
		try
		{
			map = SelfTestWorldMapChecks.ChapterWorldMap(chapter);
		}
		catch (Exception ex)
		{
			report.Result("save-world-map", success: false, "Resolving the chapter's world map threw " + ex.GetType().Name + ": " + ex.Message);
			yield break;
		}
		if (map == null)
		{
			report.Info($"Chapter {chapter} has no world map, so the world-map markers were not read.");
			yield break;
		}
		if (!(GlobalMapView.Instance != null) || GlobalMapView.Instance.Blueprint != map)
		{
			GlobalMapState globalMapState = Game.Instance?.Player?.AllGlobalMaps?.FirstOrDefault((GlobalMapState s) => s?.Blueprint == map);
			if (globalMapState == null || globalMapState.LastActivationTime == TimeSpan.MinValue || map.GlobalMapEnterPoint == null)
			{
				report.Info("The party has not been on '" + map.name + "' in this save yet, so the world-map markers were not read.");
				yield break;
			}
			int completions = watcher.Completions;
			PlanarColiseumHubManager.ScheduleTravel(map.GlobalMapEnterPoint);
			IEnumerator travel = WaitForWorld(watcher, 300f, deadline, completions);
			while (travel.MoveNext())
			{
				yield return travel.Current;
			}
			if (!LastWaitSucceeded)
			{
				report.Result("save-world-map", success: false, $"The world map '{map.name}' did not come up within {300f:0} s.", LastWaitDetails);
				yield break;
			}
		}
		yield return new WaitForSecondsRealtime(6f);
		SelfTestChecks.RunOne(report, new SelfTestChecks.Check
		{
			Id = "save-world-map",
			Run = () => SelfTestWorldMapChecks.MarkersOnLoadedMap(chapter)
		});
		yield return null;
	}

	private static IEnumerator RunSaveRoundTrip(SelfTestReport report, LoadCompletionWatcher watcher, float deadline)
	{
		IsekaiSelfTest.CurrentCheck = "save-round-trip";
		bool flag;
		try
		{
			flag = Game.Instance.SaveManager.IsSaveAllowed(SaveInfo.SaveType.Manual);
		}
		catch (Exception ex)
		{
			report.Result("save-round-trip", success: false, "Asking whether saving is allowed threw " + ex.GetType().Name + ": " + ex.Message);
			yield break;
		}
		if (!flag)
		{
			report.Info("The game will not accept a manual save where this save stands (combat, a dialog, a cutscene or ironman), so the save round trip was not attempted.");
			yield break;
		}
		List<string> before = SaveStateFingerprint.Of(SelfTestSaveChecks.ModStorages());
		SaveInfo written = null;
		bool callbackRan = false;
		try
		{
			written = Game.Instance.SaveManager.CreateNewSave("IsekaiSelfTestRoundTrip");
			written.Type = SaveInfo.SaveType.Manual;
			Game.Instance.SaveGame(written, delegate
			{
				callbackRan = true;
			});
		}
		catch (Exception ex2)
		{
			report.Result("save-round-trip", success: false, "Writing the round-trip save threw " + ex2.GetType().Name + ": " + ex2.Message, new string[1] { ex2.ToString() });
			yield break;
		}
		float saveDeadline = Math.Min(deadline, Time.realtimeSinceStartup + 240f);
		while (!callbackRan && Time.realtimeSinceStartup < saveDeadline && !report.Ended)
		{
			yield return null;
		}
		while (SafeCommitInProgress() && Time.realtimeSinceStartup < saveDeadline && !report.Ended)
		{
			yield return null;
		}
		if (!callbackRan)
		{
			report.Result("save-round-trip", success: false, $"The round-trip save was not written within {240f:0} s.");
			yield break;
		}
		int completions = watcher.Completions;
		if (!TryStart(report, "save-round-trip", delegate
		{
			Game.Instance.LoadGame(written);
		}))
		{
			yield break;
		}
		IEnumerator waited = WaitForWorld(watcher, 420f, deadline, completions);
		while (waited.MoveNext())
		{
			yield return waited.Current;
		}
		if (!LastWaitSucceeded)
		{
			report.Result("save-round-trip", success: false, $"The round-trip save did not load back within {420f:0} s.", LastWaitDetails);
			yield break;
		}
		yield return new WaitForSecondsRealtime(3f);
		List<string> after = SaveStateFingerprint.Of(SelfTestSaveChecks.ModStorages());
		string savedAs = written.Name ?? "IsekaiSelfTestRoundTrip";
		SelfTestChecks.RunOne(report, new SelfTestChecks.Check
		{
			Id = "save-round-trip",
			Run = () => SelfTestSaveChecks.RoundTripPreservedState(before, after, savedAs)
		});
		report.Info("The round-trip save '" + savedAs + "' was left on disk; delete it from the load menu when it is in the way.");
		yield return null;
	}

	private static IEnumerator WaitForWorld(LoadCompletionWatcher watcher, float timeoutSeconds, float phaseDeadline, int completionsBefore = 0)
	{
		LastWaitSucceeded = false;
		LastWaitDetails = new List<string>();
		SaveHarnessReadiness.StabilityTracker tracker = new SaveHarnessReadiness.StabilityTracker();
		float limit = Math.Min(phaseDeadline, Time.realtimeSinceStartup + timeoutSeconds);
		SaveHarnessReadiness.Sample sample = null;
		while (Time.realtimeSinceStartup < limit)
		{
			sample = Read();
			bool flag = watcher.Completions > completionsBefore;
			if (flag && tracker.Observe(sample))
			{
				LastWaitSucceeded = true;
				yield break;
			}
			if (!flag)
			{
				tracker.Reset();
			}
			yield return null;
		}
		LastWaitDetails = SaveHarnessReadiness.MissingSigns(sample);
		if (watcher.Completions <= completionsBefore)
		{
			LastWaitDetails.Insert(0, "no area load ran to completion");
		}
	}

	private static SaveHarnessReadiness.Sample Read()
	{
		SaveHarnessReadiness.Sample sample = new SaveHarnessReadiness.Sample();
		try
		{
			Game instance = Game.Instance;
			sample.HasLoadedArea = instance?.CurrentlyLoadedArea != null && instance.State?.LoadedAreaState != null;
			sample.LoadingInProcess = LoadingProcess.Instance != null && LoadingProcess.Instance.IsLoadingInProcess;
			sample.HasMainCharacter = instance?.Player?.MainCharacter.Value != null;
			sample.PartyCount = (instance?.Player?.Party?.Count).GetValueOrDefault();
			sample.ModeName = instance?.CurrentMode?.Name;
		}
		catch
		{
			sample.LoadingInProcess = true;
		}
		return sample;
	}

	private static bool TryStart(SelfTestReport report, string id, Action action)
	{
		try
		{
			action();
			return true;
		}
		catch (Exception ex)
		{
			report.Result(id, success: false, "Asking the game to load threw " + ex.GetType().Name + ": " + ex.Message, new string[1] { ex.ToString() });
			return false;
		}
	}

	private static bool SafeDlcCheck(SaveInfo save)
	{
		try
		{
			return save.CheckDlcAvailable();
		}
		catch
		{
			return true;
		}
	}

	private static bool SafeCommitInProgress()
	{
		try
		{
			return Game.Instance.SaveManager.CommitInProgress;
		}
		catch
		{
			return false;
		}
	}
}
