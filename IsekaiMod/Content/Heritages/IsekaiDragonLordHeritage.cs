using IsekaiMod.Config;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
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
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages;

internal class IsekaiDragonLordHeritage
{
	public static void Add()
	{
		Sprite Icon_DragonBreath = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("63bb12d87f99c4748a890956e5f6b4c7"))?.m_Icon;
		BlueprintItemWeapon DragonClaw = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("18dc77b96c009804399c834e028d0552");
		BlueprintFeature DraconicWingsFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("df86a121e5d1bd04b8afdbaf8d24a1ce");
		BlueprintAbilityResource DragonBreathResource = Helpers.CreateBlueprint(Main.IsekaiContext, "DragonLordBreathResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = new BlueprintAbilityResource.Amount
			{
				BaseValue = 3,
				IncreasedByLevel = false,
				IncreasedByStat = false
			};
		});
		BlueprintAbility DragonLordBreathAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "DragonLordBreathAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Dragon Breath");
			bp.SetDescription(Main.IsekaiContext, "Exhale a 30 ft cone of burning draconic fire, 3 times per day. Deals 1d6 fire damage, plus 1d6 per 2 character levels. Reflex save halves (DC 10 + 1/2 character level (at most +9) + Constitution modifier).");
			((BlueprintUnitFact)bp).m_Icon = Icon_DragonBreath;
			bp.Type = AbilityType.Supernatural;
			bp.Range = AbilityRange.Projectile;
			bp.CanTargetPoint = true;
			bp.CanTargetEnemies = true;
			bp.CanTargetFriends = true;
			bp.CanTargetSelf = true;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.BreathWeapon;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.AddComponent(delegate(SpellDescriptorComponent c)
			{
				c.Descriptor = SpellDescriptor.Fire | SpellDescriptor.BreathWeapon;
			});
			bp.AddComponent(delegate(AbilityDeliverProjectile c)
			{
				c.m_Projectiles = new BlueprintProjectileReference[1] { BlueprintTools.GetBlueprint<BlueprintProjectile>("52c3a84f628ddde4dbfb38e4a581b01a")?.ToReference<BlueprintProjectileReference>() };
				c.Type = AbilityProjectileType.Cone;
				c.m_Length = 30.Feet();
				c.m_LineWidth = 5.Feet();
				c.NeedAttackRoll = false;
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
				c.m_Progression = ContextRankProgression.OnePlusDiv2;
			});
			bp.AddComponent(delegate(ContextCalculateAbilityParams c)
			{
				c.StatType = StatType.Constitution;
				c.ReplaceCasterLevel = false;
				c.ReplaceSpellLevel = false;
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
			bp.SetDescription(Main.IsekaiContext, "Reincarnated carrying the primordial majesty and unmatched might of the supreme dragon lords. Their scales deflect the sharpest blades, and their breath turns armies to cinder.\nThe Dragon Lord gains a +4 racial bonus to Strength and a +2 racial bonus to Constitution, which replace the ability score adjustments of an elf or dwarf. They have a +2 natural armor bonus to AC, a draconic claw attack (1d6), immunity to fire, and red draconic wings that grant a +3 dodge bonus to AC against melee attacks and immunity to difficult terrain.\nDragon Breath: 3 times per day, a 30-foot cone of fire deals 1d6 damage plus 1d6 per 2 character levels (Reflex half, DC 10 + 1/2 character level (at most +9) + Constitution modifier).");
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
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.NaturalArmor;
				c.Stat = StatType.AC;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddAdditionalLimb c)
			{
				c.m_Weapon = DragonClaw.ToReference<BlueprintItemWeaponReference>();
			});
			bp.AddComponent(delegate(AddEnergyImmunity c)
			{
				c.Type = DamageEnergyType.Fire;
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
					DraconicWingsFeature.ToReference<BlueprintUnitFactReference>()
				};
			});
			string[] array = new string[2] { "DevourerArchetype", "OverlordArchetype" };
			foreach (string archetypeName in array)
			{
				bp.AddComponent(delegate(PrerequisiteNoArchetype c)
				{
					c.m_CharacterClass = BlueprintTools.GetModBlueprintReference<BlueprintCharacterClassReference>(Main.IsekaiContext, "IsekaiProtagonistClass");
					c.m_Archetype = BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, archetypeName);
				});
			}
			bp.Groups = new FeatureGroup[1] { FeatureGroup.Racial };
			bp.ReapplyOnLevelUp = true;
		});
		ContentOffering.OfferInSelection(FeatTools.Selections.ElvenHeritageSelection, feature, OfferedContent.IsekaiHeritages);
		BlueprintFeatureSelection blueprint = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("fd6e1f53589049cbbbc6a8e058d83b74");
		if (blueprint != null)
		{
			ContentOffering.OfferInSelection(blueprint, feature, OfferedContent.IsekaiHeritages);
		}
		BlueprintFeatureSelection blueprint2 = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("8c3244440e0b4d1d9d9b182685cbacbd");
		if (blueprint2 != null)
		{
			ContentOffering.OfferInSelection(blueprint2, feature, OfferedContent.IsekaiHeritages);
		}
		HumanHeritageSelection.Register(feature);
	}
}
