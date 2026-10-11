using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds;

internal class RebornDemonLord
{
	public static void Add()
	{
		IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundRebornDemonLord", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Reborn Demon Lord");
			bp.SetBackgroundDescription(Main.IsekaiContext, "Once a Demon Lord who ruled the storm-wracked wastes of another world, she was reborn into a mortal body that still remembers its old power. Lord of Storms: she has immunity to electricity. She also gains a +2 bonus to Strength, a +2 competence bonus on Will saves, and adds Persuasion to her class skills with a +4 competence bonus.");
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Trait;
				c.Stat = StatType.Strength;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddEnergyImmunity c)
			{
				c.Type = DamageEnergyType.Electricity;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillPersuasion;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillPersuasion;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillPersuasion;
				c.Value = 4;
			});
		}));
	}
}
