using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Dialogue;
using IsekaiMod.Content.Features;
using IsekaiMod.Content.Features.IsekaiProtagonist;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestKeptRewardsChecks
{
	private sealed class Target
	{
		public BlueprintUnitFact Fact;

		public string GrantedBy;

		public bool PermanentBuff;
	}

	private sealed class EntityFactHolder
	{
		private readonly EntityFact fact;

		internal EntityFactHolder(EntityFact fact)
		{
			this.fact = fact;
		}

		internal void Remove(UnitEntityData unit)
		{
			if (fact == null)
			{
				return;
			}
			try
			{
				unit.Descriptor.RemoveFact(fact);
			}
			catch
			{
			}
		}
	}

	private static readonly MethodInfo SurvivesRespecMethod = AccessTools.Method(typeof(UnitHelper), "IsFeatureSurvivesRespec", new Type[1] { typeof(Feature) });

	private static string FieldOf(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return "";
		}
		int num = path.LastIndexOf('.');
		if (num >= 0)
		{
			return path.Substring(num + 1);
		}
		return path;
	}

	private static List<Target> CollectTargets(List<string> notes, out int recordedCheats)
	{
		Dictionary<BlueprintUnitFact, Target> targets = new Dictionary<BlueprintUnitFact, Target>();
		recordedCheats = 0;
		foreach (SelfTestRecorder.CreatedBlueprint item in SelfTestRecorder.CreatedBlueprints())
		{
			if (item.Blueprint == null)
			{
				continue;
			}
			List<BlueprintReferenceWalker.Found> list;
			try
			{
				list = BlueprintReferenceWalker.ReferencesIn(item.Blueprint);
			}
			catch (Exception ex)
			{
				notes.Add(item.Name + ": its references could not be walked (" + ex.GetType().Name + ").");
				continue;
			}
			foreach (BlueprintReferenceWalker.Found item2 in list)
			{
				if (item2.Reference == null)
				{
					continue;
				}
				string text = FieldOf(item2.Path);
				string text2 = item.Name + " (" + item2.Owner?.GetType().Name + ")";
				object owner = item2.Owner;
				if (!(owner is ContextActionSpendCoinsAndGrantFact contextActionSpendCoinsAndGrantFact))
				{
					if (!(owner is ContextActionAscendAvatar))
					{
						if (!(owner is ActionGiveOtherworlderRewards actionGiveOtherworlderRewards))
						{
							if (!(owner is ContextActionConsumeSkillBook))
							{
								if (owner is ContextActionAddFeature && text == "m_PermanentFeature")
								{
									Add(item2.Reference.GetBlueprint(), text2, permanentBuff: false);
								}
							}
							else if (text == "m_Feature")
							{
								Add(item2.Reference.GetBlueprint(), text2, permanentBuff: false);
							}
						}
						else if (text == "Feature")
						{
							Add(item2.Reference.GetBlueprint(), text2 + " " + actionGiveOtherworlderRewards.ClaimKey, permanentBuff: false);
						}
						else
						{
							ActionGiveOtherworlderRewards actionGiveOtherworlderRewards2 = actionGiveOtherworlderRewards;
							if (text == "Buff" && KeptRewards.IsPermanentDuration(actionGiveOtherworlderRewards2.BuffMinutes))
							{
								Add(item2.Reference.GetBlueprint(), text2 + " " + actionGiveOtherworlderRewards2.ClaimKey, permanentBuff: true);
							}
						}
					}
					else if (text == "m_AvatarFeature")
					{
						Add(item2.Reference.GetBlueprint(), text2, permanentBuff: false);
					}
				}
				else if (text == "m_Fact")
				{
					if (contextActionSpendCoinsAndGrantFact.RecordForRespec)
					{
						recordedCheats++;
					}
					else
					{
						Add(item2.Reference.GetBlueprint(), text2, permanentBuff: false);
					}
				}
			}
		}
		Add(ShardOfTheShatteredLoop.Get(), "TimelineManager (the NG+ Shard)", permanentBuff: false);
		Add(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "OmnipotentSovereignsDieFeature"), "MerchantsGamble (the 6-Star prize)", permanentBuff: false);
		return targets.Values.OrderBy((Target t) => t.Fact.name, StringComparer.Ordinal).ToList();
		void Add(SimpleBlueprint blueprint, string grantedBy, bool permanentBuff)
		{
			if (blueprint is BlueprintUnitFact blueprintUnitFact)
			{
				if (!targets.TryGetValue(blueprintUnitFact, out var value))
				{
					targets[blueprintUnitFact] = new Target
					{
						Fact = blueprintUnitFact,
						GrantedBy = grantedBy,
						PermanentBuff = permanentBuff
					};
				}
				else if (permanentBuff)
				{
					value.PermanentBuff = true;
				}
			}
		}
	}

	internal static SelfTestChecks.Outcome RewardsSurvive()
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<Target> list3 = CollectTargets(list, out var recordedCheats);
		if (SurvivesRespecMethod == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "UnitHelper.IsFeatureSurvivesRespec is gone from the engine, so nothing here could be asked of it.", list);
		}
		int num = 0;
		int num2 = 0;
		string problem;
		using (SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem))
		{
			if (selfTestUnit == null)
			{
				return SelfTestChecks.Outcome.Of(passed: false, "No blank unit could be created: " + problem + ".");
			}
			UnitEntityData unit = selfTestUnit.Unit;
			foreach (Target item in list3)
			{
				try
				{
					if (item.Fact is BlueprintFeature feature)
					{
						num++;
						CheckFeature(unit, feature, item.GrantedBy, list2);
					}
					else if (item.Fact is BlueprintBuff blueprintBuff)
					{
						num2++;
						if (item.PermanentBuff)
						{
							CheckPermanentBuff(unit, blueprintBuff, item.GrantedBy, list2);
						}
						else if (blueprintBuff.IsClassFeature)
						{
							list2.Add(blueprintBuff.name + ", bought by " + item.GrantedBy + ", is a class feature: a respec drops it.");
						}
					}
				}
				catch (Exception ex)
				{
					list2.Add(item.Fact.name + ": the check threw " + ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message));
				}
			}
			CheckEmberCake(unit, list2);
		}
		int num3 = KeptRewards.PermanentBuffRewards().Count() + KeptRewards.FeatureRewards().Count();
		if (num3 == 0)
		{
			list2.Add("No claim-once reward was recorded in KeptRewards, so a save that lost one cannot be repaired.");
		}
		if (num == 0)
		{
			list2.Add("No bought or earned feature was found at all; the walk over the mod's blueprints found nothing to check.");
		}
		list.Insert(0, $"{num} features and {num2} buffs handed over outside class progression; {recordedCheats} Forbidden Cheat offers left to ForbiddenCheatRecord; {num3} claim-once rewards recorded for repair.");
		list.AddRange(list2);
		string summary = $"{num} bought or earned features survive a respec by the engine's own rule, and the permanent reward buffs survive a respec and a death; {list2.Count} problems.";
		return SelfTestChecks.Outcome.Of(list2.Count == 0, summary, list);
	}

	private static void CheckFeature(UnitEntityData unit, BlueprintFeature feature, string grantedBy, List<string> problems)
	{
		if (feature.IsClassFeature)
		{
			problems.Add(feature.name + ", handed over by " + grantedBy + ", is a class feature: a respec drops it.");
			return;
		}
		Feature feature2 = null;
		try
		{
			feature2 = unit.Descriptor.Progression.Features.AddFeature(feature);
			if (feature2 == null)
			{
				problems.Add(feature.name + ": the blank unit refused it, so the engine could not be asked.");
			}
			else if (!(bool)SurvivesRespecMethod.Invoke(null, new object[1] { feature2 }))
			{
				problems.Add(feature.name + ", handed over by " + grantedBy + ": UnitHelper.IsFeatureSurvivesRespec says a respec drops it.");
			}
		}
		finally
		{
			if (feature2 != null)
			{
				try
				{
					unit.Descriptor.RemoveFact(feature2);
				}
				catch
				{
				}
			}
		}
	}

	private static void CheckPermanentBuff(UnitEntityData unit, BlueprintBuff buff, string grantedBy, List<string> problems)
	{
		if (buff.IsClassFeature)
		{
			problems.Add(buff.name + ", given for good by " + grantedBy + ", is a class feature: UnitHelper.RespecOnCommit does not carry it over.");
		}
		if (!buff.StayOnDeath)
		{
			problems.Add(buff.name + ", given for good by " + grantedBy + ", is not StayOnDeath: BuffCollection.UpdateOnDeadOwner removes it.");
		}
		Buff buff2 = null;
		try
		{
			buff2 = unit.Descriptor.Buffs.AddBuff(buff, unit, null);
			if (buff2 == null)
			{
				problems.Add(buff.name + ": the blank unit refused it, so its death could not be driven.");
				return;
			}
			if (buff2.MaybeContext?.ParentContext != null)
			{
				problems.Add(buff.name + ": added as ActionGiveOtherworlderRewards adds it, it has a parent context, which RespecOnCommit does not carry over.");
			}
			UnitState state = unit.Descriptor.State;
			UnitLifeState lifeState = state.LifeState;
			try
			{
				state.UpdateLifeState(UnitLifeState.Dead);
				unit.Descriptor.Buffs.UpdateOnDeadOwner();
				if (unit.Descriptor.Buffs.GetBuff(buff) == null)
				{
					problems.Add(buff.name + ": gone after its owner died (BuffCollection.UpdateOnDeadOwner).");
				}
			}
			finally
			{
				state.UpdateLifeState(lifeState);
				unit.Descriptor.Buffs.UpdateOnDeadOwner();
			}
		}
		finally
		{
			Buff buff3 = unit.Descriptor.Buffs.GetBuff(buff);
			if (buff3 != null)
			{
				try
				{
					unit.Descriptor.RemoveFact(buff3);
				}
				catch
				{
				}
			}
		}
	}

	private static void CheckEmberCake(UnitEntityData unit, List<string> problems)
	{
		BlueprintBuff spark = Port62Rewards.Buff("EmberSparkBuff");
		if (spark == null)
		{
			problems.Add("EmberSparkBuff does not exist, so the camp cake could not be checked.");
			return;
		}
		try
		{
			unit.Descriptor.Buffs.AddBuff(spark, unit, null);
			unit.Descriptor.Buffs.AddBuff(spark, unit, TimeSpan.FromHours(24.0));
			Buff buff = unit.Descriptor.Buffs.GetBuff(spark);
			if (buff == null)
			{
				problems.Add("Ember's Saintly Spark is gone after the camp cake.");
			}
			else if (!buff.IsPermanent)
			{
				problems.Add("Ember's camp cake turned the permanent Saintly Spark into a 24-hour one.");
			}
			int num = unit.Descriptor.Buffs.RawFacts.Count((Buff b) => b.Blueprint == spark);
			if (num > 1)
			{
				problems.Add($"Ember's Saintly Spark is on the unit {num} times after the camp cake; its bonuses would count twice.");
			}
		}
		finally
		{
			foreach (Buff item in unit.Descriptor.Buffs.RawFacts.Where((Buff b) => b.Blueprint == spark).ToList())
			{
				try
				{
					unit.Descriptor.RemoveFact(item);
				}
				catch
				{
				}
			}
		}
	}

	internal static SelfTestChecks.Outcome StackingBonuses()
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		BlueprintFeature blueprintFeature = IsekaiLegendAscension.Get();
		BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "WishStrengthAbilityFeature");
		if (blueprintFeature == null || modBlueprint == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "Unbound Legend (" + ((blueprintFeature == null) ? "missing" : "found") + ") and the Cosmic Wish Strength boost (" + ((modBlueprint == null) ? "missing" : "found") + ") are both needed here.");
		}
		BlueprintFeature blueprintFeature2 = new BlueprintFeature
		{
			name = "SelfTestStandInInherentAndLuck"
		};
		blueprintFeature2.SetComponents(new AddStatBonus
		{
			Descriptor = ModifierDescriptor.Inherent,
			Stat = StatType.Strength,
			Value = 10
		}, new AddStatBonus
		{
			Descriptor = ModifierDescriptor.Luck,
			Stat = StatType.AC,
			Value = 10
		}, new AddStatBonus
		{
			Descriptor = ModifierDescriptor.Luck,
			Stat = StatType.SaveWill,
			Value = 10
		});
		string problem;
		using (SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem))
		{
			if (selfTestUnit == null)
			{
				return SelfTestChecks.Outcome.Of(passed: false, "No blank unit could be created: " + problem + ".");
			}
			UnitEntityData unit = selfTestUnit.Unit;
			List<EntityFactHolder> list3 = new List<EntityFactHolder>();
			try
			{
				int modifiedValue = unit.Stats.Strength.ModifiedValue;
				int modifiedValue2 = unit.Stats.AC.ModifiedValue;
				int modifiedValue3 = unit.Stats.SaveWill.ModifiedValue;
				list3.Add(new EntityFactHolder(unit.Descriptor.AddFact(blueprintFeature2)));
				int modifiedValue4 = unit.Stats.Strength.ModifiedValue;
				int modifiedValue5 = unit.Stats.AC.ModifiedValue;
				int modifiedValue6 = unit.Stats.SaveWill.ModifiedValue;
				list.Add($"The stand-in (+10 inherent Strength, +10 luck AC and Will): Strength {modifiedValue} -> {modifiedValue4}, AC {modifiedValue2} -> {modifiedValue5}, Will {modifiedValue3} -> {modifiedValue6}.");
				if (modifiedValue4 - modifiedValue != 10 || modifiedValue5 - modifiedValue2 != 10 || modifiedValue6 - modifiedValue3 != 10)
				{
					list2.Add("The stand-in's own bonuses did not apply as written, so nothing after it can be judged.");
				}
				int bonus = unit.Stats.Dexterity.Bonus;
				int bonus2 = unit.Stats.Wisdom.Bonus;
				list3.Add(new EntityFactHolder(unit.Descriptor.AddFact(blueprintFeature)));
				int modifiedValue7 = unit.Stats.Strength.ModifiedValue;
				int modifiedValue8 = unit.Stats.AC.ModifiedValue;
				int modifiedValue9 = unit.Stats.SaveWill.ModifiedValue;
				int bonus3 = unit.Stats.Dexterity.Bonus;
				int bonus4 = unit.Stats.Wisdom.Bonus;
				list.Add($"Unbound Legend on top: Strength {modifiedValue4} -> {modifiedValue7}, AC {modifiedValue5} -> {modifiedValue8} (Dexterity modifier {bonus} -> {bonus3}), Will {modifiedValue6} -> {modifiedValue9} (Wisdom modifier {bonus2} -> {bonus4}).");
				string[] array = new string[3]
				{
					SelfTestRuntimeRules.StackedBonusProblem("Unbound Legend", "Strength", modifiedValue4, modifiedValue7, 0, 0, 8, "inherent"),
					SelfTestRuntimeRules.StackedBonusProblem("Unbound Legend", "AC", modifiedValue5, modifiedValue8, bonus, bonus3, 5, "luck"),
					SelfTestRuntimeRules.StackedBonusProblem("Unbound Legend", "Will", modifiedValue6, modifiedValue9, bonus2, bonus4, 5, "luck")
				};
				foreach (string text in array)
				{
					if (text != null)
					{
						list2.Add(text);
					}
				}
				Feature feature = unit.Descriptor.Progression.Features.AddFeature(modBlueprint);
				list3.Add(new EntityFactHolder(feature));
				feature?.SetRank(3);
				int modifiedValue10 = unit.Stats.Strength.ModifiedValue;
				list.Add($"A Cosmic Wish Strength boost at rank 3 on top: Strength {modifiedValue7} -> {modifiedValue10}.");
				if (modifiedValue10 - modifiedValue7 != 6)
				{
					list2.Add($"Three Cosmic Wish Strength boosts added {modifiedValue10 - modifiedValue7} Strength next to two other inherent bonuses; each promises +2 that stacks.");
				}
			}
			finally
			{
				foreach (EntityFactHolder item in Enumerable.Reverse(list3))
				{
					item.Remove(unit);
				}
			}
		}
		list.AddRange(list2);
		return SelfTestChecks.Outcome.Of(list2.Count == 0, $"Unbound Legend's +8 inherent and +5 luck and the Cosmic Wish's +2 inherent per rank add to other bonuses of their type on a real unit; {list2.Count} problems.", list);
	}
}
