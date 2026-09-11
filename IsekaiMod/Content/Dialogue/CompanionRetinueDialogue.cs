using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue
{
	internal class CompanionRetinueDialogue
	{
		public static void Add()
		{
			AddRegillRetinueDialogue();
			AddArueshalaeRetinueDialogue();
			AddDaeranRetinueDialogue();
			AddWenduagRetinueDialogue();
			AddSeelahRetinueDialogue();
			AddCamelliaRetinueDialogue();
			AddLannRetinueDialogue();
		}

		private static void AddRegillRetinueDialogue()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("2366a8db6481070439fee222c0c52e45");
			BlueprintUnit regillUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("0d37024170b172346b3769df92a971f5");
			if (answersList == null)
			{
				return;
			}
			BlueprintCue reply = TTCoreExtensions.CreateCue("IsekaiRetinueRegillReply", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}Regill sets down his tactical slate, his cold amber gaze assessing you with clinical precision.{/n} \"From a strict standpoint of Chelish martial doctrine, an extradimensional tether that distorts probability borders on logistical heresy. The laws of nature should not bend to accommodate individual commanders. And yet...\"\n\n{n}A faint, tight twitch at the corner of his mouth hints at something resembling approval.{/n} \"The combat data is irrefutable. Our frontlines hold against impossible odds, strikes consistently pierce demonic damage reduction, and troop attrition has plummeted by measurable margins. If your otherworld soul provides this tactical superiority, then utilizing it ruthlessly is the only logical course. Continue leading from the vanguard, Commander. The Order will exploit this anomaly to its fullest strategic advantage.\"");
				if (regillUnit != null)
				{
					blueprintCue.Speaker = new DialogSpeaker
					{
						m_Blueprint = regillUnit.ToReference<BlueprintUnitReference>(),
						MoveCamera = true
					};
				}
				blueprintCue.Answers = answersList.Answers;
			});
			BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiRetinueRegill", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Otherworld Retinue) \"Regill, how does fighting alongside someone whose soul hails from another reality affect your discipline? Does our retinue bond align with Hellknight doctrine?\"");
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

		private static void AddArueshalaeRetinueDialogue()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("03ebad9587cbea0438d901a0f8df44f1");
			BlueprintUnit arueshalaeUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("a352873d37ec6c54c9fa8f6da3a6b3e1");
			if (answersList == null)
			{
				return;
			}
			BlueprintCue reply = TTCoreExtensions.CreateCue("IsekaiRetinueArueshalaeReply", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}Arueshalae smiles gently, a constellation of soft starlight reflecting in her eyes.{/n} \"Standing beside you feels like stepping into a waking dream where the nightmares of the Abyss can no longer touch me. When you share your otherworldly resonance with us, the noise of demonic corruption fades into silence.\"\n\n{n}She lightly touches her bowstring, watching the faint ethereal glow dancing along her fingers.{/n} \"Desna showed me that dreams can defy the darkest fate, but your presence makes that truth tangible on the battlefield. When arrows fly and reality shields our hearts, I know that no matter where you originally came from, you brought hope across the stars to save us all.\"");
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
			BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiRetinueArueshalae", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Otherworld Retinue) \"Arueshalae, what does our soul bond feel like to you? Can you sense the otherworld energies flowing through our retinue?\"");
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

		private static void AddDaeranRetinueDialogue()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("4d978cbd2aa780d46874255282039f3f");
			BlueprintUnit daeranUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("096fc4a96d675bb45a0396bcaa7aa993");
			if (answersList == null)
			{
				return;
			}
			BlueprintCue reply = TTCoreExtensions.CreateCue("IsekaiRetinueDaeranReply", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}Daeran swirls his goblet with theatrical admiration, flashing a wicked, conspiratorial grin.{/n} \"Oh, it is simply magnificent, my dear Commander! Do you know how tiring it is to look over one's shoulder expecting assassins, Inquisitors, or otherworldly entities at every turn? And then you stroll in, radiating narrative immunity like cheap cologne!\"\n\n{n}He leans back against his cushions, basking in sheer amusement.{/n} \"Swords miss by inches, curses fizzle out, and every critical blow somehow feels like a grand spectacle orchestrated for our personal amusement. If the cosmos decided to make you the star of this absurd crusade, having VIP front-row seats in your retinue is the finest luxury I could ever buy. Do keep it up, darling, dying is so dreadfully pedestrian.\"");
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
			BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiRetinueDaeran", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Otherworld Retinue) \"Daeran, you seem entirely too comfortable benefiting from otherworldly plot armor. Aren't you worried about the philosophical implications?\"");
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

		private static void AddWenduagRetinueDialogue()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("ced27e744d2dded40bbb5adf17816dbb");
			BlueprintUnit wenduagUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("ae766624c03058440a036de90a7f2009");
			if (answersList == null)
			{
				return;
			}
			BlueprintCue reply = TTCoreExtensions.CreateCue("IsekaiRetinueWenduagReply", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}Wenduag flexes her claws, her eyes burning with fierce, hungry devotion.{/n} \"In the dark caves beneath Kenabres, power was simple: kill or be eaten. Weaklings prayed to useless statues while monsters tore them apart. But what you possess is something beyond ordinary strength.\"\n\n{n}She steps close, her breath warm against your ear as she bares sharp fangs in a grin.{/n} \"Your retinue bond turns us into apex predators that reality itself fears to touch. When my arrows pierce straight through enemy illusions and shatter demon armor, I feel the heartbeat of your alien world pumping through my veins. You are the alpha whose shadow covers this whole continent, master. And I will hunt down anyone foolish enough to question your reign.\"");
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
			BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiRetinueWenduag", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Otherworld Retinue) \"Wenduag, how do you feel about the instincts and authority channeled through our retinue bond?\"");
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

		private static void AddSeelahRetinueDialogue()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("417fa384f3250634bb71859fbc913453");
			BlueprintUnit seelahUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("54be53f0b35bf3c4592a97ae335fe765");
			if (answersList == null)
			{
				return;
			}
			BlueprintCue reply = TTCoreExtensions.CreateCue("IsekaiRetinueSeelahReply", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}Seelah lets out a hearty, booming laugh, slapping her gauntleted thigh before resting a hand on her sword hilt.{/n} \"Honestly? It feels like the best tavern crew in the world, except instead of a round of ale, we share invisible shields and miraculous good luck! I used to think only the Inheritor worked miracles like this.\"\n\n{n}Her expression turns warm and earnestly loyal.{/n} \"Back on the streets of Yanmass, I never had anyone covering my back like this. But when we charge into a swarm of Vrock or Balors, I can feel your resolve pulling all of us forward, like an unseen banner that never falters. Whatever realm you came from, Commander, I am glad it sent you here. We're going to win this war, together.\"");
				if (seelahUnit != null)
				{
					blueprintCue.Speaker = new DialogSpeaker
					{
						m_Blueprint = seelahUnit.ToReference<BlueprintUnitReference>(),
						MoveCamera = true
					};
				}
				blueprintCue.Answers = answersList.Answers;
			});
			BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiRetinueSeelah", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Otherworld Retinue) \"Seelah, how does fighting alongside our retinue compare to serving in a standard paladin order?\"");
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

		private static void AddCamelliaRetinueDialogue()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("0816cfae5780af142be53f1d296a8308") ?? BlueprintTools.GetBlueprint<BlueprintAnswersList>("9497b573bde03724c8e08090509a1774");
			BlueprintUnit camelliaUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("397b090721c41044ea3220445300e1b8");
			if (answersList == null)
			{
				return;
			}
			BlueprintCue reply = TTCoreExtensions.CreateCue("IsekaiRetinueCamelliaReply", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}Camellia shivers with quiet ecstasy, her delicate fingers caressing the edge of her necklace as she leans intimately close.{/n} \"The spirits... they hum with such vibrant, untamed hunger whenever you draw near. The spirits of Golarion are ancient and heavy with sorrow, but your otherworldly essence is fresh, electric, and utterly unconstrained by moral prattle.\"\n\n{n}Her eyes glint with a crimson thrill as she whispers.{/n} \"When we fight in your retinue, each strike cuts deeper, the blood flows hotter, and that sublime warmth revitalizes my spirit immediately. You call it a protagonist bond, but to me, it is the most refined form of communion. Let us find more demons to bleed, my dear Commander... the spirits demand another feast.\"");
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
			BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiRetinueCamellia", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Otherworld Retinue) \"Camellia, what do your spirits have to say about the otherworldly power circulating through our retinue?\"");
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

		private static void AddLannRetinueDialogue()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("66385ad77fa743e4bb1234078dbd804c");
			BlueprintUnit lannUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("cb29621d99b902e4da6f5d232352fbda");
			if (answersList == null)
			{
				return;
			}
			BlueprintCue reply = TTCoreExtensions.CreateCue("IsekaiRetinueLannReply", delegate(BlueprintCue blueprintCue)
			{
				blueprintCue.SetText(Main.IsekaiContext, "{n}Lann scratches behind one of his lizard-scaled ears and chuckles dryly, spinning an arrow between his fingers with impossible speed.{/n} \"Look, Commander, mongrels usually have a life expectancy somewhere between a candle in a hurricane and a stray rat at a demonic banquet. Nobody expects us to make thirty, let alone conquer Drezen.\"\n\n{n}He grins, though his one good eye carries genuine warmth.{/n} \"Yet ever since you shared this otherworld retinue mojo with me, arrows fly twice as fast, demon claws miss by a hair, and I feel like I'm breaking every demographic statistic in the caves. If walking in your narrative shadow means surviving long enough to get old and cranky, sign me up for the long haul. Just don't ask me to start giving dramatic hero speeches, all right?\"");
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
			BlueprintAnswer bp = TTCoreExtensions.CreateAnswer("IsekaiRetinueLann", delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, "(Otherworld Retinue) \"Lann, have you noticed how our retinue bond changes your archery and battlefield survivability?\"");
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
