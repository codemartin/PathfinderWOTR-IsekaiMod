using System;
using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace IsekaiMod.Content.Dialogue;

[TypeId("5b0f3f1e8a6d4c43a1f2e9d7c4b8a611")]
public class ActionGiveOtherworlderRewards : IsekaiGameAction
{
	public string ClaimKey;

	public int Coins;

	public string Sponsor;

	public int Gold;

	public int ExpCr;

	public BlueprintItemReference[] Items;

	public BlueprintBuffReference Buff;

	public int BuffMinutes;

	public BlueprintFeatureReference Feature;

	public string SponsorLine;

	private static readonly HashSet<string> OnlyOnceItems = new HashSet<string>(StringComparer.Ordinal) { "ItemAmuletOfOtherworldEquilibrium" };

	public override string GetCaption()
	{
		return "Gives the Otherworlder reward '" + ClaimKey + "' once per save";
	}

	internal static bool ClaimIn(IDictionary<string, string> ledger, string claimKey)
	{
		if (string.IsNullOrEmpty(claimKey))
		{
			return false;
		}
		return ConstellationChatManager.ClaimSceneRewardIn(ledger, claimKey);
	}

	internal static bool IsOnlyOnce(string itemName)
	{
		if (itemName != null)
		{
			return OnlyOnceItems.Contains(itemName);
		}
		return false;
	}

	internal static string OnlyOnceKey(string itemName)
	{
		return "Port62:OnlyOnceItem:" + itemName;
	}

	internal static bool MayHandOver(Func<string, bool> claim, string itemName)
	{
		if (IsOnlyOnce(itemName))
		{
			return claim(OnlyOnceKey(itemName));
		}
		return true;
	}

	internal static TimeSpan? BuffDuration(int minutes)
	{
		if (minutes <= 0)
		{
			return null;
		}
		return TimeSpan.FromMinutes(minutes);
	}

	protected override void RunSafely()
	{
		Player player = Game.Instance?.Player;
		if (player == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(ClaimKey))
		{
			Main.IsekaiContext.Logger.LogError("ActionGiveOtherworlderRewards has no claim key and pays nothing.");
		}
		else
		{
			if (!ConstellationChatManager.TryClaimSceneReward(ClaimKey))
			{
				return;
			}
			UnitEntityData main = player.MainCharacter.Value;
			RunSteps(new List<KeyValuePair<string, Action>>
			{
				Step("gold", delegate
				{
					if (Gold > 0)
					{
						player.GainMoney(Gold);
					}
				}),
				Step("experience", delegate
				{
					if (ExpCr > 0)
					{
						player.GainPartyExperience(ExperienceHelper.GetXp(EncounterType.SkillCheck, ExpCr));
					}
				}),
				Step("items", delegate
				{
					if (Items != null)
					{
						BlueprintItemReference[] items = Items;
						for (int i = 0; i < items.Length; i++)
						{
							BlueprintItem blueprintItem = items[i]?.Get();
							if (blueprintItem == null)
							{
								Main.IsekaiContext.Logger.LogError("ActionGiveOtherworlderRewards '" + ClaimKey + "': a listed item does not resolve and is not handed over.");
							}
							else if (!MayHandOver(ConstellationChatManager.TryClaimSceneReward, blueprintItem.name))
							{
								Main.IsekaiContext.Logger.Log("ActionGiveOtherworlderRewards '" + ClaimKey + "': '" + blueprintItem.name + "' was already handed over by another answer and is not given twice.");
							}
							else
							{
								player.Inventory.Add(blueprintItem, 1);
							}
						}
					}
				}),
				Step("buff", delegate
				{
					BlueprintBuff blueprintBuff = Buff?.Get();
					if (blueprintBuff != null && main != null)
					{
						main.Descriptor.Buffs.AddBuff(blueprintBuff, main, BuffDuration(BuffMinutes));
					}
				}),
				Step("feature", delegate
				{
					BlueprintFeature blueprintFeature = Feature?.Get();
					if (blueprintFeature != null && main != null && !main.Progression.Features.HasFact(blueprintFeature))
					{
						main.Descriptor.Progression.Features.AddFeature(blueprintFeature);
					}
				}),
				Step("coins", delegate
				{
					if (Coins > 0)
					{
						DivineTokens.AddClaimedCoins(Coins, Sponsor);
					}
				}),
				Step("sponsor line", delegate
				{
					if (!string.IsNullOrEmpty(SponsorLine))
					{
						ConstellationChatManager.PostLog(SponsorLine, Sponsor, ConstellationCategory.Subclass, DivineTokens.ShownCoins(Coins), ClaimKey);
					}
				})
			}, delegate(string step, Exception ex)
			{
				Main.IsekaiContext.Logger.LogError($"ActionGiveOtherworlderRewards '{ClaimKey}': the {step} step failed and the rest was still paid: {ex}");
			});
		}
	}

	private static KeyValuePair<string, Action> Step(string name, Action action)
	{
		return new KeyValuePair<string, Action>(name, action);
	}

	internal static int RunSteps(IEnumerable<KeyValuePair<string, Action>> steps, Action<string, Exception> onFailure)
	{
		int num = 0;
		foreach (KeyValuePair<string, Action> step in steps)
		{
			try
			{
				step.Value();
			}
			catch (Exception arg)
			{
				num++;
				onFailure?.Invoke(step.Key, arg);
			}
		}
		return num;
	}
}
