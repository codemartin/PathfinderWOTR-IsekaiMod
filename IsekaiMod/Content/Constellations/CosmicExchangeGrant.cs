using System;
using System.Collections.Generic;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using Kingmaker;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Content.Constellations;

internal static class CosmicExchangeGrant
{
	private sealed class ExchangeGrantHandler : IAreaHandler, ISubscriber, IGlobalSubscriber, IUnitChangedAfterRespecHandler
	{
		public void OnAreaDidLoad()
		{
			GrantMissing();
		}

		public void OnAreaBeginUnloading()
		{
		}

		public void HandleUnitChangedAfterRespec(UnitEntityData unit)
		{
			GrantMissing();
		}
	}

	private static bool s_subscribed;

	internal static bool ShouldGrant(bool constellationsEnabled, bool hasIsekaiLevels, bool hasExchange)
	{
		if (constellationsEnabled & hasIsekaiLevels)
		{
			return !hasExchange;
		}
		return false;
	}

	internal static void GrantMissing()
	{
		try
		{
			if (!ConstellationSettings.Enabled)
			{
				return;
			}
			BlueprintFeature cosmicSponsorshipExchangeFeature = DivineSponsorshipStore.CosmicSponsorshipExchangeFeature;
			BlueprintCharacterClass blueprintCharacterClass = IsekaiProtagonistClass.Get();
			if (cosmicSponsorshipExchangeFeature == null || blueprintCharacterClass == null)
			{
				return;
			}
			List<UnitEntityData> list = Game.Instance?.Player?.AllCharacters;
			if (list == null)
			{
				return;
			}
			foreach (UnitEntityData item in list)
			{
				UnitProgressionData unitProgressionData = item?.Descriptor?.Progression;
				if (unitProgressionData != null)
				{
					bool hasIsekaiLevels = unitProgressionData.GetClassData(blueprintCharacterClass) != null;
					bool hasExchange = unitProgressionData.Features.HasFact(cosmicSponsorshipExchangeFeature);
					if (ShouldGrant(constellationsEnabled: true, hasIsekaiLevels, hasExchange))
					{
						unitProgressionData.Features.AddFeature(cosmicSponsorshipExchangeFeature);
						Main.IsekaiContext.Logger.Log("Constellations: gave " + item.CharacterName + " the Cosmic Sponsorship Exchange they took Isekai level 1 without.");
					}
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error granting the Cosmic Sponsorship Exchange: " + ex);
		}
	}

	public static void Init()
	{
		if (!s_subscribed)
		{
			s_subscribed = true;
			EventBus.Subscribe(new ExchangeGrantHandler());
		}
	}
}
