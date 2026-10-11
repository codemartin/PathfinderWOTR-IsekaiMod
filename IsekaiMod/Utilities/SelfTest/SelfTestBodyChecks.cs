using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Components;
using IsekaiMod.Content.Features.IsekaiProtagonist;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Controllers.Units;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestBodyChecks
{
	private const int RetributionStacks = 3;

	private const string SmallRaceGuid = "b0c3ef2729c498f47970bb50fa1acd30";

	private const string MediumRaceGuid = "0a5d473ead98b0646b94495af250fdc4";

	private const string TwinProbePortraitGuid = "60c35a74347646cba970e6c9c0d76321";

	internal const string ForeignShapeGuid = "8b75d829538aa464594271cfa81fe0b2";

	internal static SelfTestChecks.Outcome ThreatHitPoints()
	{
		BlueprintBuff modBlueprint = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "RampantCheatingCurseBuff");
		BlueprintBuff modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "CosmicRetributionBuff");
		if (modBlueprint == null || modBlueprint2 == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "RampantCheatingCurseBuff or CosmicRetributionBuff was not created.");
		}
		string problem;
		using SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem);
		if (selfTestUnit == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "No blank unit: " + problem);
		}
		UnitEntityData unit = selfTestUnit.Unit;
		List<string> list = new List<string>();
		if (unit.Stats.HitPoints.BaseValue < 50)
		{
			unit.Stats.HitPoints.BaseValue = 50;
		}
		int maxHP = unit.MaxHP;
		int temporaryHP = unit.Descriptor.TemporaryHP;
		List<Buff> list2 = new List<Buff> { unit.Descriptor.Buffs.AddBuff(modBlueprint, unit, null) };
		for (int i = 0; i < 3; i++)
		{
			list2.Add(unit.Descriptor.Buffs.AddBuff(modBlueprint2, unit, null));
		}
		if (list2.Any((Buff b) => b == null || !b.Active))
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The threat buffs did not all become active on a blank unit.");
		}
		int maxHP2 = unit.MaxHP;
		int num = unit.Descriptor.TemporaryHP - temporaryHP;
		int num2 = 400;
		list.Add($"Maximum hit points {maxHP} -> {maxHP2} with the buffs; temporary hit points +{num} (expected +{num2}).");
		bool num3 = maxHP2 == maxHP && num == num2;
		int num4 = unit.Descriptor.TemporaryHP + Math.Max(0, maxHP - 1);
		unit.Damage += unit.Stats.TemporaryHitPoints.HandleDamage(num4);
		int hPLeft = unit.HPLeft;
		CosmicThreatScaling.Instance.HandleUnitLeaveCombat(unit);
		bool flag = !unit.Descriptor.HasFact(modBlueprint) && !unit.Descriptor.HasFact(modBlueprint2);
		int maxHP3 = unit.MaxHP;
		int hPLeft2 = unit.HPLeft;
		UnitLifeController.ForceTickOnUnit(unit);
		UnitLifeState lifeState = unit.Descriptor.State.LifeState;
		list.Add($"Dealt {num4}: {hPLeft} hit point(s) left in the fight; after leaving combat the buffs are " + (flag ? "gone" : "STILL ON") + $", maximum {maxHP3}, {hPLeft2} left, life state {lifeState}.");
		bool num5 = num3 & (flag && maxHP3 == maxHP && hPLeft2 == hPLeft && hPLeft2 > 0 && lifeState == UnitLifeState.Conscious);
		return SelfTestChecks.Outcome.Of(num5, num5 ? "Rampant Cheating and Cosmic Retribution add temporary hit points only, and a hurt creature that leaves combat keeps the hit points it had." : "Taking the threat buffs off a hurt creature changes its hit points or kills it.", list);
	}

	internal static SelfTestChecks.Outcome PetBodies()
	{
		Dictionary<BlueprintFeature, AddPet> dictionary = SelfTestPetChecks.ModPetFeatures();
		if (dictionary.Count == 0)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The mod registered no AddPet feature.");
		}
		List<string> list = new List<string>();
		int num = 0;
		int num2 = 0;
		foreach (KeyValuePair<BlueprintFeature, AddPet> item in dictionary)
		{
			BlueprintUnit pet = item.Value.Pet;
			if (pet == null)
			{
				continue;
			}
			string name = pet.name;
			if (pet.Body != null && pet.Body.DisableHands && pet.Body.EmptyHandWeapon != null)
			{
				num++;
				list.Add(name + ": its blueprint disables hands but names fists, which the engine never creates for a body without hands.");
			}
			UnitEntityData unitEntityData = null;
			try
			{
				using (ContextData<UnitEntityData.ChargenUnit>.Request())
				{
					unitEntityData = Game.Instance.CreateUnitVacuum(pet);
				}
			}
			catch (Exception ex)
			{
				num++;
				list.Add(name + ": could not be created (" + ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message) + ").");
				continue;
			}
			if (unitEntityData == null)
			{
				num++;
				list.Add(name + ": could not be created.");
				continue;
			}
			try
			{
				num2++;
				UnitBody body = unitEntityData.Body;
				bool flag = pet.Body != null && !pet.Body.DisableHands;
				if (flag)
				{
					if (!body.HandsAreEnabled)
					{
						num++;
						list.Add(name + ": its blueprint has hands but the body was built without them.");
					}
					else if (body.EmptyHandWeapon == null)
					{
						num++;
						list.Add(name + ": has hands and no fists, so with its hand weapon off it has no attack at all.");
					}
				}
				for (int i = 0; i < body.HandsEquipmentSets.Count; i++)
				{
					HandSlot[] array = new HandSlot[2]
					{
						body.HandsEquipmentSets[i].PrimaryHand,
						body.HandsEquipmentSets[i].SecondaryHand
					};
					foreach (HandSlot handSlot in array)
					{
						if (handSlot?.MaybeItem is ItemEntityWeapon { IsPermanentEmptyHand: false, Blueprint: not null } itemEntityWeapon && itemEntityWeapon.Blueprint.IsNatural && handSlot.CanRemoveItem())
						{
							num++;
							list.Add($"{name}: its natural weapon {itemEntityWeapon.Blueprint.name} sits in an unlocked hand slot (weapon set {i + 1})" + (flag ? "" : ", where a body without hands keeps its first natural limb,") + " and can be dragged out.");
						}
					}
				}
				Sprite sprite = null;
				try
				{
					sprite = pet.PortraitSafe?.Data?.PetEyePortrait;
				}
				catch (Exception ex2)
				{
					list.Add(name + ": reading its pet eye-strip threw " + ex2.GetType().Name + ".");
				}
				if (sprite == null)
				{
					num++;
					list.Add(name + ": its portrait gives the party bar no pet eye-strip.");
				}
			}
			catch (Exception ex3)
			{
				num++;
				list.Add(name + ": the check threw " + ex3.GetType().Name + ": " + SelfTestReport.Clean(ex3.Message));
			}
			finally
			{
				try
				{
					unitEntityData.Dispose();
				}
				catch
				{
				}
			}
		}
		num += TwinPortraitProblems(list);
		num += TwinSizeProblems(list);
		return SelfTestChecks.Outcome.Of(num == 0, $"{num2} mod pets built; {num} problems with their hands, fists, natural weapons, pet eye-strips, the Twin Avatar's portrait or its size.", list);
	}

	private static int TwinPortraitProblems(List<string> details)
	{
		BlueprintUnit modBlueprint = BlueprintTools.GetModBlueprint<BlueprintUnit>(Main.IsekaiContext, "TwinAvatarCompanionUnit");
		BlueprintPortrait blueprint = BlueprintTools.GetBlueprint<BlueprintPortrait>("60c35a74347646cba970e6c9c0d76321");
		if (modBlueprint == null || blueprint == null)
		{
			details.Add("TwinAvatarCompanionUnit or the probe portrait is missing, so the Twin Avatar's portrait could not be checked.");
			return 1;
		}
		string problem;
		using SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem);
		if (selfTestUnit == null)
		{
			details.Add("No blank master for the Twin Avatar: " + problem);
			return 1;
		}
		UnitEntityData unitEntityData = null;
		try
		{
			using (ContextData<UnitEntityData.ChargenUnit>.Request())
			{
				unitEntityData = Game.Instance.CreateUnitVacuum(modBlueprint);
			}
			selfTestUnit.Unit.UISettings.SetPortrait(blueprint);
			CopyMasterAppearanceToPet.SyncTwin(selfTestUnit.Unit, unitEntityData);
			BlueprintPortrait portraitBlueprint = unitEntityData.UISettings.PortraitBlueprint;
			if (portraitBlueprint != blueprint)
			{
				details.Add("The Twin Avatar shows portrait '" + (portraitBlueprint?.name ?? "none") + "' after the sync, not its master's '" + blueprint.name + "'.");
				return 1;
			}
			if (blueprint.Data?.PetEyePortrait == null)
			{
				details.Add("The Twin Avatar wearing its master's portrait has no pet eye-strip.");
				return 1;
			}
			details.Add("The Twin Avatar shows its master's portrait '" + blueprint.name + "' after the sync.");
			return 0;
		}
		catch (Exception ex)
		{
			details.Add("Syncing the Twin Avatar threw " + ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message));
			return 1;
		}
		finally
		{
			try
			{
				unitEntityData?.Dispose();
			}
			catch
			{
			}
		}
	}

	private static int TwinSizeProblems(List<string> details)
	{
		BlueprintUnit modBlueprint = BlueprintTools.GetModBlueprint<BlueprintUnit>(Main.IsekaiContext, "TwinAvatarCompanionUnit");
		BlueprintFeature modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "TwinEvolutionLargeEffect");
		if (modBlueprint == null || modBlueprint2 == null)
		{
			details.Add("TwinAvatarCompanionUnit or TwinEvolutionLargeEffect is missing, so the Twin Avatar's size could not be checked.");
			return 1;
		}
		Size size = modBlueprint.Size;
		int num = 0;
		string[] array = new string[2] { "b0c3ef2729c498f47970bb50fa1acd30", "0a5d473ead98b0646b94495af250fdc4" };
		foreach (string text in array)
		{
			BlueprintRace blueprint = BlueprintTools.GetBlueprint<BlueprintRace>(text);
			if (blueprint == null)
			{
				num++;
				details.Add("The probe race " + text + " is not in the game.");
				continue;
			}
			UnitEntityData unitEntityData = null;
			try
			{
				using (ContextData<UnitEntityData.ChargenUnit>.Request())
				{
					unitEntityData = Game.Instance.CreateUnitVacuum(modBlueprint);
				}
				unitEntityData.Descriptor.Progression.m_Race = blueprint;
				EntityFact fact = unitEntityData.Descriptor.AddFact(modBlueprint2);
				if (unitEntityData.State.Size != Size.Large)
				{
					num++;
					details.Add($"A {blueprint.name} master's Twin Avatar with Towering Frame is {unitEntityData.State.Size}, not Large.");
					continue;
				}
				unitEntityData.Descriptor.RemoveFact(fact);
				if (unitEntityData.State.Size != size)
				{
					num++;
					details.Add($"A {blueprint.name} master's Twin Avatar without evolutions is {unitEntityData.State.Size}, not the {size} its blueprint promises.");
				}
				else
				{
					details.Add($"A {blueprint.name} master's Twin Avatar (race size {blueprint.Size}) is Large with Towering Frame and {size} without it.");
				}
			}
			catch (Exception ex)
			{
				num++;
				details.Add("Checking the Twin Avatar's size for " + blueprint.name + " threw " + ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message));
			}
			finally
			{
				try
				{
					unitEntityData?.Dispose();
				}
				catch
				{
				}
			}
		}
		return num;
	}

	internal static SelfTestChecks.Outcome PolymorphClash()
	{
		BlueprintBuff blueprint = BlueprintTools.GetBlueprint<BlueprintBuff>("8b75d829538aa464594271cfa81fe0b2");
		if (blueprint == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "The foreign shape 8b75d829538aa464594271cfa81fe0b2 is not in the game.");
		}
		List<string> list = new List<string>();
		int num = 0;
		string[] array = new string[2] { "SlimeFormBuff", "SkeletalOverlordFormBuff" };
		foreach (string text in array)
		{
			BlueprintBuff modBlueprint = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, text);
			if (modBlueprint == null)
			{
				num++;
				list.Add(text + " was not created.");
				continue;
			}
			string problem;
			using SelfTestUnit selfTestUnit = SelfTestUnit.CreateBlank(out problem);
			if (selfTestUnit == null)
			{
				return SelfTestChecks.Outcome.Of(passed: false, "No blank unit: " + problem);
			}
			UnitEntityData unit = selfTestUnit.Unit;
			BuffCollection buffs = unit.Descriptor.Buffs;
			try
			{
				buffs.AddBuff(blueprint, unit, null);
				string text2 = BodySignature(unit);
				if (!unit.Body.IsPolymorphed)
				{
					num++;
					list.Add(text + ": the foreign shape did not take the body at all, so this direction proves nothing.");
				}
				else
				{
					buffs.AddBuff(modBlueprint, unit, null);
					buffs.RemoveFact(modBlueprint);
					string text3 = BodySignature(unit);
					if (text3 != text2)
					{
						num++;
						list.Add(text + " toggled on and off under a foreign shape changed that shape's body: " + text2 + " -> " + text3 + ".");
					}
				}
				buffs.RemoveFact(blueprint);
				if (unit.Body.IsPolymorphed)
				{
					num++;
					list.Add(text + ": the body is still polymorphed after the foreign shape ended.");
				}
				buffs.AddBuff(modBlueprint, unit, null);
				string text4 = BodySignature(unit);
				if (!unit.Body.IsPolymorphed)
				{
					num++;
					list.Add(text + " did not take the body on a blank unit.");
				}
				else
				{
					buffs.AddBuff(blueprint, unit, null);
					buffs.RemoveFact(blueprint);
					string text5 = BodySignature(unit);
					if (text5 != text4)
					{
						num++;
						list.Add("A foreign shape refused under " + text + " changed the form's body when it ended: " + text4 + " -> " + text5 + ".");
					}
				}
				buffs.RemoveFact(modBlueprint);
				if (unit.Body.IsPolymorphed)
				{
					num++;
					list.Add(text + ": the body is still polymorphed after the form ended.");
				}
				list.Add(text + ": foreign body " + text2 + "; form body " + text4 + ".");
			}
			catch (Exception ex)
			{
				num++;
				list.Add(text + ": the check threw " + ex.GetType().Name + ": " + SelfTestReport.Clean(ex.Message));
			}
		}
		return SelfTestChecks.Outcome.Of(num == 0, (num == 0) ? "Toggling the Slime or Skeletal Overlord form under a foreign shape, or a foreign shape under a form, never undoes the other body." : "A form and a foreign shape undo each other's body.", list);
	}

	private static string BodySignature(UnitEntityData unit)
	{
		UnitBody body = unit.Body;
		return string.Concat(str2: string.Join(", ", body.AdditionalLimbs?.Where((WeaponSlot s) => s?.HasItem ?? false).Select((WeaponSlot s) => s.Item.Blueprint.name).ToList() ?? new List<string>()), str0: body.IsPolymorphed ? "polymorphed" : "own shape", str1: " [", str3: "]");
	}
}
