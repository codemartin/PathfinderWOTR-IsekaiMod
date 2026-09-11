using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch
{
	internal class ShadowHunterInstincts
	{
		private static readonly BlueprintFeature WeaponFinesse = BlueprintTools.GetBlueprint<BlueprintFeature>("90e54424d682d104ab36436bd527af09");

		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowHunterInstincts", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Shadow Hunter Instincts");
				bp.SetDescription(Main.IsekaiContext, "Awakening as a Monarch hones your predatory instincts. You gain the {g|Encyclopedia:Feat}Weapon Finesse{/g} feat, a +2 competence bonus to initiative, and add {g|Encyclopedia:Stealth}Stealth{/g}, {g|Encyclopedia:Trickery}Trickery{/g}, {g|Encyclopedia:Perception}Perception{/g}, and {g|Encyclopedia:Athletics}Athletics{/g} to your class {g|Encyclopedia:Skills}skills{/g}, gaining a +2 competence bonus on Stealth and Perception checks.");
				bp.IsClassFeature = true;
				if (WeaponFinesse != null)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { WeaponFinesse.ToReference<BlueprintUnitFactReference>() };
					});
				}
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.Initiative;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillStealth;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillThievery;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillPerception;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillAthletics;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillStealth;
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
