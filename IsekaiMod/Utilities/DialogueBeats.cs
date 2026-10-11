using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IsekaiMod.Utilities;

internal static class DialogueBeats
{
	internal static readonly GraftBeat HulrunPraysOverYou = GraftBeat.At("HulrunPraysOverYou", "87997a477e6a58d4aae46e26a3712825", "Cue_0013, Hulrun's prayer over the dying stranger, is the only cue that presents this list.", "0c1d6c7597b735b49b41ab61008e731d");

	internal static readonly GraftBeat HulrunAsksWhoYouAre = GraftBeat.At("HulrunAsksWhoYouAre", "e27807b731f3b1a4eb19c1a04fdfcf53", "Cue_0019, Hulrun asking who you are, is the only cue that presents this list.", "393f7d6b356c11347853c2a8d32b8dd3");

	internal static readonly GraftBeat TerendelevHasHealedYou = GraftBeat.At("TerendelevHasHealedYou", "0f7dddeb3f77a4f408e7dc843b9c66fb", "The answers thank Terendelev for healing you: Cue_0033 where she does, and her replies Cue_0038, 0039, 0040 and 0056 in the same exchange.", "b50327805d1073c478ed9ceb4689d382", "70cd3ebac6e5daa41b949c0347b6ba57", "f5651757bd720de4b98b5893152d504a", "d84fcf0f47de81640bdd72e410b50cef", "51db8a3fa24071146b67e6ef289f41ed");

	internal static readonly GraftBeat StitchOffersTheCrossbow = GraftBeat.At("StitchOffersTheCrossbow", "a3e8939fe8354cb4698403e4256fe0d9", "Every presenting cue re-offers the same crossbow-or-draught choice the answer takes both of.", "b2621de772e925d48b230b12a789a441", "8fac96faed6cb4746b592f802a86d25a", "9985c2a0a8645724fa286072d95a7b8f", "5ff363dc3636d0749b262d59167f6a2f");

	internal static readonly GraftBeat CamelliaIntroducesHerself = GraftBeat.At("CamelliaIntroducesHerself", "1ca6cf08fceeac141a0df689cecc784a", "Cue_0008, where Camellia introduces herself after the fall; not Anevia's Cue_0017 about the dead man or Camellia's later answers.", "e6ba26401a42d994e8906e2f867740d8");

	internal static readonly GraftBeat SeelahAsksForHelpWithTheBoulder = GraftBeat.At("SeelahAsksForHelpWithTheBoulder", "a55fc20c6f0ff56439b40d6ba53cb8d7", "Both presenting cues are Seelah asking you to help lift the boulder off Anevia: Cue_0004 when she spots you, Cue_0009 when she insists.", "159c4442a8672334c9394d9579cfd8f9", "72bdfcfe6feac1743986a42b0f0f4b46");

	internal static readonly GraftBeat AneviaAsksWhoYouAre = GraftBeat.At("AneviaAsksWhoYouAre", "da6ca50574b6c714f9166f37b64b7b59", "Cue_0017, Anevia under the rubble, is the only cue that presents this list.", "843c7cf25f27714439e8dbb6cab06074");

	internal static readonly GraftBeat LannAndWenduagFirstSighting = GraftBeat.At("LannAndWenduagFirstSighting", "61ba231b3ad6b144a918c88ca49cb92a", "The first sighting: Cue_0007 (Lann stops Wenduag) and Cue_0067 (you size up the strangers). Not the eleven later cues, where Lann has already introduced himself.", "f271dd92d1bbcfd4791957f2bdde2a7b", "4b94b19a33a090d49aac4dbafc77774c");

	internal static readonly GraftBeat WenduagAsksYouToHideTheLight = GraftBeat.At("WenduagAsksYouToHideTheLight", "c4d71c5f339b4564da1f4a86d323249e", "Every presenting cue is Wenduag, alone with you, pleading to keep the light from Sull.", "cb6bd6faeac5edb488cb0082608e8913", "c1f448ce05289094da1a0aff27ecf474", "870394880ee629b438492bdaf267abbf", "c52a7107c2e021d438839b254a4f7911");

	internal static readonly GraftBeat SullAsksForTheLight = GraftBeat.At("SullAsksForTheLight", "5233c72929cda7c48b92ec6292a730ac", "The answer stands where the vanilla reveal stands, and that answer is offered at all three presenting cues.", "01f6b764b9f70624c910e705de0aa919", "3d7b7b187ea40ea4d8cbe77336093ef1", "5ebaa512bcceb214498a41513a3aa28d");

	internal static readonly GraftBeat HorgusAndAneviaBicker = GraftBeat.At("HorgusAndAneviaBicker", "65395d8277d3b9b4f82f068616de8a56", "The cues where the two are bickering (Cue_0004 and Cue_0032, and Anevia's complaint Cue_0005), not their later answers about the city.", "a94c5691bb3ea794e9dc73c6843cf393", "3103585f1550def4c98fbc731ccb3ebc", "b8f381e3ce934664192f2ab5c3f03707");

	internal static readonly GraftBeat HorgusNamesHisPrice = GraftBeat.At("HorgusNamesHisPrice", "12e42316950f8c9498afb8b0fb2baaae", "The haggle answers the thousand-gold offer: Cue_0008 where he makes it and Cue_0033/0034 where he repeats it. Not the cues where a price is already agreed; Camellia's aside Cue_0027 is reached only as Cue_0008's continuation.", "dc503a97576443e42b48d894e9ee636f", "53eb8d487c4b03444976a72d767b7881", "40b81ea4273fd1a4db2890bcf4159a2c");

	internal static readonly GraftBeat HorgusAgreesToTheEscort = GraftBeat.At("HorgusAgreesToTheEscort", "a6b5ef97dca04894a8cc7984bff5a592", "Cue_0018 (\"Splendid. In the meantime I shall sit here\"), where the escort has just been agreed; not the openers Cue_0001, 0006, 0020 and 0022, before he has proposed it.", "4433e95fbcb4b914c870cadd09f45087");

	internal static readonly GraftBeat RadianceIsFound = GraftBeat.At("RadianceIsFound", "d7669703a6d923d4e8b34bc56ec16c31", "The moment the sword is in hand: Cue_0003 (it greets you) and Cue_0010 (it once glowed). Not Seelah's stories about Yaniel.", "413779fe71c54d7295cb327bff8dcef9", "fb9461af4e4bd0b4b81ebb102f001039");

	internal static readonly GraftBeat SavamelehVision = GraftBeat.At("SavamelehVision", "d35d5c76186af32468dc71952eb339ec", "Cue_0012 is the only cue that presents this list.", "53aeaacb3a7f1f6418b0154381643425");

	internal static readonly GraftBeat LannOrWenduag = GraftBeat.At("LannOrWenduag", "b709ea04dadce5848b4e033eed7d0851", "Every presenting cue re-asks the one choice between Lann and Wenduag, which the answer refuses to make.", "a42242760ff0d9b4d8b9e5b04071926e", "dbd001b00a212ae4d91476c0da5a3b44", "4ae12ef3eec235043b5216ed8c0c6df4", "f1ec26706687a93428b6f8234fa81a65", "2ec2ea0b25bda6045be8bb4d3146b2a3", "91b2e97b956dc474f8b3d41572118a0d", "a3470bf8a77220b49858e385be7db698", "e54bde5901ac4dd4c956de790aa6f30b");

	internal static readonly GraftBeat LannOrWenduagDecision = GraftBeat.At("LannOrWenduagDecision", "9ef32524f96c48e489e69afeff91b7d8", "Cue_0092, Camellia's \"why not join forces with the strongest of them\", is the one cue that puts the choice. The list's other two cues (Cue_0038, Cue_0100) follow a choice already made.", "96fb512b04525c54abf8d20e8078c61d");

	internal static readonly GraftBeat WoundedWenduagOrLann = GraftBeat.At("WoundedWenduagOrLann", "e02baf0076ae8e9499fe1a1f8e89f05c", "Every presenting cue (Cue_0067, 0085, 0102, 0108, 0127, 0133, 0148) is Lann and the wounded Wenduag arguing over which of them you keep.", "df83d8c582058f54384ad8c75b5ff63d", "317aa63b83cd6c942a524ff86578bb08", "690ff79a60ef6ca4d996722af5450818", "7dbc9999fa5fccf4a90e1003ba9ff40f", "7ca9ffe07cf1de941a64952b50421843", "2ab80e4afdf8d174385e32ebd04b5a69", "255688167df43d749a8bb542cc739bfa");

