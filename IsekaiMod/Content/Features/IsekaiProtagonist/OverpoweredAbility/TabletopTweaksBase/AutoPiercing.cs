using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility.TabletopTweaksBase
{
	internal class AutoPiercing
	{
		private static readonly Sprite Icon_PiercingSpell = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("fc6bc9b9c2e54bbba96e074070d3c5be"))?.m_Icon;

		public static void Add()
		{
			AutoMetamagicSelection.AddToSelection(TTCoreExtensions.CreateToggleBuffFeature("AutoPiercing", "Overpowered Ability - Auto Piercing", "Every time you cast a spell against a target, it treats the spell resistance of the target as 5 lower than its actual SR, as though using the Piercing Spell feat.", Icon_PiercingSpell, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AutoMetamagic c)
				{
					c.m_AllowedAbilities = AutoMetamagic.AllowedType.SpellOnly;
					c.Metamagic = (Metamagic)524288;
				});
			}));
		}
	}
}
