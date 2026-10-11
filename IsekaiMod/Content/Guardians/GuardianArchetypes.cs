using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Guardians;

internal static class GuardianArchetypes
{
	private static bool Added = false;

	private static readonly BlueprintFeature SimpleWeaponProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("e70ecf1ed95ca2f40b754f1adb22bbdd");

	private static readonly BlueprintFeature MartialWeaponProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("203992ef5b35c864390b4e4a1e200629");

	private static readonly BlueprintFeature ShieldsProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("cb8686e7357a68c42bdd9d4e65334633");

	private static readonly Sprite Icon_Pet = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

	public static void Add()
	{
		if (!Added)
		{
			Added = true;
			BlueprintCharacterClass blueprintCharacterClass = GuardianCompanionClass.Get();
			if (blueprintCharacterClass == null)
			{
				Main.IsekaiContext.Logger.LogError("GuardianArchetypes: GuardianCompanionClass not found!");
				return;
			}
			BlueprintCharacterClassReference classRef = blueprintCharacterClass.ToReference<BlueprintCharacterClassReference>();
			BlueprintArchetype blueprintArchetype = CreateArchetype("GuardianAngelArchetype", "Guardian Angel", "A celestial of the Upper Planes who came down to be your shield and your sword. The Guardian Angel fights with weapons and shields (simple and martial weapons, shields) and grows through celestial milestones: Celestial Resilience, Smite Evil, Angelic Wings, Holy Strike, Aura of Menace, fast healing and, at the last, Archangel Ascension.", classRef, SimpleWeaponProficiency, MartialWeaponProficiency, ShieldsProficiency);
			BlueprintArchetype blueprintArchetype2 = CreateArchetype("ShinigamiArchetype", "Shinigami", "An otherworldly reaper veiled in darkness, who harvests your enemies with a soul-severing scythe and draws strength from every death around you. It is proficient with simple and martial weapons and grows through reaper milestones: Reaper's Shroud, Ghost Touch, Soul-Severing Strike, Ethereal Form and Sovereign of the Dead.", classRef, SimpleWeaponProficiency, MartialWeaponProficiency);
			BlueprintArchetype blueprintArchetype3 = CreateArchetype("LoyalDemonArchetype", "Loyal Demon", "A demon wholly and inexplicably devoted to you: an archer with bewitching charms and abyssal agility. She is proficient with simple and martial weapons and grows through abyssal milestones: Abyssal Resilience, Bat Wings, Profane Strike, Bewitching Allure and Queen's Devotion.", classRef, SimpleWeaponProficiency, MartialWeaponProficiency);
			BlueprintArchetype blueprintArchetype4 = CreateArchetype("AstralDevourerArchetype", "Astral Devourer", "A grotesque thing of pure planar hunger caged in ethereal ribs, which eats life energy and drains the magic of everything near it. It fights with its claws alone and grows through void milestones: the Void Carapace (spell resistance 11 + its level), Ethereal Claws and the Void Singularity.", classRef);
			BlueprintArchetype blueprintArchetype5 = CreateArchetype("HavocDragonArchetype", "Havoc Dragon", "A tiny floating dragon of vibrant chaos, far too small and far too excitable to ride, who flits across the battlefield casting chaotic spells and confounding your enemies. It fights with bite, breath and pranks and grows through trickster milestones: Havoc Drake Essence, Prismatic Havoc Breath, Prankster's Slip, Confounding Prank and Avatar of Pandemonium.", classRef);
			BlueprintArchetype blueprintArchetype6 = CreateArchetype("TempestStarWolfArchetype", "Tempest Star Wolf", "The Slime's partner: a silver wolf whose fur crackles with azure lightning and shadow mist, born from the storm an apex predator left behind. It hunts with bite and trip, runs pack tactics with its slime sovereign, and grows through storm-pack milestones.", classRef);
			BlueprintArchetype blueprintArchetype7 = CreateArchetype("ShadowMarshallArchetype", "Shadow Marshal", "The Shadow Monarch's partner: Igris, the red-plumed dread knight who marshals the vanguard of the shadow army and guards his monarch in silence. He fights with a two-handed greatsword (simple and martial weapons, shields) and grows through shadow vanguard commander milestones.", classRef, SimpleWeaponProficiency, MartialWeaponProficiency, ShieldsProficiency);
			BlueprintArchetype blueprintArchetype8 = CreateArchetype("OverlordValkyrieArchetype", "Overlord Valkyrie", "The Overlord's partner: the supreme floor overseer of the Great Tomb, a winged valkyrie in jet-black armor who holds all lesser life in contempt and lives only to serve the Supreme Being. She fights with spear and shield (simple and martial weapons, shields) and grows through the overseer's aegis milestones.", classRef, SimpleWeaponProficiency, MartialWeaponProficiency, ShieldsProficiency);
			BlueprintArchetype blueprintArchetype9 = CreateArchetype("DivineHeraldArchetype", "Divine Herald", "The God Emperor's partner: the First Apostle of the Imperial Cult, a radiant seraph crowned with halos, who speaks with imperial authority and turns solar brilliance on heresy. The herald fights with weapons and shields (simple and martial weapons, shields) and grows through milestones of sanctified radiance.", classRef, SimpleWeaponProficiency, MartialWeaponProficiency, ShieldsProficiency);
			BlueprintArchetype blueprintArchetype10 = CreateArchetype("ChronoSpriteArchetype", "Chrono Sprite", "The Hero's partner: a floating orb of golden fairy light and temporal magic who never stops giving advice. She carries no weapon; she speeds up her allies' timelines instead, and grows through temporal pulse and timeline evasion milestones.", classRef);
			BlueprintArchetype blueprintArchetype11 = CreateArchetype("CoConspiratorArchetype", "Enigmatic Co-Conspirator", "The Mastermind's partner: an immortal advisor bound by an otherworldly contract, who watches the chessboard of fate with unreadable calm, shields the Mastermind's mind and demands eccentric snacks. She fights with light blades and bows (simple and martial weapons) and grows through cognitive link and command protocol milestones.", classRef, SimpleWeaponProficiency, MartialWeaponProficiency);
			BlueprintArchetype blueprintArchetype12 = CreateArchetype("MartialSpiritArchetype", "Manifested Martial Spirit", "The Martial God's partner: a translucent battle spirit that mirrors its master's forms, flowing between bare fists, blades and parries. It is proficient with simple and martial weapons and grows through twin flow and reactive parry milestones.", classRef, SimpleWeaponProficiency, MartialWeaponProficiency);
			blueprintCharacterClass.m_Archetypes = new BlueprintArchetypeReference[12]
			{
				blueprintArchetype?.ToReference<BlueprintArchetypeReference>(),
				blueprintArchetype2?.ToReference<BlueprintArchetypeReference>(),
				blueprintArchetype3?.ToReference<BlueprintArchetypeReference>(),
				blueprintArchetype4?.ToReference<BlueprintArchetypeReference>(),
				blueprintArchetype5?.ToReference<BlueprintArchetypeReference>(),
				blueprintArchetype6?.ToReference<BlueprintArchetypeReference>(),
				blueprintArchetype7?.ToReference<BlueprintArchetypeReference>(),
				blueprintArchetype8?.ToReference<BlueprintArchetypeReference>(),
				blueprintArchetype9?.ToReference<BlueprintArchetypeReference>(),
				blueprintArchetype10?.ToReference<BlueprintArchetypeReference>(),
				blueprintArchetype11?.ToReference<BlueprintArchetypeReference>(),
				blueprintArchetype12?.ToReference<BlueprintArchetypeReference>()
			};
			AddArchetypeToUnit("GuardianAngelCompanionUnit", blueprintArchetype, SimpleWeaponProficiency, MartialWeaponProficiency, ShieldsProficiency);
			AddArchetypeToUnit("ShinigamiCompanionUnit", blueprintArchetype2, SimpleWeaponProficiency, MartialWeaponProficiency);
			AddArchetypeToUnit("LoyalDemonCompanionUnit", blueprintArchetype3, SimpleWeaponProficiency, MartialWeaponProficiency);
			AddArchetypeToUnit("AstralDevourerCompanionUnit", blueprintArchetype4);
			AddArchetypeToUnit("HavocDragonCompanionUnit", blueprintArchetype5);
			AddArchetypeToUnit("TempestStarWolfCompanionUnit", blueprintArchetype6);
			AddArchetypeToUnit("ShadowMarshallCompanionUnit", blueprintArchetype7, SimpleWeaponProficiency, MartialWeaponProficiency, ShieldsProficiency);
			AddArchetypeToUnit("OverlordGuardianCompanionUnit", blueprintArchetype8, SimpleWeaponProficiency, MartialWeaponProficiency, ShieldsProficiency);
			AddArchetypeToUnit("DivineHeraldCompanionUnit", blueprintArchetype9, SimpleWeaponProficiency, MartialWeaponProficiency, ShieldsProficiency);
			AddArchetypeToUnit("ChronoSpriteCompanionUnit", blueprintArchetype10);
			AddArchetypeToUnit("EnigmaticCoConspiratorCompanionUnit", blueprintArchetype11, SimpleWeaponProficiency, MartialWeaponProficiency);
			AddArchetypeToUnit("ManifestedMartialSpiritCompanionUnit", blueprintArchetype12, SimpleWeaponProficiency, MartialWeaponProficiency);
		}
	}

