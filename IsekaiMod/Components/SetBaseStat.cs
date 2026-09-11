using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using Newtonsoft.Json;

namespace IsekaiMod.Components
{
	[AllowMultipleComponents]
	[TypeId("855a9cc5d06042398707b9085fc92484")]
	[ComponentName("Set base stat")]
	[AllowedOn(typeof(BlueprintUnitFact), false)]
	[AllowedOn(typeof(BlueprintUnit), false)]
	public class SetBaseStat : UnitFactComponentDelegate<SetBaseStat.ComponentData>
	{
		public class ComponentData
		{
			[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
			public int BaseStatValue;
		}

		public StatType Stat;

		public int Value;

		public override void OnActivate()
		{
			ModifiableValueAttributeStat modifiableValueAttributeStat = base.Owner?.Stats?.GetAttribute(Stat);
			if (modifiableValueAttributeStat != null)
			{
				base.Data.BaseStatValue = modifiableValueAttributeStat.BaseValue;
			}
		}

		public override void OnDeactivate()
		{
			ModifiableValueAttributeStat modifiableValueAttributeStat = base.Owner?.Stats?.GetAttribute(Stat);
			if (modifiableValueAttributeStat != null)
			{
				modifiableValueAttributeStat.BaseValue = base.Data.BaseStatValue;
			}
		}

		public override void OnTurnOn()
		{
			ModifiableValueAttributeStat modifiableValueAttributeStat = base.Owner?.Stats?.GetAttribute(Stat);
			if (modifiableValueAttributeStat != null)
			{
				modifiableValueAttributeStat.BaseValue = Math.Max(modifiableValueAttributeStat.BaseValue, Value);
			}
		}

		public override void OnTurnOff()
		{
		}
	}
}
