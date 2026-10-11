using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;

internal static class ArchetypeExclusivePowers
{
	internal sealed class Entry
	{
		internal string Archetype;

		internal string Feature;

		internal int Level;
	}

	internal static readonly Entry[] Table = new Entry[14]
	{
		new Entry
		{
			Archetype = "HeroArchetype",
			Feature = "ExcaliburFeature",
			Level = 5
		},
		new Entry
		{
			Archetype = "HeroArchetype",
			Feature = "HeroUnyieldingResolveFeature",
			Level = 7
		},
		new Entry
		{
			Archetype = "GodEmperorArchetype",
			Feature = "GodEmperorGoldenBodyFeature",
			Level = 5
		},
		new Entry
		{
			Archetype = "GodEmperorArchetype",
			Feature = "GodEmperorDivineDecreeFeature",
			Level = 7
		},
		new Entry
		{
			Archetype = "OverlordArchetype",
			Feature = "OverlordSupremeMajestyFeature",
			Level = 3
		},
		new Entry
		{
			Archetype = "OverlordArchetype",
			Feature = "OverlordNecromancySovereignFeature",
			Level = 7
		},
		new Entry
		{
			Archetype = "MastermindArchetype",
			Feature = "MastermindCalculatedDefenseFeature",
			Level = 3
		},
		new Entry
		{
			Archetype = "MastermindArchetype",
			Feature = "MastermindThreeStepsAheadFeature",
			Level = 7
		},
		new Entry
		{
			Archetype = "DevourerArchetype",
			Feature = "DevourerCorrosiveBodyFeature",
			Level = 3
		},
		new Entry
		{
			Archetype = "DevourerArchetype",
			Feature = "DevourerGluttonousRegenerationFeature",
			Level = 7
		},
		new Entry
		{
			Archetype = "ShadowMonarchArchetype",
			Feature = "ShadowMonarchVeilOfShadowsFeature",
			Level = 3
		},
		new Entry
		{
			Archetype = "ShadowMonarchArchetype",
			Feature = "ShadowMonarchRulersAuthorityFeature",
			Level = 7
		},
		new Entry
		{
			Archetype = "MartialGodArchetype",
			Feature = "MartialGodFlowingWaterFeature",
			Level = 3
		},
		new Entry
		{
			Archetype = "MartialGodArchetype",
			Feature = "MartialGodThousandCutsFeature",
			Level = 7
		}
	};

	private static bool Added;

