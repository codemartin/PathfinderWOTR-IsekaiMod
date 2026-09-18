using System.Collections.Generic;

namespace IsekaiMod.Content.Narrative
{
	public class NarrativeScene
	{
		public string SceneId { get; set; }

		public string TargetAnswersListGuid { get; set; }

		public string Act { get; set; }

		public string Description { get; set; }

		public List<NarrativeOption> Options { get; set; } = new List<NarrativeOption>();

		public NarrativeScene(string sceneId, string targetAnswersListGuid, string act, string description)
		{
			SceneId = sceneId;
			TargetAnswersListGuid = targetAnswersListGuid;
			Act = act;
			Description = description;
		}

		public NarrativeOption AddOption(NarrativeOption option)
		{
			Options.Add(option);
			return option;
		}
	}
}
