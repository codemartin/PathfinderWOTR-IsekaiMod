using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds
{
	internal class CollegeStudent
	{
		public static void Add()
		{
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundCollegeStudent", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "College Student");
				bp.SetBackgroundDescription(Main.IsekaiContext, "The College Student gains a +2 bonus to all attributes.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Trait;
					c.Stat = StatType.Strength;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Trait;
					c.Stat = StatType.Dexterity;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Trait;
					c.Stat = StatType.Constitution;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Trait;
					c.Stat = StatType.Intelligence;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Trait;
					c.Stat = StatType.Wisdom;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Trait;
					c.Stat = StatType.Charisma;
					c.Value = 2;
				});
			}));
		}
	}
}
