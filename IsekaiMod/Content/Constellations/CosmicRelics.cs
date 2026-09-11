using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Constellations
{
	internal class CosmicRelics
	{
		private static readonly Sprite Icon_Coin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");

		private static readonly BlueprintUnit AstralDragonUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("25b5c837d4b01b1498e541b032c32e4d");

		private static readonly BlueprintSummonPool SummonMonsterPool = BlueprintTools.GetBlueprint<BlueprintSummonPool>("d94c93e7240f10e41ae41db4c83d1cbe");

		private static readonly BlueprintBuff SummonedCreatureSpawnMonsterVI_IX = BlueprintTools.GetBlueprint<BlueprintBuff>("0dff842f06edace43baf8a2f44207045");

		public static BlueprintFeature CosmicCrownOfApotheosisFeature;

		public static BlueprintFeature CosmicRingOfOmnipresenceFeature;

		public static BlueprintFeature CosmicMythicPactFeature;

		public static BlueprintFeature CosmicAstralDragonFeature;

		public static void Add()
		{
			CosmicCrownOfApotheosisFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicCrownOfApotheosisFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Cosmic Relic: Crown of Apotheosis");
				bp.SetDescription(Main.IsekaiContext, "An astral crown forged from condensed stellar supernovas.\nGrants a +6 enhancement bonus to Intelligence, Wisdom, and Charisma, a +4 sacred bonus to all spell save DCs, and +10 Spell Resistance.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Intelligence;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Wisdom;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Charisma;
					c.Value = 6;
				});
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 4;
					c.Descriptor = ModifierDescriptor.Sacred;
				});
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = 10;
				});
			});
			CosmicRingOfOmnipresenceFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicRingOfOmnipresenceFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Cosmic Relic: Ring of Omnipresence");
				bp.SetDescription(Main.IsekaiContext, "A cosmic ring that allows its wearer to slip through the seams of physical reality.\nGrants permanent Freedom of Movement, a +5 deflection bonus to Armor Class, and a +5 sacred bonus to all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Paralyzed;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.CantMove;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Deflection;
					c.Stat = StatType.AC;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveFortitude;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = 5;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 5;
				});
			});
			CosmicMythicPactFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicMythicPactFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Cosmic Boon: Rulebreaker Mythic Pact");
				bp.SetDescription(Main.IsekaiContext, "You have purchased the absolute authority of the cosmos to dictate your own destiny.\nGrants a +3 bonus to your Caster Level for all spells, a +4 sacred bonus to all attack rolls, and +4 sacred bonus to all damage rolls.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(IncreaseCasterLevel c)
				{
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 4;
				});
			});
			BlueprintAbilityResource DragonResource = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicAstralDragonResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 1,
					IncreasedByLevel = false,
					IncreasedByStat = false
				};
			});
			BlueprintAbility DragonAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicAstralDragonSummonAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Summon Astral Sovereign Dragon");
				bp.SetDescription(Main.IsekaiContext, "Summons an ancient Astral Sovereign Dragon to fight at your side for 1 hour. The dragon wields breath attacks, divine magic, and terrifying natural attacks.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Close;
				bp.CanTargetPoint = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.LocalizedDuration = Helpers.CreateString(Main.IsekaiContext, "CosmicAstralDragonAbility.Duration", "1 hour");
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionSpawnMonster
					{
						m_Blueprint = AstralDragonUnit.ToReference<BlueprintUnitReference>(),
						m_SummonPool = SummonMonsterPool.ToReference<BlueprintSummonPoolReference>(),
						DurationValue = new ContextDurationValue
						{
							Rate = DurationRate.Hours,
							DiceType = DiceType.Zero,
							DiceCountValue = 0,
							BonusValue = 1,
							m_IsExtendable = true
						},
						CountValue = Values.Dice.One,
						LevelValue = 0,
						AfterSpawn = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
						{
							b.Permanent = true;
							b.m_Buff = SummonedCreatureSpawnMonsterVI_IX.ToReference<BlueprintBuffReference>();
							b.DurationValue = Values.Duration.Zero;
						})
					});
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = DragonResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
			});
			CosmicAstralDragonFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicAstralDragonFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Cosmic Companion: Astral Sovereign Dragon");
				bp.SetDescription(Main.IsekaiContext, "You have forged an eternal pact with an Astral Sovereign Dragon.\nBenefit: You gain the ability to summon an ancient Astral Sovereign Dragon to fight at your side for 1 hour once per day.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { DragonAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = DragonResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
			});
		}
	}
}
