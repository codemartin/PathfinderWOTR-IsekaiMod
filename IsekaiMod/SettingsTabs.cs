using System.Linq;
using IsekaiMod.Config;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Features.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using TabletopTweaks.Core.UMMTools;
using UnityEngine;

namespace IsekaiMod;

internal static class SettingsTabs
{
	private static readonly AddedContent addedContent = Main.IsekaiContext.AddedContent;

	internal const string DisableRenamedSpellbookLabel = "Disable Martial God Spellbook.";

	internal const string SpellsKnownIncrementLabel = "Isekai Protagonist Spells Known Multiplier: multiplies the number of spells known at every level. 1 = like a sorcerer (37 spells known at level 20), 2 = twice that (the default, matching the doubled spells per day), 6 = knows most of the spell list by level 20. Applies to future level-ups only: levels already taken are not recalculated, and spells already known are kept. Restart.";

	internal const string DisableSpellbookLabel = "Disabling a spellbook applies only to characters who take that archetype while the setting is on: they never get the book, even if the setting is turned off later. Characters who already have the archetype keep their spellbook either way. Restart.";

	public static void AddedContent()
	{
		SetttingUI.TabLevel level = SetttingUI.TabLevel.Zero;
		UI.Div();
		UI.Toggle("New Settings Off By Default".bold() + ": when an update adds a setting to the groups below, it starts switched off instead of on. Settings you have already set are unchanged. Restart.", ref addedContent.NewSettingsOffByDefault);
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
		UI.Label("Any setting can be changed on an existing save: nothing a character already has is removed. Restart the game after changing a setting unless its text says otherwise.".yellow());
		UI.Toggle("Exclude Companions and Mercenaries from having the Isekai Protagonist Class. Companions who already have Isekai levels keep them. Applies at once.", ref addedContent.ExcludeCompanionsFromIsekaiClass);
		UI.Toggle("Allow Main Character to take Otherworld Retinue Prestige Class. Off: levels already taken are kept. Applies at once.", ref addedContent.AllowMainCharacterRetinue);
		UI.Toggle("Restrict Exceptional Feats to the Isekai Protagonist Class. Applies to new picks; feats already taken are kept. Has no effect when the Isekai Protagonist class is not offered, and a Trickster main character ignores it, since Trickster ignores feat prerequisites. Restart.", ref addedContent.RestrictExceptionalFeats);
		UI.Toggle("Restrict Mythic Overpowered Abilities to the Isekai Protagonist Class. Applies to new picks; picks already made are kept. Restart.", ref addedContent.RestrictMythicOPAbility);
		UI.Toggle("Restrict Mythic Special Powers to the Isekai Protagonist Class. Applies to new picks; picks already made are kept. Restart.", ref addedContent.RestrictMythicSpecialPower);
		UI.Toggle("Mythic Overpowered Abilities no longer restricted to one. Off: extra picks already made are kept. Restart.", ref addedContent.MultipleMythicOPAbility);
		UI.Toggle("Mythic Special Powers no longer restricted to one. Off: extra picks already made are kept. Restart.", ref addedContent.MultipleMythicSpecialPower);
		UI.Toggle("Merge the Isekai Protagonist spell list with the other mods' spell lists. Off: spells already known are kept; only new picks use the smaller list. Restart.", ref addedContent.MergeIsekaiSpellList);
		UI.Slider("Isekai Protagonist Spells Known Multiplier: multiplies the number of spells known at every level. 1 = like a sorcerer (37 spells known at level 20), 2 = twice that (the default, matching the doubled spells per day), 6 = knows most of the spell list by level 20. Applies to future level-ups only: levels already taken are not recalculated, and spells already known are kept. Restart.", ref addedContent.IsekaiSpellsKnownIncrement, 1, 6, 2, "x spells known", GUILayout.ExpandWidth(expand: false));
		bool enableLevelUncapping = addedContent.EnableLevelUncapping;
		UI.Toggle("Enable Native Level Uncapping (Levels 21 to 40) for characters whose first class is Isekai Protagonist. Every other character, companions included, stays capped at 20. Off: no new levels past 20; levels and features already gained are kept. Applies at once.", ref addedContent.EnableLevelUncapping);
		if (!enableLevelUncapping && addedContent.EnableLevelUncapping)
		{
			Epic21Abilities.OfferPendingEpicContent();
		}
		UI.Toggle("Enable Dynamic Cosmic Threat Scaling for Enemies: from level 10, enemies gain +2 to attack, AC and saves, and 2 more every 5 levels (+4 at 15, +6 at 20, up to +14 at 40), times the multiplier. Enemies also gain +1 to attack, AC and saves per mythic rank of the main character. Only when an Isekai Protagonist is in the party; never in army battles or the coliseum. Off: an enemy loses the bonus the next time it joins a fight.", ref addedContent.EnableCosmicThreatScaling);
		UI.Slider("Cosmic Threat Difficulty Multiplier (multiplies the enemy attack, AC and save bonus; lowering it takes effect the next time an enemy joins a fight)", ref addedContent.CosmicThreatDifficultyMultiplier, 1, 5, 1, "x", GUILayout.ExpandWidth(expand: false));
		UI.Toggle("Enable Rampant Cheating: with more than one Isekai Protagonist in the party, each enemy gains +4 attack, AC and saves, +100 temporary hit points and an extra attack for that fight. Cosmic Retribution: for every 3 cheats in the party (Cosmic bottles drunk, Forbidden Cheats, and 10 per extra protagonist), each enemy gains +2 attack, AC and saves and +100 temporary hit points (times the multiplier, at most 50 stacks), and from 5 stacks True Seeing, Freedom of Movement and an extra attack. Only when an Isekai Protagonist is in the party. Off: no new curses; a curse already on an enemy ends with its fight.", ref addedContent.EnableRampantCheating);
		UI.Toggle("Enable Rival Reincarnator Encounters: one in ten enemies of CR at least your level becomes a Rival worth 100 Cosmic Coins, once. Only when an Isekai Protagonist is in the party; never story bosses, summons or army battles. Off: no new Rivals; existing Rivals still pay. Applies to fights that start after the change.", ref addedContent.EnableRivalReincarnators);
		UI.Toggle("Enable Dimensional Reinforcements: a story boss below 50% HP calls a small wave of demons suited to the chapter, once. Only when an Isekai Protagonist is in the party; never plot-critical or yielding bosses. Off: no new waves; a wave already called leaves when its fight ends. Applies at once.", ref addedContent.EnableDimensionalReinforcements);
		UI.Toggle(DimensionalReinforcements.EncounterMultiplierLabel(), ref addedContent.EnableIsekaiEncounterMultiplier);
		UI.Toggle("Enable Constellations and Cosmic Sponsorships. Turning this off takes effect at once: every Cosmic Coin source stops, the Cosmic Sponsorship Store closes and the broadcasts go quiet; coins, relics and features already earned are kept. If the game was started with it off, turning it on restores the coins and the store at once; the broadcasts, the paid dialogue answers and the sponsor badges need a restart.", ref addedContent.EnableConstellations);
		UI.Toggle(ConstellationCombatReactions.SettingLabel(), ref addedContent.EnableConstellationCombatReactions);
		bool enableConstellationChatOverlay = addedContent.EnableConstellationChatOverlay;
		UI.Toggle("Enable the Constellation Chat Overlay: a live broadcast panel that stays on screen, with the History button and the archive window on it. Off: broadcasts are still archived but nothing is drawn.", ref addedContent.EnableConstellationChatOverlay);
		if (!enableConstellationChatOverlay && addedContent.EnableConstellationChatOverlay)
		{
			ConstellationChatOverlay.Wake();
		}
		UI.Slider("Constellation Chat Message Duration", ref addedContent.ConstellationChatDurationSeconds, 3, 60, 14, "seconds", GUILayout.ExpandWidth(expand: false));
		UI.Slider("Constellation Chat Position (0 = Top Right, 1 = Top Left, 2 = Bottom Right)", ref addedContent.ConstellationChatPosition, 0, 2, 0, "", GUILayout.ExpandWidth(expand: false));
		UI.Slider("Constellation Chat Messages On Screen (how tall the live panel is; it keeps a longer backlog you can scroll back through)", ref addedContent.ConstellationChatMaxMessages, 1, 12, 5, "messages", GUILayout.ExpandWidth(expand: false));
		UI.Slider("Constellation Chat Arrival Pace (the gap between the lines of one broadcast, so they arrive like a live chat; 0 posts the whole broadcast at once)", ref addedContent.ConstellationChatArrivalTenths, 0, 20, 6, "tenths of a second", GUILayout.ExpandWidth(expand: false));
		UI.Toggle("Enable Dimensional Arena (Planar Coliseum). Off: the Otherworldly Emissary leaves, the coliseum entrances, map tiles and trials are hidden, and the loop no longer asks for the arenas; rewards and progress already earned are kept. Applies at once, except the barren anomalies an Isekai Protagonist receives at level 1, which follow the setting the game was started with.", ref addedContent.EnableDimensionalArena);
		UI.Toggle("I own Pathfinder: Kingmaker. Use this when your Kingmaker saves are not found: it counts Kingmaker as detected, so the Sylvan Fey Alliance quest can start in Act 3 and the Treaty of the Stolen Lands is offered. Off: detection uses the Kingmaker save folder only; a Sylvan quest already started can still be finished, and rewards already earned are kept. Applies at once.", ref addedContent.IOwnKingmaker);
		UI.Div(0f, 25f);
		UI.Label("Disabling a spellbook applies only to characters who take that archetype while the setting is on: they never get the book, even if the setting is turned off later. Characters who already have the archetype keep their spellbook either way. Restart.".orange());
		UI.HStack("Disable Spellbook", 1, delegate
		{
			UI.Toggle("Disable Martial God Spellbook.", ref addedContent.DisableSpellbookPhantomBlade);
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
