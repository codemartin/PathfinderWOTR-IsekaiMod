using IsekaiMod.Config;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
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

internal class IsekaiTitanDwarfHeritage
{
	public static void Add()
	{
		BlueprintBuff ProneBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("24cf3deb078d3df4d92ba24b176bda97");
		Sprite Icon_Stomp = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("41a86940915b4181ba07668e223a5e65"))?.m_Icon ?? ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("e48638596c955a74c8a32dbc90b518c1"))?.m_Icon;
		BlueprintAbilityResource EarthquakeStompResource = Helpers.CreateBlueprint(Main.IsekaiContext, "EarthquakeStompResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = new BlueprintAbilityResource.Amount
			{
				BaseValue = 3,
				IncreasedByLevel = false,
				IncreasedByStat = false
			};
		});
		BlueprintAbility EarthquakeStompAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "EarthquakeStompAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Earthquake Stomp");
			bp.SetDescription(Main.IsekaiContext, "Slam the earth with primeval titanic force as a standard action. Enemies within 20 feet take 1d6 bludgeoning damage per 2 character levels and must succeed at a Fortitude saving throw (DC 10 + 1/2 character level (at most +9) + Constitution modifier) or be knocked prone.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Stomp;
			bp.Type = AbilityType.Special;
			bp.Range = AbilityRange.Personal;
			bp.CanTargetSelf = true;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.AddComponent(delegate(AbilityTargetsAround c)
			{
				c.m_Radius = 20.Feet();
				c.m_TargetType = TargetType.Enemy;
				c.m_IncludeDead = false;
			});
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(new ContextActionDealDamage
				{
					DamageType = new DamageTypeDescription
					{
						Type = DamageType.Physical,
						Physical = new DamageTypeDescription.PhysicalData
						{
							Material = (PhysicalDamageMaterial)0,
							Form = PhysicalDamageForm.Bludgeoning
						}
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
					IsAoE = true
				}, new ContextActionSavingThrow
				{
					Type = SavingThrowType.Fortitude,
					Actions = Helpers.CreateActionList(new ContextActionConditionalSaved
					{
						Succeed = ActionFlow.DoNothing(),
						Failed = ActionFlow.DoSingle(delegate(ContextActionApplyBuff a)
						{
							a.m_Buff = ProneBuff.ToReference<BlueprintBuffReference>();
							a.DurationValue = new ContextDurationValue
							{
								Rate = DurationRate.Rounds,
								DiceType = DiceType.Zero,
								BonusValue = 1
							};
						})
					})
				});
			});
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.Default;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				c.m_Progression = ContextRankProgression.Div2;
			});
			bp.AddComponent(delegate(ContextCalculateAbilityParams c)
			{
				c.StatType = StatType.Constitution;
				c.ReplaceCasterLevel = false;
				c.ReplaceSpellLevel = false;
			});
			bp.AddComponent(delegate(AbilityResourceLogic c)
			{
				c.m_RequiredResource = EarthquakeStompResource.ToReference<BlueprintAbilityResourceReference>();
				c.m_IsSpendResource = true;
				c.Amount = 1;
			});
		});
		BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiTitanDwarfHeritage", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Titan Dwarf");
			bp.SetDescription(Main.IsekaiContext, "Reincarnated carrying the blood of primeval subterranean titans who forged the world's deep foundations. Their stone-hardened bodies can shrug off blows that would shatter mountains.\nThe Titan Dwarf gains a +4 racial bonus to Constitution and a +2 racial bonus to Wisdom, which replace the dwarf's ability score adjustments. They have DR 5/adamantine, a +2 racial bonus on damage rolls, a +2 natural armor bonus to AC, and a +4 bonus to Combat Maneuver Defense against Bull Rush and Trip.\nEarthquake Stomp: 3 times per day, enemies within 20 feet take 1d6 bludgeoning damage per 2 character levels and must succeed at a Fortitude save (DC 10 + 1/2 character level (at most +9) + Constitution modifier) or be knocked prone.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Stomp;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Constitution;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Wisdom;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 5;
				c.BypassedByMaterial = true;
				c.Material = PhysicalDamageMaterial.Adamantite;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.NaturalArmor;
				c.Stat = StatType.AC;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddAbilityResources c)
			{
				c.m_Resource = EarthquakeStompResource.ToReference<BlueprintAbilityResourceReference>();
				c.RestoreAmount = true;
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { EarthquakeStompAbility.ToReference<BlueprintUnitFactReference>() };
			});
			bp.Groups = new FeatureGroup[1] { FeatureGroup.Racial };
			bp.ReapplyOnLevelUp = true;
		});
		BlueprintFeatureSelection blueprint = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("fd6e1f53589049cbbbc6a8e058d83b74");
		if (blueprint != null)
		{
			ContentOffering.OfferInSelection(blueprint, feature, OfferedContent.IsekaiHeritages);
		}
	}
}
