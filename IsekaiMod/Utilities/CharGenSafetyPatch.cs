using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using IsekaiMod.Content.Guardians;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.Selection;
using Kingmaker.UI.ServiceWindow;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.View;
using Kingmaker.Visual.Animation.Kingmaker;
using Kingmaker.Visual.CharacterSystem;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Utilities;

[HarmonyPatch]
internal static class CharGenSafetyPatch
{
	internal const int DollRoomLayer = 15;

	private static bool s_ReportedMissingOverlordPrefab;

	private static bool s_ReportedMissingSlimePrefab;

	private static bool s_ReportedBlankOverlordBody;

	private static bool s_ReportedBlankSlimeBody;

	[HarmonyPrepare]
	private static bool Prepare()
	{
		return PatchGuard.ResolvesAll(typeof(CharGenSafetyPatch), (typeof(DollRoom), "UpdateDoll"));
	}

	[HarmonyPatch(typeof(LevelUpController), "FindPet")]
	[HarmonyPrefix]
	public static bool LevelUpController_FindPet_Prefix(LevelUpController __instance, BlueprintFeature feature, ref UnitEntityData __result)
	{
		if (__instance == null || __instance.Unit == null || feature == null)
		{
			__result = null;
			return false;
		}
		return true;
	}

	[HarmonyPatch(typeof(LevelUpController), "ApplyLevelUpActions")]
	[HarmonyPostfix]
	public static void LevelUpController_ApplyLevelUpActions_Postfix(LevelUpController __instance, UnitEntityData unit)
	{
		if (__instance != null && !(unit == null) && !(unit != __instance.Preview) && IsPreviewMode(__instance.State))
		{
			DollRoom dollRoom = Game.Instance?.UI?.Common?.DollRoom;
			if (dollRoom != null)
			{
				UpdateDollRoomPreview(dollRoom);
			}
		}
	}

	[HarmonyPatch(typeof(DollRoom), "UpdateDoll")]
	[HarmonyPostfix]
	public static void DollRoom_UpdateDoll_Postfix(DollRoom __instance)
	{
		UpdateDollRoomPreview(__instance);
	}

	[HarmonyPatch(typeof(DollRoom), "SetupInfo")]
	[HarmonyPostfix]
	public static void DollRoom_SetupInfo_Postfix(DollRoom __instance)
	{
		UpdateDollRoomPreview(__instance);
	}

	[HarmonyPatch(typeof(DollRoom), "UpdateCharacter")]
	[HarmonyPostfix]
	public static void DollRoom_UpdateCharacter_Postfix(DollRoom __instance)
	{
		UpdateDollRoomPreview(__instance);
	}

	[HarmonyPatch(typeof(DollRoom), "Cleanup")]
	[HarmonyPrefix]
	public static void DollRoom_Cleanup_Prefix(DollRoom __instance)
	{
		try
		{
			UnitEntityView value = Traverse.Create(__instance).Field("m_SimpleAvatar").GetValue<UnitEntityView>();
			if (IsModPreview(value))
			{
				UnityEngine.Object.Destroy(value.gameObject);
				Traverse.Create(__instance).Field("m_SimpleAvatar").SetValue(null);
			}
		}
		catch (Exception arg)
		{
			Main.Log($"DollRoom_Cleanup_Prefix exception: {arg}");
		}
	}

	private static DollRoomPreviewLayerKeeper PreviewMarkerOf(UnitEntityView view)
	{
		if (view == null)
		{
			return null;
		}
		try
		{
			return view.GetComponent<DollRoomPreviewLayerKeeper>();
		}
		catch
		{
			return null;
		}
	}

	private static bool IsModPreview(UnitEntityView view)
	{
		return PreviewMarkerOf(view) != null;
	}

	[HarmonyPatch(typeof(LevelUpController), "Commit")]
	[HarmonyFinalizer]
	public static void LevelUpController_Commit_Finalizer(LevelUpController __instance)
	{
		RemoveCharGenPreview(__instance);
	}

