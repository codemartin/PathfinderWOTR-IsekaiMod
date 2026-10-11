using System.Collections.Generic;
using System.Linq;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.DialogSystem.Blueprints;

namespace IsekaiMod.Utilities;

[TypeId("023d729676af4890a66b15a596422a74")]
public class ConditionCurrentCueIs : IsekaiCondition
{
	public BlueprintCueBaseReference[] m_Cues = new BlueprintCueBaseReference[0];

	protected override bool CheckSafely()
	{
		BlueprintCue blueprintCue = Game.Instance?.DialogController?.CurrentCue;
		if (blueprintCue == null || m_Cues == null)
		{
			return false;
		}
		return IsAnyOf(blueprintCue.AssetGuid.ToString(), from c in m_Cues
			where c != null
			select ((BlueprintReferenceBase)c).deserializedGuid.ToString());
	}

	internal static bool IsAnyOf(string currentCueId, IEnumerable<string> allowedCueIds)
	{
		string current = GraftBeat.Normalize(currentCueId);
		if (current.Length == 0 || allowedCueIds == null)
		{
			return false;
		}
		return allowedCueIds.Any((string id) => GraftBeat.Normalize(id) == current);
	}

	public override string GetConditionCaption()
	{
		BlueprintCueBaseReference[] cues = m_Cues;
		return "The cue on screen is one of " + ((cues != null) ? cues.Length : 0) + " cues this answer was written for";
	}
}
