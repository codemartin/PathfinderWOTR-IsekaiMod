using System;
using IsekaiMod.Content.Constellations;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Narrative.Actions
{
	public class ContextActionCompleteBounty : ContextAction
	{
		public string BountyId;

		public override string GetCaption()
		{
			return "Completes Patron Bounty: " + BountyId;
		}

		public override void RunAction()
		{
			try
			{
				if (!string.IsNullOrEmpty(BountyId))
				{
					ConstellationBounties.CompleteBounty(BountyId);
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ContextActionCompleteBounty: " + ex);
			}
		}
	}
}
