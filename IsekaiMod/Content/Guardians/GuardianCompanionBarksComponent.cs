using System;
using System.Collections.Generic;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.GameModes;
using Kingmaker.Localization;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic;
using Kingmaker.View;
using UnityEngine;

namespace IsekaiMod.Content.Guardians;

[TypeId("4c50a7786d2b48a1ac8ff7e5dbed7f95")]
public class GuardianCompanionBarksComponent : UnitFactComponentDelegate, IUnitCombatHandler, ISubscriber, IGlobalSubscriber, IUnitRestHandler, IUnitClickUIHandler, ISelectionHandler
{
	public LocalizedString[] CombatStartBarks = new LocalizedString[0];

	public LocalizedString[] CombatEndBarks = new LocalizedString[0];

	public LocalizedString[] RestBarks = new LocalizedString[0];

	public LocalizedString[] ClickBarks = new LocalizedString[0];

	public void HandleUnitJoinCombat(UnitEntityData unit)
	{
		if (!(unit == null) && !(unit != base.Owner))
		{
			TriggerBark(GuardianBarkRules.BarkCategory.CombatStart, CombatStartBarks);
		}
	}

	public void HandleUnitLeaveCombat(UnitEntityData unit)
	{
		if (!(unit == null) && !(unit != base.Owner))
		{
			TriggerBark(GuardianBarkRules.BarkCategory.CombatEnd, CombatEndBarks);
		}
	}

	public void HandleUnitRest(UnitEntityData unit)
	{
		if (!(base.Owner == null) && !(unit == null) && (!(unit != base.Owner) || !(unit != base.Owner.Master)))
		{
			TriggerBark(GuardianBarkRules.BarkCategory.Rest, RestBarks);
		}
	}

	public void HandleUnitRightClick(UnitEntityView unit)
	{
		if (!(unit == null) && !(unit.Data != base.Owner))
		{
			TriggerBark(GuardianBarkRules.BarkCategory.Click, ClickBarks);
		}
	}

	public void OnUnitSelectionAdd(UnitEntityData selected)
	{
		if (!(selected == null) && !(selected != base.Owner))
		{
			List<UnitEntityData> list = Game.Instance?.SelectionCharacter?.SelectedUnits;
			if (list != null && list.Count == 1)
			{
				TriggerBark(GuardianBarkRules.BarkCategory.Click, ClickBarks);
			}
		}
	}

	public void OnUnitSelectionRemove(UnitEntityData selected)
	{
	}

	private void TriggerBark(GuardianBarkRules.BarkCategory category, LocalizedString[] lines)
	{
		try
		{
			if (lines == null || lines.Length == 0)
			{
				return;
			}
			UnitEntityData owner = base.Owner;
			Game instance = Game.Instance;
			if (owner == null || owner.Descriptor == null || instance?.Player == null)
			{
				return;
			}
			GameModeType currentMode = instance.CurrentMode;
			bool dialogOrCutscene = currentMode == GameModeType.Dialog || currentMode == GameModeType.Cutscene || currentMode == GameModeType.CutsceneGlobalMap;
			bool restingInHiddenScene = currentMode == GameModeType.Rest && instance.RestController != null && instance.RestController.HideScene;
			if (!GuardianBarkRules.MayBark(owner.Descriptor.State.IsConscious, owner.Master != null, instance.Player.PartyAndPets.Contains(owner), owner.IsInGame, restingInHiddenScene, dialogOrCutscene) || !GuardianBarkRules.PassesChance(category, UnityEngine.Random.value) || !GuardianBarkRules.TryConsumeCooldown(owner.UniqueId, category, Time.unscaledTime))
			{
				return;
			}
			LocalizedString localizedString = lines[UnityEngine.Random.Range(0, lines.Length)];
			if (localizedString == null)
			{
				return;
			}
			UnitEntityData master = owner.Master;
			string text = GuardianBarkRules.ApplyTokens(localizedString, master?.CharacterName, master != null && master.Gender == Gender.Female);
			if (!string.IsNullOrEmpty(text))
			{
				float num = UIUtility.GetBarkDuration(text);
				if (num < 3f)
				{
					num = 3f;
				}
				instance.UI?.Bark(owner, text, num);
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Guardian bark failed: " + ex);
		}
	}
}
