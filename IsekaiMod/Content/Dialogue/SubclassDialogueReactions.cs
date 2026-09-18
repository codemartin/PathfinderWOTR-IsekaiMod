using System.Collections.Generic;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.AreaLogic.Etudes;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Dialogue
{
	internal class SubclassDialogueReactions
	{
		private static BlueprintFeature GetRequiredDialogueFact(string factName)
		{
			if (factName == "IsekaiProficiencies")
			{
				return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PlotArmor") ?? BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProficiencies");
			}
			return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, factName);
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
		}

		private static void AddHulrunReactions()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("e27807b731f3b1a4eb19c1a04fdfcf53");
			BlueprintCue dontRememberCue = BlueprintTools.GetBlueprint<BlueprintCue>("ba9c82193a32275408973a8aebdb3a6d");
			// The vanilla "I don't remember" answer starts this etude; the Aeon Kenabres flashback in chapter 5 reads it
			// to replay the choice. The archetype answers rejoin that path, so they start it too.
			BlueprintEtude dontRememberEtude = BlueprintTools.GetBlueprint<BlueprintEtude>("d6c6161d2cf0ac44786f9df67fca5ce9");
			if (answersList != null && dontRememberCue != null)
			{
				AddArchetypeHulrunAnswer("IsekaiHulrunGeneral", "(Isekai Protagonist) \"Who am I? To be completely honest, I don't think you'd believe me if I told you. Let's just say I took a wrong turn across realities and woke up bleeding on your cobblestones.\"", "{n}Hulrun narrows his cold eyes, scrutinizing your face with fierce intensity.{/n} \"'Across realities'? Either you're delirious from blood loss, or you're mocking an officer of the crusade. Terendelev vouched for you, so I will let it pass for now. Step aside.\"", "IsekaiProficiencies");
				AddArchetypeHulrunAnswer("IsekaiHulrunMartialGod", "(Martial God) \"Lower that halberd, inquisitor. I just woke up in this square, and waving polearms at recovering patients is hardly proper festival etiquette.\"", "{n}Hulrun blinks rapidly, his knuckles whitening on his halberd.{/n} \"A speedster... and an arrogant one at that. I saw no dust from your stride. Either you are blessed by an astral wind, or you bear strange foreign sorcery...\"", "MartialGodProficiencies");
				AddArchetypeHulrunAnswer("IsekaiHulrunGodEmperor", "(God Emperor) \"You speak to an emperor whose ascension will eclipse the heavens. Lower your halberd before your sovereign, inquisitor, and remember your place.\"", "{n}Hulrun stumbles backward half a pace as a radiant, suffocating pressure radiates from your mantle.{/n} \"What madness is this... an emperor? In Kenabres? The festival heat must be twisting mortal minds...\"", "GodEmperorProficiencies");
				AddArchetypeHulrunAnswer("IsekaiHulrunOverlord", "(Overlord) \"You dare interrogate an Overlord, mortal? Be grateful my unfathomable mercy permits your insignificant spark of life to continue burning.\"", "{n}Hulrun shudders as a cold, necrotic chill sweeps across the stones of the square.{/n} \"Such insolence! Such unhallowed darkness! By Iomedae's light, you reek of a tyrant from some forgotten age... but Terendelev herself vouched for your recovery, so I will not arrest you on a festival day. Mind yourself!\"", "OverlordProficiencies");
				AddArchetypeHulrunAnswer("IsekaiHulrunDevourer", "(Slime) \"Easy with the polearm, sir! I just woke up starving after that terrible carriage accident. I came to the square looking for festival skewers and pastries, not an inquisition.\"", "{n}Hulrun glares at you with stern suspicion.{/n} \"Starving? Looking for festival treats? You look completely hale thanks to Lady Terendelev, yet you speak like an insatiable wanderer. Keep your hands where the guards can see them.\"", "DevourerProficiencies");
				AddArchetypeHulrunAnswer("IsekaiHulrunShadowMonarch", "(Shadow Monarch) \"My pulse is steady and my memory is clearing, inquisitor. The only shadows following me are the ones your festival banners cast on the cobblestones. There is no need for threats.\"", "{n}The shadows beneath Hulrun's boots seem to stillness as you meet his gaze, causing the veteran inquisitor to pause.{/n} \"You speak quietly, yet there is an unnatural coldness in your eyes. If Terendelev had not tended to you with her own hands, I would have you detained for questioning. Step aside.\"", "ShadowMonarchProficiencies");
				AddArchetypeHulrunAnswer("IsekaiHulrunHero", "(Hero) \"I came here to stand with the protectors of this city, not cause trouble. Lower your halberd, inquisitor. We should be celebrating peace with the townsfolk, not drawing steel on innocent people.\"", "{n}Hulrun sighs heavily, lowering his halberd by a fraction of an inch.{/n} \"Idealistic words. Every young recruit arrives in Kenabres believing courage alone will win this crusade. But at least you speak with reverence for the peace we guard. Move along.\"", "HeroProficiencies");
				AddArchetypeHulrunAnswer("IsekaiHulrunMastermind", "(Mastermind) \"Aggressive interrogations of recently healed concussion victims yield statistically useless data, Prelate Hulrun. You glanced toward the city gates three times in sixty seconds; your defensive focus is being wasted on an unarmed stranger.\"", "{n}Hulrun glares at you with suspicion, clearly unsettled by your clinical deduction.{/n} \"Deductions and statistics? You speak like an aloof scholar from an eastern academy. Terendelev ensured you can stand, so move along before my patience runs thin.\"", "MastermindProficiencies");
			}
			void AddArchetypeHulrunAnswer(string name, string text, string cueText, string proficiencyFactName)
			{
				BlueprintFeature proficiencyFact = GetRequiredDialogueFact(proficiencyFactName);
				if (proficiencyFact != null)
				{
					BlueprintCue reply = TTCoreExtensions.CreateCue(name + "Reply", delegate(BlueprintCue bp)
					{
						bp.SetText(Main.IsekaiContext, cueText);
						bp.Continue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { dontRememberCue.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
					});
					BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
					{
						bp.SetText(Main.IsekaiContext, text);
						bp.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
						bp.ShowOnce = true;
						bp.AddShowCondition(delegate(HasFact c)
						{
							c.Unit = new PlayerCharacter();
							c.m_Fact = proficiencyFact.ToReference<BlueprintUnitFactReference>();
						});
						if (dontRememberEtude != null)
						{
							bp.OnSelect = ActionFlow.DoSingle(delegate(StartEtude c)
							{
								c.Etude = dontRememberEtude.ToReference<BlueprintEtudeReference>();
								c.Evaluate = false;
							});
						}
					});
					answersList.InsertAnswer(answer);
				}
			}
		}

		private static void AddMinaghoReactions()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("5acd8001d9f7d2443bd57fb1291a03e4");
			BlueprintCue thatsNotVeryNiceCue = BlueprintTools.GetBlueprint<BlueprintCue>("3bd9a4263d8064b49a9d1eec365807b9");
			if (answersList != null && thatsNotVeryNiceCue != null)
			{
				AddArchetypeMinaghoAnswer("IsekaiMinaghoOverlord", "(Overlord) \"A mere regional general of the Abyss? You are an insignificant insect before the absolute sovereign of the Great Tomb. Kneel and prepare to be crushed.\"", "{n}Minagho's seductive sneer freezes, a flicker of genuine bewilderment and rage flashing across her demonic features.{/n} \"Sovereign? Tomb? You babble like a concussed peasant, mortal! When I tear your heart out, I will see what kingdom lies inside your ribs!\"", "OverlordProficiencies");
				AddArchetypeMinaghoAnswer("IsekaiMinaghoGodEmperor", "(God Emperor) \"Your demonic corruption ends here, Minagho. The radiant mandate of my empire shall cleanse Kenabres and cast your filth back to the dark.\"", "{n}Minagho recoils slightly as the aura of divine sovereignty flares around you.{/n} \"An empire? Your miserable crusade has no empire left! Only ash and despair await you!\"", "GodEmperorProficiencies");
				AddArchetypeMinaghoAnswer("IsekaiMinaghoDevourer", "(Slime) \"You carry a tremendous concentration of demonic energy. I was looking for a decent meal to settle my appetite, and your magical aura smells surprisingly potent. Yield now, or you are going to become dessert.\"", "{n}Minagho recoils with disgust at your assessing, hungry gaze.{/n} \"Dessert?! What manner of repulsive creature are you?! Even the lowest dretches have more dignity than a ravenous mortal!\"", "DevourerProficiencies");
				AddArchetypeMinaghoAnswer("IsekaiMinaghoShadowMonarch", "(Shadow Monarch) \"Enjoy your fleeting victory, Minagho. When your broken corpse lies at my feet, your soul will rise to serve as a soldier in my eternal shadow army.\"", "{n}Minagho narrows her eyes, sensing the cold abyss of souls slumbering in your shadow.{/n} \"You dare speak of enslaving a daughter of the Abyss?! You will die screaming in the dark!\"", "ShadowMonarchProficiencies");
				AddArchetypeMinaghoAnswer("IsekaiMinaghoMartialGod", "(Martial God) \"You talk too much for someone whose head hasn't noticed it's already separated from her neck. Let's see if your magic can outpace a thought.\"", "{n}Minagho's lips curl into a vicious snarl.{/n} \"Fast words from meat that hasn't met the blade of Deskari! Let us see you outrun your own flayed skin!\"", "MartialGodProficiencies");
				AddArchetypeMinaghoAnswer("IsekaiMinaghoHero", "(Hero) \"You can tear down our walls, Minagho, but you can never break our bond! My friends and I will fight until every corner of Kenabres is free!\"", "{n}Minagho laughs, high and shrill.{/n} \"Friendship! Love! How charmingly pathetic! I will make necklaces from the teeth of your 'friends' before this night ends!\"", "HeroProficiencies");
				AddArchetypeMinaghoAnswer("IsekaiMinaghoMastermind", "(Mastermind) \"Your strategic bluster cannot mask the obvious flaws in your deployment, Minagho. You overextended your vanguard into Kenabres, and now you are cornered in this garrison with nowhere left to maneuver.\"", "{n}Minagho sneers coldly, though her demonic eyes dart to her flanks.{/n} \"Cornered? You arrogant little mortal! The Wardstone is in my grasp, and your pathetic crusade ends right here!\"", "MastermindProficiencies");
			}
			void AddArchetypeMinaghoAnswer(string name, string text, string cueText, string proficiencyFactName)
			{
				BlueprintFeature proficiencyFact = GetRequiredDialogueFact(proficiencyFactName);
				if (proficiencyFact != null)
				{
					BlueprintCue reply = TTCoreExtensions.CreateCue(name + "Reply", delegate(BlueprintCue bp)
					{
						bp.SetText(Main.IsekaiContext, cueText);
						bp.Continue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { thatsNotVeryNiceCue.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
					});
					BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
					{
						bp.SetText(Main.IsekaiContext, text);
						bp.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
						bp.ShowOnce = true;
						bp.AddShowCondition(delegate(HasFact c)
						{
							c.Unit = new PlayerCharacter();
							c.m_Fact = proficiencyFact.ToReference<BlueprintUnitFactReference>();
						});
					});
					answersList.InsertAnswer(answer);
				}
			}
		}

		private static void AddDefendersHeartReactions()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("871af36f2ab2b1f40b5de77976c54276");
			BlueprintUnit irabethUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("e778129f817a5fa4286e64b061df84a5");
			if (answersList != null)
			{
				AddArchetypeCouncilAnswer("IsekaiCouncilGodEmperor", "(God Emperor) \"Irabeth, Kenabres will not survive on desperate faith alone. My imperial treasury and celestial mandate shall fund the defense and steel the soldiers' wills.\"", "{n}Irabeth straightens her posture, visibly heartened by your commanding imperial confidence.{/n} \"Your resources and leadership have already kept this tavern standing. If you can organize the troops under such discipline, we might actually hold.\"", "GodEmperorProficiencies");
				AddArchetypeCouncilAnswer("IsekaiCouncilMastermind", "(Mastermind) \"Irabeth, I have analyzed the demon attack patterns against Defender's Heart. If we bottleneck them at the western barricade and deploy incendiary alchemists, we reduce casualty rates by 72%.\"", "{n}Irabeth studies your tactical diagrams with growing astonishment.{/n} \"This... this is brilliant. I don't know where you learned military logistics, but I will reposition our archers immediately.\"", "MastermindProficiencies");
			}
			void AddArchetypeCouncilAnswer(string name, string text, string cueText, string proficiencyFactName)
			{
				BlueprintFeature proficiencyFact = GetRequiredDialogueFact(proficiencyFactName);
				if (proficiencyFact != null)
				{
					BlueprintCue reply = TTCoreExtensions.CreateCue(name + "Reply", delegate(BlueprintCue bp)
					{
						bp.SetText(Main.IsekaiContext, cueText);
						if (irabethUnit != null)
						{
							bp.Speaker = new DialogSpeaker
							{
								m_Blueprint = irabethUnit.ToReference<BlueprintUnitReference>(),
								MoveCamera = true
							};
						}
						bp.SetAnswersList(answersList);
					});
					BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
					{
						bp.SetText(Main.IsekaiContext, text);
						bp.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
						bp.ShowOnce = true;
						bp.AddShowCondition(delegate(HasFact c)
						{
							c.Unit = new PlayerCharacter();
							c.m_Fact = proficiencyFact.ToReference<BlueprintUnitFactReference>();
						});
					});
					answersList.InsertAnswer(answer);
				}
			}
		}

		private static void AddWelcomeDialogueReactions()
		{
			BlueprintAnswersList stretcherAnswersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("87997a477e6a58d4aae46e26a3712825");
			BlueprintCue waterCue = BlueprintTools.GetBlueprint<BlueprintCue>("9ac3764dd79c3854e88d601e6162bfd1");
			if (stretcherAnswersList != null && waterCue != null)
			{
				AddStretcherAnswer("IsekaiStretcherGeneral", "(Isekai Protagonist) \"Status Window... Open! Wait, where is my HUD? Where are my cheat skills?! Ugh, my chest hurts...\"", "{n}The lady in white leans over you with gentle concern, placing a cool flask of water to your lips.{/n} \"Hush now, traveler. Do not strain yourself speaking strange words from your homeland. Drink this water, and let me tend to your wounds.\"", "IsekaiProficiencies");
				AddStretcherAnswer("IsekaiStretcherSlime", "(Slime) \"I'm not water... I'm a slime! ...Wait, my human hands are still here? Whew. But my throat is completely parched...\"", "{n}The noblewoman in white robes tilts her head with a faint, bemused smile, offering a flask of cool spring water.{/n} \"A slime? That blow to your chest has left you feverish, friend. Drink, and let divine grace mend your body.\"", "DevourerProficiencies");
				AddStretcherAnswer("IsekaiStretcherOverlord", "(Overlord) \"Ugh... did the server shut down? Where are the Floor Guardians?! What is this fragile mortal body?!\"", "{n}The silver-haired lady softly places a soothing hand upon your brow.{/n} \"Rest easy, traveler. Your mind wanders in the grip of lethal poison. Drink this water while holy light restores your strength.\"", "OverlordProficiencies");
				AddStretcherAnswer("IsekaiStretcherShadowMonarch", "(Shadow Monarch) \"The system... reawakened me here? What is this burning laceration across my chest...?\"", "{n}The healer in white robes offers a gentle, reassuring smile as she tilts a flask to your lips.{/n} \"Save your strength. Whatever darkness touched your heart, the dawn is near. Drink.\"", "ShadowMonarchProficiencies");
			}
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("0f7dddeb3f77a4f408e7dc843b9c66fb");
			BlueprintCue continueCue = BlueprintTools.GetBlueprint<BlueprintCue>("70cd3ebac6e5daa41b949c0347b6ba57");
			if (answersList != null && continueCue != null)
			{
				BlueprintCue dejaVuReply = TTCoreExtensions.CreateCue("IsekaiWelcomeDejaVuReply", delegate(BlueprintCue bp)
				{
					bp.SetText(Main.IsekaiContext, "{n}The silver-haired noblewoman pauses, her eyes widening slightly in curious contemplation before she offers a serene, enigmatic smile.{/n} \"A sensation of having lived this moment before? Time flows in mysterious currents, stranger. Perhaps your destiny here was written long before today. Stand tall, for whatever cycle you have traversed, your journey in Kenabres begins anew.\"");
					bp.Continue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { continueCue.ToReference<BlueprintCueBaseReference>() },
						Strategy = Strategy.First
					};
				});
				BlueprintAnswer answer = TTCoreExtensions.CreateAnswer("IsekaiWelcomeDejaVu", delegate(BlueprintAnswer bp)
				{
					bp.SetText(Main.IsekaiContext, "[Deja Vu: Memory of Past Cycles] \"Wait... this square, the festival banner, your gentle touch... I remember this waking moment. I have walked this crusade before in another cycle. Am I trapped in an eternal loop?\"");
					bp.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { dejaVuReply.ToReference<BlueprintCueBaseReference>() },
						Strategy = Strategy.First
					};
					bp.ShowOnce = true;
					bp.AddShowCondition<ConditionHasPastCycles>();
					bp.OnSelect = ActionFlow.DoSingle<ContextActionPastCycleReflectionBanter>();
				});
				answersList.InsertAnswer(answer);
				AddWelcomeAnswer("IsekaiWelcomeGeneral", "(Isekai Protagonist) \"I'm alive...? I don't know what kind of miraculous field medicine you just used, my lady, but a moment ago I was coughing blood. Thank you. I owe you my life.\"", "{n}The silver-haired noblewoman in white robes smiles with serene warmth, her touch radiating a calming, gentle glow.{/n} \"You owe me nothing. In Kenabres, no one is left behind while hope still endures. Save your strength; the festival is just beginning.\"", "IsekaiProficiencies");
				AddWelcomeAnswer("IsekaiWelcomeDevourer", "(Slime) \"Whoa... the burning agony in my stomach is completely gone. That soothing healing aura... you're a remarkably kind doctor, miss. Thank you for patching me up!\"", "{n}The lady in white smiles gently at your earnest relief, an amused twinkle in her pale eyes.{/n} \"A doctor? A flattering title. Rest easy, friend. Your vitality is remarkably resilient; whatever poison touched your chest has dissolved into nothing.\"", "DevourerProficiencies");
				AddWelcomeAnswer("IsekaiWelcomeShadowMonarch", "(Shadow Monarch) \"The cold grip of death in my chest... lifted. Your restorative power was swift and absolute, my lady. I will remember this debt.\"", "{n}The noblewoman in white robes nods gently, her silver gaze holding ancient depth and compassion.{/n} \"Do not speak of debts, traveler. Kenabres stands united under the heavens. Stand tall and celebrate your recovery with our people.\"", "ShadowMonarchProficiencies");
				AddWelcomeAnswer("IsekaiWelcomeMastermind", "(Mastermind) \"Cellular tissue restoration completed in under three seconds with zero scarring... Remarkable medical craft, my lady. My cognitive faculties are fully restored. I am in your debt.\"", "{n}The silver-haired lady blinks in bemusement at your clinical vocabulary, then smiles gracefully.{/n} \"Such clinical detachment for someone who just stared into the abyss! Keep that keen mind guarded; clarity is a rare and precious gift in these troubled times.\"", "MastermindProficiencies");
				AddWelcomeAnswer("IsekaiWelcomeMartialGod", "(Martial God) \"Lethal chest laceration closed without a single suture... your triage technique is extraordinary, healer. Where I'm from, emergency rooms take hours to pull off miracles like this.\"", "{n}She tilts her head with an amused glimmer in her pale eyes.{/n} \"'Emergency rooms'? A curious term from distant lands. But I am pleased divine grace reached your heart in time.\"", "MartialGodProficiencies");
				AddWelcomeAnswer("IsekaiWelcomeHero", "(Hero) \"Thank you... your gentle hands pulled me back from the brink. When I opened my eyes and saw you standing over me in radiant white robes, I thought an angel had descended from the heavens.\"", "{n}A soft blush touches the lady's fair cheeks as she gives a musical laugh.{/n} \"Hardly an angel! Just a guardian watching over this fair city. Take heart, brave soul! The festival needs such bright spirits.\"", "HeroProficiencies");
				AddWelcomeAnswer("IsekaiWelcomeGodEmperor", "(God Emperor) \"A magnificent restoration. You possess hands blessed by true celestial grace, noble healer. When my sovereign dominion is established, your benevolence shall not be forgotten.\"", "{n}The lady in white arches an eyebrow with dignified amusement.{/n} \"A regal demeanor even with the dust of the road on your tunic! May your high ambitions serve peace and justice, young traveler.\"", "GodEmperorProficiencies");
				AddWelcomeAnswer("IsekaiWelcomeOverlord", "(Overlord) \"To reverse fatal trauma with a mere touch... admirable proficiency, healer. You have preserved a vessel destined for supreme dominion.\"", "{n}She offers a calm, knowing smile, unfazed by your dark demeanor.{/n} \"Every life holds supreme destiny in the grand tapestry of fate. Go forth and use this second chance well.\"", "OverlordProficiencies");
			}
			void AddStretcherAnswer(string name, string text, string cueText, string proficiencyFactName)
			{
				BlueprintFeature proficiencyFact = GetRequiredDialogueFact(proficiencyFactName);
				if (proficiencyFact != null)
				{
					BlueprintCue reply = TTCoreExtensions.CreateCue(name + "Reply", delegate(BlueprintCue bp)
					{
						bp.SetText(Main.IsekaiContext, cueText);
						bp.Continue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { waterCue.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
					});
					BlueprintAnswer answer2 = TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
					{
						bp.SetText(Main.IsekaiContext, text);
						bp.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
						bp.ShowOnce = true;
						bp.AddShowCondition(delegate(HasFact c)
						{
							c.Unit = new PlayerCharacter();
							c.m_Fact = proficiencyFact.ToReference<BlueprintUnitFactReference>();
						});
					});
					stretcherAnswersList.InsertAnswer(answer2);
				}
			}
			void AddWelcomeAnswer(string name, string text, string cueText, string proficiencyFactName)
			{
				BlueprintFeature proficiencyFact = GetRequiredDialogueFact(proficiencyFactName);
				if (proficiencyFact != null)
				{
					BlueprintCue reply = TTCoreExtensions.CreateCue(name + "Reply", delegate(BlueprintCue bp)
					{
						bp.SetText(Main.IsekaiContext, cueText);
						bp.Continue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { continueCue.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
					});
					BlueprintAnswer answer2 = TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
					{
						bp.SetText(Main.IsekaiContext, text);
						bp.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
						bp.ShowOnce = true;
						bp.AddShowCondition(delegate(HasFact c)
						{
							c.Unit = new PlayerCharacter();
							c.m_Fact = proficiencyFact.ToReference<BlueprintUnitFactReference>();
						});
					});
					answersList.InsertAnswer(answer2);
				}
			}
		}

		private static void AddMeetCameliaReactions()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("1ca6cf08fceeac141a0df689cecc784a");
			BlueprintUnitReference camelliaRef;
			if (answersList != null)
			{
				camelliaRef = BlueprintTools.GetBlueprint<BlueprintUnit>("397b090721c41044ea3220445300e1b8")?.ToReference<BlueprintUnitReference>();
				AddMeetCameliaAnswer("IsekaiCameliaGeneral", "(Isekai Protagonist) \"Did... did you see that giant silver dragon up there?! In my world, dragons are myths and fantasy legends! That thing was REAL! And that demon with the giant scythe... holy crap, are you alright?!\"", "{n}Camellia wipes a smudge of dirt from her pale cheek, giving you a cool, calculating look.{/n} \"Of course Terendelev was real. She was the ancient protector of Kenabres. But dwelling on her fall won't pull us out of this pit. Stand up; panicking like a startled peasant will get us killed down here.\"", "IsekaiProficiencies");
				AddMeetCameliaAnswer("IsekaiCameliaDevourer", "(Slime) \"The ground collapsed... but more importantly, that giant demon lord just executed the nice lady who was also a dragon?! What kind of insane death world have I reincarnated into?!\"", "{n}Camellia shivers slightly, eyeing you with restrained disdain and curiosity.{/n} \"Reincarnated? You hit your head quite hard, didn't you? Get your bearings. We need to find a way back to the surface before whatever lurks down here smells our blood.\"", "DevourerProficiencies");
				AddMeetCameliaAnswer("IsekaiCameliaShadowMonarch", "(Shadow Monarch) \"A mythical silver dragon slaughtered in seconds. The power scale in this realm is monstrous... but panic changes nothing. Are you able to walk?\"", "{n}Camellia arches an eyebrow, surprised by your sudden, chilling calm.{/n} \"A remarkably quick recovery from terror. Yes, I can walk. Follow me, and let us hope the rubble hasn't sealed every exit.\"", "ShadowMonarchProficiencies");
				AddMeetCameliaAnswer("IsekaiCameliaMastermind", "(Mastermind) \"Subterranean stratum collapsed; draconic guardian decapitated by an abyssal sovereign in under forty seconds. Panic yields zero tactical value. Can you fight, or are you non-combatant personnel?\"", "{n}Camellia blinks, momentarily taken aback by your icy clinical composure.{/n} \"Non-combatant? I assure you, I am more than capable of defending myself. Keep that analytical tongue focused on finding our way out.\"", "MastermindProficiencies");
				AddMeetCameliaAnswer("IsekaiCameliaHero", "(Hero) \"The lady who healed me... she gave her life protecting the whole city. I won't let her sacrifice be in vain. Don't worry, miss, I'll protect you until we find a way out!\"", "{n}Camellia smiles with faint, mocking amusement.{/n} \"How gallantly naive. I survived the fall without a scratch, 'hero.' Keep your blade ready for demons, not posturing.\"", "HeroProficiencies");
			}
			void AddMeetCameliaAnswer(string name, string text, string cueText, string proficiencyFactName)
			{
				BlueprintFeature proficiencyFact = GetRequiredDialogueFact(proficiencyFactName);
				if (proficiencyFact != null)
				{
					BlueprintCue reply = TTCoreExtensions.CreateCue(name + "Reply", delegate(BlueprintCue bp)
					{
						if (camelliaRef != null)
						{
							bp.Speaker = new DialogSpeaker
							{
								m_Blueprint = camelliaRef,
								MoveCamera = true
							};
						}
						bp.SetText(Main.IsekaiContext, cueText);
						bp.SetAnswersList(answersList);
					});
					BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
					{
						bp.SetText(Main.IsekaiContext, text);
						bp.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
						bp.ShowOnce = true;
						bp.AddShowCondition(delegate(HasFact c)
						{
							c.Unit = new PlayerCharacter();
							c.m_Fact = proficiencyFact.ToReference<BlueprintUnitFactReference>();
						});
					});
					answersList.InsertAnswer(answer);
				}
			}
		}

		private static void AddMeetSeelahAneviaReactions()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("a55fc20c6f0ff56439b40d6ba53cb8d7");
			BlueprintCue cue0012 = BlueprintTools.GetBlueprint<BlueprintCue>("36cac4b77d7e15e4e8c69a4340717cad");
			if (answersList == null || cue0012 == null)
			{
				return;
			}
			BlueprintUnitReference seelahRef = BlueprintTools.GetBlueprint<BlueprintUnit>("54be53f0b35bf3c4592a97ae335fe765")?.ToReference<BlueprintUnitReference>();
			BlueprintFeature generalProficiency = GetRequiredDialogueFact("IsekaiProficiencies");
			if (generalProficiency != null)
			{
				BlueprintCue generalReply = TTCoreExtensions.CreateCue("IsekaiSeelahGeneralReply", delegate(BlueprintCue bp)
				{
					if (seelahRef != null)
					{
						bp.Speaker = new DialogSpeaker
						{
							m_Blueprint = seelahRef,
							MoveCamera = true
						};
					}
					bp.SetText(Main.IsekaiContext, "{n}Seelah grunts as she heaves against the stone, her eyes brimming with grief.{/n} \"Terendelev was our guardian... the shining soul of Kenabres. Seeing Deskari strike her down tore the heart out of every crusader in that square. But we can't let her sacrifice be in vain! On three... HEAVE!\"");
					bp.Continue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { cue0012.ToReference<BlueprintCueBaseReference>() },
						Strategy = Strategy.First
					};
				});
				BlueprintAnswer answer = TTCoreExtensions.CreateAnswer("IsekaiSeelahGeneral", delegate(BlueprintAnswer bp)
				{
					bp.SetText(Main.IsekaiContext, "(Isekai Protagonist) \"Hold on, let me help you lift that rock! But please tell me I didn't hallucinate that... the kind lady who healed me earlier was a literal SILVER DRAGON?! Dragons actually exist here?!\"");
					bp.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { generalReply.ToReference<BlueprintCueBaseReference>() },
						Strategy = Strategy.First
					};
					bp.ShowOnce = true;
					bp.AddShowCondition(delegate(HasFact c)
					{
						c.Unit = new PlayerCharacter();
						c.m_Fact = generalProficiency.ToReference<BlueprintUnitFactReference>();
					});
				});
				answersList.InsertAnswer(answer);
			}
			BlueprintFeature mastermindProficiency = GetRequiredDialogueFact("MastermindProficiencies");
			if (mastermindProficiency != null)
			{
				BlueprintCue mastermindReply = TTCoreExtensions.CreateCue("IsekaiSeelahMastermindReply", delegate(BlueprintCue bp)
				{
					if (seelahRef != null)
					{
						bp.Speaker = new DialogSpeaker
						{
							m_Blueprint = seelahRef,
							MoveCamera = true
						};
					}
					bp.SetText(Main.IsekaiContext, "{n}Seelah stares at you in complete disbelief.{/n} \"'Tactical viability'?! Anevia's leg is crushed and Terendelev is dead, you clinical weirdo! Help me lift this damn rock already!\"");
					bp.SetAnswersList(answersList);
				});
				BlueprintAnswer answer2 = TTCoreExtensions.CreateAnswer("IsekaiSeelahMastermind", delegate(BlueprintAnswer bp)
				{
					bp.SetText(Main.IsekaiContext, "(Mastermind) \"A silver dragon bisected by a demon lord of the outer rifts. The geopolitical balance of Kenabres collapsed in under forty seconds. We must extract the injured immediately to preserve tactical viability.\"");
					bp.NextCue = new CueSelection
					{
						Cues = new List<BlueprintCueBaseReference> { mastermindReply.ToReference<BlueprintCueBaseReference>() },
						Strategy = Strategy.First
					};
					bp.ShowOnce = true;
					bp.AddShowCondition(delegate(HasFact c)
					{
						c.Unit = new PlayerCharacter();
						c.m_Fact = mastermindProficiency.ToReference<BlueprintUnitFactReference>();
					});
				});
				answersList.InsertAnswer(answer2);
			}
			BlueprintCue talkBackReply = TTCoreExtensions.CreateCue("IsekaiTalkBackStarsReply", delegate(BlueprintCue bp)
			{
				if (seelahRef != null)
				{
					bp.Speaker = new DialogSpeaker
					{
						m_Blueprint = seelahRef,
						MoveCamera = true
					};
				}
				bp.SetText(Main.IsekaiContext, "{n}Seelah glances up at the jagged cavern ceiling, then reaches out to touch your forehead with genuine concern.{/n} \"Whoa... hey, easy! You took a nasty blow to the skull when the square collapsed. There's nobody up there but limestone and dripping water. Stay with me, alright? Don't lose your mind now.\"\n\n{n}Camellia takes half a step back, her hand subtly resting near her rapier.{/n} \"Raving at unseen voices in the ceiling? How utterly unhinged... I had hoped for a competent guide, not a lunatic suffering from cranial fractures.\"\n\n{n}Anevia winces through her broken leg, offering a dry grimace.{/n} \"Voices in the sky betting on us? Honestly, kid, after seeing Deskari chop a dragon in half today, gods gambling on our corpses wouldn't even be in my top five weirdest crusader stories. Just help Seelah lift this rock.\"");
				bp.SetAnswersList(answersList);
			});
			BlueprintAnswer answer3 = TTCoreExtensions.CreateAnswer("IsekaiTalkBackStarsAnswer", delegate(BlueprintAnswer bp)
			{
				bp.SetText(Main.IsekaiContext, "[Look Upward] \"Shut up, all of you! Stop betting coins on my survival like I'm a gladiator in your cosmic circus! I can hear you mocking me from the clouds!\"");
				bp.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { talkBackReply.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				bp.ShowOnce = true;
				bp.OnSelect = ActionFlow.DoSingle(delegate(ContextActionAwardCosmicCoins a)
				{
					a.Amount = 250;
					a.Sponsor = "The Laughing King";
				});
				bp.RequirePlotArmor();
			});
			answersList.InsertAnswer(answer3);
			BlueprintCue slimeReply = TTCoreExtensions.CreateCue("IsekaiSeelahDevourerReply", delegate(BlueprintCue bp)
			{
				if (seelahRef != null)
				{
					bp.Speaker = new DialogSpeaker
					{
						m_Blueprint = seelahRef,
						MoveCamera = true
					};
				}
				bp.SetText(Main.IsekaiContext, "{n}Seelah stumbles backward, her mouth falling open in utter disbelief as your arm briefly turns into a fluid ribbon of green slime, slipping beneath the boulder with effortless hydraulic power.{/n} \"By Iomedae's shield... did your arm just melt into emerald jelly?! What kind of creature are you?! You're not secretly some subterranean demon parasite, are you?! Stand back, let me help you lift! On three... HEAVE!\"");
				bp.Continue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { cue0012.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
			});
			BlueprintAnswer answer4 = TTCoreExtensions.CreateAnswer("IsekaiSeelahDevourer", delegate(BlueprintAnswer bp)
			{
				bp.SetText(Main.IsekaiContext, "(Slime) \"Stand back, let me handle this. [Your arm briefly dissolves into a fluid ribbon of green slime, slipping under the boulder and heaving it upward with effortless hydraulic pressure.]\"");
				bp.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { slimeReply.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				bp.ShowOnce = true;
				bp.AddShowCondition(delegate(HasFact c)
				{
					c.Unit = new PlayerCharacter();
					c.m_Fact = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DevourerProficiencies")?.ToReference<BlueprintUnitFactReference>();
				});
				bp.OnSelect = ActionFlow.DoSingle<ContextActionSlimeRevealBanter>();
			});
			answersList.InsertAnswer(answer4);
			BlueprintCue overlordReply = TTCoreExtensions.CreateCue("IsekaiSeelahOverlordReply", delegate(BlueprintCue bp)
			{
				if (seelahRef != null)
				{
					bp.Speaker = new DialogSpeaker
					{
						m_Blueprint = seelahRef,
						MoveCamera = true
					};
				}
				bp.SetText(Main.IsekaiContext, "{n}Seelah violently flinches back, her hand instantly flying to the pommel of her holy sword with wide, terrified eyes as skeletal ivory hands grip the underside of the boulder.{/n} \"Your hands... your face! For a second you looked like... a walking skeleton! What kind of dark sorcery is this?! Stand back before... wait, you're heaving the entire rock off her! On three... HEAVE!\"");
				bp.Continue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { cue0012.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
			});
			BlueprintAnswer answer5 = TTCoreExtensions.CreateAnswer("IsekaiSeelahOverlord", delegate(BlueprintAnswer bp)
			{
				bp.SetText(Main.IsekaiContext, "(Overlord) \"Step aside, crusader. Your mortal straining is inefficient. Allow true necromantic might to handle this. [You grip the stone. For a split second, your mortal disguise flickers, revealing cold ivory bones and burning red eye sockets heaving the rock aside.]\"");
				bp.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference> { overlordReply.ToReference<BlueprintCueBaseReference>() },
					Strategy = Strategy.First
				};
				bp.ShowOnce = true;
				bp.AddShowCondition(delegate(HasFact c)
				{
					c.Unit = new PlayerCharacter();
					c.m_Fact = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "OverlordProficiencies")?.ToReference<BlueprintUnitFactReference>();
				});
				bp.OnSelect = ActionFlow.DoSingle<ContextActionOverlordRevealBanter>();
			});
			answersList.InsertAnswer(answer5);
		}

		private static void AddMeetLannReactions()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("61ba231b3ad6b144a918c88ca49cb92a");
			BlueprintUnitReference lannRef;
			if (answersList != null)
			{
				lannRef = BlueprintTools.GetBlueprint<BlueprintUnit>("cb29621d99b902e4da6f5d232352fbda")?.ToReference<BlueprintUnitReference>();
				AddMeetLannAnswer("IsekaiLannGeneral", "(Isekai Protagonist) \"Lizard scales, feathers, and horns... honestly, after falling a mile into the earth and watching a dragon fight, beastfolk scouts don't even phase me. Greetings from the surface.\"", "{n}Lann blinks his reptilian eye in surprise, then grins lopsidedly.{/n} \"Well, that's refreshing! Usually surface dwellers scream, pray to Iomedae, or try to stab us. I'm Lann, by the way.\"", "IsekaiProficiencies");
				AddMeetLannAnswer("IsekaiLannDevourer", "(Slime) \"Whoa, chimera-like anatomy! Are those scales adapted for subterranean dampness? Fascinating biology!\"", "{n}Lann scratches his scaled cheek, chuckling.{/n} \"'Fascinating biology'? First time anyone's called our cursed mongrel hides a scientific marvel! You're a weird one, surface-dweller.\"", "DevourerProficiencies");
			}
			void AddMeetLannAnswer(string name, string text, string cueText, string proficiencyFactName)
			{
				BlueprintFeature proficiencyFact = GetRequiredDialogueFact(proficiencyFactName);
				if (proficiencyFact != null)
				{
					BlueprintCue reply = TTCoreExtensions.CreateCue(name + "Reply", delegate(BlueprintCue bp)
					{
						if (lannRef != null)
						{
							bp.Speaker = new DialogSpeaker
							{
								m_Blueprint = lannRef,
								MoveCamera = true
							};
						}
						bp.SetText(Main.IsekaiContext, cueText);
						bp.SetAnswersList(answersList);
					});
					BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
					{
						bp.SetText(Main.IsekaiContext, text);
						bp.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
						bp.ShowOnce = true;
						bp.AddShowCondition(delegate(HasFact c)
						{
							c.Unit = new PlayerCharacter();
							c.m_Fact = proficiencyFact.ToReference<BlueprintUnitFactReference>();
						});
					});
					answersList.InsertAnswer(answer);
				}
			}
		}

		private static void AddWardstoneMythicReactions()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("be1fcfe08bbae9a46b5105c4d1e4be7c");
			BlueprintCue continueCue = BlueprintTools.GetBlueprint<BlueprintCue>("8b9d4bbc8ac6cb74fb75a37fe28a6431");
			if (answersList != null && continueCue != null)
			{
				AddWardstoneAnswer("IsekaiWardstoneMythicGeneral", "(Isekai Protagonist) [Channel Unscripted Power] \"You thought this was a predictable script? Let's rewrite it right here and now. Feel free to scream, Minagho.\"", "{n}The celestial flames and cosmic starlight swirl violently around your fingertips, shattering the ambient demonic miasma in the garrison hall with blinding brilliance. Minagho stumbles backward, her claws scraping futilely at her chest as raw terror drains the color from her face.{/n} \"What... what manner of power is this?! This wasn't supposed to happen! Who are you?!\"", "IsekaiProficiencies");
				AddWardstoneAnswer("IsekaiWardstoneMythicGodEmperor", "(God Emperor) [Unleash Sovereign Radiance] \"Mortal, angel, demon, it matters not. Today, the mantle of absolute sovereignty descends upon this garrison. Kneel before your emperor, Minagho!\"", "{n}Blinding golden light erupts from your chest, towering above the fractured Wardstone in the form of a celestial throne and crown. Minagho shields her eyes with a shriek of agony, the holy radiance searing her fiendish skin.{/n} \"A crown?! An emperor?! What is this suffocating light?!\"", "GodEmperorProficiencies");
				AddWardstoneAnswer("IsekaiWardstoneMythicDevourer", "(Slime) [Engulf the Cosmic Resonance] \"That obelisk contained so much concentrated essence... and now it's mine. Every demon in Kenabres is just calories waiting to be digested.\"", "{n}A vortex of pure, insatiable hunger spirals out from your open palm, pulling the demonic fog and magic of the room into your chest. Minagho recoils in horror as she feels her very soul being tugged toward the void.{/n} \"You... you aren't just resisting corruption... you're eating it?! What kind of monster are you?!\"", "DevourerProficiencies");
				AddWardstoneAnswer("IsekaiWardstoneMythicShadowMonarch", "(Shadow Monarch) [Raise the Abyssal Veil] \"The Wardstone was a beacon of light. But true power awakens in the abyss between the stars. Rise, shadows of the fallen, and witness your monarch!\"", "{n}The bright flash of the Wardstone twists into chilling, pitch-black sovereign shade. The shadows cast by the pillars and fallen crusaders stand upright, kneeling toward you in silent loyalty. Minagho trembles violently.{/n} \"The shadows... they are obeying you?! This power has nothing to do with the heavens!\"", "ShadowMonarchProficiencies");
				AddWardstoneAnswer("IsekaiWardstoneMythicOverlord", "(Overlord) [Proclaim Absolute Dominance] \"Did you truly believe you could corrupt an artifact in my presence? You are a pathetic ant crawling across my domain. Die, insect.\"", "{n}A crushing wave of tyrannical necrotic and celestial pressure slams Minagho into the flagstones. Her knees shatter against the stone as she gasps for breath under your suffocating presence.{/n} \"Impossible... a mortal cannot possess such suffocating authority! Who gave you this strength?!\"", "OverlordProficiencies");
				AddWardstoneAnswer("IsekaiWardstoneMythicMastermind", "(Mastermind) [Synthesize the Harmonic Paradox] \"Fascinating. 4,096 causal branches just collapsed into a singular deterministic triumph. Your defeat was calculated before we breached the threshold, Minagho.\"", "{n}Geometrical matrices of luminous light hover around your eyes as the residual Wardstone energy synchronizes perfectly with your mental focus. Minagho clutches her head in pain.{/n} \"Matrices? Calculations?! You speak as if this whole battle was just an equation! Die, arrogant fool!\"", "MastermindProficiencies");
				AddWardstoneAnswer("IsekaiWardstoneMythicHero", "(Hero) [Ignite the Beacon of Hope] \"The people of Kenabres bled for this stone, and their hope will not be snuffed out by demons! Together, we will cleanse every corner of this land!\"", "{n}A radiant pillar of pure dawnfire engulfs the room, washing away every trace of demon stench and mending the crusaders' wounds. Minagho howls in fury and dread.{/n} \"Hope?! That sentimental garbage is burning through my flesh! Stop it! Tear them to pieces!\"", "HeroProficiencies");
			}
			void AddWardstoneAnswer(string name, string text, string cueText, string proficiencyFactName)
			{
				BlueprintFeature proficiencyFact = GetRequiredDialogueFact(proficiencyFactName);
				if (proficiencyFact != null)
				{
					BlueprintCue reply = TTCoreExtensions.CreateCue(name + "Reply", delegate(BlueprintCue bp)
					{
						bp.SetText(Main.IsekaiContext, cueText);
						bp.Continue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { continueCue.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
					});
					BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
					{
						bp.SetText(Main.IsekaiContext, text);
						bp.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
						bp.ShowOnce = true;
						bp.AddShowCondition(delegate(HasFact c)
						{
							c.Unit = new PlayerCharacter();
							c.m_Fact = proficiencyFact.ToReference<BlueprintUnitFactReference>();
						});
						bp.OnSelect = ActionFlow.DoSingle<ContextActionWardstoneMythicAwakening>();
					});
					answersList.InsertAnswer(answer);
				}
			}
		}

		private static void AddDesnaTempleNatureCommunion()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("16d9e0a48d5746a4fbb1497e4ee4cf81");
			if (answersList != null)
			{
				AddDesnaAnswer("IsekaiDesnaCommunionDevourer", "(Slime) [Commune with the Song of Elysium] \"My slime core resonates with this melody... It's like the whispering winds and primeval beasts of Sarkoris are speaking to my living jelly.\"", "{n}As your gelatinous form trembles in harmonic resonance with the shrine's chimes, the spiritual remnants of Sarkorian beasts stir in Elysian light. A primal pack of phantom predators dips their heads in silent kinship with your predatory hunger, swearing to heed your banner when the march across the Wounded Lands begins.{/n} \"The song echoes back from the wild steppes... A pack of Sarkorian beast vanguards has answered your call!\"", "DevourerProficiencies", new ContextActionDesnaNatureCommunion());
				AddDesnaAnswer("IsekaiDesnaCommunionOverlord", "(Overlord) [Subjugate the Resonance] \"An amusing little melody. Even the capricious spirits of the Sphere of Dreams must bow to true metaphysical authority.\"", "{n}The delicate song momentarily wavers under your chilling magical pressure before snapping into rigid cadence. The spectral chimes ring out like tolling iron bells.{/n} \"A hollow, obedient echo resonates through the stone hall... even dream spirits acknowledge supreme dominion.\"", "OverlordProficiencies");
				AddDesnaAnswer("IsekaiDesnaCommunionShadowMonarch", "(Shadow Monarch) [Attune the Shadows] \"Where there is brilliant starlight, the shadows cast are absolute. Let the dusk embrace this song.\"", "{n}The flickering candlelight and celestial brilliance cast long, sentient silhouettes that bow in solemn formation across the shrine walls.{/n} \"The shadows deepen into a silent guard, absorbing the echoes of the celestial melody.\"", "ShadowMonarchProficiencies");
				AddDesnaAnswer("IsekaiDesnaCommunionGeneral", "(Isekai Protagonist) [Hum an Otherworldly Melody] \"This melody reminds me of anime theme songs and fantasy tavern tunes from back home. Let's see if the bells can play my favorite chorus.\"", "{n}The chimes ripple with sudden playful syncopation, matching your humming note-for-note. The priests stare at you with jaw-dropped fascination as an otherworldly tune dances through the ancient temple.{/n} \"The shrine chimes with vibrant, unmistakable joy!\"", "IsekaiProficiencies");
			}
			void AddDesnaAnswer(string name, string text, string cueText, string proficiencyFactName, GameAction onSelect = null)
			{
				BlueprintFeature proficiencyFact = GetRequiredDialogueFact(proficiencyFactName);
				if (proficiencyFact != null)
				{
					BlueprintCue reply = TTCoreExtensions.CreateCue(name + "Reply", delegate(BlueprintCue bp)
					{
						bp.SetText(Main.IsekaiContext, cueText);
						bp.SetAnswersList(answersList);
					});
					BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
					{
						bp.SetText(Main.IsekaiContext, text);
						bp.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
						bp.ShowOnce = true;
						bp.AddShowCondition(delegate(HasFact c)
						{
							c.Unit = new PlayerCharacter();
							c.m_Fact = proficiencyFact.ToReference<BlueprintUnitFactReference>();
						});
						if (onSelect != null)
						{
							bp.OnSelect = Helpers.CreateActionList(onSelect);
						}
					});
					answersList.InsertAnswer(answer);
				}
			}
		}

		private static void AddQueenGalfreyWarCampReactions()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("eae6ac8c30bb31249aa266b2e0a87fb9");
			BlueprintUnit galfreyUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("97d1c33c784100140b9fa317aef5a499");
			if (answersList != null)
			{
				AddWarCampAnswer("IsekaiWarCampOverlord", "(Overlord) [Summon Deathbound Legion] \"Your Majesty, mortal crusaders bleed and break. I have summoned 30 Graveknights from the sepulchers of the ancient world. They know no fear, require no rations, and obey without hesitation.\"", "{n}Queen Galfrey stares at you in grim apprehension as cold, armored dread marshals beyond the perimeter.{/n} \"Graveknights... Under any other commander, such blasphemy would see you executed on the spot. But if you can truly bind their dark wills to defend Mendev... I will not turn away steel when the realm is on the brink. Keep them on a tight leash, Commander.\"", "OverlordProficiencies", new ContextActionMobilizeSubclassArmy
				{
					UnitGuid = "15c574688d6e44e79925411c5234765e",
					Count = 30,
					ArmyName = "Graveknights"
				});
				AddWarCampAnswer("IsekaiWarCampShadowMonarch", "(Shadow Monarch) [Raise 50 Shadow Soldiers] \"An army that leaves no corpses and requires no supply lines. 50 Shadow Soldiers stand assembled in the camp's twilight, awaiting the march on Drezen.\"", "{n}The Queen watches in awe as fifty silent silhouettes rise up from the shadows of the crusade pavilions, their spectral blades gleaming with cold voidfire.{/n} \"Shadow soldiers... They move like living mirages. If they can strike behind demon lines without suffering attrition, Drezen's outer bastions will crumble far faster. See them deployed to the vanguard, Commander.\"", "ShadowMonarchProficiencies", new ContextActionMobilizeSubclassArmy
				{
					UnitGuid = "5a49b18137ebd634e96f3dfba01b77f2",
					Count = 50,
					ArmyName = "Shadow Soldiers"
				});
				AddWarCampAnswer("IsekaiWarCampDevourer", "(Slime) [Summon Tempest Beast Vanguard] \"My communion with the spirits at Desna's shrine paid off. 20 Sarkorian Tempest Beast Riders have emerged from the wastes, eager to tear through fiendish lines.\"", "{n}Feral howls echo across the hills as savage beast riders surge up to the camp boundaries, their mounts sniffing the air with predatory relish.{/n} \"Sarkorian beast riders! We thought their breeds were extinct after the opening of the Worldwound. Their ferocity in battle is legendary. Well done, Commander.\"", "DevourerProficiencies", new ContextActionMobilizeSubclassArmy
				{
					UnitGuid = "c7904164350a4d5199c68da5c79aef74",
					Count = 20,
					ArmyName = "Tempest Beast Riders"
				});
				AddWarCampAnswer("IsekaiWarCampHero", "(Hero) [Rally 50 Volunteer Crusaders] \"Word of our triumph in Kenabres has spread across the border provinces! 50 heavily armored veteran crusaders have flocked to our banner to stand with us!\"", "{n}Queen Galfrey smiles with radiant hope as fresh ranks of resolute crusaders form up, their shields polished and eyes burning with righteous fervor.{/n} \"The morale of the crusade hasn't been this high in seventy years. With stalwarts like these rallying to your standard, I truly believe we can reclaim the Sword of Valor.\"", "HeroProficiencies", new ContextActionMobilizeSubclassArmy
				{
					UnitGuid = "24ad1a10190de5c459a4521996dcec4b",
					Count = 50,
					ArmyName = "Heavy Infantry Crusaders"
				});
				AddWarCampAnswer("IsekaiWarCampGodEmperor", "(God Emperor) [Inspire Imperial Discipline] \"Your Majesty, I have restructured the vanguard logistics and infused the ranks with imperial martial doctrine. The crusade does not merely fight; it conquers.\"", "{n}Galfrey nods with deep respect.{/n} \"Your military discipline and regal authority have united even the most unruly mercenaries into an iron fist. Lead them well, Commander.\"", "GodEmperorProficiencies", new ContextActionMobilizeSubclassArmy
				{
					UnitGuid = "24ad1a10190de5c459a4521996dcec4b",
					Count = 50,
					ArmyName = "Imperial Heavy Legionnaires"
				});
				AddWarCampAnswer("IsekaiWarCampMastermind", "(Mastermind) [Deploy Precision Vanguard] \"I have calculated the optimal army configuration for breaching Drezen's outer trenchworks. 50 disciplined vanguard infantry have been assigned to specific breach coordinates.\"", "{n}Galfrey looks over your operational schematics with genuine admiration.{/n} \"The meticulous precision of your campaign plans is breathtaking. Every unit placed with surgical intent. Execute the plan, Commander.\"", "MastermindProficiencies", new ContextActionMobilizeSubclassArmy
				{
					UnitGuid = "24ad1a10190de5c459a4521996dcec4b",
					Count = 50,
					ArmyName = "Precision Vanguard Infantry"
				});
			}
			void AddWarCampAnswer(string name, string text, string cueText, string proficiencyFactName, ContextActionMobilizeSubclassArmy mobilizeAction)
			{
				BlueprintFeature proficiencyFact = GetRequiredDialogueFact(proficiencyFactName);
				if (proficiencyFact != null)
				{
					BlueprintCue reply = TTCoreExtensions.CreateCue(name + "Reply", delegate(BlueprintCue bp)
					{
						bp.SetText(Main.IsekaiContext, cueText);
						if (galfreyUnit != null)
						{
							bp.Speaker = new DialogSpeaker
							{
								m_Blueprint = galfreyUnit.ToReference<BlueprintUnitReference>(),
								MoveCamera = true
							};
						}
						bp.SetAnswersList(answersList);
					});
					BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
					{
						bp.SetText(Main.IsekaiContext, text);
						bp.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
						bp.ShowOnce = true;
						bp.AddShowCondition(delegate(HasFact c)
						{
							c.Unit = new PlayerCharacter();
							c.m_Fact = proficiencyFact.ToReference<BlueprintUnitFactReference>();
						});
						if (mobilizeAction != null)
						{
							bp.OnSelect = Helpers.CreateActionList(mobilizeAction);
						}
					});
					answersList.InsertAnswer(answer);
				}
			}
		}

		private static void AddRegillHellknightReactions()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("5128a604b13cf554a846ab7c16054cbf");
			BlueprintUnit regillUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("68d4a5b150c7c2c42aeea6f24044113a");
			if (answersList != null)
			{
				AddRegillAnswer("IsekaiRegillMastermind", "(Mastermind) [Counter-Analyze Regill's Tactics] \"Your retreat into the canyon minimized flanking angles by 63%, but your casualty triage was suboptimal. If you had deployed cross-firing crossbowmen on the eastern ridge, you would have suffered zero Hellknight losses.\"", "{n}Regill narrows his golden eyes, studying you with razor-sharp analytical respect.{/n} \"A rigorous assessment. Most who witness our discipline see only cruelty; you see geometry and resource management. We may find common ground yet, Commander.\"", "MastermindProficiencies");
				AddRegillAnswer("IsekaiRegillOverlord", "(Overlord) [Demonstrate Superior Authority] \"Your Order of the Godclaw prides itself on absolute order. But true order does not beg mortal magistrates for legitimacy; it bends reality by supreme will.\"", "{n}Regill stands motionless under your dark gaze, his expression unyielding stone.{/n} \"A tyrant's philosophy. Yet the Worldwound is chaos incarnate; against utter disorder, absolute conviction is an acceptable weapon. Prove your supreme will on the march to Drezen.\"", "OverlordProficiencies");
				AddRegillAnswer("IsekaiRegillShadowMonarch", "(Shadow Monarch) [Acknowledge Strategic Elimination] \"Sacrificing your wounded to preserve unit cohesion was brutal, but necessary. In the abyss of war, hesitation is the only true sin.\"", "{n}Regill nods once with crisp, measured approval.{/n} \"A rare spark of pragmatic clarity. Those who flinch from necessary amputations allow gangrene to consume the whole regiment. Remember that principle when you command the crusade.\"", "ShadowMonarchProficiencies");
				AddRegillAnswer("IsekaiRegillGodEmperor", "(God Emperor) [Offer Imperial Commission] \"Paralictor Regill, your discipline is formidable, but incomplete without an emperor worthy of your service. Swear your legions to my imperial mandate, and we shall forge an eternal realm.\"", "{n}Regill's jaw tightens with professional composure.{/n} \"The Order of the Godclaw serves no individual sovereign on whim. But if your imperial mandate enforces absolute law and eradicates the abyssal cancer, our hammers will march at your side.\"", "GodEmperorProficiencies");
				AddRegillAnswer("IsekaiRegillDevourer", "(Slime) [Assess Gargoyle Caloric Density] \"Those gargoyles were surprisingly tough. Not much meat on them, mostly crunchy stone... but their magical essence made a decent post-battle snack.\"", "{n}Regill blinks once, his expression briefly flickering with profound bafflement before returning to stern discipline.{/n} \"...Consuming stony fiends for sustenance. Unorthodox. Biologically dubious. But if your digestion removes enemy combatants from the battlefield, I have no tactical objection.\"", "DevourerProficiencies");
			}
			void AddRegillAnswer(string name, string text, string cueText, string proficiencyFactName)
			{
				BlueprintFeature proficiencyFact = GetRequiredDialogueFact(proficiencyFactName);
				if (proficiencyFact != null)
				{
					BlueprintCue reply = TTCoreExtensions.CreateCue(name + "Reply", delegate(BlueprintCue bp)
					{
						bp.SetText(Main.IsekaiContext, cueText);
						if (regillUnit != null)
						{
							bp.Speaker = new DialogSpeaker
							{
								m_Blueprint = regillUnit.ToReference<BlueprintUnitReference>(),
								MoveCamera = true
							};
						}
						bp.SetAnswersList(answersList);
					});
					BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
					{
						bp.SetText(Main.IsekaiContext, text);
						bp.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
						bp.ShowOnce = true;
						bp.AddShowCondition(delegate(HasFact c)
						{
							c.Unit = new PlayerCharacter();
							c.m_Fact = proficiencyFact.ToReference<BlueprintUnitFactReference>();
						});
					});
					answersList.InsertAnswer(answer);
				}
			}
		}

		private static void AddZachariusLostChapelReactions()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("16f23cbdc188b3348a47531d2bb747c2");
			BlueprintUnit zachariusUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("d67527d9c2246734baffc69808d40b01");
			if (answersList != null)
			{
				AddZachariusAnswer("IsekaiZachariusOverlord", "(Overlord) [Assert Arch-Lich Supremacy] \"You hide in a damp basement whimpering over an ancient defeat, Zacharius. Kneel before a true Sovereign of the Undead and surrender your phylactery, or I will crush your soul into bone powder.\"", "{n}Zacharius reels back, the necrotic flames in his empty eye sockets flickering with genuine shock and primordial dread.{/n} \"What... what manner of entity are you?! The tyrannical death aura you emanate... it dwarfs even the archmages of ancient Geb! I... I yield to your absolute supremacy, Sovereign! Take what you will, only grant me passage into your grand design!\"", "OverlordProficiencies");
				AddZachariusAnswer("IsekaiZachariusDevourer", "(Slime) [Appraise Ancient Bone Calcium] \"Ooh, a talking skeleton! Are you preserved with ancient magical jerky? You look like you've been curing down here for a century. How many calories is a lich anyway?\"", "{n}Zacharius recoils in utter disgust and disbelief.{/n} \"Calories?! Curing?! You insufferable, gelatinous monstrosity! I am Zacharius, hero of the Second Crusade and master of death itself, not livestock for your gluttony! Keep your vile slime away from my robes!\"", "DevourerProficiencies");
				AddZachariusAnswer("IsekaiZachariusShadowMonarch", "(Shadow Monarch) [Extract Soul Echo] \"Death is not your prison, Zacharius. In my kingdom of shadows, every soul finds its eternal purpose under my throne.\"", "{n}Zacharius tilts his skull, mesmerized by the deep sovereign darkness wrapping around your fingers.{/n} \"Fascinating... not mere necromancy, but the primal gravity of the endless void. Perhaps your descent into darkness will achieve what the paladins never could.\"", "ShadowMonarchProficiencies");
				AddZachariusAnswer("IsekaiZachariusMastermind", "(Mastermind) [Diagnose Phylactery Deterioration] \"Your warding runes have degraded by 41% over seventy years. Your eternal preservation is decaying into senility. Hand over the Wand of Zacharius before your enchantments suffer critical collapse.\"", "{n}Zacharius's bony fingers tremble with rage and mortification.{/n} \"Degraded?! Insolent whelp! My wards are... they are... {n}he glances anxiously at the flickering runes{/n} ...Curse your keen eyes. Take the wand and begone from my sanctuary!\"", "MastermindProficiencies");
			}
			void AddZachariusAnswer(string name, string text, string cueText, string proficiencyFactName)
			{
				BlueprintFeature proficiencyFact = GetRequiredDialogueFact(proficiencyFactName);
				if (proficiencyFact != null)
				{
					BlueprintCue reply = TTCoreExtensions.CreateCue(name + "Reply", delegate(BlueprintCue bp)
					{
						bp.SetText(Main.IsekaiContext, cueText);
						if (zachariusUnit != null)
						{
							bp.Speaker = new DialogSpeaker
							{
								m_Blueprint = zachariusUnit.ToReference<BlueprintUnitReference>(),
								MoveCamera = true
							};
						}
						bp.SetAnswersList(answersList);
					});
					BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
					{
						bp.SetText(Main.IsekaiContext, text);
						bp.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
						bp.ShowOnce = true;
						bp.AddShowCondition(delegate(HasFact c)
						{
							c.Unit = new PlayerCharacter();
							c.m_Fact = proficiencyFact.ToReference<BlueprintUnitFactReference>();
						});
					});
					answersList.InsertAnswer(answer);
				}
			}
		}

		private static void AddDrezenCitadelProclamationReactions()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("44704bddb6223b84989dd26bcf20b601");
			BlueprintUnit galfreyUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("97d1c33c784100140b9fa317aef5a499");
			if (answersList != null)
			{
				AddProclamationAnswer("IsekaiDrezenProclamationSlime", "(Slime) [Proclaim the Tempest Multiverse Federation] \"From this day forth, Drezen stands as the sovereign capital of the Tempest Multiverse Federation! A realm of boundless innovation, hot-spring resorts, culinary delights, and sanctuary for mortals, mongrels, and reformed fiends alike!\"", "{n}Queen Galfrey pauses, looking across the liberated citadel walls with a faint, bemused smile.{/n} \"A... multiverse federation? With hot springs and monster sanctuaries? Under any other crusader, I would consider this sheer madness. But your people adore you, and your strength retook these walls. As Queen of Mendev, I officially recognize the Tempest Federation of Drezen. May your boundless ambition bring life back to these barren lands, Commander.\"", "DevourerProficiencies");
				AddProclamationAnswer("IsekaiDrezenProclamationOverlord", "(Overlord) [Consecrate Exterior Bastion of the Great Tomb] \"Hear me, mortal Queen! Drezen is claimed as an exterior bastion of the Great Tomb. Death shall govern these ramparts with absolute, unwavering order. Let all who dwell here serve with supreme devotion.\"", "{n}Galfrey shivers as an icy, necromantic weight settles over the stone battlement, yet she holds your gaze with queenly resolve.{/n} \"An unyielding necropolis in the heart of the Worldwound... While I cannot officially endorse the worship of undeath in holy Mendev, the peace and discipline your forces maintain cannot be denied. Drezen is yours to command. Rule it with the justice you promised.\"", "OverlordProficiencies");
				AddProclamationAnswer("IsekaiDrezenProclamationShadowMonarch", "(Shadow Monarch) [Raise the Citadel of Eternal Shadows] \"Let the sun set on the old world. Drezen is now the Citadel of Eternal Shadows. An unbreachable bastion where the silent legion stands eternal guard over northern Avistan.\"", "{n}Galfrey looks out at the darkening towers, where silhouettes of shadow soldiers stand vigilant along the parapets.{/n} \"The demons took Drezen through light and fire, but they will never break through your shadows. The people feel protected, even if the cold chill in the air never quite fades. Drezen is safe in your shadow, Commander.\"", "ShadowMonarchProficiencies");
				AddProclamationAnswer("IsekaiDrezenProclamationGodEmperor", "(God Emperor) [Declare the First Radiant Imperium] \"By divine right and unshakeable will, I consecrate Drezen as the First Radiant Imperium! Here begins the golden age of civilization, law, and solar prosperity that will illuminate all of Golarion!\"", "{n}Queen Galfrey bows her head with profound regal respect as radiant golden light bathes the citadel square.{/n} \"An Imperium born from the ashes of demon tyranny... The majesty of your presence inspires hope even in the most cynical crusaders. Lead your people wisely, Sovereign Commander.\"", "GodEmperorProficiencies");
				AddProclamationAnswer("IsekaiDrezenProclamationHero", "(Hero) [Dedicate Drezen to Mortal Freedom] \"Drezen belongs to no tyrant, no god, and no demon! It belongs to the farmers, the knights, and the orphans who bled to take back their homes. Drezen is the beacon of mortal freedom!\"", "{n}A thunderous cheer erupts from the assembled crusaders below as Galfrey smiles with radiant warmth.{/n} \"Spoken like a true champion of the people. For seventy years, we fought under the heavy shadow of despair. Today, your courage gave them back their future. Long live the Knight Commander!\"", "HeroProficiencies");
				AddProclamationAnswer("IsekaiDrezenProclamationMastermind", "(Mastermind) [Establish the Grand Sovereign Nexus] \"Drezen is restructured into the central nexus of our grand geopolitical board. Every resource route, intelligence bureau, and bastion is calibrated to mathematically guarantee total victory.\"", "{n}Galfrey reviews your architectural and strategic blueprints with awe.{/n} \"Incredible. In a matter of hours, you have organized logistics that took our Royal Council months to debate. With your intellect guiding the crusade, the demons will find no flaws to exploit.\"", "MastermindProficiencies");
				AddProclamationAnswer("IsekaiDrezenProclamationGeneral", "(Isekai Protagonist) [Proclaim the New Dawn of Drezen] \"We took back Drezen with our own hands, strange powers, and loyal friends. This fortress is now a symbol that anything is possible when you refuse to surrender!\"", "{n}Galfrey nods with deep heartfelt sincerity.{/n} \"Indeed, Commander. You came from unknown origins and turned the tide of an eighty-year war. Mendev salutes you, and the entire continent looks to your standard with hope.\"", "IsekaiProficiencies");
			}
			void AddProclamationAnswer(string name, string text, string cueText, string proficiencyFactName)
			{
				BlueprintFeature proficiencyFact = GetRequiredDialogueFact(proficiencyFactName);
				if (proficiencyFact != null)
				{
					BlueprintCue reply = TTCoreExtensions.CreateCue(name + "Reply", delegate(BlueprintCue bp)
					{
						bp.SetText(Main.IsekaiContext, cueText);
						if (galfreyUnit != null)
						{
							bp.Speaker = new DialogSpeaker
							{
								m_Blueprint = galfreyUnit.ToReference<BlueprintUnitReference>(),
								MoveCamera = true
							};
						}
						bp.SetAnswersList(answersList);
					});
					BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
					{
						bp.SetText(Main.IsekaiContext, text);
						bp.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
						bp.ShowOnce = true;
						bp.AddShowCondition(delegate(HasFact c)
						{
							c.Unit = new PlayerCharacter();
							c.m_Fact = proficiencyFact.ToReference<BlueprintUnitFactReference>();
						});
					});
					answersList.InsertAnswer(answer);
				}
			}
		}

		private static void AddAreeluLabLoopTruthReactions()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("cd9c9facc3a8ded4e9683cde8958295e");
			BlueprintUnit areeluUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("92f29ae2c49edb74f8943d30ea788dd0");
			if (answersList != null)
			{
				AddLabTruthAnswer("IsekaiAreeluLabTruthGeneral", "(Isekai Protagonist) [Confront the Cosmic Glitch] \"Your ritual didn't pull your child's soul, Areelu. It cracked open reality just wide enough for Yog-Sothoth's temporal loop to siphon me in from another world! This entire crusade is an otherworldly replay loop, broadcast to cosmic entities!\"", "{n}Areelu Vorlesh's spectral projection halts. Her analytical gaze widens with profound, disturbing shock as she calculates the metaphysical equations.{/n} \"An anomaly from beyond... Yog-Sothoth's causal loop? {n}Her fingers twitch against her staff as runes spiral around her{/n} ...I felt the dimensional drag during the rift ritual, but I assumed... no. The Akashic signature is completely alien to Golarion! You are telling the truth. My child's soul was not the payload... an entire otherworldly protagonist was caught in the tear!\"", "IsekaiProficiencies", new ContextActionAreeluLoopTruthBanter());
				AddLabTruthAnswer("IsekaiAreeluLabTruthDevourer", "(Slime) [Digest the Planar Glitch] \"Your summoning ritual was basically a giant dimensional vacuum cleaner! Instead of your kid, you sucked in an otherworldly slime with cheat digestion skills. And now Yog-Sothoth's loop has us all on playback!\"", "{n}Areelu stares at you in complete disbelief and horror.{/n} \"A slime... an otherworldly, morphic digestive organism with impossible cellular density. The ritual pulled a conceptual predator through the void instead of my child... {n}she touches her forehead, trembling{/n} The cosmic irony is suffocating. Yet your growth exceeds even my wildest laboratory models...\"", "DevourerProficiencies", new ContextActionAreeluLoopTruthBanter());
				AddLabTruthAnswer("IsekaiAreeluLabTruthOverlord", "(Overlord) [Dissect the Flawed Incantation] \"A pathetic miscalculation, witch. Your amateurish rift failed to tether your offspring. Instead, your dimensional tear snagged an Overlord from beyond reality into Yog-Sothoth's recurring cycle. Bow before your superior.\"", "{n}Areelu Vorlesh staggers back as an aura of supreme tier authority presses against her laboratory projection.{/n} \"Amateurish?! I spent decades... {n}she stops, her breath catching as she senses the unfathomable depth of your soul matrix{/n} ...By the Abyss. The soul anchored in this flesh belongs to no mortal offspring. You are a supreme sovereign entity ensnared by the cosmic loop. What have I unleashed upon the planes...?\"", "OverlordProficiencies", new ContextActionAreeluLoopTruthBanter());
				AddLabTruthAnswer("IsekaiAreeluLabTruthShadowMonarch", "(Shadow Monarch) [Reveal the Sovereign Truth] \"The void you opened reached far beyond the Worldwound. It pulled the sovereign of shadows into Yog-Sothoth's playback loop. Your child is not here; only the monarch remains.\"", "{n}The shadows in the laboratory coil and bow before you as Areelu looks on in silent terror.{/n} \"The primordial void... not death, but the ancient monarch of darkness. The rift didn't retrieve my child; it tore open the gates to the Netherworld and siphoned its king. The calculations were doomed from the start.\"", "ShadowMonarchProficiencies", new ContextActionAreeluLoopTruthBanter());
				AddLabTruthAnswer("IsekaiAreeluLabTruthMastermind", "(Mastermind) [Expose the Infinite Playback Algorithm] \"I reverse-engineered your ritual matrices, Vorlesh. The probability of your child's soul traversing the rift was zero. Yog-Sothoth intercepted your dimensional breach to feed an infinite spectator broadcast for cosmic entities.\"", "{n}Areelu's eyes narrow as she watches you mentally dismantle her life's work.{/n} \"Zero probability... an intercepted broadcast? {n}She lets out a dry, bitter laugh that echoes through the laboratory{/n} All my suffering, all my meticulous sacrifices... hijacked by an Outer God to produce cosmic entertainment for the stars. And you... you see the entire board, don't you, Commander?\"", "MastermindProficiencies", new ContextActionAreeluLoopTruthBanter());
				AddLabTruthAnswer("IsekaiAreeluLabTruthHero", "(Hero) [Declare Moral Rejection of the Loop] \"Your grief opened a wound that swallowed an innocent soul from another world. But I won't let Yog-Sothoth's loop or your obsession destroy Golarion. I will protect these people, and I will break this cycle!\"", "{n}Areelu smiles faintly, a bittersweet sadness softening her demonic features.{/n} \"Such unwavering, stubborn courage... even when knowing you were plucked from your home to dance on a celestial stage. Perhaps that unyielding spirit is why the rift chose you. Break the loop if you can, hero. I will be watching.\"", "HeroProficiencies", new ContextActionAreeluLoopTruthBanter());
			}
			void AddLabTruthAnswer(string name, string text, string cueText, string proficiencyFactName, ContextAction extraAction = null)
			{
				BlueprintFeature proficiencyFact = GetRequiredDialogueFact(proficiencyFactName);
				if (proficiencyFact != null)
				{
					BlueprintCue reply = TTCoreExtensions.CreateCue(name + "Reply", delegate(BlueprintCue bp)
					{
						bp.SetText(Main.IsekaiContext, cueText);
						if (areeluUnit != null)
						{
							bp.Speaker = new DialogSpeaker
							{
								m_Blueprint = areeluUnit.ToReference<BlueprintUnitReference>(),
								MoveCamera = true
							};
						}
						bp.SetAnswersList(answersList);
					});
					BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
					{
						bp.SetText(Main.IsekaiContext, text);
						bp.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
						bp.ShowOnce = true;
						bp.AddShowCondition(delegate(HasFact c)
						{
							c.Unit = new PlayerCharacter();
							c.m_Fact = proficiencyFact.ToReference<BlueprintUnitFactReference>();
						});
						if (extraAction != null)
						{
							bp.OnSelect = Helpers.CreateActionList(extraAction);
						}
					});
					answersList.InsertAnswer(answer);
				}
			}
		}

		private static void AddBaphometConfrontationReactions()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("9ab9ad8f6e11d67499b48fd595e50972");
			BlueprintUnit baphometUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("f8007503fe211da4eb027e070eeb3f8c");
			if (answersList != null)
			{
				AddBaphometAnswer("IsekaiBaphometOverlord", "(Overlord) [Mock the Lord of the Labyrinth] \"You swagger into this mine bellowing like a slaughterhouse bull, Baphomet. In the Great Tomb, cattle know their place. Kneel before supreme tier magic, or I shall turn your horns into drinking goblets.\"", "{n}Baphomet's massive nostrils flare with blistering hellfire as he clutches his glaive in furious shock.{/n} \"Cattle?! Drinking goblets?! Incomprehensible worm! You stand before the Lord of Minotaurs, master of the Ivory Labyrinth! I shall tear your flesh and grind your arrogant bones into dust!\"", "OverlordProficiencies", new ContextActionBaphometDefianceBanter());
				AddBaphometAnswer("IsekaiBaphometDevourer", "(Slime) [Inspect Demon Lord Caloric Density] \"Whoa, fresh beef! I've eaten dretches, minotaurs, and balors, but a full Demon Lord? You must have enough magical calories to evolve my entire federation! Come here!\"", "{n}Baphomet reels back, staring in profound revulsion at your quivering, hungry form.{/n} \"Fresh beef?! Caloric density?! You gelatinous abomination, I am a Demon Lord of the Abyss, not livestock for your gluttony! I shall scorch your vile ooze into dry ash!\"", "DevourerProficiencies", new ContextActionBaphometDefianceBanter());
				AddBaphometAnswer("IsekaiBaphometShadowMonarch", "(Shadow Monarch) [Claim the Ivory Horns] \"Your labyrinth is just a maze of shadows, Baphomet. And in the dark, every shadow answers to me. Even the shadows of demon lords.\"", "{n}Baphomet grunts in fury as the darkness at his hooves twists and resists his commands.{/n} \"Shadows answering to a mortal?! Insolent wretch! My labyrinth is eternal, and your soul shall wander its twisting corridors screaming forever!\"", "ShadowMonarchProficiencies", new ContextActionBaphometDefianceBanter());
				AddBaphometAnswer("IsekaiBaphometGodEmperor", "(God Emperor) [Issue Imperial Extinction Decree] \"Baphomet, by decree of the Radiant Imperium, your reign of deceit is terminated. The light of the solar throne shall bleach your labyrinth to ash.\"", "{n}Baphomet roars, the cavern shaking as brimstone explodes from his cloven hooves.{/n} \"An emperor?! In my mines?! Your pathetic solar light will be swallowed by the eternal darkness of the Abyss! Die!\"", "GodEmperorProficiencies", new ContextActionBaphometDefianceBanter());
				AddBaphometAnswer("IsekaiBaphometMastermind", "(Mastermind) [Deconstruct Baphomet's Strategy] \"You sacrificed your own daughter to test my defenses, Lord of the Labyrinth. A remarkably pedestrian gambit. Your tactical failure rate in this encounter is already 100%.\"", "{n}Baphomet barks with guttural rage, his crimson eyes bulging with murderous fury.{/n} \"Pedestrian gambit?! Hepzamirah served her purpose! And your purpose is to die here at the bottom of the world, arrogant strategist!\"", "MastermindProficiencies", new ContextActionBaphometDefianceBanter());
				AddBaphometAnswer("IsekaiBaphometHero", "(Hero) [Stand Resolute Against the Fiend] \"You betrayed your own blood and sacrificed countless lives for your labyrinth. Today, your cruelty ends, Baphomet. We aren't afraid of you!\"", "{n}Baphomet sneers with cruel, jagged teeth.{/n} \"Not afraid? You will learn terror when my glaive pins your heart to these caverns, righteous fool!\"", "HeroProficiencies", new ContextActionBaphometDefianceBanter());
			}
			void AddBaphometAnswer(string name, string text, string cueText, string proficiencyFactName, ContextAction extraAction = null)
			{
				BlueprintFeature proficiencyFact = GetRequiredDialogueFact(proficiencyFactName);
				if (proficiencyFact != null)
				{
					BlueprintCue reply = TTCoreExtensions.CreateCue(name + "Reply", delegate(BlueprintCue bp)
					{
						bp.SetText(Main.IsekaiContext, cueText);
						if (baphometUnit != null)
						{
							bp.Speaker = new DialogSpeaker
							{
								m_Blueprint = baphometUnit.ToReference<BlueprintUnitReference>(),
								MoveCamera = true
							};
						}
						bp.SetAnswersList(answersList);
					});
					BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
					{
						bp.SetText(Main.IsekaiContext, text);
						bp.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
						bp.ShowOnce = true;
						bp.AddShowCondition(delegate(HasFact c)
						{
							c.Unit = new PlayerCharacter();
							c.m_Fact = proficiencyFact.ToReference<BlueprintUnitFactReference>();
						});
						if (extraAction != null)
						{
							bp.OnSelect = Helpers.CreateActionList(extraAction);
						}
					});
					answersList.InsertAnswer(answer);
				}
			}
		}

		private static void AddNocticulaAudienceReactions()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("75678bafb5e33854baaee17ee6eaf69c");
			BlueprintUnit nocticulaUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("0cca8c841d634d84fbec2609c8db3465");
			if (answersList != null)
			{
				AddNocticulaAnswer("IsekaiNocticulaGeneral", "(Isekai Protagonist) [Resist Charm with Akashic Encryption] \"You can drop the mental probes, Lady Nocticula. My soul is backed by otherworldly Akashic encryption and live-broadcast to millions of Constellations. You won't find a puppet here.\"", "{n}Nocticula raises a sculpted eyebrow, leaning back on her velvet cushions with a purr of genuine amusement.{/n} \"Akashic encryption... and a live celestial audience? How delightfully exotic. Most mortals who enter my palace fall weeping to their knees begging for a single touch. It seems the rumors of your otherworldly charm were not exaggerated, Commander. Let us speak as equals.\"", "IsekaiProficiencies");
				AddNocticulaAnswer("IsekaiNocticulaOverlord", "(Overlord) [Establish Equal Planar Dominance] \"You rule an island of assassins and hedonists, Lady of the Shadow. Entertaining, but do not mistake an Overlord for mortal crusader clay. Speak with dignity, or we shall find how silk burns under tier 10 magic.\"", "{n}Nocticula's lips curve into a tantalizing, dangerous smile, unruffled by your chilling threat.{/n} \"Such delicious arrogance... and the terrifying magical aura to back it up. A ruler of death who bows to no god. I like you, Overlord. Alushinyrra is honored by your presence. Let us conduct our business with mutual respect.\"", "OverlordProficiencies");
				AddNocticulaAnswer("IsekaiNocticulaDevourer", "(Slime) [Admire the Midnight Cuisine] \"Nice city you have here! The architecture is a bit edgy, but the street food in the Lower City is fascinating. Any chance you'd trade some Midnight Isle culinary recipes with the Tempest Federation?\"", "{n}Nocticula blinks once, then bursts into a melodious, sultry laugh that echoes through the House of Silken Shadows.{/n} \"Culinary recipes?! You tore through the Abyss, defeated Hepzamirah, marched into my royal palace... and your first request is Midnight Isle street food recipes?! {n}She claps her delicate hands, delighted{/n} You are utterly charming, little slime. Consider my kitchen archives open to your federation.\"", "DevourerProficiencies");
				AddNocticulaAnswer("IsekaiNocticulaShadowMonarch", "(Shadow Monarch) [Commune in Absolute Darkness] \"You call yourself Our Lady in Shadow, Nocticula. But you rule merely the shadows cast by flesh. I rule the void itself.\"", "{n}The shadows beneath Nocticula's throne ripple in harmonic resonance with your own sovereign aura.{/n} \"The primal void... Yes, I can feel it. You do not merely borrow the dark; you are its undisputed master. How refreshing to meet someone who understands the true elegance of shadow. Welcome to my court, Monarch.\"", "ShadowMonarchProficiencies");
				AddNocticulaAnswer("IsekaiNocticulaMastermind", "(Mastermind) [Counter-Propose Planar Geopolitics] \"Your assassination network is impressive, Lady Nocticula, but economically fragile. If you supply our crusade with Nahyndrian intelligence, I can guarantee Alushinyrra a 34% increase in illicit planar trade routes post-war.\"", "{n}Nocticula leans forward, her golden eyes sparkling with sharp appreciation.{/n} \"A strategist who speaks in trade margins and economic dominance rather than righteous hymns. You truly are a breath of fresh air, Commander. Let us review your proposal.\"", "MastermindProficiencies");
			}
			void AddNocticulaAnswer(string name, string text, string cueText, string proficiencyFactName)
			{
				BlueprintFeature proficiencyFact = GetRequiredDialogueFact(proficiencyFactName);
				if (proficiencyFact != null)
				{
					BlueprintCue reply = TTCoreExtensions.CreateCue(name + "Reply", delegate(BlueprintCue bp)
					{
						bp.SetText(Main.IsekaiContext, cueText);
						if (nocticulaUnit != null)
						{
							bp.Speaker = new DialogSpeaker
							{
								m_Blueprint = nocticulaUnit.ToReference<BlueprintUnitReference>(),
								MoveCamera = true
							};
						}
						bp.SetAnswersList(answersList);
					});
					BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
					{
						bp.SetText(Main.IsekaiContext, text);
						bp.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
						bp.ShowOnce = true;
						bp.AddShowCondition(delegate(HasFact c)
						{
							c.Unit = new PlayerCharacter();
							c.m_Fact = proficiencyFact.ToReference<BlueprintUnitFactReference>();
						});
					});
					answersList.InsertAnswer(answer);
				}
			}
		}

		private static void AddDivineSummitGoddessesReactions()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("e3a71f123c7aae7409d08855827dbea5");
			BlueprintUnit iomedaeUnit = BlueprintTools.GetBlueprint<BlueprintUnit>("9a1443603c9353d4194a583a31228c8b");
			if (answersList != null)
			{
				AddSummitAnswer("IsekaiGoddessesSummitLoopBreaker", "(Isekai Protagonist) [Reject Divine Scripts for Mortal Freedom] \"Iomedae demands blind faith, and Nocticula offers abyssal chains. Neither of you understands: this power was forged by my own choices, my companions, and mortal will! I reject both your scripts. Golarion's future belongs to mortals, and I will shatter this causal loop with my own hands!\"", "{n}Iomedae stares at you in solemn, awe-struck sorrow, while Nocticula watches with breathless admiration.{/n} {b}Iomedae:{/b} \"To reject both Heaven and the Abyss... to cast aside the divine design in the name of unguided mortal will. It is a path of terrifying peril, Commander. Yet... the purity of your conviction rivals that of the greatest martyrs of old. May your mortal spirit withstand the storm to come.\"", "IsekaiProficiencies", new ContextActionLoopBreakerInsight());
				AddSummitAnswer("IsekaiGoddessesSummitTrickster", "(Isekai Protagonist) [Mock Both Pantheons] \"Hold on, wait a moment! Look at the two of you posing like rival divas on opening night! Half the watching gods in the celestial sphere want Iomedae's righteousness, the other half want Nocticula's wicked charm, and I'm just here enjoying the greatest celestial spectacle in planar history! Why choose when we can keep everyone guessing?\"", "{n}Both deities pause in sheer, disarmed bewilderment as starlight ripples across the air in amused celestial harmony.{/n} {b}Iomedae:{/b} \"Rival divas?! Opening night?! You treat the fate of mortal souls like an evening tavern play!\" {b}Nocticula:{/b} {n}smirking with wicked delight{/n} \"A theatrical spectacle... oh, Commander, your insolence is positively intoxicating. Let the celestial gallery watch!\"", "IsekaiProficiencies", new ContextActionGoddessesChatBanter());
				AddSummitAnswer("IsekaiGoddessesSummitOverlord", "(Overlord) [Castigate Both Goddesses] \"An insolent goddess preaching submission and a demon queen whispering seduction. Neither of you commands a Supreme Being. The Great Tomb bows to no deity. The Worldwound shall be sealed by my sovereign decree alone.\"", "{n}Iomedae's radiant sword blazes with righteous indignation, while Nocticula chuckles softly into her sleeve.{/n} {b}Iomedae:{/b} \"Supreme Being? Your pride borders on the greatest heresies of the ancient world!\" {b}Nocticula:{/b} \"Let him speak, Inheritor! A tyrant who looks down on both heaven and hell is far more amusing than your pious sermons.\"", "OverlordProficiencies", new ContextActionGoddessesChatBanter());
				AddSummitAnswer("IsekaiGoddessesSummitDevourer", "(Slime) [Proclaim the Tempest Way] \"Why does everything have to be an ultimatum? In the Tempest Federation, we don't force people to throw away their strength or become monsters. We protect our friends, build cozy homes, and eat good food. Keep your dogmas; we're fixing this our way!\"", "{n}Iomedae blinks, momentarily disarmed by your earnest, cheerful declaration.{/n} {b}Iomedae:{/b} \"A cozy home... and good food? You speak of simple mortal peace in the face of planar destruction. If your strange federation can truly restore peace to these tortured lands, then prove it at Threshold, Commander.\"", "DevourerProficiencies", new ContextActionGoddessesChatBanter());
				AddSummitAnswer("IsekaiGoddessesSummitGodEmperor", "(God Emperor) [Declare Sovereign Autarchy] \"The era of foreign gods deciding mortal destinies ends today. The Golden Throne of Sarkoris shall not kneel to Heaven nor Hell. My imperial reign is absolute.\"", "{n}Iomedae looks upon you with solemn gravity.{/n} {b}Iomedae:{/b} \"Many mortal kings have proclaimed absolute autarchy, only to fall to hubris. If you claim the throne of Sarkoris, rule with righteousness, not tyranny.\"", "GodEmperorProficiencies", new ContextActionGoddessesChatBanter());
				AddSummitAnswer("IsekaiGoddessesSummitMastermind", "(Mastermind) [Expose the Binary False Choice] \"A classic false dichotomy. Iomedae offers sacrificial obsolescence; Nocticula offers parasitic dependency. Both algorithms lead to systemic failure. I select Option Three: mortal transcendence.\"", "{n}Nocticula laughs with sharp, appreciative brilliance.{/n} {b}Nocticula:{/b} \"Parasitic dependency? Oh, you wound me, Commander! But I cannot fault a mind so delightfully immune to divine coercion. Show us your Option Three, then!\"", "MastermindProficiencies", new ContextActionGoddessesChatBanter());
				AddSummitAnswer("IsekaiGoddessesSummitShadowMonarch", "(Shadow Monarch) [Embrace the Sovereign Eclipse] \"Light and lust are equally fleeting. When the battle for Threshold begins, neither Heaven nor Alushinyrra will hold the line. My legion of shadows shall swallow the Abyss whole.\"", "{n}Iomedae tightens her grip on her holy blade as shadows swallow the stones of the summit.{/n} {b}Iomedae:{/b} \"The dark power you harbor is perilous beyond measure, Commander. Do not let the shadows you command become your own tomb.\"", "ShadowMonarchProficiencies", new ContextActionGoddessesChatBanter());
			}
			void AddSummitAnswer(string name, string text, string cueText, string proficiencyFactName, ContextAction extraAction = null)
			{
				BlueprintFeature proficiencyFact = GetRequiredDialogueFact(proficiencyFactName);
				if (proficiencyFact != null)
				{
					BlueprintCue reply = TTCoreExtensions.CreateCue(name + "Reply", delegate(BlueprintCue bp)
					{
						bp.SetText(Main.IsekaiContext, cueText);
						if (iomedaeUnit != null)
						{
							bp.Speaker = new DialogSpeaker
							{
								m_Blueprint = iomedaeUnit.ToReference<BlueprintUnitReference>(),
								MoveCamera = true
							};
						}
						bp.SetAnswersList(answersList);
					});
					BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
					{
						bp.SetText(Main.IsekaiContext, text);
						bp.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
						bp.ShowOnce = true;
						bp.AddShowCondition(delegate(HasFact c)
						{
							c.Unit = new PlayerCharacter();
							c.m_Fact = proficiencyFact.ToReference<BlueprintUnitFactReference>();
						});
						if (extraAction != null)
						{
							bp.OnSelect = Helpers.CreateActionList(extraAction);
						}
					});
					answersList.InsertAnswer(answer);
				}
			}
		}

		private static void AddThresholdGrandFinalEndingReactions()
		{
			BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>("1dd910a1bafd4af4f818b65a5eed2a46");
			BlueprintCue continueCue = BlueprintTools.GetBlueprint<BlueprintCue>("91e29e9f52c11c841bcdb271f44400a3");
			if (answersList != null)
			{
				AddEndingAnswer("IsekaiEndingUnwrittenDawn", "(Isekai Protagonist) [Shatter the Cosmic Loop: The Unwritten Dawn] \"I invoke the four Keys of Causal Rupture! Yog-Sothoth, your recurring playback ends today! The loop is broken, and Golarion and Earth are forever linked!\"", "{n}The four Keys of Causal Rupture ignite in brilliant, unearthly light, shattering the invisible temporal anchors holding the Worldwound in recurrence. Reality fractures and reforms into an open, boundless dawn.{/n} \"The cycle is unchained. An open sky awaits.\"", "IsekaiProficiencies", "UnwrittenDawn", "The Unwritten Dawn", checkKeys: true);
				AddEndingAnswer("IsekaiEndingInterdimensionalTraveler", "(Isekai Protagonist) [Slipstream Wanderer: The Interdimensional Traveler] \"I seal the Worldwound and step into the dimensional slipstream to explore the Great Beyond, leaving Golarion in mortal hands.\"", "{n}With a calm smile, you seal the tear from within the slipstream and set forth into the endless multiverse, roaming the astral currents as an eternal wanderer.{/n}", "IsekaiProficiencies", "InterdimensionalTraveler", "The Interdimensional Traveler", checkKeys: false);
				AddEndingAnswer("IsekaiEndingEternalActor", "(Isekai Protagonist) [Surrender to the Broadcast: The Eternal Actor] \"Let the Constellations have their eternal show. When this era fades, let the causal clock rewind for another season.\"", "{n}You bow to the watching stars, accepting the warm embrace of celebrity and comfort as the celestial audience applauds the final act of this run.{/n}", "IsekaiProficiencies", "EternalActor", "The Eternal Actor", checkKeys: false);
				AddEndingAnswer("IsekaiEndingPaxSepulchrum", "(Overlord) [Sovereign Protectorate: Pax Sepulchrum] \"I claim Sarkoris under the eternal protectorate of the Great Tomb. The dead shall labor and defend, and mortal citizens shall live in unprecedented prosperity.\"", "{n}An army of disciplined deathknights and bone laborers spreads out across Sarkoris, establishing an era of impeccable order, rich harvests, and zero crime under the Supreme One's impartial gaze.{/n}", "OverlordProficiencies", "PaxSepulchrum", "Pax Sepulchrum - Guardian of the Great Tomb", checkKeys: true);
				AddEndingAnswer("IsekaiEndingIronCitadel", "(Overlord) [Isolated Bastion: Dominion of the Iron Citadel] \"I raise obsidian ramparts around Sarkoris, isolating our necropolis from the petty squabbles of the outside world.\"", "{n}Massive black stone towers seal the borders of Sarkoris, establishing an impregnable, quiet sanctuary of undeath where no outsider dares tread.{/n}", "OverlordProficiencies", "IronCitadel", "Dominion of the Iron Citadel", checkKeys: false);
				AddEndingAnswer("IsekaiEndingNecroTyrant", "(Overlord) [Total Harvest: The Eternal Necro-Tyrant] \"Mortal life is a fragile, chaotic disease. I harvest all living souls in Avistan into the silent, disciplined undead legion.\"", "{n}A chilling, necrotic storm sweeps across northern Avistan. Life falls silent; every soul rises in flawless, eternal obedience to the Great Tomb.\"", "OverlordProficiencies", "NecroTyrant", "The Eternal Necro-Tyrant", checkKeys: false);
				AddEndingAnswer("IsekaiEndingTempestFederation", "(Slime) [Multiversal Haven: The Tempest Multiverse Federation] \"I metabolize the Worldwound into pure fertile mana and establish the Tempest Multiverse Federation for all beings to live and feast together!\"", "{n}The demonic rift dissolves into a sea of radiant, fertile mana. The Jura Forest blooms where the scar once bled, founding a paradise of hot-spring resorts, culinary marvels, and peaceful co-existence.{/n}", "DevourerProficiencies", "TempestFederation", "The Tempest Multiverse Federation", checkKeys: true);
				AddEndingAnswer("IsekaiEndingPrimevalMarsh", "(Slime) [Ancient Slumber: The Primeval Marsh] \"I return Sarkoris to a wild primeval haven, curling up beneath the World Tree as Golarion's guardian beast-god.\"", "{n}Nature reclaims the scorched plains. Curling into a tranquil, living sphere at the roots of the World Tree, you sleep in harmony with the wild beasts of the world.{/n}", "DevourerProficiencies", "PrimevalMarsh", "The Primeval Marsh", checkKeys: false);
				AddEndingAnswer("IsekaiEndingAbyssalSingularity", "(Slime) [Infinite Gluttony: The Abyssal Singularity] \"I dive straight into the Abyss to devour demon lords and planar rifts until Azathoth stirs in fitful slumber, forcing Yog-Sothoth to trigger an emergency temporal reset!\"", "{n}Your bottomless stomach devours abyssal layers and dimensional fabric alike. As your hunger begins to disturb the nuclear chaos of Azathoth at the center of infinity, a panicked Yog-Sothoth triggers an emergency temporal failsafe, snapping reality back to the start of the loop!{/n}", "DevourerProficiencies", "AbyssalSingularity", "The Abyssal Singularity", checkKeys: false);
				AddEndingAnswer("IsekaiEndingEternalHope", "(Hero) [Celestial Nova: The Sovereign Beacon of Eternal Hope] \"I channel the collective hope of every mortal who ever lived into an undying nova, ascending as the eternal protector of worlds!\"", "{n}A brilliant, golden star blazes where the Worldwound used to fester. You ascend as the wandering guardian spirit who answers innocent prayers across all dimensions.{/n}", "HeroProficiencies", "EternalHope", "The Sovereign Beacon of Eternal Hope", checkKeys: true);
				AddEndingAnswer("IsekaiEndingRetiredLegend", "(Hero) [Humble Peace: The Retired Legend] \"I refuse godhood and crowns. I will open a cozy tavern and training hall in Kenabres, living a fulfilled mortal life.\"", "{n}Laying down your legendary weapons, you hang a simple wooden sign over a Kenabres tavern, training new generations and sharing laughter with beloved companions.{/n}", "HeroProficiencies", "RetiredLegend", "The Retired Legend", checkKeys: false);
				AddEndingAnswer("IsekaiEndingCorruptedChampion", "(Hero) [Fallen Paragon: The Corrupted Champion] \"I have lived long enough to become the villain. In the name of protecting everyone, I shall enforce absolute peace with an iron fist.\"", "{n}Driven by paranoia and the unbearable weight of sacrifice, your crusade for good hardens into ruthless, despotic tyranny until the world trembles before your conviction.{/n}", "HeroProficiencies", "CorruptedChampion", "The Corrupted Champion", checkKeys: false);
				AddEndingAnswer("IsekaiEndingPaxSolaris", "(God Emperor) [Solar Renaissance: Pax Solaris] \"I ascend the Golden Throne of Sarkoris as the eternal God-King, ushering in an era of solar technology, justice, and prosperity!\"", "{n}Radiant solar towers rise from Drezen to the World's Edge Mountains. The Solar Commonwealth unites all nations in an era of unprecedented cultural and scientific enlightenment.{/n}", "GodEmperorProficiencies", "PaxSolaris", "Pax Solaris - The Golden Renaissance", checkKeys: true);
				AddEndingAnswer("IsekaiEndingSarkorianImperium", "(God Emperor) [Iron Mandate: The Sarkorian Imperium] \"I forge the Sarkorian Imperium, ruling Avistan through unassailable military discipline and absolute law.\"", "{n}An imperial legion marches to every border, establishing absolute stability and commercial dominance through unwavering military authority.{/n}", "GodEmperorProficiencies", "SarkorianImperium", "The Sarkorian Imperium", checkKeys: false);
				AddEndingAnswer("IsekaiEndingDualThrone", "(God Emperor) [Conquest of Two Realms: The Dual Throne of Sun and Abyss] \"I march the solar legions into the Abyss, binding mortal and fiend under the undisputed supremacy of the Dual Throne.\"", "{n}Solar banners are planted into abyssal magma. Dissent is eradicated across two planes as both mortals and demons kneel before your absolute authority.{/n}", "GodEmperorProficiencies", "DualThrone", "The Dual Throne of Sun and Abyss", checkKeys: false);
				AddEndingAnswer("IsekaiEndingSilentAegis", "(Shadow Monarch) [Watchers in the Dark: The Silent Aegis of Shadows] \"I command the Legion of Shadows to shield Golarion from the dark, ensuring night becomes a guardian rather than a terror.\"", "{n}Shadow soldiers stand silent, invisible guard over every road and hamlet. Innocents sleep peacefully, protected by the sovereign darkness of the Netherworld.{/n}", "ShadowMonarchProficiencies", "SilentAegis", "The Silent Aegis of Shadows", checkKeys: true);
				AddEndingAnswer("IsekaiEndingObsidianBastion", "(Shadow Monarch) [The Shadow Gate: The Obsidian Bastion] \"I transform the Worldwound into the Shadow Gate, sitting upon the threshold so neither demon nor conqueror ever crosses.\"", "{n}An immense black monolith seals the boundary between planes. Seated upon the obsidian throne, you remain the eternal gatekeeper between realities.{/n}", "ShadowMonarchProficiencies", "ObsidianBastion", "The Obsidian Bastion", checkKeys: false);
				AddEndingAnswer("IsekaiEndingNetherworldCataclysm", "(Shadow Monarch) [Eternal Void: The Netherworld Cataclysm] \"I open the Netherworld wide across Avistan, drowning the continent in an eternal black void where only shadows live.\"", "{n}The sun never rises again. Cold, abyssal darkness submerges northern Avistan, turning Golarion into a silent realm where shadows walk the earth forever.{/n}", "ShadowMonarchProficiencies", "NetherworldCataclysm", "The Netherworld Cataclysm", checkKeys: false);
				AddEndingAnswer("IsekaiEndingGrandArchitect", "(Mastermind) [The Grand Architect] \"Using my immortality and foresight, I shall weave an unseen web across centuries, guiding nations toward quiet prosperity from the shadows.\"", "{n}Operating from behind the velvet curtain, you gently orchestrate history like a grand symphony of statecraft, nurturing centuries of enlightened human flourishing.{/n}", "MastermindProficiencies", "GrandArchitect", "The Grand Architect", checkKeys: true);
				AddEndingAnswer("IsekaiEndingPuppetParliament", "(Mastermind) [The Puppet Parliament] \"I install figureheads across every throne in Avistan, pulling the geopolitical strings of kings and archbishops as a secret shadow government.\"", "{n}Crowns and miters bow to invisible strings. In a quiet, candlelit chamber, your council of shadows directs the destinies of empires without a single mortal suspecting who truly rules.{/n}", "MastermindProficiencies", "PuppetParliament", "The Puppet Parliament", checkKeys: false);
				AddEndingAnswer("IsekaiEndingChessboardApocalypse", "(Mastermind) [The Chessboard Apocalypse] \"Mortals, demons, and gods are mere pawns on my board. I sacrifice whole civilizations to see what patterns emerge from the chaos.\"", "{n}Detached from mortal sentiment, you orchestrate devastating continental conflicts and planar collapses purely for your intellectual amusement, observing the slaughter like a game of speed chess.{/n}", "MastermindProficiencies", "ChessboardApocalypse", "The Chessboard Apocalypse", checkKeys: false, forceShatter: true);
				AddEndingAnswer("IsekaiEndingInfiniteHorizons", "(Martial God) [Infinite Horizon: Sovereign Ki Ascension] \"My Ki is sovereign, and my martial will transcends cosmic cycles. I slice through Yog-Sothoth's causal anchors with one infinite strike!\"", "{n}Your radiant Ki surges to an unbearable crescendo, slicing the very fabric of the causal loop in twain. The infinite horizons of the cosmos lie open before you.{/n}", "MartialGodProficiencies", "InfiniteHorizons", "The Edge of Infinite Horizons", checkKeys: true);
				AddEndingAnswer("IsekaiEndingBoundaryHermit", "(Martial God) [Meditative Boundary: The Boundary Hermit] \"I retire to a tranquil pocket dimension on the astral fringe, meditating upon the eternal Dao of Ki until the causal loop resets.\"", "{n}Seated in quiet contemplation between planes, you refine your internal Ki and cultivate the eternal Dao. At the moment of ultimate enlightenment, the cosmic clock chimes and time gently rewinds.{/n}", "MartialGodProficiencies", "BoundaryHermit", "The Boundary Hermit", checkKeys: false);
				AddEndingAnswer("IsekaiEndingSeveredHorizon", "(Martial God) [Reality Fracture: The Severed Horizon] \"I unleash an unrestrained eruption of Ki that strikes reality too deeply, collapsing the dimensional boundaries of Golarion!\"", "{n}The boundless Ki detonates beyond all containment, shattering the foundational tapestry of existence. The world collapses into glowing static, leaving you standing alone in the void.{/n}", "MartialGodProficiencies", "SeveredHorizon", "The Severed Horizon", checkKeys: false, forceShatter: true);
			}
			void AddEndingAnswer(string name, string text, string cueText, string proficiencyFactName, string endingId, string endingTitle, bool checkKeys, bool forceShatter = false)
			{
				BlueprintFeature proficiencyFact = GetRequiredDialogueFact(proficiencyFactName);
				if (proficiencyFact != null)
				{
					BlueprintCue reply = TTCoreExtensions.CreateCue(name + "Reply", delegate(BlueprintCue bp)
					{
						bp.SetText(Main.IsekaiContext, cueText);
						if (continueCue != null)
						{
							bp.Continue = new CueSelection
							{
								Cues = new List<BlueprintCueBaseReference> { continueCue.ToReference<BlueprintCueBaseReference>() },
								Strategy = Strategy.First
							};
						}
					});
					BlueprintAnswer answer = TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer bp)
					{
						bp.SetText(Main.IsekaiContext, text);
						bp.NextCue = new CueSelection
						{
							Cues = new List<BlueprintCueBaseReference> { reply.ToReference<BlueprintCueBaseReference>() },
							Strategy = Strategy.First
						};
						bp.ShowOnce = true;
						bp.AddShowCondition(delegate(HasFact c)
						{
							c.Unit = new PlayerCharacter();
							c.m_Fact = proficiencyFact.ToReference<BlueprintUnitFactReference>();
						});
						bp.OnSelect = Helpers.CreateActionList(new ContextActionTriggerIsekaiEnding
						{
							EndingId = endingId,
							EndingTitle = endingTitle,
							CheckLoopBreakerKeys = checkKeys,
							ForceLoopShatter = forceShatter
						});
					});
					answersList.InsertAnswer(answer);
				}
			}
		}
	}
}
