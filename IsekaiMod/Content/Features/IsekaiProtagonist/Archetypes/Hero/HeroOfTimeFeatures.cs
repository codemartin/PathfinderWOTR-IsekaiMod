using IsekaiMod.Components;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Hero;

internal static class HeroOfTimeFeatures
{
	private static readonly Sprite Icon_Courage = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e45ab30f49215054e83b4ea12165409f"))?.m_Icon;

	private static readonly Sprite Icon_Power = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("b175001b42b1a02479881b72fe132116"))?.m_Icon ?? Icon_Courage;

	private static readonly Sprite Icon_Wisdom = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("73fc1d19f14339042ba5af34872c1745"))?.m_Icon ?? Icon_Courage;

	private static readonly Sprite Icon_Time = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("03464790f40c3c24aa684b57155f3280"))?.m_Icon ?? Icon_Courage;

	private static readonly Sprite Icon_Triad = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("faf473e3a977fd4428cd3f1a526346d2"))?.m_Icon ?? Icon_Courage;

	private static readonly Sprite Icon_Dark = Icon_Courage;

	public static BlueprintFeature TriforceCourageFeature;

	public static BlueprintFeature TriforcePowerFeature;

	public static BlueprintBuff TriforcePowerBuff;

	public static BlueprintFeature TriforceCourageWardFeature;

	public static BlueprintFeature TriforceWisdomFeature;

	public static BlueprintFeature TriforceBeaconFeature;

	public static BlueprintAbilityResource SongOfTimeResource;

	public static BlueprintBuff SongOfTimeBuff;

	public static BlueprintAbility SongOfTimeAbility;

	public static BlueprintFeature SongOfTimeFeature;

	public static BlueprintFeature SacredTriadFeature;

	public static BlueprintFeature CorruptedAuraOfDreadFeature;

	public static BlueprintFeature CorruptedBondsOfDominationFeature;

	public static BlueprintFeature CorruptedBladeOfCalamityFeature;

	public static BlueprintFeature CorruptedEyeOfTheVoidFeature;

	public static BlueprintFeature CorruptedSongOfTimeFeature;

	public static BlueprintFeature CorruptedDarkTriforceFeature;

	public static BlueprintFeature CorruptedShatteredCycleFeature;

	public static void Add()
	{
		BlueprintBuff TriforceCourageAuraBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "TriforceCourageAuraBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Triforce of Courage");
			bp.SetDescription(Main.IsekaiContext, "The golden emblem of valor burns away hesitation. You gain a +2 morale bonus to attack rolls, weapon damage, and saving throws against Fear and Mind-Affecting effects.");
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
			bp.AddComponent(delegate(SavingThrowBonusAgainstDescriptor c)
			{
				c.SpellDescriptor = SpellDescriptor.Fear | SpellDescriptor.Shaken | SpellDescriptor.Frightened;
				c.ModifierDescriptor = ModifierDescriptor.Morale;
				c.Value = 2;
				c.Bonus = 0;
			});
			bp.AddComponent(delegate(SavingThrowBonusAgainstDescriptor c)
			{
				c.SpellDescriptor = SpellDescriptor.MindAffecting;
				c.ModifierDescriptor = ModifierDescriptor.Morale;
				c.Value = 2;
				c.Bonus = 0;
			});
		});
		BlueprintAbilityAreaEffect TriforceCourageArea = Helpers.CreateBlueprint(Main.IsekaiContext, "TriforceCourageArea", delegate(BlueprintAbilityAreaEffect bp)
		{
			bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
			bp.SpellResistance = false;
			bp.AggroEnemies = false;
			bp.AffectEnemies = false;
			bp.Shape = AreaEffectShape.Cylinder;
			bp.Size = new Feet(30f);
			bp.Fx = new PrefabLink();
			bp.AddUnconditionalAuraEffect(TriforceCourageAuraBuff.ToReference<BlueprintBuffReference>());
		});
		BlueprintBuff TriforceCourageAuraCarrierBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "TriforceCourageAuraCarrierBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Triforce of Courage Aura");
			bp.SetDescription(Main.IsekaiContext, "Emits a 30-foot aura of valor around you.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Courage;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddAreaEffect c)
			{
				c.m_AreaEffect = TriforceCourageArea.ToReference<BlueprintAbilityAreaEffectReference>();
			});
		});
		TriforceCourageFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "TriforceCourageFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Triforce of Courage");
			bp.SetDescription(Main.IsekaiContext, "The sacred emblem of unbroken valor shines upon your hand. You and all allies within 30 feet gain a +2 morale bonus to attack rolls, weapon damage, and saving throws against Fear and Mind-Affecting effects.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Courage;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { TriforceCourageAuraCarrierBuff.ToReference<BlueprintUnitFactReference>() };
			});
		});
		BlueprintBuff TriforceCourageWardBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "TriforceCourageWardBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Shield of Courage");
			bp.SetDescription(Main.IsekaiContext, "The Hero's courage is spent on you rather than on himself. You gain DR 2/-.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Courage;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 2;
			});
		});
		BlueprintAbilityAreaEffect TriforceCourageWardArea = Helpers.CreateBlueprint(Main.IsekaiContext, "TriforceCourageWardArea", delegate(BlueprintAbilityAreaEffect bp)
		{
			bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
			bp.SpellResistance = false;
			bp.AggroEnemies = false;
			bp.AffectEnemies = false;
			bp.Shape = AreaEffectShape.Cylinder;
			bp.Size = new Feet(30f);
			bp.Fx = new PrefabLink();
			bp.AddUnconditionalAuraEffect(TriforceCourageWardBuff.ToReference<BlueprintBuffReference>());
		});
		BlueprintBuff TriforceCourageWardCarrierBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "TriforceCourageWardCarrierBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Shield of Courage Aura");
			bp.SetDescription(Main.IsekaiContext, "Emits a 30-foot aura that takes the edge off every blow aimed at your companions.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Courage;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddAreaEffect c)
			{
				c.m_AreaEffect = TriforceCourageWardArea.ToReference<BlueprintAbilityAreaEffectReference>();
			});
		});
		TriforceCourageWardFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "TriforceCourageWardFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Shield of Courage");
			bp.SetDescription(Main.IsekaiContext, "Courage is not what you keep, it is what you put between your companions and the blow. You and all allies within 30 feet gain DR 2/-.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Courage;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { TriforceCourageWardCarrierBuff.ToReference<BlueprintUnitFactReference>() };
			});
		});
		TriforcePowerBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "TriforcePowerBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Triforce of Power: Awakened Force");
			bp.SetDescription(Main.IsekaiContext, "Raw sacred might surges through your sinews. Weapon attacks deal an additional +1d6 force damage, and you gain a +2 inherent bonus to Strength and Constitution.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Power;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Strength;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Constitution;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddAdditionalWeaponDamage c)
			{
				c.DamageType = new DamageTypeDescription
				{
					Type = DamageType.Force
				};
				c.Value = new ContextDiceValue
				{
					DiceType = DiceType.D6,
					DiceCountValue = 1,
					BonusValue = 0
				};
			});
			bp.AddComponent(delegate(CriticalConfirmationBonus c)
			{
				c.Value = 0;
				c.Bonus = 2;
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
			bp.SetDescription(Main.IsekaiContext, "Temporal rhythms quicken your steps. You gain a +30 foot enhancement bonus to speed, a +1 enhancement bonus to attack rolls, a +2 dodge bonus to AC, and one extra attack when making a full attack (this does not stack with haste).");
			((BlueprintUnitFact)bp).m_Icon = Icon_Time;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(BuffMovementSpeed c)
			{
				c.Value = 30;
				c.Descriptor = ModifierDescriptor.Enhancement;
				c.ContextBonus = new ContextValue();
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
			bp.SetDescription(Main.IsekaiContext, "Play or channel the mystic notes of the Ocarina of Time as a swift action. Rewinds local causality for 1 minute: you and every ally within 30 feet gain the benefits of Haste, which do not stack with another haste effect, and a +2 dodge bonus to AC.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Time;
			bp.Type = AbilityType.Supernatural;
			bp.Range = AbilityRange.Personal;
			bp.CanTargetSelf = true;
			bp.CanTargetPoint = false;
			bp.ActionType = UnitCommand.CommandType.Swift;
			bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
			bp.AddComponent(delegate(AbilityTargetsAround c)
			{
				c.m_Radius = new Feet(30f);
				c.m_TargetType = TargetType.Ally;
			});
			ContextActionApplyBuff applyToAlly = new ContextActionApplyBuff
			{
				m_Buff = SongOfTimeBuff.ToReference<BlueprintBuffReference>(),
				DurationValue = new ContextDurationValue
				{
					Rate = DurationRate.Minutes,
					DiceType = DiceType.Zero,
					DiceCountValue = 0,
					BonusValue = 1
				}
			};
			bp.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = Helpers.CreateActionList(applyToAlly);
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
			bp.SetDescription(Main.IsekaiContext, "You learn the legendary Song of Time. As a swift action, you can spend a use of Song of Time to accelerate your party's timeline: you and every ally within 30 feet gain Haste and +2 dodge AC for 1 minute (usable 3 + Charisma modifier times per day).");
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
		BlueprintBuff TriforceBeaconAuraBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "TriforceBeaconAuraBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Beacon of the Triforce");
			bp.SetDescription(Main.IsekaiContext, "A share of the golden relics rests on you as well. You gain a +2 sacred bonus to Strength and Charisma.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Triad;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.Strength;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.Charisma;
				c.Value = 2;
			});
		});
		BlueprintAbilityAreaEffect TriforceBeaconArea = Helpers.CreateBlueprint(Main.IsekaiContext, "TriforceBeaconArea", delegate(BlueprintAbilityAreaEffect bp)
		{
			bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
			bp.SpellResistance = false;
			bp.AggroEnemies = false;
			bp.AffectEnemies = false;
			bp.Shape = AreaEffectShape.Cylinder;
			bp.Size = new Feet(30f);
			bp.Fx = new PrefabLink();
			bp.AddUnconditionalAuraEffect(TriforceBeaconAuraBuff.ToReference<BlueprintBuffReference>());
		});
		BlueprintBuff TriforceBeaconCarrierBuff = Helpers.CreateBlueprint(Main.IsekaiContext, "TriforceBeaconCarrierBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Beacon of the Triforce Aura");
			bp.SetDescription(Main.IsekaiContext, "Emits a 30-foot aura in which the golden relics are shared rather than owned.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Triad;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddAreaEffect c)
			{
				c.m_AreaEffect = TriforceBeaconArea.ToReference<BlueprintAbilityAreaEffectReference>();
			});
		});
		TriforceBeaconFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "TriforceBeaconFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Beacon of the Triforce");
			bp.SetDescription(Main.IsekaiContext, "The relics of Courage and Power are a trust, not a crown. You refuse to hold them alone: you and all allies within 30 feet gain a +2 sacred bonus to Strength and Charisma.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Triad;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[1] { TriforceBeaconCarrierBuff.ToReference<BlueprintUnitFactReference>() };
			});
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
			bp.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
			});
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Death;
			});
			bp.AddComponent<AddImmunityToEnergyDrain>();
		});
		CorruptedAuraOfDreadFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CorruptedAuraOfDreadFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Aura of Calamity Dread");
			bp.SetDescription(Main.IsekaiContext, "Dark Triforce resonance radiates from your silhouette, sharpening every strike you make. You gain a +2 profane bonus to attack rolls.");
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
			bp.SetDescription(Main.IsekaiContext, "The despair of everything that has fallen before you fuels your dark sovereignty. You gain a +2 profane bonus to weapon damage.");
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
			bp.SetDescription(Main.IsekaiContext, "Your master sword is infused with malice and dark miasma. Weapon attacks deal an additional 2d6 unholy damage on hit.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Dark;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddAdditionalWeaponDamage c)
			{
				c.DamageType = new DamageTypeDescription
				{
					Type = DamageType.Energy,
					Energy = DamageEnergyType.Unholy
				};
				c.Value = new ContextDiceValue
				{
					DiceType = DiceType.D6,
					DiceCountValue = 2,
					BonusValue = 0
				};
			});
		});
		CorruptedEyeOfTheVoidFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CorruptedEyeOfTheVoidFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Eye of the Calamity Void");
			bp.SetDescription(Main.IsekaiContext, "Piercing through mortal illusions, you gain permanent True Seeing and can never be blinded.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Dark;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddCondition c)
			{
				c.Condition = UnitCondition.TrueSeeing;
			});
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Blindness;
			});
		});
		CorruptedSongOfTimeFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CorruptedSongOfTimeFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Song of the Shattered Moon");
			bp.SetDescription(Main.IsekaiContext, "The inverted Song of Time runs through you constantly rather than being played. Time slips for everything around you: you gain a +2 profane bonus to Armor Class and a +10 foot enhancement bonus to movement speed.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Dark;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Profane;
				c.Stat = StatType.AC;
				c.Value = 2;
			});
			bp.AddComponent(delegate(BuffMovementSpeed c)
			{
				c.Value = 10;
				c.Descriptor = ModifierDescriptor.Enhancement;
				c.ContextBonus = new ContextValue();
			});
		});
		CorruptedDarkTriforceFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CorruptedDarkTriforceFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Dark Triforce Sovereign");
			bp.SetDescription(Main.IsekaiContext, "You claim the inverted Dark Triforce. Grants a +4 profane bonus to Strength and Charisma.");
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
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Death;
			});
			bp.AddComponent<AddImmunityToEnergyDrain>();
			bp.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Paralyzed;
			});
			var array = new[]
			{
				new
				{
					Light = false,
					TwoHanded = false,
					Ranged = false
				},
				new
				{
					Light = true,
					TwoHanded = false,
					Ranged = false
				},
				new
				{
					Light = false,
					TwoHanded = true,
					Ranged = false
				},
				new
				{
					Light = false,
					TwoHanded = false,
					Ranged = true
				}
			};
			foreach (var anon in array)
			{
				var captured = anon;
				bp.AddComponent(delegate(WeaponParametersCriticalMultiplierIncrease c)
				{
					c.SpecificWeaponBlueprint = false;
					c.Light = captured.Light;
					c.TwoHanded = captured.TwoHanded;
					c.Ranged = captured.Ranged;
					c.AdditionalMultiplier = 1;
				});
			}
		});
		AddTriforcePathSelection();
	}

	private static void AddTriforcePathSelection()
	{
		BlueprintFeature radiant = Helpers.CreateBlueprint(Main.IsekaiContext, "HeroRadiantTriforcePathFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Golden Triforce");
			bp.SetDescription(Main.IsekaiContext, "You carry the three golden relics of creation as they were meant to be carried. You gain the Triforce of Courage now, the Shield of Courage at 3rd level, the Triforce of Power at 7th, the Song of Time at 10th, the Triforce of Wisdom at 13th, the Beacon of the Triforce at 15th, and the Sacred Triad at 20th.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Triad;
			bp.IsClassFeature = true;
		});
		Ladder(radiant, new(BlueprintFeature, int)[7]
		{
			(TriforceCourageFeature, 1),
			(TriforceCourageWardFeature, 3),
			(TriforcePowerFeature, 7),
			(SongOfTimeFeature, 10),
			(TriforceWisdomFeature, 13),
			(TriforceBeaconFeature, 15),
			(SacredTriadFeature, 20)
		});
		BlueprintFeature corrupted = Helpers.CreateBlueprint(Main.IsekaiContext, "HeroCorruptedTriforcePathFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Dark Triforce");
			bp.SetDescription(Main.IsekaiContext, "The relics answered you, and something answered with them. You gain the Aura of Calamity Dread now, Bonds of Domination at 3rd level, the Blade of Calamity at 7th, the Song of the Shattered Moon at 10th, the Eye of the Calamity Void at 13th, Dark Triforce Sovereign at 15th, and the Shattered Cycle of the Hero at 20th.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Dark;
			bp.IsClassFeature = true;
		});
		Ladder(corrupted, new(BlueprintFeature, int)[7]
		{
			(CorruptedAuraOfDreadFeature, 1),
			(CorruptedBondsOfDominationFeature, 3),
			(CorruptedBladeOfCalamityFeature, 7),
			(CorruptedSongOfTimeFeature, 10),
			(CorruptedEyeOfTheVoidFeature, 13),
			(CorruptedDarkTriforceFeature, 15),
			(CorruptedShatteredCycleFeature, 20)
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "HeroTriforcePathSelection", delegate(BlueprintFeatureSelection bp)
		{
			bp.SetName(Main.IsekaiContext, "Path of the Triforce");
			bp.SetDescription(Main.IsekaiContext, "The golden relics of creation have chosen you. Decide now what they will make of you: the Golden Triforce of the hero the legends remember, or the Dark Triforce of the one they warn about. Your choice decides which relic powers you gain as you rise in level.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Courage;
			bp.Ranks = 1;
			bp.IsClassFeature = true;
			bp.IgnorePrerequisites = true;
			bp.m_AllFeatures = new BlueprintFeatureReference[2]
			{
				radiant.ToReference<BlueprintFeatureReference>(),
				corrupted.ToReference<BlueprintFeatureReference>()
			};
			bp.m_Features = (BlueprintFeatureReference[])bp.m_AllFeatures.Clone();
		});
		static void Ladder(BlueprintFeature path, params (BlueprintFeature feature, int level)[] rungs)
		{
			for (int i = 0; i < rungs.Length; i++)
			{
				(BlueprintFeature, int) tuple = rungs[i];
				if (tuple.Item1 != null)
				{
					(BlueprintFeature feature, int level) captured = tuple;
					if (captured.level <= 1)
					{
						path.AddComponent(delegate(AddFacts c)
						{
							c.m_Facts = new BlueprintUnitFactReference[1] { captured.feature.ToReference<BlueprintUnitFactReference>() };
						});
					}
					else
					{
						path.AddComponent(delegate(AddFeatureOnClassLevel c)
						{
							c.m_Class = IsekaiProtagonistClass.GetReference();
							c.Level = captured.level;
							c.m_Feature = captured.feature.ToReference<BlueprintFeatureReference>();
							c.BeforeThisLevel = false;
							c.m_AdditionalClasses = new BlueprintCharacterClassReference[0];
							c.m_Archetypes = new BlueprintArchetypeReference[0];
						});
					}
				}
			}
		}
	}
}
