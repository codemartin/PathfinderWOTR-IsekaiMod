using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Heritages;

internal class HumanHeritageSelection
{
	private sealed class AutoPickRepair : IAreaHandler, ISubscriber, IGlobalSubscriber
	{
		public void OnAreaDidLoad()
		{
			try
			{
				Game instance = Game.Instance;
				List<UnitEntityData> list = instance?.State?.Units?.ToList();
				if (list == null || ourHeritageSelection == null)
				{
					return;
				}
				HashSet<UnitEntityData> characters = new HashSet<UnitEntityData>(instance.Player?.AllCharacters ?? new List<UnitEntityData>());
				int num = 0;
				foreach (UnitEntityData item in list)
				{
					if (!(item == null) && item.Progression != null && ShouldRepair(item.Progression.Selections.ContainsKey(ourHeritageSelection), IsPlayerSide(item, characters)))
					{
						int num2 = RepairUnit(item);
						if (num2 > 0)
						{
							num += num2;
							Main.IsekaiContext.Logger.Log("[HumanHeritage] " + item.CharacterName + " (" + item.Blueprint?.name + ") had taken an Isekai heritage only because it sorted first; it is 'None' now.");
						}
					}
				}
				if (num > 0)
				{
					Main.IsekaiContext.Logger.Log($"[HumanHeritage] Took back {num} auto-picked Isekai heritages in this area.");
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("[HumanHeritage] Could not repair auto-picked heritages: " + ex);
			}
		}

		public void OnAreaBeginUnloading()
		{
		}
	}

	private static BlueprintFeature[] ourHeritages;

	private static BlueprintFeature dummyNoFeat;

	private static BlueprintFeatureSelection ourHeritageSelection;

