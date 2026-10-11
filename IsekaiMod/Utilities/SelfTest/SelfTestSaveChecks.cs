using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Arenas;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.GlobalMap;
using IsekaiMod.Content.Quests;
using Kingmaker;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Quests;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Parts;
using UnityEngine;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestSaveChecks
{
	internal sealed class ArenaVisit
	{
		public bool Arrived;

		public string ArrivedAreaName = "";

		public bool HasReturnAbility;

		public List<string> HubUnitsSeen = new List<string>();

		public List<string> ServiceNpcProblems = new List<string>();

		public List<string> WavePlacementProblems = new List<string>();

		public bool ReturnedToGolarion;

		public string ReturnedAreaName = "";

		public string StartedAreaName = "";

		public List<string> Exceptions = new List<string>();

		public List<string> PartyBefore = new List<string>();

		public List<string> PartyAfter = new List<string>();

		public bool TradeProbeStarted;

		public bool TradingAfterReturn;

		public bool FullScreenAfterReturn;

		public string EnvoyTradeOutcome = "";

		public int EnvoyStock;

		public List<string> UndrawableItems = new List<string>();
	}

	internal const string IdPrefix = "save-";

	internal const int ImplausibleCoinBalance = 1000000;

	internal static readonly string[] QuestRecordKeyPrefixes = new string[5] { "Relic.", "Coins.", "Trial.", "Decree.", "Devourer." };

	internal const string ServiceNpcProblemPrefix = "Service NPC: ";

	internal const string EmissaryUnreachablePrefix = "Gateway Emissary unreachable: ";

	internal const string WavePlacementPrefix = "Wave placement: ";

	private static readonly Vector2[][] ProbeWaves = new Vector2[4][]
	{
		new Vector2[3]
		{
			new Vector2(0f, 7f),
			new Vector2(-5f, 0f),
			new Vector2(5f, 0f)
		},
		new Vector2[3]
		{
			new Vector2(0f, 6f),
			new Vector2(-6f, -3f),
			new Vector2(6f, -3f)
		},
		new Vector2[3]
		{
			new Vector2(0f, 7f),
			new Vector2(0f, -7f),
			new Vector2(-6f, 0f)
		},
		new Vector2[4]
		{
			new Vector2(0f, 7f),
			new Vector2(-6f, 0f),
			new Vector2(6f, 0f),
			new Vector2(0f, -7f)
		}
	};

	internal const string EmissaryRouteProblemPrefix = "Emissary route: ";

	internal const string ProbeBroadcastText = "[IsekaiSelfTest] live broadcast probe";

	internal const string ProbeBroadcastSponsor = "Isekai Self-Test";

	internal static readonly string[][] CompanionConversations = new string[4][]
	{
		new string[3] { "Wenduag", "ae766624c03058440a036de90a7f2009", "7383531c4dcf5f54d9d291e452bb84dd" },
		new string[3] { "Lann", "cb29621d99b902e4da6f5d232352fbda", "054c1b209de5c9042a02d6736cd3b2ac" },
		new string[3] { "Seelah", "54be53f0b35bf3c4592a97ae335fe765", "ed7b39d0716d25c439f2b93c409da883" },
		new string[3] { "Camellia", "397b090721c41044ea3220445300e1b8", "ee686e5c551c67840946f9a87592470a" }
	};

	internal static List<UnitEntityData> UnitsInArea()
	{
		try
		{
			return Game.Instance?.LoadedAreaState?.AllEntityData?.OfType<UnitEntityData>().Where((UnitEntityData u) => u != null && !u.Destroyed).ToList() ?? new List<UnitEntityData>();
		}
		catch
		{
			return new List<UnitEntityData>();
		}
	}

	internal static int CountOf(IEnumerable<UnitEntityData> units, string blueprintName)
	{
		return units.Count((UnitEntityData u) => string.Equals(u.Blueprint?.name, blueprintName, StringComparison.Ordinal));
	}

	internal static Dictionary<string, Dictionary<string, string>> ModStorages()
	{
		Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
		try
		{
			Dictionary<string, Dictionary<string, string>> dictionary2 = (Game.Instance?.Player?.Get<EntityPartKeyValueStorage>())?.m_Data;
			if (dictionary2 == null)
			{
				return dictionary;
			}
			foreach (KeyValuePair<string, Dictionary<string, string>> item in dictionary2)
			{
				if (item.Key != null)
				{
					dictionary[item.Key] = ((item.Value == null) ? new Dictionary<string, string>(StringComparer.Ordinal) : new Dictionary<string, string>(item.Value, StringComparer.Ordinal));
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[IsekaiSelfTest] Could not read the mod's save storages: " + ex);
		}
		return dictionary;
	}

	private static string AreaName()
	{
		return Game.Instance?.CurrentlyLoadedArea?.name ?? "";
	}

	private static int Chapter()
	{
		return (Game.Instance?.Player?.Chapter).GetValueOrDefault();
	}

	internal static SelfTestChecks.Outcome WorldMatchesSave(string expectedAreaName, string expectedCharacterName)
	{
		List<string> list = new List<string>();
		Game instance = Game.Instance;
		BlueprintArea blueprintArea = instance?.CurrentlyLoadedArea;
		if (blueprintArea == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "No area is loaded: the save did not open a world at all.");
		}
		if (!string.IsNullOrEmpty(expectedAreaName) && !string.Equals(blueprintArea.name, expectedAreaName, StringComparison.Ordinal))
		{
			list.Add("The save's header names the area '" + expectedAreaName + "', but '" + blueprintArea.name + "' loaded.");
		}
		UnitEntityData unitEntityData = instance.Player?.MainCharacter.Value;
		if (unitEntityData == null)
		{
			list.Add("There is no main character in the loaded game.");
		}
		else if (!string.IsNullOrEmpty(expectedCharacterName) && !string.Equals(unitEntityData.CharacterName, expectedCharacterName, StringComparison.Ordinal))
		{
			list.Add("The save's header names the character '" + expectedCharacterName + "', but '" + unitEntityData.CharacterName + "' loaded.");
		}
		int valueOrDefault = (instance.Player?.Party?.Count).GetValueOrDefault();
		if (valueOrDefault <= 0)
		{
			list.Add("The party is empty.");
		}
		string text = $"Loaded '{blueprintArea.name}' in chapter {Chapter()} with {valueOrDefault} party member(s)" + ((unitEntityData == null) ? "" : (", main character '" + unitEntityData.CharacterName + "'")) + ".";
		return SelfTestChecks.Outcome.Of(list.Count == 0, (list.Count == 0) ? text : (text + " " + list.Count + " problem(s)."), list);
	}

	internal static SelfTestChecks.Outcome NoExceptionsDuringLoad(IReadOnlyList<string> raised)
	{
		List<string> list = (raised ?? new string[0]).ToList();
		return SelfTestChecks.Outcome.Of(list.Count == 0, (list.Count == 0) ? "No IsekaiMod exception was logged while the save loaded." : (list.Count + " IsekaiMod exception(s) were logged while the save loaded."), list);
	}

	internal static SelfTestChecks.Outcome ModStateLoaded()
	{
		List<string> list = new List<string>();
		Dictionary<string, Dictionary<string, string>> dictionary = ModStorages();
		list.Add("Storages on the save's Player entity: " + ((dictionary.Count == 0) ? "none (a save made before this mod wrote any)" : string.Join(", ", from s in dictionary.OrderBy((KeyValuePair<string, Dictionary<string, string>> s) => s.Key, StringComparer.Ordinal)
			select s.Key + " (" + s.Value.Count + ")")));
		int num;
		try
		{
			num = DivineTokens.GetBalance();
		}
		catch (Exception ex)
		{
			list.Add("Reading the cosmic coin balance threw " + ex.GetType().Name + ": " + ex.Message);
			num = 0;
		}
		list.Add("Cosmic coin balance: " + num + ".");
		if (num < 0)
		{
			list.Add("The coin balance is negative, which nothing in the economy can produce.");
		}
		if (num > 1000000)
		{
			list.Add($"The coin balance is {num}, past the {1000000} sanity bound; the record is corrupt or overflowed.");
		}
		Dictionary<string, string> dictionary2 = PersonalQuestRecord.Find();
		if (dictionary2 == null)
		{
			list.Add("No personal-quest record in this save (nothing has been recorded yet).");
		}
		else
		{
			list.Add("Personal-quest record: " + dictionary2.Count + " entrie(s).");
			foreach (string item in dictionary2.Keys.OrderBy((string k) => k, StringComparer.Ordinal))
			{
				if (!IsKnownQuestRecordKey(item))
				{
					list.Add("Unrecognised key in the personal-quest record: '" + item + "'.");
				}
			}
		}
		List<string> list2 = QuestBookProblems();
		list.AddRange(list2);
		bool flag = num < 0 || num > 1000000 || list2.Count > 0 || list.Any((string d) => d.StartsWith("Unrecognised key", StringComparison.Ordinal)) || list.Any((string d) => d.StartsWith("Reading the cosmic", StringComparison.Ordinal));
		return SelfTestChecks.Outcome.Of(!flag, flag ? "The mod's per-save state came back wrong." : "The mod's per-save state came back readable and consistent.", list);
	}

	internal static bool IsKnownQuestRecordKey(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			return false;
		}
		if (string.Equals(key, "ArchetypeRelicClaimed", StringComparison.Ordinal))
		{
			return true;
		}
		return QuestRecordKeyPrefixes.Any((string p) => key.StartsWith(p, StringComparison.Ordinal));
	}

	internal static List<string> QuestBookProblems()
	{
		List<string> list = new List<string>();
		try
		{
			QuestBook questBook = Game.Instance?.Player?.QuestBook;
			if (questBook == null)
			{
				return list;
			}
			foreach (Quest item in questBook.Quests.ToList())
			{
				BlueprintQuest blueprintQuest = item?.Blueprint;
				if (blueprintQuest == null)
				{
					list.Add("The quest book holds a quest whose blueprint did not resolve.");
				}
				else
				{
					if (!IsModBlueprintName(blueprintQuest.name))
					{
						continue;
					}
					foreach (QuestObjective item2 in item.Objectives.ToList())
					{
						BlueprintQuestObjective objectiveBlueprint = item2?.Blueprint;
						if (objectiveBlueprint == null)
						{
							list.Add("'" + blueprintQuest.name + "' holds an objective whose blueprint did not resolve.");
							continue;
						}
						if (!blueprintQuest.Objectives.Any((BlueprintQuestObjective o) => o != null && o == objectiveBlueprint))
						{
							list.Add("'" + blueprintQuest.name + "' holds the objective '" + objectiveBlueprint.name + "', which that quest does not list.");
						}
						if (item.State != QuestState.Started && item.State != QuestState.Completed && item.State != QuestState.Failed && item2.State != QuestObjectiveState.None)
						{
							list.Add($"'{objectiveBlueprint.name}' is {item2.State} although its quest '{blueprintQuest.name}' is {item.State}.");
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			list.Add("Reading the quest book threw " + ex.GetType().Name + ": " + ex.Message);
		}
		return list;
	}

	internal static bool IsModBlueprintName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return false;
		}
		if (!name.StartsWith("Isekai", StringComparison.Ordinal) && !name.StartsWith("Planar", StringComparison.Ordinal))
		{
			return name.StartsWith("Cosmic", StringComparison.Ordinal);
		}
		return true;
	}

	internal static List<string> WavePlacementProblems()
	{
		List<string> list = new List<string>();
		try
		{
			if (!PlanarColiseumHubManager.TryGetArenaAnchor(out var anchor, out var forward))
			{
				list.Add("Wave placement: the arena has no enter point on the scene to lay the fight out from.");
				return list;
			}
			Vector3 vector = NpcLayout.FightPoint(anchor, forward, 0f, 0f);
			Vector3 vector2 = NpcLayout.RightOf(forward);
			Vector3 vector3 = NpcLayout.Flatten(forward);
			List<Vector3> list2 = new List<Vector3>
			{
				vector,
				vector + vector2 * 1.5f,
				vector - vector2 * 1.5f,
				vector + vector3 * 1.5f,
				vector - vector3 * 1.5f
			};
			uint? area = IsekaiNpcUnit.PartyGroundArea();
			List<Vector3> list3 = IsekaiNpcUnit.ServiceNpcPositions();
			int num = 0;
			Vector2[][] probeWaves = ProbeWaves;
			foreach (Vector2[] obj in probeWaves)
			{
				num++;
				List<Vector3> list4 = new List<Vector3>();
				Vector2[] array = obj;
				for (int j = 0; j < array.Length; j++)
				{
					Vector2 vector4 = array[j];
					Vector3 spot = IsekaiNpcUnit.PlaceCombatant(NpcLayout.FightPoint(anchor, forward, vector4.x, vector4.y), forward, list4, list2);
					string text = $"probe wave {num}, contestant written at ({vector4.x}, {vector4.y})";
					if (!IsekaiNpcUnit.SnapToReachableGround(spot, area).HasValue)
					{
						string[] obj2 = new string[5] { "Wave placement: ", text, " lands at ", null, null };
						Vector3 vector5 = spot;
						obj2[3] = vector5.ToString();
						obj2[4] = ", on ground the party cannot walk to.";
						list.Add(string.Concat(obj2));
					}
					float num2 = list2.Min((Vector3 p) => NpcLayout.GroundDistance(p, spot));
					if (num2 < 5.99f)
					{
						list.Add("Wave placement: " + text + $" lands {num2:0.0} m from a party member.");
					}
					if (list3.Count > 0)
					{
						float num3 = list3.Min((Vector3 p) => NpcLayout.GroundDistance(p, spot));
						if (num3 < 5.99f)
						{
							list.Add("Wave placement: " + text + $" lands {num3:0.0} m from a hub NPC.");
						}
					}
					if (list4.Count > 0)
					{
						float num4 = list4.Min((Vector3 p) => NpcLayout.GroundDistance(p, spot));
						if (num4 < 2.99f)
						{
							list.Add("Wave placement: " + text + $" lands {num4:0.0} m from another contestant of its wave.");
						}
					}
					list4.Add(spot);
				}
			}
		}
		catch (Exception ex)
		{
			list.Add("Wave placement: the probe threw " + ex.GetType().Name + ": " + ex.Message);
		}
		return list;
	}

	internal static List<string> ServiceNpcProblems()
	{
		List<string> list = new List<string>();
		try
		{
			List<UnitEntityData> list2 = IsekaiNpcUnit.ServiceNpcsInArea();
			list.AddRange(NpcLayout.TooClose(list2.Select((UnitEntityData u) => u.Blueprint?.name ?? "?").ToList(), list2.Select((UnitEntityData u) => u.Position).ToList(), 2f));
			foreach (UnitEntityData item in list2)
			{
				list.AddRange(IsekaiNpcUnit.ProtectionProblems(item));
			}
		}
		catch (Exception ex)
		{
			list.Add("Reading the service NPCs threw " + ex.GetType().Name + ": " + ex.Message);
		}
		return list;
	}

	internal static SelfTestChecks.Outcome SpawnedContent()
	{
		List<string> list = new List<string>();
		List<UnitEntityData> list2 = UnitsInArea();
		string text = AreaName();
		int num = Chapter();
		bool arenaEnabled = Main.IsekaiContext.AddedContent?.EnableDimensionalArena ?? true;
		bool flag;
		try
		{
			flag = PlanarGatewayEmissary.IsEmissaryArea(text, num) && PlanarGatewayEmissary.IsPresent(text, num, arenaEnabled, PlanarGatewayEmissary.AnySiteOfferedHere());
		}
		catch (Exception ex)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "Deciding whether the Gateway Emissary belongs in '" + text + "' threw " + ex.GetType().Name + ": " + ex.Message);
		}
		int num2 = CountOf(list2, "PlanarGatewayEmissaryUnit");
		list.Add($"Area '{text}', chapter {num}, {list2.Count} unit(s) loaded; the Gateway Emissary is " + (flag ? "expected" : "not expected") + " here and " + num2 + " is/are standing.");
		if (flag && num2 == 0)
		{
			list.Add("The rules say the Gateway Emissary stands in this area, but no such unit is in it.");
		}
		if (!flag && num2 > 0)
		{
			list.Add("The Gateway Emissary is standing in an area the rules exclude him from.");
		}
		if (num2 > 1)
		{
			list.Add($"{num2} Gateway Emissaries are standing in one area; the spawn handler keeps at most one.");
		}
		if (num2 == 1)
		{
			try
			{
				Game instance = Game.Instance;
				UnitEntityData unitEntityData = list2.First((UnitEntityData u) => u?.Blueprint?.name == "PlanarGatewayEmissaryUnit");
				if (!PlanarGatewayEmissary.IsAreaOwnPart(instance?.CurrentlyLoadedAreaPart, instance?.CurrentlyLoadedArea))
				{
					list.Add("The party stands in a sub-part of the area, so the emissary's reachability was not asked.");
				}
				else if (!PlanarGatewayEmissary.IsReachableFromParty(unitEntityData))
				{
					list.Add("Gateway Emissary unreachable: he stands at " + unitEntityData.Position.ToString() + ", on ground the party cannot walk to from where it stands.");
				}
			}
			catch (Exception ex2)
			{
				list.Add("Gateway Emissary unreachable: asking whether the party can reach him threw " + ex2.GetType().Name + ": " + ex2.Message);
			}
		}
		bool flag2 = PlanarColiseumHubManager.IsInColiseum();
		string[] hubUnitNames = PlanarColiseumHubManager.HubUnitNames;
		foreach (string text2 in hubUnitNames)
		{
			if (CountOf(list2, text2) > 0 && !flag2)
			{
				list.Add("The coliseum hub unit '" + text2 + "' is standing in '" + text + "', which is not an arena.");
			}
		}
		list.AddRange(WorldMapMarkerProblems());
		foreach (string item in ServiceNpcProblems())
		{
			list.Add("Service NPC: " + item);
		}
		if (flag)
		{
			list.AddRange(EmissaryRouteProblems(text, num));
		}
		bool flag3 = (flag && num2 == 0) || (!flag && num2 > 0) || num2 > 1 || list.Any((string d) => d.StartsWith("Gateway Emissary unreachable: ", StringComparison.Ordinal)) || list.Any((string d) => d.StartsWith("Service NPC: ", StringComparison.Ordinal)) || list.Any((string d) => d.StartsWith("The coliseum hub unit", StringComparison.Ordinal)) || list.Any((string d) => d.StartsWith("World-map marker", StringComparison.Ordinal)) || list.Any((string d) => d.StartsWith("Emissary route: ", StringComparison.Ordinal));
		return SelfTestChecks.Outcome.Of(!flag3, flag3 ? "Spawned content does not match what the mod's rules say for this area." : "The Gateway Emissary, the hub units and the world-map markers are where this area and chapter say they should be.", list);
	}

	internal static List<string> EmissaryRouteProblems(string area, int chapter)
	{
		List<string> list = new List<string>();
		try
		{
			for (int i = 1; i <= 5; i++)
			{
				if (BossSites.CurrentAnomalyOffered(i))
				{
					if (!BossSiteDelivery.For(BossSiteKind.Anomaly, i, chapter).Any((Delivery r) => r.Route == DeliveryRoute.Emissary && string.Equals(r.TownAreaName, area, StringComparison.OrdinalIgnoreCase)))
					{
						list.Add(string.Format("{0}Barren anomaly {1} is open in chapter {2}, but the route table does not send it through the emissary in '{3}'.", "Emissary route: ", i, chapter, area));
					}
					else if (!PlanarGatewayEmissary.CurrentlyOffersAnomaly(i))
					{
						list.Add(string.Format("{0}Barren anomaly {1} is open and routed through the emissary in '{2}', but his dialog does not offer it.", "Emissary route: ", i, area));
					}
				}
			}
			if (BossSites.CurrentRiftOffered())
			{
				int num = BossSites.CurrentAccessibleRiftAct();
				if (BossSiteDelivery.For(BossSiteKind.Rift, num, chapter).Any((Delivery r) => r.Route == DeliveryRoute.Emissary && string.Equals(r.TownAreaName, area, StringComparison.OrdinalIgnoreCase)) && !PlanarGatewayEmissary.CurrentlyOffersRift())
				{
					list.Add(string.Format("{0}Rift act {1} is open and routed through the emissary in '{2}', but his dialog does not offer it.", "Emissary route: ", num, area));
				}
			}
		}
		catch (Exception ex)
		{
			list.Add("Emissary route: reading the open fights threw " + ex.GetType().Name + ": " + ex.Message);
		}
		return list;
	}

	internal static List<string> WorldMapMarkerProblems()
	{
		List<string> list = new List<string>();
		foreach (var item in PlanarColiseumMapPoint.Tiles.Select((PlanarColiseumMapPoint.Tile t) => new
		{
			Label = (t?.Point?.name ?? "(unnamed tile)"),
			Point = t?.Point
		}))
		{
			try
			{
				if (item.Point != null)
				{
					BlueprintGlobalMap globalMap = item.Point.GlobalMap;
					if (globalMap == null)
					{
						list.Add("World-map marker " + item.Label + " belongs to no global map.");
					}
					else if ((globalMap.GetEdges(item.Point)?.Where((BlueprintGlobalMapEdge e) => e != null).ToList() ?? new List<BlueprintGlobalMapEdge>()).Count == 0)
					{
						list.Add("World-map marker " + item.Label + " is on '" + globalMap.name + "' with no edge reaching it, so the party can never travel to it.");
					}
				}
			}
			catch (Exception ex)
			{
				list.Add("World-map marker " + item.Label + " threw " + ex.GetType().Name + ": " + ex.Message);
			}
		}
		return list;
	}

	internal static SelfTestChecks.Outcome BroadcastReachesOverlayAndArchive()
	{
		if (!ConstellationSettings.Enabled)
		{
			return SelfTestChecks.Outcome.Of(passed: true, "The constellations are switched off in the settings, so no broadcast surfaces exist to post to.");
		}
		List<string> list = new List<string>();
		ConstellationChatOverlay.EnsureInstance();
		int num = ConstellationChatOverlay.LiveMessageCount + ConstellationChatOverlay.PendingMessageCount;
		int totalCount = ConstellationEventHistory.GetTotalCount();
		int version = ConstellationEventHistory.Version;
		ConstellationChatManager.PostLog("[IsekaiSelfTest] live broadcast probe", "Isekai Self-Test", ConstellationCategory.Subclass, 0, "Self-test");
		ConstellationChatOverlay.FlushPending();
		int num2 = ConstellationChatOverlay.LiveMessageCount + ConstellationChatOverlay.PendingMessageCount;
		int totalCount2 = ConstellationEventHistory.GetTotalCount();
		int version2 = ConstellationEventHistory.Version;
		list.Add($"Overlay backlog {num} -> {num2}; archive {totalCount} -> {totalCount2}; archive version {version} -> {version2}.");
		if (num2 <= num)
		{
			list.Add("The live overlay did not receive the broadcast.");
		}
		if (totalCount2 <= totalCount)
		{
			list.Add("The archive did not receive the broadcast.");
		}
		if (version2 == version)
		{
			list.Add("The archive's version did not change, so the archive window would not refresh.");
		}
		if (!ConstellationEventHistory.GetEvents("Isekai Self-Test").Any((ConstellationMessageEvent e) => (e?.Message ?? "").IndexOf("[IsekaiSelfTest] live broadcast probe", StringComparison.Ordinal) >= 0))
		{
			list.Add("The archive holds no row with the probe's text, so the broadcast was recorded under the wrong sponsor or not at all.");
		}
		bool flag = num2 <= num || totalCount2 <= totalCount || version2 == version || list.Any((string d) => d.StartsWith("The archive holds no row", StringComparison.Ordinal));
		return SelfTestChecks.Outcome.Of(!flag, flag ? "A broadcast posted in a live game did not reach both surfaces." : "A broadcast posted through the chat manager reached the live overlay and the archive.", list);
	}

	internal static List<string> UndrawableInventoryItems()
	{
		List<string> list = new List<string>();
		try
		{
			List<ItemEntity> list2 = Game.Instance?.Player?.Inventory?.Items;
			if (list2 == null)
			{
				return list;
			}
			foreach (ItemEntity item in list2.ToList())
			{
				string text = ItemRenderGuard.RenderProblem(item?.Blueprint);
				if (text != null)
				{
					list.Add((item?.Blueprint?.name ?? "<no blueprint>") + ": " + text);
				}
			}
		}
		catch (Exception ex)
		{
			list.Add("Reading the inventory threw " + ex.GetType().Name + ": " + ex.Message);
		}
		return list;
	}

	internal static List<string> PartyRoster()
	{
		try
		{
			return Game.Instance?.Player?.Party?.Where((UnitEntityData u) => u != null).Select((UnitEntityData u) => u.UniqueId ?? "").ToList() ?? new List<string>();
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[IsekaiSelfTest] Reading the party roster threw: " + ex.Message);
			return new List<string>();
		}
	}

	internal static SelfTestChecks.Outcome ArenaRoundTrip(ArenaVisit visit)
	{
		List<string> list = new List<string>();
		if (visit == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The area transition recorded nothing at all.");
		}
		list.Add("Started in '" + visit.StartedAreaName + "', arrived in '" + visit.ArrivedAreaName + "', came back to '" + visit.ReturnedAreaName + "'.");
		list.AddRange(visit.Exceptions);
		if (!visit.Arrived)
		{
			list.Add("The party never arrived in an arena.");
		}
		else
		{
			if (!PlanarColiseumHubManager.IsColiseumArea("", visit.ArrivedAreaName))
			{
				list.Add("'" + visit.ArrivedAreaName + "' is not one of the mod's arenas.");
			}
			List<string> list2 = PlanarColiseumHubManager.HubUnitNames.Where((string n) => !visit.HubUnitsSeen.Contains(n)).ToList();
			list.Add("Hub units on the sands: " + ((visit.HubUnitsSeen.Count == 0) ? "none" : string.Join(", ", visit.HubUnitsSeen)) + ".");
			if (list2.Count > 0)
			{
				list.Add("Hub units the arena should hold and does not: " + string.Join(", ", list2) + ".");
			}
			foreach (string serviceNpcProblem in visit.ServiceNpcProblems)
			{
				list.Add("Service NPC: " + serviceNpcProblem);
			}
			list.AddRange(visit.WavePlacementProblems);
			if (!visit.HasReturnAbility)
			{
				list.Add("The main character was not given Return to Golarion, so the only way off the sands is the herald.");
			}
		}
		if (!visit.ReturnedToGolarion)
		{
			list.Add("The party did not come back to Golarion; a real playthrough would be stranded in the arena.");
		}
		else if (PlanarColiseumHubManager.IsColiseumArea("", visit.ReturnedAreaName))
		{
			list.Add("The return landed in '" + visit.ReturnedAreaName + "', which is still an arena.");
		}
		bool flag = visit.PartyBefore.SequenceEqual(visit.PartyAfter);
		bool flag2 = ColiseumUiStateGuard.HudIsUsable(visit.TradingAfterReturn, visit.FullScreenAfterReturn);
		if (visit.ReturnedToGolarion)
		{
			list.Add(visit.TradeProbeStarted ? "A trade with a hub vendor was left open on the way out of the arena." : "No hub vendor was there to leave a trade open with, so only the plain return was measured.");
			if (!flag2)
			{
				list.Add("The HUD came back unusable: " + (visit.TradingAfterReturn ? "the engine still thinks the party is trading" : "the game is still in a full-screen UI mode") + ", so the local map, level up, inventory and character sheet buttons do nothing and every companion is on the party bar.");
			}
			if (!flag)
			{
				list.Add($"The party bar changed across the trip: {visit.PartyBefore.Count} before, {visit.PartyAfter.Count} after.");
			}
		}
		bool flag3 = true;
		if (visit.Arrived)
		{
			flag3 = visit.EnvoyTradeOutcome == "Opened" && visit.EnvoyStock > 0;
			list.Add(string.Format("The Astral Coin Envoy's shop: {0}, {1} stacks on the shelf.", (visit.EnvoyTradeOutcome.Length == 0) ? "not tried" : visit.EnvoyTradeOutcome, visit.EnvoyStock));
			if (!flag3)
			{
				list.Add("The envoy's first answer would not have opened a shop with its stock.");
			}
		}
		if (visit.ReturnedToGolarion && visit.UndrawableItems.Count > 0)
		{
			list.Add("Items the equipment screen cannot draw, so it would open on a blank page: " + string.Join("; ", visit.UndrawableItems) + ".");
		}
		bool flag4 = !visit.Arrived || !visit.ReturnedToGolarion || !visit.HasReturnAbility || !flag3 || (visit.ReturnedToGolarion && visit.UndrawableItems.Count > 0) || (visit.ReturnedToGolarion && (!flag2 || !flag)) || visit.Exceptions.Count > 0 || list.Any((string d) => d.StartsWith("Hub units the arena should hold", StringComparison.Ordinal)) || list.Any((string d) => d.StartsWith("Service NPC: ", StringComparison.Ordinal)) || list.Any((string d) => d.StartsWith("Wave placement: ", StringComparison.Ordinal)) || list.Any((string d) => d.StartsWith("The return landed", StringComparison.Ordinal)) || list.Any((string d) => d.EndsWith("is not one of the mod's arenas.", StringComparison.Ordinal));
		return SelfTestChecks.Outcome.Of(!flag4, flag4 ? "The arena round trip did not behave." : ("The party travelled to '" + visit.ArrivedAreaName + "', found the hub, and came back to '" + visit.ReturnedAreaName + "'."), list);
	}

	internal static SelfTestChecks.Outcome RoundTripPreservedState(List<string> before, List<string> after, string savedAs)
	{
		List<string> list = SaveStateFingerprint.Differences(before, after);
		List<string> list2 = new List<string> { $"Wrote '{savedAs}' and loaded it again. The mod held {before.Count} record(s) before and {after.Count} after." };
		list2.AddRange(list);
		return SelfTestChecks.Outcome.Of(list.Count == 0, (list.Count == 0) ? "Every per-save record this mod keeps survived a save and reload unchanged." : (list.Count + " per-save record(s) did not survive a save and reload."), list2);
	}

	internal static SelfTestChecks.Outcome CompanionConversationsOpen()
	{
		List<string> list = new List<string>();
		int num = 0;
		int num2 = 0;
		string[][] companionConversations = CompanionConversations;
		foreach (string[] array in companionConversations)
		{
			UnitEntityData unitEntityData = null;
			try
			{
				HashSet<UnitEntityData> present = new HashSet<UnitEntityData>(from u in Game.Instance.State.Units.Concat(Game.Instance.Player.Party)
					where u != null && u.IsInGame && !u.Suppressed
					select u);
				BlueprintGuid blueprint = BlueprintGuid.Parse(array[1]);
				unitEntityData = Game.Instance.Player.AllCharacters.FirstOrDefault((UnitEntityData u) => u != null && u.Blueprint != null && u.Blueprint.AssetGuid == blueprint && IsRecruited(u.Get<UnitPartCompanion>()?.State) && present.Contains(u));
			}
			catch (Exception ex)
			{
				list.Add(array[0] + ": finding the companion threw " + ex.GetType().Name + ": " + ex.Message);
				num2++;
				continue;
			}
			if (unitEntityData == null)
			{
				list.Add(array[0] + ": not a companion standing in this area, not played.");
				continue;
			}
			BlueprintDialog blueprintDialog = ResourcesLibrary.TryGetBlueprint<BlueprintDialog>(BlueprintGuid.Parse(array[2]));
			if (blueprintDialog == null)
			{
				list.Add(array[0] + ": the conversation " + array[2] + " did not resolve.");
				num2++;
				continue;
			}
			num++;
			try
			{
				CompanionConversationRules.Probe probe = PlayToFirstCue(blueprintDialog, out var firstCueName);
				string text = CompanionConversationRules.Problem(probe);
				string text2 = array[0] + ": " + blueprintDialog.name + ((!probe.ConditionsPass) ? " runs its ReplaceActions (its own conditions do not pass)" : (string.Format(" opens on {0}, {1} answer(s) to pick", firstCueName ?? "nothing", probe.SelectableAnswers) + ((probe.SkippedShowChecks > 0) ? $", {probe.SkippedShowChecks} with a skill check not asked" : "") + (probe.Continues ? ", continues" : "")));
				if (text != null)
				{
					num2++;
					text2 = text2 + ". FAIL: " + text + ".";
				}
				list.Add(text2);
			}
			catch (Exception ex2)
			{
				num2++;
				list.Add(array[0] + ": playing " + blueprintDialog.name + " to its first cue threw " + ex2.GetType().Name + ": " + ex2.Message + ". Thrown inside the window's cue handler, this leaves the conversation window blank.");
				list.Add("    " + ex2);
			}
		}
		return SelfTestChecks.Outcome.Of(num2 == 0, (num2 > 0) ? (num2 + " companion conversation(s) would open empty or broken.") : ((num == 0) ? "None of Wenduag, Lann, Seelah and Camellia is a companion standing in this area; nothing to play." : (num + " companion conversation(s) open on a cue with text and answers to pick.")), list);
	}

	private static bool IsRecruited(CompanionState? state)
	{
		if (state != CompanionState.InParty && state != CompanionState.Remote)
		{
			return state == CompanionState.InPartyDetached;
		}
		return true;
	}

	private static CompanionConversationRules.Probe PlayToFirstCue(BlueprintDialog dialog, out string firstCueName)
	{
		firstCueName = null;
		CompanionConversationRules.Probe probe = new CompanionConversationRules.Probe
		{
			ConditionsPass = (dialog.Conditions == null || dialog.Conditions.Check())
		};
		if (!probe.ConditionsPass)
		{
			return probe;
		}
		BlueprintCueBase blueprintCueBase = dialog.FirstCue?.Select();
		probe.FirstCueFound = blueprintCueBase != null;
		if (blueprintCueBase == null)
		{
			return probe;
		}
		firstCueName = blueprintCueBase.name;
		BlueprintCue blueprintCue = blueprintCueBase as BlueprintCue;
		probe.FirstCueIsPlainCue = blueprintCue != null;
		if (blueprintCue == null)
		{
			return probe;
		}
		probe.FirstCueHasText = !string.IsNullOrEmpty(blueprintCue.DisplayText);
		probe.Continues = blueprintCue.Continue?.Select() != null;
		if (probe.Continues)
		{
			return probe;
		}
		foreach (BlueprintAnswer item in OfferedAnswers(blueprintCue.Answers))
		{
			if (item.HasShowCheck)
			{
				probe.SkippedShowChecks++;
			}
			else if (item.CanShow() && item.CanSelect())
			{
				probe.SelectableAnswers++;
			}
		}
		return probe;
	}

	private static List<BlueprintAnswer> OfferedAnswers(IEnumerable<BlueprintAnswerBaseReference> references)
	{
		List<BlueprintAnswerBase> source = (from r in references ?? Enumerable.Empty<BlueprintAnswerBaseReference>()
			select r?.Get() into a
			where a != null
			select a).ToList();
		foreach (BlueprintAnswersList item in source.OfType<BlueprintAnswersList>())
		{
			if (item.CanSelect())
			{
				return OfferedAnswers(item.Answers);
			}
		}
		return source.OfType<BlueprintAnswer>().ToList();
	}
}
