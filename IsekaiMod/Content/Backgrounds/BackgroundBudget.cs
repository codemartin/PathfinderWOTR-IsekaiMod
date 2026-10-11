using System;
using System.Collections.Generic;
using System.Linq;

namespace IsekaiMod.Content.Backgrounds;

internal static class BackgroundBudget
{
	internal enum GrantKind
	{
		Stat,
		ClassSkills,
		SkillSwaps,
		SituationalSave,
		SituationalAttack,
		CasterOnly,
		Flat
	}

	internal sealed class Grant
	{
		internal GrantKind Kind;

		internal string Stat;

		internal string Descriptor;

		internal int Value;

		internal double Weight;

		internal string[] Components = new string[0];

		internal string What;

		public override string ToString()
		{
			return What ?? (Kind.ToString() + " " + Stat + " " + Descriptor + " " + Value);
		}
	}

	internal sealed class Background
	{
		internal string Blueprint;

		internal string Signature;

		internal string SignatureMechanic;

		internal string SignatureText;

		internal Grant[] Grants;
	}

	internal const double BandMin = 4.0;

	internal const double BandMax = 5.0;

	internal const int MaxAttributeBonus = 2;

	internal const int MaxAttributePoints = 6;

	internal static readonly Background[] Backgrounds = new Background[35]
	{
		B("BackgroundCollegeStudent", "All-Nighter", "immunity:sleep", "immune to sleep", S("Strength", "Trait", 1), S("Dexterity", "Trait", 1), S("Constitution", "Trait", 1), S("Intelligence", "Trait", 1), S("Wisdom", "Trait", 1), S("Charisma", "Trait", 1), Skills(2), Immune("sleep immunity", 1.0, 1)),
		B("BackgroundHighschoolStudent", "Growing Body", "immunity:ability damage and drain", "immune to ability score damage and drain", S("Strength", "Trait", 1), S("Dexterity", "Trait", 1), S("Constitution", "Trait", 1), S("SaveFortitude", "Competence", 2), Skills(2), Flat("ability damage and drain immunity", 1.5, "AddImmunityToAbilityScoreDamage")),
		B("BackgroundRationalist", "Unbeliever", "immunity:spells that allow spell resistance", "is immune to any", Flat("immune to spells that allow spell resistance, including allies' buffs and heals; cannot cast", 4.5, "AddSpellImmunity", "AreaEffectImmunity", "AddCondition")),
		B("BackgroundRebornDemonLord", "Lord of Storms", "immunity:electricity", "immunity to electricity", S("Strength", "Trait", 2), S("SaveWill", "Competence", 2), S("SkillPersuasion", "Competence", 4), Skills(1), Flat("electricity immunity", 2.0, "AddEnergyImmunity")),
		B("BackgroundMartialArtist", "Iron Fists", "unarmed strike size +1", "one size category larger", S("AC", "Dodge", 1), S("SkillAthletics", "Competence", 4), S("SkillMobility", "Competence", 4), Skills(2), Flat("Improved Unarmed Strike and all exotic weapon proficiencies", 2.0, "AddFacts"), Flat("unarmed strikes one size larger", 1.0, "WeaponSizeChange")),
		B("BackgroundModernSoldier", "Marksman Doctrine", "attack bonus:ranged", "ranged attack rolls", S("Initiative", "Competence", 2), S("SaveFortitude", "Competence", 2), S("SkillAthletics", "Competence", 3), S("SkillPerception", "Competence", 1), Skills(2), Flat("martial weapon proficiency", 1.0, "AddFacts"), Attack("+2 on ranged attack rolls", 2, "AttackTypeAttackBonus")),
		B("BackgroundGamer", "I-Frames", "feat:Evasion", "Evasion", S("Initiative", "Competence", 2), S("AC", "Dodge", 1), S("SaveReflex", "Competence", 2), S("SkillMobility", "Competence", 4), S("SkillKnowledgeWorld", "Competence", 4), S("SkillKnowledgeArcana", "Competence", 4), Skills(3), Swaps(1), Flat("Evasion", 1.0, "AddFacts")),
		B("BackgroundSpeedrunner", "Any% Route", "speed:+20 feet", "+20 foot bonus to base movement speed", S("Speed", "UntypedStackable", 20), S("Initiative", "Competence", 4), S("SaveReflex", "Competence", 2), S("AdditionalCMD", "Competence", 4), S("SkillMobility", "Competence", 4), S("SkillThievery", "Competence", 4), Skills(2), Swaps(1)),
		B("BackgroundChessGrandmaster", "Thinking Ahead", "initiative:+Intelligence bonus", "Intelligence bonus to Initiative", S("AC", "Insight", 2), S("SkillPerception", "Competence", 4), S("SkillKnowledgeWorld", "Competence", 4), Skills(2), Flat("Uncanny Dodge", 1.0, "AddFacts"), Flat("Intelligence bonus to initiative", 1.0, "DerivativeStatBonus", "RecalculateOnStatChange")),
		B("BackgroundTruckDriver", "Built Like a Truck", "damage reduction:2/-", "Damage Reduction 2/-", S("SaveFortitude", "Competence", 2), S("AdditionalCMB", "Competence", 2), S("SkillAthletics", "Competence", 4), Skills(1), Swaps(1), Immune("fear immunity", 1.5, 1), Flat("DR 2/-", 0.4, "AddDamageResistancePhysical"), Flat("greatclub and warhammer proficiency", 0.5)),
		B("BackgroundSalaryman", "Unpaid Overtime", "immunity:fatigue and exhaustion", "immune to fatigue and exhaustion", S("SaveWill", "Competence", 2), S("SaveFortitude", "Competence", 2), S("AC", "Competence", 1), Skills(2), Swaps(1), Immune("fatigue and exhaustion immunity", 1.5, 2)),
		B("BackgroundMasterChef", "Iron Stomach", "immunity:nausea and sickness", "immune to nausea and sickness", S("HitPoints", "UntypedStackable", 10), S("SaveFortitude", "Competence", 3), S("SaveWill", "Competence", 2), S("SkillLoreNature", "Competence", 4), S("SkillPerception", "Competence", 2), Skills(2), Flat("dagger and kukri proficiency", 0.5), Immune("nausea and sickened immunity", 1.0, 2)),
		B("BackgroundSurgeon", "Anatomy Expert", "critical confirmation +4", "+4 bonus on critical confirmation rolls", S("SaveFortitude", "Competence", 2), S("SaveReflex", "Competence", 2), S("SkillLoreNature", "Competence", 4), S("SkillPerception", "Competence", 4), Skills(2), Flat("dagger and scythe proficiency", 0.5), Save("+2 against poison and disease", 2, "SavingThrowBonusAgainstDescriptor"), Flat("+4 critical confirmation", 1.0, "CriticalConfirmationBonus"), Immune("bleed immunity", 0.5, 0)),
		B("BackgroundSoftwareEngineer", "Exploit the Code", "spell penetration +4", "+4 bonus on caster level checks to overcome spell resistance", S("SaveWill", "Competence", 2), S("Initiative", "Competence", 2), S("SkillKnowledgeArcana", "Competence", 4), S("SkillKnowledgeWorld", "Competence", 4), S("SkillThievery", "Competence", 3), S("SkillUseMagicDevice", "Competence", 3), Skills(4), Swaps(2), Save("+2 against enchantment", 2, "SavingThrowBonusAgainstSchool"), Save("+2 against illusion", 2, "SavingThrowBonusAgainstSchool"), Caster("+4 spell penetration", 1.0, "SpellPenetrationBonus")),
		B("BackgroundTabletopRPGPlayer", "Roll Twice", "reroll:skill checks", "rolls every skill check twice", S("SaveWill", "Competence", 2), Skills(4), Swaps(4), Save("+2 against mind-affecting", 2, "SavingThrowBonusAgainstDescriptor"), Flat("roll skill checks twice and take the better", 1.5, "ModifyD20")),
		B("BackgroundWebNovelAuthor", "Seen Every Plot Twist", "immunity:daze and confusion", "immune to daze and confusion", S("SaveWill", "Competence", 2), S("Initiative", "Competence", 2), S("SkillKnowledgeWorld", "Competence", 4), S("SkillLoreReligion", "Competence", 4), S("SkillKnowledgeArcana", "Competence", 4), Skills(3), Immune("daze and confusion immunity", 1.5, 2), Caster("+1 DC of illusion spells", 0.75, "IncreaseSpellSchoolDC")),
		B("BackgroundMusician", "Stage Presence", "will save:+Charisma bonus", "Charisma bonus to Will saves", S("SaveReflex", "Competence", 2), Flat("Charisma bonus to Will saves", 1.0, "DerivativeStatBonus", "RecalculateOnStatChange"), S("SkillPersuasion", "Competence", 4), S("SkillPerception", "Competence", 4), Skills(3), Caster("+2 caster level and +2 DC for sonic spells", 2.0, "IncreaseSpellDescriptorCasterLevel", "IncreaseSpellDescriptorDC"), Flat("sonic resistance 10", 0.75, "AddDamageResistanceEnergy")),
		B("BackgroundBetaTester", "Patch Notes", "all saves +2 insight", "+2 insight bonus on all saving throws", S("Initiative", "Competence", 4), S("SaveFortitude", "Insight", 2), S("SaveReflex", "Insight", 2), S("SaveWill", "Insight", 2), Skills(5)),
		B("BackgroundEnlightenedSage", "Third Eye", "concealment:ignore all", "ignores all concealment", Skills(5), Swaps(5), Immune("blindness immunity", 1.0, 1), Flat("ignores all concealment", 1.5, "IgnoreConcealment")),
		B("BackgroundOtaku", "Genre Savvy", "reroll:failed Reflex saves", "fails a Reflex save, she rolls it again", S("Initiative", "Competence", 2), S("SkillKnowledgeArcana", "Competence", 2), S("SkillKnowledgeWorld", "Competence", 2), S("SkillLoreNature", "Competence", 2), S("SkillLoreReligion", "Competence", 2), Skills(10), Flat("reroll failed Reflex saves", 1.5, "ModifyD20")),
		B("BackgroundDemonicCultivator", "Poison-Tempered Body", "immunity:poison", "immune to poison", S("SaveFortitude", "Competence", 2), S("AdditionalCMB", "Competence", 2), S("SkillAthletics", "Competence", 4), S("SkillMobility", "Competence", 4), Skills(2), Swaps(2), Immune("poison immunity", 1.0, 0), Flat("fire resistance 10", 0.75, "AddDamageResistanceEnergy")),
		B("BackgroundSpiritDetective", "Spirit Gun", "attack bonus:ranged touch", "ranged touch attack rolls", S("Initiative", "Competence", 4), S("SaveWill", "Competence", 2), S("SkillPerception", "Competence", 2), S("SkillLoreReligion", "Competence", 2), Skills(2), Attack("+2 sacred on ranged touch attack rolls", 2, "AttackTypeAttackBonus"), Save("+2 against death effects", 2, "SavingThrowBonusAgainstDescriptor")),
		B("BackgroundHollowedBladeVessel", "Soul Blade", "attack bonus:light and heavy blades", "attack rolls with swords", S("SkillPersuasion", "Competence", 2), S("SaveWill", "Competence", 2), S("SaveFortitude", "Competence", 2), S("AdditionalCMB", "Competence", 2), S("HitPoints", "UntypedStackable", 10), Skills(1), Attack("+2 on attack rolls with light and heavy blades", 2, "WeaponGroupAttackBonus", "WeaponGroupAttackBonus"), Save("+2 against fear", 2, "SavingThrowBonusAgainstDescriptor")),
		B("BackgroundKiMartialProdigy", "Gravity Training", "immunity:staggered and slowed", "immune to being staggered or slowed", S("SkillAthletics", "Competence", 2), S("SaveFortitude", "Competence", 2), S("SaveReflex", "Competence", 2), S("Speed", "UntypedStackable", 5), S("HitPoints", "UntypedStackable", 20), S("AdditionalCMB", "Competence", 4), Skills(1), Immune("staggered and slowed immunity", 1.0, 2)),
		B("BackgroundNenAuraSpecialist", "Ten", "armor class:+2 deflection", "+2 deflection bonus to AC", S("AC", "Deflection", 2), S("AC", "Dodge", 1), S("SkillPerception", "Competence", 2), S("SaveWill", "Competence", 2), Skills(1), Save("+2 against mind-affecting", 2, "SavingThrowBonusAgainstDescriptor")),
		B("BackgroundSubstitutionShinobi", "Substitution Jutsu", "concealment:20%", "20% concealment", S("SkillStealth", "Competence", 2), S("SkillMobility", "Competence", 2), S("SaveReflex", "Competence", 2), S("Initiative", "Competence", 2), S("AC", "Dodge", 1), Skills(2), Flat("20% miss chance against attacks", 1.5, "AddConcealment")),
		B("BackgroundGourmetHunter", "Gourmet Cells", "immunity:disease", "immune to disease", S("HitPoints", "UntypedStackable", 30), S("SkillLoreNature", "Competence", 2), S("SaveFortitude", "Competence", 2), S("SaveReflex", "Competence", 2), Skills(1), Save("+2 against poison", 2, "SavingThrowBonusAgainstDescriptor"), Immune("disease immunity", 1.0, 0), Flat("fast healing 2", 0.4, "AddEffectFastHealing")),
		B("BackgroundVRGuildMaster", "Cooldown Management", "all spells DC +1", "+1 bonus to the DC of all your spells", S("SkillUseMagicDevice", "Competence", 2), S("SkillKnowledgeWorld", "Competence", 2), S("Initiative", "Competence", 2), S("SaveWill", "Competence", 2), S("SaveReflex", "Competence", 2), S("AC", "Dodge", 1), S("HitPoints", "UntypedStackable", 10), Skills(1), Caster("+1 DC of all spells", 1.5, "IncreaseAllSpellsDC")),
		B("BackgroundCardDuelistKing", "Heart of the Cards", "reroll:initiative", "roll initiative twice", S("SkillPersuasion", "Competence", 2), S("SaveWill", "Competence", 2), S("SaveReflex", "Competence", 2), S("Initiative", "Competence", 2), S("HitPoints", "UntypedStackable", 10), S("AdditionalCMD", "Competence", 2), Skills(1), Save("+2 luck against fear and emotion", 2, "SavingThrowBonusAgainstDescriptor"), Flat("roll initiative twice and take the better", 1.0, "ModifyD20")),
		B("BackgroundTimeLeapingMadScientist", "Reading Steiner", "reroll:failed Will saves", "fail a Will save, you roll it again", S("SkillKnowledgeWorld", "Competence", 2), S("SkillKnowledgeArcana", "Competence", 2), S("Initiative", "Insight", 2), S("SaveReflex", "Competence", 2), S("SaveFortitude", "Competence", 2), Skills(2), Save("+2 against confusion and mind-affecting", 2, "SavingThrowBonusAgainstDescriptor"), Flat("reroll failed Will saves", 1.5, "ModifyD20")),
		B("BackgroundShrunkHighSchoolSleuth", "Only One Truth", "see invisibility", "see invisible creatures", S("SkillPerception", "Competence", 3), S("SkillKnowledgeWorld", "Competence", 2), S("Initiative", "Insight", 2), S("SaveWill", "Competence", 2), S("SaveReflex", "Competence", 2), Skills(2), Save("+4 against illusion", 4, "SavingThrowBonusAgainstSchool"), Flat("see invisibility", 1.5, "AddCondition")),
		B("BackgroundUndergroundGambler", "All In", "attack rolls +2 luck", "+2 luck bonus on attack rolls", S("AdditionalAttackBonus", "Luck", 2), S("SkillThievery", "Competence", 2), S("Initiative", "Competence", 2), S("SaveReflex", "Competence", 2), S("HitPoints", "UntypedStackable", 10), Skills(1), Save("+2 luck against mind-affecting", 2, "SavingThrowBonusAgainstDescriptor")),
		B("BackgroundStateAlchemist", "Equivalent Exchange", "transmutation spells DC +2", "Transmutation spells", S("SkillKnowledgeArcana", "Competence", 2), S("SaveFortitude", "Competence", 2), S("AdditionalCMB", "Competence", 2), S("Initiative", "Competence", 2), Skills(1), Caster("+2 DC of transmutation spells", 1.5, "IncreaseSpellSchoolDC"), Flat("acid resistance 10", 0.75, "AddDamageResistanceEnergy"), Flat("cold resistance 10", 0.75, "AddDamageResistanceEnergy")),
		B("BackgroundLimitBreakerTrainee", "One Punch", "damage rolls +2", "+2 bonus on damage rolls", S("AdditionalDamage", "UntypedStackable", 2), S("SkillAthletics", "Competence", 2), S("SaveFortitude", "Competence", 2), S("SaveWill", "Competence", 2), S("AC", "NaturalArmor", 1), S("HitPoints", "UntypedStackable", 10), S("AdditionalCMB", "Competence", 2), Skills(1)),
		B("BackgroundReincarnatedMobCharacter", "Shadow Garden", "sneak attack +1d6", "+1d6 sneak attack", S("SneakAttack", "UntypedStackable", 1), S("SkillStealth", "Competence", 4), S("SaveReflex", "Competence", 2), S("SaveWill", "Competence", 2), S("Initiative", "Competence", 2), S("AC", "Dodge", 1), S("HitPoints", "UntypedStackable", 10), Skills(2))
	};

