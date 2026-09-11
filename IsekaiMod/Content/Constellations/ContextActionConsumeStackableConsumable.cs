using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Constellations
{
	public class ContextActionConsumeStackableConsumable : ContextAction
	{
		public BlueprintFeatureReference m_Feature;

		public string ItemName;

		public int ValuePerRank = 2;

		public string StatDisplayName = "Stat";

		public override string GetCaption()
		{
			return "Consumes " + ItemName + " and increases " + StatDisplayName;
		}

		public override void RunAction()
		{
			UnitEntityData target = base.Target.Unit ?? Game.Instance?.Player?.SafeGetMainCharacter();
			BlueprintFeature blueprintFeature = m_Feature?.Get();
			if (!(target == null) && blueprintFeature != null)
			{
				Feature fact = target.Descriptor.Progression.Features.GetFact(blueprintFeature);
				if (fact != null)
				{
					fact.AddRank();
				}
				else
				{
					target.Descriptor.Progression.Features.AddFeature(blueprintFeature);
				}
				int currentRank = target.Descriptor.Progression.Features.GetRank(blueprintFeature);
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage($"<color=#FFD700>[Cosmic Consumable]</color> {target.CharacterName} consumed <b>{ItemName}</b>! {StatDisplayName} increased by +{ValuePerRank}! (Total untyped bonus: +{currentRank * ValuePerRank})");
				});
			}
		}
	}
}
