using IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages
{
	internal class HeteromorphicOverlordHeritage
	{
		public static void Add()
		{
			Sprite Icon_Overlord = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("bd9a5db280284025ab01474b0a4cd434"))?.m_Icon ?? ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("4e83f1e0e52b4613982b14ee2796928f"))?.m_Icon;
			BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "HeteromorphicOverlordHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Heteromorphic Overlord Heritage");
				bp.SetDescription(Main.IsekaiContext, "Reincarnated into Golarion as an ancient heteromorphic undead sovereign, your physical shell is merely an external projection concealing an immortal, supreme skeletal monarch. You possess unnatural arcane mastery, overwhelming presence, and innate dominance over death and undeath.\nGrants a +2 racial bonus to Intelligence and Charisma, a +1 bonus to Caster Level and DC of Necromancy spells, a +2 racial bonus on Spell Penetration checks, and a +2 racial bonus on Knowledge (Arcana) and Lore (Religion) checks.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Overlord;
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
				BlueprintFeature overlordForm = SkeletalOverlordForm.Get();
				if (overlordForm != null)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { overlordForm.ToReference<BlueprintUnitFactReference>() };
					});
				}
				bp.Groups = new FeatureGroup[1] { FeatureGroup.Racial };
				bp.ReapplyOnLevelUp = true;
			});
			FeatTools.Selections.DhampirHeritageSelection.AddToSelection(feature);
			HumanHeritageSelection.Register(feature);
		}

		public static BlueprintFeature Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeteromorphicOverlordHeritage");
		}

		public static BlueprintFeatureReference GetReference()
		{
			return Get().ToReference<BlueprintFeatureReference>();
		}
	}
}
