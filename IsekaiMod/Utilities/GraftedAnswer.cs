using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities;

internal sealed class GraftedAnswer
{
	internal sealed class Record
	{
		public BlueprintAnswer Answer;

		public BlueprintAnswersList Host;

		public GraftBeat Beat;
	}

	internal static class BeatGates
	{
		private sealed class Registered
		{
			public ConditionCurrentCueIs Gate;

			public List<GraftBeat> Beats;
		}

		private sealed class Return
		{
			public GraftBeat Beat;

			public BlueprintCueBaseReference Reply;
		}

		private static readonly List<Registered> Gates = new List<Registered>();

		private static readonly List<Return> Returns = new List<Return>();

		private static readonly Dictionary<string, HashSet<string>> ContinuationCuesByList = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);

		private const int MaxChain = 8;

		internal static ConditionCurrentCueIs GateFor(IList<GraftBeat> beats, IList<BlueprintAnswersList> hosts)
		{
			List<BlueprintCueBaseReference> list = new List<BlueprintCueBaseReference>();
			for (int i = 0; i < beats.Count; i++)
			{
				GraftBeat graftBeat = beats[i];
				List<string> list2 = new List<string>();
				foreach (string presentingCue in graftBeat.PresentingCues)
				{
					BlueprintCueBase blueprint = BlueprintTools.GetBlueprint<BlueprintCueBase>(presentingCue);
					if (blueprint == null)
					{
						Main.IsekaiContext.Logger.LogError("GraftedAnswer: cue " + presentingCue + " of " + graftBeat.Name + " was not found.");
					}
					else if (!Presents(blueprint, hosts[i]))
					{
						Main.IsekaiContext.Logger.LogError("GraftedAnswer: cue " + blueprint.name + " (" + presentingCue + ") of " + graftBeat.Name + " does not present list " + hosts[i].name + ".");
					}
					else
					{
						list.Add(blueprint.ToReference<BlueprintCueBaseReference>());
						list2.Add(GraftBeat.Normalize(blueprint.AssetGuid.ToString()));
					}
				}
				string text = GraftBeat.Normalize(hosts[i].AssetGuid.ToString());
				foreach (string exitCue in ContinuationCues(list2, text, LiveKind, LiveField))
				{
					BlueprintCueBase blueprint2 = BlueprintTools.GetBlueprint<BlueprintCueBase>(exitCue);
					if (blueprint2 != null && !list.Any((BlueprintCueBaseReference r) => GraftBeat.Normalize(((BlueprintReferenceBase)r).deserializedGuid.ToString()) == exitCue))
					{
						list.Add(blueprint2.ToReference<BlueprintCueBaseReference>());
						if (!ContinuationCuesByList.TryGetValue(text, out var value))
						{
							value = (ContinuationCuesByList[text] = new HashSet<string>(StringComparer.Ordinal));
						}
						value.Add(exitCue);
					}
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			ConditionCurrentCueIs conditionCurrentCueIs = new ConditionCurrentCueIs();
			foreach (Return ret in Returns)
			{
				if (beats.Any((GraftBeat b) => SameExchange(ret.Beat, b)) && !list.Any((BlueprintCueBaseReference r) => ((BlueprintReferenceBase)r).deserializedGuid == ((BlueprintReferenceBase)ret.Reply).deserializedGuid))
				{
					list.Add(ret.Reply);
				}
			}
			Gates.Add(new Registered
			{
				Gate = conditionCurrentCueIs,
				Beats = beats.ToList()
			});
			conditionCurrentCueIs.m_Cues = list.ToArray();
			return conditionCurrentCueIs;
		}

		internal static void AddReturn(GraftBeat beat, BlueprintCue reply)
		{
			BlueprintCueBaseReference reference = reply.ToReference<BlueprintCueBaseReference>();
			if (Returns.Any((Return r) => r.Beat == beat && ((BlueprintReferenceBase)r.Reply).deserializedGuid == ((BlueprintReferenceBase)reference).deserializedGuid))
			{
				return;
			}
			Returns.Add(new Return
			{
				Beat = beat,
				Reply = reference
			});
			foreach (Registered gate2 in Gates)
			{
				if (gate2.Beats.Any((GraftBeat b) => SameExchange(beat, b)))
				{
					ConditionCurrentCueIs gate = gate2.Gate;
					if (!gate.m_Cues.Any((BlueprintCueBaseReference r) => r != null && ((BlueprintReferenceBase)r).deserializedGuid == ((BlueprintReferenceBase)reference).deserializedGuid))
					{
						gate.m_Cues = gate.m_Cues.AppendToArray(reference);
					}
				}
			}
		}

		internal static bool SameExchange(GraftBeat replyBeat, GraftBeat gateBeat)
		{
			if (replyBeat == null || gateBeat == null || replyBeat.Hub || gateBeat.Hub)
			{
				return false;
			}
			if (replyBeat.HostList != gateBeat.HostList)
			{
				return false;
			}
			if (replyBeat == gateBeat || replyBeat.Name == gateBeat.Name)
			{
				return true;
			}
			if (replyBeat.PresentingCues.Count > 0)
			{
				return replyBeat.PresentingCues.All((string c) => gateBeat.PresentingCues.Contains(c));
			}
			return false;
		}

		internal static bool IsContinuationCue(BlueprintCueBase cue, BlueprintAnswersList list)
		{
			if (cue == null || list == null)
			{
				return false;
			}
			if (ContinuationCuesByList.TryGetValue(GraftBeat.Normalize(list.AssetGuid.ToString()), out var value))
			{
				return value.Contains(GraftBeat.Normalize(cue.AssetGuid.ToString()));
			}
			return false;
		}

		internal static List<string> ContinuationCues(IEnumerable<string> presentingCues, string hostList, Func<string, string> kindOf, Func<string, string, IList<string>> field)
		{
			List<string> list = new List<string>();
			if (presentingCues == null || kindOf == null || field == null)
			{
				return list;
			}
			string host = GraftBeat.Normalize(hostList);
			HashSet<string> hashSet = new HashSet<string>(presentingCues.Select(GraftBeat.Normalize), StringComparer.Ordinal);
			HashSet<string> hashSet2 = new HashSet<string>(StringComparer.Ordinal);
			Queue<KeyValuePair<string, int>> queue = new Queue<KeyValuePair<string, int>>();
			foreach (string item in hashSet)
			{
				foreach (string item2 in Field(field, item, "Continue"))
				{
					queue.Enqueue(new KeyValuePair<string, int>(item2, 1));
				}
			}
			while (queue.Count > 0)
			{
				KeyValuePair<string, int> keyValuePair = queue.Dequeue();
				string key = keyValuePair.Key;
				if (keyValuePair.Value > 8 || !hashSet2.Add(key))
				{
					continue;
				}
				switch (kindOf(key))
				{
				case "BlueprintCue":
					if (!hashSet.Contains(key) && !list.Contains(key) && ListReached(Field(field, key, "Answers"), host, kindOf, field))
					{
						list.Add(key);
					}
					foreach (string item3 in Field(field, key, "Continue"))
					{
						queue.Enqueue(new KeyValuePair<string, int>(item3, keyValuePair.Value + 1));
					}
					break;
				case "BlueprintCheck":
					foreach (string item4 in Field(field, key, "Success").Concat(Field(field, key, "Fail")))
					{
						queue.Enqueue(new KeyValuePair<string, int>(item4, keyValuePair.Value + 1));
					}
					break;
				case "BlueprintCueSequence":
				{
					string text = Field(field, key, "Exit").FirstOrDefault();
					if (text == null)
					{
						break;
					}
					if (ListReached(Field(field, text, "Answers"), host, kindOf, field))
					{
						foreach (string item5 in CuesOfSequence(key, kindOf, field))
						{
							if (!list.Contains(item5))
							{
								list.Add(item5);
							}
						}
						break;
					}
					foreach (string item6 in Field(field, text, "Continue"))
					{
						queue.Enqueue(new KeyValuePair<string, int>(item6, keyValuePair.Value + 1));
					}
					break;
				}
				}
			}
			return list;
		}

		private static IEnumerable<string> CuesOfSequence(string sequence, Func<string, string> kindOf, Func<string, string, IList<string>> field)
		{
			List<string> list = new List<string>();
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			Queue<KeyValuePair<string, int>> queue = new Queue<KeyValuePair<string, int>>();
			foreach (string item in Field(field, sequence, "Cues"))
			{
				queue.Enqueue(new KeyValuePair<string, int>(item, 0));
			}
			while (queue.Count > 0)
			{
				KeyValuePair<string, int> keyValuePair = queue.Dequeue();
				string key = keyValuePair.Key;
				if (keyValuePair.Value > 8 || !hashSet.Add(key))
				{
					continue;
				}
				switch (kindOf(key))
				{
				case "BlueprintCue":
					list.Add(key);
					foreach (string item2 in Field(field, key, "Continue"))
					{
						queue.Enqueue(new KeyValuePair<string, int>(item2, keyValuePair.Value + 1));
					}
					break;
				case "BlueprintCheck":
					foreach (string item3 in Field(field, key, "Success").Concat(Field(field, key, "Fail")))
					{
						queue.Enqueue(new KeyValuePair<string, int>(item3, keyValuePair.Value + 1));
					}
					break;
				case "BlueprintCueSequence":
					foreach (string item4 in Field(field, key, "Cues"))
					{
						queue.Enqueue(new KeyValuePair<string, int>(item4, keyValuePair.Value + 1));
					}
					break;
				}
			}
			return list;
		}

		private static bool ListReached(IEnumerable<string> answers, string host, Func<string, string> kindOf, Func<string, string, IList<string>> field)
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			Queue<string> queue = new Queue<string>(answers ?? new List<string>());
			while (queue.Count > 0)
			{
				string text = queue.Dequeue();
				if (!hashSet.Add(text))
				{
					continue;
				}
				if (text == host)
				{
					return true;
				}
				if (kindOf(text) != "BlueprintAnswersList")
				{
					continue;
				}
				foreach (string item in Field(field, text, "Answers"))
				{
					queue.Enqueue(item);
				}
			}
			return false;
		}

		private static IEnumerable<string> Field(Func<string, string, IList<string>> field, string id, string name)
		{
			if (string.IsNullOrEmpty(id))
			{
				return new string[0];
			}
			return (from s in (field(id, name) ?? new List<string>()).Select(GraftBeat.Normalize)
				where s.Length > 0
				select s).ToList();
		}

		private static string LiveKind(string id)
		{
			return BlueprintTools.GetBlueprint<SimpleBlueprint>(id)?.GetType().Name;
		}

		private static IList<string> LiveField(string id, string name)
		{
			SimpleBlueprint blueprint = BlueprintTools.GetBlueprint<SimpleBlueprint>(id);
			IEnumerable<BlueprintReferenceBase> enumerable = null;
			if (!(blueprint is BlueprintCue blueprintCue))
			{
				if (!(blueprint is BlueprintCueSequence blueprintCueSequence))
				{
					if (!(blueprint is BlueprintSequenceExit blueprintSequenceExit))
					{
						if (!(blueprint is BlueprintCheck blueprintCheck))
						{
							if (blueprint is BlueprintAnswersList blueprintAnswersList && name == "Answers")
							{
								enumerable = blueprintAnswersList.Answers;
							}
						}
						else
						{
							if (name == "Success" && blueprintCheck.Success != null)
							{
								return new string[1] { blueprintCheck.Success.AssetGuid.ToString() };
							}
							if (name == "Fail" && blueprintCheck.Fail != null)
							{
								return new string[1] { blueprintCheck.Fail.AssetGuid.ToString() };
							}
						}
					}
					else if (name == "Continue")
					{
						enumerable = blueprintSequenceExit.Continue?.Cues;
					}
					else if (name == "Answers")
					{
						enumerable = blueprintSequenceExit.Answers;
					}
				}
				else if (name == "Cues")
				{
					enumerable = blueprintCueSequence.Cues;
				}
				else if (name == "Exit" && blueprintCueSequence.Exit != null)
				{
					return new string[1] { blueprintCueSequence.Exit.AssetGuid.ToString() };
				}
			}
			else if (name == "Continue")
			{
				enumerable = blueprintCue.Continue?.Cues;
			}
			else if (name == "Answers")
			{
				enumerable = blueprintCue.Answers;
			}
			if (enumerable != null)
			{
				return (from r in enumerable
					where r != null && !r.IsEmpty()
					select r.deserializedGuid.ToString()).ToArray();
			}
			return new string[0];
		}

		internal static bool Presents(BlueprintCueBase cue, BlueprintAnswersList list)
		{
			if (!(cue is BlueprintCue { Answers: not null } blueprintCue))
			{
				return false;
			}
			HashSet<BlueprintAnswersList> hashSet = new HashSet<BlueprintAnswersList>();
			Queue<BlueprintAnswerBaseReference> queue = new Queue<BlueprintAnswerBaseReference>(blueprintCue.Answers);
			while (queue.Count > 0)
			{
				if (!(queue.Dequeue()?.Get() is BlueprintAnswersList blueprintAnswersList) || !hashSet.Add(blueprintAnswersList))
				{
					continue;
				}
				if (blueprintAnswersList == list)
				{
					return true;
				}
				foreach (BlueprintAnswerBaseReference item in blueprintAnswersList.Answers ?? new List<BlueprintAnswerBaseReference>())
				{
					queue.Enqueue(item);
				}
			}
			return false;
		}
	}

