using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Content.Constellations;

[TypeId("4b1f6f0e2d6a4c1f9a3b7c5d8e2f1a60")]
public class ContextActionConsumeStackableConsumable : IsekaiContextAction
{
	public BlueprintFeatureReference m_Feature;

	public string ItemName;

	public string EffectPerBottle;

	public override string GetCaption()
	{
		return "Consumes " + ItemName + ": " + EffectPerBottle;
	}

	protected override void RunSafely()
	{
		UnitEntityData target = base.Target?.Unit ?? Game.Instance?.Player?.MainCharacter.Value;
		BlueprintFeature blueprintFeature = m_Feature?.Get();
		if (!(target == null) && blueprintFeature != null)
		{
			int bottles = AddOneBottle(target, blueprintFeature);
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(ConstellationText.LocalizedFormat("Constellation.CosmicConsumable.Drank", "<color=#5B2A86>[Cosmic Consumable]</color> {0} drank <b>{1}</b>: {2}. Bottles drunk: {3}.", target.CharacterName, ConstellationText.LocalizeIfKnown(ItemName), ConstellationText.LocalizeIfKnown(EffectPerBottle), bottles));
			});
		}
	}

	internal static int AddOneBottle(UnitEntityData unit, BlueprintFeature feature)
	{
		Feature fact = unit.Descriptor.Progression.Features.GetFact(feature);
		if (fact != null)
		{
			fact.AddRank();
		}
		else
		{
			unit.Descriptor.Progression.Features.AddFeature(feature);
		}
		return unit.Descriptor.Progression.Features.GetRank(feature);
	}
}
