using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker.AreaLogic.Etudes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue;

internal class SubclassDialogueReactions
{
	internal const string GeneralAnswerGate = "IsekaiProficiencies";

	internal static string GateFactFor(string proficiencyFactName)
	{
		if (!(proficiencyFactName == "IsekaiProficiencies"))
		{
			return proficiencyFactName;
		}
		return "PlotArmor";
	}

	private static Condition AnswerGate(string proficiencyFactName)
	{
		if (GateFactFor(proficiencyFactName) == "PlotArmor")
		{
			return ExtensionMethods.PlotArmorCondition();
		}
		BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, proficiencyFactName);
		if (modBlueprint == null)
		{
			return null;
		}
		return new HasFact
		{
			Unit = new PlayerCharacter(),
			m_Fact = modBlueprint.ToReference<BlueprintUnitFactReference>()
		};
	}

	public static void Add()
	{
		AddHulrunReactions();
		AddMinaghoReactions();
		AddDefendersHeartReactions();
		AddWelcomeDialogueReactions();
		AddMeetCameliaReactions();
		AddMeetSeelahAneviaReactions();
		AddMeetLannReactions();
		AddWardstoneMythicReactions();
		AddDesnaTempleNatureCommunion();
		AddQueenGalfreyWarCampReactions();
		AddRegillHellknightReactions();
		AddZachariusLostChapelReactions();
		AddDrezenCitadelProclamationReactions();
		AddAreeluLabLoopTruthReactions();
		AddBaphometConfrontationReactions();
		AddNocticulaAudienceReactions();
		AddDivineSummitGoddessesReactions();
		AddThresholdGrandFinalEndingReactions();
		AddLepersSmileBookEventReactions();
		AddAngelMythicBookEventReactions();
	}

	private static void AddHulrunReactions()
	{
		BlueprintCue dontRememberCue = BlueprintTools.GetBlueprint<BlueprintCue>("ba9c82193a32275408973a8aebdb3a6d");
		BlueprintEtude dontRememberEtude = BlueprintTools.GetBlueprint<BlueprintEtude>("d6c6161d2cf0ac44786f9df67fca5ce9");
		BlueprintUnit hulrunUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("77fbcde261cd4396bba5d9fe0b2c57c6");
		if (dontRememberCue != null)
		{
			AddArchetypeHulrunAnswer("IsekaiHulrunPhantomBlade", "(Martial God) \"Lower that halberd, Prelate. I just woke up in this square, and waving polearms at recovering patients is hardly proper festival etiquette.\"", "{n}Hulrun's brows knit at your flippant tone.{/n} \"Woke up here? You were carried into this square half dead, stranger. Where were you before that? Who brought you to Kenabres?\" {n}You reach for the answer, but your memory gives you nothing.{/n}", "PhantomBladeProficiencies");
			AddArchetypeHulrunAnswer("IsekaiHulrunGodEmperor", "(God Emperor) \"You speak to an emperor whose ascension will eclipse the heavens. Lower your halberd before your sovereign, Prelate, and remember your place.\"", "{n}Hulrun stumbles backward half a pace as a radiant, suffocating pressure radiates from your mantle.{/n} \"What madness is this... an emperor? In Kenabres? Then name your realm, your Majesty.\" {n}You open your mouth, but no name comes to you.{/n}", "GodEmperorProficiencies");
			AddArchetypeHulrunAnswer("IsekaiHulrunOverlord", "(Overlord) \"You dare interrogate an Overlord, mortal? Be grateful my unfathomable mercy permits your insignificant spark of life to continue burning.\"", "{n}Hulrun shudders as a cold, necrotic chill sweeps across the stones of the square.{/n} \"Such insolence! By Iomedae's light, you speak like a tyrant from some forgotten age. Which land do you claim to rule?\" {n}The answer should be obvious, yet your memory offers only fog.{/n}", "OverlordProficiencies");
			AddArchetypeHulrunAnswer("IsekaiHulrunDevourer", "(Slime) \"Easy with the polearm, sir! I just woke up starving after that terrible carriage accident. I came to the square looking for festival skewers and pastries, not an inquisition.\"", "{n}Hulrun glares at you with stern suspicion.{/n} \"Starving? You were dragged in with a wound in your chest and you think of pastries? A carriage, you say. Coming from where?\" {n}You try to recall the road, but nothing comes.{/n}", "DevourerProficiencies");
			AddArchetypeHulrunAnswer("IsekaiHulrunShadowMonarch", "(Shadow Monarch) \"My pulse is steady and my memory is clearing, Prelate. The only shadows following me are the ones your festival banners cast on the cobblestones.\"", "{n}The shadows beneath Hulrun's boots seem to go still as you meet his gaze, and the veteran pauses.{/n} \"You speak quietly, yet there is an unnatural coldness in your eyes. Clearing, is it? Then tell me where you came from.\" {n}You search for the answer, but it slips away.{/n}", "ShadowMonarchProficiencies");
			AddArchetypeHulrunAnswer("IsekaiHulrunHero", "(Hero) \"I came here to stand with the protectors of this city, not to cause trouble. Lower your halberd, Prelate. We should be celebrating with the townsfolk, not drawing steel.\"", "{n}Hulrun sighs heavily, lowering his halberd by a fraction of an inch.{/n} \"Idealistic words. Every young recruit arrives in Kenabres believing courage alone will win this crusade. Which order sent you, then? Who is your commander?\" {n}You try to answer, but the names will not come.{/n}", "HeroProficiencies");
			AddArchetypeHulrunAnswer("IsekaiHulrunMastermind", "(Mastermind) \"Interrogating a concussed patient yields useless data, Prelate Hulrun. You have glanced at the city gates three times in a minute; your focus is wasted on an unarmed stranger.\"", "{n}Hulrun glares at you with suspicion, clearly unsettled by your clinical deduction.{/n} \"Deductions and statistics? You speak like an aloof scholar from some eastern academy. Which academy, then? And what is your name?\" {n}For all your clarity, you find no memory of either.{/n}", "MastermindProficiencies");
		}
		void AddArchetypeHulrunAnswer(string name, string text, string cueText, string proficiencyFactName)
		{
			Condition condition = AnswerGate(proficiencyFactName);
			if (condition != null)
			{
				GraftedAnswer.Named(name).At(DialogueBeats.HulrunAsksWhoYouAre).Says(text)
					.ShownWhen(condition)
					.OnSelect((dontRememberEtude == null) ? null : new StartEtude
					{
						Etude = dontRememberEtude.ToReference<BlueprintEtudeReference>(),
						Evaluate = false
					})
					.RepliedToBy(name + "Reply", cueText)
					.SpokenBy(hulrunUnit)
					.ThenContinueTo(dontRememberCue)
					.Graft();
			}
		}
	}

	private static void AddMinaghoReactions()
	{
		BlueprintCue thatsNotVeryNiceCue = BlueprintTools.GetBlueprint<BlueprintCue>("3bd9a4263d8064b49a9d1eec365807b9");
		if (thatsNotVeryNiceCue != null)
		{
			AddArchetypeMinaghoAnswer("IsekaiMinaghoOverlord", "(Overlord) \"A mere regional general of the Abyss? You are an insignificant insect before the absolute sovereign of the Great Tomb. Kneel and prepare to be crushed.\"", "{n}Minagho's seductive sneer freezes, a flicker of genuine bewilderment and rage flashing across her demonic features.{/n} \"Sovereign? Tomb? You babble like a concussed peasant, mortal! When I tear your heart out, I will see what kingdom lies inside your ribs!\"", "OverlordProficiencies");
			AddArchetypeMinaghoAnswer("IsekaiMinaghoGodEmperor", "(God Emperor) \"Your demonic corruption ends here, Minagho. The radiant mandate of my empire shall cleanse Kenabres and cast your filth back to the dark.\"", "{n}Minagho recoils slightly as the aura of divine sovereignty flares around you.{/n} \"An empire? Your miserable crusade has no empire left! Only ash and despair await you!\"", "GodEmperorProficiencies");
			AddArchetypeMinaghoAnswer("IsekaiMinaghoDevourer", "(Slime) \"You carry a tremendous concentration of demonic energy. I was looking for a decent meal, and your aura smells surprisingly potent. Yield now, or you are going to become dessert.\"", "{n}Minagho recoils with disgust at your assessing, hungry gaze.{/n} \"Dessert?! What manner of repulsive creature are you?! Even the lowest dretches have more dignity than a ravenous mortal!\"", "DevourerProficiencies");
			AddArchetypeMinaghoAnswer("IsekaiMinaghoShadowMonarch", "(Shadow Monarch) \"Enjoy your fleeting victory, Minagho. When your broken corpse lies at my feet, your soul will rise to serve as a soldier in my eternal shadow army.\"", "{n}Minagho narrows her eyes, sensing the cold abyss of souls slumbering in your shadow.{/n} \"You dare speak of enslaving a daughter of the Abyss?! You will die screaming in the dark!\"", "ShadowMonarchProficiencies");
			AddArchetypeMinaghoAnswer("IsekaiMinaghoPhantomBlade", "(Martial God) \"You talk too much for someone whose head hasn't noticed it's already separated from her neck. Let's see if your magic can outpace a thought.\"", "{n}Minagho's lips curl into a vicious snarl.{/n} \"Fast words from meat that hasn't met the blade of Deskari! Let us see you outrun your own flayed skin!\"", "PhantomBladeProficiencies");
			AddArchetypeMinaghoAnswer("IsekaiMinaghoHero", "(Hero) \"You can tear down our walls, Minagho, but you can never break our bond! My friends and I will fight until every corner of Kenabres is free!\"", "{n}Minagho laughs, high and shrill.{/n} \"Friendship! Love! How charmingly pathetic! I will make necklaces from the teeth of your 'friends' before this night ends!\"", "HeroProficiencies");
			AddArchetypeMinaghoAnswer("IsekaiMinaghoMastermind", "(Mastermind) \"Your bluster cannot mask the flaws in your deployment, Minagho. You overextended your vanguard into Kenabres, and your lines are stretched far too thin to hold this garrison.\"", "{n}Minagho sneers coldly, though her demonic eyes dart to her flanks.{/n} \"Stretched thin? You arrogant little mortal! The Wardstone is in my grasp, and your pathetic crusade ends right here!\"", "MastermindProficiencies");
		}
		void AddArchetypeMinaghoAnswer(string name, string text, string cueText, string proficiencyFactName)
		{
			Condition condition = AnswerGate(proficiencyFactName);
			if (condition != null)
			{
				GraftedAnswer.Named(name).At(DialogueBeats.IrabethNamesMinagho).Says(text)
					.ShownWhen(condition)
					.RepliedToBy(name + "Reply", cueText)
					.ThenContinueTo(thatsNotVeryNiceCue)
					.Graft();
			}
		}
	}

	private static void AddDefendersHeartReactions()
	{
		BlueprintEtude chapter01 = BlueprintTools.GetBlueprint<BlueprintEtude>("df17ab913c348644b9bd3fe3f9781a84");
		BlueprintEtude dhDefended = BlueprintTools.GetBlueprint<BlueprintEtude>("b4f7860936e1248468d7ec3583b7441e");
		BlueprintEtude dhLost = BlueprintTools.GetBlueprint<BlueprintEtude>("11f47a92b8a99bf47a2a9a487433befd");
		if (chapter01 != null)
		{
			AddArchetypeCouncilAnswer("IsekaiCouncilGodEmperor", "(God Emperor) \"Irabeth, Kenabres will not survive on desperate faith alone. My imperial treasury and celestial mandate shall fund the defense and steel the soldiers' wills.\"", "{n}Irabeth straightens her posture, visibly heartened by your commanding imperial confidence.{/n} \"Your resources and leadership have already kept this tavern standing. If you can organize the troops under such discipline, we might actually hold.\"", "GodEmperorProficiencies", beforeTheAssault: false);
			AddArchetypeCouncilAnswer("IsekaiCouncilMastermind", "(Mastermind) \"Irabeth, I have analyzed the demon attack patterns against the Defender's Heart. Bottleneck them at the western barricade with incendiary alchemists, and casualties drop by 72%.\"", "{n}Irabeth studies your tactical diagrams with growing astonishment.{/n} \"This... this is brilliant. I don't know where you learned military logistics, but I will reposition our archers immediately.\"", "MastermindProficiencies", beforeTheAssault: true);
		}
		void AddArchetypeCouncilAnswer(string name, string text, string cueText, string proficiencyFactName, bool beforeTheAssault)
		{
			Condition condition = AnswerGate(proficiencyFactName);
			if (condition != null)
			{
				List<Condition> list = new List<Condition>
				{
					condition,
					new EtudeStatus
					{
						m_Etude = chapter01.ToReference<BlueprintEtudeReference>(),
						Playing = true
					}
				};
				if (beforeTheAssault)
				{
					BlueprintEtude[] array = new BlueprintEtude[2] { dhDefended, dhLost };
					foreach (BlueprintEtude blueprintEtude in array)
					{
						if (blueprintEtude != null)
						{
							list.Add(new EtudeStatus
							{
								Not = true,
								m_Etude = blueprintEtude.ToReference<BlueprintEtudeReference>(),
								Playing = true
							});
						}
					}
				}
				GraftedAnswer.Named(name).At(DialogueBeats.IrabethDefendersHeartCouncil).Says(text)
					.ShownWhen(list.ToArray())
					.RepliedToBy(name + "Reply", cueText)
					.ThenBackToTheList()
					.Graft();
			}
		}
	}

	private static void AddWelcomeDialogueReactions()
	{
		BlueprintUnit terendelevUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("045e2ea911644fa0abaa2f6c76b83feb");
		GraftedAnswer.Named("IsekaiWelcomeDejaVu").At(DialogueBeats.TerendelevHasHealedYou).Says("[Deja Vu: Memory of Past Cycles] \"Wait... this square, the banners, your gentle touch... I remember this moment. I have walked this crusade before. Am I trapped in an eternal loop?\"")
			.ShownWhen(new ConditionHasPastCycles())
			.OnSelect(new ContextActionPastCycleReflectionBanter())
			.RepliedToBy("IsekaiWelcomeDejaVuReply", "{n}The silver-haired noblewoman pauses, her eyes widening slightly in curious contemplation before she offers a serene, enigmatic smile.{/n} \"A sensation of having lived this moment before? Time flows in mysterious currents, stranger. Perhaps your destiny here was written long before today. Stand tall, for whatever cycle you have traversed, your journey in Kenabres begins anew.\"")
			.SpokenBy(terendelevUnit)
			.ThenBackToTheList()
			.Graft();
		AddWelcomeAnswer("IsekaiWelcomeGeneral", "(Isekai Protagonist) \"I'm alive...? I don't know what kind of miraculous field medicine you just used, my lady, but a moment ago I was coughing blood. Thank you. I owe you my life.\"", "{n}The silver-haired noblewoman in white robes smiles with serene warmth, her touch radiating a calming, gentle glow.{/n} \"You owe me nothing. In Kenabres, no one is left behind while hope still endures. Save your strength; the festival is just beginning.\"", "IsekaiProficiencies");
		AddWelcomeAnswer("IsekaiWelcomeDevourer", "(Slime) \"Whoa... the burning agony in my stomach is completely gone. That soothing healing aura... you're a remarkably kind doctor, miss. Thank you for patching me up!\"", "{n}The lady in white smiles gently at your earnest relief, an amused twinkle in her pale eyes.{/n} \"A doctor? A flattering title. Rest easy, friend. Your vitality is remarkably resilient, though this wound is no ordinary one.\"", "DevourerProficiencies");
		AddWelcomeAnswer("IsekaiWelcomeShadowMonarch", "(Shadow Monarch) \"The cold grip of death in my chest... lifted. Your restorative power was swift and absolute, my lady. I will remember this debt.\"", "{n}The noblewoman in white robes nods gently, her silver gaze holding ancient depth and compassion.{/n} \"Do not speak of debts, traveler. Kenabres stands united under the heavens. Stand tall and celebrate your recovery with our people.\"", "ShadowMonarchProficiencies");
		AddWelcomeAnswer("IsekaiWelcomeMastermind", "(Mastermind) \"Cellular tissue restoration completed in under three seconds with zero scarring... Remarkable medical craft, my lady. My cognitive faculties are fully restored. I am in your debt.\"", "{n}The silver-haired lady blinks in bemusement at your clinical vocabulary, then smiles gracefully.{/n} \"Such clinical detachment for someone who just stared into the abyss! Keep that keen mind guarded; clarity is a rare and precious gift in these troubled times.\"", "MastermindProficiencies");
		AddWelcomeAnswer("IsekaiWelcomePhantomBlade", "(Martial God) \"Lethal chest laceration closed without a single suture... your triage technique is extraordinary, healer. Where I'm from, emergency rooms take hours to pull off miracles like this.\"", "{n}She tilts her head with an amused glimmer in her pale eyes.{/n} \"'Emergency rooms'? A curious term from distant lands. But I am pleased divine grace reached your heart in time.\"", "PhantomBladeProficiencies");
		AddWelcomeAnswer("IsekaiWelcomeHero", "(Hero) \"Thank you... your gentle hands pulled me back from the brink. When I opened my eyes and saw you standing over me in radiant white robes, I thought an angel had descended from the heavens.\"", "{n}A soft blush touches the lady's fair cheeks as she gives a musical laugh.{/n} \"Hardly an angel! Just a guardian watching over this fair city. Take heart, brave soul! The festival needs such bright spirits.\"", "HeroProficiencies");
		AddWelcomeAnswer("IsekaiWelcomeGodEmperor", "(God Emperor) \"A magnificent restoration. You possess hands blessed by true celestial grace, noble healer. When my sovereign dominion is established, your benevolence shall not be forgotten.\"", "{n}The lady in white arches an eyebrow with dignified amusement.{/n} \"A regal demeanor even with the dust of the road on your tunic! May your high ambitions serve peace and justice, traveler.\"", "GodEmperorProficiencies");
		AddWelcomeAnswer("IsekaiWelcomeOverlord", "(Overlord) \"To reverse fatal trauma with a mere touch... admirable proficiency, healer. You have preserved a vessel destined for supreme dominion.\"", "{n}She offers a calm, knowing smile, unfazed by your dark demeanor.{/n} \"Every life holds supreme destiny in the grand tapestry of fate. Go forth and use this second chance well.\"", "OverlordProficiencies");
		void AddWelcomeAnswer(string name, string text, string cueText, string proficiencyFactName)
		{
			Condition condition = AnswerGate(proficiencyFactName);
			if (condition != null)
			{
				GraftedAnswer.Named(name).At(DialogueBeats.TerendelevHasHealedYou).Says(text)
					.ShownWhen(condition)
					.RepliedToBy(name + "Reply", cueText)
					.SpokenBy(terendelevUnit)
					.ThenBackToTheList()
					.Graft();
			}
		}
	}

	private static void AddMeetCameliaReactions()
	{
		BlueprintUnit camelliaUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("397b090721c41044ea3220445300e1b8");
		AddMeetCameliaAnswer("IsekaiCameliaGeneral", "(Isekai Protagonist) \"Did you see that giant silver dragon up there?! Where I'm from, dragons are myths! That thing was REAL! And that demon with the giant scythe... are you alright?!\"", "{n}Camellia wipes a smudge of dirt from her pale cheek, giving you a cool, calculating look.{/n} \"Of course Terendelev was real. She was the ancient protector of Kenabres. But dwelling on her fall won't pull us out of this pit. Stand up; panicking like a startled peasant will get us killed down here.\"", "IsekaiProficiencies");
		AddMeetCameliaAnswer("IsekaiCameliaDevourer", "(Slime) \"The ground collapsed... but more importantly, that giant demon lord just struck down the nice lady who was also a dragon?! What kind of insane death world have I reincarnated into?!\"", "{n}Camellia shivers slightly, eyeing you with restrained disdain and curiosity.{/n} \"Reincarnated? You hit your head quite hard, didn't you? Get your bearings. We need to find a way back to the surface before whatever lurks down here smells our blood.\"", "DevourerProficiencies");
		AddMeetCameliaAnswer("IsekaiCameliaShadowMonarch", "(Shadow Monarch) \"A mythical silver dragon struck down in seconds. The power scale in this realm is monstrous... but panic changes nothing. Are you able to walk?\"", "{n}Camellia arches an eyebrow, surprised by your sudden, chilling calm.{/n} \"A remarkably quick recovery from terror. Yes, I can walk. Follow me, and let us hope the rubble hasn't sealed every exit.\"", "ShadowMonarchProficiencies");
		AddMeetCameliaAnswer("IsekaiCameliaMastermind", "(Mastermind) \"Subterranean stratum collapsed; draconic guardian struck down by an abyssal sovereign in under forty seconds. Panic yields zero tactical value. Can you fight?\"", "{n}Camellia blinks, momentarily taken aback by your icy clinical composure.{/n} \"Can I fight? I assure you, I am more than capable of defending myself. Keep that analytical tongue focused on finding our way out.\"", "MastermindProficiencies");
		AddMeetCameliaAnswer("IsekaiCameliaHero", "(Hero) \"The lady who healed me... she gave her life protecting the whole city. I won't let her sacrifice be in vain. Don't worry, miss, I'll protect you until we find a way out!\"", "{n}Camellia smiles with faint, mocking amusement.{/n} \"How gallantly naive. I survived the fall on my own, 'hero.' Keep your blade ready for demons, not posturing.\"", "HeroProficiencies");
		void AddMeetCameliaAnswer(string name, string text, string cueText, string proficiencyFactName)
		{
			Condition condition = AnswerGate(proficiencyFactName);
			if (condition != null)
			{
				GraftedAnswer.Named(name).At(DialogueBeats.CamelliaIntroducesHerself).Says(text)
					.ShownWhen(condition)
					.RepliedToBy(name + "Reply", cueText)
					.SpokenBy(camelliaUnit)
					.ThenBackToTheList()
					.Graft();
			}
		}
	}

	private static void AddMeetSeelahAneviaReactions()
	{
		BlueprintCue blueprint = BlueprintTools.GetBlueprint<BlueprintCue>("36cac4b77d7e15e4e8c69a4340717cad");
		BlueprintUnit seelahUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("54be53f0b35bf3c4592a97ae335fe765");
		if (blueprint != null)
		{
			AddMeetSeelahAnswer("IsekaiSeelahGeneral", "(Isekai Protagonist) \"Hold on, I'll help with that rock! But tell me I didn't hallucinate it... the kind lady who healed me earlier was a literal SILVER DRAGON?! Dragons actually exist here?!\"", "{n}Seelah braces against the stone, her eyes brimming with grief.{/n} \"Terendelev was our guardian... the shining soul of Kenabres. Seeing Deskari strike her down tore the heart out of every crusader in that square. But we can't let her sacrifice be in vain. Now help me get Anevia out of there!\"", "IsekaiProficiencies");
			AddMeetSeelahAnswer("IsekaiSeelahMastermind", "(Mastermind) \"A silver dragon struck down by a demon lord of the outer rifts. The balance of Kenabres collapsed in under forty seconds. We must extract the injured to preserve tactical viability.\"", "{n}Seelah stares at you in complete disbelief.{/n} \"'Tactical viability'?! Anevia's ankle is crushed and Terendelev is dead, you clinical weirdo! Help me lift this damn rock already!\"", "MastermindProficiencies");
			GraftedAnswer.Named("IsekaiTalkBackStarsAnswer").At(DialogueBeats.SeelahAsksForHelpWithTheBoulder).Says("[Look Upward] \"Shut up, all of you! Stop betting coins on my survival like I'm a gladiator in your cosmic circus! I can hear you mocking me from the clouds!\"")
				.OnSelect(new ContextActionAwardCosmicCoins
				{
					Amount = 250,
					Sponsor = "The Laughing King",
					SceneKey = "TalkBackStars"
				})
				.Configure(delegate(BlueprintAnswer bp)
				{
					bp.RequirePlotArmor();
				})
				.RepliedToBy("IsekaiTalkBackStarsReply", "{n}Seelah glances up at the jagged cavern ceiling, then reaches out to touch your forehead.{/n} \"Whoa... hey, easy! You took a nasty blow to the skull when the square collapsed. There's nobody up there but limestone and dripping water. Stay with me, alright? Don't lose your mind now.\"\n\n{n}Anevia winces, her ankle still pinned under the stone, and offers a dry grimace.{/n} \"Voices in the sky betting on us? Honestly, kid, after seeing Deskari strike down a dragon today, gods gambling on our corpses wouldn't even make my top five weirdest crusader stories. Just help Seelah lift this rock.\"")
				.SpokenBy(seelahUnit)
				.ThenBackToTheList()
				.Graft();
			GraftedAnswer.Named("IsekaiSeelahDevourer").At(DialogueBeats.SeelahAsksForHelpWithTheBoulder).Says("(Slime) \"Stand back, let me handle this. [Your arm dissolves into a fluid ribbon of green slime, slipping under the boulder and pushing it upward with hydraulic pressure.]\"")
				.ShownWhen(new HasFact
				{
					Unit = new PlayerCharacter(),
					m_Fact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "DevourerProficiencies")
				})
				.OnSelect(new ContextActionSlimeRevealBanter())
				.RepliedToBy("IsekaiSeelahDevourerReply", "{n}Seelah stumbles backward, her mouth falling open as your arm snaps back into solid flesh around the edge of the stone.{/n} \"By Iomedae's shield... did your arm just melt into green jelly?! What kind of creature are you?! You're not secretly some subterranean demon parasite, are you?!\"\n\n{n}Anevia laughs weakly through her pain.{/n} \"Seelah, stand down. If a friendly puddle of green ooze wants to lift a ton of limestone off my ankle, I'm not going to complain about its pedigree. Either that's the weirdest spell I've ever seen, or Kenabres just recruited the world's strangest crusader.\"")
				.ThenContinueTo(blueprint)
				.Graft();
			GraftedAnswer.Named("IsekaiSeelahOverlord").At(DialogueBeats.SeelahAsksForHelpWithTheBoulder).Says("(Overlord) \"Step aside, crusader. Allow true necromantic might to handle this. [You grip the stone. For a split second your disguise flickers, revealing ivory bones and burning red eye sockets.]\"")
				.ShownWhen(new HasFact
				{
					Unit = new PlayerCharacter(),
					m_Fact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "OverlordProficiencies")
				})
				.OnSelect(new ContextActionOverlordRevealBanter())
				.RepliedToBy("IsekaiSeelahOverlordReply", "{n}Seelah violently flinches back, her hand flying to the pommel of her holy sword, her eyes wide with terror.{/n} \"Your hands... your face! For a second you looked like... an undead skeleton! A walking corpse! What are you?!\"\n\n{n}Anevia groans under the shifting stone, waving her hand frantically.{/n} \"Seelah, sheath your steel! Look, they're flesh and blood again. An illusion, or some bizarre planar dust from the Worldwound... Either way, the dead usually try to chop off our heads, not heave boulders off our legs. Let's get out of these tunnels alive before we start a religious tribunal.\"")
				.ThenContinueTo(blueprint)
				.Graft();
		}
		void AddMeetSeelahAnswer(string name, string text, string cueText, string proficiencyFactName)
		{
			Condition condition = AnswerGate(proficiencyFactName);
			if (condition != null)
			{
				GraftedAnswer.Named(name).At(DialogueBeats.SeelahAsksForHelpWithTheBoulder).Says(text)
					.ShownWhen(condition)
					.RepliedToBy(name + "Reply", cueText)
					.SpokenBy(seelahUnit)
					.ThenBackToTheList()
					.Graft();
			}
		}
	}

	private static void AddMeetLannReactions()
	{
		BlueprintUnit lannUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("cb29621d99b902e4da6f5d232352fbda");
		AddMeetLannAnswer("IsekaiLannGeneral", "(Isekai Protagonist) \"Scales, horns, hooves and tails... honestly, after falling a mile into the earth and watching a dragon fight, beastfolk scouts don't even faze me. Greetings from the surface.\"", "{n}Lann blinks his reptilian eye in surprise, then grins lopsidedly.{/n} \"Well, that's refreshing! Usually surface dwellers scream, pray to Iomedae, or try to stab us. I'm Lann, by the way.\"", "IsekaiProficiencies");
		AddMeetLannAnswer("IsekaiLannDevourer", "(Slime) \"Whoa, chimera-like anatomy! Are those scales adapted for subterranean dampness? Fascinating biology!\"", "{n}Lann scratches his scaled cheek, chuckling.{/n} \"'Fascinating biology'? First time anyone's called our cursed mongrel hides a scientific marvel! You're a weird one, surface-dweller.\"", "DevourerProficiencies");
		void AddMeetLannAnswer(string name, string text, string cueText, string proficiencyFactName)
		{
			Condition condition = AnswerGate(proficiencyFactName);
			if (condition != null)
			{
				GraftedAnswer.Named(name).At(DialogueBeats.LannAndWenduagFirstSighting).Says(text)
					.ShownWhen(condition)
					.RepliedToBy(name + "Reply", cueText)
					.SpokenBy(lannUnit)
					.ThenBackToTheList()
					.Graft();
			}
		}
	}

	private static void AddWardstoneMythicReactions()
	{
		BlueprintCue continueCue = BlueprintTools.GetBlueprint<BlueprintCue>("e9b4e3d349a29b2449218b474cb78563");
		BlueprintUnit minaghoUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("bbcd85f6c1762364ba48be9f72f115ae");
		if (continueCue != null)
		{
			AddWardstoneAnswer("IsekaiWardstoneMythicGeneral", "(Isekai Protagonist) [Channel Unscripted Power] \"You thought this was a predictable script? Let's rewrite it right here and now. Feel free to scream, Minagho.\"", "{n}The celestial flames and cosmic starlight swirl violently around your fingertips, shattering the ambient demonic miasma in the garrison hall with blinding brilliance. Minagho stumbles backward, her claws scraping futilely at her chest as raw terror drains the color from her face.{/n} \"What... what manner of power is this?! This wasn't supposed to happen! Who are you?!\"", "IsekaiProficiencies");
			AddWardstoneAnswer("IsekaiWardstoneMythicGodEmperor", "(God Emperor) [Unleash Sovereign Radiance] \"Mortal, angel, demon, it matters not. Today, the mantle of absolute sovereignty descends upon this garrison. Kneel before your emperor, Minagho!\"", "{n}Blinding golden light erupts from your chest, towering above the fractured Wardstone in the form of a celestial throne and crown. Minagho shields her eyes with a shriek of agony, the holy radiance searing her fiendish skin.{/n} \"A crown?! An emperor?! What is this suffocating light?!\"", "GodEmperorProficiencies");
			AddWardstoneAnswer("IsekaiWardstoneMythicDevourer", "(Slime) [Engulf the Cosmic Resonance] \"That obelisk contained so much concentrated essence... and now it's mine. Every demon in Kenabres is just calories waiting to be digested.\"", "{n}A vortex of pure, insatiable hunger spirals out from your open palm, pulling the demonic fog and magic of the room into your chest. Minagho recoils in horror as she feels her very soul being tugged toward the void.{/n} \"You... you aren't just resisting corruption... you're eating it?! What kind of monster are you?!\"", "DevourerProficiencies");
			AddWardstoneAnswer("IsekaiWardstoneMythicShadowMonarch", "(Shadow Monarch) [Raise the Abyssal Veil] \"The Wardstone was a beacon of light. But true power awakens in the abyss between the stars. Rise, shadows of the fallen, and witness your monarch!\"", "{n}The bright flash of the Wardstone twists into chilling, pitch-black sovereign shade. The shadows cast by the pillars and fallen crusaders stand upright, kneeling toward you in silent loyalty. Minagho trembles violently.{/n} \"The shadows... they are obeying you?! This power has nothing to do with the heavens!\"", "ShadowMonarchProficiencies");
			AddWardstoneAnswer("IsekaiWardstoneMythicOverlord", "(Overlord) [Proclaim Absolute Dominance] \"Did you truly believe you could corrupt an artifact in my presence? You are a pathetic ant crawling across my domain. Die, insect.\"", "{n}A crushing wave of tyrannical necrotic and celestial pressure slams Minagho against the flagstones, and she gasps for breath under your suffocating presence.{/n} \"Impossible... a mortal cannot possess such suffocating authority! Who gave you this strength?!\"", "OverlordProficiencies");
			AddWardstoneAnswer("IsekaiWardstoneMythicMastermind", "(Mastermind) [Synthesize the Harmonic Paradox] \"Fascinating. 4,096 causal branches just collapsed into a single deterministic triumph. Your defeat was calculated before we breached the threshold, Minagho.\"", "{n}Geometrical matrices of luminous light hover around your eyes as the residual Wardstone energy synchronizes perfectly with your mental focus. Minagho clutches her head in pain.{/n} \"Matrices? Calculations?! You speak as if this whole battle was just an equation! Die, arrogant fool!\"", "MastermindProficiencies");
			AddWardstoneAnswer("IsekaiWardstoneMythicHero", "(Hero) [Ignite the Beacon of Hope] \"The people of Kenabres bled for this stone, and their hope will not be snuffed out by demons! Together, we will cleanse every corner of this land!\"", "{n}A radiant pillar of pure dawnfire engulfs the room, washing away every trace of demon stench. Minagho howls in fury and dread.{/n} \"Hope?! That sentimental garbage is burning through my flesh! Stop it! Tear them to pieces!\"", "HeroProficiencies");
		}
		void AddWardstoneAnswer(string name, string text, string cueText, string proficiencyFactName)
		{
			Condition condition = AnswerGate(proficiencyFactName);
			if (condition != null)
			{
				GraftedAnswer.Named(name).At(DialogueBeats.WardstoneTitanFeeling).Says(text)
					.ShownWhen(condition)
					.OnSelect(new ContextActionWardstoneMythicAwakening())
					.RepliedToBy(name + "Reply", cueText)
					.SpokenBy(minaghoUnit)
					.ThenContinueTo(continueCue)
					.Graft();
			}
		}
	}

	private static void AddDesnaTempleNatureCommunion()
	{
		AddDesnaAnswer("IsekaiDesnaCommunionDevourer", "(Slime) [Commune with the Song of Elysium] \"My slime core resonates with this melody... It's like the whispering winds and primeval beasts of Sarkoris are speaking to my living jelly.\"", "{n}Your gelatinous form trembles in resonance with the melody, and for a moment the spiritual remnants of Sarkorian beasts seem to stir in the Elysian light, dipping their heads in silent kinship with your hunger.{/n} \"The song echoes back from the wild steppes... Something out there heard you, and it answered.\"", "DevourerProficiencies", new ContextActionDesnaNatureCommunion());
		AddDesnaAnswer("IsekaiDesnaCommunionOverlord", "(Overlord) [Subjugate the Resonance] \"An amusing little melody. Even the capricious spirits of the Sphere of Dreams must bow to true metaphysical authority.\"", "{n}The delicate melody wavers under your chilling magical pressure before snapping into a rigid, tolling cadence. A hollow, obedient echo rolls through the stone hall.{/n} \"The song... bent to you? I have never heard it sound like that. I am not sure I want to again.\"", "OverlordProficiencies");
		AddDesnaAnswer("IsekaiDesnaCommunionShadowMonarch", "(Shadow Monarch) [Attune the Shadows] \"Where there is brilliant starlight, the shadows cast are absolute. Let the dusk embrace this song.\"", "{n}The flickering candlelight and celestial brilliance cast long, sentient silhouettes that bow in solemn formation across the shrine walls, deepening into a silent guard as they drink in the echoes of the melody.{/n} \"The shadows on the walls... are they bowing to you?\"", "ShadowMonarchProficiencies");
		AddDesnaAnswer("IsekaiDesnaCommunionGeneral", "(Isekai Protagonist) [Hum an Otherworldly Melody] \"This melody reminds me of anime theme songs and fantasy tavern tunes from back home. Let's see if the song can carry my favorite chorus.\"", "{n}The melody ripples with sudden playful syncopation, matching your humming note for note. The priests stare at you in fascination as an otherworldly tune dances through the ancient temple.{/n} \"I have never heard the song take up a mortal's tune. It sounds... joyful!\"", "IsekaiProficiencies");
		static void AddDesnaAnswer(string name, string text, string cueText, string proficiencyFactName, GameAction onSelect = null)
		{
			Condition condition = AnswerGate(proficiencyFactName);
			if (condition != null)
			{
				GraftedAnswer.Named(name).At(DialogueBeats.DesnaAdeptsAskAboutTheSong).Says(text)
					.ShownWhen(condition)
					.OnSelect(onSelect)
					.RepliedToBy(name + "Reply", cueText)
					.ThenBackToTheList()
					.Graft();
			}
		}
	}

	private static void AddQueenGalfreyWarCampReactions()
	{
		BlueprintUnit galfreyUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("97d1c33c784100140b9fa317aef5a499");
		BlueprintCue levyTakenReply = GraftedCue.Named("IsekaiWarCampLevyAlreadyTakenReply").Says("{n}Galfrey raises a hand before you have finished.{/n} \"Commander, the muster rolls closed on your name this morning. Every soul you brought me is already counted, armed and drilling in the lower field. I will not enter them twice and draw rations the crusade does not have. Keep whoever else answers your call, and bring them to Drezen's walls instead.\"").SpokenBy(galfreyUnit)
			.ThenBackTo(DialogueBeats.QueenGalfreyForces)
			.Blueprint;
		AddWarCampAnswer("IsekaiWarCampOverlord", "(Overlord) [Summon Deathbound Legion] \"Your Majesty, mortal crusaders bleed and break. I have raised 10 Grave Knights from ancient sepulchers. They know no fear and obey without question.\"", "{n}Queen Galfrey stares at you in grim apprehension as cold, armored dread marshals beyond the perimeter.{/n} \"Grave Knights... Under any other commander, such blasphemy would see you executed on the spot. But if you can truly bind their dark wills to defend Mendev... I will not turn away steel when the realm is on the brink. Keep them on a tight leash, Commander.\"", "OverlordProficiencies", new ContextActionMobilizeSubclassArmy
		{
			ClaimKey = "WarCampLevy",
			UnitGuid = "15c574688d6e44e79925411c5234765e",
			Count = 10,
			ArmyName = "Grave Knights"
		});
		AddWarCampAnswer("IsekaiWarCampShadowMonarch", "(Shadow Monarch) [Call the Shadowdancers] \"An army that leaves no corpses and needs no supply lines. 10 Shadowdancers wait in the camp's twilight, ready for the march on Drezen.\"", "{n}The Queen watches in awe as ten silent figures step out of the shadows of the crusade pavilions, their blades gleaming with cold voidfire.{/n} \"Shadowdancers... They move like living mirages. If they can strike behind demon lines without suffering attrition, Drezen's outer bastions will crumble far faster. See them deployed to the vanguard, Commander.\"", "ShadowMonarchProficiencies", new ContextActionMobilizeSubclassArmy
		{
			ClaimKey = "WarCampLevy",
			UnitGuid = "5a49b18137ebd634e96f3dfba01b77f2",
			Count = 10,
			ArmyName = "Shadowdancers"
		});
		AddWarCampAnswer("IsekaiWarCampDevourer", "(Slime) [Summon the Beast Vanguard] \"The beasts of the Sarkorian wastes answered my call. 20 Wolf Riders have emerged, eager to tear through fiendish lines.\"", "{n}Feral howls echo across the hills as savage riders surge up to the camp boundaries, their wolves sniffing the air with predatory relish.{/n} \"Wolf riders from the Sarkorian wastes! We thought their packs had scattered when the Worldwound opened. Their ferocity in battle is legendary. Well done, Commander.\"", "DevourerProficiencies", new ContextActionMobilizeSubclassArmy
		{
			ClaimKey = "WarCampLevy",
			UnitGuid = "c7904164350a4d5199c68da5c79aef74",
			Count = 20,
			ArmyName = "Wolf Riders"
		});
		AddWarCampAnswer("IsekaiWarCampHero", "(Hero) [Rally 50 Volunteer Crusaders] \"Word of our triumph in Kenabres has spread across the border provinces! 50 Shield Bearers have flocked to our banner to stand with us!\"", "{n}Queen Galfrey smiles with radiant hope as fresh ranks of resolute crusaders form up, their shields polished and eyes burning with righteous fervor.{/n} \"The morale of the crusade hasn't been this high in decades. With stalwarts like these rallying to your standard, I truly believe we can reclaim the Sword of Valor.\"", "HeroProficiencies", new ContextActionMobilizeSubclassArmy
		{
			ClaimKey = "WarCampLevy",
			UnitGuid = "24ad1a10190de5c459a4521996dcec4b",
			Count = 50,
			ArmyName = "Shield Bearers"
		});
		AddWarCampAnswer("IsekaiWarCampGodEmperor", "(God Emperor) [Inspire Imperial Discipline] \"Your Majesty, I have drilled 50 Shield Bearers in imperial martial doctrine. The crusade does not merely fight; it conquers.\"", "{n}Galfrey nods with deep respect.{/n} \"Your military discipline and regal authority have united even the most unruly mercenaries into an iron fist. Lead them well, Commander.\"", "GodEmperorProficiencies", new ContextActionMobilizeSubclassArmy
		{
			ClaimKey = "WarCampLevy",
			UnitGuid = "24ad1a10190de5c459a4521996dcec4b",
			Count = 50,
			ArmyName = "Shield Bearers"
		});
		AddWarCampAnswer("IsekaiWarCampMastermind", "(Mastermind) [Deploy Precision Vanguard] \"I have calculated the optimal force for breaching Drezen's outer trenchworks. 50 Shield Bearers have been assigned to specific breach coordinates.\"", "{n}Galfrey looks over your operational schematics with genuine admiration.{/n} \"The meticulous precision of your campaign plans is breathtaking. Every unit placed with surgical intent. Execute the plan, Commander.\"", "MastermindProficiencies", new ContextActionMobilizeSubclassArmy
		{
			ClaimKey = "WarCampLevy",
			UnitGuid = "24ad1a10190de5c459a4521996dcec4b",
			Count = 50,
			ArmyName = "Shield Bearers"
		});
		AddWarCampAnswer("IsekaiWarCampMartialGod", "(Martial God) [Drill the Vanguard in the Way of the Blade] \"Your Majesty, I spent the march teaching 50 Shield Bearers to breathe, step and strike as one. None of them will flinch when the demons come.\"", "{n}Galfrey watches a line of shields move in perfect unison, each soldier's stance as steady as a temple bell.{/n} \"I have seen paladins train half their lives for that kind of calm. If you can teach it to raw recruits in a few weeks, Drezen's walls will not stand long. Keep drilling them, Commander.\"", "PhantomBladeProficiencies", new ContextActionMobilizeSubclassArmy
		{
			ClaimKey = "WarCampLevy",
			UnitGuid = "24ad1a10190de5c459a4521996dcec4b",
			Count = 50,
			ArmyName = "Shield Bearers"
		});
		AddWarCampAnswer("IsekaiWarCampGeneral", "(Isekai Protagonist) [Rally the Survivors of Kenabres] \"Your Majesty, the people who crawled out of Kenabres keep asking where to sign up. I have gathered 50 Shield Bearers who want to fight beside us.\"", "{n}Queen Galfrey looks over the new recruits, many of them still wearing the ash of Kenabres on their tabards.{/n} \"People follow someone who walks out of the rubble and keeps walking. They will need steel and drilling, but their hearts are already in this fight. See them armed, Commander.\"", "IsekaiProficiencies", new ContextActionMobilizeSubclassArmy
		{
			ClaimKey = "WarCampLevy",
			UnitGuid = "24ad1a10190de5c459a4521996dcec4b",
			Count = 50,
			ArmyName = "Shield Bearers"
		});
		void AddWarCampAnswer(string name, string text, string cueText, string proficiencyFactName, ContextActionMobilizeSubclassArmy mobilizeAction)
		{
			Condition condition = AnswerGate(proficiencyFactName);
			if (condition != null)
			{
				bool claims = mobilizeAction != null && !string.IsNullOrEmpty(mobilizeAction.ClaimKey);
				GraftedAnswer.Named(name).At(DialogueBeats.QueenGalfreyForces).Says(text)
					.ShownWhen(condition)
					.OnSelect(mobilizeAction)
					.RepliedToBy(name + "Reply", cueText)
					.SpokenBy(galfreyUnit)
					.ConfigureReply(delegate(GraftedCue reply)
					{
						reply.ShownWhen(claims ? new ConditionWarCampLevyStillOwed
						{
							ClaimKey = mobilizeAction.ClaimKey
						} : null);
					})
					.ThenBackToTheList()
					.OrElseReply(claims ? levyTakenReply : null)
					.Graft();
			}
		}
	}

	private static void AddRegillHellknightReactions()
	{
		AddRegillAnswer("IsekaiRegillMastermind", "(Mastermind) [Counter-Analyze Regill's Tactics] \"Your retreat into the canyon cut flanking angles by 63%, but your triage was poor. Crossbowmen on the eastern ridge would have cost you no Hellknights.\"", "{n}Regill narrows his golden eyes, studying you with razor-sharp analytical respect.{/n} \"A rigorous assessment. Most who witness our discipline see only cruelty; you see geometry and resource management. We may find common ground yet, Commander.\"", "MastermindProficiencies");
		AddRegillAnswer("IsekaiRegillOverlord", "(Overlord) [Demonstrate Superior Authority] \"Your Order of the Godclaw prides itself on absolute order. But true order does not beg magistrates for legitimacy; it bends reality by supreme will.\"", "{n}Regill stands motionless under your dark gaze, his expression unyielding stone.{/n} \"A tyrant's philosophy. Yet the Worldwound is chaos incarnate; against utter disorder, absolute conviction is an acceptable weapon. Prove your supreme will on the march to Drezen.\"", "OverlordProficiencies");
		AddRegillAnswer("IsekaiRegillShadowMonarch", "(Shadow Monarch) [Acknowledge Strategic Elimination] \"Sacrificing your wounded to preserve unit cohesion was brutal, but necessary. In the abyss of war, hesitation is the only true sin.\"", "{n}Regill nods once with crisp, measured approval.{/n} \"A rare spark of pragmatic clarity. Those who flinch from necessary amputations allow gangrene to consume the whole regiment. Remember that principle when you command the crusade.\"", "ShadowMonarchProficiencies");
		AddRegillAnswer("IsekaiRegillGodEmperor", "(God Emperor) [Offer Imperial Commission] \"Paralictor Regill, your discipline is formidable, but incomplete without an emperor worthy of your service. Swear your legions to my imperial mandate.\"", "{n}Regill's jaw tightens with professional composure.{/n} \"The Order of the Godclaw serves no individual sovereign on whim. But if your imperial mandate enforces absolute law and eradicates the abyssal cancer, our hammers will march at your side.\"", "GodEmperorProficiencies");
		AddRegillAnswer("IsekaiRegillDevourer", "(Slime) [Assess Gargoyle Caloric Density] \"Those gargoyles were surprisingly tough. Not much meat on them, mostly crunchy stone... but their magical essence made a decent post-battle snack.\"", "{n}Regill blinks once, his expression briefly flickering with profound bafflement before returning to stern discipline.{/n} \"...Consuming stony fiends for sustenance. Unorthodox. Biologically dubious. But if your digestion removes enemy combatants from the battlefield, I have no tactical objection.\"", "DevourerProficiencies");
		static void AddRegillAnswer(string name, string text, string cueText, string proficiencyFactName)
		{
			Condition condition = AnswerGate(proficiencyFactName);
			if (condition != null)
			{
				GraftedAnswer.Named(name).At(DialogueBeats.RegillSendsYakerOff).Says(text)
					.ShownWhen(condition)
					.RepliedToBy(name + "Reply", cueText)
					.ThenBackToTheList()
					.Graft();
			}
		}
	}

	private static void AddZachariusLostChapelReactions()
	{
		AddZachariusAnswer("IsekaiZachariusOverlord", "(Overlord) [Assert Arch-Lich Supremacy] \"You hide in a damp basement whimpering over an ancient defeat, Zacharius. Kneel before a true Sovereign of the Undead, or I will crush your soul into bone powder.\"", "{n}Zacharius reels back, the necrotic flames in his empty eye sockets flickering with genuine shock and primordial dread.{/n} \"What... what manner of entity are you?! The tyrannical death aura you emanate... it dwarfs even the archmages of ancient Geb!\" {n}He steadies himself.{/n} \"Very well, Sovereign. Then you understand why I must have my wand back.\"", "OverlordProficiencies");
		AddZachariusAnswer("IsekaiZachariusDevourer", "(Slime) [Appraise Ancient Bone Calcium] \"Ooh, a talking skeleton! Are you preserved like magical jerky? You look like you've been curing down here for a century. How many calories is a lich anyway?\"", "{n}Zacharius recoils in utter disgust and disbelief.{/n} \"Calories?! Curing?! You insufferable, gelatinous monstrosity! I am Zacharius, hero of the First Crusade and master of death itself, not livestock for your gluttony! Keep your vile slime away from my robes!\"", "DevourerProficiencies");
		AddZachariusAnswer("IsekaiZachariusShadowMonarch", "(Shadow Monarch) [Extract Soul Echo] \"Death is not your prison, Zacharius. In my kingdom of shadows, every soul finds its eternal purpose under my throne.\"", "{n}Zacharius tilts his skull, mesmerized by the deep sovereign darkness wrapping around your fingers.{/n} \"Fascinating... not mere necromancy, but the primal gravity of the endless void. Perhaps your descent into darkness will achieve what the paladins never could.\"", "ShadowMonarchProficiencies");
		AddZachariusAnswer("IsekaiZachariusMastermind", "(Mastermind) [Diagnose Phylactery Deterioration] \"Your warding runes are failing, Zacharius. A century down here has worn them thin. You are in no position to make demands about this wand.\"", "{n}Zacharius's bony fingers tremble with rage and mortification.{/n} \"Failing?! Insolent whelp! My wards are... they are...\" {n}He glances anxiously at the flickering runes.{/n} \"...Curse your keen eyes. That changes nothing. The wand is mine, and I will have it back.\"", "MastermindProficiencies");
		static void AddZachariusAnswer(string name, string text, string cueText, string proficiencyFactName)
		{
			Condition condition = AnswerGate(proficiencyFactName);
			if (condition != null)
			{
				GraftedAnswer.Named(name).At(DialogueBeats.ZachariusDemandsHisWand).Says(text)
					.ShownWhen(condition)
					.RepliedToBy(name + "Reply", cueText)
					.ThenBackToTheList()
					.Graft();
			}
		}
	}

	private static void AddDrezenCitadelProclamationReactions()
	{
		AddProclamationAnswer("IsekaiDrezenProclamationSlime", "(Slime) [Proclaim the Tempest Multiverse Federation] \"Drezen is now the capital of the Tempest Multiverse Federation! Hot springs, good food, and a home for mortals, mongrels, and reformed fiends alike!\"", "{n}Queen Galfrey pauses, looking across the liberated citadel walls with a faint, bemused smile.{/n} \"A... multiverse federation? With hot springs and monster sanctuaries? Under any other crusader, I would consider this sheer madness. But your people adore you, and your strength retook these walls. Call your citadel what you like, so long as it holds. May your boundless ambition bring life back to these barren lands, Commander.\"", "DevourerProficiencies");
		AddProclamationAnswer("IsekaiDrezenProclamationOverlord", "(Overlord) [Consecrate Exterior Bastion of the Great Tomb] \"Hear me, mortal Queen! Drezen is claimed as an exterior bastion of the Great Tomb. Death shall govern these ramparts with unwavering order.\"", "{n}Galfrey shivers as an icy, necromantic weight settles over the stone battlement, yet she holds your gaze with queenly resolve.{/n} \"An unyielding necropolis in the heart of the Worldwound... While I cannot officially endorse the worship of undeath in holy Mendev, the peace and discipline your forces maintain cannot be denied. Drezen is yours to command. Rule it with the justice you promised.\"", "OverlordProficiencies");
		AddProclamationAnswer("IsekaiDrezenProclamationShadowMonarch", "(Shadow Monarch) [Raise the Citadel of Eternal Shadows] \"Let the sun set on the old world. Drezen is now the Citadel of Eternal Shadows, where the silent legion stands eternal guard over northern Avistan.\"", "{n}Galfrey looks out at the darkening towers, where silhouettes of shadow soldiers stand vigilant along the parapets.{/n} \"The demons took Drezen through light and fire, but they will never break through your shadows. The people feel protected, even if the cold chill in the air never quite fades. Drezen is safe in your shadow, Commander.\"", "ShadowMonarchProficiencies");
		AddProclamationAnswer("IsekaiDrezenProclamationGodEmperor", "(God Emperor) [Declare the First Radiant Imperium] \"By divine right, I consecrate Drezen as the First Radiant Imperium! Here begins a golden age of law and solar prosperity for all Golarion!\"", "{n}Queen Galfrey bows her head with profound regal respect as radiant golden light bathes the citadel square.{/n} \"An Imperium born from the ashes of demon tyranny... The majesty of your presence inspires hope even in the most cynical crusaders. Lead your people wisely, Sovereign Commander.\"", "GodEmperorProficiencies");
		AddProclamationAnswer("IsekaiDrezenProclamationHero", "(Hero) [Dedicate Drezen to Mortal Freedom] \"Drezen belongs to no tyrant, no god, and no demon! It belongs to the farmers, the knights, and the orphans who bled to take back their homes!\"", "{n}Galfrey smiles with radiant warmth.{/n} \"Spoken like a true champion of the people. For more than a century, we fought under the heavy shadow of despair. Today, your courage gave them back their future. Long live the Knight Commander!\"", "HeroProficiencies");
		AddProclamationAnswer("IsekaiDrezenProclamationMastermind", "(Mastermind) [Establish the Grand Sovereign Nexus] \"Drezen is restructured into the nexus of our geopolitical board. Every resource route, intelligence bureau, and bastion is calibrated for total victory.\"", "{n}Galfrey reviews your architectural and strategic blueprints with awe.{/n} \"Incredible. In a few weeks, you have organized logistics that took our Royal Council months to debate. With your intellect guiding the crusade, the demons will find no flaws to exploit.\"", "MastermindProficiencies");
		AddProclamationAnswer("IsekaiDrezenProclamationGeneral", "(Isekai Protagonist) [Proclaim the New Dawn of Drezen] \"We took back Drezen with our own hands, strange powers, and loyal friends. This fortress proves anything is possible when you refuse to surrender!\"", "{n}Galfrey nods with deep heartfelt sincerity.{/n} \"Indeed, Commander. You came from unknown origins and turned the tide of a war older than a century. Mendev salutes you, and the entire continent looks to your standard with hope.\"", "IsekaiProficiencies");
		static void AddProclamationAnswer(string name, string text, string cueText, string proficiencyFactName)
		{
			Condition condition = AnswerGate(proficiencyFactName);
			if (condition != null)
			{
				GraftedAnswer.Named(name).At(DialogueBeats.GalfreyPraisesDrezen).Says(text)
					.ShownWhen(condition)
					.RepliedToBy(name + "Reply", cueText)
					.ThenBackToTheList()
					.Graft();
			}
		}
	}

	private static void AddAreeluLabLoopTruthReactions()
	{
		BlueprintUnit sutureUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("90215a976d13b254ab905d0d6f747deb");
		AddLabTruthAnswer("IsekaiAreeluLabTruthGeneral", "(Isekai Protagonist) [Confront the Cosmic Glitch] \"Tell your mistress her ritual never pulled her child's soul. It cracked reality open, and Yog-Sothoth's loop dragged me in from another world!\"", "{n}The dretch freezes, his eyes darting from you to the purple shimmer of the book and back again.{/n} \"Another world? A loop? Heh... heh. My lady doesn't make mistakes. She doesn't!\" {n}He wrings his claws.{/n} \"But if she did... oh, she won't like that at all. Don't tell her I laughed.\"", "IsekaiProficiencies", new ContextActionAreeluLoopTruthBanter());
		AddLabTruthAnswer("IsekaiAreeluLabTruthDevourer", "(Slime) [Digest the Planar Glitch] \"Your mistress's ritual was a dimensional vacuum cleaner! It sucked in an otherworldly slime instead of her kid, and now Yog-Sothoth's loop has us on playback!\"", "{n}Suture stares at you, then edges away from your quivering form.{/n} \"A slime? From another world? That's not in any of her notes, and I've read all her notes. Most of them.\" {n}He licks his lips nervously.{/n} \"Just... don't eat the book. She'd have my hide.\"", "DevourerProficiencies");
		AddLabTruthAnswer("IsekaiAreeluLabTruthOverlord", "(Overlord) [Dissect the Flawed Incantation] \"Your mistress's rift was a pathetic miscalculation, dretch. It failed to tether her offspring and snagged an Overlord from beyond reality instead.\"", "{n}Suture cringes as an aura of supreme authority presses down on the laboratory.{/n} \"A miscalculation? My lady? Heh... you tell her that yourself, great one. I only fetch things and sweep up.\" {n}His gaze slides back to the book.{/n} \"But whatever you are, you'll want power worthy of you. Right there. Take it.\"", "OverlordProficiencies");
		AddLabTruthAnswer("IsekaiAreeluLabTruthShadowMonarch", "(Shadow Monarch) [Reveal the Sovereign Truth] \"The void your mistress opened pulled the sovereign of shadows into Yog-Sothoth's loop. Her child is not here; only the monarch remains.\"", "{n}The shadows of the laboratory coil around your feet, and Suture scrambles back from them.{/n} \"Keep them off me! I'm useful! I'm very useful!\" {n}He swallows hard.{/n} \"A monarch, pulled through her rift... She'll want to know. She always wants to know. Take the book, and maybe she won't have to hear it from me.\"", "ShadowMonarchProficiencies");
		AddLabTruthAnswer("IsekaiAreeluLabTruthMastermind", "(Mastermind) [Expose the Infinite Playback Algorithm] \"I reverse-engineered your mistress's ritual. Her child's soul never had a chance; Yog-Sothoth intercepted the breach to feed an endless broadcast.\"", "{n}Suture's eyes narrow into something almost clever.{/n} \"Never had a chance? You did her sums, just like that?\" {n}He lets out a dry, wheezing cackle that echoes through the laboratory.{/n} \"All those years, all those experiments... for a show. You see the whole board, don't you? Then you know that book is the next move.\"", "MastermindProficiencies");
		AddLabTruthAnswer("IsekaiAreeluLabTruthHero", "(Hero) [Declare Moral Rejection of the Loop] \"Your mistress's grief swallowed an innocent soul from another world. I won't let Yog-Sothoth's loop or her obsession destroy Golarion. I will break this cycle!\"", "{n}Suture snorts, though he doesn't quite meet your eyes.{/n} \"Break the cycle, protect the people, save the world. Heroes all say the same things.\" {n}He glances toward the depths of the laboratory.{/n} \"She'd find you interesting, though. Plucked from your home and still fighting. Go on, then. Break it if you can.\"", "HeroProficiencies");
		void AddLabTruthAnswer(string name, string text, string cueText, string proficiencyFactName, ContextAction extraAction = null)
		{
			Condition condition = AnswerGate(proficiencyFactName);
			if (condition != null)
			{
				GraftedAnswer.Named(name).At(DialogueBeats.SutureOffersTheLexicon).Says(text)
					.ShownWhen(condition)
					.OnSelect(extraAction)
					.RepliedToBy(name + "Reply", cueText)
					.SpokenBy(sutureUnit)
					.ThenBackToTheList()
					.Graft();
			}
		}
	}

	private static void AddBaphometConfrontationReactions()
	{
		AddBaphometAnswer("IsekaiBaphometOverlord", "(Overlord) [Mock the Lord of the Labyrinth] \"You bellow like a slaughterhouse bull, Baphomet. In the Great Tomb, cattle know their place. Kneel, or I shall turn your horns into drinking goblets.\"", "{n}Baphomet's massive nostrils flare with blistering hellfire as he clutches his glaive in furious shock.{/n} \"Cattle?! Drinking goblets?! Incomprehensible worm! You stand before the Lord of Minotaurs, master of the Ivory Labyrinth! I shall grind your arrogant bones into dust... but speak, while your tongue is still yours.\"", "OverlordProficiencies", new ContextActionBaphometDefianceBanter
		{
			Line = "Did the Commander just threaten to turn the Lord of the Labyrinth's horns into drinking goblets?! Absolute legendary performance!"
		});
		AddBaphometAnswer("IsekaiBaphometDevourer", "(Slime) [Inspect Demon Lord Caloric Density] \"Whoa, fresh beef! I've eaten dretches, minotaurs, and balors, but a full Demon Lord? You must have enough magical calories to evolve my whole federation!\"", "{n}Baphomet reels back, staring in profound revulsion at your quivering, hungry form.{/n} \"Fresh beef?! Caloric density?! You gelatinous abomination, I am a Demon Lord of the Abyss, not livestock for your gluttony! I shall scorch your vile ooze into dry ash... but speak, while you still have a mouth to speak with.\"", "DevourerProficiencies", new ContextActionBaphometDefianceBanter
		{
			Line = "Did the Commander just call the Lord of the Labyrinth fresh beef, to his face?! Absolute legendary performance!"
		});
		AddBaphometAnswer("IsekaiBaphometShadowMonarch", "(Shadow Monarch) [Claim the Ivory Horns] \"Your labyrinth is just a maze of shadows, Baphomet. And in the dark, every shadow answers to me. Even the shadows of demon lords.\"", "{n}Baphomet grunts in fury as the darkness at his hooves twists and resists his commands.{/n} \"Shadows answering to a mortal?! Insolent wretch! My labyrinth is eternal, and your soul shall wander its twisting corridors forever... once I have heard what else you came to say.\"", "ShadowMonarchProficiencies");
		AddBaphometAnswer("IsekaiBaphometGodEmperor", "(God Emperor) [Issue Imperial Extinction Decree] \"Baphomet, by decree of the Radiant Imperium, your reign of deceit is terminated. The light of the solar throne shall bleach your labyrinth to ash.\"", "{n}Baphomet roars, the cavern shaking as brimstone explodes from his cloven hooves.{/n} \"An emperor?! In my mines?! Your pathetic solar light will be swallowed by the eternal darkness of the Abyss! But go on, little emperor. Speak, while your tongue is still yours.\"", "GodEmperorProficiencies");
		AddBaphometAnswer("IsekaiBaphometMastermind", "(Mastermind) [Deconstruct Baphomet's Strategy] \"You sacrificed your own daughter to test my defenses, Lord of the Labyrinth. A pedestrian gambit. Your failure rate in this encounter is already 100%.\"", "{n}Baphomet barks with guttural rage, his crimson eyes bulging with murderous fury.{/n} \"Pedestrian gambit?! Hepzamirah served her purpose! And your purpose is to die here at the bottom of the world, arrogant strategist... but speak, while your tongue is still yours.\"", "MastermindProficiencies");
		AddBaphometAnswer("IsekaiBaphometHero", "(Hero) [Stand Resolute Against the Fiend] \"You betrayed your own blood and sacrificed countless lives for your labyrinth. Today, your cruelty ends, Baphomet. We aren't afraid of you!\"", "{n}Baphomet sneers with cruel, jagged teeth.{/n} \"Not afraid? You will learn terror when my glaive pins your heart to these caverns, righteous fool. But speak, while your tongue is still yours.\"", "HeroProficiencies");
		static void AddBaphometAnswer(string name, string text, string cueText, string proficiencyFactName, ContextAction extraAction = null)
		{
			Condition condition = AnswerGate(proficiencyFactName);
			if (condition != null)
			{
				GraftedAnswer.Named(name).At(DialogueBeats.BaphometTaunts).Says(text)
					.ShownWhen(condition)
					.OnSelect(extraAction)
					.RepliedToBy(name + "Reply", cueText)
					.ThenBackToTheList()
					.Graft();
			}
		}
	}

	private static void AddNocticulaAudienceReactions()
	{
		BlueprintCue returnVisitCue = BlueprintTools.GetBlueprint<BlueprintCue>("e9a8471fdce2791498242f2ce0f6ecb3");
		AddNocticulaAnswer("IsekaiNocticulaGeneral", "(Isekai Protagonist) [Resist Charm with Akashic Encryption] \"Drop the mental probes, Lady Nocticula. My soul has otherworldly Akashic encryption and a live audience of Constellations. No puppet here.\"", "{n}Nocticula raises a sculpted eyebrow, leaning back on her velvet cushions with a purr of genuine amusement.{/n} \"Akashic encryption... and a live celestial audience? How delightfully exotic. Most mortals who enter my palace fall weeping to their knees begging for a single touch. It seems the rumors of your otherworldly charm were not exaggerated, Commander. Let us speak as equals.\"", "IsekaiProficiencies");
		AddNocticulaAnswer("IsekaiNocticulaOverlord", "(Overlord) [Establish Equal Planar Dominance] \"You rule an island of assassins and hedonists, Lady of Shadow. Do not mistake an Overlord for crusader clay, or silk will burn under tier 10 magic.\"", "{n}Nocticula's lips curve into a tantalizing, dangerous smile, unruffled by your chilling threat.{/n} \"Such delicious arrogance... and the terrifying magical aura to back it up. A ruler of death who bows to no god. I like you, Overlord. Alushinyrra is honored by your presence. Let us conduct our business with mutual respect.\"", "OverlordProficiencies");
		AddNocticulaAnswer("IsekaiNocticulaDevourer", "(Slime) [Admire the Midnight Cuisine] \"Nice city you have here! The street food in the Lower City is fascinating. Any chance you'd trade some Midnight Isle recipes with the Tempest Federation?\"", "{n}Nocticula blinks once, then bursts into a melodious, sultry laugh that echoes through the House of Silken Shadows.{/n} \"Culinary recipes?! You tore through the Abyss, fought your way through Alushinyrra, marched into my royal palace... and your first request is Midnight Isle street food recipes?!\" {n}She claps her delicate hands, delighted.{/n} \"You are utterly charming, little slime. Consider my kitchen archives open to your federation.\"", "DevourerProficiencies");
		AddNocticulaAnswer("IsekaiNocticulaShadowMonarch", "(Shadow Monarch) [Commune in Absolute Darkness] \"You call yourself Our Lady in Shadow, Nocticula. But you rule merely the shadows cast by flesh. I rule the void itself.\"", "{n}The shadows beneath Nocticula's throne ripple in harmonic resonance with your own sovereign aura.{/n} \"The primal void... Yes, I can feel it. You do not merely borrow the dark; you are its undisputed master. How refreshing to meet someone who understands the true elegance of shadow. Welcome to my court, Monarch.\"", "ShadowMonarchProficiencies");
		AddNocticulaAnswer("IsekaiNocticulaMastermind", "(Mastermind) [Counter-Propose Planar Geopolitics] \"Impressive assassins, Lady Nocticula, but a fragile economy. Share Nahyndrian intelligence, and Alushinyrra gains 34% more trade after the war.\"", "{n}Nocticula leans forward, her golden eyes sparkling with sharp appreciation.{/n} \"A strategist who speaks in trade margins and economic dominance rather than righteous hymns. You truly are a breath of fresh air, Commander. Let us review your proposal.\"", "MastermindProficiencies");
		void AddNocticulaAnswer(string name, string text, string cueText, string proficiencyFactName)
		{
			Condition condition = AnswerGate(proficiencyFactName);
			if (condition != null)
			{
				GraftedAnswer.Named(name).At(DialogueBeats.NocticulaFirstAudience).Says(text)
					.ShownWhen(condition)
					.ShownWhen((returnVisitCue == null) ? new Condition[0] : new Condition[1]
					{
						new CueSeen
						{
							Not = true,
							m_Cue = returnVisitCue.ToReference<BlueprintCueBaseReference>()
						}
					})
					.RepliedToBy(name + "Reply", cueText)
					.ThenBackToTheList()
					.Graft();
			}
		}
	}

	private static void AddDivineSummitGoddessesReactions()
	{
		BlueprintUnit iomedaeUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("9a1443603c9353d4194a583a31228c8b");
		BlueprintUnit nocticulaUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("0cca8c841d634d84fbec2609c8db3465");
		BlueprintCue blueprint = GraftedCue.Named("IsekaiGoddessesSummitTricksterNocticulaReply").Says("{n}Nocticula smirks with wicked delight.{/n} \"I don't know what a live chat is, Commander, but I adore being the crowd favorite. Keep entertaining the cosmos!\"").SpokenBy(nocticulaUnit)
			.ThenBackTo(DialogueBeats.GoddessesAwaitTheDecision)
			.Blueprint;
		BlueprintCue blueprint2 = GraftedCue.Named("IsekaiGoddessesSummitOverlordNocticulaReply").Says("\"Let {mf|him|her} speak, Inheritor! A tyrant who looks down on both heaven and hell is far more amusing than your pious sermons.\"").SpokenBy(nocticulaUnit)
			.ThenBackTo(DialogueBeats.GoddessesAwaitTheDecision)
			.Blueprint;
		AddSummitAnswer("IsekaiGoddessesSummitLoopBreaker", "(Isekai Protagonist) [Reject Divine Scripts for Mortal Freedom] \"Neither blind faith nor abyssal chains! My power is my own, and my companions'. I reject both your scripts and will shatter this loop myself!\"", "{n}Iomedae stares at you in solemn, awe-struck sorrow, while Nocticula watches with breathless admiration.{/n} \"To reject both Heaven and the Abyss... to cast aside the divine design in the name of unguided mortal will. It is a path of terrible peril, Commander. Yet the purity of your conviction rivals that of the greatest martyrs of old. May your mortal spirit withstand the storm to come.\"", "IsekaiProficiencies", nocticulaSpeaks: false, null, new ContextActionLoopBreakerInsight());
		AddSummitAnswer("IsekaiGoddessesSummitTrickster", "(Isekai Protagonist) [Check the Live Chat Banter] \"Hold on! Live chat is going wild! 40% for Iomedae, 50% for Nocticula, and 10% betting I flip both of you off! Let's give them a show!\"", "{n}Both deities pause in sheer, profound bewilderment as holographic reaction emojis and coin donations flash across the air.{/n} \"Live chat? Constellation betting pools? What manner of alien madness is this?!\"", "IsekaiProficiencies", nocticulaSpeaks: false, blueprint, new ContextActionGoddessesChatBanter());
		AddSummitAnswer("IsekaiGoddessesSummitOverlord", "(Overlord) [Castigate Both Goddesses] \"A goddess preaching submission and a demon queen whispering seduction. Neither of you commands a Supreme Being. The Worldwound shall be sealed by my decree alone.\"", "{n}Iomedae's radiant sword blazes with righteous indignation, while Nocticula chuckles softly into her sleeve.{/n} \"Supreme Being? Your pride borders on the greatest heresies of the ancient world!\"", "OverlordProficiencies", nocticulaSpeaks: false, blueprint2);
		AddSummitAnswer("IsekaiGoddessesSummitDevourer", "(Slime) [Proclaim the Tempest Way] \"Why must everything be an ultimatum? In the Tempest Federation we protect our friends, build cozy homes, and eat good food. Keep your dogmas; we're fixing this our way!\"", "{n}Iomedae blinks, momentarily disarmed by your earnest, cheerful declaration.{/n} \"A cozy home... and good food? You speak of simple mortal peace in the face of planar destruction. If your strange federation can truly restore peace to these tortured lands, then prove it at Threshold, Commander.\"", "DevourerProficiencies");
		AddSummitAnswer("IsekaiGoddessesSummitGodEmperor", "(God Emperor) [Declare Sovereign Autarchy] \"The era of foreign gods deciding mortal destinies ends today. The Golden Throne of Sarkoris shall not kneel to Heaven nor Hell. My reign is absolute.\"", "{n}Iomedae looks upon you with solemn gravity.{/n} \"Many mortal kings have proclaimed absolute autarchy, only to fall to hubris. If you claim the throne of Sarkoris, rule with righteousness, not tyranny.\"", "GodEmperorProficiencies");
		AddSummitAnswer("IsekaiGoddessesSummitMastermind", "(Mastermind) [Expose the Binary False Choice] \"A classic false dichotomy: sacrificial obsolescence or parasitic dependency. Both algorithms fail. I select Option Three: mortal transcendence.\"", "{n}Nocticula laughs with sharp, appreciative brilliance.{/n} \"Parasitic dependency? Oh, you wound me, Commander! But I cannot fault a mind so delightfully immune to divine coercion. Show us your Option Three, then!\"", "MastermindProficiencies", nocticulaSpeaks: true);
		AddSummitAnswer("IsekaiGoddessesSummitShadowMonarch", "(Shadow Monarch) [Embrace the Sovereign Eclipse] \"Light and lust are equally fleeting. When the battle for Threshold begins, neither Heaven nor Alushinyrra will hold the line. My shadows will.\"", "{n}Iomedae tightens her grip on her holy blade as shadows swallow the stones of the summit.{/n} \"The dark power you harbor is perilous beyond measure, Commander. Do not let the shadows you command become your own tomb.\"", "ShadowMonarchProficiencies");
		void AddSummitAnswer(string name, string text, string cueText, string proficiencyFactName, bool nocticulaSpeaks = false, BlueprintCue followUp = null, ContextAction extraAction = null)
		{
			Condition condition = AnswerGate(proficiencyFactName);
			if (condition != null)
			{
				GraftedAnswer graftedAnswer = GraftedAnswer.Named(name).At(DialogueBeats.GoddessesAwaitTheDecision).Says(text)
					.ShownWhen(condition)
					.OnSelect(extraAction)
					.RepliedToBy(name + "Reply", cueText)
					.SpokenBy(nocticulaSpeaks ? nocticulaUnit : iomedaeUnit)
					.ThenBackToTheList();
				if (followUp != null)
				{
					graftedAnswer.ThenContinueTo(followUp);
				}
				graftedAnswer.Graft();
			}
		}
	}

	private static void AddThresholdGrandFinalEndingReactions()
	{
		List<BlueprintAnswer> endingAnswers = new List<BlueprintAnswer>();
		AddEndingAnswer("IsekaiEndingUnwrittenDawn", "(Isekai Protagonist) [Shatter the Cosmic Loop: The Unwritten Dawn] \"I reach for the Keys of Causal Rupture! Yog-Sothoth, let us see whether your playback ends today!\"", "{n}The Keys of Causal Rupture stir in answer, straining against the invisible temporal anchors that hold this era in recurrence. Whether they break will be decided by everything you have done, not by what you say here.{/n}", "IsekaiProficiencies", "UnwrittenDawn", "The Unwritten Dawn");
		AddEndingAnswer("IsekaiEndingInterdimensionalTraveler", "(Isekai Protagonist) [Slipstream Wanderer: The Interdimensional Traveler] \"When this is done, I step into the dimensional slipstream to explore the Great Beyond and leave Golarion in mortal hands.\"", "{n}The violet flames of the Wound bend around your otherworldly mana as you gather it for the crossing still to come. Beyond this last choice, the stars are waiting.{/n}", "IsekaiProficiencies", "InterdimensionalTraveler", "The Interdimensional Traveler");
		AddEndingAnswer("IsekaiEndingEternalActor", "(Isekai Protagonist) [Surrender to the Broadcast: The Eternal Actor] \"Let the Constellations have their eternal show. When this era fades, let the causal clock rewind for another season.\"", "{n}You bow to the empty sky. Somewhere beyond it an unseen audience roars, and the causal clock begins, very quietly, to wind itself back.{/n}", "IsekaiProficiencies", "EternalActor", "The Eternal Actor");
		AddEndingAnswer("IsekaiEndingPaxSepulchrum", "(Overlord) [Sovereign Protectorate: Pax Sepulchrum] \"Whatever becomes of the Wound, Sarkoris falls under the protectorate of the Great Tomb. The dead shall labor and defend so the living may prosper.\"", "{n}For a moment you glimpse the future you have claimed: disciplined death knights and bone laborers spread across Sarkoris, and an era of impeccable order, rich harvests and zero crime begins under the Supreme One's impartial gaze.{/n}", "OverlordProficiencies", "PaxSepulchrum", "Pax Sepulchrum - Guardian of the Great Tomb");
		AddEndingAnswer("IsekaiEndingIronCitadel", "(Overlord) [Isolated Bastion: Dominion of the Iron Citadel] \"I will raise obsidian ramparts around Sarkoris and isolate our necropolis from the petty squabbles of the outside world.\"", "{n}You picture it clearly: obsidian walls closing around a silent necropolis. A sanctuary of undeath, and a quiet, lifeless prison of your own making.{/n}", "OverlordProficiencies", "IronCitadel", "Dominion of the Iron Citadel");
		AddEndingAnswer("IsekaiEndingNecroTyrant", "(Overlord) [Total Harvest: The Eternal Necro-Tyrant] \"Mortal life is a fragile, chaotic disease. I will harvest every living soul in Avistan into a silent, disciplined undead legion.\"", "{n}A chilling vision sweeps over you: a necrotic storm across northern Avistan, life falling silent, and every soul rising in flawless, eternal obedience to the Great Tomb.{/n}", "OverlordProficiencies", "NecroTyrant", "The Eternal Necro-Tyrant");
		AddEndingAnswer("IsekaiEndingTempestFederation", "(Slime) [Multiversal Haven: The Tempest Multiverse Federation] \"Whatever happens to the Wound, these scarred lands become the Tempest Multiverse Federation, where all beings live and feast together!\"", "{n}A profound, comforting warmth radiates from you as you picture it: monsters and mortals sharing one table, and hot springs steaming where the demons once marched.{/n}", "DevourerProficiencies", "TempestFederation", "The Tempest Multiverse Federation");
		AddEndingAnswer("IsekaiEndingPrimevalMarsh", "(Slime) [Ancient Slumber: The Primeval Marsh] \"I will return Sarkoris to a wild primeval haven and curl up beneath the World Tree as Golarion's guardian beast-god.\"", "{n}The thought is strangely peaceful: a beast-god asleep beneath the World Tree, while the wild slowly reclaims Sarkoris around you.{/n}", "DevourerProficiencies", "PrimevalMarsh", "The Primeval Marsh");
		AddEndingAnswer("IsekaiEndingAbyssalSingularity", "(Slime) [Infinite Gluttony: The Abyssal Singularity] \"I will dive straight into the Abyss and devour demon lords until I wake Azathoth, collapsing the timeline and resetting the loop!\"", "{n}A terrifying, cosmic hunger stirs in your core. If you devour the Abyss itself, the temporal cascade will unmake everything, and you find that you do not mind at all.{/n}", "DevourerProficiencies", "AbyssalSingularity", "The Abyssal Singularity");
		AddEndingAnswer("IsekaiEndingEternalHope", "(Hero) [Celestial Nova: The Sovereign Beacon of Eternal Hope] \"I reach for the Triforce of Courage! I will channel the hope of every mortal who ever lived into an undying nova and ascend as the eternal protector of worlds!\"", "{n}An impossible, blinding light gathers around you, and the mark of Courage burns gold across the back of your hand. To become a beacon for all dimensions is an endless sacrifice, and you accept it gladly. Whichever path you swore to on the day you were called here, it is Courage that answers you at the end.{/n}", "HeroProficiencies", "EternalHope", "The Sovereign Beacon of Eternal Hope", AlignmentMaskType.Good);
		AddEndingAnswer("IsekaiEndingRetiredLegend", "(Hero) [Humble Peace: The Retired Legend] \"I refuse godhood and crowns. The Triforce of Wisdom asks for neither courage nor power, only the sense to put the blade down. When this is over, I will open a cozy tavern and training hall in Kenabres and live a full mortal life.\"", "{n}After holding the power to shape reality, you find yourself dreaming of pouring ale and telling stories. Wisdom is the piece that never asked you to be the bravest or the strongest, and this is the best dream you have had in years.{/n}", "HeroProficiencies", "RetiredLegend", "The Retired Legend");
		AddEndingAnswer("IsekaiEndingCorruptedChampion", "(Hero) [Fallen Paragon: The Corrupted Champion] \"I have lived long enough to become the villain. I close my hand on the Triforce of Power, and in the name of protecting everyone I will enforce absolute peace with an iron fist.\"", "{n}Something dark takes root beneath your righteousness as the mark of Power settles into your hand. Let them fear your goodness, just as they once feared the demons. Whichever path you swore to on the day you were called here, it is Power that answers you at the end.{/n}", "HeroProficiencies", "CorruptedChampion", "The Corrupted Champion", AlignmentMaskType.Evil);
		AddEndingAnswer("IsekaiEndingPaxSolaris", "(God Emperor) [Solar Renaissance: Pax Solaris] \"I will ascend the Golden Throne of Sarkoris as God-King and usher in an era of solar technology, justice, and prosperity!\"", "{n}For a moment you glimpse the future you have claimed: radiant solar towers rising from Drezen to the World's Edge Mountains, and a Solar Commonwealth uniting the nations in an era of cultural and scientific enlightenment.{/n}", "GodEmperorProficiencies", "PaxSolaris", "Pax Solaris - The Golden Renaissance");
		AddEndingAnswer("IsekaiEndingSarkorianImperium", "(God Emperor) [Iron Mandate: The Sarkorian Imperium] \"I will forge the Sarkorian Imperium and rule Avistan through unassailable military discipline and absolute law.\"", "{n}You see imperial legions marching to every border, bringing absolute stability and commercial dominance under your unwavering authority.{/n}", "GodEmperorProficiencies", "SarkorianImperium", "The Sarkorian Imperium");
		AddEndingAnswer("IsekaiEndingDualThrone", "(God Emperor) [Conquest of Two Realms: The Dual Throne of Sun and Abyss] \"I will march the solar legions into the Abyss and bind mortal and fiend under the supremacy of the Dual Throne.\"", "{n}You see solar banners planted in abyssal magma, and mortals and demons alike kneeling before your absolute authority across two planes.{/n}", "GodEmperorProficiencies", "DualThrone", "The Dual Throne of Sun and Abyss");
		AddEndingAnswer("IsekaiEndingSilentAegis", "(Shadow Monarch) [Watchers in the Dark: The Silent Aegis of Shadows] \"I will command the Legion of Shadows to shield Golarion, so night becomes a guardian rather than a terror.\"", "{n}You picture shadow soldiers standing silent, invisible guard over every road and hamlet, while innocents sleep peacefully under the sovereign darkness of the Netherworld.{/n}", "ShadowMonarchProficiencies", "SilentAegis", "The Silent Aegis of Shadows");
		AddEndingAnswer("IsekaiEndingObsidianBastion", "(Shadow Monarch) [The Shadow Gate: The Obsidian Bastion] \"Whatever becomes of the Wound, I will raise the Shadow Gate and sit upon the threshold, so neither demon nor conqueror ever crosses.\"", "{n}You picture an immense black monolith at the boundary between planes, and yourself seated upon its obsidian throne, the eternal gatekeeper between realities.{/n}", "ShadowMonarchProficiencies", "ObsidianBastion", "The Obsidian Bastion");
		AddEndingAnswer("IsekaiEndingNetherworldCataclysm", "(Shadow Monarch) [Eternal Void: The Netherworld Cataclysm] \"I will open the Netherworld across Avistan and drown the continent in an eternal black void where only shadows live.\"", "{n}A cold vision fills your mind: a sun that never rises again, and northern Avistan sinking into a silent realm where shadows walk the earth forever.{/n}", "ShadowMonarchProficiencies", "NetherworldCataclysm", "The Netherworld Cataclysm");
		AddEndingAnswer("IsekaiEndingGrandArchitect", "(Mastermind) [Unseen Maestro: The Immortal Philanthropist] \"With my immortality and foresight, I will quietly resolve every crisis before blood is spilled and guide centuries of prosperity.\"", "{n}The board lays itself out before you: wars solved before they ignite, history guided from behind the curtain like a flawless game of chess, and centuries of quiet flourishing.{/n}", "MastermindProficiencies", "GrandArchitect", "The Immortal Philanthropist");
		AddEndingAnswer("IsekaiEndingShadowSyndicate", "(Mastermind) [Information Sovereign: The Shadow Syndicate] \"I will erase every record of my existence and build an invisible broker network that keeps the world's economy and politics stable.\"", "{n}You picture your name vanishing from histories and songs, and an unassuming office in a peaceful city where you keep the ledger that discreetly governs world affairs.{/n}", "MastermindProficiencies", "ShadowSyndicate", "The Shadow Syndicate");
		AddEndingAnswer("IsekaiEndingNpcSimulator", "(Mastermind) [Cold Simulation: The NPC Simulator] \"Mortals, demons, and gods are mere NPCs on a board. I will farm their interactions and treat this reality as my private playground.\"", "{n}Detached from mortal sentiment, you already see kingdoms and fiends as pieces to move for optimal entertainment, and the multiverse as a sandbox built for your amusement.{/n}", "MastermindProficiencies", "NpcSimulator", "The NPC Simulator");
		AddEndingAnswer("IsekaiEndingInfiniteHorizons", "(Martial God) [Astral Severance: The Edge of Infinite Horizons] \"When this is done, my blade will cut a path into the Astral Plane, and I will ascend there as the God of the Sword!\"", "{n}Your phantom blade hums in its sheath. In a single transcendent draw you can already see the astral expanse opening, and yourself standing there as the supreme deity of the blade, guarding planar wanderers.{/n}", "PhantomBladeProficiencies", "InfiniteHorizons", "Astral Severance: The Edge of Infinite Horizons");
		AddEndingAnswer("IsekaiEndingBoundaryHermit", "(Martial God) [Meditative Boundary: The Boundary Hermit] \"I will retire to a tranquil pocket dimension on the astral fringe and perfect the singular transcendent strike of true enlightenment.\"", "{n}You picture yourself seated in quiet contemplation between planes, refining the perfect cut until, at the moment of enlightenment, the cosmic clock chimes and time gently rewinds.{/n}", "PhantomBladeProficiencies", "BoundaryHermit", "Meditative Boundary: The Boundary Hermit");
		AddEndingAnswer("IsekaiEndingSeveredHorizon", "(Martial God) [Reality Fracture: The Severed Horizon] \"I will strike reality too deeply, sever the metaphysical thread of Golarion, and watch existence dissolve into iridescent static.\"", "{n}For an instant you feel how the blade could cut too deep: the world unraveling like silk into glowing static, and you standing alone in the void.{/n}", "PhantomBladeProficiencies", "SeveredHorizon", "Reality Fracture: The Severed Horizon");
		Condition[] values = ((IEnumerable<BlueprintAnswer>)endingAnswers).Select((Func<BlueprintAnswer, Condition>)((BlueprintAnswer other) => new AnswerSelected
		{
			Not = true,
			m_Answer = other.ToReference<BlueprintAnswerReference>()
		})).ToArray();
		foreach (BlueprintAnswer item in endingAnswers)
		{
			item.ShowConditions.Conditions = item.ShowConditions.Conditions.AppendToArray(values);
		}
		void AddEndingAnswer(string name, string text, string cueText, string proficiencyFactName, string endingId, string endingTitle, AlignmentMaskType alignmentRequirement = AlignmentMaskType.None)
		{
			Condition condition = AnswerGate(proficiencyFactName);
			if (condition != null)
			{
				BlueprintAnswer blueprintAnswer = GraftedAnswer.Named(name).At(DialogueBeats.DecisionAtTheWound).Says(text)
					.ShownWhen(condition)
					.ShownWhen((alignmentRequirement == AlignmentMaskType.None) ? new Condition[0] : new Condition[1]
					{
						new PlayerAlignmentIs
						{
							Alignment = alignmentRequirement
						}
					})
					.OnSelect(new ContextActionTriggerIsekaiEnding
					{
						EndingId = endingId,
						EndingTitle = endingTitle
					})
					.RepliedToBy(name + "Reply", cueText)
					.ThenBackToTheList()
					.Graft();
				if (blueprintAnswer != null)
				{
					endingAnswers.Add(blueprintAnswer);
				}
			}
		}
	}

	private static void AddAngelMythicBookEventReactions()
	{
		BlueprintBookPage blueprint = BlueprintTools.GetBlueprint<BlueprintBookPage>("ee776f7d205f3484e9def0e007c5623f");
		BlueprintBookPage visionThoughtsPage = BlueprintTools.GetBlueprint<BlueprintBookPage>("2212f276d642d1d4fb8f93a0b74cb320");
		BlueprintBookPage visionShadowPage = BlueprintTools.GetBlueprint<BlueprintBookPage>("77ec7207718bf02409322455b1fbbf6c");
		BlueprintBookPage blueprint2 = BlueprintTools.GetBlueprint<BlueprintBookPage>("a6df6ed440dce0e499eb66b832243229");
		BlueprintBookPage visionClosingPage = BlueprintTools.GetBlueprint<BlueprintBookPage>("d1da445d30718914a8b7794864af8039");
		BlueprintEtude salvationEtude = BlueprintTools.GetBlueprint<BlueprintEtude>("4cefa6f7338ff6744b04179320a3cecb");
		BlueprintEtude retributionEtude = BlueprintTools.GetBlueprint<BlueprintEtude>("893252fb235b69a47b86b404e1d8accd");
		if (blueprint != null && visionThoughtsPage != null && visionShadowPage != null && blueprint2 != null && visionClosingPage != null && salvationEtude != null && retributionEtude != null)
		{
			BlueprintAnswer recognizeAnswer = TTCoreExtensions.CreateAnswer("IsekaiAngelVisionRecognizeAnswer", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"...stop fighting the memory and let it land. [You died once already. You know exactly what this is: a life ending, and its last thought looking for somewhere to go.]\"");
				blueprintAnswer.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { visionThoughtsPage.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				blueprintAnswer.ShowOnce = true;
				blueprintAnswer.OnSelect = Helpers.CreateActionList(Start(salvationEtude), new ContextActionAngelVisionBanter
				{
					Devoured = false
				}, IsekaiPrologueExpansions.StorybookReward("IsekaiStorybookPage1DualAction", IsekaiPrologueExpansions.PageOneRewardLine));
				blueprintAnswer.RequirePlotArmor();
			});
			BlueprintAnswer devourAnswer = TTCoreExtensions.CreateAnswer("IsekaiAngelVisionDevourAnswer", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Slime) \"...swallow the memory whole. [Your core closes around the foreign thoughts the way it closes around anything else, and begins, patiently, to digest a dead man's last hour.]\"");
				blueprintAnswer.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { visionThoughtsPage.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				blueprintAnswer.ShowOnce = true;
				blueprintAnswer.OnSelect = Helpers.CreateActionList(Start(retributionEtude), new ContextActionAngelVisionBanter
				{
					Devoured = true
				}, IsekaiPrologueExpansions.StorybookReward("IsekaiStorybookPage1DualAction", IsekaiPrologueExpansions.PageOneRewardLine));
				blueprintAnswer.ShowConditions.Conditions = blueprintAnswer.ShowConditions.Conditions.AppendToArray(new HasFact
				{
					Unit = new PlayerCharacter(),
					m_Fact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "DevourerProficiencies")
				});
			});
			BlueprintCue bp = TTCoreExtensions.CreateCue("IsekaiAngelVisionRecognizeCue", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}You stop resisting, and the torrent stops battering you and simply arrives, the way you once arrived: whole, uninvited and far too fast. Lariel's grief becomes something you are holding rather than something hitting you, and for one breath you feel both deaths at once, his and your own. Through his eyes, the wounded girl's gaze finds yours.{/n} \"You... you're not him,\" she whispers. \"But you came anyway.\"");
				blueprintCue.Speaker = new DialogSpeaker
				{
					NoSpeaker = true
				};
				blueprintCue.TurnSpeaker = false;
				blueprintCue.Conditions.Conditions = blueprintCue.Conditions.Conditions.AppendToArray(new AnswerSelected
				{
					Not = false,
					m_Answer = recognizeAnswer.ToReference<BlueprintAnswerReference>()
				});
			});
			BlueprintCue bp2 = TTCoreExtensions.CreateCue("IsekaiAngelVisionDevourCue", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}You do to the vision what you do to everything: you take it in. Lariel's fury dissolves into your core and comes apart into pieces you can name, and what settles at the bottom is a list. The half-elf with his hair twisted into a knot. The two standing behind him. The name they were promised everything by. You have eaten a stranger's death and kept the receipt.{/n}");
				blueprintCue.Speaker = new DialogSpeaker
				{
					NoSpeaker = true
				};
				blueprintCue.TurnSpeaker = false;
				blueprintCue.Conditions.Conditions = blueprintCue.Conditions.Conditions.AppendToArray(new AnswerSelected
				{
					Not = false,
					m_Answer = devourAnswer.ToReference<BlueprintAnswerReference>()
				});
			});
			BlueprintAnswer outsiderAnswer = TTCoreExtensions.CreateAnswer("IsekaiAngelVisionOutsiderAnswer", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) But this mind was not born on this world, and the vision finds nothing in it shaped to take hold of.");
				blueprintAnswer.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { visionShadowPage.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				blueprintAnswer.ShowOnce = true;
				blueprintAnswer.OnSelect = Helpers.CreateActionList(new ContextActionAwardCosmicCoins
				{
					Amount = 250,
					Sponsor = "The Key and the Gate",
					SceneKey = "AngelVisionOutsider"
				}, IsekaiPrologueExpansions.StorybookReward("IsekaiStorybookPage12SoulAegis", IsekaiPrologueExpansions.PageTwelveRewardLine, withCoins: false));
				blueprintAnswer.RequirePlotArmor();
			});
			BlueprintCue bp3 = TTCoreExtensions.CreateCue("IsekaiAngelVisionOutsiderCue", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}The attack arrives and finds the wrong shape. Terror, grief, an angel's dying certainty: all of it is built for a soul that grew up on this world, and yours did not. It passes through you like weather through an open window and leaves you standing, unhurt and obscurely ashamed of it. The one who sent the vision is not so fortunate. He is broken and exhausted.{/n}");
				blueprintCue.Speaker = new DialogSpeaker
				{
					NoSpeaker = true
				};
				blueprintCue.TurnSpeaker = false;
				blueprintCue.Conditions.Conditions = blueprintCue.Conditions.Conditions.AppendToArray(new AnswerSelected
				{
					Not = false,
					m_Answer = outsiderAnswer.ToReference<BlueprintAnswerReference>()
				});
			});
			BlueprintAnswer rewriteAnswer = TTCoreExtensions.CreateAnswer("IsekaiAngelVisionRewriteAnswer", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"...and rewrite the ending. I have already read how this story goes. I am not leaving it that way.\"");
				blueprintAnswer.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { visionClosingPage.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				blueprintAnswer.ShowOnce = true;
				blueprintAnswer.OnSelect = Helpers.CreateActionList(Start(salvationEtude), new ContextActionAngelVisionBanter
				{
					Devoured = false
				}, IsekaiPrologueExpansions.StorybookReward("IsekaiStorybookPage33DualCovenant", IsekaiPrologueExpansions.PageThirtyThreeRewardLine));
				blueprintAnswer.RequirePlotArmor();
			});
			BlueprintCue bp4 = TTCoreExtensions.CreateCue("IsekaiAngelVisionRewriteCue", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}You do not repeat the angel's last words. You answer them. Somewhere behind your ribs, the thing that pulled you out of one death and dropped you into this world turns over and listens. The vow is not a crusader's: not vengeance, not even mercy, but a flat refusal to let an ending stand as written. The heat in your chest goes quiet, as though something very large has accepted the terms.{/n}");
				blueprintCue.Speaker = new DialogSpeaker
				{
					NoSpeaker = true
				};
				blueprintCue.TurnSpeaker = false;
				blueprintCue.Conditions.Conditions = blueprintCue.Conditions.Conditions.AppendToArray(new AnswerSelected
				{
					Not = false,
					m_Answer = rewriteAnswer.ToReference<BlueprintAnswerReference>()
				});
			});
			blueprint.Answers?.Insert(0, recognizeAnswer.ToReference<BlueprintAnswerBaseReference>());
			blueprint.Answers?.Insert(0, devourAnswer.ToReference<BlueprintAnswerBaseReference>());
			visionThoughtsPage.Cues?.Insert(0, bp.ToReference<BlueprintCueBaseReference>());
			visionThoughtsPage.Cues?.Insert(0, bp2.ToReference<BlueprintCueBaseReference>());
			visionThoughtsPage.Answers?.Insert(0, outsiderAnswer.ToReference<BlueprintAnswerBaseReference>());
			visionShadowPage.Cues?.Insert(0, bp3.ToReference<BlueprintCueBaseReference>());
			blueprint2.Answers?.Insert(0, rewriteAnswer.ToReference<BlueprintAnswerBaseReference>());
			visionClosingPage.Cues?.Add(bp4.ToReference<BlueprintCueBaseReference>());
		}
		static StartEtude Start(BlueprintEtude etude)
		{
			return new StartEtude
			{
				Etude = etude.ToReference<BlueprintEtudeReference>(),
				Evaluate = false
			};
		}
	}

	private static void AddLepersSmileBookEventReactions()
	{
		BlueprintBookPage blueprint = BlueprintTools.GetBlueprint<BlueprintBookPage>("ab8c9446c25e3c54cac6d74f90535741");
		BlueprintBookPage bookPage39 = BlueprintTools.GetBlueprint<BlueprintBookPage>("a688fdddddca15a42b54265829a17b3d");
		BlueprintCue blueprint2 = BlueprintTools.GetBlueprint<BlueprintCue>("66df3b129e5421f4caa72c25e4f6f4cd");
		BlueprintCue blueprint3 = BlueprintTools.GetBlueprint<BlueprintCue>("ed4baea51c56e104e9b34c8503ffd951");
		BlueprintCue blueprint4 = BlueprintTools.GetBlueprint<BlueprintCue>("0cb0717a22bd0a54e84a39af95d50052");
		if (blueprint == null || bookPage39 == null)
		{
			return;
		}
		BlueprintAnswer shieldAnswer = TTCoreExtensions.CreateAnswer("IsekaiSwarmShieldAnswer", delegate(BlueprintAnswer blueprintAnswer)
		{
			blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) [Absolute Aegis] \"...Deploy an otherworldly absolute barrier. No soldier dies today under my command.\"");
			blueprintAnswer.NextCue = new CueSelection
			{
				Cues = new List<BlueprintCueBaseReference> { bookPage39.ToReference<BlueprintCueBaseReference>() },
				Strategy = Strategy.First
			};
			blueprintAnswer.ShowOnce = true;
			blueprintAnswer.RequirePlotArmor();
		});
		string[][] obj = new string[3][]
		{
			new string[2] { "4e30b6b69abc1b24ba38f211f98b9da2", "c623317974582b04589d98fc346f2384" },
			new string[2] { "30f9530e73f479244a31e313186aead0", "fd0fcafbbf3c44a469cb997472651b1f" },
			new string[2] { "9ae9fa0d32654284d901cef12e8e2ccf", "0e24c5f356b7bf046a6b5eefc09af230" }
		};
		List<GameAction> recordSuccess = new List<GameAction>();
		string[][] array = obj;
		foreach (string[] obj2 in array)
		{
			BlueprintAnswer blueprint5 = BlueprintTools.GetBlueprint<BlueprintAnswer>(obj2[0]);
			BlueprintUnlockableFlag blueprint6 = BlueprintTools.GetBlueprint<BlueprintUnlockableFlag>(obj2[1]);
			if (blueprint5 != null && blueprint6 != null)
			{
				recordSuccess.Add(new Conditional
				{
					Comment = "Isekai shield route: the squad's commander returns with every soldier",
					ConditionsChecker = ActionFlow.IfAll(new AnswerSelected
					{
						m_Answer = blueprint5.ToReference<BlueprintAnswerReference>()
					}),
					IfTrue = Helpers.CreateActionList(new UnlockFlag
					{
						m_flag = blueprint6.ToReference<BlueprintUnlockableFlagReference>(),
						flagValue = 1
					}),
					IfFalse = ActionFlow.DoNothing()
				});
			}
		}
		BlueprintCue bp = TTCoreExtensions.CreateCue("IsekaiSwarmShieldCue", delegate(BlueprintCue blueprintCue2)
		{
			blueprintCue2.SetText(Main.IsekaiContext, "{n}Drawing upon power from beyond this realm, you erect an impenetrable crystalline barrier of radiant force across the canyon. The swarming vescavors throw themselves against the light, their mandibles shattering on impact before bursting into harmless cinder. Not a single crusader is lost as the army marches safely past, staring in awe at your godlike protection.{/n}");
			blueprintCue2.Speaker = new DialogSpeaker
			{
				NoSpeaker = true
			};
			blueprintCue2.TurnSpeaker = false;
			blueprintCue2.OnShow = Helpers.CreateActionList(recordSuccess.ToArray());
			blueprintCue2.Conditions.Conditions = blueprintCue2.Conditions.Conditions.AppendToArray(new AnswerSelected
			{
				Not = false,
				m_Answer = shieldAnswer.ToReference<BlueprintAnswerReference>()
			});
		});
		blueprint.Answers?.Insert(0, shieldAnswer.ToReference<BlueprintAnswerBaseReference>());
		bookPage39.Cues?.Insert(0, bp.ToReference<BlueprintCueBaseReference>());
		BlueprintCue[] array2 = new BlueprintCue[3] { blueprint2, blueprint3, blueprint4 };
		foreach (BlueprintCue blueprintCue in array2)
		{
			if (blueprintCue?.Conditions != null)
			{
				blueprintCue.Conditions.Conditions = (blueprintCue.Conditions.Conditions ?? new Condition[0]).AppendToArray(new AnswerSelected
				{
					Not = true,
					m_Answer = shieldAnswer.ToReference<BlueprintAnswerReference>()
				});
			}
		}
		KeepTheCanyonBodyOffTheShieldRoute(shieldAnswer);
	}

	private static void KeepTheCanyonBodyOffTheShieldRoute(BlueprintAnswer shieldAnswer)
	{
		BlueprintScriptZone blueprint = BlueprintTools.GetBlueprint<BlueprintScriptZone>("0bc7507dbf7893545aab5c15122e9272");
		if (blueprint?.EnterActions != null)
		{
			blueprint.EnterActions = Helpers.CreateActionList(new Conditional
			{
				Comment = "Isekai shield route: no one from the diversion squad dies",
				ConditionsChecker = ActionFlow.IfAll(new AnswerSelected
				{
					Not = true,
					m_Answer = shieldAnswer.ToReference<BlueprintAnswerReference>()
				}),
				IfTrue = blueprint.EnterActions,
				IfFalse = ActionFlow.DoNothing()
			});
		}
	}
}
