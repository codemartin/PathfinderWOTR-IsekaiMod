using System;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Narrative.Actions
{
	public class ContextActionGiveOtherworlderRewards : ContextAction
	{
		public int Gold;

		public int Coins;

		public string Sponsor;

		public string[] ItemGuids;

		public BlueprintItem[] DirectItems;

		public BlueprintBuff BuffToApply;

		public TimeSpan BuffDuration = TimeSpan.Zero;

		public BlueprintFeature FeatureToGrant;

		public string BannerMessage;

		public override string GetCaption()
		{
			return "Awards Otherworlder gold, coins, items, buffs, and combat log recognition";
		}

		public override void RunAction()
		{
			try
			{
				Player player = Game.Instance?.Player;
				if (player != null)
				{
					if (Gold > 0)
					{
						player.GainMoney(Gold);
					}
					if (ItemGuids != null)
					{
						string[] itemGuids = ItemGuids;
						foreach (string text in itemGuids)
						{
							if (!string.IsNullOrEmpty(text))
							{
								BlueprintItem blueprint = BlueprintTools.GetBlueprint<BlueprintItem>(text);
								if (blueprint != null)
								{
									player.Inventory.Add(blueprint, 1);
								}
							}
						}
					}
					if (DirectItems != null)
					{
						BlueprintItem[] directItems = DirectItems;
						foreach (BlueprintItem blueprintItem in directItems)
						{
							if (blueprintItem != null)
							{
								player.Inventory.Add(blueprintItem, 1);
							}
						}
					}
				}
				UnitEntityData unitEntityData = BlueprintSafetyExtensions.SafeGetMainCharacter();
				if (unitEntityData != null)
				{
					if (BuffToApply != null)
					{
						if (BuffDuration > TimeSpan.Zero)
						{
							unitEntityData.Descriptor.Buffs.AddBuff(BuffToApply, unitEntityData, BuffDuration);
						}
						else
						{
							unitEntityData.Descriptor.Buffs.AddBuff(BuffToApply, unitEntityData, null);
						}
					}
					if (FeatureToGrant != null && !unitEntityData.Descriptor.Progression.Features.HasFact(FeatureToGrant))
					{
						unitEntityData.Descriptor.Progression.Features.AddFeature(FeatureToGrant);
					}
				}
				if (Coins > 0 && !string.IsNullOrEmpty(Sponsor))
				{
					DivineTokens.AddCoins(Coins, Sponsor);
				}
				if (!string.IsNullOrEmpty(BannerMessage))
				{
					EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
					{
						h.HandleLogMessage(BannerMessage);
					});
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ContextActionGiveOtherworlderRewards: " + ex);
			}
		}
	}
}
