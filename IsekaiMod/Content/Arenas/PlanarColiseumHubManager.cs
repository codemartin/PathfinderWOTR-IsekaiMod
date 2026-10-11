using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Persistence;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Interaction;
using Kingmaker.View;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Arenas;

public static class PlanarColiseumHubManager
{
	private class ColiseumHubSpawnHandler : IAreaHandler, ISubscriber, IGlobalSubscriber
	{
		public void OnAreaDidLoad()
		{
			try
			{
				if (IsInBossSite())
				{
					GrantReturnAbilityToParty();
					return;
				}
				if (!IsInColiseum())
				{
					RemoveReturnAbilityFromParty();
					return;
				}
				List<UnitEntityData> list = Game.Instance?.LoadedAreaState?.AllEntityData?.OfType<UnitEntityData>()?.Where((UnitEntityData u) => u != null && !u.Destroyed).ToList();
				HashSet<string> hashSet = new HashSet<string>();
				if (list != null)
				{
					List<int> extra = DuplicateHubUnits(list.Select((UnitEntityData u) => u.Blueprint?.name).ToList());
					foreach (int item in extra)
					{
						list[item].MarkForDestroy();
					}
					list = list.Where((UnitEntityData u, int i) => !extra.Contains(i)).ToList();
					foreach (UnitEntityData item2 in list)
					{
						string text = item2.Blueprint?.name;
						if (IsHubUnit(text))
						{
							hashSet.Add(text);
							IsekaiNpcUnit.Protect(item2);
						}
					}
				}
				UnitEntityData unitEntityData = Game.Instance?.Player?.MainCharacter.Value;
				bool returnAbilityGranted = GrantReturnAbilityToParty();
				bool flag = hashSet.Contains("PlanarColiseumHeraldUnit");
				if (hashSet.Count < HubUnitNames.Length && unitEntityData != null && Game.Instance?.LoadedAreaState?.MainState != null)
				{
					flag |= SpawnMissingHubUnits(list ?? new List<UnitEntityData>(), hashSet);
				}
				if (NeedsEmergencyReturn(flag, returnAbilityGranted))
				{
					Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Neither the herald nor Return to Golarion could be provided; returning the party.");
					PostLog(ConstellationText.Localized("Constellation.Coliseum", "<color=#7A1010><b>[Planar Coliseum]</b></color> The rift wavers and cannot hold you here. It returns you to Golarion."));
					ReturnToGolarion();
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in PlanarColiseumHubManager.OnAreaDidLoad: " + ex);
			}
		}

		private static bool SpawnMissingHubUnits(List<UnitEntityData> units, HashSet<string> present)
		{
			if (!TryGetArenaAnchor(out var anchor, out var forward))
			{
				return false;
			}
			List<Vector3> list = IsekaiNpcUnit.WithParty(from u in units
				where u.IsInGame
				select u.Position);
			UnitEntityData unitEntityData = null;
			Dictionary<string, BlueprintUnit> dictionary = new Dictionary<string, BlueprintUnit>
			{
				{
					"PlanarColiseumHeraldUnit",
					PlanarColiseumEntrance.ColiseumHeraldUnit
				},
				{ "AstralCoinEnvoyUnit", AstralCoinEnvoyUnit },
				{ "VoidMarketSmugglerUnit", VoidMarketSmugglerUnit },
				{ "ConstellationOracleUnit", ConstellationOracleUnit }
			};
			string[] hubUnitNames = HubUnitNames;
			foreach (string text in hubUnitNames)
			{
				if (present.Contains(text) || !dictionary.TryGetValue(text, out var value) || value == null)
				{
					continue;
				}
				Vector3 position = PlaceColiseumNpc(text, anchor, forward, list);
				UnitEntityData unitEntityData2 = Game.Instance.EntityCreator.SpawnUnit(value, position, Quaternion.LookRotation(-forward), Game.Instance.LoadedAreaState.MainState);
				if (!(unitEntityData2 == null))
				{
					IsekaiNpcUnit.Protect(unitEntityData2);
					list.Add(unitEntityData2.Position);
					present.Add(text);
					if (text == "PlanarColiseumHeraldUnit")
					{
						unitEntityData = unitEntityData2;
					}
				}
			}
			Main.IsekaiContext.Logger.Log("[PlanarColiseumHubManager] Populated the Planar Coliseum gallery.");
			return unitEntityData != null;
		}

		public void OnAreaBeginUnloading()
		{
			try
			{
				if (!IsInColiseum())
				{
					return;
				}
				Game instance = Game.Instance;
				if (ColiseumUiStateGuard.ShouldEndTradeBeforeUnload(instance != null && instance.Vendor?.IsTrading == true, ColiseumUiStateGuard.CurrentVendorName()))
				{
					ColiseumUiStateGuard.EndStuckTrade("The arena is unloading");
				}
				List<UnitEntityData> list = Game.Instance?.LoadedAreaState?.AllEntityData?.OfType<UnitEntityData>()?.ToList();
				if (list == null)
				{
					return;
				}
				foreach (UnitEntityData item in list)
				{
					if (item != null && IsHubUnit(item.Blueprint?.name))
					{
						item.MarkForDestroy();
					}
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in PlanarColiseumHubManager.OnAreaBeginUnloading: " + ex);
			}
		}
	}

	private const string MonadicDevaStandardGuid = "7bcffd91514f489469a6fb6dea0a50d2";

	public static BlueprintUnit AstralCoinEnvoyUnit;

	public static BlueprintUnit VoidMarketSmugglerUnit;

	public static BlueprintUnit ConstellationOracleUnit;

	public static BlueprintDialog AstralCoinEnvoyDialog;

	public static BlueprintDialog VoidMarketSmugglerDialog;

	public static BlueprintDialog ConstellationOracleDialog;

	public static BlueprintAbility ReturnAbility;

	public static BlueprintAnswersList OracleMainAnswers;

	private static BlueprintAnswer s_OracleLeaveAnswer;

	internal const string ReturnAbilityName = "PlanarColiseumReturnAbility";

	private const string HeraldUnitName = "PlanarColiseumHeraldUnit";

	internal static readonly string[] HubUnitNames = new string[4] { "PlanarColiseumHeraldUnit", "AstralCoinEnvoyUnit", "VoidMarketSmugglerUnit", "ConstellationOracleUnit" };

	public static void AddOracleAnswer(BlueprintAnswer answer)
	{
		if (answer == null || OracleMainAnswers == null)
		{
			return;
		}
		BlueprintAnswerBaseReference reference = answer.ToReference<BlueprintAnswerBaseReference>();
		if (!OracleMainAnswers.Answers.Any((BlueprintAnswerBaseReference r) => r != null && ((BlueprintReferenceBase)r).deserializedGuid == ((BlueprintReferenceBase)reference).deserializedGuid))
		{
			int num = ((s_OracleLeaveAnswer == null) ? (-1) : OracleMainAnswers.Answers.FindIndex((BlueprintAnswerBaseReference r) => r != null && ((BlueprintReferenceBase)r).deserializedGuid == s_OracleLeaveAnswer.AssetGuid));
			if (num < 0)
			{
				OracleMainAnswers.Answers.Add(reference);
			}
			else
			{
				OracleMainAnswers.Answers.Insert(num, reference);
			}
		}
	}

	internal static DialogSpeaker CreateOracleSpeaker()
	{
		return new DialogSpeaker
		{
			m_Blueprint = ConstellationOracleUnit?.ToReference<BlueprintUnitReference>(),
			MoveCamera = false
		};
	}

	internal static void PrepareArrival()
	{
		GrantReturnAbility(Game.Instance?.Player?.MainCharacter.Value);
	}

	internal static bool NeedsEmergencyReturn(bool heraldPresent, bool returnAbilityGranted)
	{
		if (!heraldPresent)
		{
			return !returnAbilityGranted;
		}
		return false;
	}

	public static void ReturnToGolarion()
	{
		if (!IsInPlanarArea())
		{
			PostLog(ConstellationText.Localized("Constellation.Coliseum", "<color=#7A1010><b>[Planar Coliseum]</b></color> You are already standing on Golarion."));
			return;
		}
		BlueprintAreaEnterPoint destination = PlanarColiseumTravel.ResolveReturn();
		if (IsInColiseum() && DimensionalArenaManager.Instance.IsActive)
		{
			PostLog(ConstellationText.LocalizedFormat("Constellation.Coliseum", "<color=#5B2A86><b>[Planar Coliseum]</b></color> You leave the <b>{0}</b> unfinished.", DimensionalArenaManager.Instance.CurrentCupName));
		}
		ScheduleTravel(destination);
	}

	internal static bool GrantReturnAbility(UnitEntityData unit)
	{
		BlueprintAbility blueprintAbility = ReturnAbility ?? BlueprintTools.GetModBlueprint<BlueprintAbility>(Main.IsekaiContext, "PlanarColiseumReturnAbility");
		if (unit == null || blueprintAbility == null)
		{
			return false;
		}
		try
		{
			if (!unit.HasFact(blueprintAbility))
			{
				unit.AddFact(blueprintAbility);
			}
			return unit.HasFact(blueprintAbility);
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Could not grant Return to Golarion: " + ex);
			return false;
		}
	}

	internal static bool GrantReturnAbilityToParty()
	{
		Player player = Game.Instance?.Player;
		if (player == null)
		{
			return false;
		}
		List<UnitEntityData> members = new List<UnitEntityData>();
		if (player.MainCharacter.Value != null)
		{
			members.Add(player.MainCharacter.Value);
		}
		members.AddRange((player.Party ?? new List<UnitEntityData>()).Where((UnitEntityData u) => u != null && !members.Contains(u)));
		bool flag = false;
		foreach (UnitEntityData item in members)
		{
			flag |= GrantReturnAbility(item);
		}
		return flag;
	}

	internal static bool HoldsReturnAbility(bool inColiseum, bool inBossSite)
	{
		return inColiseum | inBossSite;
	}

	internal static void RemoveReturnAbilityFromParty()
	{
		BlueprintAbility blueprintAbility = ReturnAbility ?? BlueprintTools.GetModBlueprint<BlueprintAbility>(Main.IsekaiContext, "PlanarColiseumReturnAbility");
		List<UnitEntityData> list = Game.Instance?.Player?.AllCharacters;
		if (blueprintAbility == null || list == null)
		{
			return;
		}
		foreach (UnitEntityData item in list.ToList())
		{
			if (item != null && item.HasFact(blueprintAbility))
			{
				item.RemoveFact(blueprintAbility);
			}
		}
	}

	internal static bool IsColiseumArea(string areaGuid, string areaName)
	{
		return PlanarColiseumArea.IsArenaArea(areaGuid, areaName);
	}

	public static bool IsInColiseum()
	{
		BlueprintArea blueprintArea = Game.Instance?.CurrentlyLoadedArea;
		if (blueprintArea == null)
		{
			return false;
		}
		return IsColiseumArea(blueprintArea.AssetGuid.ToString(), blueprintArea.name);
	}

	public static bool IsInBossSite()
	{
		BlueprintArea blueprintArea = Game.Instance?.CurrentlyLoadedArea;
		if (blueprintArea == null)
		{
			return false;
		}
		return BossSites.IsSiteArea(blueprintArea.AssetGuid.ToString(), blueprintArea.name);
	}

	public static bool IsInPlanarArea()
	{
		return HoldsReturnAbility(IsInColiseum(), IsInBossSite());
	}

	internal static void PostLog(string message)
	{
		ConstellationChatManager.PostCombatLog(message);
	}

	public static void ScheduleTravel(BlueprintAreaEnterPoint destination, Action beforeLoad = null)
	{
		if (destination == null || Game.Instance == null)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] No destination to travel to.");
			return;
		}
		Game.Instance.ScheduleAction(delegate
		{
			try
			{
				if (!LoadingProcess.Instance.IsLoadingInProcess)
				{
					beforeLoad?.Invoke();
					Game.Instance.LoadArea(destination, AutoSaveMode.None);
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Travel to " + destination.name + " failed: " + ex);
			}
		});
	}

