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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod
{
	internal class MartialVelocity
	{
		private const string Name = "Martial Velocity";

		private const string DescriptionBuff = "This character circulates divine Ki with blinding speed: +1 additional attack to their main weapon, a +10 ft bonus to speed, and enemy attacks have a 50% displacement miss chance.";

		private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "MartialVelocity.Description", "The Martial God and allies within 120 feet move with supernatural velocity fueled by sovereign Ki circulation, gaining 1 additional attack to their main weapon, a +10 ft bonus to speed, and a 50% displacement miss chance against enemy attacks.");

		public static void Add()
		{
			Sprite Icon_MartialVelocity = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_CHUUNIBYOU.png");
			BlueprintBuff MartialVelocityBuff = TTCoreExtensions.CreateBuff("MartialVelocityBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Martial Velocity");
				bp.SetDescription(Main.IsekaiContext, "This character circulates divine Ki with blinding speed: +1 additional attack to their main weapon, a +10 ft bonus to speed, and enemy attacks have a 50% displacement miss chance.");
				bp.IsClassFeature = true;
				((BlueprintUnitFact)bp).m_Icon = Icon_MartialVelocity;
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
			BlueprintAbilityAreaEffect MartialVelocityArea = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialVelocityArea", delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
				bp.SpellResistance = false;
				bp.AggroEnemies = false;
				bp.AffectEnemies = false;
				bp.Shape = AreaEffectShape.Cylinder;
				bp.Size = new Feet(120f);
				bp.Fx = new PrefabLink();
				bp.AddUnconditionalAuraEffect(MartialVelocityBuff.ToReference<BlueprintBuffReference>());
			});
			BlueprintBuff MartialVelocityAreaBuff = TTCoreExtensions.CreateBuff("MartialVelocityAreaBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Martial Velocity");
				bp.SetDescription(Description);
				((BlueprintUnitFact)bp).m_Icon = Icon_MartialVelocity;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddAreaEffect c)
				{
					c.m_AreaEffect = MartialVelocityArea.ToReference<BlueprintAbilityAreaEffectReference>();
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MartialVelocityFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Martial Velocity");
				bp.SetDescription(Description);
				((BlueprintUnitFact)bp).m_Icon = Icon_MartialVelocity;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { MartialVelocityAreaBuff.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
