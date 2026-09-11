using IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal static class EpicFeats
	{
		private static readonly Sprite Icon_Feat = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e45ab30f49215054e83b4ea12165409f"))?.m_Icon;

		private static readonly Sprite Icon_Sword = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("779179912e6c6fe458fa4cfb90d96e10"))?.m_Icon ?? Icon_Feat;

		private static readonly Sprite Icon_Shield = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("9c0fa9b438ada3f43864be8dd8b3e741"))?.m_Icon ?? Icon_Feat;

		private static readonly Sprite Icon_Spell = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("faf473e3a977fd4428cd3f1a526346d2"))?.m_Icon ?? Icon_Feat;

		public static void Add()
		{
			RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "EpicWeaponMastery", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Weapon Mastery");
				bp.SetDescription(Main.IsekaiContext, "Your mastery of weaponry reaches transcendent heights. You gain a +4 competence bonus to attack rolls with all weapons, and your critical strike multiplier increases by 1 (e.g. x2 becomes x3).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
			}));
			RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "EpicDevastatingCritical", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Devastating Critical");
				bp.SetDescription(Main.IsekaiContext, "When you score a critical hit, your weapon shatters bone and armor, dealing an additional 2d10 direct damage and forcing the target to succeed at a Fortitude save (DC = 10 + 1/2 character level + Strength modifier) or be Paralyzed for 1 round.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 6;
				});
			}));
			RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "InfiniteCombatReflexes", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Infinite Combat Reflexes");
				bp.SetDescription(Main.IsekaiContext, "You can make an unlimited number of attacks of opportunity each round, and gain a +4 circumstance bonus to attack rolls on all attacks of opportunity.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Circumstance;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
			}));
			RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "EpicOverwhelmingPower", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Overwhelming Power");
				bp.SetDescription(Main.IsekaiContext, "The sheer kinetic force of your strikes ignores physical resistance. Your melee weapon attacks bypass all forms of damage reduction, including DR/Epic and DR/-.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 8;
				});
			}));
			RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "EpicRapidShot", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Rapid Shot");
				bp.SetDescription(Main.IsekaiContext, "You fire with superhuman speed. When making a full attack with a ranged weapon, you gain 2 additional attacks at your highest base attack bonus with no attack penalty.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 2;
					c.Haste = true;
				});
			}));
			RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "EpicDistantShot", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Distant Shot");
				bp.SetDescription(Main.IsekaiContext, "Your arrows and bolts transcend spatial limits. Ranged attacks ignore range increment penalties, point-blank range penalties, and target concealment miss chances.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
				bp.IsClassFeature = true;
				bp.AddComponent<IgnoreConcealment>();
			}));
			RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "TranscendentToughness", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Transcendent Toughness");
				bp.SetDescription(Main.IsekaiContext, "Your body attains immortal vitality. You gain +100 maximum hit points, damage reduction 10/-, and Fast Healing 5.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.HitPoints;
					c.Value = 100;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 5;
				});
			}));
			RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "UnyieldingMind", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Unyielding Mind");
				bp.SetDescription(Main.IsekaiContext, "Your psyche is an impenetrable fortress. You gain permanent immunity to all mind-affecting effects, compulsions, feeblemind, and madness, as well as a +4 bonus to Will saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
			}));
			RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "PreternaturalEvasion", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Preternatural Evasion");
				bp.SetDescription(Main.IsekaiContext, "You instinctively slip between the atoms of incoming danger. You gain Improved Evasion and a 50% displacement miss chance against all incoming attacks.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent<ImprovedEvasion>();
				bp.AddComponent(delegate(AddConcealment c)
				{
					c.Concealment = Concealment.Total;
					c.Descriptor = ConcealmentDescriptor.Displacement;
				});
			}));
			RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "EpicArmorMastery", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Armor Mastery");
				bp.SetDescription(Main.IsekaiContext, "Armor fits you like a second skin. Armor check penalty is reduced to 0, maximum Dexterity bonus to AC is increased by +10, and you gain an additional +6 shield and armor bonus to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.ArmorEnhancement;
					c.Stat = StatType.AC;
					c.Value = 6;
				});
			}));
			RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "EpicSpellPenetration", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Spell Penetration");
				bp.SetDescription(Main.IsekaiContext, "Your magical incantations tear effortlessly through magical abjurations. You gain a +6 bonus to caster level checks to overcome spell resistance. Stacks with all other Spell Penetration feats.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Spell;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(SpellPenetrationBonus c)
				{
					c.Value = 6;
				});
			}));
			RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "EpicMetamagicApotheosis", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Metamagic Apotheosis");
				bp.SetDescription(Main.IsekaiContext, "Metamagic flows naturally through your conduits without expanding spell slots. Completely removes the spell slot level increase for Empower Spell and Extend Spell, and grants +2 to all spell save DCs.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Spell;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 2;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
			}));
			RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "EpicSpellFocus", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Spell Focus");
				bp.SetDescription(Main.IsekaiContext, "Your mastery of all spell schools reaches absolute supremacy. The saving throw DC of all your spells increases by +3 across all schools. Stacks with all Spell Focus feats.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Spell;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 3;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
			}));
			RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "EpicMultispell", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Multispell");
				bp.SetDescription(Main.IsekaiContext, "You can weave spells with dual-threaded casting speed, gaining the ability to cast an extra quickened or swift action spell each round.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Spell;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Initiative;
					c.Value = 6;
				});
			}));
			BlueprintBuff EpicGrandSummonerBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "EpicGrandSummonerBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Grand Summoner");
				bp.SetDescription(Main.IsekaiContext, "Summoned creatures and companions gain a +6 enhancement bonus to all ability scores and a +4 natural armor bonus to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Spell;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Strength;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Dexterity;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Constitution;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Intelligence;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Wisdom;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Charisma;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
			});
			RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "EpicGrandSummoner", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Grand Summoner");
				bp.SetDescription(Main.IsekaiContext, "Your summoned creatures and companions are imbued with epic planar resonance: they gain a +6 enhancement bonus to all ability scores, maximum hit points per hit die, and a +4 natural armor bonus.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Spell;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(BuffExtraEffects c)
				{
					c.m_CheckedBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("706c182e86d9be848b59ddccca73d13e")?.ToReference<BlueprintBuffReference>();
					c.m_ExtraEffectBuff = EpicGrandSummonerBuff.ToReference<BlueprintBuffReference>();
				});
			}));
			RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "EpicFlurryOfBlades", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Epic Flurry of Blades");
				bp.SetDescription(Main.IsekaiContext, "You wield two weapons with unmatched grace and coordination: you suffer no attack penalties when dual wielding, your off-hand weapon adds your full Strength or Dexterity modifier to damage, and you gain 1 additional off-hand attack.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
					c.Haste = true;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 4;
				});
			}));
			static void RegisterEpicFeat(BlueprintFeature feat)
			{
				feat.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 21;
				});
				SpecialPowerSelection.AddToSelection(feat);
				BlueprintFeatureSelection modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiBonusFeatSelection");
				if (modBlueprint != null)
				{
					modBlueprint.m_Features = modBlueprint.m_Features.AppendToArray(feat.ToReference<BlueprintFeatureReference>());
					modBlueprint.m_AllFeatures = modBlueprint.m_AllFeatures.AppendToArray(feat.ToReference<BlueprintFeatureReference>());
				}
			}
		}
	}
}
