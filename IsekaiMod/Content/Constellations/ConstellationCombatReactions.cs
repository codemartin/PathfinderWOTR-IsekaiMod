using System;
using IsekaiMod.Config;
using IsekaiMod.Content.Features.IsekaiProtagonist;
using IsekaiMod.Content.Quests;
using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;

namespace IsekaiMod.Content.Constellations;

public class ConstellationCombatReactions : IGlobalRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IGlobalRulebookSubscriber, IDamageHandler, IGlobalSubscriber, IUnitCombatHandler, IPartyCombatHandler
{
	public enum Reaction
	{
		CriticalHit,
		Kill,
		BroughtLow,
		BossFight
	}

	internal const int CriticalChance = 20;

	internal const int KillChance = 12;

	internal const int BroughtLowChance = 35;

	internal const int BossChance = 100;

	internal const double CriticalCooldownSeconds = 45.0;

	internal const double KillCooldownSeconds = 60.0;

	internal const double BroughtLowCooldownSeconds = 90.0;

	internal const double BossCooldownSeconds = 120.0;

	internal const double SharedFloorSeconds = 20.0;

	internal const double BroughtLowFraction = 0.25;

	private static bool _initialized;

	internal const string CombatLineName = "Constellation.CombatReaction";

	internal static readonly string[][] Lines = new string[4][]
	{
		new string[4] { "<color=#7A1010><b>[The Constellation 'Our Lord in Iron']</b></color>: <i>\"THAT is how steel is meant to land! Again!\"</i>", "<color=#5B2A86><b>[The Constellation 'The Savored Sting']</b></color>: <i>\"Right where it hurts. I do love a precise lover of pain.\"</i>", "<color=#5B2A86><b>[The Constellation 'The Lucky Drunk']</b></color>: <i>\"Ha! Put that one on the tab, I'm buying the next round!\"</i>", "<color=#5B2A86><b>[The Constellation 'The All-Seeing Eye']</b></color>: <i>\"A flaw in the defence, found and exploited. Elegant.\"</i>" },
		new string[4] { "<color=#133E71><b>[The Constellation 'The Lady of Graves']</b></color>: <i>\"Another soul for the River. I will see it judged.\"</i>", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color>: <i>\"Efficiently concluded. The contract of that life is void.\"</i>", "<color=#7A1010><b>[The Constellation 'Our Lord in Iron']</b></color>: <i>\"Down and staying down! Who is next?\"</i>", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color>: <i>\"And the extra exits, stage left, forever! The audience roars!\"</i>" },
		new string[4] { "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color>: <i>\"Stand, champion. A crusader who still breathes has not yet fallen.\"</i>", "<color=#133E71><b>[The Constellation 'The Song of the Spheres']</b></color>: <i>\"Careful, starlight. The dream is not finished with you.\"</i>", "<color=#5B2A86><b>[The Constellation 'The Everbloom']</b></color>: <i>\"Someone help them up! Roots hold even when the stem is bent.\"</i>", "<color=#133E71><b>[The Constellation 'The Silence Between the Stars']</b></color>: <i>\"...\" The silence between the stars leans closer, and waits.</i>" },
		new string[4] { "<color=#5B2A86><b>[The Constellation 'The Key and the Gate']</b></color>: <i>\"A knot in the thread. I have seen it tied and cut ten thousand times. Show me this one.\"</i>", "<color=#5B2A86><b>[The Constellation 'The Laughing King']</b></color>: <i>\"BOSS FIGHT! Everyone quiet, the good part is starting!\"</i>", "<color=#5B2A86><b>[The Constellation 'The Inheritor (Parallel Echo)']</b></color>: <i>\"This is the foe the others served. Break it, and the rest will break.\"</i>", "<color=#7A1010><b>[The Constellation 'The Prince of Darkness']</b></color>: <i>\"At last, an opponent worth the terms. Do not disappoint me.\"</i>" }
	};

	internal static readonly string[][] Speakers = new string[4][]
	{
		new string[4] { "Our Lord in Iron", "The Savored Sting", "The Lucky Drunk", "The All-Seeing Eye" },
		new string[4] { "The Lady of Graves", "The Prince of Darkness", "Our Lord in Iron", "The Laughing King" },
		new string[4] { "The Inheritor", "The Song of the Spheres", "The Everbloom", "The Silence Between the Stars" },
		new string[4] { "The Key and the Gate", "The Laughing King", "The Inheritor", "The Prince of Darkness" }
	};