	internal static readonly List<Record> Grafts = new List<Record>();

	private readonly BlueprintAnswer _answer;

	private readonly List<GraftBeat> _beats = new List<GraftBeat>();

	private bool _endsConversation;

	private GraftedCue _reply;

	private GraftedCue _twin;

	private readonly List<BlueprintCueBase> _alternatives = new List<BlueprintCueBase>();

	private readonly List<BlueprintCueBase> _straightTo = new List<BlueprintCueBase>();

	private BlueprintCueBase _continue;

	private BlueprintCueBase[] _fallbacks = new BlueprintCueBase[0];

	private bool _backToList;

	private BlueprintUnit _companion;

	private bool _broken;

	public BlueprintAnswer Blueprint => _answer;

	private GraftedAnswer(BlueprintAnswer answer)
	{
		_answer = answer;
		_answer.ShowOnce = true;
	}

	public static GraftedAnswer Named(string name)
	{
		return new GraftedAnswer(TTCoreExtensions.CreateAnswer(name));
	}

	public GraftedAnswer At(GraftBeat beat, params GraftBeat[] more)
	{
		if (beat != null)
		{
			_beats.Add(beat);
		}
		if (more != null)
		{
			_beats.AddRange(more.Where((GraftBeat b) => b != null));
		}
		return this;
	}

