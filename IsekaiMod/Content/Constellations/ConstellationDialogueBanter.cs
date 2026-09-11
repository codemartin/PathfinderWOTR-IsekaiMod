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

		private static readonly Random _rng = new Random();

		public static List<string> GetAlignmentBanter(AlignmentShiftDirection shift, string answerText, out int coinsAwarded, out string primarySponsor)
		{
			List<string> list = new List<string>();
			coinsAwarded = 0;
			primarySponsor = null;
			if (!string.IsNullOrEmpty(answerText))
			{
				string text = answerText.ToLower();
				if (text.Contains("isekai") || text.Contains("martial god") || text.Contains("overlord") || text.Contains("slime") || text.Contains("devourer") || text.Contains("shadow monarch") || text.Contains("mastermind") || text.Contains("villain") || text.Contains("hero") || text.Contains("god emperor") || text.Contains("radiance") || (text.Contains("lann") && text.Contains("wenduag")) || text.Contains("wardstone") || text.Contains("krebus"))
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
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> falls over laughing: \"Areelu looks like she swallowed a lemon! Out of four hundred crusades, no one has ever done THAT! CLIP THAT!\"");
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
			else
			{
				coinsAwarded = 300;
				primarySponsor = "The Lucky Drunk";
				list.Add("<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> slams his fists on the table: \"CLIP THAT! That was pure main-character energy right there! Barkeep, pour a double for the champion!\"");
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> chuckles warmly: \"The director's cut is completely off the rails and I love every single second of it! Tossing coins into the tip jar!\"");
				list.Add("<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> rotates across spatial facets: \"Anomalous causal vector executed with 99.8% subjective confidence. The multiversal broadcast records the event.\"");
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
				list.Add("<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> whispers playfully: \"Keep smiling, hero. Out of all the iterations we've watched, nobody ever makes choices as weird as yours! Keep the cameras rolling!\"");
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

		public static List<string> GetSlimeRevealBanter()
		{
			return new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CONSTELLATION DISCOVERY] The True Nature of the Slime is Witnessed!</b></color>", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> laughs hysterically: \"WAIT! DID YOU SEE THAT?! The mortal is a giant sentient jelly! Look at the paladin's face! She thinks her holy crusade is being led by blue gelatin! PWAHAHAHA!\"", "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles: \"Hey, if it can hold a flagon of ale and dissolve a glabrezu, it's a drinking buddy in my book! Just... don't melt my tankard, kid!\"", "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smirks coldly: \"An amorphous heteromorphic predator concealing itself as a mortal crusader. Deceptive, highly resilient, and distinctly entertaining to observe.\"", "<color=#9932CC><b>[The Constellation 'The All-Seeing Eye']</b></color> whispers in dual voices: \"A fluid cellular matrix immune to physical cleavage. Truly fascinating biological transmutation.\"", "<color=#F5C542><b>=======================================================</b></color>" };
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
			return new List<string> { "<color=#F5C542><b>=======================================================</b></color>", "<color=#F5C542><b>[CONSTELLATION SOVEREIGN RETURN] The Shattered Loop Acknowledged!</b></color>", "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> revolves in crystalline harmony: \"The causal barrier remains broken. Subject transmigrated by voluntary will rather than recursive compulsion. Shard of the Shattered Loop manifested.\"", "<color=#FF8C00><b>[The Constellation 'The Laughing King']</b></color> laughs with unrestrained glee: \"You actually came back! You broke out of the cosmic fishbowl and then jumped right back into the water! Absolute legend! Take these coins, you earned them!\"", "<color=#20B2AA><b>[The Constellation 'The Pirate Queen']</b></color> tips her tricorn: \"A captain who sails the outer dark and chooses to visit our waters once more? Now that is the kind of reckless sovereign I respect!\"", "<color=#F5C542><b>=======================================================</b></color>" };
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
	}
}
