using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.JsonSystem;

namespace IsekaiMod.Content.Arenas;

[TypeId("d8e0a2c4f6b84d1a93c5e7f9a1b3d5c6")]
public class ContextActionLanternKingAnnounceDiscount : IsekaiContextAction
{
	public override string GetCaption()
	{
		return "The Lantern King announces his Avatar discount (once per save)";
	}

	protected override void RunSafely()
	{
		Dictionary<string, string> dictionary = ColiseumProgress.Ensure();
		if (dictionary != null && !SaveScopedStorage.GetFlag(dictionary, "LanternKing.DiscountAnnounced"))
		{
			SaveScopedStorage.SetFlag(dictionary, "LanternKing.DiscountAnnounced", value: true);
			LanternKingReveal.PostVoices(LanternKingReveal.DiscountReactions());
		}
	}
}
