using System;
using System.Collections.Generic;
using Kingmaker;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Controllers.Dialog;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Constellations
{
	public class ConstellationChatManager : ISelectAnswerHandler, ISubscriber, IGlobalSubscriber, IDialogCueHandler, IAreaHandler
	{
		private static bool _initialized = false;

		private static readonly HashSet<string> _triggeredMilestones = new HashSet<string>();

		private static readonly HashSet<string> _visitedAreasInCycle = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		private static readonly Random _rng = new Random();

		private static bool _warnedCheatingThisCombat = false;

		private static readonly Dictionary<string, int> _deityDisfavor = new Dictionary<string, int>();

		public static bool HasTriggeredMilestone(string milestone)
		{
			return _triggeredMilestones.Contains(milestone);
		}

		public static void MarkMilestoneTriggered(string milestone)
		{
			_triggeredMilestones.Add(milestone);
		}

		public void OnAreaBeginUnloading()
		{
		}

		public void OnAreaDidLoad()
		{
			try
			{
				ConstellationChatOverlay.EnsureInstance();
				DivineTokens.SyncInventoryCoins(DivineTokens.GetBalance());
				BlueprintArea blueprintArea = Game.Instance?.CurrentlyLoadedArea;
				if (blueprintArea == null)
				{
					return;
				}
				string name = blueprintArea.name;
				if (string.IsNullOrEmpty(name) || _visitedAreasInCycle.Contains(name))
				{
					return;
				}
				int currentCycle = TimelineManager.GetCurrentCycle();
				string playerDeityName = GetPlayerDeityName();
				bool hasAvatar = DeityAvatarAscension.HasAnyAvatar(DivineTokens.GetPlayer());
				if (!ConstellationDialogueBanter.TryGetAreaBanter(name, currentCycle, playerDeityName, hasAvatar, out var result))
				{
					return;
				}
				_visitedAreasInCycle.Add(name);
				foreach (string line in result.Lines)
				{
					PostLog(line, result.Sponsor, result.Category, result.Coins, result.SceneContext);
				}
				if (result.Coins > 0)
				{
					DivineTokens.AddCoins(result.Coins, result.Sponsor);
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ConstellationChatManager.OnAreaDidLoad: " + ex);
			}
		}

		public static void Init()
		{
			if (!_initialized)
			{
				_initialized = true;
				EventBus.Subscribe(new ConstellationChatManager());
				ConstellationChatOverlay.EnsureInstance();
				ConstellationBounties.Init();
			}
		}

		public static void ResetMilestones()
		{
			_triggeredMilestones.Clear();
			_visitedAreasInCycle.Clear();
		}

		public static void PostLog(string message, string sponsor = null, ConstellationCategory category = ConstellationCategory.Subclass, int coins = 0, string sceneContext = null)
		{
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(message);
			});
			ConstellationEventHistory.RecordEvent(message, sponsor, category, coins, sceneContext);
			ConstellationChatOverlay.AddMessage(message, sponsor, coins);
		}

		public static void PostSuperChat(string sponsor, int coins, string title, string message)
		{
			string text = "<color=#FFD700><b>[VIP SUPER-CHAT: " + title.ToUpper() + "]</b></color>";
			string text2 = $"<color=#FFD700><b>★ {sponsor}</b></color> has sent a <b>VIP Super-Chat ({coins} Cosmic Coins)</b>!";
			string formattedMessage = text + "\n" + text2 + "\n<i>\"" + message + "\"</i>";
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(formattedMessage);
			});
			ConstellationEventHistory.RecordEvent(formattedMessage, sponsor, ConstellationCategory.Quest, coins, title);
			ConstellationChatOverlay.AddMessage(formattedMessage, sponsor, coins);
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
					$"<color=#FF4500><b>[Cosmic Outrage: Soul Registry Error!]</b></color> <color=#9370DB>Pharasma:</color> \"The river of souls rejects this mockery. The cosmic registry is utterly corrupted with {count} illegal otherworlders. Cosmic balance demands immediate retribution!\"",
					"<color=#FF4500><b>[Cosmic Outrage: Anomaly Overload!]</b></color> <color=#DC143C>Gorum:</color> \"A whole platoon of cheat gods?! Bah! True warriors fight with grit, not cheat codes! Come, beasts, tear them to pieces!\"",
					$"<color=#FF4500><b>[Cosmic Outrage: Dimensional Collapse!]</b></color> <color=#00FFFF>Yog-Sothoth:</color> \"Dimensional boundary integrity collapsing. Excessive cheat density detected ({count} anomalies). Cosmic equilibrium enforcement protocols engaged!\""
				};
				Random random = new Random();
				PostLog(array[random.Next(array.Length)], "Cosmic Threat", ConstellationCategory.Combat, 0, "Combat Retribution");
				PostLog("<color=#DC143C><i>[Multiverse Retribution Active: All enemies have received heavily scaled combat stats, extra attacks, and hit points!]</i></color>", "Cosmic Threat", ConstellationCategory.Combat, 0, "Combat Retribution");
			}
		}

		public static void WarnPlanarIncursion(int count)
		{
			PostLog($"<color=#FF4500><b>[Planar Incursion: Reality Distortion Critical!]</b></color> <color=#00FFFF>The Grand Arbiter:</color> \"Multiversal causality breach detected! Deploying {count} Planar Enforcers to enforce cosmic equilibrium!\"", "The Grand Arbiter", ConstellationCategory.Combat, 0, "Planar Incursion");
		}

		public static void WarnBossPhaseGate(string bossName)
		{
			PostLog("<color=#FF4500><b>[Cosmic Phase Gate: Dimensional Aegis Activated!]</b></color> <color=#FFD700>The Weaver:</color> \"The threads of fate deny a swift demise! " + bossName + " fractures the planar barrier, calling forth dimensional reinforcements!\"", "The Weaver", ConstellationCategory.Combat, 0, "Boss Phase Gate");
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
				if (!(blueprint is BlueprintFeatureSelection) && !(blueprint.name == "DeitySelection") && blueprint.Groups != null && Array.IndexOf(blueprint.Groups, FeatureGroup.Deities) >= 0)
				{
					string name = blueprint.Name;
					if (!string.IsNullOrEmpty(name) && !name.Equals("Deity Selection", StringComparison.OrdinalIgnoreCase))
					{
						return name;
					}
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
				PostLog("<color=#DC143C><b>[Planar Bounty Incursion!]</b></color> Your defiance has deeply offended <b>" + deity + "</b>! Hostile planar mercenaries and hunters have been summoned to test your survival!", deity, ConstellationCategory.DeityPatron, 0, "Deity Disfavor Incursion");
			}
		}

		public void HandleOnCueShow(CueShowData cueShowData)
		{
			if (cueShowData?.Cue == null)
			{
				return;
			}
			try
			{
				ConstellationChatOverlay.EnsureInstance();
				if (!(((cueShowData.Cue.AssetGuid != null) ? cueShowData.Cue.AssetGuid.ToString() : string.Empty) == "03715430fba15d141995a2f7e5d6cc3b") || _triggeredMilestones.Contains("DaeranDitchCue"))
				{
					return;
				}
				_triggeredMilestones.Add("DaeranDitchCue");
				List<string> prologueDaeranDitchBanter = ConstellationDialogueBanter.GetPrologueDaeranDitchBanter(TimelineManager.GetCurrentCycle(), out var coinsAwarded, out var primarySponsor);
				if (prologueDaeranDitchBanter != null)
				{
					foreach (string item in prologueDaeranDitchBanter)
					{
						PostLog(item, primarySponsor, ConstellationCategory.Prologue, 0, "Daeran Ditches the Convalescents");
					}
				}
				if (coinsAwarded > 0)
				{
					DivineTokens.AddCoins(coinsAwarded, primarySponsor);
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ConstellationChatManager.HandleOnCueShow: " + ex);
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
				ConstellationChatOverlay.EnsureInstance();
				AlignmentShiftDirection alignmentShiftDirection = answer.AlignmentShift?.Direction ?? AlignmentShiftDirection.TrueNeutral;
				string text = answer.DisplayText ?? answer.name;
				string text2 = ((answer.AssetGuid != null) ? answer.AssetGuid.ToString() : string.Empty);
				string text3 = answer.name ?? string.Empty;
				string text4 = Game.Instance?.DialogController?.Dialog?.name ?? string.Empty;
				UnitEntityData unitEntityData = Game.Instance?.DialogController?.CurrentSpeaker;
				string text5 = unitEntityData?.CharacterName ?? unitEntityData?.Blueprint?.name ?? string.Empty;
				string text6 = Game.Instance?.CurrentlyLoadedArea?.AreaDisplayName ?? Game.Instance?.CurrentlyLoadedArea?.name ?? string.Empty;
				string playerDeityName = GetPlayerDeityName();
				bool hasAvatar = DeityAvatarAscension.HasAnyAvatar(DivineTokens.GetPlayer());
				int currentCycle = TimelineManager.GetCurrentCycle();
				List<string> list = null;
				int coinsAwarded = 0;
				string primarySponsor = null;
				string sceneContext = null;
				ConstellationCategory category = ConstellationCategory.Subclass;
				DialogueContext ctx = new DialogueContext(playerDeityName, hasAvatar, currentCycle, text4, text, text2, text3, text5, text6);
				if (ConstellationDialogueBanter.TryGetAnswerBanter(in ctx, out var result))
				{
					string item = ((!string.IsNullOrEmpty(ctx.AnswerGuid)) ? ctx.AnswerGuid : ctx.AnswerName);
					if (!_triggeredMilestones.Contains(item))
					{
						_triggeredMilestones.Add(item);
						list = result.Lines;
						coinsAwarded = result.Coins;
						primarySponsor = result.Sponsor;
						category = result.Category;
						sceneContext = result.SceneContext;
					}
				}
				else
				{
					if (ConstellationDialogueBanter.IsExitOrInformationalAnswer(text, text3))
					{
						return;
					}
					if (ConstellationDialogueBanter.TryGetSceneBanter(text4, in ctx, out var result2))
					{
						string item2 = "Scene_" + (result2.SceneContext ?? text4);
						if (!_triggeredMilestones.Contains(item2))
						{
							_triggeredMilestones.Add(item2);
							list = result2.Lines;
							coinsAwarded = result2.Coins;
							primarySponsor = result2.Sponsor;
							category = result2.Category;
							sceneContext = result2.SceneContext;
						}
					}
					else if (!_triggeredMilestones.Contains("PrologueStatusWindow") && text3 == "IsekaiPrologueSquareAwakening")
					{
						_triggeredMilestones.Add("PrologueStatusWindow");
						list = ConstellationDialogueBanter.GetPrologueStatusWindowBanter(currentCycle, out coinsAwarded, out primarySponsor);
						category = ConstellationCategory.Prologue;
						sceneContext = "Kenabres Square: Status Window Protocol";
					}
					else
					{
						if (!_triggeredMilestones.Contains("PrologueAwakening"))
						{
							switch (text2)
							{
							case "eea5643f25d86dc43bb3024bbb83212f":
							case "47cbc8ab5ca57a14684db2344d8057f4":
							case "bad44b6d69be7b64e8db6a07dcdcd9fc":
							case "db8320c0269ad9c4bb4408688adb8067":
								goto IL_0320;
							}
							if (text3.StartsWith("IsekaiStretcher") || text4.Contains("WelcomeDialogue"))
							{
								goto IL_0320;
							}
						}
						string compKey;
						string npcKey;
						string dlcKey;
						if (!_triggeredMilestones.Contains("PrologueHulrun") && (text3.StartsWith("IsekaiHulrun") || text2 == "6898ccdc3dd26944c8e0ebb39d8c7777" || text2 == "820751da02c91d54481d8732082b4806" || text3.Contains("Hulrun")))
						{
							_triggeredMilestones.Add("PrologueHulrun");
							list = ConstellationDialogueBanter.GetPrologueHulrunBanter(out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Prologue;
							sceneContext = "Kenabres Square: Prelate Hulrun";
						}
						else if (!_triggeredMilestones.Contains("PrologueTerendelev") && (text3.StartsWith("IsekaiWelcome") || (text4.Contains("WelcomeDialogue") && _triggeredMilestones.Contains("PrologueAwakening"))))
						{
							_triggeredMilestones.Add("PrologueTerendelev");
							list = ConstellationDialogueBanter.GetPrologueTerendelevBanter(out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Prologue;
							sceneContext = "Kenabres Square: Terendelev";
						}
						else if (!_triggeredMilestones.Contains("PrologueDeskari") && (text4.Contains("StitchGivesCrossbow") || text3.Contains("Stitch") || text3.Contains("Crossbow")))
						{
							_triggeredMilestones.Add("PrologueDeskari");
							list = ConstellationDialogueBanter.GetPrologueDeskariInvasionBanter(out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Prologue;
							sceneContext = "Kenabres Square: Deskari Invasion";
						}
						else if (!_triggeredMilestones.Contains("PrologueCavesSeelah") && (text4.Contains("MeetSeelahAnevia") || text3.StartsWith("IsekaiSeelah") || text3.Contains("IsekaiTalkBackStars")))
						{
							_triggeredMilestones.Add("PrologueCavesSeelah");
							list = ConstellationDialogueBanter.GetPrologueCavesAwakeningBanter(out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Prologue;
							sceneContext = "Caves: Rescue Anevia";
						}
						else if (!_triggeredMilestones.Contains("PrologueCamellia") && (text4.Contains("MeetCamelia") || text3.StartsWith("IsekaiCamelia")))
						{
							_triggeredMilestones.Add("PrologueCamellia");
							list = ConstellationDialogueBanter.GetPrologueCamelliaEncounterBanter(out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Prologue;
							sceneContext = "Caves: Meeting Camellia";
						}
						else if (!_triggeredMilestones.Contains("PrologueLann") && (text4.Contains("MeetLann") || text3.StartsWith("IsekaiLann")))
						{
							_triggeredMilestones.Add("PrologueLann");
							list = ConstellationDialogueBanter.GetPrologueLannWenduagBanter(out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Prologue;
							sceneContext = "Caves: Lann & Wenduag";
						}
						else if (!_triggeredMilestones.Contains("Act1DefendersHeart") && (text3.Contains("DefendersHeartMessengerAlarm") || text3.StartsWith("IsekaiCouncil") || text2 == "ab362e5b1af4b354ea59d583376cf661" || text4.Contains("TavernAttack") || text4.Contains("DefendersHeart")))
						{
							_triggeredMilestones.Add("Act1DefendersHeart");
							list = ConstellationDialogueBanter.GetAct1DefendersHeartBanter(currentCycle, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "Defender's Heart: The Defense of Kenabres";
						}
						else if (!_triggeredMilestones.Contains("Act1WardstoneClimax") && (text3.StartsWith("IsekaiWardstoneMythic") || text2 == "be1fcfe08bbae9a46b5105c4d1e4be7c" || text4.Contains("Wardstone") || text3.Contains("WardstoneMythic")))
						{
							_triggeredMilestones.Add("Act1WardstoneClimax");
							list = ConstellationDialogueBanter.GetWardstoneMythicAwakeningBanter(currentCycle, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "Gray Garrison: Wardstone Mythic Climax";
						}
						else if (!_triggeredMilestones.Contains("Act2GalfreyAppointment") && (text3.StartsWith("IsekaiAct1QueenGalfrey") || text3.StartsWith("IsekaiWarCamp") || text2 == "8c7d0b0b6a1701d49835740d65d3d922" || text4.Contains("GalfreyPostGarrison") || text4.Contains("WarCampGalfrey")))
						{
							_triggeredMilestones.Add("Act2GalfreyAppointment");
							list = ConstellationDialogueBanter.GetAct2GalfreyWarCampBanter(currentCycle, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "Mantle of the Crusade: Appointing the Knight-Commander";
						}
						else if (!_triggeredMilestones.Contains("Act2DrezenLiberation") && (text3.StartsWith("IsekaiDrezenProclamation") || text2 == "44704bddb6223b84989dd26bcf20b601" || text4.Contains("DrezenCitadel") || text4.Contains("DrezenBanner")))
						{
							_triggeredMilestones.Add("Act2DrezenLiberation");
							list = ConstellationDialogueBanter.GetAct2DrezenLiberationBanter(currentCycle, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "Citadel of Drezen: Reclaimed & The Mythic Mantle";
						}
						else if (!_triggeredMilestones.Contains("Act3AreeluLabTruth") && (text3.StartsWith("IsekaiAreeluLabTruth") || text2 == "cd9c9facc3a8ded4e9683cde8958295e" || text4.Contains("AreeluLab")))
						{
							_triggeredMilestones.Add("Act3AreeluLabTruth");
							list = ConstellationDialogueBanter.GetAct3AreeluLabTruthBanter(currentCycle, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "Areelu's Laboratory: The Unscripted Soul";
						}
						else if (!_triggeredMilestones.Contains("Act4BaphometConfrontation") && (text3.StartsWith("IsekaiBaphomet") || text2 == "9ab9ad8f6e11d67499b48fd595e50972" || text4.Contains("Baphomet")))
						{
							_triggeredMilestones.Add("Act4BaphometConfrontation");
							list = ConstellationDialogueBanter.GetAct4BaphometConfrontationBanter(currentCycle, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "Colyphyr Mines: Defying the Lord of the Labyrinth";
						}
						else if (!_triggeredMilestones.Contains("Act5DivineSummit") && (text3.StartsWith("IsekaiGoddessesSummit") || text2 == "e3a71f123c7aae7409d08855827dbea5" || text4.Contains("GoddessesSummit") || text4.Contains("IomedaeSummit")))
						{
							_triggeredMilestones.Add("Act5DivineSummit");
							list = ConstellationDialogueBanter.GetAct5DivineSummitBanter(currentCycle, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "The Goddesses Summit: Heaven & Abyss Face the Wildcard";
						}
						else if (!_triggeredMilestones.Contains("Act5IzTerendelev") && (text3.StartsWith("IsekaiTerendelev") || text4.Contains("TerendelevIz") || text4.Contains("UndeadTerendelev")))
						{
							_triggeredMilestones.Add("Act5IzTerendelev");
							list = ConstellationDialogueBanter.GetAct5IzTerendelevBanter(currentCycle, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "City of Iz: The Dragon's Final Rest";
						}
						else if (!_triggeredMilestones.Contains("Act6ThresholdEnding") && (text3.StartsWith("IsekaiEnding") || text2 == "1dd910a1bafd4af4f818b65a5eed2a46" || text4.Contains("ThresholdEnding")))
						{
							_triggeredMilestones.Add("Act6ThresholdEnding");
							string endingId = "UnwrittenDawn";
							if (text3.Contains("InterdimensionalTraveler"))
							{
								endingId = "InterdimensionalTraveler";
							}
							else if (text3.Contains("EternalActor"))
							{
								endingId = "EternalActor";
							}
							list = ConstellationDialogueBanter.GetAct6ThresholdLoopBreakerBanter(endingId, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.MetaLoop;
							sceneContext = "The Threshold: Grand Multiverse Finale";
						}
						else if (!_triggeredMilestones.Contains("Act1EmberRescue") && (text3.StartsWith("IsekaiMeetEmber") || text2 == "4bba85261e5f2064989ebdc878c0228a" || text4.Contains("MeetEmber") || text4.Contains("EmberRescue")))
						{
							_triggeredMilestones.Add("Act1EmberRescue");
							list = ConstellationDialogueBanter.GetAct1EmberRescueBanter(currentCycle, playerDeityName, hasAvatar, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "Market Square: Rescuing Ember";
						}
						else if (!_triggeredMilestones.Contains("Act1HulrunRamien") && (text3.StartsWith("IsekaiMarketSquareFeud") || text2 == "cc0bf28eab0e60b429dac7b53ec8e4d2" || text2 == "e27807b731f3b1a4eb19c1a04fdfcf53" || (text4.Contains("Hulrun") && text4.Contains("Ramien"))))
						{
							_triggeredMilestones.Add("Act1HulrunRamien");
							list = ConstellationDialogueBanter.GetAct1HulrunRamienBanter(currentCycle, playerDeityName, hasAvatar, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "Market Square: Hulrun vs. Ramien";
						}
						else if (!_triggeredMilestones.Contains("Act1WoljifRecruit") && (text3.StartsWith("IsekaiWoljifCell") || text2 == "a9e697b9ad6fa1c4d9c2ae0895bd56d2" || text4.Contains("WoljifInPrison") || text4.Contains("WoljifCell")))
						{
							_triggeredMilestones.Add("Act1WoljifRecruit");
							list = ConstellationDialogueBanter.GetAct1WoljifRecruitBanter(currentCycle, playerDeityName, hasAvatar, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "Defender's Heart: Recruiting Woljif";
						}
						else if (!_triggeredMilestones.Contains("Act2LepersSmile") && (text3.StartsWith("IsekaiLepersSmile") || text2 == "7be206b993f3dea49a89a88ce5e6ce6f" || text4.Contains("LepersSmile") || text4.Contains("VescavorQueen")))
						{
							_triggeredMilestones.Add("Act2LepersSmile");
							list = ConstellationDialogueBanter.GetAct2LepersSmileBanter(currentCycle, playerDeityName, hasAvatar, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "Leper's Smile: Vescavor Swarm Purge";
						}
						else if (!_triggeredMilestones.Contains("Act2RegillRecruit") && (text3.StartsWith("IsekaiRegillFieldTriage") || text3.StartsWith("IsekaiRegillOverlord") || text2 == "ab4c009b7ff77da4fad0abe34ad79c08" || text4.Contains("RegillRescue") || text4.Contains("HellknightsCamp")))
						{
							_triggeredMilestones.Add("Act2RegillRecruit");
							list = ConstellationDialogueBanter.GetAct2RegillRecruitBanter(currentCycle, playerDeityName, hasAvatar, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "Reliable Redoubt: Regill & Hellknights";
						}
						else if (!_triggeredMilestones.Contains("Act2DaeranEstate") && (text3.StartsWith("IsekaiDaeranManor") || text2 == "583314478921a19419304d5f83b72109" || text4.Contains("DaeranParty") || text4.Contains("ArendaeEstate")))
						{
							_triggeredMilestones.Add("Act2DaeranEstate");
							list = ConstellationDialogueBanter.GetAct2DaeranEstateBanter(currentCycle, playerDeityName, hasAvatar, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "Arendae Manor: Daeran's Banquet";
						}
						else if (!_triggeredMilestones.Contains("Act3ArueshalaePrison") && (text4.Contains("MeetArueshalae") || text4.Contains("ArueshalaeRedoubt") || text4.Contains("ArueshalaePrison") || (text3.Contains("Arueshalae") && (text3.Contains("Prison") || text3.Contains("Lucid") || text3.Contains("Dream")))))
						{
							_triggeredMilestones.Add("Act3ArueshalaePrison");
							list = ConstellationDialogueBanter.GetAct3ArueshalaePrisonBanter(currentCycle, playerDeityName, hasAvatar, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "Drezen Prison: Arueshalae's Starlight";
						}
						else if (!_triggeredMilestones.Contains("Act3Wintersun") && (text3.StartsWith("IsekaiWintersun") || text2 == "f240afdd8595d2340aa18ecf5ed233a3" || text4.Contains("Jerribeth") || text4.Contains("Wintersun")))
						{
							_triggeredMilestones.Add("Act3Wintersun");
							list = ConstellationDialogueBanter.GetAct3WintersunBanter(currentCycle, playerDeityName, hasAvatar, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "Wintersun: Jerribeth's Illusion";
						}
						else if (!_triggeredMilestones.Contains("Act3IvorySanctum") && (text3.StartsWith("IsekaiIvorySanctum") || text3.StartsWith("IsekaiXanthir") || text2 == "cbe3b0c3ea8b47045b66c085c1a1b354" || text4.Contains("Xanthir")))
						{
							_triggeredMilestones.Add("Act3IvorySanctum");
							list = ConstellationDialogueBanter.GetAct3IvorySanctumBanter(currentCycle, playerDeityName, hasAvatar, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "Ivory Sanctum: Xanthir Vang";
						}
						else if (!_triggeredMilestones.Contains("Act4Fleshmarket") && (text3.StartsWith("IsekaiFleshmarket") || text2 == "c896137fe31c15445bba592672ebfbc9" || text4.Contains("Fleshmarket") || text4.Contains("Dyunk")))
						{
							_triggeredMilestones.Add("Act4Fleshmarket");
							list = ConstellationDialogueBanter.GetAct4FleshmarketBanter(currentCycle, playerDeityName, hasAvatar, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "Alushinyrra: Fleshmarket Liberation";
						}
						else if (!_triggeredMilestones.Contains("Act4NocticulaAudience") && (text3.StartsWith("IsekaiNocticula") || text2 == "75678bafb5e33854baaee17ee6eaf69c" || text4.Contains("Nocticula")))
						{
							_triggeredMilestones.Add("Act4NocticulaAudience");
							list = ConstellationDialogueBanter.GetAct4NocticulaAudienceBanter(currentCycle, playerDeityName, hasAvatar, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "House of Silken Shadows: Lady Nocticula";
						}
						else if (!_triggeredMilestones.Contains("Act5IzGalfreyFate") && (text3.StartsWith("IsekaiIz") || text2 == "ec740ac6a039d8c4b83a076cee6c1ff0" || text2 == "20a1be61aa7680e45890656b204e6fe8" || text4.Contains("IzGalfrey") || text4.Contains("IrabethIz")))
						{
							_triggeredMilestones.Add("Act5IzGalfreyFate");
							list = ConstellationDialogueBanter.GetAct5IzGalfreyFateBanter(currentCycle, playerDeityName, hasAvatar, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "City of Iz: The Triad Salvation";
						}
						else if (!_triggeredMilestones.Contains("Act6ThresholdEveCamp") && (text3.Contains("Farewell") || text4.Contains("CompanionFarewell") || text4.Contains("ThresholdCamp") || text2 == "bb3528754c7e741438acef95ec3b6430"))
						{
							_triggeredMilestones.Add("Act6ThresholdEveCamp");
							list = ConstellationDialogueBanter.GetAct6ThresholdEveCampBanter(currentCycle, playerDeityName, hasAvatar, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "The Threshold: Eve of the Breach";
						}
						else if (!string.IsNullOrEmpty(playerDeityName) && !IsOneOf13(playerDeityName) && !playerDeityName.Contains("Atheism") && !_triggeredMilestones.Contains("GuestDeity_" + playerDeityName))
						{
							_triggeredMilestones.Add("GuestDeity_" + playerDeityName);
							list = ConstellationDialogueBanter.GetGuestDeityBanter(playerDeityName, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.DeityPatron;
							sceneContext = "Guest Constellation: " + playerDeityName;
						}
						else if (IsCompanionSpeaker(text5, text4, out compKey))
						{
							list = ConstellationDialogueBanter.GetCompanionDialogueBanter(compKey, text, currentCycle, playerDeityName, hasAvatar, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Subclass;
							sceneContext = "Companion: " + text5;
						}
						else if (IsMajorNpcSpeaker(text5, text4, out npcKey))
						{
							list = ConstellationDialogueBanter.GetNpcDialogueBanter(npcKey, text, currentCycle, playerDeityName, hasAvatar, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "NPC: " + text5;
						}
						else if (IsDlcContext(text6, text4, out dlcKey))
						{
							list = ConstellationDialogueBanter.GetDlcDialogueBanter(dlcKey, text, currentCycle, playerDeityName, hasAvatar, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Quest;
							sceneContext = "DLC: " + text6;
						}
						else if (alignmentShiftDirection != AlignmentShiftDirection.TrueNeutral || text.IndexOf("isekai", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("martial god", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("god emperor", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("overlord", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("slime", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("devourer", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("shadow monarch", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("mastermind", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("hero", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("villain", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("retinue", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("radiance", StringComparison.OrdinalIgnoreCase) >= 0 || (text.IndexOf("lann", StringComparison.OrdinalIgnoreCase) >= 0 && text.IndexOf("wenduag", StringComparison.OrdinalIgnoreCase) >= 0) || text.IndexOf("wardstone", StringComparison.OrdinalIgnoreCase) >= 0 || text.IndexOf("krebus", StringComparison.OrdinalIgnoreCase) >= 0)
						{
							list = ConstellationDialogueBanter.GetAlignmentBanter(alignmentShiftDirection, text, out coinsAwarded, out primarySponsor);
							if (list != null && list.Count > 0)
							{
								category = ((alignmentShiftDirection == AlignmentShiftDirection.TrueNeutral) ? ConstellationCategory.Subclass : ConstellationCategory.AlignmentShift);
								sceneContext = ((alignmentShiftDirection == AlignmentShiftDirection.TrueNeutral) ? "Archetype Roleplay" : $"Alignment Choice ({alignmentShiftDirection})");
							}
						}
						else if (_rng.Next(100) < 50)
						{
							list = ConstellationDialogueBanter.GetContinuousDialogueBanter(text, text4, text6, currentCycle, playerDeityName, hasAvatar, out coinsAwarded, out primarySponsor);
							category = ConstellationCategory.Subclass;
							sceneContext = "Dialogue Choice";
						}
					}
				}
				goto IL_1298;
				IL_162e:
				double num = 1.0;
				goto IL_1642;
				IL_1298:
				double num2;
				int num3;
				if (list != null && list.Count > 0)
				{
					foreach (string item3 in list)
					{
						PostLog(item3, primarySponsor, category, coinsAwarded, sceneContext);
					}
					if (coinsAwarded > 0)
					{
						num2 = ConstellationDialogueBanter.RollMultiplier(primarySponsor, out var shoutoutMsg);
						if (!string.IsNullOrEmpty(shoutoutMsg))
						{
							PostLog(shoutoutMsg, primarySponsor, category, 0, sceneContext);
						}
						if (!string.IsNullOrEmpty(playerDeityName))
						{
							if ((!primarySponsor.Contains("Lucky Drunk") || !playerDeityName.Contains("Cayden")) && (!primarySponsor.Contains("Inheritor") || !playerDeityName.Contains("Iomedae")) && (!primarySponsor.Contains("Prince of Darkness") || !playerDeityName.Contains("Asmodeus")) && (!primarySponsor.Contains("Song of the Spheres") || !playerDeityName.Contains("Desna")) && (!primarySponsor.Contains("Lady of Graves") || !playerDeityName.Contains("Pharasma")) && (!primarySponsor.Contains("Savored Sting") || !playerDeityName.Contains("Calistria")) && (!primarySponsor.Contains("All-Seeing Eye") || !playerDeityName.Contains("Nethys")) && (!primarySponsor.Contains("Lord in Iron") || !playerDeityName.Contains("Gorum")) && (!primarySponsor.Contains("Pirate Queen") || !playerDeityName.Contains("Besmara")) && (!primarySponsor.Contains("Laughing King") || !playerDeityName.Contains("Lantern King")) && (!primarySponsor.Contains("Mischievous Friend") || !playerDeityName.Contains("Chaldira")) && (!primarySponsor.Contains("Silence Between") || !playerDeityName.Contains("Black Butterfly")) && (!primarySponsor.Contains("Pallid Princess") || !playerDeityName.Contains("Urgathoa")) && (!primarySponsor.Contains("Key and the Gate") || !playerDeityName.Contains("Yog")) && (!primarySponsor.Contains("Father of Creation") || !playerDeityName.Contains("Torag")) && (!primarySponsor.Contains("Dawnflower") || !playerDeityName.Contains("Sarenrae")) && (!primarySponsor.Contains("Master of the First Vault") || !playerDeityName.Contains("Abadar")) && (!primarySponsor.Contains("Eternal Rose") || !playerDeityName.Contains("Shelyn")) && (!primarySponsor.Contains("Old Deadeye") || !playerDeityName.Contains("Erastil")) && (!primarySponsor.Contains("Master of Masters") || !playerDeityName.Contains("Irori")) && (!primarySponsor.Contains("Wind and the Waves") || !playerDeityName.Contains("Gozreh")) && (!primarySponsor.Contains("Midnight Lord") || !playerDeityName.Contains("Zon-Kuthon")) && (!primarySponsor.Contains("Shimmering Maiden") || !playerDeityName.Contains("Pulura")) && (!primarySponsor.Contains("Fivefold Order") || !playerDeityName.Contains("Godclaw")))
							{
								if (string.IsNullOrEmpty(primarySponsor))
								{
									num3 = 0;
									goto IL_162e;
								}
								if (primarySponsor.IndexOf(playerDeityName, StringComparison.OrdinalIgnoreCase) < 0)
								{
									num3 = ((playerDeityName.IndexOf(primarySponsor, StringComparison.OrdinalIgnoreCase) >= 0) ? 1 : 0);
									if (num3 == 0)
									{
										goto IL_162e;
									}
								}
								else
								{
									num3 = 1;
								}
							}
							else
							{
								num3 = 1;
							}
							num = 1.25;
							goto IL_1642;
						}
						num3 = 0;
						goto IL_162e;
					}
				}
				goto IL_16d8;
				IL_1642:
				double num4 = num;
				int amount = (int)Math.Round((double)coinsAwarded * num2 * num4);
				if (num3 != 0)
				{
					PostLog("<color=#F5C542><b>[Patron Sponsor Bonus!]</b></color> <b>" + playerDeityName + "</b> backs your decision with a +25% personal favor bonus!", playerDeityName, ConstellationCategory.DeityPatron, 0, sceneContext);
				}
				else if (!string.IsNullOrEmpty(playerDeityName) && playerDeityName.Contains("Atheism"))
				{
					PostLog("<color=#F5C542><b>[The Free Agent]</b></color> The watching deities scoff at your persistent refusal to kneel, tossing coins merely to see how long your blasphemy lasts!", "The Free Agent", ConstellationCategory.DeityPatron, 0, sceneContext);
				}
				else if (!string.IsNullOrEmpty(playerDeityName) && !IsOneOf13(playerDeityName))
				{
					PostLog("<color=#F5C542><b>[Guest Sponsor: " + playerDeityName + "]</b></color> Your patron observes the broadcast from outside the 13 Seats with quiet pride!", playerDeityName, ConstellationCategory.DeityPatron, 0, sceneContext);
				}
				DivineTokens.AddCoins(amount, primarySponsor);
				goto IL_16d8;
				IL_16d8:
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
				IL_0320:
				_triggeredMilestones.Add("PrologueAwakening");
				list = ConstellationDialogueBanter.GetPrologueAwakeningBanter(out coinsAwarded, out primarySponsor);
				category = ConstellationCategory.Prologue;
				sceneContext = "Kenabres Square: The Awakening";
				goto IL_1298;
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
					PostLog(item, "The Constellations", ConstellationCategory.MetaLoop, 0, "Death & Loop Reload");
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
				ResetMilestones();
				List<string> metaLoopBanter = ConstellationDialogueBanter.GetMetaLoopBanter(runCount);
				if (metaLoopBanter == null)
				{
					return;
				}
				foreach (string item in metaLoopBanter)
				{
					PostLog(item, "The Constellations", ConstellationCategory.MetaLoop, 0, "New Cycle Progression");
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
					PostLog(item, "The Constellations", ConstellationCategory.Quest, 0, $"Act {act} Progression");
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ConstellationChatManager.OnActProgression: " + ex);
			}
		}

		private static bool IsCompanionSpeaker(string speakerName, string dialogName, out string compKey)
		{
			compKey = null;
			string text = ((speakerName ?? string.Empty) + " " + (dialogName ?? string.Empty)).ToLower();
			if (text.Contains("seelah"))
			{
				compKey = "seelah";
			}
			else if (text.Contains("camellia") || text.Contains("camelia"))
			{
				compKey = "camellia";
			}
			else if (text.Contains("lann"))
			{
				compKey = "lann";
			}
			else if (text.Contains("wenduag") || text.Contains("wendu"))
			{
				compKey = "wenduag";
			}
			else if (text.Contains("woljif"))
			{
				compKey = "woljif";
			}
			else if (text.Contains("daeran"))
			{
				compKey = "daeran";
			}
			else if (text.Contains("ember"))
			{
				compKey = "ember";
			}
			else if (text.Contains("nenio"))
			{
				compKey = "nenio";
			}
			else if (text.Contains("sosiel"))
			{
				compKey = "sosiel";
			}
			else if (text.Contains("regill"))
			{
				compKey = "regill";
			}
			else if (text.Contains("greybor"))
			{
				compKey = "greybor";
			}
			else if (text.Contains("arueshalae") || text.Contains("arue"))
			{
				compKey = "arueshalae";
			}
			else if (text.Contains("finnean"))
			{
				compKey = "finnean";
			}
			else if (text.Contains("trever"))
			{
				compKey = "trever";
			}
			else if (text.Contains("ulbrig"))
			{
				compKey = "ulbrig";
			}
			else if (text.Contains("rekarth"))
			{
				compKey = "rekarth";
			}
			else if (text.Contains("sendri"))
			{
				compKey = "sendri";
			}
			else if (text.Contains("penta"))
			{
				compKey = "penta";
			}
			return compKey != null;
		}

		private static bool IsMajorNpcSpeaker(string speakerName, string dialogName, out string npcKey)
		{
			npcKey = null;
			string text = ((speakerName ?? string.Empty) + " " + (dialogName ?? string.Empty)).ToLower();
			if (text.Contains("galfrey"))
			{
				npcKey = "galfrey";
			}
			else if (text.Contains("irabeth"))
			{
				npcKey = "irabeth";
			}
			else if (text.Contains("anevia"))
			{
				npcKey = "anevia";
			}
			else if (text.Contains("storyteller"))
			{
				npcKey = "storyteller";
			}
			else if (text.Contains("horgus") || text.Contains("gwerm"))
			{
				npcKey = "horgus";
			}
			else if (text.Contains("minagho"))
			{
				npcKey = "minagho";
			}
			else if (text.Contains("staunton"))
			{
				npcKey = "staunton";
			}
			else if (text.Contains("herald") || text.Contains("handofinheritor") || text.Contains("hand of the inheritor"))
			{
				npcKey = "herald";
			}
			else if (text.Contains("areelu"))
			{
				npcKey = "areelu";
			}
			else if (text.Contains("nocticula"))
			{
				npcKey = "nocticula";
			}
			else if (text.Contains("shamira"))
			{
				npcKey = "shamira";
			}
			else if (text.Contains("jerribeth"))
			{
				npcKey = "jerribeth";
			}
			else if (text.Contains("baphomet"))
			{
				npcKey = "baphomet";
			}
			else if (text.Contains("deskari"))
			{
				npcKey = "deskari";
			}
			else if (text.Contains("hulrun"))
			{
				npcKey = "hulrun";
			}
			else if (text.Contains("zacharius"))
			{
				npcKey = "zacharius";
			}
			else if (text.Contains("xanthir"))
			{
				npcKey = "xanthir";
			}
			else if (text.Contains("valmallos"))
			{
				npcKey = "valmallos";
			}
			else if (text.Contains("sithhud"))
			{
				npcKey = "sithhud";
			}
			else if (text.Contains("razmir"))
			{
				npcKey = "razmir";
			}
			else if (text.Contains("mutasafen"))
			{
				npcKey = "mutasafen";
			}
			else if (text.Contains("nurah"))
			{
				npcKey = "nurah";
			}
			else if (text.Contains("terendelev"))
			{
				npcKey = "terendelev";
			}
			else if (text.Contains("suture"))
			{
				npcKey = "suture";
			}
			else if (text.Contains("liotr"))
			{
				npcKey = "liotr";
			}
			else if (text.Contains("vellexia"))
			{
				npcKey = "vellexia";
			}
			else if (text.Contains("khorramzadeh"))
			{
				npcKey = "khorramzadeh";
			}
			else if (text.Contains("alderpash"))
			{
				npcKey = "alderpash";
			}
			return npcKey != null;
		}

		private static bool IsDlcContext(string areaName, string dialogName, out string dlcKey)
		{
			dlcKey = null;
			string text = ((areaName ?? string.Empty) + " " + (dialogName ?? string.Empty)).ToLower();
			if (text.Contains("dlc1") || text.Contains("inevitable") || text.Contains("valmallos"))
			{
				dlcKey = "dlc1_inevitable";
			}
			else if (text.Contains("dlc2") || text.Contains("ashes") || text.Contains("survivors"))
			{
				dlcKey = "dlc2_ashes";
			}
			else if (text.Contains("dlc3") || text.Contains("isles") || text.Contains("steersman") || text.Contains("ship"))
			{
				dlcKey = "dlc3_isles";
			}
			else if (text.Contains("dlc4") || text.Contains("sarkorian") || text.Contains("ulbrig") || text.Contains("gundrun"))
			{
				dlcKey = "dlc4_sarkorians";
			}
			else if (text.Contains("dlc5") || text.Contains("nothing") || text.Contains("sithhud"))
			{
				dlcKey = "dlc5_nothing";
			}
			else if (text.Contains("dlc6") || text.Contains("masks") || text.Contains("festival") || text.Contains("arena"))
			{
				dlcKey = "dlc6_masks";
			}
			return dlcKey != null;
		}
	}
}
