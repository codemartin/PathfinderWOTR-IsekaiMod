using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Prerequisites;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class SneakyMagic
	{
		private static readonly Sprite Icon_InvisibilityAlmostGreater = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("8dcb9c02148a704489948eaf84ab04bf")).m_Icon;

		public static void Add()
		{
			SpecialPowerSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "SneakyMagic", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Sneaky Magic");
				bp.SetDescription(Main.IsekaiContext, "You can add your sneak {g|Encyclopedia:Attack}attack{/g} {g|Encyclopedia:Damage}damage{/g} to any {g|Encyclopedia:Spell}spell{/g} that deals damage, if the targets are {g|Encyclopedia:Flat_Footed}flat-footed{/g}. This additional damage only applies to spells that deal hit point damage, and the additional damage is of the same type as the spell. If the spell allows a {g|Encyclopedia:Saving_Throw}saving throw{/g} to negate or halve the damage, it also negates or halves the sneak attack damage.");
				((BlueprintUnitFact)bp).m_Icon = Icon_InvisibilityAlmostGreater;
				bp.AddComponent<SurpriseSpells>();
				bp.AddComponent(delegate(PrerequisiteFullStatValue c)
				{
					c.Stat = StatType.SneakAttack;
					c.Value = 1;
				});
			}));
		}
	}
}
