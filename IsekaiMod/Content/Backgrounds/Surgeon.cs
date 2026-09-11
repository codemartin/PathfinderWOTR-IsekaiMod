using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds
{
	internal class Surgeon
	{
		public static void Add()
		{
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundSurgeon", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Surgeon");
				bp.SetBackgroundDescription(Main.IsekaiContext, "Trained in elite clinical surgery, trauma intervention, and anatomical dissection, the Surgeon knows exactly how to preserve life and where mortal flesh is most vulnerable. She gains proficiency with daggers and scythes, a +2 competence bonus on Fortitude saves, an additional +2 bonus on saving throws against poison and disease, a +2 bonus on critical confirmation rolls, and adds Lore (Nature) and Perception to her class skills with a +4 competence bonus to both.");
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
				bp.AddComponent(delegate(SavingThrowBonusAgainstDescriptor c)
				{
					c.ModifierDescriptor = ModifierDescriptor.Competence;
					c.SpellDescriptor = SpellDescriptor.Poison | SpellDescriptor.Disease;
					c.Value = 2;
				});
				bp.AddComponent(delegate(CriticalConfirmationBonus c)
				{
					c.Bonus = 2;
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
}
