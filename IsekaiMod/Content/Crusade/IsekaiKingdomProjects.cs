using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Config;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.KingmakerIntegration;
using IsekaiMod.Content.Quests;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints.Quests;
using Kingmaker.ElementsSystem;
using Kingmaker.Kingdom;
using Kingmaker.Kingdom.Blueprints;
using Kingmaker.Kingdom.Tasks;
using Kingmaker.Settings;
using Kingmaker.UnitLogic.Alignments;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Crusade;

public static class IsekaiKingdomProjects
{
	public static BlueprintKingdomProject ProjectTreatyStolenLands;

	public static BlueprintKingdomProject ProjectTreatyAbsalom;

	public static BlueprintKingdomProject ProjectTreatyNex;

	public static BlueprintKingdomProject ProjectTreatyGeb;

	public static BlueprintKingdomProject ProjectTreatyOsirion;

	public static BlueprintKingdomProject ProjectTreatyBrevoy;

	public static BlueprintKingdomProject ProjectDecreeLanternKing;

	public static BlueprintKingdomProject ProjectDecreeYogSothoth;

	public static BlueprintKingdomProject ProjectDecreeGorum;

	public static BlueprintKingdomProject ProjectDecreeImperialDais;

	public static BlueprintKingdomProject ProjectCollectDividends;

	public static BlueprintKingdomProject ProjectReinvestCapital;

	private static readonly List<BlueprintKingdomProject> AllProjects = new List<BlueprintKingdomProject>();

	private static readonly Dictionary<BlueprintKingdomProject, DecreeTerms> TermsByProject = new Dictionary<BlueprintKingdomProject, DecreeTerms>();

	internal const int TreatyStartFinances = 1000;

	internal const int TreatyStartMaterials = 100;

	internal const int CommercialMaxRuns = 4;

	internal static readonly Dictionary<string, DecreeTerms> Terms = new Dictionary<string, DecreeTerms>(StringComparer.Ordinal)
	{
		["ProjectTreatyStolenLands"] = PaidOncePerSave(Treaty(2500, 125, 1500)),
		["ProjectTreatyAbsalom"] = Treaty(4000, 250, 1000),
		["ProjectTreatyNex"] = Treaty(3000, 300, 1000),
		["ProjectTreatyGeb"] = Treaty(2000, 150, 1000),
		["ProjectTreatyOsirion"] = Treaty(3000, 150, 1000),
		["ProjectTreatyBrevoy"] = Treaty(2500, 150, 1000),
		["ProjectDecreeLanternKing"] = new DecreeTerms
		{
			Finances = 2000,
			Materials = 100,
			CosmicCoins = 500,
			Days = 7,
			Gate = DecreeGate.Constellations
		},
		["ProjectDecreeYogSothoth"] = new DecreeTerms
		{
			Finances = 1000,
			Materials = 100,
			CosmicCoins = 1000,
			Days = 7,
			Gate = DecreeGate.Constellations
		},
		["ProjectDecreeGorum"] = new DecreeTerms
		{
			Finances = 2000,
			Materials = 200,
			CosmicCoins = 750,
			Days = 7,
			Gate = DecreeGate.Constellations
		},
		["ProjectDecreeImperialDais"] = new DecreeTerms
		{
			Finances = 3000,
			Materials = 300,
			CosmicCoins = 750,
			Days = 7,
			Gate = DecreeGate.Quest
		},
		["ProjectCollectDividends"] = new DecreeTerms
		{
			PersonalGold = 100000,
			CosmicCoins = 500,
			Days = 7,
			Cooldown = 7,
			MaxRuns = 4,
			Gate = DecreeGate.Isekai
		},
		["ProjectReinvestCapital"] = new DecreeTerms
		{
			Finances = 50000,
			Materials = 1000,
			CosmicCoins = 750,
			StartFinances = 25000,
			Days = 14,
			Cooldown = 14,
			MaxRuns = 4,
			Gate = DecreeGate.Isekai
		}
	};

	private static ActionCompleteKingdomProjectRewards s_StolenLandsTreatyReward;

