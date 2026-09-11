using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class CreationMagic
	{
		private static readonly Sprite Icon_Forge = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_DEUS_EX_MACHINA.png");

		public static void Add()
		{
			OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "CreationMagicFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Creation Magic");
				bp.SetDescription(Main.IsekaiContext, "Through mastery of conceptual creation magic, you materialize supreme weaponry, armor, and endless munitions from pure astral mana.\nBenefit: All weapon attacks deal an additional +6 physical force damage and bypass all enemy damage reduction. You gain a +3 enhancement bonus to attack rolls, a +4 armor bonus to AC, and DR 10/Adamantine.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Forge;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Armor;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 10;
					c.BypassedByMaterial = true;
					c.Material = PhysicalDamageMaterial.Adamantite;
				});
				bp.AddComponent<IgnoreDamageReductionOnAttack>();
			}));
		}
	}
}
