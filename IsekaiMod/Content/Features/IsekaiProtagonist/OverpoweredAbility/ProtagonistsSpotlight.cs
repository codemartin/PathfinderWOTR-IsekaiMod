using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class ProtagonistsSpotlight
	{
		private static readonly Sprite Icon_Spotlight = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_AURA_MAJESTIC.png");

		public static void Add()
		{
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "ProtagonistsSpotlightFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Protagonist's Spotlight");
				bp.SetDescription(Main.IsekaiContext, "You are the undisputed star of every festival, grand arena, and dramatic confrontation in the multiverse.\nBenefit: You gain a +4 morale bonus to Armor Class, attack rolls, damage rolls, and saving throws, a +15 competence bonus on Persuasion checks, and Fast Healing 5.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Spotlight;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillPersuasion;
					c.Value = 15;
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 5;
					c.Bonus = 0;
				});
			}));
		}
	}
}
