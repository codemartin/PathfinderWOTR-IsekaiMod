using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.NewUnitParts;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestSpellbookChecks
{
	private sealed class GuardedCase
	{
		internal string Component;

		internal BlueprintFeature Feature;

		internal BlueprintCharacterClass CharacterClass;

		internal FeatureParam Param;

		internal Func<UnitEntityData, bool> DidItsJob;
	}

	private const int VanillaLevelCap = 20;

	private const int UncappedLevelCap = 40;

	private const int MulticlassSplit = 10;

	private static List<KeyValuePair<BlueprintSpellbook, int>> BooksOf(UnitEntityData unit)
	{
		List<KeyValuePair<BlueprintSpellbook, int>> list = new List<KeyValuePair<BlueprintSpellbook, int>>();
		try
		{
			foreach (Spellbook spellbook in unit.Descriptor.Spellbooks)
			{
				if (spellbook != null)
				{
					list.Add(new KeyValuePair<BlueprintSpellbook, int>(spellbook.Blueprint, spellbook.CasterLevel));
				}
			}
		}
		catch
		{
		}
		return list;
	}

	private static void CheckBooks(UnitEntityData unit, string label, IEnumerable<KeyValuePair<BlueprintSpellbook, int>> owed, List<string> problems)
	{
		List<KeyValuePair<BlueprintSpellbook, int>> list = BooksOf(unit);
		List<KeyValuePair<BlueprintSpellbook, int>> list2 = owed.Where((KeyValuePair<BlueprintSpellbook, int> o) => o.Key != null).ToList();
		foreach (KeyValuePair<BlueprintSpellbook, int> want in list2)
		{
			KeyValuePair<BlueprintSpellbook, int> keyValuePair = list.FirstOrDefault((KeyValuePair<BlueprintSpellbook, int> a) => a.Key == want.Key);
			if (keyValuePair.Key == null)
			{
				problems.Add($"{label}: expected the spellbook '{want.Key.name}' at caster level {want.Value} and the character has none. " + "It holds " + SelfTestProgressionChecks.DescribeSpellbooks(unit) + ".");
			}
			else if (keyValuePair.Value != want.Value)
			{
				problems.Add($"{label}: '{want.Key.name}' is at caster level {keyValuePair.Value}, expected {want.Value}.");
			}
		}
		foreach (KeyValuePair<BlueprintSpellbook, int> have in list)
		{
			if (!list2.Any((KeyValuePair<BlueprintSpellbook, int> e) => e.Key == have.Key))
			{
				problems.Add(string.Format("{0}: an unexpected spellbook '{1}' at caster level {2}. ", label, (have.Key != null) ? have.Key.name : "(no blueprint)", have.Value) + "A book nothing owes the character is one a component demanded for a class it does not have. " + DemandedBy());
			}
		}
	}

	private static BlueprintSpellbook BookOf(BlueprintCharacterClass characterClass)
	{
		return characterClass?.Spellbook;
	}

	private static KeyValuePair<BlueprintSpellbook, int> Owed(BlueprintSpellbook book, int casterLevel)
	{
		return new KeyValuePair<BlueprintSpellbook, int>(book, casterLevel);
	}

	private static string DemandedBy()
	{
		List<string> list = SelfTestForeignSpellbookRecorder.Recorded();
		if (list.Count != 0)
		{
			return "Demanded by: " + string.Join("; ", list.ToArray()) + ".";
		}
		return "Nothing was recorded demanding it, so it was not created by a DemandSpellbook call during the level-up.";
	}

	private static void ReportSelectionNotes(List<string> notes, List<string> details)
	{
		List<string> list = notes.Distinct().OrderBy((string n) => n, StringComparer.Ordinal).ToList();
		if (list.Count != 0)
		{
			details.Add($"{list.Count} selections could not be filled by a scripted build and are noted, not failed " + "(the level still applied, and a human picking by hand would simply choose something):");
			details.AddRange(list);
		}
	}

	private static bool LevelThrough(UnitEntityData unit, IList<BlueprintCharacterClass> perLevel, BlueprintArchetype archetype, string label, List<string> problems, HashSet<BlueprintGuid> preferred = null, List<string> selectionNotes = null)
	{
		for (int i = 1; i <= perLevel.Count; i++)
		{
			BlueprintCharacterClass characterClass = perLevel[i - 1];
			BlueprintArchetype archetype2 = ((archetype != null && i == 1) ? archetype : null);
			if (!SelfTestProgressionChecks.TakeLevel(unit, characterClass, archetype2, i, label, problems, preferred, selectionNotes))
			{
				return false;
			}
		}
		return true;
	}

	private static IEnumerable<BlueprintCharacterClass> Run(BlueprintCharacterClass characterClass, int count)
	{
		for (int i = 0; i < count; i++)
		{
			yield return characterClass;
		}
	}

	internal static SelfTestChecks.Outcome GuardedComponents()
	{
		List<string> list = new List<string>();
		BlueprintCharacterClass blueprintCharacterClass = IsekaiProtagonistClass.Get();
		if (blueprintCharacterClass == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The Isekai Protagonist class does not exist, so the guard cannot be driven from the Isekai side.");
		}
		List<GuardedCase> cases = FindGuardedCases(list);
		List<string> notes = new List<string>();
		int num = 0;
		int num2 = 0;
		Stopwatch stopwatch = Stopwatch.StartNew();
		foreach (GuardedCase item in cases)
		{
			num++;
			num2 += RunPositiveDirection(item, list, notes);
			num2 += RunNegativeDirection(item, blueprintCharacterClass, list, notes);
		}
		stopwatch.Stop();
		ReportSelectionNotes(notes, list);
		foreach (string item2 in (from n in GuardedComponentNames()
			where !cases.Any((GuardedCase c) => c.Component == n)
			select n).ToList())
		{
			GuardedCase guardedCase = SyntheticCase(item2, out var why);
			if (guardedCase == null)
			{
				num2++;
				list.Add(item2 + ": no feature in the game carries this component and no stand-in could be built (" + why + "), so neither direction of the guard was driven for it and this component is NOT covered.");
				continue;
			}
			list.Add(item2 + ": no feature in the game carries this component, so the guard was driven with a stand-in feature built here, carrying that component and nothing else. It is never registered and never reaches a save.");
			num++;
			num2 += RunPositiveDirection(guardedCase, list, notes);
			num2 += RunNegativeDirection(guardedCase, blueprintCharacterClass, list, notes);
		}
		string summary = $"{num} of {GuardedComponentNames().Count} guarded components driven in both directions " + $"(the class present and the class absent) in {stopwatch.Elapsed.TotalSeconds:0} s; {num2} problems.";
		return SelfTestChecks.Outcome.Of(num2 == 0, summary, list);
	}

	private static GuardedCase SyntheticCase(string component, out string why)
	{
		why = null;
		BlueprintCharacterClass wizard = ClassTools.Classes.WizardClass;
		if (wizard == null)
		{
			why = "the Wizard class does not exist to scope the component to";
			return null;
		}
		try
		{
			BlueprintFeature blueprintFeature = new BlueprintFeature
			{
				name = "SelfTestStandIn" + component
			};
			BlueprintCharacterClassReference blueprintCharacterClassReference = wizard.ToReference<BlueprintCharacterClassReference>();
			switch (component)
			{
			case "AddOppositionDescriptor":
			{
				AddOppositionDescriptor addOppositionDescriptor = new AddOppositionDescriptor
				{
					m_CharacterClass = blueprintCharacterClassReference,
					m_Descriptor = SpellDescriptor.Fear
				};
				blueprintFeature.ComponentsArray = new BlueprintComponent[1] { addOppositionDescriptor };
				return new GuardedCase
				{
					Component = component,
					Feature = blueprintFeature,
					CharacterClass = wizard,
					DidItsJob = delegate(UnitEntityData unit)
					{
						Spellbook spellbook = BookFor(unit, wizard);
						return spellbook != null && (spellbook.OppositionDescriptors & SpellDescriptor.Fear) != 0;
					}
				};
			}
			case "LearnSpellParametrized":
			{
				BlueprintSpellList blueprintSpellList2 = wizard.Spellbook?.SpellList;
				BlueprintAbility spell = FirstSpellOf(blueprintSpellList2);
				if (spell == null)
				{
					why = "the Wizard spell list holds no spell to choose";
					return null;
				}
				LearnSpellParametrized learnSpellParametrized = new LearnSpellParametrized
				{
					m_SpellcasterClass = blueprintCharacterClassReference,
					m_SpellList = blueprintSpellList2.ToReference<BlueprintSpellListReference>(),
					SpecificSpellLevel = true,
					SpellLevel = 1
				};
				blueprintFeature.ComponentsArray = new BlueprintComponent[1] { learnSpellParametrized };
				return new GuardedCase
				{
					Component = component,
					Feature = blueprintFeature,
					CharacterClass = wizard,
					Param = new FeatureParam(spell),
					DidItsJob = (UnitEntityData unit) => BookFor(unit, wizard)?.IsKnown(spell) ?? false
				};
			}
			case "SpellKenningComponent":
			{
				BlueprintCharacterClass skaldClass = ClassTools.Classes.SkaldClass;
				BlueprintSpellbook blueprintSpellbook = skaldClass?.Spellbook;
				BlueprintSpellList blueprintSpellList = blueprintSpellbook?.SpellList;
				if (blueprintSpellbook == null || blueprintSpellList == null)
				{
					why = "the Skald class or its spellbook is absent, so there is no kenning book to name";
					return null;
				}
				SpellKenningComponent spellKenningComponent = new SpellKenningComponent();
				spellKenningComponent.m_Spellbook = blueprintSpellbook.ToReference<BlueprintSpellbookReference>();
				spellKenningComponent.m_SpellLists = new BlueprintSpellListReference[1] { blueprintSpellList.ToReference<BlueprintSpellListReference>() };
				SpellKenningComponent spellKenningComponent2 = spellKenningComponent;
				blueprintFeature.ComponentsArray = new BlueprintComponent[1] { spellKenningComponent2 };
				return new GuardedCase
				{
					Component = component,
					Feature = blueprintFeature,
					CharacterClass = skaldClass,
					DidItsJob = HasKenningPart
				};
			}
			default:
				why = "no stand-in is written for this component";
				return null;
			}
		}
		catch (Exception ex)
		{
			why = ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message);
			return null;
		}
	}

	private static List<string> GuardedComponentNames()
	{
		return new List<string> { "AddSpecialSpellList", "AddOppositionSchool", "AddOppositionDescriptor", "SpontaneousSpellConversion", "ScrollSpecialization", "LearnSpellParametrized", "SpellKenningComponent" };
	}

	private static BlueprintCharacterClass ClassGranting(BlueprintSpellbook book)
	{
		if (book == null)
		{
			return null;
		}
		foreach (BlueprintCharacterClass item in ScannedCasterClasses())
		{
			if (item != null && item.Spellbook == book)
			{
				return item;
			}
		}
		return null;
	}

	private static BlueprintAbility FirstSpellOf(BlueprintSpellList list)
	{
		try
		{
			foreach (SpellLevelList item in (list?.SpellsByLevel ?? new SpellLevelList[0]).OrderBy((SpellLevelList l) => l?.SpellLevel ?? 99))
			{
				if (item != null && item.SpellLevel != 0)
				{
					BlueprintAbility blueprintAbility = item.Spells?.FirstOrDefault((BlueprintAbility s) => s != null);
					if (blueprintAbility != null)
					{
						return blueprintAbility;
					}
				}
			}
		}
		catch
		{
		}
		return null;
	}

	private static int RunPositiveDirection(GuardedCase guarded, List<string> details, List<string> notes)
	{
		string text = guarded.Component + " on '" + guarded.Feature.name + "' with " + guarded.CharacterClass.name;
		List<string> list = new List<string>();
		string problem;
		using (SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem))
		{
			if (selfTestUnit == null)
			{
				details.Add(text + ": " + problem + ".");
				return 1;
			}
			if (!LevelThrough(selfTestUnit.Unit, Run(guarded.CharacterClass, 1).ToList(), null, text, list, null, notes))
			{
				details.AddRange(list);
				return list.Count;
			}
			try
			{
				selfTestUnit.Unit.Descriptor.AddFact(guarded.Feature, null, guarded.Param);
			}
			catch (Exception ex)
			{
				details.Add(text + ": adding the feature to a real " + guarded.CharacterClass.name + " threw " + ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message));
				return 1;
			}
			bool flag;
			try
			{
				flag = guarded.DidItsJob(selfTestUnit.Unit);
			}
			catch (Exception ex2)
			{
				details.Add(text + ": reading what the component did threw " + ex2.GetType().Name + ": " + SelfTestReport.Clean(ex2.Message));
				return 1;
			}
			if (!flag)
			{
				details.Add(text + ": a character WITH the class did not get what the component gives, so the guard is too broad and is taking something away from the characters the component was written for. The character holds " + SelfTestProgressionChecks.DescribeSpellbooks(selfTestUnit.Unit) + ".");
				return 1;
			}
		}
		details.AddRange(list);
		return list.Count;
	}

	private static int RunNegativeDirection(GuardedCase guarded, BlueprintCharacterClass isekai, List<string> details, List<string> notes)
	{
		string text = guarded.Component + " on '" + guarded.Feature.name + "' without " + guarded.CharacterClass.name;
		List<string> list = new List<string>();
		string problem;
		using (SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem))
		{
			if (selfTestUnit == null)
			{
				details.Add(text + ": " + problem + ".");
				return 1;
			}
			if (!LevelThrough(selfTestUnit.Unit, Run(isekai, 1).ToList(), null, text, list, null, notes))
			{
				details.AddRange(list);
				return list.Count;
			}
			List<KeyValuePair<BlueprintSpellbook, int>> owed = BooksOf(selfTestUnit.Unit);
			try
			{
				selfTestUnit.Unit.Descriptor.AddFact(guarded.Feature, null, guarded.Param);
			}
			catch (Exception ex)
			{
				details.Add(text + ": adding the feature to an Isekai Protagonist threw " + ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message));
				return 1;
			}
			CheckBooks(selfTestUnit.Unit, text, owed, list);
			bool flag = false;
			try
			{
				flag = guarded.DidItsJob(selfTestUnit.Unit);
			}
			catch
			{
			}
			if (flag)
			{
				list.Add(text + ": the component did its job on a character with no levels in " + guarded.CharacterClass.name + ", which is what the guard exists to stop.");
			}
		}
		details.AddRange(list);
		return list.Count;
	}

	private static List<GuardedCase> FindGuardedCases(List<string> details)
	{
		Dictionary<string, GuardedCase> dictionary = new Dictionary<string, GuardedCase>();
		foreach (BlueprintFeature item in CasterClassFeatures())
		{
			BlueprintComponent[] array = item.ComponentsArray ?? new BlueprintComponent[0];
			foreach (BlueprintComponent component in array)
			{
				GuardedCase guardedCase = DescribeComponent(item, component);
				if (guardedCase != null && guardedCase.CharacterClass != null && !dictionary.ContainsKey(guardedCase.Component))
				{
					dictionary[guardedCase.Component] = guardedCase;
				}
			}
		}
		if (dictionary.Count > 0)
		{
			details.Add("Features driving the guard: " + string.Join(", ", (from c in dictionary.Values.OrderBy((GuardedCase c) => c.Component, StringComparer.Ordinal)
				select c.Component + " from '" + c.Feature.name + "' (" + c.CharacterClass.name + ")").ToArray()) + ".");
		}
		return dictionary.Values.OrderBy((GuardedCase c) => c.Component, StringComparer.Ordinal).ToList();
	}

	private static GuardedCase DescribeComponent(BlueprintFeature feature, BlueprintComponent component)
	{
		AddSpecialSpellList specialList = component as AddSpecialSpellList;
		if (specialList != null)
		{
			if (specialList.ForArchetypeOnly || specialList.SpellList == null)
			{
				return null;
			}
			BlueprintSpellList list = specialList.SpellList;
			return new GuardedCase
			{
				Component = "AddSpecialSpellList",
				Feature = feature,
				CharacterClass = specialList.CharacterClass,
				DidItsJob = delegate(UnitEntityData unit)
				{
					Spellbook spellbook = BookFor(unit, specialList.CharacterClass);
					return spellbook != null && spellbook.SpecialLists?.Contains(list) == true;
				}
			};
		}
		AddOppositionSchool oppositionSchool = component as AddOppositionSchool;
		if (oppositionSchool != null)
		{
			SpellSchool school = oppositionSchool.School;
			return new GuardedCase
			{
				Component = "AddOppositionSchool",
				Feature = feature,
				CharacterClass = oppositionSchool.CharacterClass,
				DidItsJob = delegate(UnitEntityData unit)
				{
					Spellbook spellbook = BookFor(unit, oppositionSchool.CharacterClass);
					return spellbook != null && spellbook.OppositionSchools?.Contains(school) == true;
				}
			};
		}
		AddOppositionDescriptor oppositionDescriptor = component as AddOppositionDescriptor;
		if (oppositionDescriptor != null)
		{
			SpellDescriptor descriptor = oppositionDescriptor.m_Descriptor;
			if (descriptor == SpellDescriptor.None)
			{
				return null;
			}
			return new GuardedCase
			{
				Component = "AddOppositionDescriptor",
				Feature = feature,
				CharacterClass = oppositionDescriptor.CharacterClass,
				DidItsJob = delegate(UnitEntityData unit)
				{
					Spellbook spellbook = BookFor(unit, oppositionDescriptor.CharacterClass);
					return spellbook != null && (spellbook.OppositionDescriptors & descriptor) != 0;
				}
			};
		}
		SpontaneousSpellConversion conversion = component as SpontaneousSpellConversion;
		if (conversion != null)
		{
			if (conversion.m_SpellsByLevel == null || conversion.m_SpellsByLevel.Length == 0)
			{
				return null;
			}
			return new GuardedCase
			{
				Component = "SpontaneousSpellConversion",
				Feature = feature,
				CharacterClass = conversion.CharacterClass,
				DidItsJob = delegate(UnitEntityData unit)
				{
					Spellbook spellbook = BookFor(unit, conversion.CharacterClass);
					if (spellbook == null)
					{
						return false;
					}
					for (int i = 0; i < conversion.m_SpellsByLevel.Length; i++)
					{
						if (spellbook.GetSpontaneousConversionSpells(i).Any())
						{
							return true;
						}
					}
					return false;
				}
			};
		}
		if (component is ScrollSpecialization scrollSpecialization)
		{
			return new GuardedCase
			{
				Component = "ScrollSpecialization",
				Feature = feature,
				CharacterClass = scrollSpecialization.SpecializedClass,
				DidItsJob = (UnitEntityData unit) => unit.Get<UnitPartScrollSpecialization>()?.SpecializedClassSpellbook != null
			};
		}
		if (component is LearnSpellParametrized learnSpellParametrized)
		{
			BlueprintCharacterClass learnClass = learnSpellParametrized.SpellcasterClass;
			BlueprintAbility spell = FirstSpellOf(learnSpellParametrized.SpellList);
			if (learnClass == null || spell == null)
			{
				return null;
			}
			return new GuardedCase
			{
				Component = "LearnSpellParametrized",
				Feature = feature,
				CharacterClass = learnClass,
				Param = new FeatureParam(spell),
				DidItsJob = (UnitEntityData unit) => BookFor(unit, learnClass)?.IsKnown(spell) ?? false
			};
		}
		if (component is SpellKenningComponent spellKenningComponent)
		{
			BlueprintSpellbook obj = spellKenningComponent.m_Spellbook?.Get();
			BlueprintCharacterClass blueprintCharacterClass = ClassGranting(obj);
			if (obj == null || blueprintCharacterClass == null)
			{
				return null;
			}
			return new GuardedCase
			{
				Component = "SpellKenningComponent",
				Feature = feature,
				CharacterClass = blueprintCharacterClass,
				DidItsJob = HasKenningPart
			};
		}
		return null;
	}

	private static bool HasKenningPart(UnitEntityData unit)
	{
		try
		{
			return unit?.Parts?.Get<UnitPartSpellKenning>() != null;
		}
		catch
		{
			return false;
		}
	}

	private static Spellbook BookFor(UnitEntityData unit, BlueprintCharacterClass characterClass)
	{
		if (characterClass == null)
		{
			return null;
		}
		try
		{
			return unit.Descriptor.GetSpellbook(characterClass);
		}
		catch
		{
			return null;
		}
	}

	private static IEnumerable<BlueprintFeature> CasterClassFeatures()
	{
		HashSet<BlueprintGuid> seen = new HashSet<BlueprintGuid>();
		List<BlueprintFeature> list = new List<BlueprintFeature>();
		foreach (BlueprintCharacterClass item in ScannedCasterClasses())
		{
			if (item == null)
			{
				continue;
			}
			List<BlueprintFeatureBase> list2 = new List<BlueprintFeatureBase>();
			LevelEntry[] array = item.Progression?.LevelEntries ?? new LevelEntry[0];
			foreach (LevelEntry levelEntry in array)
			{
				if (levelEntry?.Features != null)
				{
					list2.AddRange(levelEntry.Features);
				}
			}
			foreach (BlueprintArchetype archetype in item.Archetypes)
			{
				array = archetype?.AddFeatures ?? new LevelEntry[0];
				foreach (LevelEntry levelEntry2 in array)
				{
					if (levelEntry2?.Features != null)
					{
						list2.AddRange(levelEntry2.Features);
					}
				}
			}
			foreach (BlueprintFeatureBase item2 in list2)
			{
				Collect(item2, seen, list, 0);
			}
		}
		return list;
	}

	private static void Collect(BlueprintFeatureBase blueprint, HashSet<BlueprintGuid> seen, List<BlueprintFeature> features, int depth)
	{
		if (blueprint == null || depth > 6 || !seen.Add(blueprint.AssetGuid))
		{
			return;
		}
		if (blueprint is BlueprintFeature item)
		{
			features.Add(item);
		}
		if (blueprint is BlueprintFeatureSelection { AllFeatures: var allFeatures })
		{
			foreach (BlueprintFeature item2 in allFeatures)
			{
				Collect(item2, seen, features, depth + 1);
			}
		}
		if (!(blueprint is BlueprintProgression blueprintProgression))
		{
			return;
		}
		LevelEntry[] array = blueprintProgression.LevelEntries ?? new LevelEntry[0];
		for (int i = 0; i < array.Length; i++)
		{
			foreach (BlueprintFeatureBase item3 in array[i]?.Features ?? new List<BlueprintFeatureBase>())
			{
				Collect(item3, seen, features, depth + 1);
			}
		}
	}

	private static IEnumerable<BlueprintCharacterClass> ScannedCasterClasses()
	{
		yield return ClassTools.Classes.WizardClass;
		yield return ClassTools.Classes.ArcanistClass;
		yield return ClassTools.Classes.ClericClass;
		yield return ClassTools.Classes.DruidClass;
		yield return ClassTools.Classes.SorcererClass;
		yield return ClassTools.Classes.MagusClass;
		yield return ClassTools.Classes.AlchemistClass;
		yield return ClassTools.Classes.WitchClass;
		yield return ClassTools.Classes.BardClass;
		yield return ClassTools.Classes.OracleClass;
		yield return ClassTools.Classes.ShamanClass;
		yield return ClassTools.Classes.InquisitorClass;
		yield return ClassTools.Classes.SkaldClass;
	}

	internal static SelfTestChecks.Outcome LegacySpellbooks()
	{
		List<string> list = new List<string>();
		BlueprintCharacterClass blueprintCharacterClass = IsekaiProtagonistClass.Get();
		if (blueprintCharacterClass == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The Isekai Protagonist class does not exist, so no legacy could be levelled.");
		}
		BlueprintSpellbook blueprintSpellbook = BookOf(blueprintCharacterClass);
		if (blueprintSpellbook == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The Isekai Protagonist class has no spellbook, so there is nothing to compare a legacy against.");
		}
		BlueprintFeatureSelection classFeature = LegacySelection.GetClassFeature();
		IEnumerable<BlueprintFeature> source;
		if (classFeature != null)
		{
			IEnumerable<BlueprintFeature> enumerable = classFeature.AllFeatures;
			source = enumerable;
		}
		else
		{
			source = Enumerable.Empty<BlueprintFeature>();
		}
		List<BlueprintFeature> list2 = (from f in source
			where f != null
			group f by f.AssetGuid into g
			select g.First()).OrderBy((BlueprintFeature f) => f.name, StringComparer.Ordinal).ToList();
		if (list2.Count == 0)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The legacy selection offers nothing, so the sweep covered no legacy at all.");
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		List<string> list3 = new List<string>();
		Stopwatch stopwatch = Stopwatch.StartNew();
		List<BlueprintCharacterClass> perLevel = Run(blueprintCharacterClass, 20).ToList();
		foreach (BlueprintFeature item in list2)
		{
			string text = "Isekai Protagonist + " + item.name;
			List<string> list4 = new List<string>();
			SelfTestForeignSpellbookRecorder.Reset();
			string problem;
			using (SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem))
			{
				if (selfTestUnit == null)
				{
					list.Add(text + ": " + problem + ".");
					num2++;
					continue;
				}
				HashSet<BlueprintGuid> preferred = new HashSet<BlueprintGuid> { item.AssetGuid };
				if (!LevelThrough(selfTestUnit.Unit, perLevel, null, text, list4, preferred, list3))
				{
					list.AddRange(list4);
					num2 += Math.Max(1, list4.Count);
					continue;
				}
				num++;
				bool flag;
				try
				{
					flag = selfTestUnit.Unit.Descriptor.HasFact(item);
				}
				catch
				{
					flag = false;
				}
				if (!flag)
				{
					num3++;
					list.Add(text + ": the legacy selection did not offer this legacy to a bare Isekai Protagonist, so this build proves nothing about it.");
					continue;
				}
				CheckBooks(selfTestUnit.Unit, text, new KeyValuePair<BlueprintSpellbook, int>[1] { Owed(blueprintSpellbook, 20) }, list4);
			}
			num2 += list4.Count;
			list.AddRange(list4);
		}
		stopwatch.Stop();
		ReportSelectionNotes(list3, list);
		string summary = $"{num} of {list2.Count} legacies levelled to {20} as a bare Isekai Protagonist" + $" ({num3} were not offered to this build); each must end with '{blueprintSpellbook.name}' at caster level {20} and no other book;" + $" {stopwatch.Elapsed.TotalSeconds:0} s; {num2} problems.";
		return SelfTestChecks.Outcome.Of(num2 == 0, summary, list);
	}

	internal static SelfTestChecks.Outcome MulticlassSpellbooks()
	{
		List<string> list = new List<string>();
		BlueprintCharacterClass blueprintCharacterClass = IsekaiProtagonistClass.Get();
		if (blueprintCharacterClass == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The Isekai Protagonist class does not exist, so no multiclass could be built.");
		}
		BlueprintSpellbook blueprintSpellbook = BookOf(blueprintCharacterClass);
		if (blueprintSpellbook == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The Isekai Protagonist class has no spellbook to multiclass with.");
		}
		int built = 0;
		int num = 0;
		List<string> list2 = new List<string>();
		Stopwatch stopwatch = Stopwatch.StartNew();
		foreach (BlueprintCharacterClass item in MatrixCasterClasses())
		{
			if (item == null)
			{
				continue;
			}
			BlueprintSpellbook blueprintSpellbook2 = BookOf(item);
			if (blueprintSpellbook2 == null)
			{
				list.Add(item.name + " has no spellbook of its own, so it was skipped.");
				continue;
			}
			KeyValuePair<BlueprintSpellbook, int>[] owed = new KeyValuePair<BlueprintSpellbook, int>[2]
			{
				Owed(blueprintSpellbook, 10),
				Owed(blueprintSpellbook2, 10)
			};
			bool[] array = new bool[2] { true, false };
			foreach (bool num2 in array)
			{
				string text = (num2 ? $"Isekai Protagonist {10} / {item.name} {10}" : $"{item.name} {10} / Isekai Protagonist {10}");
				List<BlueprintCharacterClass> perLevel = (num2 ? Run(blueprintCharacterClass, 10).Concat(Run(item, 10)).ToList() : Run(item, 10).Concat(Run(blueprintCharacterClass, 10)).ToList());
				List<string> list3 = new List<string>();
				SelfTestForeignSpellbookRecorder.Reset();
				string problem;
				using (SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem))
				{
					if (selfTestUnit == null)
					{
						list.Add(text + ": " + problem + ".");
						num++;
						continue;
					}
					if (LevelThrough(selfTestUnit.Unit, perLevel, null, text, list3, null, list2))
					{
						built++;
						CheckBooks(selfTestUnit.Unit, text, owed, list3);
					}
				}
				num += list3.Count;
				list.AddRange(list3);
			}
		}
		num += RunMysticTheurge(blueprintCharacterClass, blueprintSpellbook, list, list2, ref built);
		num += RunReplacementArchetypes(blueprintCharacterClass, list, list2, ref built);
		stopwatch.Stop();
		ReportSelectionNotes(list2, list);
		string summary = $"{built} multiclass builds ({10} and {10} in both orders for each caster class, " + "the Mystic Theurge case and the book-replacing archetypes); " + $"{stopwatch.Elapsed.TotalSeconds:0} s; {num} problems.";
		return SelfTestChecks.Outcome.Of(num == 0, summary, list);
	}

	private static IEnumerable<BlueprintCharacterClass> MatrixCasterClasses()
	{
		yield return ClassTools.Classes.ArcanistClass;
		yield return ClassTools.Classes.WizardClass;
		yield return ClassTools.Classes.SorcererClass;
		yield return ClassTools.Classes.MagusClass;
		yield return ClassTools.Classes.ClericClass;
		yield return ClassTools.Classes.DruidClass;
	}

	private static int RunMysticTheurge(BlueprintCharacterClass isekai, BlueprintSpellbook isekaiBook, List<string> details, List<string> notes, ref int built)
	{
		BlueprintCharacterClass mysticTheurgeClass = ClassTools.Classes.MysticTheurgeClass;
		BlueprintCharacterClass clericClass = ClassTools.Classes.ClericClass;
		if (mysticTheurgeClass == null || clericClass == null)
		{
			details.Add("The Mystic Theurge or Cleric class does not exist in this install, so that case was not built.");
			return 0;
		}
		string text = $"Isekai Protagonist {6} / Cleric {4} / Mystic Theurge {10}";
		List<string> list = new List<string>();
		SelfTestForeignSpellbookRecorder.Reset();
		string problem;
		using (SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem))
		{
			if (selfTestUnit == null)
			{
				details.Add(text + ": " + problem + ".");
				return 1;
			}
			List<BlueprintCharacterClass> perLevel = Run(isekai, 6).Concat(Run(clericClass, 4)).Concat(Run(mysticTheurgeClass, 10)).ToList();
			if (!LevelThrough(selfTestUnit.Unit, perLevel, null, text, list, null, notes))
			{
				details.AddRange(list);
				return Math.Max(1, list.Count);
			}
			built++;
			details.Add(text + ": " + SelfTestProgressionChecks.DescribeSpellbooks(selfTestUnit.Unit) + ".");
			foreach (KeyValuePair<BlueprintSpellbook, int> item in BooksOf(selfTestUnit.Unit))
			{
				if (item.Value <= 0)
				{
					list.Add(text + ": '" + ((item.Key != null) ? item.Key.name : "(no blueprint)") + "' is at caster level 0, which is what a book demanded by a component for a class the character does not have looks like. " + DemandedBy());
				}
			}
			if (!BooksOf(selfTestUnit.Unit).Any((KeyValuePair<BlueprintSpellbook, int> b) => b.Key == isekaiBook))
			{
				list.Add(text + ": the Isekai book is gone; the Mystic Theurge levels replaced it rather than advancing it.");
			}
		}
		details.AddRange(list);
		return list.Count;
	}

	private static int RunReplacementArchetypes(BlueprintCharacterClass isekai, List<string> details, List<string> notes, ref int built)
	{
		List<string> list = new List<string>();
		List<BlueprintArchetype> list2 = isekai.Archetypes.Where((BlueprintArchetype a) => a != null && a.m_ReplaceSpellbook != null && a.m_ReplaceSpellbook.Get() != null).OrderBy((BlueprintArchetype a) => a.name, StringComparer.Ordinal).ToList();
		if (list2.Count == 0)
		{
			details.Add("No Isekai archetype declares a replacement spellbook, so that group was empty.");
			return 0;
		}
		foreach (BlueprintArchetype item in list2)
		{
			BlueprintSpellbook book = item.m_ReplaceSpellbook.Get();
			string text = $"Isekai Protagonist + {item.name} to {20}";
			SelfTestForeignSpellbookRecorder.Reset();
			string problem;
			using SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem);
			if (selfTestUnit == null)
			{
				details.Add(text + ": " + problem + ".");
				list.Add(text + ": no unit could be built.");
			}
			else if (LevelThrough(selfTestUnit.Unit, Run(isekai, 20).ToList(), item, text, list, null, notes))
			{
				built++;
				CheckBooks(selfTestUnit.Unit, text, new KeyValuePair<BlueprintSpellbook, int>[1] { Owed(book, 20) }, list);
			}
		}
		details.AddRange(list);
		return list.Count;
	}

	internal static SelfTestChecks.Outcome RespecRebuild()
	{
		List<string> list = new List<string>();
		BlueprintCharacterClass blueprintCharacterClass = IsekaiProtagonistClass.Get();
		BlueprintCharacterClass blueprintCharacterClass2 = IsekaiAscendantClass.Get();
		if (blueprintCharacterClass == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The Isekai Protagonist class does not exist, so nothing could be respecced.");
		}
		bool flag = false;
		try
		{
			flag = Main.IsekaiContext.AddedContent.EnableLevelUncapping;
		}
		catch
		{
		}
		if (flag && blueprintCharacterClass2 == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "Level uncapping is on but the Isekai Ascendant class does not exist, so no 20/20 character can be respecced.");
		}
		int num = (flag ? 40 : 20);
		Stopwatch stopwatch = Stopwatch.StartNew();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		List<BlueprintCharacterClass> list4 = (flag ? Run(blueprintCharacterClass, 20).Concat(Run(blueprintCharacterClass2, 20)).ToList() : Run(blueprintCharacterClass, 20).ToList());
		string text = (flag ? "Isekai Protagonist 20 / Isekai Ascendant 20" : "Isekai Protagonist 20 (uncapping off)");
		bool flag2 = false;
		string problem;
		using (SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem))
		{
			if (selfTestUnit == null)
			{
				return SelfTestChecks.Outcome.Of(passed: false, "No blank unit could be created, so no character could be respecced: " + problem + ".");
			}
			if (!LevelThrough(selfTestUnit.Unit, list4, null, "the original " + text, list2, null, list3))
			{
				return SelfTestChecks.Outcome.Of(passed: false, "The " + text + " character could not be built, so there was nothing to respec.", list2);
			}
			int count = BooksOf(selfTestUnit.Unit).Count;
			int num2 = SelfTestProgressionChecks.HighestCasterLevel(selfTestUnit.Unit);
			int num3 = ClassLevel(selfTestUnit.Unit, blueprintCharacterClass);
			int num4 = ClassLevel(selfTestUnit.Unit, blueprintCharacterClass2);
			int experience = selfTestUnit.Unit.Descriptor.Progression.Experience;
			list.Add($"The original: Isekai {num3} / Ascendant {num4}, {count} spellbook(s) " + $"[{SelfTestProgressionChecks.DescribeSpellbooks(selfTestUnit.Unit)}], caster level {num2}, {experience} XP.");
			UnitEntityData unitEntityData = null;
			bool flag3 = false;
			try
			{
				string text2 = SelfTestRespecChecks.TryEngineRespec(selfTestUnit.Unit, out var config);
				string text3;
				if (text2 == null)
				{
					unitEntityData = config.Unit;
					flag3 = true;
					flag2 = true;
					text3 = "the engine's UnitHelper.Respec built the respec unit and asked for the Respec window";
				}
				else
				{
					unitEntityData = SelfTestRespecChecks.RespecUnitLikeTheEngine(selfTestUnit.Unit);
					flag3 = true;
					text3 = "UnitHelper.Respec stopped before the window (" + text2 + "), so the respec unit was built the way it builds it";
				}
				list.Add("Respec: " + text3 + ".");
				SelfTestLevelUpWindow.Plan plan = new SelfTestLevelUpWindow.Plan
				{
					Class = blueprintCharacterClass,
					Mode = LevelUpState.CharBuildMode.Respec
				};
				plan.BeforeFill = delegate(LevelUpController controller, SelfTestLevelUpWindow.Result result2)
				{
					if (controller.State.Mode != LevelUpState.CharBuildMode.Respec)
					{
						result2.Problems.Add($"the respec level-up state is in {controller.State.Mode} mode, not Respec.");
					}
				};
				SelfTestLevelUpWindow.Result result = SelfTestLevelUpWindow.DriveLevel(unitEntityData, plan, "the respec, level 1 in Respec mode through the window");
				list2.AddRange(result.Problems);
				list2.AddRange(result.Lost.Select((KeyValuePair<string, string> l) => l.Value));
				list2.AddRange(result.Refused.Select((KeyValuePair<string, string> r) => r.Value));
				if (result.Committed)
				{
					if (unitEntityData.PreviewOf.Entity != selfTestUnit.Unit)
					{
						list2.Add("After the Respec-mode level the respec unit no longer points at the original through PreviewOf.");
					}
					if (!LevelUncapperPatch.StartedAsIsekaiUnit(unitEntityData))
					{
						list2.Add("While the respec is open the respec unit is not judged an Isekai-first character (LevelUncapperPatch.StartedAsIsekaiUnit).");
					}
					int maxCharacterLevel = unitEntityData.Descriptor.Progression.MaxCharacterLevel;
					if (maxCharacterLevel < num)
					{
						list2.Add($"While the respec is open the respec unit can only reach level {maxCharacterLevel}, the original reached {num}.");
					}
					if (unitEntityData.Descriptor.Progression.Experience != experience)
					{
						list2.Add($"The respec unit has {unitEntityData.Descriptor.Progression.Experience} XP, the original {experience}.");
					}
					bool flag4 = true;
					for (int num5 = 2; (num5 <= list4.Count) & flag4; num5++)
					{
						flag4 = SelfTestProgressionChecks.TakeLevel(unitEntityData, list4[num5 - 1], null, num5, "the respecced character", list2, null, list3);
					}
					if (flag4)
					{
						int count2 = BooksOf(unitEntityData).Count;
						int num6 = SelfTestProgressionChecks.HighestCasterLevel(unitEntityData);
						int num7 = ClassLevel(unitEntityData, blueprintCharacterClass);
						int num8 = ClassLevel(unitEntityData, blueprintCharacterClass2);
						list.Add($"The rebuild: Isekai {num7} / Ascendant {num8}, {count2} spellbook(s) " + $"[{SelfTestProgressionChecks.DescribeSpellbooks(unitEntityData)}], caster level {num6}.");
						if (count2 != count)
						{
							list2.Add($"The rebuilt character has {count2} spellbook(s), the original had {count}.");
						}
						if (num6 != num2)
						{
							list2.Add($"The rebuilt character is at caster level {num6}, the original at {num2}.");
						}
						if (num7 != num3)
						{
							list2.Add($"The rebuilt character has {num7} Isekai Protagonist levels, the original had {num3}.");
						}
						if (num8 != num4)
						{
							list2.Add($"The rebuilt character has {num8} Isekai Ascendant levels, the original had {num4}.");
						}
						if (flag)
						{
							string[] array = new string[2] { "TranscendentProtagonistFeature", "AnimeFinalFormFeature" };
							foreach (string text4 in array)
							{
								BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, text4);
								if (modBlueprint == null)
								{
									list2.Add("The capstone '" + text4 + "' does not exist, so the rebuild could not be checked for it.");
									continue;
								}
								bool flag5;
								try
								{
									flag5 = unitEntityData.Descriptor.HasFact(modBlueprint);
								}
								catch
								{
									flag5 = false;
								}
								if (!flag5)
								{
									list2.Add("The rebuilt character does not have the capstone '" + text4 + "'.");
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				list2.Add("The respec threw " + ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message));
			}
			finally
			{
				if (flag3)
				{
					try
					{
						unitEntityData?.Dispose();
					}
					catch
					{
					}
				}
			}
		}
		stopwatch.Stop();
		string summary = "A " + text + " character taken into the engine's respec (" + (flag2 ? "UnitHelper.Respec reached the window" : "UnitHelper.Respec stops at the main menu, so its respec unit was built the same way") + ")," + $" level 1 rebuilt in Respec mode through the window path and the rest levelled back to {num}, then compared with the original." + " RespecOnCommit, the party swap, needs a loaded game and is NOT run here (save-respec runs it with -SaveName). " + $"{stopwatch.Elapsed.TotalSeconds:0} s; {list2.Count} problems.";
		list.AddRange(list2);
		ReportSelectionNotes(list3, list);
		return SelfTestChecks.Outcome.Of(list2.Count == 0, summary, list);
	}

	private static int ClassLevel(UnitEntityData unit, BlueprintCharacterClass characterClass)
	{
		try
		{
			return (characterClass != null) ? unit.Descriptor.Progression.GetClassLevel(characterClass) : 0;
		}
		catch
		{
			return 0;
		}
	}
}
