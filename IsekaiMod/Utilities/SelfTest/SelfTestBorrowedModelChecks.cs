using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Content.Guardians;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.View;
using Kingmaker.View.Animation;
using Kingmaker.Visual.Animation.Kingmaker;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using UnityEngine;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestBorrowedModelChecks
{
	private static readonly Gender[] Genders = new Gender[2]
	{
		Gender.Male,
		Gender.Female
	};

	private static readonly string[] NotTheBody = new string[10] { "weapon", "staff", "scythe", "sword", "spear", "bow", "shield", "blade", "axe", "wing" };

	internal static int FormBodies(List<string> details, out string summary)
	{
		int num = 0;
		List<string> list = new List<string>();
		List<BlueprintGuid> list2 = GuardianVisualScale.RegisteredPolymorphBuffs.Union(BorrowedModelFixes.RegisteredPolymorphBuffs).ToList();
		if (list2.Count == 0)
		{
			num++;
			details.Add("Bodies: no form body is registered for its size correction or its repairs; SkeletalOverlordFormBodyBuff should be.");
		}
		foreach (BlueprintGuid item in list2)
		{
			BlueprintBuff blueprintBuff;
			try
			{
				blueprintBuff = ResourcesLibrary.TryGetBlueprint(item) as BlueprintBuff;
			}
			catch
			{
				blueprintBuff = null;
			}
			Polymorph polymorph = blueprintBuff?.GetComponent<Polymorph>();
			if (polymorph == null)
			{
				num++;
				details.Add($"Bodies: registered form body {item} is not a buff with a Polymorph.");
				continue;
			}
			Gender[] genders = Genders;
			foreach (Gender gender in genders)
			{
				UnitViewLink unitViewLink = GuardianVisualScale.PrefabFor(polymorph, gender);
				string label = blueprintBuff.name + ", " + ((gender == Gender.Male) ? "male" : "female") + " wearer (" + (unitViewLink?.AssetId ?? "no model") + ")";
				num += ProbeBody(label, unitViewLink, polymorph.Size, GuardianVisualScale.PolymorphFactor(polymorph, gender), polymorphView: true, null, details, list);
			}
		}
		summary = ((list.Count == 0) ? "no body probed" : string.Join("; ", list.ToArray()));
		return num;
	}

	internal static int UnitBodies(List<string> details, out string summary)
	{
		int num = 0;
		List<string> list = new List<string>();
		foreach (BlueprintGuid item in BorrowedModelFixes.RegisteredUnits.ToList())
		{
			BlueprintUnit blueprintUnit;
			try
			{
				blueprintUnit = ResourcesLibrary.TryGetBlueprint(item) as BlueprintUnit;
			}
			catch
			{
				blueprintUnit = null;
			}
			if (blueprintUnit == null)
			{
				num++;
				details.Add($"Bodies: registered unit {item} does not resolve to a unit.");
			}
			else
			{
				float factor = GuardianVisualScale.RegisteredFactor(blueprintUnit.AssetGuid);
				num += ProbeBody(blueprintUnit.name + " (" + (blueprintUnit.Prefab?.AssetId ?? "no model") + ")", blueprintUnit.Prefab, blueprintUnit.Size, factor, polymorphView: false, blueprintUnit, details, list);
			}
		}
		summary = $"{list.Count} guardian bodies probed";
		return num;
	}

	private static int ProbeBody(string label, UnitViewLink link, Size size, float factor, bool polymorphView, BlueprintUnit unit, List<string> details, List<string> probed)
	{
		UnitEntityView unitEntityView = null;
		try
		{
			unitEntityView = link?.Load();
		}
		catch (Exception ex)
		{
			details.Add("Bodies: " + label + ": loading the model threw " + ex.GetType().Name + ": " + ex.Message);
		}
		if (unitEntityView == null)
		{
			details.Add("Bodies: " + label + ": the model does not load.");
			return 1;
		}
		int num = 0;
		List<string> list = new List<string>();
		UnitEntityView unitEntityView2 = null;
		try
		{
			unitEntityView2 = UnityEngine.Object.Instantiate(unitEntityView);
			unitEntityView2.transform.position = new Vector3(0f, -10000f, 0f);
			if (!unitEntityView2.gameObject.activeSelf)
			{
				unitEntityView2.gameObject.SetActive(value: true);
			}
			int num2 = BorrowedModelFixes.CountDanglingBones(unitEntityView2.gameObject, activeOnly: false);
			BorrowedModelFixes.BoneRepair boneRepair = BorrowedModelFixes.RepairDanglingBones(unitEntityView2.gameObject);
			int num3 = BorrowedModelFixes.CountDanglingBones(unitEntityView2.gameObject, activeOnly: true);
			if (num3 > 0)
			{
				num++;
				details.Add($"Bodies: {label}: {num3} bones a drawn renderer names are still missing after the repair, so part of the model floats loose in the body.");
			}
			list.Add((num2 == 0) ? "no loose bones" : $"{num2} loose bones, {boneRepair.Rebuilt} rebuilt, {boneRepair.Hidden} renderers hidden");
			foreach (string note in boneRepair.Notes)
			{
				details.Add("Bodies: " + label + ": " + note);
			}
			float num4 = GuardianVisualScale.NominalHeight(size);
			float? num5 = GuardianVisualScale.MeasuredHeight(link.AssetId);
			if (polymorphView || num5.HasValue)
			{
				CharGenSafetyPatch.PrepareDollRoomBody(unitEntityView2, "SelfTest " + label, null, out var _);
			}
			float? num6 = BodyMeshHeight(unitEntityView2.gameObject);
			if (!num6.HasValue)
			{
				list.Add("no body mesh to measure");
			}
			else
			{
				float num7 = num6.Value * factor;
				list.Add($"{num6.Value:0.###} m drawn at {factor:0.###} = {num7:0.##} m");
				if ((num5.HasValue | polymorphView) && Math.Abs(num7 - num4) > num4 * 0.25f)
				{
					num++;
					details.Add($"Bodies: {label}: the body mesh is {num6.Value:0.###} m and is drawn at {factor:0.###}, " + $"so it stands {num7:0.##} m where a {size} body stands {num4:0.##} m.");
				}
				if (num5.HasValue && Math.Abs(num5.Value - num6.Value) > 0.02f * num6.Value)
				{
					num++;
					details.Add($"Bodies: {label}: GuardianVisualScale lists this model at {num5.Value:0.###} m, but its body mesh is {num6.Value:0.###} m.");
				}
			}
			if (polymorphView)
			{
				unitEntityView2.DisableSizeScaling = true;
				Vector3 overrideDollRoomScale = unitEntityView2.OverrideDollRoomScale;
				GuardianVisualScale.CorrectPolymorphView(unitEntityView2, factor);
				float sizeScale = unitEntityView2.GetSizeScale();
				if (Math.Abs(sizeScale - factor) > 0.001f)
				{
					num++;
					details.Add($"Bodies: {label}: a polymorph view corrected to {factor:0.###} reports GetSizeScale {sizeScale:0.###}, " + "so the world draws the form at the wrong height.");
				}
				if (Math.Abs(1f - factor) >= 0.001f)
				{
					Vector3 vector = ((overrideDollRoomScale == Vector3.zero) ? Vector3.one : overrideDollRoomScale) * factor;
					if ((unitEntityView2.OverrideDollRoomScale - vector).magnitude > 0.001f)
					{
						num++;
						details.Add($"Bodies: {label}: the inventory doll room override is {unitEntityView2.OverrideDollRoomScale}, not {vector}, " + "so the character screen would show the uncorrected body.");
					}
				}
				GuardianVisualScale.CorrectPolymorphView(unitEntityView2, 1f);
			}
			num += Attacks(label, unitEntityView2, unit, polymorphView, details, list);
		}
		catch (Exception ex2)
		{
			num++;
			details.Add("Bodies: " + label + ": probing it threw " + ex2.GetType().Name + ": " + ex2.Message);
		}
		finally
		{
			try
			{
				if (unitEntityView2 != null)
				{
					UnityEngine.Object.Destroy(unitEntityView2.gameObject);
				}
			}
			catch
			{
			}
		}
		probed.Add(label + ": " + string.Join(", ", list.ToArray()));
		return num;
	}

	private static float? BodyMeshHeight(GameObject root)
	{
		float? result = null;
		SkinnedMeshRenderer[] componentsInChildren = root.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren)
		{
			if (skinnedMeshRenderer == null || !skinnedMeshRenderer.gameObject.activeInHierarchy)
			{
				continue;
			}
			Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
			if (sharedMesh == null)
			{
				continue;
			}
			string name = (skinnedMeshRenderer.name + " " + sharedMesh.name).ToLowerInvariant();
			if (!NotTheBody.Any((string word) => name.Contains(word)))
			{
				float y = sharedMesh.bounds.max.y;
				if (!result.HasValue || y > result.Value)
				{
					result = y;
				}
			}
		}
		return result;
	}

	private static int Attacks(string label, UnitEntityView instance, BlueprintUnit unit, bool everyWeapon, List<string> details, List<string> notes)
	{
		UnitAnimationManager componentInChildren = instance.GetComponentInChildren<UnitAnimationManager>(includeInactive: true);
		if (componentInChildren == null || componentInChildren.AnimationSet == null)
		{
			notes.Add("animation set chosen at run time, attacks not checked");
			return 0;
		}
		int num = 0;
		WeaponAnimationStyle? weaponAnimationStyle = BorrowedModelFixes.FallbackFrom(BorrowedModelFixes.StylesWithClips(componentInChildren.GetAction(UnitAnimationType.MainHandAttack) as UnitAnimationActionHandAttack));
		int native = 0;
		int swung = 0;
		List<string> missing = new List<string>();
		if (everyWeapon)
		{
			foreach (WeaponAnimationStyle value3 in Enum.GetValues(typeof(WeaponAnimationStyle)))
			{
				if (value3 != WeaponAnimationStyle.None)
				{
					UnitAnimationType[] array = new UnitAnimationType[2]
					{
						UnitAnimationType.MainHandAttack,
						UnitAnimationType.OffHandAttack
					};
					foreach (UnitAnimationType unitAnimationType in array)
					{
						Count(BorrowedModelFixes.PlanSwing(componentInChildren, unitAnimationType, UnitAnimationSpecialAttackType.None, value3), $"{unitAnimationType} {value3}");
					}
				}
			}
			foreach (UnitAnimationSpecialAttackType value4 in Enum.GetValues(typeof(UnitAnimationSpecialAttackType)))
			{
				if (value4 != UnitAnimationSpecialAttackType.None)
				{
					Count(BorrowedModelFixes.PlanSwing(componentInChildren, UnitAnimationType.SpecialAttack, value4, WeaponAnimationStyle.None), $"special {value4}");
				}
			}
			if (missing.Count > 0)
			{
				num++;
				details.AddRange(SelfTestReport.WrapList($"Bodies: {label}: {missing.Count} attacks have no clip at all, because the body has no main-hand swing to fall back on:", missing));
			}
			notes.Add(string.Format("attacks: {0} animated by the body's own clips, {1} by its {2} swing", native, swung, weaponAnimationStyle.HasValue ? weaponAnimationStyle.Value.ToString() : "missing"));
		}
		if (unit?.Body != null)
		{
			List<KeyValuePair<string, BlueprintItemWeapon>> list = new List<KeyValuePair<string, BlueprintItemWeapon>>();
			if (unit.Body.PrimaryHand is BlueprintItemWeapon value)
			{
				list.Add(new KeyValuePair<string, BlueprintItemWeapon>("primary hand", value));
			}
			if (unit.Body.SecondaryHand is BlueprintItemWeapon value2)
			{
				list.Add(new KeyValuePair<string, BlueprintItemWeapon>("secondary hand", value2));
			}
			foreach (BlueprintItemWeapon additionalLimb in unit.Body.AdditionalLimbs)
			{
				if (additionalLimb != null)
				{
					list.Add(new KeyValuePair<string, BlueprintItemWeapon>("limb", additionalLimb));
				}
			}
			List<string> list2 = new List<string>();
			foreach (KeyValuePair<string, BlueprintItemWeapon> item in list)
			{
				WeaponVisualParameters visualParameters = item.Value.VisualParameters;
				UnitAnimationSpecialAttackType special = UnitAnimationSpecialAttackType.None;
				WeaponAnimationStyle style = WeaponAnimationStyle.None;
				UnitAnimationType type;
				if (visualParameters != null && visualParameters.HasSpecialAnimation)
				{
					type = UnitAnimationType.SpecialAttack;
					special = visualParameters.SpecialAnimation;
				}
				else if (item.Key != "limb")
				{
					type = ((item.Key == "primary hand") ? UnitAnimationType.MainHandAttack : UnitAnimationType.OffHandAttack);
					style = visualParameters?.AnimStyle ?? WeaponAnimationStyle.None;
				}
				else
				{
					type = UnitAnimationType.SpecialAttack;
					special = UnitAnimationSpecialAttackType.Weapon;
				}
				BorrowedModelFixes.SwingPlan swingPlan = BorrowedModelFixes.PlanSwing(componentInChildren, type, special, style);
				if (swingPlan == null)
				{
					num++;
					details.Add("Bodies: " + label + ": its " + item.Key + " " + item.Value.name + " has no clip on this body at all.");
				}
				else
				{
					list2.Add(item.Value.name + " " + (swingPlan.Vanilla ? "native" : ("as " + swingPlan.Style.ToString() + " swing")));
				}
			}
			if (list2.Count > 0)
			{
				notes.Add("own weapons: " + string.Join(", ", list2.ToArray()));
			}
		}
		return num;
		void Count(BorrowedModelFixes.SwingPlan plan, string what)
		{
			if (plan == null)
			{
				missing.Add(what);
			}
			else if (plan.Vanilla)
			{
				native++;
			}
			else
			{
				swung++;
			}
		}
	}
}
