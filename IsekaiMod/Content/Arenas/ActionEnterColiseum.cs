using System;
using Kingmaker;
using Kingmaker.Blueprints.Area;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Persistence;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Arenas
{
	public class ActionEnterColiseum : GameAction
	{
		public const string ArenaEnterPointGuid = "f3e32ac1e70ccda429fb08bbadf9a116";

		public const string DefendersHeartEnterGuid = "81b33df4ecc0f5e44abbb3b66e2e9943";

		public const string WarCampEnterGuid = "e5272ff75e9253d49b358be1af6a0415";

		public const string DrezenCapitalEnterGuid = "cdaba43118a82ff429b803ae846b35b0";

		public override string GetCaption()
		{
			return "Enter Planar Coliseum";
		}

		public override void RunAction()
		{
			try
			{
				string text = Game.Instance?.CurrentlyLoadedArea?.name ?? "";
				if (text.IndexOf("AreshkaArena", StringComparison.OrdinalIgnoreCase) < 0)
				{
					string coliseumReturnEnterPointGuid = "cdaba43118a82ff429b803ae846b35b0";
					if (text.IndexOf("DefendersHeart", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						coliseumReturnEnterPointGuid = "81b33df4ecc0f5e44abbb3b66e2e9943";
					}
					else if (text.IndexOf("WarCamp", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						coliseumReturnEnterPointGuid = "e5272ff75e9253d49b358be1af6a0415";
					}
					else if (text.IndexOf("Drezen", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						coliseumReturnEnterPointGuid = "cdaba43118a82ff429b803ae846b35b0";
					}
					PlanarColiseumHubManager.ColiseumReturnEnterPointGuid = coliseumReturnEnterPointGuid;
				}
				BlueprintAreaEnterPoint blueprint = BlueprintTools.GetBlueprint<BlueprintAreaEnterPoint>("f3e32ac1e70ccda429fb08bbadf9a116");
				if (blueprint != null)
				{
					Game.Instance.LoadArea(blueprint, AutoSaveMode.None, delegate
					{
					});
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error transitioning into Planar Coliseum: " + ex);
			}
		}
	}
}
