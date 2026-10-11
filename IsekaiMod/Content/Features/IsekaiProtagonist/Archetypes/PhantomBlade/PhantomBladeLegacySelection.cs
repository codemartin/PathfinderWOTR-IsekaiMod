using System.Collections.Generic;
using HarmonyLib;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.PhantomBlade;

public class PhantomBladeLegacySelection
{
	private static readonly LocalizedString Name = Helpers.CreateString(Main.IsekaiContext, "PhantomBladeLegacySelection.Name", "Phantom Blade Legacy");

	private static readonly LocalizedString Description = Helpers.CreateString(Main.IsekaiContext, "PhantomBladeLegacySelection.Description", "You recall memories of a past life of supreme speed, wielding an ethereal edge that cuts through dimensions.");

	private static BlueprintFeatureSelection ClassFeature;

	private static readonly HashSet<BlueprintFeature> prohibited = new HashSet<BlueprintFeature>();

	public static void Configure()
	{
		if (ClassFeature == null)
		{
			ClassFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "PhantomBladeLegacySelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Name);
				bp.SetDescription(Description);
				bp.m_AllFeatures = new BlueprintFeatureReference[0];
				bp.m_Features = new BlueprintFeatureReference[0];
				bp.IsClassFeature = true;
			});
		}
	}

	public static BlueprintFeatureSelection getClassFeature()
	{
		if (ClassFeature == null)
		{
			Configure();
		}
		return ClassFeature;
	}

	public static void Register(BlueprintProgression prog)
	{
		getClassFeature();
		ClassFeature.m_AllFeatures = ClassFeature.m_AllFeatures.AddToArray(prog.ToReference<BlueprintFeatureReference>());
		ClassFeature.m_Features = ClassFeature.m_Features.AddToArray(prog.ToReference<BlueprintFeatureReference>());
	}

	public static void Prohibit(BlueprintProgression prog)
	{
		if (prog != null)
		{
			prohibited.Add(prog);
		}
	}

	public static void Finish()
	{
		if (!Main.IsekaiContext.AddedContent.Other.IsDisabled("Relax Legacy Choices"))
		{
			return;
		}
		BlueprintArchetypeReference archetypeRef = MartialGodArchetype.GetReference();
		foreach (BlueprintFeature item in prohibited)
		{
			item?.AddComponent(delegate(PrerequisiteNoArchetype c)
			{
				c.m_Archetype = archetypeRef;
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
			});
		}
	}
}
