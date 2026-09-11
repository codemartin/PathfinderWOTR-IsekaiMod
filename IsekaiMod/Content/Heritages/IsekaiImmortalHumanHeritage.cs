using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Heritages
{
	internal class IsekaiImmortalHumanHeritage
	{
		public static void Add()
		{
			HumanHeritageSelection.Register(Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiImmortalHumanHeritage", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Isekai Immortal Human");
				bp.SetDescription(Main.IsekaiContext, "Otherworldly souls who are reincarnated as Immortal Humans possess an unyielding life force anchored to higher planes. Their wounds continuously knit closed, making them nearly impossible to permanently fell in battle.\nImmortal Humans have a +2 racial bonus to Constitution and Charisma, Fast Healing 4, a +2 bonus on saving throws against death effects, and immunity to bleed effects and non-magical disease.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Constitution;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Racial;
					c.Stat = StatType.Charisma;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 4;
				});
				bp.AddComponent(delegate(SavingThrowBonusAgainstDescriptor c)
				{
					c.SpellDescriptor = SpellDescriptor.Death;
					c.ModifierDescriptor = ModifierDescriptor.Racial;
					c.Value = 2;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Disease | SpellDescriptor.Bleed;
				});
				bp.Groups = new FeatureGroup[0];
				bp.ReapplyOnLevelUp = true;
			}));
		}
	}
}
