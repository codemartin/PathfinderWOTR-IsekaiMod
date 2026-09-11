using IsekaiMod.Utilities;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.Armies.TacticalCombat.Parts;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.ElementsSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace IsekaiMod.Components
{
	[ComponentName("Killed Enemy Trigger")]
	[AllowedOn(typeof(BlueprintUnitFact), false)]
	[TypeId("83ffe0002e564ab2a97029983b4b409a")]
	public class GainExperienceOnKill : UnitFactComponentDelegate<AddOutgoingDamageTrigger.ComponentData>, IInitiatorRulebookHandler<RuleDealDamage>, IRulebookHandler<RuleDealDamage>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleDrainEnergy>, IRulebookHandler<RuleDrainEnergy>, IInitiatorRulebookHandler<RuleDealStatDamage>, IRulebookHandler<RuleDealStatDamage>
	{
		public class ComponentData
		{
			public bool WasTargetAlive;
		}

		public void OnEventAboutToTrigger(RuleDealDamage evt)
		{
			SetDataTargetWasAlive(evt);
		}

		public void OnEventAboutToTrigger(RuleDrainEnergy evt)
		{
			SetDataTargetWasAlive(evt);
		}

		public void OnEventAboutToTrigger(RuleDealStatDamage evt)
		{
			SetDataTargetWasAlive(evt);
		}

		public void OnEventDidTrigger(RuleDealDamage evt)
		{
			GainExperience(evt);
		}

		public void OnEventDidTrigger(RuleDrainEnergy evt)
		{
			GainExperience(evt);
		}

		public void OnEventDidTrigger(RuleDealStatDamage evt)
		{
			GainExperience(evt);
		}

		private void SetDataTargetWasAlive(RulebookTargetEvent evt)
		{
			if (base.Data != null && !(evt.Initiator != base.Owner) && evt.Target?.Descriptor != null)
			{
				if (TacticalCombatHelper.IsActive)
				{
					int num = evt.Target.Get<UnitPartTacticalCombat>()?.Count ?? 1;
					base.Data.WasTargetAlive = num > TacticalCombatHelper.GetDeathCount(evt.Target, evt.Target.HPLeft, num);
				}
				else
				{
					base.Data.WasTargetAlive = !evt.Target.Descriptor.State.IsDead;
				}
			}
		}

		private void GainExperience(RulebookTargetEvent evt)
		{
			if (base.Data == null || evt.Initiator != base.Owner || evt.Target?.Stats == null || evt.Target.Blueprint == null)
			{
				return;
			}
			bool flag;
			if (TacticalCombatHelper.IsActive)
			{
				int num = evt.Target.Get<UnitPartTacticalCombat>()?.Count ?? 1;
				flag = num <= TacticalCombatHelper.GetDeathCount(evt.Target, evt.Target.HPLeft, num);
			}
			else
			{
				flag = (int)evt.Target.Stats.HitPoints <= evt.Target.Damage;
			}
			if (!base.Data.WasTargetAlive || !flag || !(base.Fact is IFactContextOwner factContextOwner))
			{
				return;
			}
			Experience experience = evt.Target.Blueprint.GetComponent<Experience>();
			if (experience != null)
			{
				ActionList action = ActionFlow.DoSingle(delegate(GainExp c)
				{
					c.GainPureExp = false;
					c.Encounter = experience.Encounter;
					c.CR = experience.CR;
					c.Modifier = experience.Modifier;
					c.Count = new IntConstant
					{
						Value = 1
					};
				});
				factContextOwner.RunActionInContext(action, base.Owner);
			}
		}
	}
}
