using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Constellations;

[TypeId("5dc97e48ea784ed0b2cf961f6c3c3a71")]
public class ContextActionSpendCoinsAndGrantFact : IsekaiContextAction
{
	public int Cost;

	public BlueprintUnitFactReference m_Fact;

	public BlueprintFeatureReference m_KeyOfInfinitePathways;

	public AlignmentMaskType AllowedAlignment = AlignmentMaskType.Any;

	public bool RecordForRespec;

	public override string GetCaption()
	{
		return $"Spends {Cost} Cosmic Coins and grants permanent fact";
	}

	protected override void RunSafely()
	{
		UnitEntityData unitEntityData = DivineTokens.ResolveDialogTarget(base.Target, DivineTokens.GetPlayer);
		BlueprintUnitFact blueprintUnitFact = m_Fact?.Get();
		if (unitEntityData == null || blueprintUnitFact == null)
		{
			return;
		}
		if (unitEntityData.Descriptor.Progression.Features.HasFact(blueprintUnitFact))
		{
			DivineTokens.Log(CosmicStoreText.AlreadyOwned);
		}
		else if ((m_KeyOfInfinitePathways == null || !unitEntityData.Descriptor.Progression.Features.HasFact(m_KeyOfInfinitePathways.Get())) && !DivineSponsorshipStore.AlignmentAllows(AllowedAlignment, unitEntityData.Descriptor.Alignment.ValueRaw))
		{
			DivineTokens.Log(CosmicStoreText.WrongAlignment, DivineSponsorshipStore.AlignmentName(AllowedAlignment));
		}
		else if (DivineTokens.SpendCoins(Cost))
		{
			unitEntityData.Descriptor.AddFact(blueprintUnitFact);
			if (RecordForRespec)
			{
				ForbiddenCheatRecord.RecordPurchase(unitEntityData, blueprintUnitFact);
			}
			DivineTokens.Log(CosmicStoreText.BoonAcquired, blueprintUnitFact.Name);
		}
	}
}
