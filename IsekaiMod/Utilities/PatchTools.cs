using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities;

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
			if (!(p is SpellReference spellReference))
			{
				return false;
			}
			if ((object)this == spellReference)
			{
				return true;
			}
			if (value == null || spellReference.value == null)
			{
				return value == spellReference.value;
			}
			return value.Guid == spellReference.value.Guid;
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
			return value?.GetHashCode() ?? 0;
		}
	}

	internal static class ArcanistPatcher
	{
		public static void Patch(BlueprintCharacterClassReference classRef, params BlueprintSpellbookReference[] spellbookRefs)
		{
			PatchArcaneResrvoir(classRef, CollectIsekaiSpellbooks(spellbookRefs));
			PatchConsumeSpells(classRef);
			PatchArcanistExploits(classRef);
		}

		private static void PatchArcaneResrvoir(BlueprintCharacterClassReference classRef, BlueprintSpellbookReference[] spellbookRefs)
		{
			BlueprintAbilityResource blueprint = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("cac948cbbe79b55459459dd6a8fe44ce");
			PatchBuff(BlueprintTools.GetBlueprint<BlueprintBuff>("1dd776b7b27dcd54ab3cedbbaf440cf3"), classRef);
			PatchResource(blueprint, classRef);
			PatchArcaneReservoirBuffs(spellbookRefs);
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

		private static void PatchArcaneReservoirBuffs(BlueprintSpellbookReference[] spellbookRefs)
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
					foreach (BlueprintSpellbookReference spellbookRef in spellbookRefs)
					{
						PatchBuff(blueprintBuff, spellbookRef);
					}
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
				if (component?.m_Variants != null)
				{
					BlueprintAbilityReference[] variants = component.m_Variants;
					for (int j = 0; j < variants.Length; j++)
					{
						PatchBlastVariant(variants[j]?.Get(), classRef);
					}
				}
			}
		}

		internal static void PatchBlastVariant(BlueprintAbility ability, BlueprintCharacterClassReference classRef)
		{
			if (ability?.ComponentsArray == null || classRef == null)
			{
				return;
			}
			for (int i = 0; i < ability.ComponentsArray.Length; i++)
			{
				BlueprintComponent blueprintComponent = ability.ComponentsArray[i];
				if (blueprintComponent is ContextCalculateAbilityParamsBasedOnClasses contextCalculateAbilityParamsBasedOnClasses)
				{
					ContextCalculateAbilityParamsBasedOnClasses contextCalculateAbilityParamsBasedOnClasses2 = contextCalculateAbilityParamsBasedOnClasses;
					if (contextCalculateAbilityParamsBasedOnClasses2.m_CharacterClasses == null)
					{
						contextCalculateAbilityParamsBasedOnClasses2.m_CharacterClasses = new BlueprintCharacterClassReference[0];
					}
					if (!Enumerable.Contains(contextCalculateAbilityParamsBasedOnClasses.m_CharacterClasses, classRef))
					{
						contextCalculateAbilityParamsBasedOnClasses.m_CharacterClasses = contextCalculateAbilityParamsBasedOnClasses.m_CharacterClasses.AddToArray(classRef);
					}
				}
				else if (blueprintComponent is ContextCalculateAbilityParamsBasedOnClass contextCalculateAbilityParamsBasedOnClass)
				{
					ability.ComponentsArray[i] = new ContextCalculateAbilityParamsBasedOnClasses
					{
						m_CharacterClasses = new BlueprintCharacterClassReference[2] { contextCalculateAbilityParamsBasedOnClass.m_CharacterClass, classRef },
						StatType = contextCalculateAbilityParamsBasedOnClass.StatType,
						UseKineticistMainStat = contextCalculateAbilityParamsBasedOnClass.UseKineticistMainStat,
						name = contextCalculateAbilityParamsBasedOnClass.name
					};
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
					if (!Enumerable.Contains(component.m_AdditionalClasses, classRef))
					{
						component.m_AdditionalClasses = component.m_AdditionalClasses.AddToArray(classRef);
					}
					foreach (BlueprintCharacterClassReference item in ClassLevelContinuationsOf(classRef))
					{
						if (!Enumerable.Contains(component.m_AdditionalClasses, item))
						{
							component.m_AdditionalClasses = component.m_AdditionalClasses.AddToArray(item);
						}
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
					if (component2.m_BaseValueType == ContextRankBaseValueType.ClassLevel || component2.m_BaseValueType == ContextRankBaseValueType.SummClassLevelWithArchetype || component2.m_BaseValueType == ContextRankBaseValueType.MaxClassLevelWithArchetype)
					{
						if (component2.m_Class != null && !Enumerable.Contains(component2.m_Class, classRef))
						{
							component2.m_Class = component2.m_Class.AddToArray(classRef);
						}
						component2.m_Class = AppendClassOnce(component2.m_Class, classRef);
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
		internal const string TabletopTweaksBaseHarmonyId = "TabletopTweaks-Base";

		[HarmonyPrepare]
		private static bool Prepare()
		{
			return PatchGuard.Resolves(typeof(UnitPartMagusPatcher), typeof(UnitPartMagus), "get_Spellbook");
		}

		public static BlueprintSpellbook SelectMagusSpellbookOverride(BlueprintSpellbook vanillaSpellbook, BlueprintSpellbook isekaiSpellbook, Func<BlueprintSpellbook, bool> ownerHasSpellbook)
		{
			if (ownerHasSpellbook == null)
			{
				return null;
			}
			if (vanillaSpellbook != null && ownerHasSpellbook(vanillaSpellbook))
			{
				return null;
			}
			if (isekaiSpellbook == null)
			{
				return null;
			}
			if (!ownerHasSpellbook(isekaiSpellbook))
			{
				return null;
			}
			return isekaiSpellbook;
		}

		[HarmonyBefore(new string[] { "TabletopTweaks-Base" })]
		public static bool Prefix(UnitPartMagus __instance, ref Spellbook __result)
		{
			if (__instance == null)
			{
				return true;
			}
			if (__instance.m_Spellbook != null)
			{
				return true;
			}
			return DecideUncached(__instance, ref __result);
		}

		private static bool DecideUncached(UnitPartMagus __instance, ref Spellbook __result)
		{
			if (__instance.Owner?.Progression == null)
			{
				return true;
			}
			BlueprintCharacterClass blueprintCharacterClass = __instance.Class;
			BlueprintSpellbook vanillaSpellbook = ((blueprintCharacterClass == null) ? null : (__instance.Owner.Progression.GetClassData(blueprintCharacterClass)?.Spellbook ?? blueprintCharacterClass.Spellbook));
			BlueprintSpellbook isekaiSpellbook = __instance.Owner.Progression.GetClassData(IsekaiProtagonistClass.Get())?.Spellbook;
			BlueprintSpellbook blueprintSpellbook = SelectMagusSpellbookOverride(vanillaSpellbook, isekaiSpellbook, (BlueprintSpellbook book) => __instance.Owner.GetSpellbook(book) != null);
			if (blueprintSpellbook == null)
			{
				return true;
			}
			Spellbook spellbook = __instance.Owner.GetSpellbook(blueprintSpellbook);
			if (spellbook == null)
			{
				return true;
			}
			__result = spellbook;
			return false;
		}
	}

	internal static class MagusSpellbookTiming
	{
		internal const int ReportEvery = 2000;

		private static long _cachedHits;

		private static long _decisions;

		private static long _decisionTicks;

		private static long _maxDecisionTicks;

		private static long _windowStart;

		[System.Diagnostics.Conditional("DEBUG")]
		internal static void CountCachedHit()
		{
			_cachedHits++;
			MaybeReport();
		}

		[System.Diagnostics.Conditional("DEBUG")]
		internal static void RecordDecision(long elapsedTicks)
		{
			_decisions++;
			_decisionTicks += elapsedTicks;
			if (elapsedTicks > _maxDecisionTicks)
			{
				_maxDecisionTicks = elapsedTicks;
			}
			MaybeReport();
		}

		private static void MaybeReport()
		{
			long timestamp = Stopwatch.GetTimestamp();
			if (_windowStart == 0L)
			{
				_windowStart = timestamp;
			}
			if (_cachedHits + _decisions >= 2000)
			{
				_cachedHits = (_decisions = (_decisionTicks = (_maxDecisionTicks = 0L)));
				_windowStart = timestamp;
			}
		}

		internal static string Summarize(long cachedHits, long decisions, long decisionTicks, long maxDecisionTicks, long windowTicks, long ticksPerSecond)
		{
			double num = ((ticksPerSecond > 0) ? ((double)windowTicks / (double)ticksPerSecond) : 0.0);
			long num2 = cachedHits + decisions;
			double num3 = ((num > 0.0) ? ((double)num2 / num) : 0.0);
			double num4 = ((decisions > 0 && ticksPerSecond > 0) ? ((double)decisionTicks * 1000000.0 / (double)ticksPerSecond / (double)decisions) : 0.0);
			double num5 = ((ticksPerSecond > 0) ? ((double)maxDecisionTicks * 1000000.0 / (double)ticksPerSecond) : 0.0);
			return string.Format(CultureInfo.InvariantCulture, "[MagusSpellbookTiming] {0} calls in {1:F1} s ({2:F1}/s): {3} cached, {4} uncached decisions, avg {5:F2} us, max {6:F2} us", num2, num, num3, cachedHits, decisions, num4, num5);
		}
	}

	[HarmonyPatch(typeof(AddVendorDiscount), "OnTurnOn")]
	public static class AddVendorDiscountPatcher
	{
		[HarmonyPrepare]
		private static bool Prepare()
		{
			return PatchGuard.Resolves(typeof(AddVendorDiscountPatcher), typeof(AddVendorDiscount), "OnTurnOn");
		}

		public static void Prefix(AddVendorDiscount __instance)
		{
			((EntityFactComponentDelegate<UnitEntityData, EmptyComponentData>)__instance)?.OnInitialize();
		}
	}

	private static BlueprintSpellbookReference[] patchableSpellBooks = new BlueprintSpellbookReference[0];

	private static HashSet<BlueprintFact> s_FeaturesIgnoredWhenPatching;

	private static readonly HashSet<string> s_PatchedAreaEffects = new HashSet<string>(StringComparer.Ordinal);

	private static readonly Dictionary<BlueprintCharacterClassReference, List<BlueprintCharacterClassReference>> ClassLevelContinuations = new Dictionary<BlueprintCharacterClassReference, List<BlueprintCharacterClassReference>>();

	private static readonly HashSet<string> s_WalkedProgressions = new HashSet<string>(StringComparer.Ordinal);

	private static readonly HashSet<string> LargeSelectionsWorthWalking = new HashSet<string> { "60008a10ad7ad6543b1f63016741a5d2", "c074a5d615200494b8f2a9c845799d93", "4223fe18c75d4d14787af196a04e14e7", "28710502f46848d48b3f0d6132817c4e", "2476514e31791394fa140f1a07941c96", "9846043cf51251a4897728ed6e24e76f", "99999999000900000009000000000001", "58d6f8e9eea63f6418b107ce64f315ea", "5c883ae0cd6d7d5448b7a420f51f8459" };

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
		if (!Enumerable.Contains(patchableSpellBooks, blueprintSpellbookReference))
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

	internal static LevelEntry[] BuildRemovalEntries(BlueprintProgression referenceProgression, params BlueprintFeatureBase[] features)
	{
		LevelEntry[] array = new LevelEntry[0];
		if (referenceProgression?.LevelEntries == null || features == null)
		{
			return array;
		}
		LevelEntry[] levelEntries = referenceProgression.LevelEntries;
		foreach (LevelEntry levelEntry in levelEntries)
		{
			if (levelEntry?.m_Features == null)
			{
				continue;
			}
			BlueprintFeatureBase[] array2 = new BlueprintFeatureBase[0];
			foreach (BlueprintFeatureBase blueprintFeatureBase in features)
			{
				if (blueprintFeatureBase != null)
				{
					BlueprintFeatureBaseReference item = blueprintFeatureBase.ToReference<BlueprintFeatureBaseReference>();
					if (levelEntry.m_Features.Contains(item) && !Enumerable.Contains(array2, blueprintFeatureBase))
					{
						array2 = array2.AddToArray(blueprintFeatureBase);
					}
				}
			}
			if (array2.Length != 0)
			{
				array = array.AddToArray(Helpers.CreateLevelEntry(levelEntry.Level, array2));
			}
		}
		return array;
	}

	internal static LevelEntry[] BuildReplacementEntries(LevelEntry[] removals, BlueprintFeatureBase replacement)
	{
		LevelEntry[] array = new LevelEntry[0];
		if (removals == null || replacement == null)
		{
			return array;
		}
		foreach (LevelEntry removal in removals)
		{
			if (removal != null && !array.Any((LevelEntry e) => e.Level == removal.Level))
			{
				array = array.AddToArray(Helpers.CreateLevelEntry(removal.Level, replacement));
			}
		}
		return array;
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
				if (!Enumerable.Contains(array6, blueprintFeatureBaseReference))
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
					if (!Enumerable.Contains(array2, blueprintFeatureBaseReference2))
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
						if (!Enumerable.Contains(array2, feature))
						{
							array2 = array2.AddToArray(feature);
						}
					}
				}
			}
			prog.LevelEntries = prog.LevelEntries.AddToArray(Helpers.CreateLevelEntry(levelEntry.Level, array4));
		}
		array2 = AppendLevelsMissingFromProgression(prog, refArchetype.AddFeatures, array2);
		array2 = AppendLevelsMissingFromProgression(prog, additionalReference, array2);
		if (array2.Length != 0)
		{
			prog.UIGroups = prog.UIGroups.AddToArray(Helpers.CreateUIGroup(array2));
		}
		return prog;
	}

	internal static BlueprintFeatureBase[] AppendLevelsMissingFromProgression(BlueprintProgression prog, LevelEntry[] source, BlueprintFeatureBase[] missingUIGroup)
	{
		if (missingUIGroup == null)
		{
			missingUIGroup = new BlueprintFeatureBase[0];
		}
		if (prog == null || source == null)
		{
			return missingUIGroup;
		}
		if (prog.LevelEntries == null)
		{
			prog.LevelEntries = new LevelEntry[0];
		}
		foreach (LevelEntry levelEntry in source)
		{
			if (levelEntry?.m_Features == null)
			{
				continue;
			}
			bool flag = false;
			LevelEntry[] levelEntries = prog.LevelEntries;
			foreach (LevelEntry levelEntry2 in levelEntries)
			{
				if (levelEntry2 != null && levelEntry2.Level == levelEntry.Level)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				continue;
			}
			prog.LevelEntries = prog.LevelEntries.AddToArray(Helpers.CreateLevelEntry(levelEntry.Level, levelEntry.m_Features.ToArray()));
			foreach (BlueprintFeatureBaseReference feature in levelEntry.m_Features)
			{
				if (feature != null && !Enumerable.Contains(missingUIGroup, feature))
				{
					missingUIGroup = missingUIGroup.AddToArray(feature);
				}
			}
		}
		return missingUIGroup;
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

	internal static int DropEmptyFactEntries(AddFacts addFacts)
	{
		BlueprintUnitFactReference[] array = addFacts?.m_Facts;
		if (array == null || array.Length == 0)
		{
			return 0;
		}
		BlueprintUnitFactReference[] array2 = array.Where((BlueprintUnitFactReference f) => f != null && ((BlueprintReferenceBase)f).deserializedGuid != BlueprintGuid.Empty).ToArray();
		int num = array.Length - array2.Length;
		if (num > 0)
		{
			addFacts.m_Facts = array2;
		}
		return num;
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
				Main.IsekaiContext.Logger.LogError($"reference class={referenceClass.Guid} Stop Feature={feature.AssetGuid} name={feature.name}");
				{
					foreach (BlueprintFact item in loopPrevention)
					{
						Main.IsekaiContext.Logger.LogError($"guid={item.AssetGuid}");
					}
					return;
				}
			}
			Main.IsekaiContext.Logger.LogError($"reference class={referenceClass.Guid} Stop Feature={feature.AssetGuid}");
		}
		else
		{
			if (FeaturesIgnoredWhenPatching.Contains(feature) || loopPrevention.Contains(feature))
			{
				return;
			}
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
				BlueprintFact[] array = CollectNestedPatchTargets(feature);
				for (int i = 0; i < array.Length; i++)
				{
					PatchClassIntoFeatureOfReferenceClass(array[i], myClass, referenceClass, num, loopPrevention);
				}
				BlueprintAbilityAreaEffect[] array2 = CollectAreaEffects(feature);
				for (int i = 0; i < array2.Length; i++)
				{
					PatchAreaEffect(array2[i], myClass, referenceClass, num, loopPrevention);
				}
				PatchClassScaledComponents(feature, myClass, referenceClass);
				if (((BlueprintScriptableObject)feature).Components == null || ((BlueprintScriptableObject)feature).Components.Length == 0)
				{
					return;
				}
				HashSet<SpellReference> hashSet = new HashSet<SpellReference>();
				List<SpontaneousSpellConversion> list = new List<SpontaneousSpellConversion>();
				List<CannyDefensePermanent> list2 = new List<CannyDefensePermanent>();
				HashSet<SpellReference> hashSet2 = new HashSet<SpellReference>();
				bool flag = false;
				bool flag2 = false;
				BlueprintComponent[] components = ((BlueprintScriptableObject)feature).Components;
				foreach (BlueprintComponent blueprintComponent in components)
				{
					if (blueprintComponent is AddKnownSpell { m_Spell: not null, m_CharacterClass: not null } addKnownSpell && addKnownSpell.m_CharacterClass.Equals(myClass))
					{
						hashSet2.Add(new SpellReference(addKnownSpell.SpellLevel, addKnownSpell.m_Spell));
					}
					else if (blueprintComponent is SpontaneousSpellConversion { m_CharacterClass: not null } spontaneousSpellConversion && spontaneousSpellConversion.m_CharacterClass.Equals(myClass))
					{
						flag = true;
					}
					else if (blueprintComponent is CannyDefensePermanent { m_CharacterClass: not null } cannyDefensePermanent && cannyDefensePermanent.m_CharacterClass.Equals(myClass))
					{
						flag2 = true;
					}
				}
				components = ((BlueprintScriptableObject)feature).Components;
				foreach (BlueprintComponent blueprintComponent2 in components)
				{
					if (blueprintComponent2 != null)
					{
						HandleComponent(feature.AssetGuid, myClass, referenceClass, num, hashSet, blueprintComponent2, loopPrevention);
						if (blueprintComponent2 is ContextRankConfig rankConfig)
						{
							PatchClassIntoRankConfig(rankConfig, myClass, referenceClass);
						}
						else if (blueprintComponent2 is SpontaneousSpellConversion { m_CharacterClass: not null } spontaneousSpellConversion2 && spontaneousSpellConversion2.m_CharacterClass.Equals(referenceClass))
						{
							list.Add(spontaneousSpellConversion2);
						}
						else if (blueprintComponent2 is CannyDefensePermanent { m_CharacterClass: not null } cannyDefensePermanent2 && cannyDefensePermanent2.m_CharacterClass.Equals(referenceClass))
						{
							list2.Add(cannyDefensePermanent2);
						}
					}
				}
				hashSet.ExceptWith(hashSet2);
				foreach (SpellReference item2 in hashSet)
				{
					AddKnownSpellUnlessDlcRestricted.AddTo(feature, myClass, item2.level, item2.value);
				}
				if (!flag)
				{
					foreach (SpontaneousSpellConversion conversion in list)
					{
						feature.AddComponent(delegate(SpontaneousSpellConversion c)
						{
							c.m_CharacterClass = myClass;
							c.m_SpellsByLevel = conversion.m_SpellsByLevel;
						});
					}
				}
				if (flag2)
				{
					return;
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
			}
			catch (Exception ex)
			{
				if (feature.name != null)
				{
					Main.IsekaiContext.Logger.LogError($"Unpatchable Feature={feature.AssetGuid} name={feature.name} at level={num} error={ex.GetType().Name} reason={ex.Message}");
				}
				else
				{
					Main.IsekaiContext.Logger.LogError($"Unpatchable Feature={feature.AssetGuid} at level={num} error={ex.GetType().Name} reason={ex.Message}");
				}
			}
		}
	}

	internal static BlueprintFact[] CollectNestedPatchTargets(BlueprintFact feature)
	{
		if (feature == null)
		{
			return new BlueprintFact[0];
		}
		List<BlueprintFact> list = new List<BlueprintFact>();
		if (feature is BlueprintActivatableAbility blueprintActivatableAbility)
		{
			BlueprintBuff blueprintBuff = blueprintActivatableAbility.m_Buff?.Get();
			if (blueprintBuff != null)
			{
				list.Add(blueprintBuff);
			}
		}
		if (((BlueprintScriptableObject)feature).Components != null)
		{
			BlueprintComponent[] components = ((BlueprintScriptableObject)feature).Components;
			foreach (BlueprintComponent blueprintComponent in components)
			{
				if (blueprintComponent is AbilityVariants { m_Variants: not null, m_Variants: var variants })
				{
					for (int j = 0; j < variants.Length; j++)
					{
						BlueprintAbility blueprintAbility = variants[j]?.Get();
						if (blueprintAbility != null)
						{
							list.Add(blueprintAbility);
						}
					}
				}
				if (blueprintComponent is FactSinglify { m_NewFacts: not null, m_NewFacts: var newFacts })
				{
					for (int j = 0; j < newFacts.Length; j++)
					{
						BlueprintUnitFact blueprintUnitFact = newFacts[j]?.Get();
						if (blueprintUnitFact != null)
						{
							list.Add(blueprintUnitFact);
						}
					}
				}
				foreach (BlueprintBuff item in ActionTargets.BuffsIn(blueprintComponent))
				{
					if (item != null && !list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
		}
		return list.ToArray();
	}

	internal static BlueprintAbilityAreaEffect[] CollectAreaEffects(SimpleBlueprint blueprint)
	{
		List<BlueprintAbilityAreaEffect> list = new List<BlueprintAbilityAreaEffect>();
		BlueprintComponent[] array = (blueprint as BlueprintScriptableObject)?.ComponentsArray;
		if (array == null)
		{
			return list.ToArray();
		}
		BlueprintComponent[] array2 = array;
		foreach (BlueprintComponent obj in array2)
		{
			if (obj is AddAreaEffect addAreaEffect)
			{
				BlueprintAbilityAreaEffect blueprintAbilityAreaEffect = addAreaEffect.m_AreaEffect?.Get();
				if (blueprintAbilityAreaEffect != null && !list.Contains(blueprintAbilityAreaEffect))
				{
					list.Add(blueprintAbilityAreaEffect);
				}
			}
			foreach (BlueprintAbilityAreaEffect item in ActionTargets.AreaEffectsIn(obj))
			{
				if (item != null && !list.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		return list.ToArray();
	}

	private static void PatchAreaEffect(BlueprintAbilityAreaEffect area, BlueprintCharacterClassReference myClass, BlueprintCharacterClassReference referenceClass, int level, HashSet<BlueprintFact> loopPrevention)
	{
		if (area?.ComponentsArray == null)
		{
			return;
		}
		HashSet<string> hashSet = s_PatchedAreaEffects;
		string[] array = new string[5];
		BlueprintGuid assetGuid = area.AssetGuid;
		array[0] = assetGuid.ToString();
		array[1] = "|";
		array[2] = myClass.Guid.ToString();
		array[3] = "|";
		array[4] = referenceClass.Guid.ToString();
		if (!hashSet.Add(string.Concat(array)))
		{
			return;
		}
		BlueprintComponent[] componentsArray = area.ComponentsArray;
		foreach (BlueprintComponent obj in componentsArray)
		{
			if (obj is ContextRankConfig rankConfig)
			{
				PatchClassIntoRankConfig(rankConfig, myClass, referenceClass);
			}
			if (obj is AbilityAreaEffectBuff abilityAreaEffectBuff)
			{
				BlueprintBuff blueprintBuff = abilityAreaEffectBuff.m_Buff?.Get();
				if (blueprintBuff != null)
				{
					PatchClassIntoFeatureOfReferenceClass(blueprintBuff, myClass, referenceClass, level, loopPrevention);
				}
			}
			foreach (BlueprintBuff item in ActionTargets.BuffsIn(obj))
			{
				PatchClassIntoFeatureOfReferenceClass(item, myClass, referenceClass, level, loopPrevention);
			}
		}
		PatchClassScaledComponents(area, myClass, referenceClass);
	}

	private static List<BlueprintCharacterClassReference> WithContinuations(BlueprintCharacterClassReference myClass)
	{
		List<BlueprintCharacterClassReference> list = new List<BlueprintCharacterClassReference> { myClass };
		foreach (BlueprintCharacterClassReference item in ClassLevelContinuationsOf(myClass))
		{
			if (item != null && !list.Contains(item))
			{
				list.Add(item);
			}
		}
		return list;
	}

	internal static void PatchClassScaledComponents(BlueprintScriptableObject blueprint, BlueprintCharacterClassReference myClass, BlueprintCharacterClassReference referenceClass)
	{
		if (blueprint?.ComponentsArray == null || myClass == null || referenceClass == null)
		{
			return;
		}
		List<BlueprintCharacterClassReference> list = WithContinuations(myClass);
		List<BlueprintComponent> list2 = new List<BlueprintComponent>();
		for (int i = 0; i < blueprint.ComponentsArray.Length; i++)
		{
			BlueprintComponent blueprintComponent = blueprint.ComponentsArray[i];
			if (blueprintComponent is ContextCalculateAbilityParamsBasedOnClass { m_CharacterClass: not null } contextCalculateAbilityParamsBasedOnClass && contextCalculateAbilityParamsBasedOnClass.m_CharacterClass.Equals(referenceClass))
			{
				ContextCalculateAbilityParamsBasedOnClasses contextCalculateAbilityParamsBasedOnClasses = new ContextCalculateAbilityParamsBasedOnClasses();
				contextCalculateAbilityParamsBasedOnClasses.m_CharacterClasses = new BlueprintCharacterClassReference[1] { contextCalculateAbilityParamsBasedOnClass.m_CharacterClass }.Concat(list).ToArray();
				contextCalculateAbilityParamsBasedOnClasses.StatType = contextCalculateAbilityParamsBasedOnClass.StatType;
				contextCalculateAbilityParamsBasedOnClasses.UseKineticistMainStat = contextCalculateAbilityParamsBasedOnClass.UseKineticistMainStat;
				contextCalculateAbilityParamsBasedOnClasses.name = contextCalculateAbilityParamsBasedOnClass.name;
				ContextCalculateAbilityParamsBasedOnClasses contextCalculateAbilityParamsBasedOnClasses2 = contextCalculateAbilityParamsBasedOnClasses;
				contextCalculateAbilityParamsBasedOnClasses2.OwnerBlueprint = blueprint;
				blueprint.ComponentsArray[i] = contextCalculateAbilityParamsBasedOnClasses2;
				continue;
			}
			if (blueprintComponent is ContextCalculateAbilityParamsBasedOnClasses { m_CharacterClasses: not null } contextCalculateAbilityParamsBasedOnClasses3 && Enumerable.Contains(contextCalculateAbilityParamsBasedOnClasses3.m_CharacterClasses, referenceClass))
			{
				foreach (BlueprintCharacterClassReference item in list)
				{
					if (!Enumerable.Contains(contextCalculateAbilityParamsBasedOnClasses3.m_CharacterClasses, item))
					{
						contextCalculateAbilityParamsBasedOnClasses3.m_CharacterClasses = contextCalculateAbilityParamsBasedOnClasses3.m_CharacterClasses.AddToArray(item);
					}
				}
				continue;
			}
			if (blueprintComponent is ReplaceCasterLevelOfAbility { m_Class: not null } replaceCasterLevelOfAbility)
			{
				if (!replaceCasterLevelOfAbility.m_Class.Equals(referenceClass))
				{
					BlueprintCharacterClassReference[] additionalClasses = replaceCasterLevelOfAbility.m_AdditionalClasses;
					if (additionalClasses == null || !Enumerable.Contains(additionalClasses, referenceClass))
					{
						goto IL_01fd;
					}
				}
				ReplaceCasterLevelOfAbility replaceCasterLevelOfAbility2 = replaceCasterLevelOfAbility;
				if (replaceCasterLevelOfAbility2.m_AdditionalClasses == null)
				{
					replaceCasterLevelOfAbility2.m_AdditionalClasses = new BlueprintCharacterClassReference[0];
				}
				foreach (BlueprintCharacterClassReference item2 in list)
				{
					if (!replaceCasterLevelOfAbility.m_Class.Equals(item2) && !Enumerable.Contains(replaceCasterLevelOfAbility.m_AdditionalClasses, item2))
					{
						replaceCasterLevelOfAbility.m_AdditionalClasses = replaceCasterLevelOfAbility.m_AdditionalClasses.AddToArray(item2);
					}
				}
				continue;
			}
			goto IL_01fd;
			IL_01fd:
			IncreaseResourcesByClass increase = blueprintComponent as IncreaseResourcesByClass;
			if (increase == null || increase.m_CharacterClass == null || !increase.m_CharacterClass.Equals(referenceClass) || increase.m_Resource == null)
			{
				continue;
			}
			foreach (BlueprintCharacterClassReference c in list)
			{
				if (!blueprint.ComponentsArray.Concat(list2).OfType<IncreaseResourcesByClass>().Any((IncreaseResourcesByClass other) => other.m_CharacterClass != null && other.m_CharacterClass.Equals(c) && other.m_Resource != null && other.m_Resource.Equals(increase.m_Resource)))
				{
					list2.Add(new IncreaseResourcesByClass
					{
						m_Resource = increase.m_Resource,
						m_CharacterClass = c,
						Stat = StatType.Unknown,
						BaseValue = 0,
						name = (increase.name ?? "$IncreaseResourcesByClass") + "$Isekai" + list2.Count
					});
				}
			}
		}
		foreach (BlueprintComponent item3 in list2)
		{
			item3.OwnerBlueprint = blueprint;
			blueprint.ComponentsArray = blueprint.ComponentsArray.AppendToArray(item3);
		}
	}

	internal static bool PatchClassIntoBindAbilitiesToClass(BindAbilitiesToClass bind, BlueprintCharacterClassReference myClass, BlueprintCharacterClassReference referenceClass, BlueprintArchetypeReference[] referenceClassArchetypes)
	{
		if (bind == null || myClass == null || referenceClass == null)
		{
			return false;
		}
		if (bind.m_CharacterClass == null || bind.m_CharacterClass.Guid == BlueprintGuid.Empty)
		{
			return false;
		}
		if (!bind.m_CharacterClass.Equals(referenceClass) && (bind.m_AdditionalClasses == null || !Enumerable.Contains(bind.m_AdditionalClasses, referenceClass)))
		{
			return false;
		}
		if (bind.m_CharacterClass.Equals(myClass) || (bind.m_AdditionalClasses != null && Enumerable.Contains(bind.m_AdditionalClasses, myClass)))
		{
			return false;
		}
		if (bind.m_Archetypes != null && bind.m_Archetypes.Length != 0 && referenceClassArchetypes != null && bind.m_Archetypes.All((BlueprintArchetypeReference a) => a != null && Enumerable.Contains(referenceClassArchetypes, a)))
		{
			return false;
		}
		bind.m_AdditionalClasses = (bind.m_AdditionalClasses ?? new BlueprintCharacterClassReference[0]).AddToArray(myClass);
		foreach (BlueprintCharacterClassReference item in ClassLevelContinuationsOf(myClass))
		{
			if (!Enumerable.Contains(bind.m_AdditionalClasses, item))
			{
				bind.m_AdditionalClasses = bind.m_AdditionalClasses.AddToArray(item);
			}
		}
		return true;
	}

	internal static void PatchClassIntoRankConfig(ContextRankConfig rankConfig, BlueprintCharacterClassReference myClass, BlueprintCharacterClassReference referenceClass)
	{
		if (rankConfig == null || myClass == null || referenceClass == null || (rankConfig.m_BaseValueType != ContextRankBaseValueType.ClassLevel && rankConfig.m_BaseValueType != ContextRankBaseValueType.SummClassLevelWithArchetype && rankConfig.m_BaseValueType != ContextRankBaseValueType.OwnerSummClassLevelWithArchetype && rankConfig.m_BaseValueType != ContextRankBaseValueType.MaxClassLevelWithArchetype) || rankConfig.m_Class == null || rankConfig.m_ExceptClasses)
		{
			return;
		}
		if (!Enumerable.Contains(rankConfig.m_Class, myClass))
		{
			if (!Enumerable.Contains(rankConfig.m_Class, referenceClass))
			{
				return;
			}
			rankConfig.m_Class = rankConfig.m_Class.AddToArray(myClass);
		}
		foreach (BlueprintCharacterClassReference item in ClassLevelContinuationsOf(myClass))
		{
			if (!Enumerable.Contains(rankConfig.m_Class, item))
			{
				rankConfig.m_Class = rankConfig.m_Class.AddToArray(item);
			}
		}
	}

	public static void RegisterClassLevelContinuation(BlueprintCharacterClassReference baseClass, BlueprintCharacterClassReference continuation)
	{
		if (baseClass != null && continuation != null && !baseClass.Equals(continuation))
		{
			if (!ClassLevelContinuations.TryGetValue(baseClass, out var value))
			{
				value = new List<BlueprintCharacterClassReference>();
				ClassLevelContinuations[baseClass] = value;
			}
			if (!value.Contains(continuation))
			{
				value.Add(continuation);
			}
		}
	}

	internal static IEnumerable<BlueprintCharacterClassReference> ClassLevelContinuationsOf(BlueprintCharacterClassReference baseClass)
	{
		if (baseClass != null && ClassLevelContinuations.TryGetValue(baseClass, out var value))
		{
			return value;
		}
		return Enumerable.Empty<BlueprintCharacterClassReference>();
	}

	private static void PatchClassProgression(BlueprintProgression progression, BlueprintCharacterClassReference myClass, BlueprintCharacterClassReference referenceClass, int mylevel, HashSet<BlueprintFact> loopPrevention)
	{
		LegacyProgressionBackfill.Register(progression);
		if (progression.m_Classes != null && progression.m_Classes.Length != 0)
		{
			bool flag = false;
			BlueprintProgression.ClassWithLevel[] classes = progression.m_Classes;
			foreach (BlueprintProgression.ClassWithLevel classWithLevel in classes)
			{
				if (classWithLevel != null && myClass.Equals(classWithLevel.m_Class))
				{
					flag = true;
					break;
				}
			}
			HashSet<string> hashSet = s_WalkedProgressions;
			string[] array = new string[5];
			BlueprintGuid assetGuid = progression.AssetGuid;
			array[0] = assetGuid.ToString();
			array[1] = "|";
			array[2] = myClass.Guid.ToString();
			array[3] = "|";
			array[4] = referenceClass.Guid.ToString();
			if (!hashSet.Add(string.Concat(array)))
			{
				return;
			}
			if (!flag)
			{
				progression.AddClass(myClass);
			}
		}
		HashSet<BlueprintFeatureBase> hashSet2 = new HashSet<BlueprintFeatureBase>();
		LevelEntry[] levelEntries = progression.LevelEntries;
		for (int i = 0; i < levelEntries.Length; i++)
		{
			foreach (BlueprintFeatureBase feature in levelEntries[i].Features)
			{
				if (feature != null && !hashSet2.Contains(feature))
				{
					hashSet2.Add(feature);
				}
			}
		}
		foreach (BlueprintFeatureBase item in hashSet2)
		{
			PatchClassIntoFeatureOfReferenceClass(item, myClass, referenceClass, mylevel, loopPrevention);
		}
	}

	internal static bool IsSelectionWorthWalking(BlueprintFeatureSelection selection, int depth)
	{
		if (selection?.m_AllFeatures == null)
		{
			return false;
		}
		if (selection.m_AllFeatures.Length <= 30)
		{
			return true;
		}
		if (depth <= 1)
		{
			return true;
		}
		return LargeSelectionsWorthWalking.Contains(selection.AssetGuid.ToString());
	}

	private static void PatchClassSelection(BlueprintFeatureSelection selection, BlueprintCharacterClassReference myClass, BlueprintCharacterClassReference referenceClass, int mylevel, HashSet<BlueprintFact> loopPrevention)
	{
		if (selection == null || selection.m_AllFeatures == null)
		{
			return;
		}
		if (!IsSelectionWorthWalking(selection, mylevel))
		{
			Main.IsekaiContext.Logger.Log($"reference class={referenceClass.Guid} Stop Feature={selection.AssetGuid} name={selection.name} reason=selection contains too many features and thus likely is a basic feat variation");
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
				if (component is AddSpecialSpellList addSpecialSpellList && addSpecialSpellList.m_CharacterClass.Equals(referenceClass))
				{
					SpellLevelList[] spellsByLevel = addSpecialSpellList.SpellList.SpellsByLevel;
					foreach (SpellLevelList spellLevelList in spellsByLevel)
					{
						foreach (BlueprintAbilityReference spell in spellLevelList.m_Spells)
						{
							mySpellSet.Add(new SpellReference(spellLevelList.SpellLevel, spell));
						}
					}
				}
			}
			catch (NullReferenceException)
			{
				Main.IsekaiContext.Logger.LogError($"{featureGuid} component cast AddSpecialSpellList failed due to Nullpointer");
			}
			BlueprintSpellbookReference referenceSpellbook = referenceClass?.Get()?.m_Spellbook;
			if (component is AddAbilityUseTrigger addAbilityUseTrigger)
			{
				addAbilityUseTrigger.m_Spellbooks = MergeIsekaiSpellbooks(addAbilityUseTrigger.m_Spellbooks, referenceSpellbook, patchableSpellBooks);
			}
			if (component is AddCasterLevelForSpellbook addCasterLevelForSpellbook)
			{
				addCasterLevelForSpellbook.m_Spellbooks = MergeIsekaiSpellbooks(addCasterLevelForSpellbook.m_Spellbooks, referenceSpellbook, patchableSpellBooks);
			}
			if (component is IncreaseSpellSpellbookDC increaseSpellSpellbookDC)
			{
				increaseSpellSpellbookDC.m_Spellbooks = MergeIsekaiSpellbooks(increaseSpellSpellbookDC.m_Spellbooks, referenceSpellbook, patchableSpellBooks);
			}
			if (component is AddFeatureOnClassLevel addFeatureOnClassLevel)
			{
				PatchClassIntoFeatureOfReferenceClass(addFeatureOnClassLevel.m_Feature.Get(), myClass, referenceClass, num, loopPrevention);
				if ((addFeatureOnClassLevel.m_Class != null && addFeatureOnClassLevel.m_Class.Equals(referenceClass)) || (addFeatureOnClassLevel.m_AdditionalClasses != null && Enumerable.Contains(addFeatureOnClassLevel.m_AdditionalClasses, referenceClass)))
				{
					AddFeatureOnClassLevel addFeatureOnClassLevel2 = addFeatureOnClassLevel;
					if (addFeatureOnClassLevel2.m_AdditionalClasses == null)
					{
						addFeatureOnClassLevel2.m_AdditionalClasses = new BlueprintCharacterClassReference[0];
					}
					if (!Enumerable.Contains(addFeatureOnClassLevel.m_AdditionalClasses, myClass))
					{
						addFeatureOnClassLevel.m_AdditionalClasses = addFeatureOnClassLevel.m_AdditionalClasses.AddToArray(myClass);
					}
					foreach (BlueprintCharacterClassReference item in ClassLevelContinuationsOf(myClass))
					{
						if (!Enumerable.Contains(addFeatureOnClassLevel.m_AdditionalClasses, item))
						{
							addFeatureOnClassLevel.m_AdditionalClasses = addFeatureOnClassLevel.m_AdditionalClasses.AddToArray(item);
						}
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
			if (component is BindAbilitiesToClass bind)
			{
				PatchClassIntoBindAbilitiesToClass(bind, myClass, referenceClass, referenceClass.Get()?.m_Archetypes);
			}
			try
			{
				if (component is AddFacts { Facts: var facts } addFacts)
				{
					foreach (BlueprintUnitFact item2 in facts)
					{
						if (item2 != null)
						{
							PatchClassIntoFeatureOfReferenceClass(item2, myClass, referenceClass, num, loopPrevention);
						}
					}
					int num2 = DropEmptyFactEntries(addFacts);
					if (num2 > 0)
					{
						Main.IsekaiContext.Logger.Log($"{featureGuid} AddFacts had {num2} entry(ies) with the empty id (a reference to a blueprint that was never created); removed so the feature no longer throws when gained");
					}
				}
			}
			catch (NullReferenceException)
			{
				Main.IsekaiContext.Logger.LogError($"{featureGuid} component cast AddFacts failed due to Nullpointer");
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
				if (Enumerable.Contains(blueprintAbilityResource.m_MaxAmount.m_ClassDiv, referenceClass))
				{
					flag = true;
				}
				if (Enumerable.Contains(blueprintAbilityResource.m_MaxAmount.m_ClassDiv, myClass))
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
				if (Enumerable.Contains(blueprintAbilityResource.m_MaxAmount.m_Class, referenceClass))
				{
					flag = true;
				}
				if (Enumerable.Contains(blueprintAbilityResource.m_MaxAmount.m_Class, myClass))
				{
					flag2 = true;
				}
				if (flag && !flag2)
				{
					blueprintAbilityResource.m_MaxAmount.m_Class = blueprintAbilityResource.m_MaxAmount.m_Class.AddToArray(myClass);
				}
			}
			if (!flag)
			{
				return;
			}
			foreach (BlueprintCharacterClassReference item3 in ClassLevelContinuationsOf(myClass))
			{
				if (blueprintAbilityResource.m_MaxAmount.m_ClassDiv != null && Enumerable.Contains(blueprintAbilityResource.m_MaxAmount.m_ClassDiv, myClass))
				{
					blueprintAbilityResource.m_MaxAmount.m_ClassDiv = AppendClassOnce(blueprintAbilityResource.m_MaxAmount.m_ClassDiv, item3);
				}
				if (blueprintAbilityResource.m_MaxAmount.m_Class != null && Enumerable.Contains(blueprintAbilityResource.m_MaxAmount.m_Class, myClass))
				{
					blueprintAbilityResource.m_MaxAmount.m_Class = AppendClassOnce(blueprintAbilityResource.m_MaxAmount.m_Class, item3);
				}
			}
		}
	}

	private static BlueprintCharacterClassReference[] AppendClassOnce(BlueprintCharacterClassReference[] classes, BlueprintCharacterClassReference classRef)
	{
		if (classes == null || classes.Length == 0 || classRef == null)
		{
			return classes;
		}
		if (!Enumerable.Contains(classes, classRef))
		{
			classes = classes.AppendToArray(classRef);
		}
		foreach (BlueprintCharacterClassReference item in ClassLevelContinuationsOf(classRef))
		{
			if (!Enumerable.Contains(classes, item))
			{
				classes = classes.AppendToArray(item);
			}
		}
		return classes;
	}

	internal static BlueprintSpellbookReference[] MergeIsekaiSpellbooks(BlueprintSpellbookReference[] scopedSpellbooks, BlueprintSpellbookReference referenceSpellbook, BlueprintSpellbookReference[] isekaiSpellbooks)
	{
		if (scopedSpellbooks == null || scopedSpellbooks.Length == 0)
		{
			return scopedSpellbooks;
		}
		if (referenceSpellbook == null || isekaiSpellbooks == null)
		{
			return scopedSpellbooks;
		}
		if (!Enumerable.Contains(scopedSpellbooks, referenceSpellbook))
		{
			return scopedSpellbooks;
		}
		foreach (BlueprintSpellbookReference spellbookRef in isekaiSpellbooks)
		{
			scopedSpellbooks = AppendSpellbookOnce(scopedSpellbooks, spellbookRef);
		}
		return scopedSpellbooks;
	}

	private static BlueprintSpellbookReference[] AppendSpellbookOnce(BlueprintSpellbookReference[] spellbooks, BlueprintSpellbookReference spellbookRef)
	{
		if (spellbookRef == null)
		{
			return spellbooks;
		}
		if (spellbooks == null)
		{
			spellbooks = new BlueprintSpellbookReference[0];
		}
		if (Enumerable.Contains(spellbooks, spellbookRef))
		{
			return spellbooks;
		}
		return spellbooks.AppendToArray(spellbookRef);
	}

	internal static void PatchResource(BlueprintAbilityResource resource, BlueprintCharacterClassReference classRef)
	{
		if (resource != null)
		{
			resource.m_MaxAmount.m_Class = AppendClassOnce(resource.m_MaxAmount.m_Class, classRef);
			resource.m_MaxAmount.m_ClassDiv = AppendClassOnce(resource.m_MaxAmount.m_ClassDiv, classRef);
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
			if (components[i] is ContextRankConfig contextRankConfig)
			{
				contextRankConfig.m_Class = AppendClassOnce(contextRankConfig.m_Class, classRef);
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
				addAbilityUseTrigger.m_Spellbooks = AppendSpellbookOnce(addAbilityUseTrigger.m_Spellbooks, spellbookRef);
			}
			else if (blueprintComponent is AddCasterLevelForSpellbook addCasterLevelForSpellbook)
			{
				addCasterLevelForSpellbook.m_Spellbooks = AppendSpellbookOnce(addCasterLevelForSpellbook.m_Spellbooks, spellbookRef);
			}
			else if (blueprintComponent is IncreaseSpellSpellbookDC increaseSpellSpellbookDC)
			{
				increaseSpellSpellbookDC.m_Spellbooks = AppendSpellbookOnce(increaseSpellSpellbookDC.m_Spellbooks, spellbookRef);
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
			if (components[i] is ContextRankConfig contextRankConfig)
			{
				contextRankConfig.m_Class = AppendClassOnce(contextRankConfig.m_Class, classRef);
			}
		}
	}

	internal static BlueprintSpellbookReference[] CollectIsekaiSpellbooks(params BlueprintSpellbookReference[] extra)
	{
		return MergeSpellbookLists(patchableSpellBooks, extra);
	}

	internal static BlueprintSpellbookReference[] MergeSpellbookLists(BlueprintSpellbookReference[] registered, BlueprintSpellbookReference[] extra)
	{
		BlueprintSpellbookReference[] array = registered ?? new BlueprintSpellbookReference[0];
		if (extra != null)
		{
			foreach (BlueprintSpellbookReference spellbookRef in extra)
			{
				array = AppendSpellbookOnce(array, spellbookRef);
			}
		}
		return array;
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
