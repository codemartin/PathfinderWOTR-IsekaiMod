using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Utilities;
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

internal sealed class Port62SceneGraft
{
	public string Answer;

	public string Scene;

	public string ListGuid;

	public GraftBeat Beat;

	public string ReplySpeaker;

	public bool Companion;

	public bool Narrated;

	public string Gate;

	public string Mythic;

	public string[] RequiredCompanions = new string[0];

	public AlignmentShiftDirection Shift = AlignmentShiftDirection.TrueNeutral;

	public int ExpCr = 2;

	public string ClaimKey;

	public int Coins;

	public string SixTwoSponsor;

	public int Gold;

	public string[] VanillaItems = new string[0];

	public string[] ModItems = new string[0];

	public string Buff;

	public int BuffMinutes;

	public string Feature;

	public string Sponsor => Port62Rewards.Seat(SixTwoSponsor);

	public bool HasReward => ClaimKey != null;

	internal static Port62SceneGraft Find(IEnumerable<Port62SceneGraft> table, string answer, string owner)
	{
		Port62SceneGraft port62SceneGraft = table.FirstOrDefault((Port62SceneGraft g) => g.Answer == answer);
		if (port62SceneGraft == null)
		{
			Main.IsekaiContext.Logger.LogError(owner + ": '" + answer + "' has no row in the table, skipping.");
		}
		return port62SceneGraft;
	}

	internal static Condition[] Gates(Port62SceneGraft graft, string owner)
	{
		if (graft == null)
		{
			return null;
		}
		if (graft.Beat == null || graft.Beat.HostList != GraftBeat.Normalize(graft.ListGuid))
		{
			Main.IsekaiContext.Logger.LogError(owner + ": '" + graft.Answer + "' has no beat on its list " + graft.ListGuid + ", skipping.");
			return null;
		}
		List<Condition> list = new List<Condition>();
		Condition condition = Port62Rewards.Gate(graft.Gate);
		if (condition == null)
		{
			Main.IsekaiContext.Logger.LogWarning(owner + ": gate " + graft.Gate + " not found, skipping " + graft.Answer + ".");
			return null;
		}
		list.Add(condition);
		if (!string.IsNullOrEmpty(graft.Mythic))
		{
			Condition condition2 = Port62Rewards.MythicGate(graft.Mythic);
			if (condition2 == null)
			{
				Main.IsekaiContext.Logger.LogWarning(owner + ": mythic class " + graft.Mythic + " not found, skipping " + graft.Answer + ".");
				return null;
			}
			list.Add(condition2);
		}
		string[] requiredCompanions = graft.RequiredCompanions;
		foreach (string text in requiredCompanions)
		{
			BlueprintUnit blueprint = BlueprintTools.GetBlueprint<BlueprintUnit>(text);
			if (blueprint == null)
			{
				Main.IsekaiContext.Logger.LogWarning(owner + ": companion " + text + " not found, skipping " + graft.Answer + ".");
				return null;
			}
			list.Add(new CompanionInParty
			{
				m_companion = blueprint.ToReference<BlueprintUnitReference>(),
				MatchWhenActive = true,
				MatchWhenRemote = true,
				MatchWhenDetached = true
			});
		}
		return list.ToArray();
	}

	internal static GameAction[] OnSelectActions(Port62SceneGraft graft, string sponsorLine)
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
		if (graft.HasReward)
		{
			list.Add(Port62Rewards.Reward(graft.ClaimKey, graft.Coins, graft.SixTwoSponsor, graft.Gold, graft.VanillaItems, graft.ModItems, graft.Buff, graft.BuffMinutes, graft.Feature, sponsorLine));
		}
		return list.ToArray();
	}

	internal static void Configure(BlueprintAnswer answer, Port62SceneGraft graft)
	{
		if (graft.Shift != AlignmentShiftDirection.TrueNeutral)
		{
			answer.AlignmentShift = new AlignmentShift
			{
				Direction = graft.Shift,
				Value = 1,
				Description = new LocalizedString()
			};
		}
	}
}
