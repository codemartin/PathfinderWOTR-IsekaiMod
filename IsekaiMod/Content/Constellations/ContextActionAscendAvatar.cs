using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Constellations
{
	public class ContextActionAscendAvatar : ContextAction
	{
		public int Cost = 10000;

		public BlueprintFeatureReference m_AvatarFeature;

		public override string GetCaption()
		{
			return $"Ascends as Avatar for {Cost} Cosmic Coins";
		}

		public override void RunAction()
		{
			UnitEntityData player = DivineTokens.GetPlayer();
			BlueprintFeature avatarFeat = m_AvatarFeature?.Get();
			if (player == null || avatarFeat == null)
			{
				return;
			}
			if (player.Descriptor.Progression.Features.HasFact(avatarFeat))
			{
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage("<color=#DC143C>[Cosmic Ascension]</color> You already embody this Avatar form!");
				});
			}
			else if (DivineTokens.SpendCoins(Cost))
			{
				player.Descriptor.Progression.Features.AddFeature(avatarFeat);
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage("<color=#F5C542><b>[AVATAR ASCENSION!]</b></color> The skies split with divine brilliance as you ascend as the <b>" + avatarFeat.Name + "</b>!");
				});
			}
		}
	}
}
