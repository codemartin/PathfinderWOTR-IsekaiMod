using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility
{
	internal class AutoMetamagicSelection
	{
		public static BlueprintFeature CosmicAutoQuickenFeature;

		public static void Add()
		{
			Sprite Icon_AutoMagic = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_AUTO_MAGIC.png");
			Sprite icon = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("fbf5d9ce931f47f3a0c818b3f8ef8414")).m_Icon;
			Sprite icon2 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("a1de1e4f92195b442adb946f0e2b9d4e")).m_Icon;
			Sprite icon3 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("f180e72e4a9cbaa4da8be9bc958132ef")).m_Icon;
			Sprite icon4 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("7f2b282626862e345935bbea5e66424b")).m_Icon;
			Sprite icon5 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("ef7ece7bb5bb66a41b256976b27f424e")).m_Icon;
			Sprite icon6 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("46fad72f54a33dc4692d3b62eca7bb78")).m_Icon;
			Sprite icon7 = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("85f3340093d144dd944fff9a9adfd2f2")).m_Icon;
			BlueprintFeature bp = CreateAutoMetamagicFeature("AutoSelective", "Overpowered Ability - Auto Selective", "Every time you cast a spell, you can exclude targets from the effects of your spell, as though using the Selective Spell feat.", icon7, Metamagic.Selective);
			BlueprintFeature bp2 = CreateAutoMetamagicFeature("AutoExtend", "Overpowered Ability - Auto Extend", "Every time you cast a spell, it becomes extended, as though using the Extend Spell feat.", icon3, Metamagic.Extend);
			BlueprintFeature bp3 = CreateAutoMetamagicFeature("AutoReach", "Overpowered Ability - Auto Reach", "Every time you cast a spell, it increases its range by one range category, as though using the Reach Spell feat.", icon6, Metamagic.Reach);
			BlueprintFeature bp4 = CreateAutoMetamagicFeature("AutoBolster", "Overpowered Ability - Auto Bolster", "Every time you cast a spell, it becomes bolstered, as though using the Bolster Spell feat.", icon, Metamagic.Bolstered, 0, 5);
			BlueprintFeature bp5 = CreateAutoMetamagicFeature("AutoEmpower", "Overpowered Ability - Auto Empower", "Every time you cast a spell, it becomes empowered, as though using the Empower Spell feat.", icon2, Metamagic.Empower, 0, 5);
			BlueprintFeature bp6 = CreateAutoMetamagicFeature("AutoMaximize", "Overpowered Ability - Auto Maximize", "Every time you cast a spell, it becomes maximized, as though using the Maximize Spell feat. Requires character level 10.", icon4, Metamagic.Maximize, 0, 10);
			BlueprintFeature bp7 = CreateAutoMetamagicFeature("AutoQuicken", "Overpowered Ability - Auto Quicken", "Every time you cast a spell of 5th level or lower, it becomes quickened, as though using the Quicken Spell feat. Requires character level 15.", icon5, Metamagic.Quicken, 5, 15);
			CosmicAutoQuickenFeature = CreateAutoMetamagicFeature("CosmicAutoQuicken", "Cosmic Quicken Matrix", "Every time you cast a spell, it becomes quickened regardless of spell level (1st through 10th level spells), as though using the Quicken Spell feat. Requires character level 21.", icon5, Metamagic.Quicken, 0, 21);
			LocalizedString AutoMetamagicDesc = Helpers.CreateString(Main.IsekaiContext, "AutometamagicSelection.Description", "You gain powerful metamagic effects that automatically apply to your spells, with higher tier metamagics unlocking as your character level increases.");
			BlueprintFeatureReference[] allMetamagicList = new BlueprintFeatureReference[8]
			{
				bp.ToReference<BlueprintFeatureReference>(),
				bp2.ToReference<BlueprintFeatureReference>(),
				bp3.ToReference<BlueprintFeatureReference>(),
				bp4.ToReference<BlueprintFeatureReference>(),
				bp5.ToReference<BlueprintFeatureReference>(),
				bp6.ToReference<BlueprintFeatureReference>(),
				bp7.ToReference<BlueprintFeatureReference>(),
				CosmicAutoQuickenFeature.ToReference<BlueprintFeatureReference>()
			};
			BlueprintFeatureSelection feature = Helpers.CreateBlueprint(Main.IsekaiContext, "AutoMetamagicSelection", delegate(BlueprintFeatureSelection blueprintFeatureSelection)
			{
				blueprintFeatureSelection.SetName(Main.IsekaiContext, "Overpowered Ability - Auto Metamagic");
				blueprintFeatureSelection.SetDescription(AutoMetamagicDesc);
				((BlueprintUnitFact)blueprintFeatureSelection).m_Icon = Icon_AutoMagic;
				blueprintFeatureSelection.Ranks = 5;
				blueprintFeatureSelection.IsClassFeature = true;
				blueprintFeatureSelection.m_AllFeatures = allMetamagicList;
				blueprintFeatureSelection.m_Features = allMetamagicList;
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "AutoMetamagicSelectionMastermind", delegate(BlueprintFeatureSelection blueprintFeatureSelection)
			{
				blueprintFeatureSelection.SetName(Main.IsekaiContext, "Auto Metamagic");
				blueprintFeatureSelection.SetDescription(AutoMetamagicDesc);
				((BlueprintUnitFact)blueprintFeatureSelection).m_Icon = Icon_AutoMagic;
				blueprintFeatureSelection.Ranks = 5;
				blueprintFeatureSelection.IsClassFeature = true;
				blueprintFeatureSelection.m_AllFeatures = allMetamagicList;
				blueprintFeatureSelection.m_Features = allMetamagicList;
			});
			OverpoweredAbilitySelection.AddToSelection(feature);
		}

		public static void AddToSelection(BlueprintFeature feature)
		{
			BlueprintFeatureSelection modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "AutoMetamagicSelection");
			BlueprintFeatureSelection modBlueprint2 = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "AutoMetamagicSelectionMastermind");
			modBlueprint.AddToSelection(feature);
			modBlueprint2.AddToSelection(feature);
		}

		private static BlueprintFeature CreateAutoMetamagicFeature(string name, string displayName, string description, Sprite icon, Metamagic metamagic, int maxSpellLevel = 0, int minLevel = 0)
		{
			BlueprintFeature blueprintFeature = TTCoreExtensions.CreateToggleBuffFeature(name, displayName, description, icon, delegate(BlueprintBuff bp)
			{
				bp.AddComponent(delegate(AutoMetamagic c)
				{
					c.m_AllowedAbilities = AutoMetamagic.AllowedType.SpellOnly;
					c.Metamagic = metamagic;
					c.MaxSpellLevel = maxSpellLevel;
				});
			});
			if (minLevel > 0)
			{
				blueprintFeature.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = minLevel;
				});
			}
			return blueprintFeature;
		}
	}
}
