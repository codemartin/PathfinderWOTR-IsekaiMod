using System;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Content.Crusade;
using IsekaiMod.Content.Quests;
using Kingmaker;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Controllers.Rest;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Classes.IsekaiProtagonist;

public class SubclassRespecManager : IUnitLevelUpHandler, ISubscriber, IGlobalSubscriber, IRestFinishedHandler, IAreaHandler, IUnitChangedAfterRespecHandler
{
	private static SubclassRespecManager Instance;

	private string LastActiveArchetype;

	public static void Init()
	{
		if (Instance == null)
		{
			Instance = new SubclassRespecManager();
			EventBus.Subscribe(Instance);
		}
	}

	public static void ResetPerPlaythroughState()
	{
		if (Instance != null)
		{
			Instance.LastActiveArchetype = null;
		}
	}

	public void OnAreaDidLoad()
	{
		UnitEntityData unitEntityData = Game.Instance?.Player?.MainCharacter.Value;
		if (unitEntityData != null)
		{
			EnsureMortalGuiseOnGameStart(unitEntityData);
			EvaluateResonance(unitEntityData);
			SylvanFeyAllianceQuest.CheckAndActivateSylvanQuest();
			IsekaiKingdomProjects.CheckAndAddAvailableProjects();
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
			BlueprintActivatableAbility modBlueprint = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(Main.IsekaiContext, "SkeletalOverlordFormAbility");
			BlueprintBuff modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "SkeletalOverlordFormBuff");
			if (modBlueprint2 != null && descriptor.Buffs.HasFact(modBlueprint2))
			{
				ActivatableAbility activatableAbility = ((modBlueprint != null) ? descriptor.ActivatableAbilities.GetFact(modBlueprint) : null);
				if (activatableAbility == null || !activatableAbility.IsOn)
				{
					descriptor.Buffs.RemoveFact(modBlueprint2);
				}
			}
			BlueprintActivatableAbility modBlueprint3 = BlueprintTools.GetModBlueprint<BlueprintActivatableAbility>(Main.IsekaiContext, "SlimeFormAbility");
			BlueprintBuff modBlueprint4 = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "SlimeFormBuff");
			if (modBlueprint4 != null && descriptor.Buffs.HasFact(modBlueprint4))
			{
				ActivatableAbility activatableAbility2 = ((modBlueprint3 != null) ? descriptor.ActivatableAbilities.GetFact(modBlueprint3) : null);
				if (activatableAbility2 == null || !activatableAbility2.IsOn)
				{
					descriptor.Buffs.RemoveFact(modBlueprint4);
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
		if (IsCommittedMainCharacter(unit))
		{
			EvaluateResonance(unit);
		}
	}

	internal static bool IsCommittedMainCharacter(UnitEntityData unit)
	{
		if (unit == null)
		{
			return false;
		}
		if (unit.PreviewOf.Entity != null)
		{
			return false;
		}
		return unit.IsMainCharacter;
	}

	public void HandleUnitChangedAfterRespec(UnitEntityData unit)
	{
		UnitEntityData unitEntityData = Game.Instance?.Player?.MainCharacter.Value;
		if (IsCommittedMainCharacter(unitEntityData))
		{
			EvaluateResonance(unitEntityData);
		}
	}

	public void HandleRestFinished(RestStatus status)
	{
		if (status != null && status.RestSucceeded)
		{
			UnitEntityData unitEntityData = Game.Instance?.Player?.MainCharacter.Value;
			if (unitEntityData != null)
			{
				EvaluateResonance(unitEntityData);
			}
		}
	}

	private void EvaluateResonance(UnitEntityData mainChar)
	{
		try
		{
			string currentArchetype = DetectActiveArchetype(mainChar);
			UnitDescriptor unitDescriptor = mainChar?.Descriptor;
			BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "EdgeLordProficiencies");
			BlueprintFeature modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PhantomBladeProficiencies");
			if (unitDescriptor != null && modBlueprint != null && modBlueprint2 != null && unitDescriptor.HasFact(modBlueprint) && !unitDescriptor.HasFact(modBlueprint2))
			{
				unitDescriptor.AddFact(modBlueprint2);
				Main.IsekaiContext.Logger.Log("Synchronized PhantomBladeProficiencies from legacy EdgeLordProficiencies.");
			}
			if (currentArchetype != null && currentArchetype != LastActiveArchetype)
			{
				LastActiveArchetype = currentArchetype;
				TimelineManager.SetActiveRunArchetype(currentArchetype);
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage(ConstellationText.LocalizedFormat("Constellation.SubclassResonance.Attuned", "<color=#5B2A86>[Subclass Resonance]</color> Attuned to <b>{0}</b>. Awakening subclass features and personal quest.", ArchetypeOrigin.DisplayName(currentArchetype)));
				});
			}
			PersonalQuestDirector.Reconcile();
			MythicQuestSynergies.CheckAndApplyResonance(mainChar, currentArchetype);
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error evaluating subclass resonance: " + ex);
		}
	}

	private string DetectActiveArchetype(UnitEntityData mainChar)
	{
		return DetectArchetypeOf(mainChar);
	}

	internal static string DetectArchetypeOf(UnitEntityData mainChar)
	{
		if (mainChar?.Descriptor == null)
		{
			return null;
		}
		UnitDescriptor descriptor = mainChar.Descriptor;
		if (descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PhantomBladeProficiencies")) || descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "EdgeLordProficiencies")) || descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MartialTranscendenceFeature")))
		{
			return "MartialGod";
		}
		if (descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GodEmperorProficiencies")))
		{
			return "GodEmperor";
		}
		if (descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "OverlordProficiencies")))
		{
			return "Overlord";
		}
		if (descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "DevourerProficiencies")))
		{
			return "Devourer";
		}
		if (descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ShadowMonarchProficiencies")))
		{
			return "ShadowMonarch";
		}
		if (descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeroProficiencies")))
		{
			return "Hero";
		}
		if (descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "MastermindProficiencies")))
		{
			return "Mastermind";
		}
		if (descriptor.HasFact(BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiProficiencies")))
		{
			return "Base";
		}
		return null;
	}
}
