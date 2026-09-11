using TabletopTweaks.Core.UMMTools;
using UnityModManagerNet;

namespace IsekaiMod
{
	public static class UMMSettingsUI
	{
		private static int selectedTab;

		public static void OnGUI(UnityModManager.ModEntry modEntry)
		{
			UI.AutoWidth();
			UI.TabBar(ref selectedTab, delegate
			{
				UI.Label("Select your preferred settings and restart your game.".yellow().bold());
			}, new NamedAction("Added Content", delegate
			{
				SettingsTabs.AddedContent();
			}), new NamedAction("Appearance", delegate
			{
				SettingsTabs.Appearance();
			}), new NamedAction("Settings", delegate
			{
				SettingsTabs.Settings();
			}));
		}
	}
}
