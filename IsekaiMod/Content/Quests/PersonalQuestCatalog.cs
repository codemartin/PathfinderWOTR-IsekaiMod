using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Classes.IsekaiProtagonist;

namespace IsekaiMod.Content.Quests;

internal static class PersonalQuestCatalog
{
	internal const string NostalgiaId = "Nostalgia";

	internal const string PhantomBladeId = "PhantomBlade";

	internal const string GodEmperorId = "GodEmperor";

	internal const string OverlordId = "Overlord";

	internal const string DevourerId = "Devourer";

	internal const string ShadowMonarchId = "ShadowMonarch";

	internal const string HeroId = "Hero";

	internal const string MastermindId = "Mastermind";

	internal const string ObjWardstoneCleanserGuid = "488bfd3ac02152148886b9fa2856ba59";

	internal const string ObjGatherAlliesGuid = "5d6fc7d9baa485142a19010710bc1177";

	internal const string ObjHangSwordOfValorGuid = "00cb8592e0ba7674c9dec3a60351cfd5";

	internal const string ObjMidnightFaneLeaderGuid = "bcf5a9f544fd8bb46a1562dcde735893";

	internal const string ObjSlayHosillaGuid = "1992ddc7d1b615748a0652a964846d4c";

	internal const string ObjGrayGarrisonGuid = "388790cbbec808e4487f547e7d805cc9";

	internal const string ObjDefendDefendersHeartGuid = "5b8cb2c804cc9b74f99f6f640955e381";

	internal const string ObjSurviveNightAttackGuid = "c53e98fd90a8cd8438c0b364dd8ef67d";

	internal const string ObjAnswerTheHornGuid = "659bfbb92c0c17e429a89196196ef85f";

	internal const string ObjCultistDispatchGuid = "a5f23dc3dd2747f4fb8034315c7e921d";

	internal const string ObjTakeCitadelGuid = "f2e6d4a07ccf9314190cf1ddee028513";

	internal const string ObjSlayXanthirVangGuid = "d44f91b07f9914349aa0b6c082d98c25";

	internal const string ObjThresholdAssaultGuid = "5f0fa112e73203947a3717db16d3106e";

	internal const string ObjAlushinyrraAudienceGuid = "1bb76ec5b42c2644abe5e895cecf0cea";

	internal const string AreaBlackWingRuinsGuid = "4cf3a74ce8b53b949abad207502da4dc";

	internal const string AreaLostChapelGuid = "2183cc056a7b5d647ad475c8bc6c2074";

	internal const string AreaMidnightFaneGuid = "8c78892604c58f443a8fa522ca99fcfc";

	internal const string AreaAlushinyrraLowerCityGuid = "4f849f5683145d0489db4077e0d7eccf";

	internal const string AreaAlushinyrraMiddleCityGuid = "180cdb4b48d561f4cb4ef9a066727960";

	internal const string AreaAlushinyrraUpperCityGuid = "8217b05e37078414981d994151f0ffb1";

	internal const string PointBlackwingLibraryGuid = "fa47687825d597848ab45ca89f733db3";

	internal const string PointMarketSquareGuid = "89f28f2bfc23533489f7b9e7c2d9ce03";

	internal const string PointDefendersHeartGuid = "40b6a5c759b584343987a1dfcf919a06";

	internal const string PointGrayGarrisonGuid = "556d74cd8f75e674981862b10a84fa70";

	internal const string PointLostChapelGuid = "c69821ae78cf7614b943fd0098fec57f";

	internal const string PointDrezenGuid = "70990a0f7010447478e68b8e16ae52b2";

	internal const string PointIvorySanctumGuid = "669220945fa20fa4a9317dea3618a19c";

	internal const string PointThresholdGuid = "4e43f2e05241536408268467bfdfde86";

	internal const string TrialPhantomSwordSaintId = "Trial.PhantomBlade.Act2";

	internal const string TrialBladeSpiritId = "Trial.PhantomBlade.Act3";

	internal const string TrialBoneCourtId = "Trial.Overlord.Act2";

	internal const string TrialCorrosiveWyrmId = "Trial.Devourer.Act1";

