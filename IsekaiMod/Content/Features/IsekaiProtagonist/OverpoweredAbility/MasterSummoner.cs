using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class MasterSummoner
	{
		public static void Add()
		{
			Sprite icon = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("7f4b66a2b1fdab142904a263c7866d46")).m_Icon;
			BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleBuffFeature("MasterSummoner", "Overpowered Ability - Master Summoner", "Each time you cast a summoning {g|Encyclopedia:Spell}spell{/g}, summon up to 2 additional creatures (scaling to 3 additional creatures at level 15) based on your highest mental attribute modifier.", icon, delegate(BlueprintBuff bp)
			{
				bp.AddComponent<ExtraSummonCount>();
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 5;
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
		}
	}
}
