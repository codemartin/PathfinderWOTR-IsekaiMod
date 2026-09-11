using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Deities
{
	internal class Milani
	{
		private static readonly BlueprintFeature ChaosDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("8c7d778bc39fec642befc1435b00f613");

		private static readonly BlueprintFeature GoodDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("882521af8012fc749930b03dc18a69de");

		private static readonly BlueprintFeature HealingDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("73ae164c388990c43ade94cfe8ed5755");

		private static readonly BlueprintFeature LiberationDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("801ca88338451a546bca2ee59da87c53");

		private static readonly BlueprintFeature ProtectionDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("d4ce7592bd12d63439907ad64e986e59");

		private static readonly BlueprintFeature ChannelPositiveAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("8c769102f3996684fb6e09a2c4e7e5b9");

		private static readonly BlueprintArchetype FeralChampionArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("f68ca492c9c15e241ab73735fbd0fb9f");

		private static readonly BlueprintArchetype PriestOfBalance = BlueprintTools.GetBlueprint<BlueprintArchetype>("a4560e3fb5d247d68fb1a2738fcc0855");

		private static readonly BlueprintFeature HeavyMaceProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("3f18330d717ea0148b496ee8cc291a60");

		private static readonly BlueprintItem HeavyMacePlus1 = BlueprintTools.GetBlueprint<BlueprintItem>("86d5d758c2dd24747bad1c7b1f32e9df");

		public static void Add()
		{
			Sprite Icon_Milani = AssetLoader.LoadInternal(Main.IsekaiContext, "Deities", "ICON_MILANI.png");
			IsekaiDeitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "MilaniFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Milani");
				bp.SetDescription(Main.IsekaiContext, "Milani, known as the Everbloom, is the goddess of hope, devotion, and desperate uprisings against tyrannical oppressors.\nOnce a mortal who fought tirelessly for freedom, she rose to divinity and now shields partisans, rebels, and crusaders fighting against impossible odds.\nHer followers kindle hope in the darkest of battlefields, standing steadfast against demon lords and tyrants alike.\nDomains: Chaos, Good, Healing, Liberation, Protection\nFavoured Weapon: Morningstar / Heavy Mace");
				((BlueprintUnitFact)bp).m_Icon = Icon_Milani;
				bp.HideInCharacterSheetAndLevelUp = false;
				bp.Groups = new FeatureGroup[1] { FeatureGroup.Deities };
				bp.AddComponent(delegate(PrerequisiteNoArchetype c)
				{
					c.m_CharacterClass = ClassTools.ClassReferences.ClericClass;
					c.m_Archetype = PriestOfBalance.ToReference<BlueprintArchetypeReference>();
				});
				bp.AddComponent(delegate(PrerequisiteNoArchetype c)
				{
					c.m_CharacterClass = ClassTools.ClassReferences.WarpriestClass;
					c.m_Archetype = FeralChampionArchetype.ToReference<BlueprintArchetypeReference>();
				});
				bp.AddComponent(delegate(PrerequisiteAlignment c)
				{
					c.Alignment = AlignmentMaskType.Good;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[6]
					{
						ChaosDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						GoodDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						HealingDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						LiberationDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						ProtectionDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						ChannelPositiveAllowed.ToReference<BlueprintUnitFactReference>()
					};
				});
				bp.AddComponent(delegate(ForbidSpellbookOnAlignmentDeviation c)
				{
					c.m_Spellbooks = new BlueprintSpellbookReference[3]
					{
						SpellTools.Spellbook.CrusaderSpellbook.ToReference<BlueprintSpellbookReference>(),
						SpellTools.Spellbook.ClericSpellbook.ToReference<BlueprintSpellbookReference>(),
						SpellTools.Spellbook.InquisitorSpellbook.ToReference<BlueprintSpellbookReference>()
					};
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = ClassTools.ClassReferences.ClericClass;
					c.m_Feature = HeavyMaceProficiency.ToReference<BlueprintFeatureReference>();
					c.Level = 1;
					c.m_Archetypes = null;
					c.m_AdditionalClasses = new BlueprintCharacterClassReference[2]
					{
						ClassTools.ClassReferences.InquisitorClass,
						ClassTools.ClassReferences.WarpriestClass
					};
				});
				bp.AddComponent(delegate(AddStartingEquipment c)
				{
					c.m_BasicItems = new BlueprintItemReference[1] { HeavyMacePlus1.ToReference<BlueprintItemReference>() };
					c.m_RestrictedByClass = new BlueprintCharacterClassReference[3]
					{
						ClassTools.ClassReferences.ClericClass,
						ClassTools.ClassReferences.InquisitorClass,
						ClassTools.ClassReferences.WarpriestClass
					};
				});
			}));
		}
	}
}
