using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Interaction;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Arenas;

internal static class LanternKingCameo
{
	internal struct RevealCues
	{
		public BlueprintCue Reveal;

		public BlueprintCue Discount;

		public BlueprintCue Greeting;
	}

	internal const string VanillaLanternKingGuid = "8e74850f1bb94808b3c46822f6edd739";

	internal const string FallbackModelGuid = "5eb5b47aebaaf3347a146c5cd3a45e3d";

	internal const string UnitName = "LanternKingCameoUnit";

	internal const string DialogName = "LanternKingCameoDialog";

	public static BlueprintUnit Unit;

	public static BlueprintDialog Dialog;

	private static BlueprintUnit s_Dlc6Model;

	private static BlueprintUnit s_FallbackModel;

	internal static string ModelSourceGuid(bool dlc6Owned)
	{
		if (!dlc6Owned)
		{
			return "5eb5b47aebaaf3347a146c5cd3a45e3d";
		}
		return "8e74850f1bb94808b3c46822f6edd739";
	}

	internal static BlueprintUnit ModelSource(bool dlc6Owned)
	{
		object obj;
		if (!dlc6Owned)
		{
			obj = s_FallbackModel;
			if (obj == null)
			{
				return s_Dlc6Model;
			}
		}
		else
		{
			obj = s_Dlc6Model ?? s_FallbackModel;
		}
		return (BlueprintUnit)obj;
	}

	internal static void ApplyModel(bool dlc6Owned)
	{
		try
		{
			CopyModel(Unit, ModelSource(dlc6Owned));
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("[LanternKing] Could not dress the cameo: " + ex);
		}
	}

	private static void CopyModel(BlueprintUnit target, BlueprintUnit source)
	{
		if (target != null && source != null)
		{
			target.m_Portrait = source.m_Portrait;
			target.Prefab = source.Prefab;
			target.Visual = source.Visual;
			target.Gender = source.Gender;
			target.Size = source.Size;
		}
	}

	private static BlueprintUnit TryUnit(string guid)
	{
		try
		{
			return BlueprintTools.GetBlueprint<BlueprintUnit>(guid);
		}
		catch (Exception)
		{
			return null;
		}
	}

