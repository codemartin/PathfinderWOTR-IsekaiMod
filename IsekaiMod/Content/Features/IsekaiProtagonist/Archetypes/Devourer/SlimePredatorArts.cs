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
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer
{
	internal static class SlimePredatorArts
	{
		private static readonly Sprite Icon_Web = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("134cb6d492269aa4f8662700ef57449f"))?.m_Icon;

		private static readonly Sprite Icon_Armor = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("9c0fa9b438ada3f43864be8dd8b3e741"))?.m_Icon ?? Icon_Web;

		private static readonly Sprite Icon_Hellfire = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("700cfcbd0cb2975419bcab7dbb8c6210"))?.m_Icon ?? Icon_Web;

		private static readonly Sprite Icon_Haste = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("03464790f40c3c24aa684b57155f3280"))?.m_Icon ?? Icon_Web;

		private static readonly Sprite Icon_DragonBreath = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("22862fcda5a1d8a4f91e6154d0d8d721"))?.m_Icon ?? Icon_Web;

		private static readonly Sprite Icon_Frenzy = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("208ce0902f20b1e4896a85b2339468ff"))?.m_Icon ?? Icon_Web;

		public static BlueprintAbility PredatorArtStickyThreadAbility;

		public static BlueprintFeature PredatorArtStickyThreadFeature;

		public static BlueprintFeature PredatorArtBodyArmorFeature;

		public static BlueprintAbility PredatorArtBlackFlameAbility;

		public static BlueprintFeature PredatorArtBlackFlameFeature;

		public static BlueprintAbility PredatorArtThoughtAccelerationAbility;

		public static BlueprintFeature PredatorArtThoughtAccelerationFeature;

		public static BlueprintAbility PredatorArtDragonBreathAbility;

		public static BlueprintFeature PredatorArtDragonBreathFeature;

		public static BlueprintBuff PredatorFrenzyBuff;

		public static BlueprintBuff SlimeDemonSurgeBuff;

		public static BlueprintBuff SlimeDragonSurgeBuff;

		public static BlueprintBuff SlimeUndeadSurgeBuff;

		public static BlueprintBuff SlimeBeastSurgeBuff;

		public static BlueprintBuff SlimeConstructSurgeBuff;

		public static BlueprintBuff SlimeCasterSurgeBuff;

		public static void Add()
		{
			PredatorFrenzyBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorFrenzyBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Predator Frenzy");
				bp.SetDescription(Main.IsekaiContext, "Consumption of a worthy adversary triggers an instinctual apex predator frenzy: +4 inherent bonus to Strength and Constitution, +30 ft base movement speed, and one additional attack per round.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Frenzy;
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
					c.Stat = StatType.Constitution;
					c.Value = 4;
				});
				bp.AddComponent(delegate(BuffMovementSpeed c)
				{
					c.Value = 30;
					c.Descriptor = ModifierDescriptor.Enhancement;
				});
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
					c.Haste = true;
				});
			});
			SlimeDemonSurgeBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeDemonSurgeBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Species Surge: Abyssal Titan");
				bp.SetDescription(Main.IsekaiContext, "Absorbing demonic essence grants DR 10/Good and a +4 profane bonus to Strength for 1 minute.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Frenzy;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 10;
					c.BypassedByAlignment = true;
					c.Alignment = DamageAlignment.Good;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
			});
			SlimeDragonSurgeBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeDragonSurgeBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Species Surge: Ancient Drake");
				bp.SetDescription(Main.IsekaiContext, "Absorbing draconic essence grants a +4 natural armor bonus, immunity to paralysis, and +4 Constitution for 1 minute.");
				((BlueprintUnitFact)bp).m_Icon = Icon_DragonBreath;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Constitution;
					c.Value = 4;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Paralysis;
				});
			});
			SlimeUndeadSurgeBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeUndeadSurgeBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Species Surge: Lich Core");
				bp.SetDescription(Main.IsekaiContext, "Absorbing necrotic core energy grants immunity to ability drain, energy drain, and mind-affecting effects for 1 minute.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Hellfire;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.NegativeLevel;
				});
			});
			SlimeBeastSurgeBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeBeastSurgeBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Species Surge: Primal Apex");
				bp.SetDescription(Main.IsekaiContext, "Absorbing primeval beast traits grants Pounce and a +30 foot bonus to movement speed for 1 minute.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Haste;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(BuffMovementSpeed c)
				{
					c.Value = 30;
					c.Descriptor = ModifierDescriptor.Enhancement;
				});
				bp.AddComponent(delegate(AddMechanicsFeature c)
				{
					c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.Pounce;
				});
			});
			SlimeConstructSurgeBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeConstructSurgeBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Species Surge: Adamantine Matrix");
				bp.SetDescription(Main.IsekaiContext, "Absorbing construct plating grants DR 10/Adamantine and immunity to bleed and critical hits for 1 minute.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Armor;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 10;
					c.BypassedByMaterial = true;
					c.Material = PhysicalDamageMaterial.Adamantite;
				});
				bp.AddComponent<AddImmunityToCriticalHits>();
			});
			SlimeCasterSurgeBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "SlimeCasterSurgeBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Species Surge: Arcane Archon");
				bp.SetDescription(Main.IsekaiContext, "Absorbing high-tier spellcaster essence grants a +2 bonus to the DC of all spells and spell penetration for 1 minute.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Web;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 2;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
				bp.AddComponent(delegate(SpellPenetrationBonus c)
				{
					c.Value = 4;
				});
			});
			BlueprintBuff StickyThreadBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "StickyThreadBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Sticky Thread: Restrained");
				bp.SetDescription(Main.IsekaiContext, "Entangled in unbreakable adhesive slime filaments: cannot move, suffers a -2 penalty to attack rolls and -4 penalty to Dexterity.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Web;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.Entangled;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.Dexterity;
					c.Value = -4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = -2;
				});
			});
			PredatorArtStickyThreadAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtStickyThreadAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Predator Art: Sticky Steel Thread");
				bp.SetDescription(Main.IsekaiContext, "As a standard action, launch viscous tensile threads at an enemy within 40 feet. The target is Entangled and immobilized for 1 minute (Reflex DC = 10 + 1/2 protagonist level + Constitution modifier negates).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Web;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Close;
				bp.CanTargetEnemies = true;
				bp.CanTargetFriends = false;
				bp.CanTargetSelf = false;
				bp.CanTargetPoint = false;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
				ContextActionApplyBuff applyDebuff = new ContextActionApplyBuff
				{
					m_Buff = StickyThreadBuff.ToReference<BlueprintBuffReference>(),
					DurationValue = new ContextDurationValue
					{
						Rate = DurationRate.Minutes,
						DiceType = DiceType.Zero,
						DiceCountValue = 0,
						BonusValue = 1
					}
				};
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionSavingThrow
					{
						Type = SavingThrowType.Reflex,
						Actions = Helpers.CreateActionList(new ContextActionConditionalSaved
						{
							Failed = Helpers.CreateActionList(applyDebuff)
						})
					});
				});
			});
			PredatorArtStickyThreadFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtStickyThreadFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Predator Art: Sticky Steel Thread");
				bp.SetDescription(Main.IsekaiContext, "You produce hyper-resilient steel filaments from your fluid biology. You gain the Sticky Steel Thread active ability to entangle and ground foes.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Web;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { PredatorArtStickyThreadAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
			PredatorArtBodyArmorFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtBodyArmorFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Predator Art: Viscous Body Armor");
				bp.SetDescription(Main.IsekaiContext, "Your slime cells continuously absorb impact and harden dynamically. You gain DR 5/Slashing, immunity to critical hits and sneak attacks, and an inherent +1 natural armor bonus per 3 protagonist levels.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Armor;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 5;
					c.BypassedByMaterial = false;
					c.BypassedByForm = true;
					c.Form = PhysicalDamageForm.Slashing;
				});
				bp.AddComponent<AddImmunityToCriticalHits>();
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
			});
			PredatorArtBlackFlameAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtBlackFlameAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Predator Art: Black Flame Hellfire");
				bp.SetDescription(Main.IsekaiContext, "Unleash a 30-foot cone of unquenchable black hellfire. Enemies in the area take 1d6 damage per protagonist level (half unholy, half fire). A successful Reflex save halves the damage (DC = 10 + 1/2 protagonist level + Constitution modifier).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Hellfire;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Close;
				bp.CanTargetPoint = true;
				bp.CanTargetEnemies = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.BreathWeapon;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionDealDamage
					{
						DamageType = new DamageTypeDescription
						{
							Type = DamageType.Direct,
							Common = new DamageTypeDescription.CommomData(),
							Physical = new DamageTypeDescription.PhysicalData()
						},
						Value = new ContextDiceValue
						{
							DiceType = DiceType.D6,
							DiceCountValue = new ContextValue
							{
								ValueType = ContextValueType.Rank
							},
							BonusValue = 0
						},
						HalfIfSaved = true
					});
				});
				bp.AddContextRankConfig(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
					c.m_Class = new BlueprintCharacterClassReference[1] { BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(Main.IsekaiContext, "IsekaiProtagonistClass").ToReference<BlueprintCharacterClassReference>() };
				});
			});
			PredatorArtBlackFlameFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtBlackFlameFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Predator Art: Black Flame Hellfire");
				bp.SetDescription(Main.IsekaiContext, "Synthesized from devoured demon lords and fire elementals, you master Black Flame, unleashing an unquenchable cone of unholy hellfire.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Hellfire;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { PredatorArtBlackFlameAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
			BlueprintBuff ThoughtAccelBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "ThoughtAccelBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Thought Acceleration: Hyper-Speed");
				bp.SetDescription(Main.IsekaiContext, "Perception of time accelerated by 1,000%: +4 insight bonus to AC and attack rolls, and an additional standard action this round.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Haste;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
					c.Haste = true;
				});
			});
			PredatorArtThoughtAccelerationAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtThoughtAccelerationAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Predator Art: Thought Acceleration");
				bp.SetDescription(Main.IsekaiContext, "As a swift action, accelerate neural processing speed by a thousandfold for 1 round. Grants a +4 insight bonus to AC, +4 to attack rolls, and the effects of Haste.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Haste;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				ContextActionApplyBuff applySelf = new ContextActionApplyBuff
				{
					m_Buff = ThoughtAccelBuff.ToReference<BlueprintBuffReference>(),
					DurationValue = new ContextDurationValue
					{
						Rate = DurationRate.Rounds,
						DiceType = DiceType.Zero,
						DiceCountValue = 0,
						BonusValue = 2
					},
					ToCaster = true
				};
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(applySelf);
				});
			});
			PredatorArtThoughtAccelerationFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtThoughtAccelerationFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Predator Art: Thought Acceleration");
				bp.SetDescription(Main.IsekaiContext, "Your fluid internal core functions as a multi-threaded organic supercomputer. You gain the Thought Acceleration active ability (swift action).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Haste;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { PredatorArtThoughtAccelerationAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
			PredatorArtDragonBreathAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtDragonBreathAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Predator Art: Draconic Plasma Breath");
				bp.SetDescription(Main.IsekaiContext, "Exhale a 50-foot cone of pure plasma synthesized from devoured ancient dragons. Deals 1d8 damage per protagonist level (half sonic, half divine). Reflex save halves (DC = 10 + 1/2 protagonist level + Constitution modifier).");
				((BlueprintUnitFact)bp).m_Icon = Icon_DragonBreath;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Close;
				bp.CanTargetPoint = true;
				bp.CanTargetEnemies = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.BreathWeapon;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionDealDamage
					{
						DamageType = new DamageTypeDescription
						{
							Type = DamageType.Direct,
							Common = new DamageTypeDescription.CommomData(),
							Physical = new DamageTypeDescription.PhysicalData()
						},
						Value = new ContextDiceValue
						{
							DiceType = DiceType.D8,
							DiceCountValue = new ContextValue
							{
								ValueType = ContextValueType.Rank
							},
							BonusValue = 0
						},
						HalfIfSaved = true
					});
				});
				bp.AddContextRankConfig(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
					c.m_Class = new BlueprintCharacterClassReference[1] { BlueprintTools.GetModBlueprint<BlueprintCharacterClass>(Main.IsekaiContext, "IsekaiProtagonistClass").ToReference<BlueprintCharacterClassReference>() };
				});
			});
			PredatorArtDragonBreathFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorArtDragonBreathFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Predator Art: Draconic Plasma Breath");
				bp.SetDescription(Main.IsekaiContext, "Assimilation of draconic dragon-factor enables you to exhale a catastrophic cone of divine sonic plasma.");
				((BlueprintUnitFact)bp).m_Icon = Icon_DragonBreath;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { PredatorArtDragonBreathAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
