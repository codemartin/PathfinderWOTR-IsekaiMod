using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using IsekaiMod.Content.Constellations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.DLC;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.ResourceLinks;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.Class;
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
		private struct FeatureOwnerKey : IEquatable<FeatureOwnerKey>
		{
			public readonly BlueprintFeature Feature;

			public readonly BlueprintScriptableObject Owner;

			public FeatureOwnerKey(BlueprintFeature feature, BlueprintScriptableObject owner)
			{
				Feature = feature;
				Owner = owner;
			}

			public bool Equals(FeatureOwnerKey other)
			{
				if (Feature == other.Feature)
				{
					return Owner == other.Owner;
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				if (obj is FeatureOwnerKey other)
				{
					return Equals(other);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return (((Feature != null) ? Feature.GetHashCode() : 0) * 397) ^ ((Owner != null) ? Owner.GetHashCode() : 0);
			}
		}

		private struct ArchetypeOwnerKey : IEquatable<ArchetypeOwnerKey>
		{
			public readonly BlueprintArchetype Archetype;

			public readonly BlueprintScriptableObject Owner;

			public ArchetypeOwnerKey(BlueprintArchetype archetype, BlueprintScriptableObject owner)
			{
				Archetype = archetype;
				Owner = owner;
			}

			public bool Equals(ArchetypeOwnerKey other)
			{
				if (Archetype == other.Archetype)
				{
					return Owner == other.Owner;
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				if (obj is ArchetypeOwnerKey other)
				{
					return Equals(other);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return (((Archetype != null) ? Archetype.GetHashCode() : 0) * 397) ^ ((Owner != null) ? Owner.GetHashCode() : 0);
			}
		}

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

		private static readonly HashSet<string> OtherSubclassesForSlime = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "OverlordArchetype", "GodEmperorArchetype", "HeroArchetype", "MartialGodArchetype", "MastermindArchetype", "ShadowMonarchArchetype" };

		private static readonly HashSet<string> OtherSubclassesForOverlord = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "DevourerArchetype", "GodEmperorArchetype", "HeroArchetype", "MartialGodArchetype", "MastermindArchetype", "ShadowMonarchArchetype" };

		private static readonly Dictionary<FeatureOwnerKey, bool> _relevantFeatureCache = new Dictionary<FeatureOwnerKey, bool>();

		private static readonly Dictionary<ArchetypeOwnerKey, bool> _relevantArchetypeCache = new Dictionary<ArchetypeOwnerKey, bool>();

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
				if (__instance.State.Mode == LevelUpState.CharBuildMode.CharGen)
				{
					TimelineManager.ClearActiveRunArchetype();
				}
				DollRoom dollRoom = Game.Instance?.UI?.Common?.DollRoom;
				if (dollRoom != null)
				{
					UpdateDollRoomPreview(dollRoom);
				}
			}
		}

		[HarmonyPatch(typeof(CharGenClassPhaseVM), "OnBeginDetailedView")]
		[HarmonyPostfix]
		public static void CharGenClassPhaseVM_OnBeginDetailedView_Postfix(CharGenClassPhaseVM __instance)
		{
			if (__instance != null && __instance.Mode == LevelUpState.CharBuildMode.CharGen)
			{
				TimelineManager.ClearActiveRunArchetype();
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

		public static bool IsOtherIsekaiArchetype(LevelUpController controller, UnitDescriptor d, LevelUpState state)
		{
			if (d != null && d.Progression.Classes.Any((ClassData c) => c.Archetypes.Any((BlueprintArchetype a) => OtherSubclassesForSlime.Contains(a.name))))
			{
				return true;
			}
			string text = controller?.TryGetSeletedArchetype()?.name;
			if (!string.IsNullOrEmpty(text) && OtherSubclassesForSlime.Contains(text))
			{
				return true;
			}
			if (controller?.LevelUpActions != null)
			{
				foreach (ILevelUpAction levelUpAction in controller.LevelUpActions)
				{
					if (levelUpAction is AddArchetype { Archetype: not null } addArchetype && OtherSubclassesForSlime.Contains(addArchetype.Archetype.name))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool IsOtherIsekaiArchetypeForOverlord(LevelUpController controller, UnitDescriptor d, LevelUpState state)
		{
			if (d != null && d.Progression.Classes.Any((ClassData c) => c.Archetypes.Any((BlueprintArchetype a) => OtherSubclassesForOverlord.Contains(a.name))))
			{
				return true;
			}
			string text = controller?.TryGetSeletedArchetype()?.name;
			if (!string.IsNullOrEmpty(text) && OtherSubclassesForOverlord.Contains(text))
			{
				return true;
			}
			if (controller?.LevelUpActions != null)
			{
				foreach (ILevelUpAction levelUpAction in controller.LevelUpActions)
				{
					if (levelUpAction is AddArchetype { Archetype: not null } addArchetype && OtherSubclassesForOverlord.Contains(addArchetype.Archetype.name))
					{
						return true;
					}
				}
			}
			return false;
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
				}
				else
				{
					if ((levelUpController.Preview ?? levelUpController.Unit)?.Descriptor == null)
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
						Polymorph polymorph2 = SlimeBuff?.GetComponent<Polymorph>();
						if (polymorph2 != null)
						{
							try
							{
								unitEntityView2 = polymorph2.m_Prefab?.Load();
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
									AssetId = "230a6b88cd620d04d811f0b8cefc4e42"
								}.Load();
							}
							catch
							{
							}
						}
						if (unitEntityView2 == null)
						{
							BlueprintUnit blueprintUnit2 = BlueprintSafetyExtensions.SafeGetBlueprint<BlueprintUnit>("a2c5957b102919d45851d45903b4eb8d") ?? BlueprintSafetyExtensions.SafeGetBlueprint<BlueprintUnit>("46f09f7a14c1e084bb091d18ab385db6");
							if (blueprintUnit2?.Prefab != null)
							{
								try
								{
									unitEntityView2 = blueprintUnit2.Prefab.Load();
								}
								catch
								{
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
					if (flag2)
					{
						unitEntityView3.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
					}
					else if (unitEntityView2.OverrideDollRoomScale != Vector3.zero)
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
					if (componentInChildren2 != null)
					{
						try
						{
							SetupAnimationManagerInvoke?.Invoke(dollRoom, componentInChildren2);
							return;
						}
						catch (Exception arg)
						{
							Main.Log($"SetupAnimationManager exception: {arg}");
							return;
						}
					}
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

		[HarmonyPatch(typeof(CharGenClassPhaseVM), "MeetsPrerequisites", new Type[]
		{
			typeof(LevelUpController),
			typeof(BlueprintCharacterClass)
		})]
		[HarmonyPrefix]
		public static bool CharGenClassPhaseVM_MeetsPrerequisites_Prefix(LevelUpController controller, BlueprintCharacterClass characterClass, ref bool __result)
		{
			if (controller == null || characterClass == null)
			{
				__result = false;
				return false;
			}
			UnitEntityData unitEntityData = controller.Unit ?? controller.Preview;
			if (unitEntityData == null || unitEntityData.Descriptor == null || controller.State == null)
			{
				__result = false;
				return false;
			}
			try
			{
				__result = characterClass.MeetsPrerequisites(unitEntityData.Descriptor, controller.State);
			}
			catch (Exception ex)
			{
				Main.Log("CharGenClassPhaseVM.MeetsPrerequisites safe catch for " + characterClass.name + ": " + ex.Message);
				__result = false;
			}
			return false;
		}

		[HarmonyPatch(typeof(CharGenClassPhaseVM), "IsClassAvailable", new Type[]
		{
			typeof(LevelUpController),
			typeof(BlueprintCharacterClass),
			typeof(bool)
		})]
		[HarmonyPrefix]
		public static bool CharGenClassPhaseVM_IsClassAvailable_Prefix(LevelUpController controller, BlueprintCharacterClass charClass, bool ignoreAlignment, ref bool __result)
		{
			if (controller == null || charClass == null)
			{
				__result = false;
				return false;
			}
			UnitEntityData unitEntityData = controller.Unit ?? controller.Preview;
			if (unitEntityData == null || unitEntityData.Descriptor == null || controller.State == null)
			{
				__result = false;
				return false;
			}
			try
			{
				bool flag = !charClass.IsDlcRestricted();
				bool flag2 = charClass.MeetsPrerequisites(unitEntityData.Descriptor, controller.State, ignoreAlignment);
				__result = flag & flag2;
			}
			catch (Exception ex)
			{
				Main.Log("CharGenClassPhaseVM.IsClassAvailable safe catch for " + charClass.name + ": " + ex.Message);
				__result = false;
			}
			return false;
		}

		private static bool IsRelevantPrereqFeature(BlueprintFeature feat, BlueprintScriptableObject owner)
		{
			if (feat == null)
			{
				return false;
			}
			FeatureOwnerKey key = new FeatureOwnerKey(feat, owner);
			if (_relevantFeatureCache.TryGetValue(key, out var value))
			{
				return value;
			}
			string text = feat.name ?? "";
			string text2 = owner?.name ?? "";
			value = text.StartsWith("Isekai", StringComparison.OrdinalIgnoreCase) || text.StartsWith("GodEmperor", StringComparison.OrdinalIgnoreCase) || text.StartsWith("Overlord", StringComparison.OrdinalIgnoreCase) || text.StartsWith("Hero", StringComparison.OrdinalIgnoreCase) || text.StartsWith("Martial", StringComparison.OrdinalIgnoreCase) || text.StartsWith("Mastermind", StringComparison.OrdinalIgnoreCase) || text.StartsWith("ShadowMonarch", StringComparison.OrdinalIgnoreCase) || text.StartsWith("Slime", StringComparison.OrdinalIgnoreCase) || text.StartsWith("Devourer", StringComparison.OrdinalIgnoreCase) || text.StartsWith("Heteromorphic", StringComparison.OrdinalIgnoreCase) || text.IndexOf("Legacy", StringComparison.OrdinalIgnoreCase) >= 0 || text2.StartsWith("Isekai", StringComparison.OrdinalIgnoreCase) || text2.StartsWith("GodEmperor", StringComparison.OrdinalIgnoreCase) || text2.StartsWith("Overlord", StringComparison.OrdinalIgnoreCase) || text2.StartsWith("Hero", StringComparison.OrdinalIgnoreCase) || text2.StartsWith("Martial", StringComparison.OrdinalIgnoreCase) || text2.StartsWith("Mastermind", StringComparison.OrdinalIgnoreCase) || text2.StartsWith("ShadowMonarch", StringComparison.OrdinalIgnoreCase) || text2.StartsWith("Slime", StringComparison.OrdinalIgnoreCase) || text2.StartsWith("Devourer", StringComparison.OrdinalIgnoreCase);
			_relevantFeatureCache[key] = value;
			return value;
		}

		private static bool IsRelevantPrereqArchetype(BlueprintArchetype arch, BlueprintScriptableObject owner)
		{
			if (arch == null)
			{
				return false;
			}
			ArchetypeOwnerKey key = new ArchetypeOwnerKey(arch, owner);
			if (_relevantArchetypeCache.TryGetValue(key, out var value))
			{
				return value;
			}
			string text = arch.name ?? "";
			string text2 = owner?.name ?? "";
			value = text.StartsWith("GodEmperor", StringComparison.OrdinalIgnoreCase) || text.StartsWith("Overlord", StringComparison.OrdinalIgnoreCase) || text.StartsWith("Hero", StringComparison.OrdinalIgnoreCase) || text.StartsWith("Martial", StringComparison.OrdinalIgnoreCase) || text.StartsWith("Mastermind", StringComparison.OrdinalIgnoreCase) || text.StartsWith("ShadowMonarch", StringComparison.OrdinalIgnoreCase) || text.StartsWith("Slime", StringComparison.OrdinalIgnoreCase) || text.StartsWith("Devourer", StringComparison.OrdinalIgnoreCase) || text2.StartsWith("Isekai", StringComparison.OrdinalIgnoreCase);
			_relevantArchetypeCache[key] = value;
			return value;
		}

		[HarmonyPatch(typeof(PrerequisiteNoFeature), "CheckInternal")]
		[HarmonyPostfix]
		public static void PrerequisiteNoFeature_CheckInternal_Postfix(PrerequisiteNoFeature __instance, ref bool __result, FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state)
		{
			if (!__result || __instance?.Feature == null || !IsRelevantPrereqFeature(__instance.Feature, __instance.OwnerBlueprint))
			{
				return;
			}
			// A rule that names its own owner means "not already owned". The pending level-up actions and the
			// preview always contain the feature being selected, so treating them as ownership would refuse
			// every re-selection. Other mods attach such rules to entries in the background list, which is
			// how the Isekai background group became unselectable after a change of mind.
			if (__instance.Feature == __instance.OwnerBlueprint)
			{
				return;
			}
			try
			{
				LevelUpController levelUpController = Game.Instance?.LevelUpController;
				if (levelUpController != null)
				{
					if (levelUpController.Preview?.Descriptor != null && levelUpController.Preview.Descriptor.HasFact(__instance.Feature))
					{
						__result = false;
						return;
					}
					if (levelUpController.LevelUpActions != null)
					{
						foreach (ILevelUpAction levelUpAction in levelUpController.LevelUpActions)
						{
							if (levelUpAction is SelectFeature selectFeature && selectFeature.Item?.Feature == __instance.Feature)
							{
								__result = false;
								return;
							}
						}
					}
				}
				if (state?.Selections == null)
				{
					return;
				}
				foreach (FeatureSelectionState selection in state.Selections)
				{
					FeatureSelectionState featureSelectionState = selection;
					while (featureSelectionState != null)
					{
						if (featureSelectionState.SelectedItem?.Feature == __instance.Feature)
						{
							__result = false;
							return;
						}
						featureSelectionState = featureSelectionState.Next;
					}
				}
			}
			catch (Exception ex)
			{
				Main.Log("PrerequisiteNoFeature_CheckInternal_Postfix safe catch: " + ex.Message);
			}
		}

		[HarmonyPatch(typeof(PrerequisiteNoArchetype), "CheckInternal")]
		[HarmonyPostfix]
		public static void PrerequisiteNoArchetype_CheckInternal_Postfix(PrerequisiteNoArchetype __instance, ref bool __result, FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state)
		{
			if (!__result || __instance?.Archetype == null || !IsRelevantPrereqArchetype(__instance.Archetype, __instance.OwnerBlueprint))
			{
				return;
			}
			try
			{
				LevelUpController levelUpController = Game.Instance?.LevelUpController;
				if (levelUpController == null)
				{
					return;
				}
				if (levelUpController.TryGetSeletedArchetype() == __instance.Archetype)
				{
					__result = false;
					return;
				}
				if (levelUpController.Preview?.Descriptor?.Progression?.Classes != null)
				{
					foreach (ClassData @class in levelUpController.Preview.Descriptor.Progression.Classes)
					{
						if (@class?.Archetypes != null && @class.Archetypes.Contains(__instance.Archetype))
						{
							__result = false;
							return;
						}
					}
				}
				if (levelUpController.LevelUpActions == null)
				{
					return;
				}
				foreach (ILevelUpAction levelUpAction in levelUpController.LevelUpActions)
				{
					if (levelUpAction is AddArchetype addArchetype && addArchetype.Archetype == __instance.Archetype)
					{
						__result = false;
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Main.Log("PrerequisiteNoArchetype_CheckInternal_Postfix safe catch: " + ex.Message);
			}
		}

		[HarmonyPatch(typeof(LevelUpController), "AddArchetype", new Type[] { typeof(BlueprintArchetype) })]
		[HarmonyPrefix]
		public static bool LevelUpController_AddArchetype1_Prefix(LevelUpController __instance, BlueprintArchetype archetype)
		{
			return ValidateArchetypeSelection(__instance, archetype);
		}

		[HarmonyPatch(typeof(LevelUpController), "AddArchetype", new Type[]
		{
			typeof(BlueprintCharacterClass),
			typeof(BlueprintArchetype)
		})]
		[HarmonyPrefix]
		public static bool LevelUpController_AddArchetype2_Prefix(LevelUpController __instance, BlueprintCharacterClass characterClass, BlueprintArchetype archetype)
		{
			return ValidateArchetypeSelection(__instance, archetype);
		}

		private static bool ValidateArchetypeSelection(LevelUpController controller, BlueprintArchetype archetype)
		{
			if (controller == null || archetype == null)
			{
				return true;
			}
			try
			{
				if (OtherSubclassesForSlime.Contains(archetype.name) && (IsSlime(controller, controller.Preview?.Descriptor, controller.State) || IsSlime(controller, controller.Unit?.Descriptor, controller.State)))
				{
					Main.Log("LevelUpController: Blocked " + archetype.name + " selection because Slime is active.");
					return false;
				}
				if (OtherSubclassesForOverlord.Contains(archetype.name) && (IsOverlord(controller, controller.Preview?.Descriptor, controller.State) || IsOverlord(controller, controller.Unit?.Descriptor, controller.State)))
				{
					Main.Log("LevelUpController: Blocked " + archetype.name + " selection because Overlord is active.");
					return false;
				}
			}
			catch
			{
				return true;
			}
			return true;
		}

		[HarmonyPatch(typeof(LevelUpController), "SelectFeature")]
		[HarmonyPrefix]
		public static bool LevelUpController_SelectFeature_Prefix(LevelUpController __instance, FeatureSelectionState selection, IFeatureSelectionItem item)
		{
			if (__instance == null || item?.Feature == null)
			{
				return true;
			}
			try
			{
				switch (item.Feature.name)
				{
				case "SlimeReincarnateHeritage":
				case "SlimeFormFeature":
					if (IsOtherIsekaiArchetype(__instance, __instance.Preview?.Descriptor, __instance.State) || IsOtherIsekaiArchetype(__instance, __instance.Unit?.Descriptor, __instance.State))
					{
						Main.Log("LevelUpController: Blocked Slime selection because another Isekai archetype is active.");
						return false;
					}
					break;
				case "HeteromorphicOverlordHeritage":
				case "SkeletalOverlordFormFeature":
					if (IsOtherIsekaiArchetypeForOverlord(__instance, __instance.Preview?.Descriptor, __instance.State) || IsOtherIsekaiArchetypeForOverlord(__instance, __instance.Unit?.Descriptor, __instance.State))
					{
						Main.Log("LevelUpController: Blocked Overlord selection because another Isekai archetype is active.");
						return false;
					}
					break;
				}
			}
			catch
			{
				return true;
			}
			return true;
		}

		[HarmonyPatch(typeof(BlueprintFeatureSelection), "ExtractSelectionItems")]
		[HarmonyPostfix]
		public static void BlueprintFeatureSelection_ExtractSelectionItems_Postfix(BlueprintFeatureSelection __instance, ref IEnumerable<IFeatureSelectionItem> __result, UnitDescriptor beforeLevelUpUnit, UnitDescriptor previewUnit)
		{
			if (__result == null)
			{
				return;
			}
			try
			{
				LevelUpController levelUpController = Game.Instance?.LevelUpController;
				UnitDescriptor d = previewUnit ?? beforeLevelUpUnit ?? levelUpController?.Preview?.Descriptor ?? levelUpController?.Unit?.Descriptor;
				LevelUpState state = levelUpController?.State;
				bool flag = IsOtherIsekaiArchetype(levelUpController, d, state);
				bool flag2 = IsOtherIsekaiArchetypeForOverlord(levelUpController, d, state);
				if (flag | flag2)
				{
					List<IFeatureSelectionItem> list = (__result as List<IFeatureSelectionItem>) ?? __result.ToList();
					bool flag3 = false;
					if (flag && list.RemoveAll((IFeatureSelectionItem item) => item?.Feature?.name == "SlimeReincarnateHeritage" || item?.Feature?.name == "SlimeFormFeature") > 0)
					{
						flag3 = true;
					}
					if (flag2 && list.RemoveAll((IFeatureSelectionItem item) => item?.Feature?.name == "HeteromorphicOverlordHeritage" || item?.Feature?.name == "SkeletalOverlordFormFeature") > 0)
					{
						flag3 = true;
					}
					if (flag3)
					{
						__result = list;
					}
				}
			}
			catch (Exception ex)
			{
				Main.Log("BlueprintFeatureSelection_ExtractSelectionItems_Postfix safe catch: " + ex.Message);
			}
		}
	}
}
