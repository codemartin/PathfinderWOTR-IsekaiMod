using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Heritages;

internal class IsekaiHumanCrossbreedLegacy
{
	private static BlueprintFeature ourHeritage;

	internal static readonly string[] RaceGuids = new string[11]
	{
		"b7f02ba92b363064fb873963bec275ee", "64e8b7d5f1ae91d45bbf1e56a3fdff01", "c4faf439f0e70bd40b5e36ee80d06be7", "25a5878d125338244896ebd3238226c8", "ef35a22c9a27da345a4528f0d5889157", "1dc20e195581a804890ddc74218bfd8e", "fd188bb7bb0002e49863aec93bfb9d99", "4d4555326b9b7144f93be1ea61337cd7", "5c4e42124dc2b4647af6e36cf2590500", "b0c3ef2729c498f47970bb50fa1acd30",
		"b3646842ffbd01643ab4dac7479b20b0"
	};

	public static void Add()
	{
		BlueprintFeature OrcFerocity = BlueprintTools.GetBlueprint<BlueprintFeature>("c99f3405d1ef79049bd90678a666e1d7");
		ourHeritage = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiHumanCrossbreedLegacy", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Isekai Crossbreed Human");
			bp.SetDescription(Main.IsekaiContext, "Humanity's remarkable adaptability has long intertwined with planar entities, fey, and ancient bloodlines. \nIn you, dormant ancestral lineages have awakened in vibrant harmony.\nThe Isekai Crossbreed Human has a +2 racial bonus to Dexterity, Constitution and Wisdom, a +1 racial bonus on all saving throws, and a +2 racial bonus on Perception checks.\nMixed Lineage: they are immune to sleep, have a +2 racial bonus on saving throws against poison, and have orc ferocity. They also count as an aasimar, dhampir, dwarf, elf, gnome, half-elf, half-orc, halfling, kitsune, oread and tiefling for the prerequisites of racial feats.");
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Dexterity;
				c.Value = 2;
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
				c.Stat = StatType.Wisdom;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.SaveFortitude;
				c.Value = 1;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.SaveReflex;
				c.Value = 1;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.SaveWill;
				c.Value = 1;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.SkillPerception;
				c.Value = 2;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Sleep;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Sleep;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Sleeping;
			});
			bp.AddComponent(delegate(SavingThrowBonusAgainstDescriptor c)
			{
				c.SpellDescriptor = SpellDescriptor.Poison;
				c.ModifierDescriptor = ModifierDescriptor.Racial;
				c.Value = 2;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(AddFeatureIfHasFact c)
			{
				c.m_CheckedFact = OrcFerocity.ToReference<BlueprintUnitFactReference>();
				c.m_Feature = OrcFerocity.ToReference<BlueprintUnitFactReference>();
				c.Not = true;
			});
			bp.Groups = new FeatureGroup[1] { FeatureGroup.Racial };
			bp.ReapplyOnLevelUp = true;
		});
		HumanHeritageSelection.Register(ourHeritage);
	}

	internal static bool ShouldOfferCrossbreed(bool raceIsAll, bool hasOtherAnyGroup, bool needsRacialTrait)
	{
		if (needsRacialTrait)
		{
			return false;
		}
		return !(raceIsAll & hasOtherAnyGroup);
	}

	public static void Patch()
	{
		List<BlueprintFeatureReference> races = RaceGuids.Select((string g) => BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(g)).ToList();
		if (ourHeritage == null)
		{
			return;
		}
		BlueprintFeatureReference[] allFeatures = FeatTools.Selections.BasicFeatSelection.m_AllFeatures;
		foreach (BlueprintFeatureReference blueprintFeatureReference in allFeatures)
		{
			if (blueprintFeatureReference == null)
			{
				continue;
			}
			BlueprintFeature blueprintFeature = blueprintFeatureReference.Get();
			if (blueprintFeature == null || ((BlueprintScriptableObject)blueprintFeature).Components == null)
			{
				continue;
			}
			List<PrerequisiteFeature> racePrereqs = (from p in ((BlueprintScriptableObject)blueprintFeature).Components.OfType<PrerequisiteFeature>()
				where p.m_Feature != null && races.Contains(p.m_Feature)
				select p).ToList();
			if (racePrereqs.Count == 0)
			{
				continue;
			}
			bool hasOtherAnyGroup = ((BlueprintScriptableObject)blueprintFeature).Components.OfType<Prerequisite>().Any((Prerequisite p) => p.Group == Prerequisite.GroupType.Any && !racePrereqs.Contains(p));
			bool raceIsAll = racePrereqs.Any((PrerequisiteFeature p) => p.Group == Prerequisite.GroupType.All);
			bool needsRacialTrait = ((BlueprintScriptableObject)blueprintFeature).Components.OfType<PrerequisiteFeature>().Any((PrerequisiteFeature p) => p.Group == Prerequisite.GroupType.All && !racePrereqs.Contains(p) && p.m_Feature != null && p.m_Feature.Get()?.Groups?.Contains(FeatureGroup.Racial) == true);
			if (!ShouldOfferCrossbreed(raceIsAll, hasOtherAnyGroup, needsRacialTrait))
			{
				continue;
			}
			foreach (PrerequisiteFeature item in racePrereqs)
			{
				item.Group = Prerequisite.GroupType.Any;
			}
			blueprintFeature.AddComponent(delegate(PrerequisiteFeature c)
			{
				c.Group = Prerequisite.GroupType.Any;
				c.CheckInProgression = false;
				c.HideInUI = false;
				c.m_Feature = ourHeritage.ToReference<BlueprintFeatureReference>();
			});
		}
	}
}
