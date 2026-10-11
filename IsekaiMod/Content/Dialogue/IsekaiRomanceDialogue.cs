using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.DialogSystem.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue;

internal class IsekaiRomanceDialogue
{
	public static void Add()
	{
		AddArueshalaeRomanceDialogue();
		AddDaeranRomanceDialogue();
		AddWenduagRomanceDialogue();
		AddLannRomanceDialogue();
		AddSosielRomanceDialogue();
		AddGalfreyWarCampRomanceDialogue(AddGalfreyRomanceDialogue());
		AddCamelliaRomanceDialogue();
		AddUlbrigRomanceDialogue();
		AddUlbrigDanceOfMasksRomanceDialogue();
	}

	private static void AddArueshalaeRomanceDialogue()
	{
		BlueprintUnit blueprint = BlueprintTools.GetBlueprint<BlueprintUnit>("a352873d37ec6c54c9fa8f6da3a6b3e1");
		GraftedCue graftedCue = GraftedCue.Named("IsekaiRomanceArueshalaeReply2").Says("{n}She draws your hand to her chest, where you can feel the steady, warm beat of her heart.{/n} \"If you are from beyond the stars, then you and I are both exiles who found each other in the dark. If that distant sky ever calls you back... promise me you won't let go without taking me with you. Because wherever you are, that is the only world I ever want to belong to.\"").SpokenBy(blueprint)
			.ThenBackTo(DialogueBeats.ArueshalaeHub);
		GraftedCue reply = GraftedCue.Named("IsekaiRomanceArueshalaeReply").Says("{n}Arueshalae looks at you with wide, luminous eyes filled with profound understanding. She gently takes your hand, her fingers trembling slightly.{/n} \"Every single day of my life, my love. For centuries I was a creature of the Abyss, a monster woven from sin and agony. Desna touched my soul and tore me from the only existence I ever knew, casting me into a realm of light and guilt where I never felt I belonged. I used to wake up every night terrified that Elysium was an illusion and that I would awaken back in the blood-soaked mud of the Rasping Rifts.\"").SpokenBy(blueprint)
			.ThenContinueTo((BlueprintCue)graftedCue);
		GraftedAnswer.Named("IsekaiRomanceArueshalae").At(DialogueBeats.ArueshalaeHub).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Isekai Protagonist) \"Arueshalae... have you ever felt like an intruder in a world you weren't born into? Sometimes I look at the stars and dread waking up back where I came from, with all of this, including us, gone.\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.AddShowCondition(delegate(EtudeStatus c)
				{
					c.m_Etude = BlueprintTools.GetBlueprintReference<BlueprintEtudeReference>("6a3fdd0758fe78d4aa2c3b26d7614fbc");
					c.Playing = true;
				});
			})
			.RepliedWith(reply)
			.SpokenByCompanion(blueprint)
			.Graft();
	}

	private static void AddDaeranRomanceDialogue()
	{
		BlueprintUnit blueprint = BlueprintTools.GetBlueprint<BlueprintUnit>("096fc4a96d675bb45a0396bcaa7aa993");
		GraftedCue graftedCue = GraftedCue.Named("IsekaiRomanceDaeranReply2").Says("{n}He lowers the glass and looks directly at you, his cynical mask slipping away for a tender second.{/n} \"I noticed it the day we met, that faraway glaze in your eyes whenever the crusaders start babbling about prophecy and righteous duty. You look at Golarion the way I look at high society balls: like a bemused tourist trapped in a burning ballroom. But listen to me. If some capricious author is writing this farce, they made one unforgivable blunder. They made me fall in love with the stranger from the gallery. And I intend to enjoy the performance until the curtain falls.\"").SpokenBy(blueprint)
			.ThenBackTo(DialogueBeats.DaeranHub);
		GraftedCue reply = GraftedCue.Named("IsekaiRomanceDaeranReply").Says("{n}Daeran raises his glass in mock salute, swirling the wine with a razor-thin smirk that doesn't quite conceal the sharpness in his eyes.{/n} \"My darling, I have treated life as a hideous stage play since I was a boy. The costumes are itchy, the script is atrocious, and the audience... well, the audience is an insatiable cosmic horror peering through the peepholes in our skulls.\"").SpokenBy(blueprint)
			.ThenContinueTo((BlueprintCue)graftedCue);
		GraftedAnswer.Named("IsekaiRomanceDaeran").At(DialogueBeats.DaeranHub).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Isekai Protagonist) \"Daeran, be honest with me. Do you ever get the feeling that none of this is real? That we're just actors playing out roles in some grand, absurd cosmic comedy?\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.AddShowCondition(delegate(EtudeStatus c)
				{
					c.m_Etude = BlueprintTools.GetBlueprintReference<BlueprintEtudeReference>("8541453b31379964e834cf2309444388");
					c.Playing = true;
				});
			})
			.RepliedWith(reply)
			.SpokenByCompanion(blueprint)
			.Graft();
	}

	private static void AddWenduagRomanceDialogue()
	{
		BlueprintUnit blueprint = BlueprintTools.GetBlueprint<BlueprintUnit>("ae766624c03058440a036de90a7f2009");
		GraftedCue graftedCue = GraftedCue.Named("IsekaiRomanceWenduagReply2").Says("{n}A vicious, possessive smile curls across her lips.{/n} \"The underground taught me that masters and gods are beasts who only care about what you can take with your claws. If unseen entities threw you into this pit for their amusement, and you turned around and conquered their demons and bent this continent to your will, then you are ten times the predator they could ever imagine! We are both rats who climbed out of our cages to tear out the throats of our keepers. If your gods ever come down from their stars to reclaim you... I will feather them with arrows until the sky bleeds.\"").SpokenBy(blueprint)
			.ThenBackTo(DialogueBeats.WenduagQuestionsHub);
		GraftedCue reply = GraftedCue.Named("IsekaiRomanceWenduagReply").Says("{n}Wenduag freezes, her golden feline eyes narrowing into predatory slits. Her fingers twitch toward her quiver, but slowly uncurl as she steps close enough for you to feel her ragged breath.{/n} \"You think that makes you weak, my {mf|master|mistress}? You think knowing the universe is cruel makes you frail?\"").SpokenBy(blueprint)
			.ThenContinueTo((BlueprintCue)graftedCue);
		GraftedAnswer.Named("IsekaiRomanceWenduag").At(DialogueBeats.WenduagQuestionsHub).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Isekai Protagonist) \"Wenduag... you worship strength because the underground showed you no mercy. But what if my power isn't mine? What if I'm an ordinary person from another realm, chosen by cosmic spectators who watch us like theater?\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.AddShowCondition(delegate(EtudeStatus c)
				{
					c.m_Etude = BlueprintTools.GetBlueprintReference<BlueprintEtudeReference>("33c4c2f66f2461e4993df21566252079");
					c.Playing = true;
				});
			})
			.RepliedWith(reply)
			.SpokenByCompanion(blueprint)
			.Graft();
	}

	private static void AddLannRomanceDialogue()
	{
		BlueprintUnit blueprint = BlueprintTools.GetBlueprint<BlueprintUnit>("cb29621d99b902e4da6f5d232352fbda");
		GraftedCue graftedCue = GraftedCue.Named("IsekaiRomanceLannReply2").Says("{n}He reaches over and firmly clasps your shoulder.{/n} \"If you get ripped back to your world of concrete and glass towers tomorrow, then today was still the greatest adventure of my short mongrel life. And hey, if whatever cosmic force thinks it can take you away without a fight, it clearly hasn't met an archer with twenty-four arrows in flight before it can even blink. You're stuck with me, Chief. Planar barriers be damned.\"").SpokenBy(blueprint)
			.ThenBackTo(DialogueBeats.LannHub);
		GraftedCue reply = GraftedCue.Named("IsekaiRomanceLannReply").Says("{n}Lann chuckles softly, turning a spare arrow over in his fingers. He looks up at you, his half-human, half-lizard expression softening into quiet sincerity.{/n} \"Hey... look at me. Thirty years, forty years, an immortal spark from another dimension, who's counting? Down in the caves, our lives were measured in days between demonic incursions. If I've learned anything since stepping onto the surface, it's that time is a joke. It's what you do with the time that matters.\"").SpokenBy(blueprint)
			.ThenContinueTo((BlueprintCue)graftedCue);
		GraftedAnswer.Named("IsekaiRomanceLann").At(DialogueBeats.LannHub).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Isekai Protagonist) \"Lann, you joke about your mongrel lifespan... but I have the opposite nightmare. I came here from another world. What if I can't stay, or I'm torn back before we can build a life together?\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.AddShowCondition(delegate(EtudeStatus c)
				{
					c.m_Etude = BlueprintTools.GetBlueprintReference<BlueprintEtudeReference>("af394a43ba7c1314bb31aea74a5e4c0e");
					c.Playing = true;
				});
			})
			.RepliedWith(reply)
			.SpokenByCompanion(blueprint)
			.Graft();
	}

	private static void AddSosielRomanceDialogue()
	{
		BlueprintUnit blueprint = BlueprintTools.GetBlueprint<BlueprintUnit>("1cbbbb892f93c3d439f8417ad7cbb6aa");
		GraftedCue graftedCue = GraftedCue.Named("IsekaiRomanceSosielReply2").Says("{n}He gently cups your cheek, leaving a faint streak of cobalt pigment against your skin.{/n} \"Shelyn teaches us that true beauty is born from harmony, not origin. It does not matter whether your soul was forged in the Inner Sea, the Great Beyond, or a world without magic. The devotion you offer this world, and the love you share with me, is the most authentic masterpiece I have ever witnessed. You are no ghost, my dear. You are the light that brought color back to my canvas.\"").SpokenBy(blueprint)
			.ThenBackTo(DialogueBeats.SosielHub);
		GraftedCue reply = GraftedCue.Named("IsekaiRomanceSosielReply").Says("{n}Sosiel sets down his paintbrush, wiping his pigment-stained hands on his apron. He gazes into your eyes with Shelynite warmth and calm reassurance.{/n} \"A shadow does not bleed for Kenabres. A shadow does not stand between ravenous demons and terrified children. A shadow does not look at me with the tender vulnerability you carry right now.\"").SpokenBy(blueprint)
			.ThenContinueTo((BlueprintCue)graftedCue);
		GraftedAnswer.Named("IsekaiRomanceSosiel").At(DialogueBeats.SosielHub).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Isekai Protagonist) \"Sosiel... when you paint, you capture the soul of Golarion. But when I look at the sky, I feel like a ghost wandering someone else's tapestry. Do you ever fear that loving an otherworldly stranger is loving a shadow?\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.AddShowCondition(delegate(EtudeStatus c)
				{
					c.m_Etude = BlueprintTools.GetBlueprintReference<BlueprintEtudeReference>("efb130b8a22c9534ca40a1e41ef8e931");
					c.Playing = true;
				});
			})
			.RepliedWith(reply)
			.SpokenByCompanion(blueprint)
			.Graft();
	}

	private static BlueprintAnswer AddGalfreyRomanceDialogue()
	{
		GraftedCue graftedCue = GraftedCue.Named("IsekaiRomanceGalfreyReply2").Says("{n}She steps closer, her armored hand resting gently over yours.{/n} \"When this war is won, Commander... whether your home lies across the ocean or across the stars... let us promise to lay down our armor together, if only for an hour. Just two weary souls who answered the call.\"").ThenBackTo(DialogueBeats.GalfreyVisitsDrezen);
		GraftedCue graftedCue2 = GraftedCue.Named("IsekaiRomanceGalfreyReplyKinship").Says("{n}She looks at you with a mixture of sorrow and profound kinship.{/n} \"Everyone sees the radiance of the Sunburst, the sunlit armor, the divine savior. No one sees the prison it creates. To know that you, too, were chosen by powers beyond your control... it is both heartbreaking and a strange, quiet comfort. In this citadel of thousands, you are the only one who understands what it means to be an instrument of fate.\"").ThenContinueTo((BlueprintCue)graftedCue);
		GraftedCue reply = GraftedCue.Named("IsekaiRomanceGalfreyReply").Says("{n}Galfrey's breath hitches. For a brief moment, the solemn, unyielding Queen of Mendev vanishes, replaced by an exhausted woman whose heart has carried a century of funeral pyres.{/n} \"More times than I can bear to confess to Iomedae, Commander.\"").ThenContinueTo((BlueprintCue)graftedCue2);
		return GraftedAnswer.Named("IsekaiRomanceGalfrey").At(DialogueBeats.GalfreyVisitsDrezen).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Isekai Protagonist) \"Your Majesty... a century on the throne, carrying a continent's expectations. I did not choose this mythic mantle either. I was torn from my home and thrust into this crusade. Do you ever wish you could set the crown aside and just be yourself?\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.ShownWhen(new EtudeStatus
			{
				m_Etude = BlueprintTools.GetBlueprintReference<BlueprintEtudeReference>("c9358d866e0b3844b8d72536ca60e4b4"),
				Playing = true
			})
			.RepliedWith(reply)
			.Graft();
	}

	private static void AddGalfreyWarCampRomanceDialogue(BlueprintAnswer drezenAnswer)
	{
		GraftedCue graftedCue = GraftedCue.Named("IsekaiRomanceGalfreyWarCampReply2").Says("{n}For a moment the soldier's disguise slips and the Queen is only a tired woman.{/n} \"When this war is won, Commander, wherever your home truly lies, let us lay down our armor together, if only for an hour. Until then, not a word of who I am.\"").ThenBackTo(DialogueBeats.GalfreyIncognitoHub);
		GraftedCue reply = GraftedCue.Named("IsekaiRomanceGalfreyWarCampReply").Says("{n}Galfrey glances around the tent before she answers, as if the plain armor could still give her away.{/n} \"More times than I can bear to confess to Iomedae, Commander. That is half of why I came here without the crown.\"\n\n{n}She lowers her voice.{/n} \"Everyone sees the Sunburst, the sunlit armor, the divine savior. No one sees the prison it makes. To learn that you, too, were chosen by powers beyond your control... it is heartbreaking, and a strange, quiet comfort. In this camp of thousands, you may be the only one who understands what it means to be an instrument of fate.\"").ThenContinueTo((BlueprintCue)graftedCue);
		BlueprintAnswer answer = GraftedAnswer.Named("IsekaiRomanceGalfreyWarCamp").At(DialogueBeats.GalfreyIncognitoHub).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Isekai Protagonist) \"Your Majesty... a century on the throne, carrying a continent's expectations. I did not choose this mythic mantle either. I was torn from my home and thrust into this crusade. Do you ever wish you could set the crown aside and just be yourself?\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.AddShowCondition(delegate(EtudeStatus c)
				{
					c.m_Etude = BlueprintTools.GetBlueprintReference<BlueprintEtudeReference>("c9358d866e0b3844b8d72536ca60e4b4");
					c.Playing = true;
				});
			})
			.RepliedWith(reply)
			.Graft();
		if (drezenAnswer != null && answer != null)
		{
			answer.AddShowCondition(delegate(AnswerSelected c)
			{
				c.Not = true;
				c.m_Answer = drezenAnswer.ToReference<BlueprintAnswerReference>();
			});
			drezenAnswer.AddShowCondition(delegate(AnswerSelected c)
			{
				c.Not = true;
				c.m_Answer = answer.ToReference<BlueprintAnswerReference>();
			});
		}
	}

	private static void AddCamelliaRomanceDialogue()
	{
		BlueprintUnit blueprint = BlueprintTools.GetBlueprint<BlueprintUnit>("397b090721c41044ea3220445300e1b8");
		GraftedCue graftedCue = GraftedCue.Named("IsekaiRomanceCamelliaReply2").Says("{n}She smooths her skirts, her eyes darting anywhere but at you.{/n} \"Frighten me? No. The spirits have always whispered that there was something strange about you. I simply assumed it was bad manners. If you are a stranger here, perhaps it is no wonder our 'normal' date went so poorly. Neither of us seems to know what normal is supposed to look like.\"\n\n{n}She draws a steadying breath and meets your gaze, if only for a moment.{/n} \"But that is not what I came here to say.\"").SpokenBy(blueprint)
			.ThenBackTo(DialogueBeats.CamelliaMorningAfter);
		GraftedCue reply = GraftedCue.Named("IsekaiRomanceCamelliaReply").Says("{n}Camellia's blush deepens. For a moment she seems to suspect a joke, then she studies your face and finds none.{/n} \"Not from this world? You truly have a talent for ruining a perfectly composed apology.\"").SpokenBy(blueprint)
			.ThenContinueTo((BlueprintCue)graftedCue);
		GraftedAnswer.Named("IsekaiRomanceCamellia").At(DialogueBeats.CamelliaMorningAfter).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Isekai Protagonist) \"Camellia... you wanted a 'normal' date. What if I told you I'm not even from this world? Where I come from there were no spirits or gods, just cold technology. Does that frighten you?\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.RepliedWith(reply)
			.SpokenBy(blueprint)
			.Graft();
	}

	private static void AddUlbrigRomanceDialogue()
	{
		BlueprintUnit blueprint = BlueprintTools.GetBlueprint<BlueprintUnit>("42f0d5ec3dc844feb44b04507a7c1bfc");
		GraftedCue graftedCue = GraftedCue.Named("IsekaiRomanceUlbrigReply2").Says("{n}He rests a large, calloused hand on the back of your neck, warm and certain.{/n} \"Then I met you. You fought like no warrior of the tribes, spoke of strange cities of iron and glass, and carried a sadness no priest could soothe. The spirits had not abandoned me after all. They sent me a kindred exile from beyond the sky.\"\n\n{n}A rare, earnest smile softens his weathered features as he looks back at the ancient trees.{/n} \"And now they have opened this garden to the two of us. If we are ghosts, then we walk here together.\"").SpokenBy(blueprint)
			.ThenBackTo(DialogueBeats.UlbrigInTheGardenOfGods);
		GraftedCue reply = GraftedCue.Named("IsekaiRomanceUlbrigReply").Says("{n}Ulbrig tears his gaze from the garden and looks at you, and for once the wonder leaves his voice.{/n} \"Ghosts, you say? Aye. For months after I woke, I walked the scarred plains of Sarkoris like a dead man among strangers whose speech tasted like foreign clay.\"").SpokenBy(blueprint)
			.ThenContinueTo((BlueprintCue)graftedCue);
		GraftedAnswer.Named("IsekaiRomanceUlbrig").At(DialogueBeats.UlbrigInTheGardenOfGods).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Isekai Protagonist) \"Ulbrig... you slept a century in stone while everything you loved turned to ash, and I can never go back to the world I came from. Are we both just ghosts in a world that moved on without us?\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.RepliedWith(reply)
			.SpokenBy(blueprint)
			.Graft();
	}

	private static void AddUlbrigDanceOfMasksRomanceDialogue()
	{
		BlueprintUnit blueprint = BlueprintTools.GetBlueprint<BlueprintUnit>("42f0d5ec3dc844feb44b04507a7c1bfc");
		GraftedCue reply = GraftedCue.Named("IsekaiRomanceUlbrigDanceOfMasksReply").Says("{n}Ulbrig chuckles softly, a grin wrinkling the corners of his eyes.{/n} \"No electric lights here, my clever traveler. Just clean air, old trees, and a griffon who finally found his favorite perch.\"\n\n{n}He pulls you close against his chest, wrapping an arm around your shoulders against the evening chill.{/n} \"The old world is gone, and your world across the void is far away. But the garden has accepted you, and so have I. No demons, no dead gods... just the two of us.\"").SpokenBy(blueprint)
			.ThenBackTo(DialogueBeats.UlbrigGardenDate);
		GraftedAnswer.Named("IsekaiRomanceUlbrigDanceOfMasks").At(DialogueBeats.UlbrigGardenDate).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Isekai Protagonist) \"After the masks and crowds of the city, this quiet garden feels like home. Where I come from, people chased that feeling under electric lights. With you, I finally found it.\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.RepliedWith(reply)
			.SpokenBy(blueprint)
			.Graft();
	}
}
