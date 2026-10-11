using IsekaiMod.Content.Features.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Guardians;

internal class TwinAvatarCompanionUnit
{
	private static readonly BlueprintUnit NeutralHumanoidTemplate = BlueprintTools.GetBlueprint<BlueprintUnit>("00f6e955cf970cb449958bd95d3ad2b7") ?? BlueprintTools.GetBlueprint<BlueprintUnit>("b3bc5994e94534d4c86613e649fe1913") ?? BlueprintTools.GetBlueprint<BlueprintUnit>("8f3bd0ecea704277a9f2b09296a7b01e");

	private static readonly BlueprintFaction Neutrals = BlueprintTools.GetBlueprint<BlueprintFaction>("d8de50cc80eb4dc409a983991e0b77ad");

	private static readonly Sprite Icon_Pet = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

	private static readonly BlueprintFeature SimpleWeaponProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("e70ecf1ed95ca2f40b754f1adb22bbdd");

	private static readonly BlueprintFeature MartialWeaponProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("203992ef5b35c864390b4e4a1e200629");

	internal const Size TwinBaseSizeValue = Size.Medium;

	public static BlueprintProgression TwinProgression { get; private set; }

	internal static int[] MilestoneLevels => TwinAvatarShape.MilestoneLevels;

	public static void Add()
	{
		if (NeutralHumanoidTemplate == null)
		{
			Main.IsekaiContext.Logger.LogError("Could not find base template for TwinAvatarCompanionUnit");
			return;
		}
		BuildProgression();
		BlueprintFeature TwinArmor = GuardianCompanionClass.CreateNaturalArmor("TwinAvatarNaturalArmor", "Mana-Forged Weave", 3);
		BlueprintFeature TwinBaseSize = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinAvatarBaseSize", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Twin Avatar Frame");
			bp.SetDescription(Main.IsekaiContext, "The Twin Avatar is Medium sized, whatever race its master is drawn as.");
			bp.HideInUI = true;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(ChangeUnitSize c)
			{
				c.m_Type = ChangeUnitSize.ChangeType.Value;
				c.Size = Size.Medium;
			});
		});
		NeutralHumanoidTemplate.CreateCopy(Main.IsekaiContext, "TwinAvatarCompanionUnit", delegate(BlueprintUnit bp)
		{
			bp.SetLocalisedName(Main.IsekaiContext, "Twin Avatar");
			((BlueprintUnitFact)bp).m_Description = Helpers.CreateString(Main.IsekaiContext, "TwinAvatarCompanionUnit.Description", "A humanoid double condensed out of your own otherworldly mana. It takes your face from the moment you were called into this world, and it learns the Guardian's discipline instead of your class.\nStarting ability scores: Str 16, Dex 14, Con 14, Int 12, Wis 12, Cha 12. Advances 1-20 in the Guardian class.");
			bp.ComponentsArray = new BlueprintComponent[0];
			bp.AddComponent(delegate(AddClassLevels c)
			{
				c.m_CharacterClass = GuardianCompanionClass.GetReference();
				c.RaceStat = StatType.Strength;
				c.LevelsStat = StatType.Strength;
				c.Skills = new StatType[2]
				{
					StatType.SkillPerception,
					StatType.SkillMobility
				};
				c.DoNotApplyAutomatically = false;
				c.m_MemorizeSpells = new BlueprintAbilityReference[0];
				c.m_SelectSpells = new BlueprintAbilityReference[0];
			});
			bp.AddComponent<AllowDyingCondition>();
			bp.AddComponent<AddResurrectOnRest>();
			if (bp.Visual != null)
			{
				bp.Visual.m_Barks = null;
			}
			bp.Alignment = Alignment.TrueNeutral;
			bp.Strength = 16;
			bp.Dexterity = 14;
			bp.Constitution = 14;
			bp.Intelligence = 12;
			bp.Wisdom = 12;
			bp.Charisma = 12;
			bp.MaxHP = 0;
			bp.Speed = new Feet(30f);
			bp.Size = Size.Medium;
			bp.m_AddFacts = new BlueprintUnitFactReference[0];
			BlueprintFeature[] array = new BlueprintFeature[5] { SimpleWeaponProficiency, MartialWeaponProficiency, TwinProgression, TwinArmor, TwinBaseSize };
			foreach (BlueprintFeature blueprintFeature in array)
			{
				if (blueprintFeature != null)
				{
					bp.m_AddFacts = bp.m_AddFacts.AppendToArray(blueprintFeature.ToReference<BlueprintUnitFactReference>());
				}
			}
			if (Neutrals != null)
			{
				bp.m_Faction = Neutrals.ToReference<BlueprintFactionReference>();
			}
		});
	}

	private static void BuildProgression()
	{
		BlueprintCharacterClassReference guardianClassRef = GuardianCompanionClass.GetReference();
		BlueprintFeature ManaForgedBody = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinAvatarManaForgedBody", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Mana-Forged Body");
			bp.SetDescription(Main.IsekaiContext, "The twin is not flesh but condensed mana wearing your shape. It gains damage reduction 5/magic and a +2 insight bonus to all saving throws.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.BypassedByMagic = true;
				c.Value = 5;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
		});
		BlueprintFeature MirroredStrike = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinAvatarMirroredStrike", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Mirrored Strike");
			bp.SetDescription(Main.IsekaiContext, "The mana that holds the twin together bleeds into its blows, dealing an additional 1d6 force damage with every attack.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddAdditionalWeaponDamage c)
			{
				c.DamageType = new DamageTypeDescription
				{
					Type = DamageType.Force
				};
				c.Value = new ContextDiceValue
				{
					DiceType = DiceType.D6,
					DiceCountValue = 1,
					BonusValue = 0
				};
			});
		});
		BlueprintFeature EchoOfTheHero = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinAvatarEchoOfTheHero", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Echo of the Hero");
			bp.SetDescription(Main.IsekaiContext, "Half a step out of step with the world, the twin gains permanent 20% concealment and cannot be flanked.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddConcealment c)
			{
				c.Concealment = Concealment.Partial;
				c.Descriptor = ConcealmentDescriptor.Displacement;
			});
			bp.AddComponent(delegate(AddMechanicsFeature c)
			{
				c.m_Feature = AddMechanicsFeature.MechanicsFeatureType.CannotBeFlanked;
			});
		});
		BlueprintFeature PerfectReflection = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinAvatarPerfectReflection", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Perfect Reflection");
			bp.SetDescription(Main.IsekaiContext, "The reflection is finally as real as the original: the twin gains a +4 inherent bonus to Strength, Dexterity and Constitution, and Fast Healing 5.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Pet;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Strength;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Dexterity;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Constitution;
				c.Value = 4;
			});
			bp.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 5;
				c.Bonus = 0;
			});
		});
		TwinProgression = Helpers.CreateBlueprint(Main.IsekaiContext, "TwinAvatarProgression", delegate(BlueprintProgression bp)
		{
			bp.SetName(Main.IsekaiContext, "Twin Avatar Progression");
			bp.SetDescription(Main.IsekaiContext, "Progression for the Twin Avatar companion.");
			bp.IsClassFeature = true;
			bp.m_Classes = new BlueprintProgression.ClassWithLevel[1]
			{
				new BlueprintProgression.ClassWithLevel
				{
					m_Class = guardianClassRef,
					AdditionalLevel = 0
				}
			};
			BlueprintFeature[] array = new BlueprintFeature[4] { ManaForgedBody, MirroredStrike, EchoOfTheHero, PerfectReflection };
			bp.LevelEntries = new LevelEntry[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				bp.LevelEntries[i] = Helpers.CreateLevelEntry(MilestoneLevels[i], array[i]);
			}
		});
	}
}
