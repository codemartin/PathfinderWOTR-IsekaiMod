using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Components;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class SuperBuff
	{
		private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "SuperBuff.Description", "You are able to shroud yourself and all nearby allies in a comprehensive protective aegis that scales in power and duration as your character level increases. Disruptive size-altering models and blinding particle storms have been removed to ensure seamless indoor navigation and visual clarity.\n• Tier 1 (Level 1+, 1 Hour): Shield, Mage Armor, Shield of Faith, Protection from Evil, False Life, Aid, Barkskin, Delay Poison, Remove Fear, Unbreakable Heart, Divine Favor, Magic Weapon Greater.\n• Tier 2 (Level 7+, 2 Hours): Haste, Blur, Mirror Image, Bull's Strength, Cat's Grace, Bear's Endurance, Fox's Cunning, Owl's Wisdom, Eagle's Splendor, Resist Fire/Cold/Acid/Sonic/Electricity, Protection from Arrows, Bestow Grace, Aura of Greater Courage, Displacement, Magic Fang, Align Weapon Good.\n• Tier 3 (Level 13+, 8 Hours): Death Ward, Freedom of Movement, Greater False Life, Stoneskin, Spell Resistance, Burst of Glory, True Seeing, Crusader's Edge, Divine Power, Protection from Fire/Cold/Acid/Sonic/Electricity, Magical Vestment (Armor & Shield), Echolocation, Life Bubble, Sense Vitals, Hurricane Bow.\n• Tier 4 (Level 17+, 24 Hours): Foresight, Eagle Soul, Shield of Law, Angelic Aspect Greater, Veil of Heaven, Veil of Positive Energy, Mind Blank, Protection from Spells, Heroic Invocation, Greater Heroism.\nYou gain 1 use per day at level 1, increasing to 2 uses at level 10 and 3 uses at level 20.");

		public static void Add()
		{
			BlueprintBuff[] array = new BlueprintBuff[12]
			{
				BlueprintTools.GetBlueprint<BlueprintBuff>("355be0688dabc21409f37942d637cdab"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("9c0fa9b438ada3f43864be8dd8b3e741"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("5274ddc289f4a7447b7ace68ad8bebb0"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("4a6911969911ce9499bf27dde9bfcedc"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("0fdb3cca6744fd94b9436459e6d9b947"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("319b4679f25779e4e9d04360381254e1"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("533592a86adecda4e9fd5ed37a028432"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("51ebd62ee464b1446bb01fa1e214942f"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("c5c86809a1c834e42a2eb33133e90a28"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("6603b27034f694e44a407a9cdf77c67e"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("5d2833d39901b844b828f9f13a0353fe"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("31848cce6c6246c19aa050e7e693ddee")
			};
			BlueprintBuff[] array2 = new BlueprintBuff[20]
			{
				BlueprintTools.GetBlueprint<BlueprintBuff>("03464790f40c3c24aa684b57155f3280"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("dd3ad347240624d46a11a092b4dd4674"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("98dc7e7cc6ef59f4abe20c65708ac623"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("b175001b42b1a02479881b72fe132116"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("f011d0ab4a405e54aa0e83cd10e54430"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("c3de8cc9a0f50e2418dde526d8855faa"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("c8c9872e9e02026479d82b9264b9cc6b"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("73fc1d19f14339042ba5af34872c1745"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("7ed853ffcfd29914cb098cd7b1c46cc4"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("468877871a8e3ba41813a9697ec4eb4e"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("dfedc0bf1d93f024d85546314c42b56a"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("8d8f20391422c0e41a1650e7a9b7a21f"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("c0f3b16ff3f79b749b121905d659a2d4"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("17aee23103aee674082ff9891c82ae2f"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("241ee6bd8c8767343994bce5dc1a95e0"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("1561924f60cbe384283b0788050eca2a"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("ff183ce55a2896e43bb2dd9f5d989119"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("00402bae4442a854081264e498e7a833"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("e7646b1dfdd22ce4ab340f295938ab8e"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("cbe785a099249164eb2204f553437e3d")
			};
			BlueprintBuff[] array3 = new BlueprintBuff[20]
			{
				BlueprintTools.GetBlueprint<BlueprintBuff>("b0253e57a75b621428c1b89de5a937d1"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("1533e782fca42b84ea370fc1dcbf4fc1"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("4814db563c105e64d948161162715661"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("37a956d0e7a84ab0bb66baf784767047"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("50a77710a7c4914499d0254e76a808e5"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("81005a24695910f4cb9b7c8ab4d932e1"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("09b4b69169304474296484c74aa12027"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("7ca348639a91ae042967f796098e3bc3"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("208ce0902f20b1e4896a85b2339468ff"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("52a2552185c4a7c4ba30e421b9e27224"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("ebf84606d5179af4baf9d4589d191c05"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("6f9d4b5d2fe2f684e816a54b4973cc58"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("e40277752759edb49b557ce8399596bc"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("1b0cd0ee398bd9b46888fe58cac4bda9"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("9e265139cf6c07c4fb8298cb8b646de9"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("2e8446f820936a44f951b50d70a82b16"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("cbfd2f5279f5946439fe82570fd61df2"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("4aa87d3319124a2daf74d80ca5d4595e"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("dea0dba1f7bff064987e03f1307bfa84"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("002c51d933574824c8ef2b04c9d09ff5")
			};
			BlueprintBuff[] array4 = new BlueprintBuff[11]
			{
				BlueprintTools.GetBlueprint<BlueprintBuff>("8c385a7610aa409468f3a6c0f904ac92"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("fb0ae0908b3d5c3459be94e11e0c1687"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("0da7299aac601d445a355152084c251a"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("87fcda72043d20840b4cdc2adcc69c63"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("f5d3311a675a7174dad7ffa99a81ad56"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("d72cc6b3a65d31247b37faf600a17977"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("35f3724d4e8877845af488d167cb8a89"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("e9947402c84e8bc4e958b9be08d7a720"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("4f0064bea5b14554f809f5e075a0070d"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("fd8fb2c1d622556468a04bea949eb7da"),
				BlueprintTools.GetBlueprint<BlueprintBuff>("b8da3ec045ec04845a126948e1f4fc1a")
			};
			List<BlueprintBuff> list = new List<BlueprintBuff>();
			list.AddRange(array.Where((BlueprintBuff b) => b != null));
			list.AddRange(array2.Where((BlueprintBuff b) => b != null));
			list.AddRange(array3.Where((BlueprintBuff b) => b != null));
			list.AddRange(array4.Where((BlueprintBuff b) => b != null));
			BlueprintBuff[] buffs = list.Distinct().ToArray();
			BlueprintBuff[] array5 = (from b in array.Concat(array2)
				where b != null
				select b).ToArray();
			BlueprintBuff[] array6 = (from b in array5.Concat(array3)
				where b != null
				select b).ToArray();
			BlueprintBuff[] buffs2 = (from b in array6.Concat(array4)
				where b != null
				select b).ToArray();
			ContextDurationValue duration = CreateHourDuration(1);
			ContextDurationValue duration2 = CreateHourDuration(2);
			ContextDurationValue duration3 = CreateHourDuration(8);
			ContextDurationValue duration4 = CreateHourDuration(24);
			List<GameAction> list2 = CreateApplyBuffActionList(array, duration);
			List<GameAction> list3 = CreateApplyBuffActionList(array5, duration2);
			List<GameAction> list4 = CreateApplyBuffActionList(array6, duration3);
			List<GameAction> list5 = CreateApplyBuffActionList(buffs2, duration4);
			Conditional conditional = new Conditional
			{
				ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionCasterLevel c)
				{
					c.MinLevel = 7;
				}),
				IfTrue = new ActionList
				{
					Actions = list3.ToArray()
				},
				IfFalse = new ActionList
				{
					Actions = list2.ToArray()
				}
			};
			Conditional conditional2 = new Conditional();
			conditional2.ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionCasterLevel c)
			{
				c.MinLevel = 13;
			});
			conditional2.IfTrue = new ActionList
			{
				Actions = list4.ToArray()
			};
			conditional2.IfFalse = new ActionList
			{
				Actions = new GameAction[1] { conditional }
			};
			Conditional conditional3 = conditional2;
			Conditional RootScalingAction = new Conditional
			{
				ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionCasterLevel c)
				{
					c.MinLevel = 17;
				}),
				IfTrue = new ActionList
				{
					Actions = list5.ToArray()
				},
				IfFalse = new ActionList
				{
					Actions = new GameAction[1] { conditional3 }
				}
			};
			List<GameAction> RemoveBuffActions = CreateRemoveBuffActionList(buffs);
			BlueprintAbilityResource SuperBuffResource = Helpers.CreateBlueprint(Main.IsekaiContext, "SuperBuffResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 1,
					IncreasedByLevelStartPlusDivStep = true,
					StartingLevel = 10,
					StartingIncrease = 1,
					LevelStep = 10,
					PerStepIncrease = 1,
					MinClassLevelIncrease = 0,
					m_ClassDiv = new BlueprintCharacterClassReference[1] { IsekaiProtagonistClass.GetReference() },
					m_ArchetypesDiv = new BlueprintArchetypeReference[0],
					OtherClassesModifier = 0f
				};
				bp.m_UseMax = true;
				bp.m_Max = 3;
			});
			Sprite Icon_SuperBuff = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_SUPER_BUFF.png");
			Sprite Icon_SuperBuffDismiss = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_SUPER_BUFF_DISMISS.png");
			BlueprintAbility SuperBuffAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SuperBuffAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Super Buff");
				bp.SetDescription(Description);
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = new ActionList
					{
						Actions = new GameAction[1] { RootScalingAction }
					};
				});
				bp.AddComponent(delegate(AbilityTargetsAround c)
				{
					c.m_Radius = new Feet(60f);
					c.m_TargetType = TargetType.Ally;
					c.m_Condition = ActionFlow.EmptyCondition();
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = SuperBuffResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(SpellComponent c)
				{
					c.School = SpellSchool.Universalist;
				});
				((BlueprintUnitFact)bp).m_Icon = Icon_SuperBuff;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetFriends = true;
				bp.CanTargetSelf = true;
				bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
				bp.EffectOnEnemy = AbilityEffectOnUnit.None;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AvailableMetamagic = Metamagic.Reach;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.OneHour;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
			});
			BlueprintAbility SuperBuffDismissAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SuperBuffDismissAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Dismiss Super Buff");
				bp.SetDescription(Main.IsekaiContext, "You dispel all Super Buff effects from you and your allies.");
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = new ActionList
					{
						Actions = RemoveBuffActions.ToArray()
					};
				});
				bp.AddComponent(delegate(AbilityTargetsAround c)
				{
					c.m_Radius = new Feet(60f);
					c.m_TargetType = TargetType.Ally;
					c.m_Condition = ActionFlow.EmptyCondition();
				});
				bp.AddComponent(delegate(SpellComponent c)
				{
					c.School = SpellSchool.Universalist;
				});
				((BlueprintUnitFact)bp).m_Icon = Icon_SuperBuffDismiss;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetFriends = true;
				bp.CanTargetSelf = true;
				bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
				bp.EffectOnEnemy = AbilityEffectOnUnit.None;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AvailableMetamagic = Metamagic.Reach;
				bp.LocalizedDuration = StaticReferences.Strings.Null;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
			});
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "SuperBuffFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Super Buff");
				bp.SetDescription(Description);
				((BlueprintUnitFact)bp).m_Icon = Icon_SuperBuff;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[2]
					{
						SuperBuffAbility.ToReference<BlueprintUnitFactReference>(),
						SuperBuffDismissAbility.ToReference<BlueprintUnitFactReference>()
					};
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = SuperBuffResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
			}));
		}

		private static ContextDurationValue CreateHourDuration(int hours)
		{
			return new ContextDurationValue
			{
				Rate = DurationRate.Hours,
				DiceType = DiceType.Zero,
				DiceCountValue = 0,
				BonusValue = new ContextValue
				{
					ValueType = ContextValueType.Simple,
					Value = hours
				}
			};
		}

		private static List<GameAction> CreateApplyBuffActionList(BlueprintBuff[] buffs, ContextDurationValue duration)
		{
			List<GameAction> list = new List<GameAction>();
			foreach (BlueprintBuff blueprintBuff in buffs)
			{
				if (blueprintBuff != null)
				{
					list.Add(new ContextActionApplyBuff
					{
						m_Buff = blueprintBuff.ToReference<BlueprintBuffReference>(),
						DurationValue = duration
					});
				}
			}
			return list;
		}

		private static List<GameAction> CreateRemoveBuffActionList(BlueprintBuff[] buffs)
		{
			List<GameAction> list = new List<GameAction>();
			foreach (BlueprintBuff blueprintBuff in buffs)
			{
				if (blueprintBuff != null)
				{
					list.Add(new ContextActionRemoveBuff
					{
						m_Buff = blueprintBuff.ToReference<BlueprintBuffReference>(),
						OnlyFromCaster = true
					});
				}
			}
			return list;
		}
	}
}