	private static BlueprintArchetype CreateArchetype(string blueprintName, string displayName, string description, BlueprintCharacterClassReference classRef, params BlueprintFeature[] startingProficiencies)
	{
		return Helpers.CreateBlueprint(Main.IsekaiContext, blueprintName, delegate(BlueprintArchetype bp)
		{
			bp.LocalizedName = Helpers.CreateString(Main.IsekaiContext, blueprintName + ".Name", displayName);
			bp.LocalizedDescription = Helpers.CreateString(Main.IsekaiContext, blueprintName + ".Description", description);
			bp.LocalizedDescriptionShort = bp.LocalizedDescription;
			bp.m_Icon = Icon_Pet;
			bp.RemoveFeatures = new LevelEntry[0];
			List<LevelEntry> list = new List<LevelEntry>
			{
				new LevelEntry
				{
					Level = 1,
					m_Features = new List<BlueprintFeatureBaseReference>()
				}
			};
			if (startingProficiencies != null && startingProficiencies.Length != 0)
			{
				LevelEntry levelEntry = list.FirstOrDefault((LevelEntry le) => le.Level == 1);
				if (levelEntry == null)
				{
					levelEntry = new LevelEntry
					{
						Level = 1,
						m_Features = new List<BlueprintFeatureBaseReference>()
					};
					list.Add(levelEntry);
				}
				BlueprintFeature[] array = startingProficiencies;
				foreach (BlueprintFeature blueprintFeature in array)
				{
					if (blueprintFeature != null)
					{
						BlueprintFeatureBaseReference item = blueprintFeature.ToReference<BlueprintFeatureBaseReference>();
						if (!levelEntry.m_Features.Contains(item))
						{
							levelEntry.m_Features.Add(item);
						}
					}
				}
			}
			bp.AddFeatures = list.OrderBy((LevelEntry le) => le.Level).ToArray();
		});
	}

