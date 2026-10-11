using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using IsekaiMod.Components;
using IsekaiMod.Config;
using IsekaiMod.Content.Arenas;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.TextTools;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Constellations;

internal class DivineSponsorshipStore
{
	internal enum PatronSwitchBlocker
	{
		None,
		ForbidsSpellbook,
		RefusesCharacter
	}

	internal const string VanillaBesmaraFeatureGuid = "470d824767d34e2e9a59b6d70681af7a";

	public static BlueprintFeature CosmicSponsorshipExchangeFeature;

	public const int CosmicWishChargeCap = 10;

	internal const string BalanceTemplateTag = "isekai_cosmic_coins";

	internal const string BalanceLine = "\n\n<b>Your balance:</b> {isekai_cosmic_coins} Cosmic Coins";

	internal static readonly IReadOnlyDictionary<string, AlignmentMaskType> MythicBoonAlignment = new Dictionary<string, AlignmentMaskType>(StringComparer.Ordinal)
	{
		{
			"BoonOfHeavenlyHost",
			AlignmentMaskType.Good
		},
		{
			"BoonOfAbyssalRage",
			AlignmentMaskType.ChaoticEvil
		},
		{
			"BoonOfLordOfBones",
			AlignmentMaskType.Evil
		},
		{
			"BoonOfWildWonder",
			AlignmentMaskType.ChaoticGood
		},
		{
			"BoonOfCosmicJester",
			AlignmentMaskType.Chaotic
		},
		{
			"BoonOfMonadicOrder",
			AlignmentMaskType.Lawful
		},
		{
			"BoonOfHellfireContract",
			AlignmentMaskType.LawfulEvil
		},
		{
			"BoonOfGoldenDragon",
			AlignmentMaskType.Good
		},
		{
			"BoonOfPrimevalSwarm",
			AlignmentMaskType.Evil
		},
		{
			"BoonOfUnboundMortal",
			AlignmentMaskType.Any
		}
	};

	internal const int TranscendenceCost = 1200;

	internal const string AtheismPatronName = "Atheism";

	internal const int AvatarCost = 10000;

	internal const string LanternKingDiscountAnswerName = "AnswerAscendAvatarLanternKingDiscount";

	private static bool s_BalanceTemplateRegistered;

	internal static bool AlignmentAllows(AlignmentMaskType allowed, Alignment alignment)
	{
		if (allowed == AlignmentMaskType.Any)
		{
			return true;
		}
		return (allowed & alignment.ToMask()) != 0;
	}

	internal static string AlignmentName(AlignmentMaskType mask)
	{
		return mask switch
		{
			AlignmentMaskType.Any => "any", 
			AlignmentMaskType.Good => "Good", 
			AlignmentMaskType.Evil => "Evil", 
			AlignmentMaskType.Lawful => "Lawful", 
			AlignmentMaskType.Chaotic => "Chaotic", 
			AlignmentMaskType.LawfulGood => "Lawful Good", 
			AlignmentMaskType.NeutralGood => "Neutral Good", 
			AlignmentMaskType.ChaoticGood => "Chaotic Good", 
			AlignmentMaskType.LawfulNeutral => "Lawful Neutral", 
			AlignmentMaskType.TrueNeutral => "True Neutral", 
			AlignmentMaskType.ChaoticNeutral => "Chaotic Neutral", 
			AlignmentMaskType.LawfulEvil => "Lawful Evil", 
			AlignmentMaskType.NeutralEvil => "Neutral Evil", 
			AlignmentMaskType.ChaoticEvil => "Chaotic Evil", 
			_ => mask.ToString(), 
		};
	}

	internal static string FormatCoins(int amount)
	{
		return amount.ToString("N0", CultureInfo.InvariantCulture);
	}

	internal static string OfferLabel(int cost, string displayName, string description)
	{
		return "(" + FormatCoins(cost) + " Coins) [" + displayName + "] " + description;
	}

	internal static string MythicOfferLabel(int cost, string displayName, string description, AlignmentMaskType alignment)
	{
		if (alignment != AlignmentMaskType.Any)
		{
			return "(" + FormatCoins(cost) + " Coins, " + AlignmentName(alignment) + " only) [" + displayName + "] " + description;
		}
		return OfferLabel(cost, displayName, description);
	}

	internal static string TranscendenceLabel(string archetype, string displayName, string description)
	{
		return "(" + FormatCoins(1200) + " Coins) [" + archetype + "] " + displayName + ": " + description;
	}

	internal static string PatronLabel(string deityName)
	{
		if (!(deityName == "Atheism"))
		{
			return "(" + FormatCoins(500) + " Coins) [Switch Patron] Align your soul with " + deityName;
		}
		return "(" + FormatCoins(500) + " Coins) [Switch Patron] Renounce all patrons (Atheism)";
	}

	internal static string AvatarLabel(string avatarName, string summary)
	{
		return "<color=#5B2A86>[Ascend]</color> Embody the " + avatarName + " (" + FormatCoins(10000) + " Coins): " + summary;
	}

	internal static string DiscountedAvatarLabel(string avatarName, string summary)
	{
		return "<color=#5B2A86>[Ascend]</color> Embody the " + avatarName + " (" + FormatCoins(LanternKingReveal.DiscountedAvatarCost) + " Coins, the Lantern King's half-price deal for winning every cup): " + summary;
	}

	internal static List<BlueprintFeature> OwnedFeatures(UnitEntityData unit)
	{
		List<BlueprintFeature> list = new List<BlueprintFeature>();
		if (unit?.Descriptor?.Progression?.Features == null)
		{
			return list;
		}
		foreach (Feature feature in unit.Descriptor.Progression.Features)
		{
			if (feature?.Blueprint != null)
			{
				list.Add(feature.Blueprint);
			}
		}
		return list;
	}

	internal static bool SpellbookWouldBeForbidden(AlignmentMaskType deityAlignment, Alignment characterAlignment, bool ignoreFactConfigured, bool hasIgnoreFact, bool castsFromListedSpellbook)
	{
		if (!castsFromListedSpellbook)
		{
			return false;
		}
		if (((characterAlignment.ToMask() & deityAlignment) == 0) & ignoreFactConfigured)
		{
			return !hasIgnoreFact;
		}
		return false;
	}

	internal static PatronSwitchBlocker PatronSwitchBlock(UnitEntityData unit, BlueprintFeature deity)
	{
		if (unit?.Descriptor == null || deity == null)
		{
			return PatronSwitchBlocker.None;
		}
		UnitDescriptor descriptor = unit.Descriptor;
		BlueprintComponent[] source = deity.ComponentsArray ?? new BlueprintComponent[0];
		foreach (ForbidSpellbookOnAlignmentDeviation item in source.OfType<ForbidSpellbookOnAlignmentDeviation>())
		{
			BlueprintSpellbookReference[] listed = item.m_Spellbooks ?? new BlueprintSpellbookReference[0];
			bool castsFromListedSpellbook = descriptor.Spellbooks.Any((Spellbook book) => book?.Blueprint != null && listed.Any((BlueprintSpellbookReference r) => r != null && r.Get() == book.Blueprint));
			BlueprintUnitFact ignoreFact = item.IgnoreFact;
			if (SpellbookWouldBeForbidden(item.Alignment, descriptor.Alignment.ValueRaw, ignoreFact != null, ignoreFact != null && descriptor.HasFact(ignoreFact), castsFromListedSpellbook))
			{
				return PatronSwitchBlocker.ForbidsSpellbook;
			}
		}
		foreach (Prerequisite item2 in source.OfType<Prerequisite>())
		{
			if (item2.Group == Prerequisite.GroupType.All && (item2 is PrerequisiteNoClassLevel || item2 is PrerequisiteNoArchetype || item2 is PrerequisiteNoFeature) && !item2.CheckRaw(null, descriptor, null))
			{
				return PatronSwitchBlocker.RefusesCharacter;
			}
		}
		return PatronSwitchBlocker.None;
	}

	private static void RegisterBalanceTemplate()
	{
		if (s_BalanceTemplateRegistered)
		{
			return;
		}
		try
		{
			TextTemplateEngine.AddTemplate("isekai_cosmic_coins", (TextTemplate)new CosmicCoinBalanceTemplate());
			s_BalanceTemplateRegistered = true;
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Cosmic Store: could not register the balance text template: " + ex);
		}
	}