	internal static readonly GraftBeat IrabethNamesMinagho = GraftBeat.At("IrabethNamesMinagho", "5acd8001d9f7d2443bd57fb1291a03e4", "Cue_0019, Irabeth naming Minagho, is the only cue that presents this list.", "d25642904519934498ad096797f7dfb5");

	internal static readonly GraftBeat IrabethDefendersHeartCouncil = GraftBeat.At("IrabethDefendersHeartCouncil", "871af36f2ab2b1f40b5de77976c54276", "The chapter 1 council at the Defender's Heart: her openers Cue_0002 and Cue_0011 and the defence planning Cue_0119 and Cue_0126. Not her grief, her sword or later chapters.", "0a6707b8ace5e9445ab98a4d1f49e2dc", "76e6f2f07048359458a4b61d78db19cf", "398f87e6c05275d46ae05cb42caed5cf", "bd2d2e0f6807f7e47849c68e46e5ca13");

	internal static readonly GraftBeat WardstoneTitanFeeling = GraftBeat.At("WardstoneTitanFeeling", "be1fcfe08bbae9a46b5105c4d1e4be7c", "Cue_0003, where you feel like a titan facing an ant, is the only cue that presents this list.", "ad62f364f16c1d646b94240761a6b320");

	internal static readonly GraftBeat DesnaAdeptsAskAboutTheSong = GraftBeat.At("DesnaAdeptsAskAboutTheSong", "16d9e0a48d5746a4fbb1497e4ee4cf81", "Cue_0002, the adepts asking about the light and the music, is the only cue that presents this list.", "685f2ceb822f0d34bae97b29d71bdfd0");

	internal static readonly GraftBeat QueenGalfreyForces = GraftBeat.At("QueenGalfreyForces", "228936fbcc45a42479c63982cc4c1e3f", "The levy answers the talk of forces: Cue_0070 (\"I shall answer\"), Cue_0026 (the forces are not enough) and Cue_0017 (the march on Drezen). Not Staunton, the tribunal or the Worldwound.", "5ae642b53303fa6439f1553c366e193e", "0f0fa8c84f8ec114eabfbdbab231296c", "8f31622770f3cbe459148d57931fd6e0");

	internal static readonly GraftBeat RegillSendsYakerOff = GraftBeat.At("RegillSendsYakerOff", "5128a604b13cf554a846ab7c16054cbf", "Cue_0004 is the only cue that presents this list.", "f8a96c186e8c1134a9699748043b3af3");

	internal static readonly GraftBeat ZachariusDemandsHisWand = GraftBeat.At("ZachariusDemandsHisWand", "16f23cbdc188b3348a47531d2bb747c2", "The lich asking for his wand: Cue_0003, Cue_0011 and Cue_0033. Not the opening narration or his later offer.", "c4234241f1b20a94d971b40b0db56138", "59b15dc28c77feb44b32b43a0c1d5be0", "e5f17c36d3e3cf34596747c779a50ea6");

	internal static readonly GraftBeat GalfreyPraisesDrezen = GraftBeat.At("GalfreyPraisesDrezen", "44704bddb6223b84989dd26bcf20b601", "A proclamation of what Drezen now is answers the Queen speaking of it: Cue_0047 (Drezen as a beacon) and Cue_0018 (the good news of the crusade's victories).", "c4f08b9610c561d48aed68c678c7a5e6", "17c3d77611ded3c44ac2907ccc014e01");

	internal static readonly GraftBeat GalfreyVisitsDrezen = GraftBeat.At("GalfreyVisitsDrezen", "44704bddb6223b84989dd26bcf20b601", "The Queen's own lines of the visit (Cue_0011, 0017, 0018, 0047), not the narration, the Lexicon or the ready-to-go prompt.", "d914281983d818b448bb3f1ef7f5a04e", "9ac59e2b45633d342bc588ba8bb72ea5", "17c3d77611ded3c44ac2907ccc014e01", "c4f08b9610c561d48aed68c678c7a5e6");

	internal static readonly GraftBeat SutureOffersTheLexicon = GraftBeat.At("SutureOffersTheLexicon", "cd9c9facc3a8ded4e9683cde8958295e", "Cue_0002, Suture offering the book. Not Cue_0052, his reply once it is taken.", "a0911bf891e2e9547adb9791282ecf20");

	internal static readonly GraftBeat BaphometTaunts = GraftBeat.At("BaphometTaunts", "9ab9ad8f6e11d67499b48fd595e50972", "Every cue Baphomet himself speaks that presents the list (Cue_0011, 0009, 0007); not the two narrations.", "92fdf8c003a85ce44bde44bd02d6e890", "c08463c6b862b6345b8c2f51cb469cb9", "c20268a62ce4e384289c13c7370b5fb4");

	internal static readonly GraftBeat NocticulaFirstAudience = GraftBeat.At("NocticulaFirstAudience", "75678bafb5e33854baaee17ee6eaf69c", "The first audience: Cue_0409 (interrogate me in my own city), Cue_0051 (indulge one's curiosity) and Cue_0113 (her intrigue). The answers are also hidden once Cue_0001, the return-visit greeting, has been seen.", "20fca5c3eddc1d241a022bdfc99831b0", "38716d11762a4574298b4457d8d3f7ea", "39c6a2b6323ec3f4aaa6922ced6838aa");

	internal static readonly GraftBeat GoddessesAwaitTheDecision = GraftBeat.At("GoddessesAwaitTheDecision", "468e0a75def68ec4096680a92b1591b1", "The three cues where both wait for the Commander's decision (Cue_0062, 0083, 0105). Not the swarm's silence, Cue_0367.", "90a4e1713dcf97a4d8b675e917d10ba2", "f467058d431cac546b178222355706f5", "87b3a18d5f51d384cb4949f4fe017764");

	internal static readonly GraftBeat DecisionAtTheWound = GraftBeat.At("DecisionAtTheWound", "294126e3264796e488ce19bfb1851355", "Every presenting cue is one mythic path's arrival at the same decision, and an ending is declared before that decision on any path.", "55f2fea01dd1cb549a0c5602dc5c93d7", "c2121ad794b31c245b66821ab3b2775f", "c7c462d94cc5edf468cd2e0966d4738e", "8f0eca90e2bc31b408b2e9a8dccbe99d", "834ba1f7f5e94f342b91da0ed9856991", "6a009eb116f2351458ee29e498a7eb39", "ec6f5ea3516d6ab498a80100c86ae76e", "dcb3c1bb20eab174aac6653c131304dc", "c107c552aceae384787d6bc73bbf49d1");

	internal static readonly GraftBeat DrowDisguisesDrop = GraftBeat.At("DrowDisguisesDrop", "b8e680587a76f064fac9a01034c02391", "The disguises dropping (Cue_0061) and the two drow who played Tran and Asty (Cue_0069, Cue_0070).", "93fd2ebf397d21d40ab5c765e2659cf4", "f3be06a404054bb4394c59ef10ce6935", "716d0149cc5f6844a8743062590acb36");

	internal static readonly GraftBeat EmptinessAsksWhoYouAre = GraftBeat.At("EmptinessAsksWhoYouAre", "db068bf5b388cce4c9f828d389ca537d", "All four presenting cues are the emptiness asking, and re-asking, its one question.", "92d035b546d4d054681d7243f931dc5f", "3ccd67724683a5646a34ad6c07e640eb", "0e28e0f9f03b32c4fb345031a6a123c8", "721826ea9d67f5c44ae7d966acc4432e");

	internal static readonly GraftBeat CamelliaMorningAfter = GraftBeat.At("CamelliaMorningAfter", "0816cfae5780af142be53f1d296a8308", "Both presenting cues are Camellia, flustered, the morning after the date.", "0bca134d815b8ac4aa0da51f0a8e57c1", "08d711e573356c1458ba12db6a215ac8");

	internal static readonly GraftBeat UlbrigInTheGardenOfGods = GraftBeat.At("UlbrigInTheGardenOfGods", "3c515b7b090043479164dac9160b6c9e", "All four presenting cues are Ulbrig marvelling at the Garden of the Gods.", "b89376c8ecd3467cad5d46e1261ff25a", "f92e59346a8c49f98a48c2c7ea0a6dd1", "46545cac9a7c4247a3b3646d4fc52fab", "219fc351752c4cc286826638b69ad310");

