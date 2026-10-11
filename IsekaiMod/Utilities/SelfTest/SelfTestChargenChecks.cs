using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Root;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UI.MVVM._VM.CharGen.Phases;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.FeatureSelector;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestChargenChecks
{
	private sealed class Tally
	{
		internal int Failures;

		internal int Lists;

		internal int Builds;

		internal int Legacies;

		internal int Rescued;

		internal int Empty;

		internal readonly List<string> Details = new List<string>();
	}

	internal static SelfTestChecks.Outcome ChargenLegacyPages()
	{
		BlueprintCharacterClass blueprintCharacterClass = IsekaiProtagonistClass.Get();
		if (blueprintCharacterClass == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The Isekai Protagonist class does not exist, so no legacy page could be opened.");
		}
		Tally tally = new Tally();
		HashSet<BlueprintGuid> modIds = new HashSet<BlueprintGuid>(from c in SelfTestProgressionChecks.ModBlueprints()
			select c.Guid);
		List<BlueprintArchetype> collection = blueprintCharacterClass.Archetypes.Where((BlueprintArchetype a) => a != null && modIds.Contains(a.AssetGuid)).OrderBy((BlueprintArchetype a) => a.name, StringComparer.Ordinal).ToList();
		List<BlueprintArchetype> list = new List<BlueprintArchetype>();
		list.Add(null);
		list.AddRange(collection);
		HashSet<BlueprintGuid> hashSet = new HashSet<BlueprintGuid>();
		Stopwatch stopwatch = Stopwatch.StartNew();
		foreach (BlueprintArchetype item2 in list)
		{
			string label = ((item2 == null) ? (blueprintCharacterClass.name + " (no archetype)") : (blueprintCharacterClass.name + " + " + item2.name));
			List<BlueprintFeatureSelection> offered = new List<BlueprintFeatureSelection>();
			tally.Builds++;
			WithLevelOne(blueprintCharacterClass, item2, label, tally, delegate(LevelUpController controller)
			{
				foreach (FeatureSelectionState item3 in LegacyStates(controller))
				{
					tally.Lists++;
					string text = SelectionName(item3);
					bool flag;
					try
					{
						flag = item3.CanSelectAnything(controller.State, controller.Preview, isForce: true);
					}
					catch
					{
						flag = false;
					}
					if (!flag)
					{
						tally.Failures++;
						tally.Details.Add(label + ": the legacy list '" + text + "' is on the page but offers nothing this character may pick, so the legacy page accepts no click at all.");
					}
					else if (item3.Selection is BlueprintFeatureSelection item)
					{
						offered.Add(item);
					}
				}
			});
			if (offered.Count == 0)
			{
				tally.Failures++;
				tally.Details.Add(label + ": no legacy list that offers anything is on the 1st-level page at all.");
			}
			foreach (BlueprintFeatureSelection item4 in offered)
			{
				foreach (BlueprintFeature item5 in LegaciesOf(item4))
				{
					if (hashSet.Add(item5.AssetGuid))
					{
						tally.Legacies++;
						tally.Builds++;
						SweepOneLegacy(blueprintCharacterClass, item2, item4, item5, tally);
					}
				}
			}
		}
		stopwatch.Stop();
		string summary = $"{tally.Lists} legacy lists and {tally.Legacies} legacies, one 1st-level build each ({tally.Builds} builds);" + " every list must offer something, and everything a legacy brings with it must sit behind the legacy page" + $" and be resolvable ({tally.Rescued} would have been drawn in front of an unmoved legacy page," + $" {tally.Empty} offer this character nothing and are noted);" + $" {stopwatch.Elapsed.TotalSeconds:0} s; {tally.Failures} problems.";
		return SelfTestChecks.Outcome.Of(tally.Failures == 0, summary, tally.Details);
	}

	private static void SweepOneLegacy(BlueprintCharacterClass isekai, BlueprintArchetype archetype, BlueprintFeatureSelection list, BlueprintFeature legacy, Tally tally)
	{
		string label = ((archetype == null) ? isekai.name : (isekai.name + " + " + archetype.name)) + " / " + legacy.name;
		WithLevelOne(isekai, archetype, label, tally, delegate(LevelUpController controller)
		{
			FeatureSelectionState featureSelectionState = LegacyStates(controller).FirstOrDefault((FeatureSelectionState s) => s.Selection == list);
			if (featureSelectionState == null)
			{
				tally.Failures++;
				tally.Details.Add(label + ": the legacy list is not on the page for this build, so nothing could be picked.");
			}
			else
			{
				IFeatureSelectionItem featureSelectionItem = list.Items.FirstOrDefault((IFeatureSelectionItem i) => i != null && i.Feature == legacy);
				if (featureSelectionItem == null)
				{
					tally.Failures++;
					tally.Details.Add(label + ": the legacy list no longer offers this legacy as an item.");
				}
				else if (list.CanSelect(controller.Preview.Descriptor, controller.State, featureSelectionState, featureSelectionItem))
				{
					int count = controller.State.Selections.Count;
					if (!controller.SelectFeature(featureSelectionState, featureSelectionItem) || !featureSelectionState.Selected)
					{
						tally.Failures++;
						tally.Details.Add(label + ": the legacy list accepted the pick and did not keep it, so the option can never be marked.");
					}
					else
					{
						CharGenFeatureSelectorPhaseVM probe = MakeProbe(controller);
						int num = PageOrder(probe, controller, featureSelectionState);
						int num2 = VanillaPageOrder(probe, controller, featureSelectionState);
						List<FeatureSelectionState> list2 = controller.State.Selections.ToList();
						for (int num3 = count; num3 < list2.Count; num3++)
						{
							FeatureSelectionState featureSelectionState2 = list2[num3];
							if (!(featureSelectionState2 == null) && !(featureSelectionState2.Parent != null))
							{
								int num4 = PageOrder(probe, controller, featureSelectionState2);
								if (num4 <= num)
								{
									tally.Failures++;
									tally.Details.Add(label + ": picking it adds the selection '" + SelectionName(featureSelectionState2) + "', whose character creation" + $" page sorts at {num4}, in front of the legacy page at {num}." + " A page in front of the legacy is not complete, so the legacy page and every page after it turn unavailable: the list stops responding to clicks and character creation cannot be finished.");
								}
								else
								{
									if (num4 <= num2)
									{
										tally.Rescued++;
										tally.Details.Add($"{label}: '{SelectionName(featureSelectionState2)}' would have drawn its page at {num4}, in front of an" + $" unmoved legacy page at {num2}; the legacy page is moved to {num} and it now follows.");
									}
									bool flag;
									try
									{
										flag = featureSelectionState2.CanSelectAnything(controller.State, controller.Preview, isForce: true);
									}
									catch
									{
										flag = false;
									}
									if (!flag && !featureSelectionState2.Selected)
									{
										tally.Empty++;
										tally.Details.Add(label + ": the selection '" + SelectionName(featureSelectionState2) + "' it introduces offers this character nothing to pick; its page is empty and reports itself complete, so it is noted rather than failed.");
									}
								}
							}
						}
					}
				}
			}
		});
	}

	private static void WithLevelOne(BlueprintCharacterClass isekai, BlueprintArchetype archetype, string label, Tally tally, Action<LevelUpController> body)
	{
		string problem;
		using SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem);
		if (selfTestUnit == null)
		{
			tally.Details.Add(label + ": " + problem + ".");
			tally.Failures++;
			return;
		}
		try
		{
			using (ContextData<AddClassLevels.ExecutionMark>.Request().Setup(selfTestUnit.Unit))
			{
				using (ContextData<AddClassLevels.LevelUpControllerStash>.Request().Stash())
				{
					LevelUpController levelUpController = LevelUpController.Start(selfTestUnit.Unit, LevelUpState.CharBuildMode.LevelUp, instantCommit: true, ignoreSettings: true);
					try
					{
						if (levelUpController.State.CanSelectRace)
						{
							object obj = selfTestUnit.Unit.Descriptor.Progression.Race;
							if (obj == null)
							{
								BlueprintRoot instance = BlueprintRoot.Instance;
								if (instance == null)
								{
									obj = null;
								}
								else
								{
									ProgressionRoot progression = instance.Progression;
									obj = ((progression != null) ? progression.CharacterRaces.FirstOrDefault() : null);
								}
							}
							BlueprintRace blueprintRace = (BlueprintRace)obj;
							if (blueprintRace != null)
							{
								levelUpController.SelectRace(blueprintRace);
							}
						}
						if (levelUpController.State.CanSelectRaceStat)
						{
							levelUpController.SelectRaceStat(StatType.Constitution);
						}
						if (archetype != null)
						{
							levelUpController.AddArchetype(isekai, archetype);
						}
						using (ContextData<IgnorePrerequisites>.Request())
						{
							if (!levelUpController.SelectClass(isekai, applyMechanics: false))
							{
								tally.Details.Add(label + ": the class could not be selected, so no legacy page exists to check.");
								tally.Failures++;
								return;
							}
						}
						levelUpController.ApplyClassMechanics();
						body(levelUpController);
					}
					finally
					{
						try
						{
							if (!levelUpController.Stopped)
							{
								levelUpController.Stop();
							}
						}
						catch
						{
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			tally.Details.Add(label + ": opening the 1st-level page threw " + ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message));
			tally.Failures++;
		}
	}

	internal static CharGenFeatureSelectorPhaseVM MakeProbe(LevelUpController controller)
	{
		CharGenFeatureSelectorPhaseVM obj = (CharGenFeatureSelectorPhaseVM)FormatterServices.GetUninitializedObject(typeof(CharGenFeatureSelectorPhaseVM));
		((CharGenPhaseBaseVM)obj).m_LevelUpController = controller;
		return obj;
	}

	private static int VanillaPageOrder(CharGenFeatureSelectorPhaseVM probe, LevelUpController controller, FeatureSelectionState state)
	{
		int num = controller.State.Selections.IndexOf(state);
		return probe.GetBaseOrderPriority(probe.GetFeaturePriority(state)) + num;
	}

	internal static int PageOrder(CharGenFeatureSelectorPhaseVM probe, LevelUpController controller, FeatureSelectionState state)
	{
		int num = controller.State.Selections.IndexOf(state);
		int vanilla = probe.GetBaseOrderPriority(probe.GetFeaturePriority(state)) + num;
		return LegacySelectionOrderPatch.WindowPageOrder(probe, controller, state, vanilla, num);
	}

	private static List<FeatureSelectionState> LegacyStates(LevelUpController controller)
	{
		List<FeatureSelectionState> list = new List<FeatureSelectionState>();
		foreach (FeatureSelectionState item in controller.State.Selections.ToList())
		{
			if (!(item == null) && !(item.Parent != null) && LegacySelectionOrderPatch.IsLegacySelection((item.Selection as SimpleBlueprint)?.AssetGuid ?? BlueprintGuid.Empty))
			{
				list.Add(item);
			}
		}
		return list;
	}

	private static IEnumerable<BlueprintFeature> LegaciesOf(BlueprintFeatureSelection selection)
	{
		return (from f in selection.AllFeatures
			where f != null
			group f by f.AssetGuid into g
			select g.First()).OrderBy((BlueprintFeature f) => f.name, StringComparer.Ordinal).ToList();
	}

	private static string SelectionName(FeatureSelectionState state)
	{
		return (state.Selection as BlueprintScriptableObject)?.name ?? state.Selection?.GetType().Name ?? "(unknown)";
	}
}
