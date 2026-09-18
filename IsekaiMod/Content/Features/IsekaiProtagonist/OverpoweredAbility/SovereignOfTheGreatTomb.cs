using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Classes.IsekaiProtagonist.Archetypes;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class SovereignOfTheGreatTomb
	{
		private static readonly Sprite Icon_TombSovereign = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_AURA_DARK.png");

		public static void Add()
		{
			BlueprintBuff SovereignOfTheGreatTombBuff = TTCoreExtensions.CreateBuff("SovereignOfTheGreatTombBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Dominion of the Great Tomb");
				bp.SetDescription(Main.IsekaiContext, "Empowered by the Supreme Ruler of the Great Tomb, you gain a profane bonus to Strength, Dexterity, Constitution, Armor Class, attack rolls, and damage rolls (+2 at levels 1--9, +3 at levels 10--14, and +4 at level 15+), DR/Good (3 at levels 1--9, 6 at levels 10--14, and 10 at level 15+), and Fast Healing (2 at levels 1--9, 4 at levels 10--14, and 6 at level 15+).");
				((BlueprintUnitFact)bp).m_Icon = Icon_TombSovereign;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[3]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 9,
							ProgressionValue = 2
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 3
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 4
						}
					};
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Strength;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Dexterity;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Constitution;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AC;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.AdditionalDamage;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[3]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 9,
							ProgressionValue = 3
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 6
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 10
						}
					};
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
					c.BypassedByAlignment = true;
					c.Alignment = DamageAlignment.Good;
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.DamageBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[3]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 9,
							ProgressionValue = 2
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 4
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 6
						}
					};
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 0;
					c.Bonus = Values.CreateContextRankValue(AbilityRankType.DamageBonus);
				});
			});
			BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleAuraFeature("SovereignOfTheGreatTomb", Helpers.CreateString(Main.IsekaiContext, "SovereignOfTheGreatTomb.Name", "Overpowered Ability - Sovereign of the Great Tomb"), Helpers.CreateString(Main.IsekaiContext, "SovereignOfTheGreatTomb.Description", "Exclusive to the Overlord archetype. As absolute ruler of the supreme tomb and master of necrotic majesty, your necromantic authority is unmatched.\nBenefit: You emit a necrotic sovereign aura within 50 feet. Allies within the aura gain a profane bonus to Strength, Dexterity, Constitution, Armor Class, attack rolls, and damage rolls (+2 at levels 1--9, +3 at levels 10--14, and +4 at level 15+), DR/Good (3 at levels 1--9, 6 at levels 10--14, and 10 at level 15+), and Fast Healing (2 at levels 1--9, 4 at levels 10--14, and 6 at level 15+). Furthermore, you personally gain complete immunity to negative energy, ability drain, energy drain, and death effects, and a profane bonus to Intelligence and Charisma (+2 at levels 1--9, +3 at levels 10--14, and +4 at level 15+)."), Icon_TombSovereign, delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
				bp.SpellResistance = false;
				bp.AggroEnemies = false;
				bp.AffectEnemies = false;
				bp.Shape = AreaEffectShape.Cylinder;
				bp.Size = new Feet(50f);
				bp.AddComponent(delegate(AbilityAreaEffectRunAction c)
				{
					c.UnitEnter = ActionFlow.DoSingle(delegate(ContextActionApplyBuff a)
					{
						a.m_Buff = SovereignOfTheGreatTombBuff.ToReference<BlueprintBuffReference>();
						a.Permanent = true;
					});
					c.UnitExit = ActionFlow.DoSingle(delegate(ContextActionRemoveBuff r)
					{
						r.m_Buff = SovereignOfTheGreatTombBuff.ToReference<BlueprintBuffReference>();
					});
					c.UnitMove = ActionFlow.DoNothing();
					c.Round = ActionFlow.DoNothing();
				});
			});
			blueprintFeature.AddComponent(delegate(AddEnergyDamageImmunity c)
			{
				c.EnergyType = DamageEnergyType.NegativeEnergy;
			});
			blueprintFeature.AddComponent(delegate(AddImmunityToAbilityScoreDamage c)
			{
				c.Drain = true;
			});
			blueprintFeature.AddComponent<AddImmunityToEnergyDrain>();
			blueprintFeature.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Death | SpellDescriptor.NegativeLevel;
			});
			blueprintFeature.AddComponent(delegate(ContextRankConfig c)
			{
				c.m_Type = AbilityRankType.StatBonus;
				c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				c.m_Progression = ContextRankProgression.Custom;
				c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[3]
				{
					new ContextRankConfig.CustomProgressionItem
					{
						BaseValue = 9,
						ProgressionValue = 2
					},
					new ContextRankConfig.CustomProgressionItem
					{
						BaseValue = 14,
						ProgressionValue = 3
					},
					new ContextRankConfig.CustomProgressionItem
					{
						BaseValue = 100,
						ProgressionValue = 4
					}
				};
			});
			blueprintFeature.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Profane;
				c.Stat = StatType.Intelligence;
				c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
			});
			blueprintFeature.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Profane;
				c.Stat = StatType.Charisma;
				c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteArchetypeLevel c)
			{
				c.m_CharacterClass = IsekaiProtagonistClass.GetReference();
				c.m_Archetype = OverlordArchetype.GetReference();
				c.Level = 1;
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
		}
	}
}