	public GraftedAnswer Says(string text)
	{
		_answer.SetText(Main.IsekaiContext, text);
		return this;
	}

	public GraftedAnswer ShownWhen(params Condition[] conditions)
	{
		if (conditions == null || conditions.Any((Condition c) => c == null))
		{
			_broken = true;
			return this;
		}
		_answer.ShowConditions.Conditions = _answer.ShowConditions.Conditions.AppendToArray(conditions);
		return this;
	}

	public GraftedAnswer Repeatable()
	{
		_answer.ShowOnce = false;
		return this;
	}

	public GraftedAnswer OnSelect(params GameAction[] actions)
	{
		GameAction[] array = GuardedGameAction.GuardAll(actions);
		if (array.Length != 0)
		{
			_answer.OnSelect = Helpers.CreateActionList(_answer.OnSelect.Actions.Concat(array).ToArray());
		}
		return this;
	}

	public GraftedAnswer Configure(Action<BlueprintAnswer> configure)
	{
		configure?.Invoke(_answer);
		return this;
	}

	public GraftedAnswer RepliedToBy(string replyName, string text)
	{
		_reply = GraftedCue.Named(replyName).Says(text);
		return this;
	}

	public GraftedAnswer RepliedWith(GraftedCue reply)
	{
		_reply = reply;
		return this;
	}

