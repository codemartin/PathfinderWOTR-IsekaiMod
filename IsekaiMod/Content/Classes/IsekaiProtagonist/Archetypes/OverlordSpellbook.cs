using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes
{
	internal class OverlordSpellbook
	{
		public static void Add()
		{
			PatchTools.RegisterSpellbook(Helpers.CreateBlueprint(Main.IsekaiContext, "OverlordSpellbook", delegate(BlueprintSpellbook bp)
			{
				bp.Name = Helpers.CreateString(Main.IsekaiContext, "OverlordSpellbook.Name", "Overlord");
				bp.Spontaneous = true;
				bp.CastingAttribute = StatType.Charisma;
				bp.CantripsType = CantripsType.Cantrips;
				bp.IsArcane = true;
				bp.IsArcanist = false;
				bp.m_SpellsPerDay = IsekaiProtagonistSpellsPerDay.GetReference();
				bp.m_SpellsKnown = IsekaiProtagonistSpellsKnown.GetReference();
				bp.m_SpellList = IsekaiProtagonistSpellList.GetReference();
				bp.m_SpellSlots = null;
				bp.SpellsPerLevel = 0;
				bp.AllSpellsKnown = false;
				bp.CanCopyScrolls = false;
				bp.IsMythic = false;
				bp.m_MythicSpellList = null;
				bp.HasSpecialSpellList = false;
				bp.SpecialSpellListName = StaticReferences.Strings.Null;
			}));
		}

		public static BlueprintSpellbook Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintSpellbook>(Main.IsekaiContext, "OverlordSpellbook");
		}

		public static BlueprintSpellbookReference GetReference()
		{
			return Get().ToReference<BlueprintSpellbookReference>();
		}
	}
}
