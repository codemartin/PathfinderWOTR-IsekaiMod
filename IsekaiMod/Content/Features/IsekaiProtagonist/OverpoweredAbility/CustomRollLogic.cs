using System.Linq;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem.LogThreads.Common;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	public class CustomRollLogic : ContextAction
	{
		public BlueprintItemReference GoldCoins { get; set; }

		public BlueprintFeatureReference OmnipotentDie { get; set; }

		public override string GetCaption()
		{
			return "Performs an Otherworldly Gacha pull with pity and tiered rewards.";
		}

		public override void RunAction()
		{
			UnitEntityData unitEntityData = BlueprintSafetyExtensions.SafeGetMainCharacter();
			if (unitEntityData == null)
			{
				return;
			}
			ItemsCollection itemsCollection = Game.Instance?.Player?.Inventory;
			TimelineManager.Data.GachaPityCount++;
			int gachaPityCount = TimelineManager.Data.GachaPityCount;
			double num = Random.Range(0f, 100f);
			bool flag = gachaPityCount >= 100;
			int num2;
			int num3;
			if (!flag)
			{
				num2 = ((num < 0.2) ? 1 : 0);
				if (num2 == 0)
				{
					num3 = ((num < 3.2) ? 1 : 0);
					goto IL_008b;
				}
			}
			else
			{
				num2 = 1;
			}
			num3 = 0;
			goto IL_008b;
			IL_008b:
			bool flag2 = (byte)num3 != 0;
			bool flag3 = num2 == 0 && !flag2 && num < 18.2;
			if (num2 != 0)
			{
				TimelineManager.Data.GachaPityCount = 0;
				TimelineManager.Save();
				int num4 = 1000000;
				Game.Instance?.Player?.GainMoney(num4);
				DivineTokens.AddCoins(5000, "Transcendent Gacha Core");
				BlueprintAbilityResource modBlueprint = BlueprintTools.GetModBlueprint<BlueprintAbilityResource>(Main.IsekaiContext, "CosmicWishResource");
				BlueprintAbility modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintAbility>(Main.IsekaiContext, "ModularCosmicWishAbility");
				if (modBlueprint2 != null && !unitEntityData.Descriptor.HasFact(modBlueprint2))
				{
					unitEntityData.Descriptor.AddFact(modBlueprint2);
				}
				if (modBlueprint != null)
				{
					if (unitEntityData.Descriptor.Resources.GetResource((BlueprintScriptableObject)modBlueprint) == null)
					{
						unitEntityData.Descriptor.Resources.Add(modBlueprint, restoreAmount: true);
					}
					unitEntityData.Descriptor.Resources.Restore(modBlueprint, 5);
				}
				BlueprintFeature blueprintFeature = OmnipotentDie?.Get();
				if (blueprintFeature != null)
				{
					if (!unitEntityData.Descriptor.Progression.Features.HasFact(blueprintFeature))
					{
						unitEntityData.Descriptor.Progression.Features.AddFeature(blueprintFeature);
					}
					else
					{
						DivineTokens.AddCoins(10000, "Omnipotent Sovereign's Die (Duplicate Bonus)");
					}
				}
				string text = (flag ? " [100-PULL PITY ACTIVATED!]" : "");
				LogToCombatLog("<color=#FFD700><b>[6★ TRANSCENDENT UR JACKPOT!]" + text + "</b></color> You pulled <b>The Omnipotent Sovereign's Die</b>, <b>1,000,000 Gold</b>, <b>5,000 Cosmic Coins</b>, and <b>5 Cosmic Wish Charges</b>!");
				TriggerSupporterReactivity(6);
			}
			else if (flag2)
			{
				TimelineManager.Save();
				int num5 = Random.Range(100000, 250001);
				Game.Instance?.Player?.GainMoney(num5);
				int num6 = Random.Range(500, 1001);
				DivineTokens.AddCoins(num6, "SSR Gacha Blessing");
				string[] array = new string[3] { "2e63277fbcdb469384efc97d910a12c4", "039fcbf4887047e8977102c23dd8b56b", "81d504243708f504dbfe3f8f72efdeda" };
				BlueprintItem blueprint = BlueprintTools.GetBlueprint<BlueprintItem>(array[Random.Range(0, array.Length)]);
				if (blueprint != null)
				{
					itemsCollection?.Add(blueprint, 1);
				}
				LogToCombatLog($"<color=#FF69B4><b>[5★ SSR PULL!]</b></color> Rainbow light floods the dimensional rift! You received <b>{num5:N0} Gold</b>, <b>{num6} Cosmic Coins</b>, and a legendary relic (Pity: {gachaPityCount}/100)!");
				TriggerSupporterReactivity(5);
			}
			else if (flag3)
			{
				TimelineManager.Save();
				int num7 = Random.Range(15000, 35001);
				Game.Instance?.Player?.GainMoney(num7);
				int num8 = Random.Range(50, 151);
				DivineTokens.AddCoins(num8, "SR Gacha Cache");
				string[] array2 = new string[3] { "1bf7ae3382d3472e956f691da66598cf", "a35044c76afa45c69f4baedf13ad5ac9", "92752bbbf04dfa1439af186f48aee0e9" };
				int num9 = Random.Range(0, array2.Length);
				BlueprintItem blueprint2 = BlueprintTools.GetBlueprint<BlueprintItem>(array2[num9]);
				if (blueprint2 != null && itemsCollection != null)
				{
					int count = ((num9 != 2) ? 1 : 50);
					itemsCollection.Add(blueprint2, count);
				}
				LogToCombatLog($"<color=#9370DB><b>[4★ SR PULL]</b></color> A golden beam illuminates your cache! You obtained <b>{num7:N0} Gold</b>, <b>{num8} Cosmic Coins</b>, and tactical armaments (Pity: {gachaPityCount}/100).");
				TriggerSupporterReactivity(4);
			}
			else
			{
				TimelineManager.Save();
				int num10 = Random.Range(1500, 4001);
				Game.Instance?.Player?.GainMoney(num10);
				string[] array3 = new string[3] { "92752bbbf04dfa1439af186f48aee0e9", "fb822a8d451d91b438514541a9a986de", "d441dfae9c6b21e47ae24eb13d8b4c4b" };
				int num11 = Random.Range(0, array3.Length);
				BlueprintItem blueprint3 = BlueprintTools.GetBlueprint<BlueprintItem>(array3[num11]);
				if (blueprint3 != null && itemsCollection != null)
				{
					int count2 = ((num11 != 0) ? 1 : 15);
					itemsCollection.Add(blueprint3, count2);
				}
				LogToCombatLog($"<color=#1E90FF><b>[3★ Common Pull]</b></color> You pulled <b>{num10:N0} Gold</b> and essential expedition supplies (Pity: {gachaPityCount}/100).");
			}
		}

		private void TriggerSupporterReactivity(int tier)
		{
			string text = (ConstellationChatManager.GetPlayerDeityName() ?? "").ToLower();
			int amount = tier switch
			{
				5 => 250, 
				6 => 1000, 
				_ => 25, 
			};
			if (text.Contains("besmara") || text.Contains("chaldira"))
			{
				LogToCombatLog("<color=#FFD700><b>[Patron Fanfare]</b></color> <color=#FF69B4>Chaldira & Besmara:</color> \"AHAHAHA! Look at that glorious haul! You took the interdimensional casino for everything it's worth! Here's a pirate bounty to celebrate!\"");
				DivineTokens.AddCoins(amount, "Chaldira & Besmara Gacha Bonus");
			}
			else if (text.Contains("lantern"))
			{
				LogToCombatLog("<color=#FFD700><b>[Patron Fanfare]</b></color> <color=#FFA500>The Lantern King:</color> \"Ohoho! Probability shattered into glittering stardust! Cheating the cosmic house is an absolute masterpiece of comedy!\"");
				DivineTokens.AddCoins(amount, "The Lantern King Chaos Boon");
			}
			else if (text.Contains("yog"))
			{
				LogToCombatLog("<color=#FFD700><b>[Patron Fanfare]</b></color> <color=#00FFFF>Yog-Sothoth:</color> \"The quantum continuum collapsed along an infinitesimal branch. A localized singularity of transcendent probability has been realized.\"");
				DivineTokens.AddCoins(amount, "Yog-Sothoth Singular Timeline Bonus");
			}
			else if (text.Contains("gorum"))
			{
				LogToCombatLog("<color=#FFD700><b>[Patron Fanfare]</b></color> <color=#DC143C>Gorum:</color> \"A MOUNTAIN OF GOLD AND SACRED WEAPONS! FORGE THEM INTO TOOLS OF SLAUGHTER AND CRUSH THE ABYSSAL HORDES!\"");
				DivineTokens.AddCoins(amount, "Gorum War Spoils");
			}
			else if (text.Contains("desna"))
			{
				LogToCombatLog("<color=#FFD700><b>[Patron Fanfare]</b></color> <color=#1E90FF>Desna:</color> \"The Song of the Spheres echoes softly for you, brave dreamer. May lucky starlight illuminate your path through the darkness.\"");
				DivineTokens.AddCoins(amount, "Desna Starlight Blessing");
			}
			else
			{
				LogToCombatLog("<color=#FFD700><b>[Patron Fanfare]</b></color> <color=#F5C542>The Observant Deities:</color> \"The cosmic constellation gazes in awe upon your spectacular fortune! Additional heavenly favor descends upon you.\"");
				DivineTokens.AddCoins(amount, "Observant Deities Favor");
			}
		}

		private void LogToCombatLog(string message)
		{
			MessageLogThread messageLogThread = LogThreadService.Instance.GetThreadsByChannelType(default(LogChannelType)).OfType<MessageLogThread>().FirstOrDefault();
			if (messageLogThread != null)
			{
				CombatLogMessage newMessage = new CombatLogMessage(message, new Color(0.9f, 0.75f, 0.2f), PrefixIcon.None, new TooltipTemplateSimple(null, message));
				((LogThreadBase)messageLogThread).AddMessage(newMessage);
			}
			else
			{
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage(message);
				});
			}
		}
	}
}