	private static void AddArchetypeToUnit(string unitBlueprintName, BlueprintArchetype archetype, params BlueprintFeature[] proficiencies)
	{
		BlueprintUnit modBlueprint = BlueprintTools.GetModBlueprint<BlueprintUnit>(Main.IsekaiContext, unitBlueprintName);
		if (modBlueprint == null)
		{
			Main.IsekaiContext.Logger.LogWarning("GuardianArchetypes: Unit " + unitBlueprintName + " not found, skipping.");
			return;
		}
		if (archetype != null)
		{
			AddClassLevels component = modBlueprint.GetComponent<AddClassLevels>();
			if (component != null)
			{
				component.m_Archetypes = new BlueprintArchetypeReference[1] { archetype.ToReference<BlueprintArchetypeReference>() };
			}
		}
		if (proficiencies == null || proficiencies.Length == 0)
		{
			return;
		}
		BlueprintUnitFactReference[] array = modBlueprint.m_AddFacts ?? new BlueprintUnitFactReference[0];
		foreach (BlueprintFeature blueprintFeature in proficiencies)
		{
			if (blueprintFeature != null)
			{
				array = array.AppendToArray(blueprintFeature.ToReference<BlueprintUnitFactReference>());
			}
		}
		modBlueprint.m_AddFacts = array;
	}
}
