using System;
using System.Collections.Generic;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
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

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;

internal class PowerLeveling
{
	public static void Add()
	{
		Sprite Icon_DimensionalAnchor = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("c0aa77246b26433fa79c8ac09b1e70d9"))?.m_Icon;
		LocalizedString PowerLevelingName = Helpers.CreateString(Main.IsekaiContext, "PowerLeveling.Name", "Overpowered Ability - Power Leveling");
		LocalizedString PowerLevelingDesc = Helpers.CreateString(Main.IsekaiContext, "PowerLeveling.Description", "You are Overpowered but overly cautious. You use overwhelming force to ensure all enemies you kill are thoroughly defeated.\nBenefit: Your party gains double experience from all sources while below the level the current chapter is built for. At that level the bonus tapers to x1.25, and past it you gain normal experience; Power Leveling never reduces the experience you would have earned without it.\nYou also project an Overpowered Surge in a 40-foot radius, granting you and your allies a +2 untyped bonus to attack rolls and Fortitude saving throws.\nRequires character level 5.");
		BlueprintBuff PowerLevelingTempBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "PowerLevelingTempBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Overpowered Surge");
			bp.SetDescription(Main.IsekaiContext, "You are within 40 feet of an Overpowered protagonist. +2 untyped bonus to attack rolls and Fortitude saving throws.");
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
			bp.m_Flags = (BlueprintBuff.Flags)0;
		});
		BlueprintAbilityAreaEffect PowerLevelingAura = Helpers.CreateBlueprint(Main.IsekaiContext, "PowerLevelingAura", delegate(BlueprintAbilityAreaEffect bp)
		{
			bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
			bp.SpellResistance = false;
			bp.AggroEnemies = false;
			bp.AffectEnemies = false;
			bp.Shape = AreaEffectShape.Cylinder;
			bp.Size = new Feet(40f);
			bp.Fx = new PrefabLink();
			bp.AddComponent(delegate(AbilityAreaEffectBuff c)
			{
				c.m_Buff = PowerLevelingTempBuff.ToReference<BlueprintBuffReference>();
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
			bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 5;
			});
		}));
	}

	internal static int SoftCapForChapter(int chapter)
	{
		return chapter switch
		{
			0 => 3, 
			1 => 6, 
			2 => 11, 
			3 => 15, 
			4 => 18, 
			_ => 40, 
		};
	}

	internal static double ExperienceMultiplier(int chapter, int characterLevel)
	{
		int num = SoftCapForChapter(chapter);
		if (characterLevel > num)
		{
			return 1.0;
		}
		if (characterLevel == num)
		{
			return 1.25;
		}
		return 2.0;
	}

	internal static int HighestPartyLevel(IEnumerable<int> partyLevels)
	{
		int num = 0;
		if (partyLevels != null)
		{
			foreach (int partyLevel in partyLevels)
			{
				if (partyLevel > num)
				{
					num = partyLevel;
				}
			}
		}
		if (num <= 0)
		{
			return 1;
		}
		return num;
	}

	internal static int ApplyExperienceMultiplier(int gained, int chapter, int characterLevel)
	{
		if (gained <= 0)
		{
			return gained;
		}
		long val = (long)Math.Round((double)gained * ExperienceMultiplier(chapter, characterLevel));
		return (int)Math.Min(2147483647L, val);
	}
}
