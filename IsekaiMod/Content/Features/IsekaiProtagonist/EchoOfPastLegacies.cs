using System.Collections.Generic;
using IsekaiMod.Components;
using IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal class EchoOfPastLegacies
	{
		private static BlueprintFeatureSelection Selection;

		public static void Add()
		{
			Sprite iconPaladin = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("8a5b5e272e5c34e41aa8b4facbb746d3"))?.m_Icon;
			Sprite iconMonk = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e241bdfd6333b9843a7bfd674d607ac4"))?.m_Icon;
			Sprite iconRogue = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("576933720c440aa4d8d42b0c54b77e80"))?.m_Icon;
			Sprite iconMagus = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("be50f4e97fff8a24ba92561f1694a945"))?.m_Icon;
			Sprite iconSorcerer = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("0d188736c79e85b44b3cc49ad8d1bb1e"))?.m_Icon;
			Sprite iconKineticist = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("0601925a028b788469365d5f8f39e14a"))?.m_Icon;
			Sprite iconFighter = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("73c471386ce917c4c8c9f70d46b48eeb"))?.m_Icon;
			Sprite iconBarbarian = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("3c08d842e802c3e4eb19d15496145709"))?.m_Icon;
			Sprite iconInquisitor = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("5602845cd22683840a6f28ec46331051"))?.m_Icon;
			Sprite iconAlchemist = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("cee8f65448ce71c4b8b8ca13751dd8ea"))?.m_Icon;
			Sprite iconShifter = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("73ef2c330ff14e4bb2f5b7300622c552"))?.m_Icon;
			Sprite iconCleric = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("a79013ff4bcd4864cb669622a29ddafb"))?.m_Icon;
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "EchoPaladinDivineGraceFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Echo of the Paladin - Divine Grace");
				bp.SetDescription(Main.IsekaiContext, "Echoes of holy champions grant you Divine Grace, adding your Charisma bonus to all saving throws, plus a +2 sacred bonus on Fortitude, Reflex, and Will saves.");
				((BlueprintUnitFact)bp).m_Icon = iconPaladin;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				BlueprintFeature dg = BlueprintTools.GetBlueprint<BlueprintFeature>("8a5b5e272e5c34e41aa8b4facbb746d3");
				if (dg != null)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { dg.ToReference<BlueprintUnitFactReference>() };
					});
				}
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveReflex;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
			});
			BlueprintFeature blueprintFeature2 = Helpers.CreateBlueprint(Main.IsekaiContext, "EchoMonkDefenseFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Echo of the Monk - Canny Defense");
				bp.SetDescription(Main.IsekaiContext, "Echoes of ascetic martial ascetics grant you the Monk AC bonus and Flurry of Blows, as well as 1 additional attack per round during full attacks.");
				((BlueprintUnitFact)bp).m_Icon = iconMonk;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				BlueprintFeature ac = BlueprintTools.GetBlueprint<BlueprintFeature>("e241bdfd6333b9843a7bfd674d607ac4");
				BlueprintFeature flurry = BlueprintTools.GetBlueprint<BlueprintFeature>("332362f3bd39ebe46a740a36960fdcb4");
				bp.AddComponent(delegate(AddFacts c)
				{
					List<BlueprintUnitFactReference> list = new List<BlueprintUnitFactReference>();
					if (ac != null)
					{
						list.Add(ac.ToReference<BlueprintUnitFactReference>());
					}
					if (flurry != null)
					{
						list.Add(flurry.ToReference<BlueprintUnitFactReference>());
					}
					c.m_Facts = list.ToArray();
				});
				bp.AddComponent(delegate(AddExtraAttack c)
				{
					c.Number = 1;
				});
			});
			BlueprintFeature blueprintFeature3 = Helpers.CreateBlueprint(Main.IsekaiContext, "EchoRogueEvasionFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Echo of the Rogue - Shadow Instinct");
				bp.SetDescription(Main.IsekaiContext, "Echoes of elusive thieves grant you Evasion and +2d6 Sneak Attack damage.");
				((BlueprintUnitFact)bp).m_Icon = iconRogue;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				BlueprintFeature evasion = BlueprintTools.GetBlueprint<BlueprintFeature>("576933720c440aa4d8d42b0c54b77e80");
				if (evasion != null)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { evasion.ToReference<BlueprintUnitFactReference>() };
					});
				}
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SneakAttack;
					c.Value = 2;
				});
			});
			BlueprintFeature blueprintFeature4 = Helpers.CreateBlueprint(Main.IsekaiContext, "EchoMagusSpellstrikeFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Echo of the Magus - Spellstrike Resonance");
				bp.SetDescription(Main.IsekaiContext, "Echoes of eldritch blade-masters grant you Spellstrike, a +2 bonus to spell difficulty classes, and a +2 bonus on melee attack rolls.");
				((BlueprintUnitFact)bp).m_Icon = iconMagus;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				BlueprintFeature ss = BlueprintTools.GetBlueprint<BlueprintFeature>("be50f4e97fff8a24ba92561f1694a945");
				if (ss != null)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { ss.ToReference<BlueprintUnitFactReference>() };
					});
				}
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 2;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
			});
			BlueprintFeature blueprintFeature5 = Helpers.CreateBlueprint(Main.IsekaiContext, "EchoSorcererArcanaFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Echo of the Sorcerer - Bloodline Arcana");
				bp.SetDescription(Main.IsekaiContext, "Echoes of primordial spellweavers infuse your magic. Your damaging spells deal +1 damage per die rolled, and you gain a +2 bonus on caster level checks to overcome spell resistance.");
				((BlueprintUnitFact)bp).m_Icon = iconSorcerer;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(IncreaseSpellDamage c)
				{
					c.DamageBonus = 1;
				});
				bp.AddComponent(delegate(SpellPenetrationBonus c)
				{
					c.Value = 2;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
			});
			BlueprintFeature blueprintFeature6 = Helpers.CreateBlueprint(Main.IsekaiContext, "EchoKineticistGatherFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Echo of the Kineticist - Planar Conduit");
				bp.SetDescription(Main.IsekaiContext, "Echoes of elemental wilders grant you Gather Power, a +2 bonus on attack rolls, and +2 bonus to damage rolls.");
				((BlueprintUnitFact)bp).m_Icon = iconKineticist;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				BlueprintFeature gp = BlueprintTools.GetBlueprint<BlueprintFeature>("0601925a028b788469365d5f8f39e14a");
				if (gp != null)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { gp.ToReference<BlueprintUnitFactReference>() };
					});
				}
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 2;
				});
			});
			BlueprintFeature blueprintFeature7 = Helpers.CreateBlueprint(Main.IsekaiContext, "EchoFighterMasteryFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Echo of the Fighter - Weapon & Armor Mastery");
				bp.SetDescription(Main.IsekaiContext, "Echoes of frontline veterans grant you Armor Training and Weapon Mastery, adding a +2 competence bonus on attack and damage rolls with manufactured weapons.");
				((BlueprintUnitFact)bp).m_Icon = iconFighter;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				BlueprintFeature at = BlueprintTools.GetBlueprint<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");
				BlueprintFeature wm = BlueprintTools.GetBlueprint<BlueprintFeature>("73c471386ce917c4c8c9f70d46b48eeb");
				bp.AddComponent(delegate(AddFacts c)
				{
					List<BlueprintUnitFactReference> list = new List<BlueprintUnitFactReference>();
					if (at != null)
					{
						list.Add(at.ToReference<BlueprintUnitFactReference>());
					}
					if (wm != null)
					{
						list.Add(wm.ToReference<BlueprintUnitFactReference>());
					}
					c.m_Facts = list.ToArray();
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 2;
				});
			});
			BlueprintFeature blueprintFeature8 = Helpers.CreateBlueprint(Main.IsekaiContext, "EchoBarbarianUncannyFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Echo of the Barbarian - Primal Ferocity");
				bp.SetDescription(Main.IsekaiContext, "Echoes of howling berserkers grant you Uncanny Dodge, +10 feet base movement speed, and a +2 untyped bonus to Fortitude saving throws.");
				((BlueprintUnitFact)bp).m_Icon = iconBarbarian;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				BlueprintFeature ud = BlueprintTools.GetBlueprint<BlueprintFeature>("3c08d842e802c3e4eb19d15496145709");
				if (ud != null)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { ud.ToReference<BlueprintUnitFactReference>() };
					});
				}
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.SaveFortitude;
					c.Value = 2;
				});
			});
			BlueprintFeature blueprintFeature9 = Helpers.CreateBlueprint(Main.IsekaiContext, "EchoInquisitorSoloFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Echo of the Inquisitor - Solo Tactics");
				bp.SetDescription(Main.IsekaiContext, "Echoes of relentless investigators grant you Solo Tactics, allowing you to reap the benefits of all teamwork feats as if all your allies possessed them, plus a +4 sacred bonus to Initiative.");
				((BlueprintUnitFact)bp).m_Icon = iconInquisitor;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				BlueprintFeature st = BlueprintTools.GetBlueprint<BlueprintFeature>("5602845cd22683840a6f28ec46331051");
				if (st != null)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { st.ToReference<BlueprintUnitFactReference>() };
					});
				}
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Initiative;
					c.Value = 4;
				});
			});
			BlueprintFeature blueprintFeature10 = Helpers.CreateBlueprint(Main.IsekaiContext, "EchoAlchemistMutagenFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Echo of the Alchemist - Alchemical Discovery");
				bp.SetDescription(Main.IsekaiContext, "Echoes of apothecary masters grant you the ability to brew Mutagens, plus a permanent +2 alchemical bonus to Armor Class and physical ability scores.");
				((BlueprintUnitFact)bp).m_Icon = iconAlchemist;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				BlueprintFeature mut = BlueprintTools.GetBlueprint<BlueprintFeature>("cee8f65448ce71c4b8b8ca13751dd8ea");
				if (mut != null)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { mut.ToReference<BlueprintUnitFactReference>() };
					});
				}
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Alchemical;
					c.Stat = StatType.Strength;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Alchemical;
					c.Stat = StatType.Dexterity;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Alchemical;
					c.Stat = StatType.Constitution;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Alchemical;
					c.Stat = StatType.AC;
					c.Value = 2;
				});
			});
			BlueprintFeature blueprintFeature11 = Helpers.CreateBlueprint(Main.IsekaiContext, "EchoShifterBeastFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Echo of the Shifter - Aspect of the Wild");
				bp.SetDescription(Main.IsekaiContext, "Echoes of feral shapeshifters grant you Shifter Aspect and Shifter Claws, plus a +3 natural armor bonus to AC and +2 damage to natural attacks.");
				((BlueprintUnitFact)bp).m_Icon = iconShifter;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				BlueprintFeature asp = BlueprintTools.GetBlueprint<BlueprintFeature>("73ef2c330ff14e4bb2f5b7300622c552");
				BlueprintFeature claw = BlueprintTools.GetBlueprint<BlueprintFeature>("512f845e29514539b1943b74633dba24");
				bp.AddComponent(delegate(AddFacts c)
				{
					List<BlueprintUnitFactReference> list = new List<BlueprintUnitFactReference>();
					if (asp != null)
					{
						list.Add(asp.ToReference<BlueprintUnitFactReference>());
					}
					if (claw != null)
					{
						list.Add(claw.ToReference<BlueprintUnitFactReference>());
					}
					c.m_Facts = list.ToArray();
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 2;
				});
			});
			BlueprintFeature blueprintFeature12 = Helpers.CreateBlueprint(Main.IsekaiContext, "EchoClericDomainFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Echo of the Cleric - Sacred Conduit");
				bp.SetDescription(Main.IsekaiContext, "Echoes of pious hierophants grant you Channel Energy, +2 extra channel uses per day, and a +2 sacred bonus to Will saving throws.");
				((BlueprintUnitFact)bp).m_Icon = iconCleric;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				BlueprintFeature ce = BlueprintTools.GetBlueprint<BlueprintFeature>("a79013ff4bcd4864cb669622a29ddafb");
				if (ce != null)
				{
					bp.AddComponent(delegate(AddFacts c)
					{
						c.m_Facts = new BlueprintUnitFactReference[1] { ce.ToReference<BlueprintUnitFactReference>() };
					});
				}
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.SaveWill;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Sacred;
					c.Stat = StatType.Wisdom;
					c.Value = 2;
				});
			});
			Selection = Helpers.CreateBlueprint(Main.IsekaiContext, "EchoOfPastLegaciesSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Echo of Past Legacies");
				bp.SetDescription(Main.IsekaiContext, "Echoes of past-life training and archetypal memories crystallize into passive hallmarks. Select an echo from a class legacy to permanently bolster your otherworldly abilities.");
				((BlueprintUnitFact)bp).m_Icon = iconPaladin;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.IgnorePrerequisites = false;
				bp.m_AllFeatures = new BlueprintFeatureReference[0];
				bp.m_Features = new BlueprintFeatureReference[0];
			});
			Selection.AddFeatures(blueprintFeature, blueprintFeature2, blueprintFeature3, blueprintFeature4, blueprintFeature5, blueprintFeature6, blueprintFeature7, blueprintFeature8, blueprintFeature9, blueprintFeature10, blueprintFeature11, blueprintFeature12);
			SpecialPowerSelection.AddToSelection(Selection);
			FeatTools.Selections.BasicFeatSelection.AddToSelection(Selection);
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
}
