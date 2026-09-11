using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class InfiniteSpace
	{
		public static void Add()
		{
			Sprite Icon_InfiniteSpace = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_INFINITE_SPACE.png");
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "InfiniteSpaceFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Infinite Space");
				bp.SetDescription(Main.IsekaiContext, "\"Greetings, esteemed commander. Have you ever grown weary of the meager limitations of standard extradimensional storage? Tired of leaving behind priceless relics, siege engines, or entire armories due to mundane encumbrance? With the Interdimensional Pocket 9000, mundane physics no longer constrain your crusade. Utilizing advanced Numerian dimensional fold technologies combined with Alkenstarian clockwork precision, the Interdimensional Pocket 9000 possesses an absolute storage capacity of infinite tons.\nCrafted from Nahyndrian crystalline weave and dragonhide nanofibers, it endures the harshest planar climates while seamlessly expanding your personal inventory.\nBenefit: You and your party have an infinite carrying capacity.\"");
				((BlueprintUnitFact)bp).m_Icon = Icon_InfiniteSpace;
				bp.AddComponent(delegate(AddPartyEncumbrance c)
				{
					c.Value = 1000000;
				});
			}));
		}
	}
}
