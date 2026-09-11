using HarmonyLib;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.Formations.Facts;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.ActivatableAbilities.Restrictions;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features
{
	internal class ExtraWings
	{
		private static readonly BlueprintBuff WingsAngel = BlueprintTools.GetBlueprint<BlueprintBuff>("25699a90ed3299e438b6fd5548930809");

		private static readonly BlueprintBuff BuffWingsMutagen = BlueprintTools.GetBlueprint<BlueprintBuff>("e4979934bdb39d842b28bee614606823");

		private static readonly BlueprintBuff WingsAngelBlack = BlueprintTools.GetBlueprint<BlueprintBuff>("a19cda073f4c2b64ca1f8bf8fe285ece");

		private static readonly BlueprintBuff BuffWingsAngelGhost = BlueprintTools.GetBlueprint<BlueprintBuff>("bd6980649fd60fa4085c34aa74ac47f2");

		private static readonly BlueprintBuff BuffWingsDemon = BlueprintTools.GetBlueprint<BlueprintBuff>("3c958be25ab34dc448569331488bee27");

		public static void Add()
		{
			TTCoreExtensions.CreateActivatableAbility("BlackWingsAbility", delegate(BlueprintActivatableAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Wings (Black)");
				((BlueprintUnitFact)bp).m_Description = ((BlueprintUnitFact)WingsAngel).m_Description;
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)WingsAngel).m_Icon;
				bp.AddComponent(delegate(RestrictionHasFact c)
				{
					c.m_Feature = BuffWingsMutagen.ToReference<BlueprintUnitFactReference>();
					c.Not = true;
				});
				bp.m_Buff = WingsAngelBlack.ToReference<BlueprintBuffReference>();
				bp.Group = ActivatableAbilityGroup.Wings;
				bp.WeightInGroup = 1;
			});
			TTCoreExtensions.CreateActivatableAbility("GhostWingsAbility", delegate(BlueprintActivatableAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Wings (Ghostly)");
				((BlueprintUnitFact)bp).m_Description = ((BlueprintUnitFact)WingsAngel).m_Description;
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)WingsAngel).m_Icon;
				bp.AddComponent(delegate(RestrictionHasFact c)
				{
					c.m_Feature = BuffWingsMutagen.ToReference<BlueprintUnitFactReference>();
					c.Not = true;
				});
				bp.m_Buff = BuffWingsAngelGhost.ToReference<BlueprintBuffReference>();
				bp.Group = ActivatableAbilityGroup.Wings;
				bp.WeightInGroup = 1;
			});
			BlueprintBuff DevilWingsBuff = TTCoreExtensions.CreateBuff("DevilWingsBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Wings");
				((BlueprintUnitFact)bp).m_Description = ((BlueprintUnitFact)WingsAngel).m_Description;
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)WingsAngel).m_Icon;
				bp.AddComponent(delegate(ACBonusAgainstAttacks c)
				{
					c.AgainstMeleeOnly = true;
					c.ArmorClassBonus = 3;
					c.Descriptor = ModifierDescriptor.Dodge;
					c.NotArmorCategory = new ArmorProficiencyGroup[0];
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.DifficultTerrain;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Ground;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Ground;
				});
				bp.AddComponent(delegate(FormationACBonus c)
				{
					c.Bonus = 3;
					c.m_IgnoreIfHasAnyFact = new BlueprintUnitFactReference[0];
				});
				bp.AddComponent(delegate(AddEquipmentEntity c)
				{
					c.EquipmentEntity = new EquipmentEntityLink
					{
						AssetId = "214b59c14bada944c83041f18dcec3d2"
					};
				});
				bp.IsClassFeature = true;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
				bp.Frequency = DurationRate.Rounds;
			});
			TTCoreExtensions.CreateActivatableAbility("DevilWingsAbility", delegate(BlueprintActivatableAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Wings (Devilish)");
				((BlueprintUnitFact)bp).m_Description = ((BlueprintUnitFact)WingsAngel).m_Description;
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)WingsAngel).m_Icon;
				bp.AddComponent(delegate(RestrictionHasFact c)
				{
					c.m_Feature = BuffWingsMutagen.ToReference<BlueprintUnitFactReference>();
					c.Not = true;
				});
				bp.m_Buff = DevilWingsBuff.ToReference<BlueprintBuffReference>();
				bp.Group = ActivatableAbilityGroup.Wings;
				bp.WeightInGroup = 1;
			});
			TTCoreExtensions.CreateActivatableAbility("DemonWingsAbility", delegate(BlueprintActivatableAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Wings (Demonic)");
				((BlueprintUnitFact)bp).m_Description = ((BlueprintUnitFact)WingsAngel).m_Description;
				((BlueprintUnitFact)bp).m_Icon = ((BlueprintUnitFact)WingsAngel).m_Icon;
				bp.AddComponent(delegate(RestrictionHasFact c)
				{
					c.m_Feature = BuffWingsMutagen.ToReference<BlueprintUnitFactReference>();
					c.Not = true;
				});
				bp.m_Buff = BuffWingsDemon.ToReference<BlueprintBuffReference>();
				bp.Group = ActivatableAbilityGroup.Wings;
				bp.WeightInGroup = 1;
			});
			PatchPitAreaEffects(DevilWingsBuff);
		}

		private static void PatchPitAreaEffects(BlueprintBuff wingsBuff)
		{
			BlueprintAbilityAreaEffect[] array = new BlueprintAbilityAreaEffect[7]
			{
				BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("b905a3c987f22cb49a246f0ab211f34c"),
				BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("bf68ec704dc186549a7c6fbf22d3d661"),
				BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("cf742a1d377378e4c8799f6a3afff1ba"),
				BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("beccc33f543b1f8469c018982c23ac06"),
				BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("e122151e93e44e0488521aed9e51b617"),
				BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("d086b1aeb367a5b43808d34c321955d1"),
				BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>("9b51157a5305dbf4184bf15bdad39226")
			};
			for (int i = 0; i < array.Length; i++)
			{
				AreaEffectPit component = array[i].GetComponent<AreaEffectPit>();
				component.m_EffectsImmunityFacts = component.m_EffectsImmunityFacts.AddToArray(wingsBuff.ToReference<BlueprintUnitFactReference>());
			}
		}
	}
}
