using System;
using IsekaiMod.Content.Constellations;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace IsekaiMod.Components
{
	[TypeId("d91a134812304910bba8081234918234")]
	public class PrerequisiteCycleOriginLock : Prerequisite
	{
		public string AllowedOrigin = "";

		public PrerequisiteCycleOriginLock()
		{
			HideInUI = true;
		}

		public override bool CheckInternal(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state)
		{
			if (state != null && (state.Mode == LevelUpState.CharBuildMode.CharGen || state.Mode == LevelUpState.CharBuildMode.Respec || state.IsFirstCharacterLevel))
			{
				return true;
			}
			if (unit?.Unit == null || !unit.Unit.IsMainCharacter)
			{
				return true;
			}
			string text = TimelineManager.Data?.ActiveRunArchetype;
			if (string.IsNullOrEmpty(text))
			{
				return true;
			}
			return text.Equals(AllowedOrigin, StringComparison.OrdinalIgnoreCase);
		}

		public override string GetUITextInternal(UnitDescriptor unit)
		{
			string text = TimelineManager.Data?.ActiveRunArchetype;
			if (string.IsNullOrEmpty(text) || text.Equals(AllowedOrigin, StringComparison.OrdinalIgnoreCase))
			{
				return "Soul Origin: " + AllowedOrigin;
			}
			return "Soul Origin is locked to " + text + " for this incarnation (resets upon cycle completion).";
		}
	}
}
