using System.Collections.Generic;

namespace IsekaiMod.Content.Narrative
{
	public class CycleVariation
	{
		public int MinCycle = 1;

		public int MaxCycle = int.MaxValue;

		public string VariantText;

		public string VariantReplyText;

		public List<string> VariantBanterLines = new List<string>();
	}
}
