using System;
using System.Collections.Generic;
using IsekaiMod.Content.GlobalMap;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Quests;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Quests;

public static class SubclassPersonalQuests
{
	public static BlueprintQuest QuestUniversalNostalgia;

	public static BlueprintQuestObjective ObjectiveNostalgiaAct1;

	public static BlueprintQuestObjective ObjectiveNostalgiaAct2;

	public static BlueprintQuestObjective ObjectiveNostalgiaAct3;

	public static BlueprintQuestObjective ObjectiveNostalgiaAct4;

	public static BlueprintQuestObjective ObjectiveNostalgiaAct5;

	public static BlueprintQuest QuestPhantomBlade;

	public static BlueprintQuestObjective ObjectivePhantomBladeAct1;

	public static BlueprintQuestObjective ObjectivePhantomBladeAct2;

	public static BlueprintQuestObjective ObjectivePhantomBladeAct3;

	public static BlueprintQuest QuestGodEmperor;

	public static BlueprintQuestObjective ObjectiveGodEmperorAct1;

	public static BlueprintQuestObjective ObjectiveGodEmperorAct2;

	public static BlueprintQuestObjective ObjectiveGodEmperorAct3;

	public static BlueprintQuest QuestOverlord;

	public static BlueprintQuestObjective ObjectiveOverlordAct1;

	public static BlueprintQuestObjective ObjectiveOverlordAct2;

	public static BlueprintQuestObjective ObjectiveOverlordAct3;

	public static BlueprintQuest QuestDevourer;

	public static BlueprintQuestObjective ObjectiveDevourerAct1;

	public static BlueprintQuestObjective ObjectiveDevourerAct2;

	public static BlueprintQuestObjective ObjectiveDevourerAct3;

	public static BlueprintQuestObjective ObjectiveDevourerAct3Void;

	public static BlueprintQuest QuestShadowMonarch;

	public static BlueprintQuestObjective ObjectiveShadowMonarchAct1;

	public static BlueprintQuestObjective ObjectiveShadowMonarchAct2;

	public static BlueprintQuestObjective ObjectiveShadowMonarchAct3;

	public static BlueprintQuest QuestHero;

	public static BlueprintQuestObjective ObjectiveHeroAct1;

	public static BlueprintQuestObjective ObjectiveHeroAct2;

	public static BlueprintQuestObjective ObjectiveHeroAct3;

	public static BlueprintQuest QuestMastermind;

	public static BlueprintQuestObjective ObjectiveMastermindAct1;

	public static BlueprintQuestObjective ObjectiveMastermindAct2;

	public static BlueprintQuestObjective ObjectiveMastermindAct3;

	public static BlueprintItemEquipmentNeck ItemTransmigrantLocket;

	public static BlueprintItemEquipmentRing ItemTearsOfSpeedMirage;

	public static BlueprintItemEquipmentRing ItemImperialSovereignSeal;

	public static BlueprintItemEquipmentRing ItemScepterOfGreatTomb;

	public static BlueprintItemEquipmentBelt ItemHeartOfBeelzebub;

	public static BlueprintItemEquipmentRing ItemRingOfMonarchShadow;

	public static BlueprintItemEquipmentShoulders ItemMantleOfTrueSavior;

	public static BlueprintItemEquipmentRing ItemTomeOfInfiniteContingencies;

	private static readonly Dictionary<string, BlueprintQuest> s_Quests = new Dictionary<string, BlueprintQuest>(StringComparer.Ordinal);

	private static readonly Dictionary<string, BlueprintQuestObjective> s_Objectives = new Dictionary<string, BlueprintQuestObjective>(StringComparer.Ordinal);

	private static readonly Dictionary<string, BlueprintItem> s_Relics = new Dictionary<string, BlueprintItem>(StringComparer.Ordinal);

	internal static BlueprintQuest QuestByName(string name)
	{
		if (name == null || !s_Quests.TryGetValue(name, out var value))
		{
			return null;
		}
		return value;
	}

	internal static BlueprintQuestObjective ObjectiveByName(string name)
	{
		if (name == null || !s_Objectives.TryGetValue(name, out var value))
		{
			return null;
		}
		return value;
	}

	internal static BlueprintItem RelicByName(string name)
	{
		if (name == null || !s_Relics.TryGetValue(name, out var value))
		{
			return null;
		}
		return value;
	}

	public static void Add()
	{
		CreateQuestsAndObjectives();
		CreateDualTierItems();
		LinkQuestsToGlobalMap();
		PersonalQuestDirector.Init();
	}

