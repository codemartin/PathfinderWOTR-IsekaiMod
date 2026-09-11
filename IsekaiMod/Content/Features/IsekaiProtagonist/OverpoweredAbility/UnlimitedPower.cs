using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class UnlimitedPower
	{
		private const string Name = "Overpowered Ability - Unlimited Power";

		private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "UnlimitedPower.Description", "On the brink of defeat, the enemies have surrounded and exhausted you. Just as they are about to deliver the finishing blow, you get up and say the following words: Not today.\nBenefit: As a standard action, you restore all of your spell slots. You gain an initial 1 use at level 1, scaling to a maximum of 5 uses by level 20 (gaining an additional use every 4 levels).");

		private static readonly Sprite Icon_UnlimitedPower = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_UNLIMITED_POWER.png");

		public static void Add()
		{
			BlueprintAbilityResource UnlimitedPowerResource = Helpers.CreateBlueprint(Main.IsekaiContext, "UnlimitedPowerResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 1,
					IncreasedByLevelStartPlusDivStep = true,
					StartingLevel = 5,
					StartingIncrease = 1,
					LevelStep = 4,
					PerStepIncrease = 1,
					MinClassLevelIncrease = 0,
					m_ClassDiv = new BlueprintCharacterClassReference[1] { IsekaiProtagonistClass.GetReference() },
					m_ArchetypesDiv = new BlueprintArchetypeReference[0],
					OtherClassesModifier = 0f
				};
				bp.m_UseMax = true;
				bp.m_Max = 5;
			});
			BlueprintAbility UnlimitedPowerAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "UnlimitedPowerAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Unlimited Power");
				bp.SetDescription(Description);
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionRestoreAllSpellSlots
					{
						m_Target = new ContextTargetUnit(),
						m_UpToSpellLevel = 10
					});
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = UnlimitedPowerResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
				bp.AddComponent(delegate(AbilitySpawnFx c)
				{
					c.PrefabLink = new PrefabLink
					{
						AssetId = "0c07afb9ee854184cb5110891324e3ad"
					};
					c.Time = AbilitySpawnFxTime.OnApplyEffect;
					c.Anchor = AbilitySpawnFxAnchor.Caster;
				});
				((BlueprintUnitFact)bp).m_Icon = Icon_UnlimitedPower;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.LocalizedDuration = StaticReferences.Strings.Null;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
			});
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "UnlimitedPowerFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Unlimited Power");
				bp.SetDescription(Description);
				((BlueprintUnitFact)bp).m_Icon = Icon_UnlimitedPower;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { UnlimitedPowerAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = UnlimitedPowerResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
					c.RestoreOnLevelUp = true;
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 10;
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
		}
	}
}
