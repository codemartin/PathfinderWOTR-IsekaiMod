using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker.AreaLogic.Etudes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Alignments;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue;

internal static class Port62Grafting
{
	internal static string Problem(Port62Graft graft)
	{
		if (graft == null)
		{
			return "has no row in the table";
		}
		if (graft.Beat == null || graft.Beat.HostList != GraftBeat.Normalize(graft.ListGuid))
		{
			return "has no beat on its list " + graft.ListGuid;
		}
		if (graft.FallBackToList && string.IsNullOrEmpty(graft.ContinueCueGuid))
		{
			return "falls back to its list but continues nowhere";
		}
		return null;
	}

	internal static Condition[] Conditions(Port62Graft graft)
	{
		List<Condition> list = new List<Condition>();
		Condition condition = Port62Rewards.Gate(graft.Gate);
		if (condition == null)
		{
			return null;
		}
		list.Add(condition);
		Condition[] array = EtudeConditions(graft, (string id) => BlueprintTools.GetBlueprint<BlueprintEtude>(id), delegate(string message)
		{
			Main.IsekaiContext.Logger.LogError(message);
		});
		if (array == null)
		{
			return null;
		}
		list.AddRange(array);
		return list.ToArray();
	}

	internal static Condition[] EtudeConditions(Port62Graft graft, Func<string, object> lookup, Action<string> error)
	{
		List<Condition> list = new List<Condition>();
		string[] whilePlaying = graft.WhilePlaying;
		foreach (string text in whilePlaying)
		{
			if (!(lookup(text) is BlueprintEtude bp))
			{
				error("Port62Grafting: " + graft.Answer + " needs etude " + text + " playing, which does not resolve; the answer is skipped.");
				return null;
			}
			list.Add(new EtudeStatus
			{
				m_Etude = bp.ToReference<BlueprintEtudeReference>(),
				Playing = true
			});
		}
		whilePlaying = graft.UnlessPlaying;
		foreach (string text2 in whilePlaying)
		{
			if (!(lookup(text2) is BlueprintEtude bp2))
			{
				error("Port62Grafting: " + graft.Answer + " is hidden while etude " + text2 + " plays, which does not resolve; the answer is skipped.");
				return null;
			}
			list.Add(new EtudeStatus
			{
				Not = true,
				m_Etude = bp2.ToReference<BlueprintEtudeReference>(),
				Playing = true
			});
		}
		return list.ToArray();
	}

	internal static void Configure(Port62Graft graft, BlueprintAnswer bp)
	{
		if (graft.Shift != AlignmentShiftDirection.TrueNeutral)
		{
			bp.AlignmentShift = new AlignmentShift
			{
				Direction = graft.Shift,
				Value = 1,
				Description = new LocalizedString()
			};
		}
	}

	internal static GameAction[] OnSelect(Port62Graft graft, string sponsorLine)
	{
		List<GameAction> list = new List<GameAction>();
		if (graft.ExpCr > 0)
		{
			list.Add(new GainExp
			{
				Encounter = EncounterType.SkillCheck,
				CR = graft.ExpCr,
				Modifier = 1f
			});
		}
		list.AddRange(VanillaActions(graft.VanillaActionsFrom));
		if (graft.ReconcileHulrunAndRamien)
		{
			list.Add(new IsekaiActionExposeMimicAndReconcile());
		}
		if (graft.HasReward)
		{
			list.Add(Port62Rewards.Reward(graft.ClaimKey, graft.Coins, graft.SixTwoSponsor, graft.Gold, graft.VanillaItems, graft.ModItems, graft.Buff, graft.BuffMinutes, graft.Feature, sponsorLine));
		}
		return list.ToArray();
	}

	internal static IEnumerable<GameAction> VanillaActions(IEnumerable<string> ids)
	{
		foreach (string item in ids ?? new string[0])
		{
			BlueprintScriptableObject blueprint = BlueprintTools.GetBlueprint<BlueprintScriptableObject>(item);
			ActionList actionList = null;
			if (blueprint is BlueprintAnswer blueprintAnswer)
			{
				actionList = blueprintAnswer.OnSelect;
			}
			else if (blueprint is BlueprintCue blueprintCue)
			{
				actionList = blueprintCue.OnShow;
			}
			if (actionList == null)
			{
				Main.IsekaiContext.Logger.LogError("Port62Grafting: " + item + " is neither an answer nor a cue with actions.");
				continue;
			}
			GameAction[] array = actionList.Actions ?? new GameAction[0];
			foreach (GameAction gameAction in array)
			{
				if (gameAction != null)
				{
					yield return gameAction;
				}
			}
		}
	}

	internal static void Exclude(Port62Graft graft, BlueprintAnswer answer, IDictionary<string, BlueprintAnswer> built)
	{
		if (answer == null)
		{
			return;
		}
		string[] exclusiveWith = graft.ExclusiveWith;
		foreach (string text in exclusiveWith)
		{
			BlueprintAnswer blueprint = BlueprintTools.GetBlueprint<BlueprintAnswer>(text);
			if (blueprint == null)
			{
				Main.IsekaiContext.Logger.LogError("Port62Grafting: " + graft.Answer + " excludes " + text + ", which is not an answer.");
			}
			else
			{
				ExtensionMethods.HideEachOther(answer, blueprint);
			}
		}
		exclusiveWith = graft.ExclusiveWithPorted;
		foreach (string key in exclusiveWith)
		{
			if (built.TryGetValue(key, out var value))
			{
				ExtensionMethods.HideEachOther(answer, value);
			}
		}
	}

	internal static GameAction[] BookOnSelect(Port62BookGraft graft, string sponsorLine)
	{
		List<GameAction> list = new List<GameAction>
		{
			new GainExp
			{
				Encounter = EncounterType.SkillCheck,
				CR = graft.ExpCr,
				Modifier = 1f
			}
		};
		if (graft.MirrorsAnswer != null)
		{
			list.AddRange(VanillaActions(new string[1] { graft.MirrorsAnswer }));
		}
		list.Add(Port62Rewards.Reward(graft.ClaimKey, graft.Coins, graft.SixTwoSponsor, 0, null, graft.ModItems, graft.Buff, graft.BuffMinutes, graft.Feature, sponsorLine));
		return GuardedGameAction.GuardAll(list.ToArray());
	}

	internal static Condition[] MirroredConditions(Port62BookGraft graft)
	{
		if (graft.MirrorsAnswer == null)
		{
			return new Condition[0];
		}
		return BlueprintTools.GetBlueprint<BlueprintAnswer>(graft.MirrorsAnswer)?.ShowConditions?.Conditions?.Where((Condition c) => c != null).ToArray() ?? new Condition[0];
	}
}
