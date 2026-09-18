using System;
using System.Collections.Generic;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Constellations
{
	public class ContextActionSwitchDeity : ContextAction
	{
		public BlueprintFeatureReference m_NewDeity;

		public override string GetCaption()
		{
			return "Switches player's patron deity";
		}

		public override void RunAction()
		{
			UnitEntityData player = DivineTokens.GetPlayer();
			BlueprintFeature newDeity = m_NewDeity?.Get();
			if (player == null || newDeity == null)
			{
				return;
			}
			if (DeityAvatarAscension.HasAnyAvatar(player))
			{
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage("<color=#DC143C>[Cosmic Ascension]</color> You embody a divine Avatar! Your soul covenant is eternally sealed for this cycle.");
				});
				return;
			}
			List<BlueprintFeature> list = new List<BlueprintFeature>();
			foreach (Feature feature in player.Descriptor.Progression.Features)
			{
				BlueprintFeature blueprint = feature.Blueprint;
				if (blueprint.Groups != null && Array.IndexOf(blueprint.Groups, FeatureGroup.Deities) >= 0)
				{
					list.Add(blueprint);
				}
			}
			foreach (BlueprintFeature item in list)
			{
				player.Descriptor.Progression.Features.RemoveFact(item);
			}
			player.Descriptor.Progression.Features.AddFeature(newDeity);
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage("<color=#F5C542><b>[Deity Fealty Changed]</b></color> You have aligned your soul with <b>" + newDeity.Name + "</b>!");
			});
		}
	}
}