	internal static readonly GraftBeat UlbrigGardenDate = GraftBeat.At("UlbrigGardenDate", "249454ec180b44759d3c9e8e47eef6cb", "Cue_2 is the only cue that presents this list.", "bdf3805a03f14d06a6070c1294660811");

	internal static readonly GraftBeat RegillHub = GraftBeat.HubOf("RegillHub", "2366a8db6481070439fee222c0c52e45", "Regill's companion hub; the retinue question is a topic, not a reply to any one line.");

	internal static readonly GraftBeat ArueshalaeHub = GraftBeat.HubOf("ArueshalaeHub", "03ebad9587cbea0438d901a0f8df44f1", "Arueshalae's companion hub; the retinue and romance questions are topics, not replies to any one line.");

	internal static readonly GraftBeat DaeranHub = GraftBeat.HubOf("DaeranHub", "4d978cbd2aa780d46874255282039f3f", "Daeran's companion hub; the retinue and romance questions are topics, not replies to any one line.");

	internal static readonly GraftBeat WenduagHub = GraftBeat.HubOf("WenduagHub", "ced27e744d2dded40bbb5adf17816dbb", "Wenduag's companion hub; the retinue question is a topic, not a reply to any one line.");

	internal static readonly GraftBeat WenduagQuestionsHub = GraftBeat.HubOf("WenduagQuestionsHub", "40a542a78c88e684b9c8908eafa41956", "Wenduag's \"ask me anything\" list; the romance question is a topic, not a reply to any one line.");

	internal static readonly GraftBeat SeelahHub = GraftBeat.HubOf("SeelahHub", "417fa384f3250634bb71859fbc913453", "Seelah's companion hub; the retinue question is a topic, not a reply to any one line.");

	internal static readonly GraftBeat CamelliaHub = GraftBeat.HubOf("CamelliaHub", "9497b573bde03724c8e08090509a1774", "Camellia's companion hub; the retinue question is a topic, not a reply to any one line.");

	internal static readonly GraftBeat LannHub = GraftBeat.HubOf("LannHub", "66385ad77fa743e4bb1234078dbd804c", "Lann's companion hub; the retinue and romance questions are topics, not replies to any one line.");

	internal static readonly GraftBeat SosielHub = GraftBeat.HubOf("SosielHub", "129b55b8b5d50974f84f7c607d894fd0", "Sosiel's companion hub; the romance question is a topic, not a reply to any one line.");

	internal static readonly GraftBeat FinneanHub = GraftBeat.HubOf("FinneanHub", "615ef80243cfc184ba42286395880b0e", "Finnean's companion hub; the question about exotic forms is a topic, not a reply to any one line.");

	internal static readonly GraftBeat GalfreyIncognitoHub = GraftBeat.HubOf("GalfreyIncognitoHub", "f1225677b6e455a4db45f2ce77533816", "The incognito Queen's question hub at the war camp; the personal question is a topic like her other personal questions.");

	internal static readonly GraftBeat DyraHub = GraftBeat.HubOf("DyraHub", "4b2c98a24f2bb304f8ee50286eec3f90", "Dyra's vendor hub; the appraisal is a topic, not a reply to any one line.");

	internal static readonly GraftBeat JoranHub = GraftBeat.HubOf("JoranHub", "38adb0a43a5ac3043ad0fe59fdcba6b8", "Joran's hub at the Defender's Heart; the party noticing the humming is not a reply to any one line.");

	internal static readonly GraftBeat GemylHub = GraftBeat.HubOf("GemylHub", "6c6ca2d78b121504ca4bedf0a1c6f07f", "Gemyl's bar hub at the Defender's Heart; the party noticing the humming is not a reply to any one line.");

	internal static readonly GraftBeat JernaughHub = GraftBeat.HubOf("JernaughHub", "e9355a3b814dbc24ea94193b093ba8e1", "Jernaugh's war camp hub; the party noticing the rift is not a reply to any one line.");

	internal static readonly GraftBeat GalfreyBriefsTheDrezenCampaign = GraftBeat.At("GalfreyBriefsTheDrezenCampaign", "228936fbcc45a42479c63982cc4c1e3f", "The Queen, alone with the Commander in her tent, sets out the march on Drezen: Cue_0017 (attack swiftly, the Sword of Valor) and Cue_0026 (the forces are not enough, hire more). Not the Staunton lines on the same list.", "8f31622770f3cbe459148d57931fd6e0", "0f0fa8c84f8ec114eabfbdbab231296c");

	internal static readonly GraftBeat YakerAsksForRelief = GraftBeat.At("YakerAsksForRelief", "daf5bff8e63f68f49a8f0fc173ae437b", "Yaker has described the flying attackers (Cue_0054) and Irabeth has weighed the detour to his squad (Cue_0056); not the heresy debate or the companions' suspicion on the same list.", "392c31226a464984f932e43ec3e9c159", "bcaea07c2e8d5b443b0544bb614ca5bf");

	internal static readonly GraftBeat RegillExplainsTheWounded = GraftBeat.At("RegillExplainsTheWounded", "ab4c009b7ff77da4fad0abe34ad79c08", "Cue_0006, Regill explaining why his wounded were finished off; the answers all answer that order.", "4e6c1132b4029fd4e9976f6102387b0f");

	internal static readonly GraftBeat GargoylesNamedInTheNight = GraftBeat.At("GargoylesNamedInTheNight", "280221f20f506b9409275949d588eb1e", "Cue_0011, the narration that names the attackers as gargoyles; the answers name them or their stone hide.", "2e820fd4b1f26a44e9697e1a6ef646b0");

	internal static readonly GraftBeat CampUnderAttackAtNight = GraftBeat.At("CampUnderAttackAtNight", "280221f20f506b9409275949d588eb1e", "The camp is burning and the wounded need defending: Anevia's report (Cue_0004) and both narrations of what is attacking (Cue_0010, Cue_0011).", "f043e7dedcacc6042a580b335ec52eb6", "84f7e6a4d7c4c154f9d62d12480934fc", "2e820fd4b1f26a44e9697e1a6ef646b0");

	internal static readonly GraftBeat NurahExcusesThePerfume = GraftBeat.At("NurahExcusesThePerfume", "16ccf8f56019e2c4f92b1caec3864b5d", "Cue_0082, Nurah blaming a secret admirer for the pheromone perfume found in her tent after the vescavor swarm.", "ff2fad83b58a99c46afe077c31bea743");

	internal static readonly GraftBeat NurahExcusesTheEvidence = GraftBeat.At("NurahExcusesTheEvidence", "16ccf8f56019e2c4f92b1caec3864b5d", "Nurah's two excuses once the evidence is in: the perfume (Cue_0082) and the alchemist's fire by the Commander's tent (Cue_0083).", "ff2fad83b58a99c46afe077c31bea743", "a0877c849b4182648a2f4a20d6367f1b");

	internal static readonly GraftBeat NurahOffersHerHelp = GraftBeat.At("NurahOffersHerHelp", "16ccf8f56019e2c4f92b1caec3864b5d", "Nurah's eager greetings and small talk (Cue_0001, 0002, 0020, 0021), while she is still trusted; not the evidence excuses.", "8e82eba71e6a73245b82b6b5a6731803", "7eeb56466ce63e0439194cd6a7067e1a", "5f271d6cd1defa34291850866c0e5b6f", "03a7c8d63a7df0a43a27c20831506f44");

	internal static readonly GraftBeat GhoulAmbushRevealed = GraftBeat.At("GhoulAmbushRevealed", "6856a7d6e123a0c46b666e8052008405", "Cue_0003, the captive signalling that the corpses are ghouls lying in wait, is the only cue that presents this list.", "bd1f94f6be8ebbb4b843224673089d49");

	internal static readonly GraftBeat ChapelSurvivorsTended = GraftBeat.At("ChapelSurvivorsTended", "364877496cda83640a9663b45ed0fc10", "Cue_0018, Anevia tending the rescued soldiers with Irabeth asleep nearby: the rescue is over.", "8fef97b95ce3ff34f94aac9575071535");

	internal static readonly GraftBeat IrabethIsSafe = GraftBeat.At("IrabethIsSafe", "364877496cda83640a9663b45ed0fc10", "Irabeth is out of the chapel: Cue_0018 (the soldiers tended, Irabeth asleep) and Cue_0024 (she will be all right).", "8fef97b95ce3ff34f94aac9575071535", "341e44abe498866438f094211eda36d1");

