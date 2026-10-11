using IsekaiMod.Utilities;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.DialogSystem.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue;

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
		BlueprintUnit regillUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("0d37024170b172346b3769df92a971f5");
		GraftedCue graftedCue = GraftedCue.Named("IsekaiRetinueRegillReply2").Says("{n}A faint, tight twitch at the corner of his mouth hints at something resembling approval.{/n} \"The combat data is irrefutable. Our lines hold against impossible odds and our strikes pierce demonic damage reduction with unsettling regularity. If your otherworld soul provides this tactical superiority, then using it ruthlessly is the only logical course. Continue leading from the vanguard, Commander. I will exploit this anomaly to its fullest strategic advantage.\"").SpokenBy(regillUnit)
			.ThenBackTo(DialogueBeats.RegillHub);
		GraftedCue reply = GraftedCue.Named("IsekaiRetinueRegillReply").Says("{n}Regill sets down his tactical slate, his cold amber gaze assessing you with clinical precision.{/n} \"From a strict standpoint of Chelish martial doctrine, an extradimensional tether that distorts probability borders on logistical heresy. The laws of nature should not bend to accommodate individual commanders. And yet...\"").SpokenBy(regillUnit)
			.ThenContinueTo((BlueprintCue)graftedCue);
		GraftedAnswer.Named("IsekaiRetinueRegill").At(DialogueBeats.RegillHub).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Otherworld Retinue) \"Regill, how does fighting alongside someone whose soul hails from another reality affect your discipline? Does our retinue bond align with Hellknight doctrine?\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.Configure(delegate(BlueprintAnswer bp)
			{
				if (regillUnit != null)
				{
					bp.AddShowCondition(delegate(HasFact c)
					{
						c.Unit = new Kingmaker.Designers.EventConditionActionSystem.Evaluators.CompanionInParty
						{
							IncludeRemote = true,
							IncludeDettached = true,
							m_Companion = regillUnit.ToReference<BlueprintUnitReference>()
						};
						c.m_Fact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "RetinueProtagonistResonanceFeature");
					});
				}
			})
			.RepliedWith(reply)
			.SpokenByCompanion(regillUnit)
			.Graft();
	}

	private static void AddArueshalaeRetinueDialogue()
	{
		BlueprintUnit arueshalaeUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("a352873d37ec6c54c9fa8f6da3a6b3e1");
		GraftedCue graftedCue = GraftedCue.Named("IsekaiRetinueArueshalaeReply2").Says("{n}She lightly touches her bowstring, watching the faint ethereal glow dancing along her fingers.{/n} \"Desna showed me that dreams can defy the darkest fate, but your presence makes that truth tangible on the battlefield. When arrows fly and reality shields our hearts, I know that no matter where you originally came from, you brought hope across the stars to save us all.\"").SpokenBy(arueshalaeUnit)
			.ThenBackTo(DialogueBeats.ArueshalaeHub);
		GraftedCue reply = GraftedCue.Named("IsekaiRetinueArueshalaeReply").Says("{n}Arueshalae smiles gently, a constellation of soft starlight reflecting in her eyes.{/n} \"Standing beside you feels like stepping into a waking dream where the nightmares of the Abyss can no longer touch me. When you share your otherworldly resonance with us, the noise of demonic corruption fades into silence.\"").SpokenBy(arueshalaeUnit)
			.ThenContinueTo((BlueprintCue)graftedCue);
		GraftedAnswer.Named("IsekaiRetinueArueshalae").At(DialogueBeats.ArueshalaeHub).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Otherworld Retinue) \"Arueshalae, what does our soul bond feel like to you? Can you sense the otherworld energies flowing through our retinue?\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.Configure(delegate(BlueprintAnswer bp)
			{
				if (arueshalaeUnit != null)
				{
					bp.AddShowCondition(delegate(HasFact c)
					{
						c.Unit = new Kingmaker.Designers.EventConditionActionSystem.Evaluators.CompanionInParty
						{
							IncludeRemote = true,
							IncludeDettached = true,
							m_Companion = arueshalaeUnit.ToReference<BlueprintUnitReference>()
						};
						c.m_Fact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "RetinueProtagonistResonanceFeature");
					});
				}
			})
			.RepliedWith(reply)
			.SpokenByCompanion(arueshalaeUnit)
			.Graft();
	}

	private static void AddDaeranRetinueDialogue()
	{
		BlueprintUnit daeranUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("096fc4a96d675bb45a0396bcaa7aa993");
		GraftedCue graftedCue = GraftedCue.Named("IsekaiRetinueDaeranReply2").Says("{n}He leans back, basking in sheer amusement.{/n} \"Swords miss by inches, curses fizzle out, and every critical blow somehow feels like a grand spectacle orchestrated for our personal amusement. If the cosmos decided to make you the star of this absurd crusade, having VIP front-row seats in your retinue is the finest luxury I could ever buy. Do keep it up, darling, dying is so dreadfully pedestrian.\"").SpokenBy(daeranUnit)
			.ThenBackTo(DialogueBeats.DaeranHub);
		GraftedCue reply = GraftedCue.Named("IsekaiRetinueDaeranReply").Says("{n}Daeran swirls his goblet with theatrical admiration, flashing a wicked, conspiratorial grin.{/n} \"Oh, it is simply magnificent, my dear Commander! Do you know how tiring it is to look over one's shoulder expecting assassins, Inquisitors, or otherworldly entities at every turn? And then you stroll in, radiating narrative immunity like cheap cologne!\"").SpokenBy(daeranUnit)
			.ThenContinueTo((BlueprintCue)graftedCue);
		GraftedAnswer.Named("IsekaiRetinueDaeran").At(DialogueBeats.DaeranHub).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Otherworld Retinue) \"Daeran, you seem entirely too comfortable benefiting from otherworldly plot armor. Aren't you worried about the philosophical implications?\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.Configure(delegate(BlueprintAnswer bp)
			{
				if (daeranUnit != null)
				{
					bp.AddShowCondition(delegate(HasFact c)
					{
						c.Unit = new Kingmaker.Designers.EventConditionActionSystem.Evaluators.CompanionInParty
						{
							IncludeRemote = true,
							IncludeDettached = true,
							m_Companion = daeranUnit.ToReference<BlueprintUnitReference>()
						};
						c.m_Fact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "RetinueProtagonistResonanceFeature");
					});
				}
			})
			.RepliedWith(reply)
			.SpokenByCompanion(daeranUnit)
			.Graft();
	}

	private static void AddWenduagRetinueDialogue()
	{
		BlueprintUnit wenduagUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("ae766624c03058440a036de90a7f2009");
		GraftedCue graftedCue = GraftedCue.Named("IsekaiRetinueWenduagReply2").Says("{n}She steps close, her breath warm against your ear as she bares sharp fangs in a grin.{/n} \"Your retinue bond turns us into apex predators that reality itself fears to touch. When my arrows pierce straight through enemy illusions and shatter demon armor, I feel the heartbeat of your alien world pumping through my veins. You are the alpha whose shadow covers this whole continent, {mf|master|mistress}. And I will hunt down anyone foolish enough to question your reign.\"").SpokenBy(wenduagUnit)
			.ThenBackTo(DialogueBeats.WenduagHub);
		GraftedCue reply = GraftedCue.Named("IsekaiRetinueWenduagReply").Says("{n}Wenduag flexes her claws, her eyes burning with fierce, hungry devotion.{/n} \"In the dark caves beneath Kenabres, power was simple: kill or be eaten. Weaklings prayed to useless statues while monsters tore them apart. But what you possess is something beyond ordinary strength.\"").SpokenBy(wenduagUnit)
			.ThenContinueTo((BlueprintCue)graftedCue);
		GraftedAnswer.Named("IsekaiRetinueWenduag").At(DialogueBeats.WenduagHub).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Otherworld Retinue) \"Wenduag, how do you feel about the instincts and authority channeled through our retinue bond?\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.Configure(delegate(BlueprintAnswer bp)
			{
				if (wenduagUnit != null)
				{
					bp.AddShowCondition(delegate(HasFact c)
					{
						c.Unit = new Kingmaker.Designers.EventConditionActionSystem.Evaluators.CompanionInParty
						{
							IncludeRemote = true,
							IncludeDettached = true,
							m_Companion = wenduagUnit.ToReference<BlueprintUnitReference>()
						};
						c.m_Fact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "RetinueProtagonistResonanceFeature");
					});
				}
			})
			.RepliedWith(reply)
			.SpokenByCompanion(wenduagUnit)
			.Graft();
	}

	private static void AddSeelahRetinueDialogue()
	{
		BlueprintUnit seelahUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("54be53f0b35bf3c4592a97ae335fe765");
		GraftedCue graftedCue = GraftedCue.Named("IsekaiRetinueSeelahReply2").Says("{n}Her expression turns warm and earnestly loyal.{/n} \"Back on the streets of Solku, I never had anyone covering my back like this. But when we charge into a swarm of vrocks or balors, I can feel your resolve pulling all of us forward, like an unseen banner that never falters. Whatever realm you came from, Commander, I am glad it sent you here. We're going to win this war, together.\"").SpokenBy(seelahUnit)
			.ThenBackTo(DialogueBeats.SeelahHub);
		GraftedCue reply = GraftedCue.Named("IsekaiRetinueSeelahReply").Says("{n}Seelah lets out a hearty, booming laugh, slapping her gauntleted thigh before resting a hand on her sword hilt.{/n} \"Honestly? It feels like the best tavern crew in the world, except instead of a round of ale, we share invisible shields and miraculous good luck! I used to think only the Inheritor worked miracles like this.\"").SpokenBy(seelahUnit)
			.ThenContinueTo((BlueprintCue)graftedCue);
		GraftedAnswer.Named("IsekaiRetinueSeelah").At(DialogueBeats.SeelahHub).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Otherworld Retinue) \"Seelah, how does fighting alongside our retinue compare to serving in a standard paladin order?\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.Configure(delegate(BlueprintAnswer bp)
			{
				if (seelahUnit != null)
				{
					bp.AddShowCondition(delegate(HasFact c)
					{
						c.Unit = new Kingmaker.Designers.EventConditionActionSystem.Evaluators.CompanionInParty
						{
							IncludeRemote = true,
							IncludeDettached = true,
							m_Companion = seelahUnit.ToReference<BlueprintUnitReference>()
						};
						c.m_Fact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "RetinueProtagonistResonanceFeature");
					});
				}
			})
			.RepliedWith(reply)
			.SpokenByCompanion(seelahUnit)
			.Graft();
	}

	private static void AddCamelliaRetinueDialogue()
	{
		BlueprintUnit camelliaUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("397b090721c41044ea3220445300e1b8");
		GraftedCue graftedCue = GraftedCue.Named("IsekaiRetinueCamelliaReply2").Says("{n}Her eyes glint with a crimson thrill as she whispers.{/n} \"When we fight in your retinue, each strike cuts deeper, the blood flows hotter, and that sublime warmth revitalizes my spirit immediately. You call it a protagonist bond, but to me, it is the most refined form of communion. Let us find more demons to bleed, my dear Commander... the spirits demand another feast.\"").SpokenBy(camelliaUnit)
			.ThenBackTo(DialogueBeats.CamelliaHub);
		GraftedCue reply = GraftedCue.Named("IsekaiRetinueCamelliaReply").Says("{n}Camellia shivers with quiet ecstasy, her delicate fingers caressing the edge of her necklace as she leans intimately close.{/n} \"The spirits... they hum with such vibrant, untamed hunger whenever you draw near. The spirits of Golarion are ancient and heavy with sorrow, but your otherworldly essence is fresh, electric, and utterly unconstrained by moral prattle.\"").SpokenBy(camelliaUnit)
			.ThenContinueTo((BlueprintCue)graftedCue);
		GraftedAnswer.Named("IsekaiRetinueCamellia").At(DialogueBeats.CamelliaHub).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Otherworld Retinue) \"Camellia, what do your spirits have to say about the otherworldly power circulating through our retinue?\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.AddShowCondition(delegate(QuestStatus c)
				{
					c.m_Quest = BlueprintTools.GetBlueprintReference<BlueprintQuestReference>("189129cc7a4afa6418ef0c162598e935");
					c.State = QuestState.Completed;
				});
				if (camelliaUnit != null)
				{
					bp.AddShowCondition(delegate(HasFact c)
					{
						c.Unit = new Kingmaker.Designers.EventConditionActionSystem.Evaluators.CompanionInParty
						{
							IncludeRemote = true,
							IncludeDettached = true,
							m_Companion = camelliaUnit.ToReference<BlueprintUnitReference>()
						};
						c.m_Fact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "RetinueProtagonistResonanceFeature");
					});
				}
			})
			.RepliedWith(reply)
			.SpokenByCompanion(camelliaUnit)
			.Graft();
	}

	private static void AddLannRetinueDialogue()
	{
		BlueprintUnit lannUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("cb29621d99b902e4da6f5d232352fbda");
		GraftedCue graftedCue = GraftedCue.Named("IsekaiRetinueLannReply2").Says("{n}He grins, though his one good eye carries genuine warmth.{/n} \"Yet ever since you shared this otherworld retinue mojo with me, arrows fly twice as fast, demon claws miss by a hair, and I feel like I'm breaking every demographic statistic in the caves. If walking in your narrative shadow means surviving long enough to get old and cranky, sign me up for the long haul. Just don't ask me to start giving dramatic hero speeches, all right?\"").SpokenBy(lannUnit)
			.ThenBackTo(DialogueBeats.LannHub);
		GraftedCue reply = GraftedCue.Named("IsekaiRetinueLannReply").Says("{n}Lann scratches behind one of his lizard-scaled ears and chuckles dryly, spinning an arrow between his fingers with impossible speed.{/n} \"Look, Commander, mongrels usually have a life expectancy somewhere between a candle in a hurricane and a stray rat at a demonic banquet. Nobody expects us to make thirty, let alone storm a demon fortress.\"").SpokenBy(lannUnit)
			.ThenContinueTo((BlueprintCue)graftedCue);
		GraftedAnswer.Named("IsekaiRetinueLann").At(DialogueBeats.LannHub).Configure(delegate(BlueprintAnswer bp)
		{
			bp.RequireIsekaiClass();
		})
			.Says("(Otherworld Retinue) \"Lann, have you noticed how our retinue bond changes your archery and battlefield survivability?\"")
			.Configure(delegate(BlueprintAnswer bp)
			{
				bp.RequirePlotArmor();
			})
			.Configure(delegate(BlueprintAnswer bp)
			{
				if (lannUnit != null)
				{
					bp.AddShowCondition(delegate(HasFact c)
					{
						c.Unit = new Kingmaker.Designers.EventConditionActionSystem.Evaluators.CompanionInParty
						{
							IncludeRemote = true,
							IncludeDettached = true,
							m_Companion = lannUnit.ToReference<BlueprintUnitReference>()
						};
						c.m_Fact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "RetinueProtagonistResonanceFeature");
					});
				}
			})
			.RepliedWith(reply)
			.SpokenByCompanion(lannUnit)
			.Graft();
	}
}
