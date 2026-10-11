using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;

internal class ParadoxSovereign
{
	private static readonly Sprite Icon_Paradox = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_SECOND_REINCARNATION.png");

	internal const string FeeblemindBuff = "8b3b4c225fe0fb046bfa8881c3ddad0d";

	internal const string InsanityBuff = "53808be3c2becd24dbe572f77a7f44f8";

	internal const int SpellResistanceBase = 15;

	public static void Add()
	{
		OverpoweredAbilitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "ParadoxSovereignFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Overpowered Ability - Paradox Sovereign");
			bp.SetDescription(Main.IsekaiContext, "Commanding the axiomatic mechanics of Axis and the Inevitable paradox, you manipulate temporal loops and causality.\nBenefit: You gain a +4 insight bonus to Armor Class, attack rolls, and saving throws, Spell Resistance equal to 15 + character level, Damage Reduction 10/-, and complete immunity to confusion, insanity, and feeblemind.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Paradox;
			bp.Ranks = 1;
			bp.IsClassFeature = true;
			StatType[] array = new StatType[5]
			{
				StatType.AC,
				StatType.AdditionalAttackBonus,
				StatType.SaveFortitude,
				StatType.SaveReflex,
				StatType.SaveWill
			};
			foreach (StatType stat in array)
			{
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = stat;
					c.Value = 4;
				});
			}
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 10;
			});
			bp.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.Default;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				c.m_Progression = ContextRankProgression.BonusValue;
				c.m_StepLevel = 15;
			});
			bp.AddComponent(delegate(AddSpellResistance c)
			{
				c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Confusion;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Confusion;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Confusion;
			});
			string[] array2 = new string[2] { "53808be3c2becd24dbe572f77a7f44f8", "8b3b4c225fe0fb046bfa8881c3ddad0d" };
			foreach (string id in array2)
			{
				bp.AddComponent(delegate(SpecificBuffImmunity c)
				{
					c.m_Buff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>(id);
				});
			}
		}));
	}
}
