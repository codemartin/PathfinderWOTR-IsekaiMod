using System;
using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic;
using Kingmaker.View;
using UnityEngine;

namespace IsekaiMod.Content.Guardians
{
	[TypeId("7e0a4f6111394c5ca60a7751f89382f1")]
	public class GuardianCompanionBarksComponent : UnitFactComponentDelegate, IUnitCombatHandler, ISubscriber, IGlobalSubscriber, IUnitRestHandler, IUnitClickUIHandler
	{
		public string[] CombatStartBarks = new string[0];

		public string[] CombatEndBarks = new string[0];

		public string[] RestBarks = new string[0];

		public string[] ClickBarks = new string[0];

		[NonSerialized]
		private float m_LastBarkTime = -10f;

		public void HandleUnitJoinCombat(UnitEntityData unit)
		{
			if (!(unit != base.Owner))
			{
				TriggerBark(CombatStartBarks);
			}
		}

		public void HandleUnitLeaveCombat(UnitEntityData unit)
		{
			if (!(unit != base.Owner))
			{
				TriggerBark(CombatEndBarks);
			}
		}

		public void HandleUnitRest(UnitEntityData unit)
		{
			if (!(base.Owner == null) && !(unit == null) && (!(unit != base.Owner) || !(unit != base.Owner.Master)))
			{
				TriggerBark(RestBarks);
			}
		}

		public void HandleUnitRightClick(UnitEntityView unit)
		{
			if (!(unit == null) && !(unit.Data != base.Owner))
			{
				TriggerBark(ClickBarks);
			}
		}

		private void TriggerBark(string[] lines)
		{
			if (lines == null || lines.Length == 0 || base.Owner == null || base.Owner.Descriptor == null || !base.Owner.Descriptor.State.IsConscious || Time.time - m_LastBarkTime < 4f)
			{
				return;
			}
			m_LastBarkTime = Time.time;
			string text = lines[UnityEngine.Random.Range(0, lines.Length)];
			if (!string.IsNullOrEmpty(text))
			{
				if (text.Contains("{name}"))
				{
					string newValue = ((base.Owner != null && base.Owner.Master != null && !string.IsNullOrEmpty(base.Owner.Master.CharacterName)) ? base.Owner.Master.CharacterName : "Master");
					text = text.Replace("{name}", newValue);
				}
				float num = UIUtility.GetBarkDuration(text);
				if (num < 3f)
				{
					num = 3f;
				}
				Game.Instance?.UI?.Bark(base.Owner, text, num);
			}
		}
	}
}
