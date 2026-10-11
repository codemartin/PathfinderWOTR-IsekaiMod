using System.Collections.Generic;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;

namespace IsekaiMod.Content.Dialogue;

internal sealed class Port62SceneBookGraft
{
	public string Answer;

	public string PageGuid;

	public string NextPageGuid;

	public string Gate;

	public int ExpCr = 5;

	public string ClaimKey;

	public int Coins;

	public string SixTwoSponsor;

	public int Gold;

	public string[] VanillaItems = new string[0];

	public string[] ModItems = new string[0];

	public string Sponsor => Port62Rewards.Seat(SixTwoSponsor);

	internal static GameAction[] OnSelectActions(Port62SceneBookGraft graft, string sponsorLine)
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
		if (graft.ClaimKey != null)
		{
			list.Add(Port62Rewards.Reward(graft.ClaimKey, graft.Coins, graft.SixTwoSponsor, graft.Gold, graft.VanillaItems, graft.ModItems, null, 0, null, sponsorLine));
		}
		return list.ToArray();
	}
}
