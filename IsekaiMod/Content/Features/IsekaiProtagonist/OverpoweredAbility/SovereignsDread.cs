using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class SovereignsDread
	{
		private static readonly Sprite Icon_Dread = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("4c349361d720e844e846ad8c19959b1e"))?.m_Icon;

		public static void Add()
		{
			BlueprintBuff SovereignsDreadPressureBuff = TTCoreExtensions.CreateBuff("SovereignsDreadPressureBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Sovereign Pressure");
				bp.SetDescription(Main.IsekaiContext, "Crushed by absolute sovereign majesty, you suffer a -4 penalty to attack rolls, AC, and saving throws.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dread;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = -4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.AC;
					c.Value = -4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveFortitude;
					c.Value = -4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveReflex;
					c.Value = -4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveWill;
					c.Value = -4;
				});
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.Shaken;
				});
			});
			BlueprintBuff SovereignsDreadCollapseBuff = TTCoreExtensions.CreateBuff("SovereignsDreadCollapseBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Sovereign Submission");
				bp.SetDescription(Main.IsekaiContext, "Forced to kneel before an otherworldly sovereign. You are Cowering and Prone.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dread;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.Cowering;
				});
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.Prone;
				});
			});
			BlueprintAbilityAreaEffect SovereignsDreadArea = Helpers.CreateBlueprint(Main.IsekaiContext, "SovereignsDreadArea", delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Enemy;
				bp.Shape = AreaEffectShape.Cylinder;
				bp.Size = new Feet(30f);
				bp.AffectEnemies = true;
				bp.Fx = new PrefabLink();
				bp.AddComponent(delegate(ContextCalculateAbilityParams c)
				{
					c.UseKineticistMainStat = false;
					c.StatType = StatType.Charisma;
				});
				bp.AddComponent(delegate(AbilityAreaEffectRunAction c)
				{
					c.UnitEnter = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = SovereignsDreadPressureBuff.ToReference<BlueprintBuffReference>(),
						Permanent = true,
						DurationValue = Values.Duration.Zero
					}, new ContextActionSavingThrow
					{
						Type = SavingThrowType.Will,
						Actions = ActionFlow.DoSingle(delegate(ContextActionConditionalSaved s)
						{
							s.Failed = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
							{
								b.m_Buff = SovereignsDreadCollapseBuff.ToReference<BlueprintBuffReference>();
								b.Permanent = false;
								b.DurationValue = new ContextDurationValue
								{
									Rate = DurationRate.Rounds,
									DiceType = DiceType.Zero,
									BonusValue = 1
								};
							});
						})
					});
					c.UnitExit = Helpers.CreateActionList(new ContextActionRemoveBuff
					{
						m_Buff = SovereignsDreadPressureBuff.ToReference<BlueprintBuffReference>(),
						OnlyFromCaster = true
					});
					c.UnitMove = ActionFlow.DoNothing();
					c.Round = Helpers.CreateActionList(new ContextActionSavingThrow
					{
						Type = SavingThrowType.Will,
						Actions = ActionFlow.DoSingle(delegate(ContextActionConditionalSaved s)
						{
							s.Failed = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
							{
								b.m_Buff = SovereignsDreadCollapseBuff.ToReference<BlueprintBuffReference>();
								b.Permanent = false;
								b.DurationValue = new ContextDurationValue
								{
									Rate = DurationRate.Rounds,
									DiceType = DiceType.Zero,
									BonusValue = 1
								};
							});
						})
					});
				});
			});
			BlueprintBuff SovereignsDreadAreaBuff = TTCoreExtensions.CreateBuff("SovereignsDreadAreaBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Sovereign's Dread");
				bp.SetDescription(Main.IsekaiContext, "You radiate the overwhelming gravitational spiritual pressure of a true supreme sovereign (Conqueror's Haki / Despair Aura). Enemies within 30 feet suffer a -4 penalty to attack rolls, Armor Class, and saving throws. Furthermore, each round enemies in the aura must succeed at a Will saving throw (DC = 10 + 1/2 character level + Charisma modifier) or collapse Prone and become Cowering for 1 round under your sovereign majesty.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dread;
				bp.IsClassFeature = true;
				bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
				bp.AddComponent(delegate(AddAreaEffect c)
				{
					c.m_AreaEffect = SovereignsDreadArea.ToReference<BlueprintAbilityAreaEffectReference>();
				});
			});
			BlueprintActivatableAbility SovereignsDreadAbility = TTCoreExtensions.CreateActivatableAbility("SovereignsDreadAbility", delegate(BlueprintActivatableAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Sovereign's Dread");
				bp.SetDescription(Main.IsekaiContext, "You radiate the overwhelming gravitational spiritual pressure of a true supreme sovereign (Conqueror's Haki / Despair Aura). Enemies within 30 feet suffer a -4 penalty to attack rolls, Armor Class, and saving throws. Furthermore, each round enemies in the aura must succeed at a Will saving throw (DC = 10 + 1/2 character level + Charisma modifier) or collapse Prone and become Cowering for 1 round under your sovereign majesty.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dread;
				bp.m_Buff = SovereignsDreadAreaBuff.ToReference<BlueprintBuffReference>();
				bp.DoNotTurnOffOnRest = true;
			});
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "SovereignsDreadFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Sovereign's Dread");
				bp.SetDescription(Main.IsekaiContext, "You radiate the overwhelming gravitational spiritual pressure of a true supreme sovereign (Conqueror's Haki / Despair Aura). Enemies within 30 feet suffer a -4 penalty to attack rolls, Armor Class, and saving throws. Furthermore, each round enemies in the aura must succeed at a Will saving throw (DC = 10 + 1/2 character level + Charisma modifier) or collapse Prone and become Cowering for 1 round under your sovereign majesty.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dread;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { SovereignsDreadAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 10;
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
		}
	}
}
