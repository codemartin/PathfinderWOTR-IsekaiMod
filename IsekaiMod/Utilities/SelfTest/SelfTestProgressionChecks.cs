using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Root;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestProgressionChecks
{
	private sealed class BuildResult
	{
		internal string Label;

		internal int LevelsReached;

		internal string Note;

		internal readonly List<string> Problems = new List<string>();

		internal List<string> FeaturesAtEnd;

		internal List<BlueprintArchetype> AscendantArchetypes;
	}

	private const int VanillaLevelCap = 20;

	private const int UncappedLevelCap = 40;

	internal static List<SelfTestRecorder.CreatedBlueprint> ModBlueprints()
	{
		Dictionary<BlueprintGuid, SelfTestRecorder.CreatedBlueprint> dictionary = new Dictionary<BlueprintGuid, SelfTestRecorder.CreatedBlueprint>();
		foreach (SelfTestRecorder.CreatedBlueprint item in SelfTestRecorder.CreatedBlueprints())
		{
			if (item.Blueprint != null)
			{
				dictionary[item.Guid] = item;
			}
		}
		return dictionary.Values.OrderBy((SelfTestRecorder.CreatedBlueprint c) => c.Name, StringComparer.Ordinal).ToList();
	}

	internal static IEnumerable<T> ModBlueprintsOf<T>() where T : SimpleBlueprint
	{
		return (from c in ModBlueprints()
			select c.Blueprint).OfType<T>();
	}

	internal static SelfTestChecks.Outcome LevelUpSimulation()
	{
		List<string> list = new List<string>();
		BlueprintCharacterClass blueprintCharacterClass = IsekaiProtagonistClass.Get();
		if (blueprintCharacterClass == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The Isekai Protagonist class does not exist, so nothing could be levelled.");
		}
		BlueprintCharacterClass ascendant = IsekaiAscendantClass.Get();
		HashSet<BlueprintGuid> hashSet = new HashSet<BlueprintGuid>(from c in ModBlueprints()
			select c.Guid);
		List<BlueprintArchetype> list2 = new List<BlueprintArchetype>();
		foreach (BlueprintArchetype archetype in blueprintCharacterClass.Archetypes)
		{
			if (archetype != null && hashSet.Contains(archetype.AssetGuid))
			{
				list2.Add(archetype);
			}
		}
		list2 = list2.OrderBy((BlueprintArchetype a) => a.name, StringComparer.Ordinal).ToList();
		bool flag = false;
		try
		{
			flag = Main.IsekaiContext.AddedContent.EnableLevelUncapping;
		}
		catch
		{
		}
		int num = (flag ? 40 : 20);
		if (flag && ascendant == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "Uncapping is on but the Isekai Ascendant class does not exist, so character levels 21-40 cannot be reached at all.");
		}
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		Stopwatch stopwatch = Stopwatch.StartNew();
		List<BlueprintArchetype> list3 = new List<BlueprintArchetype>();
		list3.Add(null);
		list3.AddRange(list2);
		foreach (BlueprintArchetype item in list3)
		{
			num2++;
			string text = ((item == null) ? (blueprintCharacterClass.name + " (no archetype)") : (blueprintCharacterClass.name + " + " + item.name));
			BlueprintArchetype mirror = ((item == null) ? null : MirrorArchetypeOf(ascendant, item));
			if (item != null && ascendant != null && mirror == null)
			{
				list.Add(text + ": no ascension mirror archetype is gated on it, so above level 20 it would be handed back the features it gives up.");
				num4++;
			}
			BuildResult buildResult = LevelOneBuild(blueprintCharacterClass, item, ascendant, mirror, num, text, hashSet);
			num3 += buildResult.LevelsReached;
			num4 += buildResult.Problems.Count;
			list.AddRange(buildResult.Problems);
			if (buildResult.Note != null)
			{
				list.Add(buildResult.Note);
			}
			if (num <= 20 || mirror == null)
			{
				continue;
			}
			num5++;
			string bareLabel = text + " (Isekai Ascendant asked for WITHOUT the mirror " + mirror.name + ")";
			BuildResult buildResult2 = LevelOneBuild(blueprintCharacterClass, item, ascendant, null, num, bareLabel, hashSet, (UnitEntityData unit) => RefusalProblems(unit, ascendant, mirror, bareLabel));
			num3 += buildResult2.LevelsReached;
			num4 += buildResult2.Problems.Count;
			list.AddRange(buildResult2.Problems);
			if (buildResult2.Note != null)
			{
				list.Add(buildResult2.Note);
			}
			string text2 = MirrorlessExtras(buildResult, buildResult2);
			if (text2 != null)
			{
				num4++;
				list.Add(text2);
			}
			if (buildResult2.AscendantArchetypes != null && !buildResult2.AscendantArchetypes.Contains(mirror))
			{
				num4++;
				list.Add(bareLabel + ": the finished character does not hold " + mirror.name + " on the Isekai Ascendant (" + ((buildResult2.AscendantArchetypes.Count == 0) ? "no archetype" : string.Join(", ", buildResult2.AscendantArchetypes.Select((BlueprintArchetype x) => x.name))) + "), so the required mirror was never added when the class was taken.");
			}
		}
		stopwatch.Stop();
		string arg = ((num > 20) ? $"Isekai Protagonist 1-{20} then Isekai Ascendant to {num}" : $"Isekai Protagonist 1-{num}");
		string summary = $"{num2} builds ({list2.Count} mod archetypes plus the bare class) levelled as {arg}" + ((num5 > 0) ? $", and {num5} of the archetypes again with the Isekai Ascendant asked for without its required mirror (it must be added anyway, and the window must refuse the level without it)" : "") + string.Format(" (uncapping is {0}); {1} levels applied in {2:0} s; {3} problems", flag ? "on" : "off", num3, stopwatch.Elapsed.TotalSeconds, num4) + string.Format(", {0} allow-listed known issues.", list.Count((string d) => d.StartsWith("KNOWN ISSUE ", StringComparison.Ordinal)));
		return SelfTestChecks.Outcome.Of(num4 == 0, summary, list);
	}

	private static string MirrorlessExtras(BuildResult withMirror, BuildResult withoutMirror)
	{
		if (withMirror.FeaturesAtEnd == null || withoutMirror.FeaturesAtEnd == null)
		{
			return null;
		}
		List<string> list = (from x in (from g in withoutMirror.FeaturesAtEnd.GroupBy((string n) => n, StringComparer.Ordinal)
				select new
				{
					Name = g.Key,
					Count = g.Count() - withMirror.FeaturesAtEnd.Count((string n) => string.Equals(n, g.Key, StringComparison.Ordinal))
				} into x
				where x.Count > 0
				select x).OrderBy(x => x.Name, StringComparer.Ordinal)
			select (x.Count <= 1) ? x.Name : $"{x.Name} x{x.Count}").ToList();
		if (list.Count == 0)
		{
			return null;
		}
		return withoutMirror.Label + " ends at level 40 with " + list.Count + " feature(s) the mirror build does not have: " + string.Join(", ", list) + ". The mirror is required (owner decision 2), so the archetype must not win back what it gave up.";
	}

	private static List<string> RefusalProblems(UnitEntityData unit, BlueprintCharacterClass ascendant, BlueprintArchetype mirror, string label)
	{
		List<string> problems = new List<string>();
		SelfTestLevelUpWindow.Plan plan = new SelfTestLevelUpWindow.Plan
		{
			Class = ascendant,
			Commit = false,
			FillPages = false
		};
		plan.BeforeFill = delegate(LevelUpController controller, SelfTestLevelUpWindow.Result result2)
		{
			ClassData classData = controller.Preview.Descriptor.Progression.GetClassData(ascendant);
			if (classData == null || !classData.Archetypes.Contains(mirror))
			{
				problems.Add(label + ": taking the Isekai Ascendant in the level-up window did not add the required " + mirror.name + ".");
			}
			if (AscendantMirrorLock.IsMissingMirror(controller.State, controller.Preview.Descriptor, out var _))
			{
				problems.Add(label + ": with " + mirror.name + " added, the Ascendant's first level is still judged to be missing it.");
			}
			controller.RemoveArchetype(mirror);
			if (!AscendantMirrorLock.IsMissingMirror(controller.State, controller.Preview.Descriptor, out var mirror3) || mirror3 != mirror)
			{
				problems.Add(label + ": with " + mirror.name + " switched off, the Ascendant's first level is not refused" + ((mirror3 != null && mirror3 != mirror) ? (" (it asks for " + mirror3.name + " instead)") : "") + ", so the no-mirror path can be committed.");
			}
			else if (controller.State.IsComplete())
			{
				problems.Add(label + ": with " + mirror.name + " switched off, LevelUpState.IsComplete still says the level can be committed.");
			}
		};
		SelfTestLevelUpWindow.Result result = SelfTestLevelUpWindow.DriveLevel(unit, plan, label + ", Isekai Ascendant level 1 through the window");
		problems.AddRange(result.Problems);
		return problems;
	}

	private static BlueprintArchetype MirrorArchetypeOf(BlueprintCharacterClass ascendant, BlueprintArchetype isekaiArchetype)
	{
		if (ascendant == null || isekaiArchetype == null)
		{
			return null;
		}
		foreach (BlueprintArchetype archetype in ascendant.Archetypes)
		{
			if (archetype == null)
			{
				continue;
			}
			foreach (PrerequisiteArchetypeLevel component in archetype.GetComponents<PrerequisiteArchetypeLevel>())
			{
				if (component?.Archetype == isekaiArchetype)
				{
					return archetype;
				}
			}
		}
		return null;
	}

	private static BuildResult LevelOneBuild(BlueprintCharacterClass isekai, BlueprintArchetype archetype, BlueprintCharacterClass ascendant, BlueprintArchetype mirror, int target, string label, HashSet<BlueprintGuid> modIds, Func<UnitEntityData, List<string>> beforeAscendant = null)
	{
		BuildResult buildResult = new BuildResult
		{
			Label = label
		};
		string problem;
		using (SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem))
		{
			if (selfTestUnit == null)
			{
				buildResult.Problems.Add(label + ": " + problem + ".");
				return buildResult;
			}
			int casterLevelAtTwenty = -1;
			string booksAtTwenty = "none";
			for (int i = 1; i <= target; i++)
			{
				bool num = i > 20;
				BlueprintCharacterClass blueprintCharacterClass = (num ? ascendant : isekai);
				BlueprintArchetype archetype2 = (num ? mirror : archetype);
				if (i == 21 && beforeAscendant != null)
				{
					try
					{
						buildResult.Problems.AddRange(beforeAscendant(selfTestUnit.Unit));
					}
					catch (Exception ex)
					{
						buildResult.Problems.Add(label + ": probing the Isekai Ascendant's first level threw " + ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message));
					}
				}
				if (blueprintCharacterClass == null)
				{
					buildResult.Problems.Add($"{label}: character level {i} has no class to take, so the build stops there.");
					return buildResult;
				}
				try
				{
					AddOneLevel(selfTestUnit.Unit, blueprintCharacterClass, archetype2, i, label, buildResult.Problems);
				}
				catch (Exception ex2)
				{
					buildResult.Problems.Add($"{label}: level {i} threw {ex2.GetType().Name}: {SelfTestReport.Clean(ex2.Message)}");
					return buildResult;
				}
				int num2 = 0;
				try
				{
					num2 = selfTestUnit.Unit.Descriptor.Progression.CharacterLevel;
				}
				catch
				{
				}
				if (num2 < i)
				{
					buildResult.Problems.Add($"{label}: the unit is level {num2} after asking for level {i}, so the level-up did not apply.");
					return buildResult;
				}
				buildResult.LevelsReached = num2;
				if (i == 1)
				{
					CheckLevelOneFeatures(selfTestUnit.Unit, isekai, archetype, label, buildResult.Problems);
					CheckCantripsAreListed(selfTestUnit.Unit, modIds, label, buildResult.Problems);
				}
				if (i == 20)
				{
					casterLevelAtTwenty = HighestCasterLevel(selfTestUnit.Unit);
					booksAtTwenty = DescribeSpellbooks(selfTestUnit.Unit);
					CheckExclusivePowers(selfTestUnit.Unit, archetype, label, buildResult.Problems);
				}
			}
			if (target > 20)
			{
				buildResult.Note = CheckAscendedCharacter(selfTestUnit.Unit, isekai, ascendant, casterLevelAtTwenty, booksAtTwenty, label, buildResult.Problems);
			}
			buildResult.FeaturesAtEnd = FeatureNames(selfTestUnit.Unit);
			if (ascendant != null)
			{
				try
				{
					ClassData classData = selfTestUnit.Unit.Descriptor.Progression.GetClassData(ascendant);
					if (classData != null)
					{
						buildResult.AscendantArchetypes = classData.Archetypes.ToList();
					}
				}
				catch
				{
				}
			}
		}
		return buildResult;
	}

	private static List<string> FeatureNames(UnitEntityData unit)
	{
		List<string> list = new List<string>();
		try
		{
			foreach (Feature item in unit.Descriptor.Progression.Features.Enumerable)
			{
				string text = item?.Blueprint?.name;
				if (!string.IsNullOrEmpty(text))
				{
					int num = Math.Max(1, item.GetRank());
					for (int i = 0; i < num; i++)
					{
						list.Add(text);
					}
				}
			}
			return list;
		}
		catch
		{
			return null;
		}
	}

	private static void CheckExclusivePowers(UnitEntityData unit, BlueprintArchetype archetype, string label, List<string> problems)
	{
		ArchetypeExclusivePowers.Entry[] table = ArchetypeExclusivePowers.Table;
		foreach (ArchetypeExclusivePowers.Entry entry in table)
		{
			BlueprintFeature blueprintFeature = ArchetypeExclusivePowers.FeatureOf(entry);
			if (blueprintFeature == null)
			{
				problems.Add(label + ": the exclusive Special Power " + entry.Feature + " does not exist.");
				continue;
			}
			List<PrerequisiteArchetypeLevel> list = blueprintFeature.GetComponents<PrerequisiteArchetypeLevel>().ToList();
			if (list.Count == 0)
			{
				problems.Add(label + ": the exclusive Special Power " + entry.Feature + " has no archetype lock, so every Isekai Protagonist can take it.");
				continue;
			}
			bool flag = archetype != null && archetype.name == entry.Archetype;
			bool flag2;
			try
			{
				flag2 = list.Any((PrerequisiteArchetypeLevel p) => p.Check(null, unit.Descriptor, null));
				if (flag)
				{
					flag2 &= blueprintFeature.GetComponents<PrerequisiteCharacterLevel>().All((PrerequisiteCharacterLevel p) => p.Check(null, unit.Descriptor, null));
				}
			}
			catch (Exception ex)
			{
				problems.Add(label + ": checking the prerequisites of " + entry.Feature + " threw " + ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message));
				continue;
			}
			if (flag && !flag2)
			{
				problems.Add(label + ": a level 20 " + entry.Archetype + " is refused its own exclusive Special Power " + entry.Feature + ".");
			}
			if (flag && entry.Archetype == "GodEmperorArchetype")
			{
				bool flag3;
				try
				{
					flag3 = unit.Descriptor.HasFact(blueprintFeature);
				}
				catch
				{
					flag3 = false;
				}
				if (!flag3)
				{
					problems.Add($"{label}: a level 20 God Emperor does not have its exclusive Special Power {entry.Feature}, which the archetype grants at level {entry.Level}.");
				}
			}
			if (!flag & flag2)
			{
				problems.Add(label + ": can take " + entry.Feature + ", which is meant for " + entry.Archetype + " only.");
			}
		}
	}

	private static string CheckAscendedCharacter(UnitEntityData unit, BlueprintCharacterClass isekai, BlueprintCharacterClass ascendant, int casterLevelAtTwenty, string booksAtTwenty, string label, List<string> problems)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		try
		{
			num = unit.Descriptor.Progression.GetClassLevel(isekai);
		}
		catch
		{
		}
		try
		{
			num2 = ((ascendant != null) ? unit.Descriptor.Progression.GetClassLevel(ascendant) : 0);
		}
		catch
		{
		}
		try
		{
			num3 = unit.Descriptor.Spellbooks?.Count() ?? 0;
		}
		catch
		{
		}
		num4 = HighestCasterLevel(unit);
		if (num != 20)
		{
			problems.Add($"{label}: {num} Isekai Protagonist levels at character level 40, expected {20}.");
		}
		if (num2 != 20)
		{
			problems.Add($"{label}: {num2} Isekai Ascendant levels at character level 40, expected {20}.");
		}
		bool flag = casterLevelAtTwenty > 0;
		if (!flag)
		{
			if (num3 > 0)
			{
				problems.Add($"{label}: the character had no spellbook at level 20 but has {num3} at level 40, " + "so the ascension levels created a book rather than advancing one.");
			}
		}
		else
		{
			if (num3 != 1)
			{
				problems.Add($"{label}: {num3} spellbooks at character level 40, expected exactly one. " + "More than one means the ascension levels started a new book instead of advancing the existing one. At level 20 it had " + booksAtTwenty + "; at level 40 it has " + DescribeSpellbooks(unit) + "." + WhoAskedForTheseBooks(unit));
			}
			if (num4 != 40)
			{
				problems.Add($"{label}: caster level {num4} at character level 40 (it was {casterLevelAtTwenty} at level 20), " + $"expected {40}. The ascension levels must advance the book the character already has, " + "through the level 1 replace-spellbook selection.");
			}
		}
		string[] array = new string[2] { "TranscendentProtagonistFeature", "AnimeFinalFormFeature" };
		foreach (string text in array)
		{
			BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, text);
			if (modBlueprint == null)
			{
				problems.Add(label + ": the capstone '" + text + "' does not exist.");
				continue;
			}
			bool flag2;
			try
			{
				flag2 = unit.Descriptor.HasFact(modBlueprint);
			}
			catch
			{
				flag2 = false;
			}
			if (!flag2)
			{
				problems.Add(label + ": the level 40 character does not have the capstone '" + text + "'. IsekaiCapstoneProgression sums the levels of the classes it lists, so the Isekai Ascendant class must be one of them.");
			}
		}
		return $"{label}: Isekai {num} / Ascendant {num2}, {num3} spellbook(s) [{DescribeSpellbooks(unit)}], " + $"caster level {casterLevelAtTwenty} at 20 and {num4} at 40" + (flag ? "." : " (no spellbook: a DisableSpellbook setting removed this archetype's book).");
	}

	internal static string WhoAskedForTheseBooks(UnitEntityData unit)
	{
		try
		{
			Dictionary<BlueprintSpellbook, List<string>> dictionary = new Dictionary<BlueprintSpellbook, List<string>>();
			foreach (Spellbook spellbook in unit.Descriptor.Spellbooks)
			{
				if (spellbook?.Blueprint != null && !dictionary.ContainsKey(spellbook.Blueprint))
				{
					dictionary[spellbook.Blueprint] = new List<string>();
				}
			}
			if (dictionary.Count == 0)
			{
				return string.Empty;
			}
			foreach (EntityFact item2 in unit.Descriptor.Facts.List)
			{
				BlueprintFact blueprintFact = item2?.Blueprint;
				if (((BlueprintScriptableObject)blueprintFact)?.Components == null)
				{
					continue;
				}
				BlueprintComponent[] components = ((BlueprintScriptableObject)blueprintFact).Components;
				foreach (BlueprintComponent blueprintComponent in components)
				{
					if (blueprintComponent == null)
					{
						continue;
					}
					foreach (BlueprintSpellbook item3 in SpellbooksNamedBy(blueprintComponent))
					{
						if (item3 != null && dictionary.TryGetValue(item3, out var value))
						{
							string item = blueprintFact.name + " (" + blueprintComponent.GetType().Name + ")";
							if (!value.Contains(item))
							{
								value.Add(item);
							}
						}
					}
				}
			}
			List<string> list = new List<string>();
			foreach (KeyValuePair<BlueprintSpellbook, List<string>> item4 in dictionary)
			{
				item4.Value.Sort(StringComparer.Ordinal);
				list.Add(item4.Key.name + " <- " + ((item4.Value.Count == 0) ? "no fact names it" : string.Join(", ", item4.Value)));
			}
			list.Sort(StringComparer.Ordinal);
			return " Facts that name each book: " + string.Join("; ", list) + ".";
		}
		catch (Exception ex)
		{
			return " (the facts behind the books could not be read: " + ex.GetType().Name + ")";
		}
	}

	private static IEnumerable<BlueprintSpellbook> SpellbooksNamedBy(BlueprintComponent component)
	{
		List<BlueprintSpellbook> list = new List<BlueprintSpellbook>();
		FieldInfo[] fields = component.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (FieldInfo fieldInfo in fields)
		{
			object value;
			try
			{
				value = fieldInfo.GetValue(component);
			}
			catch
			{
				continue;
			}
			if (value == null)
			{
				continue;
			}
			if (value is BlueprintSpellbookReference blueprintSpellbookReference)
			{
				BlueprintSpellbook blueprintSpellbook = blueprintSpellbookReference.Get();
				if (blueprintSpellbook != null)
				{
					list.Add(blueprintSpellbook);
				}
			}
			else if (value is BlueprintCharacterClassReference blueprintCharacterClassReference)
			{
				BlueprintSpellbook blueprintSpellbook2 = blueprintCharacterClassReference.Get()?.Spellbook;
				if (blueprintSpellbook2 != null)
				{
					list.Add(blueprintSpellbook2);
				}
			}
		}
		return list;
	}

	internal static string DescribeSpellbooks(UnitEntityData unit)
	{
		try
		{
			List<string> list = new List<string>();
			foreach (Spellbook spellbook in unit.Descriptor.Spellbooks)
			{
				if (spellbook != null)
				{
					string arg = ((spellbook.Blueprint != null) ? spellbook.Blueprint.name : "(no blueprint)");
					list.Add($"{arg} (caster level {spellbook.CasterLevel})");
				}
			}
			list.Sort(StringComparer.Ordinal);
			return (list.Count == 0) ? "no spellbook" : string.Join(", ", list);
		}
		catch
		{
			return "(the spellbook list could not be read)";
		}
	}

	internal static int HighestCasterLevel(UnitEntityData unit)
	{
		try
		{
			int num = 0;
			foreach (Spellbook spellbook in unit.Descriptor.Spellbooks)
			{
				if (spellbook != null && spellbook.CasterLevel > num)
				{
					num = spellbook.CasterLevel;
				}
			}
			return num;
		}
		catch
		{
			return 0;
		}
	}

	internal static bool TakeLevel(UnitEntityData unit, BlueprintCharacterClass characterClass, BlueprintArchetype archetype, int level, string label, List<string> problems, HashSet<BlueprintGuid> preferred = null, List<string> selectionNotes = null)
	{
		if (characterClass == null)
		{
			problems.Add($"{label}: character level {level} has no class to take, so the build stops there.");
			return false;
		}
		try
		{
			AddOneLevel(unit, characterClass, archetype, level, label, problems, preferred, selectionNotes);
		}
		catch (Exception ex)
		{
			problems.Add($"{label}: level {level} threw {ex.GetType().Name}: {SelfTestReport.Clean(ex.Message)}");
			return false;
		}
		int num = 0;
		try
		{
			num = unit.Descriptor.Progression.CharacterLevel;
		}
		catch
		{
		}
		if (num < level)
		{
			problems.Add($"{label}: the unit is level {num} after asking for level {level}, so the level-up did not apply.");
			return false;
		}
		return true;
	}

	private static void AddOneLevel(UnitEntityData unit, BlueprintCharacterClass characterClass, BlueprintArchetype archetype, int level, string label, List<string> problems, HashSet<BlueprintGuid> preferred = null, List<string> selectionNotes = null)
	{
		SelfTestUnit.EnsureArmor(unit);
		using (ContextData<AddClassLevels.ExecutionMark>.Request().Setup(unit))
		{
			using (ContextData<AddClassLevels.LevelUpControllerStash>.Request().Stash())
			{
				LevelUpController levelUpController = LevelUpController.Start(unit, LevelUpState.CharBuildMode.LevelUp, instantCommit: true, ignoreSettings: true);
				try
				{
					if (levelUpController.State.CanSelectRace)
					{
						BlueprintRace blueprintRace = unit.Descriptor.Progression.Race;
						if (blueprintRace == null)
						{
							ProgressionRoot progressionRoot = BlueprintRoot.Instance?.Progression;
							if (progressionRoot != null)
							{
								blueprintRace = progressionRoot.CharacterRaces.FirstOrDefault();
							}
						}
						if (blueprintRace != null)
						{
							levelUpController.SelectRace(blueprintRace);
						}
					}
					if (levelUpController.State.CanSelectRaceStat)
					{
						levelUpController.SelectRaceStat(StatType.Constitution);
					}
					if (archetype != null && unit.Descriptor.Progression.GetClassLevel(characterClass) <= 0 && !levelUpController.AddArchetype(characterClass, archetype))
					{
						problems.Add(label + ": the archetype could not be added at level 1.");
					}
					using (ContextData<IgnorePrerequisites>.Request())
					{
						if (!levelUpController.SelectClass(characterClass, applyMechanics: false))
						{
							problems.Add($"{label}: level {level} could not select the class, so the level was never applied.");
							return;
						}
					}
					levelUpController.ApplyClassMechanics();
					PerformSelections(levelUpController, level, label, problems, preferred, selectionNotes);
					levelUpController.ApplySpellbook();
					levelUpController.ApplySkillPoints();
					SpendPoints(levelUpController);
					PerformSelections(levelUpController, level, label, problems, preferred, selectionNotes);
					CheckSpellbookSelectionsWereResolved(levelUpController, level, label, problems);
					try
					{
						unit.Descriptor.Progression.ReapplyFeaturesOnLevelUp();
					}
					catch (Exception ex)
					{
						problems.Add($"{label}: re-applying features at the end of level {level} threw {ex.GetType().Name}: {SelfTestReport.Clean(ex.Message)}");
					}
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

	internal static void FillSelections(LevelUpController controller, int level, string label, List<string> problems)
	{
		PerformSelections(controller, level, label, problems);
	}

	private static void PerformSelections(LevelUpController controller, int level, string label, List<string> problems, HashSet<BlueprintGuid> preferred = null, List<string> selectionNotes = null)
	{
		List<string> list = selectionNotes ?? problems;
		ProgressionRoot progressionRoot = BlueprintRoot.Instance?.Progression;
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < controller.State.Selections.Count; j++)
			{
				FeatureSelectionState selectionState = controller.State.Selections[j];
				if (selectionState.Selected)
				{
					continue;
				}
				IFeatureSelection selection = selectionState.Selection;
				if (selection == null || (i == 0 && !IsSpellbookSelection(selection)))
				{
					continue;
				}
				bool flag = selection == progressionRoot?.BasicFeatSelection || selection == progressionRoot?.DeitySelection;
				IFeatureSelectionItem featureSelectionItem = null;
				try
				{
					List<string> thrownBy = new List<string>();
					List<IFeatureSelectionItem> source = selection.Items.Where(delegate(IFeatureSelectionItem candidate)
					{
						if (candidate == null)
						{
							return false;
						}
						try
						{
							return selection.CanSelect(controller.Preview.Descriptor, controller.State, selectionState, candidate);
						}
						catch (Exception ex3)
						{
							if (thrownBy.Count < 5)
							{
								thrownBy.Add("'" + (candidate.Feature?.name ?? "(no feature)") + "' (" + ex3.GetType().Name + " in " + (SelfTestEngineExceptions.RaisedIn(ex3.StackTrace) ?? "an unknown frame") + ")");
							}
							return false;
						}
					}).ToList();
					if (thrownBy.Count > 0)
					{
						list.Add($"{label}: level {level} selection '{SelectionName(selection)}': asking whether an option can be taken threw for " + string.Join(", ", thrownBy) + "; those options were skipped.");
					}
					featureSelectionItem = ((preferred == null || preferred.Count == 0) ? source.FirstOrDefault() : (source.FirstOrDefault((IFeatureSelectionItem candidate) => candidate.Feature != null && preferred.Contains(candidate.Feature.AssetGuid)) ?? source.FirstOrDefault()));
				}
				catch (Exception ex)
				{
					list.Add($"{label}: level {level} selection '{SelectionName(selection)}' threw {ex.GetType().Name} while looking for an option: {SelfTestReport.Clean(ex.Message)}");
					continue;
				}
				if (featureSelectionItem == null)
				{
					if (!flag)
					{
						list.Add($"{label}: level {level} selection '{SelectionName(selection)}' has no valid option, so the level-up cannot be finished.");
					}
					continue;
				}
				try
				{
					if (!controller.SelectFeature(selectionState, featureSelectionItem))
					{
						list.Add($"{label}: level {level} selection '{SelectionName(selection)}' refused the only option it offered" + " ('" + ((featureSelectionItem.Feature != null) ? featureSelectionItem.Feature.name : "(unnamed)") + "'), so the level-up is incomplete.");
					}
					else if (!selectionState.Selected)
					{
						problems.Add($"{label}: level {level} selection '{SelectionName(selection)}' accepted" + " '" + ((featureSelectionItem.Feature != null) ? featureSelectionItem.Feature.name : "(unnamed)") + "' and then lost it, so it stays unmade. An action that sorts before the one offering its selection is dropped on every redraw, which in character creation is a choice the player can never mark.");
					}
				}
				catch (Exception ex2)
				{
					list.Add($"{label}: level {level} selection '{SelectionName(selection)}' threw {ex2.GetType().Name} while selecting: {SelfTestReport.Clean(ex2.Message)}");
				}
			}
		}
	}

	private static void CheckSpellbookSelectionsWereResolved(LevelUpController controller, int level, string label, List<string> problems)
	{
		for (int i = 0; i < controller.State.Selections.Count; i++)
		{
			FeatureSelectionState featureSelectionState = controller.State.Selections[i];
			IFeatureSelection featureSelection = featureSelectionState?.Selection;
			if (featureSelection != null && !featureSelectionState.Selected)
			{
				FeatureGroup featureGroup;
				try
				{
					featureGroup = featureSelection.GetGroup();
				}
				catch
				{
					continue;
				}
				if (IsSpellbookGroup(featureGroup))
				{
					problems.Add($"{label}: level {level} offered the spellbook selection '{SelectionName(featureSelection)}' and it was never resolved." + " The level still applies, so the character would silently gain no caster level from it.");
				}
			}
		}
	}

	internal static bool IsSpellbookGroup(FeatureGroup group)
	{
		switch (group)
		{
		case FeatureGroup.ThassilonianSpellbook:
		case FeatureGroup.ArcaneTricksterSpellbook:
		case FeatureGroup.EldritchKnightSpellbook:
		case FeatureGroup.DragonDiscipleSpellbook:
		case FeatureGroup.MysticTheurgeArcaneSpellbook:
		case FeatureGroup.SelectMythicSpellbook:
		case FeatureGroup.HellknightSigniferSpellbook:
		case FeatureGroup.ReplaceSpellbook:
			return true;
		default:
			return false;
		}
	}

	private static bool IsSpellbookSelection(IFeatureSelection selection)
	{
		try
		{
			return IsSpellbookGroup(selection.GetGroup());
		}
		catch
		{
			return false;
		}
	}

	private static string SelectionName(IFeatureSelection selection)
	{
		return (selection as BlueprintScriptableObject)?.name ?? selection?.GetType().Name ?? "(unknown)";
	}

	private static void CheckLevelOneFeatures(UnitEntityData unit, BlueprintCharacterClass characterClass, BlueprintArchetype archetype, string label, List<string> problems)
	{
		List<BlueprintFeatureBase> list = new List<BlueprintFeatureBase>();
		LevelEntry[] array = characterClass.Progression?.LevelEntries ?? new LevelEntry[0];
		foreach (LevelEntry levelEntry in array)
		{
			if (levelEntry != null && levelEntry.Level == 1)
			{
				list.AddRange(levelEntry.Features);
			}
		}
		HashSet<BlueprintGuid> hashSet = new HashSet<BlueprintGuid>();
		if (archetype != null)
		{
			array = archetype.RemoveFeatures ?? new LevelEntry[0];
			foreach (LevelEntry levelEntry2 in array)
			{
				if (levelEntry2 == null || levelEntry2.Level != 1)
				{
					continue;
				}
				foreach (BlueprintFeatureBase feature in levelEntry2.Features)
				{
					if (feature != null)
					{
						hashSet.Add(feature.AssetGuid);
					}
				}
			}
			array = archetype.AddFeatures ?? new LevelEntry[0];
			foreach (LevelEntry levelEntry3 in array)
			{
				if (levelEntry3 != null && levelEntry3.Level == 1)
				{
					list.AddRange(levelEntry3.Features);
				}
			}
		}
		foreach (BlueprintFeatureBase item in list.Where((BlueprintFeatureBase f) => f != null).Distinct())
		{
			if (!hashSet.Contains(item.AssetGuid) && !(item is IFeatureSelection) && !(item is BlueprintFeatureSelection) && item is BlueprintFeature blueprintFeature)
			{
				bool flag;
				try
				{
					flag = unit.Descriptor.HasFact(blueprintFeature);
				}
				catch (Exception ex)
				{
					problems.Add(label + ": asking whether the unit has the level 1 feature '" + blueprintFeature.name + "' threw " + ex.GetType().Name + ".");
					continue;
				}
				if (!flag)
				{
					problems.Add(label + ": the progression promises '" + blueprintFeature.name + "' at level 1 but the levelled unit does not have it.");
				}
			}
		}
	}

	private static void CheckCantripsAreListed(UnitEntityData unit, HashSet<BlueprintGuid> modIds, string label, List<string> problems)
	{
		List<Spellbook> list;
		List<Ability> list2;
		try
		{
			list = unit.Descriptor.Spellbooks.ToList();
			list2 = (unit.Descriptor.Abilities.RawFacts ?? new List<Ability>()).ToList();
		}
		catch (Exception ex)
		{
			problems.Add(label + ": reading the unit's spellbooks and abilities threw " + ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message));
			return;
		}
		if (list.Count == 0)
		{
			return;
		}
		foreach (Ability item in list2)
		{
			BlueprintAbility blueprint = item?.Blueprint;
			if (blueprint == null || !blueprint.IsCantrip || item.SourceItem != null || modIds == null || !modIds.Contains(blueprint.AssetGuid))
			{
				continue;
			}
			bool flag;
			try
			{
				flag = list.Any((Spellbook b) => b.IsKnownOnLevel(blueprint, 0));
			}
			catch (Exception ex2)
			{
				problems.Add(label + ": asking the spellbooks about the cantrip '" + blueprint.name + "' threw " + ex2.GetType().Name + ".");
				continue;
			}
			if (!flag)
			{
				problems.Add(label + ": the cantrip '" + blueprint.name + "' is a fact on the level 1 unit but no spellbook knows it (books: " + DescribeSpellbooks(unit) + "). The game lists cantrip facts in no ability list and no spellbook, so once its hotbar slot is cleared it cannot be put back. Teach it to the class book (LearnSignatureCantrip).");
			}
		}
	}

	internal static SelfTestChecks.Outcome FactActivation()
	{
		List<string> list = new List<string>();
		List<BlueprintBuff> list2 = ModBlueprintsOf<BlueprintBuff>().ToList();
		List<BlueprintAbility> list3 = ModBlueprintsOf<BlueprintAbility>().ToList();
		List<BlueprintFeature> list4 = (from f in ModBlueprintsOf<BlueprintFeature>()
			where !(f is BlueprintFeatureSelection)
			select f).ToList();
		string problem;
		using SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem);
		if (selfTestUnit == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "No blank unit could be created, so no fact could be activated: " + problem + ".");
		}
		HashSet<EntityFact> hashSet = new HashSet<EntityFact>();
		try
		{
			foreach (EntityFact item in selfTestUnit.Unit.Descriptor.Facts.List)
			{
				hashSet.Add(item);
			}
		}
		catch
		{
		}
		int added = 0;
		int ticked = 0;
		int num = 0;
		List<string> list5 = new List<string>();
		foreach (BlueprintBuff item2 in list2)
		{
			using SelfTestEngineLog.Capture capture = SelfTestEngineLog.Open("fact-activation " + item2.name);
			num += RunOneBuff(selfTestUnit.Unit, item2, list, ref added, ref ticked);
			CollectLeftovers(selfTestUnit.Unit, hashSet, item2.name, list5);
			num += ClaimEngineExceptions(capture, item2.name, list);
		}
		foreach (BlueprintUnitFact item3 in list4.Cast<BlueprintUnitFact>().Concat(list3))
		{
			using SelfTestEngineLog.Capture capture2 = SelfTestEngineLog.Open("fact-activation " + item3.name);
			num += RunOneFact(selfTestUnit.Unit, item3, list, ref added);
			CollectLeftovers(selfTestUnit.Unit, hashSet, item3.name, list5);
			num += ClaimEngineExceptions(capture2, item3.name, list);
		}
		if (list5.Count > 0)
		{
			num++;
			list.Add($"{list5.Count} facts outlived the mod fact that put them on the unit; a component added them to the unit itself and nothing takes them off again:");
			list.AddRange(list5.OrderBy((string n) => n, StringComparer.Ordinal));
		}
		string summary = $"{list2.Count} buffs, {list4.Count} features and {list3.Count} abilities applied and removed on a blank unit" + $" ({added} became active, {ticked} buffs ticked); {num} problems" + string.Format(", {0} allow-listed known issues.", list.Count((string d) => d.StartsWith("KNOWN ISSUE ", StringComparison.Ordinal)));
		return SelfTestChecks.Outcome.Of(num == 0, summary, list);
	}

	private static int ClaimEngineExceptions(SelfTestEngineLog.Capture capture, string factName, List<string> details)
	{
		int num = 0;
		foreach (IGrouping<string, SelfTestEngineLog.Caught> item in (from c in capture.Items
			where c.ClaimedBy == null
			group c by c.Signature).ToList())
		{
			SelfTestEngineLog.Caught caught = item.First();
			foreach (SelfTestEngineLog.Caught item2 in item)
			{
				item2.ClaimedBy = "fact-activation " + factName;
			}
			if (!item.Any(SelfTestEngineLog.IsOurs))
			{
				details.Add($"{factName}: not counted, the game's own: the engine caught {caught.Describe()} {item.Count()} time(s){SelfTestEngineLog.SourcesOf(item)}.");
				continue;
			}
			EngineExceptionRules.Known known = SelfTestEngineLog.MatchKnown(caught);
			string text = $"{factName}: while it was added, ticked and removed, the engine caught {caught.Describe()} {item.Count()} time(s)" + SelfTestEngineLog.SourcesOf(item);
			if (known != null)
			{
				details.Add(EngineExceptionRules.KnownIssueLine(known, text));
				continue;
			}
			num++;
			details.Add(text + ". The engine swallowed it, so the fact looks applied while the component that threw did nothing.");
		}
		return num;
	}

	private static void CollectLeftovers(UnitEntityData unit, HashSet<EntityFact> known, string source, List<string> leftOver)
	{
		try
		{
			foreach (EntityFact item in unit.Descriptor.Facts.List.ToList())
			{
				if (known.Add(item) && !(item is Buff { IsPermanent: false }))
				{
					leftOver.Add("'" + (item.Blueprint?.name ?? "(unnamed)") + "' was left on the unit by '" + source + "'.");
				}
			}
		}
		catch
		{
		}
	}

	private static int RunOneBuff(UnitEntityData unit, BlueprintBuff buff, List<string> details, ref int added, ref int ticked)
	{
		Buff buff2 = null;
		try
		{
			buff2 = unit.Descriptor.Buffs.AddBuff(buff, unit, null);
		}
		catch (Exception ex)
		{
			details.Add(buff.name + ": applying the buff threw " + ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message));
			return 1;
		}
		if (buff2 == null)
		{
			return 0;
		}
		int num = 0;
		if (buff2.Active)
		{
			added++;
		}
		try
		{
			buff2.TickMechanics();
			ticked++;
		}
		catch (Exception ex2)
		{
			num++;
			details.Add(buff.name + ": ticking the buff threw " + ex2.GetType().Name + ": " + SelfTestReport.Clean(ex2.Message));
		}
		return num + RemoveAndVerify(unit, buff, buff2, details);
	}

	private static int RunOneFact(UnitEntityData unit, BlueprintUnitFact blueprint, List<string> details, ref int added)
	{
		EntityFact entityFact;
		try
		{
			entityFact = unit.Descriptor.AddFact(blueprint);
		}
		catch (Exception ex)
		{
			details.Add(blueprint.name + ": adding the fact threw " + ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message));
			return 1;
		}
		if (entityFact == null)
		{
			return 0;
		}
		if (entityFact.Active)
		{
			added++;
		}
		return RemoveAndVerify(unit, blueprint, entityFact, details);
	}

	private static int RemoveAndVerify(UnitEntityData unit, BlueprintUnitFact blueprint, EntityFact fact, List<string> details)
	{
		try
		{
			unit.Descriptor.RemoveFact(fact);
		}
		catch (Exception ex)
		{
			details.Add(blueprint.name + ": removing the fact threw " + ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message));
			return 1;
		}
		int num = 0;
		try
		{
			if (unit.Descriptor.HasFact(blueprint))
			{
				num++;
				details.Add(blueprint.name + ": the unit still has the fact after it was removed.");
			}
		}
		catch (Exception ex2)
		{
			num++;
			details.Add(blueprint.name + ": checking the removal threw " + ex2.GetType().Name + ": " + SelfTestReport.Clean(ex2.Message));
		}
		try
		{
			List<EntityFactComponent> list = fact.Components.Where((EntityFactComponent c) => c != null && !c.IsDisposed).ToList();
			if (list.Count > 0)
			{
				num++;
				details.Add($"{blueprint.name}: {list.Count} runtime components were left behind after the fact was removed" + " (" + string.Join(", ", from c in list.Take(5)
					select c.SourceBlueprintComponent?.GetType().Name ?? c.GetType().Name) + ").");
			}
		}
		catch (Exception ex3)
		{
			num++;
			details.Add(blueprint.name + ": inspecting the removed fact's components threw " + ex3.GetType().Name + ": " + SelfTestReport.Clean(ex3.Message));
		}
		return num;
	}

	internal static SelfTestChecks.Outcome AbilityParameters()
	{
		List<string> details = new List<string>();
		List<BlueprintAbility> list = ModBlueprintsOf<BlueprintAbility>().ToList();
		if (list.Count == 0)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The mod created no abilities, which cannot be right.");
		}
		string problem;
		using SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem);
		if (selfTestUnit == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "No blank unit could be created, so no ability could be measured: " + problem + ".");
		}
		string text = LevelForAbilities(selfTestUnit.Unit);
		int measured = 0;
		int num = 0;
		foreach (BlueprintAbility item in list)
		{
			num += MeasureOne(selfTestUnit.Unit, item, details, ref measured);
		}
		string summary = $"{measured} of {list.Count} mod abilities measured on a test unit ({text}); {num} problems.";
		return SelfTestChecks.Outcome.Of(num == 0, summary, details);
	}

	private static string LevelForAbilities(UnitEntityData unit)
	{
		BlueprintCharacterClass blueprintCharacterClass = IsekaiProtagonistClass.Get();
		if (blueprintCharacterClass == null)
		{
			return "the Isekai class does not exist, so the unit has no levels";
		}
		try
		{
			for (int i = 1; i <= 10; i++)
			{
				AddOneLevel(unit, blueprintCharacterClass, null, i, "ability test unit", new List<string>());
			}
			return $"Isekai Protagonist {unit.Descriptor.Progression.CharacterLevel}";
		}
		catch (Exception ex)
		{
			return $"the unit could not be levelled ({ex.GetType().Name}), so it is level {unit.Descriptor.Progression.CharacterLevel}";
		}
	}

	private static int MeasureOne(UnitEntityData unit, BlueprintAbility ability, List<string> details, ref int measured)
	{
		AbilityData abilityData;
		try
		{
			abilityData = new AbilityData(ability, unit.Descriptor);
		}
		catch (Exception ex)
		{
			details.Add(ability.name + ": building AbilityData threw " + ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message));
			return 1;
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		try
		{
			AbilityParams abilityParams = abilityData.CalculateParams();
			num2 = abilityParams.DC;
			num3 = abilityParams.CasterLevel;
		}
		catch (Exception ex2)
		{
			details.Add(ability.name + ": calculating the parameters threw " + ex2.GetType().Name + ": " + SelfTestReport.Clean(ex2.Message));
			return 1;
		}
		try
		{
			AbilityRange range = abilityData.Range;
			if (!Enum.IsDefined(typeof(AbilityRange), range))
			{
				num++;
				details.Add($"{ability.name}: its range is {(int)range}, which is not an AbilityRange the game knows.");
			}
		}
		catch (Exception ex3)
		{
			num++;
			details.Add(ability.name + ": reading the range threw " + ex3.GetType().Name + ": " + SelfTestReport.Clean(ex3.Message));
		}
		try
		{
			if (abilityData.ResourceLogic != null)
			{
				if (abilityData.Resource == null)
				{
					num++;
					details.Add(ability.name + ": the ability spends a resource but names none, so it costs nothing and can be used forever.");
				}
				else
				{
					int resourceCost = abilityData.ResourceCost;
					if (resourceCost < 0)
					{
						num++;
						details.Add($"{ability.name}: the resource cost is {resourceCost}.");
					}
				}
			}
		}
		catch (Exception ex4)
		{
			num++;
			details.Add(ability.name + ": reading the resource cost threw " + ex4.GetType().Name + ": " + SelfTestReport.Clean(ex4.Message));
		}
		string text = SelfTestRuntimeRules.DcProblem(num2);
		if (text != null)
		{
			num++;
			details.Add(ability.name + ": " + text + ".");
		}
		string text2 = SelfTestRuntimeRules.CasterLevelProblem(num3);
		if (text2 != null)
		{
			num++;
			details.Add(ability.name + ": " + text2 + ".");
		}
		measured++;
		return num;
	}
}
