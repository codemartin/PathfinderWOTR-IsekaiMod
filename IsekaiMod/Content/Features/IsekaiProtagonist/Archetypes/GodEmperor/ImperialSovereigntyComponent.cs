using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Rest;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Kingdom;
using Kingmaker.Kingdom.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor
{
	public class ImperialSovereigntyComponent : UnitFactComponentDelegate, IRestFinishedHandler, ISubscriber, IGlobalSubscriber, IKingdomDayHandler, IAreaHandler
	{
		private static readonly BlueprintKingdomBuffReference CrusadeMaxMoraleBonus20 = BlueprintTools.GetBlueprint<BlueprintKingdomBuff>("5d021876b91bdb4468d2fdb524c7de28")?.ToReference<BlueprintKingdomBuffReference>();

		private static readonly BlueprintKingdomBuffReference ArmyFamousRecruiter = BlueprintTools.GetBlueprint<BlueprintKingdomBuff>("5ab635a173de4a00a00cb1d285543bb4")?.ToReference<BlueprintKingdomBuffReference>();

		private static readonly BlueprintKingdomBuffReference ArmyUnitsEnchantedWeapons = BlueprintTools.GetBlueprint<BlueprintKingdomBuff>("30a2bdc9fd8f4d8ab94a98da3a4d41cf")?.ToReference<BlueprintKingdomBuffReference>();

		private static readonly BlueprintKingdomBuffReference ArmyUnitsSoundMind = BlueprintTools.GetBlueprint<BlueprintKingdomBuff>("de7ea4ce6760427488884157f0075c80")?.ToReference<BlueprintKingdomBuffReference>();

		public void HandleRestFinished(RestStatus status)
		{
			ApplyCrusadeSovereignty();
		}

		public void OnNewDay()
		{
			ApplyCrusadeSovereignty();
		}

		public void OnAreaDidLoad()
		{
			ApplyCrusadeSovereignty();
		}

		public void OnAreaBeginUnloading()
		{
		}

		public override void OnActivate()
		{
			base.OnActivate();
			ApplyCrusadeSovereignty();
		}

		private void ApplyCrusadeSovereignty()
		{
			UnitEntityData owner = base.Owner;
			if (owner == null || !owner.IsMainCharacter)
			{
				return;
			}
			try
			{
				KingdomState kingdomState = Game.Instance?.Player?.Kingdom;
				if (kingdomState != null)
				{
					ApplyBuffIfMissing(kingdomState, CrusadeMaxMoraleBonus20);
					ApplyBuffIfMissing(kingdomState, ArmyFamousRecruiter);
					ApplyBuffIfMissing(kingdomState, ArmyUnitsEnchantedWeapons);
					ApplyBuffIfMissing(kingdomState, ArmyUnitsSoundMind);
				}
			}
			catch
			{
			}
		}

		private void ApplyBuffIfMissing(KingdomState kingdom, BlueprintKingdomBuffReference buffRef)
		{
			if (buffRef != null)
			{
				BlueprintKingdomBuff blueprintKingdomBuff = buffRef.Get();
				if (blueprintKingdomBuff != null && !kingdom.ActiveBuffs.HasFact(blueprintKingdomBuff))
				{
					kingdom.AddBuff(blueprintKingdomBuff, null, null, 0);
				}
			}
		}
	}
}
