using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal class SecondReincarnation
	{
		private static readonly Sprite Icon_SecondReincarnation = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_SECOND_REINCARNATION.png");

		public static void Add()
		{
			LocalizedString SecondReincarnationDesc = Helpers.CreateString(Main.IsekaiContext, "SecondReincarnation.Description", "Your attacks ignore damage reduction and your spells ignore spell resistance and spell immunity.\nOnce per day, when your {g|Encyclopedia:HP}HP{/g} drops to 0, you are resurrected to full health.");
			BlueprintBuff SecondReincarnationBuff = TTCoreExtensions.CreateBuff("SecondReincarnationBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Second Reincarnation");
				bp.SetDescription(SecondReincarnationDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_SecondReincarnation;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(DeathActions c)
				{
					c.CheckResource = false;
					c.Actions = Helpers.CreateActionList(new ContextActionResurrect
					{
						FullRestore = true
					}, new ContextActionSpawnFx
					{
						PrefabLink = new PrefabLink
						{
							AssetId = "749ad3759dc93d64dba70a84d48135b5"
						}
					}, new ContextActionRemoveSelf());
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "SecondReincarnation", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Second Reincarnation");
				bp.SetDescription(SecondReincarnationDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_SecondReincarnation;
				bp.AddComponent<IgnoreDamageReductionOnAttack>();
				bp.AddComponent(delegate(IgnoreSpellImmunity c)
				{
					c.SpellDescriptor = SpellDescriptor.None;
				});
				bp.AddComponent(delegate(IgnoreSpellResistanceForSpells c)
				{
					c.m_AbilityList = new BlueprintAbilityReference[0];
					c.AllSpells = true;
				});
				bp.AddComponent(delegate(AddRestTrigger c)
				{
					c.Action = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
					{
						contextActionApplyBuff.m_Buff = SecondReincarnationBuff.ToReference<BlueprintBuffReference>();
						contextActionApplyBuff.Permanent = true;
						contextActionApplyBuff.IsFromSpell = false;
					});
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { SecondReincarnationBuff.ToReference<BlueprintUnitFactReference>() };
					c.DoNotRestoreMissingFacts = true;
				});
			});
		}
	}
}
