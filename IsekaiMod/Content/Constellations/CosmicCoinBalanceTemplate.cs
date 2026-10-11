using System;
using System.Collections.Generic;
using Kingmaker.TextTools;

namespace IsekaiMod.Content.Constellations;

internal sealed class CosmicCoinBalanceTemplate : TextTemplate
{
	public override string Generate(bool capitalized, List<string> parameters)
	{
		try
		{
			return DivineSponsorshipStore.FormatCoins(DivineTokens.GetBalance());
		}
		catch (Exception)
		{
			return "0";
		}
	}
}
