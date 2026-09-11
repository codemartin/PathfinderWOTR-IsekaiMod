using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using IsekaiMod.Components;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Parts;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities
{
	internal static class PatchTools
	{
		internal class SpellReference
		{
			public int level;

			public BlueprintAbilityReference value;

			public SpellReference(int inLevel, BlueprintAbilityReference inValue)
			{
				level = inLevel;
				value = inValue;
			}

			public override bool Equals(object p)
			{
				if (p == null)
				{
					return false;
				}
				if (this == p)
				{
					return true;
				}
				if (GetType() != p.GetType())
				{
					return false;
				}
				SpellReference spellReference = (SpellReference)p;
				if (value == null && spellReference.value == null)
				{
					return true;
				}
				if (value == null || spellReference.value == null)
				{
					return false;
				}
				return value.Guid.ToString() == spellReference.value.Guid.ToString();
			}

			public static bool operator ==(SpellReference left, SpellReference right)
			{
				if ((object)left == null && (object)right == null)
				{
					return true;
				}
				return left?.Equals(right) ?? false;
			}

			public static bool operator !=(SpellReference left, SpellReference right)
			{
				return !(left == right);
			}

			public override int GetHashCode()
			{
				return value?.Guid.ToString().GetHashCode() ?? 0;
			}
		}

		internal static class ArcanistPatcher
		{
			public static void Patch(BlueprintCharacterClassReference classRef, BlueprintSpellbookReference spellbookRef)
			{
				PatchArcaneResrvoir(classRef, spellbookRef);
				PatchConsumeSpells(classRef);
				PatchArcanistExploits(classRef);
			}

			private static void PatchArcaneResrvoir(BlueprintCharacterClassReference classRef, BlueprintSpellbookReference spellbookRef)
			{
				BlueprintAbilityResource blueprint = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("cac948cbbe79b55459459dd6a8fe44ce");
				PatchBuff(BlueprintTools.GetBlueprint<BlueprintBuff>("1dd776b7b27dcd54ab3cedbbaf440cf3"), classRef);
				PatchResource(blueprint, classRef);
				PatchArcaneReservoirBuffs(spellbookRef);
			}

			private static void PatchConsumeSpells(BlueprintCharacterClassReference classRef)
			{
				PatchResource(BlueprintTools.GetBlueprint<BlueprintAbilityResource>("d67ddd98ad019854d926f3d6a4e681c5"), classRef);
			}

			private static void PatchArcanistExploits(BlueprintCharacterClassReference classRef)
			{
				BlueprintFeatureSelection blueprint = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("b8bf3d5023f2d8c428fdf6438cecaea7");
				if (blueprint != null)
				{
					_ = blueprint.AllFeatures;
					foreach (BlueprintFeature allFeature in blueprint.AllFeatures)
					{
						if (allFeature == null)
						{
							continue;
						}
						AddFacts component = allFeature.GetComponent<AddFacts>();
						if (component == null)
						{
							continue;
						}
						_ = component.Facts;
						if (false)
						{
							continue;
						}
						foreach (BlueprintUnitFact fact in component.Facts)
						{
							if (fact is BlueprintAbility ability)
							{
								PatchAbility(ability, classRef);
							}
						}
					}
				}
				BlueprintBuff[] array = new BlueprintBuff[3]
				{
					BlueprintTools.GetBlueprint<BlueprintBuff>("d3361a1d65825aa4a952476639248792"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("d3a217dba1100f9449e7f249d2916f86"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("7b392a6348fe3ef4d907ce168a4c7773")
				};
				foreach (BlueprintBuff blueprintBuff in array)
				{
					if (blueprintBuff != null)
					{
						PatchBuff(blueprintBuff, classRef);
					}
				}
			}

			private static void PatchArcaneReservoirBuffs(BlueprintSpellbookReference spellbookRef)
			{
				BlueprintBuff[] array = new BlueprintBuff[8]
				{
					BlueprintTools.GetBlueprint<BlueprintBuff>("33e0c3a2a54c0e7489fa4ec4d79a581b"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("db4b91a8a297c4247b13cfb6ea228bf3"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("ea01ddf2c1878354990000d1c7fc5ce4"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("6fea993ed5782054a88fa54037a3e6dd"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("a27a3c5e45f9416428ce983e0d4bd2d2"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("91b2762997f0d8044baeeef0871eac6f"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("9aab299fb44ff3c49af5b8527a23fcf7"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("4425d831546249647b8c9ad06d7ed0e7")
				};
				foreach (BlueprintBuff blueprintBuff in array)
				{
					if (blueprintBuff != null)
					{
						PatchBuff(blueprintBuff, spellbookRef);
					}
				}
			}
		}

		internal static class KineticistPatcher
		{
			public static void Patch(BlueprintCharacterClassReference classRef)
			{
				BlueprintAbility[] array = new BlueprintAbility[18]
				{
					BlueprintTools.GetBlueprint<BlueprintAbility>("0ab1552e2ebdacf44bb7b20f5393366d"),
					BlueprintTools.GetBlueprint<BlueprintAbility>("403bcf42f08ca70498432cf62abee434"),
					BlueprintTools.GetBlueprint<BlueprintAbility>("e2610c88664e07343b4f3fb6336f210c"),
					BlueprintTools.GetBlueprint<BlueprintAbility>("83d5873f306ac954cad95b6aeeeb2d8c"),
					BlueprintTools.GetBlueprint<BlueprintAbility>("7980e876b0749fc47ac49b9552e259c1"),
					BlueprintTools.GetBlueprint<BlueprintAbility>("e53f34fb268a7964caf1566afb82dadd"),
					BlueprintTools.GetBlueprint<BlueprintAbility>("3baf01649a92ae640927b0f633db7c11"),
					BlueprintTools.GetBlueprint<BlueprintAbility>("8c25f52fce5113a4491229fd1265fc3c"),
					BlueprintTools.GetBlueprint<BlueprintAbility>("6276881783962284ea93298c1fe54c48"),
					BlueprintTools.GetBlueprint<BlueprintAbility>("d663a8d40be1e57478f34d6477a67270"),
					BlueprintTools.GetBlueprint<BlueprintAbility>("9afdc3eeca49c594aa7bf00e8e9803ac"),
					BlueprintTools.GetBlueprint<BlueprintAbility>("45eb571be891c4c4581b6fcddda72bcd"),
					BlueprintTools.GetBlueprint<BlueprintAbility>("b93e1f0540a4fa3478a6b47ae3816f32"),
					BlueprintTools.GetBlueprint<BlueprintAbility>("4e2e066dd4dc8de4d8281ed5b3f4acb6"),
					BlueprintTools.GetBlueprint<BlueprintAbility>("b813ceb82d97eed4486ddd86d3f7771b"),
					BlueprintTools.GetBlueprint<BlueprintAbility>("ba2113cfed0c2c14b93c20e7625a4c74"),
					BlueprintTools.GetBlueprint<BlueprintAbility>("16617b8c20688e4438a803effeeee8a6"),
					BlueprintTools.GetBlueprint<BlueprintAbility>("d29186edb20be6449b23660b39435398")
				};
				foreach (BlueprintAbility blueprintAbility in array)
				{
					if (blueprintAbility == null)
					{
						continue;
					}
					AbilityVariants component = blueprintAbility.GetComponent<AbilityVariants>();
					if (component?.m_Variants == null)
					{
						continue;
					}
					BlueprintAbilityReference[] variants = component.m_Variants;
					for (int j = 0; j < variants.Length; j++)
					{
						BlueprintAbility blueprintAbility2 = variants[j]?.Get();
						if (blueprintAbility2?.ComponentsArray == null)
						{
							continue;
						}
						for (int k = 0; k < blueprintAbility2.ComponentsArray.Length; k++)
						{
							if (blueprintAbility2.ComponentsArray[k] is ContextCalculateAbilityParamsBasedOnClass contextCalculateAbilityParamsBasedOnClass)
							{
								blueprintAbility2.ComponentsArray[k] = new ContextCalculateAbilityParamsBasedOnClasses
								{
									m_CharacterClasses = new BlueprintCharacterClassReference[2] { contextCalculateAbilityParamsBasedOnClass.m_CharacterClass, classRef },
									StatType = contextCalculateAbilityParamsBasedOnClass.StatType
								};
							}
						}
					}
				}
			}
		}

		internal static class ShifterPatcher
		{
			public static void Patch(BlueprintCharacterClassReference classRef)
			{
				BlueprintCharacterClassReference blueprintReference = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>("a406d6ebea5c46bba3160246be03e96f");
				if (blueprintReference == null || blueprintReference.Get() == null)
				{
					return;
				}
				BlueprintFeature[] array = new BlueprintFeature[7]
				{
					BlueprintTools.GetBlueprint<BlueprintFeature>("08a8cfba6ae34505a64d6ba00225c4d2"),
					BlueprintTools.GetBlueprint<BlueprintFeature>("f7996c5b51e348fc9277480d9cc0a88c"),
					BlueprintTools.GetBlueprint<BlueprintFeature>("19b7335626b3434cbe2af01fb33582ff"),
					BlueprintTools.GetBlueprint<BlueprintFeature>("8d6b338131764a4fb68eaf5b5c6cfe47"),
					BlueprintTools.GetBlueprint<BlueprintFeature>("024b8248f85d412cb7c520a9f746c547"),
					BlueprintTools.GetBlueprint<BlueprintFeature>("76b8314a83ff4825a145ac8f7b59d6e4"),
					BlueprintTools.GetBlueprint<BlueprintFeature>("28991899db1948d9bdd5f958b4add2d8")
				};
				foreach (BlueprintFeature blueprintFeature in array)
				{
					if (blueprintFeature == null)
					{
						continue;
					}
					PatchClassIntoFeatureOfReferenceClass(blueprintFeature, classRef, blueprintReference);
					foreach (AddFeatureOnClassLevel component in blueprintFeature.GetComponents<AddFeatureOnClassLevel>())
					{
						AddFeatureOnClassLevel addFeatureOnClassLevel = component;
						if (addFeatureOnClassLevel.m_AdditionalClasses == null)
						{
							addFeatureOnClassLevel.m_AdditionalClasses = new BlueprintCharacterClassReference[0];
						}
						if (!component.m_AdditionalClasses.Contains(classRef))
						{
							component.m_AdditionalClasses = component.m_AdditionalClasses.AddToArray(classRef);
						}
					}
				}
				BlueprintBuff[] array2 = new BlueprintBuff[10]
				{
					BlueprintTools.GetBlueprint<BlueprintBuff>("a237792fc2644a4ebc6eefa2d325f181"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("0fdc579eafbf4fceae649beed8188a5c"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("4a981c46dc60474cad17549a2b9f7f65"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("0bdfd34b7e5a4639bf6b716fa2ac0098"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("29c0e16558b043278f061b128b9d180c"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("c4c39e7078224b6caaf3c8a02032b5cb"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("49e196492d2c4aa588bafffea6db8c43"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("0555960c7ab8431d86de4e7db0c22160"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("d61bd26255d4425d929d73e57ef0e6dd"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("e0b35a32cf234381ab13c831106937f1")
				};
				foreach (BlueprintBuff blueprintBuff in array2)
				{
					if (blueprintBuff == null)
					{
						continue;
					}
					PatchClassIntoFeatureOfReferenceClass(blueprintBuff, classRef, blueprintReference);
					foreach (ContextRankConfig component2 in blueprintBuff.GetComponents<ContextRankConfig>())
					{
						if ((component2.m_BaseValueType == ContextRankBaseValueType.ClassLevel || component2.m_BaseValueType == ContextRankBaseValueType.SummClassLevelWithArchetype || component2.m_BaseValueType == ContextRankBaseValueType.MaxClassLevelWithArchetype) && component2.m_Class != null && !component2.m_Class.Contains(classRef))
						{
							component2.m_Class = component2.m_Class.AddToArray(classRef);
						}
					}
				}
				BlueprintAbilityResource[] array3 = new BlueprintAbilityResource[3]
				{
					BlueprintTools.GetBlueprint<BlueprintAbilityResource>("2210cea8cc94431a911dc5d4b6d72cbd"),
					BlueprintTools.GetBlueprint<BlueprintAbilityResource>("80923bd575dc48f5813c2343517414cf"),
					BlueprintTools.GetBlueprint<BlueprintAbilityResource>("72e7ec0822604f7da75c3dd32e93d5ea")
				};
				foreach (BlueprintAbilityResource blueprintAbilityResource in array3)
				{
					if (blueprintAbilityResource != null)
					{
						PatchResource(blueprintAbilityResource, classRef);
					}
				}
				BlueprintAbility[] array4 = new BlueprintAbility[3]
				{
					BlueprintTools.GetBlueprint<BlueprintAbility>("c35a9830c7684f76a704aff424128851"),
					BlueprintTools.GetBlueprint<BlueprintAbility>("2d52be9832e542bc88b1959be2f3b2e2"),
					BlueprintTools.GetBlueprint<BlueprintAbility>("f619bb36520a48478f900431b20d50c4")
				};
				foreach (BlueprintAbility blueprintAbility in array4)
				{
					if (blueprintAbility != null)
					{
						PatchClassIntoFeatureOfReferenceClass(blueprintAbility, classRef, blueprintReference);
					}
				}
				array2 = new BlueprintBuff[10]
				{
					BlueprintTools.GetBlueprint<BlueprintBuff>("ace35704bf744216b142adcbd5c58d13"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("627c3f3256494000b3cba6f461b2c44c"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("e850db860994442b83e964d3a43e9358"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("fb71be7bcbc648539d57171b0d0baf79"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("cd34e74f0d2844e3ab5580c1dbe3ff7d"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("ed9ac87aaac7419095395304c7b5d813"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("233617f6876a42ef973de8af502d4fed"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("566474477be14eaeb860086b82e5e9cf"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("e36d67f4b25a4d5ebf9add1e2bc52e74"),
					BlueprintTools.GetBlueprint<BlueprintBuff>("d9de1c4f6e68416196c193ac87962993")
				};
				foreach (BlueprintBuff blueprintBuff2 in array2)
				{
					if (blueprintBuff2 != null)
					{
						PatchClassIntoFeatureOfReferenceClass(blueprintBuff2, classRef, blueprintReference);
					}
				}
			}
		}

		[HarmonyPatch(typeof(UnitPartMagus), "get_Spellbook")]
		public static class UnitPartMagusPatcher
		{
			public static bool Prefix(UnitPartMagus __instance, ref Spellbook __result)
			{
				try
				{
					if (__instance == null)
					{
						return true;
					}
					if (__instance.m_Spellbook != null)
					{
						__result = __instance.m_Spellbook;
						return false;
					}
					if (__instance.Owner?.Progression == null)
					{
						return true;
					}
					BlueprintCharacterClass blueprintCharacterClass = IsekaiProtagonistClass.Get();
					if (blueprintCharacterClass == null)
					{
						return true;
					}
					ClassData classData = __instance.Owner.Progression.GetClassData(blueprintCharacterClass);
					if (classData == null)
					{
						return true;
					}
					BlueprintSpellbook blueprintSpellbook = classData.Spellbook ?? __instance.Class?.Spellbook;
					if (blueprintSpellbook != null)
					{
						__instance.m_Spellbook = __instance.Owner.GetSpellbook(blueprintSpellbook);
					}
					if (__instance.m_Spellbook != null)
					{
						__result = __instance.m_Spellbook;
						return false;
					}
				}
				catch
				{
					return true;
				}
				return true;
			}
		}

		[HarmonyPatch(typeof(AddVendorDiscount), "OnTurnOn")]
		public static class AddVendorDiscountPatcher
		{
			public static void Prefix(AddVendorDiscount __instance)
			{
				try
				{
					((EntityFactComponentDelegate<UnitEntityData, EmptyComponentData>)__instance)?.OnInitialize();
				}
				catch
				{
				}
			}
		}

		private static BlueprintSpellbookReference[] patchableSpellBooks = new BlueprintSpellbookReference[0];

		private static HashSet<BlueprintFact> s_FeaturesIgnoredWhenPatching;

		public static HashSet<BlueprintFact> FeaturesIgnoredWhenPatching
		{
			get
			{
				if (s_FeaturesIgnoredWhenPatching == null)
				{
					s_FeaturesIgnoredWhenPatching = new HashSet<BlueprintFact>
					{
						FeatTools.Selections.BasicFeatSelection,
						FeatTools.Selections.FighterFeatSelection,
						FeatTools.Selections.CombatTrick,
						FeatTools.Selections.SkaldFeatSelection,
						FeatTools.Selections.AnimalCompanionSelectionDomain,
						FeatTools.Selections.WarDomainGreaterFeatSelection,
						FeatTools.Selections.MagusFeatSelection,
						FeatTools.Selections.CavalierBonusFeatSelection,
						BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("f1add10c87fa4563ad5f71779eecde19")
					};
					s_FeaturesIgnoredWhenPatching.Remove(null);
				}
				return s_FeaturesIgnoredWhenPatching;
			}
		}

		public static void RegisterSpellbook(BlueprintSpellbook spellbook)
		{
			if (spellbook == null)
			{
				return;
			}
			BlueprintSpellbookReference blueprintSpellbookReference = spellbook.ToReference<BlueprintSpellbookReference>();
			if (!patchableSpellBooks.Contains(blueprintSpellbookReference))
			{
				patchableSpellBooks = patchableSpellBooks.AddToArray(blueprintSpellbookReference);
				BlueprintFeatureSelectMythicSpellbook blueprint = BlueprintTools.GetBlueprint<BlueprintFeatureSelectMythicSpellbook>("e1fbb0e0e610a3a4d91e5e5284587939");
				BlueprintFeatureSelectMythicSpellbook blueprint2 = BlueprintTools.GetBlueprint<BlueprintFeatureSelectMythicSpellbook>("3f16e9caf7c683c40884c7c455ed26af");
				BlueprintFeatureSelectMythicSpellbook blueprint3 = BlueprintTools.GetBlueprint<BlueprintFeatureSelectMythicSpellbook>("2b7027ee76cb4c58b2cff0475bc69fbb");
				BlueprintFeatureSelectMythicSpellbook blueprint4 = BlueprintTools.GetBlueprint<BlueprintFeatureSelectMythicSpellbook>("83385d9f4d714e4e94618703be762a20");
				BlueprintFeatureSelectMythicSpellbook blueprint5 = BlueprintTools.GetBlueprint<BlueprintFeatureSelectMythicSpellbook>("f3ff8515355e4738b128c3d01483f1ca");
				BlueprintFeatureSelectMythicSpellbook blueprint6 = BlueprintTools.GetBlueprint<BlueprintFeatureSelectMythicSpellbook>("c4ef6975167d4cf5acbfd66b60e63f9c");
				if (blueprint != null)
				{
					TTCoreExtensions.RegisterForMythicSpellbook(blueprint, spellbook);
				}
				if (blueprint2 != null)
				{
					TTCoreExtensions.RegisterForMythicSpellbook(blueprint2, spellbook);
				}
				if (blueprint3 != null)
				{
					TTCoreExtensions.RegisterForMythicSpellbook(blueprint3, spellbook);
				}
				if (blueprint4 != null)
				{
					TTCoreExtensions.RegisterForMythicSpellbook(blueprint4, spellbook);
				}
				if (blueprint5 != null)
				{
					TTCoreExtensions.RegisterForMythicSpellbook(blueprint5, spellbook);
				}
				if (blueprint6 != null)
				{
					TTCoreExtensions.RegisterForMythicSpellbook(blueprint6, spellbook);
				}
			}
		}

		private static BlueprintProgression PatchPatchClassProgressionBasedOnRefClassStep1(BlueprintProgression prog, BlueprintCharacterClass refClass)
		{
			if (prog == null || refClass == null)
			{
				return prog;
			}
			prog.IsClassFeature = true;
			prog.m_Classes = new BlueprintProgression.ClassWithLevel[1]
			{
				new BlueprintProgression.ClassWithLevel
				{
					m_Class = IsekaiProtagonistClass.GetReference(),
					AdditionalLevel = 0
				}
			};
			prog.AddComponent(delegate(ClassLevelsForPrerequisites c)
			{
				c.m_FakeClass = refClass.ToReference<BlueprintCharacterClassReference>();
				c.m_ActualClass = IsekaiProtagonistClass.GetReference();
				c.Modifier = 1.0;
			});
			prog.LevelEntries = new LevelEntry[0];
			prog.UIGroups = new UIGroup[0];
			if (refClass.Progression?.UIGroups != null)
			{
				UIGroup[] uIGroups = refClass.Progression.UIGroups;
				foreach (UIGroup uIGroup in uIGroups)
				{
					if (uIGroup != null)
					{
						prog.UIGroups = prog.UIGroups.AddToArray(uIGroup);
					}
				}
			}
			prog.m_UIDeterminatorsGroup = new BlueprintFeatureBaseReference[0];
			BlueprintProgression progression = refClass.Progression;
			if (progression != null)
			{
				_ = progression.UIDeterminatorsGroup;
				if (true)
				{
					foreach (BlueprintFeatureBase item in refClass.Progression.UIDeterminatorsGroup)
					{
						if (item != null)
						{
							prog.m_UIDeterminatorsGroup = prog.m_UIDeterminatorsGroup.AddToArray(item.ToReference<BlueprintFeatureBaseReference>());
						}
					}
				}
			}
			return prog;
		}

		public static BlueprintProgression PatchClassProgressionBasedOnRefClass(BlueprintProgression prog, BlueprintCharacterClass refClass)
		{
			if (prog == null || refClass == null)
			{
				return prog;
			}
			prog = PatchPatchClassProgressionBasedOnRefClassStep1(prog, refClass);
			if (refClass.Progression?.LevelEntries != null)
			{
				LevelEntry[] levelEntries = refClass.Progression.LevelEntries;
				foreach (LevelEntry levelEntry in levelEntries)
				{
					if (levelEntry != null && levelEntry.m_Features != null)
					{
						BlueprintFeatureBaseReference[] features = levelEntry.m_Features.ToArray();
						prog.LevelEntries = (prog.LevelEntries ?? new LevelEntry[0]).AddToArray(Helpers.CreateLevelEntry(levelEntry.Level, features));
					}
				}
			}
			return prog;
		}

		public static BlueprintProgression PatchClassProgressionBasedOnSeparateLists(BlueprintProgression prog, BlueprintCharacterClass refClass, LevelEntry[] additionalReference, LevelEntry[] removedReference)
		{
			if (prog == null || refClass == null)
			{
				return prog;
			}
			BlueprintArchetype refArchetype = new BlueprintArchetype
			{
				RemoveFeatures = removedReference,
				AddFeatures = additionalReference
			};
			return PatchClassProgressionBasedonRefArchetype(prog, refClass, refArchetype, null);
		}

		public static BlueprintProgression PatchClassProgressionBasedonRefArchetype(BlueprintProgression prog, BlueprintCharacterClass refClass, BlueprintArchetype refArchetype, LevelEntry[] additionalReference)
		{
			if (prog == null || refClass == null || refArchetype == null)
			{
				return prog;
			}
			prog = PatchPatchClassProgressionBasedOnRefClassStep1(prog, refClass);
			LevelEntry[] array = refClass.Progression?.LevelEntries;
			if (array == null)
			{
				return prog;
			}
			BlueprintFeatureBase[] array2 = new BlueprintFeatureBase[0];
			LevelEntry[] array3 = array;
			foreach (LevelEntry levelEntry in array3)
			{
				BlueprintFeatureBaseReference[] array4 = new BlueprintFeatureBaseReference[0];
				BlueprintFeatureBaseReference[] array5 = levelEntry.m_Features.ToArray();
				BlueprintFeatureBaseReference[] array6 = new BlueprintFeatureBaseReference[0];
				LevelEntry[] removeFeatures = refArchetype.RemoveFeatures;
				foreach (LevelEntry levelEntry2 in removeFeatures)
				{
					if (levelEntry2.Level == levelEntry.Level)
					{
						array6 = array6.AddRangeToArray(levelEntry2.m_Features.ToArray());
					}
				}
				BlueprintFeatureBaseReference[] array7 = array5;
				foreach (BlueprintFeatureBaseReference blueprintFeatureBaseReference in array7)
				{
					if (!array6.Contains(blueprintFeatureBaseReference))
					{
						array4 = array4.AddToArray(blueprintFeatureBaseReference);
					}
				}
				BlueprintFeatureBaseReference[] array8 = new BlueprintFeatureBaseReference[0];
				removeFeatures = refArchetype.AddFeatures;
				foreach (LevelEntry levelEntry3 in removeFeatures)
				{
					if (levelEntry3.Level == levelEntry.Level)
					{
						array8 = array8.AddRangeToArray(levelEntry3.m_Features.ToArray());
					}
				}
				if (array8 != null && array8.Length != 0)
				{
					array7 = array8;
					foreach (BlueprintFeatureBaseReference blueprintFeatureBaseReference2 in array7)
					{
						array4 = array4.AddToArray(blueprintFeatureBaseReference2);
						if (!array2.Contains(blueprintFeatureBaseReference2))
						{
							array2 = array2.AddToArray(blueprintFeatureBaseReference2);
						}
					}
				}
				if (additionalReference != null)
				{
					LevelEntry levelEntry4 = null;
					removeFeatures = additionalReference;
					foreach (LevelEntry levelEntry5 in removeFeatures)
					{
						if (levelEntry5.Level == levelEntry.Level)
						{
							levelEntry4 = levelEntry5;
						}
					}
					if (levelEntry4 != null)
					{
						foreach (BlueprintFeatureBaseReference feature in levelEntry4.m_Features)
						{
							array4 = array4.AddToArray(feature);
							if (!array2.Contains(feature))
							{
								array2 = array2.AddToArray(feature);
							}
						}
					}
				}
				prog.LevelEntries = prog.LevelEntries.AddToArray(Helpers.CreateLevelEntry(levelEntry.Level, array4));
			}
			if (additionalReference != null)
			{
				array3 = additionalReference;
				foreach (LevelEntry levelEntry6 in array3)
				{
					bool flag = false;
					LevelEntry[] removeFeatures = prog.LevelEntries;
					for (int j = 0; j < removeFeatures.Length; j++)
					{
						if (removeFeatures[j].Level == levelEntry6.Level)
						{
							flag = true;
						}
					}
					if (flag)
					{
						continue;
					}
					prog.LevelEntries = prog.LevelEntries.AddToArray(levelEntry6);
					foreach (BlueprintFeatureBaseReference feature2 in levelEntry6.m_Features)
					{
						if (!array2.Contains(feature2))
						{
							array2 = array2.AddToArray(feature2);
						}
					}
				}
			}
			if (array2.Length != 0)
			{
				prog.UIGroups = prog.UIGroups.AddToArray(Helpers.CreateUIGroup(array2));
			}
			return prog;
		}

		public static void PatchProgressionFeaturesBasedOnReferenceArchetype(BlueprintCharacterClassReference myClass, BlueprintCharacterClassReference referenceClass, BlueprintArchetype refArchetype)
		{
			if (myClass == null || referenceClass == null || refArchetype == null || refArchetype.AddFeatures == null)
			{
				return;
			}
			HashSet<BlueprintFact> loopPrevention = new HashSet<BlueprintFact>();
			HashSet<BlueprintFeatureBase> hashSet = new HashSet<BlueprintFeatureBase>();
			LevelEntry[] addFeatures = refArchetype.AddFeatures;
			foreach (LevelEntry levelEntry in addFeatures)
			{
				if (levelEntry?.Features == null)
				{
					continue;
				}
				foreach (BlueprintFeatureBase feature in levelEntry.Features)
				{
					if (feature != null && !hashSet.Contains(feature))
					{
						hashSet.Add(feature);
					}
				}
			}
			foreach (BlueprintFeatureBase item in hashSet)
			{
				PatchClassIntoFeatureOfReferenceClass(item, myClass, referenceClass, 0, loopPrevention);
			}
		}

		public static void PatchProgressionFeaturesBasedOnReferenceClass(BlueprintProgression prog, BlueprintCharacterClassReference myClass, BlueprintCharacterClassReference referenceClass)
		{
			if (prog == null || myClass == null || referenceClass == null || prog.LevelEntries == null)
			{
				return;
			}
			HashSet<BlueprintFact> loopPrevention = new HashSet<BlueprintFact>();
			HashSet<BlueprintFeatureBase> hashSet = new HashSet<BlueprintFeatureBase>();
			LevelEntry[] levelEntries = prog.LevelEntries;
			foreach (LevelEntry levelEntry in levelEntries)
			{
				if (levelEntry?.m_Features == null)
				{
					continue;
				}
				foreach (BlueprintFeatureBaseReference feature in levelEntry.m_Features)
				{
					BlueprintFeatureBase blueprintFeatureBase = feature?.Get();
					if (blueprintFeatureBase != null && !hashSet.Contains(blueprintFeatureBase))
					{
						hashSet.Add(blueprintFeatureBase);
					}
				}
			}
			foreach (BlueprintFeatureBase item in hashSet)
			{
				PatchClassIntoFeatureOfReferenceClass(item, myClass, referenceClass, 0, loopPrevention);
			}
		}

		public static void AddPrerequisiteNoFeature(this BlueprintProgression prog, BlueprintFeature other)
		{
			if (prog != null && other != null)
			{
				prog.AddPrerequisite(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = other.ToReference<BlueprintFeatureReference>();
				});
			}
		}

		public static void PatchClassIntoFeatureOfReferenceClass(BlueprintFact feature, BlueprintCharacterClassReference myClass, BlueprintCharacterClassReference referenceClass, int level = 0, HashSet<BlueprintFact> loopPrevention = null)
		{
			if (feature == null || myClass == null || referenceClass == null)
			{
				return;
			}
			if (loopPrevention == null)
			{
				loopPrevention = new HashSet<BlueprintFact>();
			}
			int num = level + 1;
			if (num > 30)
			{
				Main.IsekaiContext.Logger.LogError("Attempt to patch Progression Tree stopped at Level 30 to prevent endless loop, if you see this message please report so we can figure out if someone created a loop here or if this limit needs to be higher");
				if (feature.name != null)
				{
					// One line instead of one stack-trace-capturing LogError per visited fact
					// (this loop alone produced most of the 128 MB log and the slow load).
					Main.IsekaiContext.Logger.Log($"reference class={referenceClass.Guid} Stop Feature={feature.AssetGuid} name={feature.name} visited={string.Join(",", loopPrevention.Select(f => f.AssetGuid))}");
					return;
				}
				Main.IsekaiContext.Logger.LogError($"reference class={referenceClass.Guid} Stop Feature={feature.AssetGuid}");
			}
			else
			{
				if (FeaturesIgnoredWhenPatching.Contains(feature))
				{
					return;
				}
				if (!loopPrevention.Contains(feature))
				{
					loopPrevention.Add(feature);
					try
					{
						if (feature is BlueprintProgression progression)
						{
							PatchClassProgression(progression, myClass, referenceClass, num, loopPrevention);
						}
						if (feature is BlueprintFeatureSelection selection)
						{
							PatchClassSelection(selection, myClass, referenceClass, num, loopPrevention);
						}
						if (((BlueprintScriptableObject)feature).Components == null || ((BlueprintScriptableObject)feature).Components.Length == 0)
						{
							return;
						}
						HashSet<SpellReference> hashSet = new HashSet<SpellReference>();
						List<SpontaneousSpellConversion> list = new List<SpontaneousSpellConversion>();
						List<CannyDefensePermanent> list2 = new List<CannyDefensePermanent>();
						BlueprintComponent[] components = ((BlueprintScriptableObject)feature).Components;
						foreach (BlueprintComponent blueprintComponent in components)
						{
							if (blueprintComponent == null)
							{
								continue;
							}
							HandleComponent(feature.AssetGuid, myClass, referenceClass, num, hashSet, blueprintComponent, loopPrevention);
							if (blueprintComponent is ContextRankConfig contextRankConfig && (contextRankConfig.m_BaseValueType == ContextRankBaseValueType.ClassLevel || contextRankConfig.m_BaseValueType == ContextRankBaseValueType.SummClassLevelWithArchetype || contextRankConfig.m_BaseValueType == ContextRankBaseValueType.MaxClassLevelWithArchetype))
							{
								if (!contextRankConfig.m_Class.Contains(myClass) && contextRankConfig.m_Class.Contains(referenceClass))
								{
									contextRankConfig.m_Class = contextRankConfig.m_Class.AddToArray(myClass);
									contextRankConfig.m_BaseValueType = ContextRankBaseValueType.SummClassLevelWithArchetype;
								}
							}
							else if (blueprintComponent is SpontaneousSpellConversion { m_CharacterClass: not null } spontaneousSpellConversion && spontaneousSpellConversion.m_CharacterClass.Equals(referenceClass))
							{
								list.Add(spontaneousSpellConversion);
							}
							else if (blueprintComponent is CannyDefensePermanent { m_CharacterClass: not null } cannyDefensePermanent && cannyDefensePermanent.m_CharacterClass.Equals(referenceClass))
							{
								list2.Add(cannyDefensePermanent);
							}
						}
						foreach (SpellReference spellReference in hashSet)
						{
							feature.AddComponent(delegate(AddKnownSpell c)
							{
								c.m_CharacterClass = myClass;
								c.m_Spell = spellReference.value;
								c.SpellLevel = spellReference.level;
							});
						}
						foreach (SpontaneousSpellConversion conversion in list)
						{
							feature.AddComponent(delegate(SpontaneousSpellConversion c)
							{
								c.m_CharacterClass = myClass;
								c.m_SpellsByLevel = conversion.m_SpellsByLevel;
							});
						}
						foreach (CannyDefensePermanent cannyDefense in list2)
						{
							feature.AddComponent(delegate(CannyDefensePermanent c)
							{
								c.m_CharacterClass = myClass;
								c.RequiresKensai = cannyDefense.RequiresKensai;
								c.m_ChosenWeaponBlueprint = cannyDefense.m_ChosenWeaponBlueprint;
							});
						}
						return;
					}
					catch (NullReferenceException ex)
					{
						if (feature.name != null)
						{
							Main.IsekaiContext.Logger.LogError($"Unpatchable Feature={feature.AssetGuid} name={feature.name} at level={num} reason={ex.Message}");
						}
						else
						{
							Main.IsekaiContext.Logger.LogError($"Unpatchable Feature={feature.AssetGuid} at level={num} reason={ex.Message}");
						}
						return;
					}
				}
				Main.IsekaiContext.Logger.Log($"reference class={referenceClass.Guid} feature re-encountered at level={num} guid={feature.AssetGuid} name={feature.name}");
			}
		}

		private static void PatchClassProgression(BlueprintProgression progression, BlueprintCharacterClassReference myClass, BlueprintCharacterClassReference referenceClass, int mylevel, HashSet<BlueprintFact> loopPrevention)
		{
			progression.GiveFeaturesForPreviousLevels = true;
			if (progression.m_Classes != null && progression.m_Classes.Length != 0)
			{
				BlueprintProgression.ClassWithLevel[] classes = progression.m_Classes;
				foreach (BlueprintProgression.ClassWithLevel classWithLevel in classes)
				{
					if (classWithLevel != null && myClass.Equals(classWithLevel.m_Class))
					{
						return;
					}
				}
				progression.AddClass(myClass);
			}
			HashSet<BlueprintFeatureBase> hashSet = new HashSet<BlueprintFeatureBase>();
			LevelEntry[] levelEntries = progression.LevelEntries;
			for (int i = 0; i < levelEntries.Length; i++)
			{
				foreach (BlueprintFeatureBase feature in levelEntries[i].Features)
				{
					if (feature != null && !hashSet.Contains(feature))
					{
						hashSet.Add(feature);
					}
				}
			}
			foreach (BlueprintFeatureBase item in hashSet)
			{
				PatchClassIntoFeatureOfReferenceClass(item, myClass, referenceClass, mylevel, loopPrevention);
			}
		}

		private static void PatchClassSelection(BlueprintFeatureSelection selection, BlueprintCharacterClassReference myClass, BlueprintCharacterClassReference referenceClass, int mylevel, HashSet<BlueprintFact> loopPrevention)
		{
			if (selection == null || selection.m_AllFeatures == null)
			{
				return;
			}
			string text = selection.AssetGuid.ToString();
			if (selection.m_AllFeatures.Length > 30 && !text.Equals("60008a10ad7ad6543b1f63016741a5d2") && !text.Equals("c074a5d615200494b8f2a9c845799d93") && !text.Equals("4223fe18c75d4d14787af196a04e14e7") && !text.Equals("28710502f46848d48b3f0d6132817c4e") && !text.Equals("2476514e31791394fa140f1a07941c96") && !text.Equals("9846043cf51251a4897728ed6e24e76f") && !text.Equals("99999999000900000009000000000001") && !text.Equals("58d6f8e9eea63f6418b107ce64f315ea") && !text.Equals("5c883ae0cd6d7d5448b7a420f51f8459"))
			{
				Main.IsekaiContext.Logger.Log($"reference class={referenceClass.Guid} Stop Feature={text} name={selection.name} reason=selection contains too many features and thus likely is a basic feat variation");
				return;
			}
			BlueprintFeatureReference[] allFeatures = selection.m_AllFeatures;
			for (int i = 0; i < allFeatures.Length; i++)
			{
				PatchClassIntoFeatureOfReferenceClass(allFeatures[i]?.Get(), myClass, referenceClass, mylevel, loopPrevention);
			}
		}

		private static void HandleComponent(BlueprintGuid featureGuid, BlueprintCharacterClassReference myClass, BlueprintCharacterClassReference referenceClass, int level, HashSet<SpellReference> mySpellSet, BlueprintComponent component, HashSet<BlueprintFact> loopPrevention)
		{
			int num = level + 1;
			if (num > 20)
			{
				Main.IsekaiContext.Logger.LogError("Attempt to patch Progression Tree stopped at Level 20 to prevent endless loop, if you see this message please report so we can figure out if someone created a loop here or if this limit needs to be higher");
			}
			else
			{
				if (component == null)
				{
					return;
				}
				try
				{
					if (component is AddKnownSpell { m_CharacterClass: not null } addKnownSpell && addKnownSpell.m_CharacterClass.Equals(referenceClass))
					{
						mySpellSet.Add(new SpellReference(addKnownSpell.SpellLevel, addKnownSpell.m_Spell));
					}
				}
				catch (NullReferenceException)
				{
					Main.IsekaiContext.Logger.LogError($"{featureGuid} component cast asSpell failed due to Nullpointer");
				}
				try
				{
					if (component is AddSpecialSpellList { m_CharacterClass: not null } addSpecialSpellList && addSpecialSpellList.m_CharacterClass.Equals(referenceClass) && addSpecialSpellList.SpellList?.SpellsByLevel != null)
					{
						SpellLevelList[] spellsByLevel = addSpecialSpellList.SpellList.SpellsByLevel;
						foreach (SpellLevelList spellLevelList in spellsByLevel)
						{
							if (spellLevelList?.m_Spells == null)
							{
								continue;
							}
							foreach (BlueprintAbilityReference spell in spellLevelList.m_Spells)
							{
								if (spell != null)
								{
									mySpellSet.Add(new SpellReference(spellLevelList.SpellLevel, spell));
								}
							}
						}
					}
				}
				catch (Exception ex2)
				{
					Main.IsekaiContext.Logger.Log($"component cast AddSpecialSpellList notice on {featureGuid}: {ex2.Message}");
				}
				if (component is AddAbilityUseTrigger { m_Spellbooks: not null } addAbilityUseTrigger && addAbilityUseTrigger.m_Spellbooks.Length != 0)
				{
					addAbilityUseTrigger.m_Spellbooks = addAbilityUseTrigger.m_Spellbooks.AddRangeToArray(patchableSpellBooks);
				}
				if (component is AddCasterLevelForSpellbook { m_Spellbooks: not null } addCasterLevelForSpellbook && addCasterLevelForSpellbook.m_Spellbooks.Length != 0)
				{
					addCasterLevelForSpellbook.m_Spellbooks = addCasterLevelForSpellbook.m_Spellbooks.AddRangeToArray(patchableSpellBooks);
				}
				if (component is IncreaseSpellSpellbookDC { m_Spellbooks: not null } increaseSpellSpellbookDC && increaseSpellSpellbookDC.m_Spellbooks.Length != 0)
				{
					increaseSpellSpellbookDC.m_Spellbooks = increaseSpellSpellbookDC.m_Spellbooks.AddRangeToArray(patchableSpellBooks);
				}
				if (component is AddFeatureOnClassLevel addFeatureOnClassLevel)
				{
					PatchClassIntoFeatureOfReferenceClass(addFeatureOnClassLevel.m_Feature.Get(), myClass, referenceClass, num, loopPrevention);
					if ((addFeatureOnClassLevel.m_Class != null && addFeatureOnClassLevel.m_Class.Equals(referenceClass)) || (addFeatureOnClassLevel.m_AdditionalClasses != null && addFeatureOnClassLevel.m_AdditionalClasses.Contains(referenceClass)))
					{
						AddFeatureOnClassLevel addFeatureOnClassLevel2 = addFeatureOnClassLevel;
						if (addFeatureOnClassLevel2.m_AdditionalClasses == null)
						{
							addFeatureOnClassLevel2.m_AdditionalClasses = new BlueprintCharacterClassReference[0];
						}
						if (!addFeatureOnClassLevel.m_AdditionalClasses.Contains(myClass))
						{
							addFeatureOnClassLevel.m_AdditionalClasses = addFeatureOnClassLevel.m_AdditionalClasses.AddToArray(myClass);
						}
					}
				}
				if (component is MonkNoArmorFeatureUnlock monkNoArmorFeatureUnlock)
				{
					BlueprintUnitFact blueprintUnitFact = monkNoArmorFeatureUnlock.m_NewFact.Get();
					if (blueprintUnitFact != null)
					{
						PatchClassIntoFeatureOfReferenceClass(blueprintUnitFact, myClass, referenceClass, num, loopPrevention);
					}
				}
				if (component is AddFeatureIfHasFact addFeatureIfHasFact)
				{
					PatchClassIntoFeatureOfReferenceClass((BlueprintUnitFact)addFeatureIfHasFact.m_Feature, myClass, referenceClass, num, loopPrevention);
				}
				try
				{
					if (component is AddFacts { m_Facts: not null, Facts: var facts })
					{
						foreach (BlueprintUnitFact item in facts)
						{
							if (item != null)
							{
								PatchClassIntoFeatureOfReferenceClass(item, myClass, referenceClass, num, loopPrevention);
							}
						}
					}
				}
				catch (Exception ex3)
				{
					Main.IsekaiContext.Logger.Log($"component cast AddFacts notice on {featureGuid}: {ex3.Message}");
				}
				if (!(component is AddAbilityResources { m_Resource: { } resource }))
				{
					return;
				}
				BlueprintAbilityResource blueprintAbilityResource = resource.Get();
				if (blueprintAbilityResource == null)
				{
					return;
				}
				bool flag = false;
				bool flag2 = false;
				if (blueprintAbilityResource.m_MaxAmount.m_ClassDiv != null && blueprintAbilityResource.m_MaxAmount.m_ClassDiv.Length != 0)
				{
					if (blueprintAbilityResource.m_MaxAmount.m_ClassDiv.Contains(referenceClass))
					{
						flag = true;
					}
					if (blueprintAbilityResource.m_MaxAmount.m_ClassDiv.Contains(myClass))
					{
						flag2 = true;
					}
					if (flag && !flag2)
					{
						blueprintAbilityResource.m_MaxAmount.m_ClassDiv = blueprintAbilityResource.m_MaxAmount.m_ClassDiv.AddToArray(myClass);
					}
				}
				if (!flag && blueprintAbilityResource.m_MaxAmount.m_Class != null && blueprintAbilityResource.m_MaxAmount.m_Class.Length != 0)
				{
					if (blueprintAbilityResource.m_MaxAmount.m_Class.Contains(referenceClass))
					{
						flag = true;
					}
					if (blueprintAbilityResource.m_MaxAmount.m_Class.Contains(myClass))
					{
						flag2 = true;
					}
					if (flag && !flag2)
					{
						blueprintAbilityResource.m_MaxAmount.m_Class = blueprintAbilityResource.m_MaxAmount.m_Class.AddToArray(myClass);
					}
				}
			}
		}

		internal static void PatchResource(BlueprintAbilityResource resource, BlueprintCharacterClassReference classRef)
		{
			if (resource != null)
			{
				if (resource.m_MaxAmount.m_Class != null && resource.m_MaxAmount.m_Class.Length != 0)
				{
					resource.m_MaxAmount.m_Class = resource.m_MaxAmount.m_Class.AppendToArray(classRef);
				}
				if (resource.m_MaxAmount.m_ClassDiv != null && resource.m_MaxAmount.m_ClassDiv.Length != 0)
				{
					resource.m_MaxAmount.m_ClassDiv = resource.m_MaxAmount.m_ClassDiv.AppendToArray(classRef);
				}
			}
		}

		internal static void PatchAbility(BlueprintAbility ability, BlueprintCharacterClassReference classRef)
		{
			if (((BlueprintScriptableObject)ability)?.Components == null)
			{
				return;
			}
			BlueprintComponent[] components = ((BlueprintScriptableObject)ability).Components;
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i] is ContextRankConfig { m_Class: not null } contextRankConfig && contextRankConfig.m_Class.Length != 0)
				{
					contextRankConfig.m_Class = contextRankConfig.m_Class.AppendToArray(classRef);
				}
			}
		}

		private static void PatchBuff(BlueprintBuff buff, BlueprintSpellbookReference spellbookRef)
		{
			if (((BlueprintScriptableObject)buff)?.Components == null)
			{
				return;
			}
			BlueprintComponent[] components = ((BlueprintScriptableObject)buff).Components;
			foreach (BlueprintComponent blueprintComponent in components)
			{
				if (blueprintComponent is AddAbilityUseTrigger addAbilityUseTrigger)
				{
					addAbilityUseTrigger.m_Spellbooks = addAbilityUseTrigger.m_Spellbooks.AppendToArray(spellbookRef);
				}
				else if (blueprintComponent is AddCasterLevelForSpellbook addCasterLevelForSpellbook)
				{
					addCasterLevelForSpellbook.m_Spellbooks = addCasterLevelForSpellbook.m_Spellbooks.AppendToArray(spellbookRef);
				}
				else if (blueprintComponent is IncreaseSpellSpellbookDC increaseSpellSpellbookDC)
				{
					increaseSpellSpellbookDC.m_Spellbooks = increaseSpellSpellbookDC.m_Spellbooks.AppendToArray(spellbookRef);
				}
			}
		}

		private static void PatchBuff(BlueprintBuff buff, BlueprintCharacterClassReference classRef)
		{
			if (((BlueprintScriptableObject)buff)?.Components == null)
			{
				return;
			}
			BlueprintComponent[] components = ((BlueprintScriptableObject)buff).Components;
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i] is ContextRankConfig { m_Class: not null } contextRankConfig && contextRankConfig.m_Class.Length != 0)
				{
					contextRankConfig.m_Class = contextRankConfig.m_Class.AppendToArray(classRef);
				}
			}
		}

		public static void PatchWearyingStrike()
		{
			BlueprintFeature blueprint = BlueprintTools.GetBlueprint<BlueprintFeature>("b4befb0a9b58e0e4687942661c55198d");
			if (blueprint == null)
			{
				return;
			}
			AddInitiatorAttackRollTrigger component = blueprint.GetComponent<AddInitiatorAttackRollTrigger>();
			if (component != null && component.Action != null)
			{
				ActionList originalActions = component.Action;
				component.Action = ActionFlow.DoSingle(delegate(Conditional c)
				{
					c.ConditionsChecker = ActionFlow.IfSingle<ContextConditionIsEnemy>();
					c.IfTrue = originalActions;
					c.IfFalse = ActionFlow.DoNothing();
				});
			}
		}
	}
}
