using System;
using System.Linq;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities
{
	public static class ActionFlow
	{
		public static ActionList DoNothing()
		{
			return Helpers.CreateActionList();
		}

		public static ActionList DoSingle<T>(Action<T> init = null) where T : GameAction, new()
		{
			T val = new T();
			init?.Invoke(val);
			return Helpers.CreateActionList(val);
		}

		public static ActionList DealDamage(Action<ContextActionDealDamage> init = null)
		{
			ContextActionDealDamage contextActionDealDamage = new ContextActionDealDamage
			{
				m_Type = ContextActionDealDamage.Type.Damage,
				Duration = Values.Duration.Zero,
				UseWeaponDamageModifiers = true
			};
			init?.Invoke(contextActionDealDamage);
			return Helpers.CreateActionList(contextActionDealDamage);
		}

		public static ConditionsChecker IfSingle<T>(Action<T> init = null) where T : Condition, new()
		{
			T val = new T();
			init?.Invoke(val);
			ConditionsChecker conditionsChecker = new ConditionsChecker();
			conditionsChecker.Conditions = new Condition[1] { val };
			return conditionsChecker;
		}

		public static ConditionsChecker IfAll(params Condition[] conditions)
		{
			return new ConditionsChecker
			{
				Conditions = (conditions?.Where((Condition c) => c != null).ToArray() ?? Array.Empty<Condition>()),
				Operation = Operation.And
			};
		}

		public static ConditionsChecker IfAny(params Condition[] conditions)
		{
			return new ConditionsChecker
			{
				Conditions = (conditions?.Where((Condition c) => c != null).ToArray() ?? Array.Empty<Condition>()),
				Operation = Operation.Or
			};
		}

		public static ConditionsChecker EmptyCondition()
		{
			return new ConditionsChecker
			{
				Conditions = Array.Empty<Condition>()
			};
		}
	}
}
