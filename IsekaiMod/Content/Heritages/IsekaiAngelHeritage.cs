using System.Collections.Generic;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages
{
	internal class IsekaiAngelHeritage
	{
		public static void Add()
		{
			// USES-PER-DAY FIX: the ability used to borrow a base-game racial spell-like resource
			// that is only granted by (and only counts levels of) the original race's features, so an
			// Isekai character never registered a pool and the ability was unusable. Dedicated
			// resource counting Isekai Protagonist levels, granted below by AddAbilityResources.
			BlueprintAbilityResource AngelicBoltResource = Helpers.CreateBlueprint(Main.IsekaiContext, "AngelicBoltResource", delegate(BlueprintAbilityResource resource)
			{
				resource.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 1,
					IncreasedByLevel = true,
					LevelIncrease = 1,
					m_Class = new BlueprintCharacterClassReference[1] { IsekaiProtagonistClass.GetReference() }
				};
			});
			BlueprintAbility AngelBoltOfJusticeAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("c82168800b665324f8b4807b531fea46");
			BlueprintFeature AngelWingsFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("d9bd0fde6deb2e44a93268f2dfb3e169");
			BlueprintActivatableAbility BlackWingsAbility = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(Main.IsekaiContext, "BlackWingsAbility");
			BlueprintActivatableAbility GhostWingsAbility = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(Main.IsekaiContext, "GhostWingsAbility");
			BlueprintUnitProperty AngelicBoltUnitProperty = Helpers.CreateBlueprint(Main.IsekaiContext, "AngelicBoltUnitProperty", delegate(BlueprintUnitProperty bp)
			{
				bp.name = "AngelicBoltUnitProperty";
				bp.AddComponent(delegate(SimplePropertyGetter c)
				{
					c.Property = UnitProperty.Level;
				});
				bp.AddComponent(delegate(SimplePropertyGetter c)
				{
					c.Property = UnitProperty.StatBonusCharisma;
				});
				bp.BaseValue = 10;
				bp.OperationOnComponents = BlueprintUnitProperty.MathOperation.Sum;
			});
			BlueprintAbility AngelicBoltAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "AngelicBoltAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Angelic Bolt");
				bp.SetDescription(Main.IsekaiContext, "You release a powerful stroke of energy that deals {g|Encyclopedia:Dice}2d6{/g} points of holy {g|Encyclopedia:Damage}damage{/g} per character level. The target needs to make a successful Reflex saving throw, or become prone.\nIf the target is evil, the {g|Encyclopedia:Spell}spell{/g} instead deals 2d8 points of holy {g|Encyclopedia:Energy_Damage}damage{/g} per character level. The target needs to make a successful Reflex saving throw, or become prone and suffer a -2 {g|Encyclopedia:Penalty}penalty{/g} to {g|Encyclopedia:Armor_Class}AC{/g}, {g|Encyclopedia:Attack}attack rolls{/g} and saving throws.\nIf the target is an evil outsider or an undead creature, the spell instead deals 2d10 points of holy damage per character level. The target needs to make a successful Reflex saving throw, or become prone and suffer a -4 penalty to AC, attack rolls and saving throws.\nIf the target is a demon lord, an evil dragon or a lord of undead (a powerful undead creature like liches, undead dragons, nightshades and similar), the spell instead deals 2d12 points of holy damage per character level. The target suffers a -4 penalty to AC, attack rolls and saving throws. It also needs to make a successful Reflex saving throw, or become prone.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)AngelBoltOfJusticeAbility).m_Icon;
				bp.AddComponent(AngelBoltOfJusticeAbility.GetComponent<AbilityEffectRunAction>());
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.DoublePlusBonusValue;
				});
				bp.AddComponent(delegate(SpellComponent c)
				{
					c.School = SpellSchool.Conjuration;
				});
				bp.AddComponent(delegate(AbilityDeliverDelay c)
				{
					c.DelaySeconds = 1.5f;
				});
				bp.AddComponent(delegate(AbilitySpawnFx c)
				{
					c.PrefabLink = new PrefabLink
					{
						AssetId = "5c1ded6985c9c15448f1dc1ad90dbd80"
					};
					c.Time = AbilitySpawnFxTime.OnPrecastFinished;
					c.Anchor = AbilitySpawnFxAnchor.ClickedTarget;
				});
				bp.AddComponent(delegate(ContextSetAbilityParams c)
				{
					c.DC = Values.CreateContextCasterCustomPropertyValue(AngelicBoltUnitProperty);
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = AngelicBoltResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
					c.ResourceCostIncreasingFacts = new List<BlueprintUnitFactReference>();
					c.ResourceCostDecreasingFacts = new List<BlueprintUnitFactReference>();
				});
				bp.Type = AbilityType.SpellLike;
				bp.Range = AbilityRange.Long;
				bp.CanTargetEnemies = true;
				bp.CanTargetFriends = true;
				bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
				bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AvailableMetamagic = AngelBoltOfJusticeAbility.AvailableMetamagic;
				bp.LocalizedDuration = StaticReferences.Strings.Null;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
			});
			Sprite Icon_Angel = AssetLoader.LoadInternal(Main.IsekaiContext, "Heritages", "ICON_ANGEL.png");
			BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiAngelHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Isekai Angel");
				bp.SetDescription(Main.IsekaiContext, "Otherworldly entities who are reincarnated into the world of Golarion as an Angel have both extreme beauty and power. They serve as exemplars of good and light regardless of the myriad forms they may take.\nThe Isekai Angel has a +4 racial {g|Encyclopedia:Bonus}bonus{/g} to {g|Encyclopedia:Strength}Strength{/g} and {g|Encyclopedia:Charisma}Charisma{/g}, and a +2 racial bonus on {g|Encyclopedia:Persuasion}Persuasion{/g} and {g|Encyclopedia:Lore_Religion}Lore (religion){/g} checks. They have DR 10/Evil, and have spell resistance equal to 10 + their character level. They have immunity to acid, cold, and petrification as well as fire and electricity resistance 20. They can also use the Angelic Bolt ability a number of times per day equal to 1 + their Isekai Protagonist level.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Charisma;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.SkillLoreReligion;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.SkillPersuasion;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Or = true;
					c.Value = 10;
					c.BypassedByAlignment = true;
					c.Alignment = DamageAlignment.Evil;
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
					c.m_StepLevel = 10;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Acid;
					c.Value = 20;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = 20;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Fire;
					c.Value = 20;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Electricity;
					c.Value = 20;
				});
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.Acid;
				});
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.Cold;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Acid | SpellDescriptor.Cold | SpellDescriptor.Petrified;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Acid | SpellDescriptor.Cold | SpellDescriptor.Petrified;
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = AngelicBoltResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[4]
					{
						AngelicBoltAbility.ToReference<BlueprintUnitFactReference>(),
						AngelWingsFeature.ToReference<BlueprintUnitFactReference>(),
						BlackWingsAbility.ToReference<BlueprintUnitFactReference>(),
						GhostWingsAbility.ToReference<BlueprintUnitFactReference>()
					};
				});
				bp.Groups = new FeatureGroup[2]
				{
					FeatureGroup.Racial,
					FeatureGroup.AasimarHeritage
				};
				bp.ReapplyOnLevelUp = true;
			});
			FeatTools.Selections.AasimarHeritageSelection.AddToSelection(feature);
		}
	}
}
