using IsekaiMod.Components;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.MartialGod
{
	internal class MartialTranscendence
	{
		private static readonly Sprite Icon_SuperiorReflexes = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("b89373001e05f1f4aa9b9bb4f420c40f"))?.m_Icon;

		private static readonly BlueprintFeature ImprovedUnarmedStrike = BlueprintTools.GetBlueprint<BlueprintFeature>("7812ad3672a4b9a4fb894ea402095167");

		public static BlueprintFeature Feature { get; private set; }

		public static void Add()
		{
			Feature = Helpers.CreateBlueprint(Main.IsekaiContext, "MartialTranscendenceFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Martial Transcendence");
				bp.SetDescription(Main.IsekaiContext, "The Martial God dissolves the duality between raw brute power and refined supreme agility. Whether fighting with bare fists, legendary swords, polearms, or bows, you use the HIGHER of your Strength or Dexterity modifier for attack rolls, damage rolls, and Combat Maneuver Bonus (CMB).\n\nFurthermore, you can qualify for any feat using either your Strength or Dexterity, whichever is higher, and you gain Improved Unarmed Strike as a bonus feat.");
				((BlueprintUnitFact)bp).m_Icon = Icon_SuperiorReflexes;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AttackStatReplacement c)
				{
					c.ReplacementStat = StatType.Dexterity;
					c.CheckWeaponTypes = false;
				});
				bp.AddComponent(delegate(AnyWeaponDamageStatReplacementFixed c)
				{
					c.Stat = StatType.Dexterity;
				});
				bp.AddComponent(delegate(ReplaceCombatManeuverStatIfHigher c)
				{
					c.StatType = StatType.Dexterity;
				});
				bp.AddComponent(delegate(ReplaceStatForPrerequisites c)
				{
					c.OldStat = StatType.Strength;
					c.NewStat = StatType.Dexterity;
					c.Policy = ReplaceStatForPrerequisites.StatReplacementPolicy.NewStat;
				});
				bp.AddComponent(delegate(ReplaceStatForPrerequisites c)
				{
					c.OldStat = StatType.Dexterity;
					c.NewStat = StatType.Strength;
					c.Policy = ReplaceStatForPrerequisites.StatReplacementPolicy.NewStat;
				});
				if (ImprovedUnarmedStrike != null)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { ImprovedUnarmedStrike.ToReference<BlueprintUnitFactReference>() };
					});
				}
			});
		}
	}
}
