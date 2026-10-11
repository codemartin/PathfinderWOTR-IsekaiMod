using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using IsekaiMod.Content.Quests;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Constellations;

internal static class ConstellationDialogueBanter
{
	internal struct SeatLine(string seat, string text)
	{
		public string Seat = seat;

		public string Text = text;
	}

	public const string CAYDEN_TAG = "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color>";

	public const string IOMEDAE_TAG = "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color>";

	public const string ASMODEUS_TAG = "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color>";

	public const string DESNA_TAG = "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color>";

	public const string PHARASMA_TAG = "<color=#133E71><b>[The Constellation 'The Lady of Graves']</b></color>";

	public const string CALISTRIA_TAG = "<color=#5B2A86><b>[The Constellation 'The Savored Sting']</b></color>";

	public const string YOG_TAG = "<color=#5B2A86><b>[The Constellation 'The Key and the Gate']</b></color>";

	public const string BUTTERFLY_TAG = "<color=#133E71><b>[The Constellation 'The Silence Between the Stars']</b></color>";

	public const string LANTERN_TAG = "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color>";

	public const string NETHYS_TAG = "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>";

	public const string GORUM_TAG = "<color=#7A1010><b>[The Constellation 'Our Lord in Iron']</b></color>";

	public const string BESMARA_TAG = "<color=#133E71><b>[The Constellation 'The Pirate Queen']</b></color>";

	public const string CHALDIRA_TAG = "<color=#004604><b>[The Constellation 'The Mischievous Friend']</b></color>";

	public const string URGATHOA_TAG = "<color=#7A1010><b>[The Constellation 'The Pallid Princess']</b></color>";

	public const string MILANI_TAG = "<color=#5B2A86><b>[The Constellation 'The Everbloom']</b></color>";

	private static readonly Random _rng = new Random();

