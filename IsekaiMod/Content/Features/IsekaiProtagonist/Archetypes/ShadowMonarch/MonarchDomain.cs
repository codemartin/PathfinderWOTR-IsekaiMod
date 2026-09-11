using IsekaiMod.Components;
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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch
{
	internal class MonarchDomain
	{
		private static readonly Sprite Icon_Domain = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ad5ed5ea4ec52334a94e975a64dad336"))?.m_Icon;

		public static void Add()
		{
			BlueprintBuff MonarchDomainBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "MonarchDomainBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Monarch's Domain");
				bp.SetDescription(Main.IsekaiContext, "The ground turns pitch black beneath the Monarch's will. Allies and shadows receive +1 additional attack on a full attack, a +4 bonus to attack rolls, a +10 ft speed bonus, and deal +2d6 unholy damage on weapon attacks.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Domain;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddExtraAttack c)
				{
					c.Number = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
			});
			BlueprintAbilityAreaEffect MonarchDomainArea = Helpers.CreateBlueprint(Main.IsekaiContext, "MonarchDomainArea", delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
				bp.SpellResistance = false;
				bp.AggroEnemies = false;
				bp.AffectEnemies = false;
				bp.Shape = AreaEffectShape.Cylinder;
				bp.Size = new Feet(30f);
				bp.Fx = new PrefabLink();
				bp.AddUnconditionalAuraEffect(MonarchDomainBuff.ToReference<BlueprintBuffReference>());
			});
			BlueprintBuff MonarchDomainAreaBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "MonarchDomainAreaBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Monarch's Domain Aura");
				bp.SetDescription(Main.IsekaiContext, "Emits a 30-foot domain of shadows empowering all allies and shadows.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Domain;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddAreaEffect c)
				{
					c.m_AreaEffect = MonarchDomainArea.ToReference<BlueprintAbilityAreaEffectReference>();
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MonarchDomainFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Monarch's Domain");
				bp.SetDescription(Main.IsekaiContext, "At 10th level, the Shadow Monarch manifests Monarch's Domain in a 30-foot aura around themselves. The ground turns into black shadow, granting all allies and shadows +1 extra attack, +4 attack rolls, +10 ft speed, and +2d6 unholy damage on all strikes.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Domain;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { MonarchDomainAreaBuff.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
