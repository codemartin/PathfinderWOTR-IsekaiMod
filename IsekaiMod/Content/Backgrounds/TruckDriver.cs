using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds
{
	internal class TruckDriver
	{
		public static void Add()
		{
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundTruckDriver", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Truck Driver");
				bp.SetBackgroundDescription(Main.IsekaiContext, "Accustomed to operating multi-ton freight haulers through blizzards, runaway mountain passes, and brutal highway collisions, the Truck Driver is an immovable force of physical grit. She gains Damage Reduction 2/-, immunity to fear effects, a +2 competence bonus on Fortitude saves, a +2 competence bonus to Combat Maneuver Bonus, proficiency with greatclubs and warhammers, and adds Athletics to her class skills with a +4 competence bonus, using Constitution instead of Strength for Athletics checks.");
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddBackgroundWeaponProficiency c)
				{
					c.Proficiency = WeaponCategory.Greatclub;
					c.StackBonusType = ModifierDescriptor.Enhancement;
					c.StackBonus = 1;
				});
				bp.AddComponent(delegate(AddBackgroundWeaponProficiency c)
				{
					c.Proficiency = WeaponCategory.Warhammer;
					c.StackBonusType = ModifierDescriptor.Enhancement;
					c.StackBonus = 1;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Frightened;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Fear;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Fear;
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
					c.Stat = StatType.AdditionalCMB;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillAthletics;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillAthletics;
				});
				bp.AddComponent(delegate(ReplaceStatBaseAttribute c)
				{
					c.TargetStat = StatType.SkillAthletics;
					c.BaseAttributeReplacement = StatType.Constitution;
					c.ReplaceIfHigher = true;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillAthletics;
					c.Value = 4;
				});
			}));
		}
	}
}
