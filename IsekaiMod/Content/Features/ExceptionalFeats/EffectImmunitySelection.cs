using System;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Localization;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.ExceptionalFeats
{
	internal class EffectImmunitySelection
	{
		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "StatDamageNegativeLevelImmunity", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Energy Drain and Negative level Immunity");
				bp.SetDescription(Main.IsekaiContext, "You gain immunity to ability score damage and negative levels.");
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.StatDebuff | SpellDescriptor.NegativeLevel;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.StatDebuff | SpellDescriptor.NegativeLevel;
				});
				bp.AddComponent(delegate(AddImmunityToAbilityScoreDamage c)
				{
					c.Drain = true;
				});
				bp.AddComponent<AddImmunityToEnergyDrain>();
			});
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "SneakAttackImmunity", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Sneak attack Immunity");
				bp.SetDescription(Main.IsekaiContext, "You gain immunity to sneak attacks.");
				bp.AddComponent<AddImmunityToPrecisionDamage>();
			});
			BlueprintFeature blueprintFeature2 = Helpers.CreateBlueprint(Main.IsekaiContext, "CriticalHitImmunity", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Critical hit Immunity");
				bp.SetDescription(Main.IsekaiContext, "You gain immunity to critical hits.");
				bp.AddComponent<AddImmunityToCriticalHits>();
			});
			BlueprintFeature[] source = new BlueprintFeature[31]
			{
				CreateImmunity("BlindImmunity", "You gain immunity to blindness.", UnitCondition.Blindness, SpellDescriptor.Blindness),
				CreateImmunity("NauseatedImmunity", "You gain immunity to the nauseated condition.", UnitCondition.Nauseated, SpellDescriptor.Nauseated),
				CreateImmunity("FatigueImmunity", "You gain immunity to fatigue.", UnitCondition.Fatigued, SpellDescriptor.Fatigue),
				CreateImmunity("ParalyzedImmunity", "You gain immunity to paralysis.", UnitCondition.Paralyzed, SpellDescriptor.Paralysis),
				CreateImmunity("StaggeredImmunity", "You gain immunity to the staggered condition.", UnitCondition.Staggered, SpellDescriptor.Staggered),
				CreateImmunity("PetrifiedImmunity", "You gain immunity to petrification.", UnitCondition.Petrified, SpellDescriptor.Petrified),
				CreateImmunity("DazedImmunity", "You gain immunity to daze.", UnitCondition.Dazed, SpellDescriptor.Daze),
				CreateImmunity("SlowImmunity", "You gain immunity to slow.", UnitCondition.Slowed),
				CreateImmunity("EntangledImmunity", "You gain immunity to the entangled condition.", UnitCondition.Entangled),
				CreateImmunity("FrightenedImmunity", "You gain immunity to the frightened condition.", UnitCondition.Frightened, SpellDescriptor.Frightened),
				CreateImmunity("SickenedImmunity", "You gain immunity to the sickened condition.", UnitCondition.Sickened, SpellDescriptor.Sickened),
				CreateImmunity("SleepImmunity", "You gain immunity to sleep.", UnitCondition.Sleeping, SpellDescriptor.Sleep),
				CreateImmunity("ShakenImmunity", "You gain immunity to shaken effects.", UnitCondition.Shaken, SpellDescriptor.Shaken),
				CreateImmunity("DazzledImmunity", "You gain immunity to the dazzled condition.", UnitCondition.Dazzled),
				CreateImmunity("StunImmunity", "You gain immunity to stun effects.", UnitCondition.Stunned, SpellDescriptor.Stun),
				CreateImmunity("ConfusionImmunity", "You gain immunity to confusion.", UnitCondition.Confusion, SpellDescriptor.Confusion),
				CreateImmunity("MovementImpairingImmunity", "You gain immunity to movement impairing effects.", UnitCondition.MovementBan, SpellDescriptor.MovementImpairing),
				CreateImmunity("CoweringImmunity", "You gain immunity to cowering.", UnitCondition.Cowering),
				CreateImmunity("ExhaustedImmunity", "You gain immunity to exhaustion.", UnitCondition.Exhausted),
				CreateImmunity("MindAffectingImmunity", "You gain immunity to mind affecting effects.", SpellDescriptor.MindAffecting),
				CreateImmunity("FearImmunity", "You gain immunity to fear effects.", SpellDescriptor.Fear),
				CreateImmunity("CompulsionImmunity", "You gain immunity to compulsion effects.", SpellDescriptor.Compulsion),
				CreateImmunity("PoisonImmunity", "You gain immunity to poison.", SpellDescriptor.Poison),
				CreateImmunity("DiseaseImmunity", "You gain immunity to disease.", SpellDescriptor.Disease),
				CreateImmunity("CharmImmunity", "You gain immunity to charm.", SpellDescriptor.Charm),
				CreateImmunity("CurseImmunity", "You gain immunity to curses.", SpellDescriptor.Curse),
				CreateImmunity("DeathImmunity", "You gain immunity to death effects.", SpellDescriptor.Death),
				CreateImmunity("BleedImmunity", "You gain immunity to bleed.", SpellDescriptor.Bleed),
				CreateImmunity("HexImmunity", "You gain immunity to hexes.", SpellDescriptor.Hex),
				blueprintFeature,
				blueprintFeature2
			};
			BlueprintFeatureReference[] EffectImmunityList = source.Select((BlueprintFeature bp) => bp.ToReference<BlueprintFeatureReference>()).ToArray();
			LocalizedString EffectImmunitySelectionDesc = Helpers.CreateString(Main.IsekaiContext, "EffectImmunitySelection.Description", "You gain immunity to a specific condition or effect.");
			BlueprintFeatureSelection selection = Helpers.CreateBlueprint(Main.IsekaiContext, "EffectImmunitySelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Effect Immunity");
				bp.SetDescription(EffectImmunitySelectionDesc);
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.m_Features = EffectImmunityList;
				bp.m_AllFeatures = EffectImmunityList;
			});
			BlueprintFeatureSelection bonusSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "EffectImmunityBonusSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Effect Immunity");
				bp.SetDescription(EffectImmunitySelectionDesc);
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.m_Features = EffectImmunityList;
				bp.m_AllFeatures = EffectImmunityList;
			});
			ExceptionalFeatSelection.AddToSelection(selection, bonusSelection);
		}

		private static BlueprintFeature CreateImmunity(string name, string description, UnitCondition condition, SpellDescriptor descriptor)
		{
			return CreateImmunity(name, description, delegate(BlueprintFeature bp)
			{
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = condition;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = descriptor;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = descriptor;
				});
			});
		}

		private static BlueprintFeature CreateImmunity(string name, string description, UnitCondition condition)
		{
			return CreateImmunity(name, description, delegate(BlueprintFeature bp)
			{
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = condition;
				});
			});
		}

		private static BlueprintFeature CreateImmunity(string name, string description, SpellDescriptor descriptor)
		{
			return CreateImmunity(name, description, delegate(BlueprintFeature bp)
			{
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = descriptor;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = descriptor;
				});
			});
		}

		private static BlueprintFeature CreateImmunity(string name, string description, Action<BlueprintFeature> init = null)
		{
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, string.Concat(name.Select((char x) => (!char.IsUpper(x)) ? x.ToString() : (" " + x))).TrimStart(' '));
				bp.SetDescription(Main.IsekaiContext, description);
			});
			init?.Invoke(blueprintFeature);
			return blueprintFeature;
		}
	}
}
