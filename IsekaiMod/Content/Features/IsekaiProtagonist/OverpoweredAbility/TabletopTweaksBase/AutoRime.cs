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
	internal class AutoRime
	{
		private static readonly Sprite Icon_RimeSpell = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("192b00c671a64c4c8643f7c18bd1542e"))?.m_Icon;

		public static void Add()
		{
			AutoMetamagicSelection.AddToSelection(TTCoreExtensions.CreateToggleBuffFeature("AutoRime", "Overpowered Ability - Auto Rime", "Every time you cast a cold spell, it entangles the target, as though using the Rime Spell feat.", Icon_RimeSpell, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AutoMetamagic c)
				{
					c.m_AllowedAbilities = AutoMetamagic.AllowedType.SpellOnly;
					c.Metamagic = (Metamagic)65536;
				});
			}));
		}
	}
}
