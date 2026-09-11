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

namespace IsekaiMod.Content.Heritages
{
	internal class IsekaiSprigganHeritage
	{
		public static void Add()
		{
			Sprite Icon_Spriggan = AssetLoader.LoadInternal(Main.IsekaiContext, "Heritages", "ICON_SPRIGGAN.png");
			BlueprintBuff SizeAlterationBuff = TTCoreExtensions.CreateBuff("SizeAlterationBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Size Alteration");
				bp.SetDescription(Main.IsekaiContext, "This creature's size is increased by two size categories and they gain +10 Speed, +12 Strength, -2 Dexterity, +6 Constitution, and a -2 penalty to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Spriggan;
				bp.AddComponent(delegate(ChangeUnitSize c)
				{
					c.m_Type = ChangeUnitSize.ChangeType.Delta;
					c.SizeDelta = 2;
					c.Size = Size.Fine;
				});
				bp.AddComponent(delegate(AddGenericStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Size;
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddGenericStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Size;
					c.Stat = StatType.Strength;
					c.Value = 12;
				});
				bp.AddComponent(delegate(AddGenericStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Size;
					c.Stat = StatType.Dexterity;
					c.Value = -2;
				});
				bp.AddComponent(delegate(AddGenericStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Size;
					c.Stat = StatType.Constitution;
					c.Value = 6;
				});
				bp.AddComponent(delegate(AddGenericStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.AC;
					c.Value = -2;
				});
				bp.IsClassFeature = true;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
			});
			BlueprintActivatableAbility SizeAlterationAbility = TTCoreExtensions.CreateActivatableAbility("SizeAlterationAbility", delegate(BlueprintActivatableAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Size Alteration");
				bp.SetDescription(Main.IsekaiContext, "As a standard action, increase your size by two size categories and gain +10 Speed, +12 Strength, -2 Dexterity, +6 Constitution, and a -2 penalty to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Spriggan;
				bp.m_Buff = SizeAlterationBuff.ToReference<BlueprintBuffReference>();
				bp.ActivationType = AbilityActivationType.WithUnitCommand;
				bp.m_ActivateWithUnitCommand = UnitCommand.CommandType.Standard;
			});
			BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiSprigganHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Isekai Spriggan");
				bp.SetDescription(Main.IsekaiContext, "Otherworldly entities who are reincarnated into the world of Golarion as a Spriggan have both extreme beauty and power. Their shape changing abilities allow them to easily defeat everyone who would underestimate their power.\nThe Isekai Spriggan gains a +1 racial bonus on concentration checks and on the {g|Encyclopedia:DC}DC{/g} of all {g|Encyclopedia:Spell}spells{/g} they cast. They add Athletics, Trickery, Stealth, and Perception to the list of their class skills. They are also able to use the Size alteration ability to increase their size by two size categories and gain +10 Speed, +12 Strength, -2 Dexterity, +6 Constitution, and a -2 penalty to AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Spriggan;
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
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { SizeAlterationAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.Groups = new FeatureGroup[0];
			});
			FeatTools.Selections.GnomeHeritageSelection.AddToSelection(feature);
		}
	}
}
