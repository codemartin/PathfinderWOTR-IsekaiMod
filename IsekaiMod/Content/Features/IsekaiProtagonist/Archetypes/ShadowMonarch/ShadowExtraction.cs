using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch
{
	internal class ShadowExtraction
	{
		private static readonly BlueprintUnit ShadowSoldierUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("7237a32613fe55e479d1141682f2bbd4") ?? BlueprintTools.GetBlueprint<BlueprintUnit>("7121303d0f344a5abb3b43b0c9cef8e4");

		private static readonly BlueprintSummonPool ShadowSummonPool = BlueprintTools.GetBlueprint<BlueprintSummonPool>("d94c93e7240f10e41ae41db4c83d1cbe");

		private static readonly Sprite Icon_Arise = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("52b5df2a97df18242aec67610616ded0"))?.m_Icon;

		public static void Add()
		{
			BlueprintSummonPool ShadowExtractionPool = Helpers.CreateBlueprint<BlueprintSummonPool>(Main.IsekaiContext, "ShadowExtractionPool");
			BlueprintBuff shadowFXBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("8caafe9dc0ff21041b36ad225569d164");
			BlueprintBuff legendShadowBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("faf2133d48b641149343f4dac75fba47");
			BlueprintUnit ShadowSoldierSummoned = ShadowSoldierUnit?.CreateCopy(Main.IsekaiContext, "ShadowSoldierSummoned", delegate(BlueprintUnit bp)
			{
				bp.SetLocalisedName(Main.IsekaiContext, "Extracted Shadow Soldier");
				bp.m_Type = BlueprintTools.GetBlueprintReference<BlueprintUnitTypeReference>("d0aa7a6da15f0d3498b046bfaec72c9a");
				bp.m_Portrait = BlueprintTools.GetBlueprintReference<BlueprintPortraitReference>("4ee2f49b8baf48fabe567465618bf3ce");
				bp.m_Race = null;
				List<BlueprintUnitFactReference> list = (bp.m_AddFacts ?? new BlueprintUnitFactReference[0]).ToList();
				if (shadowFXBuff != null)
				{
					list.Add(shadowFXBuff.ToReference<BlueprintUnitFactReference>());
				}
				if (legendShadowBuff != null)
				{
					list.Add(legendShadowBuff.ToReference<BlueprintUnitFactReference>());
				}
				bp.m_AddFacts = list.ToArray();
			});
			BlueprintBuff ShadowSoldierBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowSoldierBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Soldier: Extracted Essence");
				bp.SetDescription(Main.IsekaiContext, "A loyal warrior risen from the shadows of death. Cloaked in an ominous shadow silhouette, deals additional unholy and cold damage on all attacks, gains a +10 ft speed bonus, and is immune to fear and mind-affecting effects.");
				// Immunities named in the description.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.MindAffecting;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.MindAffecting;
				});
				((BlueprintUnitFact)bp).m_Icon = Icon_Arise;
				bp.IsClassFeature = true;
				bp.FxOnStart = new PrefabLink
				{
					AssetId = "6d63fc82e26d16842af4024295a1f6f8"
				};
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
			});
			BlueprintAbilityResource ShadowExtractionResource = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowExtractionResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevel = false,
					IncreasedByStat = true,
					ResourceBonusStat = StatType.Charisma
				};
			});
			BlueprintAbility ShadowExtractionAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowExtractionAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Extraction: Arise");
				bp.SetDescription(Main.IsekaiContext, "The Shadow Monarch's supreme sovereign command: 'Arise!'\nExtracts the shadow of a fallen warrior, summoning a loyal Shadow Soldier to fight at your side for 1 minute per character level. Active shadows are capped at 1 soldier (increases to 2 at level 10, and 3 at level 20). Usable 3 + Charisma modifier times per day.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Arise;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Medium;
				bp.CanTargetPoint = true;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.AvailableMetamagic = Metamagic.Quicken | Metamagic.Extend;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.OneMinutePerLevel;
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = ShadowExtractionResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionEnforceShadowExtractionCap
					{
						m_SummonPool = ShadowExtractionPool.ToReference<BlueprintSummonPoolReference>()
					}, new ContextActionOnNearbyPoint
					{
						Actions = Helpers.CreateActionList(new ContextActionSpawnMonster
						{
							m_Blueprint = (ShadowSoldierSummoned ?? ShadowSoldierUnit)?.ToReference<BlueprintUnitReference>(),
							m_SummonPool = ShadowExtractionPool.ToReference<BlueprintSummonPoolReference>(),
							DurationValue = new ContextDurationValue
							{
								Rate = DurationRate.Minutes,
								DiceType = DiceType.Zero,
								DiceCountValue = 0,
								BonusValue = Values.CreateContextRankValue(AbilityRankType.Default),
								m_IsExtendable = true
							},
							CountValue = Values.Dice.One,
							LevelValue = 0,
							AfterSpawn = Helpers.CreateActionList(new ContextActionApplyBuff
							{
								Permanent = true,
								m_Buff = ShadowSoldierBuff.ToReference<BlueprintBuffReference>(),
								DurationValue = Values.Duration.Zero,
								IsNotDispelable = true
							}, new ContextActionApplyBuff
							{
								Permanent = true,
								m_Buff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("8caafe9dc0ff21041b36ad225569d164"),
								DurationValue = Values.Duration.Zero,
								IsNotDispelable = true
							}, new ContextActionApplyBuff
							{
								Permanent = true,
								m_Buff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("faf2133d48b641149343f4dac75fba47"),
								DurationValue = Values.Duration.Zero,
								IsNotDispelable = true
							})
						})
					});
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowExtractionFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Extraction: Arise");
				bp.SetDescription(Main.IsekaiContext, "At 1st level, the Shadow Monarch gains the ability to extract the souls of the fallen, commanding them: 'Arise'. Usable 3 + Charisma modifier times per day.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Arise;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = ShadowExtractionResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { ShadowExtractionAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
