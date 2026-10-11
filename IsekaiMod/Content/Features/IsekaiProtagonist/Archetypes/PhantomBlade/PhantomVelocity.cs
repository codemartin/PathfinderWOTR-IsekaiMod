using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.PhantomBlade;

internal class PhantomVelocity
{
	private const string Name = "Phantom Velocity";

	private const string DescriptionBuff = "This character moves with blinding speed: +1 additional attack to their main weapon, a +10 ft bonus to speed, and enemy attacks have a 50% displacement miss chance.";

	private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "PhantomVelocity.Description", "The Phantom Blade and allies within 120 feet move with supernatural velocity, gaining 1 additional attack to their main weapon, a +10 ft bonus to speed, and a 50% displacement miss chance against enemy attacks.");

	public static void Add()
	{
		Sprite Icon_PhantomVelocity = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_CHUUNIBYOU.png");
		BlueprintBuff PhantomVelocityBuff = TTCoreExtensions.CreateBuff("PhantomVelocityBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Phantom Velocity");
			bp.SetDescription(Main.IsekaiContext, "This character moves with blinding speed: +1 additional attack to their main weapon, a +10 ft bonus to speed, and enemy attacks have a 50% displacement miss chance.");
			bp.IsClassFeature = true;
			((BlueprintUnitFact)bp).m_Icon = Icon_PhantomVelocity;
			bp.AddComponent(delegate(AddExtraAttack c)
			{
				c.Number = 1;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Speed;
				c.Value = 10;
			});
			bp.AddComponent(delegate(SetAttackerMissChance c)
			{
				c.m_Type = SetAttackerMissChance.Type.All;
				c.Value = 50;
				c.Conditions = ActionFlow.EmptyCondition();
			});
		});
		BlueprintAbilityAreaEffect PhantomVelocityArea = Helpers.CreateBlueprint(Main.IsekaiContext, "PhantomVelocityArea", delegate(BlueprintAbilityAreaEffect bp)
		{
			bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
			bp.SpellResistance = false;
			bp.AggroEnemies = false;
			bp.AffectEnemies = false;
			bp.Shape = AreaEffectShape.Cylinder;
			bp.Size = new Feet(120f);
			bp.Fx = new PrefabLink();
			bp.AddUnconditionalAuraEffect(PhantomVelocityBuff.ToReference<BlueprintBuffReference>());
		});
		BlueprintBuff PhantomVelocityAreaBuff = TTCoreExtensions.CreateBuff("PhantomVelocityAreaBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Phantom Velocity");
			bp.SetDescription(Description);
			((BlueprintUnitFact)bp).m_Icon = Icon_PhantomVelocity;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddAreaEffect c)
			{
				c.m_AreaEffect = PhantomVelocityArea.ToReference<BlueprintAbilityAreaEffectReference>();
			});
		});
		BlueprintFeature PhantomVelocityFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "PhantomVelocityFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Phantom Velocity");
			bp.SetDescription(Description);
			((BlueprintUnitFact)bp).m_Icon = Icon_PhantomVelocity;
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { PhantomVelocityAreaBuff.ToReference<BlueprintUnitFactReference>() };
			});
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "ChuunibyouActualisationFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Phantom Velocity");
			bp.SetDescription(Description);
			((BlueprintUnitFact)bp).m_Icon = Icon_PhantomVelocity;
			bp.ComponentsArray = PhantomVelocityFeature.ComponentsArray;
		});
	}
}
