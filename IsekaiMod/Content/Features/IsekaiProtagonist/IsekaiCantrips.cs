using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist;

internal class IsekaiCantrips
{
	public static void Add()
	{
		Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiCantrips", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Cantrips");
			bp.SetDescription(Main.IsekaiContext, "You can cast a number of {g|Encyclopedia:Cantrips_Orisons}cantrips{/g}, or 0-level {g|Encyclopedia:Spell}spells{/g}. These spells are cast like any other spell, but they are not expended when cast and may be used again.");
		});
	}
}