	internal const string TrialPrimordialBehemothId = "Trial.Devourer.Act3";

	internal const string TrialFallenVanguardId = "Trial.ShadowMonarch.Act2";

	internal const string TrialThroneOfShadowsId = "Trial.ShadowMonarch.Act3";

	internal const string DevourerPathTempest = "Tempest";

	internal const string DevourerPathVoid = "Void";

	internal const string DecreeImperialDaisId = "ImperialDais";

	internal static readonly PersonalQuestDefinition Nostalgia = new PersonalQuestDefinition
	{
		Id = "Nostalgia",
		ArchetypeToken = null,
		QuestName = "QuestUniversalNostalgia",
		DormantObjectiveName = null,
		RelicItemName = "ItemTransmigrantLocket",
		Acts = new PersonalQuestAct[5]
		{
			new PersonalQuestAct
			{
				Act = 1,
				ObjectiveName = "ObjectiveNostalgiaAct1",
				Chapter = 1,
				Trigger = PersonalQuestTrigger.AreaVisited,
				TriggerIds = new string[1] { "4cf3a74ce8b53b949abad207502da4dc" },
				MarkerPointGuid = "fa47687825d597848ab45ca89f733db3"
			},
			new PersonalQuestAct
			{
				Act = 2,
				ObjectiveName = "ObjectiveNostalgiaAct2",
				Chapter = 2,
				Trigger = PersonalQuestTrigger.AreaVisited,
				TriggerIds = new string[1] { "2183cc056a7b5d647ad475c8bc6c2074" },
				MarkerPointGuid = "c69821ae78cf7614b943fd0098fec57f"
			},
			new PersonalQuestAct
			{
				Act = 3,
				ObjectiveName = "ObjectiveNostalgiaAct3",
				Chapter = 3,
				Trigger = PersonalQuestTrigger.AreaVisited,
				TriggerIds = new string[1] { "8c78892604c58f443a8fa522ca99fcfc" },
				MarkerPointGuid = "70990a0f7010447478e68b8e16ae52b2"
			},
			new PersonalQuestAct
			{
				Act = 4,
				ObjectiveName = "ObjectiveNostalgiaAct4",
				Chapter = 4,
				Trigger = PersonalQuestTrigger.AreaVisited,
				TriggerIds = new string[3] { "4f849f5683145d0489db4077e0d7eccf", "180cdb4b48d561f4cb4ef9a066727960", "8217b05e37078414981d994151f0ffb1" },
				Fallback = PersonalQuestTrigger.VanillaObjectiveCompleted,
				FallbackIds = new string[1] { "1bb76ec5b42c2644abe5e895cecf0cea" }
			},
			new PersonalQuestAct
			{
				Act = 5,
				ObjectiveName = "ObjectiveNostalgiaAct5",
				Chapter = 5,
				Trigger = PersonalQuestTrigger.CupCleared,
				TriggerIds = new string[1] { "Act5.MultiverseThreshold" },
				Fallback = PersonalQuestTrigger.VanillaObjectiveStarted,
				FallbackIds = new string[1] { "5f0fa112e73203947a3717db16d3106e" },
				MarkerPointGuid = "4e43f2e05241536408268467bfdfde86",
				MarkerIsColiseumTile = true
			}
		}
	};

	internal static readonly PersonalQuestDefinition PhantomBlade = new PersonalQuestDefinition
	{
		Id = "PhantomBlade",
		ArchetypeToken = "MartialGod",
		QuestName = "QuestPhantomBlade",
		DormantObjectiveName = "ObjectivePhantomBladeDormant",
		RelicItemName = "ItemTearsOfSpeedMirage",
		Acts = new PersonalQuestAct[3]
		{
			new PersonalQuestAct
			{
				Act = 1,
				ObjectiveName = "ObjectivePhantomBladeAct1",
				Chapter = 1,
				Trigger = PersonalQuestTrigger.VanillaObjectiveCompleted,
				TriggerIds = new string[1] { "488bfd3ac02152148886b9fa2856ba59" },
				MarkerPointGuid = "89f28f2bfc23533489f7b9e7c2d9ce03"
			},
			new PersonalQuestAct
			{
				Act = 2,
				ObjectiveName = "ObjectivePhantomBladeAct2",
				Chapter = 2,
				Trigger = PersonalQuestTrigger.Trial,
				TrialId = "Trial.PhantomBlade.Act2",
				MarkerIsColiseumTile = true
			},
			new PersonalQuestAct
			{
				Act = 3,
				ObjectiveName = "ObjectivePhantomBladeAct3",
				Chapter = 3,
				Trigger = PersonalQuestTrigger.Trial,
				TrialId = "Trial.PhantomBlade.Act3",
				MarkerIsColiseumTile = true
			}
		}
	};

