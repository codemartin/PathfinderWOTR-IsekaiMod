using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Heritages
{
	internal class GnomeOfTheFirstWorld
	{
		public static void Add()
		{
			BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "GnomeOfTheFirstWorldHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Gnome of the First World");
				bp.SetDescription(Main.IsekaiContext, "Gnomes who retain their unbroken lineage to the primordial First World. Their whimsical fey magic twists reality around them with kaleidoscopic brilliance.\nGnomes of the First World gain a +2 racial bonus to Dexterity and Charisma, a -2 penalty to Strength, a +2 luck bonus on all saving throws, +1 DC to Enchantment and Illusion spells, and immunity to curses and confusion effects.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Dexterity;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Charisma;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.Strength;
					c.Value = -2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
				bp.AddComponent(delegate(IncreaseSpellSchoolDC c)
				{
					c.School = SpellSchool.Enchantment;
					c.BonusDC = 1;
					c.Descriptor = ModifierDescriptor.Racial;
				});
				bp.AddComponent(delegate(IncreaseSpellSchoolDC c)
				{
					c.School = SpellSchool.Illusion;
					c.BonusDC = 1;
					c.Descriptor = ModifierDescriptor.Racial;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Curse;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Confusion;
				});
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Confusion;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Confusion;
				});
				bp.Groups = new FeatureGroup[0];
				bp.ReapplyOnLevelUp = true;
			});
			FeatTools.Selections.GnomeHeritageSelection.AddToSelection(feature);
		}
	}
}
