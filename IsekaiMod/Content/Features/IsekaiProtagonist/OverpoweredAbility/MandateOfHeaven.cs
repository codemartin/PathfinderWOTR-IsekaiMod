using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;

internal class MandateOfHeaven
{
	private static readonly Sprite Icon_Mandate = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_AURA_DIVINE.png");

	public static void Add()
	{
		BlueprintBuff buff = TTCoreExtensions.CreateBuff("MandateOfHeavenBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Imperial Grace of Heaven");
			bp.SetDescription(Main.IsekaiContext, "Under the sovereign mandate of heaven, you receive a +4 sacred bonus to Armor Class, attack rolls, damage rolls, and saving throws, Fast Healing 5, and immunity to fear and mind-affecting effects.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Mandate;
			bp.IsClassFeature = true;
			StatType[] acAttackDamageSaves = PortedAuras.AcAttackDamageSaves;
			foreach (StatType stat in acAttackDamageSaves)
			{
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = stat;
					c.Value = 4;
				});
			}
			bp.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 5;
				c.Bonus = 0;
			});
			PortedImmunities.AddFearImmunity(bp);
			PortedImmunities.AddMindAffectingImmunity(bp, SpellDescriptor.None);
		});
		BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleAuraFeature("MandateOfHeaven", Helpers.CreateString(Main.IsekaiContext, "MandateOfHeaven.Name", "Overpowered Ability - Mandate of Heaven"), Helpers.CreateString(Main.IsekaiContext, "MandateOfHeaven.Description", "Exclusive to the God Emperor archetype. By divine cosmic decree, you radiate absolute imperial authority that uplifts all who serve beneath your banner.\nBenefit: You emit an aura within 50 feet. Allies inside the aura gain a +4 sacred bonus to Armor Class, attack rolls, damage rolls, and all saving throws, Fast Healing 5, and total immunity to fear and mind-affecting effects."), Icon_Mandate, delegate(BlueprintAbilityAreaEffect area)
		{
			PortedAuras.ConfigureAllyAura(area, buff, 50);
		});
		blueprintFeature.AddComponent(delegate(PrerequisiteArchetypeLevel c)
		{
			c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
			c.m_Archetype = BlueprintTools.GetModBlueprintReference<BlueprintArchetypeReference>(Main.IsekaiContext, "GodEmperorArchetype");
			c.Level = 1;
		});
		OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
	}
}