	internal static readonly string[] Attributes = new string[6] { "Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma" };

	private static Grant S(string stat, string descriptor, int value)
	{
		return new Grant
		{
			Kind = GrantKind.Stat,
			Stat = stat,
			Descriptor = descriptor,
			Value = value
		};
	}

	private static Grant Skills(int count)
	{
		return new Grant
		{
			Kind = GrantKind.ClassSkills,
			Value = count
		};
	}

	private static Grant Swaps(int count)
	{
		return new Grant
		{
			Kind = GrantKind.SkillSwaps,
			Value = count
		};
	}

	private static Grant Save(string what, int value, params string[] components)
	{
		return new Grant
		{
			Kind = GrantKind.SituationalSave,
			What = what,
			Value = value,
			Components = components
		};
	}

	private static Grant Attack(string what, int value, params string[] components)
	{
		return new Grant
		{
			Kind = GrantKind.SituationalAttack,
			What = what,
			Value = value,
			Components = components
		};
	}

	private static Grant Caster(string what, double weight, params string[] components)
	{
		return new Grant
		{
			Kind = GrantKind.CasterOnly,
			What = what,
			Weight = weight,
			Components = components
		};
	}

	private static Grant Flat(string what, double weight, params string[] components)
	{
		return new Grant
		{
			Kind = GrantKind.Flat,
			What = what,
			Weight = weight,
			Components = components
		};
	}