	internal static readonly GraftBeat WilcerAtYourService = GraftBeat.At("WilcerAtYourService", "3c58e83a970a0f643a88e15f2323c805", "Wilcer's two greetings (Cue_0001, Cue_0008); not the later famine, wake or gift lines on the same hub.", "d2a00a409d4871546aa43aebded3997e", "f3882532d38a43746b4d9e1645f862c8");

	internal static readonly GraftBeat MaskedGhostAsks = GraftBeat.At("MaskedGhostAsks", "c2691373bfd094544a2d96578aae4f7c", "The four cues where the masked spirit offers its question (Cue_0002, 0003, 0004, 0005).", "0e6e342f1880c6340817583622713ab1", "5e411dff6d5001f42bd3cb85fc0bd0ef", "173a4caa035cc864799eccb3494c2e83", "2d35c81668faa4440b326bef03cc12ca");

	internal static readonly GraftBeat DrezenGateStandoff = GraftBeat.At("DrezenGateStandoff", "8b3bfe8703cffc44cb09cea58d8acc2c", "Every cue of the standoff at the gate before the fight (Cue_0005, 0011, 0020).", "3a530a95f173f2340ac968840240f108", "c2fb93ae52420de488d946e71f2adde9", "728503262e38563489951b94dda2691a");

	internal static readonly GraftBeat DrezenWarCouncil = GraftBeat.At("DrezenWarCouncil", "0c390d22c44f76c4aaa053ee987836d1", "Cue_0004, Anevia on the Sword of Valor over the city map, is the only cue that presents the plan list.", "365c47f2246f01042b3c33c044f1be92");

	internal static readonly GraftBeat StauntonAndMinaghoAtBay = GraftBeat.At("StauntonAndMinaghoAtBay", "c0a6d32e56866274bb86b0adcfab1dbe", "Staunton and Minagho cornered before the fight: his vow to fight to the end (Cue_0004), his defiance (Cue_0012), Minagho's taunt (Cue_0018) and his question (Cue_0025); not Nurah's lines.", "751800cd804ffc74ab1536c3671e9efc", "3d502107f3aa0bf4bade1dde8ddda9e1", "0c1e4fc35ecbaa9458582065fe9c4031", "c58271cd55602af42be045496f02565f");

	internal static readonly GraftBeat SosielSpeaksOfHisWork = GraftBeat.At("SosielSpeaksOfHisWork", "366eb018f3667a6479526a9416c3b412", "Sosiel on his calling and his painting (Cue_0018, 0032, 0036); not the grief for his fallen brethren (Cue_0031, 0035).", "2ae011ff331ef7644ab9e1be1f2f23eb", "d019813063af9a042aa8e47f98733907", "dc311097721c6634297759fea32bd572");

	internal static readonly GraftBeat SeelahToastsTheQueen = GraftBeat.At("SeelahToastsTheQueen", "3fafdb348338f5e4ebac24a8970494e7", "Cue_0007, Seelah caught with the flask toasting the Queen, is the only cue that presents this list.", "26e00513735edb349b969386382bb99b");

	internal static readonly GraftBeat RegillRebukesIrabeth = GraftBeat.At("RegillRebukesIrabeth", "2f4e364807b094a4c8fdd1f9f8cfb1dd", "Cue_0006, Regill's rebuke of the crusaders over the rescued Irabeth, is the only cue that presents this list.", "29de5792fc88a954cb73b3b7e4ed16ed");

	internal static readonly GraftBeat YanielTellsHerStory = GraftBeat.At("YanielTellsHerStory", "e47ef9ec640a1b4488ffb0c6a9983348", "The prisoner calling herself Yaniel telling her story: her name (Cue_0007), the demons' trophies (Cue_0032) and Staunton's fall (Cue_0034).", "d845d3c6fdba97240b8c9e580c8153a3", "1ed1497d5e772db43a3ec38194b7c0af", "d4451bbf8569620448ad92f03456088c");

	internal static readonly GraftBeat IrabethReportsOnDrezen = GraftBeat.At("IrabethReportsOnDrezen", "143d77815c045dd4abf5e81e948393b6", "Cue_0002, Irabeth ready to report on Drezen, is the only cue that presents this list.", "d342eac913fa5b7428f7bc8d5587d807");

	internal static readonly GraftBeat HeraldKneels = GraftBeat.At("HeraldKneels", "b9c56617dec2b7d42aea7b1c33fe5cbd", "Cue_0003, the Hand of the Inheritor kneeling to the Commander, is the only cue that presents this list.", "b7048d39e70c6444fa31162d1b3ace41");

	internal static readonly GraftBeat JerribethNamesHerself = GraftBeat.At("JerribethNamesHerself", "f240afdd8595d2340aa18ecf5ed233a3", "Jerribeth once she has given her name (Cue_0007) and explained Wintersun (Cue_0010, 0011, 0012); not her first disembodied question.", "a0b7a89d0ec7fa348bec5ffb9169dccd", "a657282ab833f5c42ac531d87908c9de", "b2997124f3662e147a029a6f73e13468", "e3b79793db1036445a404d82f1c28fcf");

	internal static readonly GraftBeat GreyborPlansTheAmbush = GraftBeat.At("GreyborPlansTheAmbush", "feac1861da9b5ae4b8e2629b84cac14e", "Every presenting cue is Greybor setting up the ambush for the dragon (Cue_0001, 0008, 0009).", "b5a40406d5d5a24438cfae531e4d46f6", "3f54bb3557fb62041854d66b7b702b02", "86dc9627068dee848a819600e55baab1");

	internal static readonly GraftBeat KyadoHidesTheCrypt = GraftBeat.At("KyadoHidesTheCrypt", "d7aadaa261a8023489bdba49216cc334", "Kyado hiding the crypt: his wary stammer (Cue_0004), his lie that all is quiet (Cue_0010) and his half-confession of Zanedra's threat (Cue_0082).", "e8cca55db5d7e904286db61c25a62b11", "0a07867040934b94eb4cdb75925fc4cd", "690750dd5bed20743890c085f94d61d7");

	internal static readonly GraftBeat ArueshalaeAtTheGrave = GraftBeat.At("ArueshalaeAtTheGrave", "6e5d9e9370306194ca15420193bae14d", "Arueshalae's guilt at the grave: the saved soul (Cue_0013), the hag's threat (Cue_0015), the bell she cannot touch (Cue_0017) and her lie (Cue_0020); not the map she hands over.", "3b7fd279bc1bd8e4fbab65b6d000c988", "2f4c222bc2c5c1b47a9e9226bb021e3b", "a4cacb75bc796e24980348f425c28cff", "b2f16a4e6a306b0488d9507367898e70");

	internal static readonly GraftBeat VorimeraakBoasts = GraftBeat.At("VorimeraakBoasts", "a01c59afcc9038c46a8bee2de45427cf", "The vrock screeching and bragging of her army (Cue_0001, 0011, 0013, 0025, 0026); not the page she holds or her suspicion.", "685f62f95dd7729468700c592c0dba50", "f568c5b2cb3a8be47868b2b5719c23a7", "123838979fe3e77429dacc50a1e8ee56", "3c8f6037bf290994ab4e031737b503b1", "77ae5e1e7f52bd443b5aa0ca69a59d2a");

	internal static readonly GraftBeat XanthirRailsAtHisServants = GraftBeat.At("XanthirRailsAtHisServants", "cbe3b0c3ea8b47045b66c085c1a1b354", "Cue_0002, Xanthir cursing his servants, is the only cue that presents this list.", "e3f6c92ac192c4144b00d32288e7d4ae");

	internal static readonly GraftBeat GalfreyPlansTheFane = GraftBeat.At("GalfreyPlansTheFane", "44704bddb6223b84989dd26bcf20b601", "The Queen on the assault on the Midnight Fane: why she came (Cue_0011), what her blade adds (Cue_0017) and the next step after Drezen (Cue_0048).", "d914281983d818b448bb3f1ef7f5a04e", "9ac59e2b45633d342bc588ba8bb72ea5", "7dc3b9ab6a64f03428b20eceabcee593");

