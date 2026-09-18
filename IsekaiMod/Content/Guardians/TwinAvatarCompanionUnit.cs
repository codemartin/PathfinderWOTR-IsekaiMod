using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;
using IsekaiMod.Content.Features.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Interaction;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Guardians
{
	internal static class TwinAvatarCompanionUnit
	{
		private static readonly BlueprintFeature AnimalCompanionRank = BlueprintTools.GetBlueprint<BlueprintFeature>("1670990255e4fe948a863bafd5dbda5d");

		private static readonly Sprite Icon_Pet = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

		private static readonly BlueprintFaction Neutrals = BlueprintTools.GetBlueprint<BlueprintFaction>("d8de50cc80eb4dc409a983991e0b77ad");

		private static readonly BlueprintUnit AstralDevaBase = BlueprintTools.GetBlueprint<BlueprintUnit>("8f3bd0ecea704277a9f2b09296a7b01e");

		public static BlueprintProgression TwinAvatarProgression { get; private set; }

		public static BlueprintUnit TwinUnit { get; private set; }

		public static BlueprintFeature TwinAvatarFeature { get; private set; }

		public static void Configure()
		{
			if (TwinAvatarFeature != null)
			{
				return;
			}
			BlueprintCharacterClassReference guardianClassRef = GuardianCompanionClass.GetReference();
			BlueprintFeature TwinMirrorLink = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinMirrorLink", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Twin Mirror Resonance");
				bp.SetDescription(Main.IsekaiContext, "The twin avatar is your mirror reflection in battle, gaining a +3 dodge bonus to AC and +2 enhancement bonus on attack rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
			});
			BlueprintFeature TwinCoordinatedStrike = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinCoordinatedStrike", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Synchronized Strike");
				bp.SetDescription(Main.IsekaiContext, "The twin strikes in perfect tandem with you. Attacks deal an additional 2d6 magic damage and bypass Damage Reduction as magic and silver.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				// WeaponEnergyDamageDice is a weapon enchantment logic and does nothing on a unit fact; AdditionalDiceOnAttack does the same job on the owner.
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 2, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Magic };
				});
			});
			BlueprintFeature TwinTranspositionFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinTranspositionFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Mirror Transposition Defense");
				bp.SetDescription(Main.IsekaiContext, "Fluidly interchanging positions across reality, the twin gains a +4 dodge bonus to AC and 2 additional attacks of opportunity per round.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AttackOfOpportunityCount;
					c.Value = 2;
				});
			});
			BlueprintFeature TwinDualityImmortal = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinDualityImmortal", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Twin Immortality Bond");
				bp.SetDescription(Main.IsekaiContext, "Connected by an undying metaphysical tether, the twin gains Fast Healing 10, Damage Reduction 10/-, and immunity to death effects and negative energy.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 10;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 10;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Death;
				});
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.NegativeEnergy;
				});
			});
			BlueprintFeature TwinEpicSynchrony = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinEpicSynchrony", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Twin Synchrony");
				bp.SetDescription(Main.IsekaiContext, "The twin achieves flawless tactical synchronization, gaining a +4 enhancement bonus on attack rolls and dealing an additional 3d6 force/magic damage on all strikes.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				// WeaponEnergyDamageDice is a weapon enchantment logic and does nothing on a unit fact; AdditionalDiceOnAttack does the same job on the owner.
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 3, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Magic };
				});
			});
			BlueprintFeature TwinEpicAbsoluteMirror = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinEpicAbsoluteMirror", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Absolute Mirror Defense");
				bp.SetDescription(Main.IsekaiContext, "The twin reflects all hostile sorcery and blades, gaining Fast Healing 15, a +6 dodge bonus to AC, and a +4 bonus on all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 15;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
			});
			BlueprintFeature TwinEpicSoulMeld = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinEpicSoulMeld", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Twin Soul Meld");
				bp.SetDescription(Main.IsekaiContext, "Sharing a single indestructible soul matrix, the twin gains Damage Reduction 15/-, Spell Resistance 35, and complete immunity to mind-affecting effects.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 15;
				});
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = 35;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting;
				});
			});
			BlueprintFeature TwinEpicDualApotheosis = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinEpicDualApotheosis", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Twin Dual Apotheosis");
				bp.SetDescription(Main.IsekaiContext, "At 40th level, the twin achieves divine avatar status. Gains a +10 inherent bonus to all ability scores, a +10 dodge bonus to AC, and 2 extra attacks when making a full attack.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
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
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 10;
				});
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 2;
				});
			});
			TwinAvatarProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinAvatarProgression", delegate(BlueprintProgression bp)
			{
				bp.SetName(Main.IsekaiContext, "Twin Avatar Progression");
				bp.SetDescription(Main.IsekaiContext, "Progression for the Twinned Summoner's mirror avatar.");
				bp.IsClassFeature = true;
				bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
				{
					new BlueprintProgression.ClassWithLevel
					{
						m_Class = guardianClassRef,
						AdditionalLevel = 0
					}
				};
				bp.LevelEntries = new LevelEntry[8]
				{
					Helpers.CreateLevelEntry(1, TwinMirrorLink),
					Helpers.CreateLevelEntry(7, TwinCoordinatedStrike),
					Helpers.CreateLevelEntry(13, TwinTranspositionFeature),
					Helpers.CreateLevelEntry(20, TwinDualityImmortal),
					Helpers.CreateLevelEntry(25, TwinEpicSynchrony),
					Helpers.CreateLevelEntry(30, TwinEpicAbsoluteMirror),
					Helpers.CreateLevelEntry(35, TwinEpicSoulMeld),
					Helpers.CreateLevelEntry(40, TwinEpicDualApotheosis)
				};
			});
			TwinUnit = AstralDevaBase.CreateCopy(Main.IsekaiContext, "TwinAvatarCompanionUnit", delegate(BlueprintUnit bp)
			{
				bp.SetLocalisedName(Main.IsekaiContext, "Twin Avatar");
				bp.ComponentsArray = new BlueprintComponent[0];
				bp.AddComponent(delegate(AddClassLevels c)
				{
					c.m_CharacterClass = guardianClassRef;
					c.RaceStat = StatType.Strength;
					c.LevelsStat = StatType.Strength;
					c.Skills = new StatType[0];
					c.DoNotApplyAutomatically = false;
					c.m_MemorizeSpells = new BlueprintAbilityReference[0];
					c.m_SelectSpells = new BlueprintAbilityReference[0];
				});
				bp.AddComponent<AllowDyingCondition>();
				bp.AddComponent<AddResurrectOnRest>();
				bp.AddComponent(delegate(BarkOnClick c)
				{
					c.Bark = Helpers.CreateString(Main.IsekaiContext, "TwinAvatar.ClickBark", "I am your twin reflection. Where you strike, I strike.");
					c.Cooldown = 3f;
				});
				bp.m_Faction = Neutrals.ToReference<BlueprintFactionReference>();
				bp.Alignment = Alignment.TrueNeutral;
				bp.Strength = 18;
				bp.Dexterity = 18;
				bp.Constitution = 16;
				bp.Intelligence = 14;
				bp.Wisdom = 14;
				bp.Charisma = 18;
				bp.Speed = new Feet(30f);
				bp.MaxHP = 0;
				bp.m_AddFacts = new BlueprintUnitFactReference[1] { TwinAvatarProgression.ToReference<BlueprintUnitFactReference>() };
			});
			TwinAvatarFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinAvatarFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Twin Avatar Manifestation");
				bp.SetDescription(Main.IsekaiContext, "You summon your metaphysical Twin Avatar into the physical realm. The twin levels as a Guardian Companion, sharing your destiny, traits, and combat mastery.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
				bp.IsClassFeature = true;
				bp.ReapplyOnLevelUp = true;
				bp.Groups = new FeatureGroup[1] { FeatureGroup.AnimalCompanion };
				bp.AddComponent(delegate(AddPet c)
				{
					c.Type = PetType.AnimalCompanion;
					c.ProgressionType = PetProgressionType.AnimalCompanion;
					c.m_Pet = TwinUnit.ToReference<BlueprintUnitReference>();
					c.m_LevelRank = AnimalCompanionRank.ToReference<BlueprintFeatureReference>();
				});
				bp.AddComponent(delegate(AddFeatureOnApply c)
				{
					c.m_Feature = AnimalCompanionRank.ToReference<BlueprintFeatureReference>();
				});
				bp.AddComponent(delegate(AddFeatureOnApply c)
				{
					c.m_Feature = IsekaiPetProgression.GetCompanionProgression().ToReference<BlueprintFeatureReference>();
				});
				bp.AddComponent(delegate(AddFeatureOnApply c)
				{
					c.m_Feature = TwinAvatarProgression.ToReference<BlueprintFeatureReference>();
				});
			});
		}
	}
}
