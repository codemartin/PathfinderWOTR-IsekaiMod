using System;
using IsekaiMod.Content.Features.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Arenas;

internal static class ColiseumGates
{
	internal enum TravelRefusal
	{
		None,
		CapitalLocked,
		InCombat,
		Cutscene
	}

	internal const string AllowCapitalExitEtudeGuid = "e86070a0a95c1e9429037567e18cd404";

	internal const string DHReadyForAttackEtudeGuid = "897cefd681c79c54fa70f2dcfac47a64";

	internal const string DuringDHAttackedEtudeGuid = "3a6a5a69121d2124a9a92f4385a9fe46";

	internal static TravelRefusal RefusalFor(bool inCapital, bool capitalExitAllowed, bool inCombat, bool cutscene)
	{
		if (inCombat)
		{
			return TravelRefusal.InCombat;
		}
		if (cutscene)
		{
			return TravelRefusal.Cutscene;
		}
		if (inCapital && !capitalExitAllowed)
		{
			return TravelRefusal.CapitalLocked;
		}
		return TravelRefusal.None;
	}

	internal static string RefusalMessage(TravelRefusal refusal)
	{
		return refusal switch
		{
			TravelRefusal.CapitalLocked => "The way out of the capital is held shut right now; the rift will not open until the city lets you leave.", 
			TravelRefusal.InCombat => "The rift will not open while a fight is underway.", 
			TravelRefusal.Cutscene => "The rift will not open in the middle of a scene.", 
			_ => "", 
		};
	}

	internal static TravelRefusal CurrentRefusal()
	{
		Game instance = Game.Instance;
		if (instance == null)
		{
			return TravelRefusal.None;
		}
		bool num = string.Equals(instance.CurrentlyLoadedArea?.name, "DrezenCapital", StringComparison.OrdinalIgnoreCase);
		bool capitalExitAllowed = true;
		if (num)
		{
			capitalExitAllowed = ((Condition)new EtudeStatus
			{
				m_Etude = BlueprintTools.GetBlueprintReference<BlueprintEtudeReference>("e86070a0a95c1e9429037567e18cd404"),
				Playing = true
			}).CheckCondition();
		}
		return RefusalFor(num, capitalExitAllowed, instance.Player?.IsInCombat ?? false, CampaignEncounterGuard.IsCutscene());
	}

	internal static bool RefuseTravel(string what)
	{
		TravelRefusal travelRefusal = CurrentRefusal();
		if (travelRefusal == TravelRefusal.None)
		{
			return false;
		}
		Main.IsekaiContext.Logger.Log($"[PlanarColiseum] {what} refused: {travelRefusal}.");
		PlanarColiseumHubManager.PostLog(RefusalMessage(travelRefusal));
		return true;
	}

	internal static void AddSceneStateGate(BlueprintAnswer answer)
	{
		answer.AddShowCondition<ConditionSceneAllowsTravel>();
	}

	internal static void AddDefendersHeartSiegeGate(BlueprintAnswer answer)
	{
		string[] array = new string[2] { "897cefd681c79c54fa70f2dcfac47a64", "3a6a5a69121d2124a9a92f4385a9fe46" };
		foreach (string etude in array)
		{
			answer.AddShowCondition(delegate(EtudeStatus c)
			{
				c.m_Etude = BlueprintTools.GetBlueprintReference<BlueprintEtudeReference>(etude);
				c.Playing = true;
				c.Not = true;
			});
		}
	}
}
