using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;

internal class OtherworldlyGourmet
{
	private static readonly Sprite Icon_Feast = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_REGENERATION.png");

	internal const int AuraFeet = 100;

	public static void Add()
	{
		BlueprintBuff vigor = TTCoreExtensions.CreateBuff("OtherworldlyGourmetVigorBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Otherworldly Feast: Hearty Meal");
			bp.SetDescription(Main.IsekaiContext, "The feast's hearty meal grants 50 temporary hit points, which last until they are spent or an hour has passed.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Feast;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(TemporaryHitPointsFromAbilityValue c)
			{
				c.Value = 50;
				c.RemoveWhenHitPointsEnd = true;
			});
		});
		BlueprintBuff sated = TTCoreExtensions.CreateBuff("OtherworldlyGourmetSatedBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Otherworldly Feast: Sated");
			bp.SetDescription(Main.IsekaiContext, "You have just eaten the feast's hearty meal and cannot gain its temporary hit points again for an hour.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Feast;
			bp.IsClassFeature = true;
		});
		BlueprintBuff buff = TTCoreExtensions.CreateBuff("OtherworldlyGourmetBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Otherworldly Feast");
			bp.SetDescription(Main.IsekaiContext, "Nourished by an otherworldly feast, you receive a +4 morale bonus to all six ability scores, Fast Healing 5, 50 temporary hit points (once an hour per person), and immunity to poison, disease, sickened, and nauseated conditions.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Feast;
			bp.IsClassFeature = true;
			StatType[] array = new StatType[6]
			{
				StatType.Strength,
				StatType.Dexterity,
				StatType.Constitution,
				StatType.Intelligence,
				StatType.Wisdom,
				StatType.Charisma
			};
			foreach (StatType stat in array)
			{
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = stat;
					c.Value = 4;
				});
			}
			bp.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 5;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(AddFactContextActions c)
			{
				c.Activated = Helpers.CreateActionList(new Conditional
				{
					ConditionsChecker = new ConditionsChecker
					{
						Conditions = new Condition[1]
						{
							new ContextConditionHasBuff
							{
								m_Buff = sated.ToReference<BlueprintBuffReference>(),
								Not = true
							}
						}
					},
					IfTrue = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = vigor.ToReference<BlueprintBuffReference>(),
						DurationValue = Values.Duration.OneHour
					}, new ContextActionApplyBuff
					{
						m_Buff = sated.ToReference<BlueprintBuffReference>(),
						DurationValue = Values.Duration.OneHour
					}),
					IfFalse = Helpers.CreateActionList()
				});
				c.Deactivated = Helpers.CreateActionList();
				c.NewRound = Helpers.CreateActionList();
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Sickened;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Nauseated;
			});
			PortedImmunities.AddHostileImmunity(bp, SpellDescriptor.Poison | SpellDescriptor.Disease);
		});
		OverpoweredAbilitySelection.AddToSelection(TTCoreExtensions.CreateToggleAuraFeature("OtherworldlyGourmet", Helpers.CreateString(Main.IsekaiContext, "OtherworldlyGourmet.Name", "Overpowered Ability - Otherworldly Gourmet"), Helpers.CreateString(Main.IsekaiContext, "OtherworldlyGourmet.Description", "Infusing cooking with interdimensional spices and metaphysical culinary techniques, your meals grant legendary nourishment.\nBenefit: You and your party receive a +4 morale bonus to all six ability scores (Strength, Dexterity, Constitution, Intelligence, Wisdom, Charisma), Fast Healing 5, 50 temporary hit points (once an hour per person), and complete immunity to poison, disease, sickened, and nauseated conditions. Party members share the feast while they are within 100 feet of you."), Icon_Feast, delegate(BlueprintAbilityAreaEffect area)
		{
			PortedAuras.ConfigureAllyAura(area, buff, 100);
		}));
	}
}
