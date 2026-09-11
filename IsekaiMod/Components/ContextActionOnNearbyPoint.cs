using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.View;
using Pathfinding;
using UnityEngine;

namespace IsekaiMod.Components
{
	[TypeId("bc1d7430da024608b6e623c70b380dff")]
	public class ContextActionOnNearbyPoint : ContextAction
	{
		public ActionList Actions;

		public override string GetCaption()
		{
			return "Run a context action nearby target point";
		}

		public override string GetDescription()
		{
			return "Point nearby will be action target";
		}

		public override void RunAction()
		{
			if (Actions?.Actions == null || Actions.Actions.Length == 0)
			{
				return;
			}
			int num = Actions.Actions.Length;
			Vector3 vector = base.Context.MainTarget.Point;
			NNInfo nearestNode = ObstacleAnalyzer.GetNearestNode(vector);
			if (nearestNode.node != null)
			{
				vector = nearestNode.position;
			}
			FreePlaceSelector.PlaceSpawnPlaces(num, 1f, vector);
			for (int i = 0; i < num; i++)
			{
				vector = FreePlaceSelector.GetRelaxedPosition(i, projectOnGround: true);
				using (base.Context.GetDataScope(vector))
				{
					Actions.Actions[i]?.RunAction();
				}
			}
		}
	}
}