	private static void CreateQuestsAndObjectives()
	{
		QuestUniversalNostalgia = CreateQuest("QuestUniversalNostalgia", "The Otherworld Nostalgia", "Fragments of memories and discarded relics from your previous existence on Earth lie scattered across Golarion and the Abyss. Recovering these artifacts will unlock your awakened potential.", "The last fragment of your old life is recovered, and your memories of Earth are whole again.");
		ObjectiveNostalgiaAct1 = CreateAct(PersonalQuestCatalog.Nostalgia, "ObjectiveNostalgiaAct1", "The Keepsake in the Ashes", "Search the ruined Blackwing Library beneath Kenabres for the weathered keepsake box that fell through with you.");
		ObjectiveNostalgiaAct2 = CreateAct(PersonalQuestCatalog.Nostalgia, "ObjectiveNostalgiaAct2", "The Scorched Power Core", "Recover the scorched power core from the Lost Chapel. Nothing in the Worldwound built it, and nothing in the Worldwound can read it.");
		ObjectiveNostalgiaAct3 = CreateAct(PersonalQuestCatalog.Nostalgia, "ObjectiveNostalgiaAct3", "Transmission from Below", "Follow the interdimensional transmission down into the Midnight Fane, beneath Drezen.");
		ObjectiveNostalgiaAct4 = CreateAct(PersonalQuestCatalog.Nostalgia, "ObjectiveNostalgiaAct4", "Echoes in Alushinyrra", "Trace the memory resonance through the demon city of Alushinyrra, in the Midnight Isles.");
		ObjectiveNostalgiaAct5 = CreateFinalAct(PersonalQuestCatalog.Nostalgia, "ObjectiveNostalgiaAct5", "The Transmigrant's Locket", "Forge the Transmigrant's Locket from everything you have gathered. Winning the Multiverse Threshold Cup in the Planar Coliseum's last arena gives the pieces a forge; the Threshold of the Multiverse itself will serve if you never take the sands.");
		QuestPhantomBlade = CreateQuest("QuestPhantomBlade", "The Legend of the Whispering Blade", "Campfire ghost stories tell of the Whispering Phantom: an unvanquished Sarkorian sword-saint whose spirit haunts the misty ruins of the Worldwound, challenging wandering blade-masters under the pale moon. Cross ethereal steel to inherit the phantom sword arts.", "The Whispering Phantom is at rest, and its sword arts live on in your blade.");
		ObjectivePhantomBladeAct1 = CreateAct(PersonalQuestCatalog.PhantomBlade, "ObjectivePhantomBladeAct1", "The Whispering Blade", "Follow the ghost story of the Whispering Blade while you hunt for something that can cleanse the wardstone, in the Kenabres market square where Terendelev fell.");
		ObjectivePhantomBladeAct2 = CreateAct(PersonalQuestCatalog.PhantomBlade, "ObjectivePhantomBladeAct2", "The Phantom Sword-Saint", "The Sarkorian sword-saint answers nothing but a formal challenge. Take his trial in the Planar Coliseum.");
		ObjectivePhantomBladeAct3 = CreateFinalAct(PersonalQuestCatalog.PhantomBlade, "ObjectivePhantomBladeAct3", "The Blade Spirit", "Best the legendary Blade Spirit in the Planar Coliseum and inherit the phantom sword arts.");
		CreateDormant(PersonalQuestCatalog.PhantomBlade, "ObjectivePhantomBladeDormant");
		QuestGodEmperor = CreateQuest("QuestGodEmperor", "The Gospel of the Living God", "As an ascendant sovereign, mortal laws alone are insufficient: you must establish your own divine religion. Rally worshippers, erect shrines to your glory, and bind the crusade under the holy scriptures of the Living Emperor.", "Your gospel is proclaimed across the Crusade, and the Living God sits enthroned in Drezen.");
		ObjectiveGodEmperorAct1 = CreateAct(PersonalQuestCatalog.GodEmperor, "ObjectiveGodEmperorAct1", "Altar of the Living God", "Raise the Altar of the Living God among the crusaders gathering at the Defender's Heart, while they decide who marches with you.");
		ObjectiveGodEmperorAct2 = CreateAct(PersonalQuestCatalog.GodEmperor, "ObjectiveGodEmperorAct2", "The Holy Standard", "Hang your gospel beside the Sword of Valor over Drezen, so that the banner and the scripture are raised together.");
		ObjectiveGodEmperorAct3 = CreateFinalAct(PersonalQuestCatalog.GodEmperor, "ObjectiveGodEmperorAct3", "The Imperial Dais", "Consecrate the Grand Imperial Dais of Drezen and be enthroned as the Fifth Crusade's sole divine sovereign. Issue the decree at the war table, or, if your generals run the crusade for you, let the fall of the Midnight Fane's master speak in its place.");
		CreateDormant(PersonalQuestCatalog.GodEmperor, "ObjectiveGodEmperorDormant");
		QuestOverlord = CreateQuest("QuestOverlord", "The Scepter of Absolute Order", "Excavate the subterranean necropolis of an ancient sorcerer-king to forge the Scepter of the Great Tomb.", "The Great Tomb stands beneath Drezen, and death itself answers to your command.");
		ObjectiveOverlordAct1 = CreateAct(PersonalQuestCatalog.Overlord, "ObjectiveOverlordAct1", "The Sealed Tomb", "Open the sealed tomb beneath Kenabres, in the Shield Maze, by laying Hosilla in it.");
		ObjectiveOverlordAct2 = CreateAct(PersonalQuestCatalog.Overlord, "ObjectiveOverlordAct2", "The Bone Court", "Hold court over the bone champions of the Great Tomb. They answer a challenge in the Planar Coliseum, and nowhere else.");
		ObjectiveOverlordAct3 = CreateFinalAct(PersonalQuestCatalog.Overlord, "ObjectiveOverlordAct3", "The Great Tomb Beneath Drezen", "Take the Midnight Fane beneath Drezen for the Great Tomb, and bind its master's death to your command.");
		CreateDormant(PersonalQuestCatalog.Overlord, "ObjectiveOverlordDormant");
		QuestDevourer = CreateQuest("QuestDevourer", "The Gluttonous Sovereign", "Reborn as an amorphous slime with an insatiable predator stomach, you must navigate your evolution. Will you become a benevolent sovereign uniting monsters in peace, or an abyssal world-eater devouring all?", "Your evolution is complete, and what the Gluttonous Sovereign has become can no longer be undone.");
		ObjectiveDevourerAct1 = CreateAct(PersonalQuestCatalog.Devourer, "ObjectiveDevourerAct1", "The Corrosive Wyrm", "Awaken your biological mimicry by devouring a corrosive wyrm. One answers a challenge in the Planar Coliseum.");
		ObjectiveDevourerAct2 = CreateAct(PersonalQuestCatalog.Devourer, "ObjectiveDevourerAct2", "The Sovereign Divergence", "Put the question to the High Oracle of the Constellations in the Planar Coliseum: the Benevolent Tempest, or the Abyssal Void.");
		ObjectiveDevourerAct3 = CreateFinalAct(PersonalQuestCatalog.Devourer, "ObjectiveDevourerAct3", "The Primordial Behemoth", "Assimilate the Primordial Behemoth in the Planar Coliseum, and let the Tempest's federation rise out of what you become.");
		ObjectiveDevourerAct3Void = CreateFinalAct(PersonalQuestCatalog.Devourer, "ObjectiveDevourerAct3Void", "The Primordial Behemoth", "Devour the Primordial Behemoth in the Planar Coliseum, and let the Void keep whatever is left of what you were.");
		CreateDormant(PersonalQuestCatalog.Devourer, "ObjectiveDevourerDormant");
		QuestShadowMonarch = CreateQuest("QuestShadowMonarch", "The Coronation of the Shadow King", "Answer the summons of the subterranean Monarch's Crypt, face your own shadow trial, and rise as sovereign of all departed souls.", "You sit upon the Throne of Shadows, sovereign of every soul that has passed beyond.");
		ObjectiveShadowMonarchAct1 = CreateAct(PersonalQuestCatalog.ShadowMonarch, "ObjectiveShadowMonarchAct1", "Whispers Beneath the Garrison", "Follow the whispers out of the catacombs beneath the Gray Garrison, and take the garrison above them.");
		ObjectiveShadowMonarchAct2 = CreateAct(PersonalQuestCatalog.ShadowMonarch, "ObjectiveShadowMonarchAct2", "Shadows of the Fallen Vanguard", "Take the shadows of a fallen crusader vanguard into your service. They answer a challenge in the Planar Coliseum.");
		ObjectiveShadowMonarchAct3 = CreateFinalAct(PersonalQuestCatalog.ShadowMonarch, "ObjectiveShadowMonarchAct3", "The Throne of Shadows", "Take the Throne of Shadows from the deep shadow that holds it, on the sands of the Planar Coliseum.");
		CreateDormant(PersonalQuestCatalog.ShadowMonarch, "ObjectiveShadowMonarchDormant");
		QuestHero = CreateQuest("QuestHero", "The Unbroken Promise of Fellowship", "A true hero protects every companion and ally. Uphold your oath across the crusade's darkest hours.", "Every companion stood with you to the end, and the promise of fellowship held.");
		ObjectiveHeroAct1 = CreateAct(PersonalQuestCatalog.Hero, "ObjectiveHeroAct1", "The Defender's Heart Holds", "Stand with the crusaders when the demons come for the Defender's Heart.");
		ObjectiveHeroAct2 = CreateAct(PersonalQuestCatalog.Hero, "ObjectiveHeroAct2", "The Night Raid", "Survive the night attack on the crusader war camp, and carry out everyone you can.");
		ObjectiveHeroAct3 = CreateFinalAct(PersonalQuestCatalog.Hero, "ObjectiveHeroAct3", "The Sounding Horn", "End the Midnight Fane's master beneath Drezen. If the allies' horn sounds before you reach him, answer it: nobody is owed that, which is exactly why it counts.");
		CreateDormant(PersonalQuestCatalog.Hero, "ObjectiveHeroDormant");
		QuestMastermind = CreateQuest("QuestMastermind", "The Architect's Endgame", "Engage in 4D causal chess against the demon generals. Decrypt their plans and orchestrate the flawless checkmate.", "The final checkmate is played, and the Ivory Sanctum's architect never saw it coming.");
		ObjectiveMastermindAct1 = CreateAct(PersonalQuestCatalog.Mastermind, "ObjectiveMastermindAct1", "The Cultist Dispatch", "Decrypt the cultists' dispatch and put it in Irabeth's hands at the Defender's Heart.");
		ObjectiveMastermindAct2 = CreateAct(PersonalQuestCatalog.Mastermind, "ObjectiveMastermindAct2", "Checkmate at Drezen", "Take the Drezen citadel exactly as you drew it up.");
		ObjectiveMastermindAct3 = CreateFinalAct(PersonalQuestCatalog.Mastermind, "ObjectiveMastermindAct3", "The Ivory Gambit", "Close the gambit in the Ivory Sanctum: Xanthir Vang does not walk out of it.");
		CreateDormant(PersonalQuestCatalog.Mastermind, "ObjectiveMastermindDormant");
		PersonalQuestDefinition[] all = PersonalQuestCatalog.All;
		for (int i = 0; i < all.Length; i++)
		{
			FinishQuest(all[i]);
		}
	}

