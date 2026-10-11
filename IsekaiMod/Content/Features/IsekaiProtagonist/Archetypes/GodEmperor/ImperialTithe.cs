using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;

internal class ImperialTithe
{
	private static readonly Sprite Icon_Gold = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_DUPE_GOLD.png");

	public static void Add()
	{
		Helpers.CreateBlueprint(Main.IsekaiContext, "ImperialTitheFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Imperial Treasury: Devotional Tithes");
			bp.SetDescription(Main.IsekaiContext, "As an ascending deity, the God Emperor receives continuous tribute and tithes from mortals, subjects, and crusaders across the realm. The first time you complete a rest on each in-game day, you receive scaling devotional tributes of gold and Crusade resources:\nLevel 1: 2,500 Gold, 10 Crusade Finances\nLevel 5: 10,000 Gold, 50 Crusade Finances, 10 Materials, 5 Favors\nLevel 10: 25,000 Gold, 150 Crusade Finances, 50 Materials, 20 Favors\nLevel 15: 50,000 Gold, 300 Crusade Finances, 100 Materials, 50 Favors\nLevel 20: 100,000 Gold, 1,000 Crusade Finances, 200 Materials, 100 Favors.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Gold;
			bp.IsClassFeature = true;
			bp.AddComponent<ImperialTitheComponent>();
		});
	}
}