	internal static readonly PersonalQuestDefinition GodEmperor = new PersonalQuestDefinition
	{
		Id = "GodEmperor",
		ArchetypeToken = "GodEmperor",
		QuestName = "QuestGodEmperor",
		DormantObjectiveName = "ObjectiveGodEmperorDormant",
		RelicItemName = "ItemImperialSovereignSeal",
		Acts = new PersonalQuestAct[3]
		{
			new PersonalQuestAct
			{
				Act = 1,
				ObjectiveName = "ObjectiveGodEmperorAct1",
				Chapter = 1,
				Trigger = PersonalQuestTrigger.VanillaObjectiveFinished,
				TriggerIds = new string[1] { "5d6fc7d9baa485142a19010710bc1177" },
				MarkerPointGuid = "40b6a5c759b584343987a1dfcf919a06"
			},
			new PersonalQuestAct
			{
				Act = 2,
				ObjectiveName = "ObjectiveGodEmperorAct2",
				Chapter = 2,
				Trigger = PersonalQuestTrigger.VanillaObjectiveCompleted,
				TriggerIds = new string[1] { "00cb8592e0ba7674c9dec3a60351cfd5" },
				MarkerPointGuid = "70990a0f7010447478e68b8e16ae52b2"
			},
			new PersonalQuestAct
			{
				Act = 3,
				ObjectiveName = "ObjectiveGodEmperorAct3",
				Chapter = 3,
				Trigger = PersonalQuestTrigger.Decree,
				TriggerIds = new string[1] { "ImperialDais" },
				Fallback = PersonalQuestTrigger.VanillaObjectiveCompleted,
				FallbackIds = new string[1] { "bcf5a9f544fd8bb46a1562dcde735893" },
				FallbackRequiresAutoCrusade = true,
				MarkerPointGuid = "70990a0f7010447478e68b8e16ae52b2"
			}
		}
	};

	internal static readonly PersonalQuestDefinition Overlord = new PersonalQuestDefinition
	{
		Id = "Overlord",
		ArchetypeToken = "Overlord",
		QuestName = "QuestOverlord",
		DormantObjectiveName = "ObjectiveOverlordDormant",
		RelicItemName = "ItemScepterOfGreatTomb",
		Acts = new PersonalQuestAct[3]
		{
			new PersonalQuestAct
			{
				Act = 1,
				ObjectiveName = "ObjectiveOverlordAct1",
				Chapter = 1,
				Trigger = PersonalQuestTrigger.VanillaObjectiveCompleted,
				TriggerIds = new string[1] { "1992ddc7d1b615748a0652a964846d4c" }
			},
			new PersonalQuestAct
			{
				Act = 2,
				ObjectiveName = "ObjectiveOverlordAct2",
				Chapter = 2,
				Trigger = PersonalQuestTrigger.Trial,
				TrialId = "Trial.Overlord.Act2",
				MarkerIsColiseumTile = true
			},
			new PersonalQuestAct
			{
				Act = 3,
				ObjectiveName = "ObjectiveOverlordAct3",
				Chapter = 3,
				Trigger = PersonalQuestTrigger.VanillaObjectiveCompleted,
				TriggerIds = new string[1] { "bcf5a9f544fd8bb46a1562dcde735893" },
				MarkerPointGuid = "70990a0f7010447478e68b8e16ae52b2"
			}
		}
	};

