using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using IsekaiMod.Content.Arenas;
using IsekaiMod.Content.Guardians;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.Localization.Shared;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.View;
using Kingmaker.Visual.CharacterSystem;
using Newtonsoft.Json.Linq;
using TabletopTweaks.Core.Localization;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestChecks
{
	internal sealed class Check
	{
		public string Id;

		public Func<Outcome> Run;
	}

	internal sealed class Outcome
	{
		public bool Passed;

		public string Summary;

		public List<string> Details = new List<string>();

		public static Outcome Of(bool passed, string summary, IEnumerable<string> details = null)
		{
			return new Outcome
			{
				Passed = passed,
				Summary = summary,
				Details = (details?.ToList() ?? new List<string>())
			};
		}
	}

	private sealed class UnitEvaluation
	{
		public int Evaluated;

		public int Structural;

		public string Note = "";

		public List<string> Failures = new List<string>();
	}

	private struct ProbeResult
	{
		public bool Drew;

		public bool EngineDrew;

		public string Failure;
	}

	private sealed class ReachedBuff
	{
		public BlueprintBuff Buff;

		public string Path;

		public int Depth;

		public bool ViaAddFacts;
	}

	private sealed class SizeSource
	{
		public string Source;

		public Size Size;
	}

	private static readonly HashSet<string> RequiredReferenceComponents = new HashSet<string>(StringComparer.Ordinal)
	{
		"PrerequisiteFeature", "PrerequisiteNoFeature", "PrerequisiteArchetypeLevel", "PrerequisiteNoArchetype", "PrerequisiteClassLevel", "PrerequisiteParametrizedFeature", "AddFeatureIfHasFact", "AddFeatureOnClassLevel", "AddFeatureOnApply", "AddProgressionOnApply",
		"ContextActionApplyBuff", "ContextActionRemoveBuff"
	};

	private static readonly Regex[] VanillaEmptyPositions = new Regex[2]
	{
		new Regex("^Body\\.m_QuickSlots\\[\\d+\\]$", RegexOptions.CultureInvariant),
		new Regex("^Components\\[\\d+\\]<SpontaneousSpellConversion>\\.m_SpellsByLevel\\[0\\]$", RegexOptions.CultureInvariant)
	};

	private static readonly string[] FormBuffNames = new string[2] { "SlimeFormBuff", "SkeletalOverlordFormBuff" };

	private const string SlimeFormBuffName = "SlimeFormBuff";

	private static readonly Regex BareNull = new Regex("(?<![\\w>])null(?![\\w<])", RegexOptions.CultureInvariant);

	private static readonly Regex UnformattedArgument = new Regex("\\{[0-9]\\}", RegexOptions.CultureInvariant);

	internal static IEnumerable<Check> All()
	{
		yield return new Check
		{
			Id = "blueprint-registration",
			Run = BlueprintRegistration
		};
		yield return new Check
		{
			Id = "blueprint-references",
			Run = BlueprintReferences
		};
		yield return new Check
		{
			Id = "exclusivity",
			Run = Exclusivity
		};
		yield return new Check
		{
			Id = "human-heritage-default",
			Run = SelfTestHeritageChecks.HumanHeritageDefault
		};
		yield return new Check
		{
			Id = "form-models",
			Run = FormModels
		};
		yield return new Check
		{
			Id = "unit-models",
			Run = UnitModels
		};
		yield return new Check
		{
			Id = "unit-spawnable",
			Run = UnitsCanSpawn
		};
		yield return new Check
		{
			Id = "service-npc-protection",
			Run = ServiceNpcProtection
		};
		yield return new Check
		{
			Id = "companion-character-sheet",
			Run = SelfTestCompanionChecks.CompanionCharacterSheets
		};
		yield return new Check
		{
			Id = "granted-items-removable",
			Run = SelfTestCompanionChecks.GrantedItemsAreRemovable
		};
		yield return new Check
		{
			Id = "granted-items-render",
			Run = SelfTestCompanionChecks.GrantedItemsRender
		};
		yield return new Check
		{
			Id = "localization",
			Run = Localization
		};
		yield return new Check
		{
			Id = "blueprint-text",
			Run = BlueprintText
		};
		yield return new Check
		{
			Id = "dlc-known-spells",
			Run = SelfTestDlcChecks.KnownSpells
		};
		yield return new Check
		{
			Id = "level-up-simulation",
			Run = SelfTestProgressionChecks.LevelUpSimulation
		};
		yield return new Check
		{
			Id = "chargen-legacy-pages",
			Run = SelfTestChargenChecks.ChargenLegacyPages
		};
		yield return new Check
		{
			Id = "dual-legacy-level-up",
			Run = SelfTestLegacyWindowChecks.DualLegacyLevelUp
		};
		yield return new Check
		{
			Id = "twin-summoner-level-one",
			Run = SelfTestLegacyWindowChecks.TwinSummonerLevelOne
		};
		yield return new Check
		{
			Id = "level-up-window",
			Run = SelfTestWindowLevelUpChecks.LevelUpWindow
		};
		yield return new Check
		{
			Id = "pet-level-up",
			Run = SelfTestPetChecks.PetLevelUp
		};
		yield return new Check
		{
			Id = "pet-bodies",
			Run = SelfTestBodyChecks.PetBodies
		};
		yield return new Check
		{
			Id = "fact-activation",
			Run = SelfTestProgressionChecks.FactActivation
		};
		yield return new Check
		{
			Id = "threat-hit-points",
			Run = SelfTestBodyChecks.ThreatHitPoints
		};
		yield return new Check
		{
			Id = "polymorph-clash",
			Run = SelfTestBodyChecks.PolymorphClash
		};
		yield return new Check
		{
			Id = "ability-parameters",
			Run = SelfTestProgressionChecks.AbilityParameters
		};
		yield return new Check
		{
			Id = "kept-rewards",
			Run = SelfTestKeptRewardsChecks.RewardsSurvive
		};
		yield return new Check
		{
			Id = "stacking-bonuses",
			Run = SelfTestKeptRewardsChecks.StackingBonuses
		};
		yield return new Check
		{
			Id = "guarded-components",
			Run = SelfTestSpellbookChecks.GuardedComponents
		};
		yield return new Check
		{
			Id = "legacy-spellbooks",
			Run = SelfTestSpellbookChecks.LegacySpellbooks
		};
		yield return new Check
		{
			Id = "multiclass-spellbooks",
			Run = SelfTestSpellbookChecks.MulticlassSpellbooks
		};
		yield return new Check
		{
			Id = "respec-rebuild",
			Run = SelfTestSpellbookChecks.RespecRebuild
		};
		yield return new Check
		{
			Id = "prestige-levels",
			Run = SelfTestPrestigeChecks.PrestigeLevels
		};
		yield return new Check
		{
			Id = "dialogue-graph",
			Run = SelfTestContentChecks.DialogueGraph
		};
		yield return new Check
		{
			Id = "dialogue-showchecks",
			Run = SelfTestDialogueShowChecks.Run
		};
		yield return new Check
		{
			Id = "quests-and-etudes",
			Run = SelfTestContentChecks.QuestsAndEtudes
		};
		yield return new Check
		{
			Id = "store-and-vendors",
			Run = SelfTestContentChecks.StoreAndVendors
		};
		yield return new Check
		{
			Id = "teleport-delivery",
			Run = SelfTestContentChecks.TeleportDelivery
		};
		yield return new Check
		{
			Id = "engine-exceptions",
			Run = EngineExceptions
		};
		yield return new Check
		{
			Id = "startup-exceptions",
			Run = StartupExceptions
		};
	}

	internal static void RunOne(SelfTestReport report, Check check)
	{
		Outcome outcome = null;
		Exception ex = null;
		SelfTestEngineLog.Capture capture = SelfTestEngineLog.Open(check.Id);
		try
		{
			outcome = check.Run();
		}
		catch (Exception ex2)
		{
			ex = ex2;
		}
		finally
		{
			capture.Dispose();
		}
		List<string> list = SelfTestEngineLog.Judge(capture.Items, out var unexplained, out var total);
		if (ex != null)
		{
			IEnumerable<string> first = (from l in (ex.StackTrace ?? "").Split('\n')
				select l.Trim() into l
				where l.Length > 0
				select l).Take(8);
			report.Result(check.Id, success: false, "The check itself threw " + ex.GetType().Name + ": " + ex.Message, first.Concat(list));
		}
		else
		{
			bool success = outcome.Passed && unexplained == 0;
			string text = outcome.Summary;
			if (total > 0)
			{
				text += $" The engine caught {total} exception(s) while it ran ({unexplained} kind(s) not allow-listed).";
			}
			report.Result(check.Id, success, text, list.Concat(outcome.Details));
		}
	}

	private static Dictionary<BlueprintGuid, SelfTestRecorder.CreatedBlueprint> ModBlueprintsById()
	{
		Dictionary<BlueprintGuid, SelfTestRecorder.CreatedBlueprint> dictionary = new Dictionary<BlueprintGuid, SelfTestRecorder.CreatedBlueprint>();
		foreach (SelfTestRecorder.CreatedBlueprint item in SelfTestRecorder.CreatedBlueprints())
		{
			if (item.Blueprint != null)
			{
				dictionary[item.Guid] = item;
			}
		}
		return dictionary;
	}

	private static string ReadEmbeddedResource(string suffix)
	{
		Assembly assembly = typeof(SelfTestChecks).Assembly;
		string text = assembly.GetManifestResourceNames().FirstOrDefault((string n) => n.EndsWith(suffix, StringComparison.OrdinalIgnoreCase));
		if (text == null)
		{
			return null;
		}
		using Stream stream = assembly.GetManifestResourceStream(text);
		using StreamReader streamReader = new StreamReader(stream);
		return streamReader.ReadToEnd();
	}

	private static string Describe(BlueprintGuid guid)
	{
		return guid.ToString();
	}

	private static Outcome BlueprintRegistration()
	{
		List<SelfTestRecorder.CreatedBlueprint> list = SelfTestRecorder.CreatedBlueprints();
		if (list.Count == 0)
		{
			return Outcome.Of(passed: false, "No blueprint registration was recorded; the recorder patch did not run, so nothing below can be trusted.");
		}
		string text = ReadEmbeddedResource(".Blueprints.json");
		if (text == null)
		{
			return Outcome.Of(passed: false, "Blueprints.json is not embedded in IsekaiMod.dll.");
		}
		JObject jObject = JObject.Parse(text);
		Dictionary<string, Guid> dictionary = new Dictionary<string, Guid>(StringComparer.Ordinal);
		string[] array = new string[2] { "NewBlueprints", "DerivedBlueprints" };
		foreach (string propertyName in array)
		{
			if (!(jObject[propertyName] is JObject jObject2))
			{
				continue;
			}
			foreach (JProperty item in jObject2.Properties())
			{
				if (Guid.TryParse(item.Value?.ToString(), out var result))
				{
					dictionary[item.Name] = result;
				}
			}
		}
		List<string> list2 = new List<string>();
		int num = 0;
		int num2 = 0;
		foreach (SelfTestRecorder.CreatedBlueprint item2 in (from c in list
			group c by c.Name into g
			select g.First()).OrderBy((SelfTestRecorder.CreatedBlueprint c) => c.Name, StringComparer.Ordinal))
		{
			if (!dictionary.TryGetValue(item2.Name ?? "", out var value))
			{
				num++;
				list2.Add("'" + item2.Name + "' is not in Config/Blueprints.json (runtime id " + Describe(item2.Guid) + ").");
			}
			else if (new BlueprintGuid(value) != item2.Guid)
			{
				num2++;
				list2.Add($"'{item2.Name}' is registered as {value:N} but was created with {Describe(item2.Guid)}.");
			}
		}
		List<string> list3 = (from c in list
			group c by c.Name into g
			where g.Count() > 1
			select g.Key).OrderBy((string n) => n, StringComparer.Ordinal).ToList();
		foreach (string item3 in list3)
		{
			list2.Add("'" + item3 + "' was created more than once.");
		}
		List<string> list4 = SelfTestRecorder.IdCollisions();
		list2.AddRange(list4);
		int num3 = num + num2 + list3.Count + list4.Count;
		string summary = $"{list.Count} blueprints registered; {num} missing from Blueprints.json, {num2} with a different id, {list3.Count} created twice, {list4.Count} id collisions.";
		return Outcome.Of(num3 == 0, summary, list2);
	}

	private static bool IsVanillaEmptyPosition(BlueprintReferenceWalker.Found item)
	{
		if (item.Reference == null || item.Reference.Guid == BlueprintGuid.Empty)
		{
			return VanillaEmptyPositions.Any((Regex r) => r.IsMatch(item.Path ?? ""));
		}
		return false;
	}

	private static Outcome BlueprintReferences()
	{
		Dictionary<BlueprintGuid, SelfTestRecorder.CreatedBlueprint> dictionary = ModBlueprintsById();
		if (dictionary.Count == 0)
		{
			return Outcome.Of(passed: false, "No mod blueprints were recorded, so no reference could be checked.");
		}
		List<SelfTestReport.ReferenceProblem> list = new List<SelfTestReport.ReferenceProblem>();
		List<string> list2 = new List<string>();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		Dictionary<BlueprintGuid, SimpleBlueprint> cache = new Dictionary<BlueprintGuid, SimpleBlueprint>();
		foreach (SelfTestRecorder.CreatedBlueprint item in dictionary.Values.OrderBy((SelfTestRecorder.CreatedBlueprint c) => c.Name, StringComparer.Ordinal))
		{
			List<BlueprintReferenceWalker.Found> list3;
			try
			{
				list3 = BlueprintReferenceWalker.ReferencesIn(item.Blueprint);
			}
			catch (Exception ex)
			{
				num2++;
				list2.Add(item.Name + ": could not be walked (" + ex.GetType().Name + ": " + ex.Message + ").");
				continue;
			}
			foreach (BlueprintReferenceWalker.Found item2 in list3)
			{
				num++;
				if (IsVanillaEmptyPosition(item2))
				{
					num3++;
					continue;
				}
				string text = Classify(item2, cache);
				if (text != null)
				{
					list.Add(new SelfTestReport.ReferenceProblem(item.Name, item2.Path, text));
				}
			}
		}
		list2.AddRange(SelfTestReport.GroupReferenceProblems(list));
		int num4 = list.Count + num2;
		int num5 = list.Select((SelfTestReport.ReferenceProblem p) => p.Blueprint + "|" + SelfTestReport.FieldPattern(p.Path)).Distinct().Count();
		string summary = $"{num} references in {dictionary.Count} mod blueprints; {num4} do not resolve to a live blueprint of the right type" + $" ({num5} blueprint fields); {num3} entries allowed empty where vanilla leaves them empty (unit quick slots, level 0 spell conversion).";
		return Outcome.Of(num4 == 0, summary, list2);
	}

	private static string Classify(BlueprintReferenceWalker.Found item, Dictionary<BlueprintGuid, SimpleBlueprint> cache)
	{
		string text = item.Owner?.GetType().Name ?? "";
		bool flag = item.InCollection || RequiredReferenceComponents.Contains(text);
		if (item.Reference == null)
		{
			if (!flag)
			{
				return null;
			}
			return "null entry";
		}
		BlueprintGuid guid = item.Reference.Guid;
		if (guid == BlueprintGuid.Empty)
		{
			if (!flag)
			{
				return null;
			}
			if (!item.InCollection)
			{
				return "empty reference in " + text + ", which cannot work without it";
			}
			return "empty entry; the blueprint did not exist when the list was built";
		}
		if (!cache.TryGetValue(guid, out var value))
		{
			try
			{
				value = ResourcesLibrary.TryGetBlueprint(guid);
			}
			catch
			{
				value = null;
			}
			cache[guid] = value;
		}
		if (value == null)
		{
			return $"{guid} does not resolve to any blueprint";
		}
		Type type = BlueprintReferenceWalker.TargetType(item.DeclaredType);
		if (!type.IsInstanceOfType(value))
		{
			return $"{guid} is '{value.name}', a {value.GetType().Name}, but the reference needs a {type.Name}";
		}
		return null;
	}

	private static Outcome Exclusivity()
	{
		Dictionary<BlueprintGuid, SelfTestRecorder.CreatedBlueprint> dictionary = ModBlueprintsById();
		Dictionary<string, SimpleBlueprint> byName = new Dictionary<string, SimpleBlueprint>(StringComparer.Ordinal);
		foreach (SelfTestRecorder.CreatedBlueprint value4 in dictionary.Values)
		{
			byName[value4.Name] = value4.Blueprint;
		}
		Dictionary<string, string> category = new Dictionary<string, string>(StringComparer.Ordinal);
		foreach (SelfTestRecorder.CreatedBlueprint value5 in dictionary.Values)
		{
			if (value5.Blueprint is BlueprintArchetype)
			{
				category[value5.Name] = "archetype";
			}
			else if (value5.Blueprint is BlueprintFeature blueprintFeature && !(blueprintFeature is BlueprintFeatureSelection) && blueprintFeature.Groups != null && blueprintFeature.Groups.Contains(FeatureGroup.Racial))
			{
				category[value5.Name] = "heritage";
			}
		}
		foreach (SelfTestRecorder.CreatedBlueprint value6 in dictionary.Values)
		{
			if (!(value6.Blueprint is BlueprintFeatureSelection { m_AllFeatures: not null } blueprintFeatureSelection))
			{
				continue;
			}
			string text = ExclusivityRules.SelectionCategory(value6.Name);
			if (text == null)
			{
				continue;
			}
			BlueprintFeatureReference[] allFeatures = blueprintFeatureSelection.m_AllFeatures;
			foreach (BlueprintFeatureReference blueprintFeatureReference in allFeatures)
			{
				if (blueprintFeatureReference != null && dictionary.TryGetValue(blueprintFeatureReference.Guid, out var value) && !category.ContainsKey(value.Name))
				{
					category[value.Name] = text;
				}
			}
		}
		List<string[]> list = new List<string[]>();
		Dictionary<string, List<Prerequisite>> dictionary2 = new Dictionary<string, List<Prerequisite>>(StringComparer.Ordinal);
		List<string> list2 = new List<string>();
		foreach (SelfTestRecorder.CreatedBlueprint value7 in dictionary.Values)
		{
			if (!(value7.Blueprint is BlueprintScriptableObject { ComponentsArray: not null, ComponentsArray: var componentsArray }))
			{
				continue;
			}
			foreach (BlueprintComponent blueprintComponent in componentsArray)
			{
				if (!(blueprintComponent is Prerequisite prerequisite) || !blueprintComponent.GetType().Name.StartsWith("PrerequisiteNo", StringComparison.Ordinal))
				{
					continue;
				}
				foreach (BlueprintReferenceWalker.Found item in BlueprintReferenceWalker.ReferencesInObject(blueprintComponent))
				{
					if (item.Reference != null && !(item.Reference.Guid == BlueprintGuid.Empty) && dictionary.TryGetValue(item.Reference.Guid, out var value2) && (value2.Blueprint is BlueprintFeature || value2.Blueprint is BlueprintArchetype))
					{
						list.Add(new string[2] { value7.Name, value2.Name });
						string key = value7.Name + "\u0001" + value2.Name;
						if (!dictionary2.TryGetValue(key, out var value3))
						{
							value3 = (dictionary2[key] = new List<Prerequisite>());
						}
						value3.Add(prerequisite);
						if (prerequisite.Group == Prerequisite.GroupType.Any && category.ContainsKey(value7.Name))
						{
							list2.Add(value7.Name + " excludes " + value2.Name + " only inside an 'Any' prerequisite group, so another member of the group can satisfy it.");
						}
					}
				}
			}
		}
		int count = list2.Count;
		List<string[]> list4 = ExclusivityRules.RequiredPairs.Where((string[] p) => byName.ContainsKey(p[0]) && byName.ContainsKey(p[1])).ToList();
		List<string> list5 = new List<string>();
		int num = 0;
		foreach (string[] item2 in ExclusivityRules.RequiredPairs.Where((string[] p) => !byName.ContainsKey(p[0]) || !byName.ContainsKey(p[1])))
		{
			string text2 = ExclusivityRules.DlcSkipReason(item2, byName.ContainsKey, IsekaiDlcManager.IsDlcAvailable);
			if (text2 != null)
			{
				list5.Add("Skipped, not a failure: required pair " + item2[0] + " / " + item2[1] + ": " + text2 + ".");
			}
			else
			{
				num++;
				list2.Add("Required pair " + item2[0] + " / " + item2[1] + " cannot be checked: " + (byName.ContainsKey(item2[0]) ? item2[1] : item2[0]) + " was not created.");
			}
		}
		List<string[]> missingRequired = ExclusivityRules.MissingRequiredLocks(list, list4);
		foreach (string[] miss in missingRequired)
		{
			string text3 = list4.First((string[] p) => (p[0] == miss[0] && p[1] == miss[1]) || (p[0] == miss[1] && p[1] == miss[0]))[2];
			list2.Add(miss[0] + " does not exclude " + miss[1] + " (" + text3 + ").");
		}
		List<string[]> list6 = (from e in ExclusivityRules.OneSidedLocks(list, category.Keys.ToList())
			where !missingRequired.Any((string[] m) => m[0] == e[1] && m[1] == e[0])
			select e).ToList();
		foreach (string[] item3 in list6)
		{
			list2.Add(item3[0] + " excludes " + item3[1] + " but " + item3[1] + " does not exclude " + item3[0] + ", so taking " + item3[1] + " first still allows " + item3[0] + ".");
		}
		UnitEvaluation unitEvaluation = EvaluateOnUnit(list, dictionary2, byName, category);
		list2.AddRange(unitEvaluation.Failures);
		list2.AddRange(list5);
		int num2 = count + num + missingRequired.Count + list6.Count + unitEvaluation.Failures.Count;
		string summary = $"{category.Count} archetypes, heritages, pets and legacies; {list.Count((string[] e) => category.ContainsKey(e[0]) && category.ContainsKey(e[1]))} exclusions between them. " + $"{missingRequired.Count} required locks missing, {list6.Count} one-sided, {count} in an Any group, {num} required pairs not created, {list5.Count} skipped for a DLC that is not available. " + $"On a blank unit: {unitEvaluation.Evaluated} exclusions evaluated, {unitEvaluation.Failures.Count} did not block; {unitEvaluation.Structural} checked structurally only{unitEvaluation.Note}.";
		return Outcome.Of(num2 == 0, summary, list2);
	}

	private static UnitEvaluation EvaluateOnUnit(List<string[]> edges, Dictionary<string, List<Prerequisite>> components, Dictionary<string, SimpleBlueprint> byName, Dictionary<string, string> category)
	{
		UnitEvaluation unitEvaluation = new UnitEvaluation();
		List<string> list = (from e in edges
			where category.ContainsKey(e[0]) && category.ContainsKey(e[1])
			select e[0] + "\u0001" + e[1]).Distinct().ToList();
		List<string> list2 = list.Where(delegate(string k)
		{
			string key = k.Split('\u0001')[1];
			return (category[key] == "heritage" || category[key] == "legacy") && byName[key] is BlueprintFeature;
		}).ToList();
		unitEvaluation.Structural = list.Count - list2.Count;
		if (list2.Count == 0)
		{
			return unitEvaluation;
		}
		UnitEntityData unit = null;
		try
		{
			BlueprintUnit blueprintUnit = BlueprintRoot.Instance?.DefaultPlayerCharacter;
			if (blueprintUnit == null || Game.Instance == null)
			{
				unitEvaluation.Structural += list2.Count;
				unitEvaluation.Note = " (no blank unit available)";
				return unitEvaluation;
			}
			using (ContextData<UnitEntityData.ChargenUnit>.Request())
			{
				unit = Game.Instance.CreateUnitVacuum(blueprintUnit);
			}
			foreach (IGrouping<string, string> item in from k in list2
				group k by k.Split('\u0001')[1])
			{
				BlueprintFeature blueprint = (BlueprintFeature)byName[item.Key];
				EntityFact entityFact = null;
				try
				{
					entityFact = unit.Descriptor.AddFact(blueprint);
					if (entityFact == null || !unit.Descriptor.HasFact(blueprint))
					{
						unitEvaluation.Structural += item.Count();
						continue;
					}
					foreach (string item2 in item)
					{
						string text = item2.Split('\u0001')[0];
						bool num = components[item2].Any((Prerequisite p) => !p.Check(null, unit.Descriptor, null));
						unitEvaluation.Evaluated++;
						if (!num)
						{
							unitEvaluation.Failures.Add("A unit holding " + item.Key + " still meets the prerequisites of " + text + " that are meant to exclude it.");
						}
					}
				}
				catch (Exception ex)
				{
					unitEvaluation.Structural += item.Count();
					unitEvaluation.Note = " (unit evaluation stopped on " + item.Key + ": " + ex.GetType().Name + ")";
				}
				finally
				{
					try
					{
						if (entityFact != null)
						{
							unit.Descriptor.RemoveFact(entityFact);
						}
					}
					catch
					{
					}
				}
			}
		}
		catch (Exception ex2)
		{
			unitEvaluation.Structural += list2.Count - unitEvaluation.Evaluated;
			unitEvaluation.Note = " (no blank unit: " + ex2.GetType().Name + ": " + ex2.Message + ")";
		}
		finally
		{
			try
			{
				unit?.Dispose();
			}
			catch
			{
			}
		}
		return unitEvaluation;
	}

	private static Outcome FormModels()
	{
		List<string> list = new List<string>();
		int num = 0;
		int num2 = 0;
		Dictionary<string, ProbeResult> dictionary = new Dictionary<string, ProbeResult>();
		string[] formBuffNames = FormBuffNames;
		foreach (string text in formBuffNames)
		{
			BlueprintBuff modBlueprint = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, text);
			if (modBlueprint == null)
			{
				num++;
				list.Add(text + " does not exist.");
				continue;
			}
			var list2 = ReachableBuffs(modBlueprint).SelectMany((ReachedBuff r) => from p in (r.Buff.ComponentsArray ?? new BlueprintComponent[0]).OfType<Polymorph>()
				select new
				{
					Path = r.Path,
					Polymorph = p
				}).ToList();
			if (list2.Count == 0)
			{
				num++;
				list.Add(text + " reaches no Polymorph component, so the form has no model.");
			}
			foreach (var item in list2)
			{
				var array = new[]
				{
					new
					{
						Label = "model",
						Link = item.Polymorph.m_Prefab
					},
					new
					{
						Label = "female model",
						Link = item.Polymorph.m_PrefabFemale
					}
				};
				foreach (var anon in array)
				{
					if (anon.Link == null || !anon.Link.Exists())
					{
						if (anon.Label == "model")
						{
							num++;
							list.Add(item.Path + ": the Polymorph names no model asset.");
						}
						continue;
					}
					num2++;
					UnitEntityView unitEntityView = null;
					try
					{
						unitEntityView = anon.Link.Load();
					}
					catch (Exception ex)
					{
						list.Add(item.Path + ": loading " + anon.Label + " " + anon.Link.AssetId + " threw " + ex.GetType().Name + ": " + ex.Message);
					}
					if (unitEntityView == null)
					{
						num++;
						list.Add(item.Path + ": " + anon.Label + " asset " + anon.Link.AssetId + " does not load as a UnitEntityView.");
						continue;
					}
					string text2 = DollRoomKind(unitEntityView);
					if (text2 != null)
					{
						num++;
						list.Add(item.Path + ": " + anon.Label + " asset " + anon.Link.AssetId + " loads, but " + text2);
					}
				}
			}
			bool overlord = text == "SkeletalOverlordFormBuff";
			Polymorph preview = null;
			try
			{
				preview = CharGenSafetyPatch.PreviewPolymorph(overlord);
			}
			catch (Exception ex2)
			{
				list.Add("Preview: CharGenSafetyPatch.PreviewPolymorph threw " + ex2.GetType().Name + ": " + ex2.Message);
			}
			if (preview == null)
			{
				num++;
				list.Add("Preview: the character-creation preview finds no model for " + text + "; nothing it grants carries a Polymorph naming a model, and neither does the fallback buff.");
			}
			else if (!list2.Any(p => p.Polymorph == preview))
			{
				num++;
				list.Add("Preview: the character-creation preview shows a model " + text + " does not reach, so the preview and the form in play are different bodies.");
			}
			dictionary[text] = ProbeBody(preview, text);
			UnitViewLink unitViewLink = ((preview == null) ? null : GuardianVisualScale.PrefabFor(preview, Gender.Female));
			if (unitViewLink != null && unitViewLink.AssetId != preview.m_Prefab?.AssetId)
			{
				dictionary[text + " (female)"] = ProbeBody(preview, text, Gender.Female);
			}
		}
		num += SelfTestBorrowedModelChecks.FormBodies(list, out var summary);
		string arg = "not run";
		if (dictionary.Count > 0)
		{
			foreach (KeyValuePair<string, ProbeResult> item2 in dictionary)
			{
				if (!item2.Value.Drew)
				{
					string text3 = "Render: the " + item2.Key + " model is instantiated and prepared the way the doll room preview prepares it, and still has no mesh to draw (" + item2.Value.Failure + ").";
					if (item2.Value.EngineDrew)
					{
						num++;
						list.Add(text3 + " The engine's own recipe (DollRoom.CreateAvatar) draws the same prefab here, so the preview's preparation is what is missing something.");
					}
					else
					{
						list.Add(text3 + " The engine's own recipe draws nothing here either, so this is the probe's environment and not the form.");
					}
				}
			}
			arg = string.Join(", ", dictionary.Select((KeyValuePair<string, ProbeResult> p) => p.Key + ": " + (p.Value.Drew ? "draws" : "blank")).ToArray());
		}
		BlueprintBuff modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "SlimeFormBuff");
		string description = "not evaluated";
		if (modBlueprint2 != null)
		{
			List<SizeSource> list3 = SizeSources(modBlueprint2).ToList();
			if (list3.Count == 0)
			{
				num++;
				list.Add("SlimeFormBuff sets no size at all, so the slime keeps the character's own size.");
			}
			foreach (SizeSource item3 in list3.Where((SizeSource s) => s.Size != Size.Small))
			{
				num++;
				list.Add($"{item3.Source} sets the size to {item3.Size}, not Small; the engine keeps whichever size fact is added last.");
			}
			Size? size = MeasureSlimeSize(modBlueprint2, out description);
			if (size.HasValue && size.Value != Size.Small)
			{
				num++;
				list.Add($"A blank unit given SlimeFormBuff is {size.Value}, not Small.");
			}
		}
		string summary2 = $"{num2} form models checked; render probe: {arg}; " + $"slime size on a blank unit right after the buff: {description}; bodies: {summary}; {num} problems.";
		return Outcome.Of(num == 0, summary2, list);
	}

	private static string DollRoomKind(UnitEntityView view)
	{
		try
		{
			if (CharGenSafetyPatch.HasBodyGeometry(view.gameObject))
			{
				return null;
			}
			Character componentInChildren = view.GetComponentInChildren<Character>(includeInactive: true);
			if (componentInChildren == null)
			{
				return "it carries neither a mesh of its own nor a Character to build one, so the doll room has nothing to draw.";
			}
			if (!(componentInChildren.BakedCharacter != null) && (componentInChildren.SavedEquipmentEntities == null || componentInChildren.SavedEquipmentEntities.Count <= 0))
			{
				return "it is a character-system doll with no baked character and no saved equipment entities, so it can never build a body.";
			}
			return null;
		}
		catch (Exception ex)
		{
			return "inspecting it threw " + ex.GetType().Name + ": " + ex.Message;
		}
	}

	private static ProbeResult ProbeBody(Polymorph preview, string formName, Gender gender = Gender.Male)
	{
		ProbeResult result = new ProbeResult
		{
			Drew = false,
			EngineDrew = false,
			Failure = "the preview found no model to probe"
		};
		if (preview == null)
		{
			return result;
		}
		UnitEntityView unitEntityView = null;
		try
		{
			unitEntityView = GuardianVisualScale.PrefabFor(preview, gender)?.Load();
		}
		catch
		{
			unitEntityView = null;
		}
		if (unitEntityView == null)
		{
			result.Failure = "the model asset did not load";
			return result;
		}
		result.EngineDrew = EngineCanDraw(unitEntityView, formName);
		UnitEntityView unitEntityView2 = null;
		try
		{
			unitEntityView2 = Park(UnityEngine.Object.Instantiate(unitEntityView));
			result.Drew = CharGenSafetyPatch.PrepareDollRoomBody(unitEntityView2, "SelfTest " + formName, null, out var failure);
			result.Failure = failure;
		}
		catch (Exception ex)
		{
			result.Drew = false;
			result.Failure = "probing it threw " + ex.GetType().Name + ": " + ex.Message;
		}
		finally
		{
			try
			{
				if (unitEntityView2 != null)
				{
					UnityEngine.Object.Destroy(unitEntityView2.gameObject);
				}
			}
			catch
			{
			}
		}
		return result;
	}

	private static bool EngineCanDraw(UnitEntityView prefab, string formName)
	{
		UnitEntityView unitEntityView = null;
		try
		{
			unitEntityView = Park(UnityEngine.Object.Instantiate(prefab));
			Character componentInChildren = unitEntityView.GetComponentInChildren<Character>(includeInactive: true);
			if (componentInChildren != null)
			{
				componentInChildren.PreventUpdate = false;
				componentInChildren.IsInDollRoom = true;
				componentInChildren.gameObject.name = "Doll [SelfTest oracle " + formName + "]";
				componentInChildren.Start();
			}
			return CharGenSafetyPatch.HasBodyGeometry(unitEntityView.gameObject);
		}
		catch
		{
			return false;
		}
		finally
		{
			try
			{
				if (unitEntityView != null)
				{
					UnityEngine.Object.Destroy(unitEntityView.gameObject);
				}
			}
			catch
			{
			}
		}
	}

	private static UnitEntityView Park(UnitEntityView instance)
	{
		instance.transform.position = new Vector3(0f, -10000f, 0f);
		instance.gameObject.layer = 15;
		if (!instance.gameObject.activeSelf)
		{
			instance.gameObject.SetActive(value: true);
		}
		return instance;
	}

	private static List<ReachedBuff> ReachableBuffs(BlueprintBuff root, int maxDepth = 5)
	{
		List<ReachedBuff> list = new List<ReachedBuff>
		{
			new ReachedBuff
			{
				Buff = root,
				Path = root.name,
				Depth = 0
			}
		};
		HashSet<BlueprintGuid> hashSet = new HashSet<BlueprintGuid> { root.AssetGuid };
		for (int i = 0; i < list.Count; i++)
		{
			ReachedBuff reachedBuff = list[i];
			if (reachedBuff.Depth >= maxDepth)
			{
				continue;
			}
			foreach (BlueprintReferenceWalker.Found item in BlueprintReferenceWalker.ReferencesIn(reachedBuff.Buff))
			{
				string text = item.Owner?.GetType().Name;
				if ((!(text != "AddFacts") || !(text != "ContextActionApplyBuff")) && item.Reference != null && !(item.Reference.Guid == BlueprintGuid.Empty))
				{
					BlueprintBuff blueprintBuff;
					try
					{
						blueprintBuff = ResourcesLibrary.TryGetBlueprint(item.Reference.Guid) as BlueprintBuff;
					}
					catch
					{
						blueprintBuff = null;
					}
					if (blueprintBuff != null && hashSet.Add(blueprintBuff.AssetGuid))
					{
						list.Add(new ReachedBuff
						{
							Buff = blueprintBuff,
							Path = reachedBuff.Path + " -> " + blueprintBuff.name,
							Depth = reachedBuff.Depth + 1,
							ViaAddFacts = (text == "AddFacts")
						});
					}
				}
			}
		}
		return list;
	}

	private static IEnumerable<SizeSource> SizeSources(BlueprintBuff buff)
	{
		foreach (ReachedBuff reached in ReachableBuffs(buff))
		{
			BlueprintComponent[] array = reached.Buff.ComponentsArray ?? new BlueprintComponent[0];
			foreach (BlueprintComponent blueprintComponent in array)
			{
				if (blueprintComponent is Polymorph polymorph)
				{
					yield return new SizeSource
					{
						Source = reached.Path + " (Polymorph)",
						Size = polymorph.Size
					};
				}
				else if (blueprintComponent is ChangeUnitSize changeUnitSize)
				{
					if (changeUnitSize.m_Type == ChangeUnitSize.ChangeType.Value)
					{
						yield return new SizeSource
						{
							Source = reached.Path + " (ChangeUnitSize)",
							Size = changeUnitSize.Size
						};
					}
					else
					{
						yield return new SizeSource
						{
							Source = $"{reached.Path} (ChangeUnitSize, delta {changeUnitSize.SizeDelta} from Medium)",
							Size = Size.Medium.Shift(changeUnitSize.SizeDelta, Size.Fine)
						};
					}
				}
			}
		}
	}

	private static Size? MeasureSlimeSize(BlueprintBuff slime, out string description)
	{
		UnitEntityData unitEntityData = null;
		try
		{
			BlueprintUnit blueprintUnit = BlueprintRoot.Instance?.DefaultPlayerCharacter;
			if (blueprintUnit == null || Game.Instance == null)
			{
				description = "no blank unit available";
				return null;
			}
			using (ContextData<UnitEntityData.ChargenUnit>.Request())
			{
				unitEntityData = Game.Instance.CreateUnitVacuum(blueprintUnit);
			}
			Size size = unitEntityData.Descriptor.State.Size;
			Buff buff = unitEntityData.Descriptor.Buffs.AddBuff(slime, unitEntityData, null);
			if (buff == null || !buff.Active)
			{
				description = "the buff did not activate on a blank unit, structural rule only";
				return null;
			}
			Size size2 = unitEntityData.Descriptor.State.Size;
			try
			{
				unitEntityData.Descriptor.Buffs.RemoveFact(slime);
			}
			catch
			{
			}
			description = $"{size} -> {size2}";
			return size2;
		}
		catch (Exception ex)
		{
			description = "evaluation threw " + ex.GetType().Name + ": " + ex.Message + ", structural rule only";
			return null;
		}
		finally
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

	private static Outcome UnitModels()
	{
		List<string> list = new List<string>();
		int num = 0;
		int num2 = 0;
		foreach (SelfTestRecorder.CreatedBlueprint item in ModBlueprintsById().Values.OrderBy((SelfTestRecorder.CreatedBlueprint c) => c.Name, StringComparer.Ordinal))
		{
			if (!(item.Blueprint is BlueprintUnit blueprintUnit))
			{
				continue;
			}
			num++;
			UnitViewLink prefab = blueprintUnit.Prefab;
			if (prefab == null || !prefab.Exists())
			{
				num2++;
				list.Add(item.Name + " names no model asset.");
				continue;
			}
			UnitEntityView unitEntityView = null;
			try
			{
				unitEntityView = prefab.Load();
			}
			catch (Exception ex)
			{
				list.Add(item.Name + ": loading " + prefab.AssetId + " threw " + ex.GetType().Name + ": " + ex.Message);
			}
			if (unitEntityView == null)
			{
				num2++;
				list.Add(item.Name + ": model asset " + prefab.AssetId + " does not load as a UnitEntityView.");
			}
		}
		bool flag = IsekaiDlcManager.HasDlc6();
		bool[] array = ((!flag) ? new bool[1] : new bool[2] { false, true });
		foreach (bool flag2 in array)
		{
			BlueprintUnit blueprintUnit2 = LanternKingCameo.ModelSource(flag2);
			string text = LanternKingCameo.ModelSourceGuid(flag2);
			UnitEntityView unitEntityView2 = null;
			try
			{
				unitEntityView2 = blueprintUnit2?.Prefab?.Load();
			}
			catch
			{
				unitEntityView2 = null;
			}
			if (blueprintUnit2 == null || blueprintUnit2.AssetGuid.ToString() != text || unitEntityView2 == null)
			{
				num2++;
				list.Add("The Lantern King cameo's " + (flag2 ? "DLC 6" : "base-game") + " model (" + text + ") does not load as a UnitEntityView.");
			}
		}
		int num4 = SelfTestBorrowedModelChecks.UnitBodies(list, out var summary);
		num2 += num4;
		return Outcome.Of(num2 == 0, string.Format("{0} mod units and the Lantern King cameo's {1}; ", num, flag ? "two models" : "base-game model") + $"{num2 - num4} with a model that does not load; {summary}, {num4} body problems.", list);
	}

	private static Outcome ServiceNpcProtection()
	{
		List<string> list = new List<string>();
		string[] serviceNpcNames = IsekaiNpcUnit.ServiceNpcNames;
		foreach (string text in serviceNpcNames)
		{
			List<string> list2 = IsekaiNpcUnit.BlueprintProtectionProblems(BlueprintTools.GetModBlueprint<BlueprintUnit>(Main.IsekaiContext, text), checkFaction: true);
			list.Add(((list2.Count == 0) ? "ok   " : "FAIL ") + text);
			list.AddRange(list2.Select((string p) => "  " + p));
		}
		bool num = !list.Any((string d) => d.StartsWith("FAIL", StringComparison.Ordinal));
		return Outcome.Of(num, num ? "Every service NPC is peaceful, immortal, damage immune and ignored by combat." : "A service NPC can be fought.", list);
	}

	private static Outcome UnitsCanSpawn()
	{
		List<string> list = new List<string>();
		int num = 0;
		int num2 = 0;
		foreach (SelfTestRecorder.CreatedBlueprint item in ModBlueprintsById().Values.OrderBy((SelfTestRecorder.CreatedBlueprint c) => c.Name, StringComparer.Ordinal))
		{
			if (!(item.Blueprint is BlueprintUnit blueprintUnit))
			{
				continue;
			}
			num++;
			List<string> list2 = new List<string>();
			if (blueprintUnit.Skills == null)
			{
				list2.Add("Skills");
			}
			if (blueprintUnit.Faction == null)
			{
				list2.Add("Faction");
			}
			if (blueprintUnit.FactionOverrides == null)
			{
				list2.Add("FactionOverrides");
			}
			else
			{
				if (blueprintUnit.FactionOverrides.m_AttackFactionsToAdd == null)
				{
					list2.Add("FactionOverrides.AttackFactionsToAdd");
				}
				if (blueprintUnit.FactionOverrides.m_AttackFactionsToRemove == null)
				{
					list2.Add("FactionOverrides.AttackFactionsToRemove");
				}
			}
			if (list2.Count > 0)
			{
				num2++;
				list.Add(item.Name + " cannot be spawned: " + string.Join(", ", list2) + " " + ((list2.Count == 1) ? "is" : "are") + " null.");
			}
		}
		return Outcome.Of(num2 == 0, $"{num} mod units; {num2} that the engine cannot bring to life.", list);
	}

	private static Outcome Localization()
	{
		string text = ReadEmbeddedResource(".LocalizationPack.json");
		if (text == null)
		{
			return Outcome.Of(passed: false, "LocalizationPack.json is not embedded in IsekaiMod.dll.");
		}
		HashSet<string> shipped = new HashSet<string>(StringComparer.InvariantCulture);
		if (JObject.Parse(text)["LocalizedStrings"] is JArray jArray)
		{
			foreach (JToken item in jArray)
			{
				string text2 = item?["enGB"]?.ToString();
				if (text2 != null)
				{
					shipped.Add(text2);
				}
			}
		}
		MultiLocalizationPack multiLocalizationPack = Main.IsekaiContext?.ModLocalizationPack;
		if (multiLocalizationPack?.Strings == null)
		{
			return Outcome.Of(passed: false, "The mod has no localization pack in memory.");
		}
		List<MultiLocalizationPack.MultiLocaleString> list = multiLocalizationPack.Strings.Where((MultiLocalizationPack.MultiLocaleString s) => s?.IsUsed ?? false).ToList();
		List<MultiLocalizationPack.MultiLocaleString> source = list.Where((MultiLocalizationPack.MultiLocaleString s) => !shipped.Contains(s.enGB ?? "")).OrderBy((MultiLocalizationPack.MultiLocaleString s) => s.SimpleName, StringComparer.Ordinal).ToList();
		List<MultiLocalizationPack.MultiLocaleString> list2 = source.Where((MultiLocalizationPack.MultiLocaleString s) => RuntimeBadgeStrings.IsAllowed(s.SimpleName)).ToList();
		List<MultiLocalizationPack.MultiLocaleString> list3 = source.Where((MultiLocalizationPack.MultiLocaleString s) => !RuntimeBadgeStrings.IsAllowed(s.SimpleName)).ToList();
		List<string> list4 = list3.Select((MultiLocalizationPack.MultiLocaleString s) => s.SimpleName + " (" + s.Key + "): \"" + Shorten(s.enGB, 90) + "\"").ToList();
		if (list2.Count > 0)
		{
			list4.AddRange(SelfTestReport.WrapList($"Allowed, not a failure: {list2.Count} deity sponsor badge strings built at run time in the player's language:", list2.Select((MultiLocalizationPack.MultiLocaleString s) => s.SimpleName)));
		}
		return Outcome.Of(list3.Count == 0, $"{list.Count} strings used; {list3.Count} have no LocalizationPack.json entry; {list2.Count} deity sponsor badges allowed (built at run time).", list4);
	}

	private static Outcome BlueprintText()
	{
		List<string> list = new List<string>();
		int num = 0;
		int num2 = 0;
		foreach (SelfTestRecorder.CreatedBlueprint item in SelfTestRecorder.CreatedBlueprints())
		{
			SimpleBlueprint blueprint = item.Blueprint;
			if (blueprint == null)
			{
				continue;
			}
			foreach (FieldInfo item2 in BlueprintFieldDefaults.NullLocalizedStringFields(blueprint))
			{
				num++;
				list.Add(item.Name + "." + item2.Name + " is a null LocalizedString; the engine renders it as \"<null>\".");
			}
			BlueprintComponent[] array = (blueprint as BlueprintScriptableObject)?.ComponentsArray;
			if (array != null)
			{
				BlueprintComponent[] array2 = array;
				foreach (BlueprintComponent blueprintComponent in array2)
				{
					if (blueprintComponent == null)
					{
						continue;
					}
					foreach (FieldInfo item3 in BlueprintFieldDefaults.NullLocalizedStringFields(blueprintComponent))
					{
						num++;
						list.Add(item.Name + " component " + blueprintComponent.GetType().Name + "." + item3.Name + " is a null LocalizedString; the engine renders it as \"<null>\".");
					}
				}
			}
			if (blueprint is BlueprintUnitFact blueprintUnitFact)
			{
				num2 += 3;
				InspectRenderedText(list, item.Name, "Name", blueprintUnitFact.Name);
				InspectRenderedText(list, item.Name, "Description", blueprintUnitFact.Description);
				InspectRenderedText(list, item.Name, "DescriptionShort", blueprintUnitFact.DescriptionShort);
			}
			if (blueprint is BlueprintAbility blueprintAbility)
			{
				num2 += 2;
				InspectRenderedText(list, item.Name, "LocalizedDuration", blueprintAbility.LocalizedDuration);
				InspectRenderedText(list, item.Name, "LocalizedSavingThrow", blueprintAbility.LocalizedSavingThrow);
			}
			if (blueprint is BlueprintCharacterClass blueprintCharacterClass)
			{
				num2 += 3;
				InspectRenderedText(list, item.Name, "Name", blueprintCharacterClass.Name);
				InspectRenderedText(list, item.Name, "Description", blueprintCharacterClass.Description);
				InspectRenderedText(list, item.Name, "LocalizedDescriptionShort", blueprintCharacterClass.LocalizedDescriptionShort?.ToString());
				InspectClassPage(list, item.Name, blueprintCharacterClass.Name, blueprintCharacterClass.Description, blueprintCharacterClass.LocalizedDescriptionShort?.ToString());
			}
			if (blueprint is BlueprintArchetype { HiddenInUI: false } blueprintArchetype)
			{
				num2 += 2;
				InspectRenderedText(list, item.Name, "Name", blueprintArchetype.Name);
				InspectRenderedText(list, item.Name, "Description", blueprintArchetype.Description);
				InspectClassPage(list, item.Name, blueprintArchetype.Name, blueprintArchetype.Description, null);
			}
		}
		return Outcome.Of(list.Count == 0, $"{num2} rendered strings read from {SelfTestRecorder.CreatedBlueprints().Count} mod blueprints; {num} null localized string field(s); {list.Count} problem(s).", list.Take(200));
	}

	private static void InspectRenderedText(List<string> problems, string blueprintName, string what, string text)
	{
		if (!string.IsNullOrEmpty(text))
		{
			string text2 = null;
			if (text.Contains("<null>"))
			{
				text2 = "renders as \"<null>\" (a null LocalizedString)";
			}
			else if (text.Contains("[unknown key:"))
			{
				text2 = "names a localization key the pack does not have";
			}
			else if (UnformattedArgument.IsMatch(text))
			{
				text2 = "still holds a string.Format argument";
			}
			else if (LocalizationManager.CurrentLocale == Locale.enGB && BareNull.IsMatch(text))
			{
				text2 = "contains the bare word \"null\"";
			}
			if (text2 != null)
			{
				problems.Add(blueprintName + "." + what + " " + text2 + ": \"" + Shorten(text, 120) + "\"");
			}
		}
	}

	private static void InspectClassPage(List<string> problems, string blueprintName, string displayName, string description, string descriptionShort)
	{
		if (LocalizationManager.CurrentLocale != Locale.enGB)
		{
			return;
		}
		string text = ClassDescriptionRules.Problem(displayName, description, 100);
		if (text != null)
		{
			problems.Add(blueprintName + ".Description " + text + ": \"" + Shorten(description ?? "", 120) + "\"");
		}
		if (descriptionShort != null)
		{
			text = ClassDescriptionRules.Problem(displayName, descriptionShort, 60);
			if (text != null)
			{
				problems.Add(blueprintName + ".LocalizedDescriptionShort " + text + ": \"" + Shorten(descriptionShort, 120) + "\"");
			}
		}
	}

	private static string Shorten(string text, int length)
	{
		text = SelfTestReport.Clean(text);
		if (text.Length > length)
		{
			return text.Substring(0, length) + "...";
		}
		return text;
	}

	private static Outcome EngineExceptions()
	{
		if (!SelfTestEngineLog.Installed)
		{
			return Outcome.Of(passed: false, "The engine's log was never listened to, so no engine-caught exception could be seen (see the INFO line after BEGIN).");
		}
		List<SelfTestEngineLog.Caught> list = SelfTestEngineLog.All(out var droppedCount);
		HashSet<string> checkIds = new HashSet<string>(from c in All()
			select c.Id, StringComparer.Ordinal);
		List<string> list2 = new List<string>();
		int num = 0;
		List<SelfTestEngineLog.Caught> list3 = list.Where((SelfTestEngineLog.Caught c) => c.Context == null || !checkIds.Contains(c.Context)).ToList();
		List<SelfTestEngineLog.Caught> list4 = list3.Where(SelfTestEngineLog.IsOurs).ToList();
		foreach (IGrouping<string, SelfTestEngineLog.Caught> item in (from c in list4
			group c by c.Context + "|" + c.Signature).OrderBy((IGrouping<string, SelfTestEngineLog.Caught> g) => g.Key, StringComparer.Ordinal))
		{
			SelfTestEngineLog.Caught caught = item.First();
			EngineExceptionRules.Known known = SelfTestEngineLog.MatchKnown(caught);
			string text = string.Format("{0}: the engine caught {1} {2} time(s)", caught.Context ?? "(no context)", caught.Describe(), item.Count());
			string text2 = EngineExceptionRules.FirstModFrame(caught.AllFrames);
			string text3 = text + ((text2 != null) ? (", on a stack through " + text2) : ", naming a mod blueprint") + SelfTestEngineLog.SourcesOf(item);
			if (known != null)
			{
				list2.Add(EngineExceptionRules.KnownIssueLine(known, text3));
				continue;
			}
			num++;
			list2.Add(text3 + ".");
		}
		HashSet<string> hashSet = SelfTestEngineLog.KnownMatched();
		foreach (EngineExceptionRules.Known item2 in EngineExceptionRules.KnownExceptions.Where((EngineExceptionRules.Known k) => k.ProductBug))
		{
			if (!hashSet.Contains(item2.Id + "|" + item2.ThrowSite))
			{
				num++;
				list2.Add("The allow-list entry " + item2.Id + " (" + item2.ThrowSite + ") matched nothing in this run: the bug it excuses no longer happens, so delete the entry from EngineExceptionRules.KnownExceptions and $KnownEngineExceptions in Check-GameLog.ps1.");
			}
		}
		List<string> list5 = (from g in (from c in list
				group c by c.Context ?? "(no context)" into g
				orderby g.Count() descending
				select g).ThenBy((IGrouping<string, SelfTestEngineLog.Caught> g) => g.Key, StringComparer.Ordinal)
			select $"{g.Key} {g.Count()}").ToList();
		if (list5.Count > 0)
		{
			list2.AddRange(SelfTestReport.WrapList("Engine-caught exceptions by where they happened:", list5));
		}
		string summary = string.Format("{0} engine-caught exception(s) this run{1}; ", list.Count, (droppedCount > 0) ? $" (and {droppedCount} more not kept)" : "") + $"{list3.Count} outside any check, {list4.Count} of them through the mod's code or naming its blueprints; " + $"{EngineExceptionRules.KnownExceptions.Count((EngineExceptionRules.Known k) => k.ProductBug)} known product-bug entries, {num} problems.";
		return Outcome.Of(num == 0, summary, list2);
	}

	private static Outcome StartupExceptions()
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		SelfTestRecorder.LoggedUnityExceptions(out var startup, out var duringChecks);
		list.AddRange(startup.Select((string e) => "Unity log: " + e));
		list2.AddRange(duringChecks.Select((string e) => "(during the checks, not counted) Unity log: " + e));
		string text = null;
		try
		{
			text = Application.consoleLogPath;
		}
		catch
		{
		}
		string arg;
		if (string.IsNullOrEmpty(text) || !File.Exists(text))
		{
			arg = "Player.log not found, only Unity log messages were checked";
		}
		else
		{
			int num = 0;
			bool flag = false;
			using (FileStream stream = new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
			{
				using StreamReader streamReader = new StreamReader(stream);
				string text2;
				while ((text2 = streamReader.ReadLine()) != null)
				{
					num++;
					if (!flag && text2.IndexOf("[IsekaiSelfTest]", StringComparison.Ordinal) >= 0 && text2.IndexOf("Main menu reached after", StringComparison.Ordinal) >= 0)
					{
						flag = true;
					}
					else if (SelfTestLogScan.IsModExceptionLine(text2))
					{
						if (flag)
						{
							list2.Add($"(during the checks, not counted) Player.log line {num}: {text2}");
						}
						else
						{
							list.Add($"Player.log line {num}: {text2}");
						}
					}
				}
			}
			arg = $"{num} Player.log lines read";
		}
		List<string> details = list.Concat(list2).ToList();
		return Outcome.Of(list.Count == 0, $"{list.Count} exceptions logged by IsekaiMod during startup, {list2.Count} more while the checks ran ({arg}).", details);
	}
}
