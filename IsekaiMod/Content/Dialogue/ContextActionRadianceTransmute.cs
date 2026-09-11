using System;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Dialogue
{
	public class ContextActionRadianceTransmute : ContextAction
	{
		public RadianceForm TargetForm;

		public override string GetCaption()
		{
			return $"Transmutes Radiance into {TargetForm}";
		}

		public override void RunAction()
		{
			try
			{
				IsekaiRadiance.TransmutePlayerRadiance(TargetForm);
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ContextActionRadianceTransmute: " + ex);
			}
		}
	}
}
