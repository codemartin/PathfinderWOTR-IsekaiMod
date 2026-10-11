using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.View;
using Kingmaker.View.Animation;
using Kingmaker.Visual.Animation;
using Kingmaker.Visual.Animation.Kingmaker;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using UnityEngine;

namespace IsekaiMod.Utilities;

internal static class BorrowedModelFixes
{
	internal sealed class BoneRepair
	{
		public int Rebuilt;

		public int Hidden;

		public readonly List<string> Notes = new List<string>();
	}

	[HarmonyPatch(typeof(UnitEntityView), "OnDidAttachToData")]
	internal static class UnitEntityView_OnDidAttachToData_Patch
	{
		[HarmonyPostfix]
		private static void Postfix(UnitEntityView __instance)
		{
			try
			{
				if ((s_Units.Count != 0 || s_PolymorphBuffs.Count != 0) && !(__instance == null) && AppliesTo(__instance.EntityData))
				{
					RepairDanglingBones(__instance.gameObject);
				}
			}
			catch (Exception ex)
			{
				if (!s_ReportedRepairFailure)
				{
					s_ReportedRepairFailure = true;
					Main.Log("Borrowed body: repairing a view failed: " + ex);
				}
			}
		}
	}

	internal sealed class SwingPlan
	{
		public bool Vanilla;

		public UnitAnimationType Type;

		public WeaponAnimationStyle Style;
	}

	[HarmonyPatch(typeof(AttackHandInfo), "CreateAnimationHandleForAttack")]
	internal static class AttackHandInfo_CreateAnimationHandleForAttack_Patch
	{
		[HarmonyPrefix]
		private static bool Prefix(AttackHandInfo __instance, IEnumerable<AttackHandInfo> prevAttacks, bool isMounted)
		{
			try
			{
				if (s_Units.Count == 0 && s_PolymorphBuffs.Count == 0)
				{
					return true;
				}
				return !TryBodySwing(__instance, prevAttacks, isMounted);
			}
			catch (Exception ex)
			{
				if (!s_ReportedSwingFailure)
				{
					s_ReportedSwingFailure = true;
					Main.Log("Borrowed body: choosing an attack animation failed, the engine's own choice is kept: " + ex);
				}
				return true;
			}
		}
	}

	private static readonly HashSet<BlueprintGuid> s_Units = new HashSet<BlueprintGuid>();

	private static readonly HashSet<BlueprintGuid> s_PolymorphBuffs = new HashSet<BlueprintGuid>();

	internal const string RebuiltBonePrefix = "IsekaiRebuiltBone ";

	private static bool s_ReportedRepairFailure;

	private static readonly MethodInfo s_SetAnimationHandle = AccessTools.PropertySetter(typeof(AttackHandInfo), "AnimationHandle");

	private static bool s_ReportedSwingFailure;

	internal static IEnumerable<BlueprintGuid> RegisteredUnits => s_Units;

	internal static IEnumerable<BlueprintGuid> RegisteredPolymorphBuffs => s_PolymorphBuffs;

	internal static void RegisterUnits(params BlueprintUnit[] units)
	{
		if (units == null)
		{
			return;
		}
		foreach (BlueprintUnit blueprintUnit in units)
		{
			if (blueprintUnit != null)
			{
				s_Units.Add(blueprintUnit.AssetGuid);
			}
		}
	}

	internal static void RegisterPolymorphBuff(BlueprintBuff buff)
	{
		if (buff?.GetComponent<Polymorph>() != null)
		{
			s_PolymorphBuffs.Add(buff.AssetGuid);
		}
	}

	internal static bool AppliesTo(UnitEntityData unit)
	{
		if (unit == null)
		{
			return false;
		}
		BlueprintComponentAndRuntime<Polymorph> activePolymorph = unit.GetActivePolymorph();
		if (activePolymorph.Component != null)
		{
			if (activePolymorph.Runtime?.Fact?.Blueprint is BlueprintBuff blueprintBuff)
			{
				return s_PolymorphBuffs.Contains(blueprintBuff.AssetGuid);
			}
			return false;
		}
		BlueprintUnit blueprint = unit.Blueprint;
		if (blueprint != null)
		{
			return s_Units.Contains(blueprint.AssetGuid);
		}
		return false;
	}

