using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints.Root;
using Kingmaker.Controllers.Dialog;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.UI.MVVM._VM.Dialog.Dialog;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Utilities;

[HarmonyPatch]
internal static class DialogueSoftlockGuardPatch
{
	private static readonly DialogueGuardLogic.OncePerKeyLog Reported = new DialogueGuardLogic.OncePerKeyLog(64);

	[HarmonyPatch(typeof(BlueprintAnswer), "CanSelect")]
	[HarmonyFinalizer]
	public static Exception BlueprintAnswer_CanSelect_Finalizer(Exception __exception, BlueprintAnswer __instance, ref bool __result)
	{
		if (__exception == null)
		{
			return null;
		}
		__result = false;
		ReportOnce("CanSelect:" + NameOf(__instance), () => $"Dialogue guard: answer {NameOf(__instance)} threw from CanSelect() and was greyed out instead of losing the window. {__exception}");
		return null;
	}

	[HarmonyPatch(typeof(BlueprintAnswer), "CanShow")]
	[HarmonyFinalizer]
	public static Exception BlueprintAnswer_CanShow_Finalizer(Exception __exception, BlueprintAnswer __instance, ref bool __result)
	{
		if (__exception == null)
		{
			return null;
		}
		__result = false;
		ReportOnce("CanShow:" + NameOf(__instance), () => $"Dialogue guard: answer {NameOf(__instance)} threw from CanShow() and was hidden instead of losing the window. {__exception}");
		return null;
	}

	[HarmonyPatch(typeof(DialogController), "PlayCue")]
	[HarmonyFinalizer]
	public static Exception DialogController_PlayCue_Finalizer(Exception __exception, BlueprintCueBase cue)
	{
		if (__exception == null)
		{
			return null;
		}
		string name = ((cue != null) ? cue.name : "<null cue>");
		bool flag = false;
		try
		{
			DialogController dialogController = Game.Instance?.DialogController;
			int controllerAnswers = ((dialogController?.Answers != null) ? dialogController.Answers.Count() : 0);
			if (DialogueGuardLogic.NeedsDialogStop(threw: true, dialogController?.Dialog != null, controllerAnswers))
			{
				dialogController.StopDialog();
				flag = true;
			}
		}
		catch (Exception arg)
		{
			SafeLog($"Dialogue guard: could not stop the dialog after a PlayCue failure: {arg}");
		}
		bool wasStopped = flag;
		ReportOnce("PlayCue:" + CurrentDialogName() + ":" + name, () => "Dialogue guard: " + CurrentDialogName() + " threw while playing cue " + name + "; " + (wasStopped ? "the dialog had no answers left and was stopped so the player is not stuck. " : "the conversation was left on the cue it was already showing. ") + __exception);
		return null;
	}

	[HarmonyPatch(typeof(DialogVM), "HandleOnCueShow")]
	[HarmonyFinalizer]
	public static Exception DialogVM_HandleOnCueShow_Finalizer(Exception __exception, DialogVM __instance, CueShowData data)
	{
		if (__exception == null || __instance == null)
		{
			return null;
		}
		try
		{
			RepairWindow(__instance, data, __exception);
		}
		catch (Exception arg)
		{
			SafeLog($"Dialogue guard: the repair itself failed after {__exception.GetType().Name}: {arg}");
		}
		return null;
	}

	private static void RepairWindow(DialogVM vm, CueShowData data, Exception exception)
	{
		DialogController controller = Game.Instance?.DialogController;
		string dialog = CurrentDialogName();
		string cueName = ((data?.Cue != null) ? data.Cue.name : "<null cue>");
		int survivingAnswers = 0;
		try
		{
			List<AnswerVM> list = vm.Answers?.Value;
			if (list != null)
			{
				survivingAnswers = list.Count((AnswerVM a) => a != null);
			}
		}
		catch
		{
			survivingAnswers = 0;
		}
		DialogueGuardLogic.AnswerPlan<AnswerVM> answerPlan = new DialogueGuardLogic.AnswerPlan<AnswerVM>();
		if (DialogueGuardLogic.NeedsAnswerRepair(threw: true, survivingAnswers))
		{
			answerPlan = DialogueGuardLogic.BuildAnswers(OfferedAnswers(controller), (BlueprintAnswer answer) => answer?.name, (BlueprintAnswer answer, int index) => new AnswerVM(answer, controller, index), () => BuildExitAnswer(controller));
			if (!answerPlan.Empty)
			{
				vm.Answers.Value = answerPlan.Answers;
			}
		}
		bool cuePresent;
		try
		{
			cuePresent = vm.Cue?.Value != null;
		}
		catch
		{
			cuePresent = false;
		}
		if (DialogueGuardLogic.NeedsCueRepair(threw: true, cuePresent))
		{
			string cueText = string.Empty;
			try
			{
				if (data?.Cue != null)
				{
					cueText = data.Cue.DisplayText ?? string.Empty;
				}
			}
			catch
			{
				cueText = string.Empty;
			}
			vm.Cue.Value = new CueVM(cueText, new List<SkillCheckResult>(), new List<AlignmentShift>());
		}
		int kept = 0;
		try
		{
			kept = (vm.Answers?.Value?.Count).GetValueOrDefault();
		}
		catch
		{
			kept = 0;
		}
		string key = "HandleOnCueShow:" + dialog + ":" + cueName;
		DialogueGuardLogic.AnswerPlan<AnswerVM> captured = answerPlan;
		ReportOnce(key, () => DialogueGuardLogic.FailureLine(dialog, cueName, kept, captured.UsedFallback, captured.Skipped, exception));
	}

	private static IEnumerable<BlueprintAnswer> OfferedAnswers(DialogController controller)
	{
		try
		{
			return controller?.Answers?.OfType<BlueprintAnswer>().ToList();
		}
		catch
		{
			return null;
		}
	}

	private static AnswerVM BuildExitAnswer(DialogController controller)
	{
		BlueprintAnswer blueprintAnswer = BlueprintRoot.Instance?.Dialog?.ExitAnswer;
		if (blueprintAnswer != null)
		{
			return new AnswerVM(blueprintAnswer, controller, 0);
		}
		return null;
	}

	private static string CurrentDialogName()
	{
		try
		{
			BlueprintDialog blueprintDialog = Game.Instance?.DialogController?.Dialog;
			return (blueprintDialog != null) ? blueprintDialog.name : "<unknown dialog>";
		}
		catch
		{
			return "<unknown dialog>";
		}
	}

	private static string NameOf(BlueprintAnswer answer)
	{
		try
		{
			return (answer != null) ? answer.name : "<null answer>";
		}
		catch
		{
			return "<null answer>";
		}
	}

	private static void ReportOnce(string key, Func<string> line)
	{
		try
		{
			if (Reported.ShouldLog(key))
			{
				SafeLog(line());
			}
		}
		catch
		{
		}
	}

	private static void SafeLog(string message)
	{
		try
		{
			Main.Log(message);
		}
		catch
		{
		}
	}
}