	private static readonly double[] _lastSpoken = new double[4] { -1.0, -1.0, -1.0, -1.0 };

	private static readonly int[] _lastIndex = new int[4] { -1, -1, -1, -1 };

	private static double _lastAny = -1.0;

	private static bool _bossAnnouncedThisFight;

	private static readonly Random _random = new Random();

	public static void Init()
	{
		if (!_initialized)
		{
			_initialized = true;
			EventBus.Subscribe(new ConstellationCombatReactions());
		}
	}

	internal static string SettingLabel()
	{
		return "Enable Constellation Combat Reactions: the constellations now and then remark on a critical hit (" + $"{20}%, at most every {45.0:0} s), a kill ({12}%, {60.0:0} s), a companion " + $"falling below a quarter of their hit points ({35}%, {90.0:0} s) and the start of a boss fight " + $"(always, once per fight), with at least {20.0:0} s between any two. Needs Constellations on. Not in army battles or the coliseum. Applies at once.";
	}

	internal static int ChanceFor(Reaction reaction)
	{
		return reaction switch
		{
			Reaction.CriticalHit => 20, 
			Reaction.Kill => 12, 
			Reaction.BroughtLow => 35, 
			_ => 100, 
		};
	}

	internal static double CooldownFor(Reaction reaction)
	{
		return reaction switch
		{
			Reaction.CriticalHit => 45.0, 
			Reaction.Kill => 60.0, 
			Reaction.BroughtLow => 90.0, 
			_ => 120.0, 
		};
	}

	internal static bool OffCooldown(double now, double lastOfThisKind, double lastOfAnyKind, double cooldown)
	{
		if (lastOfThisKind >= 0.0 && now >= lastOfThisKind && now - lastOfThisKind < cooldown)
		{
			return false;
		}
		if (lastOfAnyKind >= 0.0 && now >= lastOfAnyKind && now - lastOfAnyKind < 20.0)
		{
			return false;
		}
		return true;
	}

	internal static bool ClockWentBack(double now, double lastOfAnyKind)
	{
		if (lastOfAnyKind >= 0.0)
		{
			return now < lastOfAnyKind;
		}
		return false;
	}

	internal static bool RollPasses(int roll, int chancePercent)
	{
		if (roll >= 0)
		{
			return roll < chancePercent;
		}
		return false;
	}

	internal static bool CrossedLow(int hpBefore, int hpAfter, int maxHp)
	{
		if (maxHp <= 0 || hpAfter <= 0)
		{
			return false;
		}
		double num = (double)maxHp * 0.25;
		if ((double)hpBefore > num)
		{
			return (double)hpAfter <= num;
		}
		return false;
	}

	internal static bool Allowed(bool reactionsSetting, bool constellationsEnabled, bool playerIsTransmigrant)
	{
		return reactionsSetting & constellationsEnabled & playerIsTransmigrant;
	}

	internal static int NextIndex(int previous, int count)
	{
		if (count <= 0)
		{
			return -1;
		}
		if (previous >= 0)
		{
			return (previous + 1) % count;
		}
		return 0;
	}

	internal static void ResetPerPlaythroughState()
	{
		for (int i = 0; i < _lastSpoken.Length; i++)
		{
			_lastSpoken[i] = -1.0;
			_lastIndex[i] = -1;
		}
		_lastAny = -1.0;
		_bossAnnouncedThisFight = false;
	}

	private static double Now()
	{
		try
		{
			return (Game.Instance?.TimeController?.GameTime.TotalSeconds).GetValueOrDefault();
		}
		catch
		{
			return 0.0;
		}
	}

	private static bool LiveAllowed()
	{
		AddedContent addedContent = Main.IsekaiContext.AddedContent;
		if (addedContent == null)
		{
			return false;
		}
		if (!addedContent.EnableConstellationCombatReactions || !ConstellationSettings.Enabled)
		{
			return false;
		}
		return Allowed(reactionsSetting: true, constellationsEnabled: true, IsekaiTransmigrationQuest.PlayerCanStartTransmigration());
	}