	public GraftedAnswer SpokenBy(BlueprintUnit unit)
	{
		RequireReply("SpokenBy");
		_reply?.SpokenBy(unit);
		if (unit != null)
		{
			_answer.RequireValidCue = true;
		}
		return this;
	}

	public GraftedAnswer SpokenByCompanion(BlueprintUnit unit)
	{
		SpokenBy(unit);
		_companion = unit;
		return this;
	}

	public GraftedAnswer Narrated()
	{
		RequireReply("Narrated");
		_reply?.Narrated();
		return this;
	}

	public GraftedAnswer SpokenWithPortraitOf(BlueprintUnitReference portrait)
	{
		RequireReply("SpokenWithPortraitOf");
		_reply?.SpokenWithPortraitOf(portrait);
		return this;
	}

	public GraftedAnswer WithSpeakerlessTwin(string twinName)
	{
		RequireReply("WithSpeakerlessTwin");
		if (_reply == null)
		{
			return this;
		}
		_twin = GraftedCue.Named(twinName).Says(_reply.Text).Narrated();
		return this;
	}

	public GraftedAnswer ConfigureReply(Action<GraftedCue> configure)
	{
		RequireReply("ConfigureReply");
		if (_reply != null)
		{
			configure?.Invoke(_reply);
		}
		return this;
	}

