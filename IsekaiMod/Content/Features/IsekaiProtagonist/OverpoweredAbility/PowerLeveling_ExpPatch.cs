using System;
using System.Linq;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;

[HarmonyPatch(typeof(Player), "GainPartyExperience")]
internal static class PowerLeveling_ExpPatch
{
	public static void Prefix(ref int gained)
	{
		try
		{
			BlueprintFeature powerLeveling = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PowerLevelingFeature");
			if (powerLeveling != null && Game.Instance?.Player?.Party != null && Game.Instance.Player.Party.Any((UnitEntityData unit) => (object)unit != null && unit.Descriptor?.HasFact(powerLeveling) == true))
			{
				int chapter = Game.Instance.Player.Chapter;
				int characterLevel = PowerLeveling.HighestPartyLevel(Game.Instance.Player.Party.Select((UnitEntityData unit) => (unit?.Descriptor?.Progression?.CharacterLevel).GetValueOrDefault()));
				gained = PowerLeveling.ApplyExperienceMultiplier(gained, chapter, characterLevel);
			}
		}
		catch (Exception)
		{
		}
	}
}
