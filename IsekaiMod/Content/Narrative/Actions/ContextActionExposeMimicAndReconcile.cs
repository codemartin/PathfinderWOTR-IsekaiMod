using System;
using IsekaiMod.Content.Constellations;
using Kingmaker;
using Kingmaker.AreaLogic.Etudes;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Narrative.Actions
{
	public class ContextActionExposeMimicAndReconcile : ContextAction
	{
		public override string GetCaption()
		{
			return "Reconciles Hulrun and Ramien through exposure of demonic mimicry";
		}

		public override void RunAction()
		{
			try
			{
				BlueprintEtude blueprint = BlueprintTools.GetBlueprint<BlueprintEtude>("b5d3ae57a01a07440a3089452d91a391");
				BlueprintEtude blueprint2 = BlueprintTools.GetBlueprint<BlueprintEtude>("eb5658cc38d4432a9288cf7a0f273321");
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
				DivineTokens.AddCoins(400, "The Song of the Spheres");
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage("<color=#00FFFF><b>[Harmony of Kenabres]</b></color> Prelate Hulrun and High Priest Ramien join forces under your authority!");
				});
				ConstellationBounties.CompleteBounty("Desna_MarketSquare_Mercy");
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ContextActionExposeMimicAndReconcile: " + ex);
			}
		}
	}
}
