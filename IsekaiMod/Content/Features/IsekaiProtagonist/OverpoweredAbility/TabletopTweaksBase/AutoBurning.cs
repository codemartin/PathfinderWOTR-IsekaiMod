using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility.TabletopTweaksBase;

internal class AutoBurning
{
	private static readonly Sprite Icon_BurningSpell = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("4732a4b7b53f46848ae34a9dae66dbb2"))?.m_Icon;

	public static void Add()
	{
		BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleBuffFeature("AutoBurning", "Overpowered Ability - Auto Burning", "Every time you cast a acid of fire spell, it causes acid or fire damage on the next round, as though using the Burning Spell feat. Requires character level 5.", Icon_BurningSpell, delegate(BlueprintBuff bp)
		{
			bp.AddComponent(delegate(AutoMetamagic c)
			{
				c.m_AllowedAbilities = AutoMetamagic.AllowedType.SpellOnly;
				c.Metamagic = (Metamagic)131072;
			});
		});
		blueprintFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
		{
			c.Level = 5;
		});
		AutoMetamagicSelection.AddToSelection(blueprintFeature);
	}
}