	internal static readonly GraftBeat FaneThreshold = GraftBeat.At("FaneThreshold", "d8d8512aa3c48a342a68e9873f92e88e", "Every presenting cue is the Hand of the Inheritor at the threshold of the Fane before the battle.", "ce66fb41d3a254249bfe03dd8b6d141a", "53550bb0f435b974493d0aad907a6478", "83d380f65faae5d47a267f592c8e9ea8", "d04262327f8d2d5499ad859818402598", "e0e0af61d996b9e48b876c475d7280e4");

	internal static readonly GraftBeat FyeWelcomes = GraftBeat.At("FyeWelcomes", "9b15b09c244076047b02f317e55ef5e3", "Fye's greetings and his offer of wine, a meal or a talk (Cue_0002, 0003, 0004, 0017); not the stories and toasts on the same hub.", "f887a214c63db4b409881cd502c2b6c8", "876200f9ef6c37b4e95968de3876fb3e", "50bac7d9a8c729544b251b566b52de6f", "47ca16fe52f9ce441aac7910113f6ae4");

	internal static readonly GraftBeat StorytellerAsksForAStory = GraftBeat.At("StorytellerAsksForAStory", "2f5b7e0b76d3c5a42a431e1e33a8db09", "Cue_0002, the Storyteller asking whether you have brought him a new story.", "3653c46ce5a3b054e92bceed6469a780");

	internal static readonly GraftBeat CiarBeforeMorvegLeaves = GraftBeat.At("CiarBeforeMorvegLeaves", "76d61bf7eb5ba0c4087cf98ab106385c", "Ciar's greetings (Cue_0001, 0002) and his scolding of Morveg beside him (Cue_0067); not the lines once Morveg is gone.", "bed94afb552f78d4fb8041db52b420e1", "0de7fb1123ba98a42abfa277f8ceb906", "c257a21ac349d764ab9431cd194c3b95");

	internal static readonly GraftBeat ArsinoeGreets = GraftBeat.At("ArsinoeGreets", "ecaf5cfe8087a4f45a2269974f4885c9", "Arsinoe's introduction and her 'How can I help you?' (Cue_0001, 0002).", "0e83a74392bf2bb4b9a7db87b6e158ea", "19a97a51ab07206448f2409d3a1a6c71");

	internal static readonly GraftBeat JoranAtTheForge = GraftBeat.At("JoranAtTheForge", "23ab8e982ccf3d348a486db7488ddd3d", "Joran at the forge offering his work (Cue_0001, 0002).", "3499ee95dc3a72e4d95ac437fd981ca5", "e54ba401eef0a3c4da4db089132afa8c");

	internal static readonly GraftBeat JannahExplainsHerFlight = GraftBeat.At("JannahExplainsHerFlight", "b2b77a591c8808f409aa2083bd1b318a", "Jannah once she has said how she ran (Cue_0002), what she saw (Cue_0005) or that she deserted (Cue_0059).", "ef6fcb8d9315069458187e9b546844e9", "fe9fa40a86e52364bb3a5ad5d58b6166", "8c909dc650b9dff408a0d144da24b920");

	internal static readonly GraftBeat WintersunGateWarning = GraftBeat.At("WintersunGateWarning", "ee598777c32735a4eb1d19f7763a6382", "The gate guard's account of Wintersun, its chief and its Lady of the Sun, told to a traveler (Cue_0040) or to a monstrous one (Cue_0018).", "88c24ba6220cb54469cdc030833ff5d1", "af7b410b41fde174883be09dc5ce7b90");

	internal static readonly GraftBeat WoljifHub = GraftBeat.HubOf("WoljifHub", "e41585da330233143b34ef64d7d62d69", "Woljif's companion hub; the franchise and lock answers are topics, not replies to any one line.");

	internal static readonly GraftBeat EmberHub = GraftBeat.HubOf("EmberHub", "f2a35965e9bc601449498bd022b04d9d", "Ember's companion hub; the cake and the vow are topics, not replies to any one line.");

	internal static readonly GraftBeat NenioHub = GraftBeat.HubOf("NenioHub", "9b38499627b7eda4f86d959f3f222834", "Nenio's companion hub; peer review and her filing are topics, not replies to any one line.");

	internal static readonly GraftBeat GreyborHub = GraftBeat.HubOf("GreyborHub", "174d6c94b6725f44aad1d2a76993a926", "Greybor's companion hub; his rates and his axes are topics, not replies to any one line.");

	internal static readonly GraftBeat WoljifInChains = GraftBeat.At("WoljifInChains", "a9e697b9ad6fa1c4d9c2ae0895bd56d2", "Woljif's own lines while chained (Cue_0010, 0012, 0019, 0022, 0029): not his taunt at Delvan before you arrive, Delvan's Cue_0014, or his sulk after you refuse him.", "c2ef6310ae72e554b9eeed3ffa410443", "310a8b4cc1373da4ab82a68a2f5be34d", "24e74e098e8eb234b80034ec77c76392", "0aa1ce9e7a64824478e44a296ac0eda1", "6477271838812254a9d333a763fda8fe");

	internal static readonly GraftBeat FaxonCornered = GraftBeat.At("FaxonCornered", "0637eb6a25ff1f34c8cc800e53604705", "Every presenting cue is the standoff with Faxon's gang (Cue_0013, 0014 and 0023, the demons baring their teeth).", "98b9c03dd98aafe419d8356ffff3a94e", "9c4d903f050ddb74dbb64dea3a79d764", "d38ffac54b0298d4d8bc86f5ca7a277e");

	internal static readonly GraftBeat DaeranAsksAboutTheDemons = GraftBeat.At("DaeranAsksAboutTheDemons", "583314478921a19419304d5f83b72109", "Cue_0008, the count asking how the demons got into his soiree, is the only cue that presents this list.", "94107dccc6c6c83419aaa2b64bd833df");

	internal static readonly GraftBeat KnightsAboutToSacrificeEmber = GraftBeat.At("KnightsAboutToSacrificeEmber", "4bba85261e5f2064989ebdc878c0228a", "Cue_0006, Ember smiling at the knights about to kill her, is the only cue that presents this list.", "19c90cd21e5f88443b48432912e1f556");

	internal static readonly GraftBeat ChalebAtThePyre = GraftBeat.At("ChalebAtThePyre", "497b79c111a66034c920842d33b9ce71", "Chaleb's own lines at the pyre and what you notice about him (Cue_0002, 0004, 0006, 0018, 0021, 0022). Not Cue_0019, where his men prove to be real crusaders, the Trickster's instinct Cue_0032, or the empty return Cue_0112.", "0af80e74856d4c92942a6d9addce4824", "df1355626f7fb8842aba68054de6635f", "3578c3f43a66b524e99ea0793969d002", "7ea9ca74b9fb5b948b4d77ca41e1a710", "e3f2f1a0d8d235341a9865e422864f9e", "5f1cd627ba6276d4faf3ea7db2bb724d");

	internal static readonly GraftBeat ActorsUnderTheJunk = GraftBeat.At("ActorsUnderTheJunk", "833a8b5997b48b645b386b4fcb518134", "Every presenting cue is the actors still under the pile (Cue_0002, 0013, 0015).", "2cc07793c6ba34540be54ebace0e01f5", "110ecf21b0c053740b1fa029c22946e1", "50b47b2932aa46245a504a5801651d65");

	internal static readonly GraftBeat TheatreTroupeFreed = GraftBeat.At("TheatreTroupeFreed", "32a8b708ceab50b49ab5696300a893cf", "Grandma Gretlen's lines once the troupe is out (Cue_0019, 0020, 0025, 0027), where they say where they will go.", "20879962b391a3d449ed7b5576848d1d", "eff9c0a79119fc347ab6893895a2a8a0", "67c49a6630d2a954bbc50100ea63786a", "f1dee9c9af9534044b7d40024315e7ca");

	internal static readonly GraftBeat HulrunTurnsFromRamien = GraftBeat.At("HulrunTurnsFromRamien", "cc0bf28eab0e60b429dac7b53ec8e4d2", "Cue_0004, Hulrun breaking off his quarrel with Ramien to notice you, is the only cue that presents this list.", "192d4b7ab7ed1b24686362e5c1265ea3");

	internal static readonly GraftBeat ThaberdineHub = GraftBeat.HubOf("ThaberdineHub", "1a17d8053a3be7f47a7908eb6706f2fe", "Thaberdine's hub at the Drezen tavern, where he is always drinking; the hangover cure is a topic, not a reply to any one line.");

