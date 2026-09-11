using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace IsekaiMod.Components
{
	[TypeId("d80265e4a2ec40988c60eb2241910c97")]
	public class PrerequisiteIsMainCharacter : Prerequisite
	{
		public bool CompanionOnly;

		public override bool CheckInternal(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state)
		{
			if (unit?.Unit == null)
			{
				return !CompanionOnly;
			}
			if (!CompanionOnly)
			{
				return unit.Unit.IsMainCharacter;
			}
			return !unit.Unit.IsMainCharacter;
		}

		public override string GetUITextInternal(UnitDescriptor unit)
		{
			if (!CompanionOnly)
			{
				return "Is Main Character";
			}
			return "Is Companion or Mercenary";
		}
	}
}