	public static void CreateDummy()
	{
		ourHeritages = new BlueprintFeature[0];
		ourHeritageSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiHumanHeritageSelection", delegate(BlueprintFeatureSelection bp)
		{
			bp.SetName(Main.IsekaiContext, "Alternate Racial Traits");
			bp.SetDescription(Main.IsekaiContext, "The following alternate traits are available.");
			bp.IsClassFeature = true;
			bp.Groups = new FeatureGroup[1] { FeatureGroup.Racial };
			bp.Group = FeatureGroup.Racial;
			bp.m_Features = new BlueprintFeatureReference[0];
			bp.m_AllFeatures = new BlueprintFeatureReference[0];
		});
		dummyNoFeat = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiHumanHeritageNoFeat", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "None");
			bp.SetDescription(Main.IsekaiContext, "No alternate trait");
			((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)FeatTools.Selections.BasicFeatSelection).m_Icon;
			bp.IsClassFeature = true;
		});
	}

	public static void Register(BlueprintFeature feature)
	{
		ourHeritages = ourHeritages.AppendToArray(feature);
	}

	internal static T[] DefaultFirst<T>(T first, IEnumerable<T> others, Func<T, T, bool> same) where T : class
	{
		List<T> list = new List<T>();
		if (first != null)
		{
			list.Add(first);
		}
		foreach (T option in others ?? Enumerable.Empty<T>())
		{
			if (option != null && !list.Any((T existing) => same(existing, option)))
			{
				list.Add(option);
			}
		}
		return list.ToArray();
	}

	internal static T[] AppendKeepingOrder<T>(IEnumerable<T> existing, IEnumerable<T> additions, Func<T, T, bool> same) where T : class
	{
		List<T> list = new List<T>((existing ?? Enumerable.Empty<T>()).Where((T o) => o != null));
		foreach (T option in additions ?? Enumerable.Empty<T>())
		{
			if (option != null && !list.Any((T arg) => same(arg, option)))
			{
				list.Add(option);
			}
		}
		return list.ToArray();
	}

	private static bool SameReference(BlueprintFeatureReference a, BlueprintFeatureReference b)
	{
		if (a != null && b != null)
		{
			return a.Equals(b);
		}
		return false;
	}

	private static BlueprintFeatureReference[] References(IEnumerable<BlueprintFeature> features)
	{
		return (from f in features ?? Enumerable.Empty<BlueprintFeature>()
			where f != null
			select f.ToReference<BlueprintFeatureReference>()).ToArray();
	}

	public static void Patch()
	{
		BlueprintRace blueprint = BlueprintTools.GetBlueprint<BlueprintRace>("0a5d473ead98b0646b94495af250fdc4");
		if (blueprint == null || blueprint.m_Features == null)
		{
			Main.IsekaiContext.Logger.LogWarning("Human race blueprint not found or has no features; skipping human heritage patch.");
			return;
		}
		BlueprintFeatureSelection basicFeatSelection = FeatTools.Selections.BasicFeatSelection;
		BlueprintFeatureSelection blueprintFeatureSelection = null;
		BlueprintFeatureBaseReference[] features = blueprint.m_Features;
		foreach (BlueprintFeatureBaseReference blueprintFeatureBaseReference in features)
		{
			if (blueprintFeatureBaseReference != null && blueprintFeatureBaseReference.Get() is BlueprintFeatureSelection blueprintFeatureSelection2 && !blueprintFeatureSelection2.Equals(basicFeatSelection))
			{
				blueprintFeatureSelection = blueprintFeatureSelection2;
			}
		}
		if (blueprintFeatureSelection != null)
		{
			Main.IsekaiContext.Logger.Log("found a selection added by another mod, adding onto that rather than creating our own");
			BlueprintFeatureReference[] additions = References(ourHeritages);
			blueprintFeatureSelection.m_AllFeatures = AppendKeepingOrder(blueprintFeatureSelection.m_AllFeatures, additions, SameReference);
			blueprintFeatureSelection.m_Features = AppendKeepingOrder(blueprintFeatureSelection.m_Features, additions, SameReference);
			return;
		}
		Main.IsekaiContext.Logger.Log("patching our own alternate human heritage feats into the game because no other source was present");
		if (ourHeritageSelection == null)
		{
			return;
		}
		BlueprintFeatureBaseReference blueprintFeatureBaseReference2 = ourHeritageSelection.ToReference<BlueprintFeatureBaseReference>();
		bool flag = false;
		features = blueprint.m_Features;
		foreach (BlueprintFeatureBaseReference blueprintFeatureBaseReference3 in features)
		{
			if (blueprintFeatureBaseReference3 != null && blueprintFeatureBaseReference3.Equals(blueprintFeatureBaseReference2))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			blueprint.m_Features = blueprint.m_Features.AppendToArray(blueprintFeatureBaseReference2);
		}
		BlueprintFeatureReference first = dummyNoFeat?.ToReference<BlueprintFeatureReference>();
		BlueprintFeatureReference[] second = References(ourHeritages);
		ourHeritageSelection.m_AllFeatures = DefaultFirst(first, ourHeritageSelection.m_AllFeatures.Concat(second), SameReference);
		ourHeritageSelection.m_Features = DefaultFirst(first, ourHeritageSelection.m_Features.Concat(second), SameReference);
	}

	internal static bool ShouldRepair(bool pickedThroughOurSelection, bool isPlayerSide)
	{
		if (pickedThroughOurSelection)
		{
			return !isPlayerSide;
		}
		return false;
	}

	internal static void InitRepair()
	{
		EventBus.Subscribe(new AutoPickRepair());
	}

	private static bool IsPlayerSide(UnitEntityData unit, HashSet<UnitEntityData> characters)
	{
		if (unit == null)
		{
			return true;
		}
		if (unit.IsMainCharacter || unit.IsPlayerFaction || characters.Contains(unit))
		{
			return true;
		}
		UnitPartCompanion unitPartCompanion = unit.Get<UnitPartCompanion>();
		if (unitPartCompanion != null && unitPartCompanion.State != CompanionState.None)
		{
			return true;
		}
		UnitEntityData master = unit.Master;
		if (master != null)
		{
			if (!master.IsPlayerFaction)
			{
				return characters.Contains(master);
			}
			return true;
		}
		return false;
	}

	internal static int RepairUnit(UnitEntityData unit)
	{
		if (ourHeritageSelection == null || dummyNoFeat == null || unit?.Progression == null)
		{
			return 0;
		}
		if (!unit.Progression.Selections.TryGetValue(ourHeritageSelection, out var value) || value == null)
		{
			return 0;
		}
		HashSet<BlueprintFeature> hashSet = new HashSet<BlueprintFeature>(ourHeritages ?? new BlueprintFeature[0]);
		int num = 0;
		foreach (KeyValuePair<int, List<BlueprintFeature>> item in value.SelectionsByLevel.ToList())
		{
			foreach (BlueprintFeature item2 in (item.Value ?? new List<BlueprintFeature>()).ToList())
			{
				if (item2 != null && hashSet.Contains(item2))
				{
					Feature fact = unit.Progression.Features.GetFact(item2);
					if (fact != null)
					{
						unit.RemoveFact(fact);
					}
					value.RemoveSelection(item.Key, item2);
					value.AddSelection(item.Key, dummyNoFeat);
					if (!unit.HasFact(dummyNoFeat))
					{
						unit.Progression.Features.AddFeature(dummyNoFeat);
					}
					num++;
				}
			}
		}
		return num;
	}
}
