using System;
using System.Collections.Generic;
using HarmonyLib;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	[HarmonyPatch(typeof(Player), "GainPartyExperience")]
	internal static class PowerLeveling_ExpPatch
	{
		public static void Prefix(ref int gained)
		{
			try
			{
				BlueprintFeature powerLeveling = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "PowerLevelingFeature");
				List<UnitEntityData> list = Game.Instance?.Player?.Party;
				if (powerLeveling == null || list == null || !list.Any((UnitEntityData unit) => (object)unit != null && unit.Descriptor?.HasFact(powerLeveling) == true) || gained <= 0)
				{
					return;
				}
				double num = 2.0;
				int num2 = 0;
				if (IsekaiDlcManager.HasDlc3())
				{
					num2++;
				}
				if (IsekaiDlcManager.HasDlc4())
				{
					num2++;
				}
				if (IsekaiDlcManager.HasDlc6())
				{
					num2++;
				}
				if (num2 >= 3)
				{
					num = 1.4;
				}
				else
				{
					switch (num2)
					{
					case 2:
						num = 1.6;
						break;
					case 1:
						num = 1.8;
						break;
					}
				}
				long val = (long)Math.Round((double)gained * num);
				gained = (int)Math.Min(2147483647L, val);
			}
			catch (Exception)
			{
			}
		}
	}
}
