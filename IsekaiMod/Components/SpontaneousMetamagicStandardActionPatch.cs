using System;
using System.Linq;
using HarmonyLib;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Parts;

namespace IsekaiMod.Components;

[HarmonyPatch(typeof(AbilityData), "RequireFullRoundAction", MethodType.Getter)]
internal static class SpontaneousMetamagicStandardActionPatch
{
	[HarmonyPostfix]
	private static void Postfix(AbilityData __instance, ref bool __result)
	{
		if (!__result || __instance == null)
		{
			return;
		}
		try
		{
			BlueprintSpellbook book = __instance.Spellbook?.Blueprint;
			if (book == null || !book.Spontaneous || book.IsArcanist)
			{
				return;
			}
			MetamagicData metamagicData = __instance.MetamagicData;
			if (metamagicData != null && metamagicData.NotEmpty)
			{
				UnitEntityData unitEntityData = __instance.Caster?.Unit;
				if (!(unitEntityData == null) && unitEntityData.Descriptor.Progression.Features.SelectFactComponents<SpontaneousMetamagicStandardAction>().Any((SpontaneousMetamagicStandardAction c) => c.Covers(book)))
				{
					bool standardAction = __instance.RuntimeActionType == UnitCommand.CommandType.Standard;
					bool modifiersRequireFullRound = unitEntityData.Get<UnitPartAbilityModifiers>()?.RequireFullRound(__instance.Blueprint, __instance.GetDefaultActionType()) ?? false;
					__result = SpontaneousMetamagicStandardAction.RequireFullRound(__result, standardAction, spontaneous: true, arcanist: false, hasSlotMetamagic: true, bookCovered: true, __instance.Blueprint.IsFullRoundAction, modifiersRequireFullRound);
				}
			}
		}
		catch (Exception ex)
		{
			Main.Log("ERROR: SpontaneousMetamagicStandardActionPatch: " + ex);
		}
	}
}
