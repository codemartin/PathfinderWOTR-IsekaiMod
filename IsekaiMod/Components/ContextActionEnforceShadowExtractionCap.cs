using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.AreaLogic.SummonPool;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Parts;

namespace IsekaiMod.Components;

[TypeId("21f6a8e30d324e89a9a747b2c914bf26")]
public class ContextActionEnforceShadowExtractionCap : IsekaiContextAction
{
	public BlueprintSummonPoolReference m_SummonPool;

	public BlueprintUnitReference[] m_ShadowUnits = new BlueprintUnitReference[0];

	public int FixedCap;

	public BlueprintSummonPoolReference[] m_LegacySummonPools = new BlueprintSummonPoolReference[0];

	public bool AfterSpawn;

	public BlueprintSummonPool SummonPool => m_SummonPool?.Get();

	public override string GetCaption()
	{
		return "Enforce Shadow Extraction Summon Cap (1 at Lv1-9, 2 at Lv10-19, 3 at Lv20)";
	}

	internal static int ResolveCap(int fixedCap, int characterLevel)
	{
		if (fixedCap > 0)
		{
			return fixedCap;
		}
		if (characterLevel >= 20)
		{
			return 3;
		}
		if (characterLevel >= 10)
		{
			return 2;
		}
		return 1;
	}

	internal static int CountToDismiss(int activeShadows, int cap)
	{
		return CountToDismiss(activeShadows, cap, afterSpawn: false);
	}

	internal static int CountToDismiss(int activeShadows, int cap, bool afterSpawn)
	{
		int num = activeShadows - cap + ((!afterSpawn) ? 1 : 0);
		if (num < 0)
		{
			return 0;
		}
		if (num <= activeShadows)
		{
			return num;
		}
		return activeShadows;
	}

	private bool IsShadowUnit(UnitEntityData unit)
	{
		if (m_ShadowUnits == null || m_ShadowUnits.Length == 0)
		{
			return true;
		}
		BlueprintGuid guid = unit.Blueprint?.AssetGuid ?? BlueprintGuid.Empty;
		if (guid == BlueprintGuid.Empty)
		{
			return false;
		}
		return m_ShadowUnits.Any((BlueprintUnitReference reference) => reference != null && reference.Guid == guid);
	}

	private static bool IsSummonedBy(UnitEntityData unit, UnitEntityData caster)
	{
		UnitEntityData unitEntityData = unit.Get<UnitPartSummonedMonster>()?.Summoner;
		if (unitEntityData != null)
		{
			return unitEntityData.UniqueId == caster.UniqueId;
		}
		return false;
	}

	protected override void RunSafely()
	{
		if (base.Context == null)
		{
			return;
		}
		ISummonPool summonPool = Game.Instance?.SummonPools?.GetPool(SummonPool);
		if (summonPool == null)
		{
			return;
		}
		UnitEntityData caster = base.Context.MaybeCaster;
		if (caster == null)
		{
			return;
		}
		int cap = ResolveCap(FixedCap, caster.Descriptor?.Progression?.CharacterLevel ?? 1);
		IEnumerable<UnitEntityData> first = Enumerable.Empty<UnitEntityData>();
		if (m_ShadowUnits != null && m_ShadowUnits.Length != 0 && m_LegacySummonPools != null)
		{
			BlueprintSummonPoolReference[] legacySummonPools = m_LegacySummonPools;
			for (int i = 0; i < legacySummonPools.Length; i++)
			{
				BlueprintSummonPool blueprintSummonPool = legacySummonPools[i]?.Get();
				if (blueprintSummonPool != null && blueprintSummonPool != SummonPool)
				{
					ISummonPool pool = Game.Instance.SummonPools.GetPool(blueprintSummonPool);
					if (pool != null)
					{
						first = first.Concat(pool.Units);
					}
				}
			}
		}
		first = first.Concat(summonPool.Units);
		List<UnitEntityData> list = (from u in (from u in first.Distinct()
				where u?.Descriptor != null && !u.Descriptor.State.IsDead
				select u).Where(IsShadowUnit)
			where IsSummonedBy(u, caster)
			select u).ToList();
		BlueprintBuff blueprintBuff = Game.Instance?.BlueprintRoot?.SystemMechanics?.SummonedUnitBuff;
		if (blueprintBuff != null)
		{
			int num = CountToDismiss(list.Count, cap, AfterSpawn);
			for (int num2 = 0; num2 < num; num2++)
			{
				list[num2].Descriptor.RemoveFact(blueprintBuff);
			}
		}
	}
}
