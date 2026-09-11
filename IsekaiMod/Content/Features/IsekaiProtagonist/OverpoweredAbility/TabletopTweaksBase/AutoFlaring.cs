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
	internal class AutoFlaring
	{
		private static readonly Sprite Icon_FlaringSpell = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("59e1695070ef481aa958d69cb370592b"))?.m_Icon;

		public static void Add()
		{
			AutoMetamagicSelection.AddToSelection(TTCoreExtensions.CreateToggleBuffFeature("AutoFlaring", "Overpowered Ability - Auto Flaring", "Every time you cast a light, fire, or electricity spell, it causes the dazzling condition, as though using the Flaring Spell feat.", Icon_FlaringSpell, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AutoMetamagic c)
				{
					c.m_AllowedAbilities = AutoMetamagic.AllowedType.SpellOnly;
					c.Metamagic = (Metamagic)262144;
				});
			}));
		}
	}
}
