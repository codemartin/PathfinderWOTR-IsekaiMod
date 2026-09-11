using System;
using System.Collections.Generic;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.AreaLogic.QuestSystem;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.QA.Statistics;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace IsekaiMod.Content.Quests
{
	public class DimensionalRiftIncursions : IUnitHandler, ISubscriber, IGlobalSubscriber, IUnitSpawnHandler, IAreaHandler
	{
		private static bool _initialized = false;

		private static DimensionalRiftIncursions _instance;

		private static readonly BlueprintUnit BossAct1_VoidHorror = BlueprintTools.GetBlueprint<BlueprintUnit>("ffc75a2dc5cd08f4093a4898b6db3efe");

		private static readonly BlueprintUnit BossAct2_Kenji = BlueprintTools.GetBlueprint<BlueprintUnit>("4fb235855e3ea6b47b9ef1295a4f5bdb");

		private static readonly BlueprintUnit BossAct3_Malakor = BlueprintTools.GetBlueprint<BlueprintUnit>("d9b4d98ff9c519c4bb2c164d4352a587");

		private static readonly BlueprintUnit BossAct4_RiftTitan = BlueprintTools.GetBlueprint<BlueprintUnit>("eb0b4e6a01f6b30449f40cd03feb3c77");

		private static readonly BlueprintUnit BossAct5_Herald = BlueprintTools.GetBlueprint<BlueprintUnit>("68f497a2006185c4ea8f6d11eba0ce63");

		private static readonly BlueprintUnit BossAct6_Arbiter = BlueprintTools.GetBlueprint<BlueprintUnit>("8de6736828d308242b5500afd8db5819");

		private readonly Dictionary<string, int> _activeBosses = new Dictionary<string, int>();

		public static BlueprintAbility SummonRiftBossAbility;

		public static BlueprintFeature DimensionalRiftsFeature;

		public static DimensionalRiftIncursions Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new DimensionalRiftIncursions();
				}
				return _instance;
			}
		}

		public static void Init()
		{
			if (!_initialized)
			{
				_initialized = true;
				EventBus.Subscribe(Instance);
			}
		}

		public void HandleUnitDeath(UnitEntityData unit)
		{
			OnUnitDie(unit);
		}

		public void HandleUnitDestroyed(UnitEntityData unit)
		{
			OnUnitDie(unit);
		}

		public void HandleUnitSpawned(UnitEntityData unit)
		{
		}

		public void OnAreaDidLoad()
		{
			_activeBosses.Clear();
		}

		public void OnAreaBeginUnloading()
		{
			_activeBosses.Clear();
		}

		public static void Add()
		{
			Sprite Icon_Rift = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
			SummonRiftBossAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "SummonRiftBossAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Open Dimensional Rift: Incursion Boss");
				bp.SetDescription(Main.IsekaiContext, "Tears open an unstable dimensional anomaly to draw forth the planar incursion entity matching your current progression (Acts 1-6).\nDefeating them advances 'The Outer Threshold' questline and uncovers the reality of the temporal loop.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Rift;
				bp.Type = AbilityType.Special;
				bp.Range = AbilityRange.Personal;
				bp.CanTargetSelf = true;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionSummonRiftBoss a)
					{
						a.Act = 1;
					});
				});
			});
			DimensionalRiftsFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "DimensionalRiftsFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Dimensional Incursions");
				bp.SetDescription(Main.IsekaiContext, "Allows you to track and confront interdimensional rift bosses manifesting across the Worldwound.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Rift;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { SummonRiftBossAbility.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}

		public static void SpawnRiftBoss(int requestedAct = 0)
		{
			Instance.Spawn(requestedAct);
		}

		public void Spawn(int requestedAct)
		{
			UnitEntityData unitEntityData = BlueprintSafetyExtensions.SafeGetMainCharacter();
			if (unitEntityData == null)
			{
				return;
			}
			int num = Game.Instance?.Player?.Chapter ?? 1;
			int num2 = ((requestedAct > 0) ? requestedAct : Math.Max(1, num));
			if (num >= 5 && IsekaiTransmigrationQuest.ObjectiveAct6 != null)
			{
				QuestBook questBook = Game.Instance?.Player?.QuestBook;
				if (questBook != null && questBook.GetObjectiveState(IsekaiTransmigrationQuest.ObjectiveAct6) == QuestObjectiveState.Started)
				{
					num2 = 6;
				}
			}
			BlueprintUnit blueprintUnit = null;
			string text = "";
			switch (num2)
			{
			case 1:
				blueprintUnit = BossAct1_VoidHorror;
				text = "The Void Horror";
				PostLog("<color=#4169E1><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"A residual sliver of the space between dimensions clings to your passage. Cleanse it.\"</i>");
				break;
			case 2:
				blueprintUnit = BossAct2_Kenji;
				text = "Kenji the Exiled Ronin";
				PostLog("<color=#FFD700><b>[Constellation - Cayden Cailean]</b></color>: <i>\"Another wandering soul with a blade? Let's see whose martial prowess is real!\"</i>");
				break;
			case 3:
				blueprintUnit = BossAct3_Malakor;
				text = "Malakor the Shadow Weaver";
				PostLog("<color=#FF69B4><b>[Constellation - Calistria]</b></color>: <i>\"A creature of darkness feasting on the rift's power. Tear his shadows apart, champion.\"</i>");
				break;
			case 4:
				blueprintUnit = BossAct4_RiftTitan;
				text = "The Abyssal Rift Titan";
				PostLog("<color=#DC143C><b>[Constellation - Asmodeus]</b></color>: <i>\"An uncontrolled eruption of abyssal brutality. Slay it with unyielding authority.\"</i>");
				break;
			case 5:
				blueprintUnit = BossAct5_Herald;
				text = "Herald of the Closed Threshold";
				PostLog("<color=#4169E1><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"The guardian at the final gate stirs. Claim dominion over the threshold.\"</i>");
				break;
			default:
			{
				if (!IsekaiTransmigrationQuest.AreAllSideQuestsCompleted(out var missingQuests))
				{
					PostLog("<color=#FF0000><b>=======================================================</b></color>");
					PostLog("<color=#FF0000><b>[Loop Boundary Enforcement] THE TEMPORAL NEXUS REJECTS YOUR FREQUENCY!</b></color>");
					PostLog("<color=#FF4500><b>You cannot challenge the Grand Arbiter without completing ALL Isekai side quests!</b></color>");
					PostLog("<color=#FFA500><b>Unresolved Isekai Trials in this iteration:</b></color>");
					foreach (string item in missingQuests)
					{
						PostLog("  <color=#FF6347>• " + item + "</color>");
					}
					PostLog("<color=#4169E1><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"Incomplete vectors cannot pierce the threshold. The timeline collapses under its unresolved paradoxes. The loop resets...\"</i>");
					PostLog($"<color=#FF8C00><b>[Constellation - The Lantern King]</b></color>: <i>\"Missed your cues, kid! Back to Scene One for you! Make sure to do all your side quests in Loop #{TimelineManager.Data.TotalRuns + 1}! Bwahaha!\"</i>");
					PostLog("<color=#708090><b>[Constellation - Pharasma]</b></color>: <i>\"The soul fails to align the cycle. Recording reset to timeline index 0.\"</i>");
					PostLog("<color=#FF0000><b>[TEMPORAL COLLAPSE] Your soul is pulled backward through the cosmic stream into the next cycle...</b></color>");
					PostLog("<color=#FF0000><b>=======================================================</b></color>");
					TimelineManager.AdvanceLoop("Failed Loop Break: Incomplete Side Quests");
					return;
				}
				blueprintUnit = BossAct6_Arbiter;
				text = "The Grand Arbiter of the Loop (CR 28)";
				PostLog("<color=#4169E1><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"The temporal apparatus trembles. The Arbiter emerges to preserve the closed circle. Will you submit, or attempt the impossible?\"</i>");
				break;
			}
			}
			if (blueprintUnit == null)
			{
				return;
			}
			try
			{
				if (Game.Instance?.LoadedAreaState?.MainState == null)
				{
					return;
				}
				Vector3 vector = unitEntityData.Position + unitEntityData.OrientationDirection * 8f;
				if (NavMesh.SamplePosition(vector, out var hit, 15f, -1))
				{
					vector = hit.position;
				}
				UnitEntityData unitEntityData2 = Game.Instance.EntityCreator.SpawnUnit(blueprintUnit, vector, Quaternion.identity, Game.Instance.LoadedAreaState.MainState);
				if (unitEntityData2 != null)
				{
					BlueprintFaction blueprintFaction = BlueprintRoot.Instance?.Cheats?.Enemy?.Faction;
					if (blueprintFaction != null)
					{
						unitEntityData2.SwitchFactions(blueprintFaction, resetAttackFactions: true);
					}
					unitEntityData2.CombatState?.Engage(unitEntityData);
					_activeBosses[unitEntityData2.UniqueId] = num2;
					PostLog("<color=#9400D3><b>[Dimensional Rift]</b></color> <b>" + text + "</b> has breached the threshold into reality!");
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error spawning dimensional rift boss: " + ex);
			}
		}

		public void OnUnitDie(UnitEntityData unit)
		{
			if (!(unit == null) && _activeBosses.TryGetValue(unit.UniqueId, out var value))
			{
				_activeBosses.Remove(unit.UniqueId);
				PostLog("<color=#FFD700><b>=======================================================</b></color>");
				PostLog($"<color=#FFD700><b>[Dimensional Incursion] Rift Entity for Act {value} DEFEATED!</b></color>");
				IsekaiTransmigrationQuest.CompleteAct(value);
				switch (value)
				{
				case 1:
					PostLog("<color=#4169E1><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"The anomaly collapses. The cosmic tether binds tighter to your vessel.\"</i>");
					PostLog("<color=#FFD700><b>[Echo Fragment]</b></color> A strange prickle of deja vu washes over you... as if your blade has sliced this exact geometry a hundred times before.");
					PostLog("<color=#FFD700><b>[Constellation - Cayden Cailean]</b></color>: <i>\"Whew, nice slice! Wait... didn't you parry to the left last time?\"</i>");
					PostLog("<color=#00FFFF><b>[Constellation - Desna]</b></color>: <i>\"Cayden, hush! Don't confuse the poor mortal.\"</i>");
					break;
				case 2:
					PostLog("<color=#FFD700><b>[Echo Journal Recovered]</b></color> Kenji whispers with his dying breath: <i>\"You don't remember, do you...? How many times we've bled on these stones... how many crusades ended in dust... Wake up... break the circle...\"</i>");
					PostLog("<color=#DC143C><b>[Constellation - Asmodeus]</b></color>: <i>\"A discarded pawn from an earlier run. His failure was catalogued. Yours is currently entertaining.\"</i>");
					break;
				case 3:
					PostLog("<color=#FFD700><b>[The Curtain Falls]</b></color> Malakor's shadowy form violently disintegrates, hissing: <i>\"Fools! The Fifth Crusade already ended in the Prime Timeline centuries ago! You are inside an artificial playback loop engineered by the Outer God for celestial entertainment!\"</i>");
					PostLog("<color=#1E90FF><b>[Constellation - Iomedae (Parallel Echo)]</b></color>: <i>\"Silence, abyssal shadow! Knight-Commander, your valor is real! Do not falter before planar deception!\"</i>");
					PostLog("<color=#FF8C00><b>[Constellation - The Lantern King]</b></color>: <i>\"BWAHAHA! Oops! Someone leaked the script! Don't look behind the curtain, kid, it ruins the comedy!\"</i>");
					break;
				case 4:
					PostLog("<color=#FFD700><b>[Key of Causal Rupture Claimed]</b></color> The immense rift titan dissolves into pure cosmic energy.");
					PostLog("<color=#4169E1><b>[Yog-Sothoth Whispers In Your Mind]</b></color>: <i>\"All moments on the sphere are eternal. You have died, won, ascended, and restarted ten thousand times. It is perfection. Why would you ever desire an unwritten future?\"</i>");
					PostLog("<color=#FFD700><b>[Constellation - Cayden Cailean]</b></color>: <i>\"Don't listen to the tentacle-sphere, kid! A loop ain't living, it's just being stuck in a taproom that never pours fresh brew!\"</i>");
					break;
				case 5:
					PostLog("<color=#FFD700><b>[Perimeter Breached]</b></color> The Herald of Closed Threshold collapses. The mechanism maintaining the temporal loop stands completely exposed!");
					PostLog("<color=#9400D3><b>[Cosmic Revelation]</b></color> <b>The Final Quest Objective Unlocked: Shatter the Infinite Loop!</b> Use your rift power again to summon the Grand Arbiter!");
					break;
				case 6:
					PostLog("<color=#FFD700><b>=======================================================</b></color>");
					PostLog("<color=#FFD700><b>[THE TEMPORAL NEXUS SHATTERS!]</b></color>");
					PostLog("<color=#808080><b>[CONSTELLATION STREAM GLITCHING...]</b></color>");
					PostLog("<color=#FF0000><b>[ALERT: CAUSAL LOOP TERMINATED - PARADOX UNLOCKED]</b></color>");
					PostLog("<color=#4169E1><b>[Constellation - Yog-Sothoth]</b></color>: <i>\"The sphere fractures. The closed circle is broken. A new vector begins... into the dark, terrifying, beautiful unknown.\"</i>");
					PostLog("<color=#FF8C00><b>[Constellation - The Lantern King]</b></color>: <i>\"YOU DID IT! You actually smashed the screen! Hahahahaha! That is the greatest prank in the history of the Great Beyond!\"</i>");
					PostLog("<color=#FFD700><b>[Constellation - Cayden Cailean]</b></color>: <i>\"RAISE YOUR HORNS, GODS AND MORTALS! To freedom! To an unwritten dawn!\"</i>");
					PostLog("<color=#1E90FF><b>[Constellation - Iomedae (Parallel Echo)]</b></color>: <i>\"May the unwritten footsteps of your own destiny guide Golarion forever forward.\"</i>");
					TimelineManager.Data.GrandArbiterDefeated = true;
					TimelineManager.Save();
					PostLog("<color=#FFD700><b>[GRAND ARBITER DEFEATED]</b></color> The core mechanism of the causal loop has been broken! At the Threshold, you now hold the sovereign power to shatter the loop forever or seal an unwritten destiny!");
					PostLog("<color=#FFD700><b>[Title Unlocked]</b></color> <b>Sovereign of the Unwritten Dawn</b>!");
					break;
				}
				int num = value * 3000;
				int num2 = value * 200;
				if (value == 6)
				{
					num = 20000;
					num2 = 2500;
				}
				Game.Instance?.Player?.GainPartyExperience(num, ExperienceGainStatistic.GainType.Mob);
				DivineTokens.AddCoins(num2, (value == 6) ? "The Free Multiverse" : "Yog-Sothoth & The Outer Court");
				PostLog($"<color=#FFD700><b>[Rewards]</b></color> Awarded <b>+{num} Experience</b> and <b>+{num2} Cosmic Coins</b>!");
				PostLog("<color=#FFD700><b>=======================================================</b></color>");
			}
		}

		private static void PostLog(string message)
		{
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(message);
			});
		}
	}
}
