using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Constellations;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace IsekaiMod.Components;

[TypeId("d91a134812304910bba8081234918234")]
public class PrerequisiteCycleOriginLock : Prerequisite
{
	public string AllowedOrigin = "";

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
		string text = ArchetypeOrigin.Normalize(TimelineManager.ActiveRunArchetypeForCurrentSave());
		if (text.Length == 0)
		{
			return true;
		}
		return ArchetypeOrigin.Matches(text, AllowedOrigin);
	}

	public override string GetUITextInternal(UnitDescriptor unit)
	{
		string text = ArchetypeOrigin.Normalize(TimelineManager.ActiveRunArchetypeForCurrentSave());
		if (text.Length == 0 || ArchetypeOrigin.Matches(text, AllowedOrigin))
		{
			return "Transmigrated Soul Origin: " + ArchetypeOrigin.DisplayName(AllowedOrigin);
		}
		return "Transmigrated Soul Origin is locked to " + ArchetypeOrigin.DisplayName(text) + " for this incarnation (resets upon cycle completion).";
	}
}
