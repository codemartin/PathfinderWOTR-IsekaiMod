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

internal class AutoEncouraging
{
	private static readonly Sprite Icon_EncouragingSpell = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("392608e8033a409ab96afdfbf315e028"))?.m_Icon;

	public static void Add()
	{
		BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleBuffFeature("AutoEncouraging", "Overpowered Ability - Auto Encouraging", "Every time you cast a spell, increase its morale bonus by 1, as though using the Encouraging Spell feat. Requires character level 5.", Icon_EncouragingSpell, delegate(BlueprintBuff bp)
		{
			bp.AddComponent(delegate(AutoMetamagic c)
			{
				c.m_AllowedAbilities = AutoMetamagic.AllowedType.SpellOnly;
				c.Metamagic = (Metamagic)2097152;
			});
		});
		blueprintFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
		{
			c.Level = 5;
		});
		AutoMetamagicSelection.AddToSelection(blueprintFeature);
	}
}
