using System;
using System.Collections.Generic;
using IsekaiMod.Content.KingmakerIntegration;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("8f2d6a4c0e1b4d7f9a3c5e7b2d4f6a81")]
public class ContextActionLanternKingReveal : IsekaiContextAction
{
	public override string GetCaption()
	{
		return "The Lantern King reveals himself (once per save) and the constellations react";
	}

	protected override void RunSafely()
	{
		Dictionary<string, string> dictionary = ColiseumProgress.Ensure();
		if (dictionary != null && !LanternKingReveal.IsRevealed(dictionary))
		{
			SaveScopedStorage.SetFlag(dictionary, "LanternKing.Revealed", value: true);
			bool flag = false;
			string rulerName = null;
			try
			{
				flag = KingmakerCrossSaveManager.IsKingmakerInstalled;
				rulerName = (flag ? KingmakerCrossSaveManager.GetRulerName() : null);
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("[LanternKing] Could not read the Kingmaker ruler: " + ex);
			}
			LanternKingReveal.PostVoices(LanternKingReveal.RevealReactions(flag, rulerName));
		}
	}
}
