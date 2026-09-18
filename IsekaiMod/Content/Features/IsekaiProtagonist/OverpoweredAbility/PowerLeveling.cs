using IsekaiMod.Components;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class PowerLeveling
	{
		public static void Add()
		{
			Sprite Icon_DimensionalAnchor = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("c0aa77246b26433fa79c8ac09b1e70d9")).m_Icon;
			LocalizedString PowerLevelingName = Helpers.CreateString(Main.IsekaiContext, "PowerLeveling.Name", "Overpowered Ability - Power Leveling");
			LocalizedString PowerLevelingDesc = Helpers.CreateString(Main.IsekaiContext, "PowerLeveling.Description", "You are Overpowered but overly cautious. You use overwhelming force to ensure all enemies you kill are thoroughly defeated.\nBenefit: Your party gains double experience from all sources. Furthermore, whenever you or your allies defeat enemies within a 120-foot radius, your party gains bonus experience and an Overpowered Surge granting a +2 bonus to attack rolls and Fortitude saving throws.");
			BlueprintBuff PowerLevelingTempBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "PowerLevelingTempBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Surge");
				bp.SetDescription(Main.IsekaiContext, "Defeating enemies grants temporary bonuses and experience to the party.");
				((BlueprintUnitFact)bp).m_Icon = Icon_DimensionalAnchor;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
				// Prolong extends the running buff instead of removing and re-adding it; with Replace every kill
				// re-added both modifiers on every party member, which showed up as the busiest modifier source in combat.
				bp.Stacking = StackingType.Prolong;
				bp.m_Flags = (BlueprintBuff.Flags)0;
			});
			BlueprintBuff PowerLevelingBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "PowerLevelingBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(PowerLevelingName);
				bp.SetDescription(PowerLevelingDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_DimensionalAnchor;
				bp.IsClassFeature = true;
				bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
				bp.AddComponent(delegate(ApplyPartyBuffOnKill c)
				{
					c.m_Buff = PowerLevelingTempBuff.ToReference<BlueprintBuffReference>();
				});
				bp.AddComponent<GainExperienceOnKill>();
			});
			BlueprintAbilityAreaEffect PowerLevelingAura = Helpers.CreateBlueprint(Main.IsekaiContext, "PowerLevelingAura", delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
				bp.SpellResistance = false;
				bp.AggroEnemies = false;
				bp.AffectEnemies = false;
				bp.Shape = AreaEffectShape.Cylinder;
				bp.Size = new Feet(120f);
				bp.Fx = new PrefabLink();
				bp.AddComponent(delegate(AbilityAreaEffectBuff c)
				{
					c.m_Buff = PowerLevelingBuff.ToReference<BlueprintBuffReference>();
					c.Condition = new ConditionsChecker
					{
						Conditions = new Condition[0]
					};
				});
			});
			BlueprintBuff PowerLevelingAreaBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "PowerLevelingAreaBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(PowerLevelingName);
				bp.SetDescription(PowerLevelingDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_DimensionalAnchor;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddAreaEffect c)
				{
					c.m_AreaEffect = PowerLevelingAura.ToReference<BlueprintAbilityAreaEffectReference>();
				});
			});
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "PowerLevelingFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(PowerLevelingName);
				bp.SetDescription(PowerLevelingDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_DimensionalAnchor;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { PowerLevelingAreaBuff.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent<GainExperienceOnKill>();
			}));
		}
	}
}
