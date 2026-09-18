using System.Linq;
using IsekaiMod.Config;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.UMMTools;
using UnityEngine;

namespace IsekaiMod
{
	internal static class SettingsTabs
	{
		private static readonly AddedContent addedContent = Main.IsekaiContext.AddedContent;

		public static void AddedContent()
		{
			SetttingUI.TabLevel level = SetttingUI.TabLevel.Zero;
			UI.Div();
			UI.Toggle("New Settings Off By Default".bold(), ref addedContent.NewSettingsOffByDefault);
			using (UI.VerticalScope())
			{
				SetttingUI.SettingGroup("Isekai", level, addedContent.Isekai);
				SetttingUI.SettingGroup("Other", level, addedContent.Other);
			}
		}

		public static void Appearance()
		{
			UI.Div();
			UI.Label("Set the Isekai Protagonist's default clothes. " + "Remember to restart!".orange());
			using (UI.HorizontalScope())
			{
				int selected = addedContent.IsekaiDefaultClothes;
				string[] texts = (from c in StaticReferences.BaseClasses.Where((BlueprintCharacterClass c) => c != null).ToArray()
					select c.LocalizedName.ToString()).ToArray().Select((string a, int i) => (i != selected) ? a : a.orange().bold()).ToArray();
				addedContent.IsekaiDefaultClothes = GUILayout.SelectionGrid(selected, texts, 5, UI.Width(600f));
			}
		}

		public static void Settings()
		{
			UI.Div();
			UI.Toggle("Exclude Companions and Mercenaries from having the Isekai Protagonist Class.", ref addedContent.ExcludeCompanionsFromIsekaiClass);
			UI.Toggle("Allow Main Character to take Otherworld Retinue Prestige Class.", ref addedContent.AllowMainCharacterRetinue);
			UI.Toggle("Restrict Exceptional Feats to the Isekai Protagonist Class.", ref addedContent.RestrictExceptionalFeats);
			UI.Toggle("Restrict Mythic Overpowered Abilities to the Isekai Protagonist Class.", ref addedContent.RestrictMythicOPAbility);
			UI.Toggle("Restrict Mythic Special Powers to the Isekai Protagonist Class.", ref addedContent.RestrictMythicSpecialPower);
			UI.Toggle("Mythic Overpowered Abilities no longer restricted to one.", ref addedContent.MultipleMythicOPAbility);
			UI.Toggle("Mythic Special Powers no longer restricted to one.", ref addedContent.MultipleMythicSpecialPower);
			UI.Slider("Isekai Protagonist Spells Known Increment", ref addedContent.IsekaiSpellsKnownIncrement, 1, 6, 6, "spells per level", GUILayout.ExpandWidth(expand: false));
			// Without the merge the Isekai spell list only holds the spells written into the mod, so spells other
			// mods add to the class lists (Bladed Dash, Shadow Claws and the like) never appear on it.
			UI.Toggle("Merge every class spell list into the Isekai Protagonist spell list (includes spells added by other mods).", ref addedContent.MergeIsekaiSpellList);
			UI.Toggle("Enable Native Level Uncapping (Levels 21 to 40).", ref addedContent.EnableLevelUncapping);
			UI.Toggle("Enable Dynamic Cosmic Threat Scaling for Enemies.", ref addedContent.EnableCosmicThreatScaling);
			UI.Toggle("Enable Rival Reincarnator Encounters.", ref addedContent.EnableRivalReincarnators);
			UI.Toggle("Enable Dimensional Reinforcements on Bosses below 50% HP.", ref addedContent.EnableDimensionalReinforcements);
			UI.Toggle("Enable Isekai Hell / Extra Monster Wave Scaling (Unfair & Custom).", ref addedContent.EnableIsekaiEncounterMultiplier);
			UI.Toggle("Enable Cosmic Planar Incursions (Summons Enforcers on High Cheats).", ref addedContent.EnablePlanarIncursions);
			UI.Toggle("Enable Boss Cosmic Phase Gate (Prevents One-Shots & Triggers Phase 2).", ref addedContent.EnableBossPhaseGate);
			UI.Slider("Cosmic Threat Scaling Multiplier", ref addedContent.CosmicThreatDifficultyMultiplier, 1, 4, 1, "x multiplier", GUILayout.ExpandWidth(expand: false));
			UI.Div(0f, 20f);
			UI.Label("Constellation Live-Chat & Broadcast Overlay".bold());
			UI.Toggle("Enable Floating Constellation Live-Chat Overlay during conversations.", ref addedContent.EnableConstellationChatOverlay);
			UI.Toggle("Keep Constellation Live-Chat Overlay Always Visible (Stream Chat Mode).", ref addedContent.ConstellationChatAlwaysVisible);
			using (UI.HorizontalScope())
			{
				UI.Label("Screen Position: ", UI.Width(140f));
				string[] texts = new string[3] { "Top Right (Default)", "Top Left", "Bottom Right" };
				addedContent.ConstellationChatPosition = GUILayout.SelectionGrid(addedContent.ConstellationChatPosition, texts, 3, UI.Width(450f));
			}
			UI.Slider("Chat Message Display Duration (seconds)", ref addedContent.ConstellationChatDurationSeconds, 5, 30, 14, "s", GUILayout.ExpandWidth(expand: false));
			UI.Slider("Max Visible Overlay Messages", ref addedContent.ConstellationChatMaxMessages, 1, 10, 5, "msgs", GUILayout.ExpandWidth(expand: false));
			using (UI.HorizontalScope())
			{
				if (GUILayout.Button("Open Constellation Broadcast Archives Window", UI.Width(350f)))
				{
					ConstellationChatOverlay.ToggleHistoryWindow();
				}
				if (GUILayout.Button("Reset / Show Live Chat Overlay", UI.Width(250f)))
				{
					ConstellationChatOverlay.ResetAndShowOverlay();
				}
			}
			UI.Div(0f, 25f);
			UI.HStack("Disable Spellbook", 1, delegate
			{
				UI.Toggle("Disable Martial God Spellbook.", ref addedContent.DisableSpellbookMartialGod);
			}, delegate
			{
				UI.Toggle("Disable God Emperor Spellbook.", ref addedContent.DisableSpellbookGodEmperor);
			}, delegate
			{
				UI.Toggle("Disable Hero Spellbook.", ref addedContent.DisableSpellbookHero);
			}, delegate
			{
				UI.Toggle("Disable Mastermind Spellbook.", ref addedContent.DisableSpellbookMastermind);
			}, delegate
			{
				UI.Toggle("Disable Overlord Spellbook.", ref addedContent.DisableSpellbookOverlord);
			}, delegate
			{
				UI.Toggle("Disable Shadow Monarch Spellbook.", ref addedContent.DisableSpellbookShadowMonarch);
			});
		}
	}
}
