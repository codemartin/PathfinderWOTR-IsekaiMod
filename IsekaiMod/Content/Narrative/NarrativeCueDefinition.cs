using System;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;

namespace IsekaiMod.Content.Narrative
{
	public class NarrativeCueDefinition
	{
		public string SpeakerGuid { get; set; }

		public string Text { get; set; }

		public bool MoveCamera { get; set; } = true;

		public GameAction OnStopAction { get; set; }

		public ActionList OnStopActionList { get; set; }

		public Action<BlueprintCue> ConfigureCue { get; set; }
	}
}
