using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds;

internal class Musician
{
	public static void Add()
	{
		IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundMusician", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Musician");
			bp.SetBackgroundDescription(Main.IsekaiContext, "A lifetime of rehearsals, stage fright and deafening concerts trained the Musician's ear to every frequency. Stage Presence: she adds her Charisma bonus to Will saves as an insight bonus. Perfect Pitch: she has a +2 bonus to caster level and DC for Sonic spells. She also gains sonic resistance 10, a +2 competence bonus on Reflex saves, and adds Persuasion, Perception and Mobility to the list of her class skills, with a +4 competence bonus on Persuasion and Perception.");
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
			bp.AddComponent(delegate(AddDamageResistanceEnergy c)
			{
				c.Type = DamageEnergyType.Sonic;
				c.Value = 10;
			});
			bp.AddComponent(delegate(DerivativeStatBonus c)
			{
				c.BaseStat = StatType.Charisma;
				c.DerivativeStat = StatType.SaveWill;
				c.Descriptor = ModifierDescriptor.Insight;
			});
			bp.AddComponent(delegate(RecalculateOnStatChange c)
			{
				c.Stat = StatType.Charisma;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillPersuasion;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillPerception;
			});
			bp.AddComponent(delegate(AddClassSkill c)
			{
				c.Skill = StatType.SkillMobility;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillPersuasion;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillPerception;
			});
			bp.AddComponent(delegate(AddBackgroundClassSkill c)
			{
				c.Skill = StatType.SkillMobility;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillPersuasion;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillPerception;
				c.Value = 4;
			});
		}));
	}
}
