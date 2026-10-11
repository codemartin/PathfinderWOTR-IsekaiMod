using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Components;
using IsekaiMod.Config;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist;

internal class EchoOfPastLegacies
{
	private static BlueprintFeatureSelection Selection;

	internal const string DivineGrace = "8a5b5e272e5c34e41aa8b4facbb746d3";

	internal const string MonkACBonus = "e241bdfd6333b9843a7bfd674d607ac4";

	internal const string FlurryOfBlows = "332362f3bd39ebe46a740a36960fdcb4";

	internal const string Evasion = "576933720c440aa4d8d42b0c54b77e80";

	internal const string SpellStrikeFeature = "be50f4e97fff8a24ba92561f1694a945";

	internal const string SageBloodlineArcana = "0d188736c79e85b44b3cc49ad8d1bb1e";

	internal const string GatherPowerFeature = "0601925a028b788469365d5f8f39e14a";

	internal const string GatherPowerAbilitiesFeature = "71f526b1d4b50b94582b0b9cbe12b0e0";

	internal const string ArmorTraining = "3c380607706f209499d951b29d3c44f3";

	internal const string WeaponMastery = "73c471386ce917c4c8c9f70d46b48eeb";

	internal const string UncannyDodge = "3c08d842e802c3e4eb19d15496145709";

	internal const string SoloTactics = "5602845cd22683840a6f28ec46331051";

	internal const string MutagenFeature = "cee8f65448ce71c4b8b8ca13751dd8ea";

	internal const string ShifterAspectBear = "73ef2c330ff14e4bb2f5b7300622c552";

	internal const string ShifterClawsFeatureLevel1 = "512f845e29514539b1943b74633dba24";

	internal const string ChannelEnergyFeature = "a79013ff4bcd4864cb669622a29ddafb";

	internal const string ChannelEnergyResource = "5e2bba3e07c37be42909a12945c27de7";

	internal static readonly string[] EchoNames = new string[12]
	{
		"EchoPaladinDivineGraceFeature", "EchoMonkDefenseFeature", "EchoRogueEvasionFeature", "EchoMagusSpellstrikeFeature", "EchoSorcererArcanaFeature", "EchoKineticistGatherFeature", "EchoFighterMasteryFeature", "EchoBarbarianUncannyFeature", "EchoInquisitorSoloFeature", "EchoAlchemistMutagenFeature",
		"EchoShifterBeastFeature", "EchoClericDomainFeature"
	};

