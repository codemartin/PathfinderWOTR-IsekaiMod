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
	internal class AutoSolidShadows
	{
		private static readonly Sprite Icon_SolidShadowsSpell = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("48cba171d3bc4042ae2a18d503816b50"))?.m_Icon;

		public static void Add()
		{
			AutoMetamagicSelection.AddToSelection(TTCoreExtensions.CreateToggleBuffFeature("AutoSolidShadows", "Overpowered Ability - Auto Solid Shadows", "Every time you cast a shadow spell, it becomes 20% more real, as though using the Solid Shadows Spell feat.", Icon_SolidShadowsSpell, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AutoMetamagic c)
				{
					c.m_AllowedAbilities = AutoMetamagic.AllowedType.SpellOnly;
					c.Metamagic = (Metamagic)1048576;
				});
			}));
		}
	}
}
