using Kingmaker.Localization;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Constellations;

internal static class CosmicStoreText
{
	internal static LocalizedString InCombat;

	internal static LocalizedString ConstellationsDisabled;

	internal static LocalizedString PatronForbidsSpellbook;

	internal static LocalizedString PatronRefusesCharacter;

	internal static LocalizedString PatronChanged;

	internal static LocalizedString PatronSwitchCooldown;

	internal static LocalizedString AlreadyEmbodied;

	internal static LocalizedString Ascended;

	internal static LocalizedString OtherAvatarHeld;

	internal static LocalizedString AvatarLost;

	internal static LocalizedString BlessingActivated;

	internal static LocalizedString AlreadyOwned;

	internal static LocalizedString WrongAlignment;

	internal static LocalizedString BoonAcquired;

	internal static LocalizedString WishNoPool;

	internal static LocalizedString WishFull;

	internal static LocalizedString WishBought;

	internal static LocalizedString WishWealth;

	internal static LocalizedString Rejuvenated;

	internal static LocalizedString ItemBought;

	internal static void Create()
	{
		InCombat = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.InCombat", "<color=#7A1010><b>[Cosmic Store]</b></color> You cannot commune with the constellations while in combat!");
		ConstellationsDisabled = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.ConstellationsDisabled", "<color=#7A1010><b>[Cosmic Store]</b></color> The constellations are not watching: Cosmic Sponsorships are switched off in this mod's settings.");
		PatronForbidsSpellbook = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.PatronForbidsSpellbook", "<color=#7A1010>[Cosmic Store]</color> Following <b>{0}</b> would forbid your divine spellbook. Your patron is unchanged.");
		PatronRefusesCharacter = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.PatronRefusesCharacter", "<color=#7A1010>[Cosmic Store]</color> <b>{0}</b> does not accept a follower of your class. Your patron is unchanged.");
		PatronChanged = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.PatronChanged", "<color=#5B2A86><b>[Deity Fealty Changed]</b></color> You have aligned your soul with <b>{0}</b>!");
		PatronSwitchCooldown = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.PatronSwitchCooldown", "<color=#7A1010>[Cosmic Store]</color> The constellations will not re-seat your soul so soon. Ask again in <b>{0}</b> days. Nothing was spent.");
		AlreadyEmbodied = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.AlreadyEmbodied", "<color=#7A1010>[Cosmic Ascension]</color> You already embody <b>{0}</b>. Nothing was spent.");
		Ascended = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.Ascended", "<color=#5B2A86><b>[AVATAR ASCENSION!]</b></color> The skies split with divine brilliance: you now embody <b>{0}</b>!");
		OtherAvatarHeld = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.OtherAvatarHeld", "<color=#7A1010>[Cosmic Ascension]</color> You already embody <b>{0}</b>, and a soul holds one avatar at a time. Switching patron gives it up, with no refund. Nothing was spent.");
		AvatarLost = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.AvatarLost", "<color=#7A1010>[Cosmic Ascension]</color> <b>{0}</b> leaves you with your old fealty. Avatars are not refunded.");
		BlessingActivated = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.BlessingActivated", "<color=#5B2A86>[Cosmic Store]</color> Activated blessing: {0} ({1}h)!");
		AlreadyOwned = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.AlreadyOwned", "<color=#7A1010>[Cosmic Store]</color> You already possess this permanent cosmic boon!");
		WrongAlignment = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.WrongAlignment", "<color=#7A1010>[Cosmic Store]</color> This mythic path is open only to {0} characters. Nothing was spent. (The Key of Infinite Pathways lifts this requirement.)");
		BoonAcquired = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.BoonAcquired", "<color=#5B2A86>[Cosmic Store]</color> Acquired permanent boon: {0}!");
		WishNoPool = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.WishNoPool", "<color=#7A1010>[Cosmic Store]</color> Only a character sponsored through the Cosmic Sponsorship Exchange can hold a Cosmic Wish charge. Nothing was spent.");
		WishFull = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.WishFull", "<color=#7A1010>[Cosmic Store]</color> You already hold the most Cosmic Wish charges the constellations allow ({0}). Nothing was spent.");
		WishBought = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.WishBought", "<color=#5B2A86>[Cosmic Store]</color> Purchased 1 Cosmic Wish charge! You may now cast the Cosmic Wish modular ability in the field.");
		WishWealth = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.WishWealth", "<color=#5B2A86>[Cosmic Wish]</color> Granted 100,000 Gold from the cosmos!");
		Rejuvenated = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.Rejuvenated", "<color=#5B2A86>[Cosmic Elixir]</color> Rejuvenated body, mind, and soul! Hit points, spell slots, and class abilities fully restored!");
		ItemBought = Helpers.CreateString(Main.IsekaiContext, "CosmicStore.Log.ItemBought", "<color=#5B2A86>[Cosmic Store]</color> Manifested {0} into your inventory!");
	}
}