	internal static readonly PersonalQuestDefinition Devourer = new PersonalQuestDefinition
	{
		Id = "Devourer",
		ArchetypeToken = "Devourer",
		QuestName = "QuestDevourer",
		DormantObjectiveName = "ObjectiveDevourerDormant",
		RelicItemName = "ItemHeartOfBeelzebub",
		Acts = new PersonalQuestAct[3]
		{
			new PersonalQuestAct
			{
				Act = 1,
				ObjectiveName = "ObjectiveDevourerAct1",
				Chapter = 1,
				Trigger = PersonalQuestTrigger.Trial,
				TrialId = "Trial.Devourer.Act1",
				MarkerIsColiseumTile = true
			},
			new PersonalQuestAct
			{
				Act = 2,
				ObjectiveName = "ObjectiveDevourerAct2",
				Chapter = 2,
				Trigger = PersonalQuestTrigger.DevourerDivergence,
				MarkerIsColiseumTile = true
			},
			new PersonalQuestAct
			{
				Act = 3,
				ObjectiveName = "ObjectiveDevourerAct3",
				Chapter = 3,
				Trigger = PersonalQuestTrigger.Trial,
				TrialId = "Trial.Devourer.Act3",
				MarkerIsColiseumTile = true
			}
		}
	};

	internal const string DevourerAct3VoidObjectiveName = "ObjectiveDevourerAct3Void";

	internal static readonly PersonalQuestDefinition ShadowMonarch = new PersonalQuestDefinition
	{
		Id = "ShadowMonarch",
		ArchetypeToken = "ShadowMonarch",
		QuestName = "QuestShadowMonarch",
		DormantObjectiveName = "ObjectiveShadowMonarchDormant",
		RelicItemName = "ItemRingOfMonarchShadow",
		Acts = new PersonalQuestAct[3]
		{
			new PersonalQuestAct
			{
				Act = 1,
				ObjectiveName = "ObjectiveShadowMonarchAct1",
				Chapter = 1,
				Trigger = PersonalQuestTrigger.VanillaObjectiveCompleted,
				TriggerIds = new string[1] { "388790cbbec808e4487f547e7d805cc9" },
				MarkerPointGuid = "556d74cd8f75e674981862b10a84fa70"
			},
			new PersonalQuestAct
			{
				Act = 2,
				ObjectiveName = "ObjectiveShadowMonarchAct2",
				Chapter = 2,
				Trigger = PersonalQuestTrigger.Trial,
				TrialId = "Trial.ShadowMonarch.Act2",
				MarkerIsColiseumTile = true
			},
			new PersonalQuestAct
			{
				Act = 3,
				ObjectiveName = "ObjectiveShadowMonarchAct3",
				Chapter = 3,
				Trigger = PersonalQuestTrigger.Trial,
				TrialId = "Trial.ShadowMonarch.Act3",
				MarkerIsColiseumTile = true
			}
		}
	};

	internal static readonly PersonalQuestDefinition Hero = new PersonalQuestDefinition
	{
		Id = "Hero",
		ArchetypeToken = "Hero",
		QuestName = "QuestHero",
		DormantObjectiveName = "ObjectiveHeroDormant",
		RelicItemName = "ItemMantleOfTrueSavior",
		Acts = new PersonalQuestAct[3]
		{
			new PersonalQuestAct
			{
				Act = 1,
				ObjectiveName = "ObjectiveHeroAct1",
				Chapter = 1,
				Trigger = PersonalQuestTrigger.VanillaObjectiveFinished,
				TriggerIds = new string[1] { "5b8cb2c804cc9b74f99f6f640955e381" },
				MarkerPointGuid = "40b6a5c759b584343987a1dfcf919a06"
			},
			new PersonalQuestAct
			{
				Act = 2,
				ObjectiveName = "ObjectiveHeroAct2",
				Chapter = 2,
				Trigger = PersonalQuestTrigger.VanillaObjectiveCompleted,
				TriggerIds = new string[1] { "c53e98fd90a8cd8438c0b364dd8ef67d" },
				MarkerPointGuid = "70990a0f7010447478e68b8e16ae52b2"
			},
			new PersonalQuestAct
			{
				Act = 3,
				ObjectiveName = "ObjectiveHeroAct3",
				Chapter = 3,
				Trigger = PersonalQuestTrigger.VanillaObjectiveCompleted,
				TriggerIds = new string[1] { "bcf5a9f544fd8bb46a1562dcde735893" },
				BonusObjectiveId = "659bfbb92c0c17e429a89196196ef85f",
				MarkerPointGuid = "70990a0f7010447478e68b8e16ae52b2"
			}
		}
	};

