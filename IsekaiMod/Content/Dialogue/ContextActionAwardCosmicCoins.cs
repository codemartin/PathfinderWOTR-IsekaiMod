using IsekaiMod.Content.Constellations;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Dialogue
{
	public class ContextActionAwardCosmicCoins : ContextAction
	{
		public int Amount = 250;

		public string Sponsor = "The Laughing King";

		public override string GetCaption()
		{
			return $"Awards {Amount} Cosmic Coins from {Sponsor}";
		}

		public override void RunAction()
		{
			DivineTokens.AddCoins(Amount, Sponsor);
		}
	}
}
