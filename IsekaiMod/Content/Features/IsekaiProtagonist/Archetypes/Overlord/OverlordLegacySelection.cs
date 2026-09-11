using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord
{
	internal class OverlordLegacySelection
	{
		private static BlueprintFeatureSelection ClassSelection;

		private static BlueprintProgression[] registered;

		private static BlueprintProgression[] prohibited;

		public static void Configure()
		{
			if (ClassSelection != null)
			{
				Main.IsekaiContext.Logger.LogWarning("repeated configuration of =OverlordLegacySelection");
				return;
			}
			ClassSelection = Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordLegacySelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Evil Legacy");
				bp.SetDescription(Main.IsekaiContext, "You have a dark and secret past.");
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.IgnorePrerequisites = false;
				bp.m_AllFeatures = new BlueprintFeatureReference[0];
				bp.m_Features = new BlueprintFeatureReference[0];
			});
			registered = new BlueprintProgression[0];
			prohibited = new BlueprintProgression[0];
		}

		public static BlueprintFeatureSelection getClassFeature()
		{
			if (ClassSelection != null)
			{
				return ClassSelection;
			}
			return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "OverlordLegacySelection");
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
			BlueprintArchetypeReference archetypeRef = OverlordArchetype.GetReference();
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
