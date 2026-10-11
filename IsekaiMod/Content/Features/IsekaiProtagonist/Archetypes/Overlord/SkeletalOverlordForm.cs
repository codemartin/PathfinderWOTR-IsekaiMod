using System;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Guardians;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;

internal class SkeletalOverlordForm
{
	private static bool Added = false;

	private static BlueprintFeature ClassFeature;

	private static readonly Sprite Icon_Lich = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("bd9a5db280284025ab01474b0a4cd434"))?.m_Icon ?? ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("0093fc0098c984c43ac239d1211d1311"))?.m_Icon;

	private static readonly Sprite Icon_LichSustain = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("a9a52760290591844a96d0109e30e04d"))?.m_Icon ?? Icon_Lich;

	internal const string LichPolymorphBuffGuid = "e2013ae282a346c0b368559107b52f5b";

	private static readonly BlueprintBuff LichPolymorphBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("e2013ae282a346c0b368559107b52f5b");

	internal const string ThanadaemonUnitGuid = "bc6b0477fba49e54e84e2c7ba3cf2144";

	private static readonly BlueprintUnit ThanadaemonBase = BlueprintTools.GetBlueprint<BlueprintUnit>("bc6b0477fba49e54e84e2c7ba3cf2144");

	internal const string FemaleLichPolymorphBuffGuid = "85198e944e91400e99a6bfecd4065d0a";

	internal const string FemaleFallbackUnitGuid = "e6ace72105be4a73bd262be313c21ffb";

	internal const int FleshImmunitiesLevel = 5;

	internal const int NerveImmunitiesLevel = 10;

	internal const int MindImmunitiesLevel = 15;

	internal const int GraveImmunitiesLevel = 20;

	internal static UnitViewLink FemaleBodyModel()
	{
		UnitViewLink dlcBlueprint = Usable(BlueprintTools.GetBlueprint<BlueprintBuff>("85198e944e91400e99a6bfecd4065d0a")?.GetComponent<Polymorph>()?.m_Prefab);
		UnitViewLink fallback = Usable(BlueprintTools.GetBlueprint<BlueprintUnit>("e6ace72105be4a73bd262be313c21ffb")?.Prefab);
		return IsekaiDlcManager.PreferOwned(IsekaiDlcManager.HasDlc6(), dlcBlueprint, fallback);
	}

	private static UnitViewLink Usable(UnitViewLink link)
	{
		if (!(link != null) || string.IsNullOrEmpty(link.AssetId))
		{
			return null;
		}
		return link;
	}

	private static BlueprintFeature ImmunityTier(string name, string displayName, string description, SpellDescriptor descriptor, Action<BlueprintFeature> extra = null)
	{
		return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, displayName);
			bp.SetDescription(Main.IsekaiContext, description);
			((BlueprintUnitFact)bp).m_Icon = Icon_Lich;
			bp.IsClassFeature = true;
			bp.HideInUI = false;
			if (descriptor != SpellDescriptor.None)
			{
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = descriptor;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = descriptor;
				});
			}
			extra?.Invoke(bp);
		});
	}

	public static void Add()
	{
		if (Added)
		{
			return;
		}
		Added = true;
		BlueprintBuff OverlordBodyBuff = VisualPolymorph.CreateBodyBuff("SkeletalOverlordFormBodyBuff", LichPolymorphBuff);
		if (OverlordBodyBuff != null && VisualPolymorph.SetModel(OverlordBodyBuff, ThanadaemonBase))
		{
			VisualPolymorph.SetFemaleModel(OverlordBodyBuff, FemaleBodyModel());
			GuardianVisualScale.RegisterPolymorphBuff(OverlordBodyBuff);
			BorrowedModelFixes.RegisterPolymorphBuff(OverlordBodyBuff);
		}
		BlueprintFeature FleshImmunities = ImmunityTier("SkeletalOverlordFormFleshImmunitiesFeature", "Skeletal Overlord: Dead Flesh", "Your undead body has no blood to spill and no living tissue to corrupt. While in Skeletal Overlord Form you are immune to bleed, disease, poison and sleep.", SpellDescriptor.Poison | SpellDescriptor.Disease | SpellDescriptor.Sleep | SpellDescriptor.Bleed);
		BlueprintFeature NerveImmunities = ImmunityTier("SkeletalOverlordFormNerveImmunitiesFeature", "Skeletal Overlord: Unbound Bones", "Nothing remains that can be locked up or rattled loose. While in Skeletal Overlord Form you are immune to paralysis and stun, and cannot be flanked.", SpellDescriptor.Stun | SpellDescriptor.Paralysis, delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(AddMechanicsFeature c)
			{
				c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.CannotBeFlanked;
			});
		});
		BlueprintFeature MindImmunities = ImmunityTier("SkeletalOverlordFormMindImmunitiesFeature", "Skeletal Overlord: Sovereign Will", "The will that rules the grave answers to nobody. While in Skeletal Overlord Form you are immune to mind-affecting effects.", SpellDescriptor.MindAffecting);
		BlueprintFeature GraveImmunities = ImmunityTier("SkeletalOverlordFormGraveImmunitiesFeature", "Skeletal Overlord: Beyond Death", "You are already dead, and there is no vital spot left to strike. While in Skeletal Overlord Form you are immune to death effects, to critical hits and to precision damage.", SpellDescriptor.Death, delegate(BlueprintFeature bp)
		{
			bp.AddComponent<AddImmunityToCriticalHits>();
			bp.AddComponent<AddImmunityToPrecisionDamage>();
		});
		BlueprintBuff SkeletalOverlordFormBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "SkeletalOverlordFormBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Skeletal Overlord Form");
			bp.SetDescription(Main.IsekaiContext, "In your true heteromorphic undead state, you reveal the terrifying majesty of an absolute sovereign of the grave. You possess cold immunity, damage reduction that rises from 5 to 15 (bypassed only by a magic bludgeoning weapon), negative energy affinity, and a +2 profane bonus to your Armor Class and saving throws.\nYour undead nature deepens as you grow: immunity to bleed, disease, poison and sleep at 5th level, to paralysis and stun (and immunity to flanking) at 10th, to mind-affecting effects at 15th, and to death effects, critical hits and precision damage at 20th.\nYour undead body is not a mortal frame, so your armor, headgear, cloak and clothing are not drawn on it. Nothing is unequipped and everything you wear keeps working in full, with every enchantment on it. A male Overlord becomes a horned reaper that bears its own lantern staff, draws none of your gear and swings that staff whatever you wield; a female Overlord becomes an undead woman who still carries the weapons and shield in her hands.\nThe form is Medium: a Small character grows to Medium while in it, and takes that size's changes to Armor Class, attack rolls and combat maneuvers.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Lich;
			bp.IsClassFeature = true;
			bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
			bp.AddComponent(delegate(SpellDescriptorComponent c)
			{
				c.Descriptor = SpellDescriptor.Polymorph;
			});
			bp.AddComponent(delegate(AddFactContextActions c)
			{
				c.Activated = Helpers.CreateActionList(new ContextActionRemoveBuffsByDescriptor
				{
					SpellDescriptor = SpellDescriptor.Polymorph,
					NotSelf = true
				});
				c.Deactivated = Helpers.CreateActionList();
				c.NewRound = Helpers.CreateActionList();
			});
			if (OverlordBodyBuff != null)
			{
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { OverlordBodyBuff.ToReference<BlueprintUnitFactReference>() };
				});
			}
			var array = new[]
			{
				new
				{
					Feature = FleshImmunities,
					Level = 5
				},
				new
				{
					Feature = NerveImmunities,
					Level = 10
				},
				new
				{
					Feature = MindImmunities,
					Level = 15
				},
				new
				{
					Feature = GraveImmunities,
					Level = 20
				}
			};
			foreach (var anon in array)
			{
				var captured = anon;
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = captured.Level;
					c.m_Feature = captured.Feature.ToReference<BlueprintFeatureReference>();
					c.BeforeThisLevel = false;
					c.m_AdditionalClasses = new BlueprintCharacterClassReference[0];
					c.m_Archetypes = new BlueprintArchetypeReference[0];
				});
			}
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
				c.m_Class = IsekaiAscendantClass.ClassLevelScalingClasses();
				c.m_Progression = ContextRankProgression.Custom;
				c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[3]
				{
					new ContextRankConfig.CustomProgressionItem
					{
						BaseValue = 9,
						ProgressionValue = 5
					},
					new ContextRankConfig.CustomProgressionItem
					{
						BaseValue = 19,
						ProgressionValue = 10
					},
					new ContextRankConfig.CustomProgressionItem
					{
						BaseValue = 100,
						ProgressionValue = 15
					}
				};
			});
		});
		BlueprintAbility SkeletalOverlordFormToggleAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SkeletalOverlordFormToggleAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Shift Form: Overlord / Mortal Guise");
			bp.SetDescription(Main.IsekaiContext, "As a free action, you switch between your Mortal Guise and your true heteromorphic Skeletal Overlord body.\nIn Skeletal Overlord Form, you gain cold immunity, scaling physical damage resistance, profane defensive enhancements, and undead immunities that unlock at 5th, 10th, 15th and 20th level.\nActivating this ability while in Skeletal Overlord Form returns you to your Mortal Guise, concealing your terrifying undead nature.");
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
			bp.SetDescription(Main.IsekaiContext, "Continuously maintain your true heteromorphic Skeletal Overlord body. In Skeletal Overlord Form, you gain cold immunity, scaling physical damage resistance, profane defensive enhancements, and undead immunities that unlock at 5th, 10th, 15th and 20th level.");
			((BlueprintUnitFact)bp).m_Icon = Icon_LichSustain;
			bp.m_Buff = SkeletalOverlordFormBuff.ToReference<BlueprintBuffReference>();
			bp.Group = ActivatableAbilityGroup.None;
			bp.WeightInGroup = 1;
			bp.IsOnByDefault = false;
			bp.DeactivateImmediately = true;
			bp.ActivationType = AbilityActivationType.Immediately;
		});
		ClassFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "SkeletalOverlordFormFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Heteromorphic Reincarnation: Skeletal Overlord");
			bp.SetDescription(Main.IsekaiContext, "You were reincarnated into Golarion not as a living mortal, but as an immortal heteromorphic Overlord, a supreme skeletal ruler of death. While you have learned to project a convincing Mortal Guise to walk unhindered among the living, you can unleash your true Skeletal Overlord form at will, gaining cold immunity, profane defenses, terrifying physical resistance, and undead immunities that deepen as you rise in level.\n\nYou begin and travel in your mortal guise. Use Shift Form: Overlord / Mortal Guise, a free action, to change shape, or switch on Skeletal Overlord Form (Auto-Maintain) to stay revealed.\n\nYour true form is a reaper of souls: a horned, lantern-bearing reaper for a male Overlord, an undead woman for a female one. It is not a mortal frame, so your armor, headgear, cloak and clothing are not drawn on it: everything stays equipped and every bonus and enchantment still applies. The male reaper draws none of your gear and strikes with its own staff whatever you wield; the female form still carries the weapons and shield in her hands. The form is Medium, so a Small character grows to Medium while revealed.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Lich;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[2]
				{
					SkeletalOverlordFormToggleAbility.ToReference<BlueprintUnitFactReference>(),
					SkeletalOverlordFormAbility.ToReference<BlueprintUnitFactReference>()
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