	public static void Init()
	{
		CreateHubBlueprints();
		try
		{
			LanternKingCameo.Create();
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Could not create the Lantern King cameo: " + ex);
		}
		try
		{
			CreateReturnAbility();
		}
		catch (Exception ex2)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Could not create Return to Golarion: " + ex2);
		}
		EventBus.Subscribe(new ColiseumHubSpawnHandler());
		EventBus.Subscribe(new IsekaiNpcUnit.ServiceNpcGuard());
	}

	internal static bool TryGetArenaAnchor(out Vector3 anchor, out Vector3 forward)
	{
		anchor = Vector3.zero;
		forward = new Vector3(0f, 0f, 1f);
		try
		{
			BlueprintArea blueprintArea = Game.Instance?.CurrentlyLoadedArea;
			if (blueprintArea != null)
			{
				int num = PlanarColiseumArea.ArenaActForArea(blueprintArea.AssetGuid.ToString(), blueprintArea.name);
				BlueprintAreaEnterPoint blueprintAreaEnterPoint = ((num > 0) ? PlanarColiseumArea.EnterPointForAct(num) : null);
				AreaEnterPoint areaEnterPoint = ((blueprintAreaEnterPoint != null) ? AreaEnterPoint.FindAreaEnterPointOnScene(blueprintAreaEnterPoint) : null);
				if (areaEnterPoint != null)
				{
					anchor = areaEnterPoint.transform.position;
					forward = NpcLayout.Flatten(areaEnterPoint.transform.forward);
					return true;
				}
			}
			UnitEntityData unitEntityData = Game.Instance?.Player?.MainCharacter.Value;
			if (unitEntityData != null)
			{
				anchor = unitEntityData.Position;
				forward = NpcLayout.Flatten(unitEntityData.OrientationDirection);
				return true;
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[PlanarColiseum] Could not find the arena's enter point: " + ex);
		}
		return false;
	}

	internal static Vector3 PlaceColiseumNpc(string unitName, Vector3 anchor, Vector3 forward, IList<Vector3> taken)
	{
		return NpcLayout.Resolve(NpcLayout.ColiseumSlot(unitName, anchor, forward), forward, taken, 3f, IsekaiNpcUnit.SnapToGround);
	}

	internal static List<int> DuplicateHubUnits(IList<string> unitNames)
	{
		HashSet<string> hashSet = new HashSet<string>();
		List<int> list = new List<int>();
		for (int i = 0; i < unitNames.Count; i++)
		{
			if (IsHubUnit(unitNames[i]) && !hashSet.Add(unitNames[i]))
			{
				list.Add(i);
			}
		}
		return list;
	}

	private static void CreateReturnAbility()
	{
		ReturnAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "PlanarColiseumReturnAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Return to Golarion");
			bp.SetDescription(Main.IsekaiContext, "Step back through the planar rift to where you came from: the world map, the town that opened the rift, or the entrance nearest to where the Otherworldly Emissary opened it for you. Any trial in progress is abandoned, and a rift or anomaly boss left standing waits for your next visit.\nEvery member of the party holds this power while standing on the coliseum's sands or at a rift or anomaly site.");
			((BlueprintUnitFact)bp).m_Icon = BlueprintTools.GetBlueprint<BlueprintItem>("14cd10f130de35940bff8f2c79607894")?.m_Icon ?? AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
			bp.Type = AbilityType.Special;
			bp.Range = AbilityRange.Personal;
			bp.CanTargetSelf = true;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = ActionFlow.DoSingle<ContextActionReturnToGolarion>();
			});
		});
	}

	private static void CreateHubBlueprints()
	{
		BlueprintUnit baseDeva = BlueprintTools.GetBlueprint<BlueprintUnit>("7bcffd91514f489469a6fb6dea0a50d2");
		AstralCoinEnvoyUnit = Helpers.CreateBlueprint(Main.IsekaiContext, "AstralCoinEnvoyUnit", delegate(BlueprintUnit bp)
		{
			bp.SetLocalisedName(Main.IsekaiContext, "The Astral Coin Envoy");
			if (baseDeva != null)
			{
				bp.m_Portrait = baseDeva.m_Portrait;
				bp.Prefab = baseDeva.Prefab;
				bp.Visual = baseDeva.Visual;
				bp.Gender = Gender.Female;
				bp.Size = baseDeva.Size;
				bp.Alignment = Alignment.TrueNeutral;
			}
			IsekaiNpcUnit.MakeServiceNpc(bp);
			bp.AddComponent(delegate(AddSharedVendor c)
			{
				c.m_m_Table = CosmicRelicsVendorTable.Table.ToReference<BlueprintSharedVendorTableReference>();
			});
			bp.AddComponent(delegate(DialogOnClick c)
			{
				c.m_Dialog = BlueprintTools.GetModBlueprintReference<BlueprintDialogReference>(Main.IsekaiContext, "AstralCoinEnvoyDialog");
				c.Conditions = ActionFlow.EmptyCondition();
				c.NoDialogActions = ActionFlow.DoNothing();
			});
		});
		DialogSpeaker envoySpeaker = new DialogSpeaker
		{
			m_Blueprint = AstralCoinEnvoyUnit.ToReference<BlueprintUnitReference>(),
			MoveCamera = false
		};
		AstralCoinEnvoyDialog = Helpers.CreateBlueprint(Main.IsekaiContext, "AstralCoinEnvoyDialog", delegate(BlueprintDialog bp)
		{
			bp.Type = DialogType.Common;
			bp.Conditions = new ConditionsChecker
			{
				Conditions = Array.Empty<Condition>()
			};
			BlueprintCue blueprintCue = TTCoreExtensions.CreateCue("Envoy_CueIntro", delegate(BlueprintCue c)
			{
				c.SetText(Main.IsekaiContext, "{n}An ethereal custodian surrounded by constellations of drifting starlight welcomes you with serene reverence.{/n}\n\n\"Greetings, champion of the mortal spheres. I oversee the celestial relic exchange. Defeating trials across the cosmos awards Cosmic Coins, which may be bartered here for supreme artifacts and eternal knowledge.\"");
				c.Speaker = envoySpeaker;
			});
			BlueprintAnswer ansTrade = TTCoreExtensions.CreateAnswer("Ans_Envoy_Trade", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "Browse the Astral Coin Envoy's relics (Trade Cosmic Coins).");
				BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Envoy_Trade", delegate(BlueprintCue ct)
				{
					ct.SetText(Main.IsekaiContext, "\"Behold the treasures of the cosmos.\"");
					ct.Speaker = envoySpeaker;
					ct.OnStop = Helpers.CreateActionList(new ActionStartTradeWithEnvoy());
				});
				a.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
				};
			});
			BlueprintCue cueLore = null;
			BlueprintAnswer ansLore = TTCoreExtensions.CreateAnswer("Ans_Envoy_Lore", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "How do I earn more Cosmic Coins?");
				cueLore = TTCoreExtensions.CreateCue("Cue_Envoy_Lore", delegate(BlueprintCue cl)
				{
					cl.SetText(Main.IsekaiContext, "\"Cosmic Coins are awarded for conquering tournament cups in this coliseum, defeating rift bosses, progressing through New Game+ cycles, achieving milestones, and issuing commercial crusade decrees.\"");
					cl.Speaker = envoySpeaker;
				});
				a.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { cueLore.ToReference<BlueprintCueBaseReference>() }
				};
			});
			BlueprintAnswer ansLeave = TTCoreExtensions.CreateAnswer("Ans_Envoy_Leave", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "I will return when I possess more coins.");
				BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Envoy_Leave", delegate(BlueprintCue cl)
				{
					cl.SetText(Main.IsekaiContext, "\"May the stars guide your path, champion.\"");
					cl.Speaker = envoySpeaker;
				});
				a.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
				};
			});
			BlueprintAnswersList bp2 = Helpers.CreateBlueprint(Main.IsekaiContext, "Envoy_AnswersList", delegate(BlueprintAnswersList al)
			{
				al.Answers = new List<BlueprintAnswerBaseReference>
				{
					ansTrade.ToReference<BlueprintAnswerBaseReference>(),
					ansLore.ToReference<BlueprintAnswerBaseReference>(),
					ansLeave.ToReference<BlueprintAnswerBaseReference>()
				};
			});
			blueprintCue.Answers = new List<BlueprintAnswerBaseReference> { bp2.ToReference<BlueprintAnswerBaseReference>() };
			cueLore.Answers = new List<BlueprintAnswerBaseReference> { bp2.ToReference<BlueprintAnswerBaseReference>() };
			bp.FirstCue = new CueSelection
			{
				Cues = new List<BlueprintCueBaseReference> { blueprintCue.ToReference<BlueprintCueBaseReference>() }
			};
		});
		VoidMarketSmugglerUnit = Helpers.CreateBlueprint(Main.IsekaiContext, "VoidMarketSmugglerUnit", delegate(BlueprintUnit bp)
		{
			bp.SetLocalisedName(Main.IsekaiContext, "The Void Market Smuggler");
			if (baseDeva != null)
			{
				bp.m_Portrait = baseDeva.m_Portrait;
				bp.Prefab = baseDeva.Prefab;
				bp.Visual = baseDeva.Visual;
				bp.Gender = Gender.Male;
				bp.Size = baseDeva.Size;
				bp.Alignment = Alignment.ChaoticNeutral;
			}
			IsekaiNpcUnit.MakeServiceNpc(bp);
			bp.AddComponent(delegate(AddSharedVendor c)
			{
				c.m_m_Table = OtherworldLuxuryVendorTable.Table.ToReference<BlueprintSharedVendorTableReference>();
			});
			bp.AddComponent(delegate(DialogOnClick c)
			{
				c.m_Dialog = BlueprintTools.GetModBlueprintReference<BlueprintDialogReference>(Main.IsekaiContext, "VoidMarketSmugglerDialog");
				c.Conditions = ActionFlow.EmptyCondition();
				c.NoDialogActions = ActionFlow.DoNothing();
			});
		});
		DialogSpeaker smugglerSpeaker = new DialogSpeaker
		{
			m_Blueprint = VoidMarketSmugglerUnit.ToReference<BlueprintUnitReference>(),
			MoveCamera = false
		};
		VoidMarketSmugglerDialog = Helpers.CreateBlueprint(Main.IsekaiContext, "VoidMarketSmugglerDialog", delegate(BlueprintDialog bp)
		{
			bp.Type = DialogType.Common;
			bp.Conditions = new ConditionsChecker
			{
				Conditions = Array.Empty<Condition>()
			};
			BlueprintCue blueprintCue = TTCoreExtensions.CreateCue("Smuggler_CueIntro", delegate(BlueprintCue c)
			{
				c.SetText(Main.IsekaiContext, "{n}A sly merchant in starlight-woven silk leans against a planar strongbox, grinning with sharp amusement.{/n}\n\n\"Hehehe... Look at what the rift dragged in. In need of weapons, rings, or reagents that mortal blacksmiths can only dream of? For enough gold, even the gods would part with their teeth. What can I get you?\"");
				c.Speaker = smugglerSpeaker;
			});
			BlueprintAnswer ansTrade = TTCoreExtensions.CreateAnswer("Ans_Smuggler_Trade", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "Show me your luxury contraband (Trade Gold).");
				BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Smuggler_Trade", delegate(BlueprintCue ct)
				{
					ct.SetText(Main.IsekaiContext, "\"Gold talks, friend. Take a look at this lot.\"");
					ct.Speaker = smugglerSpeaker;
					ct.OnStop = Helpers.CreateActionList(new ActionStartTradeWithSmuggler());
				});
				a.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
				};
			});
			BlueprintCue cueLore = null;
			BlueprintAnswer ansLore = TTCoreExtensions.CreateAnswer("Ans_Smuggler_Lore", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "How did you acquire +5 and +6 enchantments in the Worldwound?");
				cueLore = TTCoreExtensions.CreateCue("Cue_Smuggler_Lore", delegate(BlueprintCue cl)
				{
					cl.SetText(Main.IsekaiContext, "\"Who said anything about the Worldwound? I siphon supplies straight from the Astral Plane and dead empires across the Great Beyond. The crusaders fight in mud; we deal in cosmic supremacy.\"");
					cl.Speaker = smugglerSpeaker;
				});
				a.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { cueLore.ToReference<BlueprintCueBaseReference>() }
				};
			});
			BlueprintAnswer ansLeave = TTCoreExtensions.CreateAnswer("Ans_Smuggler_Leave", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "I have no need of your wares right now.");
				BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Smuggler_Leave", delegate(BlueprintCue cl)
				{
					cl.SetText(Main.IsekaiContext, "\"Come back when your bags are heavier. I never close up shop.\"");
					cl.Speaker = smugglerSpeaker;
				});
				a.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
				};
			});
			BlueprintAnswersList bp2 = Helpers.CreateBlueprint(Main.IsekaiContext, "Smuggler_AnswersList", delegate(BlueprintAnswersList al)
			{
				al.Answers = new List<BlueprintAnswerBaseReference>
				{
					ansTrade.ToReference<BlueprintAnswerBaseReference>(),
					ansLore.ToReference<BlueprintAnswerBaseReference>(),
					ansLeave.ToReference<BlueprintAnswerBaseReference>()
				};
			});
			blueprintCue.Answers = new List<BlueprintAnswerBaseReference> { bp2.ToReference<BlueprintAnswerBaseReference>() };
			cueLore.Answers = new List<BlueprintAnswerBaseReference> { bp2.ToReference<BlueprintAnswerBaseReference>() };
			bp.FirstCue = new CueSelection
			{
				Cues = new List<BlueprintCueBaseReference> { blueprintCue.ToReference<BlueprintCueBaseReference>() }
			};
		});
		ConstellationOracleUnit = Helpers.CreateBlueprint(Main.IsekaiContext, "ConstellationOracleUnit", delegate(BlueprintUnit bp)
		{
			bp.SetLocalisedName(Main.IsekaiContext, "The High Oracle of the Constellations");
			if (baseDeva != null)
			{
				bp.m_Portrait = baseDeva.m_Portrait;
				bp.Prefab = baseDeva.Prefab;
				bp.Visual = baseDeva.Visual;
				bp.Gender = Gender.Female;
				bp.Size = baseDeva.Size;
				bp.Alignment = Alignment.TrueNeutral;
			}
			IsekaiNpcUnit.MakeServiceNpc(bp);
			bp.AddComponent(delegate(DialogOnClick c)
			{
				c.m_Dialog = BlueprintTools.GetModBlueprintReference<BlueprintDialogReference>(Main.IsekaiContext, "ConstellationOracleDialog");
				c.Conditions = ActionFlow.EmptyCondition();
				c.NoDialogActions = ActionFlow.DoNothing();
			});
		});
		DialogSpeaker oracleSpeaker = new DialogSpeaker
		{
			m_Blueprint = ConstellationOracleUnit.ToReference<BlueprintUnitReference>(),
			MoveCamera = false
		};
		ConstellationOracleDialog = Helpers.CreateBlueprint(Main.IsekaiContext, "ConstellationOracleDialog", delegate(BlueprintDialog bp)
		{
			bp.Type = DialogType.Common;
			bp.Conditions = new ConditionsChecker
			{
				Conditions = Array.Empty<Condition>()
			};
			BlueprintCue blueprintCue = TTCoreExtensions.CreateCue("Oracle_CueIntro", delegate(BlueprintCue c)
			{
				c.SetText(Main.IsekaiContext, "{n}A blindfolded seer clad in celestial stoles meditates before an astral scrying basin reflecting galaxies.{/n}\n\n\"The constellations observe all who tread beyond fate. Speak, champion: what communion do you seek with the celestial observers?\"");
				c.Speaker = oracleSpeaker;
			});
			BlueprintAnswer ansStore = TTCoreExtensions.CreateAnswer("Ans_Oracle_Store", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "Commune with the constellations at the Cosmic Constellation Exchange (Trade Cosmic Coins).");
				a.AddShowCondition(delegate(ConditionColiseumTrialActive c)
				{
					c.Not = true;
				});
				a.AddShowCondition<ConditionConstellationsEnabled>();
				BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Oracle_Store", delegate(BlueprintCue cs)
				{
					cs.SetText(Main.IsekaiContext, "\"Let the divine resonance wash over your soul.\"");
					cs.Speaker = oracleSpeaker;
					cs.OnStop = Helpers.CreateActionList(new ActionOpenCosmicStoreFromColiseum());
				});
				a.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
				};
			});
			BlueprintCue cueLore = null;
			BlueprintAnswer ansLore = TTCoreExtensions.CreateAnswer("Ans_Oracle_Lore", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "Which constellations watch over the coliseum?");
				cueLore = TTCoreExtensions.CreateCue("Cue_Oracle_Lore", delegate(BlueprintCue cl)
				{
					cl.SetText(Main.IsekaiContext, "\"Desna's song, Gorum's fury, Iomedae's righteousness, Calistria's venom, Nethys's duality, Asmodeus's order, the Lantern King's mischief, and Yog-Sothoth's timeless gate. When you fight, they all bear witness.\"");
					cl.Speaker = oracleSpeaker;
				});
				a.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { cueLore.ToReference<BlueprintCueBaseReference>() }
				};
			});
			BlueprintAnswer ansLeave = TTCoreExtensions.CreateAnswer("Ans_Oracle_Leave", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "I will meditate on your words.");
				BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_Oracle_Leave", delegate(BlueprintCue cl)
				{
					cl.SetText(Main.IsekaiContext, "\"The cosmos remains with you, always.\"");
					cl.Speaker = oracleSpeaker;
				});
				a.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
				};
			});
			s_OracleLeaveAnswer = ansLeave;
			BlueprintAnswersList bp2 = (OracleMainAnswers = Helpers.CreateBlueprint(Main.IsekaiContext, "Oracle_AnswersList", delegate(BlueprintAnswersList al)
			{
				al.Answers = new List<BlueprintAnswerBaseReference>
				{
					ansStore.ToReference<BlueprintAnswerBaseReference>(),
					ansLore.ToReference<BlueprintAnswerBaseReference>(),
					ansLeave.ToReference<BlueprintAnswerBaseReference>()
				};
			}));
			blueprintCue.Answers = new List<BlueprintAnswerBaseReference> { bp2.ToReference<BlueprintAnswerBaseReference>() };
			cueLore.Answers = new List<BlueprintAnswerBaseReference> { bp2.ToReference<BlueprintAnswerBaseReference>() };
			bp.FirstCue = new CueSelection
			{
				Cues = new List<BlueprintCueBaseReference> { blueprintCue.ToReference<BlueprintCueBaseReference>() }
			};
		});
	}

	internal static bool IsHubUnit(string unitBlueprintName)
	{
		if (!string.IsNullOrEmpty(unitBlueprintName))
		{
			return HubUnitNames.Contains(unitBlueprintName);
		}
		return false;
	}
}
