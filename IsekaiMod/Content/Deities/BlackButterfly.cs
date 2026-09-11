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
	internal class BlackButterfly
	{
		private static readonly BlueprintFeature DarknessDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("6d8e7accdd882e949a63021af5cde4b8");

		private static readonly BlueprintFeature LiberationDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("801ca88338451a546bca2ee59da87c53");

		private static readonly BlueprintFeature ProtectionDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("d4ce7592bd12d63439907ad64e986e59");

		private static readonly BlueprintFeature GoodDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("882521af8012fc749930b03dc18a69de");

		private static readonly BlueprintFeature ChannelPositiveAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("8c769102f3996684fb6e09a2c4e7e5b9");

		private static readonly BlueprintArchetype FeralChampionArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("f68ca492c9c15e241ab73735fbd0fb9f");

		private static readonly BlueprintArchetype PriestOfBalance = BlueprintTools.GetBlueprint<BlueprintArchetype>("a4560e3fb5d247d68fb1a2738fcc0855");

		private static readonly BlueprintFeature StarknifeProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("7818ba3db79ac064e88fa14a2478b24b");

		private static readonly BlueprintItem StarknifePlus1 = BlueprintTools.GetBlueprint<BlueprintItem>("bacea00ed10657043bf90641eeabde95");

		public static void Add()
		{
			Sprite Icon_BlackButterfly = AssetLoader.LoadInternal(Main.IsekaiContext, "Deities", "ICON_BLACK_BUTTERFLY.png");
			IsekaiDeitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BlackButterflyFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "The Black Butterfly");
				bp.SetDescription(Main.IsekaiContext, "The Black Butterfly, also revered as the Silence Between the Stars, is the shadow of Desna born when starlight first broke the eternal dark of the Great Beyond.\nShe watches over the desolate voids of the Dark Tapestry, bringing solace to the isolated and quietly defending mortal realms from ancient horrors beyond space.\nHer followers value silence, contemplation, and the freedom of forgotten wanderers.\nDomains: Darkness, Liberation, Protection, Good\nFavoured Weapon: Starknife");
				((BlueprintUnitFact)bp).m_Icon = Icon_BlackButterfly;
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
					c.Alignment = AlignmentMaskType.NeutralGood | AlignmentMaskType.ChaoticGood | AlignmentMaskType.TrueNeutral | AlignmentMaskType.ChaoticNeutral;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[5]
					{
						DarknessDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						LiberationDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						ProtectionDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						GoodDomainAllowed.ToReference<BlueprintUnitFactReference>(),
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
					c.m_Feature = StarknifeProficiency.ToReference<BlueprintFeatureReference>();
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
					c.m_BasicItems = new BlueprintItemReference[1] { StarknifePlus1.ToReference<BlueprintItemReference>() };
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
