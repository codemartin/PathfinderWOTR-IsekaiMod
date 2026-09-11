using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor
{
	internal class PathSelection
	{
		private static readonly Sprite Icon_Arbitrament = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("0f5bd128c76dd374b8cb9111e3b5186b")).m_Icon;

		private static readonly Sprite Icon_UnjustPath = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_UNJUST_PATH.png");

		public static void Add()
		{
			BlueprintFeature RighteousPathFeature = TTCoreExtensions.CreateToggleBuffFeature("RighteousPath", "The damage you deal is now divine damage.", Icon_Arbitrament, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(ChangeOutgoingDamageType c)
				{
					c.Type = new DamageTypeDescription
					{
						Type = DamageType.Energy,
						Common = new DamageTypeDescription.CommomData(),
						Physical = new DamageTypeDescription.PhysicalData(),
						Energy = DamageEnergyType.Divine
					};
				});
			});
			BlueprintFeature UnjustPathFeature = TTCoreExtensions.CreateToggleBuffFeature("UnjustPath", "The damage you deal is now unholy damage.", Icon_UnjustPath, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(ChangeOutgoingDamageType c)
				{
					c.Type = new DamageTypeDescription
					{
						Type = DamageType.Energy,
						Common = new DamageTypeDescription.CommomData(),
						Physical = new DamageTypeDescription.PhysicalData(),
						Energy = DamageEnergyType.Unholy
					};
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "PathSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Chosen Path");
				bp.SetDescription(Main.IsekaiContext, "At 12th level, you transform the damage you deal into either divine or unholy.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Arbitrament;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[2]
				{
					RighteousPathFeature.ToReference<BlueprintFeatureReference>(),
					UnjustPathFeature.ToReference<BlueprintFeatureReference>()
				};
				bp.m_Features = bp.m_AllFeatures;
			});
			SecretPowerSelection.AddToSelection(RighteousPathFeature);
			SecretPowerSelection.AddToSelection(UnjustPathFeature);
		}
	}
}
