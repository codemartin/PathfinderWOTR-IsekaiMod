using System;
using System.Collections.Generic;
using IsekaiMod.Content.Quests;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Root;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Constellations;

public class ConstellationChatManager : ISelectAnswerHandler, ISubscriber, IGlobalSubscriber, IDialogStartHandler
{
	public enum LogRoute
	{
		Voice,
		Notice,
		Bookkeeping
	}

	private static bool _initialized = false;

	internal const string BanterRewardsStorage = "IsekaiMod.BanterRewards";

	internal const string MilestoneKeyPrefix = "milestone:";

	internal const string AnswerKeyPrefix = "answer:";

	private static readonly HashSet<string> AwakeningAnswerGuids = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "eea5643f25d86dc43bb3024bbb83212f", "47cbc8ab5ca57a14684db2344d8057f4", "bad44b6d69be7b64e8db6a07dcdcd9fc", "db8320c0269ad9c4bb4408688adb8067" };

	private static readonly HashSet<string> HulrunAnswerGuids = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "99b61d2799fab984d91a31523b791ac3", "d766a405f2efd4b4c9f3ffcd52537e91", "69ae3450833062d468074138f5b09812", "1ff8ff46ced090243af81f89a01422ee", "cd57229f881961846a5a20b7112a924b", "6898ccdc3dd26944c8e0ebb39d8c7777", "820751da02c91d54481d8732082b4806" };

	private static readonly HashSet<string> HulrunAnswerNames = new HashSet<string>(StringComparer.Ordinal) { "IsekaiDialogueHulrun", "IsekaiHulrunPhantomBlade", "IsekaiHulrunGodEmperor", "IsekaiHulrunOverlord", "IsekaiHulrunDevourer", "IsekaiHulrunShadowMonarch", "IsekaiHulrunHero", "IsekaiHulrunMastermind" };

	private static readonly HashSet<string> TerendelevAnswerGuids = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "cb5364b7c47d7a94f9dc05a4b1836051", "1d02bdcc7df6a3943babf9c11479cfaf", "a2e64274ac23831428eeea86b5e56f95", "99fede389ef058843947cebdce3c3408", "73858cd1fea29a74ba23407726ffda6e" };

	private static readonly HashSet<string> TerendelevAnswerNames = new HashSet<string>(StringComparer.Ordinal) { "IsekaiWelcomeGeneral", "IsekaiWelcomeDevourer", "IsekaiWelcomeShadowMonarch", "IsekaiWelcomeMastermind", "IsekaiWelcomePhantomBlade", "IsekaiWelcomeHero", "IsekaiWelcomeGodEmperor", "IsekaiWelcomeOverlord" };

	private static readonly HashSet<string> CavesSeelahAnswerGuids = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "7ce19f7c9cce54d458833a5d98f77e62", "9cd8308b0fc6506459bf1e98456b94bf", "b082a1f17a3219e4a8f842f87bed0be3", "99d18ab52a1e3a84f9e4b787eb02a743", "ac8a98a286c2d664fbf93297bc1d557a" };

	private static readonly HashSet<string> CavesSeelahAnswerNames = new HashSet<string>(StringComparer.Ordinal) { "IsekaiSeelahGeneral", "IsekaiSeelahMastermind" };

	private static readonly HashSet<string> CamelliaAnswerGuids = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "35d480d928e845a40a0661de241ceb79", "cbde3303de9b26f45918cbf33d60e1e3", "712417ce7cc82544b8902dc52e097651", "ff96f56ed6e58e143a269269c115e9bb", "691061ff298ee00459d486d5784d3d7f" };

	private static readonly HashSet<string> CamelliaAnswerNames = new HashSet<string>(StringComparer.Ordinal) { "IsekaiCameliaGeneral", "IsekaiCameliaDevourer", "IsekaiCameliaShadowMonarch", "IsekaiCameliaMastermind", "IsekaiCameliaHero" };

	private static readonly HashSet<string> LannAnswerGuids = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
	{
		"e3d54aed9334b844ca2cfffcd2d6941a", "460bec2b366453f48b335e4bea7c5454", "016443f096fae154fbb42a1ea1decde4", "500630ef3d0dc1b408dba690b1462520", "16f0ddce85cc4dc4caa57c938829e853", "50b0fe3080ead9f4c85a12eb3c968f55", "0870b0f37b48cac43b69f334c5406dab", "5532bd4bc13d0074db8efabb50b2ae70", "2acf1d4b6015dbe48a32ffe6e7050d7a", "d649624cb27339f4b8a839dea4f47143",
		"331f4144a144b6540bc7d1b5e21f0c07", "6f2dc8aeaed5bf642b5b34303f922292"
	};

	private static readonly HashSet<string> LannAnswerNames = new HashSet<string>(StringComparer.Ordinal) { "IsekaiLannGeneral", "IsekaiLannDevourer" };

	private const string DeskariDialogName = "StitchGivesCrossbow_Dialogue";

	internal const string MilestoneAwakening = "PrologueAwakening";

	internal const string MilestoneHulrun = "PrologueHulrun";

	internal const string MilestoneTerendelev = "PrologueTerendelev";

	internal const string MilestoneDeskari = "PrologueDeskari";

	internal const string MilestoneCavesSeelah = "PrologueCavesSeelah";

	internal const string MilestoneCamellia = "PrologueCamellia";

	internal const string MilestoneLann = "PrologueLann";

	private static readonly Dictionary<string, string> TagDeitySeats = new Dictionary<string, string>(StringComparer.Ordinal)
	{
		{ "Desna", "The Song of the Spheres" },
		{ "Gorum", "Our Lord in Iron" },
		{ "Iomedae", "The Inheritor" },
		{ "Calistria", "The Savored Sting" },
		{ "Nethys", "The All-Seeing Eye" },
		{ "Asmodeus", "The Prince of Darkness" },
		{ "Pharasma", "The Lady of Graves" },
		{ "Cayden Cailean", "The Lucky Drunk" },
		{ "The Lantern King", "The Laughing King" },
		{ "Yog-Sothoth", "The Key and the Gate" },
		{ "Urgathoa", "The Pallid Princess" },
		{ "Besmara", "The Pirate Queen" },
		{ "Chaldira", "The Mischievous Friend" },
		{ "Milani", "The Everbloom" },
		{ "Black Butterfly", "The Silence Between the Stars" }
	};

	private static bool _warnedCheatingThisCombat = false;

	internal const string AtheismFeatureGuid = "92c0d2da0a836ce418a267093c09ca54";

	private static readonly Dictionary<string, string[]> SponsorVanillaDeities = new Dictionary<string, string[]>(StringComparer.Ordinal)
	{
		{
			"The Lucky Drunk",
			new string[1] { "300e212868bca984687c92bcb66d381b" }
		},
		{
			"The Inheritor",
			new string[1] { "88d5da04361b16746bf5b65795e0c38c" }
		},
		{
			"The Prince of Darkness",
			new string[1] { "a3a5ccc9c670e6f4ca4a686d23b89900" }
		},
		{
			"The Song of the Spheres",
			new string[1] { "2c0a3b9971327ba4d9d85354d16998c1" }
		},
		{
			"The Lady of Graves",
			new string[1] { "458750bc214ab2e44abdeae404ab22e9" }
		},
		{
			"The Savored Sting",
			new string[1] { "c7531715a3f046d4da129619be63f44c" }
		},
		{
			"The All-Seeing Eye",
			new string[1] { "6262cfce7c31626458325ca0909de997" }
		},
		{
			"Our Lord in Iron",
			new string[1] { "8f49a5d8528a82c44b8c117a89f6b68c" }
		},
		{
			"The Pirate Queen",
			new string[1] { "470d824767d34e2e9a59b6d70681af7a" }
		},
		{
			"The Pallid Princess",
			new string[1] { "812f6c07148088e41a9ac94b56ac2fc8" }
		}
	};

	private static readonly Dictionary<string, string> SponsorModDeities = new Dictionary<string, string>(StringComparer.Ordinal)
	{
		{ "The Pirate Queen", "BesmaraFeature" },
		{ "The Laughing King", "LanternKingFeature" },
		{ "The Mischievous Friend", "ChaldiraFeature" },
		{ "The Everbloom", "MilaniFeature" },
		{ "The Silence Between the Stars", "BlackButterflyFeature" },
		{ "The Key and the Gate", "YogSothothFeature" }
	};

	internal const string SceneKeyPrefix = "scene:";

	private static readonly HashSet<string> ModOwnedDialogs = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "CosmicStoreDialog", "PlanarColiseumHeraldDialog", "AstralCoinEnvoyDialog", "VoidMarketSmugglerDialog", "ConstellationOracleDialog", "PlanarGatewayEmissaryDialog", "SylvanBloomBookEvent", "LanternKingCameoDialog" };

	internal static readonly HashSet<string> SystemAnswerGuids = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "17cf846dddf09244a93b53eaf26c8539", "942b64c165b97cd47b66848c20047433", "8bd0ea4d576725548b936050567ae609", "0aa57d78541372e46bae2402e5f540a6" };

	private static string _lastDialogName;

	private static bool _neutralBroadcastInThisDialog;

	public static void Init()
	{
		if (!_initialized && ConstellationSettings.Enabled)
		{
			_initialized = true;
			EventBus.Subscribe(new ConstellationChatManager());
			ConstellationChatOverlay.EnsureInstance();
		}
	}

	public static void ResetPerPlaythroughState()
	{
		_warnedCheatingThisCombat = false;
		_neutralBroadcastInThisDialog = false;
		_lastDialogName = null;
	}

	private static Dictionary<string, string> RewardLedger(bool create)
	{
		EntityDataBase entity = SaveScopedStorage.CurrentPlayer();
		if (!create)
		{
			return SaveScopedStorage.Find(entity, "IsekaiMod.BanterRewards");
		}
		return SaveScopedStorage.Ensure(entity, "IsekaiMod.BanterRewards");
	}

	internal static HashSet<string> MilestonesIn(IDictionary<string, string> ledger)
	{
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		if (ledger == null)
		{
			return hashSet;
		}
		foreach (string key in ledger.Keys)
		{
			if (key != null && key.StartsWith("milestone:", StringComparison.Ordinal))
			{
				hashSet.Add(key.Substring("milestone:".Length));
			}
		}
		return hashSet;
	}

	internal static bool AnswerRewardedIn(IDictionary<string, string> ledger, string answerGuid)
	{
		if (ledger != null && !string.IsNullOrEmpty(answerGuid))
		{
			return ledger.ContainsKey("answer:" + answerGuid);
		}
		return false;
	}

	internal static string MatchPrologueMilestone(string dialogName, string answerGuid, string answerName, ICollection<string> alreadyTriggered)
	{
		dialogName = dialogName ?? string.Empty;
		answerGuid = answerGuid ?? string.Empty;
		answerName = answerName ?? string.Empty;
		if (!Seen("PrologueAwakening") && In(AwakeningAnswerGuids, null))
		{
			return "PrologueAwakening";
		}
		if (!Seen("PrologueHulrun") && In(HulrunAnswerGuids, HulrunAnswerNames))
		{
			return "PrologueHulrun";
		}
		if (!Seen("PrologueTerendelev") && In(TerendelevAnswerGuids, TerendelevAnswerNames))
		{
			return "PrologueTerendelev";
		}
		if (!Seen("PrologueDeskari") && string.Equals(dialogName, "StitchGivesCrossbow_Dialogue", StringComparison.Ordinal))
		{
			return "PrologueDeskari";
		}
		if (!Seen("PrologueCavesSeelah") && In(CavesSeelahAnswerGuids, CavesSeelahAnswerNames))
		{
			return "PrologueCavesSeelah";
		}
		if (!Seen("PrologueCamellia") && In(CamelliaAnswerGuids, CamelliaAnswerNames))
		{
			return "PrologueCamellia";
		}
		if (!Seen("PrologueLann") && In(LannAnswerGuids, LannAnswerNames))
		{
			return "PrologueLann";
		}
		return null;
		bool In(HashSet<string> guids, HashSet<string> names)
		{
			if (!guids.Contains(answerGuid))
			{
				return names?.Contains(answerName) ?? false;
			}
			return true;
		}
		bool Seen(string key)
		{
			if (alreadyTriggered != null)
			{
				return alreadyTriggered.Contains(key);
			}
			return false;
		}
	}

	internal static bool ReachesPanel(LogRoute route, bool constellationsEnabled)
	{
		return (route != LogRoute.Bookkeeping) & constellationsEnabled;
	}

	internal static bool ReachesCombatLog(LogRoute route, bool constellationsEnabled)
	{
		return (route != LogRoute.Voice) | constellationsEnabled;
	}

	public static void PostCombatLog(string message)
	{
		if (!string.IsNullOrEmpty(message))
		{
			string text = ConstellationText.LocalizeIfKnown(message);
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(text);
			});
		}
	}

	public static void PostRouted(LogRoute route, string message, string sponsor, ConstellationCategory category, string sceneContext = null, float duration = -1f)
	{
		if (!string.IsNullOrEmpty(message))
		{
			bool enabled = ConstellationSettings.Enabled;
			if (ReachesPanel(route, enabled))
			{
				PostLog(message, sponsor ?? SponsorFromTag(message), category, 0, sceneContext, duration);
			}
			else if (ReachesCombatLog(route, enabled))
			{
				PostCombatLog(message);
			}
		}
	}

	internal static string SponsorFromTag(string message)
	{
		if (string.IsNullOrEmpty(message))
		{
			return null;
		}
		int num = message.IndexOf('[');
		int num2 = ((num < 0) ? (-1) : message.IndexOf(']', num + 1));
		if (num2 < 0)
		{
			return null;
		}
		string text = message.Substring(num + 1, num2 - num - 1).Trim();
		if (text.StartsWith("Constellation - ", StringComparison.Ordinal))
		{
			text = text.Substring("Constellation - ".Length);
		}
		int num3 = text.IndexOf(" (", StringComparison.Ordinal);
		if (num3 > 0)
		{
			text = text.Substring(0, num3);
		}
		if (!TagDeitySeats.TryGetValue(text, out var value))
		{
			return null;
		}
		return value;
	}

	public static void PostLog(string message, string sponsor = null, ConstellationCategory category = ConstellationCategory.Subclass, int coins = 0, string sceneContext = null, float duration = -1f)
	{
		if (ConstellationSettings.Enabled)
		{
			string text = ConstellationText.LocalizeIfKnown(message);
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(text);
			});
			ConstellationEventHistory.RecordEvent(text, sponsor, category, coins, sceneContext);
			ConstellationChatOverlay.AddMessage(text, sponsor, coins, duration);
		}
	}

	public static void PostLines(IEnumerable<string> lines, string sponsor, ConstellationCategory category, string sceneContext)
	{
		if (lines == null)
		{
			return;
		}
		foreach (string line in lines)
		{
			PostLog(line, sponsor, category, 0, sceneContext);
		}
	}

	public static void ResetCombatWarning()
	{
		_warnedCheatingThisCombat = false;
	}

	public static void WarnRampantCheating(int count)
	{
		if (!_warnedCheatingThisCombat && ConstellationSettings.Enabled)
		{
			_warnedCheatingThisCombat = true;
			string[] array = new string[4]
			{
				ConstellationText.LocalizedFormat("Constellation.Chat", "<color=#7A1010><b>[Cosmic Outrage: Rampant Cheating Detected!]</b></color> <color=#5B2A86>The Lantern King:</color> \"Ohoho! One otherworld anomaly was an amusing comedy. <b>{0} of them</b> is shameless, brazen hacking! Let's see how you fare when the multiverse fights back!\"", count),
				ConstellationText.LocalizedFormat("Constellation.Chat", "<color=#7A1010><b>[Cosmic Outrage: Soul Registry Error!]</b></color> <color=#5B2A86>Pharasma:</color> \"The river of souls rejects this mockery. The cosmic registry is utterly corrupted with {0} illegal transmigrants. Cosmic balance demands immediate retribution!\"", count),
				"<color=#7A1010><b>[Cosmic Outrage: Anomaly Overload!]</b></color> <color=#7A1010>Gorum:</color> \"A whole platoon of cheat gods?! Bah! True warriors fight with grit, not cheat codes! Come, beasts, tear them to pieces!\"",
				ConstellationText.LocalizedFormat("Constellation.Chat", "<color=#7A1010><b>[Cosmic Outrage: Dimensional Collapse!]</b></color> <color=#133E71>Yog-Sothoth:</color> \"Dimensional boundary integrity collapsing. Excessive cheat density detected ({0} anomalies). Cosmic equilibrium enforcement protocols engaged!\"", count)
			};
			Random random = new Random();
			PostLog(array[random.Next(array.Length)], "Cosmic Threat", ConstellationCategory.Combat, 0, "Combat Retribution");
			PostLog("<color=#7A1010><i>[Multiverse Retribution Active: All enemies have received heavily scaled combat stats, extra attacks, and hit points!]</i></color>", "Cosmic Threat", ConstellationCategory.Combat, 0, "Combat Retribution");
		}
	}

	internal static IEnumerable<string> SponsorNames()
	{
		HashSet<string> hashSet = new HashSet<string>(SponsorVanillaDeities.Keys, StringComparer.Ordinal);
		hashSet.UnionWith(SponsorModDeities.Keys);
		return hashSet;
	}

	private static string NormalizeGuid(string guid)
	{
		if (!string.IsNullOrEmpty(guid))
		{
			return guid.Replace("-", string.Empty).ToLowerInvariant();
		}
		return string.Empty;
	}

	internal static string SponsorForDeity(string deityGuid, Func<string, string> modDeityGuid)
	{
		string text = NormalizeGuid(deityGuid);
		if (text.Length == 0)
		{
			return null;
		}
		foreach (KeyValuePair<string, string[]> sponsorVanillaDeity in SponsorVanillaDeities)
		{
			if (Array.IndexOf(sponsorVanillaDeity.Value, text) >= 0)
			{
				return sponsorVanillaDeity.Key;
			}
		}
		if (modDeityGuid == null)
		{
			return null;
		}
		foreach (KeyValuePair<string, string> sponsorModDeity in SponsorModDeities)
		{
			if (NormalizeGuid(modDeityGuid(sponsorModDeity.Value)) == text)
			{
				return sponsorModDeity.Key;
			}
		}
		return null;
	}

	private static string ModDeityGuid(string blueprintName)
	{
		try
		{
			return Main.IsekaiContext.Blueprints.GetGUID(blueprintName).ToString();
		}
		catch
		{
			return null;
		}
	}

	internal static bool IsChosenDeityFeature(BlueprintFeature feat)
	{
		if (feat == null || feat is BlueprintFeatureSelection)
		{
			return false;
		}
		if (feat.Groups != null)
		{
			return Array.IndexOf(feat.Groups, FeatureGroup.Deities) >= 0;
		}
		return false;
	}

	internal static BlueprintFeature GetPlayerDeityFeature()
	{
		UnitEntityData player = DivineTokens.GetPlayer();
		if (player == null)
		{
			return null;
		}
		foreach (Feature feature in player.Descriptor.Progression.Features)
		{
			if (IsChosenDeityFeature(feature.Blueprint))
			{
				return feature.Blueprint;
			}
		}
		return null;
	}

	public static string GetPlayerDeityName()
	{
		return GetPlayerDeityFeature()?.Name;
	}

	public static string SponsorForPlayerDeity()
	{
		BlueprintFeature playerDeityFeature = GetPlayerDeityFeature();
		if (playerDeityFeature != null)
		{
			return SponsorForDeity(playerDeityFeature.AssetGuid.ToString(), ModDeityGuid);
		}
		return null;
	}

	internal static bool RewardLedgerAvailable()
	{
		return RewardLedger(create: true) != null;
	}

	internal static bool SceneRewardedIn(IDictionary<string, string> ledger, string sceneKey)
	{
		if (ledger != null && !string.IsNullOrEmpty(sceneKey))
		{
			return ledger.ContainsKey("scene:" + sceneKey);
		}
		return false;
	}

	internal static bool SceneRewardClaimed(string sceneKey)
	{
		return SceneRewardedIn(RewardLedger(create: false), sceneKey);
	}

	internal static bool TryClaimSceneReward(string sceneKey)
	{
		if (string.IsNullOrEmpty(sceneKey))
		{
			return true;
		}
		return ClaimSceneRewardIn(RewardLedger(create: true), sceneKey);
	}

	internal static bool ClaimSceneRewardIn(IDictionary<string, string> ledger, string sceneKey)
	{
		if (string.IsNullOrEmpty(sceneKey))
		{
			return true;
		}
		if (ledger == null)
		{
			return true;
		}
		if (SceneRewardedIn(ledger, sceneKey))
		{
			return false;
		}
		ledger["scene:" + sceneKey] = "1";
		return true;
	}

	private static IDictionary<string, string> DisfavorLedger(bool create)
	{
		EntityDataBase entity = SaveScopedStorage.CurrentPlayer();
		if (!create)
		{
			return SaveScopedStorage.Find(entity, "IsekaiMod.Disfavor");
		}
		return SaveScopedStorage.Ensure(entity, "IsekaiMod.Disfavor");
	}

	internal static bool SeatIsOffended(string sponsor)
	{
		if (!ConstellationDisfavor.IsSeat(sponsor))
		{
			return false;
		}
		return ConstellationDisfavor.IsOffended(ConstellationDisfavor.Read(DisfavorLedger(create: false), sponsor));
	}

	internal static void TrackAlignmentStanding(AlignmentShiftDirection shift, string patronSponsor)
	{
		if (shift == AlignmentShiftDirection.TrueNeutral)
		{
			return;
		}
		IDictionary<string, string> dictionary = null;
		foreach (KeyValuePair<string, AlignmentMaskType> item in ConstellationDisfavor.SeatAlignment)
		{
			int num = ConstellationDisfavor.Reaction(item.Value, shift);
			if (num == 0)
			{
				continue;
			}
			dictionary = dictionary ?? DisfavorLedger(create: true);
			if (dictionary == null)
			{
				break;
			}
			int num2 = ConstellationDisfavor.Read(dictionary, item.Key);
			bool isPatron = string.Equals(item.Key, patronSponsor, StringComparison.Ordinal);
			int num3 = ConstellationDisfavor.Apply(num2, num, isPatron);
			if (num3 != num2)
			{
				ConstellationDisfavor.Write(dictionary, item.Key, num3);
				bool flag = ConstellationDisfavor.IsOffended(num2);
				bool flag2 = ConstellationDisfavor.IsOffended(num3);
				if (!flag & flag2)
				{
					PostLog(ConstellationText.LocalizedFormat("Constellation.Chat", "<color=#7A1010><b>[Divine Displeasure]</b></color> Your defiance has deeply offended <b>{0}</b>. Until you make amends, that seat pays no patron bonus and halves every sponsorship it sends you.", item.Key), item.Key, ConstellationCategory.DeityPatron, 0, "Deity Disfavor");
				}
				else if (flag && !flag2)
				{
					PostLog(ConstellationText.LocalizedFormat("Constellation.Chat", "<color=#004604><b>[Amends Made]</b></color> <b>{0}</b> is satisfied. Full sponsorship, and its patron bonus, are restored.", item.Key), item.Key, ConstellationCategory.DeityPatron, 0, "Deity Disfavor");
				}
			}
		}
	}

	internal static void TrackOuterCourtStanding(ConstellationDisfavor.OuterCourtDeed deed, string onlySeat = null)
	{
		IDictionary<string, string> dictionary = null;
		string b = SponsorForPlayerDeity();
		foreach (string outerCourtSeat in ConstellationDisfavor.OuterCourtSeats)
		{
			if (onlySeat != null && !string.Equals(outerCourtSeat, onlySeat, StringComparison.Ordinal))
			{
				continue;
			}
			int num = ConstellationDisfavor.OuterCourtReaction(outerCourtSeat, deed);
			if (num == 0)
			{
				continue;
			}
			dictionary = dictionary ?? DisfavorLedger(create: true);
			if (dictionary == null)
			{
				break;
			}
			int num2 = ConstellationDisfavor.Read(dictionary, outerCourtSeat);
			bool isPatron = string.Equals(outerCourtSeat, b, StringComparison.Ordinal);
			int num3 = ConstellationDisfavor.Apply(num2, num, isPatron);
			if (num3 != num2)
			{
				ConstellationDisfavor.Write(dictionary, outerCourtSeat, num3);
				bool flag = ConstellationDisfavor.IsOffended(num2);
				bool flag2 = ConstellationDisfavor.IsOffended(num3);
				if (!flag & flag2)
				{
					PostLog(ConstellationText.LocalizedFormat("Constellation.Chat", "<color=#7A1010><b>[The Outer Court Turns Away]</b></color> <b>{0}</b> did not want that. It cares nothing for your alignment, only for what it came to watch, and it has stopped being fed. Until you feed it again, that seat pays no patron bonus and halves every sponsorship it sends you.", outerCourtSeat), outerCourtSeat, ConstellationCategory.DeityPatron, 0, "Outer Court Standing");
				}
				else if (flag && !flag2)
				{
					PostLog(ConstellationText.LocalizedFormat("Constellation.Chat", "<color=#004604><b>[The Outer Court Leans In]</b></color> <b>{0}</b> is interested again. Full sponsorship, and its patron bonus, are restored.", outerCourtSeat), outerCourtSeat, ConstellationCategory.DeityPatron, 0, "Outer Court Standing");
				}
			}
		}
	}

	public static void RecordLoopDefiance()
	{
		try
		{
			TrackOuterCourtStanding(ConstellationDisfavor.OuterCourtDeed.LoopDefiance);
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error in ConstellationChatManager.RecordLoopDefiance: " + ex);
		}
	}

	internal static bool IsModOwnedDialog(string dialogName)
	{
		if (!string.IsNullOrEmpty(dialogName))
		{
			return ModOwnedDialogs.Contains(dialogName);
		}
		return false;
	}

	private static bool IsSystemAnswer(BlueprintAnswer answer, string answerGuid)
	{
		if (SystemAnswerGuids.Contains(answerGuid))
		{
			return true;
		}
		DialogRoot dialogRoot = Game.Instance?.BlueprintRoot?.Dialog;
		if (dialogRoot == null)
		{
			return false;
		}
		if (answer != dialogRoot.ContinueAnswer && answer != dialogRoot.ExitAnswer && answer != dialogRoot.InterchapterContinueAnswer)
		{
			return answer == dialogRoot.InterchapterExitAnswer;
		}
		return true;
	}

	internal static int DecideAnswerCoins(string dialogName, bool isMilestone, int banterCoins, bool answerAlreadyRewarded)
	{
		if (banterCoins <= 0)
		{
			return 0;
		}
		if (IsModOwnedDialog(dialogName))
		{
			return 0;
		}
		if (isMilestone)
		{
			return banterCoins;
		}
		if (!answerAlreadyRewarded)
		{
			return banterCoins;
		}
		return 0;
	}

	internal static int ComputeFinalCoins(int baseCoins, double multiplier, bool patronMatch)
	{
		return ComputeFinalCoins(baseCoins, multiplier, patronMatch, sponsorOffended: false);
	}

	internal static int ComputeFinalCoins(int baseCoins, double multiplier, bool patronMatch, bool sponsorOffended)
	{
		return ComputeFinalCoins(baseCoins, multiplier, patronMatch, sponsorOffended, innerCourtTithe: false);
	}

	internal static int ComputeFinalCoins(int baseCoins, double multiplier, bool patronMatch, bool sponsorOffended, bool innerCourtTithe)
	{
		if (baseCoins <= 0)
		{
			return 0;
		}
		if (multiplier <= 0.0)
		{
			multiplier = 1.0;
		}
		double num = ((patronMatch && !sponsorOffended) ? 1.25 : 1.0);
		int num2 = (int)Math.Round((double)baseCoins * multiplier * num);
		if (num2 < 0)
		{
			num2 = 0;
		}
		num2 = ConstellationDisfavor.AdjustPayout(num2, sponsorOffended);
		return ConstellationDisfavor.ApplyInnerCourtTithe(num2, innerCourtTithe);
	}

	internal static bool InnerCourtTitheApplies(string patronSponsor, string sponsor)
	{
		if (ConstellationDisfavor.IsOuterCourtSeat(patronSponsor))
		{
			return !ConstellationDisfavor.IsOuterCourtSeat(sponsor);
		}
		return false;
	}

	internal static AlignmentShiftDirection EffectiveShift(AlignmentShiftDirection direction, int value)
	{
		if (value <= 0)
		{
			return AlignmentShiftDirection.TrueNeutral;
		}
		return direction;
	}

	public void HandleDialogStarted(BlueprintDialog dialog)
	{
		_lastDialogName = dialog?.name;
		_neutralBroadcastInThisDialog = false;
	}

	public void HandleSelectAnswer(BlueprintAnswer answer)
	{
		if (answer == null || !ConstellationSettings.Enabled)
		{
			return;
		}
		try
		{
			if (!IsekaiTransmigrationQuest.PlayerCanStartTransmigration())
			{
				return;
			}
			string text = ((answer.AssetGuid != null) ? answer.AssetGuid.ToString() : string.Empty);
			if (IsSystemAnswer(answer, text))
			{
				return;
			}
			AlignmentShiftDirection alignmentShiftDirection = EffectiveShift(answer.AlignmentShift?.Direction ?? AlignmentShiftDirection.TrueNeutral, answer.AlignmentShift?.Value ?? 0);
			string answerText = answer.DisplayText ?? answer.name;
			string answerName = answer.name ?? string.Empty;
			string dialogName = Game.Instance.DialogController?.Dialog?.name ?? _lastDialogName ?? string.Empty;
			if (IsModOwnedDialog(dialogName))
			{
				return;
			}
			Dictionary<string, string> ledger = RewardLedger(create: false);
			bool answerAlreadyRewarded = AnswerRewardedIn(ledger, text);
			bool flag = false;
			bool flag2 = false;
			int coinsAwarded = 0;
			string primarySponsor = null;
			ConstellationCategory category = ConstellationCategory.Subclass;
			string sceneContext = "Dialogue Choice";
			List<string> list = null;
			string text2 = MatchPrologueMilestone(dialogName, text, answerName, MilestonesIn(ledger));
			if (text2 != null)
			{
				flag = true;
				category = ConstellationCategory.Prologue;
				switch (text2)
				{
				case "PrologueAwakening":
					list = ConstellationDialogueBanter.GetPrologueAwakeningBanter(out coinsAwarded, out primarySponsor);
					sceneContext = "Kenabres Square: The Awakening";
					break;
				case "PrologueHulrun":
					list = ConstellationDialogueBanter.GetPrologueHulrunBanter(out coinsAwarded, out primarySponsor);
					sceneContext = "Kenabres Square: Prelate Hulrun";
					break;
				case "PrologueTerendelev":
					list = ConstellationDialogueBanter.GetPrologueTerendelevBanter(out coinsAwarded, out primarySponsor);
					sceneContext = "Kenabres Square: Terendelev";
					break;
				case "PrologueDeskari":
					list = ConstellationDialogueBanter.GetPrologueDeskariInvasionBanter(out coinsAwarded, out primarySponsor);
					sceneContext = "Kenabres Square: Deskari Invasion";
					break;
				case "PrologueCavesSeelah":
					list = ConstellationDialogueBanter.GetPrologueCavesAwakeningBanter(out coinsAwarded, out primarySponsor);
					sceneContext = "Caves: Rescue Anevia";
					break;
				case "PrologueCamellia":
					list = ConstellationDialogueBanter.GetPrologueCamelliaEncounterBanter(out coinsAwarded, out primarySponsor);
					sceneContext = "Caves: Meeting Camellia";
					break;
				default:
					list = ConstellationDialogueBanter.GetPrologueLannWenduagBanter(out coinsAwarded, out primarySponsor);
					sceneContext = "Caves: Lann & Wenduag";
					break;
				}
				Dictionary<string, string> dictionary = RewardLedger(create: true);
				if (dictionary != null)
				{
					dictionary["milestone:" + text2] = "1";
				}
			}
			else
			{
				list = ConstellationDialogueBanter.GetAlignmentBanter(alignmentShiftDirection, answerText, out coinsAwarded, out primarySponsor);
				if (list != null && list.Count > 0)
				{
					category = ((alignmentShiftDirection == AlignmentShiftDirection.TrueNeutral) ? ConstellationCategory.Subclass : ConstellationCategory.AlignmentShift);
					sceneContext = ((alignmentShiftDirection == AlignmentShiftDirection.TrueNeutral) ? "Archetype Roleplay" : $"Alignment Choice ({alignmentShiftDirection})");
				}
				else if (!_neutralBroadcastInThisDialog)
				{
					list = ConstellationDialogueBanter.GetNeutralDialogueBanter(answerText, out coinsAwarded, out primarySponsor);
					category = ConstellationCategory.Subclass;
					sceneContext = "Dialogue Choice";
					flag2 = true;
				}
			}
			coinsAwarded = DecideAnswerCoins(dialogName, flag, coinsAwarded, answerAlreadyRewarded);
			if (list != null && list.Count > 0)
			{
				if (flag2)
				{
					_neutralBroadcastInThisDialog = true;
				}
				string shoutoutMsg = null;
				string text3 = null;
				string sponsor = null;
				ConstellationCategory category2 = ConstellationCategory.DeityPatron;
				int num = 0;
				if (coinsAwarded > 0)
				{
					double multiplier = ConstellationDialogueBanter.RollMultiplier(primarySponsor, out shoutoutMsg);
					BlueprintFeature playerDeityFeature = GetPlayerDeityFeature();
					string text4 = playerDeityFeature?.AssetGuid.ToString();
					string text5 = playerDeityFeature?.Name;
					string text6 = SponsorForDeity(text4, ModDeityGuid);
					bool flag3 = text6 != null && string.Equals(text6, primarySponsor, StringComparison.Ordinal);
					bool flag4 = SeatIsOffended(primarySponsor);
					bool flag5 = InnerCourtTitheApplies(text6, primarySponsor);
					num = ComputeFinalCoins(coinsAwarded, multiplier, flag3, flag4, flag5);
					if (flag4)
					{
						text3 = ConstellationText.LocalizedFormat("Constellation.Chat", "<color=#7A1010><b>[Divine Displeasure]</b></color> <b>{0}</b> is still offended, and sends half of what it might have.", primarySponsor);
						sponsor = primarySponsor;
					}
					else if (flag3)
					{
						text3 = ConstellationText.LocalizedFormat("Constellation.Chat", "<color=#5B2A86><b>[Patron Sponsor Bonus!]</b></color> <b>{0}</b> backs your decision with a +25% personal favor bonus!", text5);
						sponsor = text5;
					}
					else if (flag5)
					{
						text3 = ConstellationText.LocalizedFormat("Constellation.Chat", "<color=#7A1010><b>[The Outer Court Takes Its Cut]</b></color> <b>{0}</b> sends a quarter less than it meant to. The thirteen do not much like who you sit with.", primarySponsor);
						sponsor = primarySponsor;
					}
					else if (NormalizeGuid(text4) == "92c0d2da0a836ce418a267093c09ca54")
					{
						text3 = "<color=#5B2A86><b>[The Free Agent]</b></color> The watching deities scoff at your persistent refusal to kneel, tossing coins merely to see how long your blasphemy lasts!";
						sponsor = "The Free Agent";
					}
					else if (playerDeityFeature != null && text6 == null)
					{
						text3 = ConstellationText.LocalizedFormat("Constellation.Chat", "<color=#5B2A86><b>[Guest Sponsor: {0}]</b></color> Your patron observes the broadcast from outside the Seats with quiet pride!", text5);
						sponsor = text5;
					}
				}
				int num2 = DivineTokens.ApplyLoopMultiplier(num, TimelineManager.GetCoinLoopMultiplier());
				for (int i = 0; i < list.Count; i++)
				{
					PostLog(list[i], primarySponsor, category, (i == 0) ? num2 : 0, sceneContext);
				}
				if (!string.IsNullOrEmpty(shoutoutMsg))
				{
					PostLog(shoutoutMsg, primarySponsor, category, 0, sceneContext);
				}
				if (!string.IsNullOrEmpty(text3))
				{
					PostLog(text3, sponsor, category2, 0, sceneContext);
				}
				if (num > 0)
				{
					DivineTokens.AddCoins(num, primarySponsor);
					if (!flag && !string.IsNullOrEmpty(text))
					{
						Dictionary<string, string> dictionary2 = RewardLedger(create: true);
						if (dictionary2 != null)
						{
							dictionary2["answer:" + text] = "1";
						}
					}
				}
				if (ConstellationDisfavor.IsOuterCourtSeat(primarySponsor))
				{
					TrackOuterCourtStanding(ConstellationDisfavor.OuterCourtDeed.Appetite, primarySponsor);
				}
			}
			TrackAlignmentStanding(alignmentShiftDirection, SponsorForPlayerDeity());
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
			if (IsekaiTransmigrationQuest.PlayerCanStartTransmigration())
			{
				PostLines(ConstellationDialogueBanter.GetDeathReloadBanter(deathCount, lastArea), "The Constellations", ConstellationCategory.MetaLoop, "Death & Loop Reload");
				TrackOuterCourtStanding(ConstellationDisfavor.OuterCourtDeed.DeathUndone);
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
			PostLines(ConstellationDialogueBanter.GetMetaLoopBanter(runCount), "The Constellations", ConstellationCategory.MetaLoop, "New Cycle Progression");
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
			PostLines(ConstellationDialogueBanter.GetActProgressionBanter(act), "The Constellations", ConstellationCategory.Quest, $"Act {act} Progression");
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error in ConstellationChatManager.OnActProgression: " + ex);
		}
	}
}
