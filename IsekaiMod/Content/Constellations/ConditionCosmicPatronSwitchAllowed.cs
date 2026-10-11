using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;

namespace IsekaiMod.Content.Constellations;

[TypeId("69e90fd11ed54784bb028ffeff852ee4")]
public class ConditionCosmicPatronSwitchAllowed : IsekaiCondition
{
	public BlueprintFeatureReference m_Deity;

	protected override bool CheckSafely()
	{
		UnitEntityData player = DivineTokens.GetPlayer();
		BlueprintFeature blueprintFeature = m_Deity?.Get();
		if (player != null && blueprintFeature != null && CosmicPatronSwitch.AllowedNow(out var _))
		{
			return DivineSponsorshipStore.PatronSwitchBlock(player, blueprintFeature) == DivineSponsorshipStore.PatronSwitchBlocker.None;
		}
		return false;
	}

	public override string GetConditionCaption()
	{
		return "Following this patron would not break the main character's class, and the patron cooldown has elapsed";
	}
}