	public GraftedAnswer ThenBackToTheList()
	{
		RequireReply("ThenBackToTheList");
		_backToList = true;
		return this;
	}

	public GraftedAnswer ThenContinueTo(BlueprintCueBase next, params BlueprintCueBase[] fallbacks)
	{
		RequireReply("ThenContinueTo");
		if (next == null)
		{
			Main.IsekaiContext.Logger.LogError("GraftedAnswer " + _answer.name + ": the cue the reply continues into was not found; the answer is left out.");
			_broken = true;
			return this;
		}
		_continue = next;
		_fallbacks = (fallbacks ?? new BlueprintCueBase[0]).Where((BlueprintCueBase f) => f != null).ToArray();
		return this;
	}

	public GraftedAnswer ThenEndTheConversation()
	{
		RequireReply("ThenEndTheConversation");
		_endsConversation = true;
		return this;
	}

	public GraftedAnswer OrElseReply(BlueprintCueBase alternative)
	{
		if (alternative != null)
		{
			_alternatives.Add(alternative);
		}
		return this;
	}

	public GraftedAnswer ContinuesStraightTo(params BlueprintCueBase[] cues)
	{
		if (cues == null || cues.Length == 0 || cues.Any((BlueprintCueBase c) => c == null))
		{
			Main.IsekaiContext.Logger.LogError("GraftedAnswer " + _answer.name + ": a cue the answer continues into was not found; the answer is left out.");
			_broken = true;
			return this;
		}
		_straightTo.AddRange(cues);
		return this;
	}

	private void RequireReply(string call)
	{
		if (_reply == null)
		{
			Main.IsekaiContext.Logger.LogError("GraftedAnswer " + _answer.name + ": " + call + " was called before RepliedToBy; the answer is left out.");
			_broken = true;
		}
	}

