using IsekaiMod.Components;
using IsekaiMod.Content.Guardians;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist;

internal class TwinAvatarCompanion
{
	private static readonly Sprite Icon_Pet = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

	public static void Add()
	{
		BlueprintFeature AnimalCompanionRank = BlueprintTools.GetBlueprint<BlueprintFeature>("1670990255e4fe948a863bafd5dbda5d");
		BlueprintUnit TwinAvatarCompanionUnit = BlueprintTools.GetModBlueprint<BlueprintUnit>(Main.IsekaiContext, "TwinAvatarCompanionUnit");
		if (TwinAvatarCompanionUnit == null)
		{
			Main.IsekaiContext.Logger.LogWarning("TwinAvatarCompanionUnit not found, skipping TwinAvatarCompanion feature.");
			return;
		}
		Helpers.CreateBlueprint(Main.IsekaiContext, "TwinAvatarCompanion", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "★ Twin Avatar (Special Guardian)");
			bp.SetDescription(Main.IsekaiContext, "[Isekai Protagonist Exclusive Partner]\nCalls forth the Twin Avatar, a humanoid double condensed out of your own otherworldly mana.\nIt wears your own face - the appearance you were called into this world with - but it is not you: it has its own ability scores and advances in the Guardian class as you level. A companion or a pre-made hero has no character-creation appearance to copy, and their twin takes a plain humanoid body instead.\n\n{g|Encyclopedia:Size}Size{/g}: Medium\n{g|Encyclopedia:Speed}Speed{/g}: 30 ft.\n{g|Encyclopedia:Armor_Class}AC{/g}: +3 natural armor\n{g|Encyclopedia:Ability_Scores}Starting ability scores{/g}: Str 16, Dex 14, Con 14, Int 12, Wis 12, Cha 12\nAdvances 1-20 in Guardian Class with its own milestones (Mana-Forged Body, Mirrored Strike, Echo of the Hero, Perfect Reflection).\n\nIf you also take the Twin Summoner Legacy, your legacy picks become evolutions that reshape this twin.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.IsClassFeature = true;
			bp.ReapplyOnLevelUp = true;
			bp.Groups = new FeatureGroup[1] { FeatureGroup.AnimalCompanion };
			GuardianPetLevel.Register(bp);
			bp.AddComponent(delegate(AddPet c)
			{
				c.Type = PetType.AnimalCompanion;
				c.ProgressionType = PetProgressionType.AnimalCompanion;
				c.m_Pet = TwinAvatarCompanionUnit.ToReference<BlueprintUnitReference>();
				if (AnimalCompanionRank != null)
				{
					c.m_LevelRank = AnimalCompanionRank.ToReference<BlueprintFeatureReference>();
				}
			});
			bp.AddComponent(delegate(PrerequisitePet c)
			{
				c.NoCompanion = true;
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = ((AnimalCompanionRank == null) ? new BlueprintUnitFactReference[1] { IsekaiPetProgression.GetCompanionProgression().ToReference<BlueprintUnitFactReference>() } : new BlueprintUnitFactReference[2]
				{
					AnimalCompanionRank.ToReference<BlueprintUnitFactReference>(),
					IsekaiPetProgression.GetCompanionProgression().ToReference<BlueprintUnitFactReference>()
				});
			});
			bp.AddComponent(delegate(UpdateProgressionOnApply c)
			{
				c.m_Feature = IsekaiPetProgression.GetCompanionProgression().ToReference<BlueprintFeatureReference>();
			});
			bp.AddComponent(delegate(CopyMasterAppearanceToPet c)
			{
				c.m_Pet = TwinAvatarCompanionUnit.ToReference<BlueprintUnitReference>();
			});
		});
	}
}
