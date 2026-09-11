using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes
{
	internal class MastermindSpellbook
	{
		public static void Add()
		{
			PatchTools.RegisterSpellbook(Helpers.CreateBlueprint(Main.IsekaiContext, "MastermindSpellbook", delegate(BlueprintSpellbook bp)
			{
				bp.Name = Helpers.CreateString(Main.IsekaiContext, "MastermindSpellbook.Name", "Mastermind");
				bp.Spontaneous = true;
				bp.CastingAttribute = StatType.Intelligence;
				bp.CantripsType = CantripsType.Cantrips;
				bp.IsArcane = true;
				bp.IsArcanist = true;
				bp.m_SpellsPerDay = MastermindSpellsPerDay.GetReference();
				bp.m_SpellsKnown = null;
				bp.m_SpellList = MastermindSpellList.GetReference();
				bp.m_SpellSlots = MastermindSpellsPerDay.GetReference();
				bp.SpellsPerLevel = 4;
				bp.AllSpellsKnown = false;
				bp.CanCopyScrolls = true;
				bp.IsMythic = false;
				bp.m_MythicSpellList = null;
				bp.HasSpecialSpellList = false;
				bp.SpecialSpellListName = StaticReferences.Strings.Null;
			}));
		}

		public static BlueprintSpellbook Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintSpellbook>(Main.IsekaiContext, "MastermindSpellbook");
		}

		public static BlueprintSpellbookReference GetReference()
		{
			return Get().ToReference<BlueprintSpellbookReference>();
		}
	}
}
