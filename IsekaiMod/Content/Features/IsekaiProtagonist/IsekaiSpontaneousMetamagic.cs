using IsekaiMod.Components;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist;

internal static class IsekaiSpontaneousMetamagic
{
	internal static readonly string[] CoveredSpellbooks = new string[4] { "IsekaiProtagonistSpellbook", "GodEmperorSpellbook", "OverlordSpellbook", "ShadowMonarchSpellbook" };

	public static void Add()
	{
		Sprite Icon_Metamagic = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("a1de1e4f92195b442adb946f0e2b9d4e"))?.m_Icon;
		Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiSpontaneousMetamagic", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Protagonist's Improvisation");
			bp.SetDescription(Main.IsekaiContext, "Applying metamagic to a spell cast from your Isekai Protagonist spellbook does not increase its casting time. Spells from your other spellbooks are unaffected.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Metamagic;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(SpontaneousMetamagicStandardAction c)
			{
				c.m_Spellbooks = new BlueprintSpellbookReference[4]
				{
					BlueprintTools.GetModBlueprintReference<BlueprintSpellbookReference>(Main.IsekaiContext, "IsekaiProtagonistSpellbook"),
					BlueprintTools.GetModBlueprintReference<BlueprintSpellbookReference>(Main.IsekaiContext, "GodEmperorSpellbook"),
					BlueprintTools.GetModBlueprintReference<BlueprintSpellbookReference>(Main.IsekaiContext, "OverlordSpellbook"),
					BlueprintTools.GetModBlueprintReference<BlueprintSpellbookReference>(Main.IsekaiContext, "ShadowMonarchSpellbook")
				};
			});
		});
	}
}
