using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Content.Constellations;

[TypeId("f2d24a98079b4f07b3d5747890ba7c07")]
public class ContextActionSwitchDeity : IsekaiContextAction
{
	public BlueprintFeatureReference m_NewDeity;

	public int Cost = 500;

	public override string GetCaption()
	{
		return $"Switches player's patron deity for {Cost} Cosmic Coins";
	}

	protected override void RunSafely()
	{
		UnitEntityData player = DivineTokens.GetPlayer();
		BlueprintFeature blueprintFeature = m_NewDeity?.Get();
		if (player == null || blueprintFeature == null)
		{
			return;
		}
		if (!CosmicPatronSwitch.AllowedNow(out var daysRemaining))
		{
			DivineTokens.Log(CosmicStoreText.PatronSwitchCooldown, daysRemaining);
			return;
		}
		DivineSponsorshipStore.PatronSwitchBlocker patronSwitchBlocker = DivineSponsorshipStore.PatronSwitchBlock(player, blueprintFeature);
		if (patronSwitchBlocker != DivineSponsorshipStore.PatronSwitchBlocker.None)
		{
			DivineTokens.Log((patronSwitchBlocker == DivineSponsorshipStore.PatronSwitchBlocker.ForbidsSpellbook) ? CosmicStoreText.PatronForbidsSpellbook : CosmicStoreText.PatronRefusesCharacter, blueprintFeature.Name);
		}
		else
		{
			if (!DivineTokens.SpendCoins(Cost))
			{
				return;
			}
			CosmicPatronSwitch.RecordSwitch();
			List<BlueprintFeature> list = new List<BlueprintFeature>();
			List<BlueprintFeature> list2 = new List<BlueprintFeature>();
			List<string> list3 = DeityAvatarAscension.AvatarsLostOnPatronSwitch(from b in DivineSponsorshipStore.OwnedFeatures(player)
				select b.name);
			foreach (Feature feature in player.Descriptor.Progression.Features)
			{
				BlueprintFeature blueprint = feature.Blueprint;
				if (blueprint != null)
				{
					if (ConstellationChatManager.IsChosenDeityFeature(blueprint))
					{
						list.Add(blueprint);
					}
					else if (list3.Contains(blueprint.name))
					{
						list2.Add(blueprint);
					}
				}
			}
			foreach (BlueprintFeature item in list.Concat(list2))
			{
				player.Descriptor.Progression.Features.RemoveFact(item);
			}
			player.Descriptor.Progression.Features.AddFeature(blueprintFeature);
			DivineTokens.Log(CosmicStoreText.PatronChanged, blueprintFeature.Name);
			foreach (BlueprintFeature item2 in list2)
			{
				DivineTokens.Log(CosmicStoreText.AvatarLost, item2.Name);
			}
		}
	}
}
