using System.Collections.Generic;
using IsekaiMod.Config;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages;

internal class IsekaiSuccubusHeritage
{
	private static readonly BlueprintFeature DestinyBeyondBirthMythicFeat = BlueprintTools.GetBlueprint<BlueprintFeature>("325f078c584318849bfe3da9ea245b9d");

	private static readonly BlueprintBuff DominatePersonBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("c0f4e1c24c9cd334ca988ed1bd9d201f");

	private static readonly BlueprintAbilityResource TieflingSpellLikeResource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("803d7e39e05fa2a47a7e2424d0e4b623");

	public static void Add()
	{
		Sprite Icon_Charm = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_CHARM.png");
		Helpers.CreateBlueprint(Main.IsekaiContext, "SuccubusCharmUnitProperty", delegate(BlueprintUnitProperty bp)
		{
			bp.name = "SuccubusCharmUnitProperty";
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
		BlueprintAbility SuccubusCharmAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SuccubusCharmAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Demonic Charm");
			bp.SetDescription(Main.IsekaiContext, "Once per day, you can dominate any creature for 1 minute per character level unless it succeeds on a {g|Encyclopedia:Saving_Throw}Will save{/g} (DC 10 + 1/2 character level (at most +9) + Charisma modifier). It fights on your side as if it was your ally and will {g|Encyclopedia:Attack}attack{/g} your opponents to the best of its ability. However this creature will try to throw off the domination effect, making a Will save each {g|Encyclopedia:Combat_Round}round{/g}.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Charm;
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.SavingThrowType = SavingThrowType.Will;
				c.Actions = ActionFlow.DoSingle(delegate(ContextActionConditionalSaved contextActionConditionalSaved)
				{
					contextActionConditionalSaved.Succeed = ActionFlow.DoNothing();
					contextActionConditionalSaved.Failed = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
					{
						contextActionApplyBuff.m_Buff = DominatePersonBuff.ToReference<BlueprintBuffReference>();
						contextActionApplyBuff.DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Minutes,
							m_IsExtendable = true,
							DiceCountValue = 0,
							BonusValue = Values.CreateContextRankValue(AbilityRankType.Default)
						};
					});
				});
			});
			bp.AddComponent(delegate(SpellComponent c)
			{
				c.School = SpellSchool.Enchantment;
			});
			bp.AddComponent(delegate(SpellDescriptorComponent c)
			{
				c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion;
			});
			bp.AddComponent(delegate(AbilitySpawnFx c)
			{
				c.PrefabLink = new PrefabLink
				{
					AssetId = "c14a2f46018cb0e41bfeed61463510ff"
				};
				c.Time = AbilitySpawnFxTime.OnApplyEffect;
				c.Anchor = AbilitySpawnFxAnchor.SelectedTarget;
			});
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.Default;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
			});
			bp.AddComponent(delegate(ContextCalculateAbilityParams c)
			{
				c.StatType = StatType.Charisma;
				c.ReplaceCasterLevel = false;
				c.ReplaceSpellLevel = false;
			});
			bp.AddComponent(delegate(AbilityResourceLogic c)
			{
				c.m_RequiredResource = TieflingSpellLikeResource.ToReference<BlueprintAbilityResourceReference>();
				c.m_IsSpendResource = true;
				c.CostIsCustom = false;
				c.Amount = 1;
				c.ResourceCostIncreasingFacts = new List<BlueprintUnitFactReference>();
				c.ResourceCostDecreasingFacts = new List<BlueprintUnitFactReference>();
			});
			bp.Type = AbilityType.SpellLike;
			bp.Range = AbilityRange.Medium;
			bp.CanTargetEnemies = true;
			bp.CanTargetFriends = true;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.AvailableMetamagic = Metamagic.Heighten | Metamagic.Reach | Metamagic.CompletelyNormal;
			bp.LocalizedDuration = StaticReferences.Strings.Duration.OneMinutePerLevel;
			bp.LocalizedSavingThrow = StaticReferences.Strings.SavingThrow.WillNegates;
		});
		BlueprintActivatableAbility DevilWingsAbility = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(Main.IsekaiContext, "DevilWingsAbility");
		BlueprintActivatableAbility DemonWingsAbility = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(Main.IsekaiContext, "DemonWingsAbility");
		BlueprintActivatableAbility BlackWingsAbility = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(Main.IsekaiContext, "BlackWingsAbility");
		Sprite Icon_Succubus = AssetLoader.LoadInternal(Main.IsekaiContext, "Heritages", "ICON_SUCCUBUS.png");
		BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiSuccubusHeritage", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Isekai Lust Demon");
			bp.SetDescription(Main.IsekaiContext, "Otherworldly entities who are reincarnated into the world of Golarion as a Lust Demon have both extreme beauty and power, and often have a voracious appetite for sensory pleasures and carnal delights.\nThe Isekai Lust Demon has a +2 racial {g|Encyclopedia:Bonus}bonus{/g} to {g|Encyclopedia:Dexterity}Dexterity{/g} and {g|Encyclopedia:Intelligence}Intelligence{/g}, a +4 racial bonus to {g|Encyclopedia:Charisma}Charisma{/g}, a -2 {g|Encyclopedia:Penalty}penalty{/g} to {g|Encyclopedia:Strength}Strength{/g}, and a +2 racial bonus on {g|Encyclopedia:Persuasion}Persuasion{/g} and {g|Encyclopedia:Perception}Perception checks{/g}. They have DR 10/cold iron or good, immunity to poison, fire and electricity resistance 10, and can spread devil, demon or black wings for a +3 dodge bonus to AC against melee attacks.\nDemonic Charm: once per day, dominate any creature for 1 minute per character level (Will negates, DC 10 + 1/2 character level (at most +9) + Charisma modifier).");
			((BlueprintUnitFact)bp).m_Icon = Icon_Succubus;
			bp.AddComponent(delegate(AddStatBonusIfHasFact c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Strength;
				c.Value = -2;
				c.InvertCondition = true;
				c.m_CheckedFacts = new BlueprintUnitFactReference[1] { DestinyBeyondBirthMythicFeat.ToReference<BlueprintUnitFactReference>() };
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
				c.Stat = StatType.Intelligence;
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
				c.Stat = StatType.SkillPerception;
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
				c.BypassedByMaterial = true;
				c.BypassedByAlignment = true;
				c.Material = PhysicalDamageMaterial.ColdIron;
				c.Alignment = DamageAlignment.Good;
			});
			bp.AddComponent(delegate(AddDamageResistanceEnergy c)
			{
				c.Type = DamageEnergyType.Fire;
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddDamageResistanceEnergy c)
			{
				c.Type = DamageEnergyType.Electricity;
				c.Value = 10;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Poison;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Poison;
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[4]
				{
					SuccubusCharmAbility.ToReference<BlueprintUnitFactReference>(),
					DevilWingsAbility.ToReference<BlueprintUnitFactReference>(),
					DemonWingsAbility.ToReference<BlueprintUnitFactReference>(),
					BlackWingsAbility.ToReference<BlueprintUnitFactReference>()
				};
			});
			bp.Groups = new FeatureGroup[2]
			{
				FeatureGroup.Racial,
				FeatureGroup.TieflingHeritage
			};
			bp.ReapplyOnLevelUp = true;
		});
		ContentOffering.OfferInSelection(FeatTools.Selections.TieflingHeritageSelection, feature, OfferedContent.IsekaiHeritages);
	}
}
