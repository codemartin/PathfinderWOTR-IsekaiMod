using System;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using UnityEngine;

namespace IsekaiMod.Components;

[TypeId("7a1c9b2d4e6f4c8ab5d3e1f209a7c64b")]
[ComponentName("Copy the master's appearance onto a pet")]
[AllowedOn(typeof(BlueprintUnitFact), false)]
public class CopyMasterAppearanceToPet : UnitFactComponentDelegate, ISubscriber, IGlobalSubscriber, IAreaHandler
{
	[SerializeField]
	public BlueprintUnitReference m_Pet;

	public BlueprintUnit Pet => m_Pet?.Get();

	public override void OnActivate()
	{
		TryCopyAppearance();
	}

	public void OnAreaBeginUnloading()
	{
	}

	public void OnAreaDidLoad()
	{
		TryCopyAppearance();
	}

	public void TryCopyAppearance()
	{
		try
		{
			UnitEntityData owner = base.Owner;
			if (owner == null || owner.IsPet)
			{
				return;
			}
			BlueprintUnit pet = Pet;
			if (pet == null)
			{
				return;
			}
			foreach (EntityPartRef<UnitEntityData, UnitPartPet> pet2 in owner.Pets)
			{
				UnitEntityData entity = pet2.Entity;
				if (!(entity == null) && (entity.Blueprint == pet || entity.OriginalBlueprint == pet))
				{
					SyncTwin(owner, entity);
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error in CopyMasterAppearanceToPet: " + ex);
		}
	}

	internal static void SyncTwin(UnitEntityData master, UnitEntityData pet)
	{
		if (master == null || pet == null)
		{
			return;
		}
		UnitPartDollData unitPartDollData = master.Get<UnitPartDollData>();
		bool flag = unitPartDollData?.Default?.RacePreset != null;
		BlueprintRace blueprintRace = master.Descriptor?.Progression?.Race;
		UnitProgressionData unitProgressionData = pet.Descriptor?.Progression;
		if (unitProgressionData != null && TwinAppearance.ShouldCopyRace(flag, unitProgressionData.Race?.AssetGuid.ToString(), blueprintRace?.AssetGuid.ToString()))
		{
			unitProgressionData.m_Race = blueprintRace;
		}
		TwinRacialCompatibility.Sync(blueprintRace, pet);
		if (flag && pet.Get<UnitPartDollData>()?.Default?.RacePreset == null)
		{
			unitPartDollData.CopyTo(pet);
			pet.Descriptor.CustomGender = master.Gender;
			if (pet.View != null)
			{
				pet.RecreateView();
			}
		}
		UnitUISettings uISettings = master.UISettings;
		UnitUISettings uISettings2 = pet.UISettings;
		if (uISettings != null && uISettings2 != null && !TwinAppearance.SamePortrait(uISettings2.PortraitBlueprintRaw?.AssetGuid.ToString(), uISettings2.CustomPortraitRaw?.CustomId, uISettings.PortraitBlueprintRaw?.AssetGuid.ToString(), uISettings.CustomPortraitRaw?.CustomId))
		{
			uISettings2.SetAllPortraitData(uISettings);
		}
		EyePortraitInjector.RegisterCropFallback(uISettings2?.CustomPortraitRaw);
		EyePortraitInjector.RegisterCropFallback(uISettings2?.PortraitBlueprintRaw?.Data);
		EyePortraitInjector.RegisterCropFallback(uISettings2?.PortraitBlueprintRaw?.BackupPortrait?.Data);
		try
		{
			EyePortraitInjector.RegisterCropFallback(pet.Portrait);
		}
		catch (Exception)
		{
		}
	}
}
