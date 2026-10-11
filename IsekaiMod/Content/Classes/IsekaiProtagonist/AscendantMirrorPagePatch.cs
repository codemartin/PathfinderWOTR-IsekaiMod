using System;
using HarmonyLib;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UI.MVVM._VM.CharGen.Phases;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.Class;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UnitLogic.Class.LevelUp;
using Owlcat.Runtime.UI.Tooltips;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist;

[HarmonyPatch]
internal static class AscendantMirrorPagePatch
{
	[HarmonyPrepare]
	private static bool Prepare()
	{
		return PatchGuard.ResolvesAll(typeof(AscendantMirrorPagePatch), (typeof(CharGenClassPhaseVM), "CheckIsCompleted"), (typeof(CharGenClassPhaseVM), "get_NotCompletedReasonTooltip"));
	}

	private static bool MissingMirror(CharGenClassPhaseVM phase)
	{
		LevelUpController levelUpController = ((CharGenPhaseBaseVM)phase)?.LevelUpController;
		if (levelUpController?.State == null)
		{
			return false;
		}
		BlueprintArchetype mirror;
		return AscendantMirrorLock.IsMissingMirror(levelUpController.State, levelUpController.Preview?.Descriptor, out mirror);
	}

	[HarmonyPatch(typeof(CharGenClassPhaseVM), "CheckIsCompleted")]
	[HarmonyPostfix]
	private static void CheckIsCompleted_Postfix(CharGenClassPhaseVM __instance, ref bool __result)
	{
		try
		{
			if (__result && MissingMirror(__instance))
			{
				__result = false;
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[AscendantMirrorLock] could not judge the class page. {arg}");
		}
	}

	[HarmonyPatch(typeof(CharGenClassPhaseVM), "NotCompletedReasonTooltip", MethodType.Getter)]
	[HarmonyPostfix]
	private static void NotCompletedReasonTooltip_Postfix(CharGenClassPhaseVM __instance, ref TooltipBaseTemplate __result)
	{
		try
		{
			if (__result == null && AscendantMirrorLock.MirrorRequiredText != null && MissingMirror(__instance))
			{
				__result = new TooltipTemplateSimple(AscendantMirrorLock.MirrorRequiredText);
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[AscendantMirrorLock] could not explain the class page. {arg}");
		}
	}
}
