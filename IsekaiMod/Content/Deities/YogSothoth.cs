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
	internal class YogSothoth
	{
		private static readonly BlueprintFeature DarknessDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("6d8e7accdd882e949a63021af5cde4b8");

		private static readonly BlueprintFeature ChaosDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("8c7d778bc39fec642befc1435b00f613");

		private static readonly BlueprintFeature TravelDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("c008853fe044bd442ae8bd22260592b7");

		private static readonly BlueprintFeature KnowledgeDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("443d44b3e0ea84046a9bf304c82a0425");

		private static readonly BlueprintFeature MadnessDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("c346bcc77a6613040b3aa915b1ceddec");

		private static readonly BlueprintFeature ChannelNegativeAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("dab5255d809f77c4395afc2b713e9cd6");

		private static readonly BlueprintArchetype FeralChampionArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("f68ca492c9c15e241ab73735fbd0fb9f");

		private static readonly BlueprintArchetype PriestOfBalance = BlueprintTools.GetBlueprint<BlueprintArchetype>("a4560e3fb5d247d68fb1a2738fcc0855");

		private static readonly BlueprintArchetype AngelfireApostle = BlueprintTools.GetBlueprint<BlueprintArchetype>("857bc9fadf70f294795a9cba974a48b8");

		private static readonly BlueprintFeature DaggerProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("b776c19291928cf4184d4dc65f09f3a6");

		private static readonly BlueprintItem DaggerPlus1 = BlueprintTools.GetBlueprint<BlueprintItem>("2a45458f776442e43bba57de65f9b738");

		public static void Add()
		{
			Sprite Icon_YogSothoth = AssetLoader.LoadInternal(Main.IsekaiContext, "Deities", "ICON_YOG_SOTHOTH.png");
			IsekaiDeitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "YogSothothFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "★ Yog-Sothoth");
				bp.SetDescription(Main.IsekaiContext, "<color=#F5C542><b>[★ Reincarnation Patron ★]</b></color>\nYog-Sothoth, known across cosmos as the Lurker at the Threshold and the Key and the Gate, is an enigmatic Outer God coterminous with all space, time, and dimensional rifts.\nWhen the Worldwound ripped Golarion's planar flesh, it was Yog-Sothoth who pierced the barrier of reality and guided your soul from Earth across the cosmic void into this world.\nAs an Outer God beyond mortal comprehension, Yog-Sothoth is entirely genderless, perceiving all timelines simultaneously and observing your crusade as a single fascinating permutation in the infinite tapestry.\nDomains: Darkness, Chaos, Travel, Knowledge, Madness\nFavoured Weapon: Dagger");
				((BlueprintUnitFact)bp).m_Icon = Icon_YogSothoth;
				bp.HideInCharacterSheetAndLevelUp = false;
				bp.Groups = new FeatureGroup[1] { FeatureGroup.Deities };
				bp.AddComponent(delegate(PrerequisiteNoArchetype c)
				{
					c.m_CharacterClass = ClassTools.ClassReferences.ClericClass;
					c.m_Archetype = PriestOfBalance.ToReference<BlueprintArchetypeReference>();
				});
				bp.AddComponent(delegate(PrerequisiteNoArchetype c)
				{
					c.m_CharacterClass = ClassTools.ClassReferences.ClericClass;
					c.m_Archetype = AngelfireApostle.ToReference<BlueprintArchetypeReference>();
				});
				bp.AddComponent(delegate(PrerequisiteNoArchetype c)
				{
					c.m_CharacterClass = ClassTools.ClassReferences.WarpriestClass;
					c.m_Archetype = FeralChampionArchetype.ToReference<BlueprintArchetypeReference>();
				});
				bp.AddComponent(delegate(PrerequisiteAlignment c)
				{
					c.Alignment = AlignmentMaskType.TrueNeutral | AlignmentMaskType.ChaoticNeutral | AlignmentMaskType.NeutralEvil | AlignmentMaskType.ChaoticEvil;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[6]
					{
						DarknessDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						ChaosDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						TravelDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						KnowledgeDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						MadnessDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						ChannelNegativeAllowed.ToReference<BlueprintUnitFactReference>()
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
					c.m_Feature = DaggerProficiency.ToReference<BlueprintFeatureReference>();
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
					c.m_BasicItems = new BlueprintItemReference[1] { DaggerPlus1.ToReference<BlueprintItemReference>() };
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
