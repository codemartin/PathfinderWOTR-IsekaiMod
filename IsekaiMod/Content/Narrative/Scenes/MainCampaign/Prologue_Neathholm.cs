using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Narrative.Actions;
using Kingmaker.UnitLogic.Alignments;

namespace IsekaiMod.Content.Narrative.Scenes.MainCampaign
{
	public static class Prologue_Neathholm
	{
		public static void Register()
		{
			RegisterOpeningStretcher();
			RegisterMeetSeelahAnevia();
			RegisterMeetCamellia();
			RegisterMeetLannWenduag();
			RegisterNeathholmChiefSull();
			RegisterShieldMazeHosilla();
		}

		private static void RegisterOpeningStretcher()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Prologue_Festival_OpeningStretcher", "87997a477e6a58d4aae46e26a3712825", "Prologue", "Kenabres Festival: Waking up on the stretcher");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Prologue_Stretcher_Universal_Carriage",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.NeutralGood,
				PromptText = "(Isekai Protagonist) \"Let me guess... I just survived a runaway carriage, and now I'm waking up in an unfamiliar fantasy city surrounded by strangers? What kind of opening scene is this?\"",
				NpcReplyText = "{n}The medic and the townspeople look at one another with perplexed expressions, gently offering you a damp cloth.{/n} \"You're in Kenabres, friend. You were severely wounded in an accident, but Lady Terendelev healed your vitals. Rest easy!\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.MainCampaign,
					SceneContext = "Opening Stretcher: Carriage Trope",
					CosmicCoins = 50,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> chuckles into his tankard: \"Ah, the classic 'waking up in a cart' opening! Never gets old across the planes.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Prologue_Stretcher_Slime_Hunger",
				Type = OptionType.Subclass,
				RequiredProficiency = "DevourerProficiencies",
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Slime) \"I feel soft... yet oddly hollow inside. Do you have any roasted skewers or fresh fruit nearby? I need to replenish my caloric reserves immediately.\"",
				NpcReplyText = "{n}The attendant chuckles warmly, handing you a canteen of water.{/n} \"A vigorous appetite is the surest sign of recovery! Drink up, and you can visit the festival booths once you can stand.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Great Devourer",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Opening Stretcher: Slime Appetite",
					CosmicCoins = 60,
					Lines = new List<string> { "<color=#8B0000><b>[The Constellation 'The Rough Beast']</b></color> rumbles deep underground: \"HUNGER... THE FIRST PULSE OF EXISTENCE...\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Prologue_Stretcher_ShadowMonarch_Awakening",
				Type = OptionType.Subclass,
				RequiredProficiency = "ShadowMonarchProficiencies",
				Alignment = AlignmentShiftDirection.LawfulNeutral,
				PromptText = "(Shadow Monarch) \"My mana circuits are realigning. The shadows in this square feel heavy, anchored to a tear in the sky. Step back, please; I can stand on my own.\"",
				NpcReplyText = "{n}The crusader medics pause, stepping back instinctively as an unnatural hush settles over the stretcher.{/n} \"Remarkable fortitude... Most mortals would be bedridden for a week after injuries like yours.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Pale Lady",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Opening Stretcher: Shadow Awakening",
					CosmicCoins = 60,
					Lines = new List<string> { "<color=#B0C4DE><b>[The Constellation 'The Lady of Graves']</b></color> whispers coldly: \"A soul that crossed the River of Souls without judgment. We shall watch your thread.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Prologue_Stretcher_MetaLoop_CycleRestart",
				Type = OptionType.MetaLoop,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Cycle Insight) \"The square. The festival balloons. Terendelev's scales. The clockwork wheel has spun backwards to the very beginning once again.\"",
				NpcReplyText = "{n}The townsfolk glance nervously at your far-off gaze.{/n} \"Are you well, traveler? You speak as if you've seen this festival a hundred times before.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Key and the Gate",
					Category = ConstellationCategory.MetaLoop,
					SceneContext = "Opening Stretcher: Temporal Recurrence",
					CosmicCoins = 100,
					Lines = new List<string> { "<color=#BA55D3><b>[The Constellation 'The Key and the Gate']</b></color> resonates through the void: \"The tape rewinds. The actor takes their mark once more upon the stage of Kenabres.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterMeetSeelahAnevia()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Prologue_Caves_MeetSeelahAnevia", "a55fc20c6f0ff56439b40d6ba53cb8d7", "Prologue", "Underground Caves: First encounter with Seelah and wounded Anevia");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Prologue_MeetSeelah_Universal_Triage",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.LawfulGood,
				PromptText = "(Isekai Protagonist) \"Hold on, both of you. Despair won't dig us out of this cavern. Let's splint Anevia's leg properly and secure our perimeter before panic sets in.\"",
				NpcReplyText = "{n}Seelah breathes a sigh of profound relief, wiping dust from her armor.{/n} \"By the Inheritor, a level head is exactly what we needed. Let's bind her leg and find a way back to the surface!\"",
				ChainedReplies = new List<NarrativeCueDefinition>
				{
					new NarrativeCueDefinition
					{
						SpeakerGuid = "54be53f0b35bf3c4592a97ae335fe765",
						Text = "{n}Seelah secures the makeshift splint with practiced crusader knots, nodding gratefully.{/n} \"Good work. With that leg braced, she won't tear the bone walking.\"",
						MoveCamera = true
					},
					new NarrativeCueDefinition
					{
						SpeakerGuid = "ea562adea1736874c9c5616d140fe773",
						Text = "{n}Anevia bites her lip through the pain, testing her weight against the cavern wall with a wincing nod.{/n} \"Thanks to both of you. I can hobble well enough to keep up. Let's find those mongrels.\"",
						MoveCamera = true
					}
				},
				Rewards = new SceneRewards
				{
					Gold = 100,
					CosmicCoins = 75
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Inheritor",
					Category = ConstellationCategory.Companion,
					SceneContext = "Underground Caves: Field Triage",
					CosmicCoins = 75,
					Lines = new List<string> { "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> nods in approval: \"True valor shines brightest in the depths of disaster. Take heart, champions!\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Prologue_MeetSeelah_GodEmperor_Command",
				Type = OptionType.Subclass,
				RequiredProficiency = "GodEmperorProficiencies",
				Alignment = AlignmentShiftDirection.LawfulNeutral,
				PromptText = "(God Emperor) \"Rise, crusaders. You are in the presence of sovereign authority. No demon or collapsed ceiling shall claim your lives while you stand under my standard.\"",
				NpcReplyText = "{n}Anevia stares up from the rubble, her pain momentary eclipsed by awe at your commanding aura.{/n} \"Whoever you are... you certainly know how to inspire a squad. Lead the way, Commander.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Prince of Darkness",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Underground Caves: Imperial Command",
					CosmicCoins = 80,
					Lines = new List<string> { "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> smiles faintly: \"Authority assumed at the moment of crisis. Order is forged in catastrophe.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterMeetCamellia()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Prologue_Caves_MeetCamellia", "1ca6cf08fceeac141a0df689cecc784a", "Prologue", "Underground Caves: Discovering Camellia amidst the rubble");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Prologue_MeetCamellia_Universal_Intuition",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				PromptText = "(Isekai Protagonist) \"You seem surprisingly composed for someone standing beside a fresh corpse in a dark cavern, my lady. But we can discuss your hobbies once we find an exit.\"",
				NpcReplyText = "{n}Camellia stiffens for an instant, then smooths her expression into a polite, aristocratic smile.{/n} \"A noble lady learns to maintain her dignity in all circumstances. Shall we join forces, traveler? You seem... capable.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.Companion,
					SceneContext = "Underground Caves: Camellia Intuition",
					CosmicCoins = 75,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> lowers his mug: \"Keep an eye on that one, kid. That smile doesn't reach her eyes at all.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Prologue_MeetCamellia_Mastermind_MicroExpression",
				Type = OptionType.Subclass,
				RequiredProficiency = "MastermindProficiencies",
				Alignment = AlignmentShiftDirection.LawfulEvil,
				PromptText = "(Mastermind) \"Dilated pupils, elevated pulse, an almost euphoric tension in your grip on that rapier. You aren't panicked by the slaughter above; you are savoring it. Fascinating.\"",
				NpcReplyText = "{n}Camellia takes a sharp, shallow breath, her haughty demeanor faltering for a heartbeat.{/n} \"What absurd deductions! I am merely shaken by our fall! Let us proceed and speak no more of this nonsense!\"",
				ChainedReplies = new List<NarrativeCueDefinition>
				{
					new NarrativeCueDefinition
					{
						SpeakerGuid = "397b090721c41044ea3220445300e1b8",
						Text = "{n}Camellia quickly turns her face away, adjusting her necklace with trembling fingers before regaining her cool posture.{/n} \"You imagine things, stranger. We are survivors in a demon war, nothing more.\"",
						MoveCamera = true
					},
					new NarrativeCueDefinition
					{
						SpeakerGuid = "54be53f0b35bf3c4592a97ae335fe765",
						Text = "{n}Seelah glances between you and Camellia with a suspicious frown, resting a hand on her pommel.{/n} \"Hey... whatever strange tension is going on between you two, keep it under control. We need every sword ready for demons.\"",
						MoveCamera = true
					}
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Prince of Darkness",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Underground Caves: Mastermind Deduction",
					CosmicCoins = 90,
					Lines = new List<string> { "<color=#DC143C><b>[The Constellation 'The Prince of Darkness']</b></color> chuckles smoothly: \"Dismantling a sociopath's disguise in thirty seconds. Truly refined intellect.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterMeetLannWenduag()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Prologue_Caves_MeetLannWenduag", "61ba231b3ad6b144a918c88ca49cb92a", "Prologue", "Underground Caves: Meeting the mongrel hunters Lann and Wenduag");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Prologue_MeetLann_Universal_NoPrejudice",
				Type = OptionType.Universal,
				Alignment = AlignmentShiftDirection.ChaoticGood,
				PromptText = "(Isekai Protagonist) \"I don't care about surface prejudices or strange lineages. You survived down here where surface knights perished. Let's work together to reach your village.\"",
				NpcReplyText = "{n}Lann blinks in astonishment, scratching the scales on his cheek.{/n} \"Well, that's a refreshing change of pace! Usually up-dwellers scream and reach for pitchforks. Wenduag, look, an uplander who talks sense!\"",
				Rewards = new SceneRewards
				{
					CosmicCoins = 75
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Song of the Spheres",
					Category = ConstellationCategory.Companion,
					SceneContext = "Underground Caves: Mongrel Acceptance",
					CosmicCoins = 75,
					Lines = new List<string> { "<color=#00FFFF><b>[The Constellation 'The Song of the Spheres']</b></color> smiles like starlight: \"Kindness offered without hesitation to those cast into darkness. How beautiful.\"" }
				}
			});
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Prologue_MeetLann_Hero_ProtectMongrels",
				Type = OptionType.Subclass,
				RequiredProficiency = "HeroProficiencies",
				Alignment = AlignmentShiftDirection.NeutralGood,
				PromptText = "(Hero) \"Descendants of the First Crusade who kept fighting in the dark for a century? You aren't outcasts to me; you are heroes who held the line when no one was watching.\"",
				NpcReplyText = "{n}Wenduag's yellow eyes narrow with intense scrutiny while Lann stares in stunned silence.{/n} \"'Heroes'? Nobody has ever called our people that. You speak strange words, uplander... but I like them.\"",
				Banter = new ConstellationReaction
				{
					Sponsor = "The Dawnflower",
					Category = ConstellationCategory.Subclass,
					SceneContext = "Underground Caves: Honoring the Mongrels",
					CosmicCoins = 80,
					Lines = new List<string> { "<color=#FFD700><b>[The Constellation 'The Dawnflower']</b></color> radiates warmth: \"Restoring pride and honor to the forgotten champions of the First Crusade. May your light guide them!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterNeathholmChiefSull()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Prologue_Neathholm_ChiefSull", "5233c72929cda7c48b92ec6292a730ac", "Prologue", "Neathholm: Audience with Chief Sull regarding the Shield Maze and lost youths");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Prologue_ChiefSull_ThirdOption_Harmonize",
				Type = OptionType.ThirdOption,
				Alignment = AlignmentShiftDirection.LawfulGood,
				PromptText = "(Otherworld Perspective) \"Lann wants to inspire your people with the sword's light; Wenduag wants to protect them from surface wrath. Neither is wrong. Let us display the sword to awaken your youth's pride, while keeping your village location concealed from surface fanatics.\"",
				NpcReplyText = "{n}Chief Sull leans heavily upon his staff, tears glistening in his mismatched eyes.{/n} \"A path of wisdom that neither denies hope nor invites slaughter. You speak with the clarity of an elder who has walked between stars, traveler. Go with our blessing!\"",
				Rewards = new SceneRewards
				{
					Gold = 250,
					CosmicCoins = 100
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Inheritor",
					Category = ConstellationCategory.Quest,
					SceneContext = "Neathholm: Harmonizing Sull's Council",
					CosmicCoins = 100,
					Lines = new List<string> { "<color=#E6E6FA><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color> smiles with celestial grace: \"Justice tempered by prudence. You honor the legacy of the First Crusade.\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}

		private static void RegisterShieldMazeHosilla()
		{
			NarrativeScene narrativeScene = new NarrativeScene("Prologue_ShieldMaze_HosillaClimax", "d35d5c76186af32468dc71952eb339ec", "Prologue", "Shield Maze: Confrontation with Hosilla and the dual recruitment of Lann and Wenduag");
			narrativeScene.AddOption(new NarrativeOption
			{
				Id = "Prologue_ShieldMaze_ThirdOption_DualRecruit",
				Type = OptionType.ThirdOption,
				Alignment = AlignmentShiftDirection.TrueNeutral,
				CustomAction = new ContextActionLannWenduagDualRecruit(),
				PromptText = "(Omniscient Reincarnator) \"Lann, Wenduag, cease your squabbling. I refuse to let petty surface jealousy tear your people apart. Hosilla's poison ends today, and both of you will march at my side to reclaim the surface world.\"",
				NpcReplyText = "{n}Wenduag lowers her bow in stunned submission while Lann gasps in disbelief. For the first time in their lives, neither mongrel feels discarded.{/n} \"You... you would command both of us together? Then by the blood of our ancestors, our blades belong to you!\"",
				Rewards = new SceneRewards
				{
					Gold = 500,
					CosmicCoins = 250
				},
				Banter = new ConstellationReaction
				{
					Sponsor = "The Laughing King",
					Category = ConstellationCategory.Quest,
					SceneContext = "Shield Maze: Lann & Wenduag Dual Allegiance",
					CosmicCoins = 250,
					Lines = new List<string> { "<color=#F5C542><b>[The Constellation 'The Lucky Drunk']</b></color> raises his glass with a roar of laughter: \"Why choose when you can bring both into the tavern party?! Outstanding move, Commander!\"" }
				}
			});
			NarrativeRegistry.RegisterScene(narrativeScene);
		}
	}
}
