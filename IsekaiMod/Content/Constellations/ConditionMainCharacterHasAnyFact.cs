using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Content.Constellations;

[TypeId("b7c6a2f0d3e14a5c9d8e4f1a2b3c5d7e")]
public class ConditionMainCharacterHasAnyFact : IsekaiCondition
{
	public BlueprintUnitFactReference[] m_Facts;

	internal static bool AnyHeld(IEnumerable<bool> held)
	{
		return held?.Any((bool h) => h) ?? false;
	}

	protected override bool CheckSafely()
	{
		UnitEntityData player = DivineTokens.GetPlayer();
		if (player == null || m_Facts == null)
		{
			return false;
		}
		return AnyHeld(from f in m_Facts
			select f?.Get() into f
			where f != null
			select player.Descriptor.HasFact(f));
	}

	public override string GetConditionCaption()
	{
		return "The main character has any of the listed deity features";
	}
}
