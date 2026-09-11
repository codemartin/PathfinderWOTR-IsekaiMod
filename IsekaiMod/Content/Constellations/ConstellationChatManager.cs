using System;
using System.Collections.Generic;
using Kingmaker.Blueprints.Classes;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Constellations
{
	public class ConstellationChatManager : ISelectAnswerHandler, ISubscriber, IGlobalSubscriber
	{
		private static bool _initialized = false;

		private static bool _warnedCheatingThisCombat = false;

		private static readonly Dictionary<string, int> _deityDisfavor = new Dictionary<string, int>();

		public static void Init()
		{
			if (!_initialized)
			{
				_initialized = true;
				EventBus.Subscribe(new ConstellationChatManager());
			}
		}

		private static void PostLog(string message)
		{
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(message);
			});
		}

		public static void ResetCombatWarning()
		{
			_warnedCheatingThisCombat = false;
		}

		public static void WarnRampantCheating(int count)
		{
			if (!_warnedCheatingThisCombat)
			{
				_warnedCheatingThisCombat = true;
				string[] array = new string[4]
				{
					$"<color=#FF4500><b>[Cosmic Outrage: Rampant Cheating Detected!]</b></color> <color=#FFD700>The Lantern King:</color> \"Ohoho! One otherworld anomaly was an amusing comedy. <b>{count} of them</b> is shameless, brazen hacking! Let's see how you fare when the multiverse fights back!\"",
					$"<color=#FF4500><b>[Cosmic Outrage: Soul Registry Error!]</b></color> <color=#9370DB>Pharasma:</color> \"The river of souls rejects this mockery. The cosmic registry is utterly corrupted with {count} illegal transmigrants. Cosmic balance demands immediate retribution!\"",
					"<color=#FF4500><b>[Cosmic Outrage: Anomaly Overload!]</b></color> <color=#DC143C>Gorum:</color> \"A whole platoon of cheat gods?! Bah! True warriors fight with grit, not cheat codes! Come, beasts, tear them to pieces!\"",
					$"<color=#FF4500><b>[Cosmic Outrage: Dimensional Collapse!]</b></color> <color=#00FFFF>Yog-Sothoth:</color> \"Dimensional boundary integrity collapsing. Excessive cheat density detected ({count} anomalies). Cosmic equilibrium enforcement protocols engaged!\""
				};
				Random random = new Random();
				PostLog(array[random.Next(array.Length)]);
				PostLog("<color=#DC143C><i>[Multiverse Retribution Active: All enemies have received heavily scaled combat stats, extra attacks, and hit points!]</i></color>");
			}
		}

		public static void WarnPlanarIncursion(int count)
		{
			PostLog($"<color=#FF4500><b>[Planar Incursion: Reality Distortion Critical!]</b></color> <color=#00FFFF>The Grand Arbiter:</color> \"Multiversal causality breach detected! Deploying {count} Planar Enforcers to enforce cosmic equilibrium!\"");
		}

		public static void WarnBossPhaseGate(string bossName)
		{
			PostLog("<color=#FF4500><b>[Cosmic Phase Gate: Dimensional Aegis Activated!]</b></color> <color=#FFD700>The Weaver:</color> \"The threads of fate deny a swift demise! " + bossName + " fractures the planar barrier, calling forth dimensional reinforcements!\"");
		}

		public static string GetPlayerDeityName()
		{
			UnitEntityData player = DivineTokens.GetPlayer();
			if (player == null)
			{
				return null;
			}
			foreach (Feature feature in player.Descriptor.Progression.Features)
			{
				BlueprintFeature blueprint = feature.Blueprint;
				if (blueprint.Groups != null && Array.IndexOf(blueprint.Groups, FeatureGroup.Deities) >= 0)
				{
					return blueprint.Name;
				}
			}
			return null;
		}

		public static bool IsOneOf13(string deityName)
		{
			if (string.IsNullOrEmpty(deityName))
			{
				return false;
			}
			string text = deityName.ToLower();
			if (!text.Contains("cayden") && !text.Contains("iomedae") && !text.Contains("asmodeus") && !text.Contains("desna") && !text.Contains("pharasma") && !text.Contains("calistria") && !text.Contains("nethys") && !text.Contains("gorum") && !text.Contains("besmara") && !text.Contains("lantern") && !text.Contains("chaldira") && !text.Contains("milani"))
			{
				return text.Contains("butterfly");
			}
			return true;
		}

		public static void TrackDisfavor(string deity, int amount)
		{
			if (!_deityDisfavor.ContainsKey(deity))
			{
				_deityDisfavor[deity] = 0;
			}
			_deityDisfavor[deity] += amount;
			if (_deityDisfavor[deity] >= 60)
			{
				_deityDisfavor[deity] = 0;
				PostLog("<color=#DC143C><b>[Planar Bounty Incursion!]</b></color> Your defiance has deeply offended <b>" + deity + "</b>! Hostile planar mercenaries and hunters have been summoned to test your survival!");
			}
		}

		public void HandleSelectAnswer(BlueprintAnswer answer)
		{
			if (answer == null)
			{
				return;
			}
			try
			{
				AlignmentShiftDirection alignmentShiftDirection = answer.AlignmentShift?.Direction ?? AlignmentShiftDirection.TrueNeutral;
				string answerText = answer.DisplayText ?? answer.name;
				int coinsAwarded = 0;
				string primarySponsor = null;
				List<string> alignmentBanter = ConstellationDialogueBanter.GetAlignmentBanter(alignmentShiftDirection, answerText, out coinsAwarded, out primarySponsor);
				double num;
				string playerDeityName;
				int num2;
				double num3;
				if (alignmentBanter != null && alignmentBanter.Count > 0)
				{
					foreach (string item in alignmentBanter)
					{
						PostLog(item);
					}
					if (coinsAwarded > 0)
					{
						num = ConstellationDialogueBanter.RollMultiplier(primarySponsor, out var shoutoutMsg);
						if (!string.IsNullOrEmpty(shoutoutMsg))
						{
							PostLog(shoutoutMsg);
						}
						playerDeityName = GetPlayerDeityName();
						if (!string.IsNullOrEmpty(playerDeityName))
						{
							if ((primarySponsor.Contains("Lucky Drunk") && playerDeityName.Contains("Cayden")) || (primarySponsor.Contains("Inheritor") && playerDeityName.Contains("Iomedae")) || (primarySponsor.Contains("Prince of Darkness") && playerDeityName.Contains("Asmodeus")) || (primarySponsor.Contains("Song of the Spheres") && playerDeityName.Contains("Desna")) || (primarySponsor.Contains("Lady of Graves") && playerDeityName.Contains("Pharasma")) || (primarySponsor.Contains("Savored Sting") && playerDeityName.Contains("Calistria")) || (primarySponsor.Contains("All-Seeing Eye") && playerDeityName.Contains("Nethys")) || (primarySponsor.Contains("Lord in Iron") && playerDeityName.Contains("Gorum")) || (primarySponsor.Contains("Pirate Queen") && playerDeityName.Contains("Besmara")) || (primarySponsor.Contains("Laughing King") && playerDeityName.Contains("Lantern King")) || (primarySponsor.Contains("Mischievous Friend") && playerDeityName.Contains("Chaldira")))
							{
								num2 = 1;
								goto IL_0220;
							}
							if (primarySponsor.Contains("Silence Between"))
							{
								num2 = (playerDeityName.Contains("Black Butterfly") ? 1 : 0);
								if (num2 != 0)
								{
									goto IL_0220;
								}
							}
							else
							{
								num2 = 0;
							}
						}
						else
						{
							num2 = 0;
						}
						num3 = 1.0;
						goto IL_0229;
					}
				}
				goto IL_02a8;
				IL_02a8:
				switch (alignmentShiftDirection)
				{
				case AlignmentShiftDirection.Evil:
					TrackDisfavor("The Inheritor", 15);
					TrackDisfavor("The Song of the Spheres", 15);
					break;
				case AlignmentShiftDirection.Chaotic:
					TrackDisfavor("The Prince of Darkness", 15);
					break;
				case AlignmentShiftDirection.Lawful:
					TrackDisfavor("The Laughing King", 15);
					break;
				}
				if (answer.AssetGuid != null)
				{
					TimelineManager.RecordChoice(answer.AssetGuid.ToString());
				}
				return;
				IL_0229:
				double num4 = num3;
				int amount = (int)Math.Round((double)coinsAwarded * num * num4);
				if (num2 != 0)
				{
					PostLog("<color=#F5C542><b>[Patron Sponsor Bonus!]</b></color> <b>" + playerDeityName + "</b> backs your decision with a +25% personal favor bonus!");
				}
				else if (!string.IsNullOrEmpty(playerDeityName) && playerDeityName.Contains("Atheism"))
				{
					PostLog("<color=#F5C542><b>[The Free Agent]</b></color> The watching deities scoff at your persistent refusal to kneel, tossing coins merely to see how long your blasphemy lasts!");
				}
				else if (!string.IsNullOrEmpty(playerDeityName) && !IsOneOf13(playerDeityName))
				{
					PostLog("<color=#F5C542><b>[Guest Sponsor: " + playerDeityName + "]</b></color> Your patron observes the broadcast from outside the 13 Seats with quiet pride!");
				}
				DivineTokens.AddCoins(amount, primarySponsor);
				goto IL_02a8;
				IL_0220:
				num3 = 1.25;
				goto IL_0229;
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ConstellationChatManager.HandleSelectAnswer: " + ex);
			}
		}

		public static void OnDeathReload(int deathCount, string lastArea)
		{
			try
			{
				List<string> deathReloadBanter = ConstellationDialogueBanter.GetDeathReloadBanter(deathCount, lastArea);
				if (deathReloadBanter == null)
				{
					return;
				}
				foreach (string item in deathReloadBanter)
				{
					PostLog(item);
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ConstellationChatManager.OnDeathReload: " + ex);
			}
		}

		public static void OnNewRun(int runCount)
		{
			try
			{
				List<string> metaLoopBanter = ConstellationDialogueBanter.GetMetaLoopBanter(runCount);
				if (metaLoopBanter == null)
				{
					return;
				}
				foreach (string item in metaLoopBanter)
				{
					PostLog(item);
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ConstellationChatManager.OnNewRun: " + ex);
			}
		}

		public static void OnActProgression(int act)
		{
			try
			{
				List<string> actProgressionBanter = ConstellationDialogueBanter.GetActProgressionBanter(act);
				if (actProgressionBanter == null)
				{
					return;
				}
				foreach (string item in actProgressionBanter)
				{
					PostLog(item);
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ConstellationChatManager.OnActProgression: " + ex);
			}
		}
	}
}