	internal static void Create()
	{
		s_Dlc6Model = TryUnit("8e74850f1bb94808b3c46822f6edd739");
		s_FallbackModel = TryUnit("5eb5b47aebaaf3347a146c5cd3a45e3d");
		bool dlc6Owned = false;
		try
		{
			dlc6Owned = IsekaiDlcManager.HasDlc6();
		}
		catch (Exception)
		{
			dlc6Owned = false;
		}
		Unit = Helpers.CreateBlueprint(Main.IsekaiContext, "LanternKingCameoUnit", delegate(BlueprintUnit bp)
		{
			bp.SetLocalisedName(Main.IsekaiContext, "The Lantern King");
			CopyModel(bp, ModelSource(dlc6Owned));
			bp.Alignment = Alignment.ChaoticNeutral;
			IsekaiNpcUnit.MakeServiceNpc(bp);
			bp.AddComponent(delegate(DialogOnClick c)
			{
				c.m_Dialog = BlueprintTools.GetModBlueprintReference<BlueprintDialogReference>(Main.IsekaiContext, "LanternKingCameoDialog");
				c.Conditions = ActionFlow.EmptyCondition();
				c.NoDialogActions = ActionFlow.DoNothing();
			});
		});
		DialogSpeaker speaker = new DialogSpeaker
		{
			m_Blueprint = Unit.ToReference<BlueprintUnitReference>(),
			MoveCamera = false
		};
		Dialog = Helpers.CreateBlueprint(Main.IsekaiContext, "LanternKingCameoDialog", delegate(BlueprintDialog bp)
		{
			bp.Type = DialogType.Common;
			bp.Conditions = new ConditionsChecker
			{
				Conditions = Array.Empty<Condition>()
			};
			BlueprintCue blueprintCue = TTCoreExtensions.CreateCue("LanternKing_CueIntro", delegate(BlueprintCue c)
			{
				c.SetText(Main.IsekaiContext, "{n}A small, grinning figure with a lantern-bright face bows so low his nose brushes the sand, then springs upright as if yanked by a string.{/n}\n\n\"Relax, relax! No knives, no curses, no tricks. Well, no tricks yet. I am only here as a fan, transmigrator. Front-row seat for the best act in the whole cosmos!\"");
				c.Speaker = speaker;
			});
			BlueprintCue cueWhy = null;
			BlueprintAnswer ansWhy = TTCoreExtensions.CreateAnswer("Ans_LanternKing_Why", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "Why are you here, Lantern King?");
				cueWhy = TTCoreExtensions.CreateCue("Cue_LanternKing_Why", delegate(BlueprintCue c)
				{
					c.SetText(Main.IsekaiContext, "\"Why? Because you are my favorite act! A soul yanked out of another world, dropped into someone else's crusade, and still stealing the show. The other constellations applaud politely. I laugh. Laughing is the higher compliment, kid.\"");
					c.Speaker = speaker;
				});
				a.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { cueWhy.ToReference<BlueprintCueBaseReference>() }
				};
			});
			BlueprintAnswer ansLeave = TTCoreExtensions.CreateAnswer("Ans_LanternKing_Leave", delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, "Enjoy the show. I have fights to win.");
				BlueprintCue bp3 = TTCoreExtensions.CreateCue("Cue_LanternKing_Leave", delegate(BlueprintCue c)
				{
					c.SetText(Main.IsekaiContext, "\"Oh, I will! Break a leg, kid. Preferably someone else's. BWAHAHA!\"\n\n{n}He tips an invisible hat and settles in to watch, perfectly harmless.{/n}");
					c.Speaker = speaker;
				});
				a.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { bp3.ToReference<BlueprintCueBaseReference>() }
				};
			});
			BlueprintAnswersList bp2 = Helpers.CreateBlueprint(Main.IsekaiContext, "LanternKing_AnswersList", delegate(BlueprintAnswersList al)
			{
				al.Answers = new List<BlueprintAnswerBaseReference>
				{
					ansWhy.ToReference<BlueprintAnswerBaseReference>(),
					ansLeave.ToReference<BlueprintAnswerBaseReference>()
				};
			});
			blueprintCue.Answers = new List<BlueprintAnswerBaseReference> { bp2.ToReference<BlueprintAnswerBaseReference>() };
			cueWhy.Answers = new List<BlueprintAnswerBaseReference> { bp2.ToReference<BlueprintAnswerBaseReference>() };
			RevealCues revealCues = CreateReveal(speaker);
			bp.FirstCue = new CueSelection
			{
				Cues = new List<BlueprintCueBaseReference>
				{
					revealCues.Reveal.ToReference<BlueprintCueBaseReference>(),
					revealCues.Discount.ToReference<BlueprintCueBaseReference>(),
					revealCues.Greeting.ToReference<BlueprintCueBaseReference>(),
					blueprintCue.ToReference<BlueprintCueBaseReference>()
				},
				Strategy = Strategy.First
			};
		});
	}

	private static CueSelection Next(BlueprintCue cue)
	{
		return new CueSelection
		{
			Cues = new List<BlueprintCueBaseReference> { cue.ToReference<BlueprintCueBaseReference>() },
			Strategy = Strategy.First
		};
	}

	private static CueSelection NextFirstOf(params BlueprintCue[] cues)
	{
		return new CueSelection
		{
			Cues = cues.Select((BlueprintCue c) => c.ToReference<BlueprintCueBaseReference>()).ToList(),
			Strategy = Strategy.First
		};
	}

	private static RevealCues CreateReveal(DialogSpeaker speaker)
	{
		BlueprintCue blueprintCue = Cue("LanternKing_CueReveal", "{n}The grinning fan stops clapping. For one heartbeat the light behind his face goes out, and something very old and very bored looks at you through the holes. Then the grin comes back, twice as wide.{/n}\n\n\"Oh, fine, FINE, the mask is off! The Lantern King, Eldest of the First World, and the Laughing King of your little celestial table, at your service. Do not look so shocked, kid. Who did you think was sponsoring the fey waves? The herald? He cannot even juggle.\"", delegate(BlueprintCue c)
		{
			c.Conditions = ActionFlow.IfSingle<ConditionLanternKingRevealPending>();
			c.OnShow = ActionFlow.DoSingle<ContextActionLanternKingReveal>();
		});
		BlueprintCue blueprintCue2 = Cue("LanternKing_CueRevealedGreeting", "{n}The Lantern King is lounging on thin air, one leg crossed over the other.{/n}\n\n\"Back again? The mask is off, kid, no need to be polite. What will it be?\"", delegate(BlueprintCue c)
		{
			c.Conditions = ActionFlow.IfSingle<ConditionLanternKingRevealed>();
		});
		BlueprintCue blueprintCue3 = Cue("LanternKing_CueDiscount", "\"Every cup! EVERY cup! You madman, you actually did it.\"\n\n{n}He pulls a scroll out of nowhere, scribbles on it with a quill of candle flame and flicks it into the air, where it burns into a little crown of stars.{/n}\n\n\"A deal is a deal, and I only break the boring ones. Tell the Cosmic Store you want to be my Avatar, and you pay half: 5,000 Cosmic Coins instead of 10,000. You still have to follow me as your patron, of course. I am generous, not stupid.\"", delegate(BlueprintCue c)
		{
			c.Conditions = ActionFlow.IfSingle<ConditionLanternKingDiscountAnnouncementPending>();
			c.OnShow = ActionFlow.DoSingle<ContextActionLanternKingAnnounceDiscount>();
		});
		BlueprintCue blueprintCue4 = Cue("LanternKing_CueStolenLandsRuler", "\"Ah, the Stolen Lands. My masterpiece! I wrote that little kingdom a fairy tale: an exiled queen, a cursed wood, a hundred tragic endings. And then that ruler of yours tore up the script, marched into the House at the End of Time and finished my Nyrissa's story on their terms instead of mine. The ratings were magnificent. I have still not forgiven them.\"\n\n{n}He sniffs the air around you, delighted.{/n}\n\n\"You carry a little of that ruler's shadow, you know. The Wild Hunt could smell it on the sands, and so could I.\"", delegate(BlueprintCue c)
		{
			c.Conditions = ActionFlow.IfSingle<ConditionLanternKingKnowsYourRuler>();
		});
		BlueprintCue blueprintCue5 = Cue("LanternKing_CueStolenLands", "\"The Stolen Lands, kid. My masterpiece! An exiled queen, a cursed wood, a string of little kingdoms rising and rotting right on cue. Then one upstart ruler refused to rot on cue, and my whole story ended in the House at the End of Time instead of on my schedule. Very heroic. Very bad for business.\"\n\n\"Those troubles are barely cold, and here you are, dressing my sands in the same old Wild Hunt. Somebody up there has a sense of humor, and it is usually me.\"");
		BlueprintCue blueprintCue6 = Cue("LanternKing_CueLoop", "\"Because it should feel familiar! You have climbed off that festival stretcher before, kid. Once, twice, a hundred times? I do not count. I only keep score. The counting is done by the one who is both the Key and the Gate, and doors are its whole business. You came through one, did you not?\"\n\n{n}He taps the side of his nose.{/n}\n\n\"Ask yourself who gains when a story is told again and again. Then ask why the audience keeps paying for tickets. That is two hints, which is two more than the Inheritor will ever give you.\"");
		BlueprintCue blueprintCue7 = Cue("LanternKing_CuePrize", "\"Every good show has a prize! And since the rest of the table will call this cheating anyway, I might as well cheat properly. Pick one. Just one! I am a trickster, not a charity.\"");
		BlueprintCue blueprintCue8 = Cue("LanternKing_CuePrizeTaken", "\"Done and dusted! Spend it on something tasteless.\"\n\n{n}He leans in and drops his voice to a conspiratorial whisper.{/n}\n\n\"And here is a secret deal, just between us and every god who is listening. Win every cup on these sands, every last one, and I will let you become my Avatar at the Cosmic Store for half the price. Half! Nobody gets half. Well. You might.\"");
		BlueprintCue blueprintCue9 = Cue("LanternKing_CuePrizeLater", "\"Suit yourself! The prize will keep. I will not, but the prize will.\"");
		BlueprintCue cue = Cue("LanternKing_CueRevealLeave", "\"Nobody ever has enough riddles! Off you go, kid. I will be watching. I am always watching; it is the only honest thing about me. BWAHAHA!\"\n\n{n}He tips an invisible hat and goes back to being a harmless fan, or doing a very good impression of one.{/n}");
		BlueprintAnswer ansStolen = Answer("Ans_LanternKing_StolenLands", "\"What happened in the Stolen Lands?\"", NextFirstOf(blueprintCue4, blueprintCue5));
		BlueprintAnswer ansLoop = Answer("Ans_LanternKing_Loop", "\"You asked whether this crusade feels familiar. Why?\"", Next(blueprintCue6));
		BlueprintAnswer ansPrize = Answer("Ans_LanternKing_Prize", "\"You mentioned prizes.\"", Next(blueprintCue7), delegate(BlueprintAnswer a)
		{
			a.ShowConditions = ActionFlow.IfSingle<ConditionLanternKingPrizeAvailable>();
		});
		BlueprintAnswer ansLeave = Answer("Ans_LanternKing_RevealLeave", "[Leave] \"I have heard enough riddles for one day.\"", Next(cue));
		BlueprintAnswersList bp = Helpers.CreateBlueprint(Main.IsekaiContext, "LanternKing_RevealAnswersList", delegate(BlueprintAnswersList al)
		{
			al.Answers = new List<BlueprintAnswerBaseReference>
			{
				ansStolen.ToReference<BlueprintAnswerBaseReference>(),
				ansLoop.ToReference<BlueprintAnswerBaseReference>(),
				ansPrize.ToReference<BlueprintAnswerBaseReference>(),
				ansLeave.ToReference<BlueprintAnswerBaseReference>()
			};
		});
		BlueprintAnswer ansCoins = Answer("Ans_LanternKing_PrizeCoins", "[Prize] The Jester's Purse: 1,500 Cosmic Coins.", Next(blueprintCue8), delegate(BlueprintAnswer a)
		{
			a.ShowConditions = ActionFlow.IfAll(new ConditionLanternKingPrizeAvailable(), new ConditionConstellationsEnabled());
			a.OnSelect = ActionFlow.DoSingle(delegate(ContextActionLanternKingPrize p)
			{
				p.Gold = false;
			});
		});
		BlueprintAnswer ansGold = Answer("Ans_LanternKing_PrizeGold", "[Prize] The Crown's Ransom: 25,000 gold.", Next(blueprintCue8), delegate(BlueprintAnswer a)
		{
			a.ShowConditions = ActionFlow.IfSingle<ConditionLanternKingPrizeAvailable>();
			a.OnSelect = ActionFlow.DoSingle(delegate(ContextActionLanternKingPrize p)
			{
				p.Gold = true;
			});
		});
		BlueprintAnswer ansLater = Answer("Ans_LanternKing_PrizeLater", "\"Let me think about it.\"", Next(blueprintCue9));
		BlueprintAnswersList bp2 = Helpers.CreateBlueprint(Main.IsekaiContext, "LanternKing_PrizeAnswersList", delegate(BlueprintAnswersList al)
		{
			al.Answers = new List<BlueprintAnswerBaseReference>
			{
				ansCoins.ToReference<BlueprintAnswerBaseReference>(),
				ansGold.ToReference<BlueprintAnswerBaseReference>(),
				ansLater.ToReference<BlueprintAnswerBaseReference>()
			};
		});
		List<BlueprintAnswerBaseReference> collection = new List<BlueprintAnswerBaseReference> { bp.ToReference<BlueprintAnswerBaseReference>() };
		BlueprintCue[] array = new BlueprintCue[8] { blueprintCue, blueprintCue2, blueprintCue3, blueprintCue4, blueprintCue5, blueprintCue6, blueprintCue8, blueprintCue9 };
		for (int num = 0; num < array.Length; num++)
		{
			array[num].Answers = new List<BlueprintAnswerBaseReference>(collection);
		}
		blueprintCue7.Answers = new List<BlueprintAnswerBaseReference> { bp2.ToReference<BlueprintAnswerBaseReference>() };
		return new RevealCues
		{
			Reveal = blueprintCue,
			Discount = blueprintCue3,
			Greeting = blueprintCue2
		};
		static BlueprintAnswer Answer(string name, string text, CueSelection next, Action<BlueprintAnswer> init = null)
		{
			return TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer a)
			{
				a.SetText(Main.IsekaiContext, text);
				a.NextCue = next;
				init?.Invoke(a);
			});
		}
		BlueprintCue Cue(string name, string text, Action<BlueprintCue> init = null)
		{
			return TTCoreExtensions.CreateCue(name, delegate(BlueprintCue c)
			{
				c.SetText(Main.IsekaiContext, text);
				c.Speaker = speaker;
				init?.Invoke(c);
			});
		}
	}
}
