using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.FeatureSelector;
using Kingmaker.UnitLogic.Class.LevelUp;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestWindowLevelUpChecks
{
	internal static readonly Dictionary<string, KeyValuePair<string, string>> KnownIssues = new Dictionary<string, KeyValuePair<string, string>>(StringComparer.Ordinal);

	internal static SelfTestChecks.Outcome LevelUpWindow()
	{
		BlueprintCharacterClass blueprintCharacterClass = IsekaiProtagonistClass.Get();
		if (blueprintCharacterClass == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The Isekai Protagonist class does not exist, so no level could be driven through the window.");
		}
		List<string> list = new List<string>();
		int num = 0;
		int num2 = 0;
		Stopwatch stopwatch = Stopwatch.StartNew();
		BlueprintFeatureSelection dual = LegacySelection.GetOverwhelmingFeature();
		BlueprintFeatureSelection legacyList = LegacySelection.GetClassFeature();
		BlueprintFeatureSelection blueprintFeatureSelection = TriLegacySelection.Get();
		BlueprintProgression twinLegacy = TwinSummonerLegacy.Get();
		BlueprintFeature twinAvatar = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TwinAvatarCompanion");
		BlueprintFeatureSelection petList = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiPetSelection");
		BlueprintFeatureSelection opList = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "OverpoweredAbilitySelection");
		foreach (var item in new(string Name, SimpleBlueprint Blueprint)[6]
		{
			("LegacyAbilitySelection (Dual Legacy)", dual),
			("LegacyClassSelection", legacyList),
			("TwinSummonerLegacy", twinLegacy),
			("TwinAvatarCompanion", twinAvatar),
			("IsekaiPetSelection", petList),
			("OverpoweredAbilitySelection", opList)
		}.Where(((string Name, SimpleBlueprint Blueprint) b) => b.Blueprint == null))
		{
			num++;
			list.Add(item.Name + " does not exist, so the builds that need it cannot be driven.");
		}
		if (num > 0)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "Blueprints the window builds need are missing.", list);
		}
		List<string> list2 = new List<string>();
		string problem;
		using (SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem))
		{
			if (selfTestUnit == null)
			{
				list2.Add("control: " + problem + ".");
			}
			else
			{
				for (int num3 = 1; num3 <= 5; num3++)
				{
					SelfTestLevelUpWindow.Plan plan = new SelfTestLevelUpWindow.Plan
					{
						Class = blueprintCharacterClass
					};
					plan.Avoided.Add(dual.AssetGuid);
					if (blueprintFeatureSelection != null)
					{
						plan.Avoided.Add(blueprintFeatureSelection.AssetGuid);
					}
					plan.Avoided.Add(twinLegacy.AssetGuid);
					SelfTestLevelUpWindow.Result result = SelfTestLevelUpWindow.DriveLevel(selfTestUnit.Unit, plan, $"control, Isekai Protagonist level {num3}");
					list2.AddRange(result.Problems);
					list2.AddRange(result.Lost.Select((KeyValuePair<string, string> l) => l.Value));
					list2.AddRange(result.Refused.Select((KeyValuePair<string, string> r) => r.Value));
					if (!result.Committed)
					{
						break;
					}
					num2++;
				}
				int num4 = 0;
				try
				{
					num4 = selfTestUnit.Unit.Descriptor.Progression.CharacterLevel;
				}
				catch
				{
				}
				if (num4 != 5)
				{
					list2.Add($"control: the character is level {num4} after five window levels, expected 5.");
				}
			}
		}
		num += list2.Count;
		list.AddRange(list2);
		List<string> list3 = new List<string>();
		List<string> list4 = new List<string>();
		string problem2;
		using (SelfTestUnit selfTestUnit2 = SelfTestUnit.CreateBlank(out problem2))
		{
			if (selfTestUnit2 == null)
			{
				list4.Add("dual-legacy-5: " + problem2 + ".");
			}
			else
			{
				List<string> selectionNotes = new List<string>();
				bool flag = true;
				for (int num5 = 1; (num5 <= 4) & flag; num5++)
				{
					flag = SelfTestProgressionChecks.TakeLevel(selfTestUnit2.Unit, blueprintCharacterClass, null, num5, "dual-legacy-5 (levels 1-4, auto-commit)", list4, null, selectionNotes);
					if (flag)
					{
						num2++;
					}
				}
				if (flag)
				{
					SelfTestLevelUpWindow.Plan plan2 = new SelfTestLevelUpWindow.Plan
					{
						Class = blueprintCharacterClass
					};
					plan2.Preferred.Add(dual.AssetGuid);
					if (blueprintFeatureSelection != null)
					{
						plan2.Avoided.Add(blueprintFeatureSelection.AssetGuid);
					}
					SelfTestLevelUpWindow.Result result2 = SelfTestLevelUpWindow.DriveLevel(selfTestUnit2.Unit, plan2, "dual-legacy-5, Isekai Protagonist level 5 through the window");
					list4.AddRange(result2.Problems);
					if (result2.Committed)
					{
						num2++;
					}
					bool flag2 = result2.Picks.Any((SelfTestLevelUpWindow.Pick p) => p.Feature == dual) || result2.Lost.Any((KeyValuePair<string, string> l) => l.Key == opList.name) || result2.Refused.Any((KeyValuePair<string, string> r) => r.Key == opList.name);
					if (!flag2)
					{
						list4.Add("dual-legacy-5: the Overpowered Ability page never offered Dual Legacy, so the scenario could not be driven.");
					}
					foreach (KeyValuePair<string, string> item2 in result2.Lost.Concat(result2.Refused))
					{
						if (item2.Key == dual.name)
						{
							list3.Add(item2.Value);
						}
						else
						{
							list4.Add(item2.Value);
						}
					}
					bool flag3 = result2.Picks.Any((SelfTestLevelUpWindow.Pick p) => p.Selection == dual);
					if (flag2 && result2.Committed && !flag3 && list3.Count == 0)
					{
						list4.Add("dual-legacy-5: Dual Legacy was taken but no legacy from its list is on the committed character, and no pick was seen being dropped.");
					}
				}
			}
		}
		num += list4.Count;
		list.AddRange(list4);
		num += Known("dual-legacy-5", list3, list);
		List<string> reproduced = new List<string>();
		List<string> problems = new List<string>();
		string problem3;
		using (SelfTestUnit selfTestUnit3 = SelfTestUnit.CreateBlank(out problem3))
		{
			if (selfTestUnit3 == null)
			{
				problems.Add("twin-summoner-1: " + problem3 + ".");
			}
			else
			{
				SelfTestLevelUpWindow.Plan plan3 = new SelfTestLevelUpWindow.Plan
				{
					Class = blueprintCharacterClass
				};
				plan3.Preferred.Add(twinAvatar.AssetGuid);
				plan3.Preferred.Add(twinLegacy.AssetGuid);
				plan3.BeforeFill = delegate(LevelUpController controller, SelfTestLevelUpWindow.Result result5)
				{
					FeatureSelectionState featureSelectionState = controller.State.Selections.FirstOrDefault((FeatureSelectionState s) => s != null && s.Selection == legacyList);
					FeatureSelectionState featureSelectionState2 = controller.State.Selections.FirstOrDefault((FeatureSelectionState s) => s != null && s.Selection == petList);
					if (featureSelectionState == null || featureSelectionState2 == null)
					{
						problems.Add("twin-summoner-1 (page order): the 1st-level page for " + ((featureSelectionState == null) ? "the legacy" : "the pet") + " is missing.");
					}
					else
					{
						CharGenFeatureSelectorPhaseVM probe = SelfTestChargenChecks.MakeProbe(controller);
						int num7 = SelfTestChargenChecks.PageOrder(probe, controller, featureSelectionState);
						int num8 = SelfTestChargenChecks.PageOrder(probe, controller, featureSelectionState2);
						if (num7 < num8)
						{
							reproduced.Add($"twin-summoner-1 (page order): the legacy page (order {num7}) comes before the pet page (order {num8})," + " so on it the Twin Summoner legacy cannot be picked: the Twin Avatar it requires is not chosen yet.");
						}
					}
				};
				SelfTestLevelUpWindow.Result result3 = SelfTestLevelUpWindow.DriveLevel(selfTestUnit3.Unit, plan3, "twin-summoner-1 (page order)");
				problems.AddRange(result3.Problems);
				if (result3.Committed)
				{
					num2++;
				}
				foreach (KeyValuePair<string, string> item3 in result3.Lost.Concat(result3.Refused))
				{
					if (item3.Key == legacyList.name || item3.Key == petList.name)
					{
						reproduced.Add(item3.Value);
					}
					else
					{
						problems.Add(item3.Value);
					}
				}
				bool flag4 = result3.Picks.Any((SelfTestLevelUpWindow.Pick p) => p.Feature == twinAvatar);
				bool flag5 = result3.Picks.Any((SelfTestLevelUpWindow.Pick p) => p.Feature == twinLegacy);
				if (result3.Committed && (!flag4 || !flag5) && !result3.Lost.Any() && !result3.Refused.Any())
				{
					reproduced.Add("twin-summoner-1 (page order): filling the pages in the window's order, the character was committed without " + (flag4 ? "the Twin Summoner legacy" : "the Twin Avatar") + ", which was never a valid pick when its page came up.");
				}
			}
		}
		string problem4;
		using (SelfTestUnit selfTestUnit4 = SelfTestUnit.CreateBlank(out problem4))
		{
			if (selfTestUnit4 == null)
			{
				problems.Add("twin-summoner-1: " + problem4 + ".");
			}
			else
			{
				SelfTestLevelUpWindow.Plan plan4 = new SelfTestLevelUpWindow.Plan
				{
					Class = blueprintCharacterClass
				};
				plan4.Preferred.Add(twinAvatar.AssetGuid);
				plan4.Preferred.Add(twinLegacy.AssetGuid);
				plan4.Order = (LevelUpController controller, FeatureSelectionState state) => (state.Selection != petList) ? ((state.Selection == legacyList) ? 1 : 2) : 0;
				SelfTestLevelUpWindow.Result result4 = SelfTestLevelUpWindow.DriveLevel(selfTestUnit4.Unit, plan4, "twin-summoner-1 (the pet first, then the legacy)");
				problems.AddRange(result4.Problems);
				if (result4.Committed)
				{
					num2++;
				}
				bool flag6 = result4.Picks.Any((SelfTestLevelUpWindow.Pick p) => p.Feature == twinAvatar);
				if (!flag6)
				{
					problems.Add("twin-summoner-1: the pet page never kept the Twin Avatar, so the legacy that needs it could not be tried.");
				}
				foreach (KeyValuePair<string, string> item4 in result4.Lost.Concat(result4.Refused))
				{
					if (item4.Key == legacyList.name)
					{
						reproduced.Add(item4.Value);
					}
					else
					{
						problems.Add(item4.Value);
					}
				}
				bool flag7 = result4.Picks.Any((SelfTestLevelUpWindow.Pick p) => p.Feature == twinLegacy);
				if (flag6 && result4.Committed && !flag7 && !result4.Lost.Any((KeyValuePair<string, string> l) => l.Key == legacyList.name) && !result4.Refused.Any((KeyValuePair<string, string> l) => l.Key == legacyList.name))
				{
					reproduced.Add("twin-summoner-1: with the Twin Avatar chosen, the legacy page still never offered the Twin Summoner legacy as a valid pick.");
				}
			}
		}
		num += problems.Count;
		list.AddRange(problems);
		num += Known("twin-summoner-1", reproduced, list);
		stopwatch.Stop();
		int num6 = list.Count((string d) => d.StartsWith("KNOWN ISSUE ", StringComparison.Ordinal));
		string summary = "The level-up window's own path (no auto-commit: every pick re-sorted, replayed and committed): a 1-5 control build, Dual Legacy at Isekai 5 and the Twin Summoner legacy at 1st level;" + $" {num2} levels committed in {stopwatch.Elapsed.TotalSeconds:0} s; {num} problems, {num6} allow-listed known issues.";
		return SelfTestChecks.Outcome.Of(num == 0, summary, list);
	}

	private static int Known(string scenario, List<string> reproduced, List<string> details)
	{
		if (!KnownIssues.TryGetValue(scenario, out var value))
		{
			details.AddRange(reproduced);
			return reproduced.Count;
		}
		if (reproduced.Count == 0)
		{
			details.Add(scenario + ": " + value.Key + " no longer reproduces (" + value.Value + "). The fix has landed: delete the '" + scenario + "' entry from SelfTestWindowLevelUpChecks.KnownIssues so the scenario is asserted again.");
			return 1;
		}
		foreach (string item in reproduced)
		{
			details.Add("KNOWN ISSUE " + value.Key + ": " + item);
		}
		return 0;
	}
}
