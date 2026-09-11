using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Localization;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UI.GenericSlot;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class OverpoweredAuraSelection
	{
		public static void Add()
		{
			Sprite Icon_RighteousWrath = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_OP_AURA_RIGHTEOUS_WRATH.png");
			Sprite Icon_MalevolentRage = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_OP_AURA_MALEVOLENT_RAGE.png");
			Sprite Icon_ChaoticFrenzy = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_OP_AURA_CHAOTIC_FRENZY.png");
			Sprite Icon_IndignantFury = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("4c349361d720e844e846ad8c19959b1e")).m_Icon;
			LocalizedString displayName = Helpers.CreateString(Main.IsekaiContext, "RighteousWrath.Name", "Overpowered Aura - Righteous Wrath");
			LocalizedString RighteousWrathDisplayNameBuff = Helpers.CreateString(Main.IsekaiContext, "RighteousWrathBuff.Name", "Aura of Righteous Wrath");
			LocalizedString displayDesc = Helpers.CreateString(Main.IsekaiContext, "RighteousWrath.Description", "Good allies within 40 feet of you have an extra attack and deal an additional 2d6 holy damage.");
			LocalizedString RighteousWrathDisplayDescBuff = Helpers.CreateString(Main.IsekaiContext, "RighteousWrathBuff.Description", "This character has an extra attack and deals an additional 2d6 holy damage.");
			BlueprintWeaponEnchantment RighteousWrathEnchantment = Helpers.CreateBlueprint(Main.IsekaiContext, "RighteousWrathEnchantment", delegate(BlueprintWeaponEnchantment bp)
			{
				bp.SetName(RighteousWrathDisplayNameBuff);
				bp.SetDescription(RighteousWrathDisplayDescBuff);
				((BlueprintItemEnchantment)bp).m_Prefix = StaticReferences.Strings.Null;
				((BlueprintItemEnchantment)bp).m_Suffix = StaticReferences.Strings.Null;
				bp.AddComponent(delegate(WeaponEnergyDamageDice c)
				{
					c.EnergyDamageDice = new DiceFormula
					{
						m_Dice = DiceType.D6,
						m_Rolls = 2
					};
					c.Element = DamageEnergyType.Holy;
				});
			});
			BlueprintBuff buff = TTCoreExtensions.CreateBuff("RighteousWrathBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(RighteousWrathDisplayNameBuff);
				bp.SetDescription(RighteousWrathDisplayDescBuff);
				bp.IsClassFeature = true;
				((BlueprintUnitFact)bp).m_Icon = Icon_RighteousWrath;
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
				});
				bp.AddComponent(delegate(BuffEnchantAnyWeapon c)
				{
					c.m_EnchantmentBlueprint = RighteousWrathEnchantment.ToReference<BlueprintItemEnchantmentReference>();
					c.Slot = EquipSlotBase.SlotType.PrimaryHand;
				});
				bp.AddComponent(delegate(BuffEnchantAnyWeapon c)
				{
					c.m_EnchantmentBlueprint = RighteousWrathEnchantment.ToReference<BlueprintItemEnchantmentReference>();
					c.Slot = EquipSlotBase.SlotType.SecondaryHand;
				});
			});
			BlueprintFeature RighteousWrathFeature = CreateOPAuraFeature("RighteousWrath", displayName, displayDesc, Icon_RighteousWrath, buff, AlignmentComponent.Good, AlignmentMaskType.Good);
			LocalizedString displayName2 = Helpers.CreateString(Main.IsekaiContext, "MalevolentRage.Name", "Overpowered Aura - Malevolent Rage");
			LocalizedString MalevolentRageDisplayNameBuff = Helpers.CreateString(Main.IsekaiContext, "MalevolentRageBuff.Name", "Aura of Malevolent Rage");
			LocalizedString displayDesc2 = Helpers.CreateString(Main.IsekaiContext, "MalevolentRage.Description", "Evil allies within 40 feet of you have an extra attack and deal an additional 2d6 unholy damage.");
			LocalizedString MalevolentRageDisplayDescBuff = Helpers.CreateString(Main.IsekaiContext, "MalevolentRageBuff.Description", "This character has an extra attack and deals an additional 2d6 unholy damage.");
			BlueprintWeaponEnchantment MalevolentRageEnchantment = Helpers.CreateBlueprint(Main.IsekaiContext, "MalevolentRageEnchantment", delegate(BlueprintWeaponEnchantment bp)
			{
				bp.SetName(MalevolentRageDisplayNameBuff);
				bp.SetDescription(MalevolentRageDisplayDescBuff);
				((BlueprintItemEnchantment)bp).m_Prefix = StaticReferences.Strings.Null;
				((BlueprintItemEnchantment)bp).m_Suffix = StaticReferences.Strings.Null;
				bp.AddComponent(delegate(WeaponEnergyDamageDice c)
				{
					c.EnergyDamageDice = new DiceFormula
					{
						m_Dice = DiceType.D6,
						m_Rolls = 2
					};
					c.Element = DamageEnergyType.Unholy;
				});
			});
			BlueprintBuff buff2 = TTCoreExtensions.CreateBuff("MalevolentRageBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(MalevolentRageDisplayNameBuff);
				bp.SetDescription(MalevolentRageDisplayDescBuff);
				bp.IsClassFeature = true;
				((BlueprintUnitFact)bp).m_Icon = Icon_MalevolentRage;
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
				});
				bp.AddComponent(delegate(BuffEnchantAnyWeapon c)
				{
					c.m_EnchantmentBlueprint = MalevolentRageEnchantment.ToReference<BlueprintItemEnchantmentReference>();
					c.Slot = EquipSlotBase.SlotType.PrimaryHand;
				});
				bp.AddComponent(delegate(BuffEnchantAnyWeapon c)
				{
					c.m_EnchantmentBlueprint = MalevolentRageEnchantment.ToReference<BlueprintItemEnchantmentReference>();
					c.Slot = EquipSlotBase.SlotType.SecondaryHand;
				});
			});
			BlueprintFeature MalevolentRageFeature = CreateOPAuraFeature("MalevolentRage", displayName2, displayDesc2, Icon_MalevolentRage, buff2, AlignmentComponent.Evil, AlignmentMaskType.Evil);
			LocalizedString displayName3 = Helpers.CreateString(Main.IsekaiContext, "IndignantFury.Name", "Overpowered Aura - Indignant Fury");
			LocalizedString IndignantFuryDisplayNameBuff = Helpers.CreateString(Main.IsekaiContext, "IndignantFuryBuff.Name", "Aura of Indignant Fury");
			LocalizedString displayDesc3 = Helpers.CreateString(Main.IsekaiContext, "IndignantFury.Description", "Lawful allies within 40 feet of you deal an additional 3d6 force damage.");
			LocalizedString IndignantFuryDisplayDescBuff = Helpers.CreateString(Main.IsekaiContext, "IndignantFuryBuff.Description", "This character deals an additional 3d6 force damage.");
			BlueprintWeaponEnchantment IndignantFuryEnchantment = Helpers.CreateBlueprint(Main.IsekaiContext, "IndignantFuryEnchantment", delegate(BlueprintWeaponEnchantment bp)
			{
				bp.SetName(IndignantFuryDisplayNameBuff);
				bp.SetDescription(IndignantFuryDisplayDescBuff);
				((BlueprintItemEnchantment)bp).m_Prefix = StaticReferences.Strings.Null;
				((BlueprintItemEnchantment)bp).m_Suffix = StaticReferences.Strings.Null;
				bp.AddComponent(delegate(WeaponConditionalDamageDice c)
				{
					c.Conditions = ActionFlow.EmptyCondition();
					c.Damage = new DamageDescription
					{
						Dice = new DiceFormula
						{
							m_Dice = DiceType.D6,
							m_Rolls = 3
						},
						TypeDescription = new DamageTypeDescription
						{
							Type = DamageType.Force,
							Physical = new DamageTypeDescription.PhysicalData(),
							Common = new DamageTypeDescription.CommomData()
						}
					};
				});
			});
			BlueprintBuff buff3 = TTCoreExtensions.CreateBuff("IndignantFuryBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(IndignantFuryDisplayNameBuff);
				bp.SetDescription(IndignantFuryDisplayDescBuff);
				bp.IsClassFeature = true;
				((BlueprintUnitFact)bp).m_Icon = Icon_IndignantFury;
				bp.AddComponent(delegate(BuffEnchantAnyWeapon c)
				{
					c.m_EnchantmentBlueprint = IndignantFuryEnchantment.ToReference<BlueprintItemEnchantmentReference>();
					c.Slot = EquipSlotBase.SlotType.PrimaryHand;
				});
				bp.AddComponent(delegate(BuffEnchantAnyWeapon c)
				{
					c.m_EnchantmentBlueprint = IndignantFuryEnchantment.ToReference<BlueprintItemEnchantmentReference>();
					c.Slot = EquipSlotBase.SlotType.SecondaryHand;
				});
			});
			BlueprintFeature IndignantFuryFeature = CreateOPAuraFeature("IndignantFury", displayName3, displayDesc3, Icon_IndignantFury, buff3, AlignmentComponent.Lawful, AlignmentMaskType.Lawful);
			LocalizedString displayName4 = Helpers.CreateString(Main.IsekaiContext, "ChaoticFrenzy.Name", "Overpowered Aura - Chaotic Frenzy");
			LocalizedString ChaoticFrenzyDisplayNameBuff = Helpers.CreateString(Main.IsekaiContext, "ChaoticFrenzyBuff.Name", "Aura of Chaotic Frenzy");
			LocalizedString displayDesc4 = Helpers.CreateString(Main.IsekaiContext, "ChaoticFrenzy.Description", "Chaotic allies within 40 feet of you have an extra attack and deal an additional 2d6 physical damage.");
			LocalizedString ChaoticFrenzyDisplayDescBuff = Helpers.CreateString(Main.IsekaiContext, "ChaoticFrenzyBuff.Description", "This character has an extra attack and deals an additional 2d6 physical damage.");
			BlueprintWeaponEnchantment ChaoticFrenzyEnchantment = Helpers.CreateBlueprint(Main.IsekaiContext, "ChaoticFrenzyEnchantment", delegate(BlueprintWeaponEnchantment bp)
			{
				bp.SetName(ChaoticFrenzyDisplayNameBuff);
				bp.SetDescription(ChaoticFrenzyDisplayDescBuff);
				((BlueprintItemEnchantment)bp).m_Prefix = StaticReferences.Strings.Null;
				((BlueprintItemEnchantment)bp).m_Suffix = StaticReferences.Strings.Null;
				bp.AddComponent(delegate(WeaponConditionalDamageDice c)
				{
					c.Conditions = ActionFlow.EmptyCondition();
					c.Damage = new DamageDescription
					{
						Dice = new DiceFormula
						{
							m_Dice = DiceType.D6,
							m_Rolls = 2
						},
						TypeDescription = new DamageTypeDescription
						{
							Type = DamageType.Physical,
							Physical = new DamageTypeDescription.PhysicalData
							{
								Form = (PhysicalDamageForm.Bludgeoning | PhysicalDamageForm.Piercing | PhysicalDamageForm.Slashing)
							},
							Common = new DamageTypeDescription.CommomData()
						}
					};
				});
			});
			BlueprintBuff buff4 = TTCoreExtensions.CreateBuff("ChaoticFrenzyBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(ChaoticFrenzyDisplayNameBuff);
				bp.SetDescription(ChaoticFrenzyDisplayDescBuff);
				bp.IsClassFeature = true;
				((BlueprintUnitFact)bp).m_Icon = Icon_ChaoticFrenzy;
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
				});
				bp.AddComponent(delegate(BuffEnchantAnyWeapon c)
				{
					c.m_EnchantmentBlueprint = ChaoticFrenzyEnchantment.ToReference<BlueprintItemEnchantmentReference>();
					c.Slot = EquipSlotBase.SlotType.PrimaryHand;
				});
				bp.AddComponent(delegate(BuffEnchantAnyWeapon c)
				{
					c.m_EnchantmentBlueprint = ChaoticFrenzyEnchantment.ToReference<BlueprintItemEnchantmentReference>();
					c.Slot = EquipSlotBase.SlotType.SecondaryHand;
				});
			});
			BlueprintFeature ChaoticFrenzyFeature = CreateOPAuraFeature("ChaoticFrenzy", displayName4, displayDesc4, Icon_ChaoticFrenzy, buff4, AlignmentComponent.Chaotic, AlignmentMaskType.Chaotic);
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "OverpoweredAuraSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Aura");
				bp.SetDescription(Main.IsekaiContext, "You gain an Overpowered Aura that greatly enhances the combat ability of your allies.");
				((BlueprintUnitFact)bp).m_Icon = Icon_RighteousWrath;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[4]
				{
					RighteousWrathFeature.ToReference<BlueprintFeatureReference>(),
					MalevolentRageFeature.ToReference<BlueprintFeatureReference>(),
					IndignantFuryFeature.ToReference<BlueprintFeatureReference>(),
					ChaoticFrenzyFeature.ToReference<BlueprintFeatureReference>()
				};
				bp.m_Features = bp.m_AllFeatures;
			}));
		}

		private static BlueprintFeature CreateOPAuraFeature(string name, LocalizedString displayName, LocalizedString displayDesc, Sprite icon, BlueprintBuff buff, AlignmentComponent alignment, AlignmentMaskType alignmentMask)
		{
			BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleAuraFeature(name, displayName, displayDesc, icon, delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
				bp.Size = new Feet(40f);
				bp.AddComponent(delegate(AbilityAreaEffectRunAction c)
				{
					c.UnitEnter = ActionFlow.DoSingle(delegate(Conditional conditional)
					{
						conditional.ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionAlignment contextConditionAlignment)
						{
							contextConditionAlignment.Alignment = alignment;
						});
						conditional.IfTrue = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
						{
							contextActionApplyBuff.m_Buff = buff.ToReference<BlueprintBuffReference>();
							contextActionApplyBuff.DurationValue = Values.Duration.Zero;
							contextActionApplyBuff.Permanent = true;
						});
						conditional.IfFalse = ActionFlow.DoNothing();
					});
					c.UnitExit = ActionFlow.DoSingle(delegate(ContextActionRemoveBuff b)
					{
						b.m_Buff = buff.ToReference<BlueprintBuffReference>();
					});
					c.UnitMove = ActionFlow.DoNothing();
					c.Round = ActionFlow.DoNothing();
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteAlignment c)
			{
				c.Group = Prerequisite.GroupType.Any;
				c.Alignment = alignmentMask;
			});
			return blueprintFeature;
		}
	}
}
