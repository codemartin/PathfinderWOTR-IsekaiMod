using System;
using System.Collections.Generic;
using System.IO;
using IsekaiMod.Content.Features;
using IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;
using IsekaiMod.Content.Quests;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Newtonsoft.Json;

namespace IsekaiMod.Content.Constellations
{
	public class TimelineManager : IGameOverHandler, ISubscriber, IGlobalSubscriber, IAreaActivationHandler
	{
		private static TimelineData _data;

		private static string _filePath;

		public static TimelineData Data
		{
			get
			{
				if (_data == null)
				{
					Load();
				}
				return _data;
			}
		}

		public static int TotalRuns => Data.TotalRuns;

		public static bool LoopBreakerAchieved => Data.LoopBreakerAchieved;

		public static bool LoopShattered => Data.LoopShattered;

		public static List<PastCycleRecord> PastCycles => Data.PastCycles;

		private static void EnsureFilePath()
		{
			if (!string.IsNullOrEmpty(_filePath))
			{
				return;
			}
			try
			{
				string text = Main.IsekaiContext?.ModEntry?.Path;
				if (!string.IsNullOrEmpty(text))
				{
					_filePath = Path.Combine(text, "TimelineHistory.json");
				}
				else
				{
					_filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "IsekaiMod", "TimelineHistory.json");
				}
			}
			catch
			{
				_filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "IsekaiMod", "TimelineHistory.json");
			}
		}

		public static void Init()
		{
			EnsureFilePath();
			Load();
			EventBus.Subscribe(new TimelineManager());
		}

		public static void Load()
		{
			try
			{
				EnsureFilePath();
				if (File.Exists(_filePath))
				{
					_data = JsonConvert.DeserializeObject<TimelineData>(File.ReadAllText(_filePath)) ?? new TimelineData();
					return;
				}
				_data = new TimelineData();
				Save();
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Failed to load TimelineHistory: " + ex);
				_data = new TimelineData();
			}
		}

		public static void Save()
		{
			try
			{
				EnsureFilePath();
				string directoryName = Path.GetDirectoryName(_filePath);
				if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				string contents = JsonConvert.SerializeObject(_data, Formatting.Indented);
				File.WriteAllText(_filePath, contents);
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Failed to save TimelineHistory: " + ex);
			}
		}

		public static void SetActiveRunArchetype(string archetype)
		{
			if (string.IsNullOrEmpty(Data.ActiveRunArchetype))
			{
				Data.ActiveRunArchetype = archetype;
				Save();
			}
		}

		public static void RecordChoice(string choiceKey)
		{
			if (!Data.PastChoices.Contains(choiceKey))
			{
				Data.PastChoices.Add(choiceKey);
				Save();
			}
		}

		public static void RecordMythicPath(string pathName)
		{
			if (!Data.PastMythicPaths.Contains(pathName))
			{
				Data.PastMythicPaths.Add(pathName);
				Save();
			}
		}

		public static void ResetTimeline()
		{
			_data = new TimelineData();
			Save();
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage("<color=#9400D3><b>[Karmic Thread Severed]</b></color> The causal timeline chronicle has been completely reset to Loop 1. A fresh destiny awaits.");
			});
		}

		public static void RecordCycleCompletion(string endingId, string endingTitle, string archetype, string mythicPath, string romancedCompanion, string characterName, string primaryLegacy = "", string secondaryLegacy = "")
		{
			if (!Data.ClearedEndings.Contains(endingId))
			{
				Data.ClearedEndings.Add(endingId);
			}
			if (endingId.IndexOf("UnwrittenDawn", StringComparison.OrdinalIgnoreCase) >= 0 || endingId.IndexOf("LoopBreaker", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				Data.LoopShattered = true;
				Data.LoopBreakerAchieved = true;
			}
			string text = primaryLegacy;
			string text2 = secondaryLegacy;
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
			{
				try
				{
					UnitEntityData unitEntityData = BlueprintSafetyExtensions.SafeGetMainCharacter();
					if (unitEntityData != null)
					{
						BlueprintFeatureSelection classFeature = LegacySelection.GetClassFeature();
						if (classFeature != null && classFeature.m_AllFeatures != null)
						{
							BlueprintFeatureReference[] allFeatures = classFeature.m_AllFeatures;
							for (int i = 0; i < allFeatures.Length; i++)
							{
								BlueprintFeature blueprintFeature = allFeatures[i].Get();
								if (blueprintFeature != null && unitEntityData.Progression.Features.HasFact(blueprintFeature))
								{
									if (string.IsNullOrEmpty(text))
									{
										text = blueprintFeature.Name;
									}
									else if (string.IsNullOrEmpty(text2) && blueprintFeature.Name != text)
									{
										text2 = blueprintFeature.Name;
										break;
									}
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					Main.IsekaiContext.Logger.LogError("Error checking player legacies in RecordCycleCompletion: " + ex);
				}
			}
			if (!string.IsNullOrEmpty(text) && !Data.PastLegacies.Contains(text))
			{
				Data.PastLegacies.Add(text);
			}
			if (!string.IsNullOrEmpty(text2) && !Data.PastLegacies.Contains(text2))
			{
				Data.PastLegacies.Add(text2);
			}
			PastCycleRecord item = new PastCycleRecord
			{
				CycleNumber = Data.TotalRuns,
				EndingId = endingId,
				EndingTitle = endingTitle,
				CharacterName = (string.IsNullOrEmpty(characterName) ? "The Otherworlder" : characterName),
				Archetype = ((!string.IsNullOrEmpty(archetype)) ? archetype : (string.IsNullOrEmpty(Data.ActiveRunArchetype) ? "Base Protagonist" : Data.ActiveRunArchetype)),
				MythicPath = (string.IsNullOrEmpty(mythicPath) ? "Legend" : mythicPath),
				RomancedCompanion = (string.IsNullOrEmpty(romancedCompanion) ? "None" : romancedCompanion),
				CompletionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
				PrimaryLegacy = (text ?? ""),
				SecondaryLegacy = (text2 ?? "")
			};
			Data.PastCycles.Add(item);
			Save();
		}

		public static List<string> GetUnlockedPastLegacies()
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			if (Data.PastLegacies != null)
			{
				foreach (string pastLegacy in Data.PastLegacies)
				{
					if (!string.IsNullOrEmpty(pastLegacy))
					{
						hashSet.Add(pastLegacy);
					}
				}
			}
			if (Data.PastCycles != null)
			{
				foreach (PastCycleRecord pastCycle in Data.PastCycles)
				{
					if (!string.IsNullOrEmpty(pastCycle.PrimaryLegacy))
					{
						hashSet.Add(pastCycle.PrimaryLegacy);
					}
					if (!string.IsNullOrEmpty(pastCycle.SecondaryLegacy))
					{
						hashSet.Add(pastCycle.SecondaryLegacy);
					}
				}
			}
			return new List<string>(hashSet);
		}

		public static PastCycleRecord GetLastCycle()
		{
			if (Data.PastCycles == null || Data.PastCycles.Count <= 0)
			{
				return null;
			}
			return Data.PastCycles[Data.PastCycles.Count - 1];
		}

		public static bool HasClearedEnding(string endingId)
		{
			return Data.ClearedEndings.Contains(endingId);
		}

		public static bool HasPlayedArchetype(string archetype)
		{
			if (Data.PastCycles == null)
			{
				return false;
			}
			for (int i = 0; i < Data.PastCycles.Count; i++)
			{
				if (Data.PastCycles[i].Archetype.IndexOf(archetype, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		public static void AdvanceLoop(string reason = "")
		{
			Data.TotalRuns++;
			Data.RecentDeath = false;
			Data.ActiveRunArchetype = "";
			Data.ShardRelicGranted = false;
			if (!string.IsNullOrEmpty(reason))
			{
				Data.PastChoices.Add($"Loop {Data.TotalRuns - 1} End: {reason}");
			}
			Save();
		}

		public static void IncrementDeath(string areaName)
		{
			Data.DeathCount++;
			Data.RecentDeath = true;
			Data.LastArea = areaName;
			Save();
		}

		public void HandleGameOver(Player.GameOverReasonType reason)
		{
			IncrementDeath(Game.Instance?.CurrentlyLoadedArea?.name ?? "UnknownArea");
		}

		public static double GetCoinLoopMultiplier()
		{
			if (Data.TotalRuns <= 1)
			{
				return 1.0;
			}
			return 1.0 + (double)(Data.TotalRuns - 1) * 0.25;
		}

		public void OnAreaActivated()
		{
			try
			{
				IsekaiTransmigrationQuest.StartQuest();
				CodexOfReincarnation.EnsureCodexDelivered();
				DivineTokens.SyncInventoryCoins(DivineTokens.GetCoins());
				if (Data.TotalRuns > 1)
				{
					UnitEntityData unitEntityData = BlueprintSafetyExtensions.SafeGetMainCharacter();
					BlueprintFeature blueprintFeature = EchoOfPastSovereigns.Get();
					if (unitEntityData != null && blueprintFeature != null)
					{
						Feature fact = unitEntityData.Descriptor.Progression.Features.GetFact(blueprintFeature);
						int desiredRanks = Data.TotalRuns - 1;
						if (fact == null)
						{
							fact = unitEntityData.Descriptor.Progression.Features.AddFeature(blueprintFeature);
							if (fact != null && desiredRanks > 1)
							{
								for (int i = 1; i < desiredRanks; i++)
								{
									fact.AddRank();
								}
							}
							EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
							{
								h.HandleLogMessage($"<color=#00FFFF><b>[Continuity of the Multiverse]</b></color> The <b>Echo of Past Sovereigns</b> awakens within you (Loop {Data.TotalRuns}: +{desiredRanks} competence to all skills, +{Math.Round((GetCoinLoopMultiplier() - 1.0) * 100.0)}% Cosmic Coins).");
							});
						}
						else if (fact.GetRank() < desiredRanks)
						{
							while (fact.GetRank() < desiredRanks)
							{
								fact.AddRank();
							}
						}
					}
				}
				if (Data.LoopBreakerAchieved && !Data.ShardRelicGranted)
				{
					UnitEntityData unitEntityData2 = BlueprintSafetyExtensions.SafeGetMainCharacter();
					BlueprintFeature blueprintFeature2 = ShardOfTheShatteredLoop.Get();
					if (unitEntityData2 != null && blueprintFeature2 != null && !unitEntityData2.Progression.Features.HasFact(blueprintFeature2))
					{
						unitEntityData2.Progression.Features.AddFeature(blueprintFeature2);
						DivineTokens.AddCoins(1000, "The Grand Arbiter");
						Data.ShardRelicGranted = true;
						Save();
						EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
						{
							h.HandleLogMessage("<color=#F5C542><b>[Sovereign Return - NG+]</b></color> You have retained the <b>Shard of the Shattered Loop</b> and 1,000 Cosmic Coins from your victory over the causal loop!");
						});
						foreach (string line in ConstellationDialogueBanter.GetLoopBreakerSovereignReturnBanter())
						{
							EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
							{
								h.HandleLogMessage(line);
							});
						}
					}
				}
				if (Data.RecentDeath)
				{
					ConstellationChatManager.OnDeathReload(Data.DeathCount, Data.LastArea);
					Data.RecentDeath = false;
					Save();
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in TimelineManager.OnAreaActivated: " + ex);
			}
		}
	}
}
