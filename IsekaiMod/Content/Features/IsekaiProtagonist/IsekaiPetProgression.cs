using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Components;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist;

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
		_companionProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherCompanionProgression", delegate(BlueprintProgression bp)
		{
			bp.SetName(Main.IsekaiContext, "Otherworldly Companion Bond");
			bp.SetDescription(Main.IsekaiContext, "Your companion advances with your Isekai Protagonist levels.");
			bp.HideInUI = true;
			bp.IsClassFeature = true;
			bp.m_FeaturesRankIncrease = new List<BlueprintFeatureReference>();
			bp.m_Archetypes = new BlueprintProgression.ArchetypeWithLevel[0];
			List<BlueprintProgression.ClassWithLevel> list = new List<BlueprintProgression.ClassWithLevel>
			{
				new BlueprintProgression.ClassWithLevel
				{
					m_Class = IsekaiProtagonistClass.GetReference(),
					AdditionalLevel = 0
				}
			};
			string[] array = EpicPrestigeClasses.CapstoneCountingClassNames();
			foreach (string name in array)
			{
				BlueprintCharacterClassReference modBlueprintReference = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(Main.IsekaiContext, name);
				if (modBlueprintReference != null && !(modBlueprintReference.Guid == BlueprintGuid.Empty))
				{
					list.Add(new BlueprintProgression.ClassWithLevel
					{
						m_Class = modBlueprintReference,
						AdditionalLevel = 0
					});
				}
			}
			bp.m_Classes = list.ToArray();
			bp.LevelEntries = (from level in Enumerable.Range(1, 20)
				select new LevelEntry
				{
					Level = level,
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
		BlueprintProgression prog = GetCompanionProgression();
		if (prog == null || companionFeatures == null)
		{
			return;
		}
		BlueprintFeatureReference progRef = prog.ToReference<BlueprintFeatureReference>();
		foreach (BlueprintFeatureReference companionFeature in companionFeatures)
		{
			if (companionFeature == null)
			{
				continue;
			}
			BlueprintFeature blueprintFeature = companionFeature.Get();
			if (blueprintFeature != null && blueprintFeature.GetComponent<AddPet>() != null && !blueprintFeature.GetComponents<AddFeatureOnApply>().Any((AddFeatureOnApply a) => a.m_Feature != null && ((BlueprintReferenceBase)a.m_Feature).deserializedGuid == ((BlueprintReferenceBase)progRef).deserializedGuid) && !blueprintFeature.GetComponents<AddFacts>().Any((AddFacts a) => a.m_Facts != null && a.m_Facts.Any((BlueprintUnitFactReference f) => f != null && ((BlueprintReferenceBase)f).deserializedGuid == ((BlueprintReferenceBase)progRef).deserializedGuid)))
			{
				blueprintFeature.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { prog.ToReference<BlueprintUnitFactReference>() };
				});
				blueprintFeature.AddComponent(delegate(UpdateProgressionOnApply c)
				{
					c.m_Feature = progRef;
				});
			}
		}
	}
}
