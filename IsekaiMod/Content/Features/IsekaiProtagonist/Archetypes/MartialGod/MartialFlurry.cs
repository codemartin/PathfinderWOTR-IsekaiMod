using IsekaiMod.Components;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod
{
	internal class MartialFlurry
	{
		private static readonly Sprite Icon_Extra_Strike = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("3e1a13fdca87e9c49b2fac4556e5a948"))?.m_Icon;

		public static void Add()
		{
			SecretPowerSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "MartialFlurry", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Martial Flurry");
				bp.SetDescription(Main.IsekaiContext, "Through supreme circulation of sovereign Ki, the Martial God strikes with overwhelming cadence, delivering an extra attack on a full attack action. This also applies to off-hand weapons or dual unarmed strikes if you are wielding a second weapon.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Extra_Strike;
				bp.AddComponent(delegate(AddExtraAttack c)
				{
					c.Number = 1;
				});
				bp.AddComponent(delegate(AddExtraOffHandAttack c)
				{
					c.Number = 1;
				});
				bp.Ranks = 20;
			}));
		}
	}
}
