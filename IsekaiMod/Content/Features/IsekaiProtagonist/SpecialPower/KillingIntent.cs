using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
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
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class KillingIntent
	{
		private static readonly Sprite Icon_ConsumeFear = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("644d2c0d029e54d4188bc34216d9d8c0")).m_Icon;

		private static readonly BlueprintBuff Shaken = BlueprintTools.GetBlueprint<BlueprintBuff>("25ec6cb6ab1845c48a95f9c20b034220");

		public static void Add()
		{
			LocalizedString KingmakerIntentDesc = Helpers.CreateString(Main.IsekaiContext, "KillingIntent.Description", "You emit an aura of lethal malice out to 40 feet. Enemies within this aura suffer a -2 penalty to attack rolls, AC, and saving throws, and become Shaken. Furthermore, whenever enemies enter or begin their turn within the aura, they must succeed at a Will saving throw (DC = 10 + 1/2 character level + Charisma modifier) or become Staggered for 1 round from sheer terror.");
			BlueprintBuff KillingIntentDreadBuff = TTCoreExtensions.CreateBuff("KillingIntentDreadBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Overwhelming Dread");
				bp.SetDescription(Main.IsekaiContext, "Crushed by an overwhelming aura of lethal intent, you suffer a -2 penalty to attack rolls, AC, and saving throws, and are Shaken.");
				((BlueprintUnitFact)bp).m_Icon = Icon_ConsumeFear;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = -2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.AC;
					c.Value = -2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Penalty;
					c.Stat = StatType.SaveWill;
					c.Value = -2;
				});
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.Shaken;
				});
			});
			BlueprintBuff KillingIntentStaggerBuff = TTCoreExtensions.CreateBuff("KillingIntentStaggerBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Paralyzing Dread");
				bp.SetDescription(Main.IsekaiContext, "Frozen in terror by sheer killing intent, this creature is staggered for 1 round.");
				((BlueprintUnitFact)bp).m_Icon = Icon_ConsumeFear;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.Staggered;
				});
			});
			BlueprintAbilityAreaEffect KillingIntentArea = Helpers.CreateBlueprint(Main.IsekaiContext, "KillingIntentArea", delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Enemy;
				bp.Shape = AreaEffectShape.Cylinder;
				bp.Size = new Feet(40f);
				bp.AffectEnemies = true;
				bp.Fx = new PrefabLink();
				bp.AddComponent(delegate(AbilityAreaEffectRunAction c)
				{
					c.UnitEnter = Helpers.CreateActionList(new ContextActionApplyBuff
					{
						m_Buff = KillingIntentDreadBuff.ToReference<BlueprintBuffReference>(),
						Permanent = true,
						DurationValue = Values.Duration.Zero
					}, new ContextActionSavingThrow
					{
						Type = SavingThrowType.Will,
						Actions = ActionFlow.DoSingle(delegate(ContextActionConditionalSaved s)
						{
							s.Failed = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
							{
								b.m_Buff = KillingIntentStaggerBuff.ToReference<BlueprintBuffReference>();
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
						m_Buff = KillingIntentDreadBuff.ToReference<BlueprintBuffReference>(),
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
								b.m_Buff = KillingIntentStaggerBuff.ToReference<BlueprintBuffReference>();
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
			BlueprintBuff KillingIntentAreaBuff = TTCoreExtensions.CreateBuff("KillingIntentAreaBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Killing Intent");
				bp.SetDescription(KingmakerIntentDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_ConsumeFear;
				bp.IsClassFeature = true;
				bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
				bp.AddComponent(delegate(AddAreaEffect c)
				{
					c.m_AreaEffect = KillingIntentArea.ToReference<BlueprintAbilityAreaEffectReference>();
				});
			});
			BlueprintActivatableAbility KillingIntentAbility = TTCoreExtensions.CreateActivatableAbility("KillingIntentAbility", delegate(BlueprintActivatableAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Killing Intent");
				bp.SetDescription(KingmakerIntentDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_ConsumeFear;
				bp.m_Buff = KillingIntentAreaBuff.ToReference<BlueprintBuffReference>();
				bp.DoNotTurnOffOnRest = true;
			});
			SpecialPowerSelection.AddToAuthoritySelection(Helpers.CreateBlueprint(Main.IsekaiContext, "KillingIntentFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Killing Intent");
				bp.SetDescription(KingmakerIntentDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_ConsumeFear;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { KillingIntentAbility.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 5;
				});
			}));
		}
	}
}
