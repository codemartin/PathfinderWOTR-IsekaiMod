using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Heritages
{
	internal class IsekaiHumanCrossbreedLegacy
	{
		private static BlueprintFeature ourHeritage;

		public static void Add()
		{
			ourHeritage = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiHumanCrossbreedLegacy", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Isekai Crossbreed Human");
				bp.SetDescription(Main.IsekaiContext, "Humanity's remarkable adaptability has long intertwined with planar entities, fey, and ancient bloodlines. \nIn you, dormant ancestral lineages have awakened in vibrant harmony, granting you versatile racial heritage options.");
				bp.Groups = new FeatureGroup[1] { FeatureGroup.Racial };
				bp.ReapplyOnLevelUp = true;
			});
			HumanHeritageSelection.Register(ourHeritage);
		}

		/// <summary>
		/// True when the unit has the Crossbreed heritage and so counts as every race for prerequisite purposes.
		/// Does nothing when Isekai Heritages are disabled or the heritage was never created.
		/// </summary>
		private static bool CountsAsAnyRace(UnitDescriptor unit)
		{
			if (ourHeritage == null || unit == null)
			{
				return false;
			}
			if (!Main.IsekaiContext.AddedContent.Isekai.IsEnabled("Isekai Heritages"))
			{
				return false;
			}
			return unit.HasFact(ourHeritage);
		}

		private static bool IsRace(BlueprintFeature feature)
		{
			return feature is BlueprintRace;
		}

		/// <summary>
		/// Race gates are expressed as a PrerequisiteFeature on the race blueprint. Treat them as met for Crossbreed.
		/// Only the prerequisite result changes; the feat's other prerequisites still apply.
		/// </summary>
		[HarmonyPatch(typeof(PrerequisiteFeature), nameof(PrerequisiteFeature.CheckInternal))]
		private static class PrerequisiteFeaturePatcher
		{
			[HarmonyPostfix]
			private static void Postfix(PrerequisiteFeature __instance, UnitDescriptor unit, ref bool __result)
			{
				if (__result)
				{
					return;
				}
				if (IsRace(__instance.Feature) && CountsAsAnyRace(unit))
				{
					__result = true;
				}
			}
		}

		/// <summary>
		/// "One of these races" gates use PrerequisiteFeaturesFromList. Count every race entry as owned for Crossbreed
		/// and re-evaluate against the required amount, so mixed lists keep their non-race requirements.
		/// </summary>
		[HarmonyPatch(typeof(PrerequisiteFeaturesFromList), nameof(PrerequisiteFeaturesFromList.CheckInternal))]
		private static class PrerequisiteFeaturesFromListPatcher
		{
			[HarmonyPostfix]
			private static void Postfix(PrerequisiteFeaturesFromList __instance, UnitDescriptor unit, ref bool __result)
			{
				if (__result || __instance.m_Features == null)
				{
					return;
				}
				bool flag = false;
				BlueprintFeatureReference[] features = __instance.m_Features;
				foreach (BlueprintFeatureReference blueprintFeatureReference in features)
				{
					if (IsRace(blueprintFeatureReference?.Get()))
					{
						flag = true;
						break;
					}
				}
				if (!flag || !CountsAsAnyRace(unit))
				{
					return;
				}
				int num = 0;
				foreach (BlueprintFeatureReference blueprintFeatureReference2 in features)
				{
					BlueprintFeature blueprintFeature = blueprintFeatureReference2?.Get();
					if (blueprintFeature != null && (IsRace(blueprintFeature) || unit.HasFact(blueprintFeature)))
					{
						num++;
					}
				}
				if (num >= __instance.Amount)
				{
					__result = true;
				}
			}
		}
	}
}