	internal static readonly GraftBeat NenioExaminesTheCultists = GraftBeat.At("NenioExaminesTheCultists", "2e4be005071707640b4b5cac95c23d06", "Cue_0005, Nenio calling the cultists an audience for her experiment, is the only cue that presents this list.", "59805e1890212394f9ebfda0f13486dd");

	internal static readonly GraftBeat NenioNoticesYou = GraftBeat.At("NenioNoticesYou", "28a0d7c556d73e84c89211458a9adbc5", "Cue_0002, Nenio noticing you, is the only cue that presents this list.", "820a32851f906444aab9526344eb3abd");

	internal static readonly GraftBeat ShopGolemAsksForTheCodeword = GraftBeat.At("ShopGolemAsksForTheCodeword", "8c13ea607012b5c40950191b24dfe2d5", "Every presenting cue is the broken golem asking for, or refusing, the codeword (Cue_0001, 0002, 0007).", "50094bc4f2dffbb468eac5173352b026", "7d6640f4d5d440a49a592ec40f14dd3b", "b4ca45bf88e576e44b19821c4ca73ae8");

	internal static readonly GraftBeat KerismeiDemandsTheTraitor = GraftBeat.At("KerismeiDemandsTheTraitor", "a586818461e82c64c9a2f9249795b676", "Kerismei's own lines accusing Woljif (Cue_0003, 0010, 0011, 0035, 0095). Not Woljif's Cue_0029 and Cue_0068, after she has given you time.", "83d9d734b3ac05746bc6ff83726e3403", "dfcb5e4c7edf5d441a5a0d35c9d8db56", "d244e281c3ca821469759dddf81ece49", "2e3b32fb7f416b345840ff702b8a2b95", "2cd533c9d3a396b48bb6491534222e4e");

	internal static readonly GraftBeat KerismeiAfterTheTraitor = GraftBeat.At("KerismeiAfterTheTraitor", "1f1985a501be18a409aa22c31b268150", "Kerismei's own lines once the traitor is known (Cue_0003, 0005, 0009, 0011, 0022); not Woljif's Cue_0012.", "32a67b6da959f6e4a98851ba76c7e537", "e6b343554f5340644abe65ee71605676", "a749b50e05bb5c349b630c1c03b82f93", "09e108402169018449553aa6ca0ecbd6", "dbd26d3726e06df45a76ee900e9e998d");

	internal static readonly GraftBeat HorgusAsksIfYouThreatenHim = GraftBeat.At("HorgusAsksIfYouThreatenHim", "82da27c4efd945bab5ec9c3cb977dcb2", "Cue_0023, Horgus asking whether you are threatening him, is the only cue that presents this list.", "d00d3e596f0545a459df674cbe23b42e");

	internal static readonly GraftBeat ArankaAtTheParty = GraftBeat.At("ArankaAtTheParty", "2373bef62ea65d741a8f54ba95f9a8b2", "Aranka's lines about herself, her song and the inquisitors hunting her (Cue_0001, 0005, 0006, 0007, 0022). Not her farewell, the count or Daeran's aside.", "719e5da62bf89db42872831eb31f0e1c", "26535138da3817843a73e872975f85e7", "ae8ac96be3e33ee4098ea7b85271599f", "053af631839a53d408059cb64574d3a8", "6b7598a8bd55168408695ca10554c011");

	internal static readonly GraftBeat TieflingUnderTheRubble = GraftBeat.At("TieflingUnderTheRubble", "58bfbb095cc338a4cb79612cf5acf380", "Every presenting cue is the tiefling still trapped (Cue_0004, 0006, 0012, 0014, 0015, 0034); his thanks after the rescue present another list.", "187364acfe3c9e348951c9dfc77ff5a2", "8693fcb01eaada94ea4d275a7763d348", "db1391effc82c2a4cb1a1bda3611408b", "96c49835cfa97d44fa1498c0908a288f", "f641222242d04814d80217a9935e1956", "341c4ec5e5155c340b8175d44d79fcbe");

	internal static readonly GraftBeat StorytellerRescued = GraftBeat.At("StorytellerRescued", "7789c0a35cd866b41896f74f6f6b3c82", "The Storyteller's own lines after the rescue (Cue_0002, 0003, 0006, 0010, 0015, 0050); not the empty return Cue_0008.", "4f4efdb3c18f4d319f3a33e58d3e15fd", "dee138396d0895d4b8d5ab05994946db", "94b36d7ee0e74d64f9f8916b7a488b74", "664761d2255b3414e86115ac176fbd39", "df433986983c6184c975bfd83d415174", "d12e33ba404e5f0419ae1571af97ca39");

	internal static readonly GraftBeat StauntonOnHisPenance = GraftBeat.At("StauntonOnHisPenance", "1c39502dd11dbd046a7fc212702ad3fb", "Staunton on his shame, his penance and his god (Cue_0007, 0011, 0021, 0026, 0056, 0059, 0060, 0062, 0064). Not his greeting, the Storyteller or the tavern lost.", "ac2d23d70368cc249b5e0453fcdd324c", "0f1c70ecbdd8a424180bb25acb75b53f", "54a7855ba48680f40aa9e02fd66be92e", "947d824483466a94f9f99005352f6b36", "cc1b209fa8a821449b95ca71511b0922", "933d35f5be442bc43954a027fe8478d7", "f7052a5e4c52ebf4ebaaf321b72f8a70", "a5faae5e981cdff4da0dd33d921f093f", "fe1d82fdad69b404c8cd1c269d7ba4a8");

	internal static readonly GraftBeat FornSpeaksOfThePoison = GraftBeat.At("FornSpeaksOfThePoison", "cb8da702f652de9488abb22f0ae88881", "Cue_0004 and Cue_0088, where Forn says the wounds were poisoned. Not his name, his quarry or his offer of help.", "189642fe82a3a86418fae22622edfd3a", "a37470ebdcc0814469f0b9a30e21cb6f");

	internal static readonly GraftBeat IrabethBeforeTheAssault = GraftBeat.At("IrabethBeforeTheAssault", "65e2ec6ff2d50884b818df2a35700e2c", "Irabeth readying the tavern (Cue_0001, 0002, 0008); not Cue_0009, where she sends you to rest.", "911c171f2a422ec41ba9cbebc872cfb6", "2872e79dc8ef1f844ba549eb6d602a52", "aa767941fa5985345a1f573891783ca4");

	internal static readonly GraftBeat IrabethAfterTheVictory = GraftBeat.At("IrabethAfterTheVictory", "aab7657377a11d847a5ad73bf633e8e8", "Irabeth's victory lines (Cue_0001, 0013, 0014, 0024); not the two empty cues of the imported campaign.", "603b9cb39702cd1488f4156995d7bf1e", "6f450239b8e4c61469dedf6c1c4e3c1c", "fc83ca0a919c53849a5e741d9a3f13c8", "91e5ca127445c214fae24e707e05aaef");

	internal static readonly GraftBeat SuccubusTakesTheOffering = GraftBeat.At("SuccubusTakesTheOffering", "f6572cb37f42aa841808b2bbc293f3b0", "Cue_0006, the succubus taking the knight's eyes, is the only cue that presents this list.", "0e74161f2e82ae74e8992006179b3989");

	internal static readonly GraftBeat MessengerRaisesTheAlarm = GraftBeat.At("MessengerRaisesTheAlarm", "ab362e5b1af4b354ea59d583376cf661", "The messenger's alarm and plea (Cue_0011, 0012, 0014); not Cue_0017, where he insists on leading you back.", "c27998dfa33e1d94094847ceb71f1be4", "a80354dba7fbd294cb7dc1c9baa9e206", "910e2d600edb88044a6776e2123bf729");

	internal static readonly GraftBeat MongrelsReachTheSurface = GraftBeat.At("MongrelsReachTheSurface", "07d811c04bc82a34ba54c253e3ca2455", "The young mongrels ready to fight (Cue_0003, Cue_0013); not the Gozreh reply Cue_0071.", "8bf30fdd1e1f8ad48bef75221dfb80a5", "33aa7f257c10bd74ea4a3f3b36314017");

	internal static readonly GraftBeat KlaemInTheTavern = GraftBeat.At("KlaemInTheTavern", "b837a26d7444dd442a45ee54da358cc9", "Klaem at his reading (Cue_0001, 0006, 0011, 0016); not Cue_0014, his haste ritual offer.", "c4154dc59cc9a464baceee79f2032b76", "ecf03c0e0e991cd49b4b767b97a68977", "2d688c6e189b3f245bcf8db2bcb9a4de", "f401774b6096b2047b1764dcbd778e78");

