using System.Collections.Generic;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;
using IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;
using IsekaiMod.Content.Guardians;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SoloMastery
{
	public static class OtherworldGuardianPactSelection
	{
		private static bool Added = false;

		private static readonly BlueprintFeature AnimalCompanionRank = BlueprintTools.GetBlueprint<BlueprintFeature>("1670990255e4fe948a863bafd5dbda5d");

		private static readonly Sprite Icon_Pact = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

		public static BlueprintFeatureSelection OtherworldGuardianPact { get; private set; }

		public static void Add()
		{
			if (Added)
			{
				return;
			}
			Added = true;
			List<BlueprintFeatureReference> pactFeatures = new List<BlueprintFeatureReference>();
			AddPactIfUnitExists(pactFeatures, "GuardianAngelPactFeature", "Pact: Guardian Angel", "Form an unbreakable pact with a celestial Guardian Angel. The angel immediately joins your side, scaling 1:1 to your full character level. You retain all Solo Sovereign bonuses.", "GuardianAngelCompanionUnit", GuardianProgressions.AngelProgression, null);
			AddPactIfUnitExists(pactFeatures, "ShinigamiPactFeature", "Pact: Shinigami", "Form a grim pact with an otherworldly Shinigami. The reaper immediately joins your side, scaling 1:1 to your full character level. You retain all Solo Sovereign bonuses.", "ShinigamiCompanionUnit", GuardianProgressions.ShinigamiProgression, null);
			AddPactIfUnitExists(pactFeatures, "LoyalDemonPactFeature", "Pact: Loyal Demon", "Form a devoted pact with a seductive Loyal Demon. The demon immediately joins your side, scaling 1:1 to your full character level. You retain all Solo Sovereign bonuses.", "LoyalDemonCompanionUnit", GuardianProgressions.DemonProgression, null);
			AddPactIfUnitExists(pactFeatures, "AstralDevourerPactFeature", "Pact: Astral Devourer", "Form an enigmatic cosmic pact with an Astral Devourer. The entity immediately joins your side, scaling 1:1 to your full character level. You retain all Solo Sovereign bonuses.", "AstralDevourerCompanionUnit", GuardianProgressions.DevourerProgression, null);
			AddPactIfUnitExists(pactFeatures, "TricksterFriendPactFeature", "Pact: Trickster Havoc Dragon", "Form a chaotic pact with a mischievous Trickster Havoc Dragon. The dragon immediately joins your side, scaling 1:1 to your full character level. You retain all Solo Sovereign bonuses.", "HavocDragonCompanionUnit", GuardianProgressions.DragonProgression, null);
			BlueprintUnit modBlueprint = BlueprintTools.GetModBlueprint<BlueprintUnit>(Main.IsekaiContext, "DeathsnatcherUnit");
			BlueprintProgression modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "DeathsnatcherClassProgression");
			if (modBlueprint != null)
			{
				BlueprintFeature blueprintFeature = CreatePactFeature("DeathsnatcherPactFeature", "Pact: Deathsnatcher", "Form a sovereign pact with a primeval Deathsnatcher. The apex predator immediately joins your side, scaling 1:1 to your full character level. You retain all Solo Sovereign bonuses.", modBlueprint, modBlueprint2, null);
				if (blueprintFeature != null)
				{
					pactFeatures.Add(blueprintFeature.ToReference<BlueprintFeatureReference>());
				}
			}
			AddPactIfUnitExists(pactFeatures, "TempestStarWolfPactFeature", "Pact: Tempest Star Wolf", "[Slime Exclusive] Form a primal pact with the Tempest Star Wolf. The wolf immediately joins your side, scaling 1:1 to your full character level. You retain all Solo Sovereign bonuses.", "TempestStarWolfCompanionUnit", SubclassGuardianProgressions.TempestStarWolfProgression, BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "DevourerArchetype"));
			AddPactIfUnitExists(pactFeatures, "ShadowMarshallPactFeature", "Pact: Shadow Marshall (Igris)", "[Shadow Monarch Exclusive] Awaken the legendary Shadow Marshall Igris to serve at your side, scaling 1:1 to your full character level. You retain all Solo Sovereign bonuses.", "ShadowMarshallCompanionUnit", SubclassGuardianProgressions.ShadowMarshallProgression, BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "ShadowMonarchArchetype"));
			AddPactIfUnitExists(pactFeatures, "OverlordGuardianPactFeature", "Pact: Dark Valkyrie Floor Overseer", "[Overlord Exclusive] Summon the Dark Valkyrie floor overseer to serve the Great Tomb, scaling 1:1 to your full character level. You retain all Solo Sovereign bonuses.", "OverlordGuardianCompanionUnit", SubclassGuardianProgressions.OverlordGuardianProgression, BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "OverlordArchetype"));
			AddPactIfUnitExists(pactFeatures, "DivineHeraldPactFeature", "Pact: Divine Herald (First Apostle)", "[God Emperor Exclusive] Consecrate the First Apostle as your imperial herald, scaling 1:1 to your full character level. You retain all Solo Sovereign bonuses.", "DivineHeraldCompanionUnit", SubclassGuardianProgressions.DivineHeraldProgression, BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "GodEmperorArchetype"));
			AddPactIfUnitExists(pactFeatures, "ChronoSpritePactFeature", "Pact: Chrono Sprite (Fairy of Time)", "[Hero Exclusive] Attune with the Chrono Sprite to guide your timelines, scaling 1:1 to your full character level. You retain all Solo Sovereign bonuses.", "ChronoSpriteCompanionUnit", SubclassGuardianProgressions.ChronoSpriteProgression, BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "HeroArchetype"));
			AddPactIfUnitExists(pactFeatures, "EnigmaticCoConspiratorPactFeature", "Pact: Enigmatic Co-Conspirator", "[Mastermind Exclusive] Forge an immortal contract with the enigmatic co-conspirator, scaling 1:1 to your full character level. You retain all Solo Sovereign bonuses.", "EnigmaticCoConspiratorCompanionUnit", SubclassGuardianProgressions.EnigmaticCoConspiratorProgression, BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "MastermindArchetype"));
			BlueprintArchetype modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintArchetype>(Main.IsekaiContext, "MartialGodArchetype");
			AddPactIfUnitExists(pactFeatures, "ManifestedMartialSpiritPactFeature", "Pact: Manifested Martial Spirit", "[Martial God Exclusive] Materialize your twin martial spirit into physical combat, scaling 1:1 to your full character level. You retain all Solo Sovereign bonuses.", "ManifestedMartialSpiritCompanionUnit", SubclassGuardianProgressions.ManifestedMartialSpiritProgression, modBlueprint3);
			OtherworldGuardianPact = Helpers.CreateBlueprint(Main.IsekaiContext, "OtherworldGuardianPactSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Otherworldly Guardian Pact");
				bp.SetDescription(Main.IsekaiContext, "Sacrificing an Otherworldly Superpower or Overpowered Ability, you form a soul contract with a legendary companion or guardian.\n\nDual-Power Reward Rule: You retain all benefits from Solo Sovereign (saving throws, bonus feat, and subclass awakenings) while gaining a full companion that immediately scales 1:1 to your character level!");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pact;
				bp.IsClassFeature = true;
				bp.Ranks = 1;
				bp.m_AllFeatures = pactFeatures.ToArray();
				bp.m_Features = pactFeatures.ToArray();
				bp.AddComponent(delegate(PrerequisitePet c)
				{
					c.NoCompanion = true;
				});
			});
			SpecialPowerSelection.AddToSelection(OtherworldGuardianPact);
			OverpoweredAbilitySelection.AddToSelection(OtherworldGuardianPact);
		}

		private static void AddPactIfUnitExists(List<BlueprintFeatureReference> list, string name, string displayName, string description, string unitBlueprintName, BlueprintProgression progression, BlueprintArchetype archetypePrereq)
		{
			BlueprintUnit modBlueprint = BlueprintTools.GetModBlueprint<BlueprintUnit>(Main.IsekaiContext, unitBlueprintName);
			if (modBlueprint != null)
			{
				BlueprintFeature blueprintFeature = CreatePactFeature(name, displayName, description, modBlueprint, progression, archetypePrereq);
				if (blueprintFeature != null)
				{
					list.Add(blueprintFeature.ToReference<BlueprintFeatureReference>());
				}
			}
		}

		private static BlueprintFeature CreatePactFeature(string name, string displayName, string description, BlueprintUnit petUnit, BlueprintProgression progression, BlueprintArchetype archetypePrereq)
		{
			if (petUnit == null)
			{
				return null;
			}
			return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, displayName);
				bp.SetDescription(Main.IsekaiContext, description);
				((BlueprintUnitFact)bp).m_Icon = Icon_Pact;
				bp.IsClassFeature = true;
				bp.ReapplyOnLevelUp = true;
				bp.Groups = new FeatureGroup[1] { FeatureGroup.AnimalCompanion };
				bp.AddComponent(delegate(AddPet c)
				{
					c.Type = PetType.AnimalCompanion;
					c.ProgressionType = PetProgressionType.AnimalCompanion;
					c.m_Pet = petUnit.ToReference<BlueprintUnitReference>();
					c.m_LevelRank = AnimalCompanionRank.ToReference<BlueprintFeatureReference>();
				});
				bp.AddComponent(delegate(PrerequisitePet c)
				{
					c.NoCompanion = true;
				});
				bp.AddComponent(delegate(AddFeatureOnApply c)
				{
					c.m_Feature = AnimalCompanionRank.ToReference<BlueprintFeatureReference>();
				});
				bp.AddComponent(delegate(AddFeatureOnApply c)
				{
					c.m_Feature = IsekaiPetProgression.GetCompanionProgression().ToReference<BlueprintFeatureReference>();
				});
				if (progression != null)
				{
					bp.AddComponent(delegate(AddFeatureOnApply c)
					{
						c.m_Feature = progression.ToReference<BlueprintFeatureReference>();
					});
				}
				if (archetypePrereq != null)
				{
					bp.AddComponent(delegate(PrerequisiteArchetypeLevel c)
					{
						c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
						c.m_Archetype = archetypePrereq.ToReference<BlueprintArchetypeReference>();
						c.Level = 1;
					});
				}
			});
		}
	}
}
