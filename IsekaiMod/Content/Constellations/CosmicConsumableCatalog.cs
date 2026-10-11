namespace IsekaiMod.Content.Constellations;

public static class CosmicConsumableCatalog
{
	public sealed class Item
	{
		public readonly string Name;

		public readonly int Coins;

		public Item(string name, int coins)
		{
			Name = name;
			Coins = coins;
		}
	}

	public const int AmbrosiaStatPrice = 3000;

	public const int AmbrosiaCelerityPrice = 3000;

	public const int AmbrosiaEternalRenewalPrice = 10000;

	public const int NectarEndlessLifePrice = 1500;

	public const int NectarVoidStalkerPrice = 4000;

	public const int NectarWorldBreakerPrice = 4000;

	public const int NectarDiamondSoulPrice = 3500;

	public const int NectarUnbrokenAegisPrice = 3500;

	public const int DraughtCosmicVelocityPrice = 2500;

	public const int DraughtImmortalTitanPrice = 2500;

	public const int DraughtArcaneOmnisciencePrice = 3000;

	public const int DraughtApexPredatorPrice = 3000;

	public const int DraughtAbsoluteSoulAnchorPrice = 500;

	public const int CosmicWishstonePrice = 4000;

	public const int EnvoyStock = 99;

	public const int AmbrosiaStatPerBottle = 2;

	public const int AmbrosiaSpeedFeetPerBottle = 10;

	public const int AmbrosiaFastHealingPerBottle = 5;

	public const int NectarHitPointsPerBottle = 25;

	public const int NectarSpellDCPerBottle = 1;

	public const int NectarSpellPenetrationPerBottle = 2;

	public const int NectarAttackPerBottle = 2;

	public const int NectarDamagePerBottle = 2;

	public const int NectarSavesPerBottle = 2;

	public const int NectarArmorClassPerBottle = 1;

	public const int PermanentRanks = 999;

	public const int DraughtHours = 24;

	public const int VelocitySpeedFeet = 20;

	public const int VelocityExtraAttacks = 1;

	public const int TitanTemporaryHitPoints = 150;

	public const int TitanDamageReduction = 10;

	public const int TitanFastHealing = 10;

	public const int ArcaneCasterLevel = 4;

	public const int ArcaneSpellPenetration = 4;

	public const int ApexCriticalConfirmation = 6;

	public const int ApexCriticalMultiplier = 1;

	public const int FateRounds = 3;

	public const int RuinDamage = 350;

	public const int RuinRadiusFeet = 60;

	public const long WealthGold = 500000L;

	public static readonly string[] AkashicScrolls = new string[6] { "e19e85bfe282a1145a6075567a71970f", "5954d6c5bc2640d458423ff2ef8612f4", "a1bc9bf104a3c614391eccdcdf37d4ad", "449bf6d894ed8de4bbf148023cdf36f5", "33ac9588ff822a64c92167bdcec7df65", "28968352ed826da4e9c2af856aad7096" };

	public static readonly Item[] Items = new Item[19]
	{
		new Item("ItemPrimordialAmbrosiaMight", 3000),
		new Item("ItemPrimordialAmbrosiaGrace", 3000),
		new Item("ItemPrimordialAmbrosiaEndurance", 3000),
		new Item("ItemPrimordialAmbrosiaIntellect", 3000),
		new Item("ItemPrimordialAmbrosiaInsight", 3000),
		new Item("ItemPrimordialAmbrosiaMajesty", 3000),
		new Item("ItemPrimordialAmbrosiaCelerity", 3000),
		new Item("ItemPrimordialAmbrosiaEternalRenewal", 10000),
		new Item("ItemNectarEndlessLife", 1500),
		new Item("ItemNectarVoidStalker", 4000),
		new Item("ItemNectarWorldBreaker", 4000),
		new Item("ItemNectarDiamondSoul", 3500),
		new Item("ItemNectarUnbrokenAegis", 3500),
		new Item("ItemDraughtCosmicVelocity", 2500),
		new Item("ItemDraughtImmortalTitan", 2500),
		new Item("ItemDraughtArcaneOmniscience", 3000),
		new Item("ItemDraughtApexPredator", 3000),
		new Item("ItemDraughtAbsoluteSoulAnchor", 500),
		new Item("ItemCosmicWishstone", 4000)
	};

	public static readonly string[,] AbilityIcons = new string[24, 2]
	{
		{ "ItemPrimordialAmbrosiaMightAbility", "4c3d08935262b6544ae97599b3a9556d" },
		{ "ItemPrimordialAmbrosiaGraceAbility", "de7a025d48ad5da4991e7d3c682cf69d" },
		{ "ItemPrimordialAmbrosiaEnduranceAbility", "a900628aea19aa74aad0ece0e65d091a" },
		{ "ItemPrimordialAmbrosiaIntellectAbility", "ae4d3ad6a8fda1542acf2e9bbc13d113" },
		{ "ItemPrimordialAmbrosiaInsightAbility", "f0455c9295b53904f9e02fc571dd2ce1" },
		{ "ItemPrimordialAmbrosiaMajestyAbility", "446f7bf201dc1934f96ac0a26e324803" },
		{ "ItemPrimordialAmbrosiaCelerityAbility", "14c90900b690cac429b229efdf416127" },
		{ "ItemPrimordialAmbrosiaEternalRenewalAbility", "d52566ae8cbe8dc4dae977ef51c27d91" },
		{ "ItemNectarEndlessLifeAbility", "7a5b5bf845779a941a67251539545762" },
		{ "ItemNectarVoidStalkerAbility", "1978c3f91cfbbc24b9c9b0d017f4beec" },
		{ "ItemNectarWorldBreakerAbility", "ef16771cb05d1344989519e87f25b3c5" },
		{ "ItemNectarDiamondSoulAbility", "5ab0d42fb68c9e34abae4921822b9d63" },
		{ "ItemNectarUnbrokenAegisAbility", "183d5bb91dea3a1489a6db6c9cb64445" },
		{ "ItemDraughtCosmicVelocityAbility", "14ec7a4e52e90fa47a4c8d63c69fd5c1" },
		{ "ItemDraughtImmortalTitanAbility", "c66e86905f7606c4eaa5c774f0357b2b" },
		{ "ItemDraughtArcaneOmniscienceAbility", "16fa59cc9a72a6043b566b49184f53fe" },
		{ "ItemDraughtApexPredatorAbility", "f4201c85a991369408740c6888362e20" },
		{ "ItemDraughtAbsoluteSoulAnchorAbility", "0413915f355a38146bc6ad40cdf27b3f" },
		{ "WishstoneMasterAbility", "6a7cdeb14fc6ef44580cf639c5cdc113" },
		{ "WishstoneMiracleRestorationAbility", "a0fc99f0933d01643b2b8fe570caa4c5" },
		{ "WishstoneMiracleAbsoluteFateAbility", "a5e23522eda32dc45801e32c05dc9f96" },
		{ "WishstoneMiracleRuinAbility", "78abd9a61abf4c80a8a8cf05ff55f033" },
		{ "WishstoneMiracleWealthAbility", "6922eeb3b29e3fb488e955311bfc5cdc" },
		{ "WishstoneMiracleAkashicDuplicationAbility", "e19e85bfe282a1145a6075567a71970f" }
	};

	public static string IconSourceOf(string abilityName)
	{
		for (int i = 0; i < AbilityIcons.GetLength(0); i++)
		{
			if (AbilityIcons[i, 0] == abilityName)
			{
				return AbilityIcons[i, 1];
			}
		}
		return null;
	}
}