	internal static readonly GraftBeat QueenAtTheDefendersHeart = GraftBeat.At("QueenAtTheDefendersHeart", "8c7d0b0b6a1701d49835740d65d3d922", "Cue_0002, where the Queen's visit turns to the battle, is the only cue that presents this list.", "b530a1b9d96a98e439d6a1744cef06f3");

	internal static readonly GraftBeat NexusCampArrival = GraftBeat.At("NexusCampArrival", "91097a9b90537a34fbe379d5aa4d9cbe", "The camp answers react to arriving in the Abyss: Cue_0001, the Hand of the Inheritor surveying the rift's landscape, and Cue_0022, his greeting at the camp. Not his later lore answers.", "acb4a2c844fcdb24092c481621db450e", "cd0c9bad74d14cf46a9d8b7e911f538a");

	internal static readonly GraftBeat KingpinAsksAboutSlavery = GraftBeat.At("KingpinAsksAboutSlavery", "c896137fe31c15445bba592672ebfbc9", "Cue_0126 is the only cue that presents the slavery question at the Fleshmarket.", "f9d984d7765892b4b88e0333d498d012");

	internal static readonly GraftBeat AtselmHub = GraftBeat.HubOf("AtselmHub", "6edb974ede9417542abc4f2fc7c1bade", "Atselm's question hub in Battlebliss; the arena answers are topics, not replies to any one line.");

	internal static readonly GraftBeat HerraxHub = GraftBeat.HubOf("HerraxHub", "43f93812d6216c94db356622859397f1", "Herrax's hub at the Ten Thousand Delights; the answers are topics, not replies to any one line.");

	internal static readonly GraftBeat ChivarroDemandsAFavor = GraftBeat.At("ChivarroDemandsAFavor", "abf7287a88afc554da91b3cd5d7c5bd1", "Every presenting cue is Chivarro refusing to help unless you do her favor, which is what the answers push back on.", "ba0cad31fc44ec84eb82b6a3f758ac92", "bd7425739820aaa4f81dd73e44732dcb", "ff988a453e6115e4e8e309f07abf5484");

	internal static readonly GraftBeat HepzamirahConfrontation = GraftBeat.At("HepzamirahConfrontation", "995aaa29e772b594a966a2f133be702c", "Hepzamirah's own lines of the confrontation (Cue_0001, 0010, 0013, 0014, 0019). Not the Echo of Deskari's Cue_0026.", "d648693214b80cf4db864445c4d8c7f8", "5c15fc3d366ad5c40a3264fd68886213", "b03aa7395540e8040a55a62309caaa1a", "afedf9d267c8b524990b3b12fe9ede3b", "56d92e97cafa0de4dbb0161469c9f473");

	internal static readonly GraftBeat BadLuckInnkeeperHub = GraftBeat.HubOf("BadLuckInnkeeperHub", "772496e11e1b6804da503d2883417f97", "The Bad Luck innkeeper's bed, booze or bargain hub; the answers are topics.");

	internal static readonly GraftBeat BlindHalflingHub = GraftBeat.HubOf("BlindHalflingHub", "b45c22db4bfe8ad4ab106941ede45b7d", "The blind halfling's hub in the Bad Luck; the answers are topics.");

	internal static readonly GraftBeat ChivarroHub = GraftBeat.HubOf("ChivarroHub", "5ce68965a500a3142a864c7009b56799", "Chivarro's hub at the Ten Thousand Delights; the answer is a topic.");

	internal static readonly GraftBeat BatteSlaverHub = GraftBeat.HubOf("BatteSlaverHub", "c67f95deb80bb324281796413b5d28ae", "The gladiator trainer's hub in the Fleshmarket; the answers are topics.");

	internal static readonly GraftBeat FreedMeatSlaves = GraftBeat.At("FreedMeatSlaves", "642c54b135c66dd448373875b677a25a", "Both presenting cues are Dyunk's freed slaves talking to their new master.", "7ac16abec75f1c84181ae12d71897f97", "05ba7fb0a774f4148818559206f89a3c");

	internal static readonly GraftBeat AasimarSlavesHub = GraftBeat.HubOf("AasimarSlavesHub", "67823011274209e4882b60f4b95d103f", "The aasimar captives' hub; the promise is a topic.");

	internal static readonly GraftBeat ArenaHealerHub = GraftBeat.HubOf("ArenaHealerHub", "dd04ec68821f0ac4db58a756d323d213", "The Battlebliss healer's hub; the answers are topics.");

	internal static readonly GraftBeat ZeklexAfterImprisonmentHub = GraftBeat.HubOf("ZeklexAfterImprisonmentHub", "da29db4239d1c9945ba3440bd1a01141", "Zeklex's hub after the imprisonment; the tournament answer is a topic.");

	internal static readonly GraftBeat ShamiraHub = GraftBeat.HubOf("ShamiraHub", "d138954fd7cdb2d4e90bb28cbd76235e", "Shamira's audience hub; the answers are topics.");

	internal static readonly GraftBeat CiarWelcomesYouBack = GraftBeat.At("CiarWelcomesYouBack", "234a43b05125963409b93aded03a5125", "Ciar greeting the returned Commander: Cue_0001 (the first meeting in six months) and Cue_0002 (his salute). Not his war stories.", "30e04862fe53a6e4cb974fed61681b49", "0777418cb609b994c8a8db4020078a4d");

	internal static readonly GraftBeat AnemoraRevealsHerLord = GraftBeat.At("AnemoraRevealsHerLord", "c6d5321c350bf7c4fa364e34c0ee30a4", "Anemora's own lines in the temple of stone manuscripts (Cue_0028, Cue_0051); not the empty or narration cues.", "f562aa1904b5c544cafac2462fb59a10", "ac350a937f258fa47b975c843253cbf9");

	internal static readonly GraftBeat DeskariLooksDown = GraftBeat.At("DeskariLooksDown", "7d877541fed521c489e6b33845647ad3", "Deskari speaking over the ruins (Cue_0001, Cue_0062). Radiance's narration Cue_0036 is not listed; the gate reaches it only as their continuation.", "63059bf54db95b74cbae2fee6decc89b", "aadcb742b2aa88540912972f6ff82fc7");

	internal static readonly GraftBeat AlderpashInHisPrison = GraftBeat.At("AlderpashInHisPrison", "a120eec4aaca4d2398cd863e6245a551", "Cue_0093 is the only cue that presents this list.", "958f0d92445387147ac508e5aefb6cc5");

	internal static readonly GraftBeat MutasafenForcesTheChoice = GraftBeat.At("MutasafenForcesTheChoice", "6278129d95bb73744a60b07fc6612e45", "Cue_0012 is the only cue that presents this list.", "4370f12a978a6b146836a78071710b77");

	internal static readonly GraftBeat ChunHub = GraftBeat.HubOf("ChunHub", "6b8b71866da1eee44bdd1d5d93a2a0f3", "Chun's hub in Drezen; the answers are topics.");

	internal static readonly GraftBeat AneviaDrezenReturnHub = GraftBeat.HubOf("AneviaDrezenReturnHub", "46f6b0263c6992f43b33a2fda5518983", "Anevia's (and Irabeth's) question hub on the return to Drezen; the answers are topics.");

	internal static readonly GraftBeat DangerousTavernHub = GraftBeat.HubOf("DangerousTavernHub", "15abc4d1f76f3cc418b593c33c41dda1", "The tavern keeper's hub during the siege; the answer is a topic.");

	internal static readonly GraftBeat AreeluWelcomesYouToThreshold = GraftBeat.At("AreeluWelcomesYouToThreshold", "bb3528754c7e741438acef95ec3b6430", "Areelu welcoming you to Threshold (Cue_0001) and her curt answer (Cue_0027), the two cues that present this list.", "4907194aa1923d34894bca5996d3054d", "6b4c8a08d22268f41badc455825b3082");

	internal static readonly GraftBeat SeelahFarewellHub = GraftBeat.HubOf("SeelahFarewellHub", "2b1cc60873a86314c9b81b93635fa430", "Seelah's farewell before Threshold; the answers are topics.");

	internal static readonly GraftBeat RegillFarewellHub = GraftBeat.HubOf("RegillFarewellHub", "bc23c9f44b93ab9428f62db47ac84b3f", "Regill's farewell before Threshold; the answers are topics.");

