using System;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class Haggler
	{
		private static readonly Sprite Icon_Shout = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("f09453607e683784c8fca646eec49162")).m_Icon;

		public static int GetDiscountPercent(UnitEntityData player)
		{
			if (player == null)
			{
				return 0;
			}
			BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "Haggler");
			if (modBlueprint == null || !player.Descriptor.Progression.Features.HasFact(modBlueprint))
			{
				return 0;
			}
			int characterLevel = player.Descriptor.Progression.CharacterLevel;
			return Math.Min(50, Math.Max(10, 10 + characterLevel * 2));
		}

		public static void Add()
		{
			LocalizedString HagglerDesc = Helpers.CreateString(Main.IsekaiContext, "Haggler.Description", "Vendor prices and Cosmic Sponsorship Store costs are reduced by 10%, plus an additional 2% per character level (up to a maximum of 50% at 20th level).");
			SpecialPowerSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "Haggler", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Haggler");
				bp.SetDescription(HagglerDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_Shout;
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[20]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 1,
							ProgressionValue = 12
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 2,
							ProgressionValue = 14
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 3,
							ProgressionValue = 16
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 4,
							ProgressionValue = 18
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 5,
							ProgressionValue = 20
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 6,
							ProgressionValue = 22
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 7,
							ProgressionValue = 24
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 8,
							ProgressionValue = 26
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 9,
							ProgressionValue = 28
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 10,
							ProgressionValue = 30
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 11,
							ProgressionValue = 32
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 12,
							ProgressionValue = 34
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 13,
							ProgressionValue = 36
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 14,
							ProgressionValue = 38
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 15,
							ProgressionValue = 40
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 16,
							ProgressionValue = 42
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 17,
							ProgressionValue = 44
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 18,
							ProgressionValue = 46
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 19,
							ProgressionValue = 48
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 50
						}
					};
				});
				bp.AddComponent(delegate(AddVendorDiscount c)
				{
					c.m_DiscountModifierPercents = Values.CreateContextRankValue(AbilityRankType.Default);
					c.m_VendorDiscountType = AddVendorDiscount.VendorDiscountEntry.BuyOnly;
					c.m_ExcludeVendors = new BlueprintUnitReference[0];
					c.m_ExcludeItems = new BlueprintItemReference[0];
				});
				bp.AddComponent(delegate(RecalculateOnChangeParty c)
				{
					c.m_IsRecalculateIfDeath = true;
				});
			}));
		}
	}
}