	private static bool InParty(UnitEntityData unit)
	{
		if (unit == null)
		{
			return false;
		}
		Player player = Game.Instance?.Player;
		if (player != null)
		{
			if (!player.Party.Contains(unit))
			{
				return player.PartyAndPets.Contains(unit);
			}
			return true;
		}
		return false;
	}

	private static void TrySpeak(Reaction reaction)
	{
		double num = Now();
		if (OffCooldown(num, _lastSpoken[(int)reaction], _lastAny, CooldownFor(reaction)) && RollPasses(_random.Next(100), ChanceFor(reaction)))
		{
			int num2 = NextIndex(_lastIndex[(int)reaction], Lines[(int)reaction].Length);
			if (num2 >= 0)
			{
				_lastIndex[(int)reaction] = num2;
				_lastSpoken[(int)reaction] = num;
				_lastAny = num;
				ConstellationChatManager.PostRouted(ConstellationChatManager.LogRoute.Voice, ConstellationText.Localized("Constellation.CombatReaction", Lines[(int)reaction][num2]), Speakers[(int)reaction][num2], ConstellationCategory.Combat, "Combat Reaction");
			}
		}
	}

	public void OnEventAboutToTrigger(RuleAttackRoll evt)
	{
	}

	public void OnEventDidTrigger(RuleAttackRoll evt)
	{
		if (evt == null || evt.IsFake || !evt.IsHit || !evt.IsCriticalConfirmed)
		{
			return;
		}
		try
		{
			if (InParty(evt.Initiator) && LiveAllowed() && CampaignEncounterGuard.IsPartysEnemy(evt.Target) && !CampaignEncounterGuard.Excludes(evt.Target))
			{
				TrySpeak(Reaction.CriticalHit);
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error in ConstellationCombatReactions (critical hit): " + ex);
		}
	}

	public void HandleDamageDealt(RuleDealDamage dealDamage)
	{
		if (dealDamage == null || dealDamage.IsFake || dealDamage.Result <= 0)
		{
			return;
		}
		try
		{
			UnitEntityData target = dealDamage.Target;
			if (target == null || (!InParty(target) && (target.HPLeft > 0 || !InParty(dealDamage.Initiator))) || !LiveAllowed() || CampaignEncounterGuard.Excludes(target))
			{
				return;
			}
			if (InParty(target))
			{
				int hPLeft = target.HPLeft;
				if (CrossedLow(hPLeft + dealDamage.Result, hPLeft, target.MaxHP))
				{
					TrySpeak(Reaction.BroughtLow);
				}
			}
			else if (target.HPLeft <= 0 && InParty(dealDamage.Initiator) && CampaignEncounterGuard.IsPartysEnemy(target))
			{
				Game instance = Game.Instance;
				if (instance != null && instance.Player?.IsInCombat == true)
				{
					TrySpeak(Reaction.Kill);
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error in ConstellationCombatReactions (damage): " + ex);
		}
	}

	public void HandleUnitJoinCombat(UnitEntityData unit)
	{
		try
		{
			if (ClockWentBack(Now(), _lastAny))
			{
				_bossAnnouncedThisFight = false;
			}
			if (!_bossAnnouncedThisFight && !(unit == null) && CampaignEncounterGuard.IsBoss(unit.Blueprint) && LiveAllowed() && CampaignEncounterGuard.IsPartysEnemy(unit) && !CampaignEncounterGuard.Excludes(unit))
			{
				double num = _lastSpoken[3];
				TrySpeak(Reaction.BossFight);
				if (_lastSpoken[3] != num)
				{
					_bossAnnouncedThisFight = true;
				}
			}
		}
		catch (Exception ex)
		{
			Main.IsekaiContext.Logger.LogError("Error in ConstellationCombatReactions (boss): " + ex);
		}
	}

	public void HandleUnitLeaveCombat(UnitEntityData unit)
	{
	}

	public void HandlePartyCombatStateChanged(bool inCombat)
	{
		if (!inCombat)
		{
			_bossAnnouncedThisFight = false;
		}
	}
}
