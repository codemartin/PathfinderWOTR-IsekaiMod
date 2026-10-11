using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace IsekaiMod.Components;

[TypeId("6b1f0c8e2d7a4e59a3c4f1d92e0b7a35")]
public class PrerequisiteFeatureRankBelow : Prerequisite
{
	public int MaxRank = 1;

	internal static bool Allows(int currentRank, int maxRank)
	{
		return currentRank < maxRank;
	}

	public override bool CheckInternal(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state)
	{
		if (unit?.Progression == null)
		{
			return true;
		}
		if (!(base.OwnerBlueprint is BlueprintFeature feature))
		{
			return true;
		}
		return Allows(unit.Progression.Features.GetRank(feature), MaxRank);
	}

	public override string GetUITextInternal(UnitDescriptor unit)
	{
		return $"Taken fewer than {MaxRank} time(s)";
	}
}