	public BlueprintAnswer Graft()
	{
		if (_broken)
		{
			return null;
		}
		if (_beats.Count == 0)
		{
			Main.IsekaiContext.Logger.LogError("GraftedAnswer " + _answer.name + ": no beat was given (At); an answer is never grafted onto every cue of a list by accident.");
			return null;
		}
		if (_beats.Any((GraftBeat b) => b.Hub) && _beats.Any((GraftBeat b) => !b.Hub))
		{
			Main.IsekaiContext.Logger.LogError("GraftedAnswer " + _answer.name + ": its beats mix topic hubs and pinned cues; the answer is left out.");
			return null;
		}
		if (_backToList && _beats.Count > 1)
		{
			Main.IsekaiContext.Logger.LogError($"GraftedAnswer {_answer.name}: a reply can only return to one list, and the answer has {_beats.Count} beats; the answer is left out.");
			return null;
		}
		List<BlueprintAnswersList> list = new List<BlueprintAnswersList>();
		foreach (GraftBeat beat in _beats)
		{
			BlueprintAnswersList blueprint = BlueprintTools.GetBlueprint<BlueprintAnswersList>(beat.HostList);
			if (blueprint == null)
			{
				Main.IsekaiContext.Logger.LogWarning("GraftedAnswer " + _answer.name + ": host list " + beat.HostList + " (" + beat.Name + ") was not found, skipping.");
				return null;
			}
			list.Add(blueprint);
		}
		List<BlueprintCueBase> list2 = new List<BlueprintCueBase>();
		if (_reply != null)
		{
			foreach (GraftedCue item in new GraftedCue[2] { _reply, _twin }.Where((GraftedCue c) => c != null))
			{
				if (_backToList)
				{
					item.ThenBackTo(_beats[0]);
				}
				if (_continue != null)
				{
					item.ThenContinueTo(_continue, _fallbacks);
				}
				if (item.Refused)
				{
					return null;
				}
				bool num = item.Blueprint.Continue?.Cues != null && item.Blueprint.Continue.Cues.Count > 0;
				bool flag = item.Blueprint.Answers != null && item.Blueprint.Answers.Count > 0;
				if (!num && !flag && !_endsConversation)
				{
					Main.IsekaiContext.Logger.LogError("GraftedAnswer " + _answer.name + ": the reply " + item.Blueprint.name + " neither returns to the list nor continues, and ThenEndTheConversation was not said; the answer is left out.");
					return null;
				}
				list2.Add(item.Blueprint);
			}
		}
		list2.AddRange(_alternatives);
		list2.AddRange(_straightTo);
		if (list2.Count > 0)
		{
			_answer.NextCue = new CueSelection
			{
				Cues = list2.Select((BlueprintCueBase c) => c.ToReference<BlueprintCueBaseReference>()).ToList(),
				Strategy = Strategy.First
			};
			if (GraftedCue.SelectionCanRefuse(list2, null, returnsToList: false))
			{
				_answer.RequireValidCue = true;
			}
		}
		if (_companion != null)
		{
			_answer.ShowConditions.Conditions = _answer.ShowConditions.Conditions.AppendToArray(new CompanionInParty
			{
				m_companion = _companion.ToReference<BlueprintUnitReference>(),
				MatchWhenActive = true,
				MatchWhenRemote = true,
				MatchWhenDetached = true
			});
		}
		if (!_beats[0].Hub)
		{
			ConditionCurrentCueIs conditionCurrentCueIs = BeatGates.GateFor(_beats, list);
			if (conditionCurrentCueIs == null)
			{
				Main.IsekaiContext.Logger.LogError("GraftedAnswer " + _answer.name + ": no cue of " + string.Join(", ", _beats.Select((GraftBeat b) => b.Name)) + " presents its list; the answer is left out.");
				return null;
			}
			_answer.ShowConditions.Conditions = new Condition[1] { conditionCurrentCueIs }.Concat(_answer.ShowConditions.Conditions).ToArray();
		}
		for (int num2 = 0; num2 < list.Count; num2++)
		{
			BlueprintAnswersList blueprintAnswersList = list[num2];
			BlueprintAnswersList blueprintAnswersList2 = blueprintAnswersList;
			if (blueprintAnswersList2.Answers == null)
			{
				blueprintAnswersList2.Answers = new List<BlueprintAnswerBaseReference>();
			}
			if (!blueprintAnswersList.Answers.Any((BlueprintAnswerBaseReference r) => r != null && ((BlueprintReferenceBase)r).deserializedGuid == _answer.AssetGuid))
			{
				blueprintAnswersList.Answers.Insert(InsertionIndex(blueprintAnswersList.Answers.Select((BlueprintAnswerBaseReference r) => EndsConversation(r?.Get())).ToArray()), _answer.ToReference<BlueprintAnswerBaseReference>());
				Grafts.Add(new Record
				{
					Answer = _answer,
					Host = blueprintAnswersList,
					Beat = _beats[num2]
				});
			}
		}
		return _answer;
	}

	internal static int InsertionIndex(bool[] endsConversation)
	{
		if (endsConversation == null)
		{
			return 0;
		}
		int num = endsConversation.Length;
		while (num > 0 && endsConversation[num - 1])
		{
			num--;
		}
		if (num != 0)
		{
			return num;
		}
		return endsConversation.Length;
	}

	internal static bool EndsConversation(int nextCueCount, bool[] nextLeadsOnlyToPartingLines)
	{
		if (nextCueCount == 0)
		{
			return true;
		}
		if (nextLeadsOnlyToPartingLines != null && nextLeadsOnlyToPartingLines.Length == nextCueCount)
		{
			return nextLeadsOnlyToPartingLines.All((bool t) => t);
		}
		return false;
	}

	private static bool EndsConversation(BlueprintAnswerBase answerBase)
	{
		if (!(answerBase is BlueprintAnswer blueprintAnswer))
		{
			return false;
		}
		List<BlueprintCueBaseReference> list = blueprintAnswer.NextCue?.Cues ?? new List<BlueprintCueBaseReference>();
		return EndsConversation(list.Count, list.Select((BlueprintCueBaseReference r) => OnlyPartingLines(r?.Get(), 0)).ToArray());
	}

	private static bool OnlyPartingLines(BlueprintCueBase cueBase, int depth)
	{
		if (!(cueBase is BlueprintCue blueprintCue) || depth > 4)
		{
			return false;
		}
		if (blueprintCue.Answers != null && blueprintCue.Answers.Count > 0)
		{
			return false;
		}
		return (blueprintCue.Continue?.Cues ?? new List<BlueprintCueBaseReference>()).All((BlueprintCueBaseReference r) => OnlyPartingLines(r?.Get(), depth + 1));
	}
}
