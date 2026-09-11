using IsekaiMod.Content.Classes.IsekaiProtagonist;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal class IsekaiCantrips
	{
		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiCantrips", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Cantrips");
				bp.SetDescription(Main.IsekaiContext, "You can cast a number of {g|Encyclopedia:Cantrips_Orisons}cantrips{/g}, or 0-level {g|Encyclopedia:Spell}spells{/g}. These spells are cast like any other spell, but they are not expended when cast and may be used again.");
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[18]
					{
						IsekaiProtagonistSpellList.MageLightAbility.ToReference<BlueprintUnitFactReference>(),
						IsekaiProtagonistSpellList.JoltAbility.ToReference<BlueprintUnitFactReference>(),
						IsekaiProtagonistSpellList.DisruptUndeadAbility.ToReference<BlueprintUnitFactReference>(),
						IsekaiProtagonistSpellList.AcidSplashAbility.ToReference<BlueprintUnitFactReference>(),
						IsekaiProtagonistSpellList.DismissAreaEffectAbility.ToReference<BlueprintUnitFactReference>(),
						IsekaiProtagonistSpellList.DazeAbility.ToReference<BlueprintUnitFactReference>(),
						IsekaiProtagonistSpellList.TouchOfFatigueAbility.ToReference<BlueprintUnitFactReference>(),
						IsekaiProtagonistSpellList.FlareAbility.ToReference<BlueprintUnitFactReference>(),
						IsekaiProtagonistSpellList.RayOfFrostAbility.ToReference<BlueprintUnitFactReference>(),
						IsekaiProtagonistSpellList.ResistanceAbility.ToReference<BlueprintUnitFactReference>(),
						IsekaiProtagonistSpellList.DivineZapAbility.ToReference<BlueprintUnitFactReference>(),
						IsekaiProtagonistSpellList.GuidanceAbility.ToReference<BlueprintUnitFactReference>(),
						IsekaiProtagonistSpellList.VirtueAbility.ToReference<BlueprintUnitFactReference>(),
						BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "ShadowDaggerAbility"),
						BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "HeavenlyRayAbility"),
						BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "CorrosiveSlimeAbility"),
						BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "MindSpikeAbility"),
						BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "GraveRayAbility")
					};
				});
			});
		}
	}
}
