using System.Linq;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Dialogue;

internal static class Port62CampHubs
{
	private const string Folder = "Companions/CompanionDialogues/";

	internal static readonly Port62Graft[] Grafts = BuildTable();

	private static Port62Graft Hub(string answer, string companion, string list, GraftBeat beat)
	{
		return new Port62Graft
		{
			Answer = answer,
			Scene = "Companions/CompanionDialogues/" + companion,
			ListGuid = list,
			Beat = beat,
			ExpCr = 3
		};
	}

	private static Port62Graft[] BuildTable()
	{
		Port62Graft port62Graft = Paid(Hub("IsekaiEmberCampSweetCake", "Ember", "f2a35965e9bc601449498bd022b04d9d", DialogueBeats.EmberHub), "EmberCampSweetCake", 300, "The Dawnflower");
		port62Graft.Buff = "EmberSparkBuff";
		port62Graft.BuffMinutes = 1440;
		return new Port62Graft[24]
		{
			Paid(Hub("IsekaiSeelahCampFestival", "Seelah", "417fa384f3250634bb71859fbc913453", DialogueBeats.SeelahHub), "SeelahCampFestival", 250, "The Lucky Drunk"),
			Gated(Hub("IsekaiSeelahCampHero", "Seelah", "417fa384f3250634bb71859fbc913453", DialogueBeats.SeelahHub), "HeroProficiencies", 3, AlignmentShiftDirection.Good),
			Hub("IsekaiCamelliaCampTalisman", "Camelia", "9497b573bde03724c8e08090509a1774", DialogueBeats.CamelliaHub),
			Gated(Hub("IsekaiCamelliaCampOverlord", "Camelia", "9497b573bde03724c8e08090509a1774", DialogueBeats.CamelliaHub), "OverlordProficiencies", 4),
			Paid(Hub("IsekaiLannCampComedy", "Lann", "66385ad77fa743e4bb1234078dbd804c", DialogueBeats.LannHub), "LannCampComedy", 250, "The Laughing King"),
			Gated(Hub("IsekaiLannCampMartialKata", "Lann", "66385ad77fa743e4bb1234078dbd804c", DialogueBeats.LannHub), "MartialGodProficiencies", 4),
			Paid(Hub("IsekaiWenduagCampGourmet", "Wenduag", "ced27e744d2dded40bbb5adf17816dbb", DialogueBeats.WenduagHub), "WenduagCampGourmet", 300, "The Dark Prince"),
			Gated(Hub("IsekaiWenduagCampMastermind", "Wenduag", "ced27e744d2dded40bbb5adf17816dbb", DialogueBeats.WenduagHub), "MastermindProficiencies", 4),
			Paid(Hub("IsekaiWoljifCampFranchise", "Woljif", "e41585da330233143b34ef64d7d62d69", DialogueBeats.WoljifHub), "WoljifCampFranchise", 300, "The Laughing King", 250),
			Gated(Hub("IsekaiWoljifCampSlime", "Woljif", "e41585da330233143b34ef64d7d62d69", DialogueBeats.WoljifHub), "DevourerProficiencies", 4),
			Paid(Hub("IsekaiDaeranCampLuxury", "Daeran", "4d978cbd2aa780d46874255282039f3f", DialogueBeats.DaeranHub), "DaeranCampLuxury", 300, "The Lucky Drunk"),
			Gated(Hub("IsekaiDaeranCampGodEmperor", "Daeran", "4d978cbd2aa780d46874255282039f3f", DialogueBeats.DaeranHub), "GodEmperorProficiencies", 4),
			port62Graft,
			Gated(Hub("IsekaiEmberCampHero", "Ember", "f2a35965e9bc601449498bd022b04d9d", DialogueBeats.EmberHub), "HeroProficiencies", 3, AlignmentShiftDirection.Good),
			Paid(Hub("IsekaiNenioCampPeerReview", "Nenio", "9b38499627b7eda4f86d959f3f222834", DialogueBeats.NenioHub), "NenioCampPeerReview", 300, "The Hermit"),
			Gated(Hub("IsekaiNenioCampMastermind", "Nenio", "9b38499627b7eda4f86d959f3f222834", DialogueBeats.NenioHub), "MastermindProficiencies", 4),
			Paid(Hub("IsekaiSosielCampPerspective", "Sosiel", "129b55b8b5d50974f84f7c607d894fd0", DialogueBeats.SosielHub), "SosielCampPerspective", 300, "The Song of the Spheres"),
			Gated(Hub("IsekaiSosielCampMartial", "Sosiel", "129b55b8b5d50974f84f7c607d894fd0", DialogueBeats.SosielHub), "MartialGodProficiencies", 4),
			Paid(Hub("IsekaiRegillCampCorporate", "Regill", "2366a8db6481070439fee222c0c52e45", DialogueBeats.RegillHub), "RegillCampCorporate", 300, "The Grand Arbiter"),
			Gated(Hub("IsekaiRegillCampOverlord", "Regill", "2366a8db6481070439fee222c0c52e45", DialogueBeats.RegillHub), "OverlordProficiencies", 4),
			Paid(Hub("IsekaiArueshalaeCampLucid", "Arueshalae", "03ebad9587cbea0438d901a0f8df44f1", DialogueBeats.ArueshalaeHub), "ArueshalaeCampLucid", 300, "The Song of the Spheres"),
			Gated(Hub("IsekaiArueshalaeCampHero", "Arueshalae", "03ebad9587cbea0438d901a0f8df44f1", DialogueBeats.ArueshalaeHub), "HeroProficiencies", 3, AlignmentShiftDirection.Good),
			Paid(Hub("IsekaiGreyborCampContract", "Grimbor", "174d6c94b6725f44aad1d2a76993a926", DialogueBeats.GreyborHub), "GreyborCampContract", 300, "The Dark Prince", 250),
			Gated(Hub("IsekaiGreyborCampMartial", "Grimbor", "174d6c94b6725f44aad1d2a76993a926", DialogueBeats.GreyborHub), "MartialGodProficiencies", 4)
		};
		static Port62Graft Gated(Port62Graft g, string gate, int cr, AlignmentShiftDirection shift = AlignmentShiftDirection.TrueNeutral)
		{
			g.Gate = gate;
			g.ExpCr = cr;
			g.Shift = shift;
			return g;
		}
		static Port62Graft Paid(Port62Graft g, string key, int coins, string sponsor, int gold = 0)
		{
			g.ClaimKey = "Port62:" + key;
			g.Coins = coins;
			g.SixTwoSponsor = sponsor;
			g.Gold = gold;
			return g;
		}
	}