	public static void Add()
	{
		if (Added)
		{
			return;
		}
		Added = true;
		Sprite icon = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ad5ed5ea4ec52334a94e975a64dad336"))?.m_Icon;
		Sprite icon2 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ff8f1534f66559c478448723e16b6624"))?.m_Icon;
		Sprite icon3 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("e96424f70ff884947b06f41a765b7658"))?.m_Icon;
		Sprite icon4 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("b24583190f36a8442b212e45226c54fc"))?.m_Icon;
		Sprite icon5 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("a89dcbbab8f40e44e920cc60636097cf"))?.m_Icon;
		Sprite sprite = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("1f01a098d737ec6419aedc4e7ad61fdd"))?.m_Icon;
		Sprite icon6 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("b3da3fbee6a751d4197e446c7e852bcb"))?.m_Icon ?? sprite;
		Sprite icon7 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("3b646e1db3403b940bf620e01d2ce0c7"))?.m_Icon;
		Sprite icon8 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("b974af13e45639a41a04843ce1c9aa12"))?.m_Icon;
		Sprite icon9 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("5bdc37e4acfa209408334326076a43bc"))?.m_Icon;
		Sprite icon10 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("644d2c0d029e54d4188bc34216d9d8c0"))?.m_Icon;
		Sprite sprite2 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("b89373001e05f1f4aa9b9bb4f420c40f"))?.m_Icon;
		Sprite icon11 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("f81b4910d05399a4aaf5fcc8c4d713eb"))?.m_Icon ?? sprite2;
		Power("HeroUnyieldingResolveFeature", "Special Power - Unyielding Resolve", "The Hero does not break, however hopeless the fight. \nBenefit: You are immune to fear effects and gain a +4 sacred bonus on all saving throws.\nRequires character level 7. Hero archetype only.", icon, 7, delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Fear;
			});
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Fear;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveFortitude;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveReflex;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveWill;
				c.Value = 4;
			});
		});
		Power("GodEmperorGoldenBodyFeature", "Special Power - Golden Body", "The Emperor's body has begun its transformation into divine gold. \nBenefit: You gain damage reduction 5/- and a +4 natural armor bonus to AC (it stacks with other natural armor).\nRequires character level 5. God Emperor archetype only.", icon2, 5, delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 5;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.NaturalArmor;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
		});
		Power("GodEmperorDivineDecreeFeature", "Special Power - Divine Decree", "The word of a living god is law, and the world obeys it. \nBenefit: The DC of all your spells increases by 2, and you gain a +4 bonus on caster level checks to overcome spell resistance.\nRequires character level 7. God Emperor archetype only.", icon3, 7, delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(IncreaseAllSpellsDC c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Value = 2;
				c.SpellsOnly = true;
			});
			bp.AddComponent(delegate(SpellPenetrationBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Value = 4;
			});
		});
		Power("OverlordSupremeMajestyFeature", "Special Power - Majesty of the Supreme Being", "Whatever you feel, your subjects only ever see an absolute ruler. \nBenefit: You add your Charisma modifier to Will saving throws, and gain a +5 competence bonus on Persuasion checks.\nRequires character level 3. Overlord archetype only.", icon4, 3, delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(DerivativeStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.BaseStat = StatType.Charisma;
				c.DerivativeStat = StatType.SaveWill;
			});
			bp.AddComponent(delegate(RecalculateOnStatChange c)
			{
				c.Stat = StatType.Charisma;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillPersuasion;
				c.Value = 5;
			});
		});
		Power("OverlordNecromancySovereignFeature", "Special Power - Sovereign of Necromancy", "Death magic answers to its ruler before anyone else. \nBenefit: The DC of your necromancy spells increases by 2, and you gain a +4 bonus on caster level checks to overcome spell resistance.\nRequires character level 7. Overlord archetype only.", icon5, 7, delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(IncreaseSpellSchoolDC c)
			{
				c.School = SpellSchool.Necromancy;
				c.BonusDC = 2;
				c.Descriptor = ModifierDescriptor.UntypedStackable;
			});
			bp.AddComponent(delegate(SpellPenetrationBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Value = 4;
			});
		});
		Power("MastermindCalculatedDefenseFeature", "Special Power - Calculated Defense", "Every blow aimed at you was predicted three turns ago. \nBenefit: You add your Intelligence modifier to your Armor Class. The bonus stacks with insight bonuses.\nRequires character level 3. Mastermind archetype only.", sprite, 3, delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(DerivativeStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.BaseStat = StatType.Intelligence;
				c.DerivativeStat = StatType.AC;
			});
			bp.AddComponent(delegate(RecalculateOnStatChange c)
			{
				c.Stat = StatType.Intelligence;
			});
		});
		Power("MastermindThreeStepsAheadFeature", "Special Power - Three Steps Ahead", "The battle was decided before it began; you only have to arrive on time. \nBenefit: You add your Intelligence modifier to initiative checks and Reflex saving throws. The bonus stacks with insight bonuses.\nRequires character level 7. Mastermind archetype only.", icon6, 7, delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(DerivativeStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.BaseStat = StatType.Intelligence;
				c.DerivativeStat = StatType.Initiative;
			});
			bp.AddComponent(delegate(DerivativeStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.BaseStat = StatType.Intelligence;
				c.DerivativeStat = StatType.SaveReflex;
			});
			bp.AddComponent(delegate(RecalculateOnStatChange c)
			{
				c.Stat = StatType.Intelligence;
			});
		});
		Power("DevourerCorrosiveBodyFeature", "Special Power - Corrosive Body", "Everything you touch starts being digested. \nBenefit: Your weapon, natural and unarmed attacks deal an additional 1d6 acid damage.\nRequires character level 3. Slime archetype only.", icon7, 3, delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(AddAdditionalWeaponDamage c)
			{
				c.DamageType = new DamageTypeDescription
				{
					Type = DamageType.Energy,
					Energy = DamageEnergyType.Acid
				};
				c.Value = new ContextDiceValue
				{
					DiceType = DiceType.D6,
					DiceCountValue = 1,
					BonusValue = 0
				};
			});
		});
		Power("DevourerGluttonousRegenerationFeature", "Special Power - Gluttonous Regeneration", "A slime that has eaten well does not stay hurt for long. \nBenefit: You gain fast healing 5.\nRequires character level 7. Slime archetype only.", icon8, 7, delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 5;
				c.Bonus = 0;
			});
		});
		Power("ShadowMonarchVeilOfShadowsFeature", "Special Power - Veil of Shadows", "Your outline never quite settles in the light. \nBenefit: Attacks against you have a 20% miss chance from concealment, and you gain a +5 competence bonus on Stealth checks.\nRequires character level 3. Shadow Monarch archetype only.", icon9, 3, delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(AddConcealment c)
			{
				c.Descriptor = ConcealmentDescriptor.Blur;
				c.Concealment = Concealment.Partial;
				c.OnlyForAttacks = true;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillStealth;
				c.Value = 5;
			});
		});
		Power("ShadowMonarchRulersAuthorityFeature", "Special Power - Ruler's Authority", "The Monarch hunts at the head of the army, not behind it. \nBenefit: You gain a +2 bonus on attack and damage rolls and a +10 foot bonus to movement speed.\nRequires character level 7. Shadow Monarch archetype only.", icon10, 7, delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Speed;
				c.Value = 10;
			});
		});
		Power("MartialGodFlowingWaterFeature", "Special Power - Flowing Water Stance", "Blows slide off you like rain off a river stone. \nBenefit: You gain a +4 dodge bonus to AC and a +4 bonus to CMD.\nRequires character level 3. Martial God archetype only.", sprite2, 3, delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Dodge;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalCMD;
				c.Value = 4;
			});
		});
		Power("MartialGodThousandCutsFeature", "Special Power - Thousand Cuts", "Your hands move faster than the eye can follow. \nBenefit: When you make a full attack, you make one additional attack at your highest base attack bonus. This stacks with haste.\nRequires character level 7. Martial God archetype only.", icon11, 7, delegate(BlueprintFeature bp)
		{
			bp.AddComponent(delegate(BuffExtraAttack c)
			{
				c.Number = 1;
				c.Haste = false;
			});
		});
	}

	private static void Power(string name, string displayName, string description, Sprite icon, int level, Action<BlueprintFeature> components)
	{
		SpecialPowerSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, displayName);
			bp.SetDescription(Main.IsekaiContext, description);
			((BlueprintUnitFact)bp).m_Icon = icon;
			bp.IsClassFeature = true;
			components(bp);
			bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = level;
			});
		}));
	}

	public static void Lock()
	{
		Entry[] table = Table;
		foreach (Entry entry in table)
		{
			BlueprintFeature blueprintFeature = FeatureOf(entry);
			BlueprintArchetype archetype = ArchetypeOf(entry);
			if (blueprintFeature == null || archetype == null)
			{
				Main.Log("ERROR: ArchetypeExclusivePowers.Lock could not find " + entry.Feature + " or " + entry.Archetype + ".");
			}
			else if (!blueprintFeature.GetComponents<PrerequisiteArchetypeLevel>().Any())
			{
				blueprintFeature.AddComponent(delegate(PrerequisiteArchetypeLevel c)
				{
					c.Group = Prerequisite.GroupType.Any;
					c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
					c.m_Archetype = archetype.ToReference<BlueprintArchetypeReference>();
					c.Level = 1;
				});
			}
		}
	}

	internal static BlueprintFeature FeatureOf(Entry entry)
	{
		return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, entry.Feature);
	}

	internal static BlueprintArchetype ArchetypeOf(Entry entry)
	{
		BlueprintCharacterClass blueprintCharacterClass = IsekaiProtagonistClass.Get();
		if (blueprintCharacterClass != null)
		{
			return blueprintCharacterClass.Archetypes.FirstOrDefault((BlueprintArchetype a) => a != null && a.name == entry.Archetype);
		}
		return null;
	}

	internal static IEnumerable<string> PowersOf(string archetypeName)
	{
		return from e in Table
			where e.Archetype == archetypeName
			select e.Feature;
	}
}
