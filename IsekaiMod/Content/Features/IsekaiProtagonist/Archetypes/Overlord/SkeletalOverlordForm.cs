using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord
{
	internal class SkeletalOverlordForm
	{
		private static bool Added = false;

		private static BlueprintFeature ClassFeature;

		private static readonly Sprite Icon_Lich = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("bd9a5db280284025ab01474b0a4cd434"))?.m_Icon ?? ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("4e83f1e0e52b4613982b14ee2796928f"))?.m_Icon;

		private static readonly Sprite Icon_Aura = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ad5ed5ea4ec52334a94e975a64dad336"))?.m_Icon ?? Icon_Lich;

		private static readonly Sprite Icon_Rune = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("ee7dc126939e4d9438357fbd5980d459"))?.m_Icon ?? Icon_Lich;

		private const string Prefab_ArchLichSovereign = "faff8234f920503479d069ce93800862";

		private const string Prefab_SkeletalDeathKnight = "5daad0238a0aa98429ad207c8f5174cb";

		private const string Prefab_AncientRunelordLich = "26d4e5b6eac936843962b2fcbf7b82ed";

		private const string Prefab_SpectralShadowLich = "911f71fa54003a148a50d5bd1430d4ec";

		private const string SoulsTrailBuffGuid = "9f87de93b3a347ce8a163cf64548a535";

		public static void Add()
		{
			if (Added)
			{
				return;
			}
			Added = true;
			BlueprintBuff soulsBuff = BlueprintSafetyExtensions.SafeGetBlueprint<BlueprintBuff>("9f87de93b3a347ce8a163cf64548a535");
			BlueprintBuff SkeletalOverlordFormBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "SkeletalOverlordFormBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Skeletal Overlord Form");
				bp.SetDescription(Main.IsekaiContext, "In your true heteromorphic undead state, you reveal the terrifying majesty of an absolute sovereign of the grave.\nDefault Visual: Supreme Arch-Lich Sovereign, wreathed in a swirling vortex of wailing souls.\nAll equipment slots and weapons remain fully equipped and functional.\n\nUniversal Overlord Passives:\n- Complete Undead Immunities: critical hits, precision damage, bleed, death effects, disease, mind-affecting, paralysis, poison, sleep, and stun.\n- Flanking immunity and cold immunity.\n- Negative energy affinity (healed by negative energy, harmed by positive energy).\n- Overlord's Majestic Charisma: your Charisma modifier is added as a profane bonus to Armor Class, Fortitude saves, and Will saves.\n- Scaling Damage Resistance: DR 10/bludgeoning and magic (scales to 15 at level 10, 20 at level 20).\n- +2 profane bonus to all Armor Class and saving throws.\n- +2 profane bonus to the Difficulty Class and Caster Level of all Necromancy and Death spells.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Lich;
				bp.IsClassFeature = true;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
				bp.AddComponent(delegate(Polymorph c)
				{
					c.m_Prefab = new UnitViewLink
					{
						AssetId = "faff8234f920503479d069ce93800862"
					};
					c.m_KeepSlots = true;
					c.Size = Size.Medium;
					c.m_SilentCaster = true;
				});
				if (soulsBuff != null)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { soulsBuff.ToReference<BlueprintUnitFactReference>() };
					});
				}
				bp.AddComponent<AddImmunityToCriticalHits>();
				bp.AddComponent<AddImmunityToPrecisionDamage>();
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Poison | SpellDescriptor.Disease | SpellDescriptor.Stun | SpellDescriptor.Paralysis | SpellDescriptor.Death | SpellDescriptor.Sleep | SpellDescriptor.Bleed;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Poison | SpellDescriptor.Disease | SpellDescriptor.Stun | SpellDescriptor.Paralysis | SpellDescriptor.Death | SpellDescriptor.Sleep | SpellDescriptor.Bleed;
				});
				bp.AddComponent(delegate(AddMechanicsFeature c)
				{
					c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.CannotBeFlanked;
				});
				bp.AddComponent<UndeadHealth>();
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.Cold;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
				bp.AddComponent(delegate(DerivativeStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.BaseStat = StatType.Charisma;
					c.DerivativeStat = StatType.AC;
				});
				bp.AddComponent(delegate(DerivativeStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.BaseStat = StatType.Charisma;
					c.DerivativeStat = StatType.SaveFortitude;
				});
				bp.AddComponent(delegate(DerivativeStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.BaseStat = StatType.Charisma;
					c.DerivativeStat = StatType.SaveWill;
				});
				bp.AddComponent(delegate(RecalculateOnStatChange c)
				{
					c.Stat = StatType.Charisma;
				});
				bp.AddComponent(delegate(IncreaseSpellSchoolDC c)
				{
					c.School = SpellSchool.Necromancy;
					c.BonusDC = 2;
					c.Descriptor = ModifierDescriptor.Profane;
				});
				bp.AddComponent(delegate(IncreaseSpellSchoolCasterLevel c)
				{
					c.School = SpellSchool.Necromancy;
					c.BonusLevel = 2;
					c.Descriptor = ModifierDescriptor.Profane;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
					c.BypassedByMagic = true;
					c.MinEnhancementBonus = 1;
					c.BypassedByForm = true;
					c.Form = PhysicalDamageForm.Bludgeoning;
					c.Or = false;
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
					c.m_Class = new BlueprintCharacterClassReference[1] { IsekaiProtagonistClass.GetReference() };
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[3]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 9,
							ProgressionValue = 10
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 19,
							ProgressionValue = 15
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 20
						}
					};
				});
			});
			BlueprintBuff OverlordAspectDeathKnightBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordAspectDeathKnightBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Overlord Aspect: Skeletal Death Knight");
				bp.SetDescription(Main.IsekaiContext, "You assume the manifestation of an imposing, armored Skeletal Death Knight, channeling unholy martial supremacy.\n- 3D Visual: Armored Horned Bone Champion with heavy plate and glowing eye sockets.\n- +3 profane bonus to melee attack and damage rolls.\n- +4 natural armor bonus to AC.\n- Additional +5 physical Damage Reduction.\n- All weapons and equipment remain fully equipped.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Aura;
				bp.IsClassFeature = true;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
				bp.AddComponent(delegate(Polymorph c)
				{
					c.m_Prefab = new UnitViewLink
					{
						AssetId = "5daad0238a0aa98429ad207c8f5174cb"
					};
					c.m_KeepSlots = true;
					c.Size = Size.Medium;
					c.m_SilentCaster = true;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 5;
				});
			});
			BlueprintBuff OverlordAspectRunelordBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordAspectRunelordBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Overlord Aspect: Ancient Runelord Lich");
				bp.SetDescription(Main.IsekaiContext, "You assume the manifestation of an ancient Thassilonian Runelord Lich, sovereign of forgotten arcane dynasties.\n- 3D Visual: Royal Sarcophagus Lich adorned in ancient golden runic regalia.\n- +4 bonus to Spell Penetration.\n- Spell Resistance equal to 12 + your character level.\n- +2 profane bonus to all saving throws.\n- Complete immunity to energy drain and negative levels.\n- All weapons and equipment remain fully equipped.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Rune;
				bp.IsClassFeature = true;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
				bp.AddComponent(delegate(Polymorph c)
				{
					c.m_Prefab = new UnitViewLink
					{
						AssetId = "26d4e5b6eac936843962b2fcbf7b82ed"
					};
					c.m_KeepSlots = true;
					c.Size = Size.Medium;
					c.m_SilentCaster = true;
				});
				bp.AddComponent(delegate(SpellPenetrationBonus c)
				{
					c.Value = 4;
					c.Descriptor = ModifierDescriptor.Profane;
				});
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.BonusValue;
					c.m_StepLevel = 12;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.NegativeLevel;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.NegativeLevel;
				});
			});
			BlueprintBuff OverlordAspectShadowBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordAspectShadowBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Overlord Aspect: Spectral Shadow Lich");
				bp.SetDescription(Main.IsekaiContext, "You assume the manifestation of an ethereal shadow lich, enveloped in spectral ghost flames.\n- 3D Visual: Mythic Shadow Spectre Lich shrouded in ghost flames.\n- 20% incorporeal concealment (miss chance).\n- +4 dodge bonus to Armor Class.\n- Complete immunity to electricity damage.\n- +10 ft bonus to base movement speed.\n- All weapons and equipment remain fully equipped.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Lich;
				bp.IsClassFeature = true;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
				bp.AddComponent(delegate(Polymorph c)
				{
					c.m_Prefab = new UnitViewLink
					{
						AssetId = "911f71fa54003a148a50d5bd1430d4ec"
					};
					c.m_KeepSlots = true;
					c.Size = Size.Medium;
					c.m_SilentCaster = true;
				});
				bp.AddComponent(delegate(AddConcealment c)
				{
					c.Concealment = Concealment.Partial;
					c.Descriptor = ConcealmentDescriptor.Blur;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.Electricity;
				});
				bp.AddComponent(delegate(BuffMovementSpeed c)
				{
					c.Value = 10;
					c.Descriptor = ModifierDescriptor.Profane;
				});
			});
			OverlordAspectDeathKnightBuff.AddComponent(delegate(AddFactContextActions c)
			{
				c.Activated = Helpers.CreateActionList(new ContextActionRemoveBuff
				{
					m_Buff = OverlordAspectRunelordBuff.ToReference<BlueprintBuffReference>()
				}, new ContextActionRemoveBuff
				{
					m_Buff = OverlordAspectShadowBuff.ToReference<BlueprintBuffReference>()
				});
			});
			OverlordAspectRunelordBuff.AddComponent(delegate(AddFactContextActions c)
			{
				c.Activated = Helpers.CreateActionList(new ContextActionRemoveBuff
				{
					m_Buff = OverlordAspectDeathKnightBuff.ToReference<BlueprintBuffReference>()
				}, new ContextActionRemoveBuff
				{
					m_Buff = OverlordAspectShadowBuff.ToReference<BlueprintBuffReference>()
				});
			});
			OverlordAspectShadowBuff.AddComponent(delegate(AddFactContextActions c)
			{
				c.Activated = Helpers.CreateActionList(new ContextActionRemoveBuff
				{
					m_Buff = OverlordAspectDeathKnightBuff.ToReference<BlueprintBuffReference>()
				}, new ContextActionRemoveBuff
				{
					m_Buff = OverlordAspectRunelordBuff.ToReference<BlueprintBuffReference>()
				});
			});
			BlueprintAbility SkeletalOverlordFormToggleAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SkeletalOverlordFormToggleAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Shift Form: Overlord / Mortal Guise");
				bp.SetDescription(Main.IsekaiContext, "As a free action, switch between your Mortal Guise and your true heteromorphic Skeletal Overlord body.\nIn Skeletal Overlord Form, you gain complete undead immunities, cold immunity, Charisma to defenses, scaling damage resistance, and profane majesty.\nActivating this ability while in Skeletal Overlord Form returns you to your Mortal Guise, concealing your terrifying undead nature.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Lich;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.ActionType = UnitCommand.CommandType.Free;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new Conditional
					{
						ConditionsChecker = new ConditionsChecker
						{
							Conditions = new Condition[1]
							{
								new ContextConditionHasBuff
								{
									m_Buff = SkeletalOverlordFormBuff.ToReference<BlueprintBuffReference>()
								}
							}
						},
						IfTrue = Helpers.CreateActionList(new ContextActionRemoveBuff
						{
							m_Buff = SkeletalOverlordFormBuff.ToReference<BlueprintBuffReference>()
						}, new ContextActionRemoveBuff
						{
							m_Buff = OverlordAspectDeathKnightBuff.ToReference<BlueprintBuffReference>()
						}, new ContextActionRemoveBuff
						{
							m_Buff = OverlordAspectRunelordBuff.ToReference<BlueprintBuffReference>()
						}, new ContextActionRemoveBuff
						{
							m_Buff = OverlordAspectShadowBuff.ToReference<BlueprintBuffReference>()
						}),
						IfFalse = Helpers.CreateActionList(new ContextActionApplyBuff
						{
							m_Buff = SkeletalOverlordFormBuff.ToReference<BlueprintBuffReference>(),
							Permanent = true,
							DurationValue = Values.Duration.Zero,
							AsChild = false
						})
					});
				});
			});
			BlueprintActivatableAbility SkeletalOverlordFormAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SkeletalOverlordFormAbility", delegate(BlueprintActivatableAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Skeletal Overlord Form (Auto-Maintain)");
				bp.SetDescription(Main.IsekaiContext, "Continuously maintain your true heteromorphic Skeletal Overlord body.\nYou gain complete undead immunities, cold immunity, Charisma to defenses, scaling damage resistance, and profane mastery.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Lich;
				bp.m_Buff = SkeletalOverlordFormBuff.ToReference<BlueprintBuffReference>();
				bp.Group = ActivatableAbilityGroup.None;
				bp.WeightInGroup = 1;
				bp.IsOnByDefault = false;
				bp.DeactivateImmediately = true;
				bp.ActivationType = AbilityActivationType.Immediately;
			});
			BlueprintActivatableAbility OverlordAspectDeathKnightAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordAspectDeathKnightAbility", delegate(BlueprintActivatableAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overlord Stance: Skeletal Death Knight");
				bp.SetDescription(Main.IsekaiContext, "Shift your Overlord manifestation into an armored Skeletal Death Knight, focusing on martial devastation.\n- 3D Visual: Armored Horned Bone Champion.\n- +3 profane bonus to melee attack and damage rolls.\n- +4 natural armor bonus to AC and +5 DR.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Aura;
				bp.m_Buff = OverlordAspectDeathKnightBuff.ToReference<BlueprintBuffReference>();
				bp.Group = ActivatableAbilityGroup.None;
				bp.WeightInGroup = 1;
				bp.IsOnByDefault = false;
				bp.DeactivateImmediately = true;
				bp.ActivationType = AbilityActivationType.Immediately;
			});
			BlueprintActivatableAbility OverlordAspectRunelordAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordAspectRunelordAbility", delegate(BlueprintActivatableAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overlord Stance: Ancient Runelord Lich");
				bp.SetDescription(Main.IsekaiContext, "Shift your Overlord manifestation into an Ancient Runelord Lich, focusing on absolute arcane supremacy.\n- 3D Visual: Royal Sarcophagus Lich in golden runic regalia.\n- +4 Spell Penetration, Spell Resistance (12 + Level), +2 to all saves, energy drain immunity.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Rune;
				bp.m_Buff = OverlordAspectRunelordBuff.ToReference<BlueprintBuffReference>();
				bp.Group = ActivatableAbilityGroup.None;
				bp.WeightInGroup = 1;
				bp.IsOnByDefault = false;
				bp.DeactivateImmediately = true;
				bp.ActivationType = AbilityActivationType.Immediately;
			});
			BlueprintActivatableAbility OverlordAspectShadowAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordAspectShadowAbility", delegate(BlueprintActivatableAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overlord Stance: Spectral Shadow Lich");
				bp.SetDescription(Main.IsekaiContext, "Shift your Overlord manifestation into a Spectral Shadow Lich, focusing on incorporeal evasion.\n- 3D Visual: Mythic Shadow Spectre Lich enveloped in ghost flames.\n- 20% incorporeal concealment, +4 dodge AC, electricity immunity, +10 ft speed.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Lich;
				bp.m_Buff = OverlordAspectShadowBuff.ToReference<BlueprintBuffReference>();
				bp.Group = ActivatableAbilityGroup.None;
				bp.WeightInGroup = 1;
				bp.IsOnByDefault = false;
				bp.DeactivateImmediately = true;
				bp.ActivationType = AbilityActivationType.Immediately;
			});
			ClassFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "SkeletalOverlordFormFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Heteromorphic Reincarnation: Skeletal Overlord");
				bp.SetDescription(Main.IsekaiContext, "You were reincarnated into Golarion not as a living mortal, but as an immortal heteromorphic Overlord, a supreme skeletal ruler of death.\nWhile you have learned to project a convincing Mortal Guise to walk unhindered among the living, you can unleash your true Skeletal Overlord form at will.\nChoose between the Supreme Arch-Lich Sovereign, the armored Skeletal Death Knight, the Ancient Runelord Lich, or the Spectral Shadow Lich.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Lich;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[5]
					{
						SkeletalOverlordFormToggleAbility.ToReference<BlueprintUnitFactReference>(),
						SkeletalOverlordFormAbility.ToReference<BlueprintUnitFactReference>(),
						OverlordAspectDeathKnightAbility.ToReference<BlueprintUnitFactReference>(),
						OverlordAspectRunelordAbility.ToReference<BlueprintUnitFactReference>(),
						OverlordAspectShadowAbility.ToReference<BlueprintUnitFactReference>()
					};
				});
			});
		}

		public static BlueprintFeature Get()
		{
			if (!Added)
			{
				Add();
			}
			return ClassFeature ?? BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SkeletalOverlordFormFeature");
		}
	}
}
