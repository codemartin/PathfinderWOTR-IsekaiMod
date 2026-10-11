using System.Collections.Generic;
using IsekaiMod.Config;
using IsekaiMod.Utilities;
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
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages;

internal class IsekaiAngelHeritage
{
	public static void Add()
	{
		BlueprintAbilityResource AasimarSpellLikeResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("a4ea5b9becd98dd47b51c8742aeb70ec");
		BlueprintAbility AngelBoltOfJusticeAbility = BlueprintTools.GetBlueprint<BlueprintAbility>("c82168800b665324f8b4807b531fea46");
		BlueprintFeature AngelWingsFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("d9bd0fde6deb2e44a93268f2dfb3e169");
		BlueprintActivatableAbility BlackWingsAbility = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(Main.IsekaiContext, "BlackWingsAbility");
		BlueprintActivatableAbility GhostWingsAbility = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(Main.IsekaiContext, "GhostWingsAbility");
		Helpers.CreateBlueprint(Main.IsekaiContext, "AngelicBoltUnitProperty", delegate(BlueprintUnitProperty bp)
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
			bp.SetDescription(Main.IsekaiContext, "Once per day, you release a stroke of holy energy that deals {g|Encyclopedia:Dice}1d6{/g} points of holy {g|Encyclopedia:Damage}damage{/g} per 2 character levels (at least 1 die, at most 20). The target needs to make a successful Reflex saving throw (DC 10 + 1/2 character level (at most +9) + Charisma modifier), or become prone.\nIf the target is evil, the dice are d8s instead, and a failed save also gives a -2 {g|Encyclopedia:Penalty}penalty{/g} to {g|Encyclopedia:Armor_Class}AC{/g}, {g|Encyclopedia:Attack}attack rolls{/g} and saving throws.\nIf the target is an evil outsider or an undead creature, the dice are d10s, and a failed save gives a -4 penalty to AC, attack rolls and saving throws.\nIf the target is a demon lord, an evil dragon or a lord of undead (a powerful undead creature like liches, undead dragons, nightshades and similar), the dice are d12s and the target suffers a -4 penalty to AC, attack rolls and saving throws. It also needs to make a successful Reflex saving throw, or become prone.");
			((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)AngelBoltOfJusticeAbility).m_Icon;
			bp.AddComponent(AngelBoltOfJusticeAbility.GetComponent<AbilityEffectRunAction>());
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.Default;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				c.m_Progression = ContextRankProgression.Div2;
				c.m_UseMin = true;
				c.m_Min = 1;
				c.m_UseMax = true;
				c.m_Max = 20;
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
			bp.AddComponent(delegate(ContextCalculateAbilityParams c)
			{
				c.StatType = StatType.Charisma;
				c.ReplaceCasterLevel = false;
				c.ReplaceSpellLevel = false;
			});
			bp.AddComponent(delegate(AbilityResourceLogic c)
			{
				c.m_RequiredResource = AasimarSpellLikeResource.ToReference<BlueprintAbilityResourceReference>();
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
			bp.SetDescription(Main.IsekaiContext, "Otherworldly entities who are reincarnated into the world of Golarion as an Angel have both extreme beauty and power. They serve as exemplars of good and light regardless of the myriad forms they may take.\nThe Isekai Angel has a +4 racial {g|Encyclopedia:Bonus}bonus{/g} to {g|Encyclopedia:Charisma}Charisma{/g}, a +2 racial bonus to {g|Encyclopedia:Strength}Strength{/g}, and a +2 racial bonus on {g|Encyclopedia:Persuasion}Persuasion{/g} and {g|Encyclopedia:Lore_Religion}Lore (religion){/g} checks. They have DR 5/evil, acid and cold resistance 10, and immunity to petrification, and can spread angel, black or ghost wings for a +3 dodge bonus to AC against melee attacks.\nAngelic Bolt: once per day, a bolt of holy energy deals 1d6 per 2 character levels (at most 20 dice; larger dice against evil foes) and knocks the target prone on a failed Reflex save (DC 10 + 1/2 character level (at most +9) + Charisma modifier).");
			((BlueprintUnitFact)bp).m_Icon = Icon_Angel;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Strength;
				c.Value = 2;
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
				c.Value = 5;
				c.BypassedByAlignment = true;
				c.Alignment = DamageAlignment.Evil;
			});
			bp.AddComponent(delegate(AddDamageResistanceEnergy c)
			{
				c.Type = DamageEnergyType.Acid;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddDamageResistanceEnergy c)
			{
				c.Type = DamageEnergyType.Cold;
				c.Value = 10;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Petrified;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Petrified;
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
		ContentOffering.OfferInSelection(FeatTools.Selections.AasimarHeritageSelection, feature, OfferedContent.IsekaiHeritages);
	}
}
