using IsekaiMod.Components;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.ResourceLinks;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class MartialGodTranscendence
	{
		private static readonly BlueprintAbility DimensionDoor = BlueprintTools.GetBlueprint<BlueprintAbility>("5bdc37e4acfa209408334326076a43bc");

		private static readonly Sprite Icon_Transcendence = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e4450dd9c06dc034fb7c0c08abcc202b"))?.m_Icon;

		public static void Add()
		{
			BlueprintAbilityResource MartialGodFlashStepResource = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialGodFlashStepResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByStat = true,
					ResourceBonusStat = StatType.Dexterity,
					m_ClassDiv = new BlueprintCharacterClassReference[0],
					m_ArchetypesDiv = new BlueprintArchetypeReference[0]
				};
			});
			BlueprintBuff MartialGodTranscendenceBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialGodTranscendenceBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Transcendental Velocity");
				bp.SetDescription(Main.IsekaiContext, "Having flash-stepped through sheer physical velocity, your next strikes catch the foe flat-footed and carry an untyped bonus to attack rolls (+2 at levels 1--9, +3 at levels 10--14, and +4 at level 15+).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Transcendence;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.Invisible;
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[3]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 9,
							ProgressionValue = 2
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 3
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 4
						}
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
			});
			BlueprintAbility MartialGodTranscendenceAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialGodTranscendenceAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Transcendental Flash Step");
				bp.SetDescription(Main.IsekaiContext, "As a swift action, instantaneously displace yourself up to 40 feet. Foes at your destination are caught flat-footed and your attacks gain an untyped bonus (+2 at levels 1--9, +3 at levels 10--14, and +4 at level 15+) for 1 round.");
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)DimensionDoor)?.m_Icon;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Close;
				bp.CanTargetPoint = true;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.AvailableMetamagic = Metamagic.Quicken;
				bp.AddComponent(delegate(AbilityCustomDimensionDoor c)
				{
					c.Radius = new Feet(0f);
					c.PortalFromPrefab = DimensionDoor?.GetComponent<AbilityCustomDimensionDoor>()?.PortalFromPrefab;
					c.PortalToPrefab = DimensionDoor?.GetComponent<AbilityCustomDimensionDoor>()?.PortalToPrefab;
					c.PortalBone = DimensionDoor?.GetComponent<AbilityCustomDimensionDoor>()?.PortalBone ?? "";
					c.CasterDisappearFx = DimensionDoor?.GetComponent<AbilityCustomDimensionDoor>()?.CasterDisappearFx;
					c.CasterAppearFx = DimensionDoor?.GetComponent<AbilityCustomDimensionDoor>()?.CasterAppearFx;
					// Every PrefabLink on this component is loaded during delivery; a null one throws and the teleport
					// silently never happens. Copy the side effects too and fall back to empty links.
					c.SideDisappearFx = DimensionDoor?.GetComponent<AbilityCustomDimensionDoor>()?.SideDisappearFx ?? new PrefabLink();
					c.SideAppearFx = DimensionDoor?.GetComponent<AbilityCustomDimensionDoor>()?.SideAppearFx ?? new PrefabLink();
					c.PortalFromPrefab = c.PortalFromPrefab ?? new PrefabLink();
					c.PortalToPrefab = c.PortalToPrefab ?? new PrefabLink();
					c.CasterDisappearFx = c.CasterDisappearFx ?? new PrefabLink();
					c.CasterAppearFx = c.CasterAppearFx ?? new PrefabLink();
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = MartialGodFlashStepResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = MartialGodTranscendenceBuff.ToReference<BlueprintBuffReference>(),
						DurationValue = Values.Duration.OneRound,
						// The ability targets a point, so without this the buff had nobody to land on.
						ToCaster = true
					});
				});
			});
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "MartialGodTranscendenceFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Martial God Transcendence");
				bp.SetDescription(Main.IsekaiContext, "Exclusive to the Martial God archetype. You have ascended beyond mortal martial disciplines, turning your physical form into an engine of supreme combat superiority.\nBenefit: You gain a bonus to base movement speed (+10 ft at levels 1--9, +20 ft at levels 10--14, and +30 ft at level 15+), one extra attack on a full attack, +5 ft reach with melee weapons and one extra off-hand attack (unlocking at level 10), and your critical damage multiplier increases by 1 across all attack types (unlocking at level 15). Additionally, you can use Transcendental Flash Step as a swift action (3 + Dex modifier per day) to teleport up to 40 ft, catching foes flat-footed and gaining an attack bonus (+2 at levels 1--9, +3 at levels 10--14, and +4 at level 15+) for 1 round.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Transcendence;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { MartialGodTranscendenceAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = MartialGodFlashStepResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[3]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 9,
							ProgressionValue = 10
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 20
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 30
						}
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Speed;
					c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.ProjectilesCount;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[2]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 9,
							ProgressionValue = 0
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 5
						}
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Reach;
					c.Value = Values.CreateContextRankValue(AbilityRankType.ProjectilesCount);
				});
				bp.AddComponent(delegate(AddExtraAttack c)
				{
					c.Number = 1;
				});
				bp.AddComponent(delegate(AddExtraOffHandAttack c)
				{
					c.Number = 1;
					c.MinCharacterLevel = 10;
				});
				bp.AddComponent(delegate(AttackTypeCriticalMultiplierIncreaseScaled c)
				{
					c.Type = WeaponRangeType.Melee;
					c.AdditionalMultiplier = 1;
					c.MinCharacterLevel = 15;
				});
				bp.AddComponent(delegate(AttackTypeCriticalMultiplierIncreaseScaled c)
				{
					c.Type = WeaponRangeType.Ranged;
					c.AdditionalMultiplier = 1;
					c.MinCharacterLevel = 15;
				});
				bp.AddComponent(delegate(AttackTypeCriticalMultiplierIncreaseScaled c)
				{
					c.Type = WeaponRangeType.Touch;
					c.AdditionalMultiplier = 1;
					c.MinCharacterLevel = 15;
				});
				bp.AddComponent(delegate(PrerequisiteArchetypeLevel c)
				{
					c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
					c.m_Archetype = MartialGodArchetype.GetReference();
					c.Level = 1;
				});
			}));
		}
	}
}