	internal static readonly PersonalQuestDefinition Mastermind = new PersonalQuestDefinition
	{
		Id = "Mastermind",
		ArchetypeToken = "Mastermind",
		QuestName = "QuestMastermind",
		DormantObjectiveName = "ObjectiveMastermindDormant",
		RelicItemName = "ItemTomeOfInfiniteContingencies",
		Acts = new PersonalQuestAct[3]
		{
			new PersonalQuestAct
			{
				Act = 1,
				ObjectiveName = "ObjectiveMastermindAct1",
				Chapter = 1,
				Trigger = PersonalQuestTrigger.VanillaObjectiveCompleted,
				TriggerIds = new string[1] { "a5f23dc3dd2747f4fb8034315c7e921d" },
				MarkerPointGuid = "40b6a5c759b584343987a1dfcf919a06"
			},
			new PersonalQuestAct
			{
				Act = 2,
				ObjectiveName = "ObjectiveMastermindAct2",
				Chapter = 2,
				Trigger = PersonalQuestTrigger.VanillaObjectiveCompleted,
				TriggerIds = new string[1] { "f2e6d4a07ccf9314190cf1ddee028513" },
				MarkerPointGuid = "70990a0f7010447478e68b8e16ae52b2"
			},
			new PersonalQuestAct
			{
				Act = 3,
				ObjectiveName = "ObjectiveMastermindAct3",
				Chapter = 3,
				Trigger = PersonalQuestTrigger.VanillaObjectiveCompleted,
				TriggerIds = new string[1] { "d44f91b07f9914349aa0b6c082d98c25" },
				MarkerPointGuid = "669220945fa20fa4a9317dea3618a19c"
			}
		}
	};

	internal static readonly PersonalQuestDefinition[] All = new PersonalQuestDefinition[8] { Nostalgia, PhantomBlade, GodEmperor, Overlord, Devourer, ShadowMonarch, Hero, Mastermind };

	internal static IEnumerable<PersonalQuestDefinition> ArchetypeQuests => All.Where((PersonalQuestDefinition q) => q.ArchetypeToken != null);

	internal static PersonalQuestDefinition ById(string id)
	{
		if (!string.IsNullOrEmpty(id))
		{
			return All.FirstOrDefault((PersonalQuestDefinition q) => string.Equals(q.Id, id, StringComparison.Ordinal));
		}
		return null;
	}

	internal static PersonalQuestDefinition ForArchetypeToken(string token)
	{
		string normalized = ArchetypeOrigin.Normalize(token);
		if (normalized.Length == 0)
		{
			return null;
		}
		return ArchetypeQuests.FirstOrDefault((PersonalQuestDefinition q) => string.Equals(q.ArchetypeToken, normalized, StringComparison.Ordinal));
	}

	internal static PersonalQuestAct ActForTrial(string trialId)
	{
		if (string.IsNullOrEmpty(trialId))
		{
			return null;
		}
		return All.SelectMany((PersonalQuestDefinition q) => q.Acts).FirstOrDefault((PersonalQuestAct a) => string.Equals(a.TrialId, trialId, StringComparison.Ordinal));
	}

	internal static PersonalQuestDefinition QuestForTrial(string trialId)
	{
		if (string.IsNullOrEmpty(trialId))
		{
			return null;
		}
		return All.FirstOrDefault((PersonalQuestDefinition q) => q.Acts.Any((PersonalQuestAct a) => string.Equals(a.TrialId, trialId, StringComparison.Ordinal)));
	}
}
