using System;
using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Prestige;

internal static class PrestigeIdentityFeatures
{
	private const ModifierDescriptor Untyped = ModifierDescriptor.UntypedStackable;

	private static BlueprintFeature Feature(string name, Sprite icon, int ranks, Action<BlueprintFeature> init)
	{
		return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintFeature bp)
		{
			((BlueprintUnitFact)bp).m_Icon = icon;
			bp.IsClassFeature = true;
			bp.Ranks = ranks;
			init(bp);
		});
	}

	private static void RankedBy(BlueprintFeature bp)
	{
		bp.AddComponent(delegate(ContextRankConfig c)
		{
			c.m_Type = AbilityRankType.Default;
			c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
			c.m_Feature = bp.ToReference<BlueprintFeatureReference>();
			c.m_Progression = ContextRankProgression.AsIs;
		});
	}

	private static void PerRank(BlueprintFeature bp, params StatType[] stats)
	{
		foreach (StatType stat in stats)
		{
			bp.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = stat;
				c.Multiplier = 1;
				c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
			});
		}
	}

	private static void Flat(BlueprintFeature bp, int value, params StatType[] stats)
	{
		foreach (StatType stat in stats)
		{
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = stat;
				c.Value = value;
			});
		}
	}

	private static void Immune(BlueprintFeature bp, SpellDescriptor descriptor)
	{
		bp.AddComponent(delegate(BuffDescriptorImmunity c)
		{
			c.Descriptor = descriptor;
		});
		bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
		{
			c.Descriptor = descriptor;
		});
		if ((descriptor & SpellDescriptor.NegativeLevel) != SpellDescriptor.None)
		{
			bp.AddComponent<AddImmunityToEnergyDrain>();
		}
	}

	private static void SpellResistanceBonus(BlueprintFeature bp, int value)
	{
		bp.AddComponent(delegate(AddSpellResistance c)
		{
			c.AddBonusToResistance = true;
			c.Value = new ContextValue
			{
				ValueType = ContextValueType.Simple,
				Value = value
			};
		});
	}

	private static void SpellDCs(BlueprintFeature bp, int value)
	{
		bp.AddComponent(delegate(IncreaseAllSpellsDC c)
		{
			c.Value = value;
			c.Descriptor = ModifierDescriptor.UntypedStackable;
		});
	}

	private static void WeaponDice(BlueprintFeature bp, DamageTypeDescription type, int dice)
	{
		bp.AddComponent(delegate(AddAdditionalWeaponDamage c)
		{
			c.DamageType = type;
			c.Value = new ContextDiceValue
			{
				DiceType = DiceType.D6,
				DiceCountValue = dice,
				BonusValue = 0
			};
		});
	}

	private static void CritConfirm(BlueprintFeature bp, int value)
	{
		bp.AddComponent(delegate(CriticalConfirmationBonus c)
		{
			c.Bonus = value;
			c.Value = 0;
		});
	}

	private static void Concealed(BlueprintFeature bp)
	{
		bp.AddComponent(delegate(AddConcealment c)
		{
			c.Descriptor = ConcealmentDescriptor.Blur;
			c.Concealment = Concealment.Partial;
			c.OnlyForAttacks = true;
		});
	}

	private static BlueprintBuff SummonBuff(string name, Sprite icon, Action<BlueprintBuff> init)
	{
		return TTCoreExtensions.CreateBuff(name, delegate(BlueprintBuff b)
		{
			((BlueprintUnitFact)b).m_Icon = icon;
			b.IsClassFeature = true;
			b.Stacking = StackingType.Replace;
			init(b);
		});
	}

	private static void BuffSummons(BlueprintFeature bp, BlueprintBuff buff)
	{
		bp.AddComponent(delegate(OnSpawnBuff c)
		{
			c.m_buff = buff.ToReference<BlueprintBuffReference>();
			c.IsInfinity = true;
		});
	}

	internal static void CreateEpic(Sprite crown, Sprite sword, Sprite shield, Sprite spell, Sprite dark)
	{
		int ranks = PrestigeLevelPlan.EpicMasteryLevels.Length;
		Feature("DemigodPortfolioFeature", crown, ranks, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Divine Portfolio");
			bp.SetDescription(Main.IsekaiContext, "Your portfolio of divine domains grows with every step towards apotheosis. You gain a +1 untyped bonus to Armor Class and to all saving throws for each time you have gained this feature (at Demigod levels 2, 4, 6, 8 and 9, up to +5).");
			RankedBy(bp);
			PerRank(bp, StatType.AC, StatType.SaveFortitude, StatType.SaveReflex, StatType.SaveWill);
		});
		Feature("DemigodMantleFeature", crown, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Mantle of Divinity");
			bp.SetDescription(Main.IsekaiContext, "At 3rd level, a mortal body can no longer hold you down. You gain damage reduction 10/- and fast healing 10.");
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 10;
			});
			bp.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 10;
				c.Bonus = 0;
			});
		});
		Feature("DemigodMajestyFeature", crown, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Divine Majesty");
			bp.SetDescription(Main.IsekaiContext, "At 5th level, mortals feel the weight of your regard. You gain a +4 untyped bonus to Charisma and a +10 untyped bonus to Persuasion checks.");
			Flat(bp, 4, StatType.Charisma);
			Flat(bp, 10, StatType.SkillPersuasion);
		});
		Feature("DemigodSalientFeature", crown, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Salient Divine Ability");
			bp.SetDescription(Main.IsekaiContext, "At 7th level, you shed the frailties of the living. You become immune to fear, paralysis and stunning, and your spell resistance increases by 10.");
			Immune(bp, SpellDescriptor.Fear | SpellDescriptor.Stun | SpellDescriptor.Paralysis);
			SpellResistanceBonus(bp, 10);
		});
		Feature("GodhunterResolveFeature", sword, ranks, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Heretic's Resolve");
			bp.SetDescription(Main.IsekaiContext, "Every hunt hardens you against the powers you stalk. You gain a +1 untyped bonus to attack rolls and Will saves for each time you have gained this feature (at Godhunter levels 2, 4, 6, 8 and 9, up to +5).");
			RankedBy(bp);
			PerRank(bp, StatType.AdditionalAttackBonus, StatType.SaveWill);
		});
		Feature("GodhunterWardBreakerFeature", sword, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Faithless Ward");
			bp.SetDescription(Main.IsekaiContext, "At 3rd level, divine magic finds little purchase on you. Your spell resistance increases by 10 and you gain a +4 untyped bonus to Will saves.");
			SpellResistanceBonus(bp, 10);
			Flat(bp, 4, StatType.SaveWill);
		});
		Feature("GodhunterGodslayerEdgeFeature", sword, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Godslayer's Edge");
			bp.SetDescription(Main.IsekaiContext, "At 5th level, your blows find the seams in divine flesh. Your weapon attacks deal an additional 2d6 untyped damage, stacking with Deicide Strikes, and you gain a +10 bonus to confirm critical hits.");
			WeaponDice(bp, new DamageTypeDescription
			{
				Type = DamageType.Direct
			}, 2);
			CritConfirm(bp, 10);
		});
		Feature("GodhunterUnbowedFeature", sword, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Unbowed Before Heaven");
			bp.SetDescription(Main.IsekaiContext, "At 7th level, no god can make you kneel. You become immune to charm, compulsion and fear effects.");
			Immune(bp, SpellDescriptor.Fear | SpellDescriptor.Compulsion | SpellDescriptor.Charm);
		});
		Feature("WarmasterCommandFeature", shield, ranks, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Battlefield Command");
			bp.SetDescription(Main.IsekaiContext, "Every campaign teaches you more of war. You gain a +1 untyped bonus to Armor Class and to damage rolls for each time you have gained this feature (at Warmaster levels 2, 4, 6, 8 and 9, up to +5).");
			RankedBy(bp);
			PerRank(bp, StatType.AC, StatType.AdditionalDamage);
		});
		Feature("WarmasterGritFeature", shield, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Veteran's Grit");
			bp.SetDescription(Main.IsekaiContext, "At 3rd level, you have taken more wounds than most armies. You gain damage reduction 5/- and a +4 untyped bonus to Constitution.");
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 5;
			});
			Flat(bp, 4, StatType.Constitution);
		});
		Feature("WarmasterFormationFeature", shield, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Crushing Formation");
			bp.SetDescription(Main.IsekaiContext, "At 5th level, you break enemy lines and hold your own. You gain a +4 untyped bonus to combat maneuver bonus and combat maneuver defense.");
			Flat(bp, 4, StatType.AdditionalCMB, StatType.AdditionalCMD);
		});
		Feature("WarmasterUnbreakableLineFeature", shield, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Unbreakable Line");
			bp.SetDescription(Main.IsekaiContext, "At 7th level, you are the rock the army forms on. You become immune to fear, and gain 50% fortification against critical hits and sneak attacks.");
			Immune(bp, SpellDescriptor.Fear | SpellDescriptor.Shaken | SpellDescriptor.Frightened);
			bp.AddComponent(delegate(AddFortification c)
			{
				c.Bonus = 50;
			});
		});
		Feature("EternalSeekerDestinyFeature", spell, ranks, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Borrowed Destiny");
			bp.SetDescription(Main.IsekaiContext, "You borrow a little more from each life you have glimpsed. You gain a +1 untyped bonus to all skill checks and to initiative for each time you have gained this feature (at Eternal Seeker levels 2, 4, 6, 8 and 9, up to +5).");
			RankedBy(bp);
			bp.AddComponent(delegate(BuffAllSkillsBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Value = 1;
				c.Multiplier = Values.CreateContextRankValue(AbilityRankType.Default);
			});
			PerRank(bp, StatType.Initiative);
		});
		Feature("EternalSeekerGlimpseFeature", spell, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Glimpse of Timelines");
			bp.SetDescription(Main.IsekaiContext, "At 3rd level, you see the blow that lands in the timeline next door. You gain a +2 untyped bonus to Armor Class and Reflex saves and a +4 untyped bonus to initiative.");
			Flat(bp, 2, StatType.AC, StatType.SaveReflex);
			Flat(bp, 4, StatType.Initiative);
		});
		Feature("EternalSeekerGraspFeature", spell, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Scholar's Grasp");
			bp.SetDescription(Main.IsekaiContext, "At 5th level, the magic of a thousand schools answers you. Your caster level increases by 1 and you gain a +2 bonus to spell penetration checks.");
			bp.AddComponent(delegate(AddCasterLevel c)
			{
				c.Bonus = 1;
			});
			bp.AddComponent(delegate(SpellPenetrationBonus c)
			{
				c.Value = 2;
			});
		});
		Feature("EternalSeekerEchoesFeature", spell, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Echoes of Every Path");
			bp.SetDescription(Main.IsekaiContext, "At 7th level, the skills of every version of you are yours. You gain a +2 bonus to the save DCs of your spells and a +2 untyped bonus to attack rolls.");
			SpellDCs(bp, 2);
			Flat(bp, 2, StatType.AdditionalAttackBonus);
		});
		Feature("ArchmageReservoirFeature", spell, ranks, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Arcane Reservoir");
			bp.SetDescription(Main.IsekaiContext, "Your mind holds more magic with every circle you master. You gain a +1 untyped bonus to Intelligence, Wisdom and Charisma for each time you have gained this feature (at Archmage levels 2, 4, 6, 8 and 9, up to +5), which raises your spell save DCs and bonus spells.");
			RankedBy(bp);
			PerRank(bp, StatType.Intelligence, StatType.Wisdom, StatType.Charisma);
		});
		Feature("ArchmageArcaneReservesFeature", spell, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Arcane Reserves");
			bp.SetDescription(Main.IsekaiContext, "At 3rd level, you gain 1 additional spell slot per day for every spell level from 1st to 9th.");
			bp.AddComponent(delegate(AddSpellsPerDay c)
			{
				c.Amount = 1;
				c.Levels = new int[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			});
		});
		Feature("ArchmageShapingFeature", spell, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Mastery of Shaping");
			bp.SetDescription(Main.IsekaiContext, "At 5th level, your caster level increases by 2.");
			bp.AddComponent(delegate(AddCasterLevel c)
			{
				c.Bonus = 2;
			});
		});
		Feature("ArchmageArcaneFireFeature", spell, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Arcane Fire");
			bp.SetDescription(Main.IsekaiContext, "At 7th level, your spells burn through any defense. You gain a further +4 bonus to spell penetration checks and a +2 bonus to the save DCs of your spells.");
			bp.AddComponent(delegate(SpellPenetrationBonus c)
			{
				c.Value = 4;
			});
			SpellDCs(bp, 2);
		});
		Feature("ShadowSovereignArmyFeature", dark, ranks, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Army of Shadows");
			bp.SetDescription(Main.IsekaiContext, "Every soldier you raise sharpens you. You gain a +1 untyped bonus to attack rolls and Armor Class for each time you have gained this feature (at Shadow Sovereign levels 2, 4, 6, 8 and 9, up to +5).");
			RankedBy(bp);
			PerRank(bp, StatType.AdditionalAttackBonus, StatType.AC);
		});
		Feature("ShadowSovereignVeilFeature", dark, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Veil of the Monarch");
			bp.SetDescription(Main.IsekaiContext, "At 3rd level, shadows cling to you. Attacks against you have a 20% miss chance (a blur effect, which does not stack with blur), and you gain a +10 untyped bonus to Stealth checks.");
			Concealed(bp);
			Flat(bp, 10, StatType.SkillStealth);
		});
		BlueprintBuff legionBuff = SummonBuff("ShadowSovereignLegionBuff", dark, delegate(BlueprintBuff b)
		{
			b.SetName(Main.IsekaiContext, "Monarch's Legion");
			b.SetDescription(Main.IsekaiContext, "A soldier of the Monarch's legion: +4 profane bonus to attack rolls and Armor Class. Profane bonuses do not stack, so this and the Night Legion give +4 together.");
			b.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Profane;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 4;
			});
			b.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
		});
		Feature("ShadowSovereignLegionFeature", dark, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Monarch's Legion");
			bp.SetDescription(Main.IsekaiContext, "At 5th level, your summons fight as a drilled legion. Every creature you summon gains a +4 profane bonus to attack rolls and Armor Class (it does not stack with the Night Legion's).");
			BuffSummons(bp, legionBuff);
		});
		Feature("ShadowSovereignVoidFeature", dark, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Void Resilience");
			bp.SetDescription(Main.IsekaiContext, "At 7th level, the cold of the void is home. You gain cold resistance 30 and negative energy resistance 30.");
			bp.AddComponent(delegate(AddDamageResistanceEnergy c)
			{
				c.Type = DamageEnergyType.Cold;
				c.Value = 30;
			});
			bp.AddComponent(delegate(AddDamageResistanceEnergy c)
			{
				c.Type = DamageEnergyType.NegativeEnergy;
				c.Value = 30;
			});
		});
	}

	internal static IEnumerable<BlueprintFeature> EpicIdentityAt(string className, int level)
	{
		string[] array = PrestigeLevelPlan.NewEpicIdentityAt(className, level);
		foreach (string text in array)
		{
			BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, text);
			if (modBlueprint == null)
			{
				Main.IsekaiContext.Logger.LogError($"[PrestigeIdentityFeatures] '{text}' was not built; {className} level {level} is missing it.");
			}
			else
			{
				yield return modBlueprint;
			}
		}
	}

	internal static void CreateSovereign(Sprite authority, Sprite domain)
	{
		Feature("SovereignDecreeFeature", authority, PrestigeLevelPlan.SovereignDecreeLevels.Length, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Sovereign Decree");
			bp.SetDescription(Main.IsekaiContext, "Your word becomes law a little more with every level. You gain a +1 untyped bonus to attack rolls and to Charisma for each time you have gained this feature (at Transcendent Sovereign levels 2, 4, 6 and 8, up to +4).");
			RankedBy(bp);
			PerRank(bp, StatType.AdditionalAttackBonus, StatType.Charisma);
		});
		Feature("SovereignCrownFeature", authority, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Crown of Stories");
			bp.SetDescription(Main.IsekaiContext, "At 9th level, the story itself bends around you. You gain a +2 bonus to the save DCs of your spells and a +2 untyped bonus to all saving throws. A Transcendent Sovereign archetype replaces this with its own apex.");
			SpellDCs(bp, 2);
			Flat(bp, 2, StatType.SaveFortitude, StatType.SaveReflex, StatType.SaveWill);
		});
		Feature("CelestialMonarchThroneFeature", domain, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Heavenly Throne");
			bp.SetDescription(Main.IsekaiContext, "At 9th level, replace Crown of Stories: the God Emperor's throne is yours. You gain a +4 untyped bonus to Charisma and a +2 bonus to the save DCs of your spells.");
			Flat(bp, 4, StatType.Charisma);
			SpellDCs(bp, 2);
		});
		Feature("DemonSlimeAppetiteFeature", domain, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Endless Appetite");
			bp.SetDescription(Main.IsekaiContext, "At 9th level, replace Crown of Stories: whatever the Devourer takes in, it keeps. You gain fast healing 10 and damage reduction 10/-.");
			bp.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 10;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 10;
			});
		});
		BlueprintBuff nightLegionBuff = SummonBuff("ShadowSovereignNightLegionBuff", domain, delegate(BlueprintBuff b)
		{
			b.SetName(Main.IsekaiContext, "Night Legion");
			b.SetDescription(Main.IsekaiContext, "A soldier of the Shadow Monarch's night legion: +4 profane bonus to attack rolls and Armor Class. Profane bonuses do not stack, so this and the Monarch's Legion give +4 together.");
			b.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Profane;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 4;
			});
			b.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
		});
		Feature("ShadowSovereignNightLegionFeature", domain, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Sovereign of the Night Legion");
			bp.SetDescription(Main.IsekaiContext, "At 9th level, replace Crown of Stories: the Shadow Monarch's army rises stronger. Every creature you summon gains a +4 profane bonus to attack rolls and Armor Class (it does not stack with the Monarch's Legion), and attacks against you have a 20% miss chance (a blur effect, which does not stack with blur).");
			BuffSummons(bp, nightLegionBuff);
			Concealed(bp);
		});
		Feature("GrandStrategistCheckmateFeature", domain, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Checkmate Foresight");
			bp.SetDescription(Main.IsekaiContext, "At 9th level, replace Crown of Stories: the Mastermind saw this coming. You gain a +4 untyped bonus to initiative, cannot be flanked, and gain a +2 bonus to the save DCs of your spells.");
			Flat(bp, 4, StatType.Initiative);
			bp.AddComponent(delegate(AddMechanicsFeature c)
			{
				c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.CannotBeFlanked;
			});
			SpellDCs(bp, 2);
		});
		Feature("SupremeOverlordDominionFeature", domain, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Dominion Over Death");
			bp.SetDescription(Main.IsekaiContext, "At 9th level, replace Crown of Stories: the Overlord has already died once. You become immune to death effects and negative levels, and gain a +2 bonus to the save DCs of your spells.");
			Immune(bp, SpellDescriptor.Death | SpellDescriptor.NegativeLevel);
			SpellDCs(bp, 2);
		});
		Feature("ParagonOfHopeUndyingFeature", domain, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Hope Undying");
			bp.SetDescription(Main.IsekaiContext, "At 9th level, replace Crown of Stories: the Hero never stays down. You become immune to fear, gain a +2 untyped bonus to all saving throws, and gain 50% fortification against critical hits and sneak attacks.");
			Immune(bp, SpellDescriptor.Fear | SpellDescriptor.Shaken | SpellDescriptor.Frightened);
			Flat(bp, 2, StatType.SaveFortitude, StatType.SaveReflex, StatType.SaveWill);
			bp.AddComponent(delegate(AddFortification c)
			{
				c.Bonus = 50;
			});
		});
		Feature("PhantomSovereignBladesFeature", domain, 1, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Ten Thousand Blades");
			bp.SetDescription(Main.IsekaiContext, "At 9th level, replace Crown of Stories: the Phantom Blade never swings alone. Your weapon attacks deal an additional 2d6 force damage, and you gain a +10 bonus to confirm critical hits.");
			WeaponDice(bp, new DamageTypeDescription
			{
				Type = DamageType.Force
			}, 2);
			CritConfirm(bp, 10);
		});
	}
}
