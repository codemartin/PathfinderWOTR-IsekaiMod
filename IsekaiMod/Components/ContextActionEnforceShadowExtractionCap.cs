using System.Collections.Generic;
using System.Linq;
using Kingmaker;
using Kingmaker.AreaLogic.SummonPool;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Components
{
	[TypeId("21f6a8e30d324e89a9a747b2c914bf26")]
	public class ContextActionEnforceShadowExtractionCap : ContextAction
	{
		public BlueprintSummonPoolReference m_SummonPool;

		public BlueprintSummonPool SummonPool => m_SummonPool?.Get();

		public override string GetCaption()
		{
			return "Enforce Shadow Extraction Summon Cap (1 at Lv1-9, 2 at Lv10-19, 3 at Lv20)";
		}

		public override void RunAction()
		{
			try
			{
				ISummonPool summonPool = Game.Instance?.SummonPools?.GetPool(SummonPool);
				if (summonPool == null)
				{
					return;
				}
				int num = (base.Context?.MaybeCaster)?.Descriptor?.Progression?.CharacterLevel ?? 1;
				int num2 = ((num >= 20) ? 3 : ((num < 10) ? 1 : 2));
				List<UnitEntityData> list = summonPool.Units.Where((UnitEntityData u) => u != null && !u.Descriptor.State.IsDead).ToList();
				int num3 = list.Count - num2 + 1;
				for (int num4 = 0; num4 < num3 && num4 < list.Count; num4++)
				{
					UnitEntityData unitEntityData = list[num4];
					if (unitEntityData?.Descriptor != null)
					{
						BlueprintBuff blueprintBuff = Game.Instance?.BlueprintRoot?.SystemMechanics?.SummonedUnitBuff;
						if (blueprintBuff != null)
						{
							unitEntityData.Descriptor.RemoveFact(blueprintBuff);
						}
					}
				}
			}
			catch
			{
			}
		}
	}
}
