using System;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;

namespace IsekaiMod.Components
{
	[ClassInfoBox("If Owner summons, it summons additional units")]
	[TypeId("d104b3fcf9094fae838ad3793767348c")]
	public class ExtraSummonCount : UnitFactComponentDelegate, ISubscriber, IGlobalSubscriber, ICalculateSummonUnitsCount
	{
		public int Count;

		public void HandleCalculateSummonUnitsCount(MechanicsContext mechanicsContext, ContextDiceValue contextDiceValue, ref int count)
		{
			if (!(mechanicsContext.MaybeCaster != base.Owner))
			{
				int maxStat = GetMaxStat();
				int val = ((base.Owner.Progression != null && base.Owner.Progression.CharacterLevel >= 15) ? 3 : 2);
				int num = Math.Max(1, Math.Min(val, maxStat));
				count += num;
			}
		}

		public int GetMaxStat()
		{
			int valueOrDefault = (base.Owner?.Stats?.GetAttribute(StatType.Intelligence)?.Bonus).GetValueOrDefault();
			int valueOrDefault2 = (base.Owner?.Stats?.GetAttribute(StatType.Wisdom)?.Bonus).GetValueOrDefault();
			int valueOrDefault3 = (base.Owner?.Stats?.GetAttribute(StatType.Charisma)?.Bonus).GetValueOrDefault();
			return Math.Max(valueOrDefault, Math.Max(valueOrDefault2, valueOrDefault3));
		}
	}
}
