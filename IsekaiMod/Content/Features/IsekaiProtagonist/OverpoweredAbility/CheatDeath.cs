using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class CheatDeath
	{
		private static readonly Sprite Icon_CheatDeath = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_SECOND_REINCARNATION.png");

		private static readonly BlueprintBuff HasteBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("03464790f40c3c24aa684b57155f3280");

		private static readonly BlueprintBuff InvisibilityGreaterBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("e6b35473a237a6045969253beb09777c");

		public static void Add()
		{
			BlueprintBuff CheatDeathActiveBuff = TTCoreExtensions.CreateBuff("CheatDeathActiveBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Return by Death Ready");
				bp.SetDescription(Main.IsekaiContext, "Your connection to the timeline ensures that lethal defeat rewinds reality. When your HP drops to 0, you are instantly resurrected to full HP, vigor, and gain Greater Invisibility and Haste for 2 rounds.");
				((BlueprintUnitFact)bp).m_Icon = Icon_CheatDeath;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				ActionList actions = Helpers.CreateActionList(new ContextActionResurrect
				{
					FullRestore = true
				}, new ContextActionSpawnFx
				{
					PrefabLink = new PrefabLink
					{
						AssetId = "749ad3759dc93d64dba70a84d48135b5"
					}
				}, new ContextActionApplyBuff
				{
					m_Buff = ((HasteBuff != null) ? HasteBuff.ToReference<BlueprintBuffReference>() : null),
					DurationValue = new ContextDurationValue
					{
						Rate = DurationRate.Rounds,
						DiceType = DiceType.Zero,
						BonusValue = 2
					},
					Permanent = false
				}, new ContextActionApplyBuff
				{
					m_Buff = ((InvisibilityGreaterBuff != null) ? InvisibilityGreaterBuff.ToReference<BlueprintBuffReference>() : null),
					DurationValue = new ContextDurationValue
					{
						Rate = DurationRate.Rounds,
						DiceType = DiceType.Zero,
						BonusValue = 2
					},
					Permanent = false
				}, new ContextActionRemoveSelf());
				bp.AddComponent(delegate(DeathActions c)
				{
					c.CheckResource = false;
					c.Actions = actions;
				});
			});
			BlueprintFeature blueprintFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "CheatDeathFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Overpowered Ability - Cheat Death (Return by Death)");
				bp.SetDescription(Main.IsekaiContext, "Like a cursed traveler bound to a mysterious witch, death is not an ending for you. \nBenefit: Once per rest, when your HP drops to 0, reality snaps backward: you are resurrected with 100% HP and gain Greater Invisibility and Haste for 2 rounds to turn the tide of battle.");
				((BlueprintUnitFact)bp).m_Icon = Icon_CheatDeath;
				bp.AddComponent(delegate(AddRestTrigger c)
				{
					c.Action = ActionFlow.DoSingle(delegate(ContextActionApplyBuff b)
					{
						b.m_Buff = CheatDeathActiveBuff.ToReference<BlueprintBuffReference>();
						b.Permanent = true;
						b.IsFromSpell = false;
					});
				});
			});
			blueprintFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
			{
				c.Level = 10;
			});
			OverpoweredAbilitySelection.AddToSelection(blueprintFeature);
		}
	}
}
