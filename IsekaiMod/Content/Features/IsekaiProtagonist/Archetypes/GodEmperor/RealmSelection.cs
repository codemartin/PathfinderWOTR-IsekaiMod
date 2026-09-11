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
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor
{
	internal class RealmSelection
	{
		private static readonly Sprite Icon_CelestialRealm = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_REALM_CELESTIAL.png");

		private static readonly Sprite Icon_ShadowRealm = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_REALM_SHADOW.png");

		public static void Add()
		{
			BlueprintFeature CelestialRealmFeature = TTCoreExtensions.CreateToggleAuraBuffFeature("CelestialRealm", "Allies within 40 feet of you transform their damage type into divine.", "This character transforms their damage type into divine.", Icon_CelestialRealm, BlueprintAbilityAreaEffect.TargetType.Ally, new Feet(40f), affectEnemies: false, delegate(BlueprintBuff bp)
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
			BlueprintFeature ShadowRealmFeature = TTCoreExtensions.CreateToggleAuraBuffFeature("ShadowRealm", "Allies within 40 feet of you transform their damage type into unholy.", "This character transforms their damage type into unholy.", Icon_ShadowRealm, BlueprintAbilityAreaEffect.TargetType.Ally, new Feet(40f), affectEnemies: false, delegate(BlueprintBuff bp)
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
			Helpers.CreateBlueprint(Main.IsekaiContext, "RealmSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Transcendental Realm");
				bp.SetDescription(Main.IsekaiContext, "At 15th level, you are able to ascend into a higher plane of existence and harness its energies.");
				((BlueprintUnitFact)bp).m_Icon = Icon_CelestialRealm;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[2]
				{
					CelestialRealmFeature.ToReference<BlueprintFeatureReference>(),
					ShadowRealmFeature.ToReference<BlueprintFeatureReference>()
				};
				bp.m_Features = bp.m_AllFeatures;
			});
		}
	}
}
