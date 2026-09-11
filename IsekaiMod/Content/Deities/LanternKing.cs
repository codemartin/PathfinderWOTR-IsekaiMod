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
	internal class LanternKing
	{
		private static readonly BlueprintFeature TrickeryDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("eaa368e08628a8641b16cd41cbd2cb33");

		private static readonly BlueprintFeature ChaosDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("8c7d778bc39fec642befc1435b00f613");

		private static readonly BlueprintFeature CharmDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("f1ceba79ee123cc479cece27bc994ff2");

		private static readonly BlueprintFeature LuckDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("d4e192475bb1a1045859c7664addd461");

		private static readonly BlueprintFeature ChannelPositiveAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("8c769102f3996684fb6e09a2c4e7e5b9");

		private static readonly BlueprintFeature ChannelNegativeAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("dab5255d809f77c4395afc2b713e9cd6");

		private static readonly BlueprintArchetype FeralChampionArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("f68ca492c9c15e241ab73735fbd0fb9f");

		private static readonly BlueprintArchetype PriestOfBalance = BlueprintTools.GetBlueprint<BlueprintArchetype>("a4560e3fb5d247d68fb1a2738fcc0855");

		private static readonly BlueprintArchetype AngelfireApostle = BlueprintTools.GetBlueprint<BlueprintArchetype>("857bc9fadf70f294795a9cba974a48b8");

		private static readonly BlueprintFeature DaggerProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("b776c19291928cf4184d4dc65f09f3a6");

		private static readonly BlueprintItem DaggerPlus1 = BlueprintTools.GetBlueprint<BlueprintItem>("2a45458f776442e43bba57de65f9b738");

		public static void Add()
		{
			Sprite Icon_LanternKing = AssetLoader.LoadInternal(Main.IsekaiContext, "Deities", "ICON_LANTERN_KING.png");
			IsekaiDeitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "LanternKingFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "The Lantern King");
				bp.SetDescription(Main.IsekaiContext, "The Lantern King is an ancient Eldest of the First World, a primordial realm unbound by the mortal River of Souls or linear time. Known as the Laughing King, the Lord of Pranks, and the Shapeshifting Spark, he treats the entire multiverse as a grand stage for his cosmic comedy.\nWhile other gods are constrained by cosmic pacts and divine non-intervention, the Lantern King's nature as a First World trickster allows his consciousness to slip between the cracks of reality. He finds your presence in this temporal loop hilariously entertaining, offering his chaotic blessing to those who dare to laugh in the face of destiny.\nDomains: Trickery, Chaos, Charm, Luck\nFavoured Weapon: Dagger");
				((BlueprintUnitFact)bp).m_Icon = Icon_LanternKing;
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
					c.Alignment = AlignmentMaskType.Chaotic | AlignmentMaskType.TrueNeutral;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[6]
					{
						TrickeryDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						ChaosDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						CharmDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						LuckDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						ChannelPositiveAllowed.ToReference<BlueprintUnitFactReference>(),
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
