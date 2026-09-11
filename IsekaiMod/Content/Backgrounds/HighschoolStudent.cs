using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds
{
	internal class HighschoolStudent
	{
		public static void Add()
		{
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundHighschoolStudent", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Highschool Student");
				bp.SetBackgroundDescription(Main.IsekaiContext, "The Highschool Student gains a +1 bonus to all attributes.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Trait;
					c.Stat = StatType.Strength;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Trait;
					c.Stat = StatType.Dexterity;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Trait;
					c.Stat = StatType.Constitution;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Trait;
					c.Stat = StatType.Intelligence;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Trait;
					c.Stat = StatType.Wisdom;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Trait;
					c.Stat = StatType.Charisma;
					c.Value = 1;
				});
			}));
		}
	}
}
