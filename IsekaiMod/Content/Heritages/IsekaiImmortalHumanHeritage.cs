using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Heritages;

internal class IsekaiImmortalHumanHeritage
{
	public static void Add()
	{
		HumanHeritageSelection.Register(Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiImmortalHumanHeritage", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Isekai Immortal Human");
			bp.SetDescription(Main.IsekaiContext, "Otherworldly souls who are reincarnated as Immortal Humans possess an unyielding life force anchored to higher planes. Their wounds continuously knit closed, making them nearly impossible to permanently fell in battle.\nImmortal Humans have a +4 racial bonus to Constitution, a +2 racial bonus to Charisma, a +2 racial bonus on Fortitude saves, and a +4 racial bonus on saving throws against death effects.\nUndying: they have fast healing 4 and are immune to bleed and disease.");
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Constitution;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.Charisma;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Racial;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 4;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(SavingThrowBonusAgainstDescriptor c)
			{
				c.SpellDescriptor = SpellDescriptor.Death;
				c.ModifierDescriptor = ModifierDescriptor.Racial;
				c.Value = 4;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Disease | SpellDescriptor.Bleed;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Disease | SpellDescriptor.Bleed;
			});
			bp.Groups = new FeatureGroup[0];
			bp.ReapplyOnLevelUp = true;
		}));
	}
}