	private static Sprite IconOf(string guid)
	{
		return ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>(guid))?.m_Icon;
	}

	private static void GrantFacts(BlueprintFeature bp, params string[] guids)
	{
		List<BlueprintUnitFactReference> refs = new List<BlueprintUnitFactReference>();
		for (int i = 0; i < guids.Length; i++)
		{
			BlueprintFeature blueprint = BlueprintTools.GetBlueprint<BlueprintFeature>(guids[i]);
			if (blueprint != null)
			{
				refs.Add(blueprint.ToReference<BlueprintUnitFactReference>());
			}
		}
		if (refs.Count != 0)
		{
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = refs.ToArray();
			});
		}
	}

	private static void Bonus(BlueprintFeature bp, StatType stat, int value, ModifierDescriptor descriptor)
	{
		bp.AddComponent(delegate(AddStatBonus c)
		{
			c.Stat = stat;
			c.Value = value;
			c.Descriptor = descriptor;
		});
	}

	private static void Setup(BlueprintFeature bp, string iconGuid)
	{
		((BlueprintUnitFact)bp).m_Icon = IconOf(iconGuid);
		bp.Ranks = 1;
		bp.IsClassFeature = true;
	}

	public static void Add()
	{
		List<BlueprintFeature> list = new List<BlueprintFeature>();
		list.Add(Helpers.CreateBlueprint(Main.IsekaiContext, "EchoPaladinDivineGraceFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Echo of the Paladin - Divine Grace");
			bp.SetDescription(Main.IsekaiContext, "Echoes of holy champions grant you Divine Grace, adding your Charisma bonus to all saving throws, plus a +2 sacred bonus on Fortitude, Reflex, and Will saves.");
			Setup(bp, "8a5b5e272e5c34e41aa8b4facbb746d3");
			GrantFacts(bp, "8a5b5e272e5c34e41aa8b4facbb746d3");
			Bonus(bp, StatType.SaveFortitude, 2, ModifierDescriptor.Sacred);
			Bonus(bp, StatType.SaveReflex, 2, ModifierDescriptor.Sacred);
			Bonus(bp, StatType.SaveWill, 2, ModifierDescriptor.Sacred);
		}));
		list.Add(Helpers.CreateBlueprint(Main.IsekaiContext, "EchoMonkDefenseFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Echo of the Monk - Canny Defense");
			bp.SetDescription(Main.IsekaiContext, "Echoes of ascetic martial ascetics grant you the Monk AC bonus and Flurry of Blows, as well as 1 additional attack per round during full attacks.");
			Setup(bp, "e241bdfd6333b9843a7bfd674d607ac4");
			GrantFacts(bp, "e241bdfd6333b9843a7bfd674d607ac4", "332362f3bd39ebe46a740a36960fdcb4");
			bp.AddComponent(delegate(AddExtraAttack c)
			{
				c.Number = 1;
			});
		}));
		list.Add(Helpers.CreateBlueprint(Main.IsekaiContext, "EchoRogueEvasionFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Echo of the Rogue - Shadow Instinct");
			bp.SetDescription(Main.IsekaiContext, "Echoes of elusive thieves grant you Evasion and +2d6 Sneak Attack damage.");
			Setup(bp, "576933720c440aa4d8d42b0c54b77e80");
			GrantFacts(bp, "576933720c440aa4d8d42b0c54b77e80");
			Bonus(bp, StatType.SneakAttack, 2, ModifierDescriptor.UntypedStackable);
		}));
		list.Add(Helpers.CreateBlueprint(Main.IsekaiContext, "EchoMagusSpellstrikeFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Echo of the Magus - Spellstrike Resonance");
			bp.SetDescription(Main.IsekaiContext, "Echoes of eldritch blade-masters grant you Spellstrike, a +2 bonus to spell difficulty classes, and a +2 bonus on melee attack rolls.");
			Setup(bp, "be50f4e97fff8a24ba92561f1694a945");
			GrantFacts(bp, "be50f4e97fff8a24ba92561f1694a945");
			bp.AddComponent(delegate(IncreaseAllSpellsDC c)
			{
				c.Value = 2;
				c.Descriptor = ModifierDescriptor.UntypedStackable;
			});
			Bonus(bp, StatType.AdditionalAttackBonus, 2, ModifierDescriptor.UntypedStackable);
		}));
		list.Add(Helpers.CreateBlueprint(Main.IsekaiContext, "EchoSorcererArcanaFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Echo of the Sorcerer - Bloodline Arcana");
			bp.SetDescription(Main.IsekaiContext, "Echoes of primordial spellweavers infuse your magic. Your damaging spells deal +1 damage per die rolled, and you gain a +2 bonus on caster level checks to overcome spell resistance.");
			Setup(bp, "0d188736c79e85b44b3cc49ad8d1bb1e");
			bp.AddComponent(delegate(IncreaseSpellDamagePerDie c)
			{
				c.BonusPerDie = 1;
			});
			bp.AddComponent(delegate(SpellPenetrationBonus c)
			{
				c.Value = 2;
				c.Descriptor = ModifierDescriptor.UntypedStackable;
			});
		}));
		list.Add(Helpers.CreateBlueprint(Main.IsekaiContext, "EchoKineticistGatherFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Echo of the Kineticist - Planar Conduit");
			bp.SetDescription(Main.IsekaiContext, "Echoes of elemental wilders grant you Gather Power, a +2 bonus on attack rolls, and +2 bonus to damage rolls.");
			Setup(bp, "0601925a028b788469365d5f8f39e14a");
			GrantFacts(bp, "71f526b1d4b50b94582b0b9cbe12b0e0");
			Bonus(bp, StatType.AdditionalAttackBonus, 2, ModifierDescriptor.UntypedStackable);
			Bonus(bp, StatType.AdditionalDamage, 2, ModifierDescriptor.UntypedStackable);
		}));
		list.Add(Helpers.CreateBlueprint(Main.IsekaiContext, "EchoFighterMasteryFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Echo of the Fighter - Weapon & Armor Mastery");
			bp.SetDescription(Main.IsekaiContext, "Echoes of frontline veterans grant you Armor Training and a +2 competence bonus on attack and damage rolls.");
			Setup(bp, "73c471386ce917c4c8c9f70d46b48eeb");
			GrantFacts(bp, "3c380607706f209499d951b29d3c44f3", "73c471386ce917c4c8c9f70d46b48eeb");
			Bonus(bp, StatType.AdditionalAttackBonus, 2, ModifierDescriptor.Competence);
			Bonus(bp, StatType.AdditionalDamage, 2, ModifierDescriptor.Competence);
		}));
		list.Add(Helpers.CreateBlueprint(Main.IsekaiContext, "EchoBarbarianUncannyFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Echo of the Barbarian - Primal Ferocity");
			bp.SetDescription(Main.IsekaiContext, "Echoes of howling berserkers grant you Uncanny Dodge, +10 feet base movement speed, and a +2 untyped bonus to Fortitude saving throws.");
			Setup(bp, "3c08d842e802c3e4eb19d15496145709");
			GrantFacts(bp, "3c08d842e802c3e4eb19d15496145709");
			Bonus(bp, StatType.Speed, 10, ModifierDescriptor.UntypedStackable);
			Bonus(bp, StatType.SaveFortitude, 2, ModifierDescriptor.UntypedStackable);
		}));
		list.Add(Helpers.CreateBlueprint(Main.IsekaiContext, "EchoInquisitorSoloFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Echo of the Inquisitor - Solo Tactics");
			bp.SetDescription(Main.IsekaiContext, "Echoes of relentless investigators grant you Solo Tactics, allowing you to reap the benefits of all teamwork feats as if all your allies possessed them, plus a +4 sacred bonus to Initiative.");
			Setup(bp, "5602845cd22683840a6f28ec46331051");
			GrantFacts(bp, "5602845cd22683840a6f28ec46331051");
			Bonus(bp, StatType.Initiative, 4, ModifierDescriptor.Sacred);
		}));
		list.Add(Helpers.CreateBlueprint(Main.IsekaiContext, "EchoAlchemistMutagenFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Echo of the Alchemist - Alchemical Discovery");
			bp.SetDescription(Main.IsekaiContext, "Echoes of apothecary masters grant you the ability to brew Mutagens, plus a permanent +2 alchemical bonus to Armor Class and physical ability scores.");
			Setup(bp, "cee8f65448ce71c4b8b8ca13751dd8ea");
			GrantFacts(bp, "cee8f65448ce71c4b8b8ca13751dd8ea");
			Bonus(bp, StatType.Strength, 2, ModifierDescriptor.Alchemical);
			Bonus(bp, StatType.Dexterity, 2, ModifierDescriptor.Alchemical);
			Bonus(bp, StatType.Constitution, 2, ModifierDescriptor.Alchemical);
			Bonus(bp, StatType.AC, 2, ModifierDescriptor.Alchemical);
		}));
		list.Add(Helpers.CreateBlueprint(Main.IsekaiContext, "EchoShifterBeastFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Echo of the Shifter - Aspect of the Wild");
			bp.SetDescription(Main.IsekaiContext, "Echoes of feral shapeshifters grant you Shifter Aspect and Shifter Claws, plus a +3 natural armor bonus to AC and +2 damage to natural attacks.");
			Setup(bp, "73ef2c330ff14e4bb2f5b7300622c552");
			GrantFacts(bp, "73ef2c330ff14e4bb2f5b7300622c552", "512f845e29514539b1943b74633dba24");
			Bonus(bp, StatType.AC, 3, ModifierDescriptor.NaturalArmor);
			Bonus(bp, StatType.AdditionalDamage, 2, ModifierDescriptor.UntypedStackable);
		}));
		list.Add(Helpers.CreateBlueprint(Main.IsekaiContext, "EchoClericDomainFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Echo of the Cleric - Sacred Conduit");
			bp.SetDescription(Main.IsekaiContext, "Echoes of pious hierophants grant you Channel Energy, +2 extra channel uses per day, and a +2 sacred bonus to Will saving throws and to Wisdom.");
			Setup(bp, "a79013ff4bcd4864cb669622a29ddafb");
			GrantFacts(bp, "a79013ff4bcd4864cb669622a29ddafb");
			BlueprintAbilityResource resource = BlueprintTools.GetBlueprint<BlueprintAbilityResource>("5e2bba3e07c37be42909a12945c27de7");
			if (resource != null)
			{
				bp.AddComponent(delegate(IncreaseResourceAmount c)
				{
					c.m_Resource = resource.ToReference<BlueprintAbilityResourceReference>();
					c.Value = 2;
				});
			}
			Bonus(bp, StatType.SaveWill, 2, ModifierDescriptor.Sacred);
			Bonus(bp, StatType.Wisdom, 2, ModifierDescriptor.Sacred);
		}));
		Sprite selectionIcon = IconOf("8a5b5e272e5c34e41aa8b4facbb746d3");
		Selection = Helpers.CreateBlueprint(Main.IsekaiContext, "EchoOfPastLegaciesSelection", delegate(BlueprintFeatureSelection bp)
		{
			bp.SetName(Main.IsekaiContext, "Echo of Past Legacies");
			bp.SetDescription(Main.IsekaiContext, "Echoes of past-life training and archetypal memories crystallize into passive hallmarks. Select an echo from a class legacy to permanently bolster your otherworldly abilities.");
			((BlueprintUnitFact)bp).m_Icon = selectionIcon;
			bp.Ranks = 1;
			bp.AddComponent(delegate(PrerequisiteFeatureRankBelow c)
			{
				c.MaxRank = 1;
			});
			bp.AddComponent(delegate(PrerequisiteClassLevel c)
			{
				c.m_CharacterClass = IsekaiProtagonistClass.Get().ToReference<BlueprintCharacterClassReference>();
				c.Level = 1;
			});
			bp.IsClassFeature = true;
			bp.IgnorePrerequisites = false;
			bp.m_AllFeatures = new BlueprintFeatureReference[0];
			bp.m_Features = new BlueprintFeatureReference[0];
		});
		Selection.AddFeatures(list.Where((BlueprintFeature e) => e != null).ToArray());
		SpecialPowerSelection.AddToSelection(Selection);
		ContentOffering.OfferInSelection(FeatTools.Selections.BasicFeatSelection, Selection, OfferedContent.IsekaiProtagonist);
	}

	public static BlueprintFeatureSelection Get()
	{
		if (Selection != null)
		{
			return Selection;
		}
		return BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "EchoOfPastLegaciesSelection");
	}
}