	private static DecreeTerms Treaty(int finances, int materials, int coins)
	{
		return new DecreeTerms
		{
			Finances = finances,
			Materials = materials,
			CosmicCoins = coins,
			StartFinances = 1000,
			StartMaterials = 100,
			Days = 14,
			Gate = DecreeGate.Isekai
		};
	}

	private static DecreeTerms PaidOncePerSave(DecreeTerms terms)
	{
		terms.PaysOncePerSave = true;
		return terms;
	}

	public static void Add()
	{
		CreateRealmTreaties();
		CreateConstellationDecrees();
		CreateCommercialInvestmentDecrees();
	}

	private static void CreateRealmTreaties()
	{
		ProjectTreatyStolenLands = CreateProject("ProjectTreatyStolenLands", "Treaty of the Stolen Lands: Envoy of the Sovereign of the Stolen Lands", "Ratify a mutual defense pact and trade accord with the sovereign barony of the Stolen Lands ruled by the Sovereign of the Stolen Lands. Sylvan rangers and baronial treasury shipments bolster the Fifth Crusade.", "Costs 1,000 Finances and 100 Materials to begin. Grants +2,500 Crusade Finances, +125 Materials, and +1,500 Cosmic Coins upon ratification.", "<color=#004604><b>[Treaty Ratified]</b></color> The Stolen Lands under <b>the Sovereign of the Stolen Lands</b> have allied with the Fifth Crusade!", completesSylvanAlliance: true, completesImperialDais: false, "<color=#004604><b>[Treaty Ratified]</b></color> The Stolen Lands under <b>{ruler}</b> have allied with the Fifth Crusade!");
		ProjectTreatyAbsalom = CreateProject("ProjectTreatyAbsalom", "Treaty of Absalom: The Grand Starstone Accord", "Dispatch an emissary delegation to the Grand Council of Absalom, securing naval escorts, arcane grants, and banking credits from the City at the Center of the World.", "Costs 1,000 Finances and 100 Materials to begin. Grants +4,000 Crusade Finances, +250 Materials, and +1,000 Cosmic Coins upon ratification.", "<color=#5B2A86><b>[Treaty Ratified]</b></color> The Grand Council of Absalom pledges financial and logistical backing to the Fifth Crusade!");
		ProjectTreatyNex = CreateProject("ProjectTreatyNex", "Treaty of Nex: The Mana-Forge Alliance", "Establish arcane pacts with the archmagi of Nex to import refined quintessence, siege automatons, and alchemical gunpowder.", "Costs 1,000 Finances and 100 Materials to begin. Grants +3,000 Crusade Finances, +300 Materials, and +1,000 Cosmic Coins upon ratification.", "<color=#133E71><b>[Treaty Ratified]</b></color> The Mana-Forges of Nex supply advanced munitions to the Fifth Crusade!");
		ProjectTreatyGeb = CreateProject("ProjectTreatyGeb", "Treaty of Geb: The Sovereign Ossuary Accords", "Negotiate with the Blood Lords of Geb to harvest specialized embalming techniques, necrotic wardings, and osteomantic armaments.", "Costs 1,000 Finances and 100 Materials to begin. Grants +2,000 Crusade Finances, +150 Materials, and +1,000 Cosmic Coins upon ratification.", "<color=#5B2A86><b>[Treaty Ratified]</b></color> The Blood Lords of Geb ratify the Sovereign Ossuary Accords!");
		ProjectTreatyOsirion = CreateProject("ProjectTreatyOsirion", "Treaty of Osirion: The Pharaoh's Solar Compact", "Form an alliance with the Ruby Prince of Osirion, importing sun-forged gold, sacred funerary draughts, and desert skirmishers.", "Costs 1,000 Finances and 100 Materials to begin. Grants +3,000 Crusade Finances, +150 Materials, and +1,000 Cosmic Coins upon ratification.", "<color=#5B2A86><b>[Treaty Ratified]</b></color> The Pharaoh's Solar Compact illuminates the Fifth Crusade with Osirian gold!");
		ProjectTreatyBrevoy = CreateProject("ProjectTreatyBrevoy", "Treaty of Brevoy: The Aldori Dragonscale League", "Forge an accord with the swordlords of Restov and the dragon-crested barons of Brevoy to secure elite martial instructors and heavy war-plate.", "Costs 1,000 Finances and 100 Materials to begin. Grants +2,500 Crusade Finances, +150 Materials, and +1,000 Cosmic Coins upon ratification.", "<color=#133E71><b>[Treaty Ratified]</b></color> The Aldori Swordlords and Brevic nobility unite with the Fifth Crusade!");
	}

