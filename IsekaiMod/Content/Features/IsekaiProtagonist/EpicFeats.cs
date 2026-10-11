using IsekaiMod.Components;
using IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;
using IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist;

internal static class EpicFeats
{
	private static readonly Sprite Icon_Feat = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e6e1a24ce33454342b7053889391642c"))?.m_Icon;

	private static readonly Sprite Icon_Sword = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("f5f500d6a2a39fc4181af32ad79af488"))?.m_Icon ?? Icon_Feat;

	private static readonly Sprite Icon_Shield = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("9c0fa9b438ada3f43864be8dd8b3e741"))?.m_Icon ?? Icon_Feat;

	private static readonly Sprite Icon_Spell = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("faf473e3a977fd4428cd3f1a526346d2"))?.m_Icon ?? Icon_Feat;

	private const string ParalyzedBuff = "af1e2d232ebbb334aaf25e2a46a92591";

	public static void Add()
	{
		RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "EpicWeaponMastery", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Epic Weapon Mastery");
			bp.SetDescription(Main.IsekaiContext, "Your mastery of weaponry reaches transcendent heights. You gain a +4 untyped bonus to attack rolls that stacks with every other source, and the critical multiplier of your melee and ranged weapons increases by 1 (e.g. x2 becomes x3).");
			((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AttackTypeCriticalMultiplierIncrease c)
			{
				c.Type = WeaponRangeType.Normal;
				c.AdditionalMultiplier = 1;
			});
		}));
		BlueprintUnitProperty DevastatingCriticalDC = Helpers.CreateBlueprint(Main.IsekaiContext, "EpicDevastatingCriticalDCProperty", delegate(BlueprintUnitProperty bp)
		{
			bp.BaseValue = 10;
			bp.OperationOnComponents = BlueprintUnitProperty.MathOperation.Sum;
			bp.AddComponent(delegate(SimplePropertyGetter c)
			{
				c.Property = UnitProperty.Level;
				c.Settings = new PropertySettings
				{
					m_Progression = PropertySettings.Progression.Div2
				};
			});
			bp.AddComponent(delegate(SimplePropertyGetter c)
			{
				c.Property = UnitProperty.StatBonusStrength;
			});
		});
		RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "EpicDevastatingCritical", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Epic Devastating Critical");
			bp.SetDescription(Main.IsekaiContext, "When you confirm a critical hit with a weapon, your weapon shatters bone and armor, dealing an additional 2d10 direct damage (not multiplied by the critical hit) and forcing the target to succeed at a Fortitude save (DC = 10 + 1/2 character level + Strength modifier) or be Paralyzed for 1 round.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
			{
				c.OnlyHit = true;
				c.CriticalHit = true;
				c.Action = Helpers.CreateActionList(new ContextActionDealDamage
				{
					m_Type = ContextActionDealDamage.Type.Damage,
					DamageType = new DamageTypeDescription
					{
						Type = DamageType.Direct,
						Physical = new DamageTypeDescription.PhysicalData(),
						Common = new DamageTypeDescription.CommomData()
					},
					Duration = Values.Duration.Zero,
					Value = new ContextDiceValue
					{
						DiceType = DiceType.D10,
						DiceCountValue = 2,
						BonusValue = 0
					},
					IgnoreCritical = true
				}, new ContextActionSavingThrow
				{
					Type = SavingThrowType.Fortitude,
					m_ConditionalDCIncrease = new ContextActionSavingThrow.ConditionalDCIncrease[0],
					HasCustomDC = true,
					CustomDC = Values.CreateContextCasterCustomPropertyValue(DevastatingCriticalDC),
					Actions = ActionFlow.DoSingle(delegate(ContextActionConditionalSaved s)
					{
						s.Succeed = ActionFlow.DoNothing();
						s.Failed = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
						{
							b.m_Buff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("af1e2d232ebbb334aaf25e2a46a92591");
							b.DurationValue = Values.Duration.OneRound;
						});
					})
				});
			});
		}));
		RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "InfiniteCombatReflexes", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Infinite Combat Reflexes");
			bp.SetDescription(Main.IsekaiContext, "You can make 10 additional attacks of opportunity each round (still no more than one against the same creature), and gain a +4 circumstance bonus to attack rolls on attacks of opportunity.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AttackOfOpportunityCount;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AttackOfOpportunityAttackBonus c)
			{
				c.Descriptor = ModifierDescriptor.Circumstance;
				c.Bonus = 4;
			});
		}));
		RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "EpicOverwhelmingPower", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Epic Overwhelming Power");
			bp.SetDescription(Main.IsekaiContext, "The sheer kinetic force of your strikes ignores physical resistance. Your weapon attacks, melee and ranged, bypass all forms of damage reduction, including DR/Epic and DR/-.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
			bp.IsClassFeature = true;
			bp.AddComponent<IgnoreDamageReductionOnAttack>();
		}));
		RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "EpicRapidShot", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Epic Rapid Shot");
			bp.SetDescription(Main.IsekaiContext, "You fire with superhuman speed. When making a full attack with a ranged weapon, you gain 2 additional attacks at your highest base attack bonus with no attack penalty. These stack with haste and Rapid Shot.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(BuffExtraAttackWeaponSpecific c)
			{
				c.RangeType = WeaponRangeType.Ranged;
			});
			bp.AddComponent(delegate(BuffExtraAttackWeaponSpecific c)
			{
				c.RangeType = WeaponRangeType.Ranged;
			});
		}));
		RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "EpicDistantShot", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Epic Distant Shot");
			bp.SetDescription(Main.IsekaiContext, "Your arrows and bolts transcend spatial limits. You ignore the penalty for shooting into melee, and your attacks ignore concealment miss chances.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
			bp.IsClassFeature = true;
			bp.AddComponent<PreciseShot>();
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
				c.Bonus = 0;
			});
		}));
		RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "UnyieldingMind", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Unyielding Mind");
			bp.SetDescription(Main.IsekaiContext, "Your psyche is an impenetrable fortress. You gain permanent immunity to harmful mind-affecting effects (charm, compulsion, confusion, daze, fear, sleep and harmful emotion effects, which covers feeblemind and insanity), as well as a +4 bonus to Will saving throws. Morale and inspiration effects from your allies still affect you.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Compulsion | SpellDescriptor.Charm | SpellDescriptor.Daze | SpellDescriptor.Shaken | SpellDescriptor.Frightened | SpellDescriptor.Confusion | SpellDescriptor.Sleep | SpellDescriptor.NegativeEmotion;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Compulsion | SpellDescriptor.Charm | SpellDescriptor.Daze | SpellDescriptor.Shaken | SpellDescriptor.Frightened | SpellDescriptor.Confusion | SpellDescriptor.Sleep | SpellDescriptor.NegativeEmotion;
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
			bp.SetDescription(Main.IsekaiContext, "You instinctively slip between the atoms of incoming danger. You gain Evasion and Improved Evasion (no damage on a successful Reflex save, half on a failed one) and a 50% displacement miss chance against all incoming attacks.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(Evasion c)
			{
				c.SavingThrow = SavingThrowType.Reflex;
			});
			bp.AddComponent(delegate(ImprovedEvasion c)
			{
				c.SavingThrow = SavingThrowType.Reflex;
			});
			bp.AddComponent(delegate(AddConcealment c)
			{
				c.Concealment = Concealment.Total;
				c.Descriptor = ConcealmentDescriptor.Displacement;
			});
		}));
		RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "EpicArmorMastery", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Epic Armor Mastery");
			bp.SetDescription(Main.IsekaiContext, "Armor fits you like a second skin. The armor check penalty of your armor and shield is reduced to 0, the maximum Dexterity bonus of your armor increases by +10, and you gain a +6 armor focus bonus to AC that stacks with your armor (it does not apply to touch attacks).");
			((BlueprintUnitFact)bp).m_Icon = Icon_Shield;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(ArmorCheckPenaltyIncrease c)
			{
				c.Bonus = 20;
			});
			ArmorProficiencyGroup[] array = new ArmorProficiencyGroup[4]
			{
				ArmorProficiencyGroup.Buckler,
				ArmorProficiencyGroup.LightShield,
				ArmorProficiencyGroup.HeavyShield,
				ArmorProficiencyGroup.TowerShield
			};
			foreach (ArmorProficiencyGroup shield in array)
			{
				bp.AddComponent(delegate(ArmorCheckPenaltyIncrease c)
				{
					c.Bonus = 20;
					c.CheckCategory = true;
					c.Category = shield;
				});
			}
			bp.AddComponent(delegate(MaxDexBonusIncrease c)
			{
				c.Bonus = 10;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.ArmorFocus;
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
			bp.SetDescription(Main.IsekaiContext, "Metamagic flows naturally through your conduits. Extend Spell no longer increases the spell slot level, Empower Spell increases it by 1 instead of 2, and you gain +2 to all spell save DCs. The slot reduction does not stack with the mythic Favorite Metamagic for the same metamagic.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Spell;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddMechanicsFeature c)
			{
				c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.FavoriteMetamagicEmpower;
			});
			bp.AddComponent(delegate(AddMechanicsFeature c)
			{
				c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.FavoriteMetamagicExtend;
			});
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
			bp.SetDescription(Main.IsekaiContext, "You weave spells with dual-threaded speed. Quicken Spell increases the spell slot level by 3 instead of 4, and you gain a +6 bonus on initiative checks. The slot reduction does not stack with the mythic Favorite Metamagic (Quicken).");
			((BlueprintUnitFact)bp).m_Icon = Icon_Spell;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Initiative;
				c.Value = 6;
			});
			bp.AddComponent(delegate(AddMechanicsFeature c)
			{
				c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.FavoriteMetamagicQuicken;
			});
		}));
		BlueprintBuff EpicGrandSummonerSummonBuff = TTCoreExtensions.CreateBuff("EpicGrandSummonerSummonBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Epic Grand Summoner");
			bp.SetDescription(Main.IsekaiContext, "Imbued with epic planar resonance: +6 enhancement bonus to all ability scores and a +4 natural armor bonus to AC.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Spell;
			bp.IsClassFeature = true;
			bp.Stacking = StackingType.Replace;
			StatType[] array = new StatType[6]
			{
				StatType.Strength,
				StatType.Dexterity,
				StatType.Constitution,
				StatType.Intelligence,
				StatType.Wisdom,
				StatType.Charisma
			};
			foreach (StatType stat in array)
			{
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = stat;
					c.Value = 6;
				});
			}
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
			bp.SetDescription(Main.IsekaiContext, "Your summoned creatures are imbued with epic planar resonance: they gain a +6 enhancement bonus to all ability scores and a +4 natural armor bonus to AC.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Spell;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(OnSpawnBuff c)
			{
				c.m_buff = EpicGrandSummonerSummonBuff.ToReference<BlueprintBuffReference>();
				c.IsInfinity = true;
			});
		}));
		RegisterEpicFeat(Helpers.CreateBlueprint(Main.IsekaiContext, "EpicFlurryOfBlades", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Epic Flurry of Blades");
			bp.SetDescription(Main.IsekaiContext, "You wield two weapons with unmatched grace and coordination. While wielding a weapon in each hand you gain a +2 bonus on attack rolls (cancelling the Two-Weapon Fighting penalty with a light off-hand weapon), your off-hand weapon adds your full Strength bonus to damage, and you gain 1 additional off-hand attack when making a full attack.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Sword;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AttackBonusConditional c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Bonus = 2;
				c.Conditions = ActionFlow.IfSingle<ContextConditionCasterHasTwoWeapons>();
			});
			bp.AddComponent(delegate(AddMechanicsFeature c)
			{
				c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.DoubleSlice;
			});
			bp.AddComponent(delegate(AddExtraOffHandAttack c)
			{
				c.Number = 1;
			});
		}));
		static void RegisterEpicFeat(BlueprintFeature feat)
		{
			feat.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 21;
			});
			Epic21Abilities.OfferEpicContent(delegate
			{
				SpecialPowerSelection.AddToSelection(feat);
				BlueprintFeatureSelection modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiBonusFeatSelection");
				if (modBlueprint != null)
				{
					modBlueprint.m_Features = modBlueprint.m_Features.AppendToArray(feat.ToReference<BlueprintFeatureReference>());
					modBlueprint.m_AllFeatures = modBlueprint.m_AllFeatures.AppendToArray(feat.ToReference<BlueprintFeatureReference>());
				}
			});
		}
	}
}
