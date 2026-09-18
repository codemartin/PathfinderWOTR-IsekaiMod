using IsekaiMod.Content.Classes.IsekaiProtagonist;
using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers.Rest;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Kingdom;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor
{
	[TypeId("5d2c88f1239a48be991572c842918bc3")]
	public class ImperialTitheComponent : UnitFactComponentDelegate, IRestFinishedHandler, ISubscriber, IGlobalSubscriber, IKingdomDayHandler
	{
		public void HandleRestFinished(RestStatus status)
		{
			if (status != null && status.RestSucceeded)
			{
				CollectTithe();
			}
		}

		public void OnNewDay()
		{
		}

		private void CollectTithe()
		{
			UnitEntityData owner = base.Owner;
			if (!(owner == null) && owner.IsMainCharacter)
			{
				int num = owner.Progression.GetClassLevel(IsekaiProtagonistClass.Get());
				if (num <= 0)
				{
					num = owner.Progression.CharacterLevel;
				}
				int gold = 2500;
				int finance = 100;
				int materials = 0;
				int favors = 0;
				if (num >= 20)
				{
					gold = 100000;
					finance = 10000;
					materials = 2000;
					favors = 1000;
				}
				else if (num >= 15)
				{
					gold = 50000;
					finance = 3000;
					materials = 1000;
					favors = 500;
				}
				else if (num >= 10)
				{
					gold = 25000;
					finance = 1500;
					materials = 500;
					favors = 200;
				}
				else if (num >= 5)
				{
					gold = 10000;
					finance = 500;
					materials = 100;
					favors = 50;
				}
				Game.Instance?.Player?.GainMoney(gold);
				try
				{
					(Game.Instance?.Player?.Kingdom)?.GainResource(KingdomResourcesAmount.Create(finance, materials, favors));
				}
				catch
				{
				}
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage($"<color=#FFD700>[Imperial Tithe]</color> Mortals pay devotional tribute to the living God-Emperor! Received {gold:N0} gold and {finance:N0} Crusade finances.");
				});
			}
		}
	}
}
