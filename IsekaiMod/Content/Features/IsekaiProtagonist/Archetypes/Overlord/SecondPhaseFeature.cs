using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Overlord
{
	internal class SecondPhaseFeature
	{
		private const string SecondPhaseName = "Second Phase";

		private const string SecondPhaseDescBuff = "You gain a +10 profane bonus to all attributes and your size increases by one size category for 24 hours.";

		private static readonly LocalizedString SecondPhaseDesc = Helpers.CreateString(Main.IsekaiContext, "SecondPhase.Description", "Once per day, when your {g|Encyclopedia:HP}HP{/g} drops to 0, you are resurrected to full health. You also gain a +10 profane bonus to all attributes and your size also increases by one size category for 24 hours.");

		private static readonly Sprite Icon_SecondPhase = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_SECOND_FORM.png");

		private static readonly Sprite Icon_SecondPhaseInactive = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_SECOND_FORM_INACTIVE.png");

		public static void Add()
		{
			BlueprintBuff SecondPhaseBuffEffect = TTCoreExtensions.CreateBuff("SecondPhaseBuffEffect", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Second Phase");
				bp.SetDescription(Main.IsekaiContext, "You gain a +10 profane bonus to all attributes and your size increases by one size category for 24 hours.");
				((BlueprintUnitFact)bp).m_Icon = Icon_SecondPhase;
				bp.Ranks = 1;
				bp.IsClassFeature = true;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Strength;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Dexterity;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Constitution;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Intelligence;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Wisdom;
					c.Value = 10;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Profane;
					c.Stat = StatType.Charisma;
					c.Value = 10;
				});
				bp.AddComponent(delegate(ChangeUnitSize c)
				{
					c.m_Type = ChangeUnitSize.ChangeType.Delta;
					c.SizeDelta = 1;
					c.Size = Size.Fine;
				});
				bp.AddComponent(delegate(AddGenericStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Size;
					c.Stat = StatType.Strength;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddGenericStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Size;
					c.Stat = StatType.Dexterity;
					c.Value = -2;
				});
				bp.Stacking = StackingType.Replace;
			});
			BlueprintBuff SecondPhaseBuff = TTCoreExtensions.CreateBuff("SecondPhaseBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Second Phase");
				bp.SetDescription(SecondPhaseDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_SecondPhaseInactive;
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
							AssetId = "14ba08b903ee28b41a779a616d905397"
						}
					}, new ContextActionApplyBuff
					{
						m_Buff = SecondPhaseBuffEffect.ToReference<BlueprintBuffReference>(),
						DurationValue = Values.Duration.OneDay,
						IsFromSpell = false
					}, new ContextActionRemoveSelf());
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "SecondPhaseFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Second Phase");
				bp.SetDescription(SecondPhaseDesc);
				((BlueprintUnitFact)bp).m_Icon = Icon_SecondPhase;
				bp.AddComponent(delegate(AddRestTrigger c)
				{
					c.Action = ActionFlow.DoSingle(delegate(ContextActionApplyBuff contextActionApplyBuff)
					{
						contextActionApplyBuff.m_Buff = SecondPhaseBuff.ToReference<BlueprintBuffReference>();
						contextActionApplyBuff.Permanent = true;
						contextActionApplyBuff.IsFromSpell = false;
					});
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { SecondPhaseBuff.ToReference<BlueprintUnitFactReference>() };
					c.DoNotRestoreMissingFacts = true;
				});
			});
		}
	}
}
