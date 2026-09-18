using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;

namespace IsekaiMod.Components
{
	[AllowedOn(typeof(BlueprintAbility), false)]
	[TypeId("5b7b2d4f9ae54f14b0ee8df48ab29f50")]
	public class AbilityCasterCharacterLevelRestriction : BlueprintComponent, IAbilityCasterRestriction
	{
		public int RequiredLevel = 10;

		public bool IsCasterRestrictionPassed(UnitEntityData caster)
		{
			return (caster?.Progression?.CharacterLevel ?? 1) >= RequiredLevel;
		}

		public string GetAbilityCasterRestrictionUIText()
		{
			return $"Requires Character Level {RequiredLevel}";
		}
	}
}
