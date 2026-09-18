using System;
using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Narrative.Actions;
using IsekaiMod.Content.Narrative.Scenes.Companions;
using IsekaiMod.Content.Narrative.Scenes.DLCs;
using IsekaiMod.Content.Narrative.Scenes.MainCampaign;
using IsekaiMod.Content.Narrative.Scenes.MythicPaths;
using IsekaiMod.Content.Narrative.Scenes.Romance;
using IsekaiMod.Content.Narrative.Scenes.SideQuests;
using IsekaiMod.Content.Narrative.Scenes.SpecialInteractions;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Experience;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Alignments;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Narrative
{
	public static class NarrativeEngine
	{
		private static bool _compiled;

		public static BlueprintFeature GetRequiredDialogueFact(string factName)
		{
			if (string.IsNullOrEmpty(factName))
			{
				return null;
			}
			if (factName.Equals("IsekaiProficiencies", StringComparison.OrdinalIgnoreCase))
			{
				return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PlotArmor") ?? BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProficiencies");
			}
			if (factName.Equals("SlimeProficiencies", StringComparison.OrdinalIgnoreCase))
			{
				return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DevourerProficiencies");
			}
			return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, factName);
		}

		public static void RegisterAllDefaultScenes()
		{
			Prologue_Neathholm.Register();
			Act1_KenabresLiberation.Register();
			Act2_MarchOnDrezen.Register();
			Act3_DrezenReign.Register();
			Act4_MidnightIsles.Register();
			Act5_WorldwoundEnd.Register();
			Act6_ThresholdAscension.Register();
			SideQuests_Crusade.Register();
			Companions_Climaxes.Register();
			Companion_RetinueCampBanter.Register();
			Mythic_Injections.Register();
			DLC_Expansions.Register();
			Romance_Scenes.Register();
			ArtifactInteractions.Register();
		}

		public static void CompileAll()
		{
			if (_compiled)
			{
				return;
			}
			_compiled = true;
			NarrativeCustomActions.InitBuffsAndItems();
			if (NarrativeRegistry.GetAllScenes().Count == 0)
			{
				RegisterAllDefaultScenes();
			}
			int currentCycle = TimelineManager.GetCurrentCycle();
			int num = 0;
			int num2 = 0;
			foreach (NarrativeScene allScene in NarrativeRegistry.GetAllScenes())
			{
				if (string.IsNullOrEmpty(allScene.TargetAnswersListGuid))
				{
					continue;
				}
				BlueprintAnswersList answersList = BlueprintTools.GetBlueprint<BlueprintAnswersList>(allScene.TargetAnswersListGuid);
				if (answersList == null)
				{
					Main.IsekaiContext.Logger.LogWarning("[NarrativeEngine] Target AnswersList not found for scene '" + allScene.SceneId + "' (GUID: " + allScene.TargetAnswersListGuid + ")");
					continue;
				}
				num++;
				foreach (NarrativeOption option in allScene.Options)
				{
					if (string.IsNullOrEmpty(option.Id))
					{
						continue;
					}
					string promptText = option.PromptText;
					string replyText = option.NpcReplyText;
					if (option.Variations != null && option.Variations.Count > 0)
					{
						foreach (CycleVariation variation in option.Variations)
						{
							if (currentCycle >= variation.MinCycle && currentCycle <= variation.MaxCycle)
							{
								if (!string.IsNullOrEmpty(variation.VariantText))
								{
									promptText = variation.VariantText;
								}
								if (!string.IsNullOrEmpty(variation.VariantReplyText))
								{
									replyText = variation.VariantReplyText;
								}
								break;
							}
						}
					}
					BlueprintCue replyCue = null;
					if (option.ChainedReplies != null && option.ChainedReplies.Count > 0)
					{
						List<BlueprintCue> list = new List<BlueprintCue>();
						for (int i = 0; i < option.ChainedReplies.Count; i++)
						{
							int num3 = i;
							NarrativeCueDefinition def = option.ChainedReplies[num3];
							BlueprintCue item = TTCoreExtensions.CreateCue($"{option.Id}_ChainedCue_{num3}", delegate(BlueprintCue bp)
							{
								bp.SetText(Main.IsekaiContext, def.Text);
								bp.ShowOnce = false;
								if (!string.IsNullOrEmpty(def.SpeakerGuid))
								{
									BlueprintUnitReference blueprintReference2 = BlueprintTools.GetBlueprintReference<BlueprintUnitReference>(def.SpeakerGuid);
									if (blueprintReference2 != null)
									{
										bp.Speaker = new DialogSpeaker
										{
											m_Blueprint = blueprintReference2,
											MoveCamera = def.MoveCamera
										};
									}
								}
								if (def.OnStopActionList != null)
								{
									bp.OnStop = def.OnStopActionList;
								}
								else if (def.OnStopAction != null)
								{
									bp.OnStop = new ActionList
									{
										Actions = new GameAction[1] { def.OnStopAction }
									};
								}
								def.ConfigureCue?.Invoke(bp);
							});
							list.Add(item);
						}
						for (int num4 = 0; num4 < list.Count; num4++)
						{
							BlueprintCue blueprintCue = list[num4];
							if (num4 < list.Count - 1)
							{
								BlueprintCueBaseReference item2 = list[num4 + 1].ToReference<BlueprintCueBaseReference>();
								blueprintCue.Continue = new CueSelection
								{
									Cues = new List<BlueprintCueBaseReference> { item2 },
									Strategy = Strategy.First
								};
							}
							else if (!string.IsNullOrEmpty(option.NextCueGuid))
							{
								BlueprintCueBaseReference blueprintReference = BlueprintTools.GetBlueprintReference<BlueprintCueBaseReference>(option.NextCueGuid);
								if (blueprintReference != null)
								{
									blueprintCue.Continue = new CueSelection
									{
										Cues = new List<BlueprintCueBaseReference> { blueprintReference },
										Strategy = Strategy.First
									};
								}
							}
							else
							{
								blueprintCue.Answers = new List<BlueprintAnswerBaseReference> { answersList.ToReference<BlueprintAnswerBaseReference>() };
							}
						}
						replyCue = list[0];
					}
					else if (!string.IsNullOrEmpty(replyText))
					{
						string name = option.ReplyCueId ?? (option.Id + "ReplyCue");
						replyCue = TTCoreExtensions.CreateCue(name, delegate(BlueprintCue bp)
						{
							bp.SetText(Main.IsekaiContext, replyText);
							bp.ShowOnce = false;
							if (!string.IsNullOrEmpty(option.SpeakerGuid))
							{
								BlueprintUnitReference blueprintReference2 = BlueprintTools.GetBlueprintReference<BlueprintUnitReference>(option.SpeakerGuid);
								if (blueprintReference2 != null)
								{
									bp.Speaker = new DialogSpeaker
									{
										m_Blueprint = blueprintReference2,
										MoveCamera = true
									};
								}
							}
							if (option.CueOnStopActionList != null)
							{
								bp.OnStop = option.CueOnStopActionList;
							}
							else if (option.CueOnStopAction != null)
							{
								bp.OnStop = new ActionList
								{
									Actions = new GameAction[1] { option.CueOnStopAction }
								};
							}
							if (!string.IsNullOrEmpty(option.NextCueGuid))
							{
								BlueprintCueBaseReference blueprintReference3 = BlueprintTools.GetBlueprintReference<BlueprintCueBaseReference>(option.NextCueGuid);
								if (blueprintReference3 != null)
								{
									bp.Continue = new CueSelection
									{
										Cues = new List<BlueprintCueBaseReference> { blueprintReference3 },
										Strategy = Strategy.First
									};
								}
							}
							else
							{
								bp.Answers = new List<BlueprintAnswerBaseReference> { answersList.ToReference<BlueprintAnswerBaseReference>() };
							}
							option.ConfigureReply?.Invoke(bp);
						});
					}
					BlueprintAnswer blueprintAnswer = TTCoreExtensions.CreateAnswer(option.Id, delegate(BlueprintAnswer bp)
					{
						bp.SetText(Main.IsekaiContext, promptText);
						bp.ShowOnce = option.ShowOnce;
						if (option.Alignment.HasValue)
						{
							bp.AlignmentShift = new AlignmentShift
							{
								Direction = option.Alignment.Value,
								Value = 1,
								Description = Helpers.CreateString(Main.IsekaiContext, option.Id + ".AlignmentShift", $"{option.Alignment.Value} Alignment Stance")
							};
						}
						if (replyCue != null)
						{
							bp.NextCue = new CueSelection
							{
								Cues = new List<BlueprintCueBaseReference> { replyCue.ToReference<BlueprintCueBaseReference>() },
								Strategy = Strategy.First
							};
						}
						else if (!string.IsNullOrEmpty(option.NextCueGuid))
						{
							BlueprintCueBaseReference blueprintReference2 = BlueprintTools.GetBlueprintReference<BlueprintCueBaseReference>(option.NextCueGuid);
							if (blueprintReference2 != null)
							{
								bp.NextCue = new CueSelection
								{
									Cues = new List<BlueprintCueBaseReference> { blueprintReference2 },
									Strategy = Strategy.First
								};
							}
						}
						if (option.ExpCR > 0)
						{
							bp.OnSelect = ActionFlow.DoSingle(delegate(GainExp c)
							{
								c.Encounter = EncounterType.SkillCheck;
								c.CR = option.ExpCR;
								c.Modifier = 1f;
							});
						}
						if (option.CustomActionList != null && option.CustomActionList.Actions != null)
						{
							GameAction[] actions = option.CustomActionList.Actions;
							foreach (GameAction gameAction in actions)
							{
								if (gameAction != null)
								{
									if (bp.OnSelect == null)
									{
										bp.OnSelect = new ActionList
										{
											Actions = new GameAction[1] { gameAction }
										};
									}
									else
									{
										bp.OnSelect.Actions = bp.OnSelect.Actions.AppendToArray(gameAction);
									}
								}
							}
						}
						if (option.CustomAction != null)
						{
							if (bp.OnSelect == null)
							{
								bp.OnSelect = new ActionList
								{
									Actions = new GameAction[1] { option.CustomAction }
								};
							}
							else
							{
								bp.OnSelect.Actions = bp.OnSelect.Actions.AppendToArray(option.CustomAction);
							}
						}
						if (option.Rewards != null && (option.Rewards.Gold > 0 || option.Rewards.CosmicCoins > 0 || option.Rewards.Buff != null || (option.Rewards.ItemGuids != null && option.Rewards.ItemGuids.Length != 0)))
						{
							ContextActionGiveOtherworlderRewards contextActionGiveOtherworlderRewards = new ContextActionGiveOtherworlderRewards
							{
								Gold = option.Rewards.Gold,
								Coins = option.Rewards.CosmicCoins,
								Sponsor = (option.Banter?.Sponsor ?? "The Key and the Gate"),
								ItemGuids = option.Rewards.ItemGuids,
								BuffToApply = option.Rewards.Buff
							};
							if (bp.OnSelect == null)
							{
								bp.OnSelect = new ActionList
								{
									Actions = new GameAction[1] { contextActionGiveOtherworlderRewards }
								};
							}
							else
							{
								bp.OnSelect.Actions = bp.OnSelect.Actions.AppendToArray(contextActionGiveOtherworlderRewards);
							}
						}
						if (!string.IsNullOrEmpty(option.ExcludeVanillaAnswerGuid))
						{
							BlueprintAnswerReference vanillaAnswerRef = BlueprintTools.GetBlueprintReference<BlueprintAnswerReference>(option.ExcludeVanillaAnswerGuid);
							if (vanillaAnswerRef != null)
							{
								bp.AddShowCondition(delegate(AnswerSelected c)
								{
									c.Not = true;
									c.m_Answer = vanillaAnswerRef;
								});
							}
						}
						if (!string.IsNullOrEmpty(option.RequiredProficiency))
						{
							BlueprintFeature fact = GetRequiredDialogueFact(option.RequiredProficiency);
							if (fact != null)
							{
								bp.AddShowCondition(delegate(HasFact c)
								{
									c.Unit = new PlayerCharacter();
									c.m_Fact = fact.ToReference<BlueprintUnitFactReference>();
								});
							}
						}
						else
						{
							bp.RequirePlotArmor();
						}
						if (!string.IsNullOrEmpty(option.RequiredFeatureGuid))
						{
							bp.AddShowCondition(delegate(HasFact c)
							{
								c.Unit = new PlayerCharacter();
								c.m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(option.RequiredFeatureGuid);
							});
						}
						if (!string.IsNullOrEmpty(option.RequiredEtudeGuid))
						{
							bp.AddShowCondition(delegate(EtudeStatus c)
							{
								c.m_Etude = BlueprintTools.GetBlueprintReference<BlueprintEtudeReference>(option.RequiredEtudeGuid);
								c.Not = false;
							});
						}
						option.ConfigureAnswer?.Invoke(bp);
					});
					if (!string.IsNullOrEmpty(option.ExcludeVanillaAnswerGuid))
					{
						BlueprintAnswer blueprint = BlueprintTools.GetBlueprint<BlueprintAnswer>(option.ExcludeVanillaAnswerGuid);
						if (blueprint != null)
						{
							BlueprintAnswer blueprintAnswer2 = blueprint;
							if (blueprintAnswer2.ShowConditions == null)
							{
								blueprintAnswer2.ShowConditions = ActionFlow.EmptyCondition();
							}
							blueprint.ShowConditions.Conditions = (blueprint.ShowConditions.Conditions ?? Array.Empty<Condition>()).AppendToArray(new AnswerSelected
							{
								Not = true,
								m_Answer = blueprintAnswer.ToReference<BlueprintAnswerReference>()
							});
						}
					}
					answersList.InsertAnswer(blueprintAnswer);
					num2++;
					if (option.Banter == null || option.Banter.Lines == null || option.Banter.Lines.Count <= 0)
					{
						continue;
					}
					ConstellationReaction capturedBanter = option.Banter;
					string capturedSceneDesc = allScene.Description;
					ConstellationDialogueBanter.RegisterDynamicAnswerHandler(option.Id, delegate(DialogueContext ctx)
					{
						List<string> lines = new List<string>(capturedBanter.Lines);
						int cosmicCoins = capturedBanter.CosmicCoins;
						string sponsor = capturedBanter.Sponsor;
						ConstellationCategory category = capturedBanter.Category;
						string sceneContext = capturedBanter.SceneContext ?? capturedSceneDesc;
						if (capturedBanter.CycleOverrides != null && capturedBanter.CycleOverrides.Count > 0)
						{
							foreach (CycleVariation cycleOverride in capturedBanter.CycleOverrides)
							{
								if (ctx.Cycle >= cycleOverride.MinCycle && ctx.Cycle <= cycleOverride.MaxCycle)
								{
									if (cycleOverride.VariantBanterLines != null && cycleOverride.VariantBanterLines.Count > 0)
									{
										lines = new List<string>(cycleOverride.VariantBanterLines);
									}
									break;
								}
							}
						}
						return new BanterResult(lines, cosmicCoins, sponsor, sceneContext, category);
					});
				}
			}
			Main.IsekaiContext.Logger.Log($"[NarrativeEngine] Successfully compiled {num} scenes with {num2} total options.");
		}

		public static void ResetCompilation()
		{
			_compiled = false;
		}
	}
}