	private static Grant Immune(string what, double weight, int conditions)
	{
		return Flat(what, weight, new string[2] { "SpellImmunityToSpellDescriptor", "BuffDescriptorImmunity" }.Concat(Enumerable.Repeat("AddConditionImmunity", conditions)).ToArray());
	}

	private static Background B(string blueprint, string signature, string mechanic, string text, params Grant[] grants)
	{
		return new Background
		{
			Blueprint = blueprint,
			Signature = signature,
			SignatureMechanic = mechanic,
			SignatureText = text,
			Grants = grants
		};
	}

	internal static double StatWeight(string stat)
	{
		if (stat.StartsWith("Skill", StringComparison.Ordinal))
		{
			return 0.05;
		}
		if (Attributes.Contains(stat))
		{
			return 0.5;
		}
		switch (stat)
		{
		case "AdditionalAttackBonus":
			return 1.0;
		case "AC":
			return 1.0;
		case "SaveFortitude":
		case "SaveReflex":
		case "SaveWill":
			return 1.0 / 3.0;
		case "AdditionalDamage":
			return 0.5;
		case "Initiative":
			return 0.25;
		case "SneakAttack":
			return 0.75;
		case "Speed":
			return 1.0 / 30.0;
		case "AdditionalCMB":
		case "AdditionalCMD":
			return 0.25;
		case "HitPoints":
			return 0.025;
		default:
			throw new ArgumentException("No weight for stat " + stat);
		}
	}

