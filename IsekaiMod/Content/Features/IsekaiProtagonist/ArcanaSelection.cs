using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.InheritedClassFeature;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal class ArcanaSelection
	{
		public static void Configure()
		{
			BlueprintSpellListReference spellListRef = IsekaiProtagonistSpellList.Get()?.ToReference<BlueprintSpellListReference>();
			BlueprintCharacterClassReference classRef = IsekaiProtagonistClass.GetReference();
			BlueprintParametrizedFeature blueprintParametrizedFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiProtagonistArcana", delegate(BlueprintParametrizedFeature bp)
			{
				((BlueprintUnitFact)bp).m_DisplayName = ((BlueprintUnitFact)StaticReferences.BloodlineArcaneNewArcanaFeature)?.m_DisplayName;
				((BlueprintUnitFact)bp).m_Description = ((BlueprintUnitFact)StaticReferences.BloodlineArcaneNewArcanaFeature)?.m_Description;
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)StaticReferences.BloodlineArcaneNewArcanaFeature)?.m_Icon;
				bp.Ranks = 1;
				bp.ReapplyOnLevelUp = false;
				bp.IsClassFeature = true;
				bp.ParameterType = FeatureParameterType.LearnSpell;
				bp.m_SpellcasterClass = classRef;
				bp.m_SpellList = spellListRef;
				bp.SpecificSpellLevel = false;
				bp.SpellLevelPenalty = 0;
				bp.SpellLevel = 0;
				bp.DisallowSpellsInSpellList = false;
				bp.AddComponent(delegate(LearnSpellParametrized c)
				{
					c.m_SpellcasterClass = classRef;
					c.m_SpellList = spellListRef;
					c.SpecificSpellLevel = false;
					c.SpellLevelPenalty = 0;
					c.SpellLevel = 0;
				});
			});
			if (blueprintParametrizedFeature != null)
			{
				ExtraBloodlineSelection.Get()?.AddFeatures(blueprintParametrizedFeature);
				ExtraOracleSelection.Get()?.AddFeatures(blueprintParametrizedFeature);
				ShamanSelection.Get()?.AddFeatures(blueprintParametrizedFeature);
				WitchPatronSelection.Get()?.AddFeatures(blueprintParametrizedFeature);
				FeatTools.Selections.BloodlineArcaneNewArcanaSelection?.AddFeatures(blueprintParametrizedFeature);
			}
		}

		public static BlueprintParametrizedFeature get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintParametrizedFeature>(Main.IsekaiContext, "IsekaiProtagonistArcana");
		}

		public static BlueprintFeatureReference getReference()
		{
			return BlueprintTools.GetModBlueprint<BlueprintParametrizedFeature>(Main.IsekaiContext, "IsekaiProtagonistArcana")?.ToReference<BlueprintFeatureReference>();
		}
	}
}
