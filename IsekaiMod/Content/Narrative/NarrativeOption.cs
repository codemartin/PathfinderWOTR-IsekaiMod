using System;
using System.Collections.Generic;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Narrative
{
	public class NarrativeOption
	{
		public string Id { get; set; }

		public OptionType Type { get; set; }

		public string RequiredProficiency { get; set; }

		public string RequiredFeatureGuid { get; set; }

		public string RequiredEtudeGuid { get; set; }

		public int? RequiredMythicLevel { get; set; }

		public AlignmentShiftDirection? Alignment { get; set; }

		public string PromptText { get; set; }

		public string NpcReplyText { get; set; }

		public List<CycleVariation> Variations { get; set; } = new List<CycleVariation>();

		public int ExpCR { get; set; } = 2;

		public bool ShowOnce { get; set; } = true;

		public ConstellationReaction Banter { get; set; }

		public SceneRewards Rewards { get; set; }

		public string NextCueGuid { get; set; }

		public string SpeakerGuid { get; set; }

		public GameAction CueOnStopAction { get; set; }

		public ActionList CueOnStopActionList { get; set; }

		public string ReplyCueId { get; set; }

		public List<NarrativeCueDefinition> ChainedReplies { get; set; }

		public string ExcludeVanillaAnswerGuid { get; set; }

		public GameAction CustomAction { get; set; }

		public ActionList CustomActionList { get; set; }

		public Action<BlueprintAnswer> ConfigureAnswer { get; set; }

		public Action<BlueprintCue> ConfigureReply { get; set; }
	}
}
