using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Components;

[AllowedOn(typeof(BlueprintUnitFact), false)]
[TypeId("146dc4905df34e58bb7b9fcb70c78297")]
public class HostileBuffDescriptorImmunity : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCanApplyBuff>, IRulebookHandler<RuleCanApplyBuff>, ISubscriber, IInitiatorRulebookSubscriber
{
	public SpellDescriptorWrapper Descriptor;

	public void OnEventAboutToTrigger(RuleCanApplyBuff evt)
	{
		if (evt?.Context != null && !(base.Owner == null))
		{
			UnitEntityData maybeCaster = evt.Context.MaybeCaster;
			bool casterIsHostile = maybeCaster != null && maybeCaster != base.Owner && maybeCaster.IsEnemy(base.Owner);
			bool casterPiercesImmunity = maybeCaster != null && (bool)maybeCaster.State.Features.MythicReduceResistances;
			if (ShouldRefuse(Descriptor, evt.Context.SpellDescriptor, maybeCaster != null, casterIsHostile, casterPiercesImmunity))
			{
				evt.Immunity = true;
			}
		}
	}

	public void OnEventDidTrigger(RuleCanApplyBuff evt)
	{
	}

	internal static bool ShouldRefuse(SpellDescriptor immuneTo, SpellDescriptor effect, bool hasCaster, bool casterIsHostile, bool casterPiercesImmunity)
	{
		if ((immuneTo & effect) == SpellDescriptor.None)
		{
			return false;
		}
		if (!hasCaster)
		{
			return true;
		}
		if (casterIsHostile)
		{
			return !casterPiercesImmunity;
		}
		return false;
	}
}
