using IsekaiMod.Utilities;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero
{
	internal static class HeroOfTimeFeatures
	{
		private static readonly Sprite Icon_Courage = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e45ab30f49215054e83b4ea12165409f"))?.m_Icon;

		private static readonly Sprite Icon_Power = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("208ce0902f20b1e4896a85b2339468ff"))?.m_Icon ?? Icon_Courage;

		private static readonly Sprite Icon_Wisdom = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("f0455c9295b53904f9e02fc571dd2ce1"))?.m_Icon ?? Icon_Courage;

		private static readonly Sprite Icon_Time = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("03464790f40c3c24aa684b57155f3280"))?.m_Icon ?? Icon_Courage;

		private static readonly Sprite Icon_Triad = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("faf473e3a977fd4428cd3f1a526346d2"))?.m_Icon ?? Icon_Courage;

		private static readonly Sprite Icon_Dark = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("9eda82a1f78558747a03c17e0e9a1a68"))?.m_Icon ?? Icon_Courage;

		public static BlueprintFeature TriforceCourageFeature;

		public static BlueprintFeature TriforcePowerFeature;

		public static BlueprintBuff TriforcePowerBuff;

		public static BlueprintFeature TriforceWisdomFeature;

		public static BlueprintAbilityResource SongOfTimeResource;

		public static BlueprintBuff SongOfTimeBuff;

		public static BlueprintAbility SongOfTimeAbility;

		public static BlueprintFeature SongOfTimeFeature;

		public static BlueprintFeature SacredTriadFeature;

		public static BlueprintBuff TemporalRewindBuff;

		public static BlueprintFeature CorruptedAuraOfDreadFeature;

		public static BlueprintFeature CorruptedBondsOfDominationFeature;

		public static BlueprintFeature CorruptedBladeOfCalamityFeature;

		public static BlueprintFeature CorruptedEyeOfTheVoidFeature;

		public static BlueprintFeature CorruptedSongOfTimeFeature;

		public static BlueprintFeature CorruptedDarkTriforceFeature;

		public static BlueprintFeature CorruptedShatteredCycleFeature;

		public static void Add()
		{
			TriforceCourageFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "TriforceCourageFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Triforce of Courage");
				bp.SetDescription(Main.IsekaiContext, "The sacred emblem of unbroken valor shines upon your hand. You and all allies within 30 feet gain a +2 morale bonus to attack rolls, weapon damage, and saving throws against Fear and Mind-Affecting effects.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Courage;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 2;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Shaken | SpellDescriptor.Frightened;
				});
			});
			TriforcePowerBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "TriforcePowerBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Triforce of Power: Awakened Force");
				bp.SetDescription(Main.IsekaiContext, "Raw sacred might surges through your sinews. Weapon attacks deal an additional +1d6 force damage, and you gain a +2 inherent bonus to Strength and Constitution.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Power;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Strength;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Constitution;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					// WeaponEnergyDamageDice is a weapon-enchantment component; on a unit feature it throws and adds nothing.
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					// Both condition checkers are read unconditionally, so they must exist even when empty.
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 1, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Holy };
				});
			});
			TriforcePowerFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "TriforcePowerFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Triforce of Power");
				bp.SetDescription(Main.IsekaiContext, "The sacred emblem of overwhelming strength awakens. In combat, you permanently manifest the Triforce of Power: weapon attacks deal +1d6 force damage, critical confirmation rolls gain a +2 bonus, and your physical endurance surges with +2 Strength and Constitution.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Power;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { TriforcePowerBuff.ToReference<BlueprintUnitFactReference>() };
				});
			});
			TriforceWisdomFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "TriforceWisdomFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Triforce of Wisdom");
				bp.SetDescription(Main.IsekaiContext, "The sacred emblem of eternal insight and divine sorcery shines clear. You gain a +2 insight bonus to Armor Class and Reflex saves, a +2 bonus to the DC of all spells, and a +4 insight bonus to all Knowledge and Lore skill checks.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Wisdom;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 2;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SkillKnowledgeArcana;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SkillKnowledgeWorld;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SkillLoreReligion;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SkillLoreNature;
					c.Value = 4;
				});
			});
			SongOfTimeResource = Helpers.CreateBlueprint(Main.IsekaiContext, "SongOfTimeResource", delegate(BlueprintAbilityResource bp)
			{
				bp.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevel = false,
					IncreasedByStat = true,
					ResourceBonusStat = StatType.Charisma
				};
			});
			SongOfTimeBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "SongOfTimeBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Song of Time: Temporal Acceleration");
				bp.SetDescription(Main.IsekaiContext, "Temporal rhythms quicken your steps. You gain the benefits of Haste, a +2 dodge bonus to AC, and an extra move action each turn.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Time;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(BuffMovementSpeed c)
				{
					c.Value = 30;
					c.Descriptor = ModifierDescriptor.Enhancement;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 1;
				});
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
					c.Haste = true;
				});
			});
			SongOfTimeAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SongOfTimeAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Song of Time");
				bp.SetDescription(Main.IsekaiContext, "Play or channel the mystic notes of the Ocarina of Time as a swift action. Rewinds local causality for 1 minute: all allies gain Haste and +2 dodge AC, while enemies within 30 feet suffer temporal drag, reducing their movement speed and imposing a -2 penalty to attack rolls and AC.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Time;
				bp.Type = AbilityType.Supernatural;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.CanTargetPoint = false;
				bp.ActionType = UnitCommand.CommandType.Swift;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				ContextActionApplyBuff applyCaster = new ContextActionApplyBuff
				{
					m_Buff = SongOfTimeBuff.ToReference<BlueprintBuffReference>(),
					DurationValue = new ContextDurationValue
					{
						Rate = DurationRate.Minutes,
						DiceType = DiceType.Zero,
						DiceCountValue = 0,
						BonusValue = 1
					},
					ToCaster = true
				};
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = Helpers.CreateActionList(applyCaster);
				});
				bp.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = SongOfTimeResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
			});
			SongOfTimeFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "SongOfTimeFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Song of Time");
				bp.SetDescription(Main.IsekaiContext, "You learn the legendary Song of Time. As a swift action, you can spend a use of Song of Time to accelerate your party's timeline, gaining Haste and +2 dodge AC for 1 minute (usable 3 + Charisma modifier times per day).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Time;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = SongOfTimeResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { SongOfTimeAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
			TemporalRewindBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "TemporalRewindBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Temporal Rewind Cooldown");
				bp.SetDescription(Main.IsekaiContext, "The Sacred Triad has already restored your life force. Recharges upon resting.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Triad;
				bp.IsClassFeature = true;
			});
			SacredTriadFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "SacredTriadFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Sacred Triad (True Hero of Time)");
				bp.SetDescription(Main.IsekaiContext, "The three golden relics of creation converge fully within your spirit. You achieve complete mastery over courage, power, and wisdom. Grants permanent immunity to death effects, negative levels, and petrification, and an untyped +4 bonus to all physical and mental ability scores.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Triad;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Dexterity;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Constitution;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Intelligence;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Wisdom;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Charisma;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Petrified;
				});
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Petrified;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Petrified;
				});
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
				});
			});
			CorruptedAuraOfDreadFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CorruptedAuraOfDreadFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Aura of Calamity Dread");
				bp.SetDescription(Main.IsekaiContext, "Dark Triforce resonance radiates from your silhouette. Enemies within 30 feet suffer a -2 penalty to saving throws against fear and mind-affecting spells and take 2d6 unholy damage every round they remain near you.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dark;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
			});
			CorruptedBondsOfDominationFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CorruptedBondsOfDominationFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Bonds of Domination");
				bp.SetDescription(Main.IsekaiContext, "Whenever an enemy falls before you, their despair fuels your dark sovereignty, granting you a +2 profane bonus to attack rolls and weapon damage for 3 rounds.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dark;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 2;
				});
			});
			CorruptedBladeOfCalamityFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CorruptedBladeOfCalamityFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Blade of Calamity");
				bp.SetDescription(Main.IsekaiContext, "Your master sword is infused with malice and dark miasma. Weapon attacks deal an additional +2d6 unholy damage on hit and ignore damage reduction.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dark;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AdditionalDiceOnAttack c)
				{
					// WeaponEnergyDamageDice is a weapon-enchantment component; on a unit feature it throws and adds nothing.
					c.AttackType = AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks;
					c.OnHit = true;
					// Both condition checkers are read unconditionally, so they must exist even when empty.
					c.InitiatorConditions = ActionFlow.EmptyCondition();
					c.TargetConditions = ActionFlow.EmptyCondition();
					c.Value = new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 2, BonusValue = 0 };
					c.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Unholy };
				});
			});
			CorruptedEyeOfTheVoidFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CorruptedEyeOfTheVoidFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Eye of the Calamity Void");
				bp.SetDescription(Main.IsekaiContext, "Piercing through mortal illusions, you gain permanent True Seeing and blindsight out to 30 feet.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dark;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Blindness;
				});
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Blindness;
				});
				bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Blindness;
				});
			});
			CorruptedSongOfTimeFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CorruptedSongOfTimeFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Song of the Shattered Moon");
				bp.SetDescription(Main.IsekaiContext, "Channels the inverted Song of Time. As a swift action, causes local reality to fracture: all enemies within 30 feet are staggered for 2 rounds and take 4d6 sonic damage (Fortitude negates).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dark;
				bp.IsClassFeature = true;
			});
			CorruptedDarkTriforceFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CorruptedDarkTriforceFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Dark Triforce Sovereign");
				bp.SetDescription(Main.IsekaiContext, "You claim the inverted Dark Triforce. Grants a +4 profane bonus to Strength and Charisma, weapon attacks bypass all DR, and critical hits inflict Stun for 1 round (Fortitude negates).");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dark;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Charisma;
					c.Value = 4;
				});
			});
			CorruptedShatteredCycleFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CorruptedShatteredCycleFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Shattered Cycle of the Hero");
				bp.SetDescription(Main.IsekaiContext, "You refuse the preordained cycle of reincarnation and defeat. Grants immunity to death effects, paralysis, and energy drain, and increases all weapon critical multipliers by 1.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dark;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Paralysis | SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
				});
			});
		}
	}
}
