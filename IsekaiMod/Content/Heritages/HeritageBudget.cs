using System;
using System.Collections.Generic;
using System.Linq;

namespace IsekaiMod.Content.Heritages;

internal static class HeritageBudget
{
	internal enum GrantKind
	{
		Stat,
		ClassSkills,
		SituationalSave,
		SituationalAttack,
		CasterOnly,
		Other
	}

	internal sealed class Grant
	{
		internal GrantKind Kind;

		internal string Stat;

		internal string Descriptor;

		internal int Value;

		internal double[] Weights;

		internal string[] Components = new string[0];

		internal string What;

		public override string ToString()
		{
			return What ?? (Kind.ToString() + " " + Stat + " " + Descriptor + " " + Value);
		}
	}

	internal sealed class Heritage
	{
		internal string Blueprint;

		internal string Signature;

		internal string SignatureText;

		internal Grant[] Grants;
	}

	internal const double BandMin = 6.0;

	internal const double BandMax = 9.0;

	internal static readonly int[] Levels = new int[4] { 1, 5, 10, 20 };

	private const string Spell = "SpellImmunityToSpellDescriptor";

	private const string Buff = "BuffDescriptorImmunity";

	internal static readonly Heritage[] Heritages = new Heritage[20]
	{
		H("IsekaiVampireHeritage", "Blood Drain", "Blood Drain:", S("Dexterity", 2), S("Charisma", 2), S("SkillPerception", 2), S("SkillPersuasion", 2), Flat("DR 5/magic and silver", 0.75, "AddDamageResistancePhysical"), Scaled("fast healing half the character level", L(FastHealing(1), FastHealing(2), FastHealing(5), FastHealing(10)), "AddEffectFastHealing"), Flat("immunity to poison, disease and harmful mind effects", 2.5, "SpellImmunityToSpellDescriptor", "BuffDescriptorImmunity"), Scaled("Vampiric Mist Form, 1/day + 1 per 5 levels, DR 10/- for 2 rounds", L(0.5, 0.5, 0.75, 1.0), "AddAbilityResources", "AddFacts"), Flat("Blood Drain, 3/day touch attack, heals the damage dealt", 0.75, "AddAbilityResources")),
		H("IsekaiDemonLordHeritage", "Abyssal Hellfire", "Abyssal Hellfire:", S("Strength", 4), S("Charisma", 2), S("SkillPerception", 2), S("SkillPersuasion", 2), Flat("DR 10/cold iron or good", 1.5, "AddDamageResistancePhysical"), Flat("fire resistance 10", 0.5, "AddDamageResistanceEnergy"), Flat("poison immunity", 0.75, "SpellImmunityToSpellDescriptor", "BuffDescriptorImmunity"), Flat("Demon Lord's Presence, -2 Will aura", 1.0, "AddFacts"), Flat("fiendish wings, +3 dodge AC against melee", 0.75), Scaled("Abyssal Hellfire, 1/day, 1d6 per 2 levels", L(0.5, 0.75, 1.0, 1.25), "AddAbilityResources")),
		H("IsekaiAngelHeritage", "Angelic Bolt", "Angelic Bolt:", S("Strength", 2), S("Charisma", 4), S("SkillLoreReligion", 2), S("SkillPersuasion", 2), Flat("DR 5/evil", 1.0, "AddDamageResistancePhysical"), Flat("acid and cold resistance 10", 1.0, "AddDamageResistanceEnergy", "AddDamageResistanceEnergy"), Flat("petrification immunity", 0.5, "SpellImmunityToSpellDescriptor", "BuffDescriptorImmunity"), Flat("angel wings, +3 dodge AC against melee", 0.75), Scaled("Angelic Bolt, 1/day, 1 die per 2 levels up to 20", L(0.5, 0.75, 1.0, 1.25), "AddFacts")),
		H("IsekaiDragonLordHeritage", "Dragon Breath", "Dragon Breath:", S("Strength", 4), S("Constitution", 2), S("AC", 2, "NaturalArmor"), Flat("draconic claw 1d6", 0.75, "AddAdditionalLimb"), Flat("fire immunity", 1.5, "AddEnergyImmunity"), Flat("red draconic wings, +3 dodge AC against melee", 0.75), Scaled("Dragon Breath, 3/day, 1d6 + 1d6 per 2 levels", L(0.75, 1.0, 1.25, 1.5), "AddAbilityResources", "AddFacts")),
		H("IsekaiSuccubusHeritage", "Demonic Charm", "Demonic Charm:", S("Strength", -2), S("Dexterity", 2), S("Intelligence", 2), S("Charisma", 4), S("SkillPerception", 2), S("SkillPersuasion", 2), Flat("DR 10/cold iron or good", 1.5, "AddDamageResistancePhysical"), Flat("fire and electricity resistance 10", 1.0, "AddDamageResistanceEnergy", "AddDamageResistanceEnergy"), Flat("poison immunity", 0.75, "SpellImmunityToSpellDescriptor", "BuffDescriptorImmunity"), Flat("fiendish wings, +3 dodge AC against melee", 0.75), Scaled("Demonic Charm, 1/day dominate for level minutes", L(0.75, 1.0, 1.0, 1.25), "AddFacts")),
		H("IsekaiSprigganHeritage", "Size Alteration", "Size Alteration:", S("Strength", 2), S("Constitution", 2), S("Charisma", 2), Skills(4), Caster("+1 DC of all spells", 1.5, "IncreaseAllSpellsDC"), Caster("+1 on concentration checks", 0.25, "ConcentrationBonus"), Scaled("Size Alteration, Large, +4 Str +2 Con, 3 + level rounds/day", L(1.5, 2.0, 2.25, 2.5), "AddAbilityResources", "AddFacts")),
		H("IsekaiCataclysmBerserkerHeritage", "Deathless Ferocity", "Deathless Ferocity:", S("Strength", 4), S("Constitution", 4), S("AdditionalDamage", 2), S("SkillPersuasion", 4), Flat("DR 5/-", 1.5, "AddDamageResistancePhysical"), Flat("+4 critical confirmation", 1.0, "CriticalConfirmationBonus"), Flat("orc ferocity", 0.5, "AddFeatureIfHasFact")),
		H("IsekaiTitanDwarfHeritage", "Earthquake Stomp", "Earthquake Stomp:", S("Constitution", 4), S("Wisdom", 2), S("AC", 2, "NaturalArmor"), S("AdditionalDamage", 2), Flat("DR 5/adamantine", 1.0, "AddDamageResistancePhysical"), Scaled("Earthquake Stomp, 3/day, 1d6 per 2 levels, prone", L(0.75, 1.0, 1.25, 1.5), "AddAbilityResources", "AddFacts")),
		H("IsekaiFateweaverHeritage", "Supernal Luck", "Supernal Luck:", S("Dexterity", 4), S("Charisma", 2), S("AdditionalAttackBonus", 1), S("SaveFortitude", 2), S("SaveReflex", 2), S("SaveWill", 2), S("SneakAttack", 2, "UntypedStackable"), Flat("Shadow Step, 3/day swift teleport", 0.75, "AddAbilityResources", "AddFacts")),
		H("IsekaiWerewolfHeritage", "Howl of Fenrir", "Howl of Fenrir", S("Strength", 4), S("Dexterity", 2), S("Constitution", 2), Flat("bite 1d8", 0.75, "AddAdditionalLimb"), Flat("DR 10/silver", 1.5, "AddDamageResistancePhysical"), Attack("+2 on attack rolls when flanking", 2, "FlankedAttackBonus"), Flat("Howl of Fenrir, 3/day shaken", 0.75, "AddAbilityResources", "AddFacts")),
		H("IsekaiHighElfHeritage", "Arcane Reserves", "Arcane Reserves:", S("Dexterity", 2), S("Intelligence", 2), S("Wisdom", 4), S("SkillKnowledgeArcana", 2), S("SkillUseMagicDevice", 2), Flat("spell resistance 10 + level", 1.5, "AddSpellResistance"), CasterScaled("one extra spell per day of each level 1-9", L(1.0, 1.5, 2.0, 2.5), "AddSpellsPerDay")),
		H("IsekaiDarkElfHeritage", "Drow Poison", "Drow Poison:", S("Dexterity", 2), S("Intelligence", 4), S("Wisdom", 2), Flat("spell resistance 10 + level", 1.5, "AddSpellResistance"), Scaled("Drow Poison, Int/day, unconscious on a failed save", L(1.0, 1.25, 1.5, 1.5), "AddAbilityResources", "AddFacts")),
		H("IsekaiWoodElfHeritage", "Woodland Stride", "Woodland Stride:", S("Dexterity", 4), S("Intelligence", 2), S("Wisdom", 2), S("SkillPerception", 2), S("SkillStealth", 2), S("Speed", 10), Flat("spell resistance 10 + level", 1.5, "AddSpellResistance"), Flat("immunity to difficult terrain", 0.5, "AddConditionImmunity")),
		H("IsekaiKitsuneHeritage", "Divine Vigor", "Divine Vigor:", S("Dexterity", 4), S("Intelligence", 2), S("Charisma", 2), S("SaveWill", 2), S("Initiative", 2), S("Speed", 10), Scaled("fast healing equal to the character level", L(FastHealing(1), FastHealing(5), FastHealing(10), FastHealing(20)), "AddEffectFastHealing"), Flat("the kitsune spell-like ability (sweep 03 #22)", 0.5, "AddFacts")),
		H("IsekaiHighHumanHeritage", "Paragon of Humanity", "+2 racial bonus to all attributes", S("Strength", 2), S("Dexterity", 2), S("Constitution", 2), S("Intelligence", 2), S("Wisdom", 2), S("Charisma", 2)),
		H("IsekaiImmortalHumanHeritage", "Undying", "Undying:", S("Constitution", 4), S("Charisma", 2), S("SaveFortitude", 2), Flat("fast healing 4", FastHealing(4), "AddEffectFastHealing"), Save("+4 against death effects", 4, "SavingThrowBonusAgainstDescriptor"), Flat("bleed and disease immunity", 1.0, "SpellImmunityToSpellDescriptor", "BuffDescriptorImmunity")),
		H("HeteromorphicOverlordHeritage", "Deathless Sovereign", "Deathless Sovereign:", S("Intelligence", 4), S("Charisma", 2), S("SkillKnowledgeArcana", 2), S("SkillLoreReligion", 2), Caster("+1 caster level and DC of necromancy spells", 1.5, "IncreaseSpellSchoolCasterLevel", "IncreaseSpellSchoolDC"), Caster("+2 spell penetration", 0.5, "SpellPenetrationBonus"), Flat("fear immunity", 1.5, "SpellImmunityToSpellDescriptor", "BuffDescriptorImmunity", "AddConditionImmunity", "AddConditionImmunity"), Flat("negative energy resistance 10", 0.5, "AddDamageResistanceEnergy")),
		H("SlimeReincarnateHeritage", "Amorphous Body", "Amorphous Body:", S("Constitution", 4), S("Dexterity", 2), S("SkillKnowledgeWorld", 2), S("SkillPerception", 2), S("Initiative", 2), S("AdditionalCMB", 2), Save("+4 against poison, acid and paralysis", 4, "SavingThrowBonusAgainstDescriptor"), Flat("acid immunity", 1.5, "AddEnergyImmunity")),
		H("GnomeOfTheFirstWorldHeritage", "First World Mind", "immunity to curses and confusion", S("Charisma", 4), S("Dexterity", 2), S("Constitution", 2), S("Strength", -2), S("SaveFortitude", 2), S("SaveReflex", 2), S("SaveWill", 2), Caster("+1 DC of enchantment and illusion spells", 1.5, "IncreaseSpellSchoolDC", "IncreaseSpellSchoolDC"), Flat("curse and confusion immunity", 1.25, "SpellImmunityToSpellDescriptor", "BuffDescriptorImmunity", "AddConditionImmunity")),
		H("IsekaiHumanCrossbreedLegacy", "Mixed Lineage", "Mixed Lineage:", S("Dexterity", 2), S("Constitution", 2), S("Wisdom", 2), S("SaveFortitude", 1), S("SaveReflex", 1), S("SaveWill", 1), S("SkillPerception", 2), Flat("sleep immunity", 1.0, "SpellImmunityToSpellDescriptor", "BuffDescriptorImmunity", "AddConditionImmunity"), Save("+2 against poison", 2, "SavingThrowBonusAgainstDescriptor"), Flat("orc ferocity", 0.5, "AddFeatureIfHasFact"), Flat("counts as eleven races for racial feat prerequisites", 0.5))
	};

