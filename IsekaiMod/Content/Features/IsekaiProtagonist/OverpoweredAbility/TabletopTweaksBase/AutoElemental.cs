using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility.TabletopTweaksBase
{
	internal class AutoElemental
	{
		private static readonly Sprite Icon_ElementalAcidSpell = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("0114e94ae4ba4e1890d245a579ff871a"))?.m_Icon;

		private static readonly Sprite Icon_ElementalColdSpell = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("5eeda1e5fcd04784a2b7b9724eebe04a"))?.m_Icon;

		private static readonly Sprite Icon_ElementalElectricitySpell = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("579b8f5e9ad6417781a39b3dae147da2"))?.m_Icon;

		private static readonly Sprite Icon_ElementalFireSpell = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e5cd7ebbf00b4a0bbc80e623924bf7b6"))?.m_Icon;

		private static readonly BlueprintActivatableAbility ElementalSpellSplitAbility = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("36a26221b979415584190e8197adcd0c");

		public static void Add()
		{
			BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleBuffFeature("AutoElementalAcid", "Overpowered Ability - Auto Elemental (Acid)", "Every time you cast a spell, you can replace or split its damage with acid damage, as though using the Elemental Spell (Acid) feat.", Icon_ElementalAcidSpell, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AutoMetamagic c)
				{
					c.m_AllowedAbilities = AutoMetamagic.AllowedType.SpellOnly;
					c.Metamagic = (Metamagic)4194304;
				});
			});
			BlueprintFeature blueprintFeature2 = TTCoreExtensions.CreateToggleBuffFeature("AutoElementalCold", "Overpowered Ability - Auto Elemental (Cold)", "Every time you cast a spell, you can replace or split its damage with cold damage, as though using the Elemental Spell (Cold) feat.", Icon_ElementalColdSpell, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AutoMetamagic c)
				{
					c.m_AllowedAbilities = AutoMetamagic.AllowedType.SpellOnly;
					c.Metamagic = (Metamagic)8388608;
				});
			});
			BlueprintFeature blueprintFeature3 = TTCoreExtensions.CreateToggleBuffFeature("AutoElementalElectricity", "Overpowered Ability - Auto Elemental (Electricity)", "Every time you cast a spell, you can replace or split its damage with electricity damage, as though using the Elemental Spell (Electricity) feat.", Icon_ElementalElectricitySpell, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AutoMetamagic c)
				{
					c.m_AllowedAbilities = AutoMetamagic.AllowedType.SpellOnly;
					c.Metamagic = (Metamagic)16777216;
				});
			});
			BlueprintFeature blueprintFeature4 = TTCoreExtensions.CreateToggleBuffFeature("AutoElementalFire", "Overpowered Ability - Auto Elemental (Fire)", "Every time you cast a spell, you can replace or split its damage with fire damage, as though using the Elemental Spell (Fire) feat.", Icon_ElementalFireSpell, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AutoMetamagic c)
				{
					c.m_AllowedAbilities = AutoMetamagic.AllowedType.SpellOnly;
					c.Metamagic = (Metamagic)33554432;
				});
			});
			BlueprintFeature[] array = new BlueprintFeature[4] { blueprintFeature, blueprintFeature2, blueprintFeature3, blueprintFeature4 };
			foreach (BlueprintFeature blueprintFeature5 in array)
			{
				blueprintFeature5.Ranks = 1;
				blueprintFeature5.ReapplyOnLevelUp = true;
				blueprintFeature5.IsClassFeature = true;
				if (ElementalSpellSplitAbility != null)
				{
					blueprintFeature5.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { ElementalSpellSplitAbility.ToReference<BlueprintUnitFactReference>() };
					});
				}
				blueprintFeature5.AddPrerequisite(delegate(PrerequisiteStatValue c)
				{
					c.Stat = StatType.Intelligence;
					c.Value = 3;
				});
				blueprintFeature5.AddComponent<RecommendationRequiresSpellbook>();
			}
			AutoMetamagicSelection.AddToSelection(blueprintFeature);
			AutoMetamagicSelection.AddToSelection(blueprintFeature2);
			AutoMetamagicSelection.AddToSelection(blueprintFeature3);
			AutoMetamagicSelection.AddToSelection(blueprintFeature4);
		}
	}
}
