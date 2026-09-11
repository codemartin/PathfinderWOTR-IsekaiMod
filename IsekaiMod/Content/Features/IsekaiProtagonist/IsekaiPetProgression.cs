using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	public static class IsekaiPetProgression
	{
		private static readonly BlueprintFeature AnimalCompanionRank = BlueprintTools.GetBlueprint<BlueprintFeature>("1670990255e4fe948a863bafd5dbda5d");

		private static BlueprintProgression _companionProgression;

		public static BlueprintProgression GetCompanionProgression()
		{
			if (_companionProgression != null)
			{
				return _companionProgression;
			}
			_companionProgression = BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "DeathsnatcherCompanionProgression");
			if (_companionProgression != null)
			{
				return _companionProgression;
			}
			_companionProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherCompanionProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(StaticReferences.Strings.Null);
				bp.SetDescription(StaticReferences.Strings.Null);
				bp.IsClassFeature = true;
				bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
				bp.m_Archetypes = new BlueprintProgression.ArchetypeWithLevel[0];
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = IsekaiProtagonistClass.GetReference(),
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = (from i in Enumerable.Range(2, 19)
					select new LevelEntry
					{
						Level = i,
						m_Features = new List<BlueprintFeatureBaseReference> { AnimalCompanionRank.ToReference<BlueprintFeatureBaseReference>() }
					}).ToArray();
				bp.UIGroups = new UIGroup[0];
				bp.m_UIDeterminatorsGroup = new BlueprintFeatureBaseReference[0];
				bp.GiveFeaturesForPreviousLevels = true;
			});
			return _companionProgression;
		}

		public static void PatchCompanionFeatures(IEnumerable<BlueprintFeatureReference> companionFeatures)
		{
			BlueprintProgression companionProgression = GetCompanionProgression();
			if (companionProgression == null || companionFeatures == null)
			{
				return;
			}
			BlueprintFeatureReference progRef = companionProgression.ToReference<BlueprintFeatureReference>();
			foreach (BlueprintFeatureReference companionFeature in companionFeatures)
			{
				if (companionFeature == null)
				{
					continue;
				}
				BlueprintFeature blueprintFeature = companionFeature.Get();
				if (blueprintFeature != null && blueprintFeature.GetComponent<AddPet>() != null && !blueprintFeature.GetComponents<AddFeatureOnApply>().Any((AddFeatureOnApply a) => a.m_Feature != null && ((BlueprintReferenceBase)a.m_Feature).deserializedGuid == ((BlueprintReferenceBase)progRef).deserializedGuid))
				{
					blueprintFeature.AddComponent(delegate(AddFeatureOnApply c)
					{
						c.m_Feature = progRef;
					});
				}
			}
		}
	}
}
