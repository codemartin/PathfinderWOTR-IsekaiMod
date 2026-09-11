using IsekaiMod.Content.Classes.Deathsnatcher;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.Deathsnatcher
{
	internal class DeathsnatcherSizeBaby
	{
		private static readonly BlueprintUnitFact NaturalArmor10 = BlueprintTools.GetBlueprint<BlueprintUnitFact>("4179c5c08d606a6439a62bf178b738e1");

		private static readonly BlueprintUnitFact NaturalArmor16 = BlueprintTools.GetBlueprint<BlueprintUnitFact>("73a90b2a70d576f429ad401e7a5a8a4f");

		public static void Add()
		{
			LocalizedString DeathsnatcherSizeBabyDesc = Helpers.CreateString(Main.IsekaiContext, "DeathsnatcherSizeBaby.Description", "The Deathsnatcher begins as a small apex cub, maturing at 4th level. At 4th level, its size becomes Medium, natural armor increases to +8 (and to +12 at 12th level), gaining Pounce and adult ferocity.");
			BlueprintBuff DeathsnatcherSizeBabyBuff = TTCoreExtensions.CreateBuff("DeathsnatcherSizeBabyBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Baby Deathsnatcher");
				bp.SetDescription(DeathsnatcherSizeBabyDesc);
				bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
				bp.AddComponent(delegate(ChangeUnitSize c)
				{
					c.m_Type = ChangeUnitSize.ChangeType.Delta;
					c.SizeDelta = -2;
					c.Size = Size.Fine;
				});
				bp.AddComponent(delegate(AddGenericStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Size;
					c.Stat = StatType.Strength;
					c.Value = -4;
				});
				bp.AddComponent(delegate(AddGenericStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Size;
					c.Stat = StatType.Dexterity;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddGenericStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Size;
					c.Stat = StatType.Constitution;
					c.Value = -2;
				});
			});
			BlueprintFeature DeathsnatcherSizeBaby = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherSizeBaby", delegate(BlueprintFeature bp)
			{
				bp.HideInUI = true;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { DeathsnatcherSizeBabyBuff.ToReference<BlueprintUnitFactReference>() };
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
			});
			BlueprintFeature DeathsnatcherNaturalArmor = Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherNaturalArmor", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Deathsnatcher Armor");
				bp.SetDescription(Main.IsekaiContext, "The mature Deathsnatcher has +8 natural armor bonus to AC (+12 at 12th level).");
				bp.HideInUI = true;
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.NaturalArmor;
					c.Stat = StatType.AC;
					c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
					c.m_Class = new BlueprintCharacterClassReference[1] { DeathsnatcherClass.GetReference() };
					c.m_Progression = ContextRankProgression.Custom;
					c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[2]
					{
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 11,
							ProgressionValue = 8
						},
						new ContextRankConfig.CustomProgressionItem
						{
							BaseValue = 100,
							ProgressionValue = 12
						}
					};
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "DeathsnatcherSizeBabyFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Baby Deathsnatcher");
				bp.SetDescription(DeathsnatcherSizeBabyDesc);
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = DeathsnatcherClass.GetReference();
					c.Level = 4;
					c.m_Feature = DeathsnatcherSizeBaby.ToReference<BlueprintFeatureReference>();
					c.BeforeThisLevel = true;
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = DeathsnatcherClass.GetReference();
					c.Level = 4;
					c.m_Feature = DeathsnatcherNaturalArmor.ToReference<BlueprintFeatureReference>();
					c.BeforeThisLevel = false;
				});
			});
		}
	}
}
