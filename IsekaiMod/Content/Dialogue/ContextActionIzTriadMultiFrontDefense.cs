using System;
using IsekaiMod.Content.Constellations;
using Kingmaker;
using Kingmaker.AreaLogic.Etudes;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue
{
	public class ContextActionIzTriadMultiFrontDefense : ContextAction
	{
		public override string GetCaption()
		{
			return "Saves Queen Galfrey, Irabeth, and the Sword of Valor simultaneously at Iz";
		}

		public override void RunAction()
		{
			try
			{
				BlueprintEtude blueprint = BlueprintTools.GetBlueprint<BlueprintEtude>("caf51287bfd0152489d3a6fe4700060c");
				BlueprintEtude blueprint2 = BlueprintTools.GetBlueprint<BlueprintEtude>("edb31f5ddae9420f8355d7e34faf6522");
				BlueprintEtude blueprint3 = BlueprintTools.GetBlueprint<BlueprintEtude>("da115f562dbf83d40997b1dc14cac60e");
				BlueprintEtude blueprint4 = BlueprintTools.GetBlueprint<BlueprintEtude>("0fadb7da2ccab2643b29c0fa57329531");
				BlueprintEtude blueprint5 = BlueprintTools.GetBlueprint<BlueprintEtude>("b6b8d18623c54441a13e3764ba9b6d61");
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
					if (blueprint4 != null && etudesSystem.EtudeIsStarted(blueprint4))
					{
						etudesSystem.UnstartEtude(blueprint4);
					}
					if (blueprint3 != null && !etudesSystem.EtudeIsStarted(blueprint3))
					{
						etudesSystem.StartEtude(blueprint3);
					}
					if (blueprint5 != null && !etudesSystem.EtudeIsStarted(blueprint5))
					{
						etudesSystem.StartEtude(blueprint5);
					}
				}
				DivineTokens.AddCoins(500, "The Inheritor");
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage("<color=#FFD700><b>[The Golden Triumph of Iz]</b></color> Queen Galfrey, Irabeth, and the Sword of Valor are all triumphantly preserved!");
				});
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ContextActionIzTriadMultiFrontDefense: " + ex);
			}
		}
	}
}
