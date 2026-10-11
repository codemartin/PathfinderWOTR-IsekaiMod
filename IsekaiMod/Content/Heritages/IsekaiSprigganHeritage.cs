using IsekaiMod.Config;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Heritages;

internal class IsekaiSprigganHeritage
{
	public static void Add()
	{
		Sprite Icon_Spriggan = AssetLoader.LoadInternal(Main.IsekaiContext, "Heritages", "ICON_SPRIGGAN.png");
		BlueprintAbilityResource SizeAlterationResource = Helpers.CreateBlueprint(Main.IsekaiContext, "SizeAlterationResource", delegate(BlueprintAbilityResource bp)
		{
			bp.m_MaxAmount = HeritageResources.ByCharacterLevel(3, 1, 1, 1, 1);
		});
		BlueprintBuff SizeAlterationBuff = TTCoreExtensions.CreateBuff("SizeAlterationBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Size Alteration");
			bp.SetDescription(Main.IsekaiContext, "This creature's size is increased by one size category and they gain a +4 size bonus to Strength, a +2 size bonus to Constitution, and a -2 penalty to Dexterity.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Spriggan;
			bp.AddComponent(delegate(ChangeUnitSize c)
			{
				c.m_Type = ChangeUnitSize.ChangeType.Delta;
				c.SizeDelta = 1;
				c.Size = Size.Fine;
			});
			bp.AddComponent(delegate(AddGenericStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Size;
				c.Stat = StatType.Strength;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddGenericStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Size;
				c.Stat = StatType.Constitution;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddGenericStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Size;
				c.Stat = StatType.Dexterity;
				c.Value = -2;
			});
			bp.IsClassFeature = true;
		});
		BlueprintActivatableAbility SizeAlterationAbility = TTCoreExtensions.CreateActivatableAbility("SizeAlterationAbility", delegate(BlueprintActivatableAbility bp)
		{
			bp.SetName(Main.IsekaiContext, "Size Alteration");
			bp.SetDescription(Main.IsekaiContext, "As a standard action, grow by one size category and gain a +4 size bonus to Strength, a +2 size bonus to Constitution, and a -2 penalty to Dexterity. You can stay enlarged for 3 rounds per day plus 1 round per character level.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Spriggan;
			bp.m_Buff = SizeAlterationBuff.ToReference<BlueprintBuffReference>();
			bp.ActivationType = AbilityActivationType.WithUnitCommand;
			bp.m_ActivateWithUnitCommand = UnitCommand.CommandType.Standard;
			bp.DeactivateIfCombatEnded = true;
			bp.AddComponent(delegate(ActivatableAbilityResourceLogic c)
			{
				c.m_RequiredResource = SizeAlterationResource.ToReference<BlueprintAbilityResourceReference>();
				c.SpendType = ActivatableAbilityResourceLogic.ResourceSpendType.NewRound;
			});
		});
		BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiSprigganHeritage", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Isekai Spriggan");
			bp.SetDescription(Main.IsekaiContext, "Otherworldly entities who are reincarnated into the world of Golarion as a Spriggan have both extreme beauty and power. Their shape changing abilities allow them to easily defeat everyone who would underestimate their power.\nThe Isekai Spriggan has a +2 racial bonus to Strength, Constitution and Charisma, which replace the gnome's ability score adjustments. They gain a +1 racial bonus on concentration checks and on the {g|Encyclopedia:DC}DC{/g} of all {g|Encyclopedia:Spell}spells{/g} they cast, and add Athletics, Trickery, Stealth, and Perception to their class skills.\nSize Alteration: as a standard action they can grow by one size category, gaining a +4 size bonus to Strength, a +2 size bonus to Constitution and a -2 penalty to Dexterity, for 3 rounds per day plus 1 round per character level.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Spriggan;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Strength;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Constitution;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Charisma;
				c.Value = 2;
			});
			bp.AddComponent(delegate(ConcentrationBonus c)
			{
				c.CheckFact = false;
				c.Value = 1;
			});
			bp.AddComponent(delegate(IncreaseAllSpellsDC c)
			{
				c.Value = 1;
				c.Descriptor = ModifierDescriptor.Racial;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillAthletics;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillThievery;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillStealth;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillPerception;
			});
			bp.AddComponent(delegate(AddAbilityResources c)
			{
				c.m_Resource = SizeAlterationResource.ToReference<BlueprintAbilityResourceReference>();
				c.RestoreAmount = true;
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { SizeAlterationAbility.ToReference<BlueprintUnitFactReference>() };
			});
			bp.Groups = new FeatureGroup[0];
			bp.ReapplyOnLevelUp = true;
		});
		ContentOffering.OfferInSelection(FeatTools.Selections.GnomeHeritageSelection, feature, OfferedContent.IsekaiHeritages);
	}
}
