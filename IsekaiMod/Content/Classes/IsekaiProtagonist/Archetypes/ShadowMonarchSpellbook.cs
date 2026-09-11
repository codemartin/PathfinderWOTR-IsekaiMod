using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes
{
	internal class ShadowMonarchSpellbook
	{
		public static void Add()
		{
			PatchTools.RegisterSpellbook(Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowMonarchSpellbook", delegate(BlueprintSpellbook bp)
			{
				bp.Name = Helpers.CreateString(Main.IsekaiContext, "ShadowMonarchSpellbook.Name", "Shadow Monarch");
				bp.Spontaneous = true;
				bp.CastingAttribute = StatType.Charisma;
				bp.CantripsType = CantripsType.Cantrips;
				bp.IsArcane = true;
				bp.IsArcanist = false;
				bp.m_SpellsPerDay = IsekaiProtagonistSpellsPerDay.GetReference();
				bp.m_SpellsKnown = IsekaiProtagonistSpellsKnown.GetReference();
				bp.m_SpellList = IsekaiProtagonistSpellList.GetReference();
				bp.m_CharacterClass = IsekaiProtagonistClass.GetReference();
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

		public static void SetCharacterClass(BlueprintCharacterClass characterClass)
		{
			BlueprintSpellbook blueprintSpellbook = Get();
			if (blueprintSpellbook != null && characterClass != null)
			{
				blueprintSpellbook.m_CharacterClass = characterClass.ToReference<BlueprintCharacterClassReference>();
			}
		}

		public static BlueprintSpellbook Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintSpellbook>(Main.IsekaiContext, "ShadowMonarchSpellbook");
		}

		public static BlueprintSpellbookReference GetReference()
		{
			return Get().ToReference<BlueprintSpellbookReference>();
		}
	}
}