	internal static readonly GraftBeat WoljifFarewellHub = GraftBeat.HubOf("WoljifFarewellHub", "234f289beeac48e458189a331295669f", "Woljif's farewell before Threshold; the answers are topics.");

	internal static readonly GraftBeat LannFarewellHub = GraftBeat.HubOf("LannFarewellHub", "c170936600403124291df84fa2e9e9fa", "Lann's farewell before Threshold; the answers are topics.");

	internal static readonly GraftBeat WenduagFarewellHub = GraftBeat.HubOf("WenduagFarewellHub", "945033166e2524a46b3282f15a925139", "Wenduag's farewell before Threshold; the answers are topics.");

	internal static readonly GraftBeat ArueshalaeFarewellHub = GraftBeat.HubOf("ArueshalaeFarewellHub", "8bc697f29600c9446a5d531a2128f4b2", "Arueshalae's farewell before Threshold; the answers are topics.");

	internal static readonly GraftBeat EmberFarewellHub = GraftBeat.HubOf("EmberFarewellHub", "979f96a0b504daf4297e9a91016ab446", "Ember's farewell before Threshold; the answers are topics.");

	internal static readonly GraftBeat DaeranFarewellHub = GraftBeat.HubOf("DaeranFarewellHub", "727ea1dcd38772c4089ee5c696de60eb", "Daeran's farewell before Threshold; the answers are topics.");

	internal static readonly GraftBeat GreyborFarewellHub = GraftBeat.HubOf("GreyborFarewellHub", "98a6dde2e5e7b2a4abd97a0bc3719a96", "Greybor's farewell before Threshold; the answers are topics.");

	internal static readonly GraftBeat NenioFarewellHub = GraftBeat.HubOf("NenioFarewellHub", "6b068a702077f1c448d66f64afeb65f7", "Nenio's farewell before Threshold; the answers are topics.");

	internal static readonly GraftBeat AivuFarewellHub = GraftBeat.HubOf("AivuFarewellHub", "2b6acff363cf61346b319befd6229bb9", "Aivu's farewell before Threshold; the answer is a topic.");

	internal static readonly GraftBeat AreeluFinalTalkHub = GraftBeat.HubOf("AreeluFinalTalkHub", "90861396a1375684eb5bf894bedecff3", "Areelu's question hub before the final fight; every presenting cue is her answering a question, and the answers are topics.");

	internal static readonly GraftBeat PharasmaWeighsYourSoul = GraftBeat.At("PharasmaWeighsYourSoul", "f0d89a1e468b4c6f9affc010e8d47596", "Cue_4, the Lady of Graves weighing the soul, is the only cue that presents this list.", "8dff6ab5b662470bb22f5a9978c19edc");

	internal static readonly GraftBeat IrabethAneviaThresholdHub = GraftBeat.HubOf("IrabethAneviaThresholdHub", "46e1e280571bd4249af979c38846fe04", "Irabeth's and Anevia's talk at Threshold; the answers are topics.");

	internal static readonly GraftBeat GalfreyAtTheThreshold = GraftBeat.At("GalfreyAtTheThreshold", "bc636f62a8c94ab4798b32bba398bebb", "Cue_0003 is the only cue that presents this list.", "65bf1506baa7df54f9a4ed0ca04de2be");

	internal static readonly GraftBeat NorthernLightsArtifact = GraftBeat.At("NorthernLightsArtifact", "422a385b41f72094783b676f54aad062", "Cue_0001 is the only cue that presents this list.", "9dd876748a163dc43ba5dde992058c23");

	internal static readonly GraftBeat ThresholdGates = GraftBeat.At("ThresholdGates", "cf957e3715b7e3840aaab58a6eb372d0", "All three presenting cues describe the gates of Threshold before you go in.", "7efd1f87705528c41a93b5f441a8e44b", "447faea1e879cf548ad0eab63839d3e6", "9f17dec80aa73d6418bcee1f457675e5");

	internal static readonly GraftBeat ValmallosHub = GraftBeat.HubOf("ValmallosHub", "b686f80a62f24ca08899e558b96c7a1b", "Valmallos's question hub (Inevitable Excess); the answer is a topic.");

	internal static readonly GraftBeat NavigatorHub = GraftBeat.HubOf("NavigatorHub", "aac49e0c7220417c89bde055dd698ebb", "The navigator's hub (Midnight Isles); the answer is a topic.");

	internal static readonly GraftBeat GundrunEpicSpeech = GraftBeat.At("GundrunEpicSpeech", "57929455cd9546d28fc159656bf41eda", "The three rallying speeches before the battle (Last Sarkorians); the answer answers any of them.", "dafdf555c0a14484bca6c14ccb0a7933", "0779b8d7b7c1492f8d201e2592fe37e7", "ac1c7da88faa42a3a64b0622e4be27da");

	internal static readonly GraftBeat SithhudAsksWhoYouAre = GraftBeat.At("SithhudAsksWhoYouAre", "0cbb2e81134942d2aac2cf6312072c80", "Sithhud asking who you are, and the two narrations of what is left of him (Lord of Nothing).", "74420bb042994d6b9a7b84d7fd571bb7", "882e41b102a243ac84ac508d69e3ba09", "081bfc00638749f384b0fe94423c9ee4");

	internal static readonly GraftBeat FakeCommanderAtTheFestival = GraftBeat.At("FakeCommanderAtTheFestival", "470d79f6ad25457582490fd9cddb712e", "Every presenting cue shows the crowd cheering the impostor Commander (Dance of Masks).", "2836df0e3ffc4c87880385de0bdf68a8", "3de491671e924a2ea9009113bedb7f7c", "bcef768b67cf42d39e0266f370ed9a07", "981d6f396ff64ef9aff7edf231ee3489", "59261d96f71d45e6846c7e398948b1d6", "bc0a4ae7058446609272bc9fe22fcc9d");

	internal static readonly GraftBeat AlushinyrraPortalArch = GraftBeat.HubOf("AlushinyrraPortalArch", "e76902cd78ea1d24687eed6a97c51598", "The portal arch's investigation list: every presenting cue is narration about the arch and its network, and the answers are topics about it.");

	internal static readonly GraftBeat AneviaAfterTheQueenFell = GraftBeat.At("AneviaAfterTheQueenFell", "a53f40ba02af23444b63de6b9337702d", "Anevia on the soldiers after the Queen's death (Cue_0035, Cue_0211). Not the empty Cue_0424.", "6dea770281b0d1d4195d2ed6a0f9f513", "070e849ca16487340ae9641fcfabab53");

	internal static readonly GraftBeat GalfreyBriefsYouForIz = GraftBeat.At("GalfreyBriefsYouForIz", "ec740ac6a039d8c4b83a076cee6c1ff0", "Every presenting cue is Galfrey's briefing before Iz; the answers are plans and vows for Iz, true whatever happens there.", "c37c6235f1748b144903529f78462c50", "3fbc37de89f078b4d9ee085107c94e17", "7386cf07d93199b46b8689670481e825");

	internal static readonly GraftBeat AreeluAtTheGatesOfIz = GraftBeat.At("AreeluAtTheGatesOfIz", "09b8d5eb5dae3634d9dc3d8c7bdd6e9d", "Areelu's own lines at the gates of Iz (Cue_0001, 0009, 0010). Not the empty Cue_0037.", "214c8572a8c24c8439dd94c6ebe39d69", "c000ae3d47250b943953b1bd25333f30", "891a8c1c1cf92da46ad19f539d73ef15");

	internal static readonly GraftBeat TheEmptyFlagpole = GraftBeat.At("TheEmptyFlagpole", "20a1be61aa7680e45890656b204e6fe8", "Cue_0014, the destroyed camp and the empty flagpole, is the only cue that presents this list.", "710ed96131bbf4e42b930f87cfd6aa25");

	internal static readonly GraftBeat ArenaMasterAnnouncesTheLast = GraftBeat.At("ArenaMasterAnnouncesTheLast", "41e6f63769614e82a63f26c00e482112", "Cue_10, the Master of the Arena announcing the last and strongest creation (Dance of Masks).", "15c1a52843624fca92a9576f9dcd336f");

	internal static IEnumerable<GraftBeat> All()
	{
		return from f in typeof(DialogueBeats).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
			where f.FieldType == typeof(GraftBeat)
			select (GraftBeat)f.GetValue(null);
	}
}
