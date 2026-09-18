using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Constellations
{
	public class ContextActionSpendCoinsAndGrantFact : ContextAction
	{
		public int Cost;

		public BlueprintUnitFactReference m_Fact;

		public BlueprintFeatureReference m_KeyOfInfinitePathways;

		public AlignmentMaskType AllowedAlignment = AlignmentMaskType.Any;

		public override string GetCaption()
		{
			return $"Spends {Cost} Cosmic Coins and grants permanent fact";
		}

		public override void RunAction()
		{
			UnitEntityData unitEntityData = base.Target?.Unit ?? DivineTokens.GetPlayer();
			BlueprintUnitFact fact = m_Fact?.Get();
			if (unitEntityData == null || fact == null)
			{
				return;
			}
			if (unitEntityData.Descriptor.Progression.Features.HasFact(fact))
			{
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage("<color=#DC143C>[Cosmic Store]</color> You already possess this permanent cosmic boon!");
				});
			}
			else if ((m_KeyOfInfinitePathways == null || !unitEntityData.Descriptor.Progression.Features.HasFact(m_KeyOfInfinitePathways.Get())) && AllowedAlignment != AlignmentMaskType.Any && (AllowedAlignment & unitEntityData.Descriptor.Alignment.ValueRaw.ToMask()) == 0)
			{
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage("<color=#DC143C>[Cosmic Store]</color> Your alignment does not align with this mythic path! (Requires Key of Infinite Pathways to bypass)");
				});
			}
			else if (DivineTokens.SpendCoins(Cost))
			{
				unitEntityData.Descriptor.AddFact(fact);
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage("<color=#FFD700>[Cosmic Store]</color> Acquired permanent boon: " + fact.Name + "!");
				});
			}
			else
			{
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage($"<color=#DC143C>[Cosmic Store]</color> Insufficient Cosmic Coins! Required: {Cost}. Current: {DivineTokens.GetBalance()}.");
				});
			}
		}
	}
}
