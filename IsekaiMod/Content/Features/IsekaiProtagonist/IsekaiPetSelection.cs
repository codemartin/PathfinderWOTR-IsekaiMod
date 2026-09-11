using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.SoloMastery;
using IsekaiMod.Content.Guardians;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal class IsekaiPetSelection
	{
		private static readonly Sprite Icon_FriendToAnimals = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

		private static readonly BlueprintFeatureSelection AnimalCompanionSelectionDomain = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("2ecd6c64683b59944a7fe544033bb533");

		private static readonly BlueprintFeatureSelection WitchFamiliarSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("29a333b7ccad3214ea3a51943fa0d8e9");

		public static void Add()
		{
			BlueprintFeature bp = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiNoPetFeature", delegate(BlueprintFeature blueprintFeature2)
			{
				blueprintFeature2.SetName(Main.IsekaiContext, "Solo Sovereign (No Companion)");
				blueprintFeature2.SetDescription(Main.IsekaiContext, "You choose to walk the path of absolute singularity. Forgoing any companion, familiar, or retinue partner, your solitary will tempers your mind and body.\n\nGrants a +2 bonus to all saving throws, a bonus combat or adventuring feat, and unlocks an exclusive Solo Sovereign Awakening based on your archetype at 1st level.\n\nNote: If you later decide to form a bond with a companion or guardian, you may adopt one via an Otherworldly Guardian Pact without losing any of your Solo Sovereign powers.");
				((BlueprintUnitFact)blueprintFeature2).m_Icon = Icon_FriendToAnimals;
				blueprintFeature2.IsClassFeature = true;
				blueprintFeature2.Ranks = 1;
				blueprintFeature2.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				blueprintFeature2.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				blueprintFeature2.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
				if (SoloSovereign.SoloSovereignFeatSelection != null)
				{
					blueprintFeature2.AddComponent(delegate(AddFeatureOnApply c)
					{
						c.m_Feature = SoloSovereign.SoloSovereignFeatSelection.ToReference<BlueprintFeatureReference>();
					});
				}
				if (SoloSovereign.SoloAwakeningSlime != null)
				{
					blueprintFeature2.AddComponent(delegate(AddFeatureOnApply c)
					{
						c.m_Feature = SoloSovereign.SoloAwakeningSlime.ToReference<BlueprintFeatureReference>();
					});
				}
				if (SoloSovereign.SoloAwakeningShadowMonarch != null)
				{
					blueprintFeature2.AddComponent(delegate(AddFeatureOnApply c)
					{
						c.m_Feature = SoloSovereign.SoloAwakeningShadowMonarch.ToReference<BlueprintFeatureReference>();
					});
				}
				if (SoloSovereign.SoloAwakeningOverlord != null)
				{
					blueprintFeature2.AddComponent(delegate(AddFeatureOnApply c)
					{
						c.m_Feature = SoloSovereign.SoloAwakeningOverlord.ToReference<BlueprintFeatureReference>();
					});
				}
				if (SoloSovereign.SoloAwakeningGodEmperor != null)
				{
					blueprintFeature2.AddComponent(delegate(AddFeatureOnApply c)
					{
						c.m_Feature = SoloSovereign.SoloAwakeningGodEmperor.ToReference<BlueprintFeatureReference>();
					});
				}
				if (SoloSovereign.SoloAwakeningHero != null)
				{
					blueprintFeature2.AddComponent(delegate(AddFeatureOnApply c)
					{
						c.m_Feature = SoloSovereign.SoloAwakeningHero.ToReference<BlueprintFeatureReference>();
					});
				}
				if (SoloSovereign.SoloAwakeningMastermind != null)
				{
					blueprintFeature2.AddComponent(delegate(AddFeatureOnApply c)
					{
						c.m_Feature = SoloSovereign.SoloAwakeningMastermind.ToReference<BlueprintFeatureReference>();
					});
				}
				if (SoloSovereign.SoloAwakeningMartialGod != null)
				{
					blueprintFeature2.AddComponent(delegate(AddFeatureOnApply c)
					{
						c.m_Feature = SoloSovereign.SoloAwakeningMartialGod.ToReference<BlueprintFeatureReference>();
					});
				}
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiFamiliarSelection", delegate(BlueprintFeatureSelection blueprintFeatureSelection)
			{
				blueprintFeatureSelection.SetName(Main.IsekaiContext, "Familiar Selection");
				blueprintFeatureSelection.SetDescription(Main.IsekaiContext, "You gain the service of a familiar, which offers you some skill bonuses.");
				((BlueprintUnitFact)blueprintFeatureSelection).m_Icon = Icon_FriendToAnimals;
				blueprintFeatureSelection.IsClassFeature = true;
				blueprintFeatureSelection.Ranks = 1;
				blueprintFeatureSelection.m_AllFeatures = WitchFamiliarSelection?.m_AllFeatures ?? new BlueprintFeatureReference[0];
				blueprintFeatureSelection.m_Features = blueprintFeatureSelection.m_AllFeatures;
			});
			List<BlueprintFeatureReference> featureList = new List<BlueprintFeatureReference>();
			featureList.Add(bp.ToReference<BlueprintFeatureReference>());
			if (AnimalCompanionSelectionDomain != null && AnimalCompanionSelectionDomain.m_AllFeatures != null)
			{
				BlueprintFeatureReference[] allFeatures = AnimalCompanionSelectionDomain.m_AllFeatures;
				foreach (BlueprintFeatureReference blueprintFeatureReference in allFeatures)
				{
					if (blueprintFeatureReference != null && blueprintFeatureReference.Get() != null && !featureList.Contains(blueprintFeatureReference))
					{
						featureList.Add(blueprintFeatureReference);
					}
				}
			}
			if (WitchFamiliarSelection != null && WitchFamiliarSelection.m_AllFeatures != null)
			{
				BlueprintFeatureReference[] allFeatures = WitchFamiliarSelection.m_AllFeatures;
				foreach (BlueprintFeatureReference blueprintFeatureReference2 in allFeatures)
				{
					if (blueprintFeatureReference2 != null && blueprintFeatureReference2.Get() != null && !featureList.Contains(blueprintFeatureReference2))
					{
						featureList.Add(blueprintFeatureReference2);
					}
				}
			}
			BlueprintFeature[] array = new BlueprintFeature[5]
			{
				IsekaiGuardians.GuardianAngelFeature,
				IsekaiGuardians.ShinigamiFeature,
				IsekaiGuardians.LoyalDemonFeature,
				IsekaiGuardians.AstralDevourerFeature,
				IsekaiGuardians.TricksterFriendFeature
			};
			foreach (BlueprintFeature blueprintFeature in array)
			{
				if (blueprintFeature != null)
				{
					BlueprintFeatureReference blueprintFeatureReference3 = blueprintFeature.ToReference<BlueprintFeatureReference>();
					if (blueprintFeatureReference3 != null && !featureList.Contains(blueprintFeatureReference3))
					{
						featureList.Add(blueprintFeatureReference3);
					}
				}
			}
			Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiPetSelection", delegate(BlueprintFeatureSelection blueprintFeatureSelection)
			{
				blueprintFeatureSelection.SetName(Main.IsekaiContext, "Pet Selection");
				blueprintFeatureSelection.SetDescription(Main.IsekaiContext, "At 1st level, you gain the service of an animal companion, familiar, or legendary guardian, using your class level as your effective druid level.");
				((BlueprintUnitFact)blueprintFeatureSelection).m_Icon = Icon_FriendToAnimals;
				blueprintFeatureSelection.IsClassFeature = true;
				blueprintFeatureSelection.Ranks = 1;
				blueprintFeatureSelection.m_AllFeatures = featureList.ToArray();
				blueprintFeatureSelection.m_Features = featureList.ToArray();
			});
			IsekaiPetProgression.PatchCompanionFeatures(featureList);
			PatchDomainAnimalProgression();
		}

		public static void AddToSelection(BlueprintFeature feature)
		{
			if (feature == null)
			{
				return;
			}
			BlueprintFeatureSelection modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiPetSelection");
			if (modBlueprint == null)
			{
				return;
			}
			BlueprintFeatureReference blueprintFeatureReference = feature.ToReference<BlueprintFeatureReference>();
			if (blueprintFeatureReference != null)
			{
				modBlueprint.m_AllFeatures = modBlueprint.m_AllFeatures ?? new BlueprintFeatureReference[0];
				modBlueprint.m_Features = modBlueprint.m_Features ?? new BlueprintFeatureReference[0];
				if (!modBlueprint.m_AllFeatures.Contains(blueprintFeatureReference))
				{
					modBlueprint.m_AllFeatures = modBlueprint.m_AllFeatures.AddToArray(blueprintFeatureReference);
					modBlueprint.m_Features = modBlueprint.m_Features.AddToArray(blueprintFeatureReference);
					IsekaiPetProgression.PatchCompanionFeatures(new BlueprintFeatureReference[1] { blueprintFeatureReference });
				}
			}
		}

		public static void PatchDomainAnimalProgression()
		{
			BlueprintProgression blueprint = BlueprintTools.GetBlueprint<BlueprintProgression>("125af359f8bc9a145968b5d8fd8159b8");
			if (blueprint != null && blueprint.m_Classes != null && !blueprint.m_Classes.Any((BlueprintProgression.ClassWithLevel c) => c.m_Class == IsekaiProtagonistClass.GetReference()))
			{
				blueprint.m_Classes = blueprint.m_Classes.AddToArray(new BlueprintProgression.ClassWithLevel
				{
					m_Class = IsekaiProtagonistClass.GetReference(),
					AdditionalLevel = 0
				});
			}
		}
	}
}
