using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero
{
	internal class HeroLegacySelection
	{
		private static BlueprintFeatureSelection ClassSelection;

		private static BlueprintProgression[] registered = new BlueprintProgression[0];

		private static BlueprintProgression[] prohibited = new BlueprintProgression[0];

		public static void Configure()
		{
			if (ClassSelection != null)
			{
				Main.IsekaiContext.Logger.LogWarning("repeated configuration of =HeroLegacy");
				return;
			}
			ClassSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "HeroLegacySelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Heroic Legacy");
				bp.SetDescription(Main.IsekaiContext, "You are a shining beacon of Light. The beacon that shows the way through the darkness and your legacy reflects that.");
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.IgnorePrerequisites = false;
				bp.m_AllFeatures = new BlueprintFeatureReference[0];
				bp.m_Features = new BlueprintFeatureReference[0];
			});
		}

		public static BlueprintFeatureSelection getClassFeature()
		{
			if (ClassSelection != null)
			{
				return ClassSelection;
			}
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "HeroLegacySelection");
		}

		public static void Register(BlueprintProgression prog)
		{
			registered = registered.AppendToArray(prog);
		}

		public static void Prohibit(BlueprintProgression prog)
		{
			prohibited = prohibited.AppendToArray(prog);
		}

		public static void Finish()
		{
			BlueprintProgression[] array = registered;
			foreach (BlueprintFeature blueprintFeature in array)
			{
				ClassSelection.AddFeatures(blueprintFeature);
			}
			if (!Main.IsekaiContext.AddedContent.Other.IsDisabled("Relax Legacy Choices"))
			{
				return;
			}
			BlueprintArchetypeReference archetypeRef = HeroArchetype.GetReference();
			array = prohibited;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].AddComponent(delegate(PrerequisiteNoArchetype c)
				{
					c.m_Archetype = archetypeRef;
					c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				});
			}
		}
	}
}
