using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist;

internal static class StartingWeaponProficiencyFix
{
	internal static readonly (string Feature, WeaponCategory Category)[] ExoticStartingWeapons = new(string, WeaponCategory)[15]
	{
		("StartingWeaponKama", WeaponCategory.Kama),
		("StartingWeaponNunchaku", WeaponCategory.Nunchaku),
		("StartingWeaponSai", WeaponCategory.Sai),
		("StartingWeaponBastardSword", WeaponCategory.BastardSword),
		("StartingWeaponDuelingSword", WeaponCategory.DuelingSword),
		("StartingWeaponDwarvenWaraxe", WeaponCategory.DwarvenWaraxe),
		("StartingWeaponEstoc", WeaponCategory.Estoc),
		("StartingWeaponFalcata", WeaponCategory.Falcata),
		("StartingWeaponTongi", WeaponCategory.Tongi),
		("StartingWeaponElvenCurvedBlade", WeaponCategory.ElvenCurvedBlade),
		("StartingWeaponFauchard", WeaponCategory.Fauchard),
		("StartingWeaponDoubleSword", WeaponCategory.DoubleSword),
		("StartingWeaponDoubleAxe", WeaponCategory.DoubleAxe),
		("StartingWeaponUrgrosh", WeaponCategory.Urgrosh),
		("StartingWeaponHookedHammer", WeaponCategory.HookedHammer)
	};

	public static void Add()
	{
		(string, WeaponCategory)[] exoticStartingWeapons = ExoticStartingWeapons;
		for (int i = 0; i < exoticStartingWeapons.Length; i++)
		{
			(string, WeaponCategory) tuple = exoticStartingWeapons[i];
			BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, tuple.Item1);
			if (modBlueprint != null)
			{
				WeaponCategory category = tuple.Item2;
				modBlueprint.AddComponent(delegate(AddProficiencies c)
				{
					c.WeaponProficiencies = new WeaponCategory[1] { category };
				});
			}
		}
		BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowMonarchProficiencies")?.AddComponent(delegate(AddProficiencies c)
		{
			c.WeaponProficiencies = new WeaponCategory[1] { WeaponCategory.DuelingSword };
		});
	}
}
