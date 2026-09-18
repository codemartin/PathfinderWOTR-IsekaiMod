using IsekaiMod.Utilities;
using Kingmaker.Assets.UnitLogic.Mechanics.Properties;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
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
	internal class IsekaiWerewolfHeritage
	{
		public static void Add()
		{
			BlueprintBuff ShakenBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("25ec6cb6ab1845c48a95f9c20b034220");
			Sprite Icon_Howl = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("87ab3793abc347d5a47fc6b7fec8dfcb")).m_Icon;
			BlueprintItemWeapon Bite1d8 = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("61bc14eca5f8c1040900215000cfc218");
			BlueprintAbilityResource HowlOfFenrirResource = Helpers.CreateBlueprint(Main.IsekaiContext, "HowlOfFenrirResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevel = false,
					IncreasedByStat = false
				};
			});
			BlueprintUnitProperty HowlOfFenrirUnitProperty = Helpers.CreateBlueprint(Main.IsekaiContext, "HowlOfFenrirUnitProperty", delegate(BlueprintUnitProperty bp)
			{
				bp.name = "HowlOfFenrirUnitProperty";
				bp.AddComponent(delegate(ComplexPropertyGetter c)
				{
					c.Property = UnitProperty.Level;
					c.Denominator = 2;
					c.Multiplier = 1;
					c.Bonus = 0;
				});
				bp.AddComponent(delegate(SimplePropertyGetter c)
				{
					c.Property = UnitProperty.StatBonusCharisma;
				});
				bp.BaseValue = 10;
				bp.OperationOnComponents = BlueprintUnitProperty.MathOperation.Sum;
			});
			BlueprintAbility HowlOfFenrirAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "HowlOfFenrirAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Howl of Fenrir");
				bp.SetDescription(Main.IsekaiContext, "Unleash a terrifying primordial howl as a swift action. Enemies within 30 feet must succeed at a Will save (DC 10 + 1/2 character level + Charisma modifier) or become shaken for 1d4 rounds.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Howl;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Swift;
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
						Type = SavingThrowType.Will,
						Actions = Helpers.CreateActionList(new ContextActionConditionalSaved
						{
							Succeed = ActionFlow.DoNothing(),
							Failed = ActionFlow.DoSingle(delegate(ContextActionApplyBuff a)
							{
								a.m_Buff = ShakenBuff.ToReference<BlueprintBuffReference>();
								a.DurationValue = new ContextDurationValue
								{
									Rate = DurationRate.Rounds,
									DiceType = DiceType.D4,
									DiceCountValue = 1,
									BonusValue = 0
								};
							})
						})
					});
				});
				bp.AddComponent(delegate(ContextSetAbilityParams c)
				{
					c.DC = Values.CreateContextCasterCustomPropertyValue(HowlOfFenrirUnitProperty);
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = HowlOfFenrirResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
			});
			BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiWerewolfHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Werewolf Lord");
				bp.SetDescription(Main.IsekaiContext, "Reincarnated carrying the primordial savagery of the mythical wolf god Fenrir. Werewolf Lords blend untamed instinct with supreme predatory power.\nThe Werewolf Lord gains a +4 racial bonus to Strength, a +2 racial bonus to Dexterity and Constitution, a primary Bite natural attack dealing 1d8 damage, DR 10/Silver, a +2 bonus on attack rolls when flanking an opponent, and the Howl of Fenrir ability usable 3 times per day.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Howl;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Dexterity;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Constitution;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddAdditionalLimb c)
				{
					c.m_Weapon = Bite1d8.ToReference<BlueprintItemWeaponReference>();
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 10;
					c.BypassedByMaterial = true;
					c.Material = PhysicalDamageMaterial.Silver;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Value = 2;
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.AdditionalAttackBonus;
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = HowlOfFenrirResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { HowlOfFenrirAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.Groups = new FeatureGroup[1] { FeatureGroup.Racial };
				bp.ReapplyOnLevelUp = true;
			});
			FeatTools.Selections.KitsuneHeritageSelection.AddToSelection(feature);
			BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("8c3244440e0b4d1d9d9b182685cbacbd")?.AddToSelection(feature);
			HumanHeritageSelection.Register(feature);
		}
	}
}
