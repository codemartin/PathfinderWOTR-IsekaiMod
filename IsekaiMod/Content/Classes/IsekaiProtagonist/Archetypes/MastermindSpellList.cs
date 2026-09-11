using System;
using System.Collections.Generic;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes
{
	internal class MastermindSpellList
	{
		public static void Add()
		{
			Helpers.CreateBlueprint<BlueprintSpellList>(Main.IsekaiContext, "MastermindSpellList");
		}

		public static BlueprintSpellList Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintSpellList>(Main.IsekaiContext, "MastermindSpellList");
		}

		public static BlueprintSpellListReference GetReference()
		{
			return BlueprintTools.GetModBlueprintReference<BlueprintSpellListReference>(Main.IsekaiContext, "MastermindSpellList");
		}

		public static void PatchMastermindSpellList()
		{
			BlueprintSpellList blueprintSpellList = Get();
			if (blueprintSpellList == null)
			{
				return;
			}
			SpellLevelList[] array = IsekaiProtagonistSpellList.Get()?.SpellsByLevel;
			if (array != null)
			{
				SpellLevelList[] array2 = (SpellLevelList[])array.Clone();
				if (array2.Length <= 10)
				{
					Array.Resize(ref array2, 11);
				}
				array2[10] = new SpellLevelList(10)
				{
					m_Spells = new List<BlueprintAbilityReference>
					{
						BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("483157d358afd1a498c2a4762f4057ba"),
						BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("a948e10ecf1fa674dbae5eaae7f25a7f"),
						BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("af92783492851f445abb2c01d346c376"),
						BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("1b76573f991543145897702b7edc4d7a"),
						BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("7d721be6d74f07f4d952ee8d6f8f44a0"),
						BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("24067ba8e0e69a14e83ff397826f6c6d"),
						BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>("cc74245ba989480488925214dd925100")
					}
				};
				blueprintSpellList.SpellsByLevel = array2;
			}
		}
	}
}
