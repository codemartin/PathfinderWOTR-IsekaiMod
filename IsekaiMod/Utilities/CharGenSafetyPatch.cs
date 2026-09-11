using System;
using System.Linq;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.ResourceLinks;
using Kingmaker.UI.Selection;
using Kingmaker.UI.ServiceWindow;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using Kingmaker.View;
using Kingmaker.Visual.Animation.Kingmaker;
using Kingmaker.Visual.CharacterSystem;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Utilities
{
	[HarmonyPatch]
	internal static class CharGenSafetyPatch
	{
		private static readonly AccessTools.FieldRef<DollRoom, UnitEntityView> SimpleAvatarRef = AccessTools.FieldRefAccess<DollRoom, UnitEntityView>("m_SimpleAvatar");

		private static readonly AccessTools.FieldRef<DollRoom, Transform> PlaceholderRef = AccessTools.FieldRefAccess<DollRoom, Transform>("m_CharacterPlaceholder");

		private static readonly AccessTools.FieldRef<DollRoom, Character> AvatarRef = AccessTools.FieldRefAccess<DollRoom, Character>("m_Avatar");

		private static readonly AccessTools.FieldRef<DollRoom, DollRoomCharacterController> ControllerRef = AccessTools.FieldRefAccess<DollRoom, DollRoomCharacterController>("m_CharacterController");

		private static readonly AccessTools.FieldRef<DollRoom, DollCamera> CameraRef = AccessTools.FieldRefAccess<DollRoom, DollCamera>("m_Camera");

		private static readonly FastInvokeHandler SetupAnimationManagerInvoke = MethodInvoker.GetHandler(AccessTools.Method(typeof(DollRoom), "SetupAnimationManager", new Type[1] { typeof(UnitAnimationManager) }));

		private static BlueprintFeature _overlordHeritage;

		private static BlueprintFeature _overlordForm;

		private static BlueprintFeature _slimeHeritage;

		private static BlueprintFeature _slimeForm;

		private static BlueprintBuff _overlordBuff;

		private static BlueprintBuff _slimeBuff;

		private static BlueprintFeature OverlordHeritage => _overlordHeritage ?? (_overlordHeritage = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "HeteromorphicOverlordHeritage"));

		private static BlueprintFeature OverlordForm => _overlordForm ?? (_overlordForm = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SkeletalOverlordFormFeature"));

		private static BlueprintFeature SlimeHeritage => _slimeHeritage ?? (_slimeHeritage = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SlimeReincarnateHeritage"));

		private static BlueprintFeature SlimeForm => _slimeForm ?? (_slimeForm = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SlimeFormFeature"));

		private static BlueprintBuff OverlordBuff => _overlordBuff ?? (_overlordBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "SkeletalOverlordFormBuff"));

		private static BlueprintBuff SlimeBuff => _slimeBuff ?? (_slimeBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "SlimeFormBuff"));

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
			if (__instance != null && unit?.Descriptor != null && __instance.State != null && (__instance.State.Mode == LevelUpState.CharBuildMode.CharGen || __instance.State.Mode == LevelUpState.CharBuildMode.Respec))
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
				UnitEntityView unitEntityView = SimpleAvatarRef(__instance);
				if (unitEntityView != null && (unitEntityView.name == "OverlordPreviewAvatar" || unitEntityView.name == "SlimePreviewAvatar"))
				{
					UnityEngine.Object.Destroy(unitEntityView.gameObject);
					SimpleAvatarRef(__instance) = null;
				}
				Transform transform = PlaceholderRef(__instance);
				if (transform != null)
				{
					transform.localScale = Vector3.one;
				}
			}
			catch (Exception arg)
			{
				Main.Log($"DollRoom_Cleanup_Prefix exception: {arg}");
			}
		}

		[HarmonyPatch(typeof(LevelUpController), "Commit")]
		[HarmonyPostfix]
		public static void LevelUpController_Commit_Postfix(LevelUpController __instance)
		{
			if (__instance?.Unit?.Descriptor == null || (__instance.State != null && __instance.State.Mode != LevelUpState.CharBuildMode.CharGen && __instance.State.Mode != LevelUpState.CharBuildMode.Respec))
			{
				return;
			}
			UnitDescriptor descriptor = __instance.Unit.Descriptor;
			if (OverlordBuff != null && descriptor.Buffs.HasFact(OverlordBuff))
			{
				descriptor.Buffs.RemoveFact(OverlordBuff);
			}
			if (SlimeBuff != null && descriptor.Buffs.HasFact(SlimeBuff))
			{
				descriptor.Buffs.RemoveFact(SlimeBuff);
			}
			DollRoom dollRoom = Game.Instance?.UI?.Common?.DollRoom;
			if (dollRoom != null)
			{
				UnitEntityView unitEntityView = SimpleAvatarRef(dollRoom);
				if (unitEntityView != null && (unitEntityView.name == "OverlordPreviewAvatar" || unitEntityView.name == "SlimePreviewAvatar"))
				{
					UnityEngine.Object.Destroy(unitEntityView.gameObject);
					SimpleAvatarRef(dollRoom) = null;
				}
				Transform transform = PlaceholderRef(dollRoom);
				if (transform != null)
				{
					transform.localScale = Vector3.one;
				}
			}
		}

		public static bool IsOverlord(LevelUpController controller, UnitDescriptor d, LevelUpState state)
		{
			if (d != null)
			{
				if (OverlordHeritage != null && d.HasFact(OverlordHeritage))
				{
					return true;
				}
				if (OverlordForm != null && d.HasFact(OverlordForm))
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
				if (SlimeHeritage != null && d.HasFact(SlimeHeritage))
				{
					return true;
				}
				if (SlimeForm != null && d.HasFact(SlimeForm))
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

		public static void UpdateDollRoomPreview(DollRoom dollRoom)
		{
			if (dollRoom == null)
			{
				return;
			}
			try
			{
				LevelUpController levelUpController = Game.Instance?.LevelUpController;
				if (levelUpController == null || levelUpController.State == null || (levelUpController.State.Mode != LevelUpState.CharBuildMode.CharGen && levelUpController.State.Mode != LevelUpState.CharBuildMode.Respec))
				{
					return;
				}
				UnitEntityView unitEntityView = SimpleAvatarRef(dollRoom);
				bool flag = IsOverlord(levelUpController, levelUpController.Preview?.Descriptor, levelUpController.State) || IsOverlord(levelUpController, levelUpController.Unit?.Descriptor, levelUpController.State);
				bool flag2 = !flag && (IsSlime(levelUpController, levelUpController.Preview?.Descriptor, levelUpController.State) || IsSlime(levelUpController, levelUpController.Unit?.Descriptor, levelUpController.State));
				if (!flag && !flag2)
				{
					if (!(unitEntityView == null))
					{
						if (unitEntityView.name == "OverlordPreviewAvatar" || unitEntityView.name == "SlimePreviewAvatar")
						{
							UnityEngine.Object.Destroy(unitEntityView.gameObject);
							SimpleAvatarRef(dollRoom) = null;
						}
						Transform transform = PlaceholderRef(dollRoom);
						if (transform != null)
						{
							transform.localScale = Vector3.one;
						}
						Character character = AvatarRef(dollRoom);
						if (character != null && !character.gameObject.activeSelf)
						{
							character.gameObject.SetActive(value: true);
						}
					}
					return;
				}
				UnitEntityData unitEntityData = levelUpController.Preview ?? levelUpController.Unit;
				if (unitEntityData?.Descriptor == null)
				{
					return;
				}
				Transform transform2 = PlaceholderRef(dollRoom);
				if (transform2 == null)
				{
					return;
				}
				Character character2 = AvatarRef(dollRoom);
				string text = (flag ? "OverlordPreviewAvatar" : "SlimePreviewAvatar");
				if (unitEntityView != null && unitEntityView.name == text && unitEntityView.gameObject.activeSelf)
				{
					if (character2 != null && character2.gameObject.activeSelf)
					{
						character2.gameObject.SetActive(value: false);
					}
					return;
				}
				if (unitEntityView != null && (unitEntityView.name == "OverlordPreviewAvatar" || unitEntityView.name == "SlimePreviewAvatar"))
				{
					UnityEngine.Object.Destroy(unitEntityView.gameObject);
					SimpleAvatarRef(dollRoom) = null;
				}
				UnitEntityView unitEntityView2 = null;
				if (flag)
				{
					Polymorph polymorph = OverlordBuff?.GetComponent<Polymorph>();
					if (polymorph != null)
					{
						try
						{
							unitEntityView2 = polymorph.m_Prefab?.Load();
						}
						catch
						{
						}
					}
					if (unitEntityView2 == null)
					{
						try
						{
							unitEntityView2 = new UnitViewLink
							{
								AssetId = "faff8234f920503479d069ce93800862"
							}.Load();
						}
						catch
						{
						}
					}
					if (unitEntityView2 == null)
					{
						BlueprintUnit blueprintUnit = BlueprintSafetyExtensions.SafeGetBlueprint<BlueprintUnit>("50baa156cd334bbd8cd6deef65b1c12d") ?? BlueprintSafetyExtensions.SafeGetBlueprint<BlueprintUnit>("babbc46e076b78f408d8d87643daea3e");
						if (blueprintUnit?.Prefab != null)
						{
							try
							{
								unitEntityView2 = blueprintUnit.Prefab.Load();
							}
							catch
							{
							}
						}
					}
				}
				else if (flag2)
				{
					BlueprintUnit blueprint = BlueprintTools.GetBlueprint<BlueprintUnit>("46f09f7a14c1e084bb091d18ab385db6");
					if (blueprint?.Prefab != null)
					{
						try
						{
							unitEntityView2 = blueprint.Prefab.Load();
						}
						catch
						{
						}
					}
					if (unitEntityView2 == null)
					{
						Polymorph polymorph2 = BlueprintTools.GetBlueprint<BlueprintBuff>("46d4867d7d76c7d4583bdd6636a983ef")?.GetComponent<Polymorph>();
						if (polymorph2 != null)
						{
							try
							{
								unitEntityView2 = polymorph2.GetPrefab(unitEntityData)?.Load();
							}
							catch
							{
							}
							if (unitEntityView2 == null)
							{
								unitEntityView2 = polymorph2.m_Prefab?.Load();
							}
						}
					}
				}
				if (unitEntityView2 == null)
				{
					return;
				}
				if (character2 != null)
				{
					character2.gameObject.SetActive(value: false);
				}
				UnitEntityView unitEntityView3 = UnityEngine.Object.Instantiate(unitEntityView2);
				unitEntityView3.name = text;
				unitEntityView3.IsInDollRoom = true;
				unitEntityView3.SetupCreaturesTerrainAlignment();
				CharacterUIDecal componentInChildren = unitEntityView3.GetComponentInChildren<CharacterUIDecal>();
				if (componentInChildren != null)
				{
					UnityEngine.Object.Destroy(componentInChildren.gameObject);
				}
				if (unitEntityView3.BloodyController != null)
				{
					if (unitEntityView3.BloodyController.MaterialController != null)
					{
						UnityEngine.Object.DestroyImmediate(unitEntityView3.BloodyController.MaterialController);
					}
					UnityEngine.Object.DestroyImmediate(unitEntityView3.BloodyController);
					unitEntityView3.BloodyController = null;
				}
				if (transform2 != null)
				{
					transform2.localScale = Vector3.one;
					unitEntityView3.transform.SetParent(transform2, worldPositionStays: false);
				}
				unitEntityView3.transform.localPosition = Vector3.zero;
				unitEntityView3.transform.localRotation = Quaternion.identity;
				if (unitEntityView2.OverrideDollRoomScale != Vector3.zero)
				{
					unitEntityView3.transform.localScale = unitEntityView2.OverrideDollRoomScale;
				}
				else
				{
					unitEntityView3.transform.localScale = Vector3.one;
				}
				SetLayerRecursively(unitEntityView3.gameObject, 15);
				unitEntityView3.gameObject.SetActive(value: true);
				Renderer[] componentsInChildren = unitEntityView3.GetComponentsInChildren<Renderer>(includeInactive: true);
				foreach (Renderer renderer in componentsInChildren)
				{
					if (renderer != null)
					{
						renderer.enabled = true;
						renderer.gameObject.layer = 15;
						if (renderer is SkinnedMeshRenderer skinnedMeshRenderer)
						{
							skinnedMeshRenderer.updateWhenOffscreen = true;
						}
					}
				}
				foreach (Behaviour vfxBehaviour in unitEntityView3.VfxBehaviours)
				{
					if (vfxBehaviour != null)
					{
						vfxBehaviour.enabled = true;
						vfxBehaviour.gameObject.layer = 15;
					}
				}
				SimpleAvatarRef(dollRoom) = unitEntityView3;
				DollRoomCharacterController dollRoomCharacterController = ControllerRef(dollRoom);
				if (dollRoomCharacterController != null && transform2 != null)
				{
					dollRoomCharacterController.Character = transform2;
				}
				DollCamera dollCamera = CameraRef(dollRoom);
				if (dollCamera != null)
				{
					string text2 = unitEntityView3.CharacterAvatar?.Skeleton?.DollRoomZoomPreset?.TargetBoneName;
					Transform transform3 = ((!string.IsNullOrEmpty(text2)) ? FindChildRecursive(unitEntityView3.transform, text2) : null);
					if (transform3 != null)
					{
						dollCamera.LookAt(transform3, unitEntityView3.CharacterAvatar?.Skeleton?.DollRoomZoomPreset);
					}
					else
					{
						dollCamera.LookAt(unitEntityView3.transform);
					}
				}
				UnitAnimationManager componentInChildren2 = unitEntityView3.GetComponentInChildren<UnitAnimationManager>();
				if (!(componentInChildren2 != null))
				{
					return;
				}
				try
				{
					SetupAnimationManagerInvoke?.Invoke(dollRoom, componentInChildren2);
				}
				catch (Exception arg)
				{
					Main.Log($"SetupAnimationManager exception: {arg}");
				}
			}
			catch (Exception arg2)
			{
				Main.Log($"UpdateDollRoomPreview exception: {arg2}");
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
}
