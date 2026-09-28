using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Content.Constellations
{
	/// <summary>
	/// Cosmic Wish charges are bought in the store and must not come back on rest. The game refills every
	/// ability resource to its maximum when the party rests (UnitAbilityResourceCollection.Restore with
	/// restoreFull), and BlueprintAbilityResource has no "does not restore" switch, so this prefix skips the
	/// full restore for the Cosmic Wish resource only. Buying a charge restores by amount, which still passes.
	/// </summary>
	[HarmonyPatch(typeof(UnitAbilityResourceCollection), "Restore", typeof(BlueprintScriptableObject), typeof(int), typeof(bool))]
	internal static class CosmicWishRestGuard
	{
		internal static BlueprintAbilityResource Resource;

		private static bool Prefix(BlueprintScriptableObject blueprint, bool restoreFull)
		{
			return !(restoreFull && Resource != null && blueprint == Resource);
		}
	}
}
