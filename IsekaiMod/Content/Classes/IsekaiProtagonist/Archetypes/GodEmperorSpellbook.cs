using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes
{
	internal class GodEmperorSpellbook
	{
		public static void Add()
		{
			PatchTools.RegisterSpellbook(Helpers.CreateBlueprint(Main.IsekaiContext, "GodEmperorSpellbook", delegate(BlueprintSpellbook bp)
			{
				bp.Name = Helpers.CreateString(Main.IsekaiContext, "GodEmperorSpellbook.Name", "God Emperor");
				bp.Spontaneous = true;
				bp.CastingAttribute = StatType.Wisdom;
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
			return BlueprintTools.GetModBlueprint<BlueprintSpellbook>(Main.IsekaiContext, "GodEmperorSpellbook");
		}

		public static BlueprintSpellbookReference GetReference()
		{
			return Get().ToReference<BlueprintSpellbookReference>();
		}
	}
}
