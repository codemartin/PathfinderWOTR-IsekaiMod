using IsekaiMod.Config;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace IsekaiMod.Components;

[TypeId("a39e1ff511a340a0935a695d92712ffc")]
public class PrerequisiteContentOffered : Prerequisite
{
	public OfferedContent Content;

	public override bool CheckInternal(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state)
	{
		int existingClassLevels = 0;
		if (unit?.Progression != null && base.OwnerBlueprint is BlueprintCharacterClass characterClass)
		{
			existingClassLevels = unit.Progression.GetClassLevel(characterClass);
		}
		return ContentOffering.MayTakeClassLevel(ContentOffering.IsOffered(Content), existingClassLevels);
	}

	public override string GetUITextInternal(UnitDescriptor unit)
	{
		return "Switched off in the Isekai Reimagined settings (" + ContentOffering.SettingKey(Content) + ")";
	}
}
