using System;
using System.Collections.Generic;
using System.Linq;

namespace IsekaiMod.Content.Constellations;

internal static class DeityAvatarBudget
{
	internal enum GrantKind
	{
		Stat,
		AllyAuraStat,
		AbilityDC,
		SpellPenetration,
		FastHealing,
		DamageReduction,
		Flat,
		Carrier
	}

	internal sealed class Grant
	{
		internal GrantKind Kind;

		internal string Stat;

		internal string Descriptor;

		internal int Value;

		internal bool Stackable;

		internal double Weight;

		internal string[] Components = new string[0];

		internal string What;

		public override string ToString()
		{
			return What ?? (Kind.ToString() + " " + Stat + " " + Descriptor + " " + Value);
		}
	}

	internal const double BudgetMin = 12.5;

	internal const double BudgetMax = 16.5;

	internal static readonly int[] ScoredLevels = new int[4] { 5, 10, 20, 30 };

	internal static readonly IReadOnlyDictionary<string, Grant[]> Avatars = new Dictionary<string, Grant[]>(StringComparer.Ordinal)
	{
		["AvatarCaydenFeature"] = Of(S("AdditionalAttackBonus", "Sacred", 4), AllSaves("Sacred", 4), S("AC", "Sacred", 4), FastHealing(5), Flat("nausea immunity", 0.5, "AddConditionImmunity"), Immune("fear and compulsion immunity", 1.5)),
		["AvatarIomedaeFeature"] = Of(S("AC", "Sacred", 4), S("AdditionalAttackBonus", "Sacred", 4), AllSaves("Sacred", 4), S("AdditionalDamage", "Sacred", 6)),
		["AvatarAsmodeusFeature"] = Of(S("AC", "Profane", 4), S("AdditionalAttackBonus", "Profane", 4), new Grant
		{
			Kind = GrantKind.AbilityDC,
			Descriptor = "Profane",
			Value = 4,
			Components = new string[1] { "IncreaseAllSpellsDC" }
		}, Flat("fire immunity", 2.0, "AddEnergyImmunity"), Immune("harmful mind-affecting immunity", 1.5)),
		["AvatarDesnaFeature"] = Of(S("Speed", "Sacred", 30), S("AC", "Sacred", 4), AllSaves("Sacred", 4), Flat("reroll a failing saving throw", 2.5, "ModifyD20"), Flat("paralysis immunity", 0.5, "AddConditionImmunity"), Flat("entanglement immunity", 0.5, "AddConditionImmunity"), Immune("curse, movement-impairing and sleep immunity", 1.5)),
		["AvatarPharasmaFeature"] = Of(Immune("death immunity", 1.5), Flat("energy drain immunity", 1.5, "AddImmunityToEnergyDrain"), Flat("negative energy immunity", 1.5, "AddEnergyImmunity"), AllSaves("UntypedStackable", 4), S("AdditionalAttackBonus", "UntypedStackable", 2), S("AdditionalDamage", "UntypedStackable", 6)),
		["AvatarCalistriaFeature"] = Of(S("Initiative", "Insight", 4), S("SneakAttack", "Insight", 4), S("AdditionalAttackBonus", "Insight", 4), S("AC", "Insight", 4), S("SaveReflex", "Insight", 4), Immune("poison immunity", 1.0)),
		["AvatarNethysFeature"] = Of(new Grant
		{
			Kind = GrantKind.AbilityDC,
			Descriptor = "UntypedStackable",
			Value = 3,
			Components = new string[1] { "IncreaseAllSpellsDC" }
		}, new Grant
		{
			Kind = GrantKind.SpellPenetration,
			Descriptor = "UntypedStackable",
			Value = 4,
			Components = new string[1] { "SpellPenetrationBonus" }
		}, AllSaves("UntypedStackable", 4), S("AdditionalAttackBonus", "UntypedStackable", 2), S("AdditionalDamage", "UntypedStackable", 4), Flat("resist 10 acid, cold, electricity, fire and sonic", 2.5, "AddDamageResistanceEnergy", "AddDamageResistanceEnergy", "AddDamageResistanceEnergy", "AddDamageResistanceEnergy", "AddDamageResistanceEnergy"), Flat("see invisibility", 1.0, "AddCondition")),
		["AvatarGorumFeature"] = Of(S("Strength", "UntypedStackable", 6), S("Constitution", "UntypedStackable", 4), S("AdditionalDamage", "UntypedStackable", 4), Flat("extra attack on a full attack", 3.0, "BuffExtraAttack"), new Grant
		{
			Kind = GrantKind.DamageReduction,
			Value = 10,
			Stackable = true,
			Components = new string[1] { "AddDamageResistancePhysical" }
		}, Flat("fatigue immunity", 0.25, "AddConditionImmunity"), Flat("exhaustion immunity", 0.25, "AddConditionImmunity")),
		["AvatarBesmaraFeature"] = Of(S("AC", "UntypedStackable", 4), S("AdditionalAttackBonus", "Luck", 4), AllSaves("UntypedStackable", 4), S("AdditionalCMB", "UntypedStackable", 4), S("AdditionalCMD", "UntypedStackable", 4), FastHealing(5)),
		["AvatarLanternKingFeature"] = Of(S("AC", "Dodge", 6), S("Dexterity", "UntypedStackable", 6), S("SaveWill", "UntypedStackable", 4), Flat("paralysis immunity", 0.5, "AddConditionImmunity"), Flat("entanglement immunity", 0.5, "AddConditionImmunity"), Immune("confusion immunity", 0.5)),
		["AvatarChaldiraFeature"] = Of(new Grant
		{
			Kind = GrantKind.Carrier,
			What = "ally aura",
			Components = new string[1] { "AuraFeatureComponent" }
		}, from g in AllSaves("Luck", 4)
			select Aura(g.Stat, "Luck", 4), Aura("AdditionalAttackBonus", "Luck", 3), Aura("AC", "Luck", 3), S("AC", "UntypedStackable", 2), AllSaves("UntypedStackable", 2)),
		["AvatarMilaniFeature"] = Of(S("AC", "Sacred", 4), AllSaves("Sacred", 4), S("AdditionalAttackBonus", "UntypedStackable", 2), S("AdditionalDamage", "UntypedStackable", 4), FastHealing(5), Immune("disease and bleed immunity", 1.0)),
		["AvatarBlackButterflyFeature"] = Of(Flat("50% miss chance against attacks", 4.0, "AddConcealment"), S("AC", "Dodge", 4), S("SaveWill", "UntypedStackable", 4), S("Initiative", "UntypedStackable", 4), S("AdditionalDamage", "UntypedStackable", 4), Flat("cold immunity", 1.0, "AddEnergyImmunity"), Flat("sonic immunity", 1.0, "AddEnergyImmunity")),
		["GodDefierFeature"] = Of(S("Strength", "UntypedStackable", 4), S("Dexterity", "UntypedStackable", 4), S("Constitution", "UntypedStackable", 4), S("Intelligence", "UntypedStackable", 4), S("Wisdom", "UntypedStackable", 4), S("Charisma", "UntypedStackable", 4), Immune("curse and death immunity", 1.5), S("AdditionalDamage", "UntypedStackable", 8))
	};

