using System;
using HarmonyLib;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;

namespace IsekaiMod.Content.Guardians
{
	[HarmonyPatch(typeof(AddPet), "GetPetLevel")]
	internal static class PetLevelUncapperPatch
	{
		[HarmonyPostfix]
		public static void Postfix(AddPet __instance, ref int __result)
		{
			try
			{
				UnitEntityData owner = ((EntityFactComponentDelegate<UnitEntityData, AddPetData>)__instance).Owner;
				if (owner == null)
				{
					return;
				}
				UnitDescriptor descriptor = owner.Descriptor;
				if (descriptor == null || descriptor.Progression == null)
				{
					return;
				}
				BlueprintCharacterClass blueprintCharacterClass = IsekaiProtagonistClass.Get();
				if (blueprintCharacterClass == null || descriptor.Progression.GetClassData(blueprintCharacterClass) == null || descriptor.Progression.CharacterLevel <= 20)
				{
					return;
				}
				BlueprintFeature levelRank = __instance.LevelRank;
				if (levelRank == null)
				{
					return;
				}
				int rank = descriptor.Progression.Features.GetRank(levelRank);
				if (rank > 0)
				{
					int num = Math.Min(rank, descriptor.Progression.CharacterLevel);
					if (num > __result)
					{
						__result = num;
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
