using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using IsekaiMod.Content.Arenas;
using IsekaiMod.Content.Dialogue;
using IsekaiMod.Content.Quests;
using Kingmaker.AreaLogic.Etudes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Loot;
using Kingmaker.Blueprints.Quests;
using Kingmaker.Controllers.Dialog;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.Kingdom.Blueprints;
using Kingmaker.Kingdom.Settlements.BuildingComponents;
using Kingmaker.ResourceLinks;
using Kingmaker.UI.MVVM._VM.Dialog.Dialog;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestContentChecks
{
	private static readonly HashSet<BlueprintGuid> AnySpeaker;

	internal static SelfTestChecks.Outcome DialogueGraph()
	{
		List<string> list = new List<string>();
		List<SelfTestRecorder.CreatedBlueprint> source = SelfTestProgressionChecks.ModBlueprints();
		List<BlueprintDialog> list2 = source.Select((SelfTestRecorder.CreatedBlueprint c) => c.Blueprint).OfType<BlueprintDialog>().ToList();
		List<SimpleBlueprint> list3 = new List<SimpleBlueprint>();
		list3.AddRange(list2);
		list3.AddRange(source.Select((SelfTestRecorder.CreatedBlueprint c) => c.Blueprint).OfType<BlueprintCueBase>());
		list3.AddRange(source.Select((SelfTestRecorder.CreatedBlueprint c) => c.Blueprint).OfType<BlueprintAnswerBase>());
		if (list3.Count == 0)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The mod created no dialogs, cues or answers, which cannot be right.");
		}
		HashSet<BlueprintGuid> hashSet = new HashSet<BlueprintGuid>();
		Queue<SimpleBlueprint> queue = new Queue<SimpleBlueprint>(list3);
		foreach (SimpleBlueprint item in list3)
		{
			hashSet.Add(item.AssetGuid);
		}
		int num = 0;
		int num2 = 0;
		int evaluated = 0;
		int num3 = 0;
		List<string> list4 = new List<string>();
		List<SimpleBlueprint> list5 = new List<SimpleBlueprint>();
		while (queue.Count > 0)
		{
			SimpleBlueprint simpleBlueprint = queue.Dequeue();
			list5.Add(simpleBlueprint);
			num3 += EvaluateConditions(simpleBlueprint, list, list4, ref evaluated);
			if (simpleBlueprint is BlueprintDialog blueprintDialog)
			{
				num3 += FollowCueSelection(blueprintDialog.name, "FirstCue", blueprintDialog.FirstCue, fromAnswer: false, list, hashSet, queue);
			}
			else if (simpleBlueprint is BlueprintCue blueprintCue)
			{
				num++;
				num3 += FollowAnswers(blueprintCue.name, blueprintCue.Answers, list, hashSet, queue);
				num3 += FollowCueSelection(blueprintCue.name, "Continue", blueprintCue.Continue, fromAnswer: false, list, hashSet, queue);
			}
			else if (simpleBlueprint is BlueprintCueSequence blueprintCueSequence)
			{
				num++;
				num3 += FollowCues(blueprintCueSequence.name, "Cues", blueprintCueSequence.Cues, list, hashSet, queue);
				Enqueue(blueprintCueSequence.Exit, hashSet, queue);
			}
			else if (simpleBlueprint is BlueprintCheck blueprintCheck)
			{
				num++;
				Enqueue(blueprintCheck.Success, hashSet, queue);
				Enqueue(blueprintCheck.Fail, hashSet, queue);
			}
			else if (simpleBlueprint is BlueprintAnswer blueprintAnswer)
			{
				num2++;
				num3 += FollowCueSelection(blueprintAnswer.name, "NextCue", blueprintAnswer.NextCue, fromAnswer: true, list, hashSet, queue);
			}
			else if (simpleBlueprint is BlueprintAnswersList blueprintAnswersList)
			{
				num3 += FollowAnswers(blueprintAnswersList.name, blueprintAnswersList.Answers, list, hashSet, queue);
			}
		}
		HashSet<BlueprintGuid> modOwned = new HashSet<BlueprintGuid>(from c in source
			select c.Blueprint into b
			where b != null
			select b.AssetGuid);
		num3 += StrandedCycleFailures(list5, modOwned, list);
		num3 += RefusableSelectionFailures(list5, modOwned, list);
		num3 += GraftGateFailures(list5, modOwned, list, out var pinned, out var hubs);
		List<string> list6 = new List<string>();
		num3 += NoReplyAnswerFailures(list5, modOwned, list, list6);
		if (list6.Count > 0)
		{
			list.AddRange(SelfTestReport.WrapList($"Allowed, not a failure: {list6.Count} answers the walk crossed into are the game's own, and the mod neither " + "built them nor added a cue to their reply. The game's own writers rely on those conditions holding, and the mod must not rewrite vanilla wiring to satisfy a mod rule:", list6));
		}
		if (list4.Count > 0)
		{
			list.AddRange(SelfTestReport.WrapList($"Allowed, not a failure: {list4.Count} conditions could not be evaluated because there is no loaded game at the main menu (no main character, party or inventory to ask):", from n in list4
				group n by n into g
				orderby g.Count() descending
				select $"{g.Key} x{g.Count()}"));
		}
		int num4 = DialogueGuardFailures(list);
		num3 += num4;
		string summary = $"{list2.Count} mod dialogs walked with every mod cue and answer as a root: {num} cues, {num2} answers, " + $"{evaluated - list4.Count} of {evaluated} conditions evaluated (the rest need a loaded game); " + $"{pinned} grafted answers pinned to their presenting cues and {hubs} on declared topic hubs; " + "the dialogue softlock guard is on every one of its four targets; " + $"{num3} problems.";
		return SelfTestChecks.Outcome.Of(num3 == 0, summary, list);
	}

	private static int GraftGateFailures(List<SimpleBlueprint> visited, HashSet<BlueprintGuid> modOwned, List<string> details, out int pinned, out int hubs)
	{
		pinned = 0;
		hubs = 0;
		int num = 0;
		Dictionary<string, GraftedAnswer.Record> dictionary = GraftedAnswer.Grafts.Where((GraftedAnswer.Record r) => r.Answer != null && r.Host != null).ToDictionary((GraftedAnswer.Record r) => r.Answer.AssetGuid.ToString() + "|" + r.Host.AssetGuid.ToString(), (GraftedAnswer.Record r) => r);
		HashSet<BlueprintAnswersList> hashSet = new HashSet<BlueprintAnswersList>(from r in GraftedAnswer.Grafts
			select r.Host into h
			where h != null
			select h);
		hashSet.UnionWith(from l in visited.OfType<BlueprintAnswersList>()
			where !modOwned.Contains(l.AssetGuid)
			select l);
		foreach (BlueprintAnswersList item in hashSet.Where((BlueprintAnswersList l) => !modOwned.Contains(l.AssetGuid)))
		{
			foreach (BlueprintAnswer answer in (item.Answers ?? new List<BlueprintAnswerBaseReference>()).Select((BlueprintAnswerBaseReference r) => r?.Get()).OfType<BlueprintAnswer>())
			{
				if (!modOwned.Contains(answer.AssetGuid))
				{
					continue;
				}
				dictionary.TryGetValue(answer.AssetGuid.ToString() + "|" + item.AssetGuid.ToString(), out var value);
				if (value == null)
				{
					num++;
					details.Add($"Graft: {answer.name} is in vanilla list {item.name} ({item.AssetGuid}) but GraftedAnswer did not put it there, so nothing pins it to a beat.");
					continue;
				}
				if (value.Beat.Hub)
				{
					hubs++;
					continue;
				}
				List<ConditionCurrentCueIs> list = (answer.ShowConditions?.Conditions ?? new Condition[0]).OfType<ConditionCurrentCueIs>().ToList();
				List<BlueprintCueBaseReference> list2 = list.SelectMany((ConditionCurrentCueIs g) => g.m_Cues ?? new BlueprintCueBaseReference[0]).ToList();
				if (list.Count == 0 || list2.Count == 0)
				{
					num++;
					details.Add("Graft: " + answer.name + " in " + item.name + " (" + value.Beat.Name + ") has no presenting-cue gate, so every cue that presents the list offers it.");
					continue;
				}
				List<BlueprintAnswersList> list3 = (from r in GraftedAnswer.Grafts
					where r.Answer == answer && r.Host != null
					select r.Host).ToList();
				List<string> list4 = new List<string>();
				foreach (BlueprintCueBaseReference item2 in list2)
				{
					BlueprintCueBase cue = item2?.Get();
					if (cue == null)
					{
						list4.Add($"{((BlueprintReferenceBase)item2)?.deserializedGuid} (does not resolve)");
					}
					else if (!list3.Any((BlueprintAnswersList h) => GraftedAnswer.BeatGates.Presents(cue, h) || GraftedAnswer.BeatGates.IsContinuationCue(cue, h)))
					{
						list4.Add($"{cue.name} ({cue.AssetGuid}) does not present the list" + ((list3.Count > 1) ? " or any other list the answer is grafted onto" : ""));
					}
				}
				if (list4.Count > 0)
				{
					num++;
					details.AddRange(SelfTestReport.WrapList("Graft: " + answer.name + " in " + item.name + " (" + value.Beat.Name + ") is pinned to cues that cannot offer it:", list4));
				}
				else
				{
					pinned++;
				}
			}
		}
		return num;
	}

	private static int DialogueGuardFailures(List<string> details)
	{
		(Type, string)[] obj = new(Type, string)[4]
		{
			(typeof(DialogVM), "HandleOnCueShow"),
			(typeof(DialogController), "PlayCue"),
			(typeof(BlueprintAnswer), "CanSelect"),
			(typeof(BlueprintAnswer), "CanShow")
		};
		int num = 0;
		(Type, string)[] array = obj;
		for (int i = 0; i < array.Length; i++)
		{
			(Type, string) tuple = array[i];
			try
			{
				MethodInfo methodInfo = AccessTools.Method(tuple.Item1, tuple.Item2);
				if (methodInfo == null)
				{
					details.Add("Dialogue guard target " + tuple.Item1.Name + "." + tuple.Item2 + " no longer exists in this build of the game.");
					num++;
					continue;
				}
				Patches patchInfo = Harmony.GetPatchInfo(methodInfo);
				if (patchInfo == null || patchInfo.Finalizers == null || !patchInfo.Finalizers.Any((Patch p) => p.PatchMethod != null && p.PatchMethod.DeclaringType == typeof(DialogueSoftlockGuardPatch)))
				{
					details.Add("Dialogue guard is NOT applied to " + tuple.Item1.Name + "." + tuple.Item2 + ": a throw there would leave a blank dialogue window.");
					num++;
				}
			}
			catch (Exception ex)
			{
				details.Add("Dialogue guard on " + tuple.Item1.Name + "." + tuple.Item2 + " could not be verified: " + ex.Message);
				num++;
			}
		}
		return num;
	}

	private static int RefusableSelectionFailures(List<SimpleBlueprint> visited, HashSet<BlueprintGuid> modOwned, List<string> details)
	{
		int num = 0;
		foreach (SimpleBlueprint item in visited)
		{
			if (!modOwned.Contains(item.AssetGuid))
			{
				continue;
			}
			if (item is BlueprintDialog blueprintDialog)
			{
				if (SelectionCanRefuse(blueprintDialog.FirstCue, AnySpeaker))
				{
					num++;
					details.Add($"Dialog '{blueprintDialog.name}' names {blueprintDialog.FirstCue.Cues.Count} first cue(s) and every one of them can refuse " + "(" + DescribeRefusals(blueprintDialog.FirstCue, AnySpeaker) + "), so the conversation can open no window at all.");
				}
			}
			else
			{
				if (!(item is BlueprintCue blueprintCue))
				{
					continue;
				}
				bool flag = (blueprintCue.Answers?.Count ?? 0) > 0;
				bool flag2 = (blueprintCue.Continue?.Cues?.Count).GetValueOrDefault() > 0;
				if (flag2)
				{
					HashSet<BlueprintGuid> alreadyPresent = SpeakerOf(blueprintCue);
					bool flag3 = SelectionCanRefuse(blueprintCue.Continue, alreadyPresent);
					if (flag3 && !flag && ContinuationCarriesActions(blueprintCue.Continue))
					{
						num++;
						details.Add($"'{blueprintCue.name}' continues into {blueprintCue.Continue.Cues.Count} cue(s) and every one of them can refuse " + "(" + DescribeRefusals(blueprintCue.Continue, alreadyPresent) + "), the cue offers no answers to fall back on, and the continuation carries actions, so the scene can end with those actions never run. Add a speakerless, unconditional twin carrying the same actions as the last entry of the chain.");
					}
					else if (SelfTestRuntimeRules.ContinueHidesAnswers(flag, flag2, flag3))
					{
						num++;
						details.Add($"'{blueprintCue.name}' has {blueprintCue.Answers.Count} answer(s) and a Continue that can never refuse, so the engine always " + "replaces those answers with the single Continue answer and the player can never see them.");
					}
				}
			}
		}
		return num;
	}

	private static bool ContinuationCarriesActions(CueSelection selection)
	{
		foreach (BlueprintCueBaseReference item in selection.Cues ?? new List<BlueprintCueBaseReference>())
		{
			if (item != null && !(item.Guid == BlueprintGuid.Empty) && Resolve<BlueprintCueBase>(item.Guid, out var _) is BlueprintCue blueprintCue && ((blueprintCue.OnShow?.Actions?.Length).GetValueOrDefault() > 0 || (blueprintCue.OnStop?.Actions?.Length).GetValueOrDefault() > 0))
			{
				return true;
			}
		}
		return false;
	}

	private static bool NamesAnUnprovenSpeaker(BlueprintCue cue, HashSet<BlueprintGuid> alreadyPresent)
	{
		BlueprintUnitReference blueprintUnitReference = cue?.Speaker?.m_Blueprint;
		if (blueprintUnitReference == null || blueprintUnitReference.Guid == BlueprintGuid.Empty)
		{
			return false;
		}
		if (alreadyPresent == null)
		{
			return false;
		}
		return !alreadyPresent.Contains(blueprintUnitReference.Guid);
	}

	private static HashSet<BlueprintGuid> SpeakerOf(BlueprintCue cue)
	{
		HashSet<BlueprintGuid> hashSet = new HashSet<BlueprintGuid>();
		BlueprintUnitReference blueprintUnitReference = cue?.Speaker?.m_Blueprint;
		if (blueprintUnitReference != null && blueprintUnitReference.Guid != BlueprintGuid.Empty)
		{
			hashSet.Add(blueprintUnitReference.Guid);
		}
		return hashSet;
	}

	private static bool SelectionCanRefuse(CueSelection selection, HashSet<BlueprintGuid> alreadyPresent)
	{
		if (selection == null)
		{
			return false;
		}
		return SelfTestRuntimeRules.CueSelectionCanRefuseEntirely(from pair in RefusalFlags(selection, alreadyPresent)
			select pair.Value);
	}

	private static string DescribeRefusals(CueSelection selection, HashSet<BlueprintGuid> alreadyPresent)
	{
		return string.Join("; ", from pair in RefusalFlags(selection, alreadyPresent)
			select pair.Key);
	}

	private static List<KeyValuePair<string, bool>> RefusalFlags(CueSelection selection, HashSet<BlueprintGuid> alreadyPresent)
	{
		List<KeyValuePair<string, bool>> list = new List<KeyValuePair<string, bool>>();
		foreach (BlueprintCueBaseReference item in selection.Cues ?? new List<BlueprintCueBaseReference>())
		{
			if (item == null || item.Guid == BlueprintGuid.Empty)
			{
				continue;
			}
			BlueprintCueBase blueprintCueBase = Resolve<BlueprintCueBase>(item.Guid, out var _);
			if (blueprintCueBase != null)
			{
				bool showOnce = blueprintCueBase.ShowOnce;
				bool flag = (blueprintCueBase.Conditions?.Conditions?.Length).GetValueOrDefault() > 0;
				bool flag2 = NamesAnUnprovenSpeaker(blueprintCueBase as BlueprintCue, alreadyPresent);
				bool flag3 = SelfTestRuntimeRules.CueCanRefuse(showOnce, flag, flag2);
				List<string> list2 = new List<string>();
				if (showOnce)
				{
					list2.Add("shown once");
				}
				if (flag)
				{
					list2.Add("conditions");
				}
				if (flag2)
				{
					list2.Add("names a speaker");
				}
				list.Add(new KeyValuePair<string, bool>(blueprintCueBase.name + " " + (flag3 ? string.Join("+", list2) : "always shows"), flag3));
			}
		}
		return list;
	}

	private static int StrandedCycleFailures(List<SimpleBlueprint> visited, HashSet<BlueprintGuid> modOwned, List<string> details)
	{
		Dictionary<BlueprintGuid, List<BlueprintGuid>> dictionary = new Dictionary<BlueprintGuid, List<BlueprintGuid>>();
		foreach (SimpleBlueprint item in visited)
		{
			List<BlueprintGuid> list = new List<BlueprintGuid>();
			if (item is BlueprintDialog blueprintDialog)
			{
				AddCueTargets(blueprintDialog.FirstCue, list);
			}
			else if (item is BlueprintCue blueprintCue)
			{
				AddAnswerTargets(blueprintCue.Answers, list);
				AddCueTargets(blueprintCue.Continue, list);
			}
			else if (item is BlueprintCueSequence blueprintCueSequence)
			{
				AddReferences(blueprintCueSequence.Cues, list);
				if (blueprintCueSequence.Exit != null)
				{
					list.Add(blueprintCueSequence.Exit.AssetGuid);
				}
			}
			else if (item is BlueprintCheck blueprintCheck)
			{
				if (blueprintCheck.Success != null)
				{
					list.Add(blueprintCheck.Success.AssetGuid);
				}
				if (blueprintCheck.Fail != null)
				{
					list.Add(blueprintCheck.Fail.AssetGuid);
				}
			}
			else if (item is BlueprintAnswer blueprintAnswer)
			{
				AddCueTargets(blueprintAnswer.NextCue, list);
			}
			else if (item is BlueprintAnswersList blueprintAnswersList)
			{
				AddAnswerTargets(blueprintAnswersList.Answers, list);
			}
			dictionary[item.AssetGuid] = list;
		}
		int num = 0;
		foreach (BlueprintCue item2 in visited.OfType<BlueprintCue>())
		{
			bool onCycle = Reaches(dictionary, item2.AssetGuid, item2.AssetGuid);
			bool hasContinue = (item2.Continue?.Cues?.Count).GetValueOrDefault() > 0;
			List<bool> list2 = new List<bool>();
			List<KeyValuePair<string, bool>> list3 = new List<KeyValuePair<string, bool>>();
			bool flag = !modOwned.Contains(item2.AssetGuid);
			foreach (BlueprintAnswer item3 in OfferedAnswers(item2.Answers))
			{
				list2.Add(IsAlwaysOffered(item3));
				list3.Add(SoleConditionSignature(item3));
				if (!modOwned.Contains(item3.AssetGuid))
				{
					flag = true;
				}
			}
			if (flag && !SelfTestRuntimeRules.ComplementaryPairAlwaysOffers(list3) && SelfTestRuntimeRules.CycleNodeCanStrand(onCycle, hasContinue, list2))
			{
				num++;
				details.Add($"'{item2.name}' is on a cycle, has no continuation, and every one of its {list2.Count} answers " + "is shown once or carries a show condition, so a player who works through them is left with nothing to click and the conversation ends there. Gates: " + string.Join(" | ", list3.Select((KeyValuePair<string, bool> s) => (s.Key != null) ? ((s.Value ? "NOT " : "") + s.Key) : "(not a single plain gate)")));
			}
		}
		return num;
	}

	private static int NoReplyAnswerFailures(List<SimpleBlueprint> visited, HashSet<BlueprintGuid> modOwned, List<string> details, List<string> vanillaNotes)
	{
		Dictionary<BlueprintGuid, List<string>> dictionary = new Dictionary<BlueprintGuid, List<string>>();
		Dictionary<BlueprintGuid, HashSet<BlueprintGuid>> dictionary2 = new Dictionary<BlueprintGuid, HashSet<BlueprintGuid>>();
		foreach (BlueprintCue item in visited.OfType<BlueprintCue>())
		{
			foreach (BlueprintAnswer item2 in OfferedAnswers(item.Answers))
			{
				if (!dictionary.TryGetValue(item2.AssetGuid, out var value))
				{
					value = (dictionary[item2.AssetGuid] = new List<string>());
				}
				if (!value.Contains(item.name))
				{
					value.Add(item.name);
				}
				if (!dictionary2.TryGetValue(item2.AssetGuid, out var value2))
				{
					value2 = (dictionary2[item2.AssetGuid] = new HashSet<BlueprintGuid>());
				}
				value2.UnionWith(SpeakerOf(item));
			}
		}
		int num = 0;
		foreach (BlueprintAnswer item3 in visited.OfType<BlueprintAnswer>())
		{
			dictionary2.TryGetValue(item3.AssetGuid, out var value3);
			List<bool> list2 = new List<bool>();
			List<string> list3 = new List<string>();
			bool modOwnsAnyReplyCue = false;
			foreach (BlueprintCueBaseReference item4 in item3.NextCue?.Cues ?? new List<BlueprintCueBaseReference>())
			{
				if (item4 == null || item4.Guid == BlueprintGuid.Empty)
				{
					continue;
				}
				BlueprintCueBase blueprintCueBase = Resolve<BlueprintCueBase>(item4.Guid, out var _);
				if (blueprintCueBase != null)
				{
					list2.Add(SelfTestRuntimeRules.CueCanRefuse(blueprintCueBase.ShowOnce, (blueprintCueBase.Conditions?.Conditions?.Length).GetValueOrDefault() > 0, NamesAnUnprovenSpeaker(blueprintCueBase as BlueprintCue, value3 ?? new HashSet<BlueprintGuid>())));
					list3.Add(blueprintCueBase.name);
					if (modOwned.Contains(blueprintCueBase.AssetGuid))
					{
						modOwnsAnyReplyCue = true;
					}
				}
			}
			if (SelfTestRuntimeRules.AnswerMayResolveNoReply(item3.RequireValidCue, list2))
			{
				string arg = ((dictionary.TryGetValue(item3.AssetGuid, out var value4) && value4.Count > 0) ? string.Join(", ", value4) : "no cue the walk visited");
				string text = $"'{item3.name}' ({item3.AssetGuid}), offered by {arg}, replies only with " + $"{list3.Count} cue(s) that can each refuse - conditions, shown once or a named speaker - " + "(" + string.Join(", ", list3) + ")";
				if (SelfTestRuntimeRules.AnswerNoReplyIsModsToFix(modOwned.Contains(item3.AssetGuid), modOwnsAnyReplyCue, item3.RequireValidCue, list2))
				{
					num++;
					details.Add(text + " and sets no RequireValidCue, so a click can resolve to no cue at all, and the engine ends the conversation with no reply.");
				}
				else
				{
					vanillaNotes.Add(text + ".");
				}
			}
		}
		return num;
	}

	private static IEnumerable<BlueprintAnswer> OfferedAnswers(List<BlueprintAnswerBaseReference> answers)
	{
		foreach (BlueprintAnswerBaseReference item in answers ?? new List<BlueprintAnswerBaseReference>())
		{
			if (item == null || item.Guid == BlueprintGuid.Empty)
			{
				continue;
			}
			BlueprintAnswerBase blueprintAnswerBase = Resolve<BlueprintAnswerBase>(item.Guid, out var problem);
			if (blueprintAnswerBase is BlueprintAnswer blueprintAnswer)
			{
				yield return blueprintAnswer;
			}
			else
			{
				if (!(blueprintAnswerBase is BlueprintAnswersList blueprintAnswersList))
				{
					continue;
				}
				foreach (BlueprintAnswerBaseReference item2 in blueprintAnswersList.Answers ?? new List<BlueprintAnswerBaseReference>())
				{
					if (item2 != null && !(item2.Guid == BlueprintGuid.Empty) && Resolve<BlueprintAnswerBase>(item2.Guid, out problem) is BlueprintAnswer blueprintAnswer2)
					{
						yield return blueprintAnswer2;
					}
				}
			}
		}
	}

	private static KeyValuePair<string, bool> SoleConditionSignature(BlueprintAnswer answer)
	{
		KeyValuePair<string, bool> result = new KeyValuePair<string, bool>(null, value: false);
		if (answer == null || answer.ShowOnce || answer.DebugMode || answer.RequireValidCue || answer.HasShowCheck)
		{
			return result;
		}
		if ((answer.SelectConditions?.Conditions?.Length).GetValueOrDefault() != 0)
		{
			return result;
		}
		Condition[] array = answer.ShowConditions?.Conditions;
		if (array == null || array.Length != 1 || array[0] == null)
		{
			return result;
		}
		Condition condition = array[0];
		List<string> list = new List<string> { condition.GetType().FullName };
		Type type = condition.GetType();
		while (type != null && type != typeof(Element))
		{
			FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (!(fieldInfo.Name == "Not") && !fieldInfo.Name.Contains("LastResult"))
				{
					object value = fieldInfo.GetValue(condition);
					if (value is Element)
					{
						return result;
					}
					list.Add(fieldInfo.Name + "=" + ((value is BlueprintReferenceBase { Guid: var guid }) ? guid.ToString() : value?.ToString()));
				}
			}
			type = type.BaseType;
		}
		return new KeyValuePair<string, bool>(string.Join(";", list), condition.Not);
	}

	private static bool IsAlwaysOffered(BlueprintAnswer answer)
	{
		if (answer == null)
		{
			return false;
		}
		if (!answer.ShowOnce && !answer.DebugMode && !answer.RequireValidCue && !answer.HasShowCheck && (answer.ShowConditions?.Conditions?.Length).GetValueOrDefault() == 0)
		{
			return (answer.SelectConditions?.Conditions?.Length).GetValueOrDefault() == 0;
		}
		return false;
	}

	private static void AddCueTargets(CueSelection selection, List<BlueprintGuid> into)
	{
		if (selection != null)
		{
			AddReferences(selection.Cues, into);
		}
	}

	private static void AddReferences(List<BlueprintCueBaseReference> references, List<BlueprintGuid> into)
	{
		foreach (BlueprintCueBaseReference item in references ?? new List<BlueprintCueBaseReference>())
		{
			if (item != null && item.Guid != BlueprintGuid.Empty)
			{
				into.Add(item.Guid);
			}
		}
	}

	private static void AddAnswerTargets(List<BlueprintAnswerBaseReference> answers, List<BlueprintGuid> into)
	{
		foreach (BlueprintAnswerBaseReference item in answers ?? new List<BlueprintAnswerBaseReference>())
		{
			if (item != null && item.Guid != BlueprintGuid.Empty)
			{
				into.Add(item.Guid);
			}
		}
	}

	private static bool Reaches(Dictionary<BlueprintGuid, List<BlueprintGuid>> edges, BlueprintGuid from, BlueprintGuid to)
	{
		HashSet<BlueprintGuid> hashSet = new HashSet<BlueprintGuid>();
		Queue<BlueprintGuid> queue = new Queue<BlueprintGuid>();
		if (edges.TryGetValue(from, out var value))
		{
			foreach (BlueprintGuid item in value)
			{
				if (hashSet.Add(item))
				{
					queue.Enqueue(item);
				}
			}
		}
		while (queue.Count > 0)
		{
			BlueprintGuid blueprintGuid = queue.Dequeue();
			if (blueprintGuid == to)
			{
				return true;
			}
			if (!edges.TryGetValue(blueprintGuid, out var value2))
			{
				continue;
			}
			foreach (BlueprintGuid item2 in value2)
			{
				if (hashSet.Add(item2))
				{
					queue.Enqueue(item2);
				}
			}
		}
		return false;
	}

	private static void Enqueue(SimpleBlueprint blueprint, HashSet<BlueprintGuid> seen, Queue<SimpleBlueprint> queue)
	{
		if (blueprint != null && seen.Add(blueprint.AssetGuid))
		{
			queue.Enqueue(blueprint);
		}
	}

	private static int FollowCues(string owner, string field, List<BlueprintCueBaseReference> cues, List<string> details, HashSet<BlueprintGuid> seen, Queue<SimpleBlueprint> queue)
	{
		int num = 0;
		for (int i = 0; i < (cues?.Count ?? 0); i++)
		{
			BlueprintCueBaseReference blueprintCueBaseReference = cues[i];
			if (blueprintCueBaseReference == null || blueprintCueBaseReference.Guid == BlueprintGuid.Empty)
			{
				num++;
				details.Add($"{owner}.{field}[{i}] is an empty reference, so the branch leads nowhere.");
				continue;
			}
			BlueprintCueBase blueprintCueBase = Resolve<BlueprintCueBase>(blueprintCueBaseReference.Guid, out var problem);
			if (blueprintCueBase == null)
			{
				num++;
				details.Add($"{owner}.{field}[{i}]: {problem}.");
			}
			else
			{
				Enqueue(blueprintCueBase, seen, queue);
			}
		}
		return num;
	}

	private static int FollowCueSelection(string owner, string field, CueSelection selection, bool fromAnswer, List<string> details, HashSet<BlueprintGuid> seen, Queue<SimpleBlueprint> queue)
	{
		if (selection == null)
		{
			return 0;
		}
		int num = FollowCues(owner, field, selection.Cues, details, seen, queue);
		if (!fromAnswer)
		{
			return num;
		}
		foreach (BlueprintCueBaseReference item in selection.Cues ?? new List<BlueprintCueBaseReference>())
		{
			if (item != null && !(item.Guid == BlueprintGuid.Empty) && Resolve<BlueprintCueBase>(item.Guid, out var _) is BlueprintCue blueprintCue)
			{
				bool hasText = !string.IsNullOrEmpty(blueprintCue.Text?.ToString());
				bool hasAnswers = (blueprintCue.Answers?.Count ?? 0) > 0;
				bool hasContinue = (blueprintCue.Continue?.Cues?.Count).GetValueOrDefault() > 0;
				bool hasOnShowActions = (blueprintCue.OnShow?.Actions?.Length).GetValueOrDefault() > 0;
				if (SelfTestRuntimeRules.IsEmptyNode(hasText, hasAnswers, hasContinue, hasOnShowActions))
				{
					num++;
					details.Add(owner + " leads to '" + blueprintCue.name + "', which has no text, no answers, no continuation and no actions, so the player is shown an empty window.");
				}
			}
		}
		return num;
	}

	private static int FollowAnswers(string owner, List<BlueprintAnswerBaseReference> answers, List<string> details, HashSet<BlueprintGuid> seen, Queue<SimpleBlueprint> queue)
	{
		int num = 0;
		for (int i = 0; i < (answers?.Count ?? 0); i++)
		{
			BlueprintAnswerBaseReference blueprintAnswerBaseReference = answers[i];
			if (blueprintAnswerBaseReference == null || blueprintAnswerBaseReference.Guid == BlueprintGuid.Empty)
			{
				num++;
				details.Add($"{owner}.Answers[{i}] is an empty reference, so an answer is missing from the list.");
				continue;
			}
			BlueprintAnswerBase blueprintAnswerBase = Resolve<BlueprintAnswerBase>(blueprintAnswerBaseReference.Guid, out var problem);
			if (blueprintAnswerBase == null)
			{
				num++;
				details.Add($"{owner}.Answers[{i}]: {problem}.");
			}
			else
			{
				Enqueue(blueprintAnswerBase, seen, queue);
			}
		}
		return num;
	}

	private static T Resolve<T>(BlueprintGuid guid, out string problem) where T : SimpleBlueprint
	{
		problem = null;
		SimpleBlueprint simpleBlueprint;
		try
		{
			simpleBlueprint = ResourcesLibrary.TryGetBlueprint(guid);
		}
		catch (Exception ex)
		{
			problem = $"{guid} could not be looked up ({ex.GetType().Name})";
			return null;
		}
		if (simpleBlueprint == null)
		{
			problem = $"{guid} does not resolve to any blueprint";
			return null;
		}
		if (!(simpleBlueprint is T result))
		{
			problem = $"{guid} is '{simpleBlueprint.name}', a {simpleBlueprint.GetType().Name}, but a {typeof(T).Name} is needed here";
			return null;
		}
		return result;
	}

	private static int EvaluateConditions(SimpleBlueprint node, List<string> details, List<string> needGame, ref int evaluated)
	{
		int num = 0;
		foreach (KeyValuePair<string, ConditionsChecker> item in ConditionsOf(node))
		{
			if (item.Value?.Conditions == null)
			{
				continue;
			}
			Condition[] conditions = item.Value.Conditions;
			foreach (Condition condition in conditions)
			{
				if (condition != null)
				{
					evaluated++;
					num += UnresolvedReferencesIn(node.name + "." + item.Key, condition, details);
					try
					{
						condition.Check();
					}
					catch (Exception ex)
					{
						needGame.Add(condition.GetType().Name + " (" + ex.GetType().Name + ")");
					}
				}
			}
		}
		return num;
	}

	private static int UnresolvedReferencesIn(string owner, object element, List<string> details)
	{
		List<BlueprintReferenceWalker.Found> list;
		try
		{
			list = BlueprintReferenceWalker.ReferencesInObject(element);
		}
		catch
		{
			return 0;
		}
		int num = 0;
		foreach (BlueprintReferenceWalker.Found item in list)
		{
			if (item.Reference != null && !(item.Reference.Guid == BlueprintGuid.Empty))
			{
				SimpleBlueprint simpleBlueprint;
				try
				{
					simpleBlueprint = ResourcesLibrary.TryGetBlueprint(item.Reference.Guid);
				}
				catch
				{
					simpleBlueprint = null;
				}
				if (simpleBlueprint == null)
				{
					num++;
					details.Add($"{owner}: {element.GetType().Name}.{item.Path} names {item.Reference.Guid}, which does not resolve.");
				}
			}
		}
		return num;
	}

	private static IEnumerable<KeyValuePair<string, ConditionsChecker>> ConditionsOf(SimpleBlueprint node)
	{
		if (node is BlueprintCueBase blueprintCueBase)
		{
			yield return Pair("Conditions", blueprintCueBase.Conditions);
		}
		if (node is BlueprintDialog blueprintDialog)
		{
			yield return Pair("Conditions", blueprintDialog.Conditions);
		}
		if (node is BlueprintAnswer answer)
		{
			yield return Pair("ShowConditions", answer.ShowConditions);
			yield return Pair("SelectConditions", answer.SelectConditions);
		}
		if (node is BlueprintAnswersList blueprintAnswersList)
		{
			yield return Pair("Conditions", blueprintAnswersList.Conditions);
		}
	}

	private static KeyValuePair<string, ConditionsChecker> Pair(string key, ConditionsChecker value)
	{
		return new KeyValuePair<string, ConditionsChecker>(key, value);
	}

	internal static SelfTestChecks.Outcome QuestsAndEtudes()
	{
		List<string> list = new List<string>();
		List<SelfTestRecorder.CreatedBlueprint> source = SelfTestProgressionChecks.ModBlueprints();
		List<BlueprintQuest> list2 = source.Select((SelfTestRecorder.CreatedBlueprint c) => c.Blueprint).OfType<BlueprintQuest>().ToList();
		List<BlueprintQuestObjective> list3 = source.Select((SelfTestRecorder.CreatedBlueprint c) => c.Blueprint).OfType<BlueprintQuestObjective>().ToList();
		List<BlueprintEtude> list4 = source.Select((SelfTestRecorder.CreatedBlueprint c) => c.Blueprint).OfType<BlueprintEtude>().ToList();
		if (list2.Count == 0 && list3.Count == 0 && list4.Count == 0)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The mod created no quests, objectives or etudes, which cannot be right.");
		}
		int num = 0;
		foreach (BlueprintQuest item in list2)
		{
			List<BlueprintQuestObjective> list5 = item.AllObjectives.ToList();
			if (list5.Count == 0)
			{
				num++;
				list.Add(item.name + " lists no objectives, so it can never progress.");
			}
			if (list5.Any((BlueprintQuestObjective o) => o == null))
			{
				num++;
				list.Add($"{item.name} lists {list5.Count((BlueprintQuestObjective o) => o == null)} objectives that do not resolve.");
			}
			List<BlueprintQuestObjective> source2 = list5.Where((BlueprintQuestObjective o) => o != null).ToList();
			foreach (string item2 in SelfTestRuntimeRules.UnreachableObjectives(next: source2.ToDictionary((BlueprintQuestObjective o) => o.name, (BlueprintQuestObjective o) => (from n in o.NextObjectives
				where n != null
				select n.name).ToList(), StringComparer.Ordinal), addendums: source2.ToDictionary((BlueprintQuestObjective o) => o.name, (BlueprintQuestObjective o) => (from n in o.Addendums
				where n != null
				select n.name).ToList(), StringComparer.Ordinal), listed: source2.Select((BlueprintQuestObjective o) => o.name)))
			{
				num++;
				list.Add(item.name + ": nothing in the quest opens the objective '" + item2 + "'.");
			}
		}
		List<string> list6 = new List<string>();
		HashSet<BlueprintGuid> hashSet = ObjectivesSomethingCanComplete(out var byBlueprint, out var byCode, list6);
		num += list6.Count;
		list.AddRange(list6);
		foreach (BlueprintQuestObjective objective in list3)
		{
			BlueprintQuest quest = objective.Quest;
			if (quest == null)
			{
				num++;
				list.Add(objective.name + " names no quest, so it can never be shown.");
			}
			else if (!quest.AllObjectives.Any((BlueprintQuestObjective o) => o != null && o.AssetGuid == objective.AssetGuid))
			{
				num++;
				list.Add(objective.name + " says it belongs to '" + quest.name + "', but that quest does not list it.");
			}
			num += UnresolvedInList(objective.name, "NextObjectives", objective.NextObjectives, list);
			num += UnresolvedInList(objective.name, "Addendums", objective.Addendums, list);
			if (!hashSet.Contains(objective.AssetGuid))
			{
				num++;
				list.Add(objective.name + ": nothing can complete it. No mod blueprint element completes it, and no code that calls QuestBook.CompleteObjective reads a field that holds it.");
			}
		}
		foreach (BlueprintEtude item3 in list4)
		{
			num += CheckEtude(item3, list);
		}
		string summary = $"{list2.Count} quests, {list3.Count} objectives and {list4.Count} etudes; {byBlueprint} objectives completed by a mod blueprint element" + $" and {byCode} by code that calls CompleteObjective; {num} problems.";
		return SelfTestChecks.Outcome.Of(num == 0, summary, list);
	}

	private static int UnresolvedInList(string owner, string field, IList<BlueprintQuestObjective> list, List<string> details)
	{
		int num = 0;
		for (int i = 0; i < (list?.Count ?? 0); i++)
		{
			if (list[i] == null)
			{
				num++;
				details.Add($"{owner}.{field}[{i}] does not resolve.");
			}
		}
		return num;
	}

	private static int CheckEtude(BlueprintEtude etude, List<string> details)
	{
		int num = 0;
		HashSet<BlueprintGuid> hashSet = new HashSet<BlueprintGuid> { etude.AssetGuid };
		BlueprintEtudeReference parent = etude.Parent;
		BlueprintEtude blueprintEtude = etude;
		while (parent != null && parent.Guid != BlueprintGuid.Empty)
		{
			BlueprintEtude blueprintEtude2 = Resolve<BlueprintEtude>(parent.Guid, out var problem);
			if (blueprintEtude2 == null)
			{
				num++;
				details.Add(blueprintEtude.name + ".Parent: " + problem + ".");
				break;
			}
			if (!hashSet.Add(blueprintEtude2.AssetGuid))
			{
				num++;
				details.Add(etude.name + " is its own ancestor through '" + blueprintEtude2.name + "', so the etude tree loops.");
				break;
			}
			blueprintEtude = blueprintEtude2;
			parent = blueprintEtude2.Parent;
		}
		var array = new[]
		{
			new
			{
				Field = "StartsWith",
				List = etude.StartsWith
			},
			new
			{
				Field = "StartsOnComplete",
				List = etude.StartsOnComplete
			},
			new
			{
				Field = "Synchronized",
				List = etude.Synchronized
			}
		};
		foreach (var anon in array)
		{
			int num2 = 0;
			foreach (BlueprintEtudeReference item in anon.List ?? Enumerable.Empty<BlueprintEtudeReference>())
			{
				if (item == null || item.Guid == BlueprintGuid.Empty || Resolve<BlueprintEtude>(item.Guid, out var _) == null)
				{
					num++;
					details.Add($"{etude.name}.{anon.Field}[{num2}] does not resolve to an etude.");
				}
				num2++;
			}
		}
		return num;
	}

	private static HashSet<BlueprintGuid> ObjectivesSomethingCanComplete(out int byBlueprint, out int byCode, List<string> endingProblems)
	{
		HashSet<BlueprintGuid> hashSet = new HashSet<BlueprintGuid>();
		foreach (SelfTestRecorder.CreatedBlueprint item in SelfTestProgressionChecks.ModBlueprints())
		{
			List<BlueprintReferenceWalker.Found> list;
			try
			{
				list = BlueprintReferenceWalker.ReferencesIn(item.Blueprint);
			}
			catch
			{
				continue;
			}
			foreach (BlueprintReferenceWalker.Found item2 in list)
			{
				if (item2.Reference != null && !(item2.Reference.Guid == BlueprintGuid.Empty))
				{
					BlueprintQuestObjective blueprintQuestObjective = Resolve<BlueprintQuestObjective>(item2.Reference.Guid, out var _);
					if (blueprintQuestObjective != null && CompletesObjective(item2.Owner, blueprintQuestObjective))
					{
						hashSet.Add(item2.Reference.Guid);
					}
				}
			}
		}
		HashSet<BlueprintGuid> hashSet2 = new HashSet<BlueprintGuid>();
		Type[] types;
		try
		{
			types = (from t in typeof(SelfTestContentChecks).Assembly.GetTypes()
				where (t.FullName ?? "").StartsWith("IsekaiMod.", StringComparison.Ordinal) && !(t.FullName ?? "").StartsWith("IsekaiMod.Utilities.SelfTest.", StringComparison.Ordinal) && !t.IsNested
				select t).ToArray();
		}
		catch
		{
			types = new Type[0];
		}
		foreach (KeyValuePair<FieldInfo, string> item3 in IlInspector.FieldsReadByCompletingCode(types, IlInspector.IsObjectiveCompletion))
		{
			foreach (BlueprintGuid item4 in ObjectiveGuidsIn(item3.Key))
			{
				hashSet2.Add(item4);
			}
		}
		List<KeyValuePair<string, string>> list2 = new List<KeyValuePair<string, string>>();
		foreach (SelfTestRecorder.CreatedBlueprint item5 in SelfTestProgressionChecks.ModBlueprints())
		{
			if (!(item5.Blueprint is BlueprintAnswer blueprintAnswer))
			{
				continue;
			}
			foreach (ContextActionTriggerIsekaiEnding item6 in EndingTriggersIn(blueprintAnswer.OnSelect, 0))
			{
				list2.Add(new KeyValuePair<string, string>(blueprintAnswer.name, item6.EndingId));
			}
		}
		Dictionary<string, BlueprintQuestObjective> byName = new Dictionary<string, BlueprintQuestObjective>(StringComparer.Ordinal);
		foreach (string item7 in SelfTestRuntimeRules.EndingObjectivesReached(list2, delegate(string id)
		{
			BlueprintQuestObjective blueprintQuestObjective2 = IsekaiTransmigrationQuest.ObjectiveForEnding(id);
			if (blueprintQuestObjective2 == null)
			{
				return (string)null;
			}
			byName[blueprintQuestObjective2.name] = blueprintQuestObjective2;
			return blueprintQuestObjective2.name;
		}, endingProblems))
		{
			hashSet.Add(byName[item7].AssetGuid);
		}
		byBlueprint = hashSet.Count;
		byCode = hashSet2.Count;
		HashSet<BlueprintGuid> hashSet3 = new HashSet<BlueprintGuid>(hashSet);
		hashSet3.UnionWith(hashSet2);
		return hashSet3;
	}

	private static IEnumerable<ContextActionTriggerIsekaiEnding> EndingTriggersIn(ActionList list, int depth)
	{
		if (list?.Actions == null || depth > 8)
		{
			yield break;
		}
		GameAction[] actions = list.Actions;
		foreach (GameAction gameAction in actions)
		{
			if (gameAction is ContextActionTriggerIsekaiEnding contextActionTriggerIsekaiEnding)
			{
				yield return contextActionTriggerIsekaiEnding;
			}
			else
			{
				if (!(gameAction is Conditional conditional))
				{
					continue;
				}
				foreach (ContextActionTriggerIsekaiEnding item in EndingTriggersIn(conditional.IfTrue, depth + 1))
				{
					yield return item;
				}
				foreach (ContextActionTriggerIsekaiEnding item2 in EndingTriggersIn(conditional.IfFalse, depth + 1))
				{
					yield return item2;
				}
			}
		}
	}

	private static bool CompletesObjective(object owner, BlueprintQuestObjective objective)
	{
		if (owner is IQuestObjectiveReference questObjectiveReference)
		{
			try
			{
				return (questObjectiveReference.GetUsagesFor(objective) & QuestObjectiveReferenceType.Complete) != 0;
			}
			catch
			{
				return false;
			}
		}
		if (!(owner is ContextActionFinishObjective) && !(owner is FinishObjectiveOnTrigger))
		{
			return owner is BuildingPartOfQuestObjective;
		}
		return true;
	}

	private static IEnumerable<BlueprintGuid> ObjectiveGuidsIn(FieldInfo field)
	{
		if ((!typeof(BlueprintQuestObjective).IsAssignableFrom(field.FieldType) && !typeof(IEnumerable).IsAssignableFrom(field.FieldType)) || field.FieldType == typeof(string))
		{
			yield break;
		}
		object value;
		try
		{
			value = field.GetValue(null);
		}
		catch
		{
			yield break;
		}
		if (value is BlueprintQuestObjective blueprintQuestObjective)
		{
			yield return blueprintQuestObjective.AssetGuid;
		}
		else
		{
			if (!(value is IEnumerable enumerable))
			{
				yield break;
			}
			foreach (object item in enumerable)
			{
				BlueprintQuestObjective blueprintQuestObjective2 = (item as BlueprintQuestObjective) ?? (DictionaryValueOf(item) as BlueprintQuestObjective);
				if (blueprintQuestObjective2 != null)
				{
					yield return blueprintQuestObjective2.AssetGuid;
				}
			}
		}
	}

	private static object DictionaryValueOf(object entry)
	{
		if (entry == null)
		{
			return null;
		}
		Type type = entry.GetType();
		if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(KeyValuePair<, >))
		{
			return null;
		}
		try
		{
			return type.GetProperty("Value")?.GetValue(entry, null);
		}
		catch
		{
			return null;
		}
	}

	internal static SelfTestChecks.Outcome StoreAndVendors()
	{
		List<string> list = new List<string>();
		int num = 0;
		List<SelfTestRecorder.CreatedBlueprint> source = SelfTestProgressionChecks.ModBlueprints();
		BlueprintDialog blueprintDialog = source.Select((SelfTestRecorder.CreatedBlueprint c) => c.Blueprint).OfType<BlueprintDialog>().FirstOrDefault((BlueprintDialog d) => string.Equals(d.name, "CosmicStoreDialog", StringComparison.Ordinal));
		int num2 = 0;
		int num3 = 0;
		if (blueprintDialog == null)
		{
			num++;
			list.Add("CosmicStoreDialog does not exist, so no store offer could be checked.");
		}
		else
		{
			foreach (BlueprintAnswer item in StoreAnswers(blueprintDialog))
			{
				num2++;
				int? actionCost = null;
				GameAction[] array = item.OnSelect?.Actions ?? new GameAction[0];
				foreach (GameAction gameAction in array)
				{
					if (gameAction != null)
					{
						int? num5 = IntFieldOf(gameAction, "Cost");
						if (num5.HasValue)
						{
							actionCost = actionCost.GetValueOrDefault() + num5.Value;
						}
						num += CheckGrantedThing(item.name, gameAction, list);
					}
				}
				int? conditionCost = null;
				Condition[] array2 = item.SelectConditions?.Conditions ?? new Condition[0];
				for (int num4 = 0; num4 < array2.Length; num4++)
				{
					int? num6 = IntFieldOf(array2[num4], "Cost");
					if (num6.HasValue)
					{
						conditionCost = num6;
					}
				}
				if (actionCost.HasValue && actionCost.Value > 0)
				{
					num3++;
				}
				List<string> list2 = SelfTestRuntimeRules.PriceProblems(item.name, item.Text?.ToString(), actionCost, conditionCost);
				num += list2.Count;
				list.AddRange(list2);
			}
		}
		int num7 = 0;
		BlueprintSharedVendorTable blueprintSharedVendorTable = source.Select((SelfTestRecorder.CreatedBlueprint c) => c.Blueprint).OfType<BlueprintSharedVendorTable>().FirstOrDefault((BlueprintSharedVendorTable t) => string.Equals(t.name, "CosmicRelicsVendorTable", StringComparison.Ordinal));
		if (blueprintSharedVendorTable == null)
		{
			num++;
			list.Add("CosmicRelicsVendorTable does not exist, so the envoy has nothing to sell.");
		}
		else
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			BlueprintComponent[] array3 = blueprintSharedVendorTable.ComponentsArray ?? new BlueprintComponent[0];
			for (int num4 = 0; num4 < array3.Length; num4++)
			{
				if (!(array3[num4] is LootItemsPackFixed lootItemsPackFixed))
				{
					continue;
				}
				num7++;
				BlueprintItem blueprintItem = null;
				try
				{
					blueprintItem = lootItemsPackFixed.Item?.Item;
				}
				catch
				{
				}
				if (blueprintItem == null)
				{
					num++;
					list.Add($"The envoy's shelf {num7} holds no live item, so it is sold as nothing.");
					continue;
				}
				hashSet.Add(blueprintItem.name);
				hashSet.Add(blueprintItem.AssetGuid.ToString());
				int? num8 = CosmicEnvoyPrices.Lookup(blueprintItem.name, blueprintItem.AssetGuid.ToString());
				if (!num8.HasValue)
				{
					num++;
					list.Add($"The envoy stocks '{blueprintItem.name}' ({blueprintItem.AssetGuid}), which is not in the price table, so it is priced from its gold cost.");
				}
				else if (num8.Value <= 0)
				{
					num++;
					list.Add($"The envoy stocks '{blueprintItem.name}' at {num8.Value} coins.");
				}
			}
			CosmicEnvoyPrices.Entry[] table = CosmicEnvoyPrices.Table;
			foreach (CosmicEnvoyPrices.Entry entry in table)
			{
				if (!hashSet.Contains(entry.Guid ?? entry.Name))
				{
					num++;
					list.Add($"The price table prices '{entry.Name}' at {entry.Coins} coins, but the envoy does not stock it.");
				}
			}
		}
		string summary = $"{num2} Cosmic Store answers ({num3} priced) and {num7} envoy shelves against {CosmicEnvoyPrices.Table.Length} price-table rows; {num} problems.";
		return SelfTestChecks.Outcome.Of(num == 0, summary, list);
	}

	private static IEnumerable<BlueprintAnswer> StoreAnswers(BlueprintDialog dialog)
	{
		HashSet<BlueprintGuid> seen = new HashSet<BlueprintGuid> { dialog.AssetGuid };
		Queue<SimpleBlueprint> queue = new Queue<SimpleBlueprint>();
		string problem;
		foreach (BlueprintCueBaseReference item in dialog.FirstCue?.Cues ?? new List<BlueprintCueBaseReference>())
		{
			BlueprintCueBase blueprintCueBase = Resolve<BlueprintCueBase>(item?.Guid ?? BlueprintGuid.Empty, out problem);
			if (blueprintCueBase != null && seen.Add(blueprintCueBase.AssetGuid))
			{
				queue.Enqueue(blueprintCueBase);
			}
		}
		while (queue.Count > 0)
		{
			SimpleBlueprint simpleBlueprint = queue.Dequeue();
			if (simpleBlueprint is BlueprintCue cue)
			{
				foreach (BlueprintAnswerBaseReference item2 in cue.Answers ?? new List<BlueprintAnswerBaseReference>())
				{
					BlueprintAnswerBase blueprintAnswerBase = Resolve<BlueprintAnswerBase>(item2?.Guid ?? BlueprintGuid.Empty, out problem);
					if (blueprintAnswerBase != null && seen.Add(blueprintAnswerBase.AssetGuid))
					{
						queue.Enqueue(blueprintAnswerBase);
						if (blueprintAnswerBase is BlueprintAnswer blueprintAnswer)
						{
							yield return blueprintAnswer;
						}
					}
				}
				foreach (BlueprintCueBaseReference item3 in cue.Continue?.Cues ?? new List<BlueprintCueBaseReference>())
				{
					BlueprintCueBase blueprintCueBase2 = Resolve<BlueprintCueBase>(item3?.Guid ?? BlueprintGuid.Empty, out problem);
					if (blueprintCueBase2 != null && seen.Add(blueprintCueBase2.AssetGuid))
					{
						queue.Enqueue(blueprintCueBase2);
					}
				}
			}
			else
			{
				if (!(simpleBlueprint is BlueprintAnswer blueprintAnswer2))
				{
					continue;
				}
				foreach (BlueprintCueBaseReference item4 in blueprintAnswer2.NextCue?.Cues ?? new List<BlueprintCueBaseReference>())
				{
					BlueprintCueBase blueprintCueBase3 = Resolve<BlueprintCueBase>(item4?.Guid ?? BlueprintGuid.Empty, out problem);
					if (blueprintCueBase3 != null && seen.Add(blueprintCueBase3.AssetGuid))
					{
						queue.Enqueue(blueprintCueBase3);
					}
				}
			}
		}
	}

	private static int CheckGrantedThing(string answerName, GameAction action, List<string> details)
	{
		List<BlueprintReferenceWalker.Found> list;
		try
		{
			list = BlueprintReferenceWalker.ReferencesInObject(action);
		}
		catch (Exception ex)
		{
			details.Add(answerName + ": the OnSelect action " + action.GetType().Name + " could not be read (" + ex.GetType().Name + ").");
			return 1;
		}
		int num = 0;
		foreach (BlueprintReferenceWalker.Found item in list)
		{
			if (item.Reference != null && !(item.Reference.Guid == BlueprintGuid.Empty))
			{
				SimpleBlueprint simpleBlueprint;
				try
				{
					simpleBlueprint = ResourcesLibrary.TryGetBlueprint(item.Reference.Guid);
				}
				catch
				{
					simpleBlueprint = null;
				}
				if (simpleBlueprint == null)
				{
					num++;
					details.Add($"{answerName}: {action.GetType().Name}.{item.Path} names {item.Reference.Guid}, which does not resolve, so the purchase gives nothing.");
				}
			}
		}
		return num;
	}

	internal static SelfTestChecks.Outcome TeleportDelivery()
	{
		List<string> list = new List<string>();
		int num = 0;
		int num2 = 0;
		foreach (SelfTestRecorder.CreatedBlueprint item in SelfTestProgressionChecks.ModBlueprints())
		{
			if (!(item.Blueprint is BlueprintAbility blueprintAbility))
			{
				continue;
			}
			BlueprintComponent[] array = blueprintAbility.ComponentsArray ?? new BlueprintComponent[0];
			for (int i = 0; i < array.Length; i++)
			{
				if (!(array[i] is AbilityCustomDimensionDoor abilityCustomDimensionDoor))
				{
					continue;
				}
				num++;
				FieldInfo[] fields = abilityCustomDimensionDoor.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
				foreach (FieldInfo fieldInfo in fields)
				{
					if (!(fieldInfo.FieldType != typeof(PrefabLink)) && fieldInfo.GetValue(abilityCustomDimensionDoor) == null)
					{
						num2++;
						list.Add(blueprintAbility.name + ": AbilityCustomDimensionDoor." + fieldInfo.Name + " is null, and Deliver loads it before it moves the caster, so the ability throws and nobody teleports.");
					}
				}
				if (abilityCustomDimensionDoor.PortalBone == null)
				{
					num2++;
					list.Add(blueprintAbility.name + ": AbilityCustomDimensionDoor.PortalBone is null, which throws while the portal art is being placed.");
				}
				if (!blueprintAbility.CanTargetPoint)
				{
					num2++;
					list.Add(blueprintAbility.name + ": teleports, but CanTargetPoint is false, so the engine never hands it a destination.");
				}
				if (blueprintAbility.Range == AbilityRange.Personal)
				{
					num2++;
					list.Add(blueprintAbility.name + ": teleports, but its range is Personal, so the only destination it can be given is where the caster already stands.");
				}
			}
		}
		if (num == 0)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "No ability in this mod carries a dimension door, but several of them promise to move the caster.");
		}
		return SelfTestChecks.Outcome.Of(num2 == 0, (num2 == 0) ? $"All {num} teleports can be given a destination and carry no null resource link." : $"{num2} problem(s) across {num} teleports.", list);
	}

	private static int? IntFieldOf(object target, string fieldName)
	{
		if (target == null)
		{
			return null;
		}
		FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
		if (field == null || field.FieldType != typeof(int))
		{
			return null;
		}
		try
		{
			return (int)field.GetValue(target);
		}
		catch
		{
			return null;
		}
	}
}
