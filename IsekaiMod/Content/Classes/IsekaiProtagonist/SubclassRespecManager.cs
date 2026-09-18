using System;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Crusade;
using IsekaiMod.Content.Quests;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Controllers.Rest;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist
{
	public class SubclassRespecManager : IUnitLevelUpHandler, ISubscriber, IGlobalSubscriber, IRestFinishedHandler, IAreaHandler
	{
		private static SubclassRespecManager Instance;

		private string LastActiveArchetype;

		private static BlueprintActivatableAbility _overlordActivatable;

		private static BlueprintBuff _overlordBuff;

		private static BlueprintActivatableAbility _slimeActivatable;

		private static BlueprintBuff _slimeBuff;

		private static BlueprintFeature _martialProf;

		private static BlueprintFeature _martialTranscendence;

		private static BlueprintFeature _godEmperorProf;

		private static BlueprintFeature _overlordProf;

		private static BlueprintFeature _devourerProf;

		private static BlueprintFeature _shadowMonarchProf;

		private static BlueprintFeature _heroProf;

		private static BlueprintFeature _mastermindProf;

		private static BlueprintFeature _isekaiProf;

		public static void Init()
		{
			if (Instance == null)
			{
				Instance = new SubclassRespecManager();
				EventBus.Subscribe(Instance);
			}
		}

		public void OnAreaDidLoad()
		{
			try
			{
				UnitEntityData unitEntityData = BlueprintSafetyExtensions.SafeGetMainCharacter();
				if (unitEntityData != null)
				{
					EnsureMortalGuiseOnGameStart(unitEntityData);
					EvaluateResonance(unitEntityData);
					SylvanFeyAllianceQuest.CheckAndActivateSylvanQuest();
					SubclassPersonalQuests.CheckAndActivateArchetypeQuest();
					IsekaiKingdomProjects.CheckAndAddAvailableProjects();
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in SubclassRespecManager.OnAreaDidLoad: " + ex);
			}
		}

		private static void EnsureMortalGuiseOnGameStart(UnitEntityData mainChar)
		{
			try
			{
				if (mainChar?.Descriptor?.Buffs == null)
				{
					return;
				}
				UnitDescriptor descriptor = mainChar.Descriptor;
				if (_overlordBuff == null)
				{
					_overlordBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "SkeletalOverlordFormBuff");
				}
				if (_overlordActivatable == null)
				{
					_overlordActivatable = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(Main.IsekaiContext, "SkeletalOverlordFormAbility");
				}
				if (_overlordBuff != null && descriptor.Buffs.HasFact(_overlordBuff))
				{
					ActivatableAbility activatableAbility = ((_overlordActivatable != null) ? descriptor.ActivatableAbilities.GetFact(_overlordActivatable) : null);
					if (activatableAbility == null || !activatableAbility.IsOn)
					{
						descriptor.Buffs.RemoveFact(_overlordBuff);
					}
				}
				if (_slimeBuff == null)
				{
					_slimeBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "SlimeFormBuff");
				}
				if (_slimeActivatable == null)
				{
					_slimeActivatable = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(Main.IsekaiContext, "SlimeFormAbility");
				}
				if (_slimeBuff != null && descriptor.Buffs.HasFact(_slimeBuff))
				{
					ActivatableAbility activatableAbility2 = ((_slimeActivatable != null) ? descriptor.ActivatableAbilities.GetFact(_slimeActivatable) : null);
					if (activatableAbility2 == null || !activatableAbility2.IsOn)
					{
						descriptor.Buffs.RemoveFact(_slimeBuff);
					}
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error ensuring mortal guise on area load: " + ex);
			}
		}

		public void OnAreaBeginUnloading()
		{
		}

		public void HandleUnitBeforeLevelUp(UnitEntityData unit)
		{
		}

		public void HandleUnitAfterLevelUp(UnitEntityData unit, LevelUpController controller)
		{
			if (!(unit == null) && unit.IsMainCharacter)
			{
				EvaluateResonance(unit);
			}
		}

		public void HandleRestFinished(RestStatus status)
		{
			try
			{
				if (status != null && status.RestSucceeded)
				{
					UnitEntityData unitEntityData = BlueprintSafetyExtensions.SafeGetMainCharacter();
					if (unitEntityData != null)
					{
						EvaluateResonance(unitEntityData);
					}
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in SubclassRespecManager.HandleRestFinished: " + ex);
			}
		}

		private void EvaluateResonance(UnitEntityData mainChar)
		{
			try
			{
				string currentArchetype = DetectActiveArchetype(mainChar);
				UnitDescriptor unitDescriptor = mainChar?.Descriptor;
				if (_martialProf == null)
				{
					_martialProf = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialGodProficiencies");
				}
				if (_martialTranscendence == null)
				{
					_martialTranscendence = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialTranscendenceFeature");
				}
				if (unitDescriptor != null && _martialTranscendence != null && _martialProf != null && unitDescriptor.HasFact(_martialTranscendence) && !unitDescriptor.HasFact(_martialProf))
				{
					unitDescriptor.AddFact(_martialProf);
					Main.IsekaiContext.Logger.Log("Synchronized MartialGodProficiencies from MartialTranscendenceFeature.");
				}
				if (currentArchetype != null && currentArchetype != LastActiveArchetype)
				{
					LastActiveArchetype = currentArchetype;
					TimelineManager.SetActiveRunArchetype(currentArchetype);
					EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
					{
						h.HandleLogMessage("<color=#9400D3>[Subclass Resonance]</color> Attuned to <b>" + currentArchetype + "</b>. Awakening subclass features and personal quest.");
					});
					SubclassPersonalQuests.CheckAndActivateArchetypeQuest();
				}
				MythicQuestSynergies.CheckAndApplyResonance(mainChar);
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error evaluating subclass resonance: " + ex);
			}
		}

		private string DetectActiveArchetype(UnitEntityData mainChar)
		{
			if (mainChar?.Descriptor == null)
			{
				return null;
			}
			UnitDescriptor descriptor = mainChar.Descriptor;
			if (_martialProf == null)
			{
				_martialProf = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialGodProficiencies");
			}
			if (_martialTranscendence == null)
			{
				_martialTranscendence = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialTranscendenceFeature");
			}
			if ((_martialProf != null && descriptor.HasFact(_martialProf)) || (_martialTranscendence != null && descriptor.HasFact(_martialTranscendence)))
			{
				return "Martial God";
			}
			if (_godEmperorProf == null)
			{
				_godEmperorProf = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GodEmperorProficiencies");
			}
			if (_godEmperorProf != null && descriptor.HasFact(_godEmperorProf))
			{
				return "God Emperor";
			}
			if (_overlordProf == null)
			{
				_overlordProf = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "OverlordProficiencies");
			}
			if (_overlordProf != null && descriptor.HasFact(_overlordProf))
			{
				return "Overlord";
			}
			if (_devourerProf == null)
			{
				_devourerProf = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DevourerProficiencies");
			}
			if (_devourerProf != null && descriptor.HasFact(_devourerProf))
			{
				return "Slime";
			}
			if (_shadowMonarchProf == null)
			{
				_shadowMonarchProf = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowMonarchProficiencies");
			}
			if (_shadowMonarchProf != null && descriptor.HasFact(_shadowMonarchProf))
			{
				return "Shadow Monarch";
			}
			if (_heroProf == null)
			{
				_heroProf = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeroProficiencies");
			}
			if (_heroProf != null && descriptor.HasFact(_heroProf))
			{
				return "Hero";
			}
			if (_mastermindProf == null)
			{
				_mastermindProf = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindProficiencies");
			}
			if (_mastermindProf != null && descriptor.HasFact(_mastermindProf))
			{
				return "Mastermind";
			}
			if (_isekaiProf == null)
			{
				_isekaiProf = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProficiencies");
			}
			if (_isekaiProf != null && descriptor.HasFact(_isekaiProf))
			{
				return "Isekai Protagonist (Base)";
			}
			return null;
		}
	}
}