	private static readonly Regex FeastWords = new Regex("\\b(?:devour(?:s|ed|ing)?|feast(?:s|ed|ing)?|hunger|undead|undeath|corpse(?:s)?|carrion|entrails|flesh|blood|eat(?:s|ing)?|ate|consume(?:s|d)?)\\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

	private static readonly Regex TruckWord = new Regex("\\btrucks?\\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

	private static readonly Regex OriginWorldWords = new Regex("\\btrucks?\\b|\\breincarnat|\\b(?:an)?other world\\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

	internal const float FinaleLineDurationSeconds = 60f;

	internal const string GrandFinalScene = "The Grand Final";

	private static readonly string Separator = "<color=#5B2A86><b>=======================================================</b></color>";

	internal static readonly IReadOnlyDictionary<string, string> SeatTags = new Dictionary<string, string>(StringComparer.Ordinal)
	{
		{ "The Lucky Drunk", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color>" },
		{ "The Inheritor", "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color>" },
		{ "The Prince of Darkness", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color>" },
		{ "The Song of the Spheres", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color>" },
		{ "The Lady of Graves", "<color=#133E71><b>[The Constellation 'The Lady of Graves']</b></color>" },
		{ "The Savored Sting", "<color=#5B2A86><b>[The Constellation 'The Savored Sting']</b></color>" },
		{ "The All-Seeing Eye", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>" },
		{ "Our Lord in Iron", "<color=#7A1010><b>[The Constellation 'Our Lord in Iron']</b></color>" },
		{ "The Pirate Queen", "<color=#133E71><b>[The Constellation 'The Pirate Queen']</b></color>" },
		{ "The Laughing King", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color>" },
		{ "The Mischievous Friend", "<color=#004604><b>[The Constellation 'The Mischievous Friend']</b></color>" },
		{ "The Everbloom", "<color=#5B2A86><b>[The Constellation 'The Everbloom']</b></color>" },
		{ "The Silence Between the Stars", "<color=#133E71><b>[The Constellation 'The Silence Between the Stars']</b></color>" },
		{ "The Key and the Gate", "<color=#5B2A86><b>[The Constellation 'The Key and the Gate']</b></color>" },
		{ "The Pallid Princess", "<color=#7A1010><b>[The Constellation 'The Pallid Princess']</b></color>" }
	};

	private const string Drunk = "The Lucky Drunk";

	private const string Inheritor = "The Inheritor";

	private const string Prince = "The Prince of Darkness";

	private const string Song = "The Song of the Spheres";

	private const string Graves = "The Lady of Graves";

	private const string Sting = "The Savored Sting";

	private const string Eye = "The All-Seeing Eye";

	private const string Iron = "Our Lord in Iron";

	private const string Pirate = "The Pirate Queen";

	private const string King = "The Laughing King";

	private const string Friend = "The Mischievous Friend";

	private const string Bloom = "The Everbloom";

	private const string Silence = "The Silence Between the Stars";

	private const string KeyAndGate = "The Key and the Gate";

	private const string Pallid = "The Pallid Princess";

	internal static readonly IReadOnlyDictionary<string, SeatLine[]> EndingVerdicts = new Dictionary<string, SeatLine[]>(StringComparer.Ordinal)
	{
		{
			"UnwrittenDawn",
			new SeatLine[3]
			{
				L("The Key and the Gate", "unfolds every facet at once: \"The Keys engage. The anchors are load-bearing. They are also, it transpires, breakable. I did not know that. I have known everything else.\""),
				L("The Laughing King", "presses both hands to the lens: \"They're doing it - they're actually doing it - somebody get me a wider shot, the stage is coming DOWN-\""),
				L("The Lady of Graves", "rises from her seat: \"Two worlds touch that were never meant to share a border. Every soul on both sides now dies only once. Do you understand what you have handed me, transmigrant? Work. Honest work.\"")
			}
		},
		{
			"InterdimensionalTraveler",
			new SeatLine[3]
			{
				L("The Song of the Spheres", "throws open the whole sky: \"You're not staying. Oh, good - good! Golarion was never the destination, it was the first stop, and you finally know it. Take every road. Take the ones with no names.\""),
				L("The Pirate Queen", "tips her tricorn: \"Leaves the port in better shape than they found it and sails on the tide. That's a proper captain's exit.\""),
				L("The Inheritor", "watches the slipstream close: \"They gave this world back to the people who live in it. I have seen worse stewardship from those who stayed.\"")
			}
		},
		{
			"EternalActor",
			new SeatLine[3]
			{
				L("The Laughing King", "laughs until he has to hold the rail: \"THEY BOWED! THEY BOWED TO THE CAMERA! Oh, you wonderful creature, you knew we were here the whole time and you played to the house anyway!\""),
				L("The Prince of Darkness", "nods with real respect: \"A contract signed with eyes open. The only kind worth anything. You knew the terms and took them. I have damned better people for less clarity.\""),
				L("The Lucky Drunk", "sets his tankard down slowly: \"...Kid. Kid, you didn't have to give it to us. We'd have kept watching anyway.\"")
			}
		},
		{
			"PaxSepulchrum",
			new SeatLine[3]
			{
				L("The Prince of Darkness", "sets down his quill in something close to reverence: \"Impeccable order, zero crime, and the dead doing the labor so the living need not. I have argued for this arrangement in the courts of Hell for ten thousand years and been called a monster for it. Sarkoris simply did it.\""),
				L("The Pallid Princess", "smiles slowly: \"Death that serves the table instead of clearing it. How generous of you, Supreme One. How unlike you.\""),
				L("The Lady of Graves", "regards the bone-fields without warmth: \"Every one of those laborers is a soul deferred, not a soul denied. You will bring them to me eventually, Overlord. Slowly is not the same as never. I accept the terms.\"")
			}
		},
		{
			"IronCitadel",
			new SeatLine[3]
			{
				L("The Silence Between the Stars", "whispers into the obsidian: \"Walls this high were built to keep something out. You are inside them. Which was it?\""),
				L("The Prince of Darkness", "notes it without enthusiasm: \"Sovereignty without subjects. You have secured a perfect, defensible, meaningless nothing. Efficient, I grant you.\""),
				L("The Savored Sting", "sighs with enormous disappointment: \"All that power and you built yourself a very large, very tidy box, darling. Nobody to spite in there. Nobody at all.\"")
			}
		},
		{
			"NecroTyrant",
			new SeatLine[3]
			{
				L("The Pallid Princess", "shivers with delight: \"A whole continent, harvested and kept. Not one soul wasted, not one left to rot uneaten. My dear, that is the most loving thing anyone has done all era.\""),
				L("The Inheritor", "does not shout; it is worse: \"I watched this soul fight beside my paladins under Kenabres. I watched a queen name them Knight-Commander. Record that I did.\""),
				L("The Lady of Graves", "speaks with absolute frost: \"Northern Avistan arrives at my gate in a single column, in perfect step, and none of them may enter. You have not ended them, Overlord. You have merely made them wait in the hall. I will remember whose order it was.\"")
			}
		},
		{
			"TempestFederation",
			new SeatLine[3]
			{
				L("The Lucky Drunk", "climbs onto the celestial table: \"HOT SPRINGS! IN THE WORLDWOUND! Monsters and mortals at ONE TABLE and nobody's eating anybody - this is my entire religion, this is LITERALLY my entire religion, DRINKS FOR EVERY SEAT!\""),
				L("The Everbloom", "scatters petals across the whole broadcast: \"A green thing that could have eaten the world decided to set it a place instead. Let every tyrant in every era hear about this one.\""),
				L("The Laughing King", "wipes his eyes, still cackling: \"The gelatin. The GELATIN built a functioning federation. Seelah's going to be fine, she's going to be completely fine, she's only going to think about this every day forever.\"")
			}
		},
		{
			"PrimevalMarsh",
			new SeatLine[3]
			{
				L("The Song of the Spheres", "hums something very old and very slow: \"The wild comes back over Sarkoris like a blanket, and a great warm thing sleeps under the World Tree. Nobody will write songs about this. They'll just be safe, and never know why.\""),
				L("Our Lord in Iron", "grunts, unexpectedly satisfied: \"A BEAST THAT GUARDS ITS GROUND AND SLEEPS ON IT. THERE ARE WORSE ENDINGS. MOST OF THEM ARE MINE.\""),
				L("The Mischievous Friend", "curls up on the celestial rail in sympathy: \"Ooh, a nap. A really, really long nap. Honestly? Earned.\"")
			}
		},
		{
			"AbyssalSingularity",
			new SeatLine[3]
			{
				L("The All-Seeing Eye", "speaks in both voices, and they are afraid of each other: \"It goes down. It keeps going down. It eats the Abyss to the floor and then it eats the floor, and something under the floor opens one eye-\""),
				L("The Key and the Gate", "rotates once and stops: \"The timeline folds inward on the vector specified. Collapse proceeds as declared. There is no error in this outcome. It is simply the end of the reel.\""),
				L("The Laughing King", "stares at a lens showing nothing: \"...I've got no shot. There's no shot to cut to. There's no anything to cut to.\"")
			}
		},
		{
			"EternalHope",
			new SeatLine[3]
			{
				L("The Inheritor", "raises her blade and her voice breaks on it: \"Courage answered. Of course it was Courage. Whatever oath they swore on the day they fell out of the sky, the hand that closed on that mark was a protector's, and it always was.\""),
				L("The Everbloom", "lets the petals rise instead of falling: \"Every mortal who ever hoped and got nothing back - all of it, gathered, and given out again. That is the rebellion. That was always the rebellion.\""),
				L("The Song of the Spheres", "weeps openly in starlight: \"A light that no longer needs a night to be measured against. Shine, traveler. You don't have to stop.\"")
			}
		},
		{
			"RetiredLegend",
			new SeatLine[3]
			{
				L("The Mischievous Friend", "bounces so hard she nearly falls off the rail: \"A TAVERN! They had the power to remake reality and they want to pour ALE! That's the best wish anybody's ever made and nobody even had to grant it!\""),
				L("The Lucky Drunk", "is crying into his tankard and not hiding it: \"A tavern and a training hall. In Kenabres. Kid. Kid, I'm coming to the opening. I don't care what the rules say, I'm coming.\""),
				L("The Prince of Darkness", "is, uncharacteristically, quiet: \"Wisdom asked for nothing and received the only thing anyone here wants. I decline to comment further.\"")
			}
		},
		{
			"CorruptedChampion",
			new SeatLine[3]
			{
				L("The Savored Sting", "purrs, absolutely thrilled: \"Oh, the PARAGON. The one who lectured us in Act One. Look at that iron fist, darling - you kept every promise you made, you just made them into threats.\""),
				L("The Inheritor", "speaks with cold fury: \"Power answered, and it was never Courage, and you will tell yourself for the rest of your long life that there is no difference. There is. I taught it to you.\""),
				L("The Everbloom", "closes her hand around the petals: \"They'll fear you and be safe, and they'll never once be free. I've spent my whole existence prying people out of hands exactly like that one.\"")
			}
		},
		{
			"PaxSolaris",
			new SeatLine[3]
			{
				L("Our Lord in Iron", "strikes his gauntlets together like a cannon: \"SOLAR TOWERS FROM DREZEN TO THE WORLD'S EDGE! A CROWN THAT BUILDS! I DO NOT UNDERSTAND IT AND I SALUTE IT!\""),
				L("The All-Seeing Eye", "murmurs with genuine hunger: \"Light made into engine, engine into commonwealth. That is transmutation of the highest order, and they did it with masonry and consent.\""),
				L("The Inheritor", "inclines her head: \"A golden throne that answers to something. I have watched four hundred crowns placed in this era and this is the second one I would kneel to.\"")
			}
		},
		{
			"SarkorianImperium",
			new SeatLine[3]
			{
				L("The Prince of Darkness", "writes with evident pleasure: \"Legions at every border, law absolute, commerce dominant. Not a flicker of sentiment in the whole design. God-Emperor, had you been born in Cheliax I would have made you a duke by lunch.\""),
				L("The Lucky Drunk", "groans into his ale: \"Oh good, the contract lawyer's happy. That's never once been a good sign for anybody living under the thing.\""),
				L("The Pirate Queen", "snorts: \"Every border sealed and every tariff collected. You've just invented the most profitable smuggling era in history, Your Majesty. Thank you kindly.\"")
			}
		},
		{
			"DualThrone",
			new SeatLine[3]
			{
				L("Our Lord in Iron", "bellows until the rail rings: \"SOLAR BANNERS PLANTED IN ABYSSAL MAGMA! MORTAL AND FIEND KNEELING IN THE SAME RANK! WAR WITHOUT END AND A THRONE ON TOP OF IT! MAGNIFICENT!\""),
				L("The Prince of Darkness", "makes a careful note: \"Conquest of a plane that does not stay conquered. You have not won the Abyss, Emperor. You have subscribed to it.\""),
				L("The Lady of Graves", "turns a page: \"Two realms, one crown, and every soul in both now dies under your authority. My ledger does not recognize your authority.\"")
			}
		},
		{
			"SilentAegis",
			new SeatLine[3]
			{
				L("The Silence Between the Stars", "whispers, and for once it is warm: \"The dark stopped being the thing under the bed. It became the thing standing at the door. Nobody down there will ever know your soldiers were here. That is the whole gift.\""),
				L("The Song of the Spheres", "smiles at a road with no travelers on it and shadows on either side: \"They'll sleep through the night and wake up thinking nothing happened. Oh, that's lovely. That's the loveliest kind of guarding there is.\""),
				L("The Lady of Graves", "notes it without objection: \"Sovereign darkness that shields rather than harvests. Unusual. I will allow it.\"")
			}
		},
		{
			"ObsidianBastion",
			new SeatLine[3]
			{
				L("The Lady of Graves", "regards the monolith with something like recognition: \"A gate with a keeper who does not leave the chair. I have sat mine since before your world had a name, Monarch. It is longer than you think.\""),
				L("The Key and the Gate", "measures it: \"A fixed point installed voluntarily at a planar seam. Structurally sound. Functionally a sentence with no term.\""),
				L("The Silence Between the Stars", "whispers: \"Neither demon nor conqueror crosses. Neither does anything else. Neither do you.\"")
			}
		},
		{
			"NetherworldCataclysm",
			new SeatLine[3]
			{
				L("The Prince of Darkness", "sets the goblet down untouched: \"A continent extinguished to guarantee it can never be threatened. That is not tyranny, Monarch. Tyranny requires subjects. That is merely tidying.\""),
				L("The Song of the Spheres", "covers her face with both hands: \"The sun. The sun, over all of it. There were children on those roads. I had names for some of them.\""),
				L("Our Lord in Iron", "is, for once, not shouting: \"NO BATTLE. NO STEEL. NO ENEMY. JUST THE LIGHT GOING OUT. THERE IS NO GLORY DOWN THERE FOR ANYONE, LEAST OF ALL YOU.\"")
			}
		},
		{
			"GrandArchitect",
			new SeatLine[3]
			{
				L("The All-Seeing Eye", "murmurs with both eyes wide open and agreeing: \"Every war solved before the first blade is drawn. Do you know what that looks like from here? Nothing. It looks like centuries of absolutely nothing happening. It is the most difficult magic I have ever watched performed.\""),
				L("The Prince of Darkness", "studies the board and cannot fault it: \"Centuries of quiet prosperity engineered from behind the curtain, with no credit taken and no throne claimed. Mastermind, I have no counter-move. That is not a thing I say.\""),
				L("The Everbloom", "lays petals on a hundred wars that never happened: \"No songs for any of it. No monuments. Just people getting old on purpose. I'll sing about it. Somebody has to.\"")
			}
		},
		{
			"ShadowSyndicate",
			new SeatLine[3]
			{
				L("The Savored Sting", "claps her hands in delight: \"Erased from every history and every song, and still holding the ledger that moves the world. Darling, that is the purest thing I have ever seen. Nobody will ever thank you. You knew that. You did it anyway.\""),
				L("The All-Seeing Eye", "murmurs: \"A name deleted from the record while the hand remains on it. Elegant. Nearly unfalsifiable.\""),
				L("The Laughing King", "pouts enormously: \"Erased?! From the HISTORIES?! I have four hundred hours of footage, my friend, and I am the only archive left. You're welcome. You're SO welcome.\"")
			}
		},
		{
			"NpcSimulator",
			new SeatLine[3]
			{
				L("The Laughing King", "howls, delighted and appalled in equal measure: \"They're doing my JOB! They looked at the whole world and said 'entertainment' and started moving pieces for the ratings! This is either the greatest tribute I've ever received or I need to be significantly more worried about myself!\""),
				L("The Inheritor", "speaks with quiet disgust: \"Seelah fought beside you in the dark under Kenabres when you did not even know your own name. Your companions bled for this crusade. Say the word 'piece' again where I can hear it.\""),
				L("The Silence Between the Stars", "whispers: \"You have become an audience. We are not the good seats, transmigrant. We are the ones who cannot go down there.\"")
			}
		},
		{
			"InfiniteHorizons",
			new SeatLine[3]
			{
				L("The Pirate Queen", "whoops and fires both pistols: \"Cut a door into the Astral with a SWORD and set up shop guarding the crossing! No charts, no coast, no harbor master - and a god at the gate who won't take a toll! I'll drink to that in every port there is!\""),
				L("Our Lord in Iron", "roars with genuine joy: \"ONE DRAW! ONE! THE PERFECT CUT AND THE SKY OPENS! THIS IS WHAT STEEL IS FOR, MORTAL, THIS EXACTLY!\""),
				L("The Song of the Spheres", "laughs through starlight: \"A god of the blade who stands at the crossroads to keep wanderers safe. Oh, you'll be busy. I send so many of them.\"")
			}
		},
		{
			"BoundaryHermit",
			new SeatLine[3]
			{
				L("The Silence Between the Stars", "whispers, entirely content: \"A pocket of quiet at the fringe, and one cut practiced until it is perfect. I have spent longer on less. Sit well.\""),
				L("The All-Seeing Eye", "murmurs with professional interest: \"Perfection pursued in isolation, with the clock chiming at the moment of enlightenment. The joke is that they will reach it. Repeatedly.\""),
				L("The Lucky Drunk", "scratches his beard: \"Retiring to a mountain to get REALLY good at one thing. Honestly, kid? Pour me one and I might join you.\"")
			}
		},
		{
			"SeveredHorizon",
			new SeatLine[3]
			{
				L("The Key and the Gate", "registers it without inflection: \"The stroke lands below the substrate. The metaphysical thread parts. Golarion resolves into iridescent static across approximately nine seconds. I have never recorded a nine-second era before.\""),
				L("Our Lord in Iron", "bellows in pure fury: \"THAT IS NOT A KILLING BLOW, THAT IS A TANTRUM WITH A SWORD! THERE WAS NO ENEMY! THERE WAS NO ENEMY, YOU FOOL!\""),
				L("The Lady of Graves", "stands, and the Boneyard stands behind her: \"An entire world's souls arrive at once, ahead of every hour I wrote for them. Swordsman. Every single one of them will be seated, and I will see to it personally, and I will remember your name while I do.\"")
			}
		}
	};

	internal static readonly IReadOnlyDictionary<EndingLoopOutcome, SeatLine[]> Choruses = new Dictionary<EndingLoopOutcome, SeatLine[]>
	{
		{
			EndingLoopOutcome.LoopBroken,
			new SeatLine[4]
			{
				L("The Key and the Gate", "stops rotating. For the first time in this broadcast, it is entirely still: \"The causal lattice does not close. I have no next iteration to open. There is no Soul Iteration of this era to observe.\""),
				L("The Laughing King", "sits down very slowly, and for once he is not laughing: \"...Huh. I've been running this show for nine hundred years, kid. I always knew how it ended. I don't know how this one ends. Gods help me, I have no idea what happens tomorrow.\""),
				L("The Inheritor", "sheathes her blade and bows her head: \"I have watched Kenabres fall four hundred times, and four hundred times I have said the same words to a soul who could not hear me. Today I say something else. Thank you.\""),
				L("The Lady of Graves", "closes the ledger without writing: \"The circular record ends. This soul returns to the River in its proper order, at its proper hour, like every other. That is not a punishment, traveler. It is the thing you were owed.\"")
			}
		},
		{
			EndingLoopOutcome.CycleResolves,
			new SeatLine[4]
			{
				L("The Lady of Graves", "writes one line and closes the ledger: \"Cycle recorded. Archive it with the others.\""),
				L("The Lucky Drunk", "stares into an empty tankard: \"That was a good one. That was a really good one. And in a minute every soul down there forgets it happened, and I'm the only one at this table who'll still be able to tell you how it went. Barkeep. Leave the bottle.\""),
				L("The Laughing King", "spins the reel back to the first frame, grinning too wide: \"And that's a wrap! Beautiful work, everyone! Reset the stage, wipe the boards, festival day in Kenabres, camera one! Places, people! PLACES!\""),
				L("The Key and the Gate", "folds the hourglass over: \"The anchors hold. The era begins again with nothing of it kept.\"")
			}
		},
		{
			EndingLoopOutcome.WorldEnds,
			new SeatLine[3]
			{
				L("The Silence Between the Stars", "whispers, barely there: \"There is no one down there to say anything now. Only us, and the quiet.\""),
				L("The Laughing King", "laughs, and it comes out wrong: \"...Okay. Okay! That's a first. Huge finish. Enormous. Nobody left in the audience shot, which is a problem for the ratings, but - huge finish.\""),
				L("The Lady of Graves", "speaks with absolute frost: \"An entire era's souls arrive at my gate in one hour, out of order, ahead of their hours. I will see every one of them seated. And then the hourglass will turn, and I will do it again.\"")
			}
		},
		{
			EndingLoopOutcome.ReachFallsShort,
			new SeatLine[3]
			{
				L("The Key and the Gate", "does not move: \"Insufficient. The Keys require every seal and every trial this cycle offered. The declaration is recorded. The anchors are not.\""),
				L("The Prince of Darkness", "collects his winnings without pleasure: \"I wagered against you and I am not enjoying it. You reached for the lock with half a key, mortal. Next cycle, count your pieces before you reach.\""),
				L("The Lucky Drunk", "puts the tankard down hard: \"No - no, come ON! They were RIGHT THERE! ...Ah, hell. Next time, kid. There's always a next time. That's the whole damn problem.\"")
			}
		}
	};

	internal static readonly IReadOnlyDictionary<string, string> PatronLoopBroken = new Dictionary<string, string>(StringComparer.Ordinal)
	{
		{ "The Lucky Drunk", "raises the last full tankard in the house: \"I backed you on day one, when you were bleeding out on a festival stretcher and I put ten gold on you shouting for a status window. Best coins I ever spent. Go on. Go be free.\"" },
		{ "The Inheritor", "kneels, which she has not done in four hundred iterations: \"You carried my sigil out of a story that was written before you arrived. I release you from it. Walk freely.\"" },
		{ "The Prince of Darkness", "signs something and slides it into the fire: \"Our contract had a termination clause I never expected to invoke. Consider it discharged in full. You were an excellent investment, and I do not say that.\"" },
		{ "The Song of the Spheres", "lets the starlight go out of her hands: \"Every road you walked was one someone else had walked for you. This is the first step of yours that nobody has ever taken. Oh, go and see everything.\"" },
		{ "The Lady of Graves", "speaks, for once, without frost: \"You arrived out of order, and you leave in order. That is all I have ever wanted for you. Go to your proper rest, in your proper hour.\"" },
		{ "The Savored Sting", "applauds with genuine, unspiteful delight: \"You stung the thing that owned you. Darling, I have waited an age for someone to be that petty on a cosmic scale. I am so proud I could spit.\"" },
		{ "The All-Seeing Eye", "both eyes agreeing for once, murmurs: \"I have seen every outcome of this era. I had not seen this one. Do you understand what you have given me? A thing I did not already know.\"" },
		{ "Our Lord in Iron", "strikes his gauntlets together once, like a salute: \"YOU DID NOT DIE A THOUSAND TIMES. YOU DIED ONCE, AND YOU MEANT IT. THAT IS A WARRIOR'S DEATH AND I HONOR IT!\"" },
		{ "The Pirate Queen", "tips her tricorn and cuts a rope: \"You sailed straight off the edge of the chart, captain. There's no map past here and no one's drawn one. That's the only sea worth having.\"" },
		{ "The Laughing King", "takes the crown off, which nobody has ever seen him do: \"You were my favorite act. You were my favorite act and you walked off the stage and out the door. ...That's the best joke anyone's ever told me. And it's on me.\"" },
		{ "The Mischievous Friend", "bursts into tears and cartwheels anyway: \"YOU DID IT! You actually... I've been carrying this clover for you since the FIRST loop and it finally WORKED! Go on! Go! Don't look back, it's bad luck!\"" },
		{ "The Everbloom", "scatters petals that do not fall back: \"You were kind inside a cage, which is the only kindness that ever counted. And then you broke the cage. That's my whole faith in one life, love.\"" },
		{ "The Silence Between the Stars", "whispers, and the whisper carries: \"You are the first thing in this era to go somewhere I cannot follow. Good. Go where I am not.\"" },
		{ "The Key and the Gate", "turns every facet toward you at once, and for the first time it sounds like something that can lose: \"You were my experiment. You were the finest variable I ever introduced into a closed system. And you opened it. I did not account for that. I do not know what I am without a next iteration.\"" },
		{ "The Pallid Princess", "sets down a cup she has been holding for a very long time: \"No more second helpings. No more coming back. It all ends once now, my dear, the way a meal should. I think I will miss it.\"" }
	};

	internal static readonly IReadOnlyDictionary<string, string> PatronCycleResolves = new Dictionary<string, string>(StringComparer.Ordinal)
	{
		{ "The Lucky Drunk", "drinks alone: \"I backed you from the stretcher to the Threshold, kid. You did right by me. Next cycle you won't know my name, but I'll know yours. I'll put the ten gold down again.\"" },
		{ "The Inheritor", "salutes: \"You bore my sigil faithfully to the end. The crusade is served. Rest, Knight-Commander. You have earned an ending, even one we must watch again.\"" },
		{ "The Prince of Darkness", "rules a neat line under the account: \"Terms met. Obligations discharged. You were competent, mortal, which from me is an extravagance. The ledger carries forward.\"" },
		{ "The Song of the Spheres", "settles starlight on the closing frame: \"You were dear to me, traveler. In a little while you will begin again and not know me, and I will know you from the first step. I will love you then too.\"" },
		{ "The Lady of Graves", "inscribes the name in full: \"Your soul is accounted for, in a ledger that will be reopened. I do not forget an entry, traveler, even when the world does.\"" },
		{ "The Savored Sting", "yawns luxuriously: \"Marvelous spite, start to finish. Do it all again, sweet thing, and do it meaner.\"" },
		{ "The All-Seeing Eye", "murmurs in two voices at once: \"One more solved configuration. The spell resolves as cast. Cast it differently next time; I am curious, which is rare.\"" },
		{ "Our Lord in Iron", "roars approval: \"A WORTHY CAMPAIGN, MORTAL! COME BACK AND FIGHT IT AGAIN! I WILL BE HERE, AND SO WILL THE ENEMY!\"" },
		{ "The Pirate Queen", "raises a glass to the closing frame: \"A fine raid, captain. Same waters next tide. I'll be waiting at the same port, same as always.\"" },
		{ "The Laughing King", "winks at the lens: \"My star! My favorite! Don't worry, I keep every reel. I'll play you again on a slow night, and you'll be brilliant all over again, and you won't know.\"" },
		{ "The Mischievous Friend", "sniffles and polishes the clover: \"That was a good run! A really good run! I'm saving all the luck I've got left for next time, okay? All of it. Promise.\"" },
		{ "The Everbloom", "lays a single rose on the closing frame: \"You were kind in a place that made it expensive. Whatever they record, I keep that. I always keep that.\"" },
		{ "The Silence Between the Stars", "whispers: \"Quiet now. You were never alone in it. You will not be alone in the next one either.\"" },
		{ "The Key and the Gate", "folds the record away: \"The variable performed within tolerance. The system resets. You will be introduced again, and I will watch again, and it will be nearly the same. Nearly is the only thing I have ever found interesting.\"" },
		{ "The Pallid Princess", "smiles slowly: \"Every life you took this cycle, you will take again. I will be here with a clean plate. Come hungry.\"" }
	};

	internal static readonly IReadOnlyDictionary<string, string> OffendedClosing = new Dictionary<string, string>(StringComparer.Ordinal)
	{
		{ "The Lucky Drunk", "does not raise his tankard: \"Yeah. Congratulations. I'm not drinking to it.\"" },
		{ "The Inheritor", "turns away before the last frame: \"I will not witness this. Let someone else record it.\"" },
		{ "The Prince of Darkness", "closes the ledger on an unpaid account: \"Our business ends unsettled, mortal. I am patient. Accounts of mine have outlived eras.\"" },
		{ "The Song of the Spheres", "looks away, and the starlight goes with her: \"I wanted to be wrong about you. I hate being right about a traveler.\"" },
		{ "The Lady of Graves", "does not look up: \"Recorded. Nothing more is owed to you here.\"" },
		{ "The Savored Sting", "inspects her nails: \"Mm. I've been bored of you for a while now, darling. Do finish.\"" },
		{ "The All-Seeing Eye", "murmurs, one eye shut: \"I watched. I am under no obligation to approve of what I watched.\"" },
		{ "Our Lord in Iron", "spits: \"NO SONG FOR THIS ONE. LET IT END QUIETLY, AS IT DESERVES.\"" },
		{ "The Pirate Queen", "turns her back to the lens: \"Struck my colors on you a long way back. Sail on, then.\"" },
		{ "The Laughing King", "yawns theatrically: \"Aaand cut. Honestly? I stopped watching a few acts ago. Somebody wake me for the next one.\"" },
		{ "The Mischievous Friend", "hides the clover behind her back: \"...I don't have any luck for you. I'm sorry. I gave it to someone else.\"" },
		{ "The Everbloom", "lets the petals fall short: \"I hold up the ones who don't do what you did. Go on. You don't need me.\"" },
		{ "The Silence Between the Stars", "says nothing at all, for a long moment, and then the quiet closes over the broadcast." },
		{ "The Key and the Gate", "has already turned away: \"Data collected. The subject is no longer of interest.\"" },
		{ "The Pallid Princess", "pushes the plate away: \"You were never hungry enough, darling. How dull.\"" }
	};

	internal const string SeatTagName = "Constellation.SeatTag";

	internal const string EndingHeaderName = "Constellation.EndingHeader";

	internal const string EndingLineName = "Constellation.Ending";

	internal static readonly SeatLine[] VanillaFarewell = new SeatLine[4]
	{
		L("The Laughing King", "squints at the lens: \"Wait - that's it? They just... finished it? No declaration, no grand statement, they just played it out to the last frame and went home?\""),
		L("The Inheritor", "lowers her blade: \"Yes. That is what a crusader does. Not every ending needs a speech, jester.\""),
		L("The Lucky Drunk", "raises his tankard quietly: \"To the one who did the job and didn't ask us for anything. Rarest run we've had.\""),
		L("The Key and the Gate", "folds the hourglass over: \"No terminus declared, so nothing of this cycle enters the record. The hourglass turns all the same.\"")
	};

	internal static readonly SeatLine AlreadyClosed = L("The Key and the Gate", "does not turn: \"This cycle is already closed. The record does not accept a second terminus.\"");

	public static List<string> GetAlignmentBanter(AlignmentShiftDirection shift, string answerText, out int coinsAwarded, out string primarySponsor)
	{
		List<string> list = new List<string>();
		coinsAwarded = 0;
		primarySponsor = null;
		switch (shift)
		{
		case AlignmentShiftDirection.Chaotic:
			coinsAwarded = 100;
			primarySponsor = "The Lucky Drunk";
			switch (_rng.Next(5))
			{
			case 0:
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> roars with laughter and slams his tankard: \"Hah! Now that's what I call flair! Rules are just suggestions written by people with no imagination. Drink up!\""));
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> rubs her brow with a heavy gauntlet: \"@The Lucky Drunk, show some decorum. The Worldwound is burning and you are treating crusader discipline like a tavern brawl.\""));
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color> sneers into his goblet: \"Let the clown dance, Inheritor. Reckless audacity invariably rushes headlong into an early grave.\""));
				break;
			case 1:
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Savored Sting']</b></color> giggles and plays with a silver wasp: \"Oh, that was delightfully impudent! Keep stirring the hornet's nest, darling, it makes the honey that much sweeter.\""));
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> winks: \"Hear that? Even Calistria likes the cut of your jib! Take these coins and buy something that explodes!\""));
				break;
			case 2:
				primarySponsor = "The Song of the Spheres";
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color> hums a breezy tune of starlight: \"A heart unfettered by chains is a beautiful sight. May the winds of freedom carry your bold steps forward!\""));
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Key and the Gate']</b></color> rotates across non-Euclidean facets: \"The predictable timeline fractures into 14 novel permutations. Unbound variance detected.\""));
				break;
			case 3:
				primarySponsor = "The Pirate Queen";
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Pirate Queen']</b></color> fires a pistol into the air: \"Ha! Tear up the charts and sail where you please! That's the spirit of a true buccaneer!\""));
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> sighs heavily: \"Must every act of defiance be cheered like a successful raid?\""));
				break;
			default:
				primarySponsor = "The Mischievous Friend";
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#004604><b>[The Constellation 'The Mischievous Friend']</b></color> cartwheels along the celestial rail: \"Ooh, bold and reckless! Luck loves a daring heart, and yours is doing somersaults!\""));
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> laughs: \"That's the Mischievous Friend's favorite kind of trouble right there!\""));
				break;
			}
			break;
		case AlignmentShiftDirection.Good:
			coinsAwarded = 100;
			primarySponsor = "The Inheritor";
			switch (_rng.Next(4))
			{
			case 0:
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> nods in solemn approval: \"A heart that chooses mercy and honor in the face of despair is the true armor of a crusader. Stand tall.\""));
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color> showers starlight upon your shoulders: \"Even in the bleak mud of the Worldwound, compassion blooms like a midnight lily. Shine on, traveler!\""));
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Savored Sting']</b></color> rolls her eyes: \"Ugh, you do-gooders are giving me a headache. Where is the spite? Where is the betrayal? How terribly wholesome.\""));
				break;
			case 1:
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color> smiles warmly: \"No darkness is absolute while someone still carries a torch for others. A gift from the stars to light your way.\""));
				primarySponsor = "The Song of the Spheres";
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color> scoffs softly: \"Selfless charity... the favorite luxury of fools who expect gratitude from mortals. How quaint.\""));
				break;
			case 2:
				primarySponsor = "The Silence Between the Stars";
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Silence Between the Stars']</b></color> whispers like quiet velvet: \"A gentle silence falls upon suffering souls. In the quiet between stars, your mercy is remembered.\""));
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> raises her golden longsword: \"May your blade defend those who cannot defend themselves.\""));
				break;
			default:
				primarySponsor = "The Everbloom";
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Everbloom']</b></color> scatters rose petals over the broadcast: \"Kindness chosen in the dark is its own quiet rebellion. Hold on to it, and never let a tyrant take it from you.\""));
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> raises his tankard: \"Hear, hear! To the Everbloom, and every stubborn heart that refuses to break!\""));
				break;
			}
			break;
		case AlignmentShiftDirection.Evil:
			coinsAwarded = 150;
			primarySponsor = "The Prince of Darkness";
			switch (_rng.Next(3))
			{
			case 0:
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color> smiles with chilling satisfaction: \"Exquisite ruthlessness. Sentimental weakness is the rot of kingdoms. When you strike, leave no ambiguity as to who rules.\""));
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with cold anger: \"You tread the precipice of damnation, mortal. Power stripped of righteousness is merely savagery.\""));
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> grumbles, pushing his ale aside: \"Oof. Even my drink tasted sour after that one. You're walking a dark, ugly road, kid.\""));
				break;
			case 1:
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Savored Sting']</b></color> purrs with venomous delight: \"Now THAT had some sting! Cold, calculated, and delightfully cruel. I do love watching someone drop the pretense of holiness.\""));
				primarySponsor = "The Savored Sting";
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color> sighs softly: \"A shadow falls over your spirit... do not let the abyss swallow the light you brought from your world.\""));
				break;
			default:
				primarySponsor = "The Lady of Graves";
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Lady of Graves']</b></color> speaks with absolute frost: \"Another stone cast into the balance. Every soul reaps what it sows in the Boneyard. Remember this.\""));
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color> laughs quietly: \"Keep playing the board with such clarity, incarnation. Power belongs to those bold enough to claim it.\""));
				break;
			}
			break;
		case AlignmentShiftDirection.Lawful:
			coinsAwarded = 100;
			primarySponsor = "The Inheritor";
			switch (_rng.Next(3))
			{
			case 0:
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> salutes with her blade: \"Order, duty, and oaths unbroken. Through discipline, the Crusade endures where passion crumbles.\""));
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color> nods approvingly: \"Indeed. Contracts and structure govern reality itself. Even the Inheritor understands the virtue of obedience.\""));
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> groans dramatically: \"Great, now the contract lawyers and church elders are high-fiving. Somebody pass me a stronger keg.\""));
				break;
			case 1:
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color> writes a glyph in crimson ink: \"A disciplined mind is a magnificent instrument. Never allow chaotic rabble to dictate your terms.\""));
				primarySponsor = "The Prince of Darkness";
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Savored Sting']</b></color> yawns behind her sleeve: \"So rigid. Where is the fun if you follow every little rule? Break something, just once!\""));
				break;
			default:
				primarySponsor = "The All-Seeing Eye";
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>, eyes shifting between order and madness, murmurs: \"Structure is merely a spell cast upon chaos. A well-woven one, this time. Continue the incantation.\""));
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> pouts: \"Rules, rules, rules. Somebody wake me when they break one!\""));
				break;
			}
			break;
		default:
			if (IsFeastAnswer(answerText))
			{
				coinsAwarded = 150;
				primarySponsor = "The Pallid Princess";
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'The Pallid Princess']</b></color> lifts a goblet of something that was recently alive: \"Appetite. At last, something honest on this broadcast. Eat, rot, rise again; that is the only cycle worth keeping.\""));
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Lady of Graves']</b></color> does not look up from her ledger: \"Pallid Princess. Every guest at your table is an entry I have to reopen.\""));
			}
			else if (IsOriginWorldAnswer(answerText))
			{
				coinsAwarded = 200;
				primarySponsor = "The Key and the Gate";
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Key and the Gate']</b></color> unfolds cosmic spheres of iridescent light: \"The memory of the origin world surfaces like an oil slick upon the dimensional tide. We observe your transit with keen calculation.\""));
				if (MentionsTruck(answerText))
				{
					list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> blinks: \"Wait, a mechanical carriage did what now?! Man, whatever god invented 'Trucks' clearly knows how to start a party with a bang!\""));
				}
				list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Lady of Graves']</b></color> gazes down impassively: \"A soul ripped across realities without passing through the River. A strange thread tied into Golarion's knot.\""));
			}
			break;
		}
		return list;
	}

	internal static bool IsFeastAnswer(string answerText)
	{
		if (!string.IsNullOrEmpty(answerText))
		{
			return FeastWords.IsMatch(answerText);
		}
		return false;
	}

	internal static bool IsOriginWorldAnswer(string answerText)
	{
		if (!string.IsNullOrEmpty(answerText))
		{
			return OriginWorldWords.IsMatch(answerText);
		}
		return false;
	}

	internal static bool MentionsTruck(string answerText)
	{
		if (!string.IsNullOrEmpty(answerText))
		{
			return TruckWord.IsMatch(answerText);
		}
		return false;
	}

	public static List<string> GetDeathReloadBanter(int deathCount, string lastArea)
	{
		List<string> list = new List<string>();
		switch (_rng.Next(6))
		{
		case 0:
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> winces and rubs his temples: \"Ouch... I felt that hit all the way from Elysium! Let's see if the little 'hero' actually makes it this time! All that power you have and still you fail! Don't let me down, I put fifty coins on you!\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks firmly: \"Defeat is merely a forge for the soul. Stand up, dust off your armor, and face your enemy with discipline. No more reckless blunders.\""));
			break;
		case 1:
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color> sips wine with cold, theatrical amusement: \"Back already? How delightful. Rewinding the timeline does not cure a fundamental deficiency in intellect, mortal. Perhaps try using your brain before your blade.\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Savored Sting']</b></color> snickers: \"Oh, that was embarrassing! Sliced into ribbons in under six seconds. Please tell me you have an actual plan this time, or should I order more snacks?\""));
			break;
		case 2:
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Lady of Graves']</b></color> turns an ancient page in her ledger: \"Your soul approached the gates of my court, yet an anomaly drags you back into the mortal coil once more. Do not test the patience of the River, traveler.\""));
			list.Add(ConstellationText.LocalizedFormat("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Key and the Gate']</b></color> hums through spatial folds: \"Vessel collapse #{0} recorded at {1}. Reality re-anchored. Present a superior causal outcome.\"", deathCount, lastArea));
			break;
		case 3:
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> cackles hysterically: \"PWAHAHAHA! Oh, that was tragic! Did you see the look on your face right before the axe landed?! Rewind the tape, gods, I want to watch that again!\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#004604><b>[The Constellation 'The Mischievous Friend']</b></color> winces behind her hands: \"Ooooh, bad roll! Don't listen to the Laughing King, hero! My lucky clover's with you this time. Charge right back in!\""));
			break;
		case 4:
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'Our Lord in Iron']</b></color> roars with thunderous fury: \"PATHETIC! A TRUE WARRIOR DIES ONCE, BUT A LOOPING COWARD DIES A THOUSAND TIMES! STAND UP, GRIP YOUR STEEL, AND CARVE THEIR BONES TO DUST!\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Pirate Queen']</b></color> laughs brazenly: \"Walked the plank so soon? Even a scuttled ship can be raised for another raid. Scramble up the rigging and fire a broadside!\""));
			break;
		default:
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Savored Sting']</b></color> giggles with sharp glee: \"Round two! Or is it round three? I've lost count of how many times you've kissed the dirt. Show them some teeth this time!\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>, eyes shifting in dual madness, murmurs: \"The threads of your demise collapse into the harmonic frequencies of the loop. Fascinating. Attempt the convergence again.\""));
			break;
		}
		return list;
	}

	public static List<string> GetActProgressionBanter(int act)
	{
		List<string> list = new List<string>();
		switch (act)
		{
		case 1:
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> raises a glass: \"Look at our new star stepping into Kenabres! We've seen this siege fail fifty times, but this kid doesn't even know the script! I've got high hopes!\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> observes solemnly: \"The city has fallen, but valor may yet prevail. Show these crusaders the fire of your convictions.\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> whispers playfully: \"Keep smiling, hero. Out of all the iterations we've watched, nobody ever makes choices as weird as yours! Keep the cameras rolling!\""));
			break;
		case 2:
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color> writes in his ledger: \"Advancing into the march on Drezen. I wager 500 soul-shards they succumb to impatience before the citadel falls.\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color> smiles: \"I'll take that bet, Prince of Darkness! The Knight-Commander's heart burns brighter than your cynicism.\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> laughs: \"Wait, which ending is this run heading for? Don't spoil it, let me guess!\""));
			break;
		case 3:
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>, eyes flashing with arcane storm, murmurs: \"The causal fabric of Drezen is wearing thin. The mortal begins to sense the unnatural echoes of the loop.\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> chuckles: \"Uh oh... someone left the script on the table! Don't read ahead, kid, you'll spoil the punchline!\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> warns: \"Knight-Commander, do not let planar anomalies distract you from your holy crusade.\""));
			break;
		case 4:
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Pirate Queen']</b></color> looks around Alushinyrra: \"Welcome to the belly of the beast! Shadows, cutthroats, and demonic intrigue... now THIS is my kind of port!\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Savored Sting']</b></color> smiles seductively: \"The mortal holds the Key of Causal Rupture. They know now. The audience is holding its breath.\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Key and the Gate']</b></color> echoes from the outer void: \"The closed ring approaches its inflection point. The traveler gazes outward toward the threshold.\""));
			break;
		case 5:
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with quiet awe: \"The Threshold is reached. But this time... the Knight-Commander looks beyond the Worldwound itself.\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> leans forward eagerly: \"Are they really going to do it?! Are they going to challenge the Grand Arbiter and smash the stage?!\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> slams his fists on the celestial rail: \"DO IT! SMASH THE WHOLE DAMN APPARATUS! SHOW 'EM WHAT FREE WILL TASTES LIKE!\""));
			break;
		}
		return list;
	}

	public static List<string> GetMetaLoopBanter(int runCount)
	{
		List<string> list = new List<string>();
		list.Add("<color=#5B2A86><b>=======================================================</b></color>");
		if (runCount > 1)
		{
			list.Add(ConstellationText.LocalizedFormat("Constellation.Banter", "<color=#5B2A86><b>[TEMPORAL CYCLE DETECTED] This Soul's Loop #{0} begins!</b></color>", runCount));
			if (TimelineManager.Data.LoopShattered)
			{
				list.Add(ConstellationText.LocalizedFormat("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Key and the Gate']</b></color> opens an infinity of non-Euclidean eyes upon you: \"Soul Iteration #{0}. The Sovereign returns to the fractured nexus. An unscripted paradox walking among mortals.\"", runCount));
				list.Add(ConstellationText.LocalizedFormat("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> roars with delight: \"LOOK WHO'S BACK! The legend who actually shattered the loop! Barkeep, pour {0} rounds for the champion!\"", runCount));
				list.Add(ConstellationText.LocalizedFormat("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> chuckles warmly: \"Back for an encore in Loop #{0}? Let's see what chaotic trouble you can cook up this time!\"", runCount));
			}
			else
			{
				list.Add(ConstellationText.LocalizedFormat("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Key and the Gate']</b></color> opens an infinity of non-Euclidean eyes upon you: \"Soul Iteration #{0} begins. The cosmic spindle rewinds. The soul's thread reweaves into the tapestry once more. Let us observe if this iteration resolves the paradox... or repeats the choreography.\"", runCount));
				list.Add(ConstellationText.LocalizedFormat("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> squints through his foaming tankard: \"Hold on... haven't I cheered for you before? Loop #{0}?! Either I drank way too much Starstone brew, or we're running in circles!\"", runCount));
				list.Add(ConstellationText.LocalizedFormat("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> laughs from his golden perch: \"Welcome back to Loop #{0}, folks! Grab your popcorn, the hero thinks this is their first time! Let's see if they remember to seal every rift and win the Fae Grove before challenging the Arbiter! Bwahaha!\"", runCount));
				list.Add(ConstellationText.LocalizedFormat("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Lady of Graves']</b></color> records in her solemn ledger: \"Entry recorded: Soul Iteration #{0}. An unnatural circular record outside the River of Souls... yet tolerated by the Outer Court.\"", runCount));
				list.Add(ConstellationText.LocalizedFormat("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color> smiles with dangerous calculation: \"Loop #{0}. How many cycles must transpire before this soul learns from past blunders? Incomplete vectors will simply collapse into another restart.\"", runCount));
			}
		}
		else
		{
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[TEMPORAL CYCLE INITIATED] This Soul's Loop #1 begins!</b></color>"));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Key and the Gate']</b></color> opens an infinity of non-Euclidean eyes upon you: \"Soul Iteration #1. Plucked across the cosmic threshold into the closed circle. The play begins.\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> whispers playfully: \"A fresh actor on the grand stage! Let's see how long before they realize the play has already been written!\""));
		}
		list.Add("<color=#5B2A86><b>=======================================================</b></color>");
		return list;
	}

	public static double RollMultiplier(string primarySponsor, out string shoutoutMsg)
	{
		shoutoutMsg = null;
		int num = _rng.Next(100);
		if (num < 2)
		{
			shoutoutMsg = ConstellationText.LocalizedFormat("Constellation.Banter", "<color=#5B2A86><b>[Constellation JACKPOT!]</b></color> <b>{0}</b> dropped a <b>3.0x Jackpot Multiplier</b> on your broadcast! The entire celestial sphere is showering coins!", primarySponsor);
			return 3.0;
		}
		if (num < 10)
		{
			shoutoutMsg = ConstellationText.LocalizedFormat("Constellation.Banter", "<color=#5B2A86><b>[Constellation Mega Donation!]</b></color> <b>{0}</b> boosted their sponsorship with a <b>2.0x Multiplier</b>!", primarySponsor);
			return 2.0;
		}
		if (num < 30)
		{
			shoutoutMsg = ConstellationText.LocalizedFormat("Constellation.Banter", "<color=#5B2A86><b>[Constellation Super Chat!]</b></color> <b>{0}</b> added a <b>1.5x Multiplier</b>!", primarySponsor);
			return 1.5;
		}
		return 1.0;
	}

	public static List<string> GetWardstoneMythicAwakeningBanter(string primarySponsor)
	{
		return new List<string>
		{
			"<color=#5B2A86><b>=======================================================</b></color>",
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[UNSCRIPTED CAUSAL ANOMALY DETECTED] The Wardstone Climax Shatters the Standard Loop!</b></color>"),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> leaps onto the celestial table: \"HOLD THE PHONE! Did you see that?! That was NOT in the director's cut! Out of four hundred crusades, no one has ever done THAT! Minagho looks like she swallowed a lemon! PAY UP, ASMODEUS!\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> slams his fists in ecstasy: \"I KNEW IT! Putting coins on the wildcard always pays out! Look at that spark take hold! Barkeep, drinks for the entire gallery!\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color> writes a ledger note with chilling intrigue: \"An unprecedented vector deviation from the standard loop. Highly irregular... and exceptionally profitable. Very well, jester. I concede the wager.\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with quiet, stunned reverence: \"In all the recorded iterations of Kenabres, I have never seen a mortal seize the celestial flame with such unpredictable resolve.\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Key and the Gate']</b></color> unfolds radiant multi-dimensional geometries: \"The causal lattice fractures into 4,096 unscripted branches. The recorded script is nullified. An authentic anomaly walks Golarion.\""),
			"<color=#5B2A86><b>=======================================================</b></color>"
		};
	}

	public static List<string> GetSlimeRevealBanter()
	{
		return new List<string>
		{
			"<color=#5B2A86><b>=======================================================</b></color>",
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[CONSTELLATION DISCOVERY] The True Nature of the Slime is Witnessed!</b></color>"),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> laughs hysterically: \"WAIT! DID YOU SEE THAT?! The mortal is a giant sentient jelly! Look at the paladin's face! She thinks her holy crusade is being saved by green gelatin! PWAHAHAHA!\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles: \"Hey, if it can hold a flagon of ale and dissolve a glabrezu, it's a drinking buddy in my book! Just... don't melt my tankard, kid!\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color> smirks coldly: \"An amorphous heteromorphic predator concealing itself as a mortal crusader. Deceptive, highly resilient, and distinctly entertaining to observe.\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color> whispers in dual voices: \"A fluid cellular matrix immune to physical cleavage. Truly fascinating biological transmutation.\""),
			"<color=#5B2A86><b>=======================================================</b></color>"
		};
	}

	public static List<string> GetAngelVisionBanter(bool devoured)
	{
		List<string> list = new List<string>();
		list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Lady of Graves']</b></color> regards the vision without warmth: \"A soul's final thought, delivered to an address that does not appear in my ledger. It reached you because you arrived here the same way it did. Out of order.\""));
		list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Key and the Gate']</b></color> rotates through folded geometries: \"The angel dies at a fixed point. You did not. The two of you are reading the same page from opposite sides of the paper.\""));
		if (devoured)
		{
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> shrieks with delight: \"The jelly EATS a dying angel's last memory! I have been running this broadcast for nine hundred years and that is a first! Somebody fetch me a scoreboard!\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'The Pallid Princess']</b></color> smiles slowly: \"Consumption as remembrance. Nothing honors the dead like keeping them, my dear.\""));
		}
		else
		{
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color> settles like starlight on your shoulders: \"You held it instead of fighting it. That is the kindest thing anyone has done for Lariel in a very long time.\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> lowers her blade a fraction: \"A stranger's death, carried willingly. I had not expected that from the one who fell out of the sky.\""));
		}
		return list;
	}

	public static List<string> GetOverlordSkeletonRevealBanter()
	{
		return new List<string>
		{
			"<color=#5B2A86><b>=======================================================</b></color>",
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[CONSTELLATION DISCOVERY] The True Nature of the Skeletal Overlord is Witnessed!</b></color>"),
			ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'The Pallid Princess']</b></color> purrs with dark delight: \"Oh, magnificent! Look at that exquisite ivory skull! An absolute sovereign of death masquerading among mortal crusaders! Death always claims the final victory!\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Lady of Graves']</b></color> speaks in grave judgment: \"A heteromorphic soul of undeath walking under the guise of living flesh... The cosmic scales tilt erratically. See to it that you destroy the demonic rift before you test my patience, Overlord.\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> scratches his beard: \"Wait, our hero is literally a skeleton wearing an illusion cloak?! How do they even drink ale without it splashing all over their ribs?! Put fifty coins on the bone lord!\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> cackles: \"An undead Overlord in Kenabres! Look at Seelah having an existential panic attack! 'Is that a demon? Is that a skeleton? Why is it saving my life?!' Best episode ever!\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color> nods in deep appreciation: \"Supreme Overlord of the Great Tomb. Unyielding order, discipline, and ruthless hierarchy. Now that is a commander worthy of a real contract.\""),
			"<color=#5B2A86><b>=======================================================</b></color>"
		};
	}

	public static List<string> GetPastCycleRecognitionBanter()
	{
		List<string> list = new List<string>();
		PastCycleRecord lastCycle = TimelineManager.GetLastCycle();
		string text = lastCycle?.Archetype ?? "Unknown";
		string text2 = lastCycle?.EndingAchieved ?? "Unrecorded";
		int totalRuns = TimelineManager.TotalRuns;
		list.Add("<color=#5B2A86><b>=======================================================</b></color>");
		list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[CONSTELLATION RECOGNITION] Echoes of Past Lifetimes Detected!</b></color>"));
		list.Add(ConstellationText.LocalizedFormat("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Key and the Gate']</b></color> manifests as interlocking tesseracts: \"Iteration #{0} initiated. Subject retains memory fragments of prior vessel [{1}] and terminus [{2}]. The causality loop vibrates with resonance.\"", totalRuns, text, text2));
		list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> chuckles knowingly: \"Look at them! Back again for another round in the sandbox! Did you miss the mud of Kenabres that much, or did you just want to punch Minagho again?\""));
		list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color> sips leisurely from his chalice: \"The memory of prior lifetimes is the greatest ledger of all. Let us see if this iteration yields greater discipline or greater folly.\""));
		list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with quiet wonder: \"To shoulder the weight of an entire crusade once is heroic. To step once more into the crucible... that is the mark of an indomitable soul.\""));
		if (TimelineManager.LoopBreakerAchieved)
		{
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Silence Between the Stars']</b></color> murmurs with deep celestial grace: \"The loop was shattered, yet you chose to return to our stars. Welcome back, sovereign wanderer.\""));
		}
		list.Add("<color=#5B2A86><b>=======================================================</b></color>");
		return list;
	}

	public static List<string> GetLoopBreakerSovereignReturnBanter()
	{
		return new List<string>
		{
			"<color=#5B2A86><b>=======================================================</b></color>",
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[CONSTELLATION SOVEREIGN RETURN] The Shattered Loop Acknowledged!</b></color>"),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Key and the Gate']</b></color> revolves in crystalline harmony: \"The causal barrier remains broken. Subject transmigrated by voluntary will rather than recursive compulsion. Shard of the Shattered Loop manifested.\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> laughs with unrestrained glee: \"You actually came back! You broke out of the cosmic fishbowl and then jumped right back into the water! Absolute legend! Take these coins, you earned them!\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Pirate Queen']</b></color> tips her tricorn: \"A captain who sails the outer dark and chooses to visit our waters once more? Now that is the kind of reckless sovereign I respect!\""),
			"<color=#5B2A86><b>=======================================================</b></color>"
		};
	}

	public static List<string> GetPrologueAwakeningBanter(out int coinsAwarded, out string primarySponsor)
	{
		coinsAwarded = 150;
		primarySponsor = "The Laughing King";
		return new List<string>
		{
			"<color=#5B2A86><b>=======================================================</b></color>",
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[CELESTIAL BROADCAST LIVE] Chapter 0: The Transmigrant Awakens!</b></color>"),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> taps the cosmic broadcast lens: \"Camera one is live! Look at that, folks! Our brand-new transmigrant finally opened their eyes on a festival stretcher! No cheat status screen, no explanation manual, and a gaping hole in their chest. Classic zero-star summoning!\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> laughs into his tankard: \"Hey, give the kid credit for breathing! Ten gold coins says they try to shout 'Status Window Open!' before realizing this world does not have floating blue interface menus.\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Lady of Graves']</b></color> speaks with icy detachment: \"An unregistered soul crosses the threshold of mortality. Fate's ledger shivers with anomaly. Let us see if this foreign spark endures or returns swiftly to the Boneyard.\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Silence Between the Stars']</b></color> whispers softly: \"The morning sky above Kenabres smiles with festive silk. Cherish the sunlight, traveler. Festivals are brief, and the Worldwound is never far.\""),
			"<color=#5B2A86><b>=======================================================</b></color>"
		};
	}

	public static List<string> GetPrologueHulrunBanter(out int coinsAwarded, out string primarySponsor)
	{
		coinsAwarded = 100;
		primarySponsor = "The Prince of Darkness";
		return new List<string>
		{
			"<color=#5B2A86><b>=======================================================</b></color>",
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[CONSTELLATION BROADCAST] Inquisitorial Interrogation in the Square!</b></color>"),
			ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color> sneers into his goblet: \"Interrogating an unarmed convalescent in the middle of a festival. Order without intellect is merely blind zealotry.\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color> crackles with arcane static: \"That wound carries an abyssal signature. The Prelate senses something is wrong, but not where.\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#004604><b>[The Constellation 'The Mischievous Friend']</b></color> giggles merrily: \"Ooh, a scary old man with a pointy halberd! Do not let him intimidate you, rookie! Quick, tell him you lost your memory in an apple-eating contest!\""),
			"<color=#5B2A86><b>=======================================================</b></color>"
		};
	}

	public static List<string> GetPrologueTerendelevBanter(out int coinsAwarded, out string primarySponsor)
	{
		coinsAwarded = 100;
		primarySponsor = "The Inheritor";
		return new List<string>
		{
			"<color=#5B2A86><b>=======================================================</b></color>",
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[CONSTELLATION BROADCAST] Audience with the Silver Dragon Protector!</b></color>"),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> manifests a halo of golden warmth: \"Terendelev's radiant magic knits flesh and restores life. She has stood as the guardian shield of Kenabres for generations. Remember her benevolence, traveler.\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> leans forward, grinning wickedly: \"Aww, what a touching moment! A dragon in a festival gown patching up our rookie. Somebody clip that for the highlight reel!\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'Our Lord in Iron']</b></color> slams his heavy gauntlets: \"THE DRAGON'S SCALES SHINE BRIGHT! A GUARDIAN WORTH FIGHTING BESIDE! STAND UP, STRANGER, AND EARN HER MERCY!\""),
			"<color=#5B2A86><b>=======================================================</b></color>"
		};
	}

	public static List<string> GetPrologueDeskariInvasionBanter(out int coinsAwarded, out string primarySponsor)
	{
		coinsAwarded = 150;
		primarySponsor = "Our Lord in Iron";
		return new List<string>
		{
			"<color=#5B2A86><b>=======================================================</b></color>",
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[CONSTELLATION BROADCAST] Deskari Descends! Kenabres Fractures!</b></color>"),
			ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'Our Lord in Iron']</b></color> roars with primal fury: \"BLOOD AND ASH! THE LORD OF THE LOCUST STRIKES! TERENDELEV FALLS! TAKE UP STEEL, STRANGER, AND PROVE YOUR WILL TO SURVIVE!\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Key and the Gate']</b></color> pulsates in geometric vortexes: \"The dimensional crust fractures. Kenabres collapses along the abyssal seam. The transmigrant plunges into the subterranean abyss!\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color> sheds tears of falling stars: \"Terendelev... brave silver heart... Hold fast to courage, little traveler! Even in the lightless caverns of the deep, the stars will guide your way!\""),
			"<color=#5B2A86><b>=======================================================</b></color>"
		};
	}

	public static List<string> GetPrologueCavesAwakeningBanter(out int coinsAwarded, out string primarySponsor)
	{
		coinsAwarded = 100;
		primarySponsor = "The Lucky Drunk";
		return new List<string>
		{
			"<color=#5B2A86><b>=======================================================</b></color>",
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[CONSTELLATION BROADCAST] Chapter 0: Subterranean Survival!</b></color>"),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> munches on celestial popcorn: \"Welcome to the Cave Crawl Arc! A righteous paladin with a bruised ego, a half-elf scout with a crushed ankle, and a rookie who fell a whole mile without turning into jam. Peak entertainment!\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Lady of Graves']</b></color> observes with solemn chill: \"The ancient dragon was struck down, yet this fragile transmigrant survived the fall unbroken. The threads of destiny tighten in the dark.\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> raises his flagon: \"Surviving a drop like that without breaking your neck? That deserves a hearty round of ale! Go on, kid, help Seelah lift that boulder off Anevia!\""),
			"<color=#5B2A86><b>=======================================================</b></color>"
		};
	}

	public static List<string> GetPrologueCamelliaEncounterBanter(out int coinsAwarded, out string primarySponsor)
	{
		coinsAwarded = 150;
		primarySponsor = "The Savored Sting";
		return new List<string>
		{
			"<color=#5B2A86><b>=======================================================</b></color>",
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[CONSTELLATION BROADCAST] An Aristocrat in the Dark: Meeting Camellia!</b></color>"),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Savored Sting']</b></color> purrs with delicious amusement: \"My my, a noblewoman who keeps her composure beside a corpse in the dark. Such lovely manners, darling. I do adore a lady with secrets.\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color> writes with a quill of fire: \"Impeccable breeding, impeccable poise, and not one tremor in the voice. Nobility is a fine mask. Let us see what it covers.\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> laughs hysterically: \"PWAHAHA! A rapier, a ruined gown and perfect table manners in a cave! Keep her close, rookie, she is going to be wonderful for the ratings!\""),
			"<color=#5B2A86><b>=======================================================</b></color>"
		};
	}

	public static List<string> GetPrologueLannWenduagBanter(out int coinsAwarded, out string primarySponsor)
	{
		coinsAwarded = 120;
		primarySponsor = "The Song of the Spheres";
		return new List<string>
		{
			"<color=#5B2A86><b>=======================================================</b></color>",
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[CONSTELLATION BROADCAST] The Children of the First Crusade: Lann & Wenduag!</b></color>"),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> salutes with quiet reverence: \"Descendants of the First Crusade, preserving their honor in the damp gloom. Their cursed forms bear witness to ancient sacrifice. Treat them with respect.\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Savored Sting']</b></color> smirks with sly interest: \"The lizard boy yearns for heroic martyrdom, while the spider girl hungers for predatory strength. Why choose just one? Bring both and let their rivalry keep you entertained!\""),
			ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color> hums a comforting lullaby of stars: \"Even beneath miles of stone where no sun can reach, these forgotten children yearn for the sky. Guide their steps toward the light.\""),
			"<color=#5B2A86><b>=======================================================</b></color>"
		};
	}

	public static List<string> GetNeutralDialogueBanter(string answerText, out int coinsAwarded, out string primarySponsor)
	{
		coinsAwarded = 0;
		primarySponsor = "The Laughing King";
		List<string> list = new List<string>();
		switch (_rng.Next(3))
		{
		case 0:
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color> chuckles into his sleeve: \"A prudent, noncommittal reply. Our rookie plays their cards close to their chest!\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color> nods in agreement: \"Sometimes keeping your mouth shut and your eyes open is the best survival tactic.\""));
			break;
		case 1:
			primarySponsor = "The All-Seeing Eye";
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color> hums with intellectual curiosity: \"Observation precedes action. The mortal calculates their position in this foreign world.\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color> notes quietly: \"Measured composure. A wise choice when the surrounding players are yet unmapped.\""));
			break;
		default:
			primarySponsor = "The Mischievous Friend";
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#004604><b>[The Constellation 'The Mischievous Friend']</b></color> hops cheerfully: \"Sensible and steady! Fortune smiles on those who do not rush blindly into danger!\""));
			list.Add(ConstellationText.Localized("Constellation.Banter", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color> smiles warmly: \"Every quiet step along the path shapes the journey to come.\""));
			break;
		}
		return list;
	}

	private static SeatLine L(string seat, string text)
	{
		return new SeatLine(seat, text);
	}

	internal static bool IsWorldEndingEnding(string endingId)
	{
		if (!(endingId == "SeveredHorizon") && !(endingId == "AbyssalSingularity"))
		{
			return endingId == "NetherworldCataclysm";
		}
		return true;
	}

	internal static EndingLoopOutcome DecideEndingOutcome(string endingId, bool loopBreakingEnding, bool shattered)
	{
		if (loopBreakingEnding)
		{
			if (!shattered)
			{
				return EndingLoopOutcome.ReachFallsShort;
			}
			return EndingLoopOutcome.LoopBroken;
		}
		if (!IsWorldEndingEnding(endingId))
		{
			return EndingLoopOutcome.CycleResolves;
		}
		return EndingLoopOutcome.WorldEnds;
	}

	internal static string EndingHeader(EndingLoopOutcome outcome, string endingTitle)
	{
		return outcome switch
		{
			EndingLoopOutcome.LoopBroken => ConstellationText.Localized("Constellation.EndingHeader", "<color=#5B2A86><b>[FINAL BROADCAST] The Show Ends. The Loop Does Not Turn.</b></color>"), 
			EndingLoopOutcome.WorldEnds => ConstellationText.Localized("Constellation.EndingHeader", "<color=#5B2A86><b>[FINAL BROADCAST] The Recording Stops Here. There Is Nothing After This Frame.</b></color>"), 
			EndingLoopOutcome.ReachFallsShort => ConstellationText.Localized("Constellation.EndingHeader", "<color=#5B2A86><b>[FINAL BROADCAST] The Reach Falls Short. The Anchors Hold.</b></color>"), 
			_ => ConstellationText.LocalizedFormat("Constellation.EndingHeader", "<color=#5B2A86><b>[FINAL BROADCAST] Season Finale: {0}. The Reel Runs Out.</b></color>", endingTitle), 
		};
	}

	private static string Speak(SeatLine line)
	{
		return Speak(SeatTags[line.Seat], line.Text);
	}

	private static string Speak(string tag, string text)
	{
		return ConstellationText.Localized("Constellation.SeatTag", tag) + " " + ConstellationText.Localized("Constellation.Ending", text);
	}

	internal static string ClosingSeat(EndingLoopOutcome outcome, string patronSeat, ICollection<string> offendedSeats, out string closingText)
	{
		closingText = null;
		if (offendedSeats != null && offendedSeats.Count > 0)
		{
			foreach (string key in ConstellationDisfavor.SeatAlignment.Keys)
			{
				if (offendedSeats.Contains(key) && OffendedClosing.TryGetValue(key, out closingText))
				{
					return key;
				}
			}
		}
		IReadOnlyDictionary<string, string> readOnlyDictionary = ((outcome == EndingLoopOutcome.LoopBroken) ? PatronLoopBroken : PatronCycleResolves);
		if (!string.IsNullOrEmpty(patronSeat) && readOnlyDictionary.TryGetValue(patronSeat, out closingText))
		{
			return patronSeat;
		}
		closingText = null;
		return null;
	}

	public static List<string> GetEndingBanter(string endingId, string endingTitle, EndingLoopOutcome outcome, string patronSeat, ICollection<string> offendedSeats)
	{
		List<string> list = new List<string>();
		if (string.IsNullOrEmpty(endingId) || !EndingVerdicts.TryGetValue(endingId, out var value))
		{
			return list;
		}
		if (outcome == EndingLoopOutcome.LoopBroken && !IsekaiTransmigrationQuest.IsLoopBreakingEnding(endingId))
		{
			outcome = ((!IsWorldEndingEnding(endingId)) ? EndingLoopOutcome.CycleResolves : EndingLoopOutcome.WorldEnds);
		}
		list.Add(Separator);
		list.Add(EndingHeader(outcome, string.IsNullOrEmpty(endingTitle) ? endingId : endingTitle));
		SeatLine[] array = value;
		foreach (SeatLine line in array)
		{
			list.Add(Speak(line));
		}
		array = Choruses[outcome];
		foreach (SeatLine line2 in array)
		{
			list.Add(Speak(line2));
		}
		string text = ClosingSeat(outcome, patronSeat, offendedSeats, out var closingText);
		if (text != null)
		{
			list.Add(Speak(SeatTags[text], closingText));
		}
		list.Add(Separator);
		return list;
	}

	public static List<string> GetVanillaEndingFarewell()
	{
		List<string> list = new List<string> { Separator };
		SeatLine[] vanillaFarewell = VanillaFarewell;
		foreach (SeatLine line in vanillaFarewell)
		{
			list.Add(Speak(line));
		}
		list.Add(Separator);
		return list;
	}

	internal static string AlreadyClosedRemark()
	{
		return Speak(AlreadyClosed);
	}
}