	internal static int CountDanglingBones(GameObject root, bool activeOnly)
	{
		int num = 0;
		if (root == null)
		{
			return 0;
		}
		SkinnedMeshRenderer[] componentsInChildren = root.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren)
		{
			if (skinnedMeshRenderer == null || (activeOnly && !skinnedMeshRenderer.gameObject.activeInHierarchy))
			{
				continue;
			}
			Transform[] bones;
			try
			{
				bones = skinnedMeshRenderer.bones;
			}
			catch
			{
				continue;
			}
			if (bones == null)
			{
				continue;
			}
			Transform[] array = bones;
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j] == null)
				{
					num++;
				}
			}
		}
		return num;
	}

	internal static BoneRepair RepairDanglingBones(GameObject root)
	{
		BoneRepair boneRepair = new BoneRepair();
		if (root == null)
		{
			return boneRepair;
		}
		SkinnedMeshRenderer[] componentsInChildren = root.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren)
		{
			if (skinnedMeshRenderer == null)
			{
				continue;
			}
			Transform[] bones;
			try
			{
				bones = skinnedMeshRenderer.bones;
			}
			catch
			{
				continue;
			}
			if (bones == null || bones.Length == 0)
			{
				continue;
			}
			int num = -1;
			int num2 = 0;
			for (int j = 0; j < bones.Length; j++)
			{
				if (bones[j] == null)
				{
					num2++;
				}
				else if (num < 0)
				{
					num = j;
				}
			}
			if (num2 == 0)
			{
				continue;
			}
			Matrix4x4[] array = null;
			try
			{
				array = ((skinnedMeshRenderer.sharedMesh != null) ? skinnedMeshRenderer.sharedMesh.bindposes : null);
			}
			catch
			{
				array = null;
			}
			if (num < 0 || array == null || array.Length != bones.Length)
			{
				skinnedMeshRenderer.gameObject.SetActive(value: false);
				boneRepair.Hidden++;
				boneRepair.Notes.Add($"{skinnedMeshRenderer.name}: {num2} missing bones and " + ((num < 0) ? "no bone to hang them from" : "no readable bind poses") + ", so it is not drawn.");
				continue;
			}
			Transform transform = bones[num];
			Matrix4x4 matrix4x = array[num];
			for (int k = 0; k < bones.Length; k++)
			{
				if (!(bones[k] != null))
				{
					Matrix4x4 matrix4x2 = matrix4x * array[k].inverse;
					Transform transform2 = new GameObject("IsekaiRebuiltBone " + skinnedMeshRenderer.name + " " + k).transform;
					transform2.SetParent(transform, worldPositionStays: false);
					transform2.localPosition = matrix4x2.GetColumn(3);
					transform2.localRotation = matrix4x2.rotation;
					transform2.localScale = matrix4x2.lossyScale;
					bones[k] = transform2;
					boneRepair.Rebuilt++;
				}
			}
			skinnedMeshRenderer.bones = bones;
			boneRepair.Notes.Add($"{skinnedMeshRenderer.name}: {num2} missing bones rebuilt under {transform.name}.");
		}
		return boneRepair;
	}

	internal static bool IsShieldStyle(WeaponAnimationStyle style)
	{
		if (style != WeaponAnimationStyle.ShieldBuckler && style != WeaponAnimationStyle.ShieldLight && style != WeaponAnimationStyle.ShieldHeavy)
		{
			return style == WeaponAnimationStyle.ShieldTower;
		}
		return true;
	}

	internal static bool Covers(ICollection<WeaponAnimationStyle> stylesWithClips, WeaponAnimationStyle requested)
	{
		if (stylesWithClips == null || requested == WeaponAnimationStyle.None)
		{
			return false;
		}
		if (stylesWithClips.Contains(requested))
		{
			return true;
		}
		if (IsShieldStyle(requested))
		{
			return stylesWithClips.Contains(WeaponAnimationStyle.Shield);
		}
		return false;
	}

	internal static WeaponAnimationStyle? FallbackFrom(IEnumerable<WeaponAnimationStyle> mainHandStylesWithClips)
	{
		if (mainHandStylesWithClips == null)
		{
			return null;
		}
		foreach (WeaponAnimationStyle mainHandStylesWithClip in mainHandStylesWithClips)
		{
			if (mainHandStylesWithClip != WeaponAnimationStyle.None)
			{
				return mainHandStylesWithClip;
			}
		}
		return null;
	}

	internal static List<WeaponAnimationStyle> StylesWithClips(UnitAnimationActionHandAttack action)
	{
		List<WeaponAnimationStyle> list = new List<WeaponAnimationStyle>();
		if (action?.Settings == null)
		{
			return list;
		}
		foreach (UnitAnimationActionHandAttack.WeaponStyleSettings setting in action.Settings)
		{
			if (setting?.VariantWrappers != null && setting.VariantWrappers.Any((AnimationClipWrapper wrapper) => wrapper != null) && !list.Contains(setting.Style))
			{
				list.Add(setting.Style);
			}
		}
		return list;
	}

	internal static SwingPlan PlanSwing(UnitAnimationManager manager, UnitAnimationType type, UnitAnimationSpecialAttackType special, WeaponAnimationStyle style)
	{
		if (manager == null)
		{
			return null;
		}
		bool flag;
		switch (type)
		{
		case UnitAnimationType.None:
			return new SwingPlan
			{
				Vanilla = true,
				Type = type,
				Style = style
			};
		case UnitAnimationType.SpecialAttack:
			flag = special == UnitAnimationSpecialAttackType.None || manager.GetAction(special) != null;
			break;
		default:
			flag = Covers(StylesWithClips(manager.GetAction(type) as UnitAnimationActionHandAttack), style);
			break;
		}
		if (flag)
		{
			return new SwingPlan
			{
				Vanilla = true,
				Type = type,
				Style = style
			};
		}
		WeaponAnimationStyle? weaponAnimationStyle = FallbackFrom(StylesWithClips(manager.GetAction(UnitAnimationType.MainHandAttack) as UnitAnimationActionHandAttack));
		if (!weaponAnimationStyle.HasValue)
		{
			return null;
		}
		return new SwingPlan
		{
			Vanilla = false,
			Type = UnitAnimationType.MainHandAttack,
			Style = weaponAnimationStyle.Value
		};
	}

	private static WeaponAnimationStyle EngineStyle(AttackHandInfo attack)
	{
		WeaponAnimationStyle weaponAnimationStyle = attack.Weapon.GetAnimationStyle();
		if (weaponAnimationStyle == WeaponAnimationStyle.Fist && attack.Weapon.Wielder != null && attack.Weapon.Wielder.UseMartialArts)
		{
			weaponAnimationStyle = WeaponAnimationStyle.MartialArts;
		}
		return weaponAnimationStyle;
	}

	internal static bool TryBodySwing(AttackHandInfo attack, IEnumerable<AttackHandInfo> prevAttacks, bool isMounted)
	{
		if (s_SetAnimationHandle == null)
		{
			return false;
		}
		UnitEntityData unitEntityData = attack?.Hand?.Owner?.Unit;
		if (!AppliesTo(unitEntityData) || attack.Weapon == null)
		{
			return false;
		}
		UnitAnimationManager unitAnimationManager = unitEntityData.View?.AnimationManager;
		if (unitAnimationManager == null)
		{
			return false;
		}
		attack.GetAnimationType(out var animType, out var specType, out var _, prevAttacks);
		WeaponAnimationStyle style = ((animType != UnitAnimationType.SpecialAttack) ? EngineStyle(attack) : WeaponAnimationStyle.None);
		SwingPlan swingPlan = PlanSwing(unitAnimationManager, animType, specType, style);
		if (swingPlan == null || swingPlan.Vanilla)
		{
			return false;
		}
		UnitAnimationActionHandle unitAnimationActionHandle = unitAnimationManager.CreateHandle(swingPlan.Type, errorOnEmpty: false);
		if (unitAnimationActionHandle == null)
		{
			return false;
		}
		unitAnimationActionHandle.AttackWeaponStyle = swingPlan.Style;
		unitAnimationActionHandle.IsMounted = isMounted;
		int variantsCount = unitAnimationActionHandle.VariantsCount;
		if (variantsCount > 0)
		{
			unitAnimationActionHandle.Variant = UnityEngine.Random.Range(0, variantsCount);
		}
		s_SetAnimationHandle.Invoke(attack, new object[1] { unitAnimationActionHandle });
		return true;
	}
}
