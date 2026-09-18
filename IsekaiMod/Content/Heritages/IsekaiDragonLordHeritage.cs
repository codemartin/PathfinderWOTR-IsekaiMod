using IsekaiMod.Utilities;
using Kingmaker.Assets.UnitLogic.Mechanics.Properties;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages
{
	internal class IsekaiDragonLordHeritage
	{
		public static void Add()
		{
			Sprite Icon_DragonBreath = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("63bb12d87f99c4748a890956e5f6b4c7")).m_Icon;
			BlueprintItemWeapon DragonClaw = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("18dc77b96c009804399c834e028d0552");
			BlueprintFeature BaseWingsFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("85cc897d744d5dc4ba159291ff15d7ea");
			BlueprintAbilityResource DragonBreathResource = Helpers.CreateBlueprint(Main.IsekaiContext, "DragonLordBreathResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevel = false,
					IncreasedByStat = false
				};
			});
			BlueprintUnitProperty DragonLordBreathUnitProperty = Helpers.CreateBlueprint(Main.IsekaiContext, "DragonLordBreathUnitProperty", delegate(BlueprintUnitProperty bp)
			{
				bp.name = "DragonLordBreathUnitProperty";
				bp.AddComponent(delegate(ComplexPropertyGetter c)
				{
					c.Property = UnitProperty.Level;
					c.Denominator = 2;
					c.Multiplier = 1;
					c.Bonus = 0;
				});
				bp.AddComponent(delegate(SimplePropertyGetter c)
				{
					c.Property = UnitProperty.StatBonusConstitution;
				});
				bp.BaseValue = 10;
				bp.OperationOnComponents = BlueprintUnitProperty.MathOperation.Sum;
			});
			BlueprintAbility DragonLordBreathAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "DragonLordBreathAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Dragon Breath");
				bp.SetDescription(Main.IsekaiContext, "Exhale a 30 ft cone of burning draconic fire. Deals 1d6 fire damage per character level. Reflex save halves (DC 10 + 1/2 character level + Constitution modifier).");
				((BlueprintUnitFact)bp).m_Icon = Icon_DragonBreath;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Close;
				bp.CanTargetPoint = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.BreathWeapon;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AddComponent(delegate(AbilityTargetsAround c)
				{
					c.m_Radius = 30.Feet();
					c.m_TargetType = TargetType.Enemy;
					c.m_IncludeDead = false;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionSavingThrow
					{
						Type = SavingThrowType.Reflex,
						Actions = Helpers.CreateActionList(new ContextActionDealDamage
						{
							DamageType = new DamageTypeDescription
							{
								Type = DamageType.Energy,
								Energy = DamageEnergyType.Fire
							},
							Duration = new ContextDurationValue
							{
								Rate = DurationRate.Rounds,
								DiceType = DiceType.Zero
							},
							Value = new ContextDiceValue
							{
								DiceType = DiceType.D6,
								DiceCountValue = Values.CreateContextRankValue(AbilityRankType.Default),
								BonusValue = 0
							},
							IsAoE = true,
							HalfIfSaved = true
						})
					});
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				});
				bp.AddComponent(delegate(ContextSetAbilityParams c)
				{
					c.DC = Values.CreateContextCasterCustomPropertyValue(DragonLordBreathUnitProperty);
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = DragonBreathResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
			});
			BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiDragonLordHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Dragon Lord");
				bp.SetDescription(Main.IsekaiContext, "Reincarnated carrying the primordial majesty and unmatched might of the supreme dragon lords. Their scales deflect the sharpest blades, and their breath turns armies to cinder.\nThe Dragon Lord gains a +4 racial bonus to Strength and Constitution, a +2 racial bonus to Charisma, a +4 natural armor bonus to AC, Draconic Claws (1d6 slashing damage), draconic wings granting +3 dodge AC against melee attacks and immunity to ground hazards, immunity to sleep, paralysis, and fire damage, and the Dragon Breath ability usable 3 times per day.");
				((BlueprintUnitFact)bp).m_Icon = Icon_DragonBreath;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Constitution;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Charisma;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddAdditionalLimb c)
				{
					c.m_Weapon = DragonClaw.ToReference<BlueprintItemWeaponReference>();
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Fire;
					c.Value = 100;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Paralysis | SpellDescriptor.Sleep;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Paralysis | SpellDescriptor.Sleep;
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = DragonBreathResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[2]
					{
						DragonLordBreathAbility.ToReference<BlueprintUnitFactReference>(),
						BaseWingsFeature.ToReference<BlueprintUnitFactReference>()
					};
				});
				bp.Groups = new FeatureGroup[1] { FeatureGroup.Racial };
				bp.ReapplyOnLevelUp = true;
			});
			FeatTools.Selections.ElvenHeritageSelection.AddToSelection(feature);
			BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("fd6e1f53589049cbbbc6a8e058d83b74")?.AddToSelection(feature);
			BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("8c3244440e0b4d1d9d9b182685cbacbd")?.AddToSelection(feature);
			HumanHeritageSelection.Register(feature);
		}
	}
}
