using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("e7077f1104db4ace95106ee319c873f2")]
public class ActionStartColiseumCup : IsekaiGameAction
{
	public string CupId;

	public override string GetCaption()
	{
		return "Start Planar Coliseum cup " + CupId;
	}

	protected override void RunSafely()
	{
		if (!PlanarColiseumHubManager.IsInColiseum())
		{
			PlanarColiseumHubManager.PostLog("<color=#7A1010><b>[Planar Coliseum]</b></color> Tournament cups are fought only on the arena sands.");
			return;
		}
		string cupId = CupId;
		GuardedSchedule.Next("Starting Planar Coliseum cup " + cupId, delegate
		{
			DimensionalArenaManager.StartArena(cupId);
		});
	}
}