	internal static void Value(Grant grant, out double martial, out double casterOnly)
	{
		martial = 0.0;
		casterOnly = 0.0;
		switch (grant.Kind)
		{
		case GrantKind.Stat:
			martial = (double)grant.Value * StatWeight(grant.Stat);
			break;
		case GrantKind.ClassSkills:
			martial = (double)grant.Value * 0.25;
			break;
		case GrantKind.SkillSwaps:
			martial = (double)grant.Value * 0.25;
			break;
		case GrantKind.SituationalSave:
			martial = (double)grant.Value * 0.15;
			break;
		case GrantKind.SituationalAttack:
			martial = (double)grant.Value * 0.75;
			break;
		case GrantKind.CasterOnly:
			casterOnly = grant.Weight;
			break;
		case GrantKind.Flat:
			martial = grant.Weight;
			break;
		}
	}

	internal static double Score(IEnumerable<Grant> grants)
	{
		double num = 0.0;
		double num2 = 0.0;
		foreach (Grant grant in grants)
		{
			Value(grant, out var martial, out var casterOnly);
			num += martial;
			num2 += casterOnly;
		}
		return num + num2 / 2.0;
	}

	internal static double Score(string blueprint)
	{
		return Score(Backgrounds.Single((Background b) => b.Blueprint == blueprint).Grants);
	}
}
