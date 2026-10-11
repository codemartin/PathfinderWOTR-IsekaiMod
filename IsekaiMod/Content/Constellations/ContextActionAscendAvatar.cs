using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;

namespace IsekaiMod.Content.Constellations;

[TypeId("46e2fbaa0e214b129b003a04f233ff3d")]
public class ContextActionAscendAvatar : IsekaiContextAction
{
	public int Cost = 10000;

	public BlueprintFeatureReference m_AvatarFeature;

	public override string GetCaption()
	{
		return $"Ascends as Avatar for {Cost} Cosmic Coins";
	}

	protected override void RunSafely()
	{
		UnitEntityData player = DivineTokens.GetPlayer();
		BlueprintFeature blueprintFeature = m_AvatarFeature?.Get();
		if (player == null || blueprintFeature == null)
		{
			return;
		}
		List<BlueprintFeature> source = DivineSponsorshipStore.OwnedFeatures(player);
		string heldName;
		switch (DeityAvatarAscension.CheckAscend(source.Select((BlueprintFeature b) => b.name), blueprintFeature.name, out heldName))
		{
		case DeityAvatarAscension.AscendRefusal.AlreadyEmbodied:
			DivineTokens.Log(CosmicStoreText.AlreadyEmbodied, blueprintFeature.Name);
			return;
		case DeityAvatarAscension.AscendRefusal.OtherAvatarHeld:
		{
			BlueprintFeature blueprintFeature2 = source.FirstOrDefault((BlueprintFeature b) => b.name == heldName);
			DivineTokens.Log(CosmicStoreText.OtherAvatarHeld, (blueprintFeature2 != null) ? blueprintFeature2.Name : heldName);
			return;
		}
		}
		if (DivineTokens.SpendCoins(Cost))
		{
			player.Descriptor.Progression.Features.AddFeature(blueprintFeature);
			DivineTokens.Log(CosmicStoreText.Ascended, blueprintFeature.Name);
		}
	}
}
