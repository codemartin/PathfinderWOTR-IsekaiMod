using System;
using System.Collections.Generic;
using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Constellations
{
	internal class DivineSponsorshipStore
	{
		public static BlueprintFeature CosmicSponsorshipExchangeFeature;

		public static void Add()
		{
			Sprite Icon_Coin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
			BlueprintItem ResurrectionScroll = BlueprintTools.GetBlueprint<BlueprintItem>("6169a9e10d32c524ea44edb47ebd93cf");
			List<BlueprintAbility> storeAbilities = new List<BlueprintAbility>();
			BlueprintFeature KeyOfInfinitePathwaysFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "KeyOfInfinitePathwaysFeature", delegate(BlueprintFeature blueprintFeature2)
			{
				blueprintFeature2.SetName(Main.IsekaiContext, "Key of Infinite Pathways");
				blueprintFeature2.SetDescription(Main.IsekaiContext, "A metaphysical skeleton key that transcends dimensional timelines and cosmic law. Unlocks all alignment and mythic path restrictions within the Cosmic Sponsorship Store, allowing you to acquire boons from any mythic path simultaneously.");
				((BlueprintUnitFact)blueprintFeature2).m_Icon = Icon_Coin;
				blueprintFeature2.IsClassFeature = true;
			});
			BlueprintAbility item = Helpers.CreateBlueprint(Main.IsekaiContext, "BuyKeyOfInfinitePathwaysAbility", delegate(BlueprintAbility blueprintAbility)
			{
				blueprintAbility.SetName(Main.IsekaiContext, "Meta-Boon: Key of Infinite Pathways");
				blueprintAbility.SetDescription(Main.IsekaiContext, "Spend 5,000 Cosmic Coins to permanently shatter all alignment and mythic path prerequisites in the Cosmic Store.");
				((BlueprintUnitFact)blueprintAbility).m_Icon = Icon_Coin;
				blueprintAbility.Type = AbilityType.Special;
				blueprintAbility.Range = AbilityRange.Personal;
				blueprintAbility.CanTargetSelf = true;
				blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				blueprintAbility.ActionType = UnitCommand.CommandType.Standard;
				blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionSpendCoinsAndGrantFact a)
					{
						a.Cost = 5000;
						a.m_Fact = KeyOfInfinitePathwaysFeature.ToReference<BlueprintUnitFactReference>();
					});
				});
			});
			storeAbilities.Add(item);
			List<(string name, BlueprintBuff buff, int cost, string displayName, string desc)> blessingBuffs = new List<(string, BlueprintBuff, int, string, string)>();
			AddBlessing("CaydenReserveBrew", "Cayden's Reserve Brew", "Grants immunity to fatigue/exhaustion, and a +2 morale bonus to attack and damage rolls.", 100, delegate(BlueprintBuff obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 2;
				});
				obj.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Fatigued;
				});
				obj.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Exhausted;
				});
			});
			AddBlessing("StarlightAegis", "Starlight Aegis of Desna", "Grants a +2 luck bonus to AC and all saving throws.", 250, delegate(BlueprintBuff obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
			});
			AddBlessing("EssenceOfLurker", "Essence of the Lurker", "Grants a +2 profane bonus to the DC of Conjuration, Transmutation, and Illusion spells.", 500, delegate(BlueprintBuff obj)
			{
				obj.AddComponent(delegate(IncreaseSpellSchoolDC c)
				{
					c.School = SpellSchool.Conjuration;
					c.BonusDC = 2;
					c.Descriptor = ModifierDescriptor.Profane;
				});
				obj.AddComponent(delegate(IncreaseSpellSchoolDC c)
				{
					c.School = SpellSchool.Transmutation;
					c.BonusDC = 2;
					c.Descriptor = ModifierDescriptor.Profane;
				});
				obj.AddComponent(delegate(IncreaseSpellSchoolDC c)
				{
					c.School = SpellSchool.Illusion;
					c.BonusDC = 2;
					c.Descriptor = ModifierDescriptor.Profane;
				});
			});
			AddBlessing("LanternTricksterGlow", "Lantern King's Prankster Veil", "Grants a +3 competence bonus to Trickery and Stealth, plus 20% concealment.", 300, delegate(BlueprintBuff obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillThievery;
					c.Value = 3;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.SkillStealth;
					c.Value = 3;
				});
				obj.AddComponent(delegate(AddConcealment c)
				{
					c.Concealment = Concealment.Partial;
				});
			});
			AddBlessing("QueenOfTheSeasFavor", "Favor of the Pirate Queen", "Grants +10 ft movement speed, freedom of movement, and +2 attack against staggered or flat-footed foes.", 300, delegate(BlueprintBuff obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
				obj.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Paralyzed;
				});
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				obj.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Fatigue | SpellDescriptor.Exhausted | SpellDescriptor.Paralysis;
				});
				obj.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Fatigue | SpellDescriptor.Exhausted | SpellDescriptor.Paralysis;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
			});
			AddBlessing("ChaldiraHeartyLuck", "Chaldira's Audacious Fortune", "Grants a +2 luck bonus to critical confirmation rolls, initiative, and all saving throws.", 350, delegate(BlueprintBuff obj)
			{
				obj.AddComponent(delegate(CriticalConfirmationBonus c)
				{
					c.Bonus = 2;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.Initiative;
					c.Value = 2;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
			});
			AddBlessing("IronLordFury", "Fury of Our Lord in Iron", "Grants a +4 morale bonus to melee weapon damage rolls and +2 to Combat Maneuver Bonus.", 400, delegate(BlueprintBuff obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 4;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalCMB;
					c.Value = 2;
				});
			});
			AddBlessing("AllSeeingGlyph", "All-Seeing Glyph of Nethys", "Grants a +2 caster level bonus to all spells and +4 bonus to Spell Penetration.", 500, delegate(BlueprintBuff obj)
			{
				obj.AddComponent(delegate(SpellPenetrationBonus c)
				{
					c.Value = 4;
					c.Descriptor = ModifierDescriptor.Insight;
				});
			});
			AddBlessing("KissOfTheSavoredSting", "Kiss of the Savored Sting", "Grants a +3 bonus on attack rolls with Attacks of Opportunity and adds +1d6 acid damage to melee hits.", 350, delegate(BlueprintBuff obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AttackOfOpportunityCount;
					c.Value = 3;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 4;
				});
			});
			AddBlessing("LadyOfGravesWarding", "Warding of the Lady of Graves", "Grants complete immunity to death effects, negative energy damage, and energy drain.", 450, delegate(BlueprintBuff obj)
			{
				obj.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Curse | SpellDescriptor.Death;
				});
				obj.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Curse | SpellDescriptor.Death;
				});
			});
			AddBlessing("InheritorVanguard", "Vanguard of the Inheritor", "Grants a +3 sacred bonus to AC and saving throws against evil creatures.", 450, delegate(BlueprintBuff obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AC;
					c.Value = 3;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveFortitude;
					c.Value = 3;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = 3;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 3;
				});
			});
			AddBlessing("ArchfiendsCovenant", "Covenant of the Archfiend", "Grants a +2 profane bonus to all spell DCs and a +4 profane bonus to Persuasion checks.", 500, delegate(BlueprintBuff obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.SkillPersuasion;
					c.Value = 4;
				});
			});
			AddBlessing("RoseGuardsResolve", "Resolve of the Rose Guard", "Grants immunity to fear, charm, and compulsion, plus a +2 bonus to attack when adjacent to allies.", 300, delegate(BlueprintBuff obj)
			{
				obj.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Compulsion | SpellDescriptor.Charm;
				});
				obj.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Compulsion | SpellDescriptor.Charm;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
			});
			AddBlessing("SilentShroud", "Silent Shroud of the Butterfly", "Grants True Seeing, blindsense 30 ft, and immunity to silence and gaze attacks.", 400, delegate(BlueprintBuff obj)
			{
				obj.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.SightBased | SpellDescriptor.GazeAttack;
				});
			});
			List<(string name, BlueprintFeature feat, int cost, AlignmentMaskType alignment, string displayName, string desc)> mythicBoonList = new List<(string, BlueprintFeature, int, AlignmentMaskType, string, string)>();
			AddMythicBoon("BoonOfHeavenlyHost", "Boon of the Heavenly Host (Angel)", "Grants a permanent +4 sacred bonus to AC and all weapon attacks deal +2d6 holy damage.", 1500, AlignmentMaskType.Good | AlignmentMaskType.LawfulNeutral | AlignmentMaskType.LawfulEvil, delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 7;
				});
			});
			AddMythicBoon("BoonOfAbyssalRage", "Boon of Abyssal Rage (Demon)", "Grants a permanent +4 profane bonus to Strength and Constitution.", 1500, AlignmentMaskType.Evil | AlignmentMaskType.ChaoticGood | AlignmentMaskType.ChaoticNeutral, delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Constitution;
					c.Value = 4;
				});
			});
			AddMythicBoon("BoonOfLordOfBones", "Boon of the Lord of Bones (Lich)", "Grants permanent DR 10/Bludgeoning and Magic, and negative energy affinity.", 1500, AlignmentMaskType.Evil, delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 10;
					c.BypassedByMaterial = true;
					c.Material = PhysicalDamageMaterial.Adamantite;
				});
			});
			AddMythicBoon("BoonOfWildWonder", "Boon of Wild Wonder (Azata)", "Grants a permanent +3 morale bonus to all saving throws and +1 extra attack per round as if under Haste.", 1500, AlignmentMaskType.Good | AlignmentMaskType.ChaoticNeutral | AlignmentMaskType.ChaoticEvil, delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveFortitude;
					c.Value = 3;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveReflex;
					c.Value = 3;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveWill;
					c.Value = 3;
				});
				obj.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
					c.Haste = true;
				});
			});
			AddMythicBoon("BoonOfCosmicJester", "Boon of the Cosmic Jester (Trickster)", "Increases critical threat range of all weapons by 1 and grants +2d6 Sneak Attack.", 1500, AlignmentMaskType.Chaotic, delegate(BlueprintFeature obj)
			{
				BlueprintFeature sneak = BlueprintTools.GetBlueprint<BlueprintFeature>("9b9eac6709e1c084cb18c3a366e0ec87");
				obj.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[2]
					{
						sneak.ToReference<BlueprintUnitFactReference>(),
						sneak.ToReference<BlueprintUnitFactReference>()
					};
				});
			});
			AddMythicBoon("BoonOfMonadicOrder", "Boon of Monadic Order (Aeon)", "Grants a permanent +4 insight bonus to attack rolls and AC, plus immunity to mind-affecting effects.", 1500, AlignmentMaskType.Lawful, delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				obj.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting;
				});
			});
			AddMythicBoon("BoonOfHellfireContract", "Boon of Hellfire Contract (Devil)", "Grants fire immunity and a +3 profane bonus to the DC of mind-affecting and evil spells.", 1500, AlignmentMaskType.Evil | AlignmentMaskType.LawfulGood | AlignmentMaskType.LawfulNeutral, delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Fire;
					c.Value = 100;
				});
			});
			AddMythicBoon("BoonOfGoldenDragon", "Boon of the Golden Dragon (Gold Dragon)", "Grants a permanent +4 inherent bonus to all 6 ability scores and +10 hit points per character level.", 2000, AlignmentMaskType.Good, delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Dexterity;
					c.Value = 4;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Constitution;
					c.Value = 4;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Intelligence;
					c.Value = 4;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Wisdom;
					c.Value = 4;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Charisma;
					c.Value = 4;
				});
			});
			AddMythicBoon("BoonOfPrimevalSwarm", "Boon of the Primeval Swarm (Swarm)", "Grants permanent DR 10/-, immunity to flanking, and 20% swarm concealment.", 1500, AlignmentMaskType.Evil, delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 10;
				});
				obj.AddComponent(delegate(AddConcealment c)
				{
					c.Concealment = Concealment.Partial;
				});
			});
			AddMythicBoon("BoonOfUnboundMortal", "Boon of the Unbound Mortal (Legend)", "Grants a permanent +4 bonus to all base saving throws and +4 to all ability scores.", 2000, AlignmentMaskType.Any, delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Other;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Other;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Other;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Other;
					c.Stat = StatType.Strength;
					c.Value = 4;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Other;
					c.Stat = StatType.Dexterity;
					c.Value = 4;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Other;
					c.Stat = StatType.Constitution;
					c.Value = 4;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Other;
					c.Stat = StatType.Intelligence;
					c.Value = 4;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Other;
					c.Stat = StatType.Wisdom;
					c.Value = 4;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Other;
					c.Stat = StatType.Charisma;
					c.Value = 4;
				});
			});
			BlueprintAbility item2 = Helpers.CreateBlueprint(Main.IsekaiContext, "ElixirOfInstantRejuvenationAbility", delegate(BlueprintAbility blueprintAbility)
			{
				blueprintAbility.SetName(Main.IsekaiContext, "Consumable: Elixir of Instant Rejuvenation");
				blueprintAbility.SetDescription(Main.IsekaiContext, "Spend 400 Cosmic Coins to immediately cure all fatigue, exhaustion, blindness, and deafness, and fully restore all daily spell slots and class ability uses.");
				((BlueprintUnitFact)blueprintAbility).m_Icon = Icon_Coin;
				blueprintAbility.Type = AbilityType.Special;
				blueprintAbility.Range = AbilityRange.Personal;
				blueprintAbility.CanTargetSelf = true;
				blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				blueprintAbility.ActionType = UnitCommand.CommandType.Standard;
				blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle<ContextActionRejuvenation>();
				});
			});
			storeAbilities.Add(item2);
			BlueprintBuff PaperFigurineBuff = TTCoreExtensions.CreateBuff("PaperFigurineBuff", delegate(BlueprintBuff blueprintBuff)
			{
				blueprintBuff.SetName(Main.IsekaiContext, "Paper Figurine of Substitute Life");
				blueprintBuff.SetDescription(Main.IsekaiContext, "A mystical talisman woven from otherworldly talisman paper. Absorbs damage that would kill you and immediately wards you with +100 temporary hit points.");
				((BlueprintUnitFact)blueprintBuff).m_Icon = Icon_Coin;
				blueprintBuff.AddComponent(delegate(TemporaryHitPointsFromAbilityValue c)
				{
					c.Value = 100;
				});
				blueprintBuff.m_Flags = BlueprintBuff.Flags.StayOnDeath;
			});
			BlueprintAbility item3 = Helpers.CreateBlueprint(Main.IsekaiContext, "PaperFigurineOfSubstituteLifeAbility", delegate(BlueprintAbility blueprintAbility)
			{
				blueprintAbility.SetName(Main.IsekaiContext, "Talisman: Paper Figurine of Substitute Life");
				blueprintAbility.SetDescription(Main.IsekaiContext, "Spend 750 Cosmic Coins to attune a paper substitute figurine. Grants +100 temporary hit points.");
				((BlueprintUnitFact)blueprintAbility).m_Icon = Icon_Coin;
				blueprintAbility.Type = AbilityType.Special;
				blueprintAbility.Range = AbilityRange.Personal;
				blueprintAbility.CanTargetSelf = true;
				blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				blueprintAbility.ActionType = UnitCommand.CommandType.Standard;
				blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionSpendCoinsAndApplyBuff a)
					{
						a.Cost = 750;
						a.m_Buff = PaperFigurineBuff.ToReference<BlueprintBuffReference>();
						a.DurationHours = 24;
					});
				});
			});
			storeAbilities.Add(item3);
			BlueprintBuff SoulAnchorBuff = TTCoreExtensions.CreateBuff("DraughtOfAbsoluteSoulAnchorBuff", delegate(BlueprintBuff blueprintBuff)
			{
				blueprintBuff.SetName(Main.IsekaiContext, "Absolute Soul Anchor");
				blueprintBuff.SetDescription(Main.IsekaiContext, "Anchors your astral soul firmly to your physical vessel. Immune to death effects, negative levels, and mental compulsion for 24 hours.");
				((BlueprintUnitFact)blueprintBuff).m_Icon = Icon_Coin;
				blueprintBuff.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Curse | SpellDescriptor.Death;
				});
				blueprintBuff.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Curse | SpellDescriptor.Death;
				});
				blueprintBuff.m_Flags = BlueprintBuff.Flags.StayOnDeath;
			});
			BlueprintAbility item4 = Helpers.CreateBlueprint(Main.IsekaiContext, "DraughtOfAbsoluteSoulAnchorAbility", delegate(BlueprintAbility blueprintAbility)
			{
				blueprintAbility.SetName(Main.IsekaiContext, "Consumable: Draught of Absolute Soul Anchor");
				blueprintAbility.SetDescription(Main.IsekaiContext, "Spend 500 Cosmic Coins to gain complete immunity to death effects, negative levels, and mental compulsion for 24 hours.");
				((BlueprintUnitFact)blueprintAbility).m_Icon = Icon_Coin;
				blueprintAbility.Type = AbilityType.Special;
				blueprintAbility.Range = AbilityRange.Personal;
				blueprintAbility.CanTargetSelf = true;
				blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				blueprintAbility.ActionType = UnitCommand.CommandType.Standard;
				blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionSpendCoinsAndApplyBuff a)
					{
						a.Cost = 500;
						a.m_Buff = SoulAnchorBuff.ToReference<BlueprintBuffReference>();
						a.DurationHours = 24;
					});
				});
			});
			storeAbilities.Add(item4);
			BlueprintFeature OtherworlderMasteryFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "OtherworlderMasteryFeature", delegate(BlueprintFeature blueprintFeature2)
			{
				blueprintFeature2.SetName(Main.IsekaiContext, "Otherworlder's Martial Mastery");
				blueprintFeature2.SetDescription(Main.IsekaiContext, "Grants a permanent +2 competence bonus to all attack and damage rolls and +2 to all save DCs.");
				((BlueprintUnitFact)blueprintFeature2).m_Icon = Icon_Coin;
				blueprintFeature2.IsClassFeature = true;
				blueprintFeature2.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				blueprintFeature2.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 2;
				});
			});
			BlueprintAbility item5 = Helpers.CreateBlueprint(Main.IsekaiContext, "TomeOfOtherworlderMasteryAbility", delegate(BlueprintAbility blueprintAbility)
			{
				blueprintAbility.SetName(Main.IsekaiContext, "Grimoire: Tome of Otherworlder's Mastery");
				blueprintAbility.SetDescription(Main.IsekaiContext, "Spend 1,000 Cosmic Coins to absorb otherworlder wisdom. Grants a permanent +2 competence bonus to attack rolls, damage rolls, and save DCs.");
				((BlueprintUnitFact)blueprintAbility).m_Icon = Icon_Coin;
				blueprintAbility.Type = AbilityType.Special;
				blueprintAbility.Range = AbilityRange.Personal;
				blueprintAbility.CanTargetSelf = true;
				blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				blueprintAbility.ActionType = UnitCommand.CommandType.Standard;
				blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionSpendCoinsAndGrantFact a)
					{
						a.Cost = 1000;
						a.m_Fact = OtherworlderMasteryFeature.ToReference<BlueprintUnitFactReference>();
					});
				});
			});
			storeAbilities.Add(item5);
			BlueprintAbilityResource CosmicWishResource = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicWishResource", delegate(BlueprintAbilityResource blueprintAbilityResource)
			{
				blueprintAbilityResource.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 0,
					IncreasedByLevel = false,
					IncreasedByStat = false
				};
			});
			BlueprintAbility WishStrength = CreateWishVariant("WishStrengthAbility", "Strength", StatType.Strength);
			BlueprintAbility WishDexterity = CreateWishVariant("WishDexterityAbility", "Dexterity", StatType.Dexterity);
			BlueprintAbility WishConstitution = CreateWishVariant("WishConstitutionAbility", "Constitution", StatType.Constitution);
			BlueprintAbility WishIntelligence = CreateWishVariant("WishIntelligenceAbility", "Intelligence", StatType.Intelligence);
			BlueprintAbility WishWisdom = CreateWishVariant("WishWisdomAbility", "Wisdom", StatType.Wisdom);
			BlueprintAbility WishCharisma = CreateWishVariant("WishCharismaAbility", "Charisma", StatType.Charisma);
			BlueprintAbility WishWealth = Helpers.CreateBlueprint(Main.IsekaiContext, "WishWealthAbility", delegate(BlueprintAbility blueprintAbility)
			{
				blueprintAbility.SetName(Main.IsekaiContext, "Cosmic Wish: 500,000 Gold");
				blueprintAbility.SetDescription(Main.IsekaiContext, "Consumes 1 Wish charge to instantly manifest 500,000 Gold into your party purse.");
				((BlueprintUnitFact)blueprintAbility).m_Icon = Icon_Coin;
				blueprintAbility.Type = AbilityType.Special;
				blueprintAbility.Range = AbilityRange.Personal;
				blueprintAbility.CanTargetSelf = true;
				blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				blueprintAbility.ActionType = UnitCommand.CommandType.Standard;
				blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle<ContextActionWishWealth>();
				});
				blueprintAbility.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = CosmicWishResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
			});
			BlueprintAbility ModularCosmicWishAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ModularCosmicWishAbility", delegate(BlueprintAbility blueprintAbility)
			{
				blueprintAbility.SetName(Main.IsekaiContext, "Cosmic Wish (Field Cast)");
				blueprintAbility.SetDescription(Main.IsekaiContext, "Invoke cosmic reality manipulation. Consumes 1 Wish charge per cast. Requires purchasing a Wish recharge in the Cosmic Store to use again.");
				((BlueprintUnitFact)blueprintAbility).m_Icon = Icon_Coin;
				blueprintAbility.Type = AbilityType.Special;
				blueprintAbility.Range = AbilityRange.Personal;
				blueprintAbility.CanTargetSelf = true;
				blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				blueprintAbility.ActionType = UnitCommand.CommandType.Standard;
				blueprintAbility.AddComponent(delegate(AbilityVariants c)
				{
					c.m_Variants = new BlueprintAbilityReference[7]
					{
						WishStrength.ToReference<BlueprintAbilityReference>(),
						WishDexterity.ToReference<BlueprintAbilityReference>(),
						WishConstitution.ToReference<BlueprintAbilityReference>(),
						WishIntelligence.ToReference<BlueprintAbilityReference>(),
						WishWisdom.ToReference<BlueprintAbilityReference>(),
						WishCharisma.ToReference<BlueprintAbilityReference>(),
						WishWealth.ToReference<BlueprintAbilityReference>()
					};
				});
			});
			BlueprintAbility item6 = Helpers.CreateBlueprint(Main.IsekaiContext, "BuyWishChargeAbility", delegate(BlueprintAbility blueprintAbility)
			{
				blueprintAbility.SetName(Main.IsekaiContext, "Miracle: Cosmic Wish Recharge");
				blueprintAbility.SetDescription(Main.IsekaiContext, "Spend 4,000 Cosmic Coins to purchase exactly 1 charge for your modular Cosmic Wish ability. Grants a permanent +2 untyped stackable bonus to any attribute of your choice or 500,000 Gold when cast.");
				((BlueprintUnitFact)blueprintAbility).m_Icon = Icon_Coin;
				blueprintAbility.Type = AbilityType.Special;
				blueprintAbility.Range = AbilityRange.Personal;
				blueprintAbility.CanTargetSelf = true;
				blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				blueprintAbility.ActionType = UnitCommand.CommandType.Standard;
				blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionBuyWishCharge a)
					{
						a.Cost = 4000;
						a.m_WishResource = CosmicWishResource.ToReference<BlueprintAbilityResourceReference>();
					});
				});
			});
			storeAbilities.Add(item6);
			BlueprintAbility item7 = Helpers.CreateBlueprint(Main.IsekaiContext, "ManifestScrollAbility", delegate(BlueprintAbility blueprintAbility)
			{
				blueprintAbility.SetName(Main.IsekaiContext, "Miracle: Manifest Supreme Revival Scroll");
				blueprintAbility.SetDescription(Main.IsekaiContext, "Spend 1,500 Cosmic Coins to directly manifest a supreme scroll of true resurrection into your inventory.");
				((BlueprintUnitFact)blueprintAbility).m_Icon = ResurrectionScroll.m_Icon;
				blueprintAbility.Type = AbilityType.Special;
				blueprintAbility.Range = AbilityRange.Personal;
				blueprintAbility.CanTargetSelf = true;
				blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				blueprintAbility.ActionType = UnitCommand.CommandType.Standard;
				blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionManifestScroll a)
					{
						a.Cost = 1500;
						a.m_Item = ResurrectionScroll.ToReference<BlueprintItemReference>();
					});
				});
			});
			storeAbilities.Add(item7);
			List<(string name, BlueprintFeature feat, int cost, string displayName, string desc)> infusionList = new List<(string, BlueprintFeature, int, string, string)>();
			AddInfusion("DragonScaleInfusion", "Dragon's Scale Infusion", "Permanently grants a +2 Natural Armor bonus to AC.", 800, delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
			});
			AddInfusion("EyeOfCosmosInfusion", "Eye of the Cosmos Infusion", "Permanently grants True Seeing.", 800, delegate(BlueprintFeature obj)
			{
				BlueprintBuff trueSeeing = BlueprintTools.GetBlueprint<BlueprintBuff>("09b4b69169304474296484c74aa12027");
				if (trueSeeing != null)
				{
					obj.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { trueSeeing.ToReference<BlueprintUnitFactReference>() };
					});
				}
			});
			AddInfusion("StarlightSwiftnessInfusion", "Starlight Swiftness Infusion", "Permanently grants a +10 ft bonus to movement speed and +2 to Initiative.", 800, delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Initiative;
					c.Value = 2;
				});
			});
			AddInfusion("CosmicFortitudeInfusion", "Cosmic Fortitude Infusion", "Permanently grants +20 maximum hit points.", 800, delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.HitPoints;
					c.Value = 20;
				});
			});
			AddInfusion("AstralWeaponAttunement", "Astral Weapon Attunement", "Permanently causes all weapon attacks to deal an additional 1d6 force damage.", 1200, delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 5;
				});
			});
			List<(string name, BlueprintFeature masterFeat, string displayName, string desc)> subclassBoonList = new List<(string, BlueprintFeature, string, string)>();
			AddSubclassBlessing("IsekaiTranscendence", "Isekai Transcendence", "Grants a +2 Luck bonus to AC and all saving throws.", "IsekaiProficiencies", delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
			});
			AddSubclassBlessing("MartialGodBlessingTranscendence", "Martial God Transcendence", "Grants a +10 ft bonus to speed, +2 Dodge AC, and an extra attack on a full attack.", "MartialGodProficiencies", delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Dodge;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				obj.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
					c.Haste = false;
				});
			});
			AddSubclassBlessing("ImperialTranscendence", "Imperial Transcendence", "Grants a +2 Sacred bonus to Intelligence, Wisdom, and Charisma.", "GodEmperorProficiencies", delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Intelligence;
					c.Value = 2;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Wisdom;
					c.Value = 2;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Charisma;
					c.Value = 2;
				});
			});
			AddSubclassBlessing("SupremeTranscendence", "Supreme Transcendence", "Increases the DC of all spells and Despair Auras by +2.", "OverlordProficiencies", delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 2;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
			});
			AddSubclassBlessing("ApexTranscendence", "Apex Transcendence", "Grants +20 maximum hit points and a +2 Enhancement bonus to Strength and Constitution.", "DevourerProficiencies", delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.HitPoints;
					c.Value = 20;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Strength;
					c.Value = 2;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.Constitution;
					c.Value = 2;
				});
			});
			AddSubclassBlessing("MonarchTranscendence", "Monarch Transcendence", "Grants a +2 Insight bonus to AC and saves, and empowers shadow extractions.", "ShadowMonarchProficiencies", delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
			});
			AddSubclassBlessing("FellowshipTranscendence", "Fellowship Transcendence", "Grants a +2 Sacred bonus to saving throws and Fast Healing 3 to the Hero.", "HeroProficiencies", delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
				obj.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 3;
					c.Bonus = 0;
				});
			});
			AddSubclassBlessing("GrandmasterTranscendence", "Grandmaster Transcendence", "Grants a +4 Insight bonus to Initiative and +2d6 Sneak Attack damage.", "MastermindProficiencies", delegate(BlueprintFeature obj)
			{
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.Initiative;
					c.Value = 4;
				});
				obj.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SneakAttack;
					c.Value = 2;
				});
			});
			BlueprintCue blueprintCue = TTCoreExtensions.CreateCue("CueStoreBlessings", delegate(BlueprintCue blueprintCue9)
			{
				blueprintCue9.SetText(Main.IsekaiContext, "<color=#F5C542><b>[Deity Blessings Catalog]</b></color>\n\nThe constellations offer their divine favor in exchange for Cosmic Coins. Blessings last for 1 hour and persist through death.\n\nSelect a blessing to invoke:");
				blueprintCue9.Answers = new List<BlueprintAnswerBaseReference>();
			});
			BlueprintCue blueprintCue2 = TTCoreExtensions.CreateCue("CueStoreMythic", delegate(BlueprintCue blueprintCue9)
			{
				blueprintCue9.SetText(Main.IsekaiContext, "<color=#F5C542><b>[Mythic Path Boons Catalog]</b></color>\n\nEchoes of legendary planar destinies crystallized into permanent personal enhancements.\n\nSelect a mythic path boon to acquire:");
				blueprintCue9.Answers = new List<BlueprintAnswerBaseReference>();
			});
			BlueprintCue blueprintCue3 = TTCoreExtensions.CreateCue("CueStoreConsumables", delegate(BlueprintCue blueprintCue9)
			{
				blueprintCue9.SetText(Main.IsekaiContext, "<color=#F5C542><b>[Web Novel Consumables & Cosmic Wishes]</b></color>\n\nEmergency trump cards and reality-warping miracles drafted by senior Otherworlders.\n\nSelect an offering to acquire:");
				blueprintCue9.Answers = new List<BlueprintAnswerBaseReference>();
			});
			BlueprintCue blueprintCue4 = TTCoreExtensions.CreateCue("CueStoreInfusions", delegate(BlueprintCue blueprintCue9)
			{
				blueprintCue9.SetText(Main.IsekaiContext, "<color=#F5C542><b>[Permanent Infusions & Subclass Transcendence]</b></color>\n\nInscribe cosmic truth directly into your flesh, blood, and class techniques.\n\nSelect an infusion or transcendence to acquire:");
				blueprintCue9.Answers = new List<BlueprintAnswerBaseReference>();
			});
			BlueprintCue blueprintCue5 = TTCoreExtensions.CreateCue("CueStoreForbiddenCheats", delegate(BlueprintCue blueprintCue9)
			{
				blueprintCue9.SetText(Main.IsekaiContext, "<color=#FF4500><b>[Forbidden Otherworldly Cheats (Protagonist Hacks)]</b></color>\n\n<i>\"H-Hey! How did you get access to the developer console?! The cosmic balance will collapse!\"</i>\n\nThe observing constellations erupt into chaotic panic as you tap into the raw architectural code of reality. For an exorbitant fee of accumulated Cosmic Coins, they are willing to turn a blind eye and grant you truly game-breaking otherworldly cheats.\n\nSelect a forbidden cheat below:");
				blueprintCue9.Answers = new List<BlueprintAnswerBaseReference>();
			});
			BlueprintCue blueprintCue6 = TTCoreExtensions.CreateCue("CueStoreCosmicRelics", delegate(BlueprintCue blueprintCue9)
			{
				blueprintCue9.SetText(Main.IsekaiContext, "<color=#00FFFF><b>[Cosmic Relics & Sovereign Boons]</b></color>\n\nThe constellations draw back the celestial veil, presenting legendary stellar relics, ancient companions, and mythic rulebreaker pacts forged from the heart of dying stars.\n\nSelect a cosmic relic or boon:");
				blueprintCue9.Answers = new List<BlueprintAnswerBaseReference>();
			});
			BlueprintCue blueprintCue7 = TTCoreExtensions.CreateCue("CueStoreFealty", delegate(BlueprintCue blueprintCue9)
			{
				blueprintCue9.SetText(Main.IsekaiContext, "<color=#F5C542><b>[Constellation Fealty & Avatar Ascension]</b></color>\n\nThe constellations watch your battles closely. Align your soul with a sponsor deity for +25% patron donations, or ascend as their mortal Avatar for 10,000 Cosmic Coins.\n\nSelect an option:");
				blueprintCue9.Answers = new List<BlueprintAnswerBaseReference>();
			});
			BlueprintCue blueprintCue8 = TTCoreExtensions.CreateCue("CueStoreGodDefier", delegate(BlueprintCue blueprintCue9)
			{
				blueprintCue9.SetText(Main.IsekaiContext, "<color=#F5C542><b>[Path of the God Defier]</b></color>\n\n<i>\"We bow to no throne and worship no star. Our fate belongs only to us.\"</i>\n\nReject all divine authority and constellation patronage. Gaining supreme immunity to divine spells, SR 35, +5 untyped attack/damage, and true sovereign freedom.\n\n<b>Requirements:</b> Atheism & 10,000 Cosmic Coins.");
				blueprintCue9.Answers = new List<BlueprintAnswerBaseReference>();
			});
			BlueprintCue CosmicStoreRootCue = TTCoreExtensions.CreateCue("CosmicStoreRootCue", delegate(BlueprintCue blueprintCue9)
			{
				blueprintCue9.SetText(Main.IsekaiContext, "<color=#F5C542><b>[The Cosmic Constellation Exchange]</b></color>\n\nSoft starlight crystallizes around you into a shimmering celestial interface. The observing constellations watch with eager anticipation as your accumulated Cosmic Coins hum with multiversal energy.\n\nSelect a catalog category below:");
				blueprintCue9.Answers = new List<BlueprintAnswerBaseReference>();
			});
			BlueprintDialog CosmicStoreDialog = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicStoreDialog", delegate(BlueprintDialog blueprintDialog)
			{
				blueprintDialog.FirstCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { CosmicStoreRootCue.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				blueprintDialog.Type = DialogType.Common;
				blueprintDialog.TurnPlayer = false;
				blueprintDialog.TurnFirstSpeaker = false;
				blueprintDialog.Conditions = ActionFlow.EmptyCondition();
				blueprintDialog.StartActions = ActionFlow.DoNothing();
				blueprintDialog.FinishActions = ActionFlow.DoNothing();
				blueprintDialog.ReplaceActions = ActionFlow.DoNothing();
			});
			BlueprintAnswer bp = CreateStoreAnswer("AnswerStoreBlessings", "(1) [Deity Blessings] Browse 14 Constellation Patron Blessings (100 - 500 Coins)", blueprintCue);
			BlueprintAnswer bp2 = CreateStoreAnswer("AnswerStoreMythic", "(2) [Mythic Path Boons] Browse 10 Mythic Path Resonance Boons (1,500 - 2,000 Coins)", blueprintCue2);
			BlueprintAnswer bp3 = CreateStoreAnswer("AnswerStoreConsumables", "(3) [Web Novel Consumables & Wishes] Elixirs, Figurines, and Wishes", blueprintCue3);
			BlueprintAnswer bp4 = CreateStoreAnswer("AnswerStoreInfusions", "(4) [Infusions & Transcendence] Inscribe cosmic truth and subclass mastery", blueprintCue4);
			BlueprintAnswer bp5 = CreateStoreAnswer("AnswerStoreForbiddenCheats", "(5) <color=#FF4500>[Forbidden Otherworldly Cheats]</color> God-mode protagonist hacks (4,000 - 6,000 Coins)", blueprintCue5);
			BlueprintAnswer bp6 = CreateStoreAnswer("AnswerStoreCosmicRelics", "(6) <color=#00FFFF>[Cosmic Relics & Sovereign Boons]</color> Stellar artifacts and companions (3,500 - 5,500 Coins)", blueprintCue6);
			BlueprintAnswer bp7 = CreateStoreAnswer("AnswerStoreFealty", "(7) [Constellation Fealty & Avatar Ascension] Align with a patron or ascend as an Avatar", blueprintCue7);
			BlueprintAnswer bp8 = CreateStoreAnswer("AnswerStoreGodDefier", "(8) <color=#F5C542>[Path of the God Defier]</color> (Atheists) Shatter divine chains (10,000 Coins)", blueprintCue8);
			BlueprintAnswer bp9 = CreateStoreAnswer("AnswerStoreClose", "[Leave] Close the Cosmic Constellation Exchange.", null);
			CosmicStoreRootCue.Answers.Add(bp.ToReference<BlueprintAnswerBaseReference>());
			CosmicStoreRootCue.Answers.Add(bp2.ToReference<BlueprintAnswerBaseReference>());
			CosmicStoreRootCue.Answers.Add(bp3.ToReference<BlueprintAnswerBaseReference>());
			CosmicStoreRootCue.Answers.Add(bp4.ToReference<BlueprintAnswerBaseReference>());
			CosmicStoreRootCue.Answers.Add(bp5.ToReference<BlueprintAnswerBaseReference>());
			CosmicStoreRootCue.Answers.Add(bp6.ToReference<BlueprintAnswerBaseReference>());
			CosmicStoreRootCue.Answers.Add(bp7.ToReference<BlueprintAnswerBaseReference>());
			CosmicStoreRootCue.Answers.Add(bp8.ToReference<BlueprintAnswerBaseReference>());
			CosmicStoreRootCue.Answers.Add(bp9.ToReference<BlueprintAnswerBaseReference>());
			BlueprintAnswer bp10 = CreateStoreAnswer("AnswerBackFromBlessings", "[Back] Return to the Main Catalog.", CosmicStoreRootCue);
			BlueprintAnswer bp11 = CreateStoreAnswer("AnswerBackFromMythic", "[Back] Return to the Main Catalog.", CosmicStoreRootCue);
			BlueprintAnswer bp12 = CreateStoreAnswer("AnswerBackFromConsumables", "[Back] Return to the Main Catalog.", CosmicStoreRootCue);
			BlueprintAnswer bp13 = CreateStoreAnswer("AnswerBackFromInfusions", "[Back] Return to the Main Catalog.", CosmicStoreRootCue);
			BlueprintAnswer bp14 = CreateStoreAnswer("AnswerBackFromForbiddenCheats", "[Back] Return to the Main Catalog.", CosmicStoreRootCue);
			BlueprintAnswer bp15 = CreateStoreAnswer("AnswerBackFromCosmicRelics", "[Back] Return to the Main Catalog.", CosmicStoreRootCue);
			BlueprintAnswer bp16 = CreateStoreAnswer("AnswerBackFromFealty", "[Back] Return to the Main Catalog.", CosmicStoreRootCue);
			BlueprintAnswer bp17 = CreateStoreAnswer("AnswerBackFromGodDefier", "[Back] Return to the Main Catalog.", CosmicStoreRootCue);
			Dictionary<string, string> dictionary = new Dictionary<string, string>
			{
				{ "CaydenReserveBrew", "AnswerBuyBlessingCayden" },
				{ "StarlightAegis", "AnswerBuyBlessingDesna" },
				{ "EssenceOfLurker", "AnswerBuyBlessingYogSothoth" },
				{ "LanternTricksterGlow", "AnswerBuyBlessingLanternKing" },
				{ "QueenOfTheSeasFavor", "AnswerBuyBlessingBesmara" },
				{ "ChaldiraHeartyLuck", "AnswerBuyBlessingChaldira" },
				{ "IronLordFury", "AnswerBuyBlessingGorum" },
				{ "AllSeeingGlyph", "AnswerBuyBlessingNethys" },
				{ "KissOfTheSavoredSting", "AnswerBuyBlessingCalistria" },
				{ "LadyOfGravesWarding", "AnswerBuyBlessingPharasma" },
				{ "InheritorVanguard", "AnswerBuyBlessingIomedae" },
				{ "ArchfiendsCovenant", "AnswerBuyBlessingAsmodeus" },
				{ "RoseGuardsResolve", "AnswerBuyBlessingMilani" },
				{ "SilentShroud", "AnswerBuyBlessingBlackButterfly" }
			};
			foreach (var item9 in blessingBuffs)
			{
				if (dictionary.TryGetValue(item9.name, out var value))
				{
					BlueprintAnswer bp18 = CreateStoreAnswer(value, $"({item9.cost} Coins) [{item9.displayName}] {item9.desc}", blueprintCue, new ContextActionSpendCoinsAndApplyBuff
					{
						Cost = item9.cost,
						m_Buff = item9.buff.ToReference<BlueprintBuffReference>(),
						DurationHours = 1
					});
					blueprintCue.Answers.Add(bp18.ToReference<BlueprintAnswerBaseReference>());
				}
			}
			blueprintCue.Answers.Add(bp10.ToReference<BlueprintAnswerBaseReference>());
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>
			{
				{ "BoonOfHeavenlyHost", "AnswerBuyMythicAngel" },
				{ "BoonOfAbyssalRage", "AnswerBuyMythicDemon" },
				{ "BoonOfLordOfBones", "AnswerBuyMythicLich" },
				{ "BoonOfWildWonder", "AnswerBuyMythicAzata" },
				{ "BoonOfCosmicJester", "AnswerBuyMythicTrickster" },
				{ "BoonOfMonadicOrder", "AnswerBuyMythicAeon" },
				{ "BoonOfHellfireContract", "AnswerBuyMythicDevil" },
				{ "BoonOfGoldenDragon", "AnswerBuyMythicGoldDragon" },
				{ "BoonOfPrimevalSwarm", "AnswerBuyMythicSwarm" },
				{ "BoonOfUnboundMortal", "AnswerBuyMythicLegend" }
			};
			foreach (var item10 in mythicBoonList)
			{
				if (dictionary2.TryGetValue(item10.name, out var value2))
				{
					BlueprintAnswer bp19 = CreateStoreAnswer(value2, $"({item10.cost} Coins) [{item10.displayName}] {item10.desc}", blueprintCue2, new ContextActionSpendCoinsAndGrantFact
					{
						Cost = item10.cost,
						m_Fact = item10.feat.ToReference<BlueprintUnitFactReference>(),
						m_KeyOfInfinitePathways = KeyOfInfinitePathwaysFeature.ToReference<BlueprintFeatureReference>(),
						AllowedAlignment = item10.alignment
					});
					blueprintCue2.Answers.Add(bp19.ToReference<BlueprintAnswerBaseReference>());
				}
			}
			blueprintCue2.Answers.Add(bp11.ToReference<BlueprintAnswerBaseReference>());
			BlueprintAnswer bp20 = CreateStoreAnswer("AnswerBuyElixirRejuv", "(400 Coins) [Elixir of Instant Rejuvenation] Fully restores HP, spell slots, and cures conditions.", blueprintCue3, new ContextActionRejuvenation());
			BlueprintAnswer bp21 = CreateStoreAnswer("AnswerBuyPaperFigurine", "(750 Coins) [Paper Figurine of Substitute Life] Grants +100 temporary HP for 24h.", blueprintCue3, new ContextActionSpendCoinsAndApplyBuff
			{
				Cost = 750,
				m_Buff = PaperFigurineBuff.ToReference<BlueprintBuffReference>(),
				DurationHours = 24
			});
			BlueprintAnswer bp22 = CreateStoreAnswer("AnswerBuyDraughtSoulAnchor", "(500 Coins) [Draught of Absolute Soul Anchor] Immune to death effects, negative levels, and compulsion for 24h.", blueprintCue3, new ContextActionSpendCoinsAndApplyBuff
			{
				Cost = 500,
				m_Buff = SoulAnchorBuff.ToReference<BlueprintBuffReference>(),
				DurationHours = 24
			});
			BlueprintAnswer bp23 = CreateStoreAnswer("AnswerBuyTomeMastery", "(1,000 Coins) [Tome of Protagonist's Mastery] Permanent +2 competence bonus to attacks, damage, and DCs.", blueprintCue3, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 1000,
				m_Fact = OtherworlderMasteryFeature.ToReference<BlueprintUnitFactReference>()
			});
			BlueprintAnswer bp24 = CreateStoreAnswer("AnswerBuyWishCharge", "(4,000 Coins) [Cosmic Wish Recharge] Adds 1 charge to your field-cast Cosmic Wish ability.", blueprintCue3, new ContextActionBuyWishCharge
			{
				Cost = 4000,
				m_WishResource = CosmicWishResource.ToReference<BlueprintAbilityResourceReference>()
			});
			BlueprintAnswer bp25 = CreateStoreAnswer("AnswerBuyAmbrosiaMight", "(3,000 Coins) [Primordial Ambrosia of Might] Permanently grants +2 Untyped Strength (stacks infinitely).", blueprintCue3, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3000,
				m_Item = CosmicConsumables.ItemPrimordialAmbrosiaMight.ToReference<BlueprintItemReference>()
			});
			BlueprintAnswer bp26 = CreateStoreAnswer("AnswerBuyAmbrosiaGrace", "(3,000 Coins) [Primordial Ambrosia of Grace] Permanently grants +2 Untyped Dexterity (stacks infinitely).", blueprintCue3, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3000,
				m_Item = CosmicConsumables.ItemPrimordialAmbrosiaGrace.ToReference<BlueprintItemReference>()
			});
			BlueprintAnswer bp27 = CreateStoreAnswer("AnswerBuyAmbrosiaEndurance", "(3,000 Coins) [Primordial Ambrosia of Endurance] Permanently grants +2 Untyped Constitution (stacks infinitely).", blueprintCue3, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3000,
				m_Item = CosmicConsumables.ItemPrimordialAmbrosiaEndurance.ToReference<BlueprintItemReference>()
			});
			BlueprintAnswer bp28 = CreateStoreAnswer("AnswerBuyAmbrosiaIntellect", "(3,000 Coins) [Primordial Ambrosia of Intellect] Permanently grants +2 Untyped Intelligence (stacks infinitely).", blueprintCue3, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3000,
				m_Item = CosmicConsumables.ItemPrimordialAmbrosiaIntellect.ToReference<BlueprintItemReference>()
			});
			BlueprintAnswer bp29 = CreateStoreAnswer("AnswerBuyAmbrosiaInsight", "(3,000 Coins) [Primordial Ambrosia of Insight] Permanently grants +2 Untyped Wisdom (stacks infinitely).", blueprintCue3, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3000,
				m_Item = CosmicConsumables.ItemPrimordialAmbrosiaInsight.ToReference<BlueprintItemReference>()
			});
			BlueprintAnswer bp30 = CreateStoreAnswer("AnswerBuyAmbrosiaMajesty", "(3,000 Coins) [Primordial Ambrosia of Majesty] Permanently grants +2 Untyped Charisma (stacks infinitely).", blueprintCue3, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3000,
				m_Item = CosmicConsumables.ItemPrimordialAmbrosiaMajesty.ToReference<BlueprintItemReference>()
			});
			BlueprintAnswer bp31 = CreateStoreAnswer("AnswerBuyAmbrosiaCelerity", "(3,000 Coins) [Primordial Ambrosia of Celerity] Permanently grants +10 ft Untyped Base Speed (stacks infinitely).", blueprintCue3, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3000,
				m_Item = CosmicConsumables.ItemPrimordialAmbrosiaCelerity.ToReference<BlueprintItemReference>()
			});
			BlueprintAnswer bp32 = CreateStoreAnswer("AnswerBuyAmbrosiaEternalRenewal", "(10,000 Coins) [Primordial Ambrosia of Eternal Renewal] Permanently grants Fast Healing 5 (Untyped Stackable, stacks infinitely).", blueprintCue3, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 10000,
				m_Item = CosmicConsumables.ItemPrimordialAmbrosiaEternalRenewal.ToReference<BlueprintItemReference>()
			});
			BlueprintAnswer bp33 = CreateStoreAnswer("AnswerBuyNectarEndlessLife", "(1,500 Coins) [Nectar of Endless Life] Permanently grants +25 Untyped Maximum HP (stacks infinitely).", blueprintCue3, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 1500,
				m_Item = CosmicConsumables.ItemNectarEndlessLife.ToReference<BlueprintItemReference>()
			});
			BlueprintAnswer bp34 = CreateStoreAnswer("AnswerBuyNectarVoidStalker", "(4,000 Coins) [Nectar of the Void Stalker] Permanently grants +1 Untyped Spell DC and +2 Spell Penetration (stacks infinitely).", blueprintCue3, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 4000,
				m_Item = CosmicConsumables.ItemNectarVoidStalker.ToReference<BlueprintItemReference>()
			});
			BlueprintAnswer bp35 = CreateStoreAnswer("AnswerBuyNectarWorldBreaker", "(4,000 Coins) [Nectar of the World Breaker] Permanently grants +2 Untyped Attack and Damage (stacks infinitely).", blueprintCue3, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 4000,
				m_Item = CosmicConsumables.ItemNectarWorldBreaker.ToReference<BlueprintItemReference>()
			});
			BlueprintAnswer bp36 = CreateStoreAnswer("AnswerBuyNectarDiamondSoul", "(3,500 Coins) [Nectar of the Diamond Soul] Permanently grants +2 Untyped to all Saving Throws (stacks infinitely).", blueprintCue3, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3500,
				m_Item = CosmicConsumables.ItemNectarDiamondSoul.ToReference<BlueprintItemReference>()
			});
			BlueprintAnswer bp37 = CreateStoreAnswer("AnswerBuyNectarUnbrokenAegis", "(3,500 Coins) [Nectar of the Unbroken Aegis] Permanently grants +1 Untyped Armor Class (stacks infinitely).", blueprintCue3, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3500,
				m_Item = CosmicConsumables.ItemNectarUnbrokenAegis.ToReference<BlueprintItemReference>()
			});
			BlueprintAnswer bp38 = CreateStoreAnswer("AnswerBuyDraughtVelocity", "(2,500 Coins) [Draught of Cosmic Velocity] +20 ft speed and +1 extra attack on full attack for 24 hours.", blueprintCue3, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 2500,
				m_Item = CosmicConsumables.ItemDraughtCosmicVelocity.ToReference<BlueprintItemReference>()
			});
			BlueprintAnswer bp39 = CreateStoreAnswer("AnswerBuyDraughtImmortalTitan", "(2,500 Coins) [Draught of the Immortal Titan] +150 Temp HP, DR 10/--, and Fast Healing 10 for 24 hours.", blueprintCue3, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 2500,
				m_Item = CosmicConsumables.ItemDraughtImmortalTitan.ToReference<BlueprintItemReference>()
			});
			BlueprintAnswer bp40 = CreateStoreAnswer("AnswerBuyDraughtArcaneOmniscience", "(3,000 Coins) [Draught of Arcane Omniscience] +4 Caster Level, and auto-extends and empowers all spells for 24 hours.", blueprintCue3, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3000,
				m_Item = CosmicConsumables.ItemDraughtArcaneOmniscience.ToReference<BlueprintItemReference>()
			});
			BlueprintAnswer bp41 = CreateStoreAnswer("AnswerBuyDraughtApexPredator", "(3,000 Coins) [Draught of the Apex Predator] +1 Crit Multiplier, and bypasses all DR and Concealment for 24 hours.", blueprintCue3, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3000,
				m_Item = CosmicConsumables.ItemDraughtApexPredator.ToReference<BlueprintItemReference>()
			});
			BlueprintAnswer bp42 = CreateStoreAnswer("AnswerBuyDraughtSoulAnchorBottle", "(500 Coins) [Draught of Absolute Soul Anchor] Bottle granting death, negative level, and compulsion immunity for 24 hours.", blueprintCue3, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 500,
				m_Item = CosmicConsumables.ItemDraughtAbsoluteSoulAnchor.ToReference<BlueprintItemReference>()
			});
			BlueprintAnswer bp43 = CreateStoreAnswer("AnswerBuyWishstone", "(4,000 Coins) [Bottled Djinn Wishstone] Physical quickslot relic granting 5 reality-warping miracles.", blueprintCue3, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 4000,
				m_Item = CosmicConsumables.ItemCosmicWishstone.ToReference<BlueprintItemReference>()
			});
			BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SkillBookGrandScholarFeature");
			BlueprintFeature modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SkillBookPhantomInfiltratorFeature");
			BlueprintFeature modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SkillBookAbsoluteCharismaFeature");
			BlueprintFeature modBlueprint4 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SkillBookWindWalkerFeature");
			BlueprintFeature modBlueprint5 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SkillBookEyeOfProvidenceFeature");
			BlueprintAnswer bp44 = CreateStoreAnswer("AnswerBuyBookGrandScholar", "(800 Coins) [Grimoire of the Grand Scholar] Permanent +4 Knowledge/Lore, +1 Caster Level.", blueprintCue3, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 800,
				m_Fact = modBlueprint.ToReference<BlueprintUnitFactReference>()
			});
			BlueprintAnswer bp45 = CreateStoreAnswer("AnswerBuyBookPhantomInfiltrator", "(800 Coins) [Manual of the Phantom Infiltrator] Permanent +4 Stealth/Trickery, +1d6 Sneak Attack.", blueprintCue3, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 800,
				m_Fact = modBlueprint2.ToReference<BlueprintUnitFactReference>()
			});
			BlueprintAnswer bp46 = CreateStoreAnswer("AnswerBuyBookAbsoluteCharisma", "(800 Coins) [Chronicle of Absolute Charisma] Permanent +4 Persuasion/UMD, +2 Enchantment/Illusion DC.", blueprintCue3, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 800,
				m_Fact = modBlueprint3.ToReference<BlueprintUnitFactReference>()
			});
			BlueprintAnswer bp47 = CreateStoreAnswer("AnswerBuyBookWindWalker", "(800 Coins) [Codex of the Wind Walker] Permanent +4 Athletics/Mobility, +10 ft base speed.", blueprintCue3, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 800,
				m_Fact = modBlueprint4.ToReference<BlueprintUnitFactReference>()
			});
			BlueprintAnswer bp48 = CreateStoreAnswer("AnswerBuyBookEyeOfProvidence", "(800 Coins) [Tractate of the Eye of Providence] Permanent +4 Perception, immune to Blindness and Flat-Footed.", blueprintCue3, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 800,
				m_Fact = modBlueprint5.ToReference<BlueprintUnitFactReference>()
			});
			blueprintCue3.Answers.Add(bp20.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp21.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp22.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp42.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp23.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp25.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp26.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp27.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp28.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp29.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp30.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp31.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp32.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp33.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp34.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp35.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp36.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp37.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp38.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp39.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp40.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp41.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp43.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp44.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp45.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp46.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp47.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp48.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp24.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue3.Answers.Add(bp12.ToReference<BlueprintAnswerBaseReference>());
			Dictionary<string, string> dictionary3 = new Dictionary<string, string>
			{
				{ "DragonScaleInfusion", "AnswerBuyInfusionDragonScale" },
				{ "EyeOfCosmosInfusion", "AnswerBuyInfusionEyeOfCosmos" },
				{ "StarlightSwiftnessInfusion", "AnswerBuyInfusionStarlightSwiftness" },
				{ "CosmicFortitudeInfusion", "AnswerBuyInfusionCosmicFortitude" },
				{ "AstralWeaponAttunement", "AnswerBuyInfusionAstralWeapon" }
			};
			foreach (var item11 in infusionList)
			{
				if (dictionary3.TryGetValue(item11.name, out var value3))
				{
					BlueprintAnswer bp49 = CreateStoreAnswer(value3, $"({item11.cost} Coins) [{item11.displayName}] {item11.desc}", blueprintCue4, new ContextActionSpendCoinsAndGrantFact
					{
						Cost = item11.cost,
						m_Fact = item11.feat.ToReference<BlueprintUnitFactReference>()
					});
					blueprintCue4.Answers.Add(bp49.ToReference<BlueprintAnswerBaseReference>());
				}
			}
			BlueprintAnswer bp50 = CreateStoreAnswer("AnswerBuyKeyOfInfinitePathways", "(5,000 Coins) [Meta-Boon: Key of Infinite Pathways] Shatters alignment and mythic restrictions in the Cosmic Store.", blueprintCue4, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 5000,
				m_Fact = KeyOfInfinitePathwaysFeature.ToReference<BlueprintUnitFactReference>()
			});
			blueprintCue4.Answers.Add(bp50.ToReference<BlueprintAnswerBaseReference>());
			Dictionary<string, string> dictionary4 = new Dictionary<string, string>
			{
				{ "IsekaiTranscendence", "AnswerBuyIsekaiTranscendence" },
				{ "PhantomTranscendence", "AnswerBuyPhantomTranscendence" },
				{ "ImperialTranscendence", "AnswerBuyImperialTranscendence" },
				{ "SupremeTranscendence", "AnswerBuySupremeTranscendence" },
				{ "ApexTranscendence", "AnswerBuyApexTranscendence" },
				{ "MonarchTranscendence", "AnswerBuyMonarchTranscendence" },
				{ "FellowshipTranscendence", "AnswerBuyFellowshipTranscendence" },
				{ "GrandmasterTranscendence", "AnswerBuyGrandmasterTranscendence" }
			};
			foreach (var item12 in subclassBoonList)
			{
				if (dictionary4.TryGetValue(item12.name, out var value4))
				{
					BlueprintAnswer bp51 = CreateStoreAnswer(value4, "(100 Coins) [Transcendence: " + item12.displayName + "] " + item12.desc, blueprintCue4, new ContextActionSpendCoinsAndGrantFact
					{
						Cost = 100,
						m_Fact = item12.masterFeat.ToReference<BlueprintUnitFactReference>()
					});
					blueprintCue4.Answers.Add(bp51.ToReference<BlueprintAnswerBaseReference>());
				}
			}
			blueprintCue4.Answers.Add(bp13.ToReference<BlueprintAnswerBaseReference>());
			BlueprintFeature modBlueprint6 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "InstakillFeature");
			BlueprintFeature modBlueprint7 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SuperBuffFeature");
			BlueprintFeature modBlueprint8 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SummonCalamityFeature");
			BlueprintFeature modBlueprint9 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "CosmicAutoQuickenFeature");
			BlueprintFeature modBlueprint10 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "CosmicSupremeBeingFeature");
			BlueprintFeature modBlueprint11 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "CosmicChronoDomainFeature");
			BlueprintFeature bp52 = Helpers.CreateBlueprint(Main.IsekaiContext, "ManaWellspringFeature", delegate(BlueprintFeature blueprintFeature2)
			{
				blueprintFeature2.SetName(Main.IsekaiContext, "Forbidden Cheat: Mana Wellspring");
				blueprintFeature2.SetDescription(Main.IsekaiContext, "An inexhaustible fountain of otherworld mana courses through your soul. Grants +4 additional spell slots per day for all spell tiers from 1st through 9th circle.");
				((BlueprintUnitFact)blueprintFeature2).m_Icon = Icon_Coin;
				blueprintFeature2.IsClassFeature = true;
				blueprintFeature2.AddComponent(delegate(AddSpellsPerDay c)
				{
					c.Amount = 4;
					c.Levels = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
				});
			});
			BlueprintFeature bp53 = Helpers.CreateBlueprint(Main.IsekaiContext, "RealityPiercerFeature", delegate(BlueprintFeature blueprintFeature2)
			{
				blueprintFeature2.SetName(Main.IsekaiContext, "Forbidden Cheat: Reality Piercer");
				blueprintFeature2.SetDescription(Main.IsekaiContext, "Your attacks shear through reality itself. All physical and magical attacks completely ignore Damage Reduction and Concealment, and you gain a +6 untyped bonus to Spell Penetration.");
				((BlueprintUnitFact)blueprintFeature2).m_Icon = Icon_Coin;
				blueprintFeature2.IsClassFeature = true;
				blueprintFeature2.AddComponent<IgnoreDamageReductionOnAttack>();
				blueprintFeature2.AddComponent(delegate(SpellPenetrationBonus c)
				{
					c.Value = 6;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
			});
			BlueprintAbilityResource ChronoSurgeResource = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSurgeResource", delegate(BlueprintAbilityResource blueprintAbilityResource)
			{
				blueprintAbilityResource.m_MaxAmount = new BlueprintAbilityResource.Amount
				{
					BaseValue = 3,
					IncreasedByLevel = false,
					IncreasedByStat = false
				};
			});
			BlueprintAbility ChronoSurgeAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "ChronoSurgeAbility", delegate(BlueprintAbility blueprintAbility)
			{
				blueprintAbility.SetName(Main.IsekaiContext, "Chrono-Surge: Double Turn");
				blueprintAbility.SetDescription(Main.IsekaiContext, "As a free action (usable 3 times per day), warp the local temporal stream to immediately reset and refund your Standard, Move, and Swift actions. Does not desync the turn order.");
				((BlueprintUnitFact)blueprintAbility).m_Icon = Icon_Coin;
				blueprintAbility.Type = AbilityType.Special;
				blueprintAbility.Range = AbilityRange.Personal;
				blueprintAbility.CanTargetSelf = true;
				blueprintAbility.ActionType = UnitCommand.CommandType.Free;
				blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle<ContextActionChronoSurge>();
				});
				blueprintAbility.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = ChronoSurgeResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
			});
			BlueprintFeature bp54 = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicChronoSurgeFeature", delegate(BlueprintFeature blueprintFeature2)
			{
				blueprintFeature2.SetName(Main.IsekaiContext, "Forbidden Cheat: Chrono-Surge");
				blueprintFeature2.SetDescription(Main.IsekaiContext, "Mastery over temporal flow grants you the Chrono-Surge ability, allowing you to seize a double turn 3 times per day as a free action.");
				((BlueprintUnitFact)blueprintFeature2).m_Icon = Icon_Coin;
				blueprintFeature2.IsClassFeature = true;
				blueprintFeature2.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { ChronoSurgeAbility.ToReference<BlueprintUnitFactReference>() };
				});
				blueprintFeature2.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = ChronoSurgeResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = true;
				});
			});
			BlueprintFeature bp55 = Helpers.CreateBlueprint(Main.IsekaiContext, "GachaJackpotFeature", delegate(BlueprintFeature blueprintFeature2)
			{
				blueprintFeature2.SetName(Main.IsekaiContext, "Forbidden Cheat: Gacha Jackpot");
				blueprintFeature2.SetDescription(Main.IsekaiContext, "Bends probability matrices to guarantee legendary fortune. Grants a +4 luck bonus to all d20 rolls (attack rolls, saving throws, and skill checks) and manifests 250,000 Gold directly into your treasury upon acquisition.");
				((BlueprintUnitFact)blueprintFeature2).m_Icon = Icon_Coin;
				blueprintFeature2.IsClassFeature = true;
				blueprintFeature2.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				blueprintFeature2.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				blueprintFeature2.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				blueprintFeature2.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				blueprintFeature2.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SkillAthletics;
					c.Value = 4;
				});
				blueprintFeature2.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SkillPerception;
					c.Value = 4;
				});
				blueprintFeature2.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SkillPersuasion;
					c.Value = 4;
				});
			});
			BlueprintBuff AegisUndyingBuff = TTCoreExtensions.CreateBuff("AegisUndyingBuff", delegate(BlueprintBuff blueprintBuff)
			{
				blueprintBuff.SetName(Main.IsekaiContext, "Aegis of the Undying Awakening");
				blueprintBuff.SetDescription(Main.IsekaiContext, "Protected by the impenetrable aegis of destiny. Restores 150 Hit Points and grants Greater Invisibility for 3 rounds.");
				((BlueprintUnitFact)blueprintBuff).m_Icon = Icon_Coin;
				BlueprintBuff invisBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("e6b35473a237a6045969253beb09777c");
				if (invisBuff != null)
				{
					blueprintBuff.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { invisBuff.ToReference<BlueprintUnitFactReference>() };
					});
				}
			});
			BlueprintFeature bp56 = Helpers.CreateBlueprint(Main.IsekaiContext, "AegisUndyingFeature", delegate(BlueprintFeature blueprintFeature2)
			{
				blueprintFeature2.SetName(Main.IsekaiContext, "Forbidden Cheat: Aegis of the Undying");
				blueprintFeature2.SetDescription(Main.IsekaiContext, "Whenever your hit points fall below 25%, the cosmic aegis automatically triggers, instantly healing you for 150 hit points and shrouding you in Greater Invisibility for 3 rounds. Stacks seamlessly with all other survival powers.");
				((BlueprintUnitFact)blueprintFeature2).m_Icon = Icon_Coin;
				blueprintFeature2.IsClassFeature = true;
				blueprintFeature2.AddComponent(delegate(AddIncomingDamageTrigger c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(Conditional cond)
					{
						cond.ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionTargetHPLessThanPercent hp)
						{
							hp.Percent = 25;
						});
						cond.IfTrue = ActionFlow.DoSingle(delegate(ContextActionApplyBuff a)
						{
							a.m_Buff = AegisUndyingBuff.ToReference<BlueprintBuffReference>();
							a.DurationValue = new ContextDurationValue
							{
								Rate = DurationRate.Rounds,
								DiceType = DiceType.Zero,
								BonusValue = new ContextValue
								{
									ValueType = ContextValueType.Simple,
									Value = 3
								}
							};
						});
						cond.IfFalse = ActionFlow.DoNothing();
					});
				});
				blueprintFeature2.AddComponent(delegate(AddIncomingDamageTrigger c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(Conditional cond)
					{
						cond.ConditionsChecker = ActionFlow.IfSingle(delegate(ContextConditionTargetHPLessThanPercent hp)
						{
							hp.Percent = 25;
						});
						cond.IfTrue = ActionFlow.DoSingle(delegate(ContextActionHealTarget h)
						{
							h.Value = new ContextDiceValue
							{
								DiceType = DiceType.Zero,
								BonusValue = new ContextValue
								{
									ValueType = ContextValueType.Simple,
									Value = 150
								}
							};
						});
						cond.IfFalse = ActionFlow.DoNothing();
					});
				});
			});
			BlueprintFeature bp57 = Helpers.CreateBlueprint(Main.IsekaiContext, "DimensionalSanctuaryFeature", delegate(BlueprintFeature blueprintFeature2)
			{
				blueprintFeature2.SetName(Main.IsekaiContext, "Forbidden Cheat: Dimensional Sanctuary");
				blueprintFeature2.SetDescription(Main.IsekaiContext, "An otherworldly pocket sanctuary extends around you and your company. Party encumbrance capacity is increased by 1,000,000 lbs, and you gain permanent immunity to Fatigue and Exhaustion.");
				((BlueprintUnitFact)blueprintFeature2).m_Icon = Icon_Coin;
				blueprintFeature2.IsClassFeature = true;
				blueprintFeature2.AddComponent(delegate(AddPartyEncumbrance c)
				{
					c.Value = 1000000;
				});
				blueprintFeature2.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Fatigued;
				});
				blueprintFeature2.AddComponent(delegate(AddConditionImmunity c)
				{
					c.Condition = UnitCondition.Exhausted;
				});
				// Condition immunity alone leaves the delivering buff in place; blocking the descriptor stops it, as the game's own immunities do.
				blueprintFeature2.AddComponent(delegate(BuffDescriptorImmunity c)
				{
					c.Descriptor = SpellDescriptor.Fatigue | SpellDescriptor.Exhausted;
				});
				blueprintFeature2.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
				{
					c.Descriptor = SpellDescriptor.Fatigue | SpellDescriptor.Exhausted;
				});
			});
			BlueprintAnswer bp58 = CreateStoreAnswer("AnswerBuyCheatInstakill", "(5,000 Coins) [Death Note: True Instakill] Slay any mortal or immortal foe with an unblockable death decree bypassing immunities.", blueprintCue5, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 5000,
				m_Fact = modBlueprint6.ToReference<BlueprintUnitFactReference>()
			});
			BlueprintAnswer bp59 = CreateStoreAnswer("AnswerBuyCheatSuperBuff", "(4,000 Coins) [Omnipotent Buff Matrix: 24h SuperBuff] Instantly mantle yourself and your party with over 50 divine, arcane, and primal buffs for 24 hours.", blueprintCue5, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 4000,
				m_Fact = modBlueprint7.ToReference<BlueprintUnitFactReference>()
			});
			BlueprintAnswer bp60 = CreateStoreAnswer("AnswerBuyCheatAutoQuicken", "(5,000 Coins) [Cosmic Quicken Matrix: Unrestricted Auto-Quicken] Every spell you cast, up to 10th level, becomes quickened as a free swift action without level restriction.", blueprintCue5, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 5000,
				m_Fact = modBlueprint9.ToReference<BlueprintUnitFactReference>()
			});
			BlueprintAnswer bp61 = CreateStoreAnswer("AnswerBuyCheatSupremeBeing", "(6,000 Coins) [Crown of the Supreme Entity: Unbound Supreme Being] Gain a massive +25 to +35 untyped stackable bonus to all six attributes (+5 flat, +1/2 levels, +1/mythic rank).", blueprintCue5, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 6000,
				m_Fact = modBlueprint10.ToReference<BlueprintUnitFactReference>()
			});
			BlueprintAnswer bp62 = CreateStoreAnswer("AnswerBuyCheatChronoDomain", "(4,500 Coins) [Chronos Absolute Stasis: Unconditional Time Stop] Freeze all nearby enemies in time for 2 rounds as a swift action with NO saving throw and NO spell resistance.", blueprintCue5, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 4500,
				m_Fact = modBlueprint11.ToReference<BlueprintUnitFactReference>()
			});
			BlueprintAnswer bp63 = CreateStoreAnswer("AnswerBuyCheatSummonCalamity", "(5,000 Coins) [Forbidden Calamity Horn: Summon Calamity] Summon Devastators, Playful Darkness, Deskari, Baphomet, or Nocticula to fight by your side regardless of level.", blueprintCue5, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 5000,
				m_Fact = modBlueprint8.ToReference<BlueprintUnitFactReference>()
			});
			BlueprintAnswer bp64 = CreateStoreAnswer("AnswerBuyCheatManaWellspring", "(5,000 Coins) [Mana Wellspring: Reservoir of the Infinite] Gain +4 additional spell slots per day for all spell circles 1st through 9th.", blueprintCue5, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 5000,
				m_Fact = bp52.ToReference<BlueprintUnitFactReference>()
			});
			BlueprintAnswer bp65 = CreateStoreAnswer("AnswerBuyCheatRealityPiercer", "(10,000 Coins) [Reality Piercer: All-Sovereign Edge] Completely ignore all Damage Reduction and Concealment, and gain +6 Spell Penetration.", blueprintCue5, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 10000,
				m_Fact = bp53.ToReference<BlueprintUnitFactReference>()
			});
			BlueprintAnswer bp66 = CreateStoreAnswer("AnswerBuyCheatChronoSurge", "(15,000 Coins) [Protagonist Chrono-Surge: Double Turn] Gain the ability to refund standard and move actions as a free action 3 times per day.", blueprintCue5, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 15000,
				m_Fact = bp54.ToReference<BlueprintUnitFactReference>()
			});
			BlueprintAnswer bp67 = CreateStoreAnswer("AnswerBuyCheatGachaJackpot", "(5,000 Coins) [Gacha Jackpot: Midas Touch] Gain +4 luck bonus to all d20 rolls and manifest 250,000 Gold directly into your treasury.", blueprintCue5, new ContextActionBuyGachaJackpot
			{
				Cost = 5000,
				m_Fact = bp55.ToReference<BlueprintUnitFactReference>()
			});
			BlueprintAnswer bp68 = CreateStoreAnswer("AnswerBuyCheatAegisUndying", "(6,000 Coins) [Aegis of the Undying Protagonist] Falling below 25% HP triggers emergency 150 HP healing and Greater Invisibility for 3 rounds.", blueprintCue5, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 6000,
				m_Fact = bp56.ToReference<BlueprintUnitFactReference>()
			});
			BlueprintAnswer bp69 = CreateStoreAnswer("AnswerBuyCheatDimensionalSanctuary", "(4,000 Coins) [Dimensional Sanctuary: Otherworld Pocket Realm] Gain 1,000,000 lbs carry capacity and total immunity to fatigue and exhaustion.", blueprintCue5, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 4000,
				m_Fact = bp57.ToReference<BlueprintUnitFactReference>()
			});
			blueprintCue5.Answers.Add(bp58.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue5.Answers.Add(bp59.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue5.Answers.Add(bp60.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue5.Answers.Add(bp61.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue5.Answers.Add(bp62.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue5.Answers.Add(bp63.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue5.Answers.Add(bp64.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue5.Answers.Add(bp65.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue5.Answers.Add(bp66.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue5.Answers.Add(bp67.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue5.Answers.Add(bp68.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue5.Answers.Add(bp69.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue5.Answers.Add(bp14.ToReference<BlueprintAnswerBaseReference>());
			BlueprintAnswer bp70 = CreateStoreAnswer("AnswerBuyCrownApotheosis", "(4,500 Coins) [Crown of Apotheosis] Permanent +6 Int/Wis/Cha, +4 all spell DCs, +10 SR.", blueprintCue6, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 4500,
				m_Fact = CosmicRelics.CosmicCrownOfApotheosisFeature.ToReference<BlueprintUnitFactReference>()
			});
			BlueprintAnswer bp71 = CreateStoreAnswer("AnswerBuyRingOmnipresence", "(4,500 Coins) [Ring of Omnipresence] Permanent Freedom of Movement, +5 deflection AC, +5 sacred bonus to all saving throws.", blueprintCue6, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 4500,
				m_Fact = CosmicRelics.CosmicRingOfOmnipresenceFeature.ToReference<BlueprintUnitFactReference>()
			});
			BlueprintAnswer bp72 = CreateStoreAnswer("AnswerBuyAstralDragon", "(5,000 Coins) [Astral Sovereign Dragon] Gain the daily ability to summon an ancient Astral Sovereign Dragon companion.", blueprintCue6, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 5000,
				m_Fact = CosmicRelics.CosmicAstralDragonFeature.ToReference<BlueprintUnitFactReference>()
			});
			BlueprintAnswer bp73 = CreateStoreAnswer("AnswerBuyMythicPact", "(4,000 Coins) [Rulebreaker Mythic Pact] +3 Caster Level to all spells, +4 sacred attack and damage.", blueprintCue6, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = 4000,
				m_Fact = CosmicRelics.CosmicMythicPactFeature.ToReference<BlueprintUnitFactReference>()
			});
			blueprintCue6.Answers.Add(bp70.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue6.Answers.Add(bp71.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue6.Answers.Add(bp72.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue6.Answers.Add(bp73.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue6.Answers.Add(bp15.ToReference<BlueprintAnswerBaseReference>());
			(string, string, string)[] array = new(string, string, string)[14]
			{
				("AnswerSwitchFealtyCayden", "300e212868bca984687c92bcb66d381b", "Cayden Cailean"),
				("AnswerSwitchFealtyDesna", "2c0a3b9971327ba4d9d85354d16998c1", "Desna"),
				("AnswerSwitchFealtyIomedae", "88d5da04361b16746bf5b65795e0c38c", "Iomedae"),
				("AnswerSwitchFealtyPharasma", "458750bc214ab2e44abdeae404ab22e9", "Pharasma"),
				("AnswerSwitchFealtyCalistria", "c7531715a3f046d4da129619be63f44c", "Calistria"),
				("AnswerSwitchFealtyNethys", "6262cfce7c31626458325ca0909de997", "Nethys"),
				("AnswerSwitchFealtyAsmodeus", "a3a5ccc9c670e6f4ca4a686d23b89900", "Asmodeus"),
				("AnswerSwitchFealtyGorum", "8f49a5d8528a82c44b8c117a89f6b68c", "Gorum"),
				("AnswerSwitchFealtyBesmara", "470d824767d34e2e9a59b6d70681af7a", "Besmara"),
				("AnswerSwitchFealtyLanternKing", "ed7ba1da81834e55be5530ecbc2b1464", "The Lantern King"),
				("AnswerSwitchFealtyChaldira", "47a505b2df6a4401826d708ca6723223", "Chaldira"),
				("AnswerSwitchFealtyMilani", "68fb3a3c9b744951b14ea9f26bf5d1f8", "Milani"),
				("AnswerSwitchFealtyBlackButterfly", "1f29c6dc4aa940aa8c16ca61c470fe4c", "Black Butterfly"),
				("AnswerSwitchFealtyAtheism", "92c0d2da0a836ce418a267093c09ca54", "Atheism (The Free Agent)")
			};
			for (int num = 0; num < array.Length; num++)
			{
				(string, string, string) tuple = array[num];
				BlueprintFeature blueprintFeature = BlueprintSafetyExtensions.SafeGetBlueprint<BlueprintFeature>(tuple.Item2);
				if (blueprintFeature == null && tuple.Item3 == "Chaldira")
				{
					blueprintFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ChaldiraFeature");
				}
				if (blueprintFeature != null)
				{
					BlueprintAnswer bp74 = CreateStoreAnswer(tuple.Item1, "[Switch Patron] Align your soul with " + tuple.Item3, blueprintCue7, new ContextActionSwitchDeity
					{
						m_NewDeity = blueprintFeature.ToReference<BlueprintFeatureReference>()
					});
					blueprintCue7.Answers.Add(bp74.ToReference<BlueprintAnswerBaseReference>());
				}
			}
			(string, BlueprintFeature, string)[] array2 = new(string, BlueprintFeature, string)[11]
			{
				("AnswerAscendAvatarCayden", DeityAvatarAscension.AvatarCaydenFeature, "Avatar of the Lucky Drunk (Cayden Cailean)"),
				("AnswerAscendAvatarIomedae", DeityAvatarAscension.AvatarIomedaeFeature, "Avatar of the Inheritor (Iomedae)"),
				("AnswerAscendAvatarAsmodeus", DeityAvatarAscension.AvatarAsmodeusFeature, "Avatar of the Archfiend (Asmodeus)"),
				("AnswerAscendAvatarDesna", DeityAvatarAscension.AvatarDesnaFeature, "Avatar of the Song of the Spheres (Desna)"),
				("AnswerAscendAvatarPharasma", DeityAvatarAscension.AvatarPharasmaFeature, "Avatar of the Lady of Graves (Pharasma)"),
				("AnswerAscendAvatarCalistria", DeityAvatarAscension.AvatarCalistriaFeature, "Avatar of the Savored Sting (Calistria)"),
				("AnswerAscendAvatarNethys", DeityAvatarAscension.AvatarNethysFeature, "Avatar of the All-Seeing Eye (Nethys)"),
				("AnswerAscendAvatarGorum", DeityAvatarAscension.AvatarGorumFeature, "Avatar of Our Lord in Iron (Gorum)"),
				("AnswerAscendAvatarBesmara", DeityAvatarAscension.AvatarBesmaraFeature, "Avatar of the Pirate Queen (Besmara)"),
				("AnswerAscendAvatarLanternKing", DeityAvatarAscension.AvatarLanternKingFeature, "Avatar of the Prankster (Lantern King)"),
				("AnswerAscendAvatarChaldira", DeityAvatarAscension.AvatarChaldiraFeature, "Avatar of Audacious Fortune (Chaldira)")
			};
			for (int num = 0; num < array2.Length; num++)
			{
				(string, BlueprintFeature, string) tuple2 = array2[num];
				if (tuple2.Item2 != null)
				{
					BlueprintAnswer bp75 = CreateStoreAnswer(tuple2.Item1, "<color=#F5C542>[Ascend]</color> Embody the " + tuple2.Item3 + " (10,000 Coins)", blueprintCue7, new ContextActionAscendAvatar
					{
						Cost = 10000,
						m_AvatarFeature = tuple2.Item2.ToReference<BlueprintFeatureReference>()
					});
					blueprintCue7.Answers.Add(bp75.ToReference<BlueprintAnswerBaseReference>());
				}
			}
			blueprintCue7.Answers.Add(bp16.ToReference<BlueprintAnswerBaseReference>());
			BlueprintFeature blueprint = BlueprintTools.GetBlueprint<BlueprintFeature>("92c0d2da0a836ce418a267093c09ca54");
			BlueprintAnswer bp76 = CreateStoreAnswer("AnswerEmbraceGodDefier", "<color=#F5C542>[Ascend]</color> Shatter divine chains and walk the Path of the God Defier! (Cost: 10,000 Coins)", CosmicStoreRootCue, new ContextActionAscendAvatar
			{
				Cost = 10000,
				m_AvatarFeature = DeityAvatarAscension.GodDefierFeature.ToReference<BlueprintFeatureReference>()
			}, (blueprint != null) ? new HasFact
			{
				Unit = new PlayerCharacter(),
				m_Fact = blueprint.ToReference<BlueprintUnitFactReference>()
			} : null);
			blueprintCue8.Answers.Add(bp76.ToReference<BlueprintAnswerBaseReference>());
			blueprintCue8.Answers.Add(bp17.ToReference<BlueprintAnswerBaseReference>());
			Helpers.CreateBlueprint(Main.IsekaiContext, "OpenCosmicStoreAbility", delegate(BlueprintAbility blueprintAbility)
			{
				blueprintAbility.SetName(Main.IsekaiContext, "Commune with Constellations");
				blueprintAbility.SetDescription(Main.IsekaiContext, "Open the Cosmic Constellation Exchange dialog to browse offerings, redeem accumulated Cosmic Coins for divine blessings, mythic boons, web novel elixirs, modular wishes, infusions, switch patron deities, or ascend as a divine Avatar.");
				((BlueprintUnitFact)blueprintAbility).m_Icon = Icon_Coin;
				blueprintAbility.Type = AbilityType.Special;
				blueprintAbility.Range = AbilityRange.Personal;
				blueprintAbility.CanTargetSelf = true;
				blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				blueprintAbility.ActionType = UnitCommand.CommandType.Free;
				blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionOpenCosmicStoreDialog a)
					{
						a.m_Dialog = CosmicStoreDialog.ToReference<BlueprintDialogReference>();
					});
				});
			});
			DivineTokens.ItemCosmicCoin = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemCosmicCoin", delegate(BlueprintItemEquipmentUsable blueprintItemEquipmentUsable)
			{
				blueprintItemEquipmentUsable.SetName(Main.IsekaiContext, "Cosmic Coin");
				blueprintItemEquipmentUsable.SetDescription(Main.IsekaiContext, "A shimmering token forged from condensed divine favor and audience entertainment. Awarded by observing deities and higher planar entities when celebrating your heroic triumphs, audacity, and choices.\n\nYour inventory stack automatically synchronizes with your accumulated Cosmic Coins. Can be traded with the Astral Coin Envoy and planar merchants at the Planar Coliseum.");
				((BlueprintItem)blueprintItemEquipmentUsable).m_Icon = Icon_Coin;
				((BlueprintItem)blueprintItemEquipmentUsable).m_Cost = 0;
				((BlueprintItem)blueprintItemEquipmentUsable).m_Weight = 0f;
				((BlueprintItem)blueprintItemEquipmentUsable).m_IsNotable = true;
				((BlueprintItem)blueprintItemEquipmentUsable).m_ForceStackable = true;
				((BlueprintItem)blueprintItemEquipmentUsable).m_Destructible = false;
				blueprintItemEquipmentUsable.Type = UsableItemType.Other;
				blueprintItemEquipmentUsable.SpendCharges = false;
				blueprintItemEquipmentUsable.Charges = 0;
				blueprintItemEquipmentUsable.RestoreChargesOnRest = false;
			});
			CosmicSponsorshipExchangeFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicSponsorshipExchangeFeature", delegate(BlueprintFeature blueprintFeature2)
			{
				blueprintFeature2.SetName(Main.IsekaiContext, "Cosmic Sponsorship Exchange");
				blueprintFeature2.SetDescription(Main.IsekaiContext, "Allows you to commune with the observing constellations and redeem accumulated Cosmic Coins through a celestial store interface, offering divine blessings, mythic boons, web novel elixirs, modular wishes, cosmic infusions, and avatar ascensions.");
				((BlueprintUnitFact)blueprintFeature2).m_Icon = Icon_Coin;
				blueprintFeature2.IsClassFeature = true;
				blueprintFeature2.AddComponent(delegate(AddAbilityResources c)
				{
					c.m_Resource = CosmicWishResource.ToReference<BlueprintAbilityResourceReference>();
					c.RestoreAmount = false;
				});
				storeAbilities.Add(ModularCosmicWishAbility);
				blueprintFeature2.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { ModularCosmicWishAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
			void AddBlessing(string name, string displayName, string desc, int cost, Action<BlueprintBuff> configureBuff)
			{
				BlueprintBuff buff = TTCoreExtensions.CreateBuff(name + "Buff", delegate(BlueprintBuff blueprintBuff)
				{
					blueprintBuff.SetName(Main.IsekaiContext, displayName);
					blueprintBuff.SetDescription(Main.IsekaiContext, desc);
					((BlueprintUnitFact)blueprintBuff).m_Icon = Icon_Coin;
					blueprintBuff.m_Flags = BlueprintBuff.Flags.StayOnDeath;
					configureBuff(blueprintBuff);
				});
				blessingBuffs.Add((name, buff, cost, displayName, desc));
				BlueprintAbility item8 = Helpers.CreateBlueprint(Main.IsekaiContext, name + "Ability", delegate(BlueprintAbility blueprintAbility)
				{
					blueprintAbility.SetName(Main.IsekaiContext, "Sponsorship: " + displayName);
					blueprintAbility.SetDescription(Main.IsekaiContext, $"Spend {cost} Cosmic Coins to invoke this divine blessing for 1 hour (persists through death).\n{desc}");
					((BlueprintUnitFact)blueprintAbility).m_Icon = Icon_Coin;
					blueprintAbility.Type = AbilityType.Special;
					blueprintAbility.Range = AbilityRange.Personal;
					blueprintAbility.CanTargetSelf = true;
					blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
					blueprintAbility.ActionType = UnitCommand.CommandType.Standard;
					blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
					{
						c.Actions = ActionFlow.DoSingle(delegate(ContextActionSpendCoinsAndApplyBuff a)
						{
							a.Cost = cost;
							a.m_Buff = buff.ToReference<BlueprintBuffReference>();
							a.DurationHours = 1;
						});
					});
				});
				storeAbilities.Add(item8);
			}
			void AddInfusion(string name, string displayName, string desc, int cost, Action<BlueprintFeature> configureFeature)
			{
				BlueprintFeature feat = Helpers.CreateBlueprint(Main.IsekaiContext, name + "Feature", delegate(BlueprintFeature blueprintFeature2)
				{
					blueprintFeature2.SetName(Main.IsekaiContext, displayName);
					blueprintFeature2.SetDescription(Main.IsekaiContext, desc);
					((BlueprintUnitFact)blueprintFeature2).m_Icon = Icon_Coin;
					blueprintFeature2.IsClassFeature = true;
					configureFeature(blueprintFeature2);
				});
				infusionList.Add((name, feat, cost, displayName, desc));
				BlueprintAbility item8 = Helpers.CreateBlueprint(Main.IsekaiContext, name + "Ability", delegate(BlueprintAbility blueprintAbility)
				{
					blueprintAbility.SetName(Main.IsekaiContext, "Infusion: " + displayName);
					blueprintAbility.SetDescription(Main.IsekaiContext, $"Spend {cost} Cosmic Coins to permanently infuse yourself with this enhancement.\n{desc}");
					((BlueprintUnitFact)blueprintAbility).m_Icon = Icon_Coin;
					blueprintAbility.Type = AbilityType.Special;
					blueprintAbility.Range = AbilityRange.Personal;
					blueprintAbility.CanTargetSelf = true;
					blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
					blueprintAbility.ActionType = UnitCommand.CommandType.Standard;
					blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
					{
						c.Actions = ActionFlow.DoSingle(delegate(ContextActionSpendCoinsAndGrantFact a)
						{
							a.Cost = cost;
							a.m_Fact = feat.ToReference<BlueprintUnitFactReference>();
						});
					});
				});
				storeAbilities.Add(item8);
			}
			void AddMythicBoon(string name, string displayName, string desc, int cost, AlignmentMaskType alignment, Action<BlueprintFeature> configureFeature)
			{
				BlueprintFeature feat = Helpers.CreateBlueprint(Main.IsekaiContext, name + "Feature", delegate(BlueprintFeature blueprintFeature2)
				{
					blueprintFeature2.SetName(Main.IsekaiContext, displayName);
					blueprintFeature2.SetDescription(Main.IsekaiContext, desc);
					((BlueprintUnitFact)blueprintFeature2).m_Icon = Icon_Coin;
					blueprintFeature2.IsClassFeature = true;
					configureFeature(blueprintFeature2);
				});
				mythicBoonList.Add((name, feat, cost, alignment, displayName, desc));
				BlueprintAbility item8 = Helpers.CreateBlueprint(Main.IsekaiContext, name + "Ability", delegate(BlueprintAbility blueprintAbility)
				{
					blueprintAbility.SetName(Main.IsekaiContext, "Mythic Boon: " + displayName);
					blueprintAbility.SetDescription(Main.IsekaiContext, $"Spend {cost} Cosmic Coins to permanently acquire this mythic path boon.\n{desc}");
					((BlueprintUnitFact)blueprintAbility).m_Icon = Icon_Coin;
					blueprintAbility.Type = AbilityType.Special;
					blueprintAbility.Range = AbilityRange.Personal;
					blueprintAbility.CanTargetSelf = true;
					blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
					blueprintAbility.ActionType = UnitCommand.CommandType.Standard;
					blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
					{
						c.Actions = ActionFlow.DoSingle(delegate(ContextActionSpendCoinsAndGrantFact a)
						{
							a.Cost = cost;
							a.m_Fact = feat.ToReference<BlueprintUnitFactReference>();
							a.m_KeyOfInfinitePathways = KeyOfInfinitePathwaysFeature.ToReference<BlueprintFeatureReference>();
							a.AllowedAlignment = alignment;
						});
					});
				});
				storeAbilities.Add(item8);
			}
			void AddSubclassBlessing(string name, string displayName, string desc, string requiredFactName, Action<BlueprintFeature> configureAwakened)
			{
				BlueprintFeature awakenedFeat = Helpers.CreateBlueprint(Main.IsekaiContext, name + "AwakenedFeature", delegate(BlueprintFeature blueprintFeature2)
				{
					blueprintFeature2.SetName(Main.IsekaiContext, displayName + " (Awakened)");
					blueprintFeature2.SetDescription(Main.IsekaiContext, desc);
					((BlueprintUnitFact)blueprintFeature2).m_Icon = Icon_Coin;
					blueprintFeature2.IsClassFeature = true;
					configureAwakened(blueprintFeature2);
				});
				BlueprintFeature masterFeat = Helpers.CreateBlueprint(Main.IsekaiContext, name + "Feature", delegate(BlueprintFeature blueprintFeature2)
				{
					blueprintFeature2.SetName(Main.IsekaiContext, displayName);
					blueprintFeature2.SetDescription(Main.IsekaiContext, desc + "\n<i>(Requires active attunement to this subclass; automatically hibernates if respecced and re-awakens upon return)</i>");
					((BlueprintUnitFact)blueprintFeature2).m_Icon = Icon_Coin;
					blueprintFeature2.IsClassFeature = true;
					blueprintFeature2.AddComponent(delegate(AddFeatureIfHasFact c)
					{
						c.m_CheckedFact = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, requiredFactName)?.ToReference<BlueprintUnitFactReference>();
						c.m_Feature = awakenedFeat.ToReference<BlueprintUnitFactReference>();
						c.Not = false;
					});
				});
				subclassBoonList.Add((name, masterFeat, displayName, desc));
				BlueprintAbility item8 = Helpers.CreateBlueprint(Main.IsekaiContext, name + "Ability", delegate(BlueprintAbility blueprintAbility)
				{
					blueprintAbility.SetName(Main.IsekaiContext, "Transcendence: " + displayName);
					blueprintAbility.SetDescription(Main.IsekaiContext, "Spend 100 Cosmic Coins to acquire this permanent subclass transcendence blessing.\n" + desc);
					((BlueprintUnitFact)blueprintAbility).m_Icon = Icon_Coin;
					blueprintAbility.Type = AbilityType.Special;
					blueprintAbility.Range = AbilityRange.Personal;
					blueprintAbility.CanTargetSelf = true;
					blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
					blueprintAbility.ActionType = UnitCommand.CommandType.Standard;
					blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
					{
						c.Actions = ActionFlow.DoSingle(delegate(ContextActionSpendCoinsAndGrantFact a)
						{
							a.Cost = 100;
							a.m_Fact = masterFeat.ToReference<BlueprintUnitFactReference>();
						});
					});
				});
				storeAbilities.Add(item8);
			}
			static BlueprintAnswer CreateStoreAnswer(string name, string text, BlueprintCue nextCue, GameAction onSelectAction = null, Condition showCondition = null)
			{
				return TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer blueprintAnswer)
				{
					blueprintAnswer.SetText(Main.IsekaiContext, text);
					blueprintAnswer.NextCue = new CueSelection
					{
						Cues = ((nextCue != null) ? new List<BlueprintCueBaseReference> { nextCue.ToReference<BlueprintCueBaseReference>() } : new List<BlueprintCueBaseReference>()),
						Strategy = Strategy.First
					};
					blueprintAnswer.ShowConditions = ((showCondition != null) ? ActionFlow.IfAll(showCondition) : ActionFlow.IfAll());
					blueprintAnswer.OnSelect = ((onSelectAction != null) ? new ActionList
					{
						Actions = new GameAction[1] { onSelectAction }
					} : ActionFlow.DoNothing());
				});
			}
			BlueprintAbility CreateWishVariant(string name, string statName, StatType stat, int bonus = 2)
			{
				BlueprintFeature feat = Helpers.CreateBlueprint(Main.IsekaiContext, name + "Feature", delegate(BlueprintFeature f)
				{
					f.SetName(Main.IsekaiContext, $"Cosmic Boon: {statName} +{bonus}");
					f.SetDescription(Main.IsekaiContext, $"+{bonus} untyped stackable bonus to {statName}");
					((BlueprintUnitFact)f).m_Icon = Icon_Coin;
					f.Ranks = 999;
					f.AddComponent(delegate(AddContextStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.UntypedStackable;
						c.Stat = stat;
						c.Multiplier = bonus;
						c.Value = new ContextValue
						{
							ValueType = ContextValueType.Rank,
							ValueRank = AbilityRankType.Default
						};
					});
					f.AddComponent(delegate(ContextRankConfig c)
					{
						c.m_Type = AbilityRankType.Default;
						c.m_BaseValueType = ContextRankBaseValueType.FeatureRank;
						c.m_Feature = f.ToReference<BlueprintFeatureReference>();
					});
				});
				return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintAbility blueprintAbility)
				{
					blueprintAbility.SetName(Main.IsekaiContext, $"Cosmic Wish: Permanent +{bonus} {statName}");
					blueprintAbility.SetDescription(Main.IsekaiContext, $"Consumes 1 Wish charge to permanently grant a +{bonus} untyped stackable bonus to {statName}.");
					((BlueprintUnitFact)blueprintAbility).m_Icon = Icon_Coin;
					blueprintAbility.Type = AbilityType.Special;
					blueprintAbility.Range = AbilityRange.Personal;
					blueprintAbility.CanTargetSelf = true;
					blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
					blueprintAbility.ActionType = UnitCommand.CommandType.Standard;
					blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
					{
						c.Actions = ActionFlow.DoSingle(delegate(ContextActionConsumeStackableConsumable a)
						{
							a.m_Feature = feat.ToReference<BlueprintFeatureReference>();
							a.ItemName = "Cosmic Wish (" + statName + ")";
							a.ValuePerRank = bonus;
							a.StatDisplayName = statName;
						});
					});
					blueprintAbility.AddComponent(delegate(AbilityResourceLogic c)
					{
						c.m_RequiredResource = CosmicWishResource.ToReference<BlueprintAbilityResourceReference>();
						c.m_IsSpendResource = true;
						c.Amount = 1;
					});
				});
			}
		}
	}
}
