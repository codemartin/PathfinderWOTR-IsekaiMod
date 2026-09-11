using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue
{
	internal class IsekaiRomanceDialogue
	{
		public static void Add()
		{
			AddArueshalaeRomanceDialogue();
			AddDaeranRomanceDialogue();
			AddWenduagRomanceDialogue();
			AddLannRomanceDialogue();
			AddSosielRomanceDialogue();
			AddGalfreyRomanceDialogue();
			AddCamelliaRomanceDialogue();
			AddUlbrigRomanceDialogue();
			AddUlbrigDanceOfMasksRomanceDialogue();
		}

		private static void AddArueshalaeRomanceDialogue()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("03ebad9587cbea0438d901a0f8df44f1");
			BlueprintUnit arueshalaeUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("a352873d37ec6c54c9fa8f6da3a6b3e1");
			if (answersList == null)
			{
				return;
			}
			BlueprintCue reply = TTCoreExtensions.CreateCue("IsekaiRomanceArueshalaeReply", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}Arueshalae looks at you with wide, luminous eyes filled with profound understanding. She gently takes your hand, her fingers trembling slightly.{/n} \"Every single day of my life, my love. For centuries I was a creature of the Abyss, a monster woven from sin and agony. Desna touched my soul and tore me from the only existence I ever knew, casting me into a realm of light and guilt where I never felt I belonged. I used to wake up every night terrified that Elysium was an illusion and that I would awaken back in the blood-soaked mud of the Rasping Rifts.\"\n\n{n}She draws your hand to her chest, where you can feel the steady, warm beat of her heart.{/n} \"If you are from beyond the stars, then you and I are both exiles who found each other in the dark. If that distant sky ever calls you back... promise me you won't let go without taking me with you. Because wherever you are, that is the only world I ever want to belong to.\"");
				if (arueshalaeUnit != null)
				{
					blueprintCue.Speaker = new DialogSpeaker
					{
						m_Blueprint = arueshalaeUnit.ToReference<BlueprintUnitReference>(),
						MoveCamera = true
					};
				}
				blueprintCue.Answers = answersList.Answers;
			});
			BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiRomanceArueshalae", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"Arushalae... have you ever felt like you're an intruder in a world you weren't born into? Sometimes I look at the stars and dread that one morning I'll wake up back where I came from, and all of this, including us, will just vanish.\"");
				blueprintAnswer.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				blueprintAnswer.ShowOnce = true;
				blueprintAnswer.RequirePlotArmor();
			});
			answersList.Answers.Insert(0, bp.ToReference<BlueprintAnswerBaseReference>());
		}

		private static void AddDaeranRomanceDialogue()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("4d978cbd2aa780d46874255282039f3f");
			BlueprintUnit daeranUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("096fc4a96d675bb45a0396bcaa7aa993");
			if (answersList == null)
			{
				return;
			}
			BlueprintCue reply = TTCoreExtensions.CreateCue("IsekaiRomanceDaeranReply", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}Daeran raises his glass in mock salute, swirling the wine with a razor-thin smirk that doesn't quite conceal the sharpness in his eyes.{/n} \"My darling, I have treated life as a hideous stage play since I was a boy. The costumes are itchy, the script is atrocious, and the audience... well, the audience is an insatiable cosmic horror peering through the peepholes in our skull.\"\n\n{n}He lowers the glass and looks directly at you, his cynical mask slipping away for an uncharacteristically tender second.{/n} \"I noticed it the day we met, that faraway glaze in your eyes whenever the crusaders start babbling about prophecy and righteous duty. You look at Golarion the way I look at high society balls: like a bemused tourist trapped in a burning ballroom. But listen to me carefully. If some capricious author is writing this farce, they made one unforgivable blunder. They made me fall in love with the stranger from the gallery. And I intend to enjoy the performance until the curtain falls.\"");
				if (daeranUnit != null)
				{
					blueprintCue.Speaker = new DialogSpeaker
					{
						m_Blueprint = daeranUnit.ToReference<BlueprintUnitReference>(),
						MoveCamera = true
					};
				}
				blueprintCue.Answers = answersList.Answers;
			});
			BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiRomanceDaeran", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"Daeran, be honest with me. Do you ever get the feeling that none of this is real? That we're just actors playing out roles in some grand, absurd cosmic comedy?\"");
				blueprintAnswer.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				blueprintAnswer.ShowOnce = true;
				blueprintAnswer.RequirePlotArmor();
			});
			answersList.Answers.Insert(0, bp.ToReference<BlueprintAnswerBaseReference>());
		}

		private static void AddWenduagRomanceDialogue()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("40a542a78c88e684b9c8908eafa41956");
			BlueprintUnit wenduagUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("ae766624c03058440a036de90a7f2009");
			if (answersList == null)
			{
				return;
			}
			BlueprintCue reply = TTCoreExtensions.CreateCue("IsekaiRomanceWenduagReply", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}Wenduag freezes, her golden feline eyes narrowing into predatory slits. Her fingers twitch toward her quiver, but slowly uncurl as she steps close enough for you to feel her ragged breath.{/n} \"You think that makes you weak, my master? You think knowing the universe is cruel makes you frail?\"\n\n{n}A vicious, fiercely possessive smile curls across her lips.{/n} \"The underground taught me that masters and gods are beasts who only care about what you can take with your claws. If unseen entities threw you into this pit for their amusement, and you turned around and conquered their demons, butchered their terrors, and bent this continent to your will, then you are ten times the predator they could ever imagine! We are both rats who climbed out of our cages to tear out the throats of our keepers. If your gods ever come down from their stars to reclaim you... I will feather them with arrows until the sky bleeds.\"");
				if (wenduagUnit != null)
				{
					blueprintCue.Speaker = new DialogSpeaker
					{
						m_Blueprint = wenduagUnit.ToReference<BlueprintUnitReference>(),
						MoveCamera = true
					};
				}
				blueprintCue.Answers = answersList.Answers;
			});
			BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiRomanceWenduag", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"Wenduag... you worship strength because the underground showed you no mercy. But what if I told you my power isn't mine? What if I'm just an ordinary person from another realm, chosen by arbitrary cosmic spectators who watch our struggle like theater?\"");
				blueprintAnswer.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				blueprintAnswer.ShowOnce = true;
				blueprintAnswer.RequirePlotArmor();
			});
			answersList.Answers.Insert(0, bp.ToReference<BlueprintAnswerBaseReference>());
		}

		private static void AddLannRomanceDialogue()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("66385ad77fa743e4bb1234078dbd804c");
			BlueprintUnit lannUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("cb29621d99b902e4da6f5d232352fbda");
			if (answersList == null)
			{
				return;
			}
			BlueprintCue reply = TTCoreExtensions.CreateCue("IsekaiRomanceLannReply", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}Lann chuckles softly, poking the campfire with an unstrung arrow before tossing it aside. He looks up at you, his half-human, half-lizard expression softening into quiet sincerity.{/n} \"Hey... look at me. Thirty years, forty years, an immortal spark from another dimension, who's counting? Down in the caves, our lives were measured in days between demonic incursions. If I've learned anything since stepping onto the surface, it's that time is a joke. It's what you do with the time that matters.\"\n\n{n}He reaches over and firmly clasps your shoulder.{/n} \"If you get ripped back to your world of concrete and glass towers tomorrow, then today was still the greatest adventure of my short mongrel life. And hey, if whatever cosmic force thinks it can take you away without a fight, it clearly hasn't met an archer with twenty-four arrows in flight before it can even blink. You're stuck with me, Chief. Planar barriers be damned.\"");
				if (lannUnit != null)
				{
					blueprintCue.Speaker = new DialogSpeaker
					{
						m_Blueprint = lannUnit.ToReference<BlueprintUnitReference>(),
						MoveCamera = true
					};
				}
				blueprintCue.Answers = answersList.Answers;
			});
			BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiRomanceLann", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"Lann, you always joke about your mongrel lifespan... but I have the opposite nightmare. I came from a world across time and space. What if my otherworldly existence means I can't stay here, or what if I'm torn back before we can build a life together?\"");
				blueprintAnswer.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				blueprintAnswer.ShowOnce = true;
				blueprintAnswer.RequirePlotArmor();
			});
			answersList.Answers.Insert(0, bp.ToReference<BlueprintAnswerBaseReference>());
		}

		private static void AddSosielRomanceDialogue()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("129b55b8b5d50974f84f7c607d894fd0");
			BlueprintUnit sosielUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("1cbbbb892f93c3d439f8417ad7cbb6aa");
			if (answersList == null)
			{
				return;
			}
			BlueprintCue reply = TTCoreExtensions.CreateCue("IsekaiRomanceSosielReply", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}Sosiel sets down his paintbrush, wiping his pigment-stained hands on his apron. He gazes into your eyes with Shelynite warmth and calm reassurance.{/n} \"A shadow does not bleed for Kenabres. A shadow does not stand between ravenous demons and terrified children. A shadow does not look at me with the tender vulnerability you carry right now.\"\n\n{n}He gently cups your cheek, leaving a faint streak of cobalt pigment against your skin.{/n} \"Shelyn teaches us that true beauty is born from harmony, not origin. It does not matter whether your soul was forged in the Inner Sea, the Great Beyond, or a world without magic. The devotion you offer this world, and the love you share with me, is the most authentic masterpiece I have ever witnessed. You are no ghost, my dear. You are the light that brought color back to my canvas.\"");
				if (sosielUnit != null)
				{
					blueprintCue.Speaker = new DialogSpeaker
					{
						m_Blueprint = sosielUnit.ToReference<BlueprintUnitReference>(),
						MoveCamera = true
					};
				}
				blueprintCue.Answers = answersList.Answers;
			});
			BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiRomanceSosiel", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"Sosiel... when you paint, you capture the soul of Golarion. But when I look at the sky, I feel like a ghost wandering someone else's tapestry. Do you ever fear that loving an otherworldly stranger is loving a shadow?\"");
				blueprintAnswer.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				blueprintAnswer.ShowOnce = true;
				blueprintAnswer.RequirePlotArmor();
			});
			answersList.Answers.Insert(0, bp.ToReference<BlueprintAnswerBaseReference>());
		}

		private static void AddGalfreyRomanceDialogue()
		{
			BlueprintUnit galfreyUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("e46927657a79db64ea30758db3f42bb9");
			BlueprintAnswersList incognitoAnswers = BlueprintTools.GetBlueprint<BlueprintAnswersList>("f1225677b6e455a4db45f2ce77533816");
			BlueprintAnswersList drezenAnswers = BlueprintTools.GetBlueprint<BlueprintAnswersList>("44704bddb6223b84989dd26bcf20b601");
			BlueprintCue reply = TTCoreExtensions.CreateCue("IsekaiRomanceGalfreyReply", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}Galfrey's breath hitches. For a brief moment, the solemn, unyielding Queen of Mendev vanishes, replaced by an exhausted woman whose heart has carried a century of funeral pyres.{/n} \"More times than I can bear to confess to Iomedae, Commander.\"\n\n{n}She looks at you with a mixture of sorrow and profound kinship.{/n} \"Everyone sees the radiance of the Sunburst, the sunlit armor, the divine savior. No one sees the prison it creates. To know that you, too, were chosen by powers beyond your control... it is both heartbreaking and a strange, quiet comfort. In this camp of thousands, you are the only one who understands what it means to be an instrument of fate.\"\n\n{n}She steps closer, her armored hand resting gently over yours on the war table.{/n} \"When this war is won, Commander... whether your home lies across the ocean or across the stars... let us promise to lay down our armor together, if only for an hour. Just two weary souls who answered the call.\"");
				if (galfreyUnit != null)
				{
					blueprintCue.Speaker = new DialogSpeaker
					{
						m_Blueprint = galfreyUnit.ToReference<BlueprintUnitReference>(),
						MoveCamera = true
					};
				}
				if (drezenAnswers != null)
				{
					blueprintCue.Answers = drezenAnswers.Answers;
				}
				else if (incognitoAnswers != null)
				{
					blueprintCue.Answers = incognitoAnswers.Answers;
				}
			});
			BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiRomanceGalfrey", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"Your Majesty... a century on the throne, bearing the expectations of an entire continent. I must confess, I did not choose this mythic mantle either. I was ripped from my home and thrust into this crusade. Do you ever wish you could cast the crown aside and just be yourself?\"");
				blueprintAnswer.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				blueprintAnswer.ShowOnce = true;
				blueprintAnswer.RequirePlotArmor();
			});
			if (incognitoAnswers != null)
			{
				incognitoAnswers.Answers.Insert(0, bp.ToReference<BlueprintAnswerBaseReference>());
			}
			if (drezenAnswers != null)
			{
				drezenAnswers.Answers.Insert(0, bp.ToReference<BlueprintAnswerBaseReference>());
			}
		}

		private static void AddCamelliaRomanceDialogue()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("0816cfae5780af142be53f1d296a8308");
			BlueprintUnit camelliaUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("397b090721c41044ea3220445300e1b8");
			if (answersList == null)
			{
				return;
			}
			BlueprintCue reply = TTCoreExtensions.CreateCue("IsekaiRomanceCamelliaReply", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}Camellia tilts her head, her delicate lips parting in a slow, exquisitely predatory smile. She gently traces the edge of her rapier's pommel, her dark eyes gleaming with morbid fascination.{/n} \"From another world? Where spirits are dissected by machines, where blood is washed away by clinical soap, and everyone wears polite societal masks to hide their hunger?\"\n\n{n}She takes half a step closer, the floral scent of rosewater failing to mask the faint, coppery tang that always lingers around her.{/n} \"How deliciously hypocritical. You pretend to be an innocent traveler displaced by cosmic chance, yet you stride across battlefields slaughtering hundreds with that detached, calculating calm. You don't belong to Golarion... but you belong to the thrill of violence just as deeply as I do. You pretend your cheat abilities are a burden, but you love the power, don't you?\"\n\n{n}She reaches out and lightly brushes a finger against your throat, her pulse quickening.{/n} \"We are both monsters wearing noble disguises, my dear otherworlder. If your alien world ever tries to summon you back, they will find you've already acquired a taste for forbidden wine. And I have no intention of letting anyone else drink from my cup.\"");
				if (camelliaUnit != null)
				{
					blueprintCue.Speaker = new DialogSpeaker
					{
						m_Blueprint = camelliaUnit.ToReference<BlueprintUnitReference>(),
						MoveCamera = true
					};
				}
				blueprintCue.Answers = answersList.Answers;
			});
			BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiRomanceCamellia", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"Camellia... you claim you're not normal because of the spirits and your desires. But what if I told you I'm not even from this dimension? Back home, we didn't have shamanic spirits or gods, just cold technology. Does loving someone from outside reality frighten you?\"");
				blueprintAnswer.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				blueprintAnswer.ShowOnce = true;
				blueprintAnswer.RequirePlotArmor();
			});
			answersList.Answers.Insert(0, bp.ToReference<BlueprintAnswerBaseReference>());
		}

		private static void AddUlbrigRomanceDialogue()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("3c515b7b090043479164dac9160b6c9e");
			BlueprintUnit ulbrigUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("42f0d5ec3dc844feb44b04507a7c1bfc");
			if (answersList == null)
			{
				return;
			}
			BlueprintCue reply = TTCoreExtensions.CreateCue("IsekaiRomanceUlbrigReply", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}Ulbrig stops carving the wooden statuette in his hands, his amber hawk eyes fixing upon you with sudden, profound stillness. The night wind of the Sarkorian cliff rustles his feathers as he sets the knife aside.{/n} \"Ghosts, you say? Aye. For months after waking, I walked these scarred plains feeling like a dead man walking amongst strangers whose speech tasted like foreign clay.\"\n\n{n}He steps closer, his large, calloused hand reaching out to firmly cup the back of your neck with warm, protective certainty.{/n} \"Then I met you. You fought like no warrior of the tribes, spoke of strange cities built of iron and glass, and carried a sadness behind your eyes that no priest could soothe. That was when I realized the spirits had not abandoned me. They brought me a kindred exile from beyond the sky.\"\n\n{n}A rare, deeply earnest smile softens his weathered features.{/n} \"Let Golarion march into its future, my heart. If we are ghosts, then we fly together. Under Sarkorian skies or alien stars, you will never walk alone again.\"");
				if (ulbrigUnit != null)
				{
					blueprintCue.Speaker = new DialogSpeaker
					{
						m_Blueprint = ulbrigUnit.ToReference<BlueprintUnitReference>(),
						MoveCamera = true
					};
				}
				blueprintCue.Answers = answersList.Answers;
			});
			BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiRomanceUlbrig", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"Ulbrig... you survived a century frozen in stone, watching everything you loved turn to ash. I came from another world across the multiverse, knowing I can never truly go back. Do you ever feel like we're both ghosts haunting a world that moved on without us?\"");
				blueprintAnswer.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				blueprintAnswer.ShowOnce = true;
				blueprintAnswer.RequirePlotArmor();
			});
			answersList.Answers.Insert(0, bp.ToReference<BlueprintAnswerBaseReference>());
		}

		private static void AddUlbrigDanceOfMasksRomanceDialogue()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("249454ec180b44759d3c9e8e47eef6cb");
			BlueprintUnit ulbrigUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("42f0d5ec3dc844feb44b04507a7c1bfc");
			if (answersList == null)
			{
				return;
			}
			BlueprintCue reply = TTCoreExtensions.CreateCue("IsekaiRomanceUlbrigDanceOfMasksReply", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}Ulbrig chuckles softly, offering you a cup of spiced cider with a grin that wrinkles the corners of his eyes.{/n} \"No electric lights here, my clever traveler. Just good cider, warm music, and a griffon who finally found his favorite perch.\"\n\n{n}He pulls you close against his chest, wrapping his feathered arm around your shoulders against the evening chill.{/n} \"The old world is gone, and your world across the void is far away. But tonight, this city belongs to us. No demons, no dead gods... just you and me dancing beneath the masks.\"");
				if (ulbrigUnit != null)
				{
					blueprintCue.Speaker = new DialogSpeaker
					{
						m_Blueprint = ulbrigUnit.ToReference<BlueprintUnitReference>(),
						MoveCamera = true
					};
				}
				blueprintCue.Answers = answersList.Answers;
			});
			BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiRomanceUlbrigDanceOfMasks", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"Ulbrig, look at this festival. Rebuilt cobblestones, masked revelers, sweet wine... In my old world, people celebrated under electric fireworks, pretending tomorrow didn't matter either. But tonight, with you, this world actually feels like home.\"");
				blueprintAnswer.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				blueprintAnswer.ShowOnce = true;
				blueprintAnswer.RequirePlotArmor();
			});
			answersList.Answers.Insert(0, bp.ToReference<BlueprintAnswerBaseReference>());
		}
	}
}
