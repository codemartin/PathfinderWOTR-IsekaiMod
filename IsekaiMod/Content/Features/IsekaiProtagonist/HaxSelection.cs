using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	internal static class HaxSelection
	{
		private static readonly Sprite Icon_Hax = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_HAX.png");

		public static void Add()
		{
			BlueprintFeature SeriousStrike = Helpers.CreateBlueprint(Main.IsekaiContext, "SeriousStrike", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Serious Strike");
				bp.SetDescription(Main.IsekaiContext, "Consecutive serious punches with limitless force. Any target struck by your attacks is obliterated instantly, dismembering them regardless of their hit points or defenses.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Hax;
				bp.AddComponent(delegate(AddOutgoingDamageTrigger c)
				{
					c.Actions = ActionFlow.DoSingle(delegate(ContextActionKill contextActionKill)
					{
						contextActionKill.Dismember = UnitState.DismemberType.LimbsApart;
					});
				});
			});
			BlueprintFeature Invincibility = Helpers.CreateBlueprint(Main.IsekaiContext, "Invincibility", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Invincibility");
				bp.SetDescription(Main.IsekaiContext, "Your physical vessel has transcended mortal vulnerability. Grants Damage Reduction 100/- against all physical attacks.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Hax;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 100;
				});
			});
			BlueprintFeature FasterThanLight = Helpers.CreateBlueprint(Main.IsekaiContext, "FasterThanLight", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Super Speed");
				bp.SetDescription(Main.IsekaiContext, "Attacks have a 75% chance to automatically miss you.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Hax;
				bp.AddComponent(delegate(SetAttackerMissChance c)
				{
					c.m_Type = SetAttackerMissChance.Type.All;
					c.Value = 75;
					c.Conditions = ActionFlow.EmptyCondition();
				});
			});
			BlueprintFeature NoHax = Helpers.CreateBlueprint(Main.IsekaiContext, "NoHax", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "None");
				bp.SetDescription(Main.IsekaiContext, "You decide not to abuse your power.");
				((BlueprintUnitFact)bp).m_Icon = null;
			});
			BlueprintFeature Ascension = Helpers.CreateBlueprint(Main.IsekaiContext, "Ascension", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Ascension");
				bp.SetDescription(Main.IsekaiContext, "You begin ascension to godhood, you are upgraded to Deity Status increasing all stats by 20.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Hax;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Other;
					c.Stat = StatType.Strength;
					c.Value = 20;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Other;
					c.Stat = StatType.Dexterity;
					c.Value = 20;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Other;
					c.Stat = StatType.Constitution;
					c.Value = 20;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Other;
					c.Stat = StatType.Intelligence;
					c.Value = 20;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Other;
					c.Stat = StatType.Wisdom;
					c.Value = 20;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Other;
					c.Stat = StatType.Charisma;
					c.Value = 20;
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "HaxSelection", delegate(BlueprintFeatureSelection bp)
			{
				bp.SetName(Main.IsekaiContext, "Hax");
				bp.SetDescription(Main.IsekaiContext, "So this is the result of 100 push-ups, 100 sit-ups, 100 squats and a 10km run...");
				((BlueprintUnitFact)bp).m_Icon = Icon_Hax;
				bp.Ranks = 2;
				bp.IsClassFeature = true;
				bp.m_AllFeatures = new BlueprintFeatureReference[5]
				{
					SeriousStrike.ToReference<BlueprintFeatureReference>(),
					Invincibility.ToReference<BlueprintFeatureReference>(),
					FasterThanLight.ToReference<BlueprintFeatureReference>(),
					NoHax.ToReference<BlueprintFeatureReference>(),
					Ascension.ToReference<BlueprintFeatureReference>()
				};
				bp.m_Features = bp.m_AllFeatures;
			});
		}
	}
}
