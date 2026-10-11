using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.FeatureSelector;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestLevelUpWindow
{
	internal sealed class Plan
	{
		internal BlueprintCharacterClass Class;

		internal BlueprintArchetype Archetype;

		internal LevelUpState.CharBuildMode Mode;

		internal readonly HashSet<BlueprintGuid> Preferred = new HashSet<BlueprintGuid>();

		internal readonly HashSet<BlueprintGuid> Avoided = new HashSet<BlueprintGuid>();

		internal Func<LevelUpController, FeatureSelectionState, int> Order;

		internal Action<LevelUpController, Result> BeforeFill;

		internal LevelUpController.OnCommit OnCommit;

		internal LevelUpController.OnStop OnStop;

		internal bool Commit = true;

		internal bool FillPages = true;
	}

	internal sealed class Pick
	{
		internal IFeatureSelection Selection;

		internal int Index;

		internal BlueprintFeature Feature;

		internal string SelectionName;
	}

	internal sealed class Result
	{
		internal string Label;

		internal readonly List<string> Problems = new List<string>();

		internal readonly List<KeyValuePair<string, string>> Lost = new List<KeyValuePair<string, string>>();

		internal readonly List<KeyValuePair<string, string>> Refused = new List<KeyValuePair<string, string>>();

		internal readonly List<string> Notes = new List<string>();

		internal readonly List<Pick> Picks = new List<Pick>();

		internal bool Committed;
	}

	internal static Result DriveLevel(UnitEntityData unit, Plan plan, string label)
	{
		Result result = new Result
		{
			Label = label
		};
		using (ContextData<AddClassLevels.LevelUpControllerStash>.Request().Stash())
		{
			LevelUpController levelUpController = null;
			try
			{
				levelUpController = LevelUpController.Start(unit, plan.Mode, instantCommit: false, ignoreSettings: true);
				if (plan.OnCommit != null)
				{
					levelUpController.SetOnCommit(plan.OnCommit);
				}
				if (plan.OnStop != null)
				{
					levelUpController.SetOnStop(plan.OnStop);
				}
				if (levelUpController.AutoCommit)
				{
					result.Problems.Add(label + ": the controller started in auto-commit mode, so the window path was not driven at all.");
					return result;
				}
				if (levelUpController.State.CanSelectRace)
				{
					object obj = unit.Descriptor.Progression.Race;
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
				if (!levelUpController.SelectClass(plan.Class))
				{
					bool flag = false;
					try
					{
						flag = plan.Class.MeetsPrerequisites(levelUpController.Preview.Descriptor, levelUpController.State);
					}
					catch
					{
					}
					result.Problems.Add($"{label}: the window refused the class {plan.Class.name} (prerequisites met: {flag}), so no page behind it could be driven.");
					return result;
				}
				if (plan.Archetype != null && unit.Descriptor.Progression.GetClassLevel(plan.Class) <= 0 && !levelUpController.AddArchetype(plan.Class, plan.Archetype))
				{
					result.Problems.Add(label + ": the window refused the archetype " + plan.Archetype.name + ".");
				}
				plan.BeforeFill?.Invoke(levelUpController, result);
				if (plan.FillPages)
				{
					Fill(levelUpController, plan, label, result);
				}
				SpendPoints(levelUpController);
				VerifyKept(levelUpController, label, result);
				if (plan.Commit)
				{
					levelUpController.Commit();
					result.Committed = levelUpController.Committed;
					if (!result.Committed)
					{
						result.Problems.Add(label + ": Commit returned without committing the level.");
					}
					else
					{
						VerifyOnUnit(unit, label, result);
					}
				}
			}
			catch (Exception ex)
			{
				result.Problems.Add(label + ": driving the window threw " + ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message));
			}
			finally
			{
				try
				{
					if (levelUpController != null && !levelUpController.Stopped)
					{
						levelUpController.Stop();
					}
				}
				catch
				{
				}
			}
		}
		return result;
	}

	private static void Fill(LevelUpController controller, Plan plan, string label, Result result)
	{
		ProgressionRoot progressionRoot = BlueprintRoot.Instance?.Progression;
		HashSet<string> tried = new HashSet<string>(StringComparer.Ordinal);
		for (int i = 0; i < 300; i++)
		{
			CharGenFeatureSelectorPhaseVM probe = SelfTestChargenChecks.MakeProbe(controller);
			List<FeatureSelectionState> list = controller.State.Selections.Where((FeatureSelectionState s) => s != null && s.Selection != null && !s.Selected && !tried.Contains(KeyOf(s))).ToList();
			if (list.Count == 0)
			{
				return;
			}
			FeatureSelectionState state = ((plan.Order != null) ? (from s in list
				orderby plan.Order(controller, s), SafePageOrder(probe, controller, s)
				select s).First() : list.OrderBy((FeatureSelectionState s) => SafePageOrder(probe, controller, s)).First());
			tried.Add(KeyOf(state));
			IFeatureSelection selection = state.Selection;
			string text = NameOf(selection);
			bool flag = selection == progressionRoot?.BasicFeatSelection || selection == progressionRoot?.DeitySelection;
			IFeatureSelectionItem featureSelectionItem;
			try
			{
				List<IFeatureSelectionItem> source = selection.Items.Where((IFeatureSelectionItem candidate) => candidate != null && selection.CanSelect(controller.Preview.Descriptor, controller.State, state, candidate)).ToList();
				featureSelectionItem = source.FirstOrDefault((IFeatureSelectionItem c) => c.Feature != null && plan.Preferred.Contains(c.Feature.AssetGuid)) ?? source.FirstOrDefault((IFeatureSelectionItem c) => c.Feature == null || !plan.Avoided.Contains(c.Feature.AssetGuid));
			}
			catch (Exception ex)
			{
				result.Problems.Add(label + ": the page '" + text + "' threw " + ex.GetType().Name + " while its options were listed: " + SelfTestReport.Clean(ex.Message));
				continue;
			}
			if (featureSelectionItem == null)
			{
				if (!flag)
				{
					result.Notes.Add(label + ": the page '" + text + "' offers this character nothing to pick.");
				}
				continue;
			}
			string text2 = ((featureSelectionItem.Feature != null) ? featureSelectionItem.Feature.name : "(unnamed)");
			bool flag2;
			try
			{
				flag2 = controller.SelectFeature(state, featureSelectionItem);
			}
			catch (Exception ex2)
			{
				result.Problems.Add(label + ": picking '" + text2 + "' on the page '" + text + "' threw " + ex2.GetType().Name + ": " + SelfTestReport.Clean(ex2.Message));
				continue;
			}
			FeatureSelectionState featureSelectionState = Find(controller, selection, state.Index);
			if (!flag2)
			{
				result.Refused.Add(new KeyValuePair<string, string>(text, label + ": the page '" + text + "' offered '" + text2 + "' and the controller refused it."));
			}
			else if (featureSelectionState == null || !featureSelectionState.Selected || featureSelectionState.SelectedItem?.Feature != featureSelectionItem.Feature)
			{
				result.Lost.Add(new KeyValuePair<string, string>(text, label + ": the page '" + text + "' accepted '" + text2 + "' and the redraw dropped it (the pick sorts ahead of the action that creates its selection, fails SelectFeature.Check on the replay and is logged \"Invalid action\"), so the tick vanishes and the page stays unfinished."));
			}
			else
			{
				result.Picks.Add(new Pick
				{
					Selection = selection,
					Index = state.Index,
					Feature = featureSelectionItem.Feature,
					SelectionName = text
				});
			}
		}
		result.Problems.Add(label + ": the pages were still offering new selections after 300 picks, so the fill was stopped.");
	}

	private static void VerifyKept(LevelUpController controller, string label, Result result)
	{
		foreach (Pick item in result.Picks.ToList())
		{
			FeatureSelectionState featureSelectionState = Find(controller, item.Selection, item.Index);
			if (!(featureSelectionState != null) || !featureSelectionState.Selected || featureSelectionState.SelectedItem?.Feature != item.Feature)
			{
				result.Picks.Remove(item);
				result.Lost.Add(new KeyValuePair<string, string>(item.SelectionName, label + ": the page '" + item.SelectionName + "' kept '" + item.Feature?.name + "' when it was picked, and a later pick's redraw dropped it."));
			}
		}
	}

	private static void VerifyOnUnit(UnitEntityData unit, string label, Result result)
	{
		foreach (Pick item in result.Picks.ToList())
		{
			BlueprintFeature feature = item.Feature;
			if (feature != null && !feature.GetComponents<AddFeatureToPetParametrized>().Any())
			{
				bool flag;
				try
				{
					flag = unit.Descriptor.HasFact(feature);
				}
				catch
				{
					flag = false;
				}
				if (!flag)
				{
					result.Picks.Remove(item);
					result.Lost.Add(new KeyValuePair<string, string>(item.SelectionName, label + ": '" + feature.name + "' was kept on the page '" + item.SelectionName + "' and is not on the character after the commit, whose replay dropped it."));
				}
			}
		}
	}

	private static void SpendPoints(LevelUpController controller)
	{
		int num = 0;
		while (controller.State.SkillPointsRemaining > 0 && num++ < 100)
		{
			int skillPointsRemaining = controller.State.SkillPointsRemaining;
			controller.SpendSkillPoint(StatType.SkillPerception);
			if (controller.State.SkillPointsRemaining == skillPointsRemaining)
			{
				break;
			}
		}
		num = 0;
		while (controller.State.AttributePoints > 0 && num++ < 100)
		{
			int attributePoints = controller.State.AttributePoints;
			controller.SpendAttributePoint(StatType.Strength);
			if (controller.State.AttributePoints == attributePoints)
			{
				break;
			}
		}
	}

	internal static FeatureSelectionState Find(LevelUpController controller, IFeatureSelection selection, int index)
	{
		foreach (FeatureSelectionState selection2 in controller.State.Selections)
		{
			if (selection2 != null && selection2.Selection == selection && selection2.Index == index)
			{
				return selection2;
			}
		}
		return null;
	}

	internal static string NameOf(IFeatureSelection selection)
	{
		return (selection as BlueprintScriptableObject)?.name ?? selection?.GetType().Name ?? "(unknown)";
	}

	private static string KeyOf(FeatureSelectionState state)
	{
		return NameOf(state.Selection) + "#" + state.Index;
	}

	private static int SafePageOrder(CharGenFeatureSelectorPhaseVM probe, LevelUpController controller, FeatureSelectionState state)
	{
		try
		{
			return SelfTestChargenChecks.PageOrder(probe, controller, state);
		}
		catch
		{
			return int.MaxValue;
		}
	}
}
