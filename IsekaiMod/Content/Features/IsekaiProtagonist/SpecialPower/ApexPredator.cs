using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class ApexPredator
	{
		private static readonly Sprite Icon_Aspect = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("df4f34f7cac73ab40986bc33f87b1a3c")).m_Icon;

		public static void Add()
		{
			SpecialPowerSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "ApexPredator", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Special Power - Apex Predator");
				bp.SetDescription(Main.IsekaiContext, "You have claimed your rightful seat atop the food chain. \nBenefit: You gain a +4 competence bonus on all attack rolls with natural weapons (bites, claws, gores) and critical rolls with natural weapons automatically attempt a free Trip combat maneuver.\nRequires character level 5.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Aspect;
				bp.AddComponent(delegate(WeaponGroupAttackBonus c)
				{
					c.WeaponGroup = WeaponFighterGroup.Natural;
					c.AttackBonus = 4;
					c.Descriptor = ModifierDescriptor.Competence;
				});
				bp.AddComponent(delegate(AddInitiatorAttackWithWeaponTrigger c)
				{
					c.OnlyHit = true;
					c.CriticalHit = true;
					c.AllNaturalAndUnarmed = true;
					c.Action = ActionFlow.DoSingle(delegate(ContextActionCombatManeuver m)
					{
						m.Type = CombatManeuver.Trip;
						m.OnSuccess = ActionFlow.DoNothing();
					});
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 5;
				});
			}));
		}
	}
}
