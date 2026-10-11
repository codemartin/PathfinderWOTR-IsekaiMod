using IsekaiMod.Components;
using IsekaiMod.Content.Heritages;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist;

internal static class ArchetypeHeritageTraits
{
	internal const string OverlordCarrierName = "OverlordArchetypeHeritageFeature";

	internal const string OverlordTraitsName = "OverlordHeteromorphicTraitsFeature";

	internal const string OverlordHeritageName = "HeteromorphicOverlordHeritage";

	internal const string DevourerCarrierName = "DevourerArchetypeHeritageFeature";

	internal const string DevourerTraitsName = "DevourerSlimeTraitsFeature";

	internal const string DevourerHeritageName = "SlimeReincarnateHeritage";

	internal static string[][] BlueprintNames()
	{
		return new string[2][]
		{
			new string[3] { "OverlordArchetypeHeritageFeature", "OverlordHeteromorphicTraitsFeature", "HeteromorphicOverlordHeritage" },
			new string[3] { "DevourerArchetypeHeritageFeature", "DevourerSlimeTraitsFeature", "SlimeReincarnateHeritage" }
		};
	}

	public static void Add()
	{
		CreateOverlordTraits();
		CreateDevourerTraits();
	}

	public static BlueprintFeature GetOverlordCarrier()
	{
		return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "OverlordArchetypeHeritageFeature");
	}

	public static BlueprintFeature GetDevourerCarrier()
	{
		return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DevourerArchetypeHeritageFeature");
	}

	private static void CreateOverlordTraits()
	{
		BlueprintFeature heritage = HeteromorphicOverlordHeritage.Get();
		BlueprintFeature traits = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordHeteromorphicTraitsFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Heteromorphic Overlord Physiology");
			bp.SetDescription(Main.IsekaiContext, "Even without an undead sovereign's heritage, your Overlord training reshapes the shell you wear. Grants a +2 bonus to Intelligence and Charisma, a +1 bonus to Caster Level and DC of Necromancy spells, a +2 bonus on Spell Penetration checks, and a +2 bonus on Knowledge (Arcana) and Lore (Religion) checks.");
			((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)heritage)?.m_Icon;
			bp.IsClassFeature = true;
			bp.ReapplyOnLevelUp = true;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Intelligence;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Charisma;
				c.Value = 2;
			});
			bp.AddComponent(delegate(IncreaseSpellSchoolCasterLevel c)
			{
				c.School = SpellSchool.Necromancy;
				c.BonusLevel = 1;
				c.Descriptor = ModifierDescriptor.Racial;
			});
			bp.AddComponent(delegate(IncreaseSpellSchoolDC c)
			{
				c.School = SpellSchool.Necromancy;
				c.BonusDC = 1;
				c.Descriptor = ModifierDescriptor.Racial;
			});
			bp.AddComponent(delegate(SpellPenetrationBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.SkillKnowledgeArcana;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.SkillLoreReligion;
				c.Value = 2;
			});
		});
		CreateCarrier("OverlordArchetypeHeritageFeature", "Overlord Physiology", traits, heritage, "Your Overlord nature reshapes your body. If you did not take the Heteromorphic Overlord heritage at character creation, you gain its physical traits here instead; if you did, it keeps working and nothing is added twice.");
	}

	private static void CreateDevourerTraits()
	{
		BlueprintFeature heritage = SlimeReincarnateHeritage.Get();
		BlueprintFeature traits = Helpers.CreateBlueprint(Main.IsekaiContext, "DevourerSlimeTraitsFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Amorphous Physiology");
			bp.SetDescription(Main.IsekaiContext, "Even without a slime's heritage, your predatory biology turns fluid and adaptive. Grants a +2 bonus to Constitution and Dexterity, a +2 bonus on Knowledge (World), Perception and Initiative checks, a +2 bonus on saving throws against poison, acid and paralysis effects, and a +1 bonus on Combat Maneuver Checks.");
			((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)heritage)?.m_Icon;
			bp.IsClassFeature = true;
			bp.ReapplyOnLevelUp = true;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Constitution;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Dexterity;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.SkillKnowledgeWorld;
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
				c.Stat = StatType.Initiative;
				c.Value = 2;
			});
			bp.AddComponent(delegate(SavingThrowBonusAgainstDescriptor c)
			{
				c.SpellDescriptor = SpellDescriptor.Acid | SpellDescriptor.Poison | SpellDescriptor.Paralysis;
				c.ModifierDescriptor = ModifierDescriptor.Racial;
				c.Value = 2;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.AdditionalCMB;
				c.Value = 1;
			});
		});
		CreateCarrier("DevourerArchetypeHeritageFeature", "Slime Physiology", traits, heritage, "Your devourer nature turns your body amorphous. If you did not take the Slime Reincarnate heritage at character creation, you gain its physical traits here instead; if you did, it keeps working and nothing is added twice.");
	}

	private static void CreateCarrier(string name, string displayName, BlueprintFeature traits, BlueprintFeature heritage, string description)
	{
		Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, displayName);
			bp.SetDescription(Main.IsekaiContext, description);
			((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)traits)?.m_Icon;
			bp.IsClassFeature = true;
			bp.ReapplyOnLevelUp = true;
			if (traits == null || heritage == null)
			{
				Main.IsekaiContext.Logger.LogError("[ArchetypeHeritageTraits] '" + name + "' has no traits copy or no heritage to check against; the archetype grants nothing here.");
			}
			else
			{
				bp.AddComponent(delegate(AddFeatureUnlessHasFact c)
				{
					c.m_CheckedFact = heritage.ToReference<BlueprintUnitFactReference>();
					c.m_Feature = traits.ToReference<BlueprintUnitFactReference>();
				});
			}
		});
	}
}
