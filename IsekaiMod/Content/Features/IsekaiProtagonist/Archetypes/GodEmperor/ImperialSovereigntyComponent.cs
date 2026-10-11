using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers.Rest;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Kingdom;
using Kingmaker.Kingdom.Blueprints;
using Kingmaker.Kingdom.Buffs;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor;

[TypeId("d71a2c2ded1e46f7a6114d0f2734ee49")]
public class ImperialSovereigntyComponent : UnitFactComponentDelegate, IRestFinishedHandler, ISubscriber, IGlobalSubscriber, IKingdomDayHandler, IAreaHandler
{
	private static readonly BlueprintKingdomBuffReference CrusadeMaxMoraleBonus20 = BlueprintTools.GetBlueprintReference<BlueprintKingdomBuffReference>("5d021876b91bdb4468d2fdb524c7de28");

	private static readonly BlueprintKingdomBuffReference ArmyFamousRecruiter = BlueprintTools.GetBlueprintReference<BlueprintKingdomBuffReference>("5ab635a173de4a00a00cb1d285543bb4");

	private static readonly BlueprintKingdomBuffReference ArmyUnitsEnchantedWeapons = BlueprintTools.GetBlueprintReference<BlueprintKingdomBuffReference>("30a2bdc9fd8f4d8ab94a98da3a4d41cf");

	private static readonly BlueprintKingdomBuffReference ArmyUnitsSoundMind = BlueprintTools.GetBlueprintReference<BlueprintKingdomBuffReference>("de7ea4ce6760427488884157f0075c80");

	internal const string StorageName = "IsekaiMod.ImperialSovereignty";

	private static BlueprintKingdomBuffReference[] SovereigntyBuffs => new BlueprintKingdomBuffReference[4] { CrusadeMaxMoraleBonus20, ArmyFamousRecruiter, ArmyUnitsEnchantedWeapons, ArmyUnitsSoundMind };

	public void HandleRestFinished(RestStatus status)
	{
		ApplyCrusadeSovereignty();
	}

	public void OnNewDay()
	{
		ApplyCrusadeSovereignty();
	}

	public void OnAreaDidLoad()
	{
		ApplyCrusadeSovereignty();
	}

	public void OnAreaBeginUnloading()
	{
	}

	public override void OnActivate()
	{
		base.OnActivate();
		ApplyCrusadeSovereignty();
	}

	private void ApplyCrusadeSovereignty()
	{
		UnitEntityData owner = base.Owner;
		if (owner == null || !owner.IsMainCharacter)
		{
			return;
		}
		try
		{
			KingdomState kingdom = Game.Instance.Player.Kingdom;
			if (kingdom != null)
			{
				ApplyBuffIfMissing(kingdom, CrusadeMaxMoraleBonus20);
				ApplyBuffIfMissing(kingdom, ArmyFamousRecruiter);
				ApplyBuffIfMissing(kingdom, ArmyUnitsEnchantedWeapons);
				ApplyBuffIfMissing(kingdom, ArmyUnitsSoundMind);
			}
		}
		catch
		{
		}
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		try
		{
			KingdomState kingdomState = Game.Instance?.Player?.Kingdom;
			if (kingdomState != null)
			{
				BlueprintKingdomBuffReference[] sovereigntyBuffs = SovereigntyBuffs;
				foreach (BlueprintKingdomBuffReference buffRef in sovereigntyBuffs)
				{
					RemoveBuffIfPresent(kingdomState, buffRef);
				}
			}
		}
		catch
		{
		}
	}

	internal static bool ShouldRemoveOnDeactivate(bool addedBySovereignty, bool kingdomHasBuff)
	{
		return addedBySovereignty & kingdomHasBuff;
	}

	private static IDictionary<string, string> Ledger(bool create)
	{
		EntityDataBase entity = SaveScopedStorage.CurrentPlayer();
		if (!create)
		{
			return SaveScopedStorage.Find(entity, "IsekaiMod.ImperialSovereignty");
		}
		return SaveScopedStorage.Ensure(entity, "IsekaiMod.ImperialSovereignty");
	}

	private void ApplyBuffIfMissing(KingdomState kingdom, BlueprintKingdomBuffReference buffRef)
	{
		if (buffRef != null)
		{
			BlueprintKingdomBuff blueprintKingdomBuff = buffRef.Get();
			if (blueprintKingdomBuff != null && !kingdom.ActiveBuffs.HasFact(blueprintKingdomBuff))
			{
				KingdomBuff kingdomBuff = kingdom.AddBuff(blueprintKingdomBuff, null, null, 0);
				SaveScopedStorage.SetFlag(Ledger(create: true), blueprintKingdomBuff.AssetGuid.ToString(), kingdomBuff != null);
			}
		}
	}

	private void RemoveBuffIfPresent(KingdomState kingdom, BlueprintKingdomBuffReference buffRef)
	{
		if (buffRef == null)
		{
			return;
		}
		BlueprintKingdomBuff blueprintKingdomBuff = buffRef.Get();
		if (blueprintKingdomBuff != null)
		{
			IDictionary<string, string> storage = Ledger(create: false);
			string key = blueprintKingdomBuff.AssetGuid.ToString();
			if (ShouldRemoveOnDeactivate(kingdomHasBuff: kingdom.ActiveBuffs != null && kingdom.ActiveBuffs.HasFact(blueprintKingdomBuff), addedBySovereignty: SaveScopedStorage.GetFlag(storage, key)))
			{
				kingdom.ActiveBuffs.RemoveFact(blueprintKingdomBuff);
			}
			SaveScopedStorage.SetFlag(storage, key, value: false);
		}
	}
}