	private static void CreateConstellationDecrees()
	{
		ProjectDecreeLanternKing = CreateProject("ProjectDecreeLanternKing", "Constellation Decree: The Laughing Lantern", "Invoke the trickster blessings of the Lantern King along the Worldwound perimeter. His laughing devotees answer with tribute, supplies and his cosmic coin.", "Grants +2,000 Crusade Finances, +100 Materials and +500 Cosmic Coins.", "<color=#5B2A86><b>[Constellation Decree Complete]</b></color> The Lantern King's laughter echoes across the front lines!");
		ProjectDecreeYogSothoth = CreateProject("ProjectDecreeYogSothoth", "Constellation Decree: The Watcher at the Gate", "Attune Drezen's central citadel to the cosmic harmonies of Yog-Sothoth. The Watcher's cult pays its respects to the citadel in tribute, supplies and cosmic coin.", "Grants +1,000 Crusade Finances, +100 Materials and +1,000 Cosmic Coins.", "<color=#5B2A86><b>[Constellation Decree Complete]</b></color> Spacetime aligns in favor of the Fifth Crusade!");
		ProjectDecreeGorum = CreateProject("ProjectDecreeGorum", "Constellation Decree: The Iron Battle-Father", "Consecrate the crusader weapon vaults in the name of Gorum. The Battle-Father's faithful answer with iron, coin and his cosmic favour.", "Grants +2,000 Crusade Finances, +200 Materials and +750 Cosmic Coins.", "<color=#7A1010><b>[Constellation Decree Complete]</b></color> Gorum's battle-roar resounds through every crusader blade!");
		ProjectDecreeImperialDais = CreateProject("ProjectDecreeImperialDais", "Imperial Decree: Consecrate the Grand Imperial Dais", "Order the citadel's masons, your ordained disciples and every scribe who can hold a chisel to raise the Grand Imperial Dais in the heart of Drezen, and to consecrate it in your own name rather than any god's.", "Grants +3,000 Crusade Finances, +300 Materials and +750 Cosmic Coins, and enthrones you as the Fifth Crusade's divine sovereign.", "<color=#5B2A86><b>[Imperial Decree Complete]</b></color> The Grand Imperial Dais stands consecrated in Drezen, and the Fifth Crusade kneels to a living god!", completesSylvanAlliance: false, completesImperialDais: true);
	}

	private static void CreateCommercialInvestmentDecrees()
	{
		ProjectCollectDividends = CreateProject("ProjectCollectDividends", "Decree: Collect Commercial Dividends", "Order your otherworld megamalls, trading consortiums, and dimensional logistics banks to disburse accrued corporate dividends. Grants personal gold directly to the Commander and awards Cosmic Coins.", "Can be issued four times, 7 days apart (resolves in 7 days). Grants +100,000 Personal Gold and +500 Cosmic Coins each time.", "<color=#5B2A86><b>[Commercial Dividends Collected]</b></color> Corporate yields distributed: +100,000 Gold directly to your personal purse and +500 Cosmic Coins!");
		ProjectReinvestCapital = CreateProject("ProjectReinvestCapital", "Decree: Reinvest Capital - Market Expansion", "Allocate crusade funds to aggressively finance market expansions, supply depots, and interplanar merchant caravans. Reinvested capital yields substantial future financial dividends.", "Can be issued four times, 14 days apart (costs 25,000 Finances, resolves in 14 days). Grants +50,000 Crusade Finances, +1,000 Materials and +750 Cosmic Coins each time.", "<color=#004604><b>[Market Expansion Completed]</b></color> Reinvested capital yields massive returns: +50,000 Crusade Finances, +1,000 Materials, and +750 Cosmic Coins!");
	}