	internal static Port62Graft Find(string answer)
	{
		return Grafts.FirstOrDefault((Port62Graft g) => g.Answer == answer);
	}

	public static void Add()
	{
		Port("IsekaiSeelahCampFestival", "(Isekai Protagonist) [Otherworld Festivals & Street Food] \"Seelah, back in my world, we had midsummer festivals where folks dressed in full armor just to eat greasy fried street snacks, drink icy cider, and watch fireworks light up the night sky.\"", "{n}Seelah's eyes light up with unvarnished joy, a wide, radiant grin spreading across her face as she leans forward with hearty laughter.{/n} \"Fried street snacks and sky-fire?! Now that sounds like my kind of holiday! Here I thought your realm was all metal carriages and solemn glass towers! Promise me this, Commander: once we kick the demons back into the Abyss and reclaim Drezen, you're teaching the citadel cooks how to make those fried treats!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color>: <i>\"Fried food and festival fireworks! Cayden approves this doctrine with flying colors!\"</i>"));
		Port("IsekaiSeelahCampHero", "(Hero) [Good] [Radiance of Hope] \"A paladin's smile is brighter than any sun, Seelah. When fear chills the camp, seeing your laughter reminds everyone why we fight.\"", "{n}Seelah blushes slightly beneath her armor, rubbing the back of her neck with an embarrassed but deeply touched chuckle.{/n} \"Aw, stop it, you're making me blush in broad daylight! But thank you, Commander. Having someone like you watching our backs makes it a whole lot easier to keep smiling.\"");
		Port("IsekaiCamelliaCampTalisman", "(Isekai Protagonist) [Otherworlder's Appraisal: Cursed Relics] \"You keep caressing that necklace, Lady Camellia. In my world, antique jewelry that radiates spiritual static usually comes with an exorcism hotline.\"", "{n}Camellia's aristocratic smile freezes for a fraction of a second, her fingers lingering over the polished stone with delicate, practiced elegance.{/n} \"How remarkably fanciful. An 'exorcism hotline'? My spirits require no such vulgar meddling, stranger. They are refined, ancient, and quite... particular. Do mind your own curiosities, lest you uncover matters far beyond your delicate sensibilities.\"");
		Port("IsekaiCamelliaCampOverlord", "(Overlord) [Gaze of Dominion] \"You clutch that trinket like a thief concealing stolen sweets. Remember whose shadow covers this camp, girl.\"", "{n}A shiver runs down Camellia's spine, her breath catching slightly as a flicker of genuine apprehension darkens her eyes before she dips into a stiff, obedient curtsy.{/n} \"Of course... my lord. Your authority is unquestioned.\"");
		Port("IsekaiLannCampComedy", "(Isekai Protagonist) [Deadpan Appreciation] \"Lann, your dry humor would kill at an Otherworld comedy cellar. We call that 'deadpan self-deprecating satire'--people pay good money for sets like yours.\"", "{n}Lann blinks in mock astonishment, carefully checking both hands before letting out a dry, raspy chuckle.{/n} \"People pay money for someone complaining about their awful lifespan and scaly skin? Truly, your world is a paradise of questionable tastes. Maybe I picked the wrong profession--Chief Executive Stand-Up Mongrel has a nice ring to it.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color>: <i>\"The mongrel's comedic timing is second to none! Five stars!\"</i>"));
		Port("IsekaiLannCampMartialKata", "(Martial God) [Low-Stance Archery Kata] \"Your reptilian half lowers your center of gravity significantly. Let me show you an archer's root-stance that channels earth ki directly into your draw weight.\"", "{n}Lann mirrors your footing, his yellow eyes widening as his composite bow bends with startling ease and stability.{/n} \"Whoa... the tension on my shoulder blade just dropped by half. That's bizarrely effective. Remind me never to challenge you to an arm-wrestling contest, Commander.\"");
		Port("IsekaiWenduagCampGourmet", "(Isekai Protagonist) [Apex Predator's Cuisine] \"Wenduag, in my world, the greatest hunters don't just chew raw gristle; they roast monster meat with crushed herbs and smoked fat. True apex predators savor the feast.\"", "{n}Wenduag tilts her head, her slit pupils narrowing with predatory intrigue as she inhales the savory spice aroma drifting from your mess kit.{/n} \"A hunter who masters fire and seasoning as well as the bow... Interesting. I thought surface-dwellers ate only weak, soft mush. If your spices sharpen my claws and fortify my blood, I will accept your culinary instruction, master.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color>: <i>\"Refining primal savagery into aristocratic indulgence. Excellent form.\"</i>"));
		Port("IsekaiWenduagCampMastermind", "(Mastermind) [Calculated Predation] \"Ruthlessness without tactical foresight is merely wasted kinetic force, Wenduag. An apex predator plans the ambush three moves ahead.\"", "{n}Wenduag bows her head, her fangs clicking in respectful agreement.{/n} \"Your intellect is as sharp as a poisoned arrowhead. Teach me how to see through their stratagems.\"");
		Port("IsekaiWoljifCampFranchise", "(Isekai Protagonist) [Corporate Syndicate Franchising] \"Woljif, street thievery has razor-thin margins and high mortality. In my world, syndicates use supply chain logistics and franchise licensing. Ever heard of wholesale distribution?\"", "{n}Woljif's jaw drops, his tail swishing like a metronome as he leans in, his amber eyes sparkling with raw entrepreneurial greed.{/n} \"Franchise licensing?! Wholesale distribution?! Boss, every time you open your mouth, gold coins practically rain from the ceiling! You gotta write this down! If we turn the Thieflings into a legitimate logistics cartel, Irabeth won't even be able to tax us!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color>: <i>\"Teaching the street rogue corporate monopolies! Absolutely scandalous!\"</i>"));
		Port("IsekaiWoljifCampSlime", "(Slime) [Acidic Vault Cracking] \"A micro-secretion of acidic fluid dissolves iron lock pins in four seconds without making a sound. Observe.\"", "{n}Woljif stares in sheer awe as a rusted lock dissolves into vapor with a faint hiss.{/n} \"No lockpicks?! Just living acid on demand?! Boss, you are a walking master thief miracle!\"");
		Port("IsekaiDaeranCampLuxury", "(Isekai Protagonist) [Decadence in War Zones] \"Daeran, your dedication to drinking fine chilled vintage while dodging catapult boulders is almost inspiring. Do you also critique the mud texture while fleeing gargoyles?\"", "{n}Daeran bursts into delighted, theatrical applause, taking an elegant sip from his silver chalice before raising it in a mock toast.{/n} \"Naturally! If one must perish in this wretched crusade, doing so covered in inferior mud while drinking vinegar is a tragedy beyond forgiveness. I see you appreciate the finer nuances of battlefield aesthetics, my dear Commander!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color>: <i>\"Wine tastes sweetest when the world is burning! Cheers!\"</i>"));
		Port("IsekaiDaeranCampGodEmperor", "(God Emperor) [Imperial Aristocracy] \"A true sovereign commands realities, Count Arendae; you merely purchase amusements. Yet your sharp tongue pleases the court.\"", "{n}Daeran smirks, executing a sweeping, exaggerated bow.{/n} \"At your service, Your Imperial Magnificence. Do continue conquering; it makes for splendid dinner conversation.\"");
		Port("IsekaiEmberCampSweetCake", "(Isekai Protagonist) [Honey-Glazed Pastry] \"Here, Ember. I baked this honey-glazed cake using an Otherworld recipe. In the realm I hail from, people who hold onto kindness through hardship are revered as saints.\"", "{n}Ember's soot-streaked face breaks into a radiant, childlike smile as she takes the pastry in her small, scarred hands, a soft tear tracing down her cheek.{/n} \"It smells like warm mornings and peaceful dreams... Thank you! You're so kind to everyone, even when the world is angry and hurt. I'll share half with Soot!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The Everbloom']</b></color>: <i>\"A gentle hand extended to an innocent soul. The sacred starlight shines upon you.\"</i>"));
		Port("IsekaiEmberCampHero", "(Hero) [Good] [Vow of Protection] \"Your warmth reaches places iron swords could never touch, Ember. I will make sure no demon or inquisitor ever snuffs out your light.\"", "{n}Ember hugs your arm tightly, her quiet flame flickering with joyful golden warmth.{/n} \"I'm not afraid. As long as you're here, the dark doesn't feel so big anymore.\"");
		Port("IsekaiNenioCampPeerReview", "(Isekai Protagonist) [Academic Peer Review Protocol] \"Nenio, have you considered submitting your encyclopedia for peer review? Back home, if you publish bold theorems without citations, twelve angry department heads duel you with red ink.\"", "{n}Nenio gasps in pure intellectual scandal, her ears flattening back in utter indignation before her quill begins furiously scratching across parchment.{/n} \"Peer review?! By lesser intellects with inferior cranial capacity?! Preposterous! My axioms are self-evident empirical absolute truths! However... the concept of an organized academic duel using colored ink warrants extensive taxonomic documentation! Proceed with your testimony, specimen!\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"Academic bureaucracy crossing dimensional thresholds. Fascinating scholarly friction.\"</i>"));
		Port("IsekaiNenioCampMastermind", "(Mastermind) [Cognitive Defragmentation] \"Your empirical observations are encyclopedic, Nenio, but your cognitive memory cache suffers from severe indexing fragmentation. Adopt my hexadecimal filing matrix.\"", "{n}Nenio freezes, staring at your diagram with bulging eyes before letting out a high-pitched squeak of comprehension.{/n} \"Hexadecimal categorization matrix?! My data retrieval latency will drop by sixty-four percent! Extraordinary!\"");
		Port("IsekaiSosielCampPerspective", "(Isekai Protagonist) [Linear Perspective & Optical Realism] \"Sosiel, capturing light requires understanding optical geometry. In my world, Renaissance painters revolutionized art by using vanishing points and chromatic gradients.\"", "{n}Sosiel lowers his brush, his gentle eyes widening as he studies the geometric sketch you draw in the corner of his easel.{/n} \"A vanishing point on the horizon line... so that every angle recedes toward infinity? Sweet Shelyn, the illusion of three-dimensional depth is breathtaking! You see the world not merely as pigment and canvas, but as living geometry. Thank you, Commander; this changes everything.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color>: <i>\"Art and celestial geometry intertwined. A beautiful stroke of inspiration!\"</i>"));
		Port("IsekaiSosielCampMartial", "(Martial God) [Breathing of the Brush] \"The brush stroke and the glaive thrust require the exact same foundation: absolute stillness of breath before instantaneous motion.\"", "{n}Sosiel nods in quiet realization, steadying his grip as his brush touches the canvas with newfound precision.{/n} \"Stillness before motion... Yes. I can feel the tension leave my shoulder. Remarkable advice, Commander.\"");
		Port("IsekaiRegillCampCorporate", "(Isekai Protagonist) [Corporate Supply Chain Efficiency] \"Regill, have you ever considered corporate executive logistics? In my world, directors who can ruthlessly optimize shift schedules and eliminate supply waste make seven figures.\"", "{n}Regill slowly turns his slate, his cold amber eyes piercing yours with unblinking scrutiny.{/n} \"Financial compensation is irrelevant. Waste is treason against operational victory. If your world recognizes that sentiment and disciplines inefficiency without sentimental hesitation, then its governance possesses merit. Return to your duties, Commander; our frontline rotation begins in eight minutes.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"A mutual appreciation for unyielding discipline and zero wasted motion.\"</i>"));
		Port("IsekaiRegillCampOverlord", "(Overlord) [Absolute Order] \"Your Order of the Godclaw builds a sturdy foundation of obedience, Paralictor. Keep your ranks sharp; sovereign standards demand perfection.\"", "{n}Regill renders a crisp, immaculate Chelish salute.{/n} \"The Order does not tolerate deficiency, my lord. We stand ready.\"");
		Port("IsekaiArueshalaeCampLucid", "(Isekai Protagonist) [Psychology of Lucid Dreaming] \"Arueshalae, dreams are proof of a soul's autonomy. In my world, scholars write entire libraries about lucid dreaming: the realization that within your mindscape, you have absolute power to rewrite your nature.\"", "{n}Arueshalae's eyes glisten like starlight on water, a soft gasp escaping her lips as she presses both hands against her heart.{/n} \"'Absolute power to rewrite your nature'... You speak of dreams not as fleeting illusions, but as sanctuaries of true self-determination. Whenever darkness threatens to drown me, I will remember your words across the stars.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color>: <i>\"Desna's starlight dances in her heart. Dreams transcend all darkness!\"</i>"));
		Port("IsekaiArueshalaeCampHero", "(Hero) [Good] [Every Step is a Triumph] \"Every single step you take away from the Abyss is greater than slaying a demon lord. Keep walking forward; I'll always have your back.\"", "{n}Arueshalae smiles through a shimmer of grateful tears, nodding with quiet, unwavering resolve.{/n} \"With you walking beside me, Commander, I know I will never falter.\"");
		Port("IsekaiGreyborCampContract", "(Isekai Protagonist) [Security Contractor Retainers & Hazard Multipliers] \"Greybor, what are your standard freelance consulting rates? In my world, top-tier private defense contractors charge monthly retainers plus hazardous assignment multipliers.\"", "{n}Greybor puffs slowly on his pipe, an appreciative glint appearing in his heavy-lidded eyes as he blows a neat smoke ring.{/n} \"Hazard multipliers and monthly retainers? Now you're speaking the language of true professionals, Commander. Most crusaders try to pay in holy blessings and patriotic speeches. You understand that when axes swing, coin talks. Keep those retainers coming, and my blades are yours to the end.\"", ConstellationText.Localized("Constellation.Port62.SponsorLine", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color>: <i>\"Gold and iron seal the pact. Professional excellence rewarded.\"</i>"));
		Port("IsekaiGreyborCampMartial", "(Martial God) [Dual-Axe Rotational Torque] \"Dual-wielding heavy handaxes requires exceptional wrist tendon conditioning and continuous rotational momentum. Respectable balance, dwarf.\"", "{n}Greybor grunts in genuine professional respect, testing the weight of his left axe with a crisp, fluid snap.{/n} \"You know your weaponry. Few commanders recognize the torque behind a short backswing. Honored to stand in your company.\"");
	}

	private static void Port(string name, string answerText, string replyText, string line = null)
	{
		Port62Graft graft = Find(name);
		string text = Port62Grafting.Problem(graft);
		if (text != null)
		{
			Main.IsekaiContext.Logger.LogError("Port62CampHubs: '" + name + "' " + text + ", skipping.");
			return;
		}
		Condition[] array = Port62Grafting.Conditions(graft);
		if (array == null)
		{
			Main.IsekaiContext.Logger.LogWarning("Port62CampHubs: the gate of " + name + " could not be built, skipping.");
			return;
		}
		GraftedAnswer.Named(name).At(graft.Beat).Says(answerText)
			.ShownWhen(array)
			.OnSelect(Port62Grafting.OnSelect(graft, line))
			.Configure(delegate(BlueprintAnswer bp)
			{
				Port62Grafting.Configure(graft, bp);
			})
			.RepliedToBy(name + "Reply", replyText)
			.WithSpeakerlessTwin(name + "ReplyNoSpeaker")
			.ThenBackToTheList()
			.Graft();
	}
}
