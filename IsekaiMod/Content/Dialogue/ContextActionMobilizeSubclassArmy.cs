using Kingmaker.Blueprints;
using Kingmaker.Kingdom.Armies;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue
{
	public class ContextActionMobilizeSubclassArmy : ContextAction
	{
		public string UnitGuid;

		public int Count;

		public string ArmyName;

		public override string GetCaption()
		{
			return $"Mobilizes {Count} {ArmyName} into Crusade Recruit Pool";
		}

		public override void RunAction()
		{
			if (string.IsNullOrEmpty(UnitGuid) || Count <= 0)
			{
				return;
			}
			BlueprintUnit blueprint = BlueprintTools.GetBlueprint<BlueprintUnit>(UnitGuid);
			if (blueprint != null && ArmyRecruitsManager.Instance != null)
			{
				ArmyRecruitsManager.Instance.IncreasePool(blueprint, Count);
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage($"<color=#32CD32><b>[Crusade Mobilization]</b></color> <b>{Count} {ArmyName}</b> have joined your army reserve pool!");
				});
			}
		}
	}
}
