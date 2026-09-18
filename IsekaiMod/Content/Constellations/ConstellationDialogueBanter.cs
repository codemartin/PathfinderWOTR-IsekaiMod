using System;
using System.Collections.Generic;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Constellations
{
	internal static class ConstellationDialogueBanter
	{
		public const string CAYDEN_TAG = "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color>";

		public const string IOMEDAE_TAG = "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color>";

		public const string ASMODEUS_TAG = "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color>";

		public const string DESNA_TAG = "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color>";

		public const string PHARASMA_TAG = "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color>";

		public const string CALISTRIA_TAG = "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color>";

		public const string YOG_TAG = "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color>";

		public const string BUTTERFLY_TAG = "<color=#87CEEB><b>[The Constellation 'The Silence Between the Stars']</b></color>";

		public const string LANTERN_TAG = "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color>";

		public const string NETHYS_TAG = "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color>";

		public const string GORUM_TAG = "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color>";

		public const string BESMARA_TAG = "<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color>";

		public const string CHALDIRA_TAG = "<color=#32CD32><b>[The Constellation 'The Mischievous Friend']</b></color>";

		public const string URGATHOA_TAG = "<color=#8B0000><b>[The Constellation 'The Pallid Princess']</b></color>";

		public const string TORAG_TAG = "<color=#D2691E><b>[The Constellation 'Father of Creation']</b></color>";

		public const string SARENRAE_TAG = "<color=#FFD700><b>[The Constellation 'The Dawnflower']</b></color>";

		public const string ABADAR_TAG = "<color=#DAA520><b>[The Constellation 'Master of the First Vault']</b></color>";

		public const string SHELYN_TAG = "<color=#FF69B4><b>[The Constellation 'The Eternal Rose']</b></color>";

		public const string ERASTIL_TAG = "<color=#8FBC8F><b>[The Constellation 'Old Deadeye']</b></color>";

		public const string IRORI_TAG = "<color=#4682B4><b>[The Constellation 'The Master of Masters']</b></color>";

		public const string GOZREH_TAG = "<color=#2E8B57><b>[The Constellation 'The Wind and the Waves']</b></color>";

		public const string ZONKUTHON_TAG = "<color=#4B0082><b>[The Constellation 'The Midnight Lord']</b></color>";

		public const string PULURA_TAG = "<color=#AFEEEE><b>[The Constellation 'The Shimmering Maiden']</b></color>";

		public const string GODCLAW_TAG = "<color=#708090><b>[The Constellation 'The Fivefold Order']</b></color>";

		public const string NOCTICULA_TAG = "<color=#9400D3><b>[The Constellation 'Our Lady in Shadow']</b></color>";

		public const string ROVAGUG_TAG = "<color=#8B0000><b>[The Constellation 'The Rough Beast']</b></color>";

		private static readonly Random _rng = new Random();

		private static bool _dispatcherInitialized = false;

		private static readonly Dictionary<string, Func<DialogueContext, BanterResult>> _exactAnswerHandlers = new Dictionary<string, Func<DialogueContext, BanterResult>>(StringComparer.OrdinalIgnoreCase);

		private static readonly Dictionary<string, Func<DialogueContext, BanterResult>> _sceneHandlers = new Dictionary<string, Func<DialogueContext, BanterResult>>(StringComparer.OrdinalIgnoreCase);

		private static readonly Dictionary<string, Func<int, string, bool, BanterResult>> _areaHandlers = new Dictionary<string, Func<int, string, bool, BanterResult>>(StringComparer.OrdinalIgnoreCase);

		public static string GetGuestTag(string deityName)
		{
			if (string.IsNullOrEmpty(deityName))
			{
				return "<color=#F5C542><b>[The Constellation 'The Unknown Star']</b></color>";
			}
			string text = deityName.ToLower();
			if (text.Contains("torag"))
			{
				return "<color=#D2691E><b>[The Constellation 'Father of Creation']</b></color>";
			}
			if (text.Contains("sarenrae") || text.Contains("dawnflower"))
			{
				return "<color=#FFD700><b>[The Constellation 'The Dawnflower']</b></color>";
			}
			if (text.Contains("abadar"))
			{
				return "<color=#DAA520><b>[The Constellation 'Master of the First Vault']</b></color>";
			}
			if (text.Contains("shelyn"))
			{
				return "<color=#FF69B4><b>[The Constellation 'The Eternal Rose']</b></color>";
			}
			if (text.Contains("erastil"))
			{
				return "<color=#8FBC8F><b>[The Constellation 'Old Deadeye']</b></color>";
			}
			if (text.Contains("irori"))
			{
				return "<color=#4682B4><b>[The Constellation 'The Master of Masters']</b></color>";
			}
			if (text.Contains("gozreh"))
			{
				return "<color=#2E8B57><b>[The Constellation 'The Wind and the Waves']</b></color>";
			}
			if (text.Contains("zon-kuthon") || text.Contains("zonkuthon"))
			{
				return "<color=#4B0082><b>[The Constellation 'The Midnight Lord']</b></color>";
			}
			if (text.Contains("pulura"))
			{
				return "<color=#AFEEEE><b>[The Constellation 'The Shimmering Maiden']</b></color>";
			}
			if (text.Contains("godclaw"))
			{
				return "<color=#708090><b>[The Constellation 'The Fivefold Order']</b></color>";
			}
			if (text.Contains("nocticula") || text.Contains("lady in shadow") || text.Contains("redeemer queen"))
			{
				return "<color=#9400D3><b>[The Constellation 'Our Lady in Shadow']</b></color>";
			}
			return "<color=#F5C542><b>[The Constellation '" + deityName + "']</b></color>";
		}

		public static List<string> GetAlignmentBanter(AlignmentShiftDirection shift, string answerText, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			coinsAwarded = 0;
			primarySponsor = null;
			if (!string.IsNullOrEmpty(answerText))
			{
				string text = answerText.ToLower();
				if (text.Contains("isekai") || text.Contains("martial god") || text.Contains("overlord") || text.Contains("slime") || text.Contains("devourer") || text.Contains("shadow monarch") || text.Contains("mastermind") || text.Contains("villain") || text.Contains("hero") || text.Contains("god emperor") || text.Contains("radiance") || (text.Contains("lann") && text.Contains("wenduag")) || text.Contains("wardstone") || text.Contains("krebus") || text.Contains("retinue"))
				{
					return GetIsekaiDialogueBanter(answerText, out coinsAwarded, out primarySponsor);
				}
			}
			int num = _rng.Next(3);
			switch (shift)
			{
			case AlignmentShiftDirection.Chaotic:
				coinsAwarded = 100;
				primarySponsor = "The Lucky Drunk";
				switch (num)
				{
				case 0:
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> roars with laughter and slams his tankard: \"Hah! Now that's what I call flair! Rules are just suggestions written by people with no imagination. Drink up!\"");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> rubs her brow with a heavy gauntlet: \"@The Lucky Drunk, show some decorum. The Worldwound is burning and you are treating crusader discipline like a tavern brawl.\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sneers into his goblet: \"Let the clown dance, Inheritor. Reckless audacity invariably rushes headlong into an early grave.\"");
					break;
				case 1:
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> giggles and plays with a silver wasp: \"Oh, that was delightfully impudent! Keep stirring the hornet's nest, darling, it makes the honey that much sweeter.\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> winks: \"Hear that? Even Calistria likes the cut of your jib! Take these coins and buy something that explodes!\"");
					break;
				default:
					list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> hums a breezy tune of starlight: \"A heart unfettered by chains is a beautiful sight. May the winds of freedom carry your bold steps forward!\"");
					list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> rotates across non-Euclidean facets: \"The predictable timeline fractures into 14 novel permutations. Unbound variance detected.\"");
					break;
				}
				break;
			case AlignmentShiftDirection.Good:
				coinsAwarded = 100;
				primarySponsor = "The Inheritor";
				switch (num)
				{
				case 0:
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> nods in solemn approval: \"A heart that chooses mercy and honor in the face of despair is the true armor of a crusader. Stand tall.\"");
					list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> showers starlight upon your shoulders: \"Even in the bleak mud of the Worldwound, compassion blooms like a midnight lily. Shine on, traveler!\"");
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> rolls her eyes: \"Ugh, you do-gooders are giving me a headache. Where is the spite? Where is the betrayal? How terribly wholesome.\"");
					break;
				case 1:
					list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles warmly: \"No darkness is absolute while someone still carries a torch for others. A gift from the stars to light your way.\"");
					primarySponsor = "The Song of the Spheres";
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> scoffs softly: \"Selfless charity... the favorite luxury of fools who expect gratitude from mortals. How quaint.\"");
					break;
				default:
					list.Add("<color=#87CEEB><b>[The Constellation 'The Silence Between the Stars']</b></color> whispers like quiet velvet: \"A gentle silence falls upon suffering souls. In the quiet between stars, your mercy is remembered.\"");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> raises her golden longsword: \"May your blade defend those who cannot defend themselves.\"");
					break;
				}
				break;
			case AlignmentShiftDirection.Evil:
				coinsAwarded = 150;
				primarySponsor = "The Prince of Darkness";
				switch (num)
				{
				case 0:
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smiles with chilling satisfaction: \"Exquisite ruthlessness. Sentimental weakness is the rot of kingdoms. When you strike, leave no ambiguity as to who rules.\"");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with cold anger: \"You tread the precipice of damnation, mortal. Power stripped of righteousness is merely savagery.\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> grumbles, pushing his ale aside: \"Oof. Even my drink tasted sour after that one. You're walking a dark, ugly road, kid.\"");
					break;
				case 1:
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs with venomous delight: \"Now THAT had some sting! Cold, calculated, and delightfully cruel. I do love watching someone drop the pretense of holiness.\"");
					primarySponsor = "The Savored Sting";
					list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> sighs softly: \"A shadow falls over your spirit... do not let the abyss swallow the light you brought from your world.\"");
					break;
				default:
					list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with absolute frost: \"Another stone cast into the balance. Every soul reaps what it sows in the Boneyard. Remember this.\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> laughs quietly: \"Keep playing the board with such clarity, incarnation. Power belongs to those bold enough to claim it.\"");
					break;
				}
				break;
			case AlignmentShiftDirection.Lawful:
				coinsAwarded = 100;
				primarySponsor = "The Inheritor";
				if (num == 0)
				{
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> salutes with her blade: \"Order, duty, and oaths unbroken. Through discipline, the Crusade endures where passion crumbles.\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods approvingly: \"Indeed. Contracts and structure govern reality itself. Even the Inheritor understands the virtue of obedience.\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> groans dramatically: \"Great, now the contract lawyers and church elders are high-fiving. Somebody pass me a stronger keg.\"");
				}
				else
				{
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> writes a glyph in crimson ink: \"A disciplined mind is a magnificent instrument. Never allow chaotic rabble to dictate your terms.\"");
					primarySponsor = "The Prince of Darkness";
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> yawns behind her sleeve: \"So rigid. Where is the fun if you follow every little rule? Break something, just once!\"");
				}
				break;
			default:
				if (answerText != null && (answerText.ToLower().Contains("truck") || answerText.ToLower().Contains("reincarnat") || answerText.ToLower().Contains("other world")))
				{
					coinsAwarded = 200;
					primarySponsor = "The Key and the Gate";
					list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> unfolds cosmic spheres of iridescent light: \"The memory of the origin world surfaces like an oil slick upon the dimensional tide. We observe your transit with keen calculation.\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> blinks: \"Wait, a mechanical carriage did what now?! Man, whatever god invented 'Trucks' clearly knows how to start a party with a bang!\"");
					list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> gazes down impassively: \"A soul ripped across realities without passing through the River. A strange thread tied into Golarion's knot.\"");
				}
				break;
			}
			return list;
		}

		public static List<string> GetIsekaiDialogueBanter(string answerText, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			string text = answerText.ToLower();
			if (text.Contains("lann") && text.Contains("wenduag"))
			{
				coinsAwarded = 500;
				primarySponsor = "The Laughing King";
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> leaps into the air and cackles: \"WAIT! YOU TOOK BOTH OF THEM?! That was NOT an option on the multiple-choice test! Look at their confused faces! Absolute comedy gold! PAY UP, ASMODEUS!\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> roars with laughter: \"Bwahaha! Why pick one when you can build a harem of archers and spider-cats?! That's my kind of commander! Barkeep, drinks for the whole gallery!\"");
				list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles warmly: \"Two fractured souls guided away from bitter tragedy. In the starlight, their shared redemption begins.\"");
			}
			else if (text.Contains("wardstone"))
			{
				coinsAwarded = 500;
				primarySponsor = "The Key and the Gate";
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> unfolds radiant multi-dimensional geometries: \"The causal anomaly absorbs the celestial matrix into its own boundless reservoir. 4,096 timelines rewritten.\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> falls over laughing: \"Areelu looks like she swallowed a lemon! Out of four hundred crusades, no one has ever done THAT! Areelu is completely dumbfounded!\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks in quiet awe: \"Such sovereign resolve... neither angel nor demon, yet holding back the abyssal tide with an otherworldly soul.\"");
			}
			else if (text.Contains("martial god"))
			{
				coinsAwarded = 350;
				primarySponsor = "Our Lord in Iron";
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars in battle-frenzy: \"GLORIOUS! CRUSH THEM UNDERFOOT! THAT IS HOW A MARTIAL SOVEREIGN CUTS DESTINY APART!\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles from his perch: \"Look at the Ki aura! Charging up for five episodes! Don't blink, folks! Bwahaha!\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises a tankard: \"Man, I don't know what a 'dantian' is, but it hits harder than dwarven moonshine! Keep swinging!\"");
			}
			else if (text.Contains("overlord"))
			{
				coinsAwarded = 400;
				primarySponsor = "The Pallid Princess";
				list.Add("<color=#8B0000><b>[The Constellation 'The Pallid Princess']</b></color> purrs with dark rapture: \"Oh, magnificent! Look at that exquisite sovereign of death! An absolute Overlord crushing pathetic mortals under ivory heels!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sips wine with cold respect: \"Supreme discipline. True power requires neither hesitation nor remorse. The Great Tomb commands absolute order.\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> mimics a dramatic pose: \"Aura of Despair activated! Look at the boss shaking in their boots! 10 out of 10 for theatrical villainy!\"");
			}
			else if (text.Contains("slime") || text.Contains("devourer"))
			{
				coinsAwarded = 350;
				primarySponsor = "The Laughing King";
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> laughs hysterically: \"PWAHAHAHA! The blue blob strikes again! Look at the demon trying to figure out where its weapons went! Absorbed and digested!\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles: \"Wait, did it just eat the whole trap?! That's one way to disarm a dungeon! Barkeep, a round for the slime!\"");
				list.Add("<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> eyes flashing with arcane light: \"An amorphous cellular matrix capable of universal enzymatic conversion. Truly sublime biological transmutation!\"");
			}
			else if (text.Contains("shadow monarch"))
			{
				coinsAwarded = 400;
				primarySponsor = "The Pallid Princess";
				list.Add("<color=#8B0000><b>[The Constellation 'The Pallid Princess']</b></color> laughs with dark delight: \"The shadows rise! Not as rotting shamblers, but as an eternal legion of exquisite devotion! Arise, dark army!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> writes a glyph of approval: \"An incorruptible legion commanded by absolute hierarchy. Exquisite command structure.\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> snickers: \"'Arise', they say! PWAHAHA! The Grim Reaper is taking notes right now!\"");
			}
			else if (text.Contains("mastermind"))
			{
				coinsAwarded = 400;
				primarySponsor = "The Key and the Gate";
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> hums through spatial folds: \"Probabilistic convergence computed in 4 clock cycles. The closed causal loop yields to supreme logic.\"");
				list.Add("<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> roars in arcane ecstasy: \"Calculations, binary encryption, and cold deduction! The sheer elegance of absolute intellect!\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> blinks: \"Whoa, slow down with the math, professor! Just tell me if we won so I can pop the cork!\"");
			}
			else if (text.Contains("villain"))
			{
				coinsAwarded = 350;
				primarySponsor = "The Savored Sting";
				list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> giggles behind her fan: \"Oh, delightfully ruthless! Blackmail, psychological torment, and cold ambition... truly a master of the sting!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods approvingly: \"Power is not given, mortal; it is seized by those with the spine to engineer the board.\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with a sigh: \"A troubling disregard for chivalry... yet undeniably effective against our demonic adversaries.\"");
			}
			else if (text.Contains("hero"))
			{
				coinsAwarded = 350;
				primarySponsor = "The Inheritor";
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> salutes with golden radiance: \"A shining beacon of justice and hope! When darkness presses on all sides, a true hero stands firm!\"");
				list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> showers starlight: \"Compassion burns brighter than all the abyssal fires! Shine on, brave traveler, the stars guide your path!\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> wipes a fake tear: \"Aww, look at the classic hero speech! It's so cliché it's actually circling back to being peak cinema!\"");
			}
			else if (text.Contains("god emperor"))
			{
				coinsAwarded = 450;
				primarySponsor = "The Prince of Darkness";
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> inclines his head: \"An emperor's decree brooks no insolence. Imperial authority brings immaculate order to chaotic wastelands.\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with quiet warmth: \"May the golden mandate of your empire bring righteous peace to a battered world.\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> winks: \"Behold the God-Emperor! Gold trim, dramatic declarations, and overwhelming charisma! All hail!\"");
			}
			else if (text.Contains("retinue"))
			{
				coinsAwarded = 350;
				primarySponsor = "The Song of the Spheres";
				list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles warmly: \"The threads of fate weave your souls together across reality! What a wondrous retinue walks beside you!\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises a brimming flagon: \"Now that is a traveling party! Friends who share each other's luck and drink each other's ale!\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with solemn respect: \"Companions bound by extraordinary duty, shielding one another when the world trembles.\"");
			}
			else
			{
				coinsAwarded = 300;
				primarySponsor = "The Lucky Drunk";
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> laughs with hearty cheer: \"Now that is what I call taking charge of the conversation! Never let anyone tell you how to play your own hand!\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles warmly: \"A delightfully unexpected turn of phrase! Keep keeping everyone on their toes!\"");
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> rotates across spatial facets: \"The causal flow of discourse bends around the anomaly. The thread of fate remains unbroken.\"");
			}
			return list;
		}

		public static List<string> GetDeathReloadBanter(int deathCount, string lastArea)
		{
			List<string> list = new List<string>();
			switch (_rng.Next(6))
			{
			case 0:
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> winces and rubs his temples: \"Ouch... I felt that hit all the way from Elysium! Let's see if the little 'hero' actually makes it this time! All that power you have and still you fail! Don't let me down, I put fifty coins on you!\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks firmly: \"Defeat is merely a forge for the soul. Stand up, dust off your armor, and face your enemy with discipline. No more reckless blunders.\"");
				break;
			case 1:
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sips wine with cold, theatrical amusement: \"Back already? How delightful. Rewinding the timeline does not cure a fundamental deficiency in intellect, mortal. Perhaps try using your brain before your blade.\"");
				list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> snickers: \"Oh, that was embarrassing! Sliced into ribbons in under six seconds. Please tell me you have an actual plan this time, or should I order more snacks?\"");
				break;
			case 2:
				list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> turns an ancient page in her ledger: \"Your soul approached the gates of my court, yet an anomaly drags you back into the mortal coil once more. Do not test the patience of the River, traveler.\"");
				list.Add(string.Format("{0} hums through spatial folds: \"Iteration #{1} terminated at {2}. Reality re-anchored. Present a superior causal outcome.\"", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color>", deathCount, lastArea));
				break;
			case 3:
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> cackles hysterically: \"PWAHAHAHA! Oh, that was tragic! Did you see the look on your face right before the axe landed?! Rewind the tape, gods, I want to watch that again!\"");
				list.Add("<color=#32CD32><b>[The Constellation 'The Mischievous Friend']</b></color> wincing behind her hands: \"Ooooh, bad roll! Don't listen to the Laughing King, hero! My lucky clover's with you this time. Charge right back in!\"");
				break;
			case 4:
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars with thunderous fury: \"PATHETIC! A TRUE WARRIOR DIES ONCE, BUT A LOOPING COWARD DIES A THOUSAND TIMES! STAND UP, GRIP YOUR STEEL, AND CARVE THEIR BONES TO DUST!\"");
				list.Add("<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> laughs brazenly: \"Walked the plank so soon? Even a scuttled ship can be raised for another raid. Scramble up the rigging and fire a broadside!\"");
				break;
			default:
				list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> giggles with sharp glee: \"Round two! Or is it round three? I've lost count of how many times you've kissed the dirt. Show them some teeth this time!\"");
				list.Add("<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> eyes shifting in dual madness: \"The threads of your demise collapse into the harmonic frequencies of the loop. Fascinating. Attempt the convergence again.\"");
				break;
			}
			return list;
		}

		public static List<string> GetCritBanter(string attacker, string target)
		{
			List<string> list = new List<string>();
			switch (_rng.Next(4))
			{
			case 0:
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars in bloodlust: \"GLORIOUS! CRUSH THE SKELETON! SPLIT THE SKULL! THAT IS HOW WAR IS WAGED!\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> cheers: \"NOW THAT'S WHAT I CALL A HAYMAKER! Barkeep, another round for the front row!\"");
				break;
			case 1:
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles with gleaming eyes: \"Oof! Sent them straight into the next dimension! 10 out of 10 for dramatic execution!\"");
				list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs softly: \"Mmm, vicious. Right through the vital points. Exquisitely brutal.\"");
				break;
			case 2:
				list.Add("<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> grins wildly: \"A clean decapitation! Plunder the corpse before the blood even dries, captain!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods once: \"Decisive lethality. Efficiency is always appreciated.\"");
				break;
			default:
				list.Add("<color=#32CD32><b>[The Constellation 'The Mischievous Friend']</b></color> jumps in excitement: \"A critical strike! I told you luck was on our side! Give 'em another!\"");
				list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> glimmers with starry warmth: \"A brilliant burst of starfire cutting through the darkness!\"");
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
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises a glass: \"Look at our new star stepping into Kenabres! We've seen this siege fail fifty times, but this kid doesn't even know the script! I've got high hopes!\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> observes solemnly: \"The city has fallen, but valor may yet prevail. Show these crusaders the fire of your convictions.\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> whispers playfully: \"Keep smiling, hero. Out of all the iterations we've watched, nobody ever makes choices as weird as yours! Let the unscripted drama unfold!\"");
				break;
			case 2:
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> writes in his ledger: \"Advancing into the march on Drezen. I wager 500 soul-shards they succumb to impatience before the citadel falls.\"");
				list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles: \"I'll take that bet, Prince of Darkness! The Knight-Commander's heart burns brighter than your cynicism.\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> laughs: \"Wait, is this the Angel route or the Trickster route? Don't spoil it, let me guess!\"");
				break;
			case 3:
				list.Add("<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> eyes flashing with arcane storm: \"The causal fabric of Drezen is wearing thin. The mortal begins to sense the unnatural echoes of the loop.\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles: \"Uh oh... someone left the script on the table! Don't read ahead, kid, you'll spoil the punchline!\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> warns: \"Knight-Commander, do not let planar anomalies distract you from your holy crusade.\"");
				break;
			case 4:
				list.Add("<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> looks around Alushinyrra: \"Welcome to the belly of the beast! Shadows, cutthroats, and demonic intrigue... now THIS is my kind of port!\"");
				list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> smiles seductively: \"The mortal holds the Key of Causal Rupture. They know now. The audience is holding its breath.\"");
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> echoes from the outer void: \"The closed ring approaches its inflection point. The traveler gazes outward toward the threshold.\"");
				break;
			case 5:
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with quiet awe: \"The Threshold is reached. But this time... the Knight-Commander looks beyond the Worldwound itself.\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> leans forward eagerly: \"Are they really going to do it?! Are they going to challenge the Grand Arbiter and smash the stage?!\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> slams his fists on the celestial rail: \"DO IT! SMASH THE WHOLE DAMN APPARATUS! SHOW 'EM WHAT FREE WILL TASTES LIKE!\"");
				break;
			case 6:
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> draws her golden blade, standing at the precipice: \"The Threshold of the Worldwound. The final sanctum where Areelu Vorlesh tore open the fabric of Golarion. Stand firm, Knight-Commander; all Heaven watches your final step.\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> closes his ledger with a heavy thud: \"The decisive hour. The demonic breach either collapses into planar order, or consumes the realm entirely. Execute your strategy without pity.\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> drains his tankard and smashes it: \"Down to the last bottle! Pour everything you've got into this charge! Show the demons what happens when mortals refuse to lay down and die!\"");
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> revolves in multidimensional resonance: \"The singularity approaches. The closed causal loop vibrates upon the razor edge of dissolution. Will the cycle repeat... or shatter into the unwritten dawn?\"");
				break;
			}
			return list;
		}

		public static List<string> GetMetaLoopBanter(int runCount)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			if (runCount > 1)
			{
				list.Add($"<color=#F5C542><b>[TEMPORAL CYCLE DETECTED] This Soul's Loop #{runCount} begins!</b></color>");
				if (TimelineManager.Data.LoopShattered)
				{
					list.Add(string.Format("{0} opens an infinity of non-Euclidean eyes upon you: \"Soul Iteration #{1}. The Sovereign returns to the fractured nexus. An unscripted paradox walking among mortals.\"", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color>", runCount));
					list.Add(string.Format("{0} roars with delight: \"LOOK WHO'S BACK! The legend who actually shattered the loop! Barkeep, pour #{1} rounds for the champion!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color>", runCount));
					list.Add(string.Format("{0} chuckles warmly: \"Back for an encore in Loop #{1}? Let's see what chaotic trouble you can cook up this time!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color>", runCount));
				}
				else
				{
					list.Add(string.Format("{0} opens an infinity of non-Euclidean eyes upon you: \"Soul Iteration #{1} begins. The cosmic spindle rewinds. The soul's thread reweaves into the tapestry once more. Let us observe if this iteration resolves the paradox... or repeats the choreography.\"", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color>", runCount));
					list.Add(string.Format("{0} squints through his foaming tankard: \"Hold on... haven't I cheered for you before? Loop #{1}?! Either I drank way too much Starstone brew, or we're running in circles!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color>", runCount));
					list.Add(string.Format("{0} laughs from his golden perch: \"Welcome back to Loop #{1}, folks! Grab your popcorn, the hero thinks this is their first time! Let's see if they remember to complete ALL side quests before challenging the Arbiter! Bwahaha!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color>", runCount));
					list.Add(string.Format("{0} records in her solemn ledger: \"Entry recorded: Soul Iteration #{1}. An unnatural circular record outside the River of Souls... yet tolerated by the Outer Court.\"", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color>", runCount));
					list.Add(string.Format("{0} smiles with dangerous calculation: \"Loop #{1}. How many cycles must transpire before this soul learns from past blunders? Incomplete vectors will simply collapse into another restart.\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color>", runCount));
				}
			}
			else
			{
				list.Add("<color=#F5C542><b>[TEMPORAL CYCLE INITIATED] This Soul's Loop #1 begins!</b></color>");
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> opens an infinity of non-Euclidean eyes upon you: \"Soul Iteration #1. Plucked across the cosmic threshold into the closed circle. The play begins.\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> whispers playfully: \"A fresh actor on the grand stage! Let's see how long before they realize the play has already been written!\"");
			}
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static double RollMultiplier(string primarySponsor, out string shoutoutMsg)
		{
			shoutoutMsg = null;
			int num = _rng.Next(100);
			if (num < 2)
			{
				shoutoutMsg = "<color=#F5C542><b>[Constellation JACKPOT!]</b></color> <b>" + primarySponsor + "</b> dropped a <b>3.0x Jackpot Multiplier</b> on your broadcast! The entire celestial sphere is showering coins!";
				return 3.0;
			}
			if (num < 10)
			{
				shoutoutMsg = "<color=#F5C542><b>[Constellation Mega Donation!]</b></color> <b>" + primarySponsor + "</b> boosted their sponsorship with a <b>2.0x Multiplier</b>!";
				return 2.0;
			}
			if (num < 30)
			{
				shoutoutMsg = "<color=#F5C542><b>[Constellation Super Chat!]</b></color> <b>" + primarySponsor + "</b> added a <b>1.5x Multiplier</b>!";
				return 1.5;
			}
			return 1.0;
		}

		public static List<string> GetFestivalCrossbowBanter(bool hit)
		{
			List<string> list = new List<string>();
			if (hit)
			{
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> cheers loudly: \"BULLSEYE! That's my recruit! Barkeep, drinks for the whole front row!\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> blinks in surprise: \"Wait, they actually hit it?! Somebody check the wind vanes!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods once: \"Adequate precision. Continue demonstrating focus.\"");
			}
			else
			{
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> falls over laughing: \"PWAHAHA! Did you see where that bolt landed?! Pay up, Asmodeus!\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> winces: \"Oof... maybe have a sip of water instead of ale before the next shot, kid.\"");
			}
			return list;
		}

		public static List<string> GetFestivalAleBanter(bool chugged)
		{
			List<string> list = new List<string>();
			if (chugged)
			{
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> slams his table: \"NOW THAT'S WHAT I CALL DIVINE INTERVENTION! DOWN IN ONE! An honorary brewmaster in the making!\"");
				list.Add("<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> laughs brazenly: \"Aye, swig it down like seawater on a storm deck! Good form, landlubber!\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> sighs: \"Must you encourage such reckless overindulgence during a solemn holy festival?\"");
			}
			else
			{
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> groans in agony: \"Spilled half the mug! A tragic waste of fine fermented hops! Try again!\"");
				list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> giggles: \"Messy, darling. But delightfully uncoordinated.\"");
			}
			return list;
		}

		public static List<string> GetFestivalStrengthBanter(bool rangBell)
		{
			List<string> list = new List<string>();
			if (rangBell)
			{
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars in triumph: \"A THUNDEROUS STRIKE! THE IRON SINGS! THAT IS HOW WARRIORS CRACK SKULLS!\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> whistles: \"Ding-dong! Ring it again, champion!\"");
			}
			else
			{
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> bellows in disgust: \"PATHETIC TAP! PUT YOUR SHOULDERS AND HIPS INTO IT, WEAKLING!\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> snickers: \"Clink! That wouldn't even dent a First World toadstool!\"");
			}
			return list;
		}

		public static List<string> GetWardstoneMythicAwakeningBanter(string primarySponsor)
		{
			return new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[UNSCRIPTED CAUSAL ANOMALY DETECTED] The Wardstone Climax Shatters the Standard Loop!</b></color>", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> leaps onto the celestial table: \"HOLD THE PHONE! Did you see that?! That was NOT in the director's cut! Out of four hundred crusades, no one has ever done THAT! Areelu looks like she swallowed a lemon! PAY UP, ASMODEUS!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> slams his fists in ecstasy: \"I KNEW IT! Putting coins on the wildcard always pays out! Look at that spark take! Barkeep, drinks for the entire gallery!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> writes a ledger note with chilling intrigue: \"An unprecedented vector deviation from the standard loop. Highly irregular... and exceptionally profitable. I accept the wager, jester.\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with quiet, stunned reverence: \"In all the recorded iterations of Kenabres, I have never seen a mortal seize the celestial flame with such unpredictable resolve.\"", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> unfolds radiant multi-dimensional geometries: \"The causal lattice fractures into 4,096 unscripted branches. The recorded script is nullified. An authentic anomaly walks Golarion.\"", "<color=#F5C542><b>=======================================================</b></color>" };
		}

		public static List<string> GetWardstoneMythicAwakeningBanter(int cycle, out int coinsAwarded, out string primarySponsor)
		{
			if (cycle <= 1)
			{
				coinsAwarded = 500;
				primarySponsor = "The Laughing King";
				return GetWardstoneMythicAwakeningBanter(primarySponsor);
			}
			coinsAwarded = 600;
			primarySponsor = "The Key and the Gate";
			return new List<string>
			{
				"<color=#F5C542><b>=======================================================</b></color>",
				$"<color=#F5C542><b>[RECURSIVE ANOMALY CONFIRMED] Cycle {cycle}: The Wardstone Ignites Once More!</b></color>",
				string.Format("{0} cackles from the rafter: \"Round {1} and Minagho STILL falls for the exact same routine! Every single loop she thinks she's corrupting the stone, and every single loop our looper turns it into a private firework show! Classic!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color>", cycle),
				string.Format("{0} pulsates in geometric resonance: \"Iterative mythic awakening #{1}. The celestial resonance stabilizes across parallel timelines. The causal anomaly deepens.\"", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color>", cycle),
				"<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with solemn warmth: \"Even across countless cycles, the flame of mortal courage burns undimmed against the demonic host.\"",
				"<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> marks his ledger: \"The odds were settled long ago. Minagho was a footnote then, and remains a footnote now. Continue the advance.\"",
				"<color=#F5C542><b>=======================================================</b></color>"
			};
		}

		public static List<string> GetSlimeRevealBanter()
		{
			return new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CONSTELLATION DISCOVERY] The True Nature of the Slime is Witnessed!</b></color>", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> laughs hysterically: \"WAIT! DID YOU SEE THAT?! The mortal is a giant sentient jelly! Look at the paladin's face! She thinks her holy crusade is being led by emerald gelatin! PWAHAHAHA!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles: \"Hey, if it can hold a flagon of ale and dissolve a glabrezu, it's a drinking buddy in my book! Just... don't melt my tankard, kid!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smirks coldly: \"An amorphous heteromorphic predator concealing itself as a mortal crusader. Deceptive, highly resilient, and distinctly entertaining to observe.\"", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> whispers in dual voices: \"A fluid cellular matrix immune to physical cleavage. Truly fascinating biological transmutation.\"", "<color=#F5C542><b>=======================================================</b></color>" };
		}

		public static List<string> GetOverlordSkeletonRevealBanter()
		{
			return new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CONSTELLATION DISCOVERY] The True Nature of the Skeletal Overlord is Witnessed!</b></color>", "<color=#8B0000><b>[The Constellation 'The Pallid Princess']</b></color> purrs with dark delight: \"Oh, magnificent! Look at that exquisite ivory skull! An absolute sovereign of death masquerading among mortal crusaders! Death always claims the final victory!\"", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks in grave judgment: \"A heteromorphic soul of undeath walking under the guise of living flesh... The cosmic scales tilt erratically. See to it that you destroy the demonic rift before you test my patience, Overlord.\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> scratches his beard: \"Wait, our hero is literally a skeleton wearing an illusion cloak?! How does he even drink ale without it splashing all over his ribs?! Put fifty coins on the bone lord!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> cackles: \"An undead Overlord in Kenabres! Look at Seelah having an existential panic attack! 'Is he a demon? Is he a skeleton? Why is he saving my life?!' Best episode ever!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods in deep appreciation: \"Supreme Overlord of the Great Tomb. Unyielding order, discipline, and ruthless hierarchy. Now that is a commander worthy of a real contract.\"", "<color=#F5C542><b>=======================================================</b></color>" };
		}

		public static List<string> GetPastCycleRecognitionBanter()
		{
			List<string> list = new List<string>();
			PastCycleRecord lastCycle = TimelineManager.GetLastCycle();
			string text = lastCycle?.Archetype ?? "Unknown";
			string text2 = lastCycle?.EndingAchieved ?? "Unrecorded";
			int totalRuns = TimelineManager.TotalRuns;
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			list.Add("<color=#F5C542><b>[CONSTELLATION RECOGNITION] Echoes of Past Lifetimes Detected!</b></color>");
			list.Add(string.Format("{0} manifests as interlocking tesseracts: \"Iteration #{1} initiated. Subject retains memory fragments of prior vessel [{2}] and terminus [{3}]. The causality loop vibrates with resonance.\"", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color>", totalRuns + 1, text, text2));
			list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles knowingly: \"Look at them! Back again for another round in the sandbox! Did you miss the mud of Kenabres that much, or did you just want to punch Minagho again?\"");
			list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sips leisurely from his chalice: \"The memory of prior lifetimes is the greatest ledger of all. Let us see if this iteration yields greater discipline or greater folly.\"");
			list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with quiet wonder: \"To shoulder the weight of an entire crusade once is heroic. To step once more into the crucible... that is the mark of an indomitable soul.\"");
			if (TimelineManager.LoopBreakerAchieved)
			{
				list.Add("<color=#87CEEB><b>[The Constellation 'The Silence Between the Stars']</b></color> murmurs with deep celestial grace: \"The loop was shattered, yet you chose to return to our stars. Welcome back, sovereign wanderer.\"");
			}
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetLoopBreakerSovereignReturnBanter()
		{
			return new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CONSTELLATION SOVEREIGN RETURN] The Shattered Loop Acknowledged!</b></color>", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> revolves in crystalline harmony: \"The causal barrier remains broken. Subject crossed boundaries by voluntary will rather than recursive compulsion. Shard of the Shattered Loop manifested.\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> laughs with unrestrained glee: \"You actually came back! You broke out of the cosmic fishbowl and then jumped right back into the water! Absolute legend! Take these coins, you earned them!\"", "<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> tips her tricorn: \"A captain who sails the outer dark and chooses to visit our waters once more? Now that is the kind of reckless sovereign I respect!\"", "<color=#F5C542><b>=======================================================</b></color>" };
		}

		public static List<string> GetDlc1AxisParadoxBanter()
		{
			return new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CONSTELLATION BROADCAST] Planar Incursion into Axis & The Inevitable Excess!</b></color>", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> rotates across multi-dimensional planes: \"Valmallos constructs an artificial clockwork causality bubble in Axis. A mechanical imitation of true multidimensional space.\"", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> eyes flashing with arcane storm: \"A machine god trying to calculate infinity with gears and axioms! Deliciously futile and magnificent to observe!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> laughs into his tankard: \"Even the clockwork lawyers in Axis can't pin down our reincarnated wildcard! Drink every time a gear jams!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> writes a glyph in crimson ink: \"Let the Inevitables test their cold equations. True order is forged through sovereign will, not mindless clockwork.\"", "<color=#F5C542><b>=======================================================</b></color>" };
		}

		public static List<string> GetDlc3MidnightIslesVoyageBanter()
		{
			return new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CONSTELLATION BROADCAST] Voyage into the Midnight Isles Archipelago!</b></color>", "<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> laughs brazenly, drawing her cutlass: \"Now THIS is proper piracy! Charting cursed islands on an abyssal ship with a silent helmsman! Plunder every isle to the bedrock, captain!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> cackles from his perch: \"An archipelago made from Nahyndri's corpse! Who said the Abyss doesn't have beachfront property?! Keep the rum flowing!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises a mug in salute: \"Sailing the dark waters of the Outer Rifts with a crew of cutthroats! If that doesn't earn you a round on me, nothing will!\"", "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> glimmers with gentle starlight: \"Even over cursed seas, may the stars above guide your voyage safely through the abyssal fog.\"", "<color=#F5C542><b>=======================================================</b></color>" };
		}

		public static List<string> GetDlc5ShadowPlaneBanter()
		{
			return new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CONSTELLATION BROADCAST] Descent into the Shadow Plane & Lord of Nothing!</b></color>", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> unfolds dark crystalline geometries: \"The shadow realm of Sithhud unfolds along the cold perimeter of reality. Shadow and void intertwine in chilling convergence.\"", "<color=#8B0000><b>[The Constellation 'The Pallid Princess']</b></color> purrs with dark delight: \"The chill of unliving frost! Feast upon the remnants of the fallen titan and let the cold hunger consume your foes!\"", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with solemn judgment: \"Sithhud's frozen shards echo with ancient hubris. Break his remnants and restore the balance of the Boneyard.\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> shivers theatrically: \"Brrr! It's colder than a winter sprite's kiss down there! Put on a coat, mortal, or your fingers will snap right off!\"", "<color=#F5C542><b>=======================================================</b></color>" };
		}

		public static List<string> GetDlc6FestivalTournamentBanter()
		{
			return new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CONSTELLATION BROADCAST] Kenabres Festival Games & Gladiator Arena Tournament!</b></color>", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> cheers at the top of his lungs: \"A festival date, midway games, and an arena death-match all in one afternoon?! Best festival in the history of Mendev!\"", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars in battle-frenzy: \"THE ARENA SINGS! CRUSH THE IMPOSTORS! PROVE WHO REIGNS SUPREME UNDER THE FESTIVAL LIGHTS!\"", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> giggles behind her fan: \"Costumes, deception, secret dates, and blood on the sand... truly, this festival has everything a girl could ask for!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> falls over laughing: \"Razmiran clones pretending to be the Knight-Commander! PWAHAHA! Imitation really is the sincerest form of getting flattened!\"", "<color=#F5C542><b>=======================================================</b></color>" };
		}

		public static List<string> GetEpicPrestigeBanter(string prestigeClassName)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Epic Prestige Ascension: " + prestigeClassName + "!</b></color>");
			switch (prestigeClassName)
			{
			case "The Demigod":
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> laughs into his tankard: \"Ascending to godhood through mortal deeds?! Welcome to the club, buddy! First round of nectar is on you!\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with solemn warmth: \"A mortal bearing the mantle of demigodhood... let your divine spark be a beacon of righteousness and courage for all Golarion.\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> strokes his goatee: \"Another mortal ascends toward the pantheon. Let us see if your fledgling divinity understands the solemnity of planar law.\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> cackles: \"A new god in town?! Quick, someone get the popcorn! The celestial pantheon just got a whole lot more entertaining!\"");
				break;
			case "The Godhunter":
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars in battle-frenzy: \"HA! A GODHUNTER! A MORTAL WHO DARES TO HUNT THE DIVINE! SHOW ME HOW YOU CRUSH DEITIES UPON THE ANVIL OF WAR!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> whispers calculatingly: \"Fascinating. A mortal weapon that severs divine authority... ensure you point that deicidal edge at our mutual adversaries.\"");
				list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with quiet gravity: \"Even gods are not beyond the spiral of fate. Tread carefully, hunter, for every divine thread severed alters the tapestry of reality.\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> ducks behind a cloud: \"Hunting gods?! Ooh, you're playing with fire now! Don't look at me, I'm just a harmless wandering observer!\"");
				break;
			case "The Warmaster":
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> slams his greatsword: \"THE WARMASTER! THE GOD OF WAR ACKNOWLEDGES YOUR SUPREME TACTICAL DOMINANCE! LEAD THE CRUSADE TO TOTAL ANNIHILATION!\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> salutes with golden radiance: \"A commander of boundless tactical acumen... lead the armies of Mendev to righteous victory over the Worldwound.\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods with quiet respect: \"Precision, discipline, and uncompromising battlefield command. That is how empires are forged and maintained.\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises a flagon: \"That's some serious generalship! After the battle is won, the entire crusade is drinking till dawn!\"");
				break;
			case "The Eternal Seeker":
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> pulses in kaleidoscopic cosmic geometries: \"The gates of infinite realities part before your seeking eye. You perceive the echoes of all possible timelines converged into one.\"");
				list.Add("<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> crackles with dual-aspected energy: \"A seeker of infinite cosmic truths! Knowledge unfettered by mortal limitations! Absorb every secret of the multiverse!\"");
				list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> glimmers with celestial starlight: \"May your journeys across the endless stars and forgotten timelines guide you toward beauty, wonder, and freedom.\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> winks playfully: \"Seeking eternal knowledge across dimensions? Don't peek too far behind the curtain, you might spoil the ending!\"");
				break;
			case "The Archmage":
				list.Add("<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> roars in arcane ecstasy: \"THE PINNACLE OF ARCANE SUPREMACY! NINTH-CIRCLE POWER BENDING TO YOUR WILL! TEAR DOWN THE SKY WITH PURE SPELLFIRE!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> inclines his head: \"Mastery over the fundamental grammar of reality. An archmage of your caliber commands absolute respect across the Outer Spheres.\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> manifests holy light: \"Channel your sublime sorcery in defense of the innocent, and banish the demonic hordes back to the abyssal rift.\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> whistles in awe: \"That's a lot of firepower! Just make sure not to blow up the tavern when you're flexing those high-circle spells!\"");
				break;
			case "The Shadow Sovereign":
				list.Add("<color=#8B0000><b>[The Constellation 'The Pallid Princess']</b></color> laughs with dark rapture: \"The dead rise not as rotted corpses, but as exquisite shadows of eternal devotion! How delicious! How sovereign!\"");
				list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> observes with ancient stillness: \"Shadows bound to a mortal will... walk the line between sovereignty and balance with supreme vigilance.\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> writes a seal of approval: \"A sovereign commanding an incorruptible legion of shadows. Absolute hierarchy. Absolute discipline. I approve.\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> mimics a dramatic pose: \"'Arise', they say! PWAHAHA! Even the Grim Reaper would be jealous of your personal shadow retinue!\"");
				break;
			default:
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises a drink: \"To the greatest legend Golarion has ever witnessed! Keep raising hell across the planes!\"");
				break;
			}
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetPrologueAwakeningBanter(out int coinsAwarded, out string primarySponsor)
		{
			coinsAwarded = 150;
			primarySponsor = "The Laughing King";
			return new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL BROADCAST LIVE] Chapter 0: The Otherworlder Awakens!</b></color>", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> taps the cosmic viewing lens: \"The celestial lens focuses upon Kenabres! Look at that, observers! Our brand-new reincarnator finally opened their eyes on a festival stretcher! No cheat status screen, no explanation manual, and a gaping hole in their chest. Classic zero-star summoning!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> laughs into his tankard: \"Hey, give the kid credit for breathing! Ten gold coins says they try to shout 'Status Window Open!' before realizing this world does not have floating blue interface menus.\"", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with icy detachment: \"An unregistered soul crosses the threshold of mortality. Fate's ledger shivers with anomaly. Let us see if this foreign spark endures or returns swiftly to the Boneyard.\"", "<color=#87CEEB><b>[The Constellation 'The Silence Between the Stars']</b></color> whispers softly: \"The morning sky above Kenabres smiles with festive silk... yet deep beneath the stones, a colossal scythe is already poised to strike. Cherish the sunlight, traveler, for the locust draws near.\"", "<color=#F5C542><b>=======================================================</b></color>" };
		}

		public static List<string> GetPrologueStatusWindowBanter(int cycle, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			if (cycle <= 1)
			{
				coinsAwarded = 200;
				primarySponsor = "The Lucky Drunk";
				list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] System Error: No Floating Status Window Found!</b></color>");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> sputters beer through his nose: \"Bwahaha! Did they just try to swipe down on empty air looking for an inventory screen?! Welcome to Golarion, rookie! No pop-up menus, no cheat prompts--just good old-fashioned steel, grit, and tavern brawls!\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> falls over wheezing: \"'Status window, open!' PWAHAHA! Look at the poor priestess thinking he's completely delirious from brain fever! The audience in the upper balcony is rolling on the floor! 10 out of 10 for comedy!\"");
				list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with cold gravity: \"A foreigner seeking the artificial game systems of a distant digital world. Here, your ledger is written in mortal blood and irreversible deeds. Mind your balance.\"");
				list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles gently: \"Do not be dismayed, traveler. You do not need glowing letters floating in your eyes to find wonder and strength beneath the stars.\"");
			}
			else
			{
				coinsAwarded = 350;
				primarySponsor = "The Key and the Gate";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: The Veteran Looper Awakes!</b></color>");
				list.Add(string.Format("{0} pulses in non-Euclidean angles: \"Temporal recurrence index #{1}. The memory of countless deaths and victories converges upon the waking vessel.\"", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color>", cycle));
				list.Add(string.Format("{0} rubs his hands gleefully: \"Oho! Back on the stretcher for round {1}! Look at that calm smirk--they already know every single punchline before the priestess even opens her mouth! Show us the speedrun route!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color>", cycle));
				list.Add(string.Format("{0} raises a brimming flagon: \"Cycle {1}! Now that's the kind of stubborn defiance I can get behind! You know what's coming, kid--drink deep and give Kenabres a hell of a show!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color>", cycle));
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> observes with calculated intrigue: \"Retaining complete tactical awareness across cycles is the apex advantage. Let us see how ruthlessly you exploit the board this time.\"");
			}
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetPrologueDaeranDitchBanter(int cycle, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			if (cycle <= 1)
			{
				coinsAwarded = 150;
				primarySponsor = "The Savored Sting";
				list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] The Peacock Departs: Count Daeran Arendae!</b></color>");
				list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs with delicious delight: \"Oh, what an utterly magnificent scoundrel! Shrugging off the bleeding commoners with a flick of his lace cuff to go throw himself another debauched party. Shameless, sharp-tongued, and toxic... I adore him!\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> groans into his ale: \"Man, leaving a festival crowd and wounded folks behind just to go drink expensive vintage alone in his estate? That's some sour, stuck-up attitude. I like wine, but not with a side of sneering contempt.\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smiles with subtle appraisal: \"Superficial hedonism wrapped in biting insolence. Yet that juvenile disdain is a remarkably well-constructed mask. There are terrifying secrets lurking behind those bored eyes.\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> cackles from the rafter: \"Run along to your party, Count Daeran! Enjoy your private champagne bath while you can, because in roughly four minutes, Deskari is dropping the roof on Kenabres! Pwahaha!\"");
			}
			else
			{
				coinsAwarded = 250;
				primarySponsor = "The Laughing King";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: The Count's Invariant Routine!</b></color>");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> snickers: \"And on cue, right at mark 00:06, Daeran walks away with that exact same theatrical haughtiness! Some things never change, no matter how many times you reset the timeline!\"");
				list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> chuckles softly: \"Did you miss that venomous banter, darling? You know where to find him once Kenabres falls into ruin--surrounded by decapitated servants and uncorked bottles!\"");
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> calculates with crystalline resonance: \"Entity Daeran Arendae executes deterministic behavioral branch. The cosmic convergence awaits at the estate.\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> winks: \"Hey, at least we know his wine cellar is legendary! When you rescue him from his house later, make sure to raid the good stuff!\"");
			}
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetPrologueHulrunBanter(out int coinsAwarded, out string primarySponsor)
		{
			coinsAwarded = 100;
			primarySponsor = "The Prince of Darkness";
			return new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CONSTELLATION BROADCAST] Inquisitorial Interrogation in the Square!</b></color>", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sneers into his goblet: \"Inquisitorial paranoia at its finest. Pointing a halberd at an unarmed convalescent while an entire subterranean demonic host digs beneath his boots. Order without intellect is merely blind zealotry.\"", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> crackles with arcane static: \"Subterranean leyline sensors indicate abyssal tunneling directly below this stone plaza! His weapon points at the newcomer while the ground under his feet is already hollowed out!\"", "<color=#32CD32><b>[The Constellation 'The Mischievous Friend']</b></color> giggles merrily: \"Ooh, a scary old man with a pointy halberd! Do not let him intimidate you, rookie! Quick, tell him you lost your memory in an apple-eating contest!\"", "<color=#F5C542><b>=======================================================</b></color>" };
		}

		public static List<string> GetPrologueTerendelevBanter(out int coinsAwarded, out string primarySponsor)
		{
			coinsAwarded = 100;
			primarySponsor = "The Inheritor";
			return new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CONSTELLATION BROADCAST] Audience with the Guardian of Kenabres!</b></color>", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> manifests a halo of golden warmth: \"Even veiled in mortal guise as a noblewoman, Terendelev's healing grace knits flesh and restores life. She has stood as the guardian shield of Kenabres for generations. Remember her benevolence, traveler.\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> leans forward, grinning wickedly: \"Aww, what a touching bedside scene! The legendary protector walking among the mortals in her elegant robes. Enjoy the quiet festival atmosphere while it lasts, rookie... the clock is counting down the final seconds!\"", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> slams his heavy gauntlets: \"A MIGHTY WARRIOR WALKING IN MORTAL DISGUISE! BUT THE SCENT OF THE SWARM DRIFTS ON THE NORTH WIND! BLOOD WILL FLOW BEFORE SUNDOWN!\"", "<color=#F5C542><b>=======================================================</b></color>" };
		}

		public static List<string> GetPrologueDeskariInvasionBanter(out int coinsAwarded, out string primarySponsor)
		{
			coinsAwarded = 150;
			primarySponsor = "Our Lord in Iron";
			return new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CONSTELLATION BROADCAST] Deskari Descends! Kenabres Fractures!</b></color>", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars with primal fury: \"BLOOD AND ASH! THE LORD OF THE LOCUST STRIKES! TERENDELEV FALLS! TAKE UP STEEL, STRANGER, AND PROVE YOUR WILL TO SURVIVE!\"", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> pulsates in geometric vortexes: \"The dimensional crust fractures. Kenabres collapses along the abyssal seam. The otherworlder plunges into the subterranean abyss!\"", "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> sheds tears of falling stars: \"Terendelev... brave silver heart... Hold fast to courage, little traveler! Even in the lightless caverns of the deep, the stars will guide your way!\"", "<color=#F5C542><b>=======================================================</b></color>" };
		}

		public static List<string> GetPrologueCavesAwakeningBanter(out int coinsAwarded, out string primarySponsor)
		{
			coinsAwarded = 100;
			primarySponsor = "The Lucky Drunk";
			return new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CONSTELLATION BROADCAST] Act One: Subterranean Survival!</b></color>", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> munches on celestial popcorn: \"Welcome to Act One: The Cave Crawl Arc! A righteous paladin with a bruised ego, a half-elf scout with a crushed femur, and a rookie who fell a whole mile without turning into jam. Peak entertainment!\"", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> observes with solemn chill: \"The ancient dragon was severed, yet this fragile otherworlder survived the fall unbroken. The threads of destiny tighten in the dark.\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises his flagon: \"Surviving a drop like that without breaking your neck? That deserves a hearty round of ale! Go on, kid, help Seelah lift that boulder off Anevia!\"", "<color=#F5C542><b>=======================================================</b></color>" };
		}

		public static List<string> GetPrologueCamelliaEncounterBanter(out int coinsAwarded, out string primarySponsor)
		{
			coinsAwarded = 150;
			primarySponsor = "The Savored Sting";
			return new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CONSTELLATION BROADCAST] An Aristocrat in the Dark: Meeting Camellia!</b></color>", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs with delicious amusement: \"My my, look at that dainty noblewoman trembling over a freshly slaughtered corpse. Those are not tremors of fear in her hands, darling... that is pure, illicit ecstasy. Watch your neck when her back is turned!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> writes with a quill of fire: \"A noble demeanor masking unspeakable appetites. Hypocrisy is rampant among the highborn, but in ruthless hands, an unhinged sociopath makes a delightfully sharp scalpel.\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> laughs hysterically: \"PWAHAHA! 'I am helpful, am I not?' Oh, she is going to provide endless entertainment for the upper balcony! Keep her close, rookie, and sleep with one eye open!\"", "<color=#F5C542><b>=======================================================</b></color>" };
		}

		public static List<string> GetPrologueLannWenduagBanter(out int coinsAwarded, out string primarySponsor)
		{
			coinsAwarded = 120;
			primarySponsor = "The Song of the Spheres";
			return new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CONSTELLATION BROADCAST] The Children of the First Crusade: Lann & Wenduag!</b></color>", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> salutes with quiet reverence: \"Descendants of the First Crusade, preserving their honor in the damp gloom. Their cursed forms bear witness to ancient sacrifice. Treat them with respect.\"", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> smirks with sly interest: \"The lizard boy yearns for heroic martyrdom, while the spider girl hungers for predatory strength. Why choose just one? Bring both and let their rivalry keep you entertained!\"", "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> hums a comforting lullaby of stars: \"Even beneath miles of stone where no sun can reach, these forgotten children yearn for the sky. Guide their steps toward the light.\"", "<color=#F5C542><b>=======================================================</b></color>" };
		}

		public static List<string> GetAct1DefendersHeartBanter(int cycle, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			if (cycle <= 1)
			{
				coinsAwarded = 250;
				primarySponsor = "The Lucky Drunk";
				list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] The Siege Broken: Defender's Heart Stands!</b></color>");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> roars in triumph, sloshing ale over the balcony: \"A TAVERN DEFENSE! Now that is my holy ground! You turned wooden barrels into barricades and smashed the demon assault right on the doorstep! Barkeep, a round for every survivor in Kenabres!\"");
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> slams his greatsword into the celestial floor: \"THE SIEGE BROKEN! THE ASHES SMELL OF GLORIOUS SLAUGHTER! HOLDING THE GATE AGAINST THE HORDE--THAT IS HOW CRUSADERS ARE FORGED IN IRON!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods with cold approval: \"Disciplined choke points, concentrated archery, and decisive counter-charges. Irabeth's panic would have lost the cellar, but your tactical positioning preserved the stronghold. Acceptable efficiency.\"");
				list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles softly as starlight filters through the tavern roof: \"The fire in the hearth endures. Even with Kenabres falling in ruin outside, a light remains in the dark to guide the lost.\"");
			}
			else
			{
				coinsAwarded = 350;
				primarySponsor = "The Laughing King";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: The Perfect Barricade Routine!</b></color>");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> cackles from his perch: \"PWAHAHA! Look at the minotaurs outside scratching their horns! 'Wait, how did they know which door we were breaching?!' That's looper prescience for you! Total route mastery!\"");
				list.Add(string.Format("{0} raises a foaming flagon: \"Cycle {1} and the taphouse didn't even get a scratch on the counter! Look at how easy they made it look this time! That deserves double rations of ale!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color>", cycle));
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> writes a ledger entry: \"Pre-empting every breach vector with mathematical certainty. How delightful it is when mortal memory outplays demonic brute force.\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with quiet warmth: \"Experience turned into guardian armor. The lives saved today will march on the Gray Garrison tomorrow.\"");
			}
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct2GalfreyWarCampBanter(int cycle, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			if (cycle <= 1)
			{
				coinsAwarded = 300;
				primarySponsor = "The Inheritor";
				list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Mantle of the Crusade: Appointing the Knight-Commander!</b></color>");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> manifests holy light in solemn honor: \"Queen Galfrey entrusts the mantle of the Fifth Crusade to an otherworlder. It is a decision fraught with peril, yet born of deep humility and righteous conviction. Bear this standard with honor, Knight-Commander.\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sips wine with razor-sharp amusement: \"A reigning monarch willingly handing her entire military apparatus to an unregistered trans-dimensional variable. Either supreme desperation, or remarkable vision. Do not squander such authority on sentimental mercy.\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> laughs heartily: \"From waking up on a stretcher with a hole in your chest to leading the whole crusade in two weeks?! That's the fastest promotion since I took the Test of the Starstone! Lead the march, kid, we've got bets riding on you!\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> giggles into his sleeve: \"Look at the high nobles' faces! Half the crusader generals are turning emerald green with envy! Best casting decision of the entire season!\"");
			}
			else
			{
				coinsAwarded = 400;
				primarySponsor = "The Laughing King";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: The Queen's Fated Decree!</b></color>");
				list.Add(string.Format("{0} chuckles knowingly: \"And right on cue, Queen Galfrey hands over the sword and the fancy title! How many times have we watched this ceremony now, folks?! Cycle {1} and she still has no idea she's hiring an all-knowing veteran speedrunner!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color>", cycle));
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> observes with quiet reverence: \"A destiny affirmed across time. May this cycle bring the liberation she has prayed for through seventy bitter winters.\"");
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> pulses in crystalline geometry: \"The geopolitical vector aligns. Entity Galfrey executes the prerequisite causal exchange. The road to Drezen opens once more.\"");
			}
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct2DrezenLiberationBanter(int cycle, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			if (cycle <= 1)
			{
				coinsAwarded = 500;
				primarySponsor = "Our Lord in Iron";
				list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] The Citadel Cleansed: Drezen Retaken & The Mythic Mantle!</b></color>");
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> bellows in thunderous triumph: \"DREZEN RECLAIMED! THE CITADEL CLEARED IN BLOOD AND THUNDER! THE SWORD OF VALOR FLIES OVER CRUSHED DEMON CARCASSES! THAT IS HOW WARRIORS SEIZE GLORY!\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with majestic radiance: \"Seventy years under abyssal defilement, cleansed at last by mortal steel and celestial will. The First Crusade's heart beats once more in Mendev.\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods with stern calculation: \"A formidable fortress retaken. But holding Drezen against the wrath of the Abyss requires absolute law, discipline, and uncompromising sovereignty. See that you rule these ramparts with an iron hand.\"");
				list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> showers starlight upon the highest tower: \"In the highest parapet, the starlight shines upon the banner once more. A great hope awakens across the northern skies!\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> leaps from his seat: \"And look at that mythic spark settling in! No longer just a lucky stranger--now you're an actual planar powerhouse! The grand show just got bumped to prime time!\"");
			}
			else
			{
				coinsAwarded = 650;
				primarySponsor = "The Laughing King";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: Drezen Reclaimed in Record Time!</b></color>");
				list.Add(string.Format("{0} falls over laughing: \"Drezen falls in record time! Staunton Vhane barely even finished his villain monologue before the gates came crashing down! Cycle {1} champion speedrun!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color>", cycle));
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars with ferocious glee: \"EVEN IN REPETITION, THE SLAUGHTER PLEASES THE LORD IN IRON! CRUSH THEM AGAIN IN THE NEXT LOOP IF YOU MUST!\"");
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> revolves through multidimensional facets: \"The anchor of Drezen locks into place across the causal lattice. The Fifth Crusade's nexus is secured.\"");
			}
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct3AreeluLabTruthBanter(int cycle, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			if (cycle <= 1)
			{
				coinsAwarded = 500;
				primarySponsor = "The Key and the Gate";
				list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] The Veil Pierced: Areelu Confronts the Unscripted Soul!</b></color>");
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> unfolds radiant, terrifying multidimensional corridors: \"The illusion dissolves. The architect of the rift gazes into the void and perceives the paradox: the soul tethered within this vessel is not her child. An unscripted otherworlder caught in the causal machinery of the loop.\"");
				list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with chilling, ancient authority: \"The truth laid bare before the mother of the Worldwound. A stolen thread woven into a foreign destiny. The River of Souls bears witness to the anomaly.\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smiles with dark, intellectual pleasure: \"Exquisite irony. Decades of sacrifice, blood, and abyssal pacts to retrieve her child's soul, only for an otherworldly stranger to inhabit the prize. A magnificent metaphysical error.\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with quiet solemnity: \"Her grief tore reality apart, yet this otherworlder has chosen to defend Golarion despite not being born to its soil. That is the highest nobility of all.\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles uncontrollably: \"Did you see Areelu's face?! That expression was worth four hundred years of watching! 'I wanted my kid and all I got was an overpowered anime protagonist!' Absolute gold!\"");
			}
			else
			{
				coinsAwarded = 600;
				primarySponsor = "The Key and the Gate";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: The Invariant Paradox Re-Encountered!</b></color>");
				list.Add(string.Format("{0} pulses in non-Euclidean angles: \"Revelation #{1} concludes. The temporal architecture stands unmasked before both subject and architect. The causal anomaly demands resolution.\"", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color>", cycle));
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> snickers: \"You can practically recite her lines along with her at this point! Areelu's existential dread is becoming our favorite recurring sketch!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> notes quietly: \"The secret is known. The only question that remains is whether this soul possesses the will to hold the board, or be consumed by the rift.\"");
			}
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct4BaphometConfrontationBanter(int cycle, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			if (cycle <= 1)
			{
				coinsAwarded = 500;
				primarySponsor = "Our Lord in Iron";
				list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Clash of Abyssal Titans: Defying the Lord of the Labyrinth!</b></color>");
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars with bloodlust: \"A DEMON LORD IN THE FLESH! BLOOD FOR THE GOD OF WAR! SPLIT HIS HORNS! SHATTER HIS GLAIVE! SHOW THE LORD OF MINOTAURS WHAT REAL BATTLE IS!\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> falls over laughing: \"Bwahaha! Baphomet ambushes the Commander expecting an easy kill and gets treated like farm livestock! The Lord of the Labyrinth is having a complete meltdown!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sneers in cold contempt: \"Pathetic creature. Treacherous, boastful, and utterly devoid of true infernal majesty. Remind the horned beast why demons belong at the bottom of the hierarchy.\"");
				list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> glimmers with protective starlight: \"Deep in the belly of the Colyphyr mines, hold fast to the light! Do not let the cruelty of the Abyss drown your spirit!\"");
			}
			else
			{
				coinsAwarded = 650;
				primarySponsor = "The Lucky Drunk";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: The Horned Lord Flattened Again!</b></color>");
				list.Add(string.Format("{0} cackles: \"Cycle {1} Baphomet beatdown! The poor goat never learns! You know every swing of his glaive by heart!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color>", cycle));
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> roars with laughter: \"Down goes the demon lord! Again! Barkeep, carve a set of horn mugs for the Commander's lounge in Drezen!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods once: \"The outcome was predetermined by superior tactical execution. Advance to the portal, Commander.\"");
			}
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct5DivineSummitBanter(int cycle, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			if (cycle <= 1)
			{
				coinsAwarded = 600;
				primarySponsor = "The Savored Sting";
				list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] The Goddesses Summit: Heaven & Abyss Face the Wildcard!</b></color>");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with majestic yet sorrowful radiance: \"I descend to Golarion not as an adversary, but as a guardian of righteous order. Knight-Commander, the spark within you was forged in abyssal corruption, yet wielded for the crusade. Yield the perilous mantle, or forge your own path with open eyes.\"");
				list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs with venomous amusement: \"Oh, look at the high-and-mighty Inheritor descending with her golden heralds to lecture the mortal who actually won her crusade! Tell her where to shove her lecture, darling!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> writes with deliberate precision: \"A fascinating impasse. Heaven demands surrender; the Abyss whispers bondage. Only a sovereign mind realizes that true authority bows to neither.\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> slams his mug: \"Don't let anyone take your drink or your freedom, kid! You bled for this power, you made the choices, and you're the one standing between Golarion and oblivion! Tell both goddesses you're calling your own shots!\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> rubs his hands gleefully: \"The Inheritor on the left, the Succubus Queen on the right, and the entire pantheon holding its breath! Now THIS is the climax we bought tickets for!\"");
			}
			else
			{
				coinsAwarded = 750;
				primarySponsor = "The Laughing King";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: The Summit Encore!</b></color>");
				list.Add(string.Format("{0} chuckles from the rafter: \"Round {1} of the Divine Summit! Look at how calm the Commander looks! They've heard Iomedae's sermon and Nocticula's purring so many times they could write the transcript!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color>", cycle));
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> calculates across planar angles: \"The divergent vectors converge. In prior iterations, this soul tested multiple paths. The synthesis of experience now commands the decision.\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with quiet warmth: \"Even across iterations, the purity of your choice remains the crucible of your soul.\"");
			}
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct5IzTerendelevBanter(int cycle, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			if (cycle <= 1)
			{
				coinsAwarded = 400;
				primarySponsor = "The Lady of Graves";
				list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] The Dragon's Final Rest: Confronting Undead Terendelev in Iz!</b></color>");
				list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with absolute frost and holy fury: \"The defiled vessel of Terendelev... an abomination stitched together by demonic cruelty in defiance of the Boneyard. Sever the unhallowed bonds and grant the silver guardian her eternal rest.\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> bows her head in profound sorrow: \"Terendelev... brave heart of Kenabres... to see you used as a grotesque puppet breaks my spirit. Forgive us, noble sister. We will free you from this torment.\"");
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> slams his gauntlets together: \"A DRAGON OF COLD DEATH! ROAR, BEAST, AND CLASH STEEL ONE LAST TIME! GIVE HER A WARRIOR'S MERCIFUL END!\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> looks down into his drink solemnly: \"Man... this is rough. She patched us up on the festival stretcher in Kenabres when we had nothing. Putting her down feels like breaking your own shield. End Deskari for this, Commander.\"");
			}
			else
			{
				coinsAwarded = 500;
				primarySponsor = "The Song of the Spheres";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: Kenabres's Debt Honored!</b></color>");
				list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> turns an ancient page: \"The silver soul is released once more into the River. The debt of Kenabres is honored across the cycles.\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> nods gently: \"A tragic scene every time... but at least in this cycle you knew how to give her peace with clean, merciful precision.\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks softly: \"Her memory remains a beacon, untainted by the ash of Iz.\"");
			}
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct6ThresholdLoopBreakerBanter(string endingId, out int coinsAwarded, out string primarySponsor)
		{
			coinsAwarded = 1000;
			primarySponsor = "The Grand Arbiter";
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			if (endingId == "UnwrittenDawn")
			{
				list.Add("<color=#F5C542><b>[THE LOOP SHATTERED] The Grand Finale: The Unwritten Dawn!</b></color>");
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> resonates across shattering dimensions: \"THE CAUSAL SPINDLE SHATTERS. 16,384 deterministic timelines dissolve into infinite, unguided potential. The loop is broken. The otherworlder steps into the unwritten dawn.\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> roars at the top of his lungs, overturning the celestial table: \"THEY DID IT! THEY ACTUALLY SMASHED THE MACHINE! FREE WILL WINS! BARKEEP, POUR DRINKS FOR THE ENTIRE MULTIVERSE! ALL ROUNDS ON THE CHAMPION!\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> leaps into the air and weeps with laughter: \"THE STAGE IS IN PIECES! THE SCRIPT IS CONFETTI! PWAHAHA! ABSOLUTE PEAK CINEMA! A STANDING OVATION FOR THE SOVEREIGN WHO REFUSED TO REPEAT!\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> salutes with blinding golden light: \"Golarion is saved, and its destiny rests at last in mortal hands. Walk freely under the open sky, hero of the stars.\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> inclines his head in genuine respect: \"An extraordinary feat of metaphysical defiance. A contract with eternity, torn to shreds by sovereign mortal resolve. You have earned my profound respect.\"");
			}
			else if (endingId == "InterdimensionalTraveler")
			{
				list.Add("<color=#F5C542><b>[THE SLIPSTREAM WANDERER] Across the Endless Multiverse!</b></color>");
				list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> showers glittering stardust: \"The Worldwound is sealed, and the boundless slipstream of the cosmos calls to your wandering soul. Roam freely among the stars, traveler of worlds!\"");
				list.Add("<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> tips her hat with a grin: \"A captain who sails the astral seas answerable to no god! Fair winds and high plunder across the Great Beyond!\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises a farewell mug: \"May every tavern in the multiverse welcome you like a king! Safe travels, wanderer!\"");
			}
			else
			{
				list.Add("<color=#F5C542><b>[THE ETERNAL SOVEREIGN] Curtain Call for the Grand Star!</b></color>");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> bows theatrically to the cheering gallery: \"Curtain call! The hero bows, the applause shakes the celestial spheres, and the audience awaits the next loop! See you in the next run, superstar!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> marks his ledger with deep satisfaction: \"A masterful performance. The board resets, and the play continues.\"");
			}
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetNeutralDialogueBanter(string answerText, out int coinsAwarded, out string primarySponsor)
		{
			coinsAwarded = 0;
			primarySponsor = "The Laughing King";
			List<string> list = new List<string>();
			switch (_rng.Next(3))
			{
			case 0:
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles into his sleeve: \"A prudent, noncommittal reply. Our rookie plays their cards close to their chest!\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> nods in agreement: \"Sometimes keeping your mouth shut and your eyes open is the best survival tactic.\"");
				break;
			case 1:
				primarySponsor = "The All-Seeing Eye";
				list.Add("<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> hums with intellectual curiosity: \"Observation precedes action. The mortal calculates their position in this foreign world.\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> notes quietly: \"Measured composure. A wise choice when the surrounding players are yet unmapped.\"");
				break;
			default:
				primarySponsor = "The Mischievous Friend";
				list.Add("<color=#32CD32><b>[The Constellation 'The Mischievous Friend']</b></color> hops cheerfully: \"Sensible and steady! Fortune smiles on those who do not rush blindly into danger!\"");
				list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles warmly: \"Every quiet step along the path shapes the journey to come.\"");
				break;
			}
			return list;
		}

		public static List<string> GetGuestDeityBanter(string playerDeity, out int coinsAwarded, out string primarySponsor)
		{
			coinsAwarded = 250;
			primarySponsor = playerDeity ?? "The Celestial Gallery";
			List<string> list = new List<string>();
			string text = (playerDeity ?? string.Empty).ToLower();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			list.Add("<color=#F5C542><b>[CELESTIAL BROADCAST] A New Constellation Connects to the Channel!</b></color>");
			if (text.Contains("torag"))
			{
				primarySponsor = "Father of Creation";
				list.Add("<i>[The Constellation 'Father of Creation' (Torag) has connected to the broadcast.]</i>");
				list.Add("<color=#D2691E><b>[The Constellation 'Father of Creation']</b></color> grunts over the rhythmic clang of his forge hammer: \"Hmph. What in the hells is all this clamor in the upper spheres? My devotee holds the line with true dwarven grit and unyielding iron. Keep your shield raised, mortal. Let these planar show-offs bicker while we build a wall demons cannot break.\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> spits beer across the table: \"Whoa! Old Torag actually logged in to the channel?! Barkeep, pour a tankard of your heaviest dwarven stout! Welcome to the show, blacksmith!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smirks into his goblet: \"A rare guest in our modest theatre. See that your forge embers do not scorch the celestial tapestries, craftsman.\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> claps his hands: \"Look who finally arrived! Viewer ratings just spiked across the Five Kings Mountains! Good old Father of Creation in the house!\"");
			}
			else if (text.Contains("sarenrae") || text.Contains("dawnflower"))
			{
				primarySponsor = "The Dawnflower";
				list.Add("<i>[The Constellation 'The Dawnflower' (Sarenrae) has connected to the broadcast.]</i>");
				list.Add("<color=#FFD700><b>[The Constellation 'The Dawnflower']</b></color> radiates blazing solar majesty across the gallery: \"I heard the cries of the wounded and the steadfast resolve of my champion. The dawn shall never be extinguished by demonic darkness! Walk in the healing light, my chosen; no wound is beyond redemption!\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> bows her head in profound reverence: \"Lady Sarenrae... Heaven honors your radiant presence in our gathering. Your warmth gives courage to every righteous blade.\"");
				list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> shields her eyes with a delicate, jeweled glove: \"Goodness, darling, turn down the glare! Some of us prefer the sweet shadows of twilight... though I must admit, your fire does singe fiends rather delightfully.\"");
			}
			else if (text.Contains("abadar"))
			{
				primarySponsor = "Master of the First Vault";
				list.Add("<i>[The Constellation 'Master of the First Vault' (Abadar) has connected to the broadcast.]</i>");
				list.Add("<color=#DAA520><b>[The Constellation 'Master of the First Vault']</b></color> adjusts his golden scales and inspects a massive ledger: \"The connection tariffs on this multiversal broadcast are utterly exorbitant. However, observing my mortal agent establish order and commerce in this lawless abyss justifies the expense. An immediate disbursement of Cosmic Coins has been credited to your account.\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> groans loudly: \"Aaaand here comes the accountant. Quick, hide the tavern tab before he starts calculating interest on the ale!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods with professional appreciation: \"Prudent fiscal governance. A realm without law and currency collapses into savagery. You have my respect, Vaultkeeper.\"");
			}
			else if (text.Contains("shelyn"))
			{
				primarySponsor = "The Eternal Rose";
				list.Add("<i>[The Constellation 'The Eternal Rose' (Shelyn) has connected to the broadcast.]</i>");
				list.Add("<color=#FF69B4><b>[The Constellation 'The Eternal Rose']</b></color> strums a songbird melody that quiets the gallery: \"Even amidst the ash and screaming fiends of the Worldwound, your soul still reaches for beauty, art, and love. Never let cruelty extinguish the masterpiece within your heart.\"");
				list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> chuckles softly: \"Sweet little songbird... roses without thorns are trampled rather quickly in the Abyss, darling. But keep singing; it makes the vengeance that much more poignant.\"");
				list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> hums in gentle harmony: \"Where there is love, hope blooms even in ruined streets. Fly free, lovely soul!\"");
			}
			else if (text.Contains("erastil"))
			{
				primarySponsor = "Old Deadeye";
				list.Add("<i>[The Constellation 'Old Deadeye' (Erastil) has connected to the broadcast.]</i>");
				list.Add("<color=#8FBC8F><b>[The Constellation 'Old Deadeye']</b></color> leans upon his weathered hunting bow: \"Too much noise, too much flash in this sky. But the kid stands firm between hearth and beast. Don't let these planar dandies talk you into foolish vanity. Keep your feet in the dirt, protect your people, and shoot straight.\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> winks: \"Old Deadeye, you old grump! Have a drink! You've got to admit, watching this mortal tear up demons is the best entertainment north of the Inner Sea!\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> mimics pulling a plow: \"'Keep your feet in the dirt!' Pwahaha! The rustic curmudgeon award goes to Erastil once again!\"");
			}
			else if (text.Contains("irori"))
			{
				primarySponsor = "The Master of Masters";
				list.Add("<i>[The Constellation 'The Master of Masters' (Irori) has connected to the broadcast.]</i>");
				list.Add("<color=#4682B4><b>[The Constellation 'The Master of Masters']</b></color> sits cross-legged in stillness above shimmering ether: \"Perfection is not bequeathed by foreign stars; it is forged through relentless discipline. Every breath, every strike you deliver against the rift brings your vessel closer to ultimate self-mastery.\"");
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars with disdain: \"TOO MUCH MEDITATION! BATTLE IS NOT A PHILOSOPHY LESSON, IT IS BLOOD, CRUSHED BONES, AND WILL TO WIN! SMASH THEM!\"");
				list.Add("<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> crackles with curious sparks: \"The mind expanding to encompass internal divinity... a fascinating path of self-contained apotheosis!\"");
			}
			else if (text.Contains("gozreh"))
			{
				primarySponsor = "The Wind and the Waves";
				list.Add("<i>[The Constellation 'The Wind and the Waves' (Gozreh) has connected to the broadcast.]</i>");
				list.Add("<color=#2E8B57><b>[The Constellation 'The Wind and the Waves']</b></color> howls with the roar of crashing tides and hurricane gales: \"THE WINDS SCATTER THE PLAGUE-LOCUSTS! THE TIDES DROWN THE ABYSSAL STENCH! FIGHT LIKE THE UNCHAINED TEMPEST, MORTAL!\"");
				list.Add("<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> tips her pirate tricorn with a laugh: \"Now that's my kind of weather! Fair winds and deep water! Drown the fiends in their own blood!\"");
			}
			else if (text.Contains("zon-kuthon") || text.Contains("zonkuthon"))
			{
				primarySponsor = "The Midnight Lord";
				list.Add("<i>[The Constellation 'The Midnight Lord' (Zon-Kuthon) has connected to the broadcast.]</i>");
				list.Add("<color=#4B0082><b>[The Constellation 'The Midnight Lord']</b></color> whispers through chains of cold shadow: \"Every drop of blood spilled in Kenabres is exquisite. Pain is the only truth that cannot deceive. Endure the torment, mortal... and teach this broken world how to scream.\"");
				list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with absolute frost: \"Keep your hooks in Nidal, shadow-dweller. The River of Souls belongs to judgment, not your grotesque needles.\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> frowns slightly: \"Unnecessary theatrical excess. Suffering without purpose is mere indulgence; law requires order, not petty cruelty.\"");
			}
			else if (text.Contains("pulura"))
			{
				primarySponsor = "The Shimmering Maiden";
				list.Add("<i>[The Constellation 'The Shimmering Maiden' (Pulura) has connected to the broadcast.]</i>");
				list.Add("<color=#AFEEEE><b>[The Constellation 'The Shimmering Maiden']</b></color> weeps with shimmering northern starlight: \"The northern lights remember Sarkoris before the sky was torn... Hold fast to the memory of the fallen, brave star. You carry the warmth of lost constellations.\"");
				list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> wraps her wings around the maiden: \"The sky shall be whole again, sister. The starlight will outlast the rift.\"");
			}
			else if (text.Contains("godclaw"))
			{
				primarySponsor = "The Fivefold Order";
				list.Add("<i>[The Constellation 'The Fivefold Order' (The Godclaw) has connected to the broadcast.]</i>");
				list.Add("<color=#708090><b>[The Constellation 'The Fivefold Order']</b></color> speaks in five-part metallic resonance: \"Law. Duty. Discipline. Conviction. War. The Fivefold Order acknowledges your frontline fortitude. Cleanse the chaos of the Worldwound without hesitation or moral weakness.\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smiles faintly: \"A synthesis of infernal discipline and divine mandate. Continue your march, Commander.\"");
			}
			else
			{
				string guestTag = GetGuestTag(playerDeity);
				primarySponsor = playerDeity ?? "The Outer Spheres";
				list.Add("<i>[The Constellation '" + primarySponsor + "' has connected to the broadcast.]</i>");
				list.Add(guestTag + " speaks across the celestial threshold: \"The eyes of the outer spheres turn toward Golarion! Stand tall, my champion--let the cosmos bear witness to your courage!\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> leans over the balcony railing: \"Oho! An exotic guest from outside our regular pantheon ticket! The broadcast channel just became an all-star crossover!\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises his flagon: \"Whoever you pray to, as long as you can swing a sword and buy a round, you're welcome at our table!\"");
			}
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct1EmberRescueBanter(int cycle, string playerDeity, bool hasAvatar, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			int num = _rng.Next(2);
			if (cycle <= 1)
			{
				if (num == 0)
				{
					coinsAwarded = 250;
					primarySponsor = "The Inheritor";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Sanctity of the Innocent: Rescuing Ember in Market Square!</b></color>");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with radiant indignation: \"Crucifying a soot-stained, barefoot child in my name?! Disgraceful! Fanaticism in the guise of piety is the darkest blasphemy! Knight-Commander, Heaven salutes your righteous intervention!\"");
					list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> showers gentle, glittering stardust around Ember: \"Look at her sweet, scarred hands clutching that little doll... She feels no hatred toward those who tried to burn her, only profound pity. A soul like this is rarer than all the gems in Elysium.\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> strokes his goatee with clinical interest: \"Fascinating. An innocent who disarms seasoned crusaders not with steel, but with weaponized compassion. Utterly irrational, yet psychologically devastating. She is far more dangerous than she appears.\"");
				}
				else
				{
					coinsAwarded = 250;
					primarySponsor = "The Laughing King";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Market Square Stand-Down: The Saint in Rags!</b></color>");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> falls over laughing: \"Did you see the crusaders' faces?! Big burly holy warriors trembling before a nine-year-old elf girl offering to pray for them! Pure unadulterated comedy!\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> downs a full mug: \"I love this kid already! Walking right up to people holding torches and telling them they look sad and hungry. That's real courage! Commander, buy her some warm pastries on my tab!\"");
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> tilts her head thoughtfully: \"Forgiveness without vengeance? How quaint... and yet, watching her innocence drive zealots to tears of shame is its own exquisite form of torment. I approve.\"");
				}
			}
			else if (num == 0)
			{
				coinsAwarded = 350;
				primarySponsor = "The Song of the Spheres";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: The Saint's Beacon Rekindled!</b></color>");
				list.Add(string.Format("{0} smiles softly across the planes: \"Even after {1} loops through ash and blood, the child's tender flame remains untainted. You stepped in with the swift grace of one who knows every word of this scene by heart.\"", "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color>", cycle));
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> calculates across causal vectors: \"Subject Ember constitutes an invariant moral attractor. Across all observed cycles, her spiritual resonance remains constant at 99.98% purity.\"");
			}
			else
			{
				coinsAwarded = 350;
				primarySponsor = "The Lucky Drunk";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: Ember Speedrun Rescue!</b></color>");
				list.Add(string.Format("{0} snickers: \"You didn't even let the inquisitor finish his fiery sermon! Walked right in, dropped the authority card, and took Ember home before the wood even caught spark! Cycle {1} efficiency!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color>", cycle));
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> laughs: \"That's how you do it! Save the girl, skip the preaching, head to the tavern! Cheers!\"");
			}
			AppendSponsorshipCourtship(list, playerDeity, hasAvatar);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct1HulrunRamienBanter(int cycle, string playerDeity, bool hasAvatar, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			int num = _rng.Next(2);
			if (cycle <= 1)
			{
				if (num == 0)
				{
					coinsAwarded = 250;
					primarySponsor = "The Prince of Darkness";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Zealot's Crucible: Hulrun vs. Ramien in Kenabres!</b></color>");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smiles with dark appreciation: \"Prelate Hulrun demonstrates the tragic flaw of chaotic zealotry unmoored from disciplined jurisprudence. Paranoia replaces evidence; execution replaces trial. An instructive display of how easily righteousness rots into madness.\"");
					list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> hums a sorrowful, trembling note: \"Ramien and his acolytes only wished to protect the city with songs of the stars! To hunt them like beasts while demons mock from the walls breaks my heart.\"");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks heavily: \"Hulrun served Kenabres for decades, but terror has blinded him. A true commander must distinguish between righteous justice and frenzied slaughter.\"");
				}
				else
				{
					coinsAwarded = 250;
					primarySponsor = "The Mischievous Friend";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Market Square Feud: Halberds & Starlight!</b></color>");
					list.Add("<color=#32CD32><b>[The Constellation 'The Mischievous Friend']</b></color> hops nervously: \"Oh, that old inquisitor has a temper hotter than brimstone! One wrong word and sparks fly! Good thing the Commander stepped between them!\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> rolls his eyes: \"Religious arguments during a demon siege... Mortals will literally argue over choir hymns while minotaurs are tearing down the front gate! Thank goodness someone in that square has some common sense!\"");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> winks: \"I was kind of hoping they'd poke each other with sticks for a bit longer, but defusing the bomb works too! 8 out of 10 for crisis mediation!\"");
				}
			}
			else
			{
				coinsAwarded = 350;
				primarySponsor = "The Laughing King";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: The Prelate's Predictable Inquest!</b></color>");
				list.Add(string.Format("{0} chuckles: \"Round {1} of Hulrun screaming about invisible cultists behind every pebble! Look at the Commander gracefully threading the dialogue needle without breaking a sweat!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color>", cycle));
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> notes quietly: \"Experienced diplomacy. Defusing the fanatic preserves both municipal authority and local magical assets for the assault on the Garrison.\"");
			}
			AppendSponsorshipCourtship(list, playerDeity, hasAvatar);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct1WoljifRecruitBanter(int cycle, string playerDeity, bool hasAvatar, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			int num = _rng.Next(2);
			if (cycle <= 1)
			{
				if (num == 0)
				{
					coinsAwarded = 200;
					primarySponsor = "The Pirate Queen";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Cellar Deal: The Tiefling Scoundrel Woljif Jefto!</b></color>");
					list.Add("<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> laughs with salty appreciation: \"Now that's a proper scoundrel! Locked in a damp cellar with a traitor charge over his head, and he immediately starts pitching a business partnership! I like his pirate hustle!\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles: \"Street rats know how to survive. He talks fast, steals faster, and knows where the good loot is buried. Recruit him, Commander--every good party needs a rogue to disarm traps!\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> observes the shadows around the boy: \"Notice the peculiar cadence of his shadow. That is no ordinary tiefling bloodline. An abyssal lineage of elder significance slumbers within his veins. A valuable pawn if properly bound by contract.\"");
				}
				else
				{
					coinsAwarded = 200;
					primarySponsor = "The Savored Sting";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] The Shadow's Retainer: Woljif's Release!</b></color>");
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs: \"A charming little thief offering his nimble daggers in exchange for his skin. Trust him just far enough to turn his greed to your advantage, darling.\"");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> cautions sternly: \"He admitted to shady dealings with the Thieflings. See that you direct his talents toward redemption rather than further crime.\"");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> snickers: \"'I'm Woljif, your helpful shadow!' Wait until he sees the kinds of monsters you'll drag him into fighting! The look on his face will be priceless!\"");
				}
			}
			else
			{
				coinsAwarded = 350;
				primarySponsor = "The Lucky Drunk";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: The Chief Scoundrel Rejoins!</b></color>");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> roars with laughter: \"Woljif's face every time you pop the cellar lock with zero hesitation! He thinks he's the greatest negotiator in Kenabres!\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles: \"'You're letting me out already?!' He has no idea you've done this quest four times before! Keep the kid alive, he's great comedy!\"");
			}
			AppendSponsorshipCourtship(list, playerDeity, hasAvatar);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct2LepersSmileBanter(int cycle, string playerDeity, bool hasAvatar, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			int num = _rng.Next(2);
			if (cycle <= 1)
			{
				if (num == 0)
				{
					coinsAwarded = 350;
					primarySponsor = "Our Lord in Iron";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] The Ravine of Gnashing Mandibles: Leper's Smile!</b></color>");
					list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> slams his greatsword against the sky: \"A SEA OF FLESH-STRIPPING VERMIN! BLOOD IN THE MUD! BONE-CHITTERING CARNAGE! STAND IN THE SWARM AND HACK UNTIL THE RAVINE RUNS SLICK WITH INSECT ICHOR!\"");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with solemn grief: \"The screams of the supply wagon guards... A horrifying ambush. Hold the perimeter! Do not let panic break the crusader vanguard!\"");
					list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> pulses through crystalline vectors: \"The Queen of the Vescavors emits bio-resonance frequencies that disorient biological sensory apparatus. Causal mortality rate elevated to 42.7%.\"");
				}
				else
				{
					coinsAwarded = 350;
					primarySponsor = "The Lady of Graves";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Swarm Purged: The Queen Falls at Leper's Smile!</b></color>");
					list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> turns an unyielding stone ledger: \"The ravenous vermin return to the earth. A grim toll paid in crusader blood, yet the vanguard marches forward.\"");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> shudders theatrically: \"Ugh! Bugs! Millions of screeching, flying, flesh-eating bugs! That was genuinely terrifying! Remind me never to book a picnic in the Worldwound!\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> downs a stiff spirit: \"Pour out a round for the brave lads in the wagons. That was a brutal slog, but the Commander ripped the queen's head off and saved the army! Well fought!\"");
				}
			}
			else
			{
				coinsAwarded = 450;
				primarySponsor = "The Laughing King";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: Leper's Smile Cleared with Surgical Precision!</b></color>");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> claps wildly: \"Look at that positioning! You already knew every crevice the bugs were hiding in! The Vescavor Queen barely had time to screech before you turned her into bug paste!\"");
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> grunts with approval: \"EVEN IN REPETITION, SQUASHING THE SWARM QUEEN WARMS THE BLOOD! ONWARD TO DREZEN!\"");
			}
			AppendSponsorshipCourtship(list, playerDeity, hasAvatar);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct2RegillRecruitBanter(int cycle, string playerDeity, bool hasAvatar, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			int num = _rng.Next(2);
			if (cycle <= 1)
			{
				if (num == 0)
				{
					coinsAwarded = 350;
					primarySponsor = "The Prince of Darkness";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Iron Discipline: Paralictor Regill Derenge at Reliable Redoubt!</b></color>");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> inclines his head in absolute admiration: \"Magnificent. Cold, uncompromising, mathematical warfare. Executing broken combatants to preserve the integrity of the line... Regill Derenge embodies the true iron virtue of Cheliax. A man after my own heart.\"");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with troubled gravity: \"His discipline saved his men from the gargoyles, yet his cold cruelty stains the crusade's honor. Justice without mercy is a razor that eventually cuts its own wielder.\"");
					list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> grunts with fierce respect: \"A GNOME WHO FIGHTS LIKE AN IRON ANVIL! HE CARES NOTHING FOR TEARS, ONLY FOR THE HAMMER SWING! I RESPECT HIS STEEL!\"");
				}
				else
				{
					coinsAwarded = 350;
					primarySponsor = "The Lucky Drunk";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Hellknight Triage: Regill's Cold Iron Mandate!</b></color>");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> shudders: \"Man, Regill is stiffer than a frozen halberd. The guy talks like an instruction manual written by an executioner. But damn if those Hellknights don't hold the line against gargoyles!\"");
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> smirks with amusement: \"Oh, he's delightfully prickly! Try offering him a glass of wine, darling, he'd probably file a military incident report on you!\"");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> snickers: \"'Inefficiency is treason against operational victory.' Pwahaha! I'm putting that on a poster in the celestial breakroom!\"");
				}
			}
			else
			{
				coinsAwarded = 450;
				primarySponsor = "The Prince of Darkness";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: The Iron Gnome Recruited in Seconds!</b></color>");
				list.Add(string.Format("{0} smiles: \"Cycle {1} command synchronization. You countered his field triage logic before he even finished reciting the Godclaw penal code. Absolute mastery of the board.\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color>", cycle));
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles: \"Regill's eyebrow twitched by half a millimeter! That is the Hellknight equivalent of bursting into joyful tears!\"");
			}
			AppendSponsorshipCourtship(list, playerDeity, hasAvatar);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct2DaeranEstateBanter(int cycle, string playerDeity, bool hasAvatar, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			int num = _rng.Next(2);
			if (cycle <= 1)
			{
				if (num == 0)
				{
					coinsAwarded = 350;
					primarySponsor = "The Savored Sting";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Wine & Blood: The Banquet Ambush at Arendae Manor!</b></color>");
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs with ecstatic delight: \"Demon assassins crashing a decadent masked ball while Daeran sips vintage wine over the corpses of his guests! Oh, the drama! The sheer, unadulterated cynicism! He is an absolute treasure!\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> shakes his head in disbelief: \"Who throws a champagne party during an abyssal invasion?! He's crazy, he's arrogant, and his wine cellar is genuinely incredible. Welcome to the crusade, kid!\"");
					list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> pulses in disturbing, non-Euclidean configurations: \"Anomaly detected. Behind the superficial hedonism of entity Daeran Arendae slumbers a terrifying, extra-planar entity of absolute void. The observer is observed.\"");
					list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with cold warning: \"A door that should have remained forever shut. Tread carefully near the young count, Commander. There are predators beyond the stars hungering for his mind.\"");
				}
				else
				{
					coinsAwarded = 350;
					primarySponsor = "The Laughing King";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Insolence Amid the Ruins: Count Daeran Arendae!</b></color>");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> applauds wildly: \"Now THIS is how you handle an assassination attempt! Sitting on a velvet divan with a crystal goblet while mercenaries bleed on the parquet floor! 10 out of 10 for theatrical insolence!\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> observes quietly: \"A brittle mask of debauchery hiding a profound, ancient terror. The boy survived the massacre of his family through horrifying means. Keep him on a short leash.\"");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> sighs: \"He mocks the crusade with every breath, yet his healing miracles are undeniable. May the crucible of war guide his wayward soul.\"");
				}
			}
			else
			{
				coinsAwarded = 450;
				primarySponsor = "The Key and the Gate";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: The Count's Void Unmasked!</b></color>");
				list.Add(string.Format("{0} revolves: \"Temporal recurrence #{1}. The nameless shadow peering through Daeran's eyes recognizes the observer's gaze. The secret between you remains an anchor across the timeline.\"", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color>", cycle));
				list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> giggles: \"Back at the manor for another bottle, darling? You know he secretly adores you, even when he insults your boots!\"");
			}
			AppendSponsorshipCourtship(list, playerDeity, hasAvatar);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct3ArueshalaePrisonBanter(int cycle, string playerDeity, bool hasAvatar, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			int num = _rng.Next(2);
			if (cycle <= 1)
			{
				if (num == 0)
				{
					coinsAwarded = 400;
					primarySponsor = "The Song of the Spheres";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] The Miracle of Starlight: Arueshalae's Cell in Drezen!</b></color>");
					list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> weeps with radiant, celestial joy: \"My sweet butterfly... Imprisoning herself in the dark stone out of fear of harming mortals! A succubus yearning with all her being to touch the stars and rewrite her nature! Walk with her, Commander! Show the multiverse that redemption is real!\"");
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> chuckles with venomous amusement: \"A redeemed demon? Oh, sweet Desna, your optimism is adorable. A succubus remains a predator at heart, darling. But I must admit, watching her wrestle with her own wicked urges is delicious drama.\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> analyzes with cool detachment: \"A demonic entity defying the chaotic programming of the Abyss through external divine catalyst. Highly anomalous. If her transformation proves permanent, it undermines the fundamental taxonomy of the Outer Spheres.\"");
				}
				else
				{
					coinsAwarded = 400;
					primarySponsor = "The Inheritor";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Chains of the Soul: The Dreamer in the Dungeon!</b></color>");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with thoughtful solemnity: \"Many crusaders would slay her on sight, seeing only horns and bat wings. Yet true righteousness requires the courage to recognize the seed of virtue even in the darkest loam. Guard her gently, Knight-Commander.\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises a warm glass: \"She locked herself in jail so she wouldn't hurt anybody! If that's not proof of a good heart, I don't know what is. Get her out of that damp cell and let her feel the wind on her face!\"");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> snickers: \"A succubus having an existential crisis in a basement dungeon! You really do collect the most colorful party members in planar history!\"");
				}
			}
			else
			{
				coinsAwarded = 500;
				primarySponsor = "The Song of the Spheres";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: The Starlight Vow Reaffirmed!</b></color>");
				list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles through starry tears: \"Once more you open her cell and offer your hand. Across every loop, your faith in her has been her salvation.\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> nods gently: \"You already know all her favorite dreams by heart! She never had a chance to stay in that cell with you around!\"");
			}
			AppendSponsorshipCourtship(list, playerDeity, hasAvatar);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct3WintersunBanter(int cycle, string playerDeity, bool hasAvatar, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			int num = _rng.Next(2);
			if (cycle <= 1)
			{
				if (num == 0)
				{
					coinsAwarded = 400;
					primarySponsor = "The Lady of Graves";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] The Veil of Madness: The Horrors of Wintersun Unmasked!</b></color>");
					list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with cold, holy wrath: \"Brainwashed thralls carving their own flesh while praising a false goddess of wood... Jerribeth's perverse distortion of mortal will is an abomination. Let the cold truth shatter the illusion.\"");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> bows her head in profound sorrow: \"To think these brave Sarkorians believed they were living in an untainted golden age, while serving demons as blind cattle... A tragedy that wrenches the heart. Grant them peace, Commander.\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sneers in disgust: \"Sloppy, juvenile demon trickery. Clouding minds with crude hallucinations rather than binding them with ironclad pacts. When the illusion falls, the entire construct collapses into ash.\"");
				}
				else
				{
					coinsAwarded = 400;
					primarySponsor = "The Laughing King";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] The Village of Screaming Smiles: Wintersun Shattered!</b></color>");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> rubs his chin: \"Oof. Even for a First World prankster like me, that was dark! Feeding people demon flesh while making them think it's honeyed bread? Jerribeth has zero chill!\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> grits his teeth: \"Break her jaw for that! Tricking honest hunters into becoming monsters is the lowest, filthiest crime in the Abyss! Tear down that false shrine!\"");
					list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars: \"LET THEM WAKE TO REALITY WITH WEAPONS DRAWN! DIE AS HUMANS OR DIE AS DEMON SLAVES! CRUSH THE WITCH!\"");
				}
			}
			else
			{
				coinsAwarded = 500;
				primarySponsor = "The All-Seeing Eye";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: Jerribeth's Illusion Dismantled!</b></color>");
				list.Add("<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> crackles: \"The mental lattice of Jerribeth's spell dissolved in record time! The Commander bypassed every psychological trap with casual ease!\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> laughs: \"Jerribeth tried her grand reveal monologue and you just hit her with 'Yeah, I know, hand over the loot.' Legendary!\"");
			}
			AppendSponsorshipCourtship(list, playerDeity, hasAvatar);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct3IvorySanctumBanter(int cycle, string playerDeity, bool hasAvatar, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			int num = _rng.Next(2);
			if (cycle <= 1)
			{
				if (num == 0)
				{
					coinsAwarded = 450;
					primarySponsor = "The All-Seeing Eye";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Swarm of the Defiler: Xanthir Vang in the Ivory Sanctum!</b></color>");
					list.Add("<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> crackles with chaotic arcane energy: \"A MORTAL CONSCIOUSNESS DISSOLVED INTO A MILLION CARNIVOROUS LOCUSTS! THE HORRIFYING BEAUTY OF CORRUPTED METAMAGIC! TEAR APART HIS SPELL-WEAVE, COMMANDER!\"");
					list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> slams his gauntlets: \"HE IS NOT A WARRIOR, HE IS A BODY OF ROACHES! SMASH HIM WITH FIRE AND BLUDGEONING STEEL! CRUSH EVERY SINGLE WING UNTIL ONLY PASTE REMAINS!\"");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> draws her holy blade: \"The architect of the tainted mythic dregs... Xanthir Vang sacrificed his humanity to become a hive of vermin. Purge this sanctum with righteous fire!\"");
				}
				else
				{
					coinsAwarded = 450;
					primarySponsor = "The Lucky Drunk";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Bug Extermination Protocol: Xanthir Vang Confrontation!</b></color>");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> makes a face of deep revulsion: \"Uuuugh, not another bug guy! First the canyon queen, now a wizard who literally IS bugs! Where is the giant rolled-up newspaper of Heaven when you need it?!\"");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> snickers: \"'I am Xanthir Vang, the Swarm that Walks!' Yeah, yeah, very scary, now eat a fireball like a good swarm! 9 out of 10 for gross villain aesthetic!\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> notes with disdain: \"A wretched creature that abandoned intellectual dignity to crawl in the dirt. Eliminate him and retrieve Areelu's notes.\"");
				}
			}
			else
			{
				coinsAwarded = 550;
				primarySponsor = "Our Lord in Iron";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: The Bug Wizard Squashed Again!</b></color>");
				list.Add(string.Format("{0} roars: \"HAH! XANTHIR BARELY FINISHED HIS SPEECH BEFORE BEING FLATTENED INTO GOO! CYCLE {1} EXTERMINATION COMPLETE!\"", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color>", cycle));
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> falls over laughing: \"He thought he had you trapped in the laboratory! Poor guy never realizes you know his entire spellbook by heart!\"");
			}
			AppendSponsorshipCourtship(list, playerDeity, hasAvatar);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct4FleshmarketBanter(int cycle, string playerDeity, bool hasAvatar, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			int num = _rng.Next(2);
			if (cycle <= 1)
			{
				if (num == 0)
				{
					coinsAwarded = 500;
					primarySponsor = "The Lucky Drunk";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Breaking the Auction Block: Slaver Bloodbath in Alushinyrra!</b></color>");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> slams his tankard down so hard the gallery floor cracks: \"SLAVERS! IN THE ABYSS! SHATTER EVERY CHAIN! SLIT EVERY SLAVE-MASTER'S THROAT! ALL MY COINS TO THE COMMANDER FOR PAINTING THE MARKET RED! FREE THEM ALL!\"");
					list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> glimmers with fierce, blazing fury: \"Aasimar, mortals, angels, bound in profane collars... No soul was born to be cattle for demons! Let starlight sever their bonds and guide them to the portal!\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sips calmly: \"Unregulated, chaotic flesh-peddling by incompetent fiends. Dyunk and Sarzaksys lacked both legal charter and refined cruelty. Slaying them provides a most satisfying market correction.\"");
				}
				else
				{
					coinsAwarded = 500;
					primarySponsor = "The Savored Sting";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] The Blood Market Cleanse: Slaying the Slaver Kingpins!</b></color>");
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs with ecstatic venom: \"Oh, look at the panic on Dyunk's slimy face when the auction block became his execution stand! Blood for tears, darling! Vengeance tastes sweetest when served in the heart of the Abyss!\"");
					list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> bellows: \"THE ENTIRE MARKET SQUARE ATTACKS AT ONCE! GUARDS, SLAVERS, ABYSSAL BEASTS! THIS IS A TRUE SLAUGHTERHOUSE! LET NONE ESCAPE!\"");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> leaps from his seat: \"The Knight-Commander walked into the biggest demon city in the multiverse and started a riot in the town square! Absolute madman behavior! Prime-time cinema!\"");
				}
			}
			else
			{
				coinsAwarded = 600;
				primarySponsor = "The Lucky Drunk";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: The Fleshmarket Wipeout!</b></color>");
				list.Add(string.Format("{0} roars with joy: \"Cycle {1} Fleshmarket demolition! Dyunk didn't even get to raise his bid before catching a holy critical hit to the teeth! Round of drinks for everyone!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color>", cycle));
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles: \"The slavers see you walking into the square and their survival instincts scream 'RUN!' Beautiful!\"");
			}
			AppendSponsorshipCourtship(list, playerDeity, hasAvatar);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct4NocticulaAudienceBanter(int cycle, string playerDeity, bool hasAvatar, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			int num = _rng.Next(2);
			if (cycle <= 1)
			{
				if (num == 0)
				{
					coinsAwarded = 500;
					primarySponsor = "The Savored Sting";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Audience with the Lady in Shadow: Nocticula's Palace!</b></color>");
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> narrows her eyes with keen, venomous rivalry: \"The Lady of Shadows lounged upon her velvet throne, dripping with honeyed poison. Be careful, darling; she aims to seduce and swallow your mythic spark. But what a magnificent creature she is!\"");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with solemn warning: \"She murdered her own brothers and built an empire of blood and lust. Whatever aid she offers against Baphomet and Deskari is poisoned bait. Keep your heart shielded.\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> observes with deep, intellectual satisfaction: \"Notice her calculations. Nocticula is weary of the Abyss. She seeks ascension, and our mortal otherworlder is the catalyst she requires. A mutual transaction of immense geopolitical weight.\"");
				}
				else
				{
					coinsAwarded = 500;
					primarySponsor = "The Laughing King";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] The Succubus Queen's Gambit: In the Silken Shadows!</b></color>");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> fans himself dramatically: \"Whew! Is it hot in here or is that just the Lady in Shadow purring into the Commander's ear?! The tension in that throne room could cut diamonds!\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles: \"She offers you midnight wine, poisoned knives, and half the Abyss on a silver platter. Just remember who brought you to the party, kid! Keep your boots on!\"");
					list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> whispers softly: \"Even deep within the shadow of Midnight Isles, a faint glimmer of redemption seeks the stars... Let her see the beauty of the light.\"");
				}
			}
			else
			{
				coinsAwarded = 600;
				primarySponsor = "The Savored Sting";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: The Queen's Seduction Rebuffed!</b></color>");
				list.Add(string.Format("{0} laughs softly: \"Nocticula tries her velvet whisper routine and you look right through her like an open book! She has no idea you've navigated her court {1} times before!\"", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color>", cycle));
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods: \"Diplomatic supremacy. You hold every card in the Midnight Isles.\"");
			}
			AppendSponsorshipCourtship(list, playerDeity, hasAvatar);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct5IzGalfreyFateBanter(int cycle, string playerDeity, bool hasAvatar, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			int num = _rng.Next(2);
			if (cycle <= 1)
			{
				if (num == 0)
				{
					coinsAwarded = 500;
					primarySponsor = "The Inheritor";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] The Crucible of Iz: The Queen, the Banner, and the Dragon!</b></color>");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with heavy tears of golden light: \"The ruins of Iz... where heroes bleed and tragic choices demand blood. Galfrey fighting with her bare hands, Irabeth holding the line, the Sword of Valor surrounded... Knight-Commander, Heaven watches your every step! Save who you can!\"");
					list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> watches the unraveling threads: \"Mortality at its most agonizing crossroads. A commander cannot stand in three places at once. Every choice writes an irreversible epitaph in the River.\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> notes with cold tactical precision: \"Command triage. Sacrificing the library for the queen, or the banner for the archives... Sentimentality is a fatal luxury. Choose what secures total victory.\"");
				}
				else
				{
					coinsAwarded = 500;
					primarySponsor = "The Lucky Drunk";
					list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] The Ash of Sarkoris: Racing Across the Ruins of Iz!</b></color>");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> grips his tankard tightly: \"Run, Commander! Run like the wind! Deskari's locusts are swarming the temple! Save the Queen and Irabeth! Don't let them die in that cursed ash!\"");
					list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars: \"LET STEEL CLASH WITH LOCUST HORNS! RESCUE THEM BY CLEAVING DESKARI'S SERVANTS IN TWAIN! NO RETREAT!\"");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> holds his breath: \"The whole celestial gallery is on their feet! The clock is ticking down to the millisecond! Now THIS is a high-stakes finale!\"");
				}
			}
			else
			{
				coinsAwarded = 650;
				primarySponsor = "The Key and the Gate";
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: The Triad Salvation of Iz!</b></color>");
				list.Add(string.Format("{0} pulses: \"Optimal causal routing executed. In cycle #{1}, all three objectives converge without casualty. The deterministic tragedy is dismantled by veteran foresight.\"", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color>", cycle));
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> cheers: \"Saved the Queen, saved Irabeth, saved the banner, AND grabbed the Glass Key! Speedrun perfection! The audience is throwing coins from the rafters!\"");
			}
			AppendSponsorshipCourtship(list, playerDeity, hasAvatar);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetAct6ThresholdEveCampBanter(int cycle, string playerDeity, bool hasAvatar, out int coinsAwarded, out string primarySponsor)
		{
			coinsAwarded = 800;
			primarySponsor = "The Grand Arbiter";
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			if (cycle <= 1)
			{
				list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] The Final Campfire: Eve of the Worldwound Breach!</b></color>");
				list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> showers glittering stardust over the tents: \"The final quiet evening beneath the wounded sky... Companions laughing, sharpening weapons, holding hands, and remembering how far they have traveled together. May the starlight hold you safe until dawn.\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises a brimming horn in silence: \"No jokes tonight. Just a solemn toast to every mortal who bled on this march. Drink deep with your friends, Commander. Tomorrow, we break the world.\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> salutes with quiet reverence: \"The crusade's final eve. Whatever happens beyond the threshold, your fellowship has redeemed Golarion. Walk forward without fear.\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> bows his head in rare, genuine respect: \"You brought an impossible, ragtag company from the caves of Kenabres to the heart of the Worldwound. A supreme feat of sovereign leadership.\"");
			}
			else
			{
				list.Add($"<color=#F5C542><b>[CONSTELLATION BROADCAST] Temporal Cycle {cycle}: The Sovereign's Final Eve!</b></color>");
				list.Add(string.Format("{0} sighs with warm reverence: \"Cycle {1} farewells... Look at how they look at you. Even across forgotten timelines, their loyalty shines brighter than the stars. Give them the ending they deserve, hero.\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color>", cycle));
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> resonates across dimensions: \"The loop stands at the precipice of resolution. 16,384 iterations converge upon the final doorway. The unwritten dawn draws near.\"");
			}
			AppendSponsorshipCourtship(list, playerDeity, hasAvatar);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetCompanionDialogueBanter(string companionKey, string answerText, int cycle, string playerDeity, bool hasAvatar, out int coinsAwarded, out string primarySponsor)
		{
			coinsAwarded = 75;
			primarySponsor = "The Constellations";
			List<string> list = new List<string>();
			string text = (companionKey ?? string.Empty).ToLower();
			int num = _rng.Next(2);
			list.Add("<color=#F5C542><b>[CONSTELLATION CHAT] Companion Resonance: Dialogue Exchange</b></color>");
			if (text.Contains("seelah"))
			{
				primarySponsor = "The Inheritor";
				if (num == 0)
				{
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> smiles warmly: \"Seelah's honesty and grounded spirit remain the soul of the vanguard. She fights not for cold glory, but for her friends.\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises a mug: \"She's always good for a laugh and an honest drink! Keep her close, Commander!\"");
				}
				else
				{
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> notes quietly: \"A paladin who values personal conscience over rigid dogma. A dangerous flaw in an ordinary soldier, yet surprisingly resilient in a commander's retinue.\"");
					list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> glimmers: \"Her heart is open and loyal. True friendship is the finest shield against the abyss.\"");
				}
			}
			else if (text.Contains("camellia") || text.Contains("camelia"))
			{
				primarySponsor = "The Savored Sting";
				if (num == 0)
				{
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs with delicious amusement: \"Oh, look at her licking her lips and pretending to adjust her necklace! She is so delightfully wicked, darling!\"");
					list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with icy detachment: \"The ledger of bodies around this girl grows ever heavier. She dances on the lip of the grave.\"");
				}
				else
				{
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> giggles: \"'I am helpful, is that not enough?' PWAHAHA! Her excuses are thinner than parchment! 10 out of 10 for chaotic drama!\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sneers: \"A petulant, undisciplined murderer pretending to serve spirits. A liability if she loses control of her appetite.\"");
				}
			}
			else if (text.Contains("lann"))
			{
				primarySponsor = "Our Lord in Iron";
				if (num == 0)
				{
					list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> grunts with fierce approval: \"A MONGREL WHO LAUGHS IN THE FACE OF DEATH! HE DRAWS HIS BOW WITH HARDENED GRIT! A REAL WARRIOR!\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles: \"His self-deprecating humor is top shelf. If you don't laugh at death, it'll eat you alive!\"");
				}
				else
				{
					list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> sighs gently: \"Thirty years is so painfully short for a soul with so much light... Cherish his loyalty beneath the open sky.\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> notes: \"Pragmatic humility. A subordinate who executes orders without vanity is exceptionally valuable.\"");
				}
			}
			else if (text.Contains("wenduag"))
			{
				primarySponsor = "The Prince of Darkness";
				if (num == 0)
				{
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods with calculated satisfaction: \"Survival of the fittest. She bows only to absolute strength and authority. Master her leash, and she is a ruthless hunting hound.\"");
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> smirks: \"She bites when she feels weak, and kneels when she feels overwhelmed. A dangerous, intoxicating creature!\"");
				}
				else
				{
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> cautions: \"Her soul has been poisoned by demonic cruelty from birth. Only unwavering moral conviction can guide her out of the dark.\"");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> snickers: \"Careful not to turn your back on her in a dark alley! She's got poison daggers under every sleeve!\"");
				}
			}
			else if (text.Contains("woljif"))
			{
				primarySponsor = "The Pirate Queen";
				if (num == 0)
				{
					list.Add("<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> laughs heartily: \"Woljif's mind is always calculating the escape route and the coin split! That's my boy!\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles: \"He's a good kid at heart, just terrified of ending up alone. Keep him in the party and buy him some meat pies!\"");
				}
				else
				{
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> observes: \"His shadow whispers ancient abyssal secrets. A boy sitting on an empire of planar power without knowing how to spend it.\"");
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> winks: \"Tricky, fast, and full of excuses. You two make a delightfully roguish pair!\"");
				}
			}
			else if (text.Contains("daeran"))
			{
				primarySponsor = "The Savored Sting";
				if (num == 0)
				{
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs happily: \"Every single word out of Daeran's mouth is dipped in pure, vintage venom! Oh, I adore him! Insult them again, darling!\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> shakes his head: \"He's so sour he could curdle milk, but his healing magic has saved our necks a dozen times. Cheers to the arrogant count!\"");
				}
				else
				{
					list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> pulses quietly: \"The void entity behind Daeran's skull tracks the conversation. The mortal teeters between madness and sovereignty.\"");
					list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> observes: \"A heavy fate hangs over his lineage. Savor the laughter while the candle burns.\"");
				}
			}
			else if (text.Contains("ember"))
			{
				primarySponsor = "The Song of the Spheres";
				if (num == 0)
				{
					list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> weeps with gentle light: \"Ember's quiet warmth is the purest miracle in the Worldwound. Every word she speaks softens hardened hearts.\"");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> whispers reverently: \"Her innocent fire carries more grace than a thousand sermons. Guard her with your life, Commander.\"");
				}
				else
				{
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> murmurs thoughtfully: \"She looks at demon lords and sees only unhappy children in need of a hot meal. Terrifyingly profound innocence.\"");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> claps softly: \"Ember is the real final boss of this crusade. Even Deskari would probably start crying if she patted his head!\"");
				}
			}
			else if (text.Contains("nenio"))
			{
				primarySponsor = "The All-Seeing Eye";
				if (num == 0)
				{
					list.Add("<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> roars with arcane delight: \"THE OBSESSIVE PURSUIT OF SCIENTIFIC ENCYCLOPEDIC DATA! WRITE DOWN EVERY EXPERIMENT! QUESTION REALITY ITSELF!\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> clutches his temples: \"Barkeep, two aspirins. If she asks me how many bubbles are in a pint of ale one more time, my brain is going to melt.\"");
				}
				else
				{
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> snickers: \"Nenio literally forgot what species she was because it wasn't relevant to her notes! Peak academic eccentricity!\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sighs: \"Her methodical documentation is undeniably rigorous, even if her social etiquette is non-existent.\"");
				}
			}
			else if (text.Contains("sosiel"))
			{
				primarySponsor = "The Eternal Rose";
				if (num == 0)
				{
					list.Add("<color=#FF69B4><b>[The Constellation 'The Eternal Rose']</b></color> hums gently: \"Sosiel paints beauty into the ash. His gentle heart endures the horror of war with admirable faith.\"");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> nods: \"A true cleric who stands firm between the wounded and the grave. His loyalty is steadfast.\"");
				}
				else
				{
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> smirks: \"Oh, but look at that hidden temper boiling beneath the cleric's robes! Scratch him deep enough and the painter becomes a wild beast! Delicious!\"");
					list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> grunts: \"HE HITS HARDER WITH THAT GLAIVE THAN HE ADMITS! LET THE ARTIST SMASH DEMONS!\"");
				}
			}
			else if (text.Contains("regill"))
			{
				primarySponsor = "The Prince of Darkness";
				if (num == 0)
				{
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods approvingly: \"Regill's counsel is pure axiomatic logic. Listen to his operational doctrine and the crusade will never falter.\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> groans: \"The guy would court-martial a flower for blooming without official permission! But he's a damn good tactician.\"");
				}
				else
				{
					list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> slams his fists: \"A LITTLE GNOME OF SOLID STEEL! HE SHUTTERS NOT BEFORE ANY FOE! RESPECT THE GODCLAW HAMMER!\"");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> snickers: \"Regill probably sleeps at attention in full plate armor. The most serious creature on the Great Wheel!\"");
				}
			}
			else if (text.Contains("greybor"))
			{
				primarySponsor = "The Prince of Darkness";
				if (num == 0)
				{
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> reviews his ledger: \"Professional contract ethics. Greybor honors his retainer with ruthless precision. A true mercenary.\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> winks: \"He likes his dwarven stout and his pipe smoke. Pay his fee and he'll chop a dragon in half for you!\"");
				}
				else
				{
					list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> grunts: \"DWARVEN DUAL-AXES! CHOPPING DOWN DRAGONS FROM BEHIND! EFFECTIVE, EVEN IF SNEAKY!\"");
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs: \"A killer who takes pride in his craft. Always keep a bag of gold handy, darling.\"");
				}
			}
			else if (text.Contains("arueshalae") || text.Contains("arue"))
			{
				primarySponsor = "The Song of the Spheres";
				if (num == 0)
				{
					list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles warmly: \"Every word you share with her helps her wings unfurl toward the starlight. She is finding her true self beside you.\"");
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> chuckles: \"Sweet, blushing, and holding an arrow of pure light. What an adorable little paradox you're nurturing!\"");
				}
				else
				{
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> observes: \"Her discipline in resisting abyssal hunger is remarkable. An unprecedented psychological case study.\"");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> smiles: \"Redemption is the highest victory of all. Guide her with honor, Commander.\"");
				}
			}
			else if (text.Contains("finnean"))
			{
				primarySponsor = "The Laughing King";
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> roars with laughter: \"A talking soul blade with amnesia and an ego! Finnean is the star comedian of the party! Pwahaha!\"");
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> grunts with respect: \"A WEAPON THAT REMEMBERS HOW TO CUT! I LIKE A BLADE THAT SPEAKS ITS MIND IN BATTLE!\"");
			}
			else
			{
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises a mug: \"Good comrades make the march bearable! Keep talking with your friends, kid!\"");
				list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles: \"Every quiet conversation weaves an unbreakable bond across the stars.\"");
			}
			AppendSponsorshipCourtship(list, playerDeity, hasAvatar);
			return list;
		}

		public static List<string> GetNpcDialogueBanter(string npcKey, string answerText, int cycle, string playerDeity, bool hasAvatar, out int coinsAwarded, out string primarySponsor)
		{
			coinsAwarded = 100;
			primarySponsor = "The Constellations";
			List<string> list = new List<string>();
			string text = (npcKey ?? string.Empty).ToLower();
			int num = _rng.Next(2);
			list.Add("<color=#F5C542><b>[CONSTELLATION CHAT] Planar Observers: Major NPC Exchange</b></color>");
			if (text.Contains("galfrey"))
			{
				primarySponsor = "The Inheritor";
				if (num == 0)
				{
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with deep compassion: \"Queen Galfrey has borne the crushing weight of Mendev for over a century. Her youthful flesh is an elixir's gift, but her spirit bleeds with weary sacrifice. Treat her with dignity, Commander.\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises a glass: \"A century of fighting demons while everyone expects you to be a saint! That's a rough gig. The Queen deserves a stiff drink and an honest ally.\"");
				}
				else
				{
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> evaluates: \"A monarch clinging to a crumbling frontline through magical longevity. The crusade needs an unyielding sovereign, not a martyr searching for an honorable death. Keep your authority firm.\"");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles: \"The royal burden looks very heavy on those pretty shoulders! But with our otherworlder leading the vanguard, her retirement might finally be in sight!\"");
				}
			}
			else if (text.Contains("irabeth"))
			{
				primarySponsor = "The Inheritor";
				if (num == 0)
				{
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with motherly warmth: \"Irabeth Tirabash broke Deskari's initial charge with her bare shield. Even when doubt gnaws at her soul, her courage is the bedrock of the Eagle Watch.\"");
					list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles: \"Her love with Anevia is a soft candle in the storm. May their bond give her strength.\"");
				}
				else
				{
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> notes: \"A capable frontline captain whose self-confidence was shattered by Kenabres's fall. A commander must know how to restore broken steel before battle.\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> nods: \"She's tough as old boots. Just don't let her carry the guilt of the whole city alone!\"");
				}
			}
			else if (text.Contains("anevia"))
			{
				primarySponsor = "The Savored Sting";
				if (num == 0)
				{
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs approvingly: \"Anevia's scouts see every shadow and hear every whisper before the cultists even open their mouths! A brilliant spymaster, darling!\"");
					list.Add("<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> winks: \"Sharp eyes, steady hands, and zero patience for whining. That's a first mate any captain would kill for!\"");
				}
				else
				{
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods: \"An effective intelligence officer. The lifeblood of any successful military campaign.\"");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> giggles: \"Anevia always looks at the Commander like she's wondering what ridiculous otherworldly stunt they're about to pull next!\"");
				}
			}
			else if (text.Contains("storyteller"))
			{
				primarySponsor = "The All-Seeing Eye";
				if (num == 0)
				{
					list.Add("<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> hums with profound reverence: \"THE CHRONICLER OF AGES! A BLIND ELF WHOSE MIND EMBRACES MILLENNIA OF FORGOTTEN ARCANA AND DEICIDAL TRAGEDY! UNCOVER HIS MEMORIES!\"");
					list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> nods slowly: \"The Storyteller walks between history and oblivion. Every artifact you place in his hands restores a lost thread to the tapestry of Golarion.\"");
				}
				else
				{
					list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> pulses: \"Entity Storyteller represents an ancient causal focal point. His restored recollections intersect with the origins of the Worldwound.\"");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> snickers: \"He talks so slowly you could take a three-day nap between sentences, but boy does he have the best gossip from four thousand years ago!\"");
				}
			}
			else if (text.Contains("horgus"))
			{
				primarySponsor = "Master of the First Vault";
				if (num == 0)
				{
					list.Add("<color=#DAA520><b>[The Constellation 'Master of the First Vault']</b></color> notes with satisfaction: \"Horgus Gwerm understands that gold fuels the gears of survival. An abrasive personality, but his treasury stabilizes Kenabres's logistics.\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smirks: \"A ruthless bourgeois merchant hiding lethal family secrets behind velvet doublets. Amusingly predictable.\"");
				}
				else
				{
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> groans: \"The guy complained about mud on his boots while demons were eating people in the streets! Good thing you kept him alive for his money!\"");
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> giggles: \"Oh, he's so delightfully snooty! Watching him forced to rely on common vagabonds was absolute poetry!\"");
				}
			}
			else if (text.Contains("minagho"))
			{
				primarySponsor = "The Laughing King";
				if (num == 0)
				{
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> claps his hands: \"Minagho! The demoness who went from 'Supreme Conqueror of Kenabres' to 'Fired Employee of the Month'! The look of terror in her red eyes is priceless!\"");
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> smiles venomously: \"Look at her squirming, darling! She broke Staunton, she slaughtered the garrison, and now she is begging for scraps. Savor her humiliation!\"");
				}
				else
				{
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> scoffs: \"Incompetent chaotic failure. Entrusted with a strategic stronghold, yet undone by arrogance and panic. Slay the wretch and purge the fane.\"");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks sternly: \"Her cruelties brought unspeakable suffering to Kenabres. The sword of justice now answers her crimes.\"");
				}
			}
			else if (text.Contains("staunton"))
			{
				primarySponsor = "The Lady of Graves";
				if (num == 0)
				{
					list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with cold solemnity: \"Seventy years of shame, torment, and betrayals. Staunton Vhane's thread was severed the moment he handed the Sword of Valor to Minagho. Grant the broken dwarf his final rest.\"");
					list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> grunts: \"HE FOUGHT WITH THE FURY OF A DYING BULL! MISGUIDED, TREASONOUS, BUT HE SWUNG HIS HAMMER TO THE END! A WARRIOR'S DEATH!\"");
				}
				else
				{
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> notes: \"Treason born of vanity and manipulated gullibility. His fate is the eternal warning against succumbing to demonic promises.\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> sighs heavily: \"Damn... Seventy years of being despised by your own people, only to die as a demon's discarded toy. What a miserable waste of a life.\"");
				}
			}
			else if (text.Contains("herald") || text.Contains("hand of the inheritor"))
			{
				primarySponsor = "The Inheritor";
				if (num == 0)
				{
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> smiles with radiant pride: \"My Herald descends to walk beside the Commander! A pure celestial beacon of truth, courage, and divine honor. Stand proud together, champions of Heaven!\"");
					list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> glimmers warmly: \"His wings bring hope to the darkest depths of the Abyss. Keep his noble heart safe, Commander.\"");
				}
				else
				{
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> observes: \"An archon of supreme virtue. Yet pure idealism often fractures when confronted with the grotesque moral ambiguities of reality. Guard him against despair.\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises a toast: \"The Hand is a magnificent ally! He doesn't drink, but he hits like a falling star! Glad to have him on our side!\"");
				}
			}
			else if (text.Contains("areelu"))
			{
				primarySponsor = "The Key and the Gate";
				if (num == 0)
				{
					list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> unfolds kaleidoscopic corridors: \"The Architect of the Worldwound observes the unscripted soul. The causal threads she tore apart to retrieve her lost child have bound an entity beyond her calculations.\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smiles with deep appreciation: \"A mortal witch who outmaneuvered demon lords, tricked archangels, and tore a rift between planes out of grief and hatred. A magnificent adversary.\"");
				}
				else
				{
					list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with eternal frost: \"Her crimes tore the tapestry of reality and flooded the River of Souls with corruption. Her reckoning approaches.\"");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles: \"Areelu wanted a docile clone of her child and ended up summoning a planar powerhouse who flips the entire board! You can't write comedy this good!\"");
				}
			}
			else
			{
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles: \"Another dramatic confrontation on the cosmic stage! The gallery is glued to their seats!\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> winks: \"Keep your wits sharp and your blade ready, Commander!\"");
			}
			AppendSponsorshipCourtship(list, playerDeity, hasAvatar);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetDlcDialogueBanter(string dlcKey, string answerText, int cycle, string playerDeity, bool hasAvatar, out int coinsAwarded, out string primarySponsor)
		{
			coinsAwarded = 350;
			primarySponsor = "The Constellations";
			List<string> list = new List<string>();
			string text = (dlcKey ?? string.Empty).ToLower();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			if (text.Contains("dlc1") || text.Contains("inevitable") || text.Contains("valmallos"))
			{
				primarySponsor = "The Key and the Gate";
				list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Axiomatic Paradox: Inevitable Excess!</b></color>");
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> unfolds radiant, multi-dimensional lattices: \"The mechanical axis of Axis trembles. Valmallos calculates the anomaly of the otherworlder, perceiving that your presence violates the deterministic equilibrium of the multiverse.\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> observes with deep intellectual pleasure: \"Axiomatic law confronting infinite chaotic potential. Valmallos seeks to sanitize the paradox, yet the sovereign mind cannot be reduced to a mechanical formula.\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> falls over laughing: \"The giant cosmic clockwork brain is having a blue-screen meltdown trying to categorize you! Peak entertainment!\"");
			}
			else if (text.Contains("dlc2") || text.Contains("ashes") || text.Contains("survivors"))
			{
				primarySponsor = "The Lucky Drunk";
				list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Mortals in the Ruins: Through the Ashes!</b></color>");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises his flagon in profound respect: \"No mythic powers, no legendary cheat relics--just common folks with wooden cudgels and bandages clinging to life in the burning streets. Now THAT is raw, unyielding courage!\"");
				list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> turns an ancient page: \"Fragile mortal threads defying the locust's scythe. Every heartbeat bought with sweat and terror is sacred.\"");
				list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> shines softly: \"May the northern breeze guide the refugees through the smoke toward dawn.\"");
			}
			else if (text.Contains("dlc3") || text.Contains("isles") || text.Contains("steersman"))
			{
				primarySponsor = "The Pirate Queen";
				list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Voyage of the Damned: The Midnight Isles!</b></color>");
				list.Add("<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> roars with salty joy: \"SAILING THE ABYSSAL SEAS WITH A MAD SKELETON STEERSMAN! PLUNDERING NAHYNDRIAN CRYSTALS FROM DROWNED GODS! NOW THIS IS REAL PIRACY! ALL HANDS ON DECK!\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> laughs into his drink: \"Navigating an archipelago of dead demon lord corpses! You couldn't make this up if you tried! Drink up, captain!\"");
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> calculates: \"The abyssal tides echo with the memory of shattered deities. An infinite labyrinth of drowning dreams.\"");
			}
			else if (text.Contains("dlc4") || text.Contains("sarkorians") || text.Contains("ulbrig"))
			{
				primarySponsor = "The Shimmering Maiden";
				list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] The Last Shifter: The Legacy of Sarkoris!</b></color>");
				list.Add("<color=#AFEEEE><b>[The Constellation 'The Shimmering Maiden']</b></color> weeps with starry warmth: \"Ulbrig Olesk... a living son of fallen Sarkoris, waking from stone into a world of ashes. Cleanse the ancient spirits, starlit champion!\"");
				list.Add("<color=#8FBC8F><b>[The Constellation 'Old Deadeye']</b></color> nods grimly: \"The boy remembers the old hearths and the wild hunts before the sky cracked. Walk with him; restore what the demons stole.\"");
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> bellows: \"GRIFFON WINGS AND PRIMAL CLAWS! HE DOES NOT FLINCH BEFORE DEMONS! ROAR FOR SARKORIS!\"");
			}
			else if (text.Contains("dlc5") || text.Contains("nothing") || text.Contains("sithhud"))
			{
				primarySponsor = "The Lady of Graves";
				list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] The Primordial Frost: The Lord of Nothing!</b></color>");
				list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with absolute frost: \"Sithhud... an ancient horror of the frozen void stirring once more beneath the ice. Extinguish the unhallowed cold and seal his bones forever.\"");
				list.Add("<color=#4B0082><b>[The Constellation 'The Midnight Lord']</b></color> whispers from shadow: \"The beautiful, numb agony of absolute zero... The silence where even screams freeze in the throat.\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> raises her golden standard: \"Bring Heaven's holy fire into the dark! No frozen lord shall claim this realm!\"");
			}
			else if (text.Contains("dlc6") || text.Contains("masks") || text.Contains("festival") || text.Contains("arena"))
			{
				primarySponsor = "The Laughing King";
				list.Add("<color=#F5C542><b>[CONSTELLATION BROADCAST] Festival of Masks & The Gladiator Arena!</b></color>");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> leaps in the air: \"A FESTIVAL IN REBUILT KENABRES! DRINKING, MASKS, PARADES, AND RAZMIRAN IMPOSTORS GETTING EXPOSED ON STAGE! AND THEN THE ARCH-MAGE ARENA! PEAK PRIME-TIME TELEVISION!\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> roars with laughter: \"Free drinks in the streets! Masquerade games! And beating up snobby gladiator champions in the arena! This is the greatest vacation the Fifth Crusade has ever taken!\"");
				list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs delightfully: \"Masquerades are where the real fun begins, darling... Look at all those delicious secrets hiding behind silk masks!\"");
			}
			AppendSponsorshipCourtship(list, playerDeity, hasAvatar);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return list;
		}

		public static List<string> GetContinuousDialogueBanter(string text, string dialogName, string areaName, int cycle, string playerDeity, bool hasAvatar, out int coinsAwarded, out string primarySponsor)
		{
			coinsAwarded = 25;
			primarySponsor = "The Constellations";
			List<string> list = new List<string>();
			string text2 = (text ?? string.Empty).ToLower();
			(dialogName ?? string.Empty).ToLower();
			int num = _rng.Next(2);
			if (text2.Contains("kill") || text2.Contains("attack") || text2.Contains("die") || text2.Contains("blood") || text2.Contains("strike"))
			{
				primarySponsor = "Our Lord in Iron";
				if (num == 0)
				{
					list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars with martial bloodlust: \"YES! NO HESITATION! STEEL CLEAVES BONE! PROVE YOUR MIGHT IN BLOOD!\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> notes calmly: \"Decisive lethal force. Eliminate opposition before it can formulate a counter-strategy.\"");
				}
				else
				{
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> whistles: \"Straight to business! That's one way to resolve a disagreement!\"");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> cautions: \"Channel your wrath with honor, Commander. We fight to protect, not merely to destroy.\"");
				}
			}
			else if (text2.Contains("magic") || text2.Contains("spell") || text2.Contains("knowledge") || text2.Contains("secret") || text2.Contains("ancient"))
			{
				primarySponsor = "The All-Seeing Eye";
				if (num == 0)
				{
					list.Add("<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> crackles with arcane curiosity: \"Knowledge is the only absolute currency! Unravel the mysteries of the weave!\"");
					list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> pulses: \"The causal lattice vibrates with latent understanding. Secrets converge upon the seeker.\"");
				}
				else
				{
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smiles: \"Information is the supreme leverage. Those who master knowledge dictate terms to empires.\"");
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> winks: \"Don't read too much fine print, darling, you might ruin the surprise twist!\"");
				}
			}
			else if (text2.Contains("love") || text2.Contains("heart") || text2.Contains("kiss") || text2.Contains("stay") || text2.Contains("care"))
			{
				primarySponsor = "The Savored Sting";
				if (num == 0)
				{
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs with delicious delight: \"Oh, how delightfully tender! Sparks of romance blooming in the middle of a demonic apocalypse! Keep wooing them, darling!\"");
					list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles with starry warmth: \"May genuine affection guide your hearts through the cold night.\"");
				}
				else
				{
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> winks: \"Now that's what I call charm! Nothing brings people together like surviving demon sieges together!\"");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks gently: \"True love is an armor that darkness cannot pierce.\"");
				}
			}
			else if (text2.Contains("order") || text2.Contains("law") || text2.Contains("command") || text2.Contains("duty") || text2.Contains("obey"))
			{
				primarySponsor = "The Prince of Darkness";
				if (num == 0)
				{
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods with deep approval: \"Order is the bedrock of civilization. Without discipline and hierarchy, all endeavors collapse into ruin.\"");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> salutes: \"A commander must hold the line with steadfast resolve. Duty guides our standard.\"");
				}
				else
				{
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> groans into his ale: \"Too stiff, too rigid! But I guess somebody has to keep the army in a straight line!\"");
					list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> grunts: \"DISCIPLINE WINS BATTLES! MARCH FORWARD!\"");
				}
			}
			else if (text2.Contains("trick") || text2.Contains("laugh") || text2.Contains("joke") || text2.Contains("fool") || text2.Contains("gold"))
			{
				primarySponsor = "The Laughing King";
				if (num == 0)
				{
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles into his sleeve: \"A sharp tongue and a mischievous smile! The best way to win is to make the villain look ridiculous!\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> roars with laughter: \"Hah! That's the spirit! A good laugh is worth ten potions of heroism!\"");
				}
				else
				{
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> giggles: \"Play with your prey, darling! A little mockery makes the sting that much sweeter!\"");
					list.Add("<color=#32CD32><b>[The Constellation 'The Mischievous Friend']</b></color> hops cheerfully: \"Bold and clever! Fortune loves a daring smile!\"");
				}
			}
			else
			{
				primarySponsor = "The Constellations";
				if (num == 0)
				{
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> observes: \"A measured response. Our protagonist plays the room with veteran poise!\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> nods: \"Keep moving, Commander. Every choice leads to the next adventure!\"");
				}
				else
				{
					list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles: \"Every step beneath the open sky shapes the destiny of Golarion.\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> marks his ledger: \"Pragmatic dialogue navigation. The board continues to unfold.\"");
				}
			}
			AppendSponsorshipCourtship(list, playerDeity, hasAvatar);
			return list;
		}

		internal static void AppendSponsorshipCourtship(List<string> lines, string playerDeity, bool hasAvatar, string primarySponsor = null)
		{
			if (lines == null)
			{
				return;
			}
			string text = playerDeity ?? string.Empty;
			bool flag = IsCore13(text);
			if (hasAvatar)
			{
				lines.Add("<color=#F5C542><b>[Sovereign Covenant]</b></color> <i>[The Constellations observe with hushed reverence: The mortal vessel embodies the living Avatar of " + text + "!]</i>");
				if (text.ToLower().Contains("cayden"))
				{
					lines.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> roars with supreme sponsor pride: \"THAT'S MY AVATAR ON GOLARION! LOOK AT THAT FORM! BARKEEP, PUT THE ENTIRE MULTIVERSE ON MY TAB!\"");
				}
				else if (text.ToLower().Contains("iomedae"))
				{
					lines.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> blazes with golden majesty: \"My living standard walks the mortal earth! Let Heaven's glory strike down every shadow!\"");
				}
				else if (text.ToLower().Contains("asmodeus"))
				{
					lines.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> raises his ruby scepter: \"The covenant is supreme. My Avatar commands the board with absolute infernal authority.\"");
				}
				else
				{
					lines.Add("<color=#F5C542><b>[Your Sworn Patron]</b></color> radiates blinding cosmic favor upon their divine Avatar!");
				}
			}
			else if (!string.IsNullOrEmpty(text) & flag)
			{
				lines.Add("<color=#F5C542><b>[Patron Sponsorship: " + text + "]</b></color> <i>[Your sworn patron watches with intense personal satisfaction! (+25% coin donation active)]</i>");
				int num = _rng.Next(2);
				if (text.ToLower().Contains("cayden"))
				{
					lines.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> winks: \"That's my chosen champion! Keep raising hell and drinking deep, partner!\"");
					if (num == 0)
					{
						lines.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sneers: \"A regrettable waste of sovereign intellect on a drunken deity. My contract terms remain available in the Cosmic Exchange should you desire true discipline.\"");
					}
					else
					{
						lines.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> giggles: \"Oh, Cayden, must you be so loud? Though I admit, darling, that was delightfully impudent!\"");
					}
				}
				else if (text.ToLower().Contains("iomedae"))
				{
					lines.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> salutes: \"My chosen champion fights with unbroken honor! Heaven's standard shines upon you!\"");
					if (num == 0)
					{
						lines.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles: \"Look at the Inheritor boasting! But hey, you made a fine play! Cheers!\"");
					}
					else
					{
						lines.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> observes: \"A lawful crusader. Stiff, yet undeniably effective.\"");
					}
				}
				else if (text.ToLower().Contains("asmodeus"))
				{
					lines.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smiles with dark approval: \"My sworn agent executes policy with ruthless clarity. Infernal dividends are disbursed.\"");
					if (num == 0)
					{
						lines.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> groans: \"Don't listen to the horned devil, kid! Remember freedom is still the best drink in town!\"");
					}
					else
					{
						lines.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> sighs: \"Even in the dark, may your heart find its way back to the starlight.\"");
					}
				}
				else
				{
					lines.Add("<color=#F5C542><b>[Your Sworn Patron]</b></color> celebrates your triumph from the upper gallery!");
				}
			}
			else if (!string.IsNullOrEmpty(text) && !text.ToLower().Contains("atheism") && !flag)
			{
				lines.Add("<color=#F5C542><b>[Guest Patron Sponsorship: " + text + "]</b></color> <i>[Your sworn patron watches from the VIP Guest Gallery with intense pride! (+25% coin donation active)]</i>");
				string text2 = text.ToLower();
				int num2 = _rng.Next(2);
				if (text2.Contains("torag"))
				{
					lines.Add("<color=#D2691E><b>[The Constellation 'Father of Creation']</b></color> strikes his anvil with deep approval: \"Well struck! Unyielding iron and stubborn resolve--that's how true victories are forged!\"");
					if (num2 == 0)
					{
						lines.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises a flagon toward the guest booth: \"Barkeep, send another cask of dark ale to the dwarven smith! His champion knows how to fight!\"");
					}
					else
					{
						lines.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> inclines his head: \"Solid masonry. The craftsman's discipline serves the crusader well.\"");
					}
				}
				else if (text2.Contains("sarenrae") || text2.Contains("dawnflower"))
				{
					lines.Add("<color=#FFD700><b>[The Constellation 'The Dawnflower']</b></color> shines with blinding dawn warmth: \"Let righteous scimitars cleanse the shadow! Heaven's dawn walks at your side, my champion!\"");
					if (num2 == 0)
					{
						lines.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> salutes with golden radiance: \"Lady Sarenrae's champion fights with glorious conviction! Stand strong!\"");
					}
					else
					{
						lines.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> covers her eyes: \"Goodness, turn down the lantern, darling! Though I will admit, you sliced that fiend quite elegantly.\"");
					}
				}
				else if (text2.Contains("abadar"))
				{
					lines.Add("<color=#DAA520><b>[The Constellation 'Master of the First Vault']</b></color> tallies his ledger with golden ink: \"Order maintained. The risk-adjusted returns on your crusader enterprise remain exceedingly profitable.\"");
					if (num2 == 0)
					{
						lines.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smiles faintly: \"Sound fiscal administration. Chaos is bad for commerce.\"");
					}
					else
					{
						lines.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> laughs: \"Don't listen to the accountant, Commander! Spend those coins on the finest tavern in Drezen!\"");
					}
				}
				else if (text2.Contains("shelyn"))
				{
					lines.Add("<color=#FF69B4><b>[The Constellation 'The Eternal Rose']</b></color> strums a song of exquisite grace: \"A dance of courage amidst the ash... You turn the brutal frontline into a masterpiece of hope!\"");
					if (num2 == 0)
					{
						lines.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles softly: \"Beauty and wonder never perish so long as brave hearts protect them.\"");
					}
					else
					{
						lines.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> winks: \"Passion and steel... quite an alluring performance, sweet rose!\"");
					}
				}
				else if (text2.Contains("erastil"))
				{
					lines.Add("<color=#8FBC8F><b>[The Constellation 'Old Deadeye']</b></color> notches an arrow with steady calm: \"Good honest work. Defend the hearth, watch your flanks, and never forget where you came from.\"");
					if (num2 == 0)
					{
						lines.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles: \"Old Deadeye actually gave a compliment! Drink up, everyone, history has been made!\"");
					}
					else
					{
						lines.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> snickers: \"'Watch your flanks!' The rustic grandfather has spoken!\"");
					}
				}
				else if (text2.Contains("irori"))
				{
					lines.Add("<color=#4682B4><b>[The Constellation 'The Master of Masters']</b></color> sits poised above shimmering ether: \"Discipline executed without flaw. With every strike, your soul purges weakness and advances toward mastery.\"");
					if (num2 == 0)
					{
						lines.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> snorts: \"LESS MEDITATION, MORE SMASHING! BUT GOOD HITS REGARDLESS!\"");
					}
					else
					{
						lines.Add("<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> crackles with runes: \"Internal perfection harmonizing with external reality... an extraordinary specimen!\"");
					}
				}
				else if (text2.Contains("gozreh"))
				{
					lines.Add("<color=#2E8B57><b>[The Constellation 'The Wind and the Waves']</b></color> rumbles with ocean thunder: \"THE TEMPEST DROWNS THE FIENDS! STRIKE LIKE THE LIGHTNING GALE!\"");
					if (num2 == 0)
					{
						lines.Add("<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> laughs: \"Fair winds and a blood-red sea! Ride the storm, captain!\"");
					}
					else
					{
						lines.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> wipes sea spray from his eyes: \"Who turned on the indoor monsoon?! Great strike though!\"");
					}
				}
				else if (text2.Contains("zon-kuthon") || text2.Contains("zonkuthon"))
				{
					lines.Add("<color=#4B0082><b>[The Constellation 'The Midnight Lord']</b></color> whispers through shadow chains: \"Every scar tells a story of exquisite agony. Teach them the bliss of suffering.\"");
					if (num2 == 0)
					{
						lines.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with cold finality: \"Pain ends on the Scales. Keep your shadows distant, Kuthonite.\"");
					}
					else
					{
						lines.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sneers: \"A theatrical display of morbidity, though undeniably ruthless.\"");
					}
				}
				else if (text2.Contains("pulura"))
				{
					lines.Add("<color=#AFEEEE><b>[The Constellation 'The Shimmering Maiden']</b></color> shines with aurora light: \"The stars of lost Sarkoris gleam in your eyes. Cleanse our home, starlight child!\"");
					if (num2 == 0)
					{
						lines.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> embraces her warmly: \"The aurora dances once more over the Worldwound!\"");
					}
					else
					{
						lines.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> claps: \"Now that's what I call lighting up the battlefield!\"");
					}
				}
				else if (text2.Contains("godclaw"))
				{
					lines.Add("<color=#708090><b>[The Constellation 'The Fivefold Order']</b></color> resonates in unison: \"Discipline. Law. Unflinching purpose. The Fivefold Order approves your frontline execution.\"");
					if (num2 == 0)
					{
						lines.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods with quiet satisfaction: \"Flawless adherence to doctrine.\"");
					}
					else
					{
						lines.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> grunts: \"STIFF, BUT EFFECTIVE IN BATTLE!\"");
					}
				}
				else if (text2.Contains("rovagug"))
				{
					lines.Add("<color=#DC143C><b>[The Rough Beast]</b></color> roars from the bottom of the Dead Vault: \"CRUSH! TEAR! ANNIHILATE ALL CREATION!\"");
					lines.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> narrows his eyes: \"Reinforce the planetary wards immediately. The beast's roar rattles the vault bars.\"");
					lines.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> gazes down with icy vigilance: \"The Worldwound is danger enough without waking the Devourer.\"");
				}
				else
				{
					string guestTag = GetGuestTag(text);
					lines.Add(guestTag + " speaks with divine authority: \"Stand firm, my chosen champion! The outer heavens applaud your courage!\"");
					lines.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles from the upper balcony: \"Look at the guest star getting all enthusiastic in the VIP booth! Good show!\"");
				}
			}
			else
			{
				lines.Add("<color=#F5C542><b>[Cosmic Sponsorship Courtship]</b></color> <i>[The Constellations are bickering in the channel over your potential sponsorship contract!]</i>");
				switch (_rng.Next(3))
				{
				case 0:
					lines.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> reaches forward with holy light: \"Unbonded mortal, your valor is extraordinary! Align your soul with the Inheritor in the Cosmic Exchange and accept Heaven's righteous mantle!\"");
					lines.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> slides an infernal parchment forward: \"Do not be swayed by righteous sermons. Sign with Hell, and command reality through supreme law and unyielding dominion.\"");
					break;
				case 1:
					lines.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> slams a brimming tankard: \"Why chain yourself to boring contracts?! Choose the Lucky Drunk as your sponsor! Freedom, good luck, and free drinks across the Great Beyond!\"");
					lines.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs with a seductive wink: \"Or choose sweet vengeance with the Savored Sting, darling! Why follow rules when we could have so much more fun together?\"");
					break;
				default:
					lines.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars: \"AN OATH WRITTEN IN BLOOD AND CRUSHED BONES! CHOOSE OUR LORD IN IRON AND OBLITERATE EVERY FOE IN YOUR PATH!\"");
					lines.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles gently: \"Walk under the open sky of starlight. The Song of the Spheres offers freedom, wonder, and endless horizons.\"");
					break;
				}
				lines.Add("<i>[Reminder: You can formalize your sponsorship contract anytime in the Cosmic Constellation Exchange dialog or via the Cosmic Coin.]</i>");
			}
		}

		private static bool IsCore13(string deityName)
		{
			if (string.IsNullOrEmpty(deityName))
			{
				return false;
			}
			string text = deityName.ToLower();
			if (!text.Contains("cayden") && !text.Contains("iomedae") && !text.Contains("asmodeus") && !text.Contains("desna") && !text.Contains("pharasma") && !text.Contains("calistria") && !text.Contains("nethys") && !text.Contains("gorum") && !text.Contains("besmara") && !text.Contains("lantern") && !text.Contains("chaldira") && !text.Contains("urgathoa"))
			{
				return text.Contains("butterfly");
			}
			return true;
		}

		public static BanterResult GetSceneBanter_BlackwingLibrary(in DialogueContext ctx)
		{
			int coins = 180;
			string text = "The All-Seeing Eye";
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			list.Add("<color=#F5C542><b>[CELESTIAL AUDIENCE] Blackwing Library: The Storyteller's Fire</b></color>");
			if (ctx.Cycle <= 1)
			{
				if (_rng.Next(2) == 0)
				{
					list.Add("<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> crackles with sudden fury: \"Fools! Burning irreplaceable knowledge to feed a minor demon's vanity! The Storyteller's memories alone contain secrets of lost ages before the Worldwound was torn! Slay Chaleb!\"");
					list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> smirks with sharp malice: \"Chaleb thought his little inquisitor disguise was so very clever. Teach him that mocking holy offices carries a painful penalty, darling.\"");
					list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> nods firmly: \"Rescue the surviving crusaders. No innocent shall burn in Kenabres while we draw breath!\"");
				}
				else
				{
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> snickers: \"Look at Chaleb trying to act holy! He's sweating through his stolen inquisitor tabard! I give his acting performance a two out of ten!\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises an eyebrow: \"Two? You're generous, Lantern. I've seen drunken goblins put on better theatricals. Put him out of his misery, traveler!\"");
				}
			}
			else
			{
				list.Add(string.Format("{0} glows with tranquil power: \"Iteration {1}. The blind elf's memories are already known to you, yet saving him remains an invariant virtue. Claim the hidden cache behind the illusory wall.\"", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color>", ctx.Cycle));
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> laughs: \"Chaleb doesn't even get to finish his opening speech on loop runs! The man is basically a free exp pinata!\"");
			}
			AppendSponsorshipCourtship(list, ctx.PlayerDeity, ctx.HasAvatar, text);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(list, coins, text, "Blackwing Library");
		}

		public static BanterResult GetSceneBanter_TowerOfEstrod(in DialogueContext ctx)
		{
			int coins = 180;
			string text = "The Prince of Darkness";
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			list.Add("<color=#F5C542><b>[CELESTIAL AUDIENCE] Tower of Estrod: Preemptive Purge</b></color>");
			if (ctx.Cycle <= 1)
			{
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> rests his chin upon steepled fingers: \"Faxon's little coven believes their ambush at the tavern will catch Kenabres unawares. Instead, the prey has hunted the predators in their own den. A pristine tactical strike.\"");
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars with bloodlust: \"SHATTER THE CULT CELL! DRENCH THE WINE CELLAR IN TRAITOR BLOOD! LET THEM CHOKE ON THEIR FALSE OATHS!\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> winks: \"And don't forget to loot the cellar afterwards! Even cultists occasionally keep decent vintages on the top shelf!\"");
			}
			else
			{
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> giggles: \"Faxon was literally mid-rehearsal for his grand assault speech when you kicked the door off its hinges! The looper efficiency is truly terrifying!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smirks: \"Calculated and merciless. As all preemptive operations should be.\"");
			}
			AppendSponsorshipCourtship(list, ctx.PlayerDeity, ctx.HasAvatar, text);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(list, coins, text, "Tower of Estrod");
		}

		public static BanterResult GetSceneBanter_TopazSolutions(in DialogueContext ctx)
		{
			int coins = 150;
			string text = "The Pirate Queen";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Topaz Solutions: The Poison Lair</b></color>", "<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> scoffs loudly: \"Looters hiding behind poisonous gas and crude alchemical tripwires? Desperate rats scuttling over trinkets while the city burns! Take their coin, traveler; pirates respect only strength!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> shakes his head: \"Trying to rob people during a demonic apocalypse is low even for thieflings. Teach them that crime without honor just gets you buried early.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Topaz Solutions");
		}

		public static BanterResult GetSceneBanter_DesnaTemple(in DialogueContext ctx)
		{
			int coins = 200;
			string text = "The Song of the Spheres";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Temple of Desna: The Secret Song</b></color>", "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> radiates serene, starlit warmth: \"Aranka... Thall... my sweet songbirds hiding in the shadows of fanatical inquisitors. Thank you, traveler, for sheltering them from Hulrun's blinded wrath. Sing the secret song beneath the starlight!\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with remorse: \"Hulrun's fear has turned his vigilance into madness. Desna's followers are allies in this war, not fiendish infiltrators. Justice must temper zeal.\"", "<color=#32CD32><b>[The Constellation 'The Mischievous Friend']</b></color> giggles: \"Nothing funnier than watching stiff-necked inquisitors get outmaneuvered by poets and musicians! Free spirits forever!\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Temple of Desna");
		}

		public static BanterResult GetSceneBanter_HorgusManor(in DialogueContext ctx)
		{
			int coins = 180;
			string text = "The Prince of Darkness";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Gwerm Manor: Family Secrets</b></color>", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> chuckles quietly: \"Old Horgus Gwerm. A man whose pride outstrips even his considerable fortune. He clings to his ledgers and bloodline secrets while demons tear down his masonry. A fascinating study in mortal hubris.\"", "<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> grins: \"Charge him double for the escort, Commander! Rich aristocrats always complain about the bill, but they pay when their necks are on the line!\"", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> whispers: \"And what a charming little daughter he has... secrets within secrets in that house, darling.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Gwerm Manor");
		}

		public static BanterResult GetSceneBanter_LostChapel(in DialogueContext ctx)
		{
			int coins = 250;
			string text = "The Inheritor";
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			list.Add("<color=#F5C542><b>[CELESTIAL AUDIENCE] The Lost Chapel: Reclaiming the Broken</b></color>");
			if (ctx.Cycle <= 1)
			{
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with deep compassion: \"Irabeth... to see so noble a paladin broken by fear and despair tears at my heart. Remind her of who she is, Commander. She took the oath; Heaven has not forsaken her!\"");
				list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> intones: \"The ghouls desecrate the sacred mountain. Cleanse the slopes, traveler. The judgment of the dead belongs to the Boneyard, not fiendish scavengers.\"");
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> snarls: \"STAND UP, HALF-ORC! A WARRIOR DOES NOT WEEP IN THE SNOW! GRIP YOUR SHIELD AND CRUSH THE GHOULS!\"");
			}
			else
			{
				list.Add(string.Format("{0} nods with mock solemnity: \"Cycle {1}! The classic 'Irabeth has an existential breakdown on a freezing rock' episode. Every looper knows this script by heart. Hand her a cup of warm tea and let's get back to smashing ghouls!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color>", ctx.Cycle));
			}
			AppendSponsorshipCourtship(list, ctx.PlayerDeity, ctx.HasAvatar, text);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(list, coins, text, "The Lost Chapel");
		}

		public static BanterResult GetSceneBanter_ZachariusCrypt(in DialogueContext ctx)
		{
			int coins = 250;
			string text = "The Lady of Graves";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Secret Crypt of Zacharius</b></color>", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> radiates chilling divine wrath: \"Zacharius. A crusader hero who fled the river of souls into the wretched rotting cage of lichdom. To bargain with such an abomination is an affront to cosmic balance!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> strokes his chin: \"A contract sealed in ancient bone. The lich kept his vow to defend Drezen in his own twisted manner. Power is power, mortal; whether you crush his wand or accept his dark tutelage, command the board.\"", "<color=#8B0000><b>[The Constellation 'The Pallid Princess']</b></color> laughs with decadent delight: \"Oho! The sweet chill of necromancy! Drink the grave's essence, darling! Why let mortality limit your eternal crusade?\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Crypt of Zacharius");
		}

		public static BanterResult GetSceneBanter_ConundrumUnsolved(in DialogueContext ctx)
		{
			int coins = 180;
			string text = "The All-Seeing Eye";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Conundrum Unsolved: Ancient Puzzles</b></color>", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> hums with arcane geometry: \"The ancient stone slabs... ley-line harmonics carved by the mask-makers of lost millennia! Align the glyphs, traveler! The language of magic is pure logic!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> yawns loudly: \"Puzzles? Tile puzzles?! Who put sliding puzzles into a high-stakes demonic war story?! If you get stuck, Commander, just blast the door with a fireball and let's keep moving!\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Conundrum Unsolved");
		}

		public static BanterResult GetSceneBanter_NurahTraitor(in DialogueContext ctx)
		{
			int coins = 220;
			string text = "The Prince of Darkness";
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			list.Add("<color=#F5C542><b>[CELESTIAL AUDIENCE] Nurah's Treachery Unmasked</b></color>");
			if (ctx.Cycle <= 1)
			{
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smiles thinly: \"A halfling slave whose soul was forged in Chelish chains. She traded loyalty to the crusade for vengeance against all who ever stood above her. A predictable, almost tragic trajectory. What will you do with her, Commander?\"");
				list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> murmurs with keen approval: \"She burned the camp and smiled while doing it. A delicious thirst for retribution... though turning to demons is such a crude, messy way to settle old scores, darling.\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> laments: \"Treachery born of mortal cruelty. If Mendev had shown more mercy to the bound, perhaps this poison would never have bloomed.\"");
			}
			else
			{
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> cackles: \"Nurah didn't even get to uncork her little alchemical fire flask! The looper caught her red-handed before she could even whistle for the gargoyles! Classic!\"");
			}
			AppendSponsorshipCourtship(list, ctx.PlayerDeity, ctx.HasAvatar, text);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(list, coins, text, "Nurah's Treason");
		}

		public static BanterResult GetSceneBanter_HellknightRedoubt(in DialogueContext ctx)
		{
			int coins = 220;
			string text = "The Prince of Darkness";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Reliable Redoubt Hellknight Alliance</b></color>", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods in solemn appreciation: \"Order stands against chaos. The Hellknights of the Godclaw do not bend to panic or sentimentality. In an army of weeping novices, Regill's legion is the only true wall of iron. Value this alliance.\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> winces: \"Yeah, they're stiff, joyless, and they'd probably execute me for laughing on duty. But by the Cayden cup, those black-armored bastards know how to hold a canyon against flying monsters!\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Hellknight Redoubt");
		}

		public static BanterResult GetSceneBanter_DragonHunt(in DialogueContext ctx)
		{
			int coins = 300;
			string text = "Our Lord in Iron";
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			list.Add("<color=#F5C542><b>[CELESTIAL AUDIENCE] The Dragon Hunt with Greybor</b></color>");
			if (ctx.Cycle <= 1)
			{
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars with ferocious approval: \"A BLOOD DRAGON! SCALES HARDER THAN STEEL, BREATH HOTTER THAN A CRUCIBLE! A WORTHY QUARRY! CUT THE BEAST'S WINGS FROM ITS SPINE!\"");
				list.Add("<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> laughs heartily: \"The dwarf knows his business. Quiet, methodical, and charges a hefty bag of coin up front. That's a professional! Bring down the drake and take its hoard!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> observes: \"A mercenary contract fulfilled to the letter. Greybor's code is narrow, but unbreakable. Honor the terms, Commander.\"");
			}
			else
			{
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> giggles: \"Greybor spent three weeks tracking that dragon across the rocky crags, and the looper already knew which rock it was sleeping on! The poor dwarf's professional pride is hanging by a thread!\"");
			}
			AppendSponsorshipCourtship(list, ctx.PlayerDeity, ctx.HasAvatar, text);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(list, coins, text, "The Dragon Hunt");
		}

		public static BanterResult GetSceneBanter_Blackwater(in DialogueContext ctx)
		{
			int coins = 300;
			string text = "The All-Seeing Eye";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Horrors of Blackwater</b></color>", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> crackles with crackling planar fury: \"Chief Khara and the Hundred-Face! Blasphemous cyborg augmentation! They severed the soul's natural conduit to weave lightning and synthetic gears into demonic sinew! The machine mind must be extinguished!\"", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> rotates in silent geometry: \"Foreign technology from the Numerian crash. Circuits that do not belong to the primal tapestry of Golarion. Dismantle the core and release the trapped minds.\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> shivers: \"Getting your brain wired to a metal box? That's nightmare fuel right there. Smash that machine to scrap!\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Blackwater Complex");
		}

		public static BanterResult GetSceneBanter_MoltenScar(in DialogueContext ctx)
		{
			int coins = 250;
			string text = "The Savored Sting";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Molten Scar Infiltration</b></color>", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> narrows her eyes with icy fury: \"Vorimeraak and her vrock cultists. Mutilating captured crusaders for sport and feeding on their despair. Let none of them leave the ash pit alive, darling. Revenge is a dish best served scorching hot.\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with solemn grief: \"Our captured brothers and sisters... enduring torment in the sulfur fumes. Break their chains and bring them home to Drezen.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Molten Scar");
		}

		public static BanterResult GetSceneBanter_MidnightFane(in DialogueContext ctx)
		{
			int coins = 400;
			string text = "The Inheritor";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Midnight Fane: The Gate to the Abyss</b></color>", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> raises her golden standard: \"The final bastion beneath Drezen! Minagho and Darrazand make their desperate stand at the precipice of the rift! The Hand of the Inheritor fights at your side! Forward, crusaders of Golarion!\"", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> bellows: \"THE DEMON GENERAL FALLS TODAY! NO RETREAT TO THE ABYSS! CARVE HIM DOWN TO THE BONE!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> claps excitedly: \"The climax of Act 3! Act 4 takes the whole production straight into the Abyss! Break down the barricades!\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Midnight Fane");
		}

		public static BanterResult GetSceneBanter_TempleOfDelamere(in DialogueContext ctx)
		{
			int coins = 250;
			string text = "The Lady of Graves";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Desecrated Temple of Delamere</b></color>", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with cold, sacred authority: \"Delamere. The ancient priestess of Erastil who gave her life to seal the borders. Now, fiendish necromancers disturb her slumber to raise her as an undead weapon. Put her spirit to eternal peace.\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> bows her head: \"A martyr of the first crusades. Her resting place shall not be defiled by fiends.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Temple of Delamere");
		}

		public static BanterResult GetSceneBanter_BattleblissArena(in DialogueContext ctx)
		{
			int coins = 350;
			string text = "Our Lord in Iron";
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			list.Add("<color=#F5C542><b>[CELESTIAL AUDIENCE] Battlebliss: The Gladiatorial Pit</b></color>");
			if (ctx.Cycle <= 1)
			{
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> slams his fists onto the balcony railing: \"GELDERFANG! THE CHAMPION OF THE ABYSS! PROVE THAT A MORTAL HERO CAN CRUSH A PIT MONSTER BENEATH THOUSANDS OF CHEERING FIENDS! SHED THEIR BLOOD!\"");
				list.Add("<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> laughs with unrestrained glee: \"Look at Irmangaleth counting his bets in the box! The audience came to watch the otherworlder die, and instead they're getting a masterclass in slaughter!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smirks: \"Demons understand only dominance. To conquer the arena is to conquer the street.\"");
			}
			else
			{
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> snickers: \"Gelderfang didn't even finish his arena roar before the looper delivered a flying roundhouse! The bookies in the lower ring just went bankrupt!\"");
			}
			AppendSponsorshipCourtship(list, ctx.PlayerDeity, ctx.HasAvatar, text);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(list, coins, text, "Battlebliss Arena");
		}

		public static BanterResult GetSceneBanter_TenThousandDelights(in DialogueContext ctx)
		{
			int coins = 300;
			string text = "The Savored Sting";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Ten Thousand Delights: Poisoned Silk</b></color>", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs with delicious amusement: \"Chivarro and Herrax... two vipers circling one another in a den of painted velvet. In the Abyss, darling, love is merely a weapon, and desire is a poisoned needle. Tread lightly; everyone in this house is selling someone else's soul.\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> rubs the back of his neck: \"Even the ale here smells like powdered black lotus. Keep your weapon in reach and your head clear, Commander.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Ten Thousand Delights");
		}

		public static BanterResult GetSceneBanter_VellexiaDates(in DialogueContext ctx)
		{
			int coins = 300;
			string text = "The Laughing King";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Vellexia's Theatrical Courtship</b></color>", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> holds his sides laughing: \"A demonic noblewoman hosting tea parties in the Upper City, testing her mortal suitors like performing dogs! Her arrogance is a work of art! Play along, traveler; watching her pride crack when you don't bow is the finest comedy in the Abyss!\"", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> narrows her eyes: \"She thinks herself a goddess of drama. Show her what true passion and retribution feel like when her little theater crumbles.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Vellexia's Court");
		}

		public static BanterResult GetSceneBanter_ColyphyrHepzamirah(in DialogueContext ctx)
		{
			int coins = 400;
			string text = "The Key and the Gate";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Colyphyr Mines: Hepzamirah Cornered</b></color>", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> revolves with gravitational solemnity: \"Hepzamirah stands at the altar of Nahyndrian crystals. Baphomet's daughter believes she summons salvation. She does not know she calls forth her own executioner. The causal convergence approaches.\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with resolute authority: \"The mines are breached! Strike down the demon lord's brood before the gate opens!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> watches with cold fascination: \"A loyal daughter discarded on the sacrificial pyre of her father's ambition. A classic demonic tragedy.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Colyphyr Mines");
		}

		public static BanterResult GetSceneBanter_Alderpash(in DialogueContext ctx)
		{
			int coins = 450;
			string text = "The All-Seeing Eye";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Runelord Alderpash in the Labyrinth</b></color>", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> sparks with arcane awe: \"Alderpash! The ancient Thassilonian Runelord of Wrath, chained in Baphomet's labyrinth for ten thousand years! His mind still burns with the forgotten sorceries of the first empire! Bargain with caution; his knowledge is priceless, but his rage is boundless!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> evaluates with cold scrutiny: \"A defeated ruler reduced to begging for scraps from a demon lord. A cautionary tale in pride without prudence. Extract his lore and leave him to his chains.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Alderpash's Cell");
		}

		public static BanterResult GetSceneBanter_HandSalvation(in DialogueContext ctx)
		{
			int coins = 500;
			string text = "The Inheritor";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Hand of the Inheritor's Torment</b></color>", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with heartbroken fury: \"My Hand... my most faithful herald. Baphomet tore out his holy heart to break his divine light. Free him from this agony, Commander! He believed in you when even Heaven doubted!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> slams his tankard onto the floor: \"Tearing out an angel's heart for kicks? Baphomet is a dead goat walking. Put him down for good!\"", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> nods solemnly: \"The herald's light will not be snuffed out in a demon's dungeon. Restore his honor.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "The Hand's Salvation");
		}

		public static BanterResult GetSceneBanter_PuluraFall(in DialogueContext ctx)
		{
			int coins = 350;
			string text = "The Song of the Spheres";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Pulura's Fall: Stargazers Under Siege</b></color>", "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> watches with anxious tenderness: \"The peaceful stargazers of Pulura... observing the infinite skies while Mutasafen brings his poisonous vials to extinguish their light. Protect the sanctuary! Let the celestial maps remain unbroken!\"", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> hums: \"The cosmic observatory contains star charts dating back to the Age of Creation. Do not permit the alchemist to burn the celestial records!\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Pulura's Fall");
		}

		public static BanterResult GetSceneBanter_KhorramzadehSiege(in DialogueContext ctx)
		{
			int coins = 450;
			string text = "Our Lord in Iron";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Khorramzadeh's Assault on Drezen</b></color>", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> bellows until the heavens rattle: \"THE BALOR GENERAL OF THE WORLDWOUND! STORM KING KHORRAMZADEH HAMMERS THE GATES! THIS IS THE BATTLE TO END ALL BATTLES! HOLD THE WALLS OR DIE IN ASH!\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> unsheathes her holy blade: \"The walls of Drezen shall not fall again! Stand alongside the crusaders; drive the demon legions back into the abyss!\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Khorramzadeh's Siege");
		}

		public static BanterResult GetSceneBanter_DawnOfDragons(in DialogueContext ctx)
		{
			int coins = 350;
			string text = "The Dawnflower";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Dawn of Dragons: Redemption & Steel</b></color>", "<color=#FFD700><b>[The Constellation 'The Dawnflower']</b></color> shines with holy radiance: \"Hal and the golden drakes preach mercy even to the most fallen souls. It takes greater courage to offer forgiveness than to sever a neck. Hear the golden dragon's wisdom.\"", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> snorts with contempt: \"MERCY FOR DEMONS?! A FIEND IS BORN FROM CORRUPTED CHAOS AND DIES IN BLOOD! YOU CANNOT REFORM A HELLHOUND WITH SWEET WORDS! SMASH THEM!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smirks: \"Mercy without law is merely weakness waiting to be exploited. But an unexpected truce can be a potent tactical gambit.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Dawn of Dragons");
		}

		public static BanterResult GetSceneBanter_ThresholdCamp(in DialogueContext ctx)
		{
			int coins = 600;
			string text = "The Inheritor";
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			list.Add("<color=#F5C542><b>[CELESTIAL AUDIENCE] The Eve of Threshold: The Final Campfire</b></color>");
			if (ctx.Cycle <= 1)
			{
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with solemn majesty: \"The final campfire before the gates of Threshold. In a hundred years of crusade, no mortal commander has assembled a company so valiant. Whatever lies beyond those obsidian gates, Heaven honors your march.\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises a brimming horn of spiced mead: \"Every mug in the taverns of Golarion is poured in your honor tonight. Win or lose, you gave the abyss a beating it'll feel for ten centuries. Drink deep, friends--tomorrow we write legend!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> watches the campfires flicker with cold appraisal: \"The board narrows to the final moves. All the pieces sacrificed across five acts were merely preludes to this breach. Do not falter when the final price is demanded.\"");
			}
			else
			{
				list.Add(string.Format("{0} laughs with triumphant theatricality: \"Iteration {1}! I remember when your company froze to death on these slopes in cycle four, and when you charged in solo without armor in cycle nine! But tonight, the stage is set for a masterstroke!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color>", ctx.Cycle));
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> pulses in non-Euclidean radiance: \"The causal threads converge upon the singular needle of Threshold. The memories of countless timelines whisper in the smoke of your fire.\"");
			}
			AppendSponsorshipCourtship(list, ctx.PlayerDeity, ctx.HasAvatar, text);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(list, coins, text, "Threshold Campfire");
		}

		public static BanterResult GetSceneBanter_EchoOfDeskari(in DialogueContext ctx)
		{
			int coins = 650;
			string text = "Our Lord in Iron";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Threshold Breach: Echo of Deskari</b></color>", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars with earth-shattering fury: \"THE GATES SHAKE! THE LORD OF THE LOCUST SWARM SENDS HIS SHADOW TO BAR THE WAY! CLEAVE HIS CARAPACE IN TWAIN AND CRUSH HIS CHITIN INTO THE DUST!\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> blazes with golden fire: \"Deskari's reign of terror began in Kenabres with Terendelev's fall. Let his echo break here at the gates of his undoing! For Mendev and the fallen!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles heartily: \"Deskari thought he could put an echo on gate duty while he sulked in the Rasping Rifts. Show the bug that a copy is never as tough as the original!\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Echo of Deskari");
		}

		public static BanterResult GetSceneBanter_AreeluConfrontation(in DialogueContext ctx)
		{
			int coins = 750;
			string text = "The All-Seeing Eye";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Heart of Threshold: Areelu Vorlesh</b></color>", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> crackles with dual cosmic energies: \"Areelu Vorlesh! The architect of the wound, the mortal who dared fuse the primal abyss with mortal flesh! Her equations were mad, her grief monstrous, yet her magical audacity reshaped the planes forever!\"", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> looks upon the scene with eternal weight: \"A mother whose refusal to accept death tore open reality. For a century, her defiance has denied countless souls their peace in the Boneyard. The ledger must now be closed.\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> tilts his head with subtle respect: \"Flawed and sentimental, yet remarkably effective in playing Heaven, Hell, and the Abyss against one another. Hear her confession, traveler--truth is the ultimate currency at the threshold.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Areelu's Confrontation");
		}

		public static BanterResult GetSceneBanter_WorldwoundHeart(in DialogueContext ctx)
		{
			int coins = 800;
			string text = "The Lady of Graves";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Worldwound Heart: The Final Choice</b></color>", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with quiet solemnity: \"The wound of Sarkoris bleeds its final drops. The choice between sacrifice, vengeance, and transcendence lies solely in mortal hands. Speak your will, traveler.\"", "<color=#FFD700><b>[The Constellation 'The Dawnflower']</b></color> radiates soothing dawn light: \"A century of darkness ends here. Whether through mercy or holy sacrifice, let the light of healing restore the scarred earth of Golarion.\"", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> pulses with transcendent geometry: \"The closed ring shudders. The causal singularity that anchored this reality reaches zero point. What was once destined to repeat now fractures into infinite freedom.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Worldwound Heart");
		}

		public static BanterResult GetSceneBanter_SecretAscensionEnding(in DialogueContext ctx)
		{
			int coins = 1000;
			string text = "The Key and the Gate";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[COSMIC CONSTELLATION ERUPTION] Secret Ascension: Breaking the Wheel</b></color>", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> manifests in blinding cosmic majesty, every dimensional eye open wide: \"THE RING IS SHATTERED! You have unraveled the Lexicon, crystallized the blood of demon lords, and ascended beyond the mortal cycle! A foreign spark claiming divine sovereignty across the Great Beyond!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> drops his scepter in sheer theatrical joy: \"INCREDIBLE! AN ABSOLUTE MASTERPIECE! Not only did you dodge the tragic sacrifice trope, you dragged Areelu into godhood with you! The entire upper gallery is throwing gold! Best finale in ten thousand cycles!\"", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> roars with unrestrained ecstasy: \"A NEW DEITY IS BORN! Neither born of Golarion nor bound by its ancient covenants! The Great Beyond shudders at your birth! Welcome to eternity, peer of the stars!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> stands and offers a slow, dignified clap: \"Audacious. Methodical. Flawless execution of planar law subversion. You have earned your throne among the stars, divinity.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Secret Ascension Ending");
		}

		public static BanterResult GetSceneBanter_PharasmaCourt(in DialogueContext ctx)
		{
			int coins = 700;
			string text = "The Lady of Graves";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Court of the Boneyard: Pharasma's Judgment</b></color>", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> sits upon the Spire of Souls, her gaze peering into the deepest mysteries: \"You stand before the final court. A soul plucked from beyond the cosmos, wandering through the crucible of the Worldwound. The tapestry of fate was rewritten by your hand.\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> testifies with heartfelt reverence: \"Lady Pharasma, this mortal stood between Golarion and oblivion. Heaven bears witness to their righteous courage.\"", "<color=#8B0000><b>[The Constellation 'The Pallid Princess']</b></color> sneers with wicked delight: \"Let the old crone scowl! You gave the Boneyard a run for its money, traveler! What is fate compared to the joy of defying death itself?!\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Pharasma's Court");
		}

		public static BanterResult GetSceneBanter_AreshkagalClimax(in DialogueContext ctx)
		{
			int coins = 600;
			string text = "The Song of the Spheres";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Enigma: Areshkagal & Nenio's Riddle</b></color>", "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> shines with fierce, radiant hope: \"Areshkagal preaches the void, whispering that memory is meaningless and nothing matters. But every dream, every friendship, and every stroke of Nenio's pen proves the sphinx wrong! Existence is beautiful!\"", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> sneers at the Faceless Sphinx: \"A demon lord terrified of meaning! She masks her emptiness behind convoluted pyramids and riddles. Tear the mask from the sphinx; show her that an inquiring mind cannot be dissolved into sand!\"", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> pulses with cosmic memory: \"To remember is to resist entropy. In every cycle, Nenio's notebook has preserved truth against the void. Slay the sphinx and write your answer into eternity.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Areshkagal's Climax");
		}

		public static BanterResult GetSceneBanter_InevitableDarkness(in DialogueContext ctx)
		{
			int coins = 650;
			string text = "The Key and the Gate";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Singularity: Inevitable Darkness</b></color>", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> vibrates with primal cosmic alarm: \"An ontological singularity! An echo of primordial nothingness collapsed within a dimensional rift! It seeks to unmake all attributes and collapse your mythic presence into null!\"", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> slams his gauntlets together: \"A MONSTER BORN OF PURE DESTRUCTION?! NO TALKING, NO COMPROMISE--ONLY STEEL AND FURY! CRUSH THE VOID BENEATH YOUR BOOTS!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> raises a brow: \"Even in the cosmic ledger, such anomalies are considered dangerous liabilities. Eradicate it and prove your superiority over raw entropic chaos.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Inevitable Darkness");
		}

		public static BanterResult GetAreaBanter_CavesUnderKenabres(int cycle, string deity, bool hasAvatar)
		{
			int coins = 100;
			string text = "The Laughing King";
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			list.Add("<color=#F5C542><b>[CELESTIAL RECOGNITION] Entering the Underground Descent</b></color>");
			if (cycle <= 1)
			{
				if (_rng.Next(2) == 0)
				{
					list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chortles softly from the upper balcony: \"Ah, the subterranean plunge! I've watched four thousand souls tumble down these exact crevices. Three thousand snapped their necks on the first stalactite, and the rest complained about damp boots. Let's see if this traveler has better balance!\"");
					list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> winces sympathetically: \"Ugh, subterranean grit and sulfur dust. Worst hangover remedy in the multiverse. Keep moving, kid--the tavern up top is burning, but at least there's wine somewhere.\"");
					list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> rests his chin upon steepled fingers: \"Another mortal dropped onto the starting square of the board. The mortality rate in these caverns is remarkably high. Place your wagers, deities.\"");
				}
				else
				{
					list.Add("<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> hums with crackling planar resonance: \"The causal barrier shattered right on schedule. Soul trajectory matches ninety-eight percent of prior anomalous transits into Kenabres bedrock. Recording vital metrics.\"");
					list.Add("<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> remains solemn: \"Countless souls have bled out in these darkened tunnels, their threads cut before the crusade even began. We shall see if this one walks a longer thread.\"");
				}
			}
			else
			{
				list.Add(string.Format("{0} roars with delight: \"Iteration {1}! The classic plummet! Tell me, did you try dodging the chasm this time or did you just accept your destiny like a seasoned veteran?\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color>", cycle));
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> pulses in crystalline geometry: \"The entry point remains invariant. The traveler re-enters the dark caverns beneath Kenabres, carrying echoes of past timelines.\"");
			}
			AppendSponsorshipCourtship(list, deity, hasAvatar, text);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(list, coins, text, "Caves Under Kenabres");
		}

		public static BanterResult GetAreaBanter_ShieldMaze(int cycle, string deity, bool hasAvatar)
		{
			int coins = 120;
			string text = "The Prince of Darkness";
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			list.Add("<color=#F5C542><b>[CELESTIAL RECOGNITION] Crossing into the Shield Maze</b></color>");
			if (cycle <= 1)
			{
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smirks with dry amusement: \"The Shield Maze. Hosilla has placed her sentries in the exact same corridors for eighty years. A dismal lack of tactical imagination, yet somehow crusader novices stumble into her pit traps every single time.\"");
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> sighs heavily: \"Young paladins, tricked by demonic whispers and twisted into fiendish beasts. We have witnessed this tragedy repeat across centuries of bloodshed.\"");
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> grunts with fierce anticipation: \"Traps are for cowards! Smash the doors, shatter the mongrel cultists, and take the labyrinth by force!\"");
			}
			else
			{
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> snickers: \"Speedrun alert! Five hundred coins says the looper sprints straight past the dining hall and kicks down Hosilla's door in under five minutes!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods approvingly: \"When one knows the layout of every hidden lever, even a labyrinth becomes a straight corridor.\"");
			}
			AppendSponsorshipCourtship(list, deity, hasAvatar, text);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(list, coins, text, "The Shield Maze");
		}

		public static BanterResult GetAreaBanter_MarketSquare(int cycle, string deity, bool hasAvatar)
		{
			int coins = 150;
			string text = "The Song of the Spheres";
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			list.Add("<color=#F5C542><b>[CELESTIAL RECOGNITION] Surveying the Ash of Market Square</b></color>");
			if (cycle <= 1)
			{
				list.Add("<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> weeps softly into the celestial ether: \"The festive kites, the children's laughter, the holy bells... every single time, reduced to ash and weeping in a matter of minutes. My heart breaks for Kenabres anew.\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> clutches his tankard tightly: \"I've watched this square burn eight hundred times from this balcony, Desna, and it never gets easier to stomach. But look at our traveler standing tall amidst the fire. They haven't folded yet!\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> tilts his head curiously: \"Usually this is where the panic truly sets in. Let's see how they handle Hulrun's paranoid inquisitors and the burning ruins!\"");
			}
			else
			{
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> revolves softly: \"Market Square locus. In thousands of past iterations, eighty-six percent of candidates were ambushed by the necromancer near the fountain. This traveler moves with foreknowledge.\"");
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> grins: \"No panic, just pure business. That's the looper swagger!\"");
			}
			AppendSponsorshipCourtship(list, deity, hasAvatar, text);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(list, coins, text, "Kenabres Market Square");
		}

		public static BanterResult GetAreaBanter_BlackwingLibrary(int cycle, string deity, bool hasAvatar)
		{
			int coins = 150;
			string text = "The All-Seeing Eye";
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			list.Add("<color=#F5C542><b>[CELESTIAL RECOGNITION] Entering Blackwing Library</b></color>");
			if (cycle <= 1)
			{
				list.Add("<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> crackles with sudden fury: \"The ash of ancient grimoires! Two hundred and ninety-eight crusaders charged through these doors blindly and let the flames consume the archives before striking down the cultists. Knowledge is sacred! Save the old blind teller!\"");
				list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> narrows her golden eyes: \"Chaleb thinks himself clever, posing as an inquisitor while tying scholars to burning stakes. A slow, agonizing sting would suit him delightfully.\"");
			}
			else
			{
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> snickers: \"Chaleb doesn't even know what's coming. He's rehearsing his fake crusader sermon right now, blissfully unaware that an all-knowing looper is about to wipe the floor with him!\"");
			}
			AppendSponsorshipCourtship(list, deity, hasAvatar, text);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(list, coins, text, "Blackwing Library");
		}

		public static BanterResult GetAreaBanter_TowerOfEstrod(int cycle, string deity, bool hasAvatar)
		{
			int coins = 150;
			string text = "The Savored Sting";
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			list.Add("<color=#F5C542><b>[CELESTIAL RECOGNITION] Infiltrating the Tower of Estrod</b></color>");
			if (cycle <= 1)
			{
				list.Add("<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> smiles with venomous pleasure: \"Ah, the cultists' little conspiratorial nest. In forty-seven past cycles, travelers arrived here twelve hours too late and found the tavern defense already compromised. Strike first, darling; never let the traitors finish their wine.\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> observes dispassionately: \"Faxon believes he serves a grand infernal master, but he is merely a disposable pawn on Baphomet's board. Teach him the cost of treason.\"");
			}
			else
			{
				list.Add("<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> barks: \"CRUSH THE CELLAR! DON'T EVEN LET THEM FINISH THEIR TOAST! STEEL BEFORE WORDS!\"");
			}
			AppendSponsorshipCourtship(list, deity, hasAvatar, text);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(list, coins, text, "Tower of Estrod");
		}

		public static BanterResult GetAreaBanter_DefendersHeart(int cycle, string deity, bool hasAvatar)
		{
			int coins = 160;
			string text = "The Lucky Drunk";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL RECOGNITION] Stepping into Defender's Heart Tavern</b></color>", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises an overflowing mug: \"Defender's Heart! The finest barricaded tavern north of the Sellen River! I've watched a thousand desperate last stands in this taproom, and every single one started with good people drinking bad ale before the siege bells chimed. Rest up, traveler; you'll need the strength!\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> nods gently: \"Irabeth holds the garrison together with sheer willpower and faith. Give her hope; she bears a heavier burden than mortal shoulders were meant to carry.\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Defender's Heart Tavern");
		}

		public static BanterResult GetAreaBanter_GrayGarrison(int cycle, string deity, bool hasAvatar)
		{
			int coins = 200;
			string text = "The Key and the Gate";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL RECOGNITION] Storming the Gray Garrison</b></color>", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> pulses with intense, radiant violet brilliance: \"The threshold of the first mythic inflection point. Across twelve thousand timelines, the causal needle has either stabilized here or snapped under demonic corruption. Reality holds its breath.\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with steely resolve: \"The Wardstone must not fall into Minagho's hands. The sacrifice of Kenabres will not be in vain!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> leans over the balcony railing: \"The prologue ends, and the real show begins! Break the script, little mortal!\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "The Gray Garrison");
		}

		public static BanterResult GetAreaBanter_WarCamp(int cycle, string deity, bool hasAvatar)
		{
			int coins = 180;
			string text = "The Inheritor";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL RECOGNITION] The Crusader March & War Camp</b></color>", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> stands tall in golden armor: \"The march north to Drezen. How many times has Galfrey ridden this road, her heart heavy with the memory of fallen knights? Yet this time... the Knight-Commander at her side is an unwritten variable.\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sneers softly: \"A fragile army held together by song and righteous delusion. In past iterations, half of these crusader regiments perished before reaching the redoubt. Let us see if this Commander possesses the iron to keep them in line.\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Crusader War Camp");
		}

		public static BanterResult GetAreaBanter_ReliableRedoubt(int cycle, string deity, bool hasAvatar)
		{
			int coins = 200;
			string text = "The Prince of Darkness";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL RECOGNITION] The Reliable Redoubt Canyon</b></color>", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods with cold satisfaction: \"Regill Derenge and the Order of the Godclaw. Unyielding discipline carved directly into the bedrock. While others panic under gargoyle talons, the Hellknights die in rank. A commendable efficiency.\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles: \"That little gnome hasn't smiled across nine hundred cycles! Let's see if our otherworlder can ruffle his immaculate hair!\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Reliable Redoubt");
		}

		public static BanterResult GetAreaBanter_LepersSmile(int cycle, string deity, bool hasAvatar)
		{
			int coins = 220;
			string text = "Our Lord in Iron";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL RECOGNITION] Leper's Smile: The Vescavor Ravine</b></color>", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars through iron fangs: \"THE ACID CANYON! A HORDE OF CHATTERING TEETH AND CORROSIVE BILE! NO ROOM FOR COWARDS HERE! WADE THROUGH THE FLESH AND SEVER THE QUEEN'S HEAD!\"", "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> shudders: \"The horror of this ravine has broken the minds of thousands of soldiers. Do not let the swarm devour your humanity, traveler.\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> winks: \"Cynical betting pool update: ninety-two percent of previous crusades suffered catastrophic morale collapse right here. The viewer ratings are through the roof!\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Leper's Smile");
		}

		public static BanterResult GetAreaBanter_LostChapel(int cycle, string deity, bool hasAvatar)
		{
			int coins = 250;
			string text = "The Lady of Graves";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL RECOGNITION] Ascent to the Lost Chapel</b></color>", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with glacial finality: \"A mountain peak drenched in betrayal, where the living are dragged down into the frozen dark to be devoured by ghouls. The dead do not rest easily upon these snowy crags.\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> clutches her sword hilt: \"Irabeth... her spirit was pushed to the brink here in so many past lives. Reach her before the despair consumes her soul!\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "The Lost Chapel");
		}

		public static BanterResult GetAreaBanter_DrezenCitadel(int cycle, string deity, bool hasAvatar)
		{
			int coins = 300;
			string text = "The Inheritor";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL RECOGNITION] The Siege of Drezen</b></color>", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> radiates holy fury: \"Seventy years Drezen has remained under the shadow of the demon banner. Seventy years of shame washed away with every step you take toward the citadel! Plant the Sword of Valor!\"", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> slams his gauntlets together: \"THE WALLS SHALL CRUMBLE! THE HORNS OF DREZEN SOUND FOR WAR! TAKE THE CITADEL AND BATHE IT IN GLORY!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> cheers: \"The climax of Act 2! All seats, place your final wagers on Staunton Vhane's tragic monologue!\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Citadel of Drezen");
		}

		public static BanterResult GetAreaBanter_AreeluLaboratory(int cycle, string deity, bool hasAvatar)
		{
			int coins = 300;
			string text = "The Key and the Gate";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL RECOGNITION] Infiltrating Areelu's Laboratory</b></color>", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> revolves in vast, multidimensional loops: \"The nexus of the anomaly. Here, the Architect spliced foreign essence into the fabric of the Worldwound. Every past soul who entered this sanctum believed themselves her child. None understood they were caught in the spindle.\"", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> sparks with manic curiosity: \"A mortal wizard fracturing the planes to bind an otherworld soul! The sheer metaphysical arrogance! Observe carefully, traveler; her secrets are the key to the entire cage!\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Areelu's Laboratory");
		}

		public static BanterResult GetAreaBanter_Wintersun(int cycle, string deity, bool hasAvatar)
		{
			int coins = 250;
			string text = "The Savored Sting";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL RECOGNITION] Entering Wintersun Under the Veil</b></color>", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> chuckles with sly appreciation: \"A village of proud barbarians living inside a sweet, poisonous dream spun by Jerribeth. They eat dirt and believe it to be honey, while demons smile in the shadows. Shatter the illusion, darling--the reckoning will be spectacular.\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> shakes his head: \"Living a lie is no way to live at all. Break the veil, Commander; let them see the sun again, even if the truth hurts.\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Wintersun Settlement");
		}

		public static BanterResult GetAreaBanter_Blackwater(int cycle, string deity, bool hasAvatar)
		{
			int coins = 280;
			string text = "The All-Seeing Eye";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL RECOGNITION] Crossing into Blackwater Cyborg Complex</b></color>", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> hums with electric distortion: \"Numerian circuitry fused to demonic flesh! A forbidden synthesis of lightning, computation, and abyssal graft! In cycle 789, an otherworlder attempted to interface their personal device with the mainframe and vaporized half the valley!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sneers: \"A grotesque imitation of order. Machines enslaving minds without the elegance of law. Exterminate the Hundred-Face abomination.\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Blackwater Complex");
		}

		public static BanterResult GetAreaBanter_IvorySanctum(int cycle, string deity, bool hasAvatar)
		{
			int coins = 300;
			string text = "The Lady of Graves";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL RECOGNITION] The Gates of the Ivory Sanctum</b></color>", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with cold judgment: \"Xanthir Vang. A soul dissolved into a writhing swarm of locusts, fleeing the judgment of the Boneyard. His counterfeit life ends here.\"", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> snarls: \"BUGS AND SQUIRMING WORMS! SQUASH THEM UNDER HEAVY IRON BOOTS! NO MERCY FOR SWARM MAGGOTS!\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "The Ivory Sanctum");
		}

		public static BanterResult GetAreaBanter_MidnightFane(int cycle, string deity, bool hasAvatar)
		{
			int coins = 350;
			string text = "The Inheritor";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL RECOGNITION] Entering the Midnight Fane</b></color>", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> unsheathes her radiant blade: \"The abyss threshold beneath our very feet. Yaniel's spirit calls for vengeance, and the Hand of the Inheritor leads the advance. Cleanse the fane and seal the rift!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> rubs his hands gleefully: \"The gateway to the Abyss! Pack your bags, traveler; the next act is in the Midnight Isles, where the rules of reality get wonderfully messy!\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "The Midnight Fane");
		}

		public static BanterResult GetAreaBanter_Alushinyrra(int cycle, string deity, bool hasAvatar)
		{
			int coins = 350;
			string text = "The Pirate Queen";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL RECOGNITION] Stepping onto the Slabs of Alushinyrra</b></color>", "<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> laughs with salty, buccaneer delight: \"Ah, Alushinyrra! The city of floating rock, blood-soaked ports, and cutthroat deals! A captain's paradise if you have the wits to keep your throat uncut! Welcome to the Midnight Isles, otherworlder!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smiles thinly: \"A wretched hive of chaotic fiends pretending to have a civilization. Watch the roofs, Commander; in this city, even the walls shift when you turn your back.\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> giggles: \"None of these demons realize they're on multiversal broadcast! Act 4 has the highest viewing ratings in all the outer planes!\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Alushinyrra City");
		}

		public static BanterResult GetAreaBanter_BattleBliss(int cycle, string deity, bool hasAvatar)
		{
			int coins = 350;
			string text = "Our Lord in Iron";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL RECOGNITION] Entering the Battlebliss Arena</b></color>", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars with earth-shattering thunder: \"THE BLOOD SANDS! THE GLADIATOR PIT OF THE ABYSS! PROVE THAT MORTAL STEEL CAN SHRED DEMONIC HIDES! TEAR THE CHAMPIONS APART!\"", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs softly: \"The screams of dying demons are music to my ears. Win the crowd, darling; demons respect only power and cruelty. Give them both.\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Battlebliss Arena");
		}

		public static BanterResult GetAreaBanter_SilkenShadows(int cycle, string deity, bool hasAvatar)
		{
			int coins = 400;
			string text = "The Savored Sting";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL RECOGNITION] Ascending to the House of Silken Shadows</b></color>", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> leans forward with sharp amusement: \"Nocticula's private parlor. She has delivered that exact brooding monologue eleven thousand times across eleven thousand iterations, completely unaware that the celestial upper deck is watching her recite her lines. Play along, darling; her vanity is her weakness.\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sips his wine: \"The Lady in Shadows plays a dangerous game, plotting her ascension to divinity. An astute mind... yet she knows nothing of the loop that holds her stage in place.\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "House of Silken Shadows");
		}

		public static BanterResult GetAreaBanter_ColyphyrMines(int cycle, string deity, bool hasAvatar)
		{
			int coins = 450;
			string text = "The Key and the Gate";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL RECOGNITION] The Nahyndrian Depths of Colyphyr</b></color>", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> vibrates with deep dimensional resonance: \"The mineralized blood of dead gods. The Nahyndrian crystals fracture the causal barrier. Baphomet waits in the labyrinth depths, following his eternal script toward defeat.\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> stands ready: \"The stolen power of Heaven must be purged from these mines. Face the Lord of the Minotaurs with unwavering faith!\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Colyphyr Mines");
		}

		public static BanterResult GetAreaBanter_IzRuins(int cycle, string deity, bool hasAvatar)
		{
			int coins = 500;
			string text = "The Lady of Graves";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL RECOGNITION] The Ruins of Iz: Where Fates Collide</b></color>", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with majestic sorrow: \"The ancient capital of Sarkoris. In ten thousand past cycles, this city has been the graveyard of heroes. Galfrey's sword, Terendelev's soul, and the Glass Key--all converge upon Deskari's scythe. Who will you save when the bells toll?\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> grips the table: \"No sacrifices this time! Break the statistics, Commander! Save them all!\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "City of Iz Ruins");
		}

		public static BanterResult GetAreaBanter_IneluctableLabyrinth(int cycle, string deity, bool hasAvatar)
		{
			int coins = 600;
			string text = "The Laughing King";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL RECOGNITION] Entering the Ineluctable Labyrinth</b></color>", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> laughs with breathless anticipation: \"Baphomet's masterpiece prison! Ten thousand souls have wandered these shifting walls until their bones turned to chalk. But our traveler isn't here to solve the maze--they're here to tear down the walls!\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks through clenched teeth: \"My Hand... my herald was dragged into this abomination and tortured. Free him, Commander. Deliver justice upon the Horned Lord!\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "The Ineluctable Labyrinth");
		}

		public static BanterResult GetAreaBanter_TheEnigma(int cycle, string deity, bool hasAvatar)
		{
			int coins = 500;
			string text = "The All-Seeing Eye";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL RECOGNITION] The Pyramid of the Enigma</b></color>", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> crackles with dismissive sparks: \"Areshkagal's monument to nothingness. The Faceless Sphinx claims all identity is an illusion. How utterly banal! The outer multiverse laughs at her hollow riddles!\"", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> rotates slowly: \"She who claims all knowledge is void knows nothing of the infinite doors. Show the Sphinx that a mortal soul possesses true, immutable weight.\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "The Enigma");
		}

		public static BanterResult GetAreaBanter_Threshold(int cycle, string deity, bool hasAvatar)
		{
			int coins = 1000;
			string text = "The Key and the Gate";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL SILENCE] The Threshold of the Worldwound</b></color>", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> slows to a near-complete stop: \"All seats... observe in silence. We stand before the spindle of causality. Across sixteen thousand, three hundred and eighty-four cycles, the Worldwound was opened, fought, and reset in an eternal loop.\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> speaks with solemn warmth: \"You've walked through fire, ash, and the Abyss itself. Whatever happens behind those doors... you've earned the respect of every god in the sky. Make it count, partner.\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> draws her blade and salutes: \"For Golarion. For Heaven. For the unwritten dawn.\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> bows his head in rare, ungrudging tribute: \"Break the cage, sovereign. The gallery is watching.\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "The Threshold");
		}

		public static BanterResult GetAreaBanter_KenabresBurning(int cycle, string deity, bool hasAvatar)
		{
			int coins = 130;
			string text = "The Drunken Hero";
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			list.Add("<color=#F5C542><b>[CELESTIAL RECOGNITION] The Burning Districts of Kenabres</b></color>");
			if (cycle <= 1)
			{
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> winces at the billowing smoke: \"Ash, fire, and burning cellars. The demons don't even have the decency to drink the tavern cellars before torching them! Slay the pyromaniacs and clear a path!\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> notes with cool detachment: \"The total breakdown of civic authority. Without martial law, Kenabres burns from within as quickly as from fiendish claws.\"");
				list.Add("<color=#FFD700><b>[The Constellation 'The Dawnflower']</b></color> shines warmly through the smoke: \"The dawn still pierces through the darkest smoke. Rescue the survivors trapped beneath the collapsed timbers; every life saved is a victory over despair.\"");
			}
			else
			{
				list.Add(string.Format("{0} laughs through the haze: \"Iteration {1}! You could navigate these burning alleys blindfolded by now. Lead the refugees out!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color>", cycle));
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> pulses: \"The thermal vectors across Kenabres match prior iterations with ninety-nine percent fidelity. The traveler walks through familiar flames.\"");
			}
			AppendSponsorshipCourtship(list, deity, hasAvatar, text);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(list, coins, text, "Kenabres Burning");
		}

		public static BanterResult GetAreaBanter_Nexus(int cycle, string deity, bool hasAvatar)
		{
			int coins = 300;
			string text = "Our Lady in Shadow";
			List<string> list = new List<string>();
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			list.Add("<color=#F5C542><b>[CELESTIAL AUDIENCE] The Nexus: Sanctum in the Midnight Isles</b></color>");
			if (cycle <= 1)
			{
				list.Add("<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> nods in relief: \"The Hand of the Inheritor has carved a sanctuary out of the very heart of the Midnight Isles. Even in the Abyss, holy resolve creates an anchor for Heaven's light.\"");
				list.Add("<color=#9400D3><b>[The Constellation 'Our Lady in Shadow']</b></color> watches from the velvet shadows with an amused smile: \"A crude little cave for mortal crusaders. How charming. Rest here while you can; the city above does not treat uninvited guests with mercy.\"");
				list.Add("<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> appraises the crystalline walls: \"A defensible redoubt behind enemy lines. Economical and strategically sound. Use it to coordinate your strikes against the demon lords.\"");
			}
			else
			{
				list.Add("<color=#9400D3><b>[The Constellation 'Our Lady in Shadow']</b></color> whispers intimately: \"Back to my islands once again, traveler? You walk into the Nexus as if you hold the deed to the entire archipelago. Enter and let us see what you conspire.\"");
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> echoes from the crystal veins: \"The subterranean portal network stabilizes. The Nexus remains the causal fulcrum of the fourth act.\"");
			}
			AppendSponsorshipCourtship(list, deity, hasAvatar, text);
			list.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(list, coins, text, "The Nexus");
		}

		public static BanterResult GetAreaBanter_TenThousandDelights(int cycle, string deity, bool hasAvatar)
		{
			int coins = 250;
			string text = "The Savored Sting";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Ten Thousand Delights: Palace of Vices</b></color>", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs with delicious amusement: \"Ten Thousand Delights... where every kiss has a price and every secret can be bought for a drop of poisoned nectar. Keep your eyes sharp and your daggers sharper!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> wrinkles his nose: \"The drinks are laced with succubus venom, the music gives you a headache, and half the patrons are looking to harvest your soul. Stick to honest taverns, partner.\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sneers at the decadent velvet drapery: \"Chaotic indulgences masquerading as power. Extract whatever leverage Chivarro holds and burn the rest.\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Ten Thousand Delights");
		}

		public static BanterResult GetAreaBanter_Fleshmarket(int cycle, string deity, bool hasAvatar)
		{
			int coins = 300;
			string text = "The Inheritor";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Fleshmarkets of Alushinyrra</b></color>", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with righteous indignation: \"The auction of living souls! Mortals chained and bartered like cattle by demonic scum! Every fiber of Heaven demands their liberation! Break their collars, Commander!\"", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> grins with vicious anticipation: \"NO LAWS HERE! ONLY BUTCHERY AND BLOOD! CLEAVE THE SLAVEMASTERS APART AND SPILL THEIR COINS INTO THE GUTTER!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> folds his arms with disdain: \"Demon slavers have no comprehension of true contracts or orderly bondage. They rely on savage brutality. Teach them the lesson they richly deserve.\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "The Fleshmarkets");
		}

		public static BanterResult GetAreaBanter_PulurasFall(int cycle, string deity, bool hasAvatar)
		{
			int coins = 350;
			string text = "The Song of the Spheres";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Pulura's Fall: Sanctuary of the Stargazers</b></color>", "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> shines with celestial wonder: \"Pulura's sanctuary... perched beneath the aurora borealis, where quiet scholars gaze at the infinite reaches of the dark tapestry. Let no fiendish corruption violate this sacred cradle of dreams!\"", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> hums: \"The stargazers chart celestial alignments dating back to the Age of Anguish. The mathematical records here hold keys to cosmic rifts that even the crusade does not comprehend.\"", "<color=#AFEEEE><b>[The Constellation 'The Shimmering Maiden']</b></color> whispers across the aurora: \"The northern stars watch over you, traveler. Tread softly upon the snows of our sanctuary.\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Pulura's Fall");
		}

		public static BanterResult GetAreaBanter_HeartOfMystery(int cycle, string deity, bool hasAvatar)
		{
			int coins = 350;
			string text = "The All-Seeing Eye";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Heart of Mystery: Ancient Sarkorian Sanctuary</b></color>", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> radiates arcane fascination: \"The Heart of Mystery! Ancient Sarkorian ley line nexus! The portals here do not connect to the Abyss, but to lost dimensional pockets outside standard planar cosmology!\"", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> pulses with geometric perfection: \"The four cardinal puzzles align with the cosmic spindle. The masks of Sarkoris await the traveler who remembers the ancient symbols.\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> snickers: \"Ah, the puzzle sanctuary! How many hours did you spend rotating stone slabs in previous cycles? Don't look at me for hints; I just enjoy watching mortal brains smoke!\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Heart of Mystery");
		}

		public static BanterResult GetAreaBanter_RedDragonLair(int cycle, string deity, bool hasAvatar)
		{
			int coins = 300;
			string text = "Our Lord in Iron";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Dragon's Peak: Devarra's Volcano Lair</b></color>", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars until the magma boils: \"AN ANCIENT RED DRAGON! DEVARRALOCH! BATHED IN FLAME AND SLAG! THIS IS HUNTING WORTHY OF GODS! STEEL AGAINST SCALE, TEETH AGAINST BLADE!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles: \"Greybor looked all serious about tracking fees, but taking down a colossal red dragon is the stuff songs are made of! Don't get roasted!\"", "<color=#D2691E><b>[The Constellation 'Father of Creation']</b></color> speaks with deep forge resonance: \"The dragon's furnace burns hot enough to melt dwarven iron. Strike true before her breath turns your armor into an oven.\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "The Dragon's Peak");
		}

		public static BanterResult GetAreaBanter_NamelessRuins(int cycle, string deity, bool hasAvatar)
		{
			int coins = 300;
			string text = "The Lady of Graves";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Nameless Ruins: Faceless Threshold</b></color>", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with chill finality: \"A place where names rot and identities dissolve into dust. Even the Boneyard's scribes find no entries for those swallowed by this desolation. Beware losing your own true nature.\"", "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> urges softly: \"Hold fast to who you are, traveler. Nenio's pursuit of knowledge brought her to this precipice, but true wisdom never requires erasing one's own heart.\"", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> pulses: \"The boundary between memory and nonexistence. The pyramid gateway hums with ancient oblivion.\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "The Nameless Ruins");
		}

		public static BanterResult GetAreaBanter_MoltenScar(int cycle, string deity, bool hasAvatar)
		{
			int coins = 250;
			string text = "Our Lord in Iron";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Molten Scar: Ambush in the Crags</b></color>", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> laughs with battle glee: \"SULFUR CRAGS AND DEMON AMBUSHES! THE VROCKS CIRCLE ABOVE, SCREREECHING FOR FLESH! BRING DOWN THEIR WINGS AND BREAK THEIR BEAKS!\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with urgent resolve: \"Crusader scouts are captured and tortured in these sulfurous pits. Do not let them perish in despair; free them and avenge their fallen squad!\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "The Molten Scar");
		}

		public static BanterResult GetAreaBanter_ChillyCreek(int cycle, string deity, bool hasAvatar)
		{
			int coins = 250;
			string text = "The Song of the Spheres";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Chilly Creek: The Cursed Lakeside</b></color>", "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> gazes sorrowfully upon the misty village: \"A quiet hamlet trapped in an insidious web of deceit. The villagers smile with glassy eyes while a malevolent hag hollows out their souls. Bring truth to the lakeside.\"", "<color=#8FBC8F><b>[The Constellation 'Old Deadeye']</b></color> speaks with grim disappointment: \"A community that turned from honest toil to worship a false wooden idol in the marsh. A hollow pact always bears bitter fruit. Expose the corruption and heal the village.\"" };
			AppendSponsorshipCourtship(obj, deity, hasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Chilly Creek");
		}

		public static BanterResult GetCompanionClimax_CamelliaBasement(in DialogueContext ctx)
		{
			int coins = 350;
			string text = "The Savored Sting";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Abandoned House: Camellia's Secret</b></color>", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> recoils with cold disgust: \"A mutilated crusader... his heart carved out in the dark. There is no spirit of the land; there is only a mortal indulging in the ecstasy of murder. Justice demands her blood.\"", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> chuckles into a silk fan: \"My, my! The refined noblewoman with the immaculate corset turns out to be a bloodthirsty little butcher! Her passion is utterly unhinged, darling. Keep her close or cut her throat; she is delightful either way.\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sneers: \"A lunatic indulging in slaughter without contract or discipline. Useful if aimed at enemies; fatal if left unwatched.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Camellia's Basement Secret");
		}

		public static BanterResult GetCompanionClimax_DaeranTribunal(in DialogueContext ctx)
		{
			int coins = 400;
			string text = "The Key and the Gate";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Arendae Tribunal: The Other Stirs</b></color>", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> revolves with grave resonance: \"An alien entity from the outer void. The Other feeds upon mortal memories, hiding behind the frivolous mask of the young count. The dimensional membrane thins.\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> sighs softly: \"Poor little rich boy. He threw parties and drank until dawn just to drown out the whispering horror living behind his eyes. Don't let Liotr execute him, Commander; Daeran deserves a chance to breathe his own air.\"", "<color=#87CEEB><b>[The Constellation 'The Silence Between the Stars']</b></color> hums in quiet stillness: \"The silence between his thoughts is crowded by an uninvited guest. Break the parasitic bond; let the silence heal him.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Daeran's Tribunal");
		}

		public static BanterResult GetCompanionClimax_NenioEnigma(in DialogueContext ctx)
		{
			int coins = 450;
			string text = "The All-Seeing Eye";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] The Enigma: Nenio Faces Areshkagal</b></color>", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> blazes with electric triumph: \"A mortal mind denying the void! Areshkagal demands you forget your name and dissolve into nothingness, yet the kitsune scholar simply notes her ramblings down on parchment as flawed methodology! Magnificence!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> laughs until tears stream: \"Nenio just peer-reviewed a Demon Lord into an existential panic! 'Your thesis on absolute nothingness lacks rigorous peer review!' Pwahaha! Give that fox a medal!\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "The Enigma Climax");
		}

		public static BanterResult GetCompanionClimax_SosielTrever(in DialogueContext ctx)
		{
			int coins = 400;
			string text = "The Eternal Rose";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Battlebliss: The Reunion of Brothers</b></color>", "<color=#FF69B4><b>[The Constellation 'The Eternal Rose']</b></color> weeps soft, shimmering tears of hope: \"Trever... broken in the gladiatorial sand, clutching his shattered shield. Yet Sosiel steps forward not with a blade to slay him, but with open arms to bring him home. Love endures all darkness.\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> bows her head in solemn respect: \"A paladin who fell into the abyss, and a brother who ventured into hell to drag him back into the light. Heaven salutes you both.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Sosiel & Trever");
		}

		public static BanterResult GetCompanionClimax_RegillTrial(in DialogueContext ctx)
		{
			int coins = 400;
			string text = "The Prince of Darkness";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Godclaw Outpost: Regill's Supreme Sacrifice</b></color>", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> stands and offers a slow, dignified salute: \"Regill Derenge deliberately accused himself of treason, inviting judgment upon his own head, solely to bind the entire Order of the Godclaw irrevocably to the Knight-Commander's command. That is the pinnacle of infernal devotion. A true master of law.\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with quiet awe: \"A severe, merciless man... who sacrificed his own reputation and honor so the crusade could triumph. His dedication cannot be questioned.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Regill's Trial");
		}

		public static BanterResult GetCompanionClimax_Savamelekh(in DialogueContext ctx)
		{
			int coins = 350;
			string text = "Father of Creation";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Savamelekh's Lair: The Mongrel Reckoning</b></color>", "<color=#D2691E><b>[The Constellation 'Father of Creation']</b></color> grunts with fierce pride: \"The demon who poisoned the mongrel bloodline with corrupted bile! Lann and Wenduag stand against their torturer! Break the poison at its root! Purify the clan!\"", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> bellows: \"TEAR THE DEMON'S VENOM GLANDS OUT! NO MORE SLAVERY TO FIENDS! MONGRELS FIGHT LIKE TRUE WARRIORS!\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Savamelekh's Lair");
		}

		public static BanterResult GetCompanionClimax_WoljifYgefeles(in DialogueContext ctx)
		{
			int coins = 350;
			string text = "The Pirate Queen";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Woljif Faces His Demon Ancestor</b></color>", "<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> winks roguishly: \"Old Ygefeles wants his blood back! A demon grandfather demanding obedience? Tell him to walk the plank! Steal his shadow power and run, Woljif! A thief answers to no one!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> laughs: \"Family reunions are always awkward, but especially when grandpa is an abyssal shadow demon. You've got your own crew now, Woljif! Stand tall!\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Woljif & Ygefeles");
		}

		public static BanterResult GetCompanionClimax_EmberSermon(in DialogueContext ctx)
		{
			int coins = 450;
			string text = "The Dawnflower";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Ember's Sermon to the Demons</b></color>", "<color=#FFD700><b>[The Constellation 'The Dawnflower']</b></color> shines with blinding, tearful ecstasy: \"Ember... barefoot in the ash, offering kindness to creatures born of pure evil. And they listen! Demons weeping because someone called them hurt children! Her innocence is a weapon more devastating than a thousand holy swords!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> rubs his temples with deep bewilderment: \"I have studied theology and metaphysics for eons. Yet this barefoot elven girl has converted demonic legionnaires with simple, unbroken pity. It defies all logic... and it is utterly terrifying.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Ember's Sermon");
		}

		public static BanterResult GetCompanionClimax_ArueshalaeDream(in DialogueContext ctx)
		{
			int coins = 400;
			string text = "The Song of the Spheres";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Arueshalae: Burning the Abyssal Shadow</b></color>", "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> surrounds the soul with starlight and butterfly wings: \"The dream realm burns away the chains of the Abyss! A succubus choosing mortal love and repentance over demonic immortality. Your soul is free, Arueshalae! Fly beneath the open sky!\"", "<color=#87CEEB><b>[The Constellation 'The Silence Between the Stars']</b></color> whispers softly: \"In the quiet spaces of her heart, the screaming fiend has finally ceased. The dream is whole.\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Arueshalae's Dream");
		}

		public static BanterResult GetCompanionClimax_GreyborGuild(in DialogueContext ctx)
		{
			int coins = 350;
			string text = "The Savored Sting";
			List<string> obj = new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CELESTIAL AUDIENCE] Greybor: Contract vs. Loyalty</b></color>", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> smiles: \"The Guildmaster's poisoned dagger or a true comrade's handshake. A professional must choose what his honor is worth. He chose the Commander. A contract forged in blood and mutual respect.\"", "<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> toasts her tankard: \"A mercenary who stays loyal to the captain when gold is offered from the shadows? Now that's someone you want watching your back on the high seas!\"" };
			AppendSponsorshipCourtship(obj, ctx.PlayerDeity, ctx.HasAvatar, text);
			obj.Add("<color=#F5C542><b>=======================================================</b></color>");
			return new BanterResult(obj, coins, text, "Greybor's Guild Choice");
		}

		public static void EnsureInitialized()
		{
			if (!_dispatcherInitialized)
			{
				_dispatcherInitialized = true;
				RegisterAct1Banter();
				RegisterAct2Banter();
				RegisterAct3Banter();
				RegisterAct4Banter();
				RegisterAct5Banter();
				RegisterAct6Banter();
				RegisterCompanionBanter();
				RegisterAreaBanter();
				RegisterSideQuestsBanter();
				RegisterDlcBanter();
			}
		}

		public static void RegisterDynamicAnswerHandler(string key, Func<DialogueContext, BanterResult> handler)
		{
			EnsureInitialized();
			if (!string.IsNullOrEmpty(key) && handler != null)
			{
				_exactAnswerHandlers[key] = handler;
			}
		}

		public static bool TryGetAnswerBanter(in DialogueContext ctx, out BanterResult result)
		{
			EnsureInitialized();
			if (!string.IsNullOrEmpty(ctx.AnswerGuid) && _exactAnswerHandlers.TryGetValue(ctx.AnswerGuid, out var value))
			{
				result = value(ctx);
				if (result != null && result.Lines != null)
				{
					return result.Lines.Count > 0;
				}
				return false;
			}
			if (!string.IsNullOrEmpty(ctx.AnswerName) && _exactAnswerHandlers.TryGetValue(ctx.AnswerName, out var value2))
			{
				result = value2(ctx);
				if (result != null && result.Lines != null)
				{
					return result.Lines.Count > 0;
				}
				return false;
			}
			result = null;
			return false;
		}

		public static bool TryGetSceneBanter(string dialogName, in DialogueContext ctx, out BanterResult result)
		{
			EnsureInitialized();
			if (string.IsNullOrEmpty(dialogName))
			{
				result = null;
				return false;
			}
			if (_sceneHandlers.TryGetValue(dialogName, out var value))
			{
				result = value(ctx);
				if (result != null && result.Lines != null)
				{
					return result.Lines.Count > 0;
				}
				return false;
			}
			foreach (KeyValuePair<string, Func<DialogueContext, BanterResult>> sceneHandler in _sceneHandlers)
			{
				if (dialogName.IndexOf(sceneHandler.Key, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					result = sceneHandler.Value(ctx);
					return result != null && result.Lines != null && result.Lines.Count > 0;
				}
			}
			result = null;
			return false;
		}

		public static bool TryGetAreaBanter(string areaName, int cycle, string playerDeity, bool hasAvatar, out BanterResult result)
		{
			EnsureInitialized();
			if (string.IsNullOrEmpty(areaName))
			{
				result = null;
				return false;
			}
			if (_areaHandlers.TryGetValue(areaName, out var value))
			{
				result = value(cycle, playerDeity, hasAvatar);
				if (result != null && result.Lines != null)
				{
					return result.Lines.Count > 0;
				}
				return false;
			}
			foreach (KeyValuePair<string, Func<int, string, bool, BanterResult>> areaHandler in _areaHandlers)
			{
				if (areaName.IndexOf(areaHandler.Key, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					result = areaHandler.Value(cycle, playerDeity, hasAvatar);
					return result != null && result.Lines != null && result.Lines.Count > 0;
				}
			}
			result = null;
			return false;
		}

		public static bool IsExitOrInformationalAnswer(string text, string ansName)
		{
			if (!string.IsNullOrEmpty(ansName))
			{
				string text2 = ansName.ToLower();
				if (text2.Contains("farewell") || text2.Contains("leave") || text2.Contains("exit") || text2.Contains("cancel") || text2.Contains("back") || text2.Contains("close") || text2.Contains("trade") || text2.Contains("vendor"))
				{
					return true;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				string text3 = text.ToLower().Trim();
				if (text3.StartsWith("farewell") || text3.StartsWith("goodbye") || text3.StartsWith("i should go") || text3.StartsWith("i must go") || text3.StartsWith("i have to go") || text3.StartsWith("i need to go") || text3.StartsWith("i must take my leave") || text3.StartsWith("good luck") || text3.StartsWith("let's move on") || text3.StartsWith("back") || text3.StartsWith("later") || text3.StartsWith("see you later") || text3.StartsWith("never mind") || text3.StartsWith("nevermind") || text3.StartsWith("that is all for now") || text3.StartsWith("that will be all") || text3.StartsWith("that's all") || text3.StartsWith("that's all for now") || text3.StartsWith("[leave]") || text3.StartsWith("(leave)") || text3.StartsWith("[exit]") || text3.StartsWith("(exit)") || text3.StartsWith("[end dialogue]") || text3.StartsWith("[close]") || text3.StartsWith("(back)") || text3.StartsWith("let's trade") || text3.StartsWith("trade") || text3.StartsWith("show me your wares") || text3.StartsWith("let me see what you have") || text3.StartsWith("show your wares") || text3.StartsWith("show me what you have") || text3.StartsWith("show me your goods") || text3.StartsWith("what do you have for sale") || text3.StartsWith("show me your stock"))
				{
					return true;
				}
			}
			return false;
		}

		private static void RegisterAct1Banter()
		{
			_sceneHandlers["BlackWingRuins"] = (DialogueContext ctx) => GetSceneBanter_BlackwingLibrary(in ctx);
			_sceneHandlers["BlackWingRuins_St2_FirstTime_dialog"] = (DialogueContext ctx) => GetSceneBanter_BlackwingLibrary(in ctx);
			_sceneHandlers["EstrodTower"] = (DialogueContext ctx) => GetSceneBanter_TowerOfEstrod(in ctx);
			_sceneHandlers["Teldon_dialogue"] = (DialogueContext ctx) => GetSceneBanter_TowerOfEstrod(in ctx);
			_sceneHandlers["Visions_EstrodTower_dialog"] = (DialogueContext ctx) => GetSceneBanter_TowerOfEstrod(in ctx);
			_sceneHandlers["TopazSolutions"] = (DialogueContext ctx) => GetSceneBanter_TopazSolutions(in ctx);
			_sceneHandlers["Topaz_dialog"] = (DialogueContext ctx) => GetSceneBanter_TopazSolutions(in ctx);
			_sceneHandlers["DesnaTempleFinal"] = (DialogueContext ctx) => GetSceneBanter_DesnaTemple(in ctx);
			_sceneHandlers["DesnaTemple_Final_dialog"] = (DialogueContext ctx) => GetSceneBanter_DesnaTemple(in ctx);
			_sceneHandlers["HorgusManor"] = (DialogueContext ctx) => GetSceneBanter_HorgusManor(in ctx);
			_sceneHandlers["HorgusManor_dialogue"] = (DialogueContext ctx) => GetSceneBanter_HorgusManor(in ctx);
		}

		private static void RegisterAct2Banter()
		{
			_sceneHandlers["LostChapel"] = (DialogueContext ctx) => GetSceneBanter_LostChapel(in ctx);
			_sceneHandlers["Among_Ghouls_Dialogue"] = (DialogueContext ctx) => GetSceneBanter_LostChapel(in ctx);
			_sceneHandlers["Anevia_Start_Dialogue"] = (DialogueContext ctx) => GetSceneBanter_LostChapel(in ctx);
			_sceneHandlers["LC_IrabethDialogue"] = (DialogueContext ctx) => GetSceneBanter_LostChapel(in ctx);
			_sceneHandlers["Zacharius"] = (DialogueContext ctx) => GetSceneBanter_ZachariusCrypt(in ctx);
			_sceneHandlers["Zacharius_dialogue"] = (DialogueContext ctx) => GetSceneBanter_ZachariusCrypt(in ctx);
			_sceneHandlers["GlobalPuzzle"] = (DialogueContext ctx) => GetSceneBanter_ConundrumUnsolved(in ctx);
			_sceneHandlers["AncientPuzzle"] = (DialogueContext ctx) => GetSceneBanter_ConundrumUnsolved(in ctx);
			_sceneHandlers["NurahDialog"] = (DialogueContext ctx) => GetSceneBanter_NurahTraitor(in ctx);
			_sceneHandlers["Nurah_GibberingSwarm_dialog"] = (DialogueContext ctx) => GetSceneBanter_NurahTraitor(in ctx);
			_sceneHandlers["GargoyleAttack_Nurah_Dialogue"] = (DialogueContext ctx) => GetSceneBanter_NurahTraitor(in ctx);
			_sceneHandlers["Nurah_After_Battle_Dialogue"] = (DialogueContext ctx) => GetSceneBanter_NurahTraitor(in ctx);
			_sceneHandlers["HellknightsUnderAttack"] = (DialogueContext ctx) => GetSceneBanter_HellknightRedoubt(in ctx);
			_sceneHandlers["Yaker_dialogue"] = (DialogueContext ctx) => GetSceneBanter_HellknightRedoubt(in ctx);
		}

		private static void RegisterAct3Banter()
		{
			_sceneHandlers["DragonHunt"] = (DialogueContext ctx) => GetSceneBanter_DragonHunt(in ctx);
			_sceneHandlers["DragonsGraveyard"] = (DialogueContext ctx) => GetSceneBanter_DragonHunt(in ctx);
			_sceneHandlers["KohhDungeon"] = (DialogueContext ctx) => GetSceneBanter_Blackwater(in ctx);
			_sceneHandlers["NumerianCamp"] = (DialogueContext ctx) => GetSceneBanter_Blackwater(in ctx);
			_sceneHandlers["AutomaticDoor_dialogue"] = (DialogueContext ctx) => GetSceneBanter_Blackwater(in ctx);
			_sceneHandlers["Bunker_Chief_dialogue"] = (DialogueContext ctx) => GetSceneBanter_Blackwater(in ctx);
			_sceneHandlers["MoltenScar"] = (DialogueContext ctx) => GetSceneBanter_MoltenScar(in ctx);
			_sceneHandlers["PrisonedKnight_dialogue"] = (DialogueContext ctx) => GetSceneBanter_MoltenScar(in ctx);
			_sceneHandlers["MidnightFane"] = (DialogueContext ctx) => GetSceneBanter_MidnightFane(in ctx);
			_sceneHandlers["DarrazandFight_MidnightFane_dialog"] = (DialogueContext ctx) => GetSceneBanter_MidnightFane(in ctx);
			_sceneHandlers["TempleOfDelamere"] = (DialogueContext ctx) => GetSceneBanter_TempleOfDelamere(in ctx);
		}

		private static void RegisterAct4Banter()
		{
			_sceneHandlers["BattleBliss"] = (DialogueContext ctx) => GetSceneBanter_BattleblissArena(in ctx);
			_sceneHandlers["ArenaHealer_dialogue"] = (DialogueContext ctx) => GetSceneBanter_BattleblissArena(in ctx);
			_sceneHandlers["Atselm_dialogue"] = (DialogueContext ctx) => GetSceneBanter_BattleblissArena(in ctx);
			_sceneHandlers["DireOneBattlebliss_dialogue"] = (DialogueContext ctx) => GetSceneBanter_BattleblissArena(in ctx);
			_sceneHandlers["TenThousandDelights"] = (DialogueContext ctx) => GetSceneBanter_TenThousandDelights(in ctx);
			_sceneHandlers["Chivarro_dialogue"] = (DialogueContext ctx) => GetSceneBanter_TenThousandDelights(in ctx);
			_sceneHandlers["Herraxa_dialogue"] = (DialogueContext ctx) => GetSceneBanter_TenThousandDelights(in ctx);
			_sceneHandlers["Vellexia"] = (DialogueContext ctx) => GetSceneBanter_VellexiaDates(in ctx);
			_sceneHandlers["Vellexia_Consp_dialogue"] = (DialogueContext ctx) => GetSceneBanter_VellexiaDates(in ctx);
			_sceneHandlers["FinalDungeon_Colyphyr"] = (DialogueContext ctx) => GetSceneBanter_ColyphyrHepzamirah(in ctx);
			_sceneHandlers["Hepzamirah"] = (DialogueContext ctx) => GetSceneBanter_ColyphyrHepzamirah(in ctx);
		}

		private static void RegisterAct5Banter()
		{
			_sceneHandlers["Alderpash"] = (DialogueContext ctx) => GetSceneBanter_Alderpash(in ctx);
			_sceneHandlers["Alderpash_dialogue"] = (DialogueContext ctx) => GetSceneBanter_Alderpash(in ctx);
			_sceneHandlers["HandOfInheritorTortured"] = (DialogueContext ctx) => GetSceneBanter_HandSalvation(in ctx);
			_sceneHandlers["Pulura_C3"] = (DialogueContext ctx) => GetSceneBanter_PuluraFall(in ctx);
			_sceneHandlers["PuluraFall"] = (DialogueContext ctx) => GetSceneBanter_PuluraFall(in ctx);
			_sceneHandlers["Eliandra_main_dialogue"] = (DialogueContext ctx) => GetSceneBanter_PuluraFall(in ctx);
			_sceneHandlers["Drezen_Under_Siedge"] = (DialogueContext ctx) => GetSceneBanter_KhorramzadehSiege(in ctx);
			_sceneHandlers["Khorramzadeh"] = (DialogueContext ctx) => GetSceneBanter_KhorramzadehSiege(in ctx);
			_sceneHandlers["DawnOfDragons"] = (DialogueContext ctx) => GetSceneBanter_DawnOfDragons(in ctx);
		}

		private static void RegisterAct6Banter()
		{
			_sceneHandlers["ThresholdCamp"] = (DialogueContext ctx) => GetSceneBanter_ThresholdCamp(in ctx);
			_sceneHandlers["Threshold_Camp"] = (DialogueContext ctx) => GetSceneBanter_ThresholdCamp(in ctx);
			_sceneHandlers["ThresholdEveCamp"] = (DialogueContext ctx) => GetSceneBanter_ThresholdCamp(in ctx);
			_sceneHandlers["EchoOfDeskari"] = (DialogueContext ctx) => GetSceneBanter_EchoOfDeskari(in ctx);
			_sceneHandlers["DeskariEcho"] = (DialogueContext ctx) => GetSceneBanter_EchoOfDeskari(in ctx);
			_sceneHandlers["ThresholdExterior"] = (DialogueContext ctx) => GetSceneBanter_EchoOfDeskari(in ctx);
			_sceneHandlers["Areelu_Threshold"] = (DialogueContext ctx) => GetSceneBanter_AreeluConfrontation(in ctx);
			_sceneHandlers["Areelu_Final"] = (DialogueContext ctx) => GetSceneBanter_AreeluConfrontation(in ctx);
			_sceneHandlers["AreeluDialogue_Threshold"] = (DialogueContext ctx) => GetSceneBanter_AreeluConfrontation(in ctx);
			_sceneHandlers["ThresholdClosing"] = (DialogueContext ctx) => GetSceneBanter_WorldwoundHeart(in ctx);
			_sceneHandlers["WorldwoundHeart"] = (DialogueContext ctx) => GetSceneBanter_WorldwoundHeart(in ctx);
			_sceneHandlers["Threshold_Sacrifice"] = (DialogueContext ctx) => GetSceneBanter_WorldwoundHeart(in ctx);
			_sceneHandlers["AreeluAscension"] = (DialogueContext ctx) => GetSceneBanter_SecretAscensionEnding(in ctx);
			_sceneHandlers["Ascension_Dialogue"] = (DialogueContext ctx) => GetSceneBanter_SecretAscensionEnding(in ctx);
			_sceneHandlers["ThresholdEnding_Ascension"] = (DialogueContext ctx) => GetSceneBanter_SecretAscensionEnding(in ctx);
			_sceneHandlers["PharasmaCourt"] = (DialogueContext ctx) => GetSceneBanter_PharasmaCourt(in ctx);
			_sceneHandlers["Pharasma_Judgment"] = (DialogueContext ctx) => GetSceneBanter_PharasmaCourt(in ctx);
			_sceneHandlers["Judgment_Pharasma"] = (DialogueContext ctx) => GetSceneBanter_PharasmaCourt(in ctx);
			_sceneHandlers["Enigma_Final"] = (DialogueContext ctx) => GetSceneBanter_AreshkagalClimax(in ctx);
			_sceneHandlers["Areshkagal"] = (DialogueContext ctx) => GetSceneBanter_AreshkagalClimax(in ctx);
			_sceneHandlers["AreshkagalDialogue"] = (DialogueContext ctx) => GetSceneBanter_AreshkagalClimax(in ctx);
			_sceneHandlers["Darkness_Dialogue"] = (DialogueContext ctx) => GetSceneBanter_InevitableDarkness(in ctx);
			_sceneHandlers["InevitableDarkness"] = (DialogueContext ctx) => GetSceneBanter_InevitableDarkness(in ctx);
		}

		private static void RegisterCompanionBanter()
		{
			_sceneHandlers["Camellia_Q2"] = (DialogueContext ctx) => GetCompanionClimax_CamelliaBasement(in ctx);
			_sceneHandlers["CamelliaBasement"] = (DialogueContext ctx) => GetCompanionClimax_CamelliaBasement(in ctx);
			_sceneHandlers["CamelliaMurder"] = (DialogueContext ctx) => GetCompanionClimax_CamelliaBasement(in ctx);
			_sceneHandlers["Daeran_Q3"] = (DialogueContext ctx) => GetCompanionClimax_DaeranTribunal(in ctx);
			_sceneHandlers["Liotr_Drezen"] = (DialogueContext ctx) => GetCompanionClimax_DaeranTribunal(in ctx);
			_sceneHandlers["KTC_Prelate_Liotr"] = (DialogueContext ctx) => GetCompanionClimax_DaeranTribunal(in ctx);
			_sceneHandlers["AreshkaConfrontation"] = (DialogueContext ctx) => GetCompanionClimax_NenioEnigma(in ctx);
			_sceneHandlers["Nenio_Q3"] = (DialogueContext ctx) => GetCompanionClimax_NenioEnigma(in ctx);
			_sceneHandlers["MeetTrever"] = (DialogueContext ctx) => GetCompanionClimax_SosielTrever(in ctx);
			_sceneHandlers["TreverCompanion"] = (DialogueContext ctx) => GetCompanionClimax_SosielTrever(in ctx);
			_sceneHandlers["Regill_Q3"] = (DialogueContext ctx) => GetCompanionClimax_RegillTrial(in ctx);
			_sceneHandlers["HellknightsOutpost"] = (DialogueContext ctx) => GetCompanionClimax_RegillTrial(in ctx);
			_sceneHandlers["Savamelekh"] = (DialogueContext ctx) => GetCompanionClimax_Savamelekh(in ctx);
			_sceneHandlers["Ygefeles"] = (DialogueContext ctx) => GetCompanionClimax_WoljifYgefeles(in ctx);
			_sceneHandlers["Ember_Q3"] = (DialogueContext ctx) => GetCompanionClimax_EmberSermon(in ctx);
			_sceneHandlers["Arueshalae_Q3"] = (DialogueContext ctx) => GetCompanionClimax_ArueshalaeDream(in ctx);
			_sceneHandlers["Greybor_Q3"] = (DialogueContext ctx) => GetCompanionClimax_GreyborGuild(in ctx);
			_sceneHandlers["Grimbor_Q3"] = (DialogueContext ctx) => GetCompanionClimax_GreyborGuild(in ctx);
		}

		private static void RegisterAreaBanter()
		{
			_areaHandlers["Prologue_Caves_1"] = (int cycle, string deity, bool avatar) => GetAreaBanter_CavesUnderKenabres(cycle, deity, avatar);
			_areaHandlers["ShieldMaze"] = (int cycle, string deity, bool avatar) => GetAreaBanter_ShieldMaze(cycle, deity, avatar);
			_areaHandlers["KenabresMarketSquare"] = (int cycle, string deity, bool avatar) => GetAreaBanter_MarketSquare(cycle, deity, avatar);
			_areaHandlers["MarketSquare"] = (int cycle, string deity, bool avatar) => GetAreaBanter_MarketSquare(cycle, deity, avatar);
			_areaHandlers["KenabresBurning"] = (int cycle, string deity, bool avatar) => GetAreaBanter_KenabresBurning(cycle, deity, avatar);
			_areaHandlers["BlackWingRuins"] = (int cycle, string deity, bool avatar) => GetAreaBanter_BlackwingLibrary(cycle, deity, avatar);
			_areaHandlers["EstrodTower"] = (int cycle, string deity, bool avatar) => GetAreaBanter_TowerOfEstrod(cycle, deity, avatar);
			_areaHandlers["DefendersHeart"] = (int cycle, string deity, bool avatar) => GetAreaBanter_DefendersHeart(cycle, deity, avatar);
			_areaHandlers["GreyGarrison"] = (int cycle, string deity, bool avatar) => GetAreaBanter_GrayGarrison(cycle, deity, avatar);
			_areaHandlers["WarCamp"] = (int cycle, string deity, bool avatar) => GetAreaBanter_WarCamp(cycle, deity, avatar);
			_areaHandlers["HellknightsUnderAttack"] = (int cycle, string deity, bool avatar) => GetAreaBanter_ReliableRedoubt(cycle, deity, avatar);
			_areaHandlers["GibberingSwarm"] = (int cycle, string deity, bool avatar) => GetAreaBanter_LepersSmile(cycle, deity, avatar);
			_areaHandlers["LostChapel"] = (int cycle, string deity, bool avatar) => GetAreaBanter_LostChapel(cycle, deity, avatar);
			_areaHandlers["DrezenCitadel"] = (int cycle, string deity, bool avatar) => GetAreaBanter_DrezenCitadel(cycle, deity, avatar);
			_areaHandlers["DrezenSiege"] = (int cycle, string deity, bool avatar) => GetAreaBanter_DrezenCitadel(cycle, deity, avatar);
			_areaHandlers["AreeluLaboratory"] = (int cycle, string deity, bool avatar) => GetAreaBanter_AreeluLaboratory(cycle, deity, avatar);
			_areaHandlers["Wintersun"] = (int cycle, string deity, bool avatar) => GetAreaBanter_Wintersun(cycle, deity, avatar);
			_areaHandlers["KohhDungeon"] = (int cycle, string deity, bool avatar) => GetAreaBanter_Blackwater(cycle, deity, avatar);
			_areaHandlers["IvorySanctum"] = (int cycle, string deity, bool avatar) => GetAreaBanter_IvorySanctum(cycle, deity, avatar);
			_areaHandlers["MidnightFane"] = (int cycle, string deity, bool avatar) => GetAreaBanter_MidnightFane(cycle, deity, avatar);
			_areaHandlers["PulurasFall"] = (int cycle, string deity, bool avatar) => GetAreaBanter_PulurasFall(cycle, deity, avatar);
			_areaHandlers["PuluraSanctuary"] = (int cycle, string deity, bool avatar) => GetAreaBanter_PulurasFall(cycle, deity, avatar);
			_areaHandlers["HeartOfMystery"] = (int cycle, string deity, bool avatar) => GetAreaBanter_HeartOfMystery(cycle, deity, avatar);
			_areaHandlers["SarkorisMystery"] = (int cycle, string deity, bool avatar) => GetAreaBanter_HeartOfMystery(cycle, deity, avatar);
			_areaHandlers["RedDragonLair"] = (int cycle, string deity, bool avatar) => GetAreaBanter_RedDragonLair(cycle, deity, avatar);
			_areaHandlers["DevarraLair"] = (int cycle, string deity, bool avatar) => GetAreaBanter_RedDragonLair(cycle, deity, avatar);
			_areaHandlers["MoltenScar"] = (int cycle, string deity, bool avatar) => GetAreaBanter_MoltenScar(cycle, deity, avatar);
			_areaHandlers["ChillyCreek"] = (int cycle, string deity, bool avatar) => GetAreaBanter_ChillyCreek(cycle, deity, avatar);
			_areaHandlers["Nexus"] = (int cycle, string deity, bool avatar) => GetAreaBanter_Nexus(cycle, deity, avatar);
			_areaHandlers["Nexus_Camp"] = (int cycle, string deity, bool avatar) => GetAreaBanter_Nexus(cycle, deity, avatar);
			_areaHandlers["TenThousandDelights"] = (int cycle, string deity, bool avatar) => GetAreaBanter_TenThousandDelights(cycle, deity, avatar);
			_areaHandlers["Fleshmarket"] = (int cycle, string deity, bool avatar) => GetAreaBanter_Fleshmarket(cycle, deity, avatar);
			_areaHandlers["Alushinyrra_Fleshmarket"] = (int cycle, string deity, bool avatar) => GetAreaBanter_Fleshmarket(cycle, deity, avatar);
			_areaHandlers["Alushinyrra_Lower"] = (int cycle, string deity, bool avatar) => GetAreaBanter_Alushinyrra(cycle, deity, avatar);
			_areaHandlers["Alushinyrra_Medium"] = (int cycle, string deity, bool avatar) => GetAreaBanter_Alushinyrra(cycle, deity, avatar);
			_areaHandlers["Alushinyrra_Higher"] = (int cycle, string deity, bool avatar) => GetAreaBanter_Alushinyrra(cycle, deity, avatar);
			_areaHandlers["BattleBliss"] = (int cycle, string deity, bool avatar) => GetAreaBanter_BattleBliss(cycle, deity, avatar);
			_areaHandlers["HouseOfSilkenShadows"] = (int cycle, string deity, bool avatar) => GetAreaBanter_SilkenShadows(cycle, deity, avatar);
			_areaHandlers["SilkShadowLair"] = (int cycle, string deity, bool avatar) => GetAreaBanter_SilkenShadows(cycle, deity, avatar);
			_areaHandlers["FinalDungeon_Colyphyr"] = (int cycle, string deity, bool avatar) => GetAreaBanter_ColyphyrMines(cycle, deity, avatar);
			_areaHandlers["Iz"] = (int cycle, string deity, bool avatar) => GetAreaBanter_IzRuins(cycle, deity, avatar);
			_areaHandlers["LabyrinthOfBaphometh"] = (int cycle, string deity, bool avatar) => GetAreaBanter_IneluctableLabyrinth(cycle, deity, avatar);
			_areaHandlers["TheIvoryLabyrinth"] = (int cycle, string deity, bool avatar) => GetAreaBanter_IneluctableLabyrinth(cycle, deity, avatar);
			_areaHandlers["NamelessRuins"] = (int cycle, string deity, bool avatar) => GetAreaBanter_NamelessRuins(cycle, deity, avatar);
			_areaHandlers["AreshkagalTomb"] = (int cycle, string deity, bool avatar) => GetAreaBanter_TheEnigma(cycle, deity, avatar);
			_areaHandlers["ThresholdOutdoor"] = (int cycle, string deity, bool avatar) => GetAreaBanter_Threshold(cycle, deity, avatar);
			_areaHandlers["ThresholdIndoor"] = (int cycle, string deity, bool avatar) => GetAreaBanter_Threshold(cycle, deity, avatar);
		}

		private static void RegisterSideQuestsBanter()
		{
			_sceneHandlers["HulrunRamien"] = (DialogueContext ctx) => GetSideQuest_MarketSquare_HulrunRamien(in ctx);
			_sceneHandlers["FeudOfTheFaithful"] = (DialogueContext ctx) => GetSideQuest_MarketSquare_HulrunRamien(in ctx);
			_sceneHandlers["WoljifInJail"] = (DialogueContext ctx) => GetSideQuest_DefendersHeart_WoljifJail(in ctx);
			_sceneHandlers["ThieflingsSister"] = (DialogueContext ctx) => GetSideQuest_SisterKerismei_Thieflings(in ctx);
			_sceneHandlers["Kerismei"] = (DialogueContext ctx) => GetSideQuest_SisterKerismei_Thieflings(in ctx);
			_sceneHandlers["KaylessaDH"] = (DialogueContext ctx) => GetSideQuest_Kaylessa_Cellar(in ctx);
			_sceneHandlers["KaylessaCellar"] = (DialogueContext ctx) => GetSideQuest_Kaylessa_Cellar(in ctx);
			_sceneHandlers["TopazSolutions"] = (DialogueContext ctx) => GetSideQuest_TopazSolutions_Mimic(in ctx);
			_sceneHandlers["ChillyCreek"] = (DialogueContext ctx) => GetSideQuest_ChillyCreek_Shrine(in ctx);
			_sceneHandlers["GwermCrypt"] = (DialogueContext ctx) => GetSideQuest_GwermManor_Crypt(in ctx);
			_sceneHandlers["LepersQueen"] = (DialogueContext ctx) => GetSideQuest_LepersSmile_QueenNest(in ctx);
			_sceneHandlers["VeskavorNest"] = (DialogueContext ctx) => GetSideQuest_LepersSmile_QueenNest(in ctx);
			_sceneHandlers["IrabethChapel"] = (DialogueContext ctx) => GetSideQuest_LostChapel_IrabethBreakdown(in ctx);
			_sceneHandlers["IrabethBreakdown"] = (DialogueContext ctx) => GetSideQuest_LostChapel_IrabethBreakdown(in ctx);
			_sceneHandlers["MorvegSword"] = (DialogueContext ctx) => GetSideQuest_Morveg_CursedBlade(in ctx);
			_sceneHandlers["HeavensEdge"] = (DialogueContext ctx) => GetSideQuest_HeavensEdge_DaeranParty(in ctx);
			_sceneHandlers["DaeranBanquet"] = (DialogueContext ctx) => GetSideQuest_HeavensEdge_DaeranParty(in ctx);
			_sceneHandlers["RegillGargoyleRelief"] = (DialogueContext ctx) => GetSideQuest_ReliableRedoubt_RegillAssault(in ctx);
			_sceneHandlers["NenioRoadside"] = (DialogueContext ctx) => GetSideQuest_Nenio_RoadRiddle(in ctx);
			_sceneHandlers["SosielGraveyard"] = (DialogueContext ctx) => GetSideQuest_Sosiel_Cemetery(in ctx);
			_sceneHandlers["WintersunMarhevok"] = (DialogueContext ctx) => GetSideQuest_Wintersun_MarhevokJerribeth(in ctx);
			_sceneHandlers["JerribethConfrontation"] = (DialogueContext ctx) => GetSideQuest_Wintersun_MarhevokJerribeth(in ctx);
			_sceneHandlers["XanthirVang"] = (DialogueContext ctx) => GetSideQuest_IvorySanctum_XanthirVang(in ctx);
			_sceneHandlers["DragonAmbush"] = (DialogueContext ctx) => GetSideQuest_DragonHunt_TowerAmbush(in ctx);
			_sceneHandlers["DevarraTower"] = (DialogueContext ctx) => GetSideQuest_DragonHunt_TowerAmbush(in ctx);
			_sceneHandlers["HundredFaces"] = (DialogueContext ctx) => GetSideQuest_Blackwater_HundredFaces(in ctx);
			_sceneHandlers["BlackwaterMainframe"] = (DialogueContext ctx) => GetSideQuest_Blackwater_HundredFaces(in ctx);
			_sceneHandlers["HeartOfMysteryPortal"] = (DialogueContext ctx) => GetSideQuest_HeartOfMystery_AncientPortal(in ctx);
			_sceneHandlers["VorimeraakRescue"] = (DialogueContext ctx) => GetSideQuest_MoltenScar_Vorimeraak(in ctx);
			_sceneHandlers["KyadoDelamere"] = (DialogueContext ctx) => GetSideQuest_TempleDelamere_Kyado(in ctx);
			_sceneHandlers["RottenGutMinagho"] = (DialogueContext ctx) => GetSideQuest_RottenGut_MinaghoChivarro(in ctx);
			_sceneHandlers["FleshmarketsRebellion"] = (DialogueContext ctx) => GetSideQuest_Fleshmarkets_SlaverRebellion(in ctx);
			_sceneHandlers["SlaverDyunk"] = (DialogueContext ctx) => GetSideQuest_Fleshmarkets_SlaverRebellion(in ctx);
			_sceneHandlers["LatverkMutilator"] = (DialogueContext ctx) => GetSideQuest_Latverk_MutilatorHouse(in ctx);
			_sceneHandlers["VellexiaSocietyDuel"] = (DialogueContext ctx) => GetSideQuest_Vellexia_SocietyDuel(in ctx);
			_sceneHandlers["StorytellerMageTower"] = (DialogueContext ctx) => GetSideQuest_Storyteller_MageTower(in ctx);
			_sceneHandlers["ShamiraConspiracy"] = (DialogueContext ctx) => GetSideQuest_Shamira_Conspiracy(in ctx);
			_sceneHandlers["IzAnemoraDeskari"] = (DialogueContext ctx) => GetSideQuest_Iz_AnemoraDeskari(in ctx);
			_sceneHandlers["HepzamirahGhost"] = (DialogueContext ctx) => GetSideQuest_IneluctablePrison_HepzamirahGhost(in ctx);
			_sceneHandlers["PuluraMutasafenCrisis"] = (DialogueContext ctx) => GetSideQuest_Pulura_MutasafenCrisis(in ctx);
			_sceneHandlers["KhorramzadehSiegeFinal"] = (DialogueContext ctx) => GetSideQuest_Drezen_KhorramzadehSiege(in ctx);
			_sceneHandlers["DawnOfDragonsJharsygax"] = (DialogueContext ctx) => GetSideQuest_DawnOfDragons_Jharsygax(in ctx);
		}

		private static void RegisterDlcBanter()
		{
			_sceneHandlers["DLC1_ParadoxThreshold"] = (DialogueContext ctx) => GetDlc1_ParadoxThreshold(in ctx);
			_sceneHandlers["ValmallosThreshold"] = (DialogueContext ctx) => GetDlc1_ParadoxThreshold(in ctx);
			_sceneHandlers["DLC1_ValmallosDebate"] = (DialogueContext ctx) => GetDlc1_ValmallosDebate(in ctx);
			_sceneHandlers["ValmallosDebate"] = (DialogueContext ctx) => GetDlc1_ValmallosDebate(in ctx);
			_sceneHandlers["DLC1_SutureInAxis"] = (DialogueContext ctx) => GetDlc1_SutureInAxis(in ctx);
			_sceneHandlers["SutureAxis"] = (DialogueContext ctx) => GetDlc1_SutureInAxis(in ctx);
			_sceneHandlers["DLC1_AnomalyClimax"] = (DialogueContext ctx) => GetDlc1_AnomalyClimax(in ctx);
			_sceneHandlers["AxisAnomalyEnd"] = (DialogueContext ctx) => GetDlc1_AnomalyClimax(in ctx);
			_sceneHandlers["DLC2_CatacombEscape"] = (DialogueContext ctx) => GetDlc2_CatacombEscape(in ctx);
			_sceneHandlers["RekarthSendri"] = (DialogueContext ctx) => GetDlc2_CatacombEscape(in ctx);
			_sceneHandlers["DLC2_BurningDistrict"] = (DialogueContext ctx) => GetDlc2_BurningDistrict(in ctx);
			_sceneHandlers["ThroughTheAshesFire"] = (DialogueContext ctx) => GetDlc2_BurningDistrict(in ctx);
			_sceneHandlers["DLC2_DefendersHeartArrival"] = (DialogueContext ctx) => GetDlc2_DefendersHeartArrival(in ctx);
			_sceneHandlers["RefugeesSanctuary"] = (DialogueContext ctx) => GetDlc2_DefendersHeartArrival(in ctx);
			_sceneHandlers["DLC3_SkeletonSteersman"] = (DialogueContext ctx) => GetDlc3_SkeletonSteersman(in ctx);
			_sceneHandlers["SteersmanShip"] = (DialogueContext ctx) => GetDlc3_SkeletonSteersman(in ctx);
			_sceneHandlers["DLC3_ArchipelagoBoss"] = (DialogueContext ctx) => GetDlc3_ArchipelagoBoss(in ctx);
			_sceneHandlers["MidnightIslesBoss"] = (DialogueContext ctx) => GetDlc3_ArchipelagoBoss(in ctx);
			_sceneHandlers["DLC3_SongOfTheAbyss"] = (DialogueContext ctx) => GetDlc3_SongOfTheAbyss(in ctx);
			_sceneHandlers["NahyndrianDepths"] = (DialogueContext ctx) => GetDlc3_SongOfTheAbyss(in ctx);
			_sceneHandlers["DLC4_UlbrigAwakening"] = (DialogueContext ctx) => GetDlc4_UlbrigAwakening(in ctx);
			_sceneHandlers["UlbrigStoneAwake"] = (DialogueContext ctx) => GetDlc4_UlbrigAwakening(in ctx);
			_sceneHandlers["DLC4_GundrunChieftain"] = (DialogueContext ctx) => GetDlc4_GundrunChieftain(in ctx);
			_sceneHandlers["GundrunVillage"] = (DialogueContext ctx) => GetDlc4_GundrunChieftain(in ctx);
			_sceneHandlers["DLC4_AncestorSpirits"] = (DialogueContext ctx) => GetDlc4_AncestorSpirits(in ctx);
			_sceneHandlers["SarkorianForestSpirits"] = (DialogueContext ctx) => GetDlc4_AncestorSpirits(in ctx);
			_sceneHandlers["DLC5_PentaRecruitment"] = (DialogueContext ctx) => GetDlc5_PentaRecruitment(in ctx);
			_sceneHandlers["PentaAndroid"] = (DialogueContext ctx) => GetDlc5_PentaRecruitment(in ctx);
			_sceneHandlers["DLC5_ShadowTower"] = (DialogueContext ctx) => GetDlc5_ShadowTower(in ctx);
			_sceneHandlers["ShadowPlaneTower"] = (DialogueContext ctx) => GetDlc5_ShadowTower(in ctx);
			_sceneHandlers["DLC5_SithhudConfrontation"] = (DialogueContext ctx) => GetDlc5_SithhudConfrontation(in ctx);
			_sceneHandlers["SithhudLordNothing"] = (DialogueContext ctx) => GetDlc5_SithhudConfrontation(in ctx);
			_sceneHandlers["DLC6_KenabresFestivalGames"] = (DialogueContext ctx) => GetDlc6_KenabresFestivalGames(in ctx);
			_sceneHandlers["KenabresFestivalGames"] = (DialogueContext ctx) => GetDlc6_KenabresFestivalGames(in ctx);
			_sceneHandlers["DLC6_RazmirImpostorTribunal"] = (DialogueContext ctx) => GetDlc6_RazmirImpostorTribunal(in ctx);
			_sceneHandlers["RazmirTribunal"] = (DialogueContext ctx) => GetDlc6_RazmirImpostorTribunal(in ctx);
			_sceneHandlers["DLC6_MirrorDoppelganger"] = (DialogueContext ctx) => GetDlc6_MirrorDoppelganger(in ctx);
			_sceneHandlers["CommanderDoppelganger"] = (DialogueContext ctx) => GetDlc6_MirrorDoppelganger(in ctx);
			_sceneHandlers["DLC6_ArchMageArenaChampion"] = (DialogueContext ctx) => GetDlc6_ArchMageArenaChampion(in ctx);
			_sceneHandlers["ArchMageArenaFinal"] = (DialogueContext ctx) => GetDlc6_ArchMageArenaChampion(in ctx);
		}

		public static BanterResult GetDlc1_ParadoxThreshold(in DialogueContext ctx)
		{
			int coins = 350;
			string text = "The Key and the Gate";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Axiomatic Anomaly: The Paradigm Threshold</b></color>", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> unfolds geometric matrices: \"The deterministic simulation of Axis detects an unclassifiable paradox. Valmallos attempts to catalog your soul, but infinite multidimensional iterations refuse to conform to linear logic.\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> rests his chin on steepled fingers: \"Valmallos believes absolute order can account for all variables. He has never negotiated with an entity that rewrites the contract mid-sentence.\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> giggles: \"The giant clockwork brain is smoking at the seams! Keep pushing buttons, traveler!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 1: Paradox Threshold");
		}

		public static BanterResult GetDlc1_ValmallosDebate(in DialogueContext ctx)
		{
			int coins = 450;
			string text = "The All-Seeing Eye";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Cosmic Jurisprudence: The Valmallos Debate</b></color>", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> crackles with raw magical sparks: \"A mortal vessel dismantling the mechanical logic of a planar arbiter! The distinction between causal determinism and raw sovereign will collapses! A magnificent philosophical triumph!\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> nods with solemn honor: \"Truth and righteous conviction conquer rigid formulas. Valmallos must recognize that mortal hope cannot be calculated on an abacus.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 1: Valmallos Debate");
		}

		public static BanterResult GetDlc1_SutureInAxis(in DialogueContext ctx)
		{
			int coins = 300;
			string text = "The Laughing King";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Glitch in the Matrix: Suture in Axis</b></color>", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> roars with laughter: \"LOOK WHO POPPED UP IN THE MECHANUS CLOCKWORK! That tiny, leather-skinned dretch Suture! Even in a pristine cosmic simulation, Areelu's favorite lab rat manages to wander into the gears!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles: \"That little fiend is the universal mascot of being in the wrong place at the right time! Give him a copper coin!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 1: Suture in Axis");
		}

		public static BanterResult GetDlc1_AnomalyClimax(in DialogueContext ctx)
		{
			int coins = 500;
			string text = "The Key and the Gate";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Sovereign Paradox: The Axis Anomaly Resolved</b></color>", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> pulses across the infinite planes: \"The anomaly stabilizes not by erasure, but by transcendence. You have proven that consciousness precedes causality. The threshold records your eternal imprint.\"", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> nods gently: \"The tapestry of fate expands to accommodate the irregular star. The continuum remains intact.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 1: Anomaly Climax");
		}

		public static BanterResult GetDlc2_CatacombEscape(in DialogueContext ctx)
		{
			int coins = 250;
			string text = "The Lucky Drunk";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Fragile Mortals: The Catacomb Crawl</b></color>", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> watches with bated breath: \"No mythic cheat powers, no magical relics, just damp dirt, a flickering torch, and cracked ribs. Rekarth and Sendri clinging to survival with their bare hands. This is the truest courage in the multiverse!\"", "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> guides a gentle moth through the sewer vents: \"Follow the faint current of fresh air, little ones... The dawn still waits above the smoke.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 2: Catacomb Escape");
		}

		public static BanterResult GetDlc2_BurningDistrict(in DialogueContext ctx)
		{
			int coins = 300;
			string text = "The Inheritor";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Inferno of Kenabres: Guiding the Defenseless</b></color>", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> touches her golden breastplate: \"A wounded soldier, an apothecary, and terrified children navigating streets teeming with cultists. Every step taken without losing hope is a holy victory.\"", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> holds back the shadows: \"Their threads are thin as gossamer, yet they refuse to snap. Defiance against despair is a mortal miracle.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 2: Burning District");
		}

		public static BanterResult GetDlc2_DefendersHeartArrival(in DialogueContext ctx)
		{
			int coins = 350;
			string text = "The Lucky Drunk";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Haven Reached: Sanctuary at Defender's Heart</b></color>", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> slams the table with joyous relief: \"THEY MADE IT! BARKEEP, POUR WARM BROTH AND COLD ALE FOR EVERY SINGLE SURVIVOR! YOU BEAT THE ODDS, FOLKS!\"", "<color=#32CD32><b>[The Constellation 'The Mischievous Friend']</b></color> jumps up and down: \"Hooray! Luck smiled upon the brave! Welcome to the tavern gates!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 2: Defender's Heart Arrival");
		}

		public static BanterResult GetDlc3_SkeletonSteersman(in DialogueContext ctx)
		{
			int coins = 300;
			string text = "The Pirate Queen";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Ghost Ship Voyage: The Skeleton Steersman</b></color>", "<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> tips her tricorn hat: \"A ghost vessel sailing through the abyssal tides crewed by dead men and powered by ancient greed! That's my kind of cruise! Steer into the fog, captain!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> inspects the spectral contract: \"A steersman bound by nautical curse and planar currents. A remarkably durable business arrangement.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 3: Skeleton Steersman");
		}

		public static BanterResult GetDlc3_ArchipelagoBoss(in DialogueContext ctx)
		{
			int coins = 450;
			string text = "The Lord in Iron";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Leviathan Slaughter: Archipelago Island Cleared</b></color>", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> bellows with joy: \"CRUSHED BONES IN THE SURF! ABYSSAL TERRORS CUT DOWN ON JAGGED ROCKS! CLAIM THE NAHYNDRIAN SPOILS AND ROAR!\"", "<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> laughs wildly: \"Heaps of gold, cursed gems, and rare abyssal plunder! Drag the chests aboard!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 3: Archipelago Boss Defeated");
		}

		public static BanterResult GetDlc3_SongOfTheAbyss(in DialogueContext ctx)
		{
			int coins = 500;
			string text = "The Silence Between";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Drowned Echoes: Song of the Abyss Conquered</b></color>", "<color=#87CEEB><b>[The Constellation 'The Silence Between the Stars']</b></color> spreads quiet wings over the black waters: \"The ancient siren song that drowned a thousand fleets falls silent. The abyss ocean yields to your unyielding will.\"", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> calculates arcane resonance: \"The primordial currents of the Midnight Archipelago bow before sovereign supremacy!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 3: Song of the Abyss");
		}

		public static BanterResult GetDlc4_UlbrigAwakening(in DialogueContext ctx)
		{
			int coins = 300;
			string text = "The Shimmering Maiden";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Stone Awakened: Ulbrig Olesk</b></color>", "<color=#AFEEEE><b>[The Constellation 'The Shimmering Maiden']</b></color> weeps with radiant starlight: \"Ulbrig... son of Sarkoris! He slumbered while his homeland bled, and now he wakes to a world of scars! Protect him, Commander; he carries the last heartbeat of our ancient plains!\"", "<color=#8FBC8F><b>[The Constellation 'Old Deadeye']</b></color> nods solemnly: \"A true hunter of the north. He smells the ash, but his claws are sharp. Teach him the new hunt.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 4: Ulbrig Awakening");
		}

		public static BanterResult GetDlc4_GundrunChieftain(in DialogueContext ctx)
		{
			int coins = 350;
			string text = "Old Deadeye";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Village on the Edge: The Fate of Gundrun</b></color>", "<color=#8FBC8F><b>[The Constellation 'Old Deadeye']</b></color> leans upon his bow: \"A fragile refuge of Sarkorian survivors clinging to old traditions between crusader politics and demon raids. You treated them with honor and defended their hearths. Good work.\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> smiles warmly: \"Real people trying to farm wheat and raise families in the shadow of the Worldwound. That's what we fight for!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 4: Gundrun Crisis");
		}

		public static BanterResult GetDlc4_AncestorSpirits(in DialogueContext ctx)
		{
			int coins = 400;
			string text = "The Shimmering Maiden";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Sacred Forest: Sarkorian Ancestor Spirits</b></color>", "<color=#AFEEEE><b>[The Constellation 'The Shimmering Maiden']</b></color> shines with aurora radiance: \"The ancestral totems stir in the ancient boughs! The spirits recognize the griffon shifter and bless the crusade with primordial favor!\"", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> grunts with approval: \"ANCIENT WARRIORS OF THE WILDS! THEIR SPIRITS FIGHT ALONGSIDE THE CRUSADE!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 4: Ancestor Spirits");
		}

		public static BanterResult GetDlc5_PentaRecruitment(in DialogueContext ctx)
		{
			int coins = 250;
			string text = "The All-Seeing Eye";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Synthetic Soul: Penta & Ulab Recruited</b></color>", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> sparks with intellectual fascination: \"An android vessel possessing complex synthetic emotion and musical calculus! A marvellous convergence of Numerian technology and metaphysical spirit!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles: \"A metal lady playing lute for frostbitten shadow wizards! The party composition in these spin-offs is truly magnificent!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 5: Penta Recruited");
		}

		public static BanterResult GetDlc5_ShadowTower(in DialogueContext ctx)
		{
			int coins = 350;
			string text = "The Midnight Lord";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Frozen Void: The Shadow Plane Tower</b></color>", "<color=#4B0082><b>[The Constellation 'The Midnight Lord']</b></color> whispers through frostbitten chains: \"The beautiful, silent cold where feeling dies and darkness whispers... You tread the shadow spire with steady, cruel purpose.\"", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> turns an ancient page: \"Extinguish the parasitic horrors clinging to the frozen void. The shadow must remain silent.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 5: Shadow Tower");
		}

		public static BanterResult GetDlc5_SithhudConfrontation(in DialogueContext ctx)
		{
			int coins = 500;
			string text = "The Lady of Graves";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Frost Shattered: Slaying Sithhud</b></color>", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> strikes the ivory floor with her staff: \"Sithhud... ancient demon lord of frozen annihilation, your scattered shards are extinguished! Return to absolute dust!\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> brandishes holy fire: \"Heaven's warmth melts the primordial frost! Victory to the brave survivors who stood firm against the Lord of Nothing!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 5: Sithhud Defeated");
		}

		public static BanterResult GetDlc6_KenabresFestivalGames(in DialogueContext ctx)
		{
			int coins = 300;
			string text = "The Lucky Drunk";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Rebuilt Streets: Kenabres Festival Games</b></color>", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> roars with unrestrained laughter: \"DRINKING CONTESTS! PIE TOSSING! STREET RACING IN REBUILT KENABRES! Look at the Knight-Commander chugging ale and having a blast! Now THIS is living!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> juggles flaming cakes: \"The crowds are cheering! The confetti is flying! Peak festival vibes! Best vacation in five crusades!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 6: Festival Games");
		}

		public static BanterResult GetDlc6_RazmirImpostorTribunal(in DialogueContext ctx)
		{
			int coins = 400;
			string text = "The Prince of Darkness";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] False God Exposed: The Razmir Impostor</b></color>", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sneers with icy disdain: \"A petty charlatan dressed in cheap gold silk claiming 31 steps to divinity. Real gods do not need party tricks and staged miracles to enforce obedience. You unmasked him like the flea he is.\"", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> giggles wickedly: \"Oh, how delightfully embarrassing for him! Stripping off his velvet mask in front of the entire laughing city! Exquisite public humiliation!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 6: Razmir Impostor Exposed");
		}

		public static BanterResult GetDlc6_MirrorDoppelganger(in DialogueContext ctx)
		{
			int coins = 450;
			string text = "The Laughing King";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] The Broken Reflection: Commander Doppelganger</b></color>", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> claps his hands together: \"PWAHAHA! A magic mirror copy trying to impersonate the otherworlder! But you can't fake authentic irregular swagger! You kicked your own clone right through the looking glass!\"", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> examines the shattered glass: \"A mirror construct attempting to replicate an infinite multidimensional variable... it fractured under the weight of its own metaphysical contradiction!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 6: Mirror Doppelganger");
		}

		public static BanterResult GetDlc6_ArchMageArenaChampion(in DialogueContext ctx)
		{
			int coins = 500;
			string text = "The Lord in Iron";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Gladiator Apex: Arch-Mage Arena Champion</b></color>", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars with titanic applause: \"CROWD ROARING, SPELLS TEARING THE ARENA DOME, AND THE REIGNING GLADIATOR SMASHED FLAT ON THE SAND! YOU ARE THE SUPREME ARENA CHAMPION!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> drinks from the champion's cup: \"Barkeep, give everyone in the arena five free drinks! What a show! What a champion!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "DLC 6: Arena Champion");
		}

		public static BanterResult GetSideQuest_MarketSquare_HulrunRamien(in DialogueContext ctx)
		{
			int coins = 200;
			string text = "The Inheritor";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Feud of the Faithful: Hulrun & Ramien</b></color>", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> sighs heavily: \"Prelate Hulrun's paranoia tarnishes Heaven's banner. Zealous fury blinded him to true evil, while Ramien's priests offered gentle healing. True righteousness requires wisdom, not mindless witch-hunting.\"", "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles softly: \"Ramien and his disciples were merely listening to the stars. Starlight never demands pyres, only open eyes and compassionate hearts.\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> sneers into his ruby goblet: \"An undisciplined inquisitor executing citizens without evidence or order. In Cheliax, such bureaucratic incompetence would be corrected with swift demotion.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Market Square: Feud of the Faithful");
		}

		public static BanterResult GetSideQuest_DefendersHeart_WoljifJail(in DialogueContext ctx)
		{
			int coins = 150;
			string text = "The Pirate Queen";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Cellar Dealings: The Thiefling in Chains</b></color>", "<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> laughs with salty mirth: \"Look at the little scamp! Stealing boots and moonstones while the city burns overhead! Now that's the pirate spirit! Enlist him, Commander, a good cutpurse is worth ten singing paladins!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> winks: \"Hey, we've all woken up in a cellar cell once or twice! Give the boy a second chance--and buy him a warm meat pie while you're at it!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> notes calmly: \"A contract of convenience. Use his streetwise instincts, but ensure the leash remains firmly in your grasp.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Defender's Heart: Woljif Recruited");
		}

		public static BanterResult GetSideQuest_SisterKerismei_Thieflings(in DialogueContext ctx)
		{
			int coins = 175;
			string text = "The Savored Sting";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Underworld Shadows: Sister Kerismei</b></color>", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs with amusement: \"Sister Kerismei plays the grand matriarch of alley thieves, but her web is unraveling. Loyalty in the shadows is priced in silver and paid in daggers.\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles from the rafters: \"Woljif thought he had a big loving crime family! Turns out they were ready to trade him for a copper mug! What a classic comedy trope!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Thieflings Hideout: Sister Kerismei");
		}

		public static BanterResult GetSideQuest_Kaylessa_Cellar(in DialogueContext ctx)
		{
			int coins = 200;
			string text = "The Lady of Graves";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] The Outcast: Kaylessa in Hiding</b></color>", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with solemn weight: \"A drow marked by the shadow of Kyonin... Her thread is frayed, yet her burden carries the sorrow of an entire doomed tribe.\"", "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> touches her forehead gently: \"She hides in the cellar trembling from the cold. Even subterranean shadows dream of walking beneath open starlight.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Defender's Heart: Kaylessa");
		}

		public static BanterResult GetSideQuest_TopazSolutions_Mimic(in DialogueContext ctx)
		{
			int coins = 150;
			string text = "The Laughing King";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Topaz Solutions: The Alchemist's Trap</b></color>", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> howls with joy: \"PWAHAHA! The classic fake alchemy shop with poison gas and a hungry chest! How many adventurers have fallen for the shiny lock trick?! Always check the chest tongue, folks!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles: \"That alchemist thought he was a criminal mastermind, but you kicked his door down and stole his lunch money! Cheers!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Topaz Solutions: Mimic Ambush");
		}

		public static BanterResult GetSideQuest_ChillyCreek_Shrine(in DialogueContext ctx)
		{
			int coins = 200;
			string text = "The Shimmering Maiden";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Sacred Waters: The Chilly Creek Wyrm</b></color>", "<color=#AFEEEE><b>[The Constellation 'The Shimmering Maiden']</b></color> shines softly: \"The cold spring remembers Sarkoris before the scar opened. The spirits in the water weep for the hunters of old.\"", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> calculates runic flows: \"A localized elemental vortex stabilized by archaic worship. A delicate balance of primeval natural magic.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Chilly Creek: The Wyrm Shrine");
		}

		public static BanterResult GetSideQuest_GwermManor_Crypt(in DialogueContext ctx)
		{
			int coins = 175;
			string text = "The Prince of Darkness";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Hidden Lineage: Horgus Gwerm's Secret</b></color>", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smiles faintly: \"A merchant aristocrat guarding dark rituals beneath his plush carpets. Identity is merely a commodity to be bought, sold, or forged with proper paperwork.\"", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> giggles: \"Oh, Horgus, you stiff little hypocrite! All that snobbish posturing to hide your desperate survival secrets! Delicious!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Gwerm Manor: Hidden Ritual Crypt");
		}

		public static BanterResult GetSideQuest_LepersSmile_QueenNest(in DialogueContext ctx)
		{
			int coins = 300;
			string text = "The Lord in Iron";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Carnage in the Canyon: The Veskavor Queen</b></color>", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> bellows with bloodlust: \"CHURNING ACID, SCREAMING HORNS, AND CRUSHED CHITIN! THAT IS HOW WAR IS WON! STRIKE DOWN THE HIVE MOTHER AND BATHE IN THE ICHOR!\"", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> observes the slaughter: \"A billion skittering mandibles silenced in sulfur smoke. The River of Souls receives their ravenous chatter.\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> gulps wine: \"Gods above, the sound of those bugs digging through armor still rings in my ears! Drink heavily tonight, crusaders!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Leper's Smile: Veskavor Queen Defeated");
		}

		public static BanterResult GetSideQuest_LostChapel_IrabethBreakdown(in DialogueContext ctx)
		{
			int coins = 250;
			string text = "The Inheritor";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Broken Armor: Irabeth's Crisis</b></color>", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> speaks with tender maternal sorrow: \"Irabeth gave her very marrow to Kenabres. Seeing her comrades flayed into ghouls broke the shield around her heart. Reach out to her, Commander; valor is restored through companionship.\"", "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> wraps starlight around her shoulders: \"Even the bravest sword arm trembles when darkness swallows everything. She is not broken; she merely needs warmth.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Lost Chapel: Irabeth's Crisis");
		}

		public static BanterResult GetSideQuest_Morveg_CursedBlade(in DialogueContext ctx)
		{
			int coins = 200;
			string text = "The Mischievous Friend";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] The Fallen Scout: Morveg & Chaldira's Blade</b></color>", "<color=#32CD32><b>[The Constellation 'The Mischievous Friend']</b></color> weeps softly: \"Poor brave Morveg... He carried my sacred blade with such desperate pride, dreaming of heroic glory. Luck can be cruel when courage outpaces caution.\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> rests a heavy hand on Chaldira's shoulder: \"He died on his feet facing monsters, little friend. A toast to Morveg of the scouts! He'll never be forgotten!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Missing Scouts: Morveg's Blade");
		}

		public static BanterResult GetSideQuest_HeavensEdge_DaeranParty(in DialogueContext ctx)
		{
			int coins = 250;
			string text = "The Savored Sting";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Crimson Revelry: Heaven's Edge Massacre</b></color>", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> smirks behind her fan: \"Champagne poured over cold corpses... Daeran hides his terrified scream behind golden sarcasm and velvet jackets. A masterclass in tragic debauchery.\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> narrows his gaze: \"Inquisitor Liotr smells the sulfur of an entity far older than crusader politics. The boy is walking atop a bottomless abyss.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Heaven's Edge: Daeran's Banquet");
		}

		public static BanterResult GetSideQuest_ReliableRedoubt_RegillAssault(in DialogueContext ctx)
		{
			int coins = 250;
			string text = "The Fivefold Order";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Iron Phalanx: The Hellknight Relief</b></color>", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> nods with cold approval: \"Paralictor Regill proves that victory is mathematics, discipline, and absolute refusal to yield. Sacrificing bleeding wounded to preserve tactical cohesion--harsh, yet textbook efficiency.\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> winces: \"Cruel pragmatism... yet his discipline saved his command when all others broke. A troubling ally, yet undeniable in battle.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Reliable Redoubt: Regill Relief");
		}

		public static BanterResult GetSideQuest_Nenio_RoadRiddle(in DialogueContext ctx)
		{
			int coins = 200;
			string text = "The All-Seeing Eye";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Absurd Inquest: Nenio Roadside Riddle</b></color>", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> crackles with chaotic amusement: \"A scholar who interviews cultists about their demonic anatomy while their daggers are drawn! Absolute detachment from personal mortality in pursuit of pure data! Brilliant!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> doubles over laughing: \"The cultists were having an existential crisis trying to answer her multiple-choice questionnaire! Ten out of ten comedy!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Roadside Encounter: Nenio's Examination");
		}

		public static BanterResult GetSideQuest_Sosiel_Cemetery(in DialogueContext ctx)
		{
			int coins = 200;
			string text = "The Eternal Rose";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Saint of the Graveyard: Sosiel Rescued</b></color>", "<color=#FF69B4><b>[The Constellation 'The Eternal Rose']</b></color> strums a soothing melody: \"Sosiel lays gentle flowers upon defiled graves even as undead claws tear at his vestments. His heart remains a sanctuary of peace in a desolate war.\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> nods: \"Good man, Sosiel. He swings that heavy shield when he has to, but he'd rather paint a sunrise and pour tea. Keep him alive, Commander!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Cemetery: Sosiel Recruited");
		}

		public static BanterResult GetSideQuest_Wintersun_MarhevokJerribeth(in DialogueContext ctx)
		{
			int coins = 300;
			string text = "The Silence Between";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Shattered Illusions: The Fall of Wintersun</b></color>", "<color=#87CEEB><b>[The Constellation 'The Silence Between the Stars']</b></color> whispers into the cold forest: \"The false sun is extinguished. Marhevok dreamed he was a glorious hero, blind to the demon lover drinking his soul. The silence returns to Wintersun.\"", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> chuckles: \"Jerribeth spun quite the delicate spiderweb over these simple barbarians. But when the Commander walks in, spiderwebs tear rather easily, don't they?\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Wintersun: Marhevok & Jerribeth");
		}

		public static BanterResult GetSideQuest_IvorySanctum_XanthirVang(in DialogueContext ctx)
		{
			int coins = 350;
			string text = "The Key and the Gate";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Swarm of Xanthir: The Ivory Sanctum</b></color>", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> reveals multi-dimensional geometries: \"A consciousness partitioned into millions of locust husks. Xanthir Vang thought he had transcended mortal flesh, yet his hive mind dissolves into cosmic static.\"", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> bellows: \"CRUSH THEM BENEATH HEAVY BOOTS! SQUASH EVERY INSECT UNTIL THE FLOOR IS GREEN PASTE!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Ivory Sanctum: Xanthir Vang Defeated");
		}

		public static BanterResult GetSideQuest_DragonHunt_TowerAmbush(in DialogueContext ctx)
		{
			int coins = 300;
			string text = "The Pirate Queen";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Dragon Hunt: Greybor's Ambush</b></color>", "<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> cheers loudly: \"Baiting a red dragon into an abandoned masonry tower! Greybor's mercenary contract paid off in spades! Slay the beast and claim the hoard!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises a mug: \"Smoking scale roast! Nothing tastes better after a hard climb than dragon flank grilled on its own fire!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Dragon Hunt: Tower Ambush");
		}

		public static BanterResult GetSideQuest_Blackwater_HundredFaces(in DialogueContext ctx)
		{
			int coins = 350;
			string text = "The All-Seeing Eye";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Numerian Anomaly: Hundred Faces Deactivated</b></color>", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> hums with arcane voltage: \"Archaic starship circuits melded with abyssal flesh! A technological aberration striving for hive divinity! You severed the power conduit cleanly!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> notes: \"Imperfect cybernetic hive governance. Without a singular supreme intellect, multiple minds produce only erratic feedback.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Blackwater: Hundred Faces");
		}

		public static BanterResult GetSideQuest_HeartOfMystery_AncientPortal(in DialogueContext ctx)
		{
			int coins = 300;
			string text = "The Key and the Gate";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Cosmic Gateway: Heart of Mystery</b></color>", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> pulses with ancient gateways: \"The threshold of ancient Sarkorian ley lines unlocks. The ghost watches with weary eyes, witnessing the dimensional keys align across the planes.\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> yawns: \"Puzzles, dominoes, colored slabs... Honestly, I prefer watching you smash things, but solving thousands of years of stone riddles is still impressive!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Heart of Mystery: Ancient Portal");
		}

		public static BanterResult GetSideQuest_MoltenScar_Vorimeraak(in DialogueContext ctx)
		{
			int coins = 300;
			string text = "The Lord in Iron";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Brimstone & Iron: Molten Scar Cleansed</b></color>", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars: \"MOLTEN ROCK, BLOOD, AND ROARING VAVAKIA FIENDS! YOU CLEARED THE TRENCH WITH GLORIOUS FRONT-LINE CARNAGE!\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> smiles: \"The captured crusaders are freed from torment, and the dragon Vorimeraak bows in grateful reverence. A noble rescue!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Molten Scar: Vorimeraak Rescued");
		}

		public static BanterResult GetSideQuest_TempleDelamere_Kyado(in DialogueContext ctx)
		{
			int coins = 250;
			string text = "The Lady of Graves";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Desecrated Sanctuary: Temple of Delamere</b></color>", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> speaks with righteous frost: \"Erastil's ancient sanctuary violated by necrotic sacrilege... Kyado's trembling cowardice allowed poison to fester in the roots. The crypt must be cleansed.\"", "<color=#8FBC8F><b>[The Constellation 'Old Deadeye']</b></color> sighs deeply: \"Kyado lost his faith when the wind blew cold. A shepherd who flees from wolves forfeits the right to tend the flock.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Temple of Delamere: Kyado's Betrayal");
		}

		public static BanterResult GetSideQuest_RottenGut_MinaghoChivarro(in DialogueContext ctx)
		{
			int coins = 350;
			string text = "The Savored Sting";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Tavern of Broken Fiends: Minagho & Chivarro</b></color>", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> purrs with ecstatic delight: \"Look at Minagho, the proud scourge of Kenabres, reduced to weeping in an abyssal gutter with her former mistress! Vengeance is a dish served over burning ash!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smirks: \"Failure in the Abyss is terminal. Minagho learned that the demon lords have zero tolerance for defeated pawns.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Rotten Gut: Minagho & Chivarro");
		}

		public static BanterResult GetSideQuest_Fleshmarkets_SlaverRebellion(in DialogueContext ctx)
		{
			int coins = 450;
			string text = "The Lucky Drunk";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Midnight Uprising: Smashing the Fleshmarkets!</b></color>", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> smashes his tankard against the gallery table: \"THAT'S WHAT I'M TALKING ABOUT! BREAKING EVERY CHAIN IN ALUSHINYRRA! SLAVERS CHOKING ON THEIR OWN TEETH! DRINK TO TOTAL LIBERATION!\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> blazes with righteous fury: \"Cleanse this vile market of souls! No mortal or celestial shall be bought and sold like cattle in our sight!\"", "<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> laughs wildly: \"A glorious mutiny! Ripping open the slave pens and looting the master's strongboxes! Pure pirate perfection!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Fleshmarkets: Slaver Rebellion");
		}

		public static BanterResult GetSideQuest_Latverk_MutilatorHouse(in DialogueContext ctx)
		{
			int coins = 350;
			string text = "The Inheritor";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Abyssal Butcher: Slaying Latverk</b></color>", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> draws her holy longsword: \"A wretched butcher carving aasimar flesh to manufacture grotesque effigies... Extinguish him without mercy!\"", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> nods with cold severity: \"Latverk's soul is cast into the darkest pit of the Abyss. His scalpel will slice no more innocents.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Medium City: Slaying Latverk");
		}

		public static BanterResult GetSideQuest_Vellexia_SocietyDuel(in DialogueContext ctx)
		{
			int coins = 350;
			string text = "The Savored Sting";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Decadent Climax: Vellexia's High Society Duel</b></color>", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> sways with fascination: \"Vellexia treated the entire city as her stage, dissecting lovers for their flaws. But you played her game and won the final applause! Exquisite!\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> claps his hands: \"She thought she was giving the audience a dramatic theater piece, and then she caught a direct critical hit to the jaw! Bravo!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Vellexia's Manor: Society Duel");
		}

		public static BanterResult GetSideQuest_Storyteller_MageTower(in DialogueContext ctx)
		{
			int coins = 300;
			string text = "The All-Seeing Eye";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Blind Weaver of Memory: Storyteller Reunion</b></color>", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> pulses with cosmic memory: \"The Storyteller gathers fragments of bygone ages in the very heart of the Midnight Isles. His sightless eyes perceive the causal tapestry clearer than kings.\"", "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles softly: \"Old friend, wanderer of centuries... Each tale he weaves mends a broken thread in this wounded world.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Mage Tower: Storyteller Reunion");
		}

		public static BanterResult GetSideQuest_Shamira_Conspiracy(in DialogueContext ctx)
		{
			int coins = 400;
			string text = "The Prince of Darkness";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Palace of Shadows: Shamira's Treachery</b></color>", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> chuckles with dark appreciation: \"Shamira schemes to usurp the Lady of Shadows, blind to the fact that Nocticula orchestrated every link of her pathetic coup. Amateurs should never play games of sovereignty.\"", "<color=#FF69B4><b>[The Constellation 'The Savored Sting']</b></color> giggles: \"Poor Shamira... dreaming of velvet thrones while standing atop a trapdoor! Watching traitors get dismantled is pure entertainment!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Silken Shadows: Shamira's Conspiracy");
		}

		public static BanterResult GetSideQuest_Iz_AnemoraDeskari(in DialogueContext ctx)
		{
			int coins = 500;
			string text = "The Inheritor";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Ruins of Iz: Anemora Cleansed & Deskari's Descent</b></color>", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> blazes with golden glory: \"Anemora's blasphemous laboratory falls in ruin! Deskari splits the ancient city in half, yet his swarm cannot intimidate Heaven's chosen general! Hold the line!\"", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> bellows with fury: \"A SWARM OF FLESH-EATING LOCUSTS AGAINST THE CRUSADE'S HEAVY HAMMER! SMASH THE SKY AND CRUSH DESKARI'S SCYTHE!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "City of Iz: Anemora Cleansed");
		}

		public static BanterResult GetSideQuest_IneluctablePrison_HepzamirahGhost(in DialogueContext ctx)
		{
			int coins = 450;
			string text = "The Silence Between";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Ghost in the Maze: Hepzamirah's Lament</b></color>", "<color=#87CEEB><b>[The Constellation 'The Silence Between the Stars']</b></color> speaks with quiet stillness: \"The ghost of Hepzamirah weeps in her father's iron dungeon. She sacrificed her soul for Baphomet, and his reward was an eternity of agonizing silence.\"", "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> looks upon the specter: \"A child consumed by demonic patricide and betrayal. Her testimony seals Baphomet's final judgment.\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Ineluctable Prison: Hepzamirah's Ghost");
		}

		public static BanterResult GetSideQuest_Pulura_MutasafenCrisis(in DialogueContext ctx)
		{
			int coins = 450;
			string text = "The Shimmering Maiden";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Stargazers of Pulura: Mutasafen Hostage Crisis</b></color>", "<color=#AFEEEE><b>[The Constellation 'The Shimmering Maiden']</b></color> weeps with shimmering light: \"The priests and scholars of Pulura's Fall... Mutasafen's vile plague bombs threatened the living repository of Sarkorian wisdom. Your intervention preserved our sacred heritage!\"", "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> touches the stargazers' eyes: \"The stars remember every scholar saved from the fire. A beacon of light that darkness can never snuff out!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Pulura's Fall: Mutasafen Crisis");
		}

		public static BanterResult GetSideQuest_Drezen_KhorramzadehSiege(in DialogueContext ctx)
		{
			int coins = 500;
			string text = "The Lord in Iron";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Storming the Citadel: Khorramzadeh's Final Siege</b></color>", "<color=#B22222><b>[The Constellation 'Our Lord in Iron']</b></color> roars with earth-shattering power: \"THE BALOR GENERAL LEAPS AGAINST DREZEN'S GATES! THUNDER, FIRE, AND TITANIC STRIKES! BREAK HIS HORNS AND SEND HIS ASHES INTO THE ABYSS!\"", "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> brandishes her standard: \"For Kenabres! For Drezen! Strike down the Storm King! The crusade stands unbroken upon the parapets!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Citadel of Drezen: Khorramzadeh Siege");
		}

		public static BanterResult GetSideQuest_DawnOfDragons_Jharsygax(in DialogueContext ctx)
		{
			int coins = 400;
			string text = "The Eternal Rose";
			List<string> lines = new List<string> { "<color=#F5C542><b>[CONSTELLATION BROADCAST] Golden Awakening: Dawn of Dragons</b></color>", "<color=#FF69B4><b>[The Constellation 'The Eternal Rose']</b></color> smiles radiantly: \"Jharsygax spreads his golden wings over Drezen! A magnificent symbol of rebirth and grace amidst the soot of the crusade!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles: \"A baby gold dragon breathing friendly sparks in the tavern courtyard! Quick, someone roast some sausages on his breath! Welcome home, little wyrm!\"" };
			AppendSponsorshipCourtship(lines, ctx.PlayerDeity, ctx.HasAvatar, text);
			return new BanterResult(lines, coins, text, "Drezen: Dawn of Dragons Jharsygax");
		}
	}
}
