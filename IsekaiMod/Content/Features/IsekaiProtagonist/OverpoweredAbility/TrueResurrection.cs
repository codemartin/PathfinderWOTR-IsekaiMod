using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;

internal class TrueResurrection
{
	private const string Name = "Overpowered Ability - True Resurrection";

	private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "TrueResurrection.Description", "Restore life to a deceased companion. Upon completion of this ability the creature is immediately restored to full {g|Encyclopedia:HP}hit points{/g}. Ability damage, ability drain and negative levels are not removed.\nThis ability does not require diamonds, but it can only be used three times per day and the target must be within close range.");

	private static readonly Sprite Icon_Resurrection = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("80a1a388ee938aa4e90d427ce9a7a3e9"))?.m_Icon;

	public static void Add()
	{
		BlueprintAbilityResource TrueResurrectionResource = Helpers.CreateBlueprint(Main.IsekaiContext, "TrueResurrectionResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = new BlueprintAbilityResource.Amount
			{
				BaseValue = 3,
				IncreasedByLevelStartPlusDivStep = false,
				StartingLevel = 0,
				StartingIncrease = 0,
				LevelStep = 0,
				PerStepIncrease = 0,
				MinClassLevelIncrease = 0,
				m_ClassDiv = new BlueprintCharacterClassReference[0],
				m_ArchetypesDiv = new BlueprintArchetypeReference[0],
				OtherClassesModifier = 0f
			};
			bp.m_UseMax = true;
			bp.m_Max = 3;
		});
		BlueprintAbility TrueResurrectionAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "TrueResurrectionAbility", delegate(BlueprintAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Overpowered Ability - True Resurrection");
			bp.SetDescription(Description);
			((BlueprintUnitFact)bp).m_Icon = Icon_Resurrection;
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = ActionFlow.DoSingle(delegate(ContextActionResurrect contextActionResurrect)
				{
					contextActionResurrect.FullRestore = false;
					contextActionResurrect.ResultHealth = 1f;
				});
			});
			bp.AddComponent(delegate(SpellComponent c)
			{
				c.School = SpellSchool.Conjuration;
			});
			bp.AddComponent<AbilityTargetIsDeadCompanion>();
			bp.AddComponent(delegate(AbilitySpawnFx c)
			{
				c.PrefabLink = new PrefabLink
				{
					AssetId = "ee0c5b9397ffec54d86acf56c94f4b06"
				};
				c.Time = AbilitySpawnFxTime.OnPrecastFinished;
				c.Anchor = AbilitySpawnFxAnchor.SelectedTarget;
			});
			bp.AddComponent(delegate(AbilityResourceLogic c)
			{
				c.m_RequiredResource = TrueResurrectionResource.ToReference<BlueprintAbilityResourceReference>();
				c.m_IsSpendResource = true;
				c.Amount = 1;
			});
			bp.Type = AbilityType.Supernatural;
			bp.Range = AbilityRange.Close;
			bp.CanTargetFriends = true;
			bp.CanTargetSelf = true;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Point;
			bp.ActionType = UnitCommand.CommandType.Standard;
			bp.m_IsFullRoundAction = true;
			bp.AvailableMetamagic = Metamagic.Quicken | Metamagic.Heighten | Metamagic.CompletelyNormal;
			bp.LocalizedDuration = StaticReferences.Strings.Duration.Instantaneous;
			bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
		});
		OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "TrueResurrectionFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Overpowered Ability - True Resurrection");
			bp.SetDescription(Description);
			((BlueprintUnitFact)bp).m_Icon = Icon_Resurrection;
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { TrueResurrectionAbility.ToReference<BlueprintUnitFactReference>() };
			});
			bp.AddComponent(delegate(AddAbilityResources c)
			{
				c.m_Resource = TrueResurrectionResource.ToReference<BlueprintAbilityResourceReference>();
				c.RestoreAmount = true;
			});
			bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 10;
			});
		}));
	}
}
