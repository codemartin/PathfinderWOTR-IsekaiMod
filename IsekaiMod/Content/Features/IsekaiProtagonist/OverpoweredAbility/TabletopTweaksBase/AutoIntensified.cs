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
	internal class AutoIntensified
	{
		private static readonly Sprite Icon_IntensifiedSpell = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("b77d7b23bf6d46a5bf80da7ca9674e83"))?.m_Icon;

		public static void Add()
		{
			AutoMetamagicSelection.AddToSelection(TTCoreExtensions.CreateToggleBuffFeature("AutoIntensified", "Overpowered Ability - Auto Intensified", "Every time you cast a spell, increase its maximum damage dice by 5, as though using the Intensified Spell feat.", Icon_IntensifiedSpell, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AutoMetamagic c)
				{
					c.m_AllowedAbilities = AutoMetamagic.AllowedType.SpellOnly;
					c.Metamagic = (Metamagic)4096;
				});
			}));
		}
	}
}
