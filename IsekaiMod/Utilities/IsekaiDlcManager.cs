using Kingmaker.Blueprints;
using Kingmaker.DLC;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities
{
	public static class IsekaiDlcManager
	{
		public const string DLC1_InevitableExcess = "8576a633c8fe4ce78530b55c1f0d14e5";

		public const string DLC2_ThroughTheAshes = "4f7ae2d1e6e74a0c807b4020e9e99354";

		public const string DLC3_MidnightIsles = "962e8c01fd834805b3ddf93134f77d44";

		public const string DLC4_LastSarkorians = "35b89606cfe9405085a35b02cf15017f";

		public const string DLC5_LordOfNothing = "95a25ca16bd54ce3b3ea56f83538fa0d";

		public const string DLC6_DanceOfMasks = "c2340df3fdaf403baffe824ae7a0a547";

		public const string DlcCommanderPack = "d2f0852710c5497eac91979c71131c90";

		public const string DLC1_Reward = "313fb87e4c0143bd84b3fc997c3c2266";

		public const string DLC2_Reward = "3380030f72954976a775c2e9a1e9ae07";

		public const string DLC3_Reward = "04b3bf450b1e44438854f25ba1290a98";

		public const string DLC4_Reward = "e38ada3f1eab4110ae331b613162a05b";

		public const string DLC5_Reward = "1c893fd3059d4b09bd37d089aff7866e";

		public const string DLC6_Reward = "b94f823171a84e30ad7a1b892433ab5d";

		public const string CommanderPack_Reward = "52e2a91ac53442a594a413007c302d83";

		public static bool IsDlcAvailable(string dlcGuid)
		{
			try
			{
				return BlueprintTools.GetBlueprint<BlueprintDlc>(dlcGuid)?.IsAvailable ?? false;
			}
			catch
			{
				return false;
			}
		}

		public static bool IsRewardAvailable(string rewardGuid)
		{
			try
			{
				return BlueprintTools.GetBlueprint<BlueprintDlcReward>(rewardGuid)?.IsAvailable ?? false;
			}
			catch
			{
				return false;
			}
		}

		public static bool HasDlc1()
		{
			return IsDlcAvailable("8576a633c8fe4ce78530b55c1f0d14e5");
		}

		public static bool HasDlc2()
		{
			return IsDlcAvailable("4f7ae2d1e6e74a0c807b4020e9e99354");
		}

		public static bool HasDlc3()
		{
			return IsDlcAvailable("962e8c01fd834805b3ddf93134f77d44");
		}

		public static bool HasDlc4()
		{
			return IsDlcAvailable("35b89606cfe9405085a35b02cf15017f");
		}

		public static bool HasDlc5()
		{
			return IsDlcAvailable("95a25ca16bd54ce3b3ea56f83538fa0d");
		}

		public static bool HasDlc6()
		{
			return IsDlcAvailable("c2340df3fdaf403baffe824ae7a0a547");
		}

		public static IsDLCEnabled CreateDlcCondition(string rewardGuid)
		{
			BlueprintDlcReward blueprint = BlueprintTools.GetBlueprint<BlueprintDlcReward>(rewardGuid);
			return new IsDLCEnabled
			{
				m_BlueprintDlcReward = blueprint?.ToReference<BlueprintDlcRewardReference>()
			};
		}
	}
}
