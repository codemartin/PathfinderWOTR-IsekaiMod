using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.KingmakerIntegration;
using Kingmaker;
using Kingmaker.ElementsSystem;
using Kingmaker.Kingdom;
using Kingmaker.Kingdom.Blueprints;
using Kingmaker.Kingdom.Tasks;
using Kingmaker.UnitLogic.Alignments;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Crusade
{
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

		public static BlueprintKingdomProject ProjectCollectDividends;

		public static BlueprintKingdomProject ProjectReinvestCapital;

		private static readonly List<BlueprintKingdomProject> AllProjects = new List<BlueprintKingdomProject>();

		public static void Add()
		{
			CreateRealmTreaties();
			CreateConstellationDecrees();
			CreateCommercialInvestmentDecrees();
		}

		private static void CreateRealmTreaties()
		{
			string rulerName = KingmakerCrossSaveManager.GetRulerName();
			ProjectTreatyStolenLands = CreateProject("ProjectTreatyStolenLands", "Treaty of the Stolen Lands: Envoy of " + rulerName, "Ratify a mutual defense pact and trade accord with the sovereign barony of the Stolen Lands ruled by " + rulerName + ". Sylvan rangers and baronial treasury shipments bolster the Fifth Crusade.", "Grants +5,000 Crusade Finances, +250 Materials, and +1,500 Cosmic Coins upon ratification.", 14, 5000, 250, 1500, "<color=#32CD32><b>[Treaty Ratified]</b></color> The Stolen Lands under <b>" + rulerName + "</b> have allied with the Fifth Crusade!");
			ProjectTreatyAbsalom = CreateProject("ProjectTreatyAbsalom", "Treaty of Absalom: The Grand Starstone Accord", "Dispatch an emissary delegation to the Grand Council of Absalom, securing naval escorts, arcane grants, and banking credits from the City at the Center of the World.", "Grants +8,000 Crusade Finances, +500 Materials, and +1,000 Cosmic Coins upon ratification.", 14, 8000, 500, 1000, "<color=#FFD700><b>[Treaty Ratified]</b></color> The Grand Council of Absalom pledges financial and logistical backing to the Fifth Crusade!");
			ProjectTreatyNex = CreateProject("ProjectTreatyNex", "Treaty of Nex: The Mana-Forge Alliance", "Establish arcane pacts with the archmagi of Nex to import refined quintessence, siege automatons, and alchemical gunpowder.", "Grants +6,000 Crusade Finances, +600 Materials, and +1,000 Cosmic Coins upon ratification.", 14, 6000, 600, 1000, "<color=#00BFFF><b>[Treaty Ratified]</b></color> The Mana-Forges of Nex supply advanced munitions to the Fifth Crusade!");
			ProjectTreatyGeb = CreateProject("ProjectTreatyGeb", "Treaty of Geb: The Sovereign Ossuary Accords", "Negotiate with the Blood Lords of Geb to harvest specialized embalming techniques, necrotic wardings, and osteomantic armaments.", "Grants +4,000 Crusade Finances, +300 Materials, and +1,000 Cosmic Coins upon ratification.", 14, 4000, 300, 1000, "<color=#8A2BE2><b>[Treaty Ratified]</b></color> The Blood Lords of Geb ratify the Sovereign Ossuary Accords!");
			ProjectTreatyOsirion = CreateProject("ProjectTreatyOsirion", "Treaty of Osirion: The Pharaoh's Solar Compact", "Form an alliance with the Ruby Prince of Osirion, importing sun-forged gold, sacred funerary draughts, and desert skirmishers.", "Grants +6,000 Crusade Finances, +300 Materials, and +1,000 Cosmic Coins upon ratification.", 14, 6000, 300, 1000, "<color=#FFD700><b>[Treaty Ratified]</b></color> The Pharaoh's Solar Compact illuminates the Fifth Crusade with Osirian gold!");
			ProjectTreatyBrevoy = CreateProject("ProjectTreatyBrevoy", "Treaty of Brevoy: The Aldori Dragonscale League", "Forge an accord with the swordlords of Restov and the dragon-crested barons of Brevoy to secure elite martial instructors and heavy war-plate.", "Grants +5,000 Crusade Finances, +300 Materials, and +1,000 Cosmic Coins upon ratification.", 14, 5000, 300, 1000, "<color=#4682B4><b>[Treaty Ratified]</b></color> The Aldori Swordlords and Brevic nobility unite with the Fifth Crusade!");
		}

		private static void CreateConstellationDecrees()
		{
			ProjectDecreeLanternKing = CreateProject("ProjectDecreeLanternKing", "Constellation Decree: The Laughing Lantern", "Invoke the trickster blessings of the Lantern King across the Worldwound perimeter, confusing demonic scouts with illusory pathways and mirages.", "Grants +2,000 Crusade Finances, +500 Cosmic Coins, and celestial mirages that disrupt demon march logistics.", 7, 2000, 100, 500, "<color=#FF8C00><b>[Constellation Decree Complete]</b></color> The Lantern King's laughter echoes across the front lines!");
			ProjectDecreeYogSothoth = CreateProject("ProjectDecreeYogSothoth", "Constellation Decree: The Watcher at the Gate", "Attune Drezen's central citadel to the cosmic harmonies of Yog-Sothoth, bending the flow of time to accelerate strategic planning.", "Grants +1,000 Cosmic Coins and cosmic temporal acceleration for future citadel affairs.", 7, 1000, 100, 1000, "<color=#9400D3><b>[Constellation Decree Complete]</b></color> Spacetime aligns in favor of the Fifth Crusade!");
			ProjectDecreeGorum = CreateProject("ProjectDecreeGorum", "Constellation Decree: The Iron Battle-Father", "Consecrate the crusader weapon vaults in the name of Gorum, imbuing standard weaponry with unstoppable martial fury.", "Grants +750 Cosmic Coins, +2,000 Finances, and heightened morale across all crusade armies.", 7, 2000, 200, 750, "<color=#DC143C><b>[Constellation Decree Complete]</b></color> Gorum's battle-roar resounds through every crusader blade!");
		}

		private static void CreateCommercialInvestmentDecrees()
		{
			ProjectCollectDividends = CreateProject("ProjectCollectDividends", "Decree: Collect Commercial Dividends", "Order your otherworld megamalls, trading consortiums, and dimensional logistics banks to disburse accrued corporate dividends. Grants personal gold directly to the Commander and awards Cosmic Coins.", "Repeatable decree (Resolves in 7 days). Grants +100,000 Personal Gold and +500 Cosmic Coins upon completion.", 7, 0, 0, 500, "<color=#FFD700><b>[Commercial Dividends Collected]</b></color> Corporate yields distributed: +100,000 Gold directly to your personal purse and +500 Cosmic Coins!", repeatable: true, 7, 0, 100000);
			ProjectReinvestCapital = CreateProject("ProjectReinvestCapital", "Decree: Reinvest Capital - Market Expansion", "Allocate crusade funds to aggressively finance market expansions, supply depots, and interplanar merchant caravans. Reinvested capital yields substantial future financial dividends.", "Repeatable decree (Costs 25,000 Finances, resolves in 14 days). Grants +50,000 Crusade Finances, +1,000 Materials, and +750 Cosmic Coins upon resolution.", 14, 50000, 1000, 750, "<color=#32CD32><b>[Market Expansion Completed]</b></color> Reinvested capital yields massive returns: +50,000 Crusade Finances, +1,000 Materials, and +750 Cosmic Coins!", repeatable: true, 14, 25000);
		}

		private static BlueprintKingdomProject CreateProject(string name, string title, string description, string mechanicalDesc, int days, int finances, int materials, int coins, string message, bool repeatable = false, int cooldown = 0, int startCostFinances = 0, int personalGold = 0)
		{
			BlueprintKingdomProject blueprintKingdomProject = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintKingdomProject bp)
			{
				bp.LocalizedName = Helpers.CreateString(Main.IsekaiContext, name + ".Title", title);
				bp.LocalizedDescription = Helpers.CreateString(Main.IsekaiContext, name + ".Description", description);
				bp.m_MechanicalDescription = Helpers.CreateString(Main.IsekaiContext, name + ".MechanicalDesc", mechanicalDesc);
				bp.ProjectType = KingdomProjectType.Command;
				bp.ResolutionTime = days;
				bp.Repeatable = repeatable;
				bp.Cooldown = cooldown;
				bp.ProjectStartCost = new KingdomResourcesAmount
				{
					m_Finances = startCostFinances
				};
				bp.TriggerCondition = new ConditionsChecker
				{
					Conditions = Array.Empty<Condition>()
				};
				bp.SkipRoll = true;
				ActionCompleteKingdomProjectRewards actionCompleteKingdomProjectRewards = new ActionCompleteKingdomProjectRewards
				{
					Finances = finances,
					Materials = materials,
					CosmicCoins = coins,
					PersonalGold = personalGold,
					LogMessageText = message
				};
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
				PossibleEventSolution possibleEventSolution = new PossibleEventSolution
				{
					Leader = LeaderType.None,
					CanBeSolved = true,
					Resolutions = new EventResult[1] { eventResult }
				};
				bp.Solutions = new PossibleEventSolutions
				{
					Entries = new PossibleEventSolution[1] { possibleEventSolution }
				};
			});
			AllProjects.Add(blueprintKingdomProject);
			return blueprintKingdomProject;
		}

		public static void CheckAndAddAvailableProjects()
		{
			try
			{
				Player player = Game.Instance?.Player;
				if (player == null || player.Chapter < 3)
				{
					return;
				}
				KingdomState instance = KingdomState.Instance;
				if (instance == null)
				{
					return;
				}
				KingdomTimelineManager kingdomTimelineManager = new KingdomTimelineManager();
				foreach (BlueprintKingdomProject project in AllProjects)
				{
					if (project == null || (project == ProjectTreatyStolenLands && !KingmakerCrossSaveManager.IsKingmakerInstalled) || (kingdomTimelineManager.ActiveEvents != null && kingdomTimelineManager.ActiveEvents.Any((KingdomEvent e) => e?.EventBlueprint == project)))
					{
						continue;
					}
					if (!project.Repeatable)
					{
						if (instance.HasEventTriggered(project))
						{
							continue;
						}
					}
					else if (project.Cooldown > 0)
					{
						KingdomEventHistoryEntry kingdomEventHistoryEntry = instance.FinishedEvents?.LastOrDefault((KingdomEventHistoryEntry e) => e?.Event == project);
						if (kingdomEventHistoryEntry != null && instance.CurrentDay - kingdomEventHistoryEntry.SolvedOn < project.Cooldown)
						{
							continue;
						}
					}
					kingdomTimelineManager.StartEventInRegion(project, null);
					Main.IsekaiContext.Logger.Log("[IsekaiKingdomProjects] Added crusade project: " + project.name);
				}
			}
			catch (Exception arg)
			{
				Main.IsekaiContext.Logger.LogError($"[IsekaiKingdomProjects] Error in CheckAndAddAvailableProjects: {arg}");
			}
		}
	}
}
