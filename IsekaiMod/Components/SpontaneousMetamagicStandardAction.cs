using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Components;

[AllowMultipleComponents]
[ComponentName("Spontaneous metamagic does not add casting time")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
[TypeId("3f6a1c52b8e44d7a9c0e2b715d9a4e61")]
public class SpontaneousMetamagicStandardAction : UnitFactComponentDelegate
{
	public BlueprintSpellbookReference[] m_Spellbooks = new BlueprintSpellbookReference[0];

	internal bool Covers(BlueprintSpellbook spellbook)
	{
		if (spellbook == null || m_Spellbooks == null)
		{
			return false;
		}
		BlueprintGuid id = spellbook.AssetGuid;
		if (id != BlueprintGuid.Empty)
		{
			return m_Spellbooks.Any((BlueprintSpellbookReference r) => r != null && ((BlueprintReferenceBase)r).deserializedGuid == id);
		}
		return false;
	}

	internal static bool RequireFullRound(bool vanillaResult, bool standardAction, bool spontaneous, bool arcanist, bool hasSlotMetamagic, bool bookCovered, bool blueprintFullRound, bool modifiersRequireFullRound)
	{
		if (!vanillaResult)
		{
			return false;
		}
		if (((!standardAction || !spontaneous) | arcanist) || !hasSlotMetamagic || !bookCovered)
		{
			return vanillaResult;
		}
		return blueprintFullRound | modifiersRequireFullRound;
	}
}
