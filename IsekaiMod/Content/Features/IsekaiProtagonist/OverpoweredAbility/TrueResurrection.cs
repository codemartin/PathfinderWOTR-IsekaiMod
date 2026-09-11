using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class TrueResurrection
	{
		private const string Name = "Overpowered Ability - True Resurrection";

		private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "TrueResurrection.Description", "Restore life and complete {g|Encyclopedia:Strength}strength{/g} to any deceased creature. Upon completion of this ability, the creature is immediately restored to full {g|Encyclopedia:HP}hit points{/g}, vigor, and {g|Encyclopedia:Healing}health{/g}, with no loss of prepared spells.\nThis ability does not require diamonds.");

		private static readonly Sprite Icon_Resurrection = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("80a1a388ee938aa4e90d427ce9a7a3e9")).m_Icon;

		public static void Add()
		{
			BlueprintAbility TrueResurrectionAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "TrueResurrectionAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - True Resurrection");
				bp.SetDescription(Description);
				((BlueprintUnitFact)bp).m_Icon = Icon_Resurrection;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionResurrect contextActionResurrect)
					{
						contextActionResurrect.FullRestore = true;
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
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Unlimited;
				bp.CanTargetFriends = true;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Point;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.m_IsFullRoundAction = true;
				bp.AvailableMetamagic = Metamagic.Quicken | Metamagic.Heighten | Metamagic.CompletelyNormal;
				bp.LocalizedDuration = StaticReferences.Strings.Duration.Instantaneous;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
			});
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "TrueResurrectionFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - True Resurrection");
				bp.SetDescription(Description);
				((BlueprintUnitFact)bp).m_Icon = Icon_Resurrection;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { TrueResurrectionAbility.ToReference<BlueprintUnitFactReference>() };
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
