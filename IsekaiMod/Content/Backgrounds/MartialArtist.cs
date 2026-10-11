using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds;

internal class MartialArtist
{
	public static void Add()
	{
		BlueprintFeature ExoticWeaponProficiency = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ExoticWeaponProficiency");
		BlueprintFeature ImprovedUnarmedStrike = BlueprintTools.GetBlueprint<BlueprintFeature>("7812ad3672a4b9a4fb894ea402095167");
		IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundMartialArtist", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Martial Artist");
			bp.SetBackgroundDescription(Main.IsekaiContext, "Through decades of relentless physical discipline conditioning fists and bone to shatter stone, the Martial Artist is a master of hand-to-hand combat and esoteric weaponry. Iron Fists: her unarmed strikes deal damage as one size category larger. She also gains the Improved Unarmed Strike feat, a +1 dodge bonus to Armor Class, adds Athletics and Mobility to her class skills with a +4 competence bonus to both, and is proficient with all exotic weapons.");
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[2]
				{
					ExoticWeaponProficiency.ToReference<BlueprintUnitFactReference>(),
					ImprovedUnarmedStrike.ToReference<BlueprintUnitFactReference>()
				};
			});
			bp.AddComponent(delegate(WeaponSizeChange c)
			{
				c.SizeCategoryChange = 1;
				c.CheckWeaponCategory = true;
				c.Category = WeaponCategory.UnarmedStrike;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Dodge;
				c.Stat = StatType.AC;
				c.Value = 1;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillAthletics;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillMobility;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillAthletics;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillMobility;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillAthletics;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillMobility;
				c.Value = 4;
			});
		}));
	}
}
