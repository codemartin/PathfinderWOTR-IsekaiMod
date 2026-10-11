using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds;

internal class Surgeon
{
	public static void Add()
	{
		IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundSurgeon", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Surgeon");
			bp.SetBackgroundDescription(Main.IsekaiContext, "Trained in elite clinical surgery, trauma intervention, and anatomical dissection, the Surgeon knows exactly how to preserve life and where mortal flesh is most vulnerable. Anatomy Expert: she gains a +4 bonus on critical confirmation rolls. She also gains proficiency with daggers and scythes, a +2 competence bonus on Fortitude and Reflex saves, an additional +2 bonus on saving throws against poison and disease, immunity to bleed effects, and adds Lore (Nature) and Perception to her class skills with a +4 competence bonus to both.");
			bp.AddComponent(delegate(AddBackgroundWeaponProficiency c)
			{
				c.Proficiency = WeaponCategory.Dagger;
				c.StackBonusType = ModifierDescriptor.Enhancement;
				c.StackBonus = 1;
			});
			bp.AddComponent(delegate(AddBackgroundWeaponProficiency c)
			{
				c.Proficiency = WeaponCategory.Scythe;
				c.StackBonusType = ModifierDescriptor.Enhancement;
				c.StackBonus = 1;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
			bp.AddComponent(delegate(SavingThrowBonusAgainstDescriptor c)
			{
				c.ModifierDescriptor = ModifierDescriptor.Competence;
				c.SpellDescriptor = SpellDescriptor.Poison | SpellDescriptor.Disease;
				c.Value = 2;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(CriticalConfirmationBonus c)
			{
				c.Value = 0;
				c.Bonus = 4;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Bleed;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Bleed;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillLoreNature;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillPerception;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillLoreNature;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillPerception;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillLoreNature;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillPerception;
				c.Value = 4;
			});
		}));
	}
}
