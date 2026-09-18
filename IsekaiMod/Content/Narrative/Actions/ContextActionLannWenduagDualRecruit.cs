using System;
using IsekaiMod.Content.Constellations;
using Kingmaker;
using Kingmaker.AreaLogic.Etudes;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Narrative.Actions
{
	public class ContextActionLannWenduagDualRecruit : ContextAction
	{
		public override string GetCaption()
		{
			return "Recruits both Lann and Wenduag, defusing mutual betrayal flags";
		}

		public override void RunAction()
		{
			try
			{
				BlueprintEtude blueprint = BlueprintTools.GetBlueprint<BlueprintEtude>("37556ee0ee97783438d7670e03050f89");
				BlueprintEtude blueprint2 = BlueprintTools.GetBlueprint<BlueprintEtude>("bf341a4bb2fe61d44a73e5a72b54968f");
				EtudesSystem etudesSystem = Game.Instance?.Player?.EtudesSystem;
				if (etudesSystem != null)
				{
					if (blueprint != null && !etudesSystem.EtudeIsStarted(blueprint))
					{
						etudesSystem.StartEtude(blueprint);
					}
					if (blueprint2 != null && !etudesSystem.EtudeIsStarted(blueprint2))
					{
						etudesSystem.StartEtude(blueprint2);
					}
				}
				DivineTokens.AddCoins(500, "The Laughing King");
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage("<color=#FFD700><b>[The Omniscient Reincarnator]</b></color> <b>Lann and Wenduag</b> both pledge allegiance under your standard!");
				});
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ContextActionLannWenduagDualRecruit: " + ex);
			}
		}
	}
}