	internal static readonly string[] Attributes = new string[6] { "Strength", "Dexterity", "Constitution", "Intelligence", "Wisdom", "Charisma" };

	private static Grant S(string stat, int value, string descriptor = "Racial")
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
			Value = count,
			Components = Enumerable.Repeat("AddClassSkill", count).ToArray()
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
		return CasterScaled(what, new double[4] { weight, weight, weight, weight }, components);
	}

	private static Grant CasterScaled(string what, double[] weights, params string[] components)
	{
		return new Grant
		{
			Kind = GrantKind.CasterOnly,
			What = what,
			Weights = weights,
			Components = components
		};
	}

	private static Grant Flat(string what, double weight, params string[] components)
	{
		return Scaled(what, new double[4] { weight, weight, weight, weight }, components);
	}

	private static Grant Scaled(string what, double[] weights, params string[] components)
	{
		return new Grant
		{
			Kind = GrantKind.Other,
			What = what,
			Weights = weights,
			Components = components
		};
	}

	private static double[] L(double l1, double l5, double l10, double l20)
	{
		return new double[4] { l1, l5, l10, l20 };
	}

	internal static double FastHealing(int amount)
	{
		return 0.5 + 0.1 * (double)amount;
	}

	private static Heritage H(string blueprint, string signature, string text, params Grant[] grants)
	{
		return new Heritage
		{
			Blueprint = blueprint,
			Signature = signature,
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
			return 0.5;
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
		default:
			throw new ArgumentException("No weight for stat " + stat);
		}
	}

	internal static void Value(Grant grant, int levelIndex, out double martial, out double casterOnly)
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
		case GrantKind.SituationalSave:
			martial = (double)grant.Value * 0.15;
			break;
		case GrantKind.SituationalAttack:
			martial = (double)grant.Value * 0.75;
			break;
		case GrantKind.CasterOnly:
			casterOnly = grant.Weights[levelIndex];
			break;
		case GrantKind.Other:
			martial = grant.Weights[levelIndex];
			break;
		}
	}

	internal static double Score(IEnumerable<Grant> grants, int level)
	{
		int num = Array.IndexOf(Levels, level);
		if (num < 0)
		{
			throw new ArgumentException("Heritages are scored at levels " + string.Join(", ", Levels) + ", not " + level);
		}
		double num2 = 0.0;
		double num3 = 0.0;
		foreach (Grant grant in grants)
		{
			Value(grant, num, out var martial, out var casterOnly);
			num2 += martial;
			num3 += casterOnly;
		}
		return num2 + num3 / 2.0;
	}

	internal static double Score(string blueprint, int level)
	{
		return Score(Heritages.Single((Heritage h) => h.Blueprint == blueprint).Grants, level);
	}
}
