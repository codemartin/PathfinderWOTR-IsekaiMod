using IsekaiMod.Components;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.PhantomBlade;

internal class PhantomFlurry
{
	private static readonly Sprite Icon_Extra_Strike = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("3e1a13fdca87e9c49b2fac4556e5a948"))?.m_Icon;

	public static void Add()
	{
		BlueprintFeature PhantomFlurry = Helpers.CreateBlueprint(Main.IsekaiContext, "PhantomFlurry", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Phantom Flurry");
			bp.SetDescription(Main.IsekaiContext, "Through supreme velocity, the Martial God delivers an extra attack at their full base attack bonus on a full attack action, and an extra off-hand attack at full base attack bonus when wielding a second weapon. The Martial God gains a rank of Phantom Flurry at 5th, 10th, 15th and 20th level. The second rank adds another attack at full base attack bonus with each weapon. The third and fourth ranks each add one more attack with each weapon, made after your other attacks with that weapon at 5 less than the attack before it, as Improved and Greater Two-Weapon Fighting do. Further ranks add nothing.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Extra_Strike;
			bp.AddComponent<PhantomFlurryAttacks>();
			bp.Ranks = 4;
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "ExtraStrike", delegate(BlueprintFeature bp)
		{
			bp.SetName(((BlueprintUnitFact)PhantomFlurry).m_DisplayName);
			bp.SetDescription(((BlueprintUnitFact)PhantomFlurry).m_Description);
			((BlueprintUnitFact)bp).m_Icon = Icon_Extra_Strike;
			bp.ComponentsArray = PhantomFlurry.ComponentsArray;
			bp.Ranks = 4;
		});
		SecretPowerSelection.AddToSelection(PhantomFlurry);
	}
}
