using System;
using System.Collections.Generic;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.GlobalMap;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Quests;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Quests
{
	public static class SubclassPersonalQuests
	{
		public static BlueprintQuest QuestUniversalNostalgia;

		public static BlueprintQuestObjective ObjectiveNostalgiaAct1;

		public static BlueprintQuestObjective ObjectiveNostalgiaAct2;

		public static BlueprintQuestObjective ObjectiveNostalgiaAct3;

		public static BlueprintQuestObjective ObjectiveNostalgiaAct4;

		public static BlueprintQuestObjective ObjectiveNostalgiaAct5;

		public static BlueprintQuest QuestMartialGod;

		public static BlueprintQuestObjective ObjectiveMartialGodAct1;

		public static BlueprintQuestObjective ObjectiveMartialGodAct2;

		public static BlueprintQuestObjective ObjectiveMartialGodAct3;

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

		public static BlueprintItemEquipmentRing ItemRingOfMartialAscension;

		public static BlueprintItemEquipmentRing ItemImperialSovereignSeal;

		public static BlueprintItemEquipmentRing ItemScepterOfGreatTomb;

		public static BlueprintItemEquipmentBelt ItemHeartOfBeelzebub;

		public static BlueprintItemEquipmentRing ItemRingOfMonarchShadow;

		public static BlueprintItemEquipmentShoulders ItemMantleOfTrueSavior;

		public static BlueprintItemEquipmentRing ItemTomeOfInfiniteContingencies;

		private const string BlackWingRuinsGuid = "fa47687825d597848ab45ca89f733db3";

		private const string AlchemyChapter02Guid = "459d73591cd8e7347985e2a0711c7c66";

		private const string EcorcheLairGuid = "0aa646666ed406446bb1055c1a7848ce";

		private const string DragonsGraveyardGuid = "b407e08507f5f5a43a6163ea89a0c02d";

		private const string GiantsLairGuid = "79e1b2c207749d448ab4e1f16db2127a";

		private const string BastionOfJusticeGuid = "211af9a81bb9d44418b2df419443e334";

		private const string DemonicAssaultSquadLairGuid = "7d550755eea3bec40b5c432d65b1aef8";

		private const string DemonicSaboteursLairGuid = "4ac2486bc7290b34e981a3911a973e85";

		public static void Add()
		{
			CreateQuestsAndObjectives();
			CreateDualTierItems();
			LinkQuestsToGlobalMap();
		}

		private static void CreateQuestsAndObjectives()
		{
			QuestUniversalNostalgia = CreateQuest("QuestUniversalNostalgia", "The Otherworld Nostalgia", "Fragments of memories and discarded relics from your previous existence on Earth lie scattered across Golarion and the Abyss. Recovering these artifacts will unlock your awakened potential.");
			ObjectiveNostalgiaAct1 = CreateObjective(QuestUniversalNostalgia, "ObjectiveNostalgiaAct1", "Recover the Weathered Keepsake Box in the ruins of Kenabres.");
			ObjectiveNostalgiaAct2 = CreateObjective(QuestUniversalNostalgia, "ObjectiveNostalgiaAct2", "Locate the scorched technological power core near the Lost Chapel wastes.");
			ObjectiveNostalgiaAct3 = CreateObjective(QuestUniversalNostalgia, "ObjectiveNostalgiaAct3", "Decode the interdimensional transmission echoing beneath the Drezen Citadel.");
			ObjectiveNostalgiaAct4 = CreateObjective(QuestUniversalNostalgia, "ObjectiveNostalgiaAct4", "Trace the memory resonance shimmering through the lower alleys of Alushinyrra.");
			ObjectiveNostalgiaAct5 = CreateObjective(QuestUniversalNostalgia, "ObjectiveNostalgiaAct5", "Synthesize your memories at the Threshold of the Multiverse and forge the Otherworlder's Locket.");
			QuestUniversalNostalgia.m_Objectives = new List<BlueprintQuestObjectiveReference>
			{
				ObjectiveNostalgiaAct1.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveNostalgiaAct2.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveNostalgiaAct3.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveNostalgiaAct4.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveNostalgiaAct5.ToReference<BlueprintQuestObjectiveReference>()
			};
			QuestMartialGod = CreateQuest("QuestMartialGod", "The Crucible of the Martial God: Ascension of Sovereign Ki", "Legends tell of an otherworldly realm of divine martial cultivators who condensed their internal spirit into boundless Ki. Awaken your Dantian, shatter the meridian limiters, and ascend to the divine apex of martial sovereignty.");
			ObjectiveMartialGodAct1 = CreateObjective(QuestMartialGod, "ObjectiveMartialGodAct1", "Awaken your lower Dantian through extreme combat trials amidst the ruins of Kenabres.");
			ObjectiveMartialGodAct2 = CreateObjective(QuestMartialGod, "ObjectiveMartialGodAct2", "Shatter your mortal meridian limiters at the harsh, wind-swept crags of the Sarkorian frontier.");
			ObjectiveMartialGodAct3 = CreateObjective(QuestMartialGod, "ObjectiveMartialGodAct3", "Condense your Sovereign Ki into divine transcendence by conquering the Planar Crucible.");
			QuestMartialGod.m_Objectives = new List<BlueprintQuestObjectiveReference>
			{
				ObjectiveMartialGodAct1.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveMartialGodAct2.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveMartialGodAct3.ToReference<BlueprintQuestObjectiveReference>()
			};
			QuestGodEmperor = CreateQuest("QuestGodEmperor", "The Gospel of the Living God", "As an ascendant sovereign, mortal laws alone are insufficient: you must establish your own divine religion. Rally worshippers, erect shrines to your glory, and bind the crusade under the holy scriptures of the Living Emperor.");
			ObjectiveGodEmperorAct1 = CreateObjective(QuestGodEmperor, "ObjectiveGodEmperorAct1", "Establish your own religion among the disillusioned crusaders in Kenabres by erecting the Altar of the Living God.");
			ObjectiveGodEmperorAct2 = CreateObjective(QuestGodEmperor, "ObjectiveGodEmperorAct2", "Spread your holy scriptures and recruit ordained disciples across the frontier crusade garrisons.");
			ObjectiveGodEmperorAct3 = CreateObjective(QuestGodEmperor, "ObjectiveGodEmperorAct3", "Consecrate the Grand Imperial Dais of Drezen, enthroning yourself as the sole divine sovereign of the Fifth Crusade.");
			QuestGodEmperor.m_Objectives = new List<BlueprintQuestObjectiveReference>
			{
				ObjectiveGodEmperorAct1.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveGodEmperorAct2.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveGodEmperorAct3.ToReference<BlueprintQuestObjectiveReference>()
			};
			QuestOverlord = CreateQuest("QuestOverlord", "The Scepter of Absolute Order", "Excavate the subterranean necropolis of an ancient sorcerer-king to forge the Scepter of the Great Tomb.");
			ObjectiveOverlordAct1 = CreateObjective(QuestOverlord, "ObjectiveOverlordAct1", "Excavate the sealed tomb entrance beneath the Sarkorian crypts.");
			ObjectiveOverlordAct2 = CreateObjective(QuestOverlord, "ObjectiveOverlordAct2", "Subjugate the slumbering bone sorcerers and extract the Sovereign Core.");
			ObjectiveOverlordAct3 = CreateObjective(QuestOverlord, "ObjectiveOverlordAct3", "Erect the Great Tomb throne room beneath Drezen and bind all death to your command.");
			QuestOverlord.m_Objectives = new List<BlueprintQuestObjectiveReference>
			{
				ObjectiveOverlordAct1.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveOverlordAct2.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveOverlordAct3.ToReference<BlueprintQuestObjectiveReference>()
			};
			QuestDevourer = CreateQuest("QuestDevourer", "The Gluttonous Sovereign", "Reborn as an amorphous slime with an insatiable predator stomach, you must navigate your evolution. Will you become a benevolent sovereign uniting monsters in peace, or an abyssal world-eater devouring all?");
			ObjectiveDevourerAct1 = CreateObjective(QuestDevourer, "ObjectiveDevourerAct1", "Awaken your biological mimicry by consuming the corrosive acid wyrm in Kenabres.");
			ObjectiveDevourerAct2 = CreateObjective(QuestDevourer, "ObjectiveDevourerAct2", "Make the Sovereign Divergence: choose between the Benevolent Tempest path or the Abyssal Void path in the wastes.");
			ObjectiveDevourerAct3 = CreateObjective(QuestDevourer, "ObjectiveDevourerAct3", "Hunt down and assimilate the Primordial Abyssal Behemoth to complete your ultimate evolution.");
			QuestDevourer.m_Objectives = new List<BlueprintQuestObjectiveReference>
			{
				ObjectiveDevourerAct1.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveDevourerAct2.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveDevourerAct3.ToReference<BlueprintQuestObjectiveReference>()
			};
			QuestShadowMonarch = CreateQuest("QuestShadowMonarch", "The Coronation of the Shadow King", "Answer the summons of the subterranean Monarch's Crypt, face your own shadow trial, and rise as sovereign of all departed souls.");
			ObjectiveShadowMonarchAct1 = CreateObjective(QuestShadowMonarch, "ObjectiveShadowMonarchAct1", "Answer the whispers echoing from the subterranean catacombs.");
			ObjectiveShadowMonarchAct2 = CreateObjective(QuestShadowMonarch, "ObjectiveShadowMonarchAct2", "Extract and command the shadows of the fallen vanguard in the valley of tombs.");
			ObjectiveShadowMonarchAct3 = CreateObjective(QuestShadowMonarch, "ObjectiveShadowMonarchAct3", "Ascend the Throne of Shadows and proclaim your eternal reign over death.");
			QuestShadowMonarch.m_Objectives = new List<BlueprintQuestObjectiveReference>
			{
				ObjectiveShadowMonarchAct1.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveShadowMonarchAct2.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveShadowMonarchAct3.ToReference<BlueprintQuestObjectiveReference>()
			};
			QuestHero = CreateQuest("QuestHero", "The Weight of the Savior's Oath", "The burden of a hero is not merely bearing the blade, but bearing the crushing weight of every soul you swear to deliver. As the crusade's grim realities gnaw at your convictions, uphold your oath: or feel the slow creeping corrosion of martyrdom.");
			ObjectiveHeroAct1 = CreateObjective(QuestHero, "ObjectiveHeroAct1", "Rescue the isolated vanguard in Kenabres and witness the unbearable price mortals pay when heroes arrive too late.");
			ObjectiveHeroAct2 = CreateObjective(QuestHero, "ObjectiveHeroAct2", "Shield the desperate survivors at the Dragon's Graveyard, bearing their terror and suffering upon your own soul.");
			ObjectiveHeroAct3 = CreateObjective(QuestHero, "ObjectiveHeroAct3", "Stand defiant at the Bastion of Justice against overwhelming despair, cementing whether your oath purifies you or breaks your spirit.");
			QuestHero.m_Objectives = new List<BlueprintQuestObjectiveReference>
			{
				ObjectiveHeroAct1.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveHeroAct2.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveHeroAct3.ToReference<BlueprintQuestObjectiveReference>()
			};
			QuestMastermind = CreateQuest("QuestMastermind", "The Architect's Endgame", "Engage in 4D causal chess against the demon generals. Decrypt their plans and orchestrate the flawless checkmate.");
			ObjectiveMastermindAct1 = CreateObjective(QuestMastermind, "ObjectiveMastermindAct1", "Intercept and decrypt the three-layer Abyssal cipher in the alchemy ruins.");
			ObjectiveMastermindAct2 = CreateObjective(QuestMastermind, "ObjectiveMastermindAct2", "Plant false intelligence among the demonic saboteurs to lure Deskari's warlords into a lethal ambush.");
			ObjectiveMastermindAct3 = CreateObjective(QuestMastermind, "ObjectiveMastermindAct3", "Trigger the grand counter-offensive in the Giant's Lair and execute the final checkmate.");
			QuestMastermind.m_Objectives = new List<BlueprintQuestObjectiveReference>
			{
				ObjectiveMastermindAct1.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveMastermindAct2.ToReference<BlueprintQuestObjectiveReference>(),
				ObjectiveMastermindAct3.ToReference<BlueprintQuestObjectiveReference>()
			};
		}

		private static BlueprintQuest CreateQuest(string name, string title, string description)
		{
			return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintQuest bp)
			{
				bp.Title = Helpers.CreateString(Main.IsekaiContext, name + ".Title", title);
				bp.Description = Helpers.CreateString(Main.IsekaiContext, name + ".Description", description);
				bp.m_Group = QuestGroupId.None;
				bp.m_Type = QuestType.Normal;
				bp.m_LastChapter = 5;
			});
		}

		private static BlueprintQuestObjective CreateObjective(BlueprintQuest quest, string name, string description)
		{
			return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintQuestObjective bp)
			{
				bp.Title = Helpers.CreateString(Main.IsekaiContext, name + ".Title", quest.Title);
				bp.Description = Helpers.CreateString(Main.IsekaiContext, name + ".Description", description);
				bp.m_Quest = quest.ToReference<BlueprintQuestReference>();
				bp.m_Type = BlueprintQuestObjective.Type.Objective;
			});
		}

		private static void LinkQuestsToGlobalMap()
		{
			try
			{
				BlueprintGlobalMapPoint point = BlueprintSafetyExtensions.SafeGetBlueprint<BlueprintGlobalMapPoint>("fa47687825d597848ab45ca89f733db3");
				BlueprintGlobalMapPoint point2 = BlueprintSafetyExtensions.SafeGetBlueprint<BlueprintGlobalMapPoint>("459d73591cd8e7347985e2a0711c7c66");
				BlueprintGlobalMapPoint point3 = BlueprintSafetyExtensions.SafeGetBlueprint<BlueprintGlobalMapPoint>("0aa646666ed406446bb1055c1a7848ce");
				BlueprintGlobalMapPoint point4 = BlueprintSafetyExtensions.SafeGetBlueprint<BlueprintGlobalMapPoint>("b407e08507f5f5a43a6163ea89a0c02d");
				BlueprintGlobalMapPoint point5 = BlueprintSafetyExtensions.SafeGetBlueprint<BlueprintGlobalMapPoint>("79e1b2c207749d448ab4e1f16db2127a");
				BlueprintGlobalMapPoint point6 = BlueprintSafetyExtensions.SafeGetBlueprint<BlueprintGlobalMapPoint>("211af9a81bb9d44418b2df419443e334");
				BlueprintSafetyExtensions.SafeGetBlueprint<BlueprintGlobalMapPoint>("7d550755eea3bec40b5c432d65b1aef8");
				BlueprintGlobalMapPoint point7 = BlueprintSafetyExtensions.SafeGetBlueprint<BlueprintGlobalMapPoint>("4ac2486bc7290b34e981a3911a973e85");
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveNostalgiaAct1, point);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveNostalgiaAct2, point2);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveNostalgiaAct3, point3);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveMartialGodAct1, point);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveMartialGodAct2, point3);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveGodEmperorAct1, point2);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveGodEmperorAct2, point5);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveOverlordAct1, point2);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveOverlordAct2, point3);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveDevourerAct1, point);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveDevourerAct2, point2);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveDevourerAct3, point4);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveShadowMonarchAct1, point);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveShadowMonarchAct2, point4);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveHeroAct1, point);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveHeroAct2, point4);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveHeroAct3, point6);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveMastermindAct1, point2);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveMastermindAct2, point7);
				GlobalMapVariationManager.LinkObjectiveToMapPoint(ObjectiveMastermindAct3, point5);
			}
			catch (Exception arg)
			{
				Main.IsekaiContext.Logger.LogError($"[SubclassPersonalQuests] Error linking quests to global map points: {arg}");
			}
		}

		private static void CreateDualTierItems()
		{
			Sprite icon = ((BlueprintUnitFact)BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "CosmicTokensFeature"))?.m_Icon;
			ItemTransmigrantLocket = CreateEquipmentItem<BlueprintItemEquipmentNeck>("ItemTransmigrantLocket", "The Otherworlder's Locket", "A weathered locket containing a tiny digital clock and a miniature photo of a bustling metropolitan skyline.\n<b>Universal:</b> Grants a +3 Luck bonus to AC and all saving throws.\n<b>Awakened [Isekai Protagonist]:</b> Grants an additional +2 Luck bonus to attack rolls and spell DC.", icon, delegate(BlueprintFeature feat)
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
					af.SetName(Main.IsekaiContext, "Otherworlder's Locket (Awakened)");
					af.SetDescription(Main.IsekaiContext, "Grants a +2 Luck bonus to attack rolls and spell DC.");
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
					});
				});
				feat.AddComponent(delegate(AddFeatureIfHasFact c)
				{
					c.m_CheckedFact = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProficiencies")?.ToReference<BlueprintUnitFactReference>();
					c.m_Feature = awakenedFeat.ToReference<BlueprintUnitFactReference>();
					c.Not = false;
				});
			});
			ItemRingOfMartialAscension = CreateEquipmentItem<BlueprintItemEquipmentRing>("ItemRingOfMartialAscension", "Band of Transcendent Ki", "A gleaming ring forged of condensed celestial Ki and pure jade, thrumming with the heartbeat of an enlightened martial sovereign.\n<b>Universal:</b> Grants a +10 ft bonus to base land speed and a +2 Dodge bonus to AC.\n<b>Awakened [Martial God]:</b> Grants an additional attack at full Base Attack Bonus on a full attack.", icon, delegate(BlueprintFeature feat)
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
				BlueprintFeature awakenedFeat = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemRingOfMartialAscensionAwakened", delegate(BlueprintFeature af)
				{
					af.SetName(Main.IsekaiContext, "Band of Transcendent Ki (Awakened)");
					af.SetDescription(Main.IsekaiContext, "Grants an additional attack at full Base Attack Bonus.");
					af.AddComponent(delegate(BuffExtraAttack c)
					{
						c.Number = 1;
						c.Haste = false;
					});
				});
				feat.AddComponent(delegate(AddFeatureIfHasFact c)
				{
					c.m_CheckedFact = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialGodProficiencies")?.ToReference<BlueprintUnitFactReference>();
					c.m_Feature = awakenedFeat.ToReference<BlueprintUnitFactReference>();
					c.Not = false;
				});
			});
			ItemImperialSovereignSeal = CreateEquipmentItem<BlueprintItemEquipmentRing>("ItemImperialSovereignSeal", "Imperial Sovereign's Seal", "A signet ring engraved with a radiant crown and celestial dragons, radiating supreme authority.\n<b>Universal:</b> Grants a +4 Morale bonus to attack rolls for the wearer and all allies within 30 feet.\n<b>Awakened [God Emperor]:</b> Rest tithes generate +50% additional gold and crusade finances.", icon, delegate(BlueprintFeature feat)
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
					af.SetDescription(Main.IsekaiContext, "Grants a +4 Morale bonus to saving throws and increases imperial command presence.");
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
					c.m_CheckedFact = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GodEmperorProficiencies")?.ToReference<BlueprintUnitFactReference>();
					c.m_Feature = awakenedFeat.ToReference<BlueprintUnitFactReference>();
					c.Not = false;
				});
			});
			ItemScepterOfGreatTomb = CreateEquipmentItem<BlueprintItemEquipmentRing>("ItemScepterOfGreatTomb", "Ring of the Great Tomb's Sovereign", "A black ring embedded with a miniature skull whose eye sockets burn with purple eldritch flame.\n<b>Universal:</b> Grants a +2 DC to all spells and +4 to Spell Penetration checks.\n<b>Awakened [Overlord]:</b> Despair Auras and Grasp Heart DC are increased by an additional +2.", icon, delegate(BlueprintFeature feat)
			{
				feat.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 2;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
				feat.AddComponent(delegate(SpellPenetrationBonus c)
				{
					c.Value = 4;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
				BlueprintFeature awakenedFeat = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemScepterOfGreatTombAwakened", delegate(BlueprintFeature af)
				{
					af.SetName(Main.IsekaiContext, "Ring of the Great Tomb (Awakened)");
					af.SetDescription(Main.IsekaiContext, "Increases spell DC by an additional +2.");
					af.AddComponent(delegate(IncreaseAllSpellsDC c)
					{
						c.Value = 2;
						c.Descriptor = ModifierDescriptor.UntypedStackable;
					});
				});
				feat.AddComponent(delegate(AddFeatureIfHasFact c)
				{
					c.m_CheckedFact = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "OverlordProficiencies")?.ToReference<BlueprintUnitFactReference>();
					c.m_Feature = awakenedFeat.ToReference<BlueprintUnitFactReference>();
					c.Not = false;
				});
			});
			ItemHeartOfBeelzebub = CreateEquipmentItem<BlueprintItemEquipmentBelt>("ItemHeartOfBeelzebub", "Heart of Beelzebub", "A belt woven from the muscular sinew of an abyssal world-eater, thumping with ravenous biological hunger.\n<b>Universal:</b> Grants a +4 Enhancement bonus to Strength and Constitution, and Fast Healing 5.\n<b>Awakened [Slime]:</b> Predator Maw deals an additional 2d6 unholy damage and grants temporary HP on bite.", icon, delegate(BlueprintFeature feat)
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
					af.SetDescription(Main.IsekaiContext, "Grants an additional +4 Enhancement to Strength and Constitution.");
					af.AddComponent(delegate(AddStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.Enhancement;
						c.Stat = StatType.Strength;
						c.Value = 4;
					});
					af.AddComponent(delegate(AddStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.Enhancement;
						c.Stat = StatType.Constitution;
						c.Value = 4;
					});
				});
				feat.AddComponent(delegate(AddFeatureIfHasFact c)
				{
					c.m_CheckedFact = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DevourerProficiencies")?.ToReference<BlueprintUnitFactReference>();
					c.m_Feature = awakenedFeat.ToReference<BlueprintUnitFactReference>();
					c.Not = false;
				});
			});
			ItemRingOfMonarchShadow = CreateEquipmentItem<BlueprintItemEquipmentRing>("ItemRingOfMonarchShadow", "Ring of the Monarch's Shadow", "A cold obsidian band forged in the depths of the Shadow Realm, whispering commands to souls beyond death.\n<b>Universal:</b> Grants a +4 Insight bonus to AC and total immunity to negative energy and death effects.\n<b>Awakened [Shadow Monarch]:</b> Shadow minions gain +4 to all physical ability scores and +10 ft speed.", icon, delegate(BlueprintFeature feat)
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
					af.SetDescription(Main.IsekaiContext, "Grants an additional +2 Insight bonus to AC and saves.");
					af.AddComponent(delegate(AddStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.Insight;
						c.Stat = StatType.AC;
						c.Value = 2;
					});
					af.AddComponent(delegate(AddStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.Insight;
						c.Stat = StatType.SaveWill;
						c.Value = 2;
					});
				});
				feat.AddComponent(delegate(AddFeatureIfHasFact c)
				{
					c.m_CheckedFact = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowMonarchProficiencies")?.ToReference<BlueprintUnitFactReference>();
					c.m_Feature = awakenedFeat.ToReference<BlueprintUnitFactReference>();
					c.Not = false;
				});
			});
			ItemMantleOfTrueSavior = CreateEquipmentItem<BlueprintItemEquipmentShoulders>("ItemMantleOfTrueSavior", "Mantle of the True Savior", "A pristine white and gold cloak that glows whenever a nearby companion is threatened.\n<b>Universal:</b> Grants a +3 Resistance bonus to all saving throws and a +3 Deflection bonus to AC.\n<b>Awakened [Hero]:</b> Bonds of Fellowship grants Fast Healing 5 to all nearby allies.", icon, delegate(BlueprintFeature feat)
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
					af.SetDescription(Main.IsekaiContext, "Grants Fast Healing 5 to the wearer.");
					af.AddComponent(delegate(AddEffectFastHealing c)
					{
						c.Heal = 5;
						c.Bonus = 0;
					});
				});
				feat.AddComponent(delegate(AddFeatureIfHasFact c)
				{
					c.m_CheckedFact = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeroProficiencies")?.ToReference<BlueprintUnitFactReference>();
					c.m_Feature = awakenedFeat.ToReference<BlueprintUnitFactReference>();
					c.Not = false;
				});
			});
			ItemTomeOfInfiniteContingencies = CreateEquipmentItem<BlueprintItemEquipmentRing>("ItemTomeOfInfiniteContingencies", "Seal of Infinite Contingencies", "A platinum band etched with microscopic mathematical theorems predicting causal ripples across dimensions.\n<b>Universal:</b> Grants a +4 Insight bonus to Initiative and a +2 bonus to Intelligence and all Knowledge skill checks.\n<b>Awakened [Mastermind]:</b> Tactical Ambush deals an additional +3d6 sneak attack damage.", icon, delegate(BlueprintFeature feat)
			{
				feat.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
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
						c.Descriptor = ModifierDescriptor.Insight;
						c.Stat = StatType.SneakAttack;
						c.Value = 2;
					});
				});
				feat.AddComponent(delegate(AddFeatureIfHasFact c)
				{
					c.m_CheckedFact = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindProficiencies")?.ToReference<BlueprintUnitFactReference>();
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
			return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(T bp)
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
		}

		public static void CheckAndActivateArchetypeQuest()
		{
			try
			{
				Player player = Game.Instance?.Player;
				if (player == null || player.QuestBook == null)
				{
					return;
				}
				UnitEntityData unitEntityData = player.SafeGetMainCharacter();
				QuestBook qb;
				if (unitEntityData?.Descriptor != null)
				{
					qb = player.QuestBook;
					BlueprintCharacterClassReference reference = IsekaiProtagonistClass.GetReference();
					if ((reference != null && (unitEntityData.Progression?.GetClassLevel(reference) ?? 0) > 0) || unitEntityData.Descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProficiencies")))
					{
						TryStart(QuestUniversalNostalgia, ObjectiveNostalgiaAct1, ItemTransmigrantLocket);
					}
					if (unitEntityData.Descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialGodProficiencies")) || unitEntityData.Descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialTranscendenceFeature")))
					{
						TryStart(QuestMartialGod, ObjectiveMartialGodAct1, ItemRingOfMartialAscension);
					}
					else if (unitEntityData.Descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GodEmperorProficiencies")))
					{
						TryStart(QuestGodEmperor, ObjectiveGodEmperorAct1, ItemImperialSovereignSeal);
					}
					else if (unitEntityData.Descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "OverlordProficiencies")))
					{
						TryStart(QuestOverlord, ObjectiveOverlordAct1, ItemScepterOfGreatTomb);
					}
					else if (unitEntityData.Descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DevourerProficiencies")))
					{
						TryStart(QuestDevourer, ObjectiveDevourerAct1, ItemHeartOfBeelzebub);
					}
					else if (unitEntityData.Descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowMonarchProficiencies")))
					{
						TryStart(QuestShadowMonarch, ObjectiveShadowMonarchAct1, ItemRingOfMonarchShadow);
					}
					else if (unitEntityData.Descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeroProficiencies")))
					{
						TryStart(QuestHero, ObjectiveHeroAct1, ItemMantleOfTrueSavior);
					}
					else if (unitEntityData.Descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindProficiencies")))
					{
						TryStart(QuestMastermind, ObjectiveMastermindAct1, ItemTomeOfInfiniteContingencies);
					}
					CheckAdvanceByChapter();
				}
				void TryStart(BlueprintQuest quest, BlueprintQuestObjective obj, BlueprintItem itemReward)
				{
					if (quest != null && obj != null && qb.GetQuestState(quest) == QuestState.None)
					{
						qb.GiveObjective(obj);
						EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
						{
							h.HandleLogMessage($"<color=#9400D3><b>[Personal Quest]</b></color> Begun: <b>{quest.Title}</b>!");
						});
						if (itemReward != null && !player.Inventory.Contains(itemReward))
						{
							player.Inventory.Add(itemReward, 1);
							EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
							{
								h.HandleLogMessage("<color=#FFD700>[Relic Discovered]</color> Acquired origin heirloom: <b>" + itemReward.Name + "</b>!");
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in CheckAndActivateArchetypeQuest: " + ex);
			}
		}

		public static void CheckAdvanceByChapter()
		{
			try
			{
				int valueOrDefault = (Game.Instance?.Player?.Chapter).GetValueOrDefault();
				if (valueOrDefault >= 2)
				{
					AdvanceAct(1);
				}
				if (valueOrDefault >= 3)
				{
					AdvanceAct(2);
				}
				if (valueOrDefault >= 4)
				{
					AdvanceAct(3);
				}
				if (valueOrDefault >= 5)
				{
					AdvanceAct(4);
				}
			}
			catch
			{
			}
		}

		public static void AdvanceAct(int act)
		{
			try
			{
				QuestBook qb = (Game.Instance?.Player)?.QuestBook;
				if (qb == null)
				{
					return;
				}
				switch (act)
				{
				case 1:
					AdvanceStage(ObjectiveNostalgiaAct1, ObjectiveNostalgiaAct2);
					break;
				case 2:
					AdvanceStage(ObjectiveNostalgiaAct2, ObjectiveNostalgiaAct3);
					break;
				case 3:
					AdvanceStage(ObjectiveNostalgiaAct3, ObjectiveNostalgiaAct4);
					break;
				case 4:
					AdvanceStage(ObjectiveNostalgiaAct4, ObjectiveNostalgiaAct5);
					break;
				case 5:
				case 6:
					if (ObjectiveNostalgiaAct5 != null && qb.GetObjectiveState(ObjectiveNostalgiaAct5) == QuestObjectiveState.Started)
					{
						qb.CompleteObjective(ObjectiveNostalgiaAct5);
						EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
						{
							h.HandleLogMessage($"<color=#FFD700><b>[Personal Origin Quest Completed]</b></color> <b>{QuestUniversalNostalgia.Title}</b>!");
						});
					}
					break;
				}
				if (act == 1 || act == 2)
				{
					AdvanceStage(ObjectiveMartialGodAct1, ObjectiveMartialGodAct2);
					AdvanceStage(ObjectiveGodEmperorAct1, ObjectiveGodEmperorAct2);
					AdvanceStage(ObjectiveOverlordAct1, ObjectiveOverlordAct2);
					AdvanceStage(ObjectiveDevourerAct1, ObjectiveDevourerAct2);
					AdvanceStage(ObjectiveShadowMonarchAct1, ObjectiveShadowMonarchAct2);
					AdvanceStage(ObjectiveHeroAct1, ObjectiveHeroAct2);
					AdvanceStage(ObjectiveMastermindAct1, ObjectiveMastermindAct2);
				}
				else if (act == 3 || act == 4)
				{
					AdvanceStage(ObjectiveMartialGodAct2, ObjectiveMartialGodAct3);
					AdvanceStage(ObjectiveGodEmperorAct2, ObjectiveGodEmperorAct3);
					AdvanceStage(ObjectiveOverlordAct2, ObjectiveOverlordAct3);
					AdvanceStage(ObjectiveDevourerAct2, ObjectiveDevourerAct3);
					AdvanceStage(ObjectiveShadowMonarchAct2, ObjectiveShadowMonarchAct3);
					AdvanceStage(ObjectiveHeroAct2, ObjectiveHeroAct3);
					AdvanceStage(ObjectiveMastermindAct2, ObjectiveMastermindAct3);
				}
				else if (act >= 5)
				{
					CompleteFinal(QuestMartialGod, ObjectiveMartialGodAct3);
					CompleteFinal(QuestGodEmperor, ObjectiveGodEmperorAct3);
					CompleteFinal(QuestOverlord, ObjectiveOverlordAct3);
					CompleteFinal(QuestDevourer, ObjectiveDevourerAct3);
					CompleteFinal(QuestShadowMonarch, ObjectiveShadowMonarchAct3);
					CompleteFinal(QuestHero, ObjectiveHeroAct3);
					CompleteFinal(QuestMastermind, ObjectiveMastermindAct3);
				}
				void AdvanceStage(BlueprintQuestObjective current, BlueprintQuestObjective next)
				{
					if (current != null && qb.GetObjectiveState(current) == QuestObjectiveState.Started)
					{
						qb.CompleteObjective(current);
						EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
						{
							h.HandleLogMessage($"<color=#32CD32><b>[Personal Quest Step Completed]</b></color> <b>{current.Title}</b>!");
						});
						if (next != null && qb.GetObjectiveState(next) == QuestObjectiveState.None)
						{
							qb.GiveObjective(next);
							EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
							{
								h.HandleLogMessage($"<color=#9400D3><b>[Personal Quest Updated]</b></color> <b>{next.Title}</b>!");
							});
						}
					}
				}
				void CompleteFinal(BlueprintQuest quest, BlueprintQuestObjective finalObj)
				{
					if (finalObj != null && qb.GetObjectiveState(finalObj) == QuestObjectiveState.Started)
					{
						qb.CompleteObjective(finalObj);
						EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
						{
							h.HandleLogMessage($"<color=#FFD700><b>[Archetype Hero's Journey Completed]</b></color> <b>{quest?.Title ?? finalObj.Title}</b>!");
						});
					}
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in SubclassPersonalQuests.AdvanceAct: " + ex);
			}
		}
	}
}
