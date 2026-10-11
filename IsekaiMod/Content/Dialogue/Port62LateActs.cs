using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Dialogue;

internal static class Port62LateActs
{
	internal sealed class Graft
	{
		public string Answer;

		public string Scene;

		public string ListGuid;

		public string Companion;

		public string Gate;

		public string MythicPath;

		public string DlcReward;

		public bool Narrated;

		public bool IzTriad;

		public AlignmentShiftDirection Shift = AlignmentShiftDirection.TrueNeutral;

		public int ExpCr = 2;

		public string ClaimKey;

		public int Coins;

		public string SixTwoSponsor;

		public int Gold;

		public string[] ModItems = new string[0];

		public string Changed;

		public string Sponsor => Port62Rewards.Seat(SixTwoSponsor);

		public bool HasReward => ClaimKey != null;
	}

	internal enum ReplyShape
	{
		DialogSpeaker,
		Narrated,
		Companion
	}

	internal static ReplyShape ShapeOf(Graft graft)
	{
		if (string.IsNullOrEmpty(graft.Companion))
		{
			if (!graft.Narrated)
			{
				return ReplyShape.DialogSpeaker;
			}
			return ReplyShape.Narrated;
		}
		return ReplyShape.Companion;
	}

	internal static Graft Find(IEnumerable<Graft> table, string name, GraftBeat beat, string owner, ReplyShape shape)
	{
		Graft graft = table.FirstOrDefault((Graft g) => g.Answer == name);
		string text = ((graft == null) ? "has no row in the table" : ((beat == null || beat.HostList != GraftBeat.Normalize(graft.ListGuid)) ? ("is pinned to a beat that is not on its list " + graft.ListGuid) : ((ShapeOf(graft) != shape) ? ("is built as " + shape.ToString() + " but its row asks for " + ShapeOf(graft)) : null)));
		if (text != null)
		{
			Main.IsekaiContext.Logger.LogError(owner + ": '" + name + "' " + text + ", skipping.");
			return null;
		}
		return graft;
	}

	internal static Condition[] ShowConditions(Graft graft)
	{
		List<Condition> list = new List<Condition>();
		Condition condition = Port62Rewards.Gate(graft.Gate);
		if (condition == null)
		{
			return Refuse(graft, "gate " + graft.Gate);
		}
		list.Add(condition);
		if (graft.MythicPath != null)
		{
			Condition condition2 = Port62Rewards.MythicGate(Port62Rewards.MythicClassFor(graft.MythicPath));
			if (condition2 == null)
			{
				return Refuse(graft, "mythic path " + graft.MythicPath);
			}
			list.Add(condition2);
		}
		if (graft.DlcReward != null)
		{
			list.Add(IsekaiDlcManager.CreateDlcCondition(graft.DlcReward));
		}
		return list.ToArray();
	}

	private static Condition[] Refuse(Graft graft, string what)
	{
		Main.IsekaiContext.Logger.LogWarning("Port62LateActs: " + what + " not found, skipping " + graft.Answer + ".");
		return null;
	}

	internal static GameAction[] OnSelectActions(Graft graft, string sponsorLine)
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
		if (graft.IzTriad)
		{
			list.Add(new IsekaiActionIzTriadMultiFrontDefense());
		}
		if (graft.HasReward)
		{
			list.Add(Port62Rewards.Reward(graft.ClaimKey, graft.Coins, graft.SixTwoSponsor, graft.Gold, null, graft.ModItems, null, 0, null, sponsorLine));
		}
		return list.ToArray();
	}

	internal static void Configure(BlueprintAnswer answer, Graft graft)
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
