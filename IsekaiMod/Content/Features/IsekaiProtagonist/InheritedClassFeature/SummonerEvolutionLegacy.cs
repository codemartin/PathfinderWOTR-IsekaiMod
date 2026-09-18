using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature
{
	internal class SummonerEvolutionLegacy
	{
		private static readonly Sprite Icon_Pet = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

		private static BlueprintProgression prog;

		public static void Configure()
		{
			SummonerEvolutionSelection.Configure();
			BlueprintFeatureSelection evolutionSelection = SummonerEvolutionSelection.Get();
			BlueprintFeature SummonerLifeLink = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonerLifeLink", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Summoner Bond: Life Link");
				bp.SetDescription(Main.IsekaiContext, "A mystical psychic bond connects you to your companions and summons. You gain Fast Healing 5 and a +2 morale bonus on Fortitude saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
			});
			BlueprintFeature SummonerShieldAlly = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonerShieldAlly", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Summoner Bond: Shield Ally");
				bp.SetDescription(Main.IsekaiContext, "Whenever you fight alongside your allies, you gain a +2 shield bonus to Armor Class and a +2 circumstance bonus on all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Shield;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Circumstance;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Circumstance;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Circumstance;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
			});
			BlueprintFeature SummonerAspect = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonerAspect", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Summoner Aspect: Mutual Resonance");
				bp.SetDescription(Main.IsekaiContext, "You channel the primordial vigor of your summons, gaining a +2 inherent bonus to Strength, Dexterity, and Constitution.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Strength;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Dexterity;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Constitution;
					c.Value = 2;
				});
			});
			BlueprintFeature SummonerGreaterShieldAlly = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonerGreaterShieldAlly", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Summoner Bond: Greater Shield Ally");
				bp.SetDescription(Main.IsekaiContext, "Your defensive resonance deepens, increasing your shield bonus to AC by +2 and circumstance bonus to saving throws by +2.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Shield;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Circumstance;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Circumstance;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Circumstance;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
			});
			BlueprintFeature SummonerLifeBond = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonerLifeBond", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Summoner Bond: Life Bond");
				bp.SetDescription(Main.IsekaiContext, "Your life force intertwines with the eternal astral plane. You gain immunity to death effects and negative energy damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Death;
				});
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.NegativeEnergy;
				});
			});
			BlueprintFeature SummonerTwinAspect = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonerTwinAspect", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Summoner Aspect: Supreme Synthesis");
				bp.SetDescription(Main.IsekaiContext, "You attain absolute physical and mental harmony with the astral realm, gaining a +4 inherent bonus to all ability scores.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Dexterity;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Constitution;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Intelligence;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Wisdom;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Charisma;
					c.Value = 4;
				});
			});
			BlueprintFeature SummonerTranscendentalEvolution = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonerTranscendentalEvolution", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Transcendental Evolution");
				bp.SetDescription(Main.IsekaiContext, "At 28th level, your evolutions transcend mortal flesh. You and your companions gain a +6 dodge bonus to AC and Damage Reduction 10/-.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddFactsToPet c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { bp.ToReference<BlueprintUnitFactReference>() };
					c.m_AllPets = true;
				});
			});
			BlueprintFeature SummonerApexSymbiosis = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonerApexSymbiosis", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Apex Symbiosis");
				bp.SetDescription(Main.IsekaiContext, "At 36th level, your symbiotic resonance grants Fast Healing 15, a +4 morale bonus to all attack and damage rolls, and a +4 morale bonus on all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 15;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddFactsToPet c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { bp.ToReference<BlueprintUnitFactReference>() };
					c.m_AllPets = true;
				});
			});
			BlueprintFeature SummonerTrueEidolonApotheosis = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonerTrueEidolonApotheosis", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "True Eidolon Apotheosis");
				bp.SetDescription(Main.IsekaiContext, "At 40th level, you reach the absolute pinnacle of summoner evolution. You and your companions gain a +10 inherent bonus to all ability scores, +10 natural armor bonus to AC, and an extra attack when making a full attack.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Strength;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Dexterity;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Constitution;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Intelligence;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Wisdom;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Charisma;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 10;
				});
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
				});
				bp.AddComponent(delegate(AddFactsToPet c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { bp.ToReference<BlueprintUnitFactReference>() };
					c.m_AllPets = true;
				});
			});
			prog = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonerEvolutionLegacy", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Summoner Legacy - Evolution Master");
				bp.SetDescription(Main.IsekaiContext, "Forging a supreme bond between caller and eidolon, you command the endless mutable power of astral evolution. Through modular mutations and symbiotic life-links, you and your summoned guardians evolve to conquer the multiverse.");
				bp.GiveFeaturesForPreviousLevels = false;
				bp.IsClassFeature = true;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = IsekaiProtagonistClass.GetReference(),
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = new LevelEntry[11]
				{
					Helpers.CreateLevelEntry(1, SummonerLifeLink, evolutionSelection),
					Helpers.CreateLevelEntry(4, SummonerShieldAlly, evolutionSelection),
					Helpers.CreateLevelEntry(8, SummonerAspect, evolutionSelection),
					Helpers.CreateLevelEntry(12, SummonerGreaterShieldAlly, evolutionSelection),
					Helpers.CreateLevelEntry(16, SummonerLifeBond, evolutionSelection),
					Helpers.CreateLevelEntry(20, SummonerTwinAspect, evolutionSelection),
					Helpers.CreateLevelEntry(24, evolutionSelection),
					Helpers.CreateLevelEntry(28, SummonerTranscendentalEvolution, evolutionSelection),
					Helpers.CreateLevelEntry(32, evolutionSelection),
					Helpers.CreateLevelEntry(36, SummonerApexSymbiosis, evolutionSelection),
					Helpers.CreateLevelEntry(40, SummonerTrueEidolonApotheosis, evolutionSelection)
				};
				bp.UIGroups = new UIGroup[1] { Helpers.CreateUIGroup(SummonerLifeLink, SummonerShieldAlly, SummonerAspect, SummonerGreaterShieldAlly, SummonerLifeBond, SummonerTwinAspect, SummonerTranscendentalEvolution, SummonerApexSymbiosis, SummonerTrueEidolonApotheosis) };
			});
			LegacySelection.RegisterForFeat(prog);
			LegacySelection.Register(prog);
			MartialGodLegacySelection.Register(prog);
			GodEmperorLegacySelection.Register(prog);
			HeroLegacySelection.Register(prog);
			MastermindLegacySelection.Register(prog);
			OverlordLegacySelection.Register(prog);
			ShadowMonarchLegacySelection.Register(prog);
			DevourerLegacySelection.Register(prog);
		}

		public static void PatchProgression()
		{
		}

		public static BlueprintProgression Get()
		{
			if (prog != null)
			{
				return prog;
			}
			return BlueprintTools.GetModBlueprint<BlueprintProgression>(Main.IsekaiContext, "SummonerEvolutionLegacy");
		}
	}
}
