using System.Collections.Generic;
using IsekaiMod.Content.Arenas;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.AreaLogic.Cutscenes;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.Armies.TacticalCombat.Parts;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.GameModes;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Content.Features.IsekaiProtagonist;

internal static class CampaignEncounterGuard
{
	internal static bool Excludes(bool armyBattle, bool cutscene, bool dialogue, bool coliseum, bool summoned)
	{
		return armyBattle | cutscene | dialogue | coliseum | summoned;
	}

	internal static bool Excludes(UnitEntityData unit)
	{
		if (unit == null)
		{
			return true;
		}
		return Excludes(IsArmyBattle(unit), IsCutscene(), IsDialogue(), IsColiseum(unit), IsSummonedUnit(unit));
	}

	internal static bool IsArmyBattle(UnitEntityData unit)
	{
		try
		{
			if (TacticalCombatHelper.IsActive)
			{
				return true;
			}
			return unit != null && unit.Get<UnitPartTacticalCombat>() != null;
		}
		catch
		{
			return false;
		}
	}

	internal static bool IsCutscene()
	{
		try
		{
			Game instance = Game.Instance;
			if (instance == null)
			{
				return false;
			}
			CutsceneLock cutsceneLock = instance.CutsceneLock;
			return (cutsceneLock != null && cutsceneLock.Active) || instance.CurrentMode == GameModeType.Cutscene || instance.CurrentMode == GameModeType.CutsceneGlobalMap;
		}
		catch
		{
			return false;
		}
	}

	internal static bool IsDialogue()
	{
		try
		{
			Game instance = Game.Instance;
			if (instance == null)
			{
				return false;
			}
			return instance.DialogController?.CurrentCue != null || instance.CurrentMode == GameModeType.Dialog;
		}
		catch
		{
			return false;
		}
	}

	internal static bool IsColiseum(UnitEntityData unit)
	{
		try
		{
			if (DimensionalArenaManager.Instance != null && DimensionalArenaManager.Instance.IsActive)
			{
				return true;
			}
			if (PlanarColiseumArea.CurrentArenaAct() != 0)
			{
				return true;
			}
			if (unit == null)
			{
				return false;
			}
			string text = SaveScopedStorage.SpawnKindOf(unit);
			return text == "ColiseumContestant" || text == "ColiseumCameo";
		}
		catch
		{
			return false;
		}
	}

	internal static bool IsSummonedUnit(UnitEntityData unit)
	{
		if (unit == null)
		{
			return false;
		}
		try
		{
			return unit.Master != null || unit.IsPet || unit.IsSummoned();
		}
		catch
		{
			return false;
		}
	}

	internal static bool IsPartysEnemy(bool inParty, bool playerFaction, bool attacksPlayer)
	{
		return (!inParty && !playerFaction) & attacksPlayer;
	}

	internal static bool IsPartysEnemy(UnitEntityData unit)
	{
		if (unit == null)
		{
			return false;
		}
		try
		{
			Player player = Game.Instance?.Player;
			if (player == null)
			{
				return false;
			}
			return IsPartysEnemy(player.PartyAndPets.Contains(unit) || player.PartyAndPetsDetached.Contains(unit), unit.IsPlayerFaction, unit.IsPlayersEnemy);
		}
		catch
		{
			return false;
		}
	}

	internal static bool IsBossEncounter(EncounterType? encounter)
	{
		return encounter == EncounterType.Boss;
	}

	internal static bool IsBoss(BlueprintUnit blueprint)
	{
		if (blueprint == null)
		{
			return false;
		}
		try
		{
			return IsBossEncounter(blueprint.GetComponent<Experience>()?.Encounter);
		}
		catch
		{
			return false;
		}
	}

	internal static bool IsPlotProtected(UnitEntityData unit)
	{
		if (unit == null)
		{
			return false;
		}
		try
		{
			UnitDescriptor descriptor = unit.Descriptor;
			if (descriptor == null)
			{
				return false;
			}
			if (descriptor.IsEssentialForGame)
			{
				return true;
			}
			UnitMechanicFeatures unitMechanicFeatures = descriptor.State?.Features;
			return unitMechanicFeatures != null && (bool)unitMechanicFeatures.Immortality;
		}
		catch
		{
			return false;
		}
	}

	internal static bool AppliesToParty(bool settingOn, int protagonistsInParty)
	{
		if (settingOn)
		{
			return protagonistsInParty > 0;
		}
		return false;
	}

	internal static int ProtagonistsInParty()
	{
		try
		{
			BlueprintCharacterClass blueprintCharacterClass = IsekaiProtagonistClass.Get();
			List<UnitEntityData> list = Game.Instance?.Player?.Party;
			if (list == null || blueprintCharacterClass == null)
			{
				return 0;
			}
			int num = 0;
			foreach (UnitEntityData item in list)
			{
				if (item?.Descriptor?.Progression != null && item.Descriptor.Progression.GetClassLevel(blueprintCharacterClass) > 0)
				{
					num++;
				}
			}
			return num;
		}
		catch
		{
			return 0;
		}
	}

	internal static int PartyLevel()
	{
		try
		{
			return (Game.Instance?.Player?.MainCharacter.Value?.Progression?.CharacterLevel).GetValueOrDefault();
		}
		catch
		{
			return 0;
		}
	}

	internal static uint StableHash(string text)
	{
		uint num = 2166136261u;
		if (text == null)
		{
			return num;
		}
		foreach (char c in text)
		{
			num ^= c;
			num *= 16777619;
		}
		return num;
	}
}