	internal static readonly IReadOnlyDictionary<string, string> AuraEffectBuffs = new Dictionary<string, string>(StringComparer.Ordinal) { ["AvatarChaldiraFeature"] = "AvatarChaldiraAuraEffectBuff" };

	private static Grant S(string stat, string descriptor, int value)
	{
		Grant grant = new Grant();
		grant.Kind = GrantKind.Stat;
		grant.Stat = stat;
		grant.Descriptor = descriptor;
		grant.Value = value;
		grant.Components = new string[1] { "AddStatBonus" };
		return grant;
	}

	private static Grant Aura(string stat, string descriptor, int value)
	{
		return new Grant
		{
			Kind = GrantKind.AllyAuraStat,
			Stat = stat,
			Descriptor = descriptor,
			Value = value
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

	private static Grant Immune(string what, double weight)
	{
		return Flat(what, weight, "SpellImmunityToSpellDescriptor", "BuffDescriptorImmunity");
	}

	private static Grant FastHealing(int value)
	{
		Grant grant = new Grant();
		grant.Kind = GrantKind.FastHealing;
		grant.Value = value;
		grant.Components = new string[1] { "AddEffectFastHealing" };
		return grant;
	}

	private static IEnumerable<Grant> AllSaves(string descriptor, int value)
	{
		return new Grant[3]
		{
			S("SaveFortitude", descriptor, value),
			S("SaveReflex", descriptor, value),
			S("SaveWill", descriptor, value)
		};
	}

	private static Grant[] Of(params object[] parts)
	{
		List<Grant> list = new List<Grant>();
		foreach (object obj in parts)
		{
			if (obj is Grant item)
			{
				list.Add(item);
			}
			else
			{
				list.AddRange((IEnumerable<Grant>)obj);
			}
		}
		return list.ToArray();
	}

	internal static int PlotArmorLuck(int level)
	{
		return Math.Max(1, level / 2);
	}

	internal static int BodyStrengtheningDR(int level)
	{
		if (level < 11)
		{
			return 0;
		}
		return level;
	}

	private static bool IsPlotArmorStat(string stat)
	{
		switch (stat)
		{
		default:
			return stat == "SaveWill";
		case "AC":
		case "SaveFortitude":
		case "SaveReflex":
			return true;
		}
	}

	internal static int EffectiveValue(Grant grant, int level)
	{
		switch (grant.Kind)
		{
		case GrantKind.Stat:
			if (grant.Descriptor == "Luck" && IsPlotArmorStat(grant.Stat))
			{
				return Math.Max(0, grant.Value - PlotArmorLuck(level));
			}
			return grant.Value;
		case GrantKind.DamageReduction:
			if (!grant.Stackable)
			{
				return Math.Max(0, grant.Value - BodyStrengtheningDR(level));
			}
			return grant.Value;
		case GrantKind.Carrier:
			return 0;
		case GrantKind.Flat:
			return 1;
		default:
			return grant.Value;
		}
	}

	internal static double StatWeight(string stat, int level)
	{
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
		case "Strength":
			return 0.75;
		case "Dexterity":
			return 19.0 / 24.0;
		case "Constitution":
			return (1.0 / 3.0 + (double)level * 0.025) / 2.0;
		case "Wisdom":
			return 1.0 / 6.0;
		case "Charisma":
		case "Intelligence":
			return 0.0;
		default:
			throw new ArgumentException("No weight for stat " + stat);
		}
	}

	internal static void Value(Grant grant, int level, out double martial, out double casterOnly)
	{
		martial = 0.0;
		casterOnly = 0.0;
		int num = EffectiveValue(grant, level);
		switch (grant.Kind)
		{
		case GrantKind.Stat:
		case GrantKind.AllyAuraStat:
			martial = (double)num * StatWeight(grant.Stat, level);
			break;
		case GrantKind.AbilityDC:
			casterOnly = (double)num * 1.5;
			break;
		case GrantKind.SpellPenetration:
			casterOnly = (double)num * 0.25;
			break;
		case GrantKind.FastHealing:
			martial = (double)num * 0.2;
			break;
		case GrantKind.DamageReduction:
			martial = (double)num * 0.2;
			break;
		case GrantKind.Flat:
			martial = grant.Weight;
			break;
		}
	}

	internal static double Score(IEnumerable<Grant> grants, int level)
	{
		double num = 0.0;
		double num2 = 0.0;
		int num3 = 0;
		foreach (Grant grant in grants)
		{
			Value(grant, level, out var martial, out var casterOnly);
			num += martial;
			num2 += casterOnly;
			if (grant.Kind == GrantKind.Stat && (grant.Stat == "Intelligence" || grant.Stat == "Wisdom" || grant.Stat == "Charisma"))
			{
				num3 = Math.Max(num3, grant.Value);
			}
		}
		num2 += (double)(num3 / 2) * 1.5;
		return num + num2 / 2.0;
	}

	internal static double Score(string avatar, int level)
	{
		return Score(Avatars[avatar], level);
	}
}