	private static void FinishQuest(PersonalQuestDefinition definition)
	{
		BlueprintQuest blueprintQuest = QuestByName(definition.QuestName);
		if (blueprintQuest == null)
		{
			return;
		}
		List<BlueprintQuestObjectiveReference> list = new List<BlueprintQuestObjectiveReference>();
		for (int i = 0; i < definition.Acts.Length; i++)
		{
			BlueprintQuestObjective blueprintQuestObjective = ObjectiveByName(definition.Acts[i].ObjectiveName);
			if (blueprintQuestObjective != null)
			{
				list.Add(blueprintQuestObjective.ToReference<BlueprintQuestObjectiveReference>());
				bool flag = definition.Id == "Devourer" && definition.Acts[i].Act == 2;
				BlueprintQuestObjective blueprintQuestObjective2 = ((i + 1 < definition.Acts.Length) ? ObjectiveByName(definition.Acts[i + 1].ObjectiveName) : null);
				if (blueprintQuestObjective2 != null && !flag)
				{
					blueprintQuestObjective.m_NextObjectives = new List<BlueprintQuestObjectiveReference> { blueprintQuestObjective2.ToReference<BlueprintQuestObjectiveReference>() };
				}
			}
		}
		if (definition.Id == "Devourer" && ObjectiveDevourerAct3Void != null)
		{
			list.Add(ObjectiveDevourerAct3Void.ToReference<BlueprintQuestObjectiveReference>());
		}
		BlueprintQuestObjective blueprintQuestObjective3 = ObjectiveByName(definition.DormantObjectiveName);
		if (blueprintQuestObjective3 != null)
		{
			list.Add(blueprintQuestObjective3.ToReference<BlueprintQuestObjectiveReference>());
		}
		blueprintQuest.m_Objectives = list;
	}

