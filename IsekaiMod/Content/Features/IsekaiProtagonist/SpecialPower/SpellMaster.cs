using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class SpellMaster
	{
		private static readonly Sprite Icon_BatteringBlast = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("0a2f7c6aa81bc6548ac7780d8b70bcbc")).m_Icon;

		public static void Add()
		{
			SpecialPowerSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "SpellMaster", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Spell Master");
				bp.SetDescription(Main.IsekaiContext, "The DC of spells you cast increases by 2.\nRequires character level 5.");
				((BlueprintUnitFact)bp).m_Icon = Icon_BatteringBlast;
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Value = 2;
					c.SpellsOnly = true;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 5;
				});
			}));
		}
	}
}
