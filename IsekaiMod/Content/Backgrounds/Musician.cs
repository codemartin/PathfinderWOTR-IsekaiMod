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
	internal class Musician
	{
		public static void Add()
		{
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundMusician", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Musician");
				bp.SetBackgroundDescription(Main.IsekaiContext, "The Musician add Persuasion to the list of her class skills and has a +2 bonus to caster level and DC for Sonic spells.");
				bp.AddComponent(delegate(AddClassSkill c)
				{
					c.Skill = StatType.SkillPersuasion;
				});
				bp.AddComponent(delegate(AddBackgroundClassSkill c)
				{
					c.Skill = StatType.SkillPersuasion;
				});
				bp.AddComponent(delegate(IncreaseSpellDescriptorCasterLevel c)
				{
					c.Descriptor = SpellDescriptor.Sonic;
					c.BonusCasterLevel = 2;
					c.ModifierDescriptor = ModifierDescriptor.UntypedStackable;
				});
				bp.AddComponent(delegate(IncreaseSpellDescriptorDC c)
				{
					c.Descriptor = SpellDescriptor.Sonic;
					c.BonusDC = 2;
					c.ModifierDescriptor = ModifierDescriptor.UntypedStackable;
				});
			}));
		}
	}
}
