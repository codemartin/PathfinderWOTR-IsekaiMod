using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist
{
	internal class IsekaiProtagonistSpellsKnown
	{
		public static void Add()
		{
			int x = Main.IsekaiContext.AddedContent.IsekaiSpellsKnownIncrement;
			int x2 = 2 * x;
			int x3 = 3 * x;
			int x34 = 3 * x;
			int x35 = 4 * x;
			int x36 = 5 * x;
			Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiProtagonistSpellsKnown", delegate(BlueprintSpellsTable bp)
			{
				bp.Levels = new SpellsLevelEntry[41]
				{
					new SpellsLevelEntry
					{
						Count = new int[0]
					},
					new SpellsLevelEntry
					{
						Count = new int[2] { 0, x }
					},
					new SpellsLevelEntry
					{
						Count = new int[2] { 0, x2 }
					},
					new SpellsLevelEntry
					{
						Count = new int[3] { 0, x3, x }
					},
					new SpellsLevelEntry
					{
						Count = new int[3] { 0, x35, x2 }
					},
					new SpellsLevelEntry
					{
						Count = new int[4] { 0, x35, x3, x }
					},
					new SpellsLevelEntry
					{
						Count = new int[4] { 0, x36, x35, x }
					},
					new SpellsLevelEntry
					{
						Count = new int[5] { 0, x36, x35, x2, x }
					},
					new SpellsLevelEntry
					{
						Count = new int[5] { 0, x36, x36, x3, x }
					},
					new SpellsLevelEntry
					{
						Count = new int[6] { 0, x36, x36, x3, x2, x }
					},
					new SpellsLevelEntry
					{
						Count = new int[6] { 0, x36, x36, x35, x3, x }
					},
					new SpellsLevelEntry
					{
						Count = new int[7] { 0, x36, x36, x35, x3, x2, x }
					},
					new SpellsLevelEntry
					{
						Count = new int[7] { 0, x36, x36, x35, x35, x3, x }
					},
					new SpellsLevelEntry
					{
						Count = new int[8] { 0, x36, x36, x35, x35, x3, x2, x }
					},
					new SpellsLevelEntry
					{
						Count = new int[8] { 0, x36, x36, x35, x35, x35, x3, x }
					},
					new SpellsLevelEntry
					{
						Count = new int[9] { 0, x36, x36, x35, x35, x35, x3, x2, x }
					},
					new SpellsLevelEntry
					{
						Count = new int[9] { 0, x36, x36, x35, x35, x35, x35, x3, x }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x3, x2, x }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x3, x }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x2 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x34 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					},
					new SpellsLevelEntry
					{
						Count = new int[10] { 0, x36, x36, x35, x35, x35, x35, x35, x35, x36 }
					}
				};
			});
		}

		public static BlueprintSpellsTable Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintSpellsTable>(Main.IsekaiContext, "IsekaiProtagonistSpellsKnown");
		}

		public static BlueprintSpellsTableReference GetReference()
		{
			return BlueprintTools.GetModBlueprintReference<BlueprintSpellsTableReference>(Main.IsekaiContext, "IsekaiProtagonistSpellsKnown");
		}
	}
}
