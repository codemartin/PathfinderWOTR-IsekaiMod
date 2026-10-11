using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;

internal class ImperialSovereignty
{
	private static readonly Sprite Icon_Aura = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ad5ed5ea4ec52334a94e975a64dad336"))?.m_Icon;

	public static void Add()
	{
		BlueprintBuff ImperialStandardBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "ImperialStandardBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Imperial Standard");
			bp.SetDescription(Main.IsekaiContext, "The radiant presence of the God Emperor inspires divine devotion. Allies receive an insight bonus to attack rolls, weapon damage, Armor Class, and saving throws - +1, rising to +2 at 8th level and +3 at 15th - and are immune to fear and shaken effects.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Aura;
			bp.IsClassFeature = true;
			StatType[] array = new StatType[6]
			{
				StatType.AdditionalAttackBonus,
				StatType.AdditionalDamage,
				StatType.AC,
				StatType.SaveFortitude,
				StatType.SaveReflex,
				StatType.SaveWill
			};
			foreach (StatType statType in array)
			{
				StatType captured = statType;
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = captured;
					c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
				});
			}
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.Default;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				c.m_Progression = ContextRankProgression.Custom;
				c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[3]
				{
					new ContextRankConfig.CustomProgressionItem
					{
						BaseValue = 7,
						ProgressionValue = 1
					},
					new ContextRankConfig.CustomProgressionItem
					{
						BaseValue = 14,
						ProgressionValue = 2
					},
					new ContextRankConfig.CustomProgressionItem
					{
						BaseValue = 100,
						ProgressionValue = 3
					}
				};
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Shaken;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Frightened;
			});
		});
		BlueprintAbilityAreaEffect ImperialStandardArea = Helpers.CreateBlueprint(Main.IsekaiContext, "ImperialStandardArea", delegate(BlueprintAbilityAreaEffect bp)
		{
			bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
			bp.SpellResistance = false;
			bp.AggroEnemies = false;
			bp.AffectEnemies = false;
			bp.Shape = AreaEffectShape.Cylinder;
			bp.Size = new Feet(40f);
			bp.Fx = new PrefabLink();
			bp.AddUnconditionalAuraEffect(ImperialStandardBuff.ToReference<BlueprintBuffReference>());
		});
		BlueprintBuff ImperialStandardAreaBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "ImperialStandardAreaBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Imperial Standard Aura");
			bp.SetDescription(Main.IsekaiContext, "Emits a 40-foot aura that grants allies an insight bonus to attack, damage, AC and saves (+1, +2 at 8th level, +3 at 15th), and immunity to fear.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Aura;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddAreaEffect c)
			{
				c.m_AreaEffect = ImperialStandardArea.ToReference<BlueprintAbilityAreaEffectReference>();
			});
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "ImperialSovereigntyFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Imperial Sovereignty & Armies");
			bp.SetDescription(Main.IsekaiContext, "As an otherworldly sovereign, the God Emperor commands imperial authority over military forces and crusader legions:\n• +10% Crusade Finances, Materials and Favors income.\n• +25% weekly recruit growth across all military units.\n• Crusade army units gain +1 on attack rolls against demons.\n• Crusade army units gain +4 on saving throws against fear, sleep and mind-affecting effects.\n• Emits a 40-foot Imperial Standard Aura granting all companions, allies, and summons an insight bonus to attack, damage, AC and saves (+1, rising to +2 at 8th level and +3 at 15th), and immunity to fear and shaken effects.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Aura;
			bp.IsClassFeature = true;
			bp.AddComponent<ImperialSovereigntyComponent>();
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { ImperialStandardAreaBuff.ToReference<BlueprintUnitFactReference>() };
			});
		});
	}
}