	public static void Add()
	{
		Sprite Icon_Coin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
		Sprite Icon_CosmicWish = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("1325e698f4a3f224b880e3b83a551228"))?.m_Icon;
		Sprite Icon_WishWealth = BlueprintTools.GetBlueprint<BlueprintItem>("6f0b2c6a856175e469224e102d41b90b")?.m_Icon;
		Sprite variantIcon = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("9f476da1aa712284d8e652fc387ce5bb"))?.m_Icon;
		Sprite variantIcon2 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ce020c1de7b2eb74f9f56053aaec419c"))?.m_Icon;
		Sprite variantIcon3 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("8942396687d6ce543a33d4d7424211ca"))?.m_Icon;
		Sprite variantIcon4 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("776683d03cb1fd341b488d09fd4c5290"))?.m_Icon;
		Sprite variantIcon5 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("0f4eaadeb5c9a2749b7d3aaf78058a93"))?.m_Icon;
		Sprite variantIcon6 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("e0c5386bbdbf36346b0dfebe098e9b26"))?.m_Icon;
		RegisterBalanceTemplate();
		BlueprintCue CueStoreBlessings = TTCoreExtensions.CreateCue("CueStoreBlessings", delegate(BlueprintCue blueprintCue6)
		{
			blueprintCue6.SetText(Main.IsekaiContext, "<color=#5B2A86><b>[Deity Blessings Catalog]</b></color>\n\nThe constellations offer their divine favor in exchange for Cosmic Coins. Blessings last for 1 hour and persist through death.\n\nSelect a blessing to invoke:\n\n<b>Your balance:</b> {isekai_cosmic_coins} Cosmic Coins");
			blueprintCue6.Answers = new List<BlueprintAnswerBaseReference>();
		});
		BlueprintCue CueStoreMythic = TTCoreExtensions.CreateCue("CueStoreMythic", delegate(BlueprintCue blueprintCue6)
		{
			blueprintCue6.SetText(Main.IsekaiContext, "<color=#5B2A86><b>[Mythic Path Boons Catalog]</b></color>\n\nEchoes of legendary planar destinies crystallized into permanent personal enhancements. Each boon asks for the alignment shown beside its price; the Key of Infinite Pathways, offered among the Infusions, lifts that requirement.\n\nSelect a mythic path boon to acquire:\n\n<b>Your balance:</b> {isekai_cosmic_coins} Cosmic Coins");
			blueprintCue6.Answers = new List<BlueprintAnswerBaseReference>();
		});
		BlueprintCue blueprintCue = TTCoreExtensions.CreateCue("CueStoreConsumables", delegate(BlueprintCue blueprintCue6)
		{
			blueprintCue6.SetText(Main.IsekaiContext, "<color=#5B2A86><b>[Web Novel Consumables & Cosmic Wishes]</b></color>\n\nEmergency trump cards and reality-warping miracles drafted by senior transmigrators.\n\nSelect an offering to acquire:\n\n<b>Your balance:</b> {isekai_cosmic_coins} Cosmic Coins");
			blueprintCue6.Answers = new List<BlueprintAnswerBaseReference>();
		});
		BlueprintCue CueStoreInfusions = TTCoreExtensions.CreateCue("CueStoreInfusions", delegate(BlueprintCue blueprintCue6)
		{
			blueprintCue6.SetText(Main.IsekaiContext, "<color=#5B2A86><b>[Permanent Infusions & Subclass Transcendence]</b></color>\n\nInscribe cosmic truth directly into your flesh, blood, and class techniques. Each transcendence is offered only to the archetype it belongs to.\n\nSelect an infusion or transcendence to acquire:\n\n<b>Your balance:</b> {isekai_cosmic_coins} Cosmic Coins");
			blueprintCue6.Answers = new List<BlueprintAnswerBaseReference>();
		});
		BlueprintCue blueprintCue2 = TTCoreExtensions.CreateCue("CueStoreForbiddenCheats", delegate(BlueprintCue blueprintCue6)
		{
			blueprintCue6.SetText(Main.IsekaiContext, "<color=#7A1010><b>[Forbidden Otherworldly Cheats (Protagonist Hacks)]</b></color>\n\n<i>\"H-Hey! How did you get access to the developer console?! The cosmic balance will collapse!\"</i>\n\nThe observing constellations erupt into chaotic panic as you tap into the raw architectural code of reality. For an exorbitant fee of accumulated Cosmic Coins, they are willing to turn a blind eye and grant you truly game-breaking otherworldly cheats.\n\nSelect a forbidden cheat below:\n\n<b>Your balance:</b> {isekai_cosmic_coins} Cosmic Coins");
			blueprintCue6.Answers = new List<BlueprintAnswerBaseReference>();
		});
		BlueprintCue blueprintCue3 = TTCoreExtensions.CreateCue("CueStoreCosmicRelics", delegate(BlueprintCue blueprintCue6)
		{
			blueprintCue6.SetText(Main.IsekaiContext, "<color=#133E71><b>[Cosmic Relics & Sovereign Boons]</b></color>\n\nThe constellations draw back the celestial veil, presenting legendary stellar relics, ancient companions, and mythic rulebreaker pacts forged from the heart of dying stars.\n\nSelect a cosmic relic or boon:\n\n<b>Your balance:</b> {isekai_cosmic_coins} Cosmic Coins");
			blueprintCue6.Answers = new List<BlueprintAnswerBaseReference>();
		});
		BlueprintCue blueprintCue4 = TTCoreExtensions.CreateCue("CueStoreFealty", delegate(BlueprintCue blueprintCue6)
		{
			blueprintCue6.SetText(Main.IsekaiContext, "<color=#5B2A86><b>[Constellation Fealty & Avatar Ascension]</b></color>\n\nPledge to a sponsor deity for +25% patron donations (500 Cosmic Coins, once per week of travel), or ascend as your patron's Avatar (10,000 Cosmic Coins). You hold one avatar at a time: switching patron removes it with no refund. A patron who would forbid your divine spellbook or refuses your class is not offered. A seat you have deeply offended pays no patron bonus and halves its sponsorship until you make amends.\n\n<b>Your balance:</b> {isekai_cosmic_coins} Cosmic Coins");
			blueprintCue6.Answers = new List<BlueprintAnswerBaseReference>();
		});
		BlueprintCue blueprintCue5 = TTCoreExtensions.CreateCue("CueStoreGodDefier", delegate(BlueprintCue blueprintCue6)
		{
			blueprintCue6.SetText(Main.IsekaiContext, "<color=#5B2A86><b>[Path of the God Defier]</b></color>\n\n<i>\"We bow to no throne and worship no star. Our fate belongs only to us.\"</i>\n\nReject all divine authority and constellation patronage. Gain a +4 bonus to all ability scores, immunity to curses and death effects, and a +8 bonus to damage rolls.\n\n<b>Requirements:</b> Atheism & 10,000 Cosmic Coins. You embody one avatar at a time, so this is refused while you hold another; switching patron gives an avatar up, with no refund.\n\n<b>Your balance:</b> {isekai_cosmic_coins} Cosmic Coins");
			blueprintCue6.Answers = new List<BlueprintAnswerBaseReference>();
		});
		BlueprintCue CosmicStoreRootCue = TTCoreExtensions.CreateCue("CosmicStoreRootCue", delegate(BlueprintCue blueprintCue6)
		{
			blueprintCue6.SetText(Main.IsekaiContext, "<color=#5B2A86><b>[The Cosmic Constellation Exchange]</b></color>\n\nSoft starlight crystallizes around you into a shimmering celestial interface. The observing constellations watch with eager anticipation as your accumulated Cosmic Coins hum with multiversal energy.\n\nSelect a catalog category below:\n\n<b>Your balance:</b> {isekai_cosmic_coins} Cosmic Coins");
			blueprintCue6.Answers = new List<BlueprintAnswerBaseReference>();
		});
		BlueprintDialog CosmicStoreDialog = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicStoreDialog", delegate(BlueprintDialog blueprintDialog)
		{
			blueprintDialog.FirstCue = new CueSelection
			{
				Cues = new List<BlueprintCueBaseReference> { CosmicStoreRootCue.ToReference<BlueprintCueBaseReference>() },
				Strategy = Strategy.First
			};
			blueprintDialog.Type = DialogType.Common;
			blueprintDialog.TurnPlayer = false;
			blueprintDialog.TurnFirstSpeaker = false;
			blueprintDialog.Conditions = ActionFlow.EmptyCondition();
			blueprintDialog.StartActions = ActionFlow.DoNothing();
			blueprintDialog.FinishActions = ActionFlow.DoNothing();
			blueprintDialog.ReplaceActions = ActionFlow.DoNothing();
		});
		BlueprintFeature KeyOfInfinitePathwaysFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "KeyOfInfinitePathwaysFeature", delegate(BlueprintFeature blueprintFeature3)
		{
			blueprintFeature3.SetName(Main.IsekaiContext, "Key of Infinite Pathways");
			blueprintFeature3.SetDescription(Main.IsekaiContext, "A metaphysical skeleton key that transcends dimensional timelines and cosmic law. Unlocks all alignment and mythic path restrictions within the Cosmic Sponsorship Store, allowing you to acquire boons from any mythic path simultaneously.");
			((BlueprintUnitFact)blueprintFeature3).m_Icon = Icon_Coin;
			blueprintFeature3.IsClassFeature = false;
		});
		AddBlessing("CaydenReserveBrew", "AnswerBuyBlessingCayden", "Cayden's Reserve Brew", "Grants immunity to fatigue and exhaustion, and a +2 morale bonus to attack and damage rolls.", 100, delegate(BlueprintBuff obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Morale;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Morale;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Fatigued;
			});
			obj2.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Exhausted;
			});
		});
		AddBlessing("StarlightAegis", "AnswerBuyBlessingDesna", "Starlight Aegis of Desna", "Grants a +2 bonus to AC and all saving throws that stacks with everything, including Plot Armor.", 250, delegate(BlueprintBuff obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AC;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
		});
		AddBlessing("EssenceOfLurker", "AnswerBuyBlessingYogSothoth", "Essence of the Lurker", "Grants a +2 profane bonus to the DC of Conjuration, Transmutation, and Illusion spells.", 500, delegate(BlueprintBuff obj2)
		{
			obj2.AddComponent(delegate(IncreaseSpellSchoolDC c)
			{
				c.School = SpellSchool.Conjuration;
				c.BonusDC = 2;
				c.Descriptor = ModifierDescriptor.Profane;
			});
			obj2.AddComponent(delegate(IncreaseSpellSchoolDC c)
			{
				c.School = SpellSchool.Transmutation;
				c.BonusDC = 2;
				c.Descriptor = ModifierDescriptor.Profane;
			});
			obj2.AddComponent(delegate(IncreaseSpellSchoolDC c)
			{
				c.School = SpellSchool.Illusion;
				c.BonusDC = 2;
				c.Descriptor = ModifierDescriptor.Profane;
			});
		});
		AddBlessing("LanternTricksterGlow", "AnswerBuyBlessingLanternKing", "Lantern King's Prankster Veil", "Grants a +3 competence bonus to Trickery and Stealth, plus 20% concealment.", 300, delegate(BlueprintBuff obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillThievery;
				c.Value = 3;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.SkillStealth;
				c.Value = 3;
			});
			obj2.AddComponent(delegate(AddConcealment c)
			{
				c.Concealment = Concealment.Partial;
			});
		});
		AddBlessing("QueenOfTheSeasFavor", "AnswerBuyBlessingBesmara", "Favor of the Pirate Queen", "Grants a +10 ft enhancement bonus to speed, immunity to paralysis, and a +2 morale bonus on attack rolls.", 300, delegate(BlueprintBuff obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Enhancement;
				c.Stat = StatType.Speed;
				c.Value = 10;
			});
			obj2.AddComponent(delegate(AddConditionImmunity c)
			{
				c.Condition = UnitCondition.Paralyzed;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Morale;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 2;
			});
		});
		AddBlessing("ChaldiraHeartyLuck", "AnswerBuyBlessingChaldira", "Chaldira's Audacious Fortune", "Grants a +2 bonus on critical confirmation rolls, a +2 luck bonus to initiative, and a +2 bonus to all saving throws that stacks with everything, including Plot Armor.", 350, delegate(BlueprintBuff obj2)
		{
			obj2.AddComponent(delegate(CriticalConfirmationBonus c)
			{
				c.Value = 0;
				c.Bonus = 2;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Luck;
				c.Stat = StatType.Initiative;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
		});
		AddBlessing("IronLordFury", "AnswerBuyBlessingGorum", "Fury of Our Lord in Iron", "Grants a +4 morale bonus to damage rolls and a +2 morale bonus to Combat Maneuver Bonus.", 400, delegate(BlueprintBuff obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Morale;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 4;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Morale;
				c.Stat = StatType.AdditionalCMB;
				c.Value = 2;
			});
		});
		AddBlessing("AllSeeingGlyph", "AnswerBuyBlessingNethys", "All-Seeing Glyph of Nethys", "Grants a +4 insight bonus on caster level checks to overcome spell resistance.", 500, delegate(BlueprintBuff obj2)
		{
			obj2.AddComponent(delegate(SpellPenetrationBonus c)
			{
				c.Value = 4;
				c.Descriptor = ModifierDescriptor.Insight;
			});
		});
		AddBlessing("KissOfTheSavoredSting", "AnswerBuyBlessingCalistria", "Kiss of the Savored Sting", "Grants 3 extra attacks of opportunity per round and a +4 profane bonus to damage rolls.", 350, delegate(BlueprintBuff obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Profane;
				c.Stat = StatType.AttackOfOpportunityCount;
				c.Value = 3;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Profane;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 4;
			});
		});
		AddBlessing("LadyOfGravesWarding", "AnswerBuyBlessingPharasma", "Warding of the Lady of Graves", "Grants immunity to death effects and curses.", 450, delegate(BlueprintBuff obj2)
		{
			obj2.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Curse | SpellDescriptor.Death;
			});
			obj2.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Curse | SpellDescriptor.Death;
			});
		});
		AddBlessing("InheritorVanguard", "AnswerBuyBlessingIomedae", "Vanguard of the Inheritor", "Grants a +3 sacred bonus to AC and all saving throws. Sacred bonuses do not stack: Iomedae's avatar overwrites this blessing.", 450, delegate(BlueprintBuff obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.AC;
				c.Value = 3;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveFortitude;
				c.Value = 3;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveReflex;
				c.Value = 3;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveWill;
				c.Value = 3;
			});
		});
		AddBlessing("ArchfiendsCovenant", "AnswerBuyBlessingAsmodeus", "Covenant of the Archfiend", "Grants a +4 profane bonus to Persuasion checks.", 500, delegate(BlueprintBuff obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Profane;
				c.Stat = StatType.SkillPersuasion;
				c.Value = 4;
			});
		});
		AddBlessing("RoseGuardsResolve", "AnswerBuyBlessingMilani", "Resolve of the Rose Guard", "Grants immunity to fear, charm, and compulsion effects, plus a +2 morale bonus on attack rolls.", 300, delegate(BlueprintBuff obj2)
		{
			obj2.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Compulsion | SpellDescriptor.Charm;
			});
			obj2.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.Fear | SpellDescriptor.Compulsion | SpellDescriptor.Charm;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Morale;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 2;
			});
		});
		AddBlessing("SilentShroud", "AnswerBuyBlessingBlackButterfly", "Silent Shroud of the Butterfly", "Grants immunity to gaze attacks and sight-based effects.", 400, delegate(BlueprintBuff obj2)
		{
			obj2.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.SightBased | SpellDescriptor.GazeAttack;
			});
		});
		AddBackAndLeave(CueStoreBlessings, "AnswerBackFromBlessings", "AnswerLeaveFromBlessings");
		AddMythicBoon("BoonOfHeavenlyHost", "AnswerBuyMythicAngel", "Boon of the Heavenly Host (Angel)", "Grants a permanent +4 sacred bonus to AC and a +7 sacred bonus to damage rolls.", 1500, delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 7;
			});
		});
		AddMythicBoon("BoonOfAbyssalRage", "AnswerBuyMythicDemon", "Boon of Abyssal Rage (Demon)", "Grants a permanent +4 profane bonus to Strength and Constitution.", 1500, delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Profane;
				c.Stat = StatType.Strength;
				c.Value = 4;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Profane;
				c.Stat = StatType.Constitution;
				c.Value = 4;
			});
		});
		AddMythicBoon("BoonOfLordOfBones", "AnswerBuyMythicLich", "Boon of the Lord of Bones (Lich)", "Grants permanent DR 10/adamantite.", 1500, delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 10;
				c.BypassedByMaterial = true;
				c.Material = PhysicalDamageMaterial.Adamantite;
			});
		});
		AddMythicBoon("BoonOfWildWonder", "AnswerBuyMythicAzata", "Boon of Wild Wonder (Azata)", "Grants a permanent +3 morale bonus to all saving throws and +1 extra attack per round that stacks with Haste.", 1500, delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Morale;
				c.Stat = StatType.SaveFortitude;
				c.Value = 3;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Morale;
				c.Stat = StatType.SaveReflex;
				c.Value = 3;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Morale;
				c.Stat = StatType.SaveWill;
				c.Value = 3;
			});
			obj2.AddComponent(delegate(BuffExtraAttack c)
			{
				c.Number = 1;
				c.Haste = false;
			});
		});
		AddMythicBoon("BoonOfCosmicJester", "AnswerBuyMythicTrickster", "Boon of the Cosmic Jester (Trickster)", "Grants +2d6 sneak attack damage.", 1500, delegate(BlueprintFeature obj2)
		{
			BlueprintFeature sneak = BlueprintTools.GetBlueprint<BlueprintFeature>("9b9eac6709e1c084cb18c3a366e0ec87");
			obj2.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[2]
				{
					sneak.ToReference<BlueprintUnitFactReference>(),
					sneak.ToReference<BlueprintUnitFactReference>()
				};
			});
		});
		AddMythicBoon("BoonOfMonadicOrder", "AnswerBuyMythicAeon", "Boon of Monadic Order (Aeon)", "Grants a permanent +4 insight bonus to attack rolls and AC, plus immunity to mind-affecting effects.", 1500, delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 4;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.AC;
				c.Value = 4;
			});
			obj2.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.MindAffecting;
			});
		});
		AddMythicBoon("BoonOfHellfireContract", "AnswerBuyMythicDevil", "Boon of Hellfire Contract (Devil)", "Grants fire resistance 100.", 1500, delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddDamageResistanceEnergy c)
			{
				c.Type = DamageEnergyType.Fire;
				c.Value = 100;
			});
		});
		AddMythicBoon("BoonOfGoldenDragon", "AnswerBuyMythicGoldDragon", "Boon of the Golden Dragon (Gold Dragon)", "Grants a permanent +4 inherent bonus to all 6 ability scores.", 2000, delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Strength;
				c.Value = 4;
			});
			obj2.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Dexterity;
				c.Value = 4;
			});
			obj2.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Constitution;
				c.Value = 4;
			});
			obj2.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Intelligence;
				c.Value = 4;
			});
			obj2.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Wisdom;
				c.Value = 4;
			});
			obj2.AddComponent(delegate(AddStackingStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Inherent;
				c.Stat = StatType.Charisma;
				c.Value = 4;
			});
		});
		AddMythicBoon("BoonOfPrimevalSwarm", "AnswerBuyMythicSwarm", "Boon of the Primeval Swarm (Swarm)", "Grants permanent DR 10/- and 20% concealment.", 1500, delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddDamageResistancePhysical c)
			{
				c.Value = 10;
			});
			obj2.AddComponent(delegate(AddConcealment c)
			{
				c.Concealment = Concealment.Partial;
			});
		});
		AddMythicBoon("BoonOfUnboundMortal", "AnswerBuyMythicLegend", "Boon of the Unbound Mortal (Legend)", "Grants a permanent +4 bonus to all saving throws and all ability scores.", 2000, delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Other;
				c.Stat = StatType.SaveFortitude;
				c.Value = 4;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Other;
				c.Stat = StatType.SaveReflex;
				c.Value = 4;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Other;
				c.Stat = StatType.SaveWill;
				c.Value = 4;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Other;
				c.Stat = StatType.Strength;
				c.Value = 4;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Other;
				c.Stat = StatType.Dexterity;
				c.Value = 4;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Other;
				c.Stat = StatType.Constitution;
				c.Value = 4;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Other;
				c.Stat = StatType.Intelligence;
				c.Value = 4;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Other;
				c.Stat = StatType.Wisdom;
				c.Value = 4;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Other;
				c.Stat = StatType.Charisma;
				c.Value = 4;
			});
		});
		AddBackAndLeave(CueStoreMythic, "AnswerBackFromMythic", "AnswerLeaveFromMythic");
		BlueprintBuff bp = TTCoreExtensions.CreateBuff("PaperFigurineBuff", delegate(BlueprintBuff blueprintBuff)
		{
			blueprintBuff.SetName(Main.IsekaiContext, "Paper Figurine of Substitute Life");
			blueprintBuff.SetDescription(Main.IsekaiContext, "A mystical talisman woven from transmigration paper. Wards you with 100 temporary hit points.");
			((BlueprintUnitFact)blueprintBuff).m_Icon = Icon_Coin;
			blueprintBuff.AddComponent(delegate(TemporaryHitPointsFromAbilityValue c)
			{
				c.Value = 100;
			});
			blueprintBuff.m_Flags = BlueprintBuff.Flags.StayOnDeath;
		});
		BlueprintBuff bp2 = TTCoreExtensions.CreateBuff("DraughtOfAbsoluteSoulAnchorBuff", delegate(BlueprintBuff blueprintBuff)
		{
			blueprintBuff.SetName(Main.IsekaiContext, "Soul Anchor Blessing");
			blueprintBuff.SetDescription(Main.IsekaiContext, "Anchors your astral soul firmly to your physical vessel. Immune to death effects, curses, and mind-affecting effects.");
			((BlueprintUnitFact)blueprintBuff).m_Icon = Icon_Coin;
			blueprintBuff.AddComponent(delegate(BuffDescriptorImmunity c)
			{
				c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Curse | SpellDescriptor.Death;
			});
			blueprintBuff.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Curse | SpellDescriptor.Death;
			});
			blueprintBuff.m_Flags = BlueprintBuff.Flags.StayOnDeath;
		});
		BlueprintFeature bp3 = Helpers.CreateBlueprint(Main.IsekaiContext, "TransmigratorMasteryFeature", delegate(BlueprintFeature blueprintFeature3)
		{
			blueprintFeature3.SetName(Main.IsekaiContext, "Transmigrator's Martial Mastery");
			blueprintFeature3.SetDescription(Main.IsekaiContext, "Grants a permanent +2 competence bonus to all attack and damage rolls.");
			((BlueprintUnitFact)blueprintFeature3).m_Icon = Icon_Coin;
			blueprintFeature3.IsClassFeature = false;
			blueprintFeature3.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = 2;
			});
			blueprintFeature3.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 2;
			});
		});
		BlueprintAbilityResource CosmicWishResource = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicWishResource", delegate(BlueprintAbilityResource blueprintAbilityResource)
		{
			blueprintAbilityResource.m_MaxAmount = new BlueprintAbilityResource.Amount
			{
				BaseValue = 10,
				IncreasedByLevel = false,
				IncreasedByStat = false
			};
		});
		CosmicWishCharges.WishResource = CosmicWishResource;
		BlueprintAbility WishStrength = CreateWishVariant("WishStrengthAbility", "Strength", StatType.Strength, variantIcon);
		BlueprintAbility WishDexterity = CreateWishVariant("WishDexterityAbility", "Dexterity", StatType.Dexterity, variantIcon2);
		BlueprintAbility WishConstitution = CreateWishVariant("WishConstitutionAbility", "Constitution", StatType.Constitution, variantIcon3);
		BlueprintAbility WishIntelligence = CreateWishVariant("WishIntelligenceAbility", "Intelligence", StatType.Intelligence, variantIcon4);
		BlueprintAbility WishWisdom = CreateWishVariant("WishWisdomAbility", "Wisdom", StatType.Wisdom, variantIcon5);
		BlueprintAbility WishCharisma = CreateWishVariant("WishCharismaAbility", "Charisma", StatType.Charisma, variantIcon6);
		BlueprintAbility WishWealth = Helpers.CreateBlueprint(Main.IsekaiContext, "WishWealthAbility", delegate(BlueprintAbility blueprintAbility)
		{
			blueprintAbility.SetName(Main.IsekaiContext, "Cosmic Wish: 100,000 Gold");
			blueprintAbility.SetDescription(Main.IsekaiContext, "Consumes 1 Wish charge to instantly manifest 100,000 Gold into your party purse.");
			((BlueprintUnitFact)blueprintAbility).m_Icon = Icon_WishWealth ?? Icon_Coin;
			blueprintAbility.Type = AbilityType.Special;
			blueprintAbility.Range = AbilityRange.Personal;
			blueprintAbility.CanTargetSelf = true;
			blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
			blueprintAbility.ActionType = UnitCommand.CommandType.Standard;
			blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = ActionFlow.DoSingle<ContextActionWishWealth>();
			});
			blueprintAbility.AddComponent(delegate(AbilityResourceLogic c)
			{
				c.m_RequiredResource = CosmicWishResource.ToReference<BlueprintAbilityResourceReference>();
				c.m_IsSpendResource = true;
				c.Amount = 1;
			});
		});
		Helpers.CreateBlueprint(Main.IsekaiContext, "ModularCosmicWishAbility", delegate(BlueprintAbility blueprintAbility)
		{
			blueprintAbility.SetName(Main.IsekaiContext, "Cosmic Wish (Field Cast)");
			blueprintAbility.SetDescription(Main.IsekaiContext, "Invoke cosmic reality manipulation. Consumes 1 Wish charge per cast. Requires purchasing a Wish recharge in the Cosmic Store to use again.");
			((BlueprintUnitFact)blueprintAbility).m_Icon = Icon_CosmicWish ?? Icon_Coin;
			blueprintAbility.Type = AbilityType.Special;
			blueprintAbility.Range = AbilityRange.Personal;
			blueprintAbility.CanTargetSelf = true;
			blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
			blueprintAbility.ActionType = UnitCommand.CommandType.Standard;
			blueprintAbility.AddComponent(delegate(AbilityVariants c)
			{
				c.m_Variants = new BlueprintAbilityReference[7]
				{
					WishStrength.ToReference<BlueprintAbilityReference>(),
					WishDexterity.ToReference<BlueprintAbilityReference>(),
					WishConstitution.ToReference<BlueprintAbilityReference>(),
					WishIntelligence.ToReference<BlueprintAbilityReference>(),
					WishWisdom.ToReference<BlueprintAbilityReference>(),
					WishCharisma.ToReference<BlueprintAbilityReference>(),
					WishWealth.ToReference<BlueprintAbilityReference>()
				};
			});
		});
		BlueprintAnswer answer = CreateStoreAnswer("AnswerBuyElixirRejuv", "(400 Coins) [Elixir of Instant Rejuvenation] Fully restores HP, spell slots, and class ability uses.", blueprintCue, new ContextActionRejuvenation(), 400, Array.Empty<Condition>());
		BlueprintAnswer answer2 = CreateStoreAnswer("AnswerBuyPaperFigurine", "(750 Coins) [Paper Figurine of Substitute Life] Grants 100 temporary HP for 24h.", blueprintCue, new ContextActionSpendCoinsAndApplyBuff
		{
			Cost = 750,
			m_Buff = bp.ToReference<BlueprintBuffReference>(),
			DurationHours = 24
		}, 750, Array.Empty<Condition>());
		BlueprintAnswer answer3 = CreateStoreAnswer("AnswerBuyDraughtSoulAnchor", "(500 Coins) [Soul Anchor Blessing] Immune to death effects, curses, and mind-affecting effects for 24h.", blueprintCue, new ContextActionSpendCoinsAndApplyBuff
		{
			Cost = 500,
			m_Buff = bp2.ToReference<BlueprintBuffReference>(),
			DurationHours = 24
		}, 500, Array.Empty<Condition>());
		BlueprintAnswer answer4 = CreateStoreAnswer("AnswerBuyTomeMastery", "(1,000 Coins) [Tome of Transmigrator's Mastery] Permanent +2 competence bonus to attack and damage rolls.", blueprintCue, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 1000,
			m_Fact = bp3.ToReference<BlueprintUnitFactReference>()
		}, 1000, Array.Empty<Condition>());
		BlueprintUnitFactReference blueprintUnitFactReference = ((CosmicSponsorshipExchangeFeature != null) ? CosmicSponsorshipExchangeFeature.ToReference<BlueprintUnitFactReference>() : GuardedFact("CosmicSponsorshipExchangeFeature"));
		BlueprintAnswer answer5 = ((blueprintUnitFactReference == null) ? null : CreateStoreAnswer("AnswerBuyWishCharge", "(1,000 Coins) [Cosmic Wish Recharge] Adds 1 charge to your field-cast Cosmic Wish ability.", blueprintCue, new ContextActionBuyWishCharge
		{
			Cost = 1000,
			m_WishResource = CosmicWishResource.ToReference<BlueprintAbilityResourceReference>()
		}, 1000, new Condition[1] { MainCharacterHas(blueprintUnitFactReference) }));
		BlueprintUnitFactReference blueprintUnitFactReference2 = GuardedFact("SkillBookGrandScholarFeature");
		BlueprintUnitFactReference blueprintUnitFactReference3 = GuardedFact("SkillBookPhantomInfiltratorFeature");
		BlueprintUnitFactReference blueprintUnitFactReference4 = GuardedFact("SkillBookAbsoluteCharismaFeature");
		BlueprintUnitFactReference blueprintUnitFactReference5 = GuardedFact("SkillBookWindWalkerFeature");
		BlueprintUnitFactReference blueprintUnitFactReference6 = GuardedFact("SkillBookEyeOfProvidenceFeature");
		BlueprintAnswer answer6 = ((blueprintUnitFactReference2 == null) ? null : CreateStoreAnswer("AnswerBuyBookGrandScholar", "(800 Coins) [Grimoire of the Grand Scholar] Permanent +4 Knowledge/Lore, +1 Caster Level.", blueprintCue, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 800,
			m_Fact = blueprintUnitFactReference2
		}, 800, Array.Empty<Condition>()));
		BlueprintAnswer answer7 = ((blueprintUnitFactReference3 == null) ? null : CreateStoreAnswer("AnswerBuyBookPhantomInfiltrator", "(800 Coins) [Manual of the Phantom Infiltrator] Permanent +4 Stealth/Trickery, +1d6 Sneak Attack.", blueprintCue, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 800,
			m_Fact = blueprintUnitFactReference3
		}, 800, Array.Empty<Condition>()));
		BlueprintAnswer answer8 = ((blueprintUnitFactReference4 == null) ? null : CreateStoreAnswer("AnswerBuyBookAbsoluteCharisma", "(800 Coins) [Chronicle of Absolute Charisma] Permanent +4 Persuasion/UMD, +2 Enchantment/Illusion DC.", blueprintCue, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 800,
			m_Fact = blueprintUnitFactReference4
		}, 800, Array.Empty<Condition>()));
		BlueprintAnswer answer9 = ((blueprintUnitFactReference5 == null) ? null : CreateStoreAnswer("AnswerBuyBookWindWalker", "(800 Coins) [Codex of the Wind Walker] Permanent +4 Athletics/Mobility, +10 ft enhancement bonus to speed.", blueprintCue, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 800,
			m_Fact = blueprintUnitFactReference5
		}, 800, Array.Empty<Condition>()));
		BlueprintAnswer answer10 = ((blueprintUnitFactReference6 == null) ? null : CreateStoreAnswer("AnswerBuyBookEyeOfProvidence", "(800 Coins) [Tractate of the Eye of Providence] Permanent +4 Perception, immune to Blindness, uncanny dodge.", blueprintCue, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 800,
			m_Fact = blueprintUnitFactReference6
		}, 800, Array.Empty<Condition>()));
		List<BlueprintAnswer> obj = new List<BlueprintAnswer>
		{
			CreateStoreAnswer("AnswerBuyAmbrosiaMight", "(3,000 Coins) [Primordial Ambrosia of Might] Permanently grants +2 Untyped Strength (stacks infinitely).", blueprintCue, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3000,
				m_Item = Bottle("ItemPrimordialAmbrosiaMight")
			}, 3000, Array.Empty<Condition>()),
			CreateStoreAnswer("AnswerBuyAmbrosiaGrace", "(3,000 Coins) [Primordial Ambrosia of Grace] Permanently grants +2 Untyped Dexterity (stacks infinitely).", blueprintCue, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3000,
				m_Item = Bottle("ItemPrimordialAmbrosiaGrace")
			}, 3000, Array.Empty<Condition>()),
			CreateStoreAnswer("AnswerBuyAmbrosiaEndurance", "(3,000 Coins) [Primordial Ambrosia of Endurance] Permanently grants +2 Untyped Constitution (stacks infinitely).", blueprintCue, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3000,
				m_Item = Bottle("ItemPrimordialAmbrosiaEndurance")
			}, 3000, Array.Empty<Condition>()),
			CreateStoreAnswer("AnswerBuyAmbrosiaIntellect", "(3,000 Coins) [Primordial Ambrosia of Intellect] Permanently grants +2 Untyped Intelligence (stacks infinitely).", blueprintCue, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3000,
				m_Item = Bottle("ItemPrimordialAmbrosiaIntellect")
			}, 3000, Array.Empty<Condition>()),
			CreateStoreAnswer("AnswerBuyAmbrosiaInsight", "(3,000 Coins) [Primordial Ambrosia of Insight] Permanently grants +2 Untyped Wisdom (stacks infinitely).", blueprintCue, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3000,
				m_Item = Bottle("ItemPrimordialAmbrosiaInsight")
			}, 3000, Array.Empty<Condition>()),
			CreateStoreAnswer("AnswerBuyAmbrosiaMajesty", "(3,000 Coins) [Primordial Ambrosia of Majesty] Permanently grants +2 Untyped Charisma (stacks infinitely).", blueprintCue, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3000,
				m_Item = Bottle("ItemPrimordialAmbrosiaMajesty")
			}, 3000, Array.Empty<Condition>()),
			CreateStoreAnswer("AnswerBuyAmbrosiaCelerity", "(3,000 Coins) [Primordial Ambrosia of Celerity] Permanently grants +10 ft Untyped Base Speed (stacks infinitely).", blueprintCue, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3000,
				m_Item = Bottle("ItemPrimordialAmbrosiaCelerity")
			}, 3000, Array.Empty<Condition>()),
			CreateStoreAnswer("AnswerBuyAmbrosiaEternalRenewal", "(10,000 Coins) [Primordial Ambrosia of Eternal Renewal] Permanently grants Fast Healing 5 (Untyped Stackable, stacks infinitely).", blueprintCue, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 10000,
				m_Item = Bottle("ItemPrimordialAmbrosiaEternalRenewal")
			}, 10000, Array.Empty<Condition>()),
			CreateStoreAnswer("AnswerBuyNectarEndlessLife", "(1,500 Coins) [Nectar of Endless Life] Permanently grants +25 Untyped Maximum HP (stacks infinitely).", blueprintCue, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 1500,
				m_Item = Bottle("ItemNectarEndlessLife")
			}, 1500, Array.Empty<Condition>()),
			CreateStoreAnswer("AnswerBuyNectarVoidStalker", "(4,000 Coins) [Nectar of the Void Stalker] Permanently grants +1 Untyped Spell DC and +2 Spell Penetration (stacks infinitely).", blueprintCue, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 4000,
				m_Item = Bottle("ItemNectarVoidStalker")
			}, 4000, Array.Empty<Condition>()),
			CreateStoreAnswer("AnswerBuyNectarWorldBreaker", "(4,000 Coins) [Nectar of the World Breaker] Permanently grants +2 Untyped Attack and Damage (stacks infinitely).", blueprintCue, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 4000,
				m_Item = Bottle("ItemNectarWorldBreaker")
			}, 4000, Array.Empty<Condition>()),
			CreateStoreAnswer("AnswerBuyNectarDiamondSoul", "(3,500 Coins) [Nectar of the Diamond Soul] Permanently grants +2 Untyped to all Saving Throws (stacks infinitely).", blueprintCue, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3500,
				m_Item = Bottle("ItemNectarDiamondSoul")
			}, 3500, Array.Empty<Condition>()),
			CreateStoreAnswer("AnswerBuyNectarUnbrokenAegis", "(3,500 Coins) [Nectar of the Unbroken Aegis] Permanently grants +1 Untyped Armor Class (stacks infinitely).", blueprintCue, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3500,
				m_Item = Bottle("ItemNectarUnbrokenAegis")
			}, 3500, Array.Empty<Condition>()),
			CreateStoreAnswer("AnswerBuyDraughtVelocity", "(2,500 Coins) [Draught of Cosmic Velocity] +20 ft speed and +1 extra attack on full attack for 24 hours.", blueprintCue, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 2500,
				m_Item = Bottle("ItemDraughtCosmicVelocity")
			}, 2500, Array.Empty<Condition>()),
			CreateStoreAnswer("AnswerBuyDraughtImmortalTitan", "(2,500 Coins) [Draught of the Immortal Titan] +150 Temp HP, DR 10/--, and Fast Healing 10 for 24 hours.", blueprintCue, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 2500,
				m_Item = Bottle("ItemDraughtImmortalTitan")
			}, 2500, Array.Empty<Condition>()),
			CreateStoreAnswer("AnswerBuyDraughtArcaneOmniscience", "(3,000 Coins) [Draught of Arcane Omniscience] +4 Caster Level, and auto-extends and empowers all spells for 24 hours.", blueprintCue, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3000,
				m_Item = Bottle("ItemDraughtArcaneOmniscience")
			}, 3000, Array.Empty<Condition>()),
			CreateStoreAnswer("AnswerBuyDraughtApexPredator", "(3,000 Coins) [Draught of the Apex Predator] +1 Crit Multiplier, and bypasses all DR and Concealment for 24 hours.", blueprintCue, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 3000,
				m_Item = Bottle("ItemDraughtApexPredator")
			}, 3000, Array.Empty<Condition>()),
			CreateStoreAnswer("AnswerBuyDraughtSoulAnchorBottle", "(500 Coins) [Draught of Absolute Soul Anchor] Bottle granting death, negative level, and compulsion immunity for 24 hours.", blueprintCue, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 500,
				m_Item = Bottle("ItemDraughtAbsoluteSoulAnchor")
			}, 500, Array.Empty<Condition>()),
			CreateStoreAnswer("AnswerBuyWishstone", "(4,000 Coins) [Bottled Djinn Wishstone] Physical quickslot relic granting 5 reality-warping miracles. It is crushed once: choose one of the five.", blueprintCue, new ContextActionSpendCoinsAndGiveItem
			{
				Cost = 4000,
				m_Item = Bottle("ItemCosmicWishstone")
			}, 4000, Array.Empty<Condition>())
		};
		ListOffer(blueprintCue, answer);
		ListOffer(blueprintCue, answer2);
		ListOffer(blueprintCue, answer3);
		ListOffer(blueprintCue, answer4);
		ListOffer(blueprintCue, answer6);
		ListOffer(blueprintCue, answer7);
		ListOffer(blueprintCue, answer8);
		ListOffer(blueprintCue, answer9);
		ListOffer(blueprintCue, answer10);
		ListOffer(blueprintCue, answer5);
		foreach (BlueprintAnswer item in obj)
		{
			ListOffer(blueprintCue, item);
		}
		AddBackAndLeave(blueprintCue, "AnswerBackFromConsumables", "AnswerLeaveFromConsumables");
		AddInfusion("DragonScaleInfusion", "AnswerBuyInfusionDragonScale", "Dragon's Scale Infusion", "Permanently grants a +2 Natural Armor bonus to AC.", 800, delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.NaturalArmor;
				c.Stat = StatType.AC;
				c.Value = 2;
			});
		});
		AddInfusion("EyeOfCosmosInfusion", "AnswerBuyInfusionEyeOfCosmos", "Eye of the Cosmos Infusion", "Permanently grants True Seeing.", 800, delegate(BlueprintFeature obj2)
		{
			BlueprintBuff trueSeeing = BlueprintTools.GetBlueprint<BlueprintBuff>("09b4b69169304474296484c74aa12027");
			if (trueSeeing != null)
			{
				obj2.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { trueSeeing.ToReference<BlueprintUnitFactReference>() };
				});
			}
		});
		AddInfusion("StarlightSwiftnessInfusion", "AnswerBuyInfusionStarlightSwiftness", "Starlight Swiftness Infusion", "Permanently grants a +10 ft bonus to movement speed and +2 to Initiative.", 800, delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Enhancement;
				c.Stat = StatType.Speed;
				c.Value = 10;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.Initiative;
				c.Value = 2;
			});
		});
		AddInfusion("CosmicFortitudeInfusion", "AnswerBuyInfusionCosmicFortitude", "Cosmic Fortitude Infusion", "Permanently grants +20 maximum hit points.", 800, delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.HitPoints;
				c.Value = 20;
			});
		});
		AddInfusion("AstralWeaponAttunement", "AnswerBuyInfusionAstralWeapon", "Astral Weapon Attunement", "Permanently grants a +5 bonus to damage rolls.", 1200, delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalDamage;
				c.Value = 5;
			});
		});
		ListOffer(CueStoreInfusions, CreateStoreAnswer("AnswerBuyKeyOfInfinitePathways", "(5,000 Coins) [Meta-Boon: Key of Infinite Pathways] Lifts the alignment requirement of every mythic path boon in the Cosmic Store.", CueStoreInfusions, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 5000,
			m_Fact = KeyOfInfinitePathwaysFeature.ToReference<BlueprintUnitFactReference>()
		}, 5000, Array.Empty<Condition>()));
		AddSubclassBlessing("IsekaiTranscendence", "AnswerBuyIsekaiTranscendence", "Isekai Transcendence", "Isekai Protagonist", "Grants a +2 bonus to AC and all saving throws that stacks with everything, including Plot Armor.", "IsekaiProficiencies", delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AC;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
		});
		AddSubclassBlessing("PhantomTranscendence", "AnswerBuyPhantomTranscendence", "Phantom Transcendence", "Phantom Blade / Martial God", "Grants a +10 ft bonus to speed, +2 Dodge AC, and an extra attack on a full attack.", "PhantomBladeProficiencies", delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Enhancement;
				c.Stat = StatType.Speed;
				c.Value = 10;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Dodge;
				c.Stat = StatType.AC;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(BuffExtraAttack c)
			{
				c.Number = 1;
				c.Haste = false;
			});
		});
		AddSubclassBlessing("ImperialTranscendence", "AnswerBuyImperialTranscendence", "Imperial Transcendence", "God Emperor", "Grants a +2 Sacred bonus to Intelligence, Wisdom, and Charisma.", "GodEmperorProficiencies", delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.Intelligence;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.Wisdom;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.Charisma;
				c.Value = 2;
			});
		});
		AddSubclassBlessing("SupremeTranscendence", "AnswerBuySupremeTranscendence", "Supreme Transcendence", "Overlord", "Increases the DC of all spells by 2.", "OverlordProficiencies", delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(IncreaseAllSpellsDC c)
			{
				c.Value = 2;
				c.Descriptor = ModifierDescriptor.UntypedStackable;
			});
		});
		AddSubclassBlessing("ApexTranscendence", "AnswerBuyApexTranscendence", "Apex Transcendence", "Slime", "Grants +20 maximum hit points and a +2 Enhancement bonus to Strength and Constitution.", "DevourerProficiencies", delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.HitPoints;
				c.Value = 20;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Enhancement;
				c.Stat = StatType.Strength;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Enhancement;
				c.Stat = StatType.Constitution;
				c.Value = 2;
			});
		});
		AddSubclassBlessing("MonarchTranscendence", "AnswerBuyMonarchTranscendence", "Monarch Transcendence", "Shadow Monarch", "Grants a +2 Insight bonus to AC, Fortitude saves, and Will saves.", "ShadowMonarchProficiencies", delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.AC;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
		});
		AddSubclassBlessing("FellowshipTranscendence", "AnswerBuyFellowshipTranscendence", "Fellowship Transcendence", "Hero", "Grants a +2 Sacred bonus to saving throws and Fast Healing 3.", "HeroProficiencies", delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveFortitude;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveReflex;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Sacred;
				c.Stat = StatType.SaveWill;
				c.Value = 2;
			});
			obj2.AddComponent(delegate(AddEffectFastHealing c)
			{
				c.Heal = 3;
				c.Bonus = 0;
			});
		});
		AddSubclassBlessing("GrandmasterTranscendence", "AnswerBuyGrandmasterTranscendence", "Grandmaster Transcendence", "Mastermind", "Grants a +4 Insight bonus to Initiative and +2d6 Sneak Attack damage.", "MastermindProficiencies", delegate(BlueprintFeature obj2)
		{
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.Initiative;
				c.Value = 4;
			});
			obj2.AddComponent(delegate(AddStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Insight;
				c.Stat = StatType.SneakAttack;
				c.Value = 2;
			});
		});
		AddBackAndLeave(CueStoreInfusions, "AnswerBackFromInfusions", "AnswerLeaveFromInfusions");
		BlueprintUnitFactReference blueprintUnitFactReference7 = GuardedFact("InstakillFeature");
		BlueprintUnitFactReference blueprintUnitFactReference8 = GuardedFact("SuperBuffFeature");
		BlueprintUnitFactReference blueprintUnitFactReference9 = GuardedFact("SummonCalamityFeature");
		BlueprintUnitFactReference blueprintUnitFactReference10 = GuardedFact("CosmicAutoQuickenFeature");
		BlueprintUnitFactReference blueprintUnitFactReference11 = GuardedFact("CosmicSupremeBeingFeature");
		BlueprintUnitFactReference blueprintUnitFactReference12 = GuardedFact("CosmicChronoDomainFeature");
		BlueprintUnitFactReference blueprintUnitFactReference13 = GuardedFact("CosmicChronoSurgeFeature");
		BlueprintAnswer answer11 = ((blueprintUnitFactReference7 == null) ? null : CreateStoreAnswer("AnswerBuyCheatInstakill", "(5,000 Coins) [Death Note: Instakill] A daily death decree: a foe that fails a Fortitude save is slain (a foe immune to death effects takes heavy unholy damage instead), and one that succeeds is stunned for 1 round.", blueprintCue2, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 5000,
			m_Fact = blueprintUnitFactReference7,
			RecordForRespec = true
		}, 5000, Array.Empty<Condition>()));
		BlueprintAnswer answer12 = ((blueprintUnitFactReference8 == null) ? null : CreateStoreAnswer("AnswerBuyCheatSuperBuff", "(4,000 Coins) [Omnipotent Buff Matrix: Super Buff] A daily ability that mantles you and nearby allies in divine, arcane, and primal buffs. The buffs and their duration grow with your level, from 1 hour up to 24 hours at level 17.", blueprintCue2, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 4000,
			m_Fact = blueprintUnitFactReference8,
			RecordForRespec = true
		}, 4000, Array.Empty<Condition>()));
		BlueprintAnswer answer13 = ((blueprintUnitFactReference10 == null) ? null : CreateStoreAnswer("AnswerBuyCheatAutoQuicken", "(5,000 Coins) [Cosmic Quicken Matrix: Unrestricted Auto-Quicken] While active, every spell you cast is quickened, whatever its level, with no character level requirement.", blueprintCue2, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 5000,
			m_Fact = blueprintUnitFactReference10,
			RecordForRespec = true
		}, 5000, Array.Empty<Condition>()));
		BlueprintAnswer answer14 = ((blueprintUnitFactReference11 == null) ? null : CreateStoreAnswer("AnswerBuyCheatSupremeBeing", "(6,000 Coins) [Crown of the Supreme Entity: Unbound Supreme Being] Gain an untyped bonus to all six attributes: +5, plus +1 per 2 character levels and +1 per mythic rank.", blueprintCue2, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 6000,
			m_Fact = blueprintUnitFactReference11,
			RecordForRespec = true
		}, 6000, Array.Empty<Condition>()));
		BlueprintAnswer answer15 = ((blueprintUnitFactReference12 == null) ? null : CreateStoreAnswer("AnswerBuyCheatChronoDomain", "(4,500 Coins) [Chronos Absolute Stasis: Unconditional Time Stop] Freeze all nearby enemies in time for 2 rounds as a swift action with NO saving throw and NO spell resistance.", blueprintCue2, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 4500,
			m_Fact = blueprintUnitFactReference12,
			RecordForRespec = true
		}, 4500, Array.Empty<Condition>()));
		BlueprintAnswer answer16 = ((blueprintUnitFactReference13 == null) ? null : CreateStoreAnswer("AnswerBuyCheatChronoSurge", "(15,000 Coins) [Protagonist Chrono-Surge: Double Turn] Gain the ability to refund your standard, move, and swift actions as a free action 3 times per day.", blueprintCue2, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 15000,
			m_Fact = blueprintUnitFactReference13,
			RecordForRespec = true
		}, 15000, Array.Empty<Condition>()));
		BlueprintAnswer answer17 = ((blueprintUnitFactReference9 == null) ? null : CreateStoreAnswer("AnswerBuyCheatSummonCalamity", "(5,000 Coins) [Forbidden Calamity Horn: Summon Calamity] Summon Devastators, Playful Darkness, Deskari, Baphomet, or Nocticula to fight by your side regardless of level.", blueprintCue2, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 5000,
			m_Fact = blueprintUnitFactReference9,
			RecordForRespec = true
		}, 5000, Array.Empty<Condition>()));
		ListOffer(blueprintCue2, answer11);
		ListOffer(blueprintCue2, answer12);
		ListOffer(blueprintCue2, answer13);
		ListOffer(blueprintCue2, answer14);
		ListOffer(blueprintCue2, answer15);
		ListOffer(blueprintCue2, answer16);
		ListOffer(blueprintCue2, answer17);
		BlueprintUnitFactReference blueprintUnitFactReference14 = GuardedFact("ManaWellspringFeature");
		BlueprintUnitFactReference blueprintUnitFactReference15 = GuardedFact("RealityPiercerFeature");
		BlueprintUnitFactReference blueprintUnitFactReference16 = GuardedFact("AegisUndyingFeature");
		BlueprintUnitFactReference blueprintUnitFactReference17 = GuardedFact("DimensionalSanctuaryFeature");
		ListOffer(blueprintCue2, (blueprintUnitFactReference14 == null) ? null : CreateStoreAnswer("AnswerBuyCheatManaWellspring", "(5,000 Coins) [Mana Wellspring: Reservoir of the Infinite] Gain +4 additional spell slots per day for all spell circles 1st through 9th.", blueprintCue2, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 5000,
			m_Fact = blueprintUnitFactReference14,
			RecordForRespec = true
		}, 5000, Array.Empty<Condition>()));
		ListOffer(blueprintCue2, (blueprintUnitFactReference15 == null) ? null : CreateStoreAnswer("AnswerBuyCheatRealityPiercer", "(10,000 Coins) [Reality Piercer: All-Sovereign Edge] Completely ignore all Damage Reduction and Concealment, and gain +6 Spell Penetration.", blueprintCue2, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 10000,
			m_Fact = blueprintUnitFactReference15,
			RecordForRespec = true
		}, 10000, Array.Empty<Condition>()));
		ListOffer(blueprintCue2, (blueprintUnitFactReference16 == null) ? null : CreateStoreAnswer("AnswerBuyCheatAegisUndying", "(6,000 Coins) [Aegis of the Undying Protagonist] Falling below 25% HP triggers emergency 150 HP healing and Greater Invisibility for 3 rounds (6-second cooldown).", blueprintCue2, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 6000,
			m_Fact = blueprintUnitFactReference16,
			RecordForRespec = true
		}, 6000, Array.Empty<Condition>()));
		ListOffer(blueprintCue2, (blueprintUnitFactReference17 == null) ? null : CreateStoreAnswer("AnswerBuyCheatDimensionalSanctuary", "(4,000 Coins) [Dimensional Sanctuary: Otherworld Pocket Realm] Gain 1,000,000 lbs carry capacity and total immunity to fatigue and exhaustion.", blueprintCue2, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 4000,
			m_Fact = blueprintUnitFactReference17,
			RecordForRespec = true
		}, 4000, Array.Empty<Condition>()));
		bool flag = blueprintCue2.Answers.Count > 0;
		AddBackAndLeave(blueprintCue2, "AnswerBackFromForbiddenCheats", "AnswerLeaveFromForbiddenCheats");
		ListOffer(blueprintCue3, CreateStoreAnswer("AnswerBuyCrownApotheosis", "(4,500 Coins) [Crown of Apotheosis] Permanent +6 Int/Wis/Cha, +2 insight bonus to all spell DCs, and spell resistance 10.", blueprintCue3, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 4500,
			m_Fact = CosmicRelics.CosmicCrownOfApotheosisFeature.ToReference<BlueprintUnitFactReference>()
		}, 4500, Array.Empty<Condition>()));
		ListOffer(blueprintCue3, CreateStoreAnswer("AnswerBuyRingOmnipresence", "(4,500 Coins) [Ring of Omnipresence] Permanent immunity to paralysis and to being held in place, +5 deflection AC, +5 sacred bonus to all saving throws.", blueprintCue3, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 4500,
			m_Fact = CosmicRelics.CosmicRingOfOmnipresenceFeature.ToReference<BlueprintUnitFactReference>()
		}, 4500, Array.Empty<Condition>()));
		ListOffer(blueprintCue3, CreateStoreAnswer("AnswerBuyAstralDragon", "(5,000 Coins) [Astral Sovereign Dragon] Gain the daily ability to summon the Astral Sovereign Dragon, an ancient silver dragon, to fight at your side for 1 hour.", blueprintCue3, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 5000,
			m_Fact = CosmicRelics.CosmicAstralDragonFeature.ToReference<BlueprintUnitFactReference>()
		}, 5000, Array.Empty<Condition>()));
		ListOffer(blueprintCue3, CreateStoreAnswer("AnswerBuyMythicPact", "(4,000 Coins) [Rulebreaker Mythic Pact] +3 Caster Level to all spells, +4 sacred attack and damage.", blueprintCue3, new ContextActionSpendCoinsAndGrantFact
		{
			Cost = 4000,
			m_Fact = CosmicRelics.CosmicMythicPactFeature.ToReference<BlueprintUnitFactReference>()
		}, 4000, Array.Empty<Condition>()));
		AddBackAndLeave(blueprintCue3, "AnswerBackFromCosmicRelics", "AnswerLeaveFromCosmicRelics");
		(string, string, string, bool)[] array = new(string, string, string, bool)[14]
		{
			("AnswerSwitchFealtyCayden", "300e212868bca984687c92bcb66d381b", "Cayden Cailean", false),
			("AnswerSwitchFealtyDesna", "2c0a3b9971327ba4d9d85354d16998c1", "Desna", false),
			("AnswerSwitchFealtyIomedae", "88d5da04361b16746bf5b65795e0c38c", "Iomedae", false),
			("AnswerSwitchFealtyPharasma", "458750bc214ab2e44abdeae404ab22e9", "Pharasma", false),
			("AnswerSwitchFealtyCalistria", "c7531715a3f046d4da129619be63f44c", "Calistria", false),
			("AnswerSwitchFealtyNethys", "6262cfce7c31626458325ca0909de997", "Nethys", false),
			("AnswerSwitchFealtyAsmodeus", "a3a5ccc9c670e6f4ca4a686d23b89900", "Asmodeus", false),
			("AnswerSwitchFealtyGorum", "8f49a5d8528a82c44b8c117a89f6b68c", "Gorum", false),
			("AnswerSwitchFealtyBesmara", "BesmaraFeature", "Besmara", true),
			("AnswerSwitchFealtyLanternKing", "LanternKingFeature", "the Lantern King", true),
			("AnswerSwitchFealtyChaldira", "ChaldiraFeature", "Chaldira", true),
			("AnswerSwitchFealtyMilani", "MilaniFeature", "Milani", true),
			("AnswerSwitchFealtyBlackButterfly", "BlackButterflyFeature", "Black Butterfly", true),
			("AnswerSwitchFealtyAtheism", "92c0d2da0a836ce418a267093c09ca54", "Atheism", false)
		};
		for (int num = 0; num < array.Length; num++)
		{
			(string, string, string, bool) tuple = array[num];
			BlueprintFeature blueprintFeature = ResolveDeity(tuple.Item2, tuple.Item4);
			if (blueprintFeature != null)
			{
				BlueprintFeatureReference blueprintFeatureReference = blueprintFeature.ToReference<BlueprintFeatureReference>();
				ListOffer(blueprintCue4, CreateStoreAnswer(tuple.Item1, PatronLabel(tuple.Item3), blueprintCue4, new ContextActionSwitchDeity
				{
					m_NewDeity = blueprintFeatureReference,
					Cost = 500
				}, 500, new Condition[3]
				{
					new ConditionCosmicPatronSwitchAllowed
					{
						m_Deity = blueprintFeatureReference
					},
					new HasFact
					{
						Unit = new PlayerCharacter(),
						m_Fact = blueprintFeature.ToReference<BlueprintUnitFactReference>(),
						Not = true
					},
					ModDeityOffered(tuple.Item4)
				}));
			}
		}
		(string, BlueprintFeature, string, string, bool, string)[] array2 = new(string, BlueprintFeature, string, string, bool, string)[13]
		{
			("AnswerAscendAvatarCayden", DeityAvatarAscension.AvatarCaydenFeature, "Avatar of the Lucky Drunk (Cayden Cailean)", "300e212868bca984687c92bcb66d381b", false, "+4 sacred attack, AC and saves, fast healing 5, immunity to nausea, fear and compulsion (which also refuses an ally's Rage and Blessing of Luck and Resolve). Sacred does not stack with other sacred bonuses."),
			("AnswerAscendAvatarIomedae", DeityAvatarAscension.AvatarIomedaeFeature, "Avatar of the Inheritor (Iomedae)", "88d5da04361b16746bf5b65795e0c38c", false, "+4 sacred AC, attack and saves, +6 sacred damage. Sacred does not stack with other sacred bonuses, and this overwrites Iomedae's own Vanguard blessing."),
			("AnswerAscendAvatarAsmodeus", DeityAvatarAscension.AvatarAsmodeusFeature, "Avatar of the Prince of Darkness (Asmodeus)", "a3a5ccc9c670e6f4ca4a686d23b89900", false, "+4 profane AC, attack and ability DCs, immunity to fire and to harmful mind-affecting effects. Profane does not stack with other profane bonuses (Corrupt Aura, Monarch Domain)."),
			("AnswerAscendAvatarDesna", DeityAvatarAscension.AvatarDesnaFeature, "Avatar of the Song of the Spheres (Desna)", "2c0a3b9971327ba4d9d85354d16998c1", false, "+30 ft speed, +4 sacred AC and saves, reroll failed saves, immunity to paralysis, entanglement, curses, movement impairment and sleep. Sacred does not stack with other sacred bonuses."),
			("AnswerAscendAvatarPharasma", DeityAvatarAscension.AvatarPharasmaFeature, "Avatar of the Lady of Graves (Pharasma)", "458750bc214ab2e44abdeae404ab22e9", false, "Immunity to death effects, energy drain and negative energy, +4 saves, +2 attack and +6 damage, all stacking."),
			("AnswerAscendAvatarCalistria", DeityAvatarAscension.AvatarCalistriaFeature, "Avatar of the Savored Sting (Calistria)", "c7531715a3f046d4da129619be63f44c", false, "+4 insight initiative, attack, AC and Reflex, +4d6 sneak attack, poison immunity. Insight does not stack with other insight bonuses (Aeon boon, Omniscient Mimicry, Grandmaster Transcendence)."),
			("AnswerAscendAvatarNethys", DeityAvatarAscension.AvatarNethysFeature, "Avatar of the All-Seeing Eye (Nethys)", "6262cfce7c31626458325ca0909de997", false, "+3 ability DCs, +4 spell penetration, +4 saves, +2 attack and +4 damage, all stacking; resist 10 acid, cold, electricity, fire and sonic; see invisibility."),
			("AnswerAscendAvatarGorum", DeityAvatarAscension.AvatarGorumFeature, "Avatar of Our Lord in Iron (Gorum)", "8f49a5d8528a82c44b8c117a89f6b68c", false, "+6 Strength, +4 Constitution and +4 damage, an extra attack on a full attack, DR 10/- that stacks with your other DR, immunity to fatigue and exhaustion. All stacking."),
			("AnswerAscendAvatarBesmara", DeityAvatarAscension.AvatarBesmaraFeature, "Avatar of the Pirate Queen (Besmara)", "BesmaraFeature", true, "+4 AC and saves that stack with everything including Plot Armor, +4 luck attack, +4 CMB and CMD, fast healing 5. The luck attack bonus does not stack with Signature Attack."),
			("AnswerAscendAvatarLanternKing", DeityAvatarAscension.AvatarLanternKingFeature, "Avatar of the Laughing King (the Lantern King)", "LanternKingFeature", true, "+6 dodge AC, +6 Dexterity, +4 Will, immunity to paralysis, entanglement and confusion. All stacking."),
			("AnswerAscendAvatarChaldira", DeityAvatarAscension.AvatarChaldiraFeature, "Avatar of the Mischievous Friend (Chaldira)", "ChaldiraFeature", true, "Allies within 30 ft (not you) gain +4 luck saves and +3 luck attack and AC; you gain +2 AC and saves that stack with everything. The aura's luck does not stack with allies' other luck bonuses."),
			("AnswerAscendAvatarMilani", DeityAvatarAscension.AvatarMilaniFeature, "Avatar of the Everbloom (Milani)", "MilaniFeature", true, "+4 sacred AC and saves, +2 attack and +4 damage that stack, fast healing 5, immunity to disease and bleed. Sacred does not stack with other sacred bonuses."),
			("AnswerAscendAvatarBlackButterfly", DeityAvatarAscension.AvatarBlackButterflyFeature, "Avatar of the Silence Between (Black Butterfly)", "BlackButterflyFeature", true, "50% miss chance against attacks, +4 dodge AC, +4 Will and initiative, +4 damage, immunity to cold and sonic. All stacking except the miss chance.")
		};
		for (int num = 0; num < array2.Length; num++)
		{
			(string, BlueprintFeature, string, string, bool, string) a = array2[num];
			if (a.Item2 == null)
			{
				continue;
			}
			BlueprintFeature blueprintFeature2 = ResolveDeity(a.Item4, a.Item5);
			if (blueprintFeature2 != null)
			{
				bool flag2 = a.Item2 == DeityAvatarAscension.AvatarLanternKingFeature;
				ListOffer(blueprintCue4, CreateStoreAnswer(a.Item1, AvatarLabel(a.Item3, a.Item6), blueprintCue4, new ContextActionAscendAvatar
				{
					Cost = 10000,
					m_AvatarFeature = a.Item2.ToReference<BlueprintFeatureReference>()
				}, 10000, new Condition[3]
				{
					FollowerOf(a, blueprintFeature2),
					ModDeityOffered(a.Item5),
					flag2 ? new ConditionLanternKingDiscountEarned
					{
						Not = true
					} : null
				}));
				if (flag2)
				{
					int discountedAvatarCost = LanternKingReveal.DiscountedAvatarCost;
					ListOffer(blueprintCue4, CreateStoreAnswer("AnswerAscendAvatarLanternKingDiscount", DiscountedAvatarLabel(a.Item3, a.Item6), blueprintCue4, new ContextActionAscendAvatar
					{
						Cost = discountedAvatarCost,
						m_AvatarFeature = a.Item2.ToReference<BlueprintFeatureReference>()
					}, discountedAvatarCost, new Condition[3]
					{
						FollowerOf(a, blueprintFeature2),
						ModDeityOffered(a.Item5),
						new ConditionLanternKingDiscountEarned()
					}));
				}
			}
		}
		AddBackAndLeave(blueprintCue4, "AnswerBackFromFealty", "AnswerLeaveFromFealty");
		BlueprintFeature blueprint = BlueprintTools.GetBlueprint<BlueprintFeature>("92c0d2da0a836ce418a267093c09ca54");
		ListOffer(blueprintCue5, CreateStoreAnswer("AnswerEmbraceGodDefier", "<color=#5B2A86>[Ascend]</color> Shatter divine chains and walk the Path of the God Defier! (Cost: 10,000 Coins)", CosmicStoreRootCue, new ContextActionAscendAvatar
		{
			Cost = 10000,
			m_AvatarFeature = DeityAvatarAscension.GodDefierFeature.ToReference<BlueprintFeatureReference>()
		}, 10000, new Condition[1] { (blueprint != null) ? MainCharacterHas(blueprint.ToReference<BlueprintUnitFactReference>()) : null }));
		AddBackAndLeave(blueprintCue5, "AnswerBackFromGodDefier", "AnswerLeaveFromGodDefier");
		ListOffer(CosmicStoreRootCue, CreateStoreAnswer("AnswerStoreBlessings", "[Deity Blessings] Browse 14 Constellation Patron Blessings (100 - 500 Coins)", CueStoreBlessings, null, 0, Array.Empty<Condition>()));
		ListOffer(CosmicStoreRootCue, CreateStoreAnswer("AnswerStoreMythic", "[Mythic Path Boons] Browse 10 Mythic Path Resonance Boons (1,500 - 2,000 Coins)", CueStoreMythic, null, 0, Array.Empty<Condition>()));
		ListOffer(CosmicStoreRootCue, CreateStoreAnswer("AnswerStoreConsumables", "[Web Novel Consumables & Wishes] Elixirs, Figurines, and Wishes", blueprintCue, null, 0, Array.Empty<Condition>()));
		ListOffer(CosmicStoreRootCue, CreateStoreAnswer("AnswerStoreInfusions", "[Infusions & Transcendence] Inscribe cosmic truth and subclass mastery", CueStoreInfusions, null, 0, Array.Empty<Condition>()));
		if (flag)
		{
			ListOffer(CosmicStoreRootCue, CreateStoreAnswer("AnswerStoreForbiddenCheats", "<color=#7A1010>[Forbidden Otherworldly Cheats]</color> God-mode protagonist hacks (4,000 - 15,000 Coins)", blueprintCue2, null, 0, Array.Empty<Condition>()));
		}
		ListOffer(CosmicStoreRootCue, CreateStoreAnswer("AnswerStoreCosmicRelics", "<color=#133E71>[Cosmic Relics & Sovereign Boons]</color> Stellar artifacts and companions (4,000 - 5,000 Coins)", blueprintCue3, null, 0, Array.Empty<Condition>()));
		ListOffer(CosmicStoreRootCue, CreateStoreAnswer("AnswerStoreFealty", "[Constellation Fealty & Avatar Ascension] Align with a patron or ascend as an Avatar", blueprintCue4, null, 0, Array.Empty<Condition>()));
		ListOffer(CosmicStoreRootCue, CreateStoreAnswer("AnswerStoreGodDefier", "<color=#5B2A86>[Path of the God Defier]</color> (Atheists) Shatter divine chains (10,000 Coins)", blueprintCue5, null, 0, Array.Empty<Condition>()));
		ListOffer(CosmicStoreRootCue, CreateStoreAnswer("AnswerStoreClose", "[Leave] Close the Cosmic Constellation Exchange.", null, null, 0, Array.Empty<Condition>()));
		BlueprintAbility OpenCosmicStoreAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "OpenCosmicStoreAbility", delegate(BlueprintAbility blueprintAbility)
		{
			blueprintAbility.SetName(Main.IsekaiContext, "Commune with Constellations");
			blueprintAbility.SetDescription(Main.IsekaiContext, "Open the Cosmic Constellation Exchange dialog to browse offerings, redeem accumulated Cosmic Coins for divine blessings, mythic boons, web novel elixirs, modular wishes, infusions, switch patron deities, or ascend as a divine Avatar.");
			((BlueprintUnitFact)blueprintAbility).m_Icon = Icon_Coin;
			blueprintAbility.Type = AbilityType.Special;
			blueprintAbility.Range = AbilityRange.Personal;
			blueprintAbility.CanTargetSelf = true;
			blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
			blueprintAbility.ActionType = UnitCommand.CommandType.Free;
			blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
			{
				c.Actions = ActionFlow.DoSingle(delegate(ContextActionOpenCosmicStoreDialog contextActionOpenCosmicStoreDialog)
				{
					contextActionOpenCosmicStoreDialog.m_Dialog = CosmicStoreDialog.ToReference<BlueprintDialogReference>();
				});
			});
		});
		DivineTokens.ItemCosmicCoin = Helpers.CreateBlueprint(Main.IsekaiContext, "ItemCosmicCoin", delegate(BlueprintItemEquipmentUsable blueprintItemEquipmentUsable)
		{
			blueprintItemEquipmentUsable.SetName(Main.IsekaiContext, "Cosmic Coin");
			blueprintItemEquipmentUsable.SetDescription(Main.IsekaiContext, "A shimmering token forged from condensed divine favor and audience entertainment. Right-click to use from your inventory to instantly commune with the observing constellations and open the Cosmic Constellation Exchange.\n\nYour inventory stack automatically synchronizes with your accumulated Cosmic Coins.");
			((BlueprintItem)blueprintItemEquipmentUsable).m_Icon = Icon_Coin;
			((BlueprintItem)blueprintItemEquipmentUsable).m_Cost = 0;
			((BlueprintItem)blueprintItemEquipmentUsable).m_Weight = 0f;
			((BlueprintItem)blueprintItemEquipmentUsable).m_IsNotable = true;
			((BlueprintItem)blueprintItemEquipmentUsable).m_ForceStackable = true;
			((BlueprintItem)blueprintItemEquipmentUsable).m_Destructible = false;
			blueprintItemEquipmentUsable.Type = UsableItemType.Other;
			blueprintItemEquipmentUsable.SpendCharges = false;
			blueprintItemEquipmentUsable.Charges = 1;
			blueprintItemEquipmentUsable.RestoreChargesOnRest = false;
			((BlueprintItemEquipment)blueprintItemEquipmentUsable).m_Ability = OpenCosmicStoreAbility.ToReference<BlueprintAbilityReference>();
		});
		void AddBackAndLeave(BlueprintCue cue, string backName, string leaveName)
		{
			cue.Answers.Add(CreateStoreAnswer(backName, "[Back] Return to the Main Catalog.", CosmicStoreRootCue, null, 0, Array.Empty<Condition>()).ToReference<BlueprintAnswerBaseReference>());
			cue.Answers.Add(CreateStoreAnswer(leaveName, "[Leave] Close the Cosmic Constellation Exchange.", null, null, 0, Array.Empty<Condition>()).ToReference<BlueprintAnswerBaseReference>());
		}
		void AddBlessing(string name, string answerName, string displayName, string desc, int cost, Action<BlueprintBuff> configureBuff)
		{
			BlueprintBuff bp4 = TTCoreExtensions.CreateBuff(name + "Buff", delegate(BlueprintBuff blueprintBuff)
			{
				blueprintBuff.SetName(Main.IsekaiContext, displayName);
				blueprintBuff.SetDescription(Main.IsekaiContext, desc);
				((BlueprintUnitFact)blueprintBuff).m_Icon = Icon_Coin;
				blueprintBuff.m_Flags = BlueprintBuff.Flags.StayOnDeath;
				configureBuff(blueprintBuff);
			});
			ListOffer(CueStoreBlessings, CreateStoreAnswer(answerName, OfferLabel(cost, displayName, desc), CueStoreBlessings, new ContextActionSpendCoinsAndApplyBuff
			{
				Cost = cost,
				m_Buff = bp4.ToReference<BlueprintBuffReference>(),
				DurationHours = 1
			}, cost, Array.Empty<Condition>()));
		}
		void AddInfusion(string name, string answerName, string displayName, string desc, int cost, Action<BlueprintFeature> configureFeature)
		{
			BlueprintFeature bp4 = Helpers.CreateBlueprint(Main.IsekaiContext, name + "Feature", delegate(BlueprintFeature blueprintFeature3)
			{
				blueprintFeature3.SetName(Main.IsekaiContext, displayName);
				blueprintFeature3.SetDescription(Main.IsekaiContext, desc);
				((BlueprintUnitFact)blueprintFeature3).m_Icon = Icon_Coin;
				blueprintFeature3.IsClassFeature = false;
				configureFeature(blueprintFeature3);
			});
			ListOffer(CueStoreInfusions, CreateStoreAnswer(answerName, OfferLabel(cost, displayName, desc), CueStoreInfusions, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = cost,
				m_Fact = bp4.ToReference<BlueprintUnitFactReference>()
			}, cost, Array.Empty<Condition>()));
		}
		void AddMythicBoon(string name, string answerName, string displayName, string desc, int cost, Action<BlueprintFeature> configureFeature)
		{
			BlueprintFeature bp4 = Helpers.CreateBlueprint(Main.IsekaiContext, name + "Feature", delegate(BlueprintFeature blueprintFeature3)
			{
				blueprintFeature3.SetName(Main.IsekaiContext, displayName);
				blueprintFeature3.SetDescription(Main.IsekaiContext, desc);
				((BlueprintUnitFact)blueprintFeature3).m_Icon = Icon_Coin;
				blueprintFeature3.IsClassFeature = false;
				configureFeature(blueprintFeature3);
			});
			AlignmentMaskType alignmentMaskType = (MythicBoonAlignment.TryGetValue(name, out var value) ? value : AlignmentMaskType.Any);
			ListOffer(CueStoreMythic, CreateStoreAnswer(answerName, MythicOfferLabel(cost, displayName, desc, alignmentMaskType), CueStoreMythic, new ContextActionSpendCoinsAndGrantFact
			{
				Cost = cost,
				m_Fact = bp4.ToReference<BlueprintUnitFactReference>(),
				m_KeyOfInfinitePathways = KeyOfInfinitePathwaysFeature.ToReference<BlueprintFeatureReference>(),
				AllowedAlignment = alignmentMaskType
			}, cost, Array.Empty<Condition>()));
		}
		void AddSubclassBlessing(string name, string answerName, string displayName, string archetype, string desc, string requiredFactName, Action<BlueprintFeature> configureAwakened)
		{
			BlueprintFeature awakenedFeat = Helpers.CreateBlueprint(Main.IsekaiContext, name + "AwakenedFeature", delegate(BlueprintFeature blueprintFeature3)
			{
				blueprintFeature3.SetName(Main.IsekaiContext, displayName + " (Awakened)");
				blueprintFeature3.SetDescription(Main.IsekaiContext, desc);
				((BlueprintUnitFact)blueprintFeature3).m_Icon = Icon_Coin;
				blueprintFeature3.IsClassFeature = false;
				configureAwakened(blueprintFeature3);
			});
			BlueprintFeature bp4 = Helpers.CreateBlueprint(Main.IsekaiContext, name + "Feature", delegate(BlueprintFeature blueprintFeature3)
			{
				blueprintFeature3.SetName(Main.IsekaiContext, displayName);
				blueprintFeature3.SetDescription(Main.IsekaiContext, desc + "\n<i>(Takes effect only for a character with this archetype's proficiencies; checked when the blessing is gained and at each level-up.)</i>");
				((BlueprintUnitFact)blueprintFeature3).m_Icon = Icon_Coin;
				blueprintFeature3.IsClassFeature = false;
				blueprintFeature3.AddComponent(delegate(AddFeatureIfHasFact c)
				{
					c.m_CheckedFact = BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, requiredFactName);
					c.m_Feature = awakenedFeat.ToReference<BlueprintUnitFactReference>();
					c.Not = false;
				});
			});
			BlueprintUnitFactReference blueprintUnitFactReference18 = GuardedFact(requiredFactName);
			if (blueprintUnitFactReference18 != null)
			{
				ListOffer(CueStoreInfusions, CreateStoreAnswer(answerName, TranscendenceLabel(archetype, displayName, desc), CueStoreInfusions, new ContextActionSpendCoinsAndGrantFact
				{
					Cost = 1200,
					m_Fact = bp4.ToReference<BlueprintUnitFactReference>()
				}, 1200, new Condition[1] { MainCharacterHas(blueprintUnitFactReference18) }));
			}
		}
		static BlueprintItemReference Bottle(string itemName)
		{
			return BlueprintTools.GetModBlueprintReference<BlueprintItemReference>(Main.IsekaiContext, itemName);
		}
		static BlueprintAnswer CreateStoreAnswer(string name, string text, BlueprintCue nextCue, GameAction onSelectAction = null, int cost = 0, params Condition[] showConditions)
		{
			return TTCoreExtensions.CreateAnswer(name, delegate(BlueprintAnswer blueprintAnswer)
			{
				blueprintAnswer.SetText(Main.IsekaiContext, text);
				blueprintAnswer.NextCue = new CueSelection
				{
					Cues = ((nextCue != null) ? new List<BlueprintCueBaseReference> { nextCue.ToReference<BlueprintCueBaseReference>() } : new List<BlueprintCueBaseReference>()),
					Strategy = Strategy.First
				};
				Condition[] conditions = (showConditions ?? new Condition[0]).Where((Condition c) => c != null).ToArray();
				blueprintAnswer.ShowConditions = ActionFlow.IfAll(conditions);
				blueprintAnswer.SelectConditions = ((cost > 0) ? ActionFlow.IfAll(new ConditionCanAffordCosmicCoins
				{
					Cost = cost
				}) : ActionFlow.EmptyCondition());
				blueprintAnswer.OnSelect = ((onSelectAction != null) ? new ActionList
				{
					Actions = new GameAction[1] { onSelectAction }
				} : ActionFlow.DoNothing());
			});
		}
		BlueprintAbility CreateWishVariant(string name, string statName, StatType stat, Sprite sprite, int bonus = 2)
		{
			return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintAbility blueprintAbility)
			{
				blueprintAbility.SetName(Main.IsekaiContext, $"Cosmic Wish: Permanent +{bonus} {statName}");
				blueprintAbility.SetDescription(Main.IsekaiContext, $"Consumes 1 Wish charge to permanently grant a +{bonus} inherent bonus to {statName} that stacks with every other inherent bonus.");
				((BlueprintUnitFact)blueprintAbility).m_Icon = sprite ?? Icon_Coin;
				blueprintAbility.Type = AbilityType.Special;
				blueprintAbility.Range = AbilityRange.Personal;
				blueprintAbility.CanTargetSelf = true;
				blueprintAbility.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Self;
				blueprintAbility.ActionType = UnitCommand.CommandType.Standard;
				BlueprintFeature feat = Helpers.CreateBlueprint(Main.IsekaiContext, name + "Feature", delegate(BlueprintFeature f)
				{
					f.SetName(Main.IsekaiContext, $"Inherent Boost: {statName} +{bonus}");
					f.SetDescription(Main.IsekaiContext, $"+{bonus} inherent bonus to {statName} for each Wish spent on it, stacking with every other inherent bonus.");
					((BlueprintUnitFact)f).m_Icon = Icon_Coin;
					f.IsClassFeature = false;
					f.AddComponent(delegate(AddStackingStatBonus c)
					{
						c.Descriptor = ModifierDescriptor.Inherent;
						c.Stat = stat;
						c.Value = bonus;
					});
					f.Ranks = 10;
				});
				blueprintAbility.AddComponent(delegate(AbilityCasterFeatureBelowRank c)
				{
					c.m_Feature = feat.ToReference<BlueprintFeatureReference>();
					c.MaxRank = feat.Ranks;
				});
				blueprintAbility.AddComponent(delegate(AbilityEffectRunAction c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionAddFeature contextActionAddFeature)
					{
						contextActionAddFeature.m_PermanentFeature = feat.ToReference<BlueprintFeatureReference>();
					});
				});
				blueprintAbility.AddComponent(delegate(AbilityResourceLogic c)
				{
					c.m_RequiredResource = CosmicWishResource.ToReference<BlueprintAbilityResourceReference>();
					c.m_IsSpendResource = true;
					c.Amount = 1;
				});
			});
		}
		static Condition FollowerOf((string ansName, BlueprintFeature feat, string name, string deityId, bool isModDeity, string summary) tuple2, BlueprintFeature deityFeat)
		{
			if (tuple2.feat == DeityAvatarAscension.AvatarBesmaraFeature)
			{
				return new ConditionMainCharacterHasAnyFact
				{
					m_Facts = new BlueprintUnitFactReference[2]
					{
						deityFeat.ToReference<BlueprintUnitFactReference>(),
						BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("470d824767d34e2e9a59b6d70681af7a")
					}
				};
			}
			return MainCharacterHas(deityFeat.ToReference<BlueprintUnitFactReference>());
		}
		static BlueprintUnitFactReference GuardedFact(string featureName)
		{
			BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, featureName);
			if (modBlueprint == null)
			{
				Main.IsekaiContext.Logger.LogWarning("Cosmic Store: '" + featureName + "' did not resolve; its offer is not listed.");
				return null;
			}
			return modBlueprint.ToReference<BlueprintUnitFactReference>();
		}
		static void ListOffer(BlueprintCue cue, BlueprintAnswer blueprintAnswer)
		{
			if (cue != null && blueprintAnswer != null)
			{
				cue.Answers.Add(blueprintAnswer.ToReference<BlueprintAnswerBaseReference>());
			}
		}
		static Condition MainCharacterHas(BlueprintUnitFactReference fact)
		{
			return new HasFact
			{
				Unit = new PlayerCharacter(),
				m_Fact = fact
			};
		}
		static Condition ModDeityOffered(bool isModDeity)
		{
			if (!isModDeity)
			{
				return null;
			}
			return new ConditionContentOffered
			{
				Content = OfferedContent.IsekaiDeities
			};
		}
		static BlueprintFeature ResolveDeity(string id, bool isModDeity)
		{
			if (!isModDeity)
			{
				return BlueprintTools.GetBlueprint<BlueprintFeature>(id);
			}
			return BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, id);
		}
	}

	public static void CreateBlueprints()
	{
		Sprite Icon_Coin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
		CosmicStoreText.Create();
		CosmicSponsorshipExchangeFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CosmicSponsorshipExchangeFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Cosmic Sponsorship Exchange");
			bp.SetDescription(Main.IsekaiContext, "Allows you to commune with the observing constellations and redeem accumulated Cosmic Coins through a celestial store interface, offering divine blessings, mythic boons, web novel elixirs, modular wishes, cosmic infusions, and avatar ascensions.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Coin;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AddAbilityResources c)
			{
				c.m_Resource = BlueprintTools.GetModBlueprintReference<BlueprintAbilityResourceReference>(Main.IsekaiContext, "CosmicWishResource");
				c.RestoreAmount = false;
			});
			bp.AddComponent(delegate(AddFacts c)
			{
				c.m_Facts = new BlueprintUnitFactReference[2]
				{
					BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "OpenCosmicStoreAbility"),
					BlueprintTools.GetModBlueprintReference<BlueprintUnitFactReference>(Main.IsekaiContext, "ModularCosmicWishAbility")
				};
			});
		});
	}
}
