using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist
{
	internal class IsekaiProtagonistSpellsPerDay
	{
		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiProtagonistSpellsPerDay", delegate(BlueprintSpellsTable bp)
			{
				bp.Levels = new SpellsLevelEntry[41]
				{
					new SpellsLevelEntry
					{
						Count = new int[0]
					},
					new SpellsLevelEntry
					{
						Count = new int[2] { 0, 3 }
					},
					new SpellsLevelEntry
					{
						Count = new int[2] { 0, 6 }
					},
					new SpellsLevelEntry
					{
						Count = new int[3] { 0, 9, 3 }
					},
					new SpellsLevelEntry
					{
						Count = new int[3] { 0, 12, 6 }
					},
					new SpellsLevelEntry
					{
						Count = new int[4] { 0, 12, 9, 3 }
					},
					new SpellsLevelEntry
					{
						Count = new int[4] { 0, 12, 12, 6 }
					},
					new SpellsLevelEntry
					{
						Count = new int[5] { 0, 12, 12, 9, 3 }
					},
					new SpellsLevelEntry
					{
						Count = new int[5] { 0, 12, 12, 12, 6 }
					},
					new SpellsLevelEntry
					{
						Count = new int[6] { 0, 12, 12, 12, 9, 3 }
					},
					new SpellsLevelEntry
					{
						Count = new int[6] { 0, 12, 12, 12, 12, 6 }
					},
					new SpellsLevelEntry
					{
						Count = new int[7] { 0, 12, 12, 12, 12, 9, 3 }
					},
					new SpellsLevelEntry
					{
						Count = new int[7] { 0, 12, 12, 12, 12, 12, 6 }
					},
					new SpellsLevelEntry
					{
						Count = new int[8] { 0, 12, 12, 12, 12, 12, 9, 3 }
					},
					new SpellsLevelEntry
					{
						Count = new int[8] { 0, 12, 12, 12, 12, 12, 12, 6 }
					},
					new SpellsLevelEntry
					{
						Count = new int[9] { 0, 12, 12, 12, 12, 12, 12, 9, 3 }
					},
					new SpellsLevelEntry
					{
						Count = new int[9] { 0, 12, 12, 12, 12, 12, 12, 12, 6 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 9, 3 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 6 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 9 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, 12, 12, 12, 12, 12, 12, 12, 12, 12 }
					}
				};
			});
		}

		public static BlueprintSpellsTable Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintSpellsTable>(Main.IsekaiContext, "IsekaiProtagonistSpellsPerDay");
		}

		public static BlueprintSpellsTableReference GetReference()
		{
			return BlueprintTools.GetModBlueprintReference<BlueprintSpellsTableReference>(Main.IsekaiContext, "IsekaiProtagonistSpellsPerDay");
		}
	}
}
