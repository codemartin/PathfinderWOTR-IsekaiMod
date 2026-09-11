using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist
{
	internal class IsekaiProtagonistSpellbook
	{
		public static void Add()
		{
			PatchTools.RegisterSpellbook(Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiProtagonistSpellbook", delegate(BlueprintSpellbook bp)
			{
				bp.Name = Helpers.CreateString(Main.IsekaiContext, "$IsekaiProtagonistSpellbook.Name", "Isekai Protagonist");
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

		public static void SetCharacterClass(BlueprintCharacterClass characterClass)
		{
			Get().m_CharacterClass = characterClass.ToReference<BlueprintCharacterClassReference>();
		}

		public static BlueprintSpellbook Get()
		{
			return BlueprintTools.GetModBlueprint<BlueprintSpellbook>(Main.IsekaiContext, "IsekaiProtagonistSpellbook");
		}

		public static BlueprintSpellbookReference GetReference()
		{
			return Get().ToReference<BlueprintSpellbookReference>();
		}
	}
}
