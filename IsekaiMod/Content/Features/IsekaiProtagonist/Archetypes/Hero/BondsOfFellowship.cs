using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero
{
	internal class BondsOfFellowship
	{
		private static readonly Sprite Icon_Fellowship = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("90e59f4a4ada87243b7b3535a06d0638"))?.m_Icon;

		public static void Add()
		{
			BlueprintBuff BondsOfFellowshipBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "BondsOfFellowshipBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Bonds of Fellowship");
				bp.SetDescription(Main.IsekaiContext, "Fighting alongside the Hero inspires true courage. Allies within 30 feet receive a +2 Sacred bonus to attack rolls, Armor Class, and saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Fellowship;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
			});
			BlueprintAbilityAreaEffect BondsOfFellowshipArea = Helpers.CreateBlueprint(Main.IsekaiContext, "BondsOfFellowshipArea", delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
				bp.SpellResistance = false;
				bp.AggroEnemies = false;
				bp.AffectEnemies = false;
				bp.Shape = AreaEffectShape.Cylinder;
				bp.Size = new Feet(30f);
				bp.Fx = new PrefabLink();
				bp.AddUnconditionalAuraEffect(BondsOfFellowshipBuff.ToReference<BlueprintBuffReference>());
			});
			BlueprintBuff BondsOfFellowshipAreaBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "BondsOfFellowshipAreaBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Bonds of Fellowship Aura");
				bp.SetDescription(Main.IsekaiContext, "Emits a 30-foot aura of selfless fellowship empowering nearby allies.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Fellowship;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddAreaEffect c)
				{
					c.m_AreaEffect = BondsOfFellowshipArea.ToReference<BlueprintAbilityAreaEffectReference>();
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "BondsOfFellowshipFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Bonds of Fellowship");
				bp.SetDescription(Main.IsekaiContext, "At 3rd level, the Hero's unwavering devotion to their companions manifests as an aura of fellowship. Allies within 30 feet receive a +2 Sacred bonus to attack, AC, and all saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Fellowship;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { BondsOfFellowshipAreaBuff.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
