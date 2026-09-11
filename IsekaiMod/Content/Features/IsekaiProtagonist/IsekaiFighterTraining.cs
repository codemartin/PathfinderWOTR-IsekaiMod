using IsekaiMod.Content.Classes.IsekaiProtagonist;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal class IsekaiFighterTraining
	{
		private static readonly Sprite Icon_SwordSaintFighterTraining = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9ab2ec65977cc524a99600babc7fe3b6")).m_Icon;

		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiFighterTraining", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Fighter Training");
				bp.SetDescription(Main.IsekaiContext, "At 3rd level, you count your class level as your fighter level for the purpose of qualifying for {g|Encyclopedia:Feat}feats{/g}. If you have levels in fighter, these levels stack. If you have a martial legacy, the fighter level is doubled.");
				((BlueprintUnitFact)bp).m_Icon = Icon_SwordSaintFighterTraining;
				bp.AddComponent(delegate(ClassLevelsForPrerequisites c)
				{
					c.m_FakeClass = ClassTools.Classes.FighterClass.ToReference<BlueprintCharacterClassReference>();
					c.m_ActualClass = IsekaiProtagonistClass.GetReference();
					c.Modifier = 1.0;
				});
			});
		}
	}
}