	internal static PossibleEventSolutions AdvisorSolutions(EventResult[] resolutions)
	{
		LeaderType[] obj = new LeaderType[4]
		{
			LeaderType.Counselor,
			LeaderType.Strategist,
			LeaderType.Diplomat,
			LeaderType.General
		};
		PossibleEventSolution[] array = new PossibleEventSolution[obj.Length];
		LeaderType[] array2 = obj;
		foreach (LeaderType leaderType in array2)
		{
			array[(int)leaderType] = new PossibleEventSolution
			{
				Leader = leaderType,
				CanBeSolved = true,
				Resolutions = resolutions
			};
		}
		return new PossibleEventSolutions
		{
			Entries = array
		};
	}

	private static BlueprintKingdomProject CreateProject(string name, string title, string description, string mechanicalDesc, string message, bool completesSylvanAlliance = false, bool completesImperialDais = false, string runtimeLogMessage = null)
	{
		DecreeTerms terms = Terms[name];
		BlueprintKingdomProject blueprintKingdomProject = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintKingdomProject bp)
		{
			bp.LocalizedName = Helpers.CreateString(Main.IsekaiContext, name + ".Title", title);
			bp.LocalizedDescription = Helpers.CreateString(Main.IsekaiContext, name + ".Description", description);
			bp.m_MechanicalDescription = Helpers.CreateString(Main.IsekaiContext, name + ".MechanicalDesc", mechanicalDesc);
			bp.ProjectType = KingdomProjectType.Command;
			bp.ResolutionTime = terms.Days;
			bp.Repeatable = terms.Repeatable;
			bp.Cooldown = terms.Cooldown;
			bp.ProjectStartCost = new KingdomResourcesAmount
			{
				m_Finances = terms.StartFinances,
				m_Materials = terms.StartMaterials
			};
			bp.TriggerCondition = new ConditionsChecker
			{
				Conditions = Array.Empty<Condition>()
			};
			bp.SkipRoll = true;
			ActionCompleteKingdomProjectRewards actionCompleteKingdomProjectRewards = new ActionCompleteKingdomProjectRewards
			{
				Finances = terms.Finances,
				Materials = terms.Materials,
				CosmicCoins = terms.CosmicCoins,
				PersonalGold = terms.PersonalGold,
				LogMessageText = (runtimeLogMessage ?? message),
				CompletesSylvanAlliance = completesSylvanAlliance,
				CompletesImperialDais = completesImperialDais,
				RunLedgerKey = ((terms.LedgerCap > 0) ? name : null),
				MaxRuns = terms.LedgerCap
			};
			if (completesSylvanAlliance)
			{
				s_StolenLandsTreatyReward = actionCompleteKingdomProjectRewards;
			}
			EventResult eventResult = new EventResult
			{
				Margin = EventResult.MarginType.Success,
				LeaderAlignment = AlignmentMaskType.Any,
				Condition = new ConditionsChecker
				{
					Conditions = Array.Empty<Condition>()
				},
				Actions = Helpers.CreateActionList(actionCompleteKingdomProjectRewards),
				LocalizedDescription = Helpers.CreateString(Main.IsekaiContext, name + ".SuccessText", message)
			};
			bp.Solutions = AdvisorSolutions(new EventResult[1] { eventResult });
		});
		AllProjects.Add(blueprintKingdomProject);
		TermsByProject[blueprintKingdomProject] = terms;
		return blueprintKingdomProject;
	}

	internal static bool ShouldOfferStolenLandsTreaty(bool kingmakerDetected, bool sylvanQuestStarted)
	{
		return kingmakerDetected | sylvanQuestStarted;
	}

	private static bool SylvanQuestStarted()
	{
		try
		{
			BlueprintQuest questSylvanAlliance = SylvanFeyAllianceQuest.QuestSylvanAlliance;
			QuestBook questBook = Game.Instance?.Player?.QuestBook;
			return questSylvanAlliance != null && questBook != null && questBook.GetQuestState(questSylvanAlliance) == QuestState.Started;
		}
		catch
		{
			return false;
		}
	}

	internal static bool ShouldOffer(DecreeGate gate, bool isekaiOffered, bool constellationsEnabled, bool autoCrusade)
	{
		if (autoCrusade)
		{
			return false;
		}
		if (gate == DecreeGate.Quest)
		{
			return true;
		}
		if (!isekaiOffered)
		{
			return false;
		}
		return (gate != DecreeGate.Constellations) | constellationsEnabled;
	}

	internal static bool RepeatDue(int runsSoFar, int maxRuns, int? lastSolvedOn, int currentDay, int cooldown)
	{
		if (!ActionCompleteKingdomProjectRewards.RunDue(runsSoFar, maxRuns))
		{
			return false;
		}
		if (cooldown > 0 && lastSolvedOn.HasValue && currentDay - lastSolvedOn.Value < cooldown)
		{
			return false;
		}
		return true;
	}

	internal static int? LastSolvedOn(IEnumerable<KingdomEventHistoryEntry> history, BlueprintKingdomEventBase project)
	{
		if (history == null || project == null)
		{
			return null;
		}
		int? result = null;
		foreach (KingdomEventHistoryEntry item in history)
		{
			if (item != null && item.Event == project)
			{
				result = item.SolvedOn;
			}
		}
		return result;
	}

	internal static bool WarTableCanRatify(bool offered, bool underWay, bool autoCrusade)
	{
		if (!autoCrusade)
		{
			return offered | underWay;
		}
		return false;
	}

	private static bool IsUnderWay(KingdomState ks, BlueprintKingdomProject project)
	{
		if (ks?.ActiveEvents != null && project != null)
		{
			return ks.ActiveEvents.Any((KingdomEvent e) => e?.EventBlueprint == project && (e.AssociatedTask?.IsStarted ?? false));
		}
		return false;
	}

	private static int LedgerRuns(IDictionary<string, string> ledger, string key)
	{
		if (!string.IsNullOrEmpty(key))
		{
			return Math.Max(0, SaveScopedStorage.GetInt(ledger, key));
		}
		return 0;
	}

	internal static bool CanRatifyStolenLandsTreaty()
	{
		try
		{
			BlueprintKingdomProject projectTreatyStolenLands = ProjectTreatyStolenLands;
			if (projectTreatyStolenLands == null || !TermsByProject.TryGetValue(projectTreatyStolenLands, out var value))
			{
				return false;
			}
			bool autoCrusade = AutoCrusade();
			KingdomState kingdomState = (KingdomState.Founded ? KingdomState.Instance : null);
			Dictionary<string, string> ledger = SaveScopedStorage.Find(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.KingdomDecrees");
			return WarTableCanRatify(kingdomState != null && ShouldOffer(value.Gate, ContentOffering.IsOffered(OfferedContent.IsekaiProtagonist), ConstellationSettings.Enabled, autoCrusade) && ShouldOfferStolenLandsTreaty(KingmakerCrossSaveManager.IsKingmakerInstalled, SylvanQuestStarted()) && ActionCompleteKingdomProjectRewards.RunDue(LedgerRuns(ledger, projectTreatyStolenLands.name), value.LedgerCap), IsUnderWay(kingdomState, projectTreatyStolenLands), autoCrusade);
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[IsekaiKingdomProjects] Could not tell whether the Treaty of the Stolen Lands can be ratified: {arg}");
			return true;
		}
	}

	internal static void PayStolenLandsTreatyWithoutWarTable()
	{
		ActionCompleteKingdomProjectRewards actionCompleteKingdomProjectRewards = s_StolenLandsTreatyReward;
		if (actionCompleteKingdomProjectRewards != null && ActionCompleteKingdomProjectRewards.RunDue(LedgerRuns(SaveScopedStorage.Find(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.KingdomDecrees"), actionCompleteKingdomProjectRewards.RunLedgerKey), actionCompleteKingdomProjectRewards.MaxRuns))
		{
			actionCompleteKingdomProjectRewards.RunAction();
		}
	}

	internal static bool AutoCrusade()
	{
		try
		{
			return SettingsRoot.Difficulty.AutoCrusade.GetValue();
		}
		catch
		{
			return false;
		}
	}

	private static void WithdrawUnstarted(KingdomState ks, BlueprintKingdomProject project)
	{
		if (ks?.ActiveEvents == null)
		{
			return;
		}
		foreach (KingdomEvent item in ks.ActiveEvents.Where((KingdomEvent e) => e?.EventBlueprint == project && !(e.AssociatedTask?.IsStarted ?? false)).ToList())
		{
			KingdomTaskEvent associatedTask = item.AssociatedTask;
			if (associatedTask != null)
			{
				if (associatedTask.AssignedLeader != null)
				{
					associatedTask.AssignedLeader.AssignedTask = null;
					associatedTask.AssignedLeader = null;
				}
				ks.ActiveTasks.Remove(associatedTask);
			}
			ks.ActiveEvents.Remove(item);
			Main.IsekaiContext.Logger.Log("[IsekaiKingdomProjects] Withdrew crusade project no longer offered: " + project.name);
		}
	}

	public static void CheckAndAddAvailableProjects()
	{
		try
		{
			Player player = Game.Instance?.Player;
			if (player == null || player.Chapter < 3 || !KingdomState.Founded)
			{
				return;
			}
			KingdomState instance = KingdomState.Instance;
			if (instance == null)
			{
				return;
			}
			KingdomTimelineManager kingdomTimelineManager = new KingdomTimelineManager();
			bool isekaiOffered = ContentOffering.IsOffered(OfferedContent.IsekaiProtagonist);
			bool enabled = ConstellationSettings.Enabled;
			bool autoCrusade = AutoCrusade();
			Dictionary<string, string> ledger = SaveScopedStorage.Find(SaveScopedStorage.CurrentPlayer(), "IsekaiMod.KingdomDecrees");
			foreach (BlueprintKingdomProject project in AllProjects)
			{
				if (project == null || !TermsByProject.TryGetValue(project, out var value))
				{
					continue;
				}
				if (!ShouldOffer(value.Gate, isekaiOffered, enabled, autoCrusade))
				{
					WithdrawUnstarted(instance, project);
				}
				else
				{
					if (project == ProjectTreatyStolenLands && !ShouldOfferStolenLandsTreaty(KingmakerCrossSaveManager.IsKingmakerInstalled, SylvanQuestStarted()))
					{
						continue;
					}
					if (project == ProjectDecreeImperialDais && !PersonalQuestDirector.IsImperialDaisDecreeOpen())
					{
						WithdrawUnstarted(instance, project);
					}
					else if (!value.Repeatable && !ActionCompleteKingdomProjectRewards.RunDue(LedgerRuns(ledger, project.name), value.LedgerCap))
					{
						WithdrawUnstarted(instance, project);
					}
					else
					{
						if (instance.ActiveEvents != null && instance.ActiveEvents.Any((KingdomEvent e) => e?.EventBlueprint == project))
						{
							continue;
						}
						if (!value.Repeatable)
						{
							if (instance.HasEventTriggered(project))
							{
								continue;
							}
						}
						else if (!RepeatDue(LedgerRuns(ledger, project.name), value.MaxRuns, LastSolvedOn(instance.EventHistory, project), instance.CurrentDay, value.Cooldown))
						{
							continue;
						}
						kingdomTimelineManager.StartEventInRegion(project, null);
						Main.IsekaiContext.Logger.Log("[IsekaiKingdomProjects] Added crusade project: " + project.name);
					}
				}
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[IsekaiKingdomProjects] Error in CheckAndAddAvailableProjects: {arg}");
		}
	}
}
