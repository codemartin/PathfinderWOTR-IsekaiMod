using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class ParallelMetamagicResonance
	{
		private static readonly Sprite Icon_Resonance = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ef7ece7bb5bb66a41b256976b27f424e"))?.m_Icon ?? ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("6e109d21da9e1c44fb772a9eca2cafdd"))?.m_Icon;

		public static void Add()
		{
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "ParallelMetamagicResonanceFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Parallel Metamagic Resonance");
				bp.SetDescription(Main.IsekaiContext, "By synchronizing multi-threaded thought acceleration (Parallel Processing) with automated spell matrix optimization (Auto Metamagic), your spell calculations achieve perfect resonance. Spells cast with automated adjustments weave seamlessly into your simultaneous action stream.\nBenefit: You gain a +2 bonus to Caster Level and a +1 bonus to the Difficulty Class (DC) of all spells you cast. Additionally, your damaging spells resonate with amplified force, treating damage dice smaller than d10 as d10 (or dealing +1 additional point of damage per die rolled if already d10 or greater).\nRequires: Character Level 5, Parallel Processing, and any Auto Metamagic ability.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Resonance;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(IncreaseCasterLevel c)
				{
					c.Value = 2;
				});
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 1;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
				bp.AddComponent(delegate(PromoteSpellDices c)
				{
					c.MinDice = DiceType.D10;
					c.Bonus = 1;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 5;
				});
				bp.AddComponent(delegate(PrerequisiteFeature c)
				{
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "ParallelProcessingFeature");
				});
				bp.AddComponent(delegate(PrerequisiteFeaturesFromList c)
				{
					c.m_Features = new BlueprintFeatureReference[8]
					{
						BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "AutoSelectiveFeature"),
						BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "AutoExtendFeature"),
						BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "AutoReachFeature"),
						BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "AutoBolsterFeature"),
						BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "AutoEmpowerFeature"),
						BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "AutoMaximizeFeature"),
						BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "AutoQuickenFeature"),
						BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "CosmicAutoQuickenFeature")
					};
					c.Amount = 1;
				});
			}));
		}
	}
}
