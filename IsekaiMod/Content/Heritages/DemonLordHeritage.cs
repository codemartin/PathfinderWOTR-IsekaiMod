using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages
{
	internal class DemonLordHeritage
	{
		public static void Add()
		{
			Sprite Icon_Succubus = AssetLoader.LoadInternal(Main.IsekaiContext, "Heritages", "ICON_SUCCUBUS.png");
			BlueprintActivatableAbility DevilWingsAbility = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(Main.IsekaiContext, "DevilWingsAbility");
			BlueprintActivatableAbility DemonWingsAbility = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(Main.IsekaiContext, "DemonWingsAbility");
			BlueprintActivatableAbility BlackWingsAbility = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(Main.IsekaiContext, "BlackWingsAbility");
			BlueprintAbility AbyssalHellfireAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "AbyssalHellfireAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Abyssal Hellfire");
				bp.SetDescription(Main.IsekaiContext, "Exhale a 30 ft cone of pure Abyssal hellfire. Deals 1d6 damage per 2 character levels (half fire, half unholy). Reflex save halves.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Succubus;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Close;
				bp.CanTargetPoint = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.BreathWeapon;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AddComponent(delegate(AbilityTargetsAround c)
				{
					c.m_Radius = 30.Feet();
					c.m_TargetType = TargetType.Enemy;
					c.m_IncludeDead = false;
				});
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(new ContextActionDealDamage
					{
						DamageType = new DamageTypeDescription
						{
							Type = DamageType.Energy,
							Energy = DamageEnergyType.Unholy
						},
						Value = new ContextDiceValue
						{
							DiceType = DiceType.D6,
							DiceCountValue = Values.CreateContextRankValue(AbilityRankType.Default),
							BonusValue = 0
						},
						HalfIfSaved = true
					});
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Div2;
				});
			});
			BlueprintBuff DemonLordAuraBuff = TTCoreExtensions.CreateBuff("DemonLordAuraBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Demon Lord's Dominance");
				bp.SetDescription(Main.IsekaiContext, "Crushed by the oppressive presence of an Abyssal Sovereign, enemies suffer a -2 penalty to Will saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Succubus;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveWill;
					c.Value = -2;
				});
				bp.m_Flags = BlueprintBuff.Flags.Harmful;
			});
			BlueprintFeature DemonLordAuraFeature = TTCoreExtensions.CreateToggleAuraFeature("DemonLordAuraFeature", Helpers.CreateString(Main.IsekaiContext, "DemonLordAura.Name", "Demon Lord's Presence"), Helpers.CreateString(Main.IsekaiContext, "DemonLordAura.Desc", "Enemies within 30 ft suffer a -2 penalty to Will saving throws."), Icon_Succubus, delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Enemy;
				bp.Size = new Feet(30f);
				bp.AddComponent(delegate(AbilityAreaEffectRunAction c)
				{
					c.UnitEnter = ActionFlow.DoSingle(delegate(ContextActionApplyBuff a)
					{
						a.m_Buff = DemonLordAuraBuff.ToReference<BlueprintBuffReference>();
						a.Permanent = true;
					});
					c.UnitExit = ActionFlow.DoSingle(delegate(ContextActionRemoveBuff r)
					{
						r.m_Buff = DemonLordAuraBuff.ToReference<BlueprintBuffReference>();
					});
				});
			});
			BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiDemonLordHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Isekai Demon Lord");
				bp.SetDescription(Main.IsekaiContext, "Reincarnated into Golarion as an authentic Demon Lord, you command the dread might and arcane majesty of the Abyss. You suffer no physical penalties and tower over mortal fiends.\nThe Isekai Demon Lord has a +4 racial bonus to Strength and Charisma, a +2 racial bonus to Constitution and Intelligence, and a +2 racial bonus on Persuasion and Perception checks.\nThey possess DR 10/Cold Iron or Good, Spell Resistance 11 + character level, immunity to fire, electricity, and poison, acid and cold resistance 20, the Abyssal Hellfire breath, Demon Lord's Presence aura, and fiendish wings.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Succubus;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Charisma;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Constitution;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Intelligence;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.SkillPerception;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.SkillPersuasion;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Or = true;
					c.Value = 10;
					c.BypassedByMaterial = true;
					c.BypassedByAlignment = true;
					c.Material = PhysicalDamageMaterial.ColdIron;
					c.Alignment = DamageAlignment.Good;
				});
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.BonusValue;
					c.m_StepLevel = 11;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Acid;
					c.Value = 20;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Cold;
					c.Value = 20;
				});
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.Electricity;
				});
				bp.AddComponent(delegate(AddEnergyImmunity c)
				{
					c.Type = DamageEnergyType.Fire;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Fire | SpellDescriptor.Electricity | SpellDescriptor.Poison;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Fire | SpellDescriptor.Electricity | SpellDescriptor.Poison;
				});
				List<BlueprintUnitFactReference> facts = new List<BlueprintUnitFactReference>();
				if (AbyssalHellfireAbility != null)
				{
					facts.Add(AbyssalHellfireAbility.ToReference<BlueprintUnitFactReference>());
				}
				if (DemonLordAuraFeature != null)
				{
					facts.Add(DemonLordAuraFeature.ToReference<BlueprintUnitFactReference>());
				}
				if (DevilWingsAbility != null)
				{
					facts.Add(DevilWingsAbility.ToReference<BlueprintUnitFactReference>());
				}
				if (DemonWingsAbility != null)
				{
					facts.Add(DemonWingsAbility.ToReference<BlueprintUnitFactReference>());
				}
				if (BlackWingsAbility != null)
				{
					facts.Add(BlackWingsAbility.ToReference<BlueprintUnitFactReference>());
				}
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = facts.ToArray();
				});
				bp.Groups = new FeatureGroup[2]
				{
					FeatureGroup.Racial,
					FeatureGroup.TieflingHeritage
				};
				bp.ReapplyOnLevelUp = true;
			});
			FeatTools.Selections.TieflingHeritageSelection.AddToSelection(feature);
			HumanHeritageSelection.Register(feature);
		}
	}
}
