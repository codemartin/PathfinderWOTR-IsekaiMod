using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Components
{
	[TypeId("6a0e920f5f664d6eb5449def3542208b")]
	[AllowedOn(typeof(BlueprintFeature), false)]
	public class ArmorNoSpeedPenalty : UnitFactComponentDelegate
	{
		public override void OnTurnOn()
		{
			base.OnTurnOn();
			base.Owner?.State?.Features?.ImmunityToMediumArmorSpeedPenalty?.Retain();
			base.Owner?.State?.Features?.ImmuneToArmorSpeedPenalty?.Retain();
			UnitEntityData owner = base.Owner;
			if ((object)owner != null && owner.Body?.Armor?.HasArmor == true)
			{
				base.Owner.Body.Armor.Armor.RecalculateStats();
			}
		}

		public override void OnTurnOff()
		{
			base.OnTurnOff();
			base.Owner?.State?.Features?.ImmunityToMediumArmorSpeedPenalty?.Release();
			base.Owner?.State?.Features?.ImmuneToArmorSpeedPenalty?.Release();
			UnitEntityData owner = base.Owner;
			if ((object)owner != null && owner.Body?.Armor?.HasArmor == true)
			{
				base.Owner.Body.Armor.Armor.RecalculateStats();
			}
		}
	}
}
