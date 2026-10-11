using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Root;
using Kingmaker.Controllers;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UI.MVVM._VM.CharGen;
using Kingmaker.UI.MVVM._VM.CharGen.Phases;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.FeatureSelector;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestLegacyWindowChecks
{
	private sealed class Tally
	{
		internal int Failures;

		internal int Builds;

		internal int Windows;

		internal int Held;

		internal int Refused;

		internal int NotOffered;

		internal int Introduced;

		internal int Rescued;

		internal int Empty;

		internal readonly List<string> Details = new List<string>();

		internal void Fail(string line)
		{
			Failures++;
			Details.Add(line);
		}
	}

	private sealed class Build
	{
		internal string Label;

		internal string ArchetypeName;

		internal string[] Preferred;

		internal string FirstLegacy;

		internal bool TriLegacy;
	}

	private sealed class Window : IDisposable
	{
		internal LevelUpController Controller;

		internal CharGenVM View;

		private bool m_Selected;

		private UnitReference m_PreviouslySelected;

		internal static Window Open(UnitEntityData unit, LevelUpState.CharBuildMode mode)
		{
			Window window = new Window();
			try
			{
				SelectionCharacterController selectionCharacterController = Game.Instance?.SelectionCharacter;
				if (selectionCharacterController != null)
				{
					window.m_PreviouslySelected = selectionCharacterController.SelectedUnit.Value;
					selectionCharacterController.SelectedUnit.Value = unit;
					window.m_Selected = true;
				}
				LevelUpConfig config = LevelUpConfig.Create(unit, mode);
				using (ContextData<AddClassLevels.ExecutionMark>.Request().Setup(unit))
				{
					window.Controller = LevelUpController.Start(unit, mode);
				}
				if (mode != LevelUpState.CharBuildMode.CharGen)
				{
					window.View = new CharGenVM(window.Controller, delegate
					{
					}, delegate
					{
					}, config);
				}
				return window;
			}
			catch
			{
				window.Dispose();
				throw;
			}
		}

		public void Dispose()
		{
			try
			{
				View?.Dispose();
			}
			catch
			{
			}
			try
			{
				if (Controller != null && !Controller.Stopped)
				{
					Controller.Stop();
				}
			}
			catch
			{
			}
			if (m_Selected)
			{
				try
				{
					Game.Instance.SelectionCharacter.SelectedUnit.Value = m_PreviouslySelected;
				}
				catch
				{
				}
			}
		}
	}

	private static readonly string[] Reported = new string[4] { "FighterBasicIsekaiLegacy", "IsekaiNoPetFeature", "IsekaiCataclysmBerserkerHeritage", "BackgroundRebornDemonLord" };

	private static readonly string[] ReportedWithDualLegacy = new string[6] { "FighterBasicIsekaiLegacy", "IsekaiNoPetFeature", "IsekaiCataclysmBerserkerHeritage", "BackgroundRebornDemonLord", "LegacyAbilitySelection", "RogueLegacy" };

	private static readonly Build[] Table = new Build[13]
	{
		new Build
		{
			Label = "Martial God + Basic Fighter + Solo Sovereign (the report)",
			ArchetypeName = "MartialGodArchetype",
			Preferred = Reported,
			FirstLegacy = "FighterBasicIsekaiLegacy"
		},
		new Build
		{
			Label = "Martial God + Basic Fighter + Dual Legacy Rogue, Tri-Legacy at 10",
			ArchetypeName = "MartialGodArchetype",
			Preferred = ReportedWithDualLegacy,
			FirstLegacy = "FighterBasicIsekaiLegacy",
			TriLegacy = true
		},
		new Build
		{
			Label = "no archetype + Arcane Savant",
			ArchetypeName = null,
			Preferred = new string[1] { "WizardLegacy" },
			FirstLegacy = "WizardLegacy"
		},
		new Build
		{
			Label = "Devourer + Arcanist",
			ArchetypeName = "DevourerArchetype",
			Preferred = new string[1] { "ArcanistBasicLegacy" },
			FirstLegacy = "ArcanistBasicLegacy"
		},
		new Build
		{
			Label = "God Emperor + Cleric",
			ArchetypeName = "GodEmperorArchetype",
			Preferred = new string[1] { "ClericLegacy" },
			FirstLegacy = "ClericLegacy"
		},
		new Build
		{
			Label = "Hero + Paladin",
			ArchetypeName = "HeroArchetype",
			Preferred = new string[1] { "PaladinBaseLegacy" },
			FirstLegacy = "PaladinBaseLegacy"
		},
		new Build
		{
			Label = "Mastermind + Witch",
			ArchetypeName = "MastermindArchetype",
			Preferred = new string[1] { "WitchBaseLegacy" },
			FirstLegacy = "WitchBaseLegacy"
		},
		new Build
		{
			Label = "Overlord + Sorcerer",
			ArchetypeName = "OverlordArchetype",
			Preferred = new string[1] { "SorcererLegacyProgression" },
			FirstLegacy = "SorcererLegacyProgression"
		},
		new Build
		{
			Label = "Shadow Monarch + Slayer",
			ArchetypeName = "ShadowMonarchArchetype",
			Preferred = new string[1] { "SlayerLegacy" },
			FirstLegacy = "SlayerLegacy"
		},
		new Build
		{
			Label = "no archetype + Bloodrager",
			ArchetypeName = null,
			Preferred = new string[1] { "BloodragerChimeraLegacy" },
			FirstLegacy = "BloodragerChimeraLegacy"
		},
		new Build
		{
			Label = "no archetype + Kineticist",
			ArchetypeName = null,
			Preferred = new string[1] { "KineticLegacy" },
			FirstLegacy = "KineticLegacy"
		},
		new Build
		{
			Label = "no archetype + Oracle",
			ArchetypeName = null,
			Preferred = new string[1] { "OracleLegacyProgression" },
			FirstLegacy = "OracleLegacyProgression"
		},
		new Build
		{
			Label = "no archetype + Shifter",
			ArchetypeName = null,
			Preferred = new string[1] { "ShifterLegacy" },
			FirstLegacy = "ShifterLegacy"
		}
	};

	internal static SelfTestChecks.Outcome DualLegacyLevelUp()
	{
		BlueprintCharacterClass blueprintCharacterClass = IsekaiProtagonistClass.Get();
		BlueprintFeatureSelection modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "OverpoweredAbilitySelection");
		BlueprintFeatureSelection overwhelmingFeature = LegacySelection.GetOverwhelmingFeature();
		BlueprintFeatureSelection blueprintFeatureSelection = TriLegacySelection.Get();
		if (blueprintCharacterClass == null || modBlueprint == null || overwhelmingFeature == null || blueprintFeatureSelection == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The Isekai Protagonist class, the Overpowered Ability selection, Dual Legacy or Tri-Legacy does not exist, so there is no Dual Legacy to take.");
		}
		Tally tally = new Tally();
		Stopwatch stopwatch = Stopwatch.StartNew();
		foreach (Build build in Builds(blueprintCharacterClass))
		{
			tally.Builds++;
			BlueprintArchetype blueprintArchetype = null;
			if (build.ArchetypeName != null)
			{
				blueprintArchetype = blueprintCharacterClass.Archetypes.FirstOrDefault((BlueprintArchetype a) => a != null && a.name == build.ArchetypeName);
				if (blueprintArchetype == null)
				{
					tally.Fail(build.Label + ": the Isekai Protagonist offers no archetype named " + build.ArchetypeName + ".");
					continue;
				}
			}
			RunDualLegacyBuild(blueprintCharacterClass, blueprintArchetype, modBlueprint, build.TriLegacy ? blueprintFeatureSelection : overwhelmingFeature, build, tally);
		}
		stopwatch.Stop();
		string summary = $"{tally.Builds} builds (archetype and legacy pairs, the reported Martial God with the Basic Fighter legacy" + $" and Solo Sovereign among them) opened {tally.Windows} level-up windows and took Dual Legacy or Tri-Legacy in each;" + $" {tally.Held} legacies picked and held through a redraw, {tally.Refused} refused by their own prerequisites," + $" {tally.Introduced} choices they brought made, {tally.Rescued} pages kept behind the Overpowered Ability page that" + $" would have been drawn in front of it, {tally.Empty} with nothing to pick; {stopwatch.Elapsed.TotalSeconds:0} s;" + $" {tally.Failures} problems.";
		return SelfTestChecks.Outcome.Of(tally.Failures == 0, summary, tally.Details);
	}

	private static IEnumerable<Build> Builds(BlueprintCharacterClass isekai)
	{
		Build[] table = Table;
		for (int i = 0; i < table.Length; i++)
		{
			yield return table[i];
		}
		HashSet<string> covered = new HashSet<string>(from b in Table
			where b.ArchetypeName != null
			select b.ArchetypeName, StringComparer.Ordinal);
		foreach (BlueprintArchetype item in from a in ModArchetypes(isekai)
			where !covered.Contains(a.name)
			select a)
		{
			yield return new Build
			{
				Label = item.name,
				ArchetypeName = item.name
			};
		}
	}

	private static IEnumerable<BlueprintArchetype> ModArchetypes(BlueprintCharacterClass isekai)
	{
		HashSet<BlueprintGuid> modIds = new HashSet<BlueprintGuid>(from c in SelfTestProgressionChecks.ModBlueprints()
			select c.Guid);
		return isekai.Archetypes.Where((BlueprintArchetype a) => a != null && modIds.Contains(a.AssetGuid)).OrderBy((BlueprintArchetype a) => a.name, StringComparer.Ordinal).ToList();
	}

	private static void RunDualLegacyBuild(BlueprintCharacterClass isekai, BlueprintArchetype archetype, BlueprintFeatureSelection overpowered, BlueprintFeatureSelection list, Build build, Tally tally)
	{
		string label = build.Label;
		int num = (build.TriLegacy ? 10 : 5);
		string problem;
		using SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem);
		if (selfTestUnit == null)
		{
			tally.Fail(label + ": " + problem + ".");
			return;
		}
		HashSet<BlueprintGuid> hashSet = new HashSet<BlueprintGuid>();
		if (build.Preferred != null)
		{
			string[] preferred = build.Preferred;
			foreach (string name in preferred)
			{
				BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, name);
				if (modBlueprint != null)
				{
					hashSet.Add(modBlueprint.AssetGuid);
				}
			}
		}
		List<string> list2 = new List<string>();
		List<string> selectionNotes = new List<string>();
		for (int j = 1; j < num && SelfTestProgressionChecks.TakeLevel(selfTestUnit.Unit, isekai, archetype, j, label, list2, hashSet, selectionNotes); j++)
		{
		}
		foreach (string item in list2)
		{
			tally.Fail(item);
		}
		if (list2.Count > 0)
		{
			return;
		}
		if (build.FirstLegacy != null)
		{
			BlueprintFeature modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, build.FirstLegacy);
			if (!HasFact(selfTestUnit.Unit, modBlueprint2))
			{
				tally.Details.Add(label + ": the scripted levels did not end up with " + build.FirstLegacy + "; the sweep runs on what they took.");
			}
		}
		if (build.TriLegacy && !HasFact(selfTestUnit.Unit, LegacySelection.GetOverwhelmingFeature()))
		{
			tally.Fail($"{label}: the scripted levels did not take Dual Legacy at 5, so Tri-Legacy cannot be offered at {num}.");
			return;
		}
		int num2 = 0;
		foreach (BlueprintFeature item2 in DistinctFeatures(list))
		{
			if (DualLegacyWindow(selfTestUnit.Unit, isekai, overpowered, list, item2, num, label, tally))
			{
				num2++;
			}
		}
		if (num2 == 0)
		{
			tally.Fail($"{label}: {list.name} at level {num} offers this character no legacy at all, " + "so the Overpowered Ability page can never be finished once it is picked.");
		}
	}

	private static bool DualLegacyWindow(UnitEntityData unit, BlueprintCharacterClass isekai, BlueprintFeatureSelection overpowered, BlueprintFeatureSelection list, BlueprintFeature legacy, int level, string buildLabel, Tally tally)
	{
		string text = $"{buildLabel} / level {level} {list.name} -> {legacy.name}";
		tally.Windows++;
		Window window = null;
		string stage = "opening the window";
		try
		{
			window = Window.Open(unit, LevelUpState.CharBuildMode.LevelUp);
			LevelUpController controller = window.Controller;
			stage = "picking the class";
			if (!controller.SelectClass(isekai))
			{
				tally.Fail(text + ": the class page refused the Isekai Protagonist, so the level cannot even start.");
				return true;
			}
			FeatureSelectionState featureSelectionState = TopLevel(controller, overpowered);
			if (featureSelectionState == null)
			{
				tally.Fail($"{text}: level {level} has no Overpowered Ability choice to take {list.name} from.");
				return true;
			}
			IFeatureSelectionItem featureSelectionItem = ItemOf(overpowered, list);
			if (featureSelectionItem == null)
			{
				tally.Fail(text + ": the Overpowered Ability selection does not offer " + list.name + ".");
				return true;
			}
			if (!overpowered.CanSelect(controller.Preview.Descriptor, controller.State, featureSelectionState, featureSelectionItem))
			{
				tally.Fail($"{text}: the Overpowered Ability page refuses {list.name} to this character at level {level}.");
				return true;
			}
			stage = "picking " + list.name;
			string text2 = Pick(controller, featureSelectionState, featureSelectionItem);
			if (text2 != null)
			{
				tally.Fail(text + ": picking " + list.name + " on the Overpowered Ability page " + text2);
				return true;
			}
			FeatureSelectionState featureSelectionState2 = controller.State.Selections.FirstOrDefault((FeatureSelectionState s) => s != null && s.Parent != null && !s.Selected && s.Selection == list);
			if (featureSelectionState2 == null)
			{
				tally.Fail(text + ": picking " + list.name + " did not open its own list of legacies.");
				return true;
			}
			IFeatureSelectionItem featureSelectionItem2 = ItemOf(list, legacy);
			if (featureSelectionItem2 == null)
			{
				tally.Fail(text + ": " + list.name + " lists " + legacy.name + " but offers it as no item.");
				return true;
			}
			if (!list.CanSelect(controller.Preview.Descriptor, controller.State, featureSelectionState2, featureSelectionItem2))
			{
				tally.Refused++;
				return false;
			}
			stage = "picking the legacy";
			text2 = Pick(controller, featureSelectionState2, featureSelectionItem2);
			if (text2 != null)
			{
				tally.Fail(text + ": clicking the legacy on the " + list.name + " list " + text2 + " The level-up window redraws and nothing is marked, which is the reported Dual Legacy bug: the pick sorts ahead of the Overpowered Ability pick that created its list, and the engine's replay drops it.");
				return true;
			}
			stage = "making the legacy's own choices";
			List<KeyValuePair<IFeatureSelection, int>> introduced = MakeIntroducedChoices(controller, list, text, tally);
			stage = "redrawing";
			Redraw(controller);
			string text3 = StillHolds(controller, overpowered, list, topLevel: true) ?? StillHolds(controller, list, legacy, topLevel: false) ?? StillHasFact(controller, legacy) ?? StillMade(controller, introduced);
			if (text3 != null)
			{
				tally.Fail(text + ": after a redraw " + text3);
				return true;
			}
			tally.Held++;
			stage = "going back a page and picking again";
			string text4 = GoBackAndRepick(controller, overpowered, list, legacy);
			if (text4 != null)
			{
				tally.Fail(text + ": after going back to the Overpowered Ability page " + text4);
				return true;
			}
			stage = "reading the action order";
			CheckActionOrder(controller, text, tally);
			stage = "drawing the pages";
			window.View.UpdateAllPhases();
			FeatureSelectionState host = controller.State.Selections.FirstOrDefault((FeatureSelectionState s) => s != null && s.Parent == null && s.Selection == overpowered && s.SelectedItem?.Feature == list);
			CheckPagesBehind(window.View, controller, host, "the Overpowered Ability page", list, text, tally);
			return true;
		}
		catch (Exception e)
		{
			tally.Fail(Threw(text, stage, e));
			return true;
		}
		finally
		{
			window?.Dispose();
		}
	}

	private static string GoBackAndRepick(LevelUpController controller, BlueprintFeatureSelection overpowered, BlueprintFeatureSelection list, BlueprintFeature legacy)
	{
		FeatureSelectionState featureSelectionState = controller.State.Selections.FirstOrDefault((FeatureSelectionState s) => s != null && s.Parent == null && s.Selection == overpowered && s.SelectedItem?.Feature == list);
		if (featureSelectionState == null)
		{
			return "the Overpowered Ability pick was not there to take away.";
		}
		controller.UnselectFeature(featureSelectionState);
		FeatureSelectionState featureSelectionState2 = TopLevel(controller, overpowered);
		if (featureSelectionState2 == null)
		{
			return "the Overpowered Ability choice did not come back open.";
		}
		IFeatureSelectionItem featureSelectionItem = ItemOf(overpowered, list);
		if (featureSelectionItem == null || !overpowered.CanSelect(controller.Preview.Descriptor, controller.State, featureSelectionState2, featureSelectionItem))
		{
			return "the page refuses " + list.name + " the second time.";
		}
		string text = Pick(controller, featureSelectionState2, featureSelectionItem);
		if (text != null)
		{
			return "picking " + list.name + " again " + text;
		}
		FeatureSelectionState featureSelectionState3 = controller.State.Selections.FirstOrDefault((FeatureSelectionState s) => s != null && s.Parent != null && !s.Selected && s.Selection == list);
		if (featureSelectionState3 == null)
		{
			return "picking " + list.name + " again did not open its list of legacies.";
		}
		IFeatureSelectionItem featureSelectionItem2 = ItemOf(list, legacy);
		if (featureSelectionItem2 == null || !list.CanSelect(controller.Preview.Descriptor, controller.State, featureSelectionState3, featureSelectionItem2))
		{
			return "the list refuses " + legacy.name + " the second time.";
		}
		text = Pick(controller, featureSelectionState3, featureSelectionItem2);
		if (text != null)
		{
			return "clicking " + legacy.name + " again " + text;
		}
		Redraw(controller);
		return StillHolds(controller, list, legacy, topLevel: false) ?? StillHasFact(controller, legacy);
	}

	internal static SelfTestChecks.Outcome TwinSummonerLevelOne()
	{
		BlueprintCharacterClass blueprintCharacterClass = IsekaiProtagonistClass.Get();
		BlueprintFeatureSelection modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiPetSelection");
		BlueprintFeature modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TwinAvatarCompanion");
		BlueprintProgression blueprintProgression = TwinSummonerLegacy.Get();
		if (blueprintCharacterClass == null || modBlueprint == null || modBlueprint2 == null || blueprintProgression == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The Isekai Protagonist class, its pet list, the Twin Avatar or the Twin Summoner legacy does not exist, so the legacy cannot be taken.");
		}
		Tally tally = new Tally();
		Stopwatch stopwatch = Stopwatch.StartNew();
		List<BlueprintArchetype> list = new List<BlueprintArchetype>();
		list.Add(null);
		list.AddRange(ModArchetypes(blueprintCharacterClass));
		foreach (BlueprintArchetype item in list)
		{
			tally.Builds++;
			string text = ((item == null) ? "no archetype" : item.name);
			if (TwinSummonerWindow(blueprintCharacterClass, item, modBlueprint, modBlueprint2, blueprintProgression, anotherLegacyFirst: false, text, tally))
			{
				TwinSummonerWindow(blueprintCharacterClass, item, modBlueprint, modBlueprint2, blueprintProgression, anotherLegacyFirst: true, text + " (another legacy first)", tally);
			}
		}
		stopwatch.Stop();
		if (tally.Builds == tally.NotOffered)
		{
			tally.Fail("No build offers the Twin Summoner legacy at 1st level at all.");
		}
		string summary = $"{tally.Builds} builds at 1st level; {tally.Builds - tally.NotOffered} offer Twin Summoner and took it through" + $" {tally.Windows} character creations on the window's own controller, the Twin Avatar first and again after another" + $" legacy; {tally.Held} held" + $" through a redraw, {tally.Introduced} choices it brought made; {stopwatch.Elapsed.TotalSeconds:0} s; {tally.Failures} problems.";
		return SelfTestChecks.Outcome.Of(tally.Failures == 0, summary, tally.Details);
	}

	private static bool TwinSummonerWindow(BlueprintCharacterClass isekai, BlueprintArchetype archetype, BlueprintFeatureSelection pets, BlueprintFeature twin, BlueprintFeature legacy, bool anotherLegacyFirst, string label, Tally tally)
	{
		string problem;
		using SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem);
		if (selfTestUnit == null)
		{
			tally.Fail(label + ": " + problem + ".");
			return false;
		}
		Window window = null;
		string stage = "opening the window";
		try
		{
			window = Window.Open(selfTestUnit.Unit, LevelUpState.CharBuildMode.CharGen);
			LevelUpController controller = window.Controller;
			stage = "picking race and class";
			if (controller.State.CanSelectRace)
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
					controller.SelectRace(blueprintRace);
				}
			}
			if (controller.State.CanSelectRaceStat)
			{
				controller.SelectRaceStat(StatType.Constitution);
			}
			if (archetype != null && !controller.AddArchetype(isekai, archetype))
			{
				tally.Fail(label + ": the class page refused the archetype.");
				return false;
			}
			if (!controller.SelectClass(isekai))
			{
				tally.Fail(label + ": the class page refused the Isekai Protagonist.");
				return false;
			}
			BlueprintFeatureSelection list = (from s in controller.State.Selections
				where s != null && s.Parent == null && !s.Selected && LegacySelectionNestingPatch.IsLegacyList(s)
				select s.Selection as BlueprintFeatureSelection).FirstOrDefault((BlueprintFeatureSelection s) => s != null && s.AllFeatures.Contains(legacy));
			if (list == null)
			{
				if (!anotherLegacyFirst)
				{
					tally.NotOffered++;
				}
				return false;
			}
			tally.Windows++;
			if (TopLevel(controller, pets) == null)
			{
				tally.Fail(label + ": 1st level has no pet choice, so the Twin Avatar cannot be picked.");
				return true;
			}
			IFeatureSelectionItem featureSelectionItem = ItemOf(pets, twin);
			IFeatureSelectionItem featureSelectionItem2 = ItemOf(list, legacy);
			if (featureSelectionItem == null || featureSelectionItem2 == null)
			{
				tally.Fail(label + ": the pet list does not offer the Twin Avatar, or " + list.name + " does not offer Twin Summoner.");
				return true;
			}
			string text;
			if (anotherLegacyFirst)
			{
				stage = "picking another legacy first";
				FeatureSelectionState legacyState = TopLevel(controller, list);
				IFeatureSelectionItem featureSelectionItem3 = list.Items.FirstOrDefault((IFeatureSelectionItem i) => i != null && i.Feature != legacy && list.CanSelect(controller.Preview.Descriptor, controller.State, legacyState, i));
				if (featureSelectionItem3 == null)
				{
					tally.Details.Add(label + ": " + list.name + " offers no other legacy to pick first; that order is not tried.");
					return true;
				}
				text = Pick(controller, legacyState, featureSelectionItem3);
				if (text != null)
				{
					tally.Fail(label + ": picking " + featureSelectionItem3.Feature?.name + " on " + list.name + " " + text);
					return true;
				}
			}
			stage = "picking the Twin Avatar";
			text = Pick(controller, TopLevel(controller, pets), featureSelectionItem);
			if (text != null)
			{
				tally.Fail(label + ": picking the Twin Avatar on the pet page " + text);
				return true;
			}
			stage = "picking Twin Summoner";
			FeatureSelectionState featureSelectionState = controller.State.Selections.FirstOrDefault((FeatureSelectionState s) => s != null && s.Parent == null && s.Selection == list);
			if (anotherLegacyFirst)
			{
				controller.UnselectFeature(featureSelectionState);
				featureSelectionState = TopLevel(controller, list);
			}
			if (featureSelectionState == null || featureSelectionState.Selected)
			{
				tally.Fail(label + ": " + list.name + " cannot be picked again after the Twin Avatar.");
				return true;
			}
			if (!list.CanSelect(controller.Preview.Descriptor, controller.State, featureSelectionState, featureSelectionItem2))
			{
				tally.Fail(label + ": with the Twin Avatar picked, " + list.name + " still refuses Twin Summoner.");
				return true;
			}
			text = Pick(controller, featureSelectionState, featureSelectionItem2);
			if (text != null)
			{
				tally.Fail(label + ": picking Twin Summoner after the Twin Avatar " + text + " The legacy's pick sorts ahead of the pet pick, so the replay applies it before the twin exists, its prerequisite fails and it is dropped.");
				return true;
			}
			stage = "making the legacy's own choices";
			List<KeyValuePair<IFeatureSelection, int>> introduced = MakeIntroducedChoices(controller, list, label, tally);
			stage = "redrawing";
			Redraw(controller);
			string text2 = StillHolds(controller, pets, twin, topLevel: false) ?? StillHolds(controller, list, legacy, topLevel: false) ?? StillHasFact(controller, legacy) ?? StillMade(controller, introduced);
			if (text2 != null)
			{
				tally.Fail(label + ": after a redraw " + text2);
				return true;
			}
			tally.Held++;
			stage = "reading the action order";
			CheckActionOrder(controller, label, tally);
			stage = "ordering the pages";
			FeatureSelectionState state = controller.State.Selections.FirstOrDefault((FeatureSelectionState s) => s != null && s.Parent == null && s.Selection == pets);
			FeatureSelectionState state2 = controller.State.Selections.FirstOrDefault((FeatureSelectionState s) => s != null && s.Parent == null && s.Selection == list);
			int num = PageOrder(controller, state);
			int num2 = PageOrder(controller, state2);
			if (num > num2)
			{
				tally.Fail($"{label}: the pet page sorts at {num}, behind the legacy page at {num2}. The legacy page" + " then has to be finished before the Twin Avatar can be picked, so Twin Summoner is never selectable.");
			}
			foreach (FeatureSelectionState item in controller.State.Selections.Where((FeatureSelectionState s) => s != null && s.Parent == null && CameFrom(s, list)).ToList())
			{
				int num3 = PageOrder(controller, item);
				if (num3 <= num2)
				{
					tally.Fail($"{label}: the legacy's choice '{NameOf(item.Selection)}' sorts at {num3}, in front of the legacy page" + $" at {num2}, which makes the legacy page and every page after it stop responding to clicks.");
				}
			}
			return true;
		}
		catch (Exception e)
		{
			tally.Fail(Threw(label, stage, e));
			return true;
		}
		finally
		{
			window?.Dispose();
		}
	}

	private static int PageOrder(LevelUpController controller, FeatureSelectionState state)
	{
		CharGenFeatureSelectorPhaseVM obj = (CharGenFeatureSelectorPhaseVM)FormatterServices.GetUninitializedObject(typeof(CharGenFeatureSelectorPhaseVM));
		((CharGenPhaseBaseVM)obj).m_LevelUpController = controller;
		obj.m_ChargenPhasePriority = obj.GetFeaturePriority(state);
		obj.IndexInLevelupState = controller.State.Selections.IndexOf(state);
		CharGenFeatureSelectorStateVM charGenFeatureSelectorStateVM = (CharGenFeatureSelectorStateVM)FormatterServices.GetUninitializedObject(typeof(CharGenFeatureSelectorStateVM));
		charGenFeatureSelectorStateVM.SelectionState = state;
		obj.FeatureSelectorStateVM = charGenFeatureSelectorStateVM;
		return obj.OrderPriority;
	}

	private static void Redraw(LevelUpController controller)
	{
		controller.m_RecalculatePreview = true;
		controller.UpdatePreview();
	}

	private static bool HasFact(UnitEntityData unit, BlueprintFeature feature)
	{
		try
		{
			return feature != null && unit.Descriptor.HasFact(feature);
		}
		catch
		{
			return false;
		}
	}

	private static IEnumerable<BlueprintFeature> DistinctFeatures(BlueprintFeatureSelection list)
	{
		return (from f in list.AllFeatures
			where f != null
			group f by f.AssetGuid into g
			select g.First()).OrderBy((BlueprintFeature f) => f.name, StringComparer.Ordinal).ToList();
	}

	private static IFeatureSelectionItem ItemOf(BlueprintFeatureSelection selection, BlueprintFeature feature)
	{
		return selection.Items.FirstOrDefault((IFeatureSelectionItem i) => i != null && i.Feature == feature);
	}

	private static FeatureSelectionState TopLevel(LevelUpController controller, IFeatureSelection selection)
	{
		return controller.State.Selections.FirstOrDefault((FeatureSelectionState s) => s != null && s.Parent == null && !s.Selected && s.Selection == selection);
	}

	private static FeatureSelectionState Current(LevelUpController controller, IFeatureSelection selection, int index)
	{
		return controller.State.Selections.FirstOrDefault((FeatureSelectionState s) => s != null && s.Selection == selection && s.Index == index);
	}

	private static string NameOf(IFeatureSelection selection)
	{
		return (selection as BlueprintScriptableObject)?.name ?? selection?.GetType().Name ?? "(selection)";
	}

	private static string KeyOf(FeatureSelectionState state)
	{
		return ((state.Selection as SimpleBlueprint)?.AssetGuid.ToString() ?? state.Selection.GetType().Name) + ":" + state.Index;
	}

	private static string Threw(string label, string stage, Exception e)
	{
		IEnumerable<string> values = (from l in (e.StackTrace ?? "").Split('\n')
			select l.Trim() into l
			where l.Length > 0
			select l).Take(4);
		return label + ": " + stage + " threw " + e.GetType().Name + ": " + SelfTestReport.Clean(e.Message) + " at " + SelfTestReport.Clean(string.Join(" / ", values));
	}

	private static string Pick(LevelUpController controller, FeatureSelectionState state, IFeatureSelectionItem item)
	{
		if (state == null)
		{
			return "found no open choice to make.";
		}
		IFeatureSelection selection = state.Selection;
		int index = state.Index;
		bool num = controller.SelectFeature(state, item);
		FeatureSelectionState featureSelectionState = Current(controller, selection, index);
		string text = ((item.Feature != null) ? item.Feature.name : "(unnamed)");
		if (!num)
		{
			return "was refused ('" + text + "' was not accepted).";
		}
		if (featureSelectionState == null)
		{
			return "accepted '" + text + "' and then the list itself was gone.";
		}
		if (!featureSelectionState.Selected || featureSelectionState.SelectedItem?.Feature != item.Feature)
		{
			return "accepted '" + text + "' and then lost it, so it stays unmarked.";
		}
		return null;
	}

	private static bool CameFrom(FeatureSelectionState state, IFeatureSelection root)
	{
		FeatureSelectionState featureSelectionState = LegacySelectionNestingPatch.CreatorOf(state);
		int num = 0;
		while (featureSelectionState != null && num < 32)
		{
			if (featureSelectionState.Selection == root)
			{
				return true;
			}
			featureSelectionState = LegacySelectionNestingPatch.CreatorOf(featureSelectionState);
			num++;
		}
		return false;
	}

	private static List<KeyValuePair<IFeatureSelection, int>> MakeIntroducedChoices(LevelUpController controller, IFeatureSelection root, string label, Tally tally)
	{
		List<KeyValuePair<IFeatureSelection, int>> list = new List<KeyValuePair<IFeatureSelection, int>>();
		ProgressionRoot progressionRoot = BlueprintRoot.Instance?.Progression;
		HashSet<string> skipped = new HashSet<string>(StringComparer.Ordinal);
		for (int i = 0; i < 8; i++)
		{
			List<FeatureSelectionState> list2 = controller.State.Selections.Where((FeatureSelectionState s) => s != null && !s.Selected && CameFrom(s, root) && !skipped.Contains(KeyOf(s))).ToList();
			if (list2.Count == 0)
			{
				break;
			}
			foreach (FeatureSelectionState item2 in list2)
			{
				FeatureSelectionState now = Current(controller, item2.Selection, item2.Index);
				if (now == null || now.Selected)
				{
					continue;
				}
				string item = KeyOf(now);
				bool flag = now.Selection == progressionRoot?.BasicFeatSelection || now.Selection == progressionRoot?.DeitySelection;
				IFeatureSelectionItem featureSelectionItem = null;
				try
				{
					featureSelectionItem = now.Selection.Items.FirstOrDefault((IFeatureSelectionItem featureSelectionItem2) => featureSelectionItem2 != null && now.Selection.CanSelect(controller.Preview.Descriptor, controller.State, now, featureSelectionItem2));
				}
				catch
				{
					featureSelectionItem = null;
				}
				if (featureSelectionItem == null)
				{
					skipped.Add(item);
					if (!flag)
					{
						tally.Empty++;
					}
					continue;
				}
				string text = Pick(controller, now, featureSelectionItem);
				if (text != null)
				{
					skipped.Add(item);
					tally.Fail(label + ": the legacy's own choice '" + NameOf(now.Selection) + "' " + text + " The legacy can be taken but never finished.");
				}
				else
				{
					tally.Introduced++;
					list.Add(new KeyValuePair<IFeatureSelection, int>(now.Selection, now.Index));
				}
			}
		}
		return list;
	}

	private static string StillHolds(LevelUpController controller, IFeatureSelection selection, BlueprintFeature feature, bool topLevel)
	{
		if (!(controller.State.Selections.FirstOrDefault((FeatureSelectionState s) => s != null && s.Selection == selection && (!topLevel || s.Parent == null) && s.SelectedItem?.Feature == feature) == null))
		{
			return null;
		}
		return "'" + NameOf(selection) + "' no longer holds " + feature.name + ".";
	}

	private static string StillHasFact(LevelUpController controller, BlueprintFeature feature)
	{
		if (!HasFact(controller.Preview, feature))
		{
			return "the character being built no longer has " + feature.name + ".";
		}
		return null;
	}

	private static string StillMade(LevelUpController controller, List<KeyValuePair<IFeatureSelection, int>> introduced)
	{
		foreach (KeyValuePair<IFeatureSelection, int> item in introduced)
		{
			FeatureSelectionState featureSelectionState = Current(controller, item.Key, item.Value);
			if (featureSelectionState == null || !featureSelectionState.Selected)
			{
				return "the legacy's own choice '" + NameOf(item.Key) + "' is no longer made.";
			}
		}
		return null;
	}

	private static void CheckActionOrder(LevelUpController controller, string label, Tally tally)
	{
		List<ILevelUpAction> levelUpActions = controller.LevelUpActions;
		for (int i = 0; i < levelUpActions.Count; i++)
		{
			if (!(levelUpActions[i] is SelectFeature selectFeature))
			{
				continue;
			}
			FeatureSelectionState state = Current(controller, selectFeature.Selection, selectFeature.SelectionIndex);
			FeatureSelectionState creator = LegacySelectionNestingPatch.CreatorOf(state);
			if (!(creator == null) && LegacySelectionNestingPatch.ChainHasLegacyList(state))
			{
				int num = levelUpActions.FindIndex((ILevelUpAction a) => a is SelectFeature selectFeature2 && selectFeature2.Selection == creator.Selection && selectFeature2.SelectionIndex == creator.Index);
				string text = NameOf(selectFeature.Selection);
				string text2 = NameOf(creator.Selection);
				if (num < 0)
				{
					tally.Fail(label + ": the pick on '" + text + "' is in the level-up but the pick on '" + text2 + "' that created its list is not.");
				}
				else if (num > i)
				{
					tally.Fail(label + ": the pick on '" + text + "' is applied before the pick on '" + text2 + "' that creates its list.");
				}
				else if (levelUpActions[i].Priority < levelUpActions[num].Priority)
				{
					tally.Fail($"{label}: the pick on '{text}' sorts at {levelUpActions[i].Priority}, ahead of the pick on '{text2}' at" + $" {levelUpActions[num].Priority} that creates its list; any later out-of-order click would drop it.");
				}
			}
		}
	}

	private static int PageOf(List<CharGenPhaseBaseVM> pages, FeatureSelectionState state)
	{
		if (state == null)
		{
			return -1;
		}
		return pages.FindIndex(delegate(CharGenPhaseBaseVM p)
		{
			if (p is CharGenFeatureSelectorPhaseVM charGenFeatureSelectorPhaseVM)
			{
				FeatureSelectionState featureSelectionState = charGenFeatureSelectorPhaseVM.FeatureSelectorStateVM?.SelectionState;
				if ((object)featureSelectionState != null && featureSelectionState.Selection == state.Selection)
				{
					return featureSelectionState.Index == state.Index;
				}
			}
			return false;
		});
	}

	private static void CheckPagesBehind(CharGenVM view, LevelUpController controller, FeatureSelectionState host, string hostName, IFeatureSelection root, string label, Tally tally)
	{
		List<CharGenPhaseBaseVM> list = view.PhasesCollection.ToList();
		int num = PageOf(list, host);
		if (num < 0)
		{
			tally.Fail(label + ": the window draws no page for " + hostName + ".");
			return;
		}
		CharGenFeatureSelectorPhaseVM charGenFeatureSelectorPhaseVM = (CharGenFeatureSelectorPhaseVM)list[num];
		foreach (FeatureSelectionState item in controller.State.Selections.Where((FeatureSelectionState s) => s != null && s.Parent == null && CameFrom(s, root)).ToList())
		{
			int num2 = PageOf(list, item);
			if (num2 < 0)
			{
				continue;
			}
			if (num2 < num)
			{
				tally.Fail($"{label}: the legacy's choice '{NameOf(item.Selection)}' is drawn as page {num2}, in front of {hostName}" + $" ({num}) it was picked on, which makes that page and every page after it stop responding to clicks.");
			}
			else
			{
				CharGenFeatureSelectorPhaseVM charGenFeatureSelectorPhaseVM2 = (CharGenFeatureSelectorPhaseVM)list[num2];
				if (charGenFeatureSelectorPhaseVM2.GetBaseOrderPriority(charGenFeatureSelectorPhaseVM2.GetFeaturePriority(item)) + charGenFeatureSelectorPhaseVM2.IndexInLevelupState < charGenFeatureSelectorPhaseVM.OrderPriority)
				{
					tally.Rescued++;
				}
			}
		}
	}
}
