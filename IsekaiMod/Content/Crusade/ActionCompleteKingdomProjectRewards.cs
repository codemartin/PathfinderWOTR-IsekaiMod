using System;
using IsekaiMod.Content.Constellations;
using Kingmaker;
using Kingmaker.ElementsSystem;
using Kingmaker.Kingdom;
using Kingmaker.PubSubSystem;

namespace IsekaiMod.Content.Crusade
{
	public class ActionCompleteKingdomProjectRewards : GameAction
	{
		public int Finances;

		public int Materials;

		public int CosmicCoins;

		public int PersonalGold;

		public string LogMessageText;

		public override string GetCaption()
		{
			return $"Complete Project Rewards: {Finances} Fin, {Materials} Mat, {CosmicCoins} Coins, {PersonalGold} Gold";
		}

		public override void RunAction()
		{
			try
			{
				KingdomState instance = KingdomState.Instance;
				if (instance != null && (Finances > 0 || Materials > 0))
				{
					instance.GainResource(new KingdomResourcesAmount
					{
						m_Finances = Finances,
						m_Materials = Materials
					});
				}
				if (PersonalGold > 0)
				{
					Game.Instance?.Player?.GainMoney(PersonalGold);
				}
				if (CosmicCoins > 0)
				{
					DivineTokens.AddCoins(CosmicCoins, "Crusade Decree / Treaty Ratification");
				}
				if (!string.IsNullOrEmpty(LogMessageText))
				{
					EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
					{
						h.HandleLogMessage(LogMessageText);
					});
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in ActionCompleteKingdomProjectRewards: " + ex);
			}
		}
	}
}