	public static void RemoveCharGenPreview(LevelUpController controller)
	{
		try
		{
			if (controller?.Unit?.Descriptor != null && (controller.State == null || controller.State.Mode == LevelUpState.CharBuildMode.CharGen || controller.State.Mode == LevelUpState.CharBuildMode.Respec))
			{
				RemoveCharGenPreviewForms(controller);
			}
		}
		catch (Exception arg)
		{
			Main.Log($"RemoveCharGenPreview exception: {arg}");
		}
	}

	private static void RemoveCharGenPreviewForms(LevelUpController controller)
	{
		UnitDescriptor descriptor = controller.Unit.Descriptor;
		BlueprintBuff modBlueprint = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "SkeletalOverlordFormBuff");
		BlueprintBuff modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "SlimeFormBuff");
		if (modBlueprint != null && descriptor.Buffs.HasFact(modBlueprint))
		{
			descriptor.Buffs.RemoveFact(modBlueprint);
		}
		if (modBlueprint2 != null && descriptor.Buffs.HasFact(modBlueprint2))
		{
			descriptor.Buffs.RemoveFact(modBlueprint2);
		}
		DollRoom dollRoom = Game.Instance?.UI?.Common?.DollRoom;
		if (dollRoom != null)
		{
			UnitEntityView value = Traverse.Create(dollRoom).Field("m_SimpleAvatar").GetValue<UnitEntityView>();
			if (IsModPreview(value))
			{
				UnityEngine.Object.Destroy(value.gameObject);
				Traverse.Create(dollRoom).Field("m_SimpleAvatar").SetValue(null);
			}
		}
	}

	public static bool IsOverlord(LevelUpController controller, UnitDescriptor d, LevelUpState state)
	{
		if (d != null)
		{
			BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeteromorphicOverlordHeritage");
			BlueprintFeature modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SkeletalOverlordFormFeature");
			if (modBlueprint != null && d.HasFact(modBlueprint))
			{
				return true;
			}
			if (modBlueprint2 != null && d.HasFact(modBlueprint2))
			{
				return true;
			}
			if (d.Progression.Classes.Any((ClassData c) => c.Archetypes.Any((BlueprintArchetype a) => a.name == "OverlordArchetype")))
			{
				return true;
			}
		}
		if (controller?.TryGetSeletedArchetype()?.name == "OverlordArchetype")
		{
			return true;
		}
		if (state?.Selections != null)
		{
			foreach (FeatureSelectionState selection in state.Selections)
			{
				FeatureSelectionState featureSelectionState = selection;
				while (featureSelectionState != null)
				{
					string text = featureSelectionState.SelectedItem?.Feature?.name;
					if (text == "HeteromorphicOverlordHeritage" || text == "SkeletalOverlordFormFeature")
					{
						return true;
					}
					featureSelectionState = featureSelectionState.Next;
				}
			}
		}
		if (controller?.LevelUpActions != null)
		{
			foreach (ILevelUpAction levelUpAction in controller.LevelUpActions)
			{
				if (levelUpAction is SelectFeature selectFeature)
				{
					string text2 = selectFeature.Item?.Feature?.name;
					if (text2 == "HeteromorphicOverlordHeritage" || text2 == "SkeletalOverlordFormFeature")
					{
						return true;
					}
				}
				if (levelUpAction is AddArchetype addArchetype && addArchetype.Archetype?.name == "OverlordArchetype")
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsSlime(LevelUpController controller, UnitDescriptor d, LevelUpState state)
	{
		if (d != null)
		{
			BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SlimeReincarnateHeritage");
			BlueprintFeature modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SlimeFormFeature");
			if (modBlueprint != null && d.HasFact(modBlueprint))
			{
				return true;
			}
			if (modBlueprint2 != null && d.HasFact(modBlueprint2))
			{
				return true;
			}
			if (d.Progression.Classes.Any((ClassData c) => c.Archetypes.Any((BlueprintArchetype a) => a.name == "DevourerArchetype")))
			{
				return true;
			}
		}
		if (controller?.TryGetSeletedArchetype()?.name == "DevourerArchetype")
		{
			return true;
		}
		if (state?.Selections != null)
		{
			foreach (FeatureSelectionState selection in state.Selections)
			{
				FeatureSelectionState featureSelectionState = selection;
				while (featureSelectionState != null)
				{
					string text = featureSelectionState.SelectedItem?.Feature?.name;
					if (text == "SlimeReincarnateHeritage" || text == "SlimeFormFeature")
					{
						return true;
					}
					featureSelectionState = featureSelectionState.Next;
				}
			}
		}
		if (controller?.LevelUpActions != null)
		{
			foreach (ILevelUpAction levelUpAction in controller.LevelUpActions)
			{
				if (levelUpAction is SelectFeature selectFeature)
				{
					string text2 = selectFeature.Item?.Feature?.name;
					if (text2 == "SlimeReincarnateHeritage" || text2 == "SlimeFormFeature")
					{
						return true;
					}
				}
				if (levelUpAction is AddArchetype addArchetype && addArchetype.Archetype?.name == "DevourerArchetype")
				{
					return true;
				}
			}
		}
		return false;
	}

	internal static bool IsPreviewMode(LevelUpState state)
	{
		if (state == null)
		{
			return false;
		}
		if (state.Mode != LevelUpState.CharBuildMode.CharGen && state.Mode != LevelUpState.CharBuildMode.Respec)
		{
			return state.Mode == LevelUpState.CharBuildMode.LevelUp;
		}
		return true;
	}

	internal static string PreviewPolymorphBuffGuid(bool overlord)
	{
		if (!overlord)
		{
			return "81bf22b0af505554f806b18822da197a";
		}
		return "e2013ae282a346c0b368559107b52f5b";
	}

	internal static string PreviewFormBuffName(bool overlord)
	{
		if (!overlord)
		{
			return "SlimeFormBuff";
		}
		return "SkeletalOverlordFormBuff";
	}

	internal static Polymorph PreviewPolymorph(bool overlord)
	{
		return ModelPolymorphOf(FactTreeSearch.FirstMatch(overlord ? BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "SkeletalOverlordFormBuff") : BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "SlimeFormBuff"), GrantedBuffs, (BlueprintBuff buff) => ModelPolymorphOf(buff) != null)) ?? ModelPolymorphOf(BlueprintTools.GetBlueprint<BlueprintBuff>(PreviewPolymorphBuffGuid(overlord)));
	}

	internal static float PreviewModelScale(bool overlord, Gender gender)
	{
		try
		{
			return GuardianVisualScale.PolymorphFactor(PreviewPolymorph(overlord), gender);
		}
		catch
		{
			return 1f;
		}
	}

	private static Polymorph ModelPolymorphOf(BlueprintBuff buff)
	{
		try
		{
			Polymorph polymorph = buff?.GetComponent<Polymorph>();
			if (polymorph == null)
			{
				return null;
			}
			return (polymorph.m_Prefab != null && polymorph.m_Prefab.Exists()) ? polymorph : null;
		}
		catch
		{
			return null;
		}
	}

	private static IEnumerable<BlueprintBuff> GrantedBuffs(BlueprintBuff buff)
	{
		List<BlueprintBuff> list = new List<BlueprintBuff>();
		BlueprintComponent[] array = buff?.ComponentsArray ?? new BlueprintComponent[0];
		for (int i = 0; i < array.Length; i++)
		{
			AddFacts addFacts = array[i] as AddFacts;
			if (addFacts?.m_Facts == null)
			{
				continue;
			}
			BlueprintUnitFactReference[] facts = addFacts.m_Facts;
			foreach (BlueprintUnitFactReference blueprintUnitFactReference in facts)
			{
				if (blueprintUnitFactReference != null && !(blueprintUnitFactReference.Guid == BlueprintGuid.Empty))
				{
					BlueprintBuff blueprintBuff;
					try
					{
						blueprintBuff = ResourcesLibrary.TryGetBlueprint(blueprintUnitFactReference.Guid) as BlueprintBuff;
					}
					catch
					{
						blueprintBuff = null;
					}
					if (blueprintBuff != null)
					{
						list.Add(blueprintBuff);
					}
				}
			}
		}
		return list;
	}

	private static UnitEntityView LoadPreviewPrefab(bool overlord, UnitEntityData unit)
	{
		Polymorph polymorph = PreviewPolymorph(overlord);
		UnitEntityView unitEntityView = GuardianVisualScale.PrefabFor(polymorph, unit?.Gender ?? Gender.Male)?.Load();
		if (unitEntityView == null && (overlord ? (!s_ReportedMissingOverlordPrefab) : (!s_ReportedMissingSlimePrefab)))
		{
			Main.Log("[CharGenPreview] No " + (overlord ? "Overlord" : "Slime") + " preview model: neither " + PreviewFormBuffName(overlord) + " nor anything it grants, nor the fallback buff " + PreviewPolymorphBuffGuid(overlord) + ", " + ((polymorph == null) ? "carries a Polymorph naming a model." : "has a Polymorph whose prefab did not load."));
			if (overlord)
			{
				s_ReportedMissingOverlordPrefab = true;
			}
			else
			{
				s_ReportedMissingSlimePrefab = true;
			}
		}
		return unitEntityView;
	}

	public static void UpdateDollRoomPreview(DollRoom dollRoom)
	{
		if (dollRoom == null)
		{
			return;
		}
		UnitEntityView failedPreview = null;
		Character doll = null;
		try
		{
			LevelUpController levelUpController = Game.Instance?.LevelUpController;
			UnitEntityData unitEntityData = ((levelUpController != null && IsPreviewMode(levelUpController.State) && !levelUpController.Committed && !levelUpController.Stopped) ? (levelUpController.Preview ?? levelUpController.Unit) : null);
			bool flag = unitEntityData?.Descriptor != null && (IsOverlord(levelUpController, levelUpController.Preview?.Descriptor, levelUpController.State) || IsOverlord(levelUpController, levelUpController.Unit?.Descriptor, levelUpController.State));
			bool flag2 = unitEntityData?.Descriptor != null && !flag && (IsSlime(levelUpController, levelUpController.Preview?.Descriptor, levelUpController.State) || IsSlime(levelUpController, levelUpController.Unit?.Descriptor, levelUpController.State));
			Transform value = Traverse.Create(dollRoom).Field("m_CharacterPlaceholder").GetValue<Transform>();
			if (value == null)
			{
				return;
			}
			UnitEntityView value2 = Traverse.Create(dollRoom).Field("m_SimpleAvatar").GetValue<UnitEntityView>();
			Character value3 = Traverse.Create(dollRoom).Field("m_Avatar").GetValue<Character>();
			if (!flag && !flag2)
			{
				if (IsModPreview(value2))
				{
					UnityEngine.Object.Destroy(value2.gameObject);
					Traverse.Create(dollRoom).Field("m_SimpleAvatar").SetValue(null);
					if (value3 != null && !value3.gameObject.activeSelf)
					{
						value3.gameObject.SetActive(value: true);
					}
				}
				return;
			}
			string text = (flag ? "OverlordPreviewAvatar" : "SlimePreviewAvatar");
			DollRoomPreviewLayerKeeper dollRoomPreviewLayerKeeper = PreviewMarkerOf(value2);
			if (dollRoomPreviewLayerKeeper != null && dollRoomPreviewLayerKeeper.FormName == text && value2.gameObject.activeSelf)
			{
				if (value3 != null && value3.gameObject.activeSelf)
				{
					value3.gameObject.SetActive(value: false);
				}
				return;
			}
			if (dollRoomPreviewLayerKeeper != null)
			{
				UnityEngine.Object.Destroy(value2.gameObject);
				Traverse.Create(dollRoom).Field("m_SimpleAvatar").SetValue(null);
			}
			if (value3 != null && !value3.gameObject.activeSelf)
			{
				doll = value3;
			}
			if (GaveUpOnBody(flag))
			{
				RestoreDoll(ref failedPreview, ref doll);
				return;
			}
			UnitEntityView unitEntityView = LoadPreviewPrefab(flag, unitEntityData);
			if (unitEntityView == null)
			{
				RestoreDoll(ref failedPreview, ref doll);
				return;
			}
			if (value3 != null)
			{
				value3.gameObject.SetActive(value: false);
				doll = value3;
			}
			UnitEntityView unitEntityView2 = UnityEngine.Object.Instantiate(unitEntityView, value, worldPositionStays: false);
			failedPreview = unitEntityView2;
			unitEntityView2.name = text;
			unitEntityView2.gameObject.AddComponent<DollRoomPreviewLayerKeeper>().FormName = text;
			unitEntityView2.IsInDollRoom = true;
			unitEntityView2.SetupCreaturesTerrainAlignment();
			BorrowedModelFixes.RepairDanglingBones(unitEntityView2.gameObject);
			CharacterUIDecal componentInChildren = unitEntityView2.GetComponentInChildren<CharacterUIDecal>();
			if (componentInChildren != null)
			{
				UnityEngine.Object.Destroy(componentInChildren.gameObject);
			}
			if (unitEntityView2.BloodyController != null)
			{
				if (unitEntityView2.BloodyController.MaterialController != null)
				{
					UnityEngine.Object.DestroyImmediate(unitEntityView2.BloodyController.MaterialController);
				}
				UnityEngine.Object.DestroyImmediate(unitEntityView2.BloodyController);
				unitEntityView2.BloodyController = null;
			}
			unitEntityView2.transform.localPosition = Vector3.zero;
			unitEntityView2.transform.localRotation = Quaternion.identity;
			Vector3 vector = unitEntityView.transform.localScale;
			if (unitEntityView.OverrideDollRoomScale != Vector3.zero)
			{
				vector = Vector3.Scale(vector, unitEntityView.OverrideDollRoomScale);
			}
			vector *= PreviewModelScale(flag, unitEntityData?.Gender ?? Gender.Male);
			unitEntityView2.transform.localScale = vector;
			unitEntityView2.gameObject.SetActive(value: true);
			bool flag3 = PrepareDollRoomBody(unitEntityView2, text, unitEntityData, out var failure);
			Character componentInChildren2 = unitEntityView2.GetComponentInChildren<Character>(includeInactive: true);
			SetLayerRecursively(unitEntityView2.gameObject, 15);
			DollRoom.UnscaleFxTimes(unitEntityView2.gameObject);
			Renderer[] componentsInChildren = unitEntityView2.GetComponentsInChildren<Renderer>(includeInactive: true);
			foreach (Renderer renderer in componentsInChildren)
			{
				if (renderer != null)
				{
					renderer.enabled = true;
				}
			}
			foreach (Behaviour vfxBehaviour in unitEntityView2.VfxBehaviours)
			{
				if (vfxBehaviour != null)
				{
					vfxBehaviour.enabled = true;
				}
			}
			if (!flag3)
			{
				ReportBlankBody(flag, failure);
				RestoreDoll(ref failedPreview, ref doll);
				return;
			}
			Traverse.Create(dollRoom).Field("m_SimpleAvatar").SetValue(unitEntityView2);
			failedPreview = null;
			doll = null;
			DollRoomCharacterController value4 = Traverse.Create(dollRoom).Field("m_CharacterController").GetValue<DollRoomCharacterController>();
			if (value4 != null)
			{
				value4.Character = value;
			}
			DollCamera value5 = Traverse.Create(dollRoom).Field("m_Camera").GetValue<DollCamera>();
			DollRoomCameraZoomPreset dollRoomCameraZoomPreset = componentInChildren2?.Skeleton?.DollRoomZoomPreset;
			if (value5 != null && dollRoomCameraZoomPreset != null)
			{
				Transform transform = ((!string.IsNullOrEmpty(dollRoomCameraZoomPreset.TargetBoneName)) ? FindChildRecursive(unitEntityView2.transform, dollRoomCameraZoomPreset.TargetBoneName) : null);
				value5.LookAt(transform ?? unitEntityView2.transform, dollRoomCameraZoomPreset);
			}
			UnitAnimationManager unitAnimationManager = componentInChildren2?.AnimationManager ?? unitEntityView2.GetComponentInChildren<UnitAnimationManager>();
			if (!(unitAnimationManager != null) || !unitAnimationManager.PlayableGraph.IsValid())
			{
				return;
			}
			try
			{
				Traverse.Create(dollRoom).Method("SetupAnimationManager", unitAnimationManager).GetValue();
			}
			catch (Exception arg)
			{
				Main.Log($"SetupAnimationManager exception: {arg}");
			}
		}
		catch (Exception arg2)
		{
			Main.Log($"UpdateDollRoomPreview exception: {arg2}");
			RestoreDoll(ref failedPreview, ref doll);
		}
	}

	internal static bool PrepareDollRoomBody(UnitEntityView view, string dollName, UnitEntityData unit, out string failure)
	{
		failure = null;
		if (view == null)
		{
			failure = "there is no view to prepare";
			return false;
		}
		try
		{
			view.IsInDollRoom = true;
			Character componentInChildren = view.GetComponentInChildren<Character>(includeInactive: true);
			if (componentInChildren != null)
			{
				componentInChildren.PreventUpdate = false;
				componentInChildren.IsInDollRoom = true;
				componentInChildren.gameObject.name = "Doll [" + dollName + "]";
				componentInChildren.SetRaceAndGender(unit?.Gender ?? Gender.Male, (unit?.Descriptor?.Progression?.Race?.RaceId).GetValueOrDefault());
				componentInChildren.Start();
				if (componentInChildren.Renderers.Count == 0)
				{
					componentInChildren.UpdateMesh();
				}
			}
		}
		catch (Exception ex)
		{
			failure = "building the body threw " + ex.GetType().Name + ": " + ex.Message;
			return false;
		}
		if (HasBodyGeometry(view.gameObject))
		{
			return true;
		}
		failure = "the model produced no mesh to draw";
		return false;
	}

	internal static bool HasBodyGeometry(GameObject root)
	{
		if (root == null)
		{
			return false;
		}
		Renderer[] componentsInChildren = root.GetComponentsInChildren<Renderer>(includeInactive: true);
		foreach (Renderer renderer in componentsInChildren)
		{
			if (renderer == null)
			{
				continue;
			}
			SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
			if (skinnedMeshRenderer != null)
			{
				if (skinnedMeshRenderer.sharedMesh != null)
				{
					return true;
				}
			}
			else if (renderer is MeshRenderer)
			{
				MeshFilter component = renderer.GetComponent<MeshFilter>();
				if (component != null && component.sharedMesh != null)
				{
					return true;
				}
			}
		}
		return false;
	}

	private static void RestoreDoll(ref UnitEntityView failedPreview, ref Character doll)
	{
		try
		{
			if (failedPreview != null)
			{
				UnityEngine.Object.Destroy(failedPreview.gameObject);
			}
		}
		catch
		{
		}
		try
		{
			if (doll != null && !doll.gameObject.activeSelf)
			{
				doll.gameObject.SetActive(value: true);
			}
		}
		catch
		{
		}
		failedPreview = null;
		doll = null;
	}

	private static bool GaveUpOnBody(bool overlord)
	{
		if (!overlord)
		{
			return s_ReportedBlankSlimeBody;
		}
		return s_ReportedBlankOverlordBody;
	}

	private static void ReportBlankBody(bool overlord, string failure)
	{
		if (!GaveUpOnBody(overlord))
		{
			if (overlord)
			{
				s_ReportedBlankOverlordBody = true;
			}
			else
			{
				s_ReportedBlankSlimeBody = true;
			}
			Main.Log("[CharGenPreview] The " + (overlord ? "Overlord" : "Slime") + " model loaded but cannot be shown: " + (failure ?? "unknown reason") + ". Showing the character's own body instead of an empty doll room.");
		}
	}

	private static Transform FindChildRecursive(Transform parent, string name)
	{
		if (parent == null || string.IsNullOrEmpty(name))
		{
			return null;
		}
		if (parent.name == name)
		{
			return parent;
		}
		for (int i = 0; i < parent.childCount; i++)
		{
			Transform transform = FindChildRecursive(parent.GetChild(i), name);
			if (transform != null)
			{
				return transform;
			}
		}
		return null;
	}

	private static void SetLayerRecursively(GameObject obj, int layer)
	{
		if (obj == null)
		{
			return;
		}
		obj.layer = layer;
		for (int i = 0; i < obj.transform.childCount; i++)
		{
			Transform child = obj.transform.GetChild(i);
			if (child != null)
			{
				SetLayerRecursively(child.gameObject, layer);
			}
		}
	}
}
