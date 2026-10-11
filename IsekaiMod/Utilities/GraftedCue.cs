using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities;

internal sealed class GraftedCue
{
	private readonly BlueprintCue _cue;

	private string _text;

	public BlueprintCue Blueprint => _cue;

	internal string Text => _text;

	internal bool Refused { get; private set; }

	private GraftedCue(BlueprintCue cue)
	{
		_cue = cue;
	}

	public static GraftedCue Named(string name)
	{
		return new GraftedCue(TTCoreExtensions.CreateCue(name));
	}

	public static implicit operator BlueprintCue(GraftedCue cue)
	{
		return cue?._cue;
	}

	public GraftedCue Says(string text)
	{
		_text = text;
		_cue.SetText(Main.IsekaiContext, text);
		return this;
	}

	public GraftedCue SpokenBy(BlueprintUnit unit)
	{
		if (unit != null)
		{
			_cue.Speaker = new DialogSpeaker
			{
				m_Blueprint = unit.ToReference<BlueprintUnitReference>(),
				MoveCamera = true
			};
		}
		return this;
	}

	public GraftedCue Narrated()
	{
		_cue.Speaker = new DialogSpeaker
		{
			NoSpeaker = true
		};
		_cue.TurnSpeaker = false;
		return this;
	}

	public GraftedCue SpokenWithPortraitOf(BlueprintUnitReference portrait)
	{
		_cue.Speaker = new DialogSpeaker
		{
			m_Blueprint = null,
			MoveCamera = false,
			m_SpeakerPortrait = portrait
		};
		_cue.TurnSpeaker = false;
		return this;
	}

	public GraftedCue ShownWhen(params Condition[] conditions)
	{
		if (conditions != null)
		{
			_cue.Conditions.Conditions = _cue.Conditions.Conditions.AppendToArray(conditions.Where((Condition c) => c != null).ToArray());
		}
		return this;
	}

	public GraftedCue OnStop(params GameAction[] actions)
	{
		GameAction[] array = GuardedGameAction.GuardAll(actions);
		if (array.Length != 0)
		{
			_cue.OnStop = Helpers.CreateActionList(_cue.OnStop.Actions.Concat(array).ToArray());
		}
		return this;
	}

	public GraftedCue Configure(Action<BlueprintCue> configure)
	{
		configure?.Invoke(_cue);
		return this;
	}

	public GraftedCue ThenBackTo(GraftBeat beat)
	{
		BlueprintAnswersList blueprintAnswersList = ((beat == null) ? null : BlueprintTools.GetBlueprint<BlueprintAnswersList>(beat.HostList));
		if (blueprintAnswersList == null)
		{
			return this;
		}
		_cue.Answers = new List<BlueprintAnswerBaseReference> { blueprintAnswersList.ToReference<BlueprintAnswerBaseReference>() };
		if (!beat.Hub)
		{
			GraftedAnswer.BeatGates.AddReturn(beat, _cue);
		}
		return this;
	}

	public GraftedCue ThenContinueTo(BlueprintCueBase next, params BlueprintCueBase[] fallbacks)
	{
		List<BlueprintCueBase> list = new List<BlueprintCueBase> { next };
		list.AddRange(fallbacks ?? new BlueprintCueBase[0]);
		if (list.Any((BlueprintCueBase c) => c == null))
		{
			Main.IsekaiContext.Logger.LogError("GraftedCue " + _cue.name + ": a cue it continues into was not found.");
			Refused = true;
			return this;
		}
		bool returnsToList = _cue.Answers != null && _cue.Answers.Count > 0;
		if (SelectionCanRefuse(list, SpeakerId(_cue), returnsToList))
		{
			Main.IsekaiContext.Logger.LogError("GraftedCue " + _cue.name + ": every cue it continues into (" + string.Join(", ", list.Select((BlueprintCueBase c) => c.name)) + ") can refuse to show and nothing follows, so the conversation could end here with the scene half done. Add a speakerless, unconditional fallback.");
			Refused = true;
			return this;
		}
		_cue.Continue = new CueSelection
		{
			Cues = list.Select((BlueprintCueBase c) => c.ToReference<BlueprintCueBaseReference>()).ToList(),
			Strategy = Strategy.First
		};
		return this;
	}

	internal static string SpeakerId(BlueprintCueBase cueBase)
	{
		if (!(cueBase is BlueprintCue { Speaker: not null } blueprintCue) || blueprintCue.Speaker.NoSpeaker)
		{
			return null;
		}
		BlueprintUnitReference blueprint = blueprintCue.Speaker.m_Blueprint;
		if (blueprint == null || blueprint.IsEmpty())
		{
			return null;
		}
		return GraftBeat.Normalize(((BlueprintReferenceBase)blueprint).deserializedGuid.ToString());
	}

	internal static bool SelectionCanRefuse(IList<BlueprintCueBase> chain, string provenSpeaker, bool returnsToList)
	{
		return CanRefuse(chain.Select((BlueprintCueBase c) => c != null).ToArray(), chain.Select((BlueprintCueBase c) => c?.Conditions != null && c.Conditions.HasConditions).ToArray(), chain.Select(SpeakerId).ToArray(), provenSpeaker, returnsToList);
	}

	internal static bool CanRefuse(bool[] exists, bool[] conditioned, string[] speakers, string provenSpeaker, bool returnsToList)
	{
		if (returnsToList)
		{
			return false;
		}
		for (int i = 0; i < exists.Length; i++)
		{
			if (exists[i] && !conditioned[i])
			{
				string text = speakers[i];
				if (string.IsNullOrEmpty(text))
				{
					return false;
				}
				if (!string.IsNullOrEmpty(provenSpeaker) && GraftBeat.Normalize(text) == GraftBeat.Normalize(provenSpeaker))
				{
					return false;
				}
			}
		}
		return true;
	}
}
