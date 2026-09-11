using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Devourer
{
	internal class PredatorInstincts
	{
		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "PredatorInstincts", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Predator Instincts");
				bp.SetDescription(Main.IsekaiContext, "A Slime possesses an apex organism's adaptive physiology. You gain a +2 natural armor bonus to {g|Encyclopedia:Armor_Class}AC{/g}, and add {g|Encyclopedia:Athletics}Athletics{/g}, {g|Encyclopedia:Lore_Nature}Lore (Nature){/g}, and {g|Encyclopedia:Perception}Perception{/g} to your class {g|Encyclopedia:Skills}skills{/g}, gaining a +2 competence bonus on all three skills.");
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillAthletics;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillLoreNature;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillPerception;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillAthletics;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillLoreNature;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillPerception;
					c.Value = 2;
				});
			});
		}
	}
}
