using System;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class SummonCalamity
	{
		private static readonly BlueprintUnit Devastator = BlueprintTools.GetBlueprint<BlueprintUnit>("99c16c4360534129b45706841a7df3fe");

		private static readonly BlueprintUnit PlayfulDarkness = BlueprintTools.GetBlueprint<BlueprintUnit>("a1b06220efb4f0545a870ce52fadd678");

		private static readonly BlueprintUnit Baphomet = BlueprintTools.GetBlueprint<BlueprintUnit>("f8007503fe211da4eb027e070eeb3f8c");

		private static readonly BlueprintUnit DemonLordDeskari = BlueprintTools.GetBlueprint<BlueprintUnit>("5a75db49bf7aeaf4c9f0264cac3eed5c");

		private static readonly BlueprintUnit Nocticula = BlueprintTools.GetBlueprint<BlueprintUnit>("0cca8c841d634d84fbec2609c8db3465");

		private static readonly BlueprintUnit Mephistopheles = BlueprintTools.GetBlueprint<BlueprintUnit>("c3dfbb136aa27e74eb7a7b5159395a80");

		private static readonly BlueprintUnit Areshkagal = BlueprintTools.GetBlueprint<BlueprintUnit>("7a1b0862dd2443b49adaba36b194e5de");

		private static readonly BlueprintSummonPool SummonMonsterPool = BlueprintTools.GetBlueprint<BlueprintSummonPool>("d94c93e7240f10e41ae41db4c83d1cbe");

		private static readonly BlueprintBuff SummonedCreatureSpawnMonsterVI_IX = BlueprintTools.GetBlueprint<BlueprintBuff>("0dff842f06edace43baf8a2f44207045");

		private static readonly Sprite Icon_SummonMonsterIX = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("52b5df2a97df18242aec67610616ded0")).m_Icon;

		public static void Add()
		{
			BlueprintAbilityResource SummonCalamityResource = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonCalamityResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 1,
					IncreasedByLevelStartPlusDivStep = true,
					StartingLevel = 15,
					StartingIncrease = 1,
					LevelStep = 5,
					PerStepIncrease = 1,
					MinClassLevelIncrease = 0,
					m_ClassDiv = new BlueprintCharacterClassReference[1] { IsekaiProtagonistClass.GetReference() },
					m_ArchetypesDiv = new BlueprintArchetypeReference[0],
					OtherClassesModifier = 0f
				};
				bp.m_UseMax = true;
				bp.m_Max = 2;
			});
			BlueprintAbility SummonCalamityAbility = CreateSummonAbility("SummonCalamityAbility", SummonCalamityResource, delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Summon Calamity");
				bp.SetSummonDescription(Main.IsekaiContext, "This {g|Encyclopedia:Spell}spell{/g} summons a Devastator, Playful Darkness, Baphomet, Deskari, Nocticula, Mephistopheles, or Areshkagal.");
			});
			BlueprintAbility SummonDevastator = CreateSummonAbility("SummonDevastator", SummonCalamityResource, delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Summon Calamity (Devastator)");
				bp.SetSummonDescription(Main.IsekaiContext, "This {g|Encyclopedia:Spell}spell{/g} summons a Devastator.");
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = SpawnCalamity(delegate(ContextActionSpawnMonster contextActionSpawnMonster)
					{
						contextActionSpawnMonster.m_Blueprint = Devastator?.ToReference<BlueprintUnitReference>();
					});
				});
			});
			BlueprintAbility SummonPlayfulDarkness = CreateSummonAbility("SummonPlayfulDarkness", SummonCalamityResource, delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Summon Calamity (Playful Darkness)");
				bp.SetSummonDescription(Main.IsekaiContext, "This {g|Encyclopedia:Spell}spell{/g} summons Playful Darkness.");
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = SpawnCalamity(delegate(ContextActionSpawnMonster contextActionSpawnMonster)
					{
						contextActionSpawnMonster.m_Blueprint = PlayfulDarkness?.ToReference<BlueprintUnitReference>();
					});
				});
			});
			BlueprintAbility SummonBaphomet = CreateSummonAbility("SummonBaphomet", SummonCalamityResource, delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Summon Calamity (Baphomet)");
				bp.SetSummonDescription(Main.IsekaiContext, "This {g|Encyclopedia:Spell}spell{/g} summons Demon Lord Baphomet.");
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = SpawnCalamity(delegate(ContextActionSpawnMonster contextActionSpawnMonster)
					{
						contextActionSpawnMonster.m_Blueprint = Baphomet?.ToReference<BlueprintUnitReference>();
					});
				});
			});
			BlueprintAbility SummonDemonLordDeskari = CreateSummonAbility("SummonDemonLordDeskari", SummonCalamityResource, delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Summon Calamity (Deskari)");
				bp.SetSummonDescription(Main.IsekaiContext, "This {g|Encyclopedia:Spell}spell{/g} summons Demon Lord Deskari.");
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = SpawnCalamity(delegate(ContextActionSpawnMonster contextActionSpawnMonster)
					{
						contextActionSpawnMonster.m_Blueprint = DemonLordDeskari?.ToReference<BlueprintUnitReference>();
					});
				});
			});
			BlueprintAbility SummonDemonNocticula = CreateSummonAbility("SummonDemonNocticula", SummonCalamityResource, delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Summon Calamity (Nocticula)");
				bp.SetSummonDescription(Main.IsekaiContext, "This {g|Encyclopedia:Spell}spell{/g} summons Demon Lord Nocticula.");
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = SpawnCalamity(delegate(ContextActionSpawnMonster contextActionSpawnMonster)
					{
						contextActionSpawnMonster.m_Blueprint = Nocticula?.ToReference<BlueprintUnitReference>();
					});
				});
			});
			BlueprintAbility SummonDemonMephistopheles = CreateSummonAbility("SummonDemonMephistopheles", SummonCalamityResource, delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Summon Calamity (Mephistopheles)");
				bp.SetSummonDescription(Main.IsekaiContext, "This {g|Encyclopedia:Spell}spell{/g} summons Archdevil Mephistopheles.");
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = SpawnCalamity(delegate(ContextActionSpawnMonster contextActionSpawnMonster)
					{
						contextActionSpawnMonster.m_Blueprint = Mephistopheles?.ToReference<BlueprintUnitReference>();
					});
				});
			});
			BlueprintAbility SummonAreshkagal = CreateSummonAbility("SummonAreshkagal", SummonCalamityResource, delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Summon Calamity (Areshkagal)");
				bp.SetSummonDescription(Main.IsekaiContext, "This {g|Encyclopedia:Spell}spell{/g} summons Areshkagal.");
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = SpawnCalamity(delegate(ContextActionSpawnMonster contextActionSpawnMonster)
					{
						contextActionSpawnMonster.m_Blueprint = Areshkagal?.ToReference<BlueprintUnitReference>();
					});
				});
			});
			BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonCalamityFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Summon Calamity");
				bp.SetDescription(Main.IsekaiContext, "There is nothing you cannot summon. Your calls are heeded; your commands are absolute. Even demon lords show no negligence... and tremblingly obey.\nBenefit: As a full action, you summon a powerful being to bring calamity. You can summon one of the following: Devastator, Playful Darkness, Baphomet, Deskari, Nocticula, Mephistopheles, or Areshkagal.");
				((BlueprintUnitFact)bp).m_Icon = Icon_SummonMonsterIX;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { SummonCalamityAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = SummonCalamityResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 15;
				});
			});
			SummonCalamityAbility.AddComponent(delegate(AbilityVariants c)
			{
				c.m_Variants = new BlueprintAbilityReference[7]
				{
					SummonDevastator.ToReference<BlueprintAbilityReference>(),
					SummonPlayfulDarkness.ToReference<BlueprintAbilityReference>(),
					SummonBaphomet.ToReference<BlueprintAbilityReference>(),
					SummonDemonLordDeskari.ToReference<BlueprintAbilityReference>(),
					SummonDemonNocticula.ToReference<BlueprintAbilityReference>(),
					SummonDemonMephistopheles.ToReference<BlueprintAbilityReference>(),
					SummonAreshkagal.ToReference<BlueprintAbilityReference>()
				};
			});
			OverpoweredAbilitySelection.AddToSelection(feature);
		}

		private static BlueprintAbility CreateSummonAbility(string name, BlueprintAbilityResource resource, Action<BlueprintAbility> init = null)
		{
			BlueprintAbility blueprintAbility = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintAbility bp)
			{
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				});
				bp.AddComponent(delegate(SpellComponent c)
				{
					c.School = SpellSchool.Conjuration;
				});
				bp.AddComponent(delegate(SpellDescriptorComponent c)
				{
					c.Descriptor = SpellDescriptor.Summoning;
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = resource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				((BlueprintUnitFact)bp).m_Icon = Icon_SummonMonsterIX;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Close;
				bp.CanTargetPoint = true;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AvailableMetamagic = Metamagic.Quicken;
				bp.m_IsFullRoundAction = true;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.OneRoundPerLevel;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
			});
			init?.Invoke(blueprintAbility);
			return blueprintAbility;
		}

		private static ActionList SpawnCalamity(Action<ContextActionSpawnMonster> init = null)
		{
			ContextActionSpawnMonster contextActionSpawnMonster = new ContextActionSpawnMonster
			{
				m_SummonPool = SummonMonsterPool?.ToReference<BlueprintSummonPoolReference>(),
				DurationValue = new ContextDurationValue
				{
					Rate = DurationRate.Rounds,
					DiceType = DiceType.Zero,
					DiceCountValue = 0,
					BonusValue = Values.CreateContextRankValue(AbilityRankType.Default),
					m_IsExtendable = true
				},
				CountValue = Values.Dice.One,
				LevelValue = 0,
				AfterSpawn = ActionFlow.DoSingle(delegate(ContextActionApplyBuff c)
				{
					c.Permanent = true;
					c.m_Buff = SummonedCreatureSpawnMonsterVI_IX?.ToReference<BlueprintBuffReference>();
					c.DurationValue = Values.Duration.Zero;
					c.IsNotDispelable = true;
				})
			};
			init?.Invoke(contextActionSpawnMonster);
			return Helpers.CreateActionList(contextActionSpawnMonster);
		}
	}
}
