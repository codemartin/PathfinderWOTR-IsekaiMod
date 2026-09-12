using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System;
using System.Linq;

namespace IsekaiMod.Components {
	[TypeId("1838cc68fe8444f5bce125f31bf7090d")]
	internal class ClassLevelGetterWithAlternatives : PropertyValueGetter {
		public override int GetBaseValue(UnitEntityData unit) {
			m_OriginalGetter ??= new ClassLevelGetter {
				m_Class = m_Class,
				m_Archetype = m_Archetype
			};
			return Math.Max(m_OriginalGetter.GetBaseValue(unit), GetAlternativeLevel(unit));
		}

		private int GetAlternativeLevel(UnitEntityData unit) {
			return (m_AlternativeClasses ?? Array.Empty<BlueprintCharacterClassReference>())
				.Select(characterClass => characterClass?.Get())
				.Where(characterClass => characterClass != null)
				.Select(characterClass => unit.Progression.GetClassLevel(characterClass))
				.DefaultIfEmpty(0)
				.Max();
		}

		public BlueprintCharacterClassReference m_Class;
		public BlueprintArchetypeReference m_Archetype;
		public BlueprintCharacterClassReference[] m_AlternativeClasses = Array.Empty<BlueprintCharacterClassReference>();
		[NonSerialized]
		private ClassLevelGetter m_OriginalGetter;
	}

	[TypeId("9506106d31af44d29a805444c5a68670")]
	internal class SummClassLevelGetterWithAlternatives : PropertyValueGetter {
		public override int GetBaseValue(UnitEntityData unit) {
			m_OriginalGetter ??= new SummClassLevelGetter {
				m_Class = m_Classes,
				Archetype = Archetype,
				m_Archetypes = m_Archetypes
			};
			return Math.Max(m_OriginalGetter.GetBaseValue(unit), GetAlternativeLevel(unit));
		}

		private int GetAlternativeLevel(UnitEntityData unit) {
			return (m_AlternativeClasses ?? Array.Empty<BlueprintCharacterClassReference>())
				.Select(characterClass => characterClass?.Get())
				.Where(characterClass => characterClass != null)
				.Select(characterClass => unit.Progression.GetClassLevel(characterClass))
				.DefaultIfEmpty(0)
				.Max();
		}

		public BlueprintCharacterClassReference[] m_Classes = Array.Empty<BlueprintCharacterClassReference>();
		public BlueprintArchetypeReference Archetype;
		public BlueprintArchetypeReference[] m_Archetypes = Array.Empty<BlueprintArchetypeReference>();
		public BlueprintCharacterClassReference[] m_AlternativeClasses = Array.Empty<BlueprintCharacterClassReference>();
		[NonSerialized]
		private SummClassLevelGetter m_OriginalGetter;
	}
}