	private static BlueprintQuest CreateQuest(string name, string title, string description, string completionText)
	{
		BlueprintQuest blueprintQuest = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintQuest bp)
		{
			bp.Title = Helpers.CreateString(Main.IsekaiContext, name + ".Title", title);
			bp.Description = Helpers.CreateString(Main.IsekaiContext, name + ".Description", description);
			bp.CompletionText = Helpers.CreateString(Main.IsekaiContext, name + ".CompletionText", completionText);
			bp.m_Group = QuestGroupId.None;
			bp.m_Type = QuestType.Normal;
			bp.m_LastChapter = 6;
		});
		s_Quests[name] = blueprintQuest;
		return blueprintQuest;
	}

	private static BlueprintQuestObjective CreateAct(PersonalQuestDefinition definition, string name, string title, string description)
	{
		int cr = CRFor(definition, name);
		BlueprintQuestObjective blueprintQuestObjective = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintQuestObjective bp)
		{
			bp.Title = Helpers.CreateString(Main.IsekaiContext, name + ".Title", title);
			bp.Description = Helpers.CreateString(Main.IsekaiContext, name + ".Description", description);
			bp.m_Quest = QuestByName(definition.QuestName).ToReference<BlueprintQuestReference>();
			bp.m_Type = BlueprintQuestObjective.Type.Objective;
			bp.m_FinishParent = false;
			bp.Locations = new List<BlueprintGlobalMapPoint.Reference>();
			bp.MultiEntranceEntries = new List<BlueprintMultiEntranceEntry.Reference>();
			AddActExperience(bp, cr, 1f);
		});
		s_Objectives[name] = blueprintQuestObjective;
		return blueprintQuestObjective;
	}

	private static BlueprintQuestObjective CreateFinalAct(PersonalQuestDefinition definition, string name, string title, string description)
	{
		int cr = CRFor(definition, name);
		BlueprintQuestObjective blueprintQuestObjective = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintQuestObjective bp)
		{
			bp.Title = Helpers.CreateString(Main.IsekaiContext, name + ".Title", title);
			bp.Description = Helpers.CreateString(Main.IsekaiContext, name + ".Description", description);
			bp.m_Quest = QuestByName(definition.QuestName).ToReference<BlueprintQuestReference>();
			bp.m_Type = BlueprintQuestObjective.Type.Objective;
			bp.m_FinishParent = true;
			bp.Locations = new List<BlueprintGlobalMapPoint.Reference>();
			bp.MultiEntranceEntries = new List<BlueprintMultiEntranceEntry.Reference>();
			AddActExperience(bp, cr, 1.5f);
		});
		s_Objectives[name] = blueprintQuestObjective;
		return blueprintQuestObjective;
	}

	private static BlueprintQuestObjective CreateDormant(PersonalQuestDefinition definition, string name)
	{
		BlueprintQuestObjective blueprintQuestObjective = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintQuestObjective bp)
		{
			bp.Title = Helpers.CreateString(Main.IsekaiContext, name + ".Title", "Dormant");
			bp.Description = Helpers.CreateString(Main.IsekaiContext, name + ".Description", "You walk another path now, and this journey has gone quiet. It waits here, exactly where you left it, for the day you take up that name again.");
			bp.m_Quest = QuestByName(definition.QuestName).ToReference<BlueprintQuestReference>();
			bp.m_Type = BlueprintQuestObjective.Type.Objective;
			bp.m_FinishParent = false;
			bp.Locations = new List<BlueprintGlobalMapPoint.Reference>();
			bp.MultiEntranceEntries = new List<BlueprintMultiEntranceEntry.Reference>();
		});
		s_Objectives[name] = blueprintQuestObjective;
		return blueprintQuestObjective;
	}

	private static void AddActExperience(BlueprintQuestObjective bp, int experienceCR, float modifier)
	{
		if (experienceCR > 0)
		{
			bp.AddComponent(delegate(Experience c)
			{
				c.Encounter = EncounterType.QuestNormal;
				c.CR = experienceCR;
				c.Modifier = modifier;
			});
		}
	}

	private static int CRFor(PersonalQuestDefinition definition, string name)
	{
		return (definition.ActNamed(name) ?? definition.ActAt(definition.Acts.Length)).ExperienceCR;
	}

	private static void LinkQuestsToGlobalMap()
	{
		try
		{
			PersonalQuestDefinition[] all = PersonalQuestCatalog.All;
			foreach (PersonalQuestDefinition personalQuestDefinition in all)
			{
				PersonalQuestAct[] acts = personalQuestDefinition.Acts;
				foreach (PersonalQuestAct personalQuestAct in acts)
				{
					BlueprintQuestObjective blueprintQuestObjective = ObjectiveByName(personalQuestAct.ObjectiveName);
					if (blueprintQuestObjective != null)
					{
						LinkAct(blueprintQuestObjective, personalQuestAct);
						if (personalQuestDefinition.Id == "Devourer" && personalQuestAct.Act == 3)
						{
							LinkAct(ObjectiveDevourerAct3Void, personalQuestAct);
						}
					}
				}
			}
		}
		catch (Exception arg)
		{
			Main.IsekaiContext.Logger.LogError($"[SubclassPersonalQuests] Error linking quests to global map points: {arg}");
		}
	}

	private static void LinkAct(BlueprintQuestObjective objective, PersonalQuestAct act)
	{
		if (objective == null)
		{
			return;
		}
		if (act.MarkerPointGuid != null)
		{
			GlobalMapVariationManager.LinkObjectiveToMapPoint(objective, BlueprintTools.GetBlueprint<BlueprintGlobalMapPoint>(act.MarkerPointGuid));
		}
		if (act.MarkerIsColiseumTile)
		{
			BlueprintGlobalMapPoint blueprintGlobalMapPoint = ((act.Chapter <= 1) ? PlanarColiseumMapPoint.KenabresPoint : PlanarColiseumMapPoint.WorldWoundPoint);
			if (blueprintGlobalMapPoint != null)
			{
				GlobalMapVariationManager.LinkObjectiveToMapPoint(objective, blueprintGlobalMapPoint);
			}
		}
	}

	private static void CreateDualTierItems()
	{
		Sprite icon = ((BlueprintUnitFact)BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "CosmicTokensFeature"))?.m_Icon;
		ItemTransmigrantLocket = CreateEquipmentItem<BlueprintItemEquipmentNeck>("ItemTransmigrantLocket", "The Transmigrant's Locket", "A weathered locket containing a tiny digital clock and a miniature photo of a bustling metropolitan skyline.\n<b>Universal:</b> Grants a +3 luck bonus to AC and to all saving throws.\n<b>Awakened [Isekai Protagonist]:</b> Grants a further +2 luck bonus to attack rolls and to the DC of all your spells.", icon, delegate(BlueprintFeature feat)
		{
			feat.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Luck;
				c.Stat = StatType.AC;
				c.Value = 3;
			});
			feat.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Luck;
				c.Stat = StatType.SaveFortitude;
				c.Value = 3;
			});
			feat.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Luck;
				c.Stat = StatType.SaveReflex;
				c.Value = 3;
			});
			feat.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Luck;
				c.Stat = StatType.SaveWill;
				c.Value = 3;
			});
			BlueprintFeature awakenedFeat = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemTransmigrantLocketAwakened", delegate(BlueprintFeature af)
			{
				af.SetName(Main.IsekaiContext, "Transmigrant's Locket (Awakened)");
				af.SetDescription(Main.IsekaiContext, "Grants a +2 luck bonus to attack rolls and to the DC of all your spells.");
				af.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				af.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 2;
					c.Descriptor = ModifierDescriptor.Luck;
					c.SpellsOnly = true;
				});
			});
			feat.AddComponent(delegate(AddFeatureIfHasFact c)
			{
				c.m_CheckedFact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "IsekaiProficiencies");
				c.m_Feature = awakenedFeat.ToReference<BlueprintUnitFactReference>();
				c.Not = false;
			});
		});
		ItemTearsOfSpeedMirage = CreateEquipmentItem<BlueprintItemEquipmentRing>("ItemTearsOfSpeedMirage", "Tears of the Whispering Phantom", "A spectral silver band recovered from the legendary Sarkorian blade-spirit, cold to the touch and humming with the rhythm of phantom swordplay.\n<b>Universal:</b> Grants a +10 ft enhancement bonus to base land speed and a +2 dodge bonus to AC.\n<b>Awakened [Martial God]:</b> Grants one additional attack at your full base attack bonus on a full attack.", icon, delegate(BlueprintFeature feat)
		{
			feat.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Enhancement;
				c.Stat = StatType.Speed;
				c.Value = 10;
			});
			feat.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Dodge;
				c.Stat = StatType.AC;
				c.Value = 2;
			});
			BlueprintFeature awakenedFeat = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemTearsOfSpeedMirageAwakened", delegate(BlueprintFeature af)
			{
				af.SetName(Main.IsekaiContext, "Tears of the Whispering Phantom (Awakened)");
				af.SetDescription(Main.IsekaiContext, "Grants one additional attack at your full base attack bonus.");
				af.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
					c.Haste = false;
				});
			});
			feat.AddComponent(delegate(AddFeatureIfHasFact c)
			{
				c.m_CheckedFact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "PhantomBladeProficiencies");
				c.m_Feature = awakenedFeat.ToReference<BlueprintUnitFactReference>();
				c.Not = false;
			});
		});
		ItemImperialSovereignSeal = CreateEquipmentItem<BlueprintItemEquipmentRing>("ItemImperialSovereignSeal", "Imperial Sovereign's Seal", "A signet ring engraved with a radiant crown and celestial dragons, radiating supreme authority.\n<b>Universal:</b> Grants its wearer a +4 morale bonus to attack rolls.\n<b>Awakened [God Emperor]:</b> Grants a further +4 morale bonus to Fortitude and Will saving throws.", icon, delegate(BlueprintFeature feat)
		{
			feat.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Morale;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 4;
			});
			BlueprintFeature awakenedFeat = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemImperialSovereignSealAwakened", delegate(BlueprintFeature af)
			{
				af.SetName(Main.IsekaiContext, "Imperial Sovereign's Seal (Awakened)");
				af.SetDescription(Main.IsekaiContext, "Grants a +4 morale bonus to Fortitude and Will saving throws.");
				af.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				af.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
			});
			feat.AddComponent(delegate(AddFeatureIfHasFact c)
			{
				c.m_CheckedFact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "GodEmperorProficiencies");
				c.m_Feature = awakenedFeat.ToReference<BlueprintUnitFactReference>();
				c.Not = false;
			});
		});
		ItemScepterOfGreatTomb = CreateEquipmentItem<BlueprintItemEquipmentRing>("ItemScepterOfGreatTomb", "Ring of the Great Tomb's Sovereign", "A black ring embedded with a miniature skull whose eye sockets burn with purple eldritch flame.\n<b>Universal:</b> Grants a +2 insight bonus to the DC of all your spells and a +4 bonus to spell penetration checks.\n<b>Awakened [Overlord]:</b> The insight bonus to the DC of your spells rises to +4.", icon, delegate(BlueprintFeature feat)
		{
			feat.AddComponent(delegate(IncreaseAllSpellsDC c)
			{
				c.Value = 2;
				c.Descriptor = ModifierDescriptor.Insight;
				c.SpellsOnly = true;
			});
			feat.AddComponent(delegate(SpellPenetrationBonus c)
			{
				c.Value = 4;
				c.Descriptor = ModifierDescriptor.UntypedStackable;
			});
			BlueprintFeature awakenedFeat = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemScepterOfGreatTombAwakened", delegate(BlueprintFeature af)
			{
				af.SetName(Main.IsekaiContext, "Ring of the Great Tomb (Awakened)");
				af.SetDescription(Main.IsekaiContext, "The insight bonus to the DC of your spells rises to +4.");
				af.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 4;
					c.Descriptor = ModifierDescriptor.Insight;
					c.SpellsOnly = true;
				});
			});
			feat.AddComponent(delegate(AddFeatureIfHasFact c)
			{
				c.m_CheckedFact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "OverlordProficiencies");
				c.m_Feature = awakenedFeat.ToReference<BlueprintUnitFactReference>();
				c.Not = false;
			});
		});
		ItemHeartOfBeelzebub = CreateEquipmentItem<BlueprintItemEquipmentBelt>("ItemHeartOfBeelzebub", "Heart of Beelzebub", "A belt woven from the muscular sinew of an abyssal world-eater, thumping with ravenous biological hunger.\n<b>Universal:</b> Grants a +4 enhancement bonus to Strength and Constitution, and Fast Healing 5.\n<b>Awakened [Slime]:</b> Grants damage reduction 5/-.", icon, delegate(BlueprintFeature feat)
		{
			feat.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Enhancement;
				c.Stat = StatType.Strength;
				c.Value = 4;
			});
			feat.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Enhancement;
				c.Stat = StatType.Constitution;
				c.Value = 4;
			});
			feat.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 5;
				c.Bonus = 0;
			});
			BlueprintFeature awakenedFeat = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemHeartOfBeelzebubAwakened", delegate(BlueprintFeature af)
			{
				af.SetName(Main.IsekaiContext, "Heart of Beelzebub (Awakened)");
				af.SetDescription(Main.IsekaiContext, "Grants the wearer damage reduction 5/-.");
				af.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 5;
				});
			});
			feat.AddComponent(delegate(AddFeatureIfHasFact c)
			{
				c.m_CheckedFact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "DevourerProficiencies");
				c.m_Feature = awakenedFeat.ToReference<BlueprintUnitFactReference>();
				c.Not = false;
			});
		});
		ItemRingOfMonarchShadow = CreateEquipmentItem<BlueprintItemEquipmentRing>("ItemRingOfMonarchShadow", "Ring of the Monarch's Shadow", "A cold obsidian band forged in the depths of the Shadow Realm, whispering commands to souls beyond death.\n<b>Universal:</b> Grants a +4 insight bonus to AC and immunity to negative energy damage.\n<b>Awakened [Shadow Monarch]:</b> Grants a +4 insight bonus to Will saving throws and a +2 insight bonus to attack rolls.", icon, delegate(BlueprintFeature feat)
		{
			feat.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
			feat.AddComponent(delegate(AddEnergyDamageImmunity c)
			{
				c.EnergyType = DamageEnergyType.NegativeEnergy;
			});
			BlueprintFeature awakenedFeat = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemRingOfMonarchShadowAwakened", delegate(BlueprintFeature af)
			{
				af.SetName(Main.IsekaiContext, "Ring of the Monarch's Shadow (Awakened)");
				af.SetDescription(Main.IsekaiContext, "Grants a +4 insight bonus to Will saving throws and a +2 insight bonus to attack rolls.");
				af.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				af.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
			});
			feat.AddComponent(delegate(AddFeatureIfHasFact c)
			{
				c.m_CheckedFact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "ShadowMonarchProficiencies");
				c.m_Feature = awakenedFeat.ToReference<BlueprintUnitFactReference>();
				c.Not = false;
			});
		});
		ItemMantleOfTrueSavior = CreateEquipmentItem<BlueprintItemEquipmentShoulders>("ItemMantleOfTrueSavior", "Mantle of the True Savior", "A pristine white and gold cloak that glows whenever a nearby companion is threatened.\n<b>Universal:</b> Grants a +3 resistance bonus to all saving throws and a +3 deflection bonus to AC.\n<b>Awakened [Hero]:</b> Grants its wearer Fast Healing 5.", icon, delegate(BlueprintFeature feat)
		{
			feat.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Resistance;
				c.Stat = StatType.SaveFortitude;
				c.Value = 3;
			});
			feat.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Resistance;
				c.Stat = StatType.SaveReflex;
				c.Value = 3;
			});
			feat.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Resistance;
				c.Stat = StatType.SaveWill;
				c.Value = 3;
			});
			feat.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Deflection;
				c.Stat = StatType.AC;
				c.Value = 3;
			});
			BlueprintFeature awakenedFeat = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemMantleOfTrueSaviorAwakened", delegate(BlueprintFeature af)
			{
				af.SetName(Main.IsekaiContext, "Mantle of the True Savior (Awakened)");
				af.SetDescription(Main.IsekaiContext, "Grants the wearer Fast Healing 5.");
				af.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 5;
					c.Bonus = 0;
				});
			});
			feat.AddComponent(delegate(AddFeatureIfHasFact c)
			{
				c.m_CheckedFact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "HeroProficiencies");
				c.m_Feature = awakenedFeat.ToReference<BlueprintUnitFactReference>();
				c.Not = false;
			});
		});
		ItemTomeOfInfiniteContingencies = CreateEquipmentItem<BlueprintItemEquipmentRing>("ItemTomeOfInfiniteContingencies", "Seal of Infinite Contingencies", "A platinum band etched with microscopic mathematical theorems predicting causal ripples across dimensions.\n<b>Universal:</b> Grants a +4 untyped bonus to Initiative, a +2 enhancement bonus to Intelligence, and a +4 competence bonus to Knowledge (Arcana) and Knowledge (World).\n<b>Awakened [Mastermind]:</b> Grants +2d6 sneak attack damage.", icon, delegate(BlueprintFeature feat)
		{
			feat.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Initiative;
				c.Value = 4;
			});
			feat.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Enhancement;
				c.Stat = StatType.Intelligence;
				c.Value = 2;
			});
			feat.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillKnowledgeArcana;
				c.Value = 4;
			});
			feat.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillKnowledgeWorld;
				c.Value = 4;
			});
			BlueprintFeature awakenedFeat = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemTomeOfInfiniteContingenciesAwakened", delegate(BlueprintFeature af)
			{
				af.SetName(Main.IsekaiContext, "Seal of Infinite Contingencies (Awakened)");
				af.SetDescription(Main.IsekaiContext, "Grants +2d6 Sneak Attack damage.");
				af.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SneakAttack;
					c.Value = 2;
				});
			});
			feat.AddComponent(delegate(AddFeatureIfHasFact c)
			{
				c.m_CheckedFact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "MastermindProficiencies");
				c.m_Feature = awakenedFeat.ToReference<BlueprintUnitFactReference>();
				c.Not = false;
			});
		});
	}

	private static T CreateEquipmentItem<T>(string name, string displayName, string description, Sprite icon, Action<BlueprintFeature> configureFeature) where T : BlueprintItemEquipment, new()
	{
		BlueprintFeature equipFeature = Helpers.CreateBlueprint(Main.IsekaiContext, name + "Feature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, displayName);
			bp.SetDescription(Main.IsekaiContext, description);
			((BlueprintUnitFact)bp).m_Icon = icon;
			configureFeature(bp);
		});
		T val = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(T bp)
		{
			((BlueprintItem)bp).m_DisplayNameText = Helpers.CreateString(Main.IsekaiContext, name + ".DisplayName", displayName);
			((BlueprintItem)bp).m_DescriptionText = Helpers.CreateString(Main.IsekaiContext, name + ".Description", description);
			((BlueprintItem)bp).m_Icon = icon;
			((BlueprintItem)bp).m_Cost = 15000;
			bp.AddComponent(delegate(AddFactToEquipmentWielder c)
			{
				c.m_Fact = equipFeature.ToReference<BlueprintUnitFactReference>();
			});
		});
		s_Relics[name] = val;
		return val;
	}

	public static void CheckAndActivateArchetypeQuest()
	{
		PersonalQuestDirector.Reconcile();
	}
}
