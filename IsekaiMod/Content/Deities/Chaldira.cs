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
	internal class Chaldira
	{
		private static readonly BlueprintFeature GoodDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("882521af8012fc749930b03dc18a69de");

		private static readonly BlueprintFeature ChaosDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("8c7d778bc39fec642befc1435b00f613");

		private static readonly BlueprintFeature TrickeryDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("eaa368e08628a8641b16cd41cbd2cb33");

		private static readonly BlueprintFeature LuckDomainAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("d4e192475bb1a1045859c7664addd461");

		private static readonly BlueprintFeature ChannelPositiveAllowed = BlueprintTools.GetBlueprint<BlueprintFeature>("8c769102f3996684fb6e09a2c4e7e5b9");

		private static readonly BlueprintArchetype FeralChampionArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("f68ca492c9c15e241ab73735fbd0fb9f");

		private static readonly BlueprintArchetype PriestOfBalance = BlueprintTools.GetBlueprint<BlueprintArchetype>("a4560e3fb5d247d68fb1a2738fcc0855");

		private static readonly BlueprintFeature ShortswordProficiency = BlueprintTools.GetBlueprint<BlueprintFeature>("9e828934974f0fc4bbf7542eb0446e45");

		private static readonly BlueprintItem ShortswordPlus1 = BlueprintTools.GetBlueprint<BlueprintItem>("9f455505128866146a9bd81895d4cecd");

		public static void Add()
		{
			Sprite Icon_Chaldira = AssetLoader.LoadInternal(Main.IsekaiContext, "Deities", "ICON_CHALDIRA.png");
			IsekaiDeitySelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "ChaldiraFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Chaldira Zuzaristan");
				bp.SetDescription(Main.IsekaiContext, "Chaldira Zuzaristan, known as the Calamitous Turn and the Mischievous Friend, is the halfling goddess of battle, luck, daring mischief, and plucky underdogs.\nA close friend and companion to Desna, Chaldira charges headlong into danger, trusting in boundless optimism, cheeky tricks, and sheer miraculous luck to see her through.\nIn the Constellation Chat, Chaldira is always rooting for the underdog hero, giggling at unexpected critical hits, and playfully teasing Desna and Cayden Cailean.\nDomains: Good, Chaos, Trickery, Luck\nFavoured Weapon: Shortsword");
				((BlueprintUnitFact)bp).m_Icon = Icon_Chaldira;
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
					c.Alignment = AlignmentMaskType.Good | AlignmentMaskType.TrueNeutral;
				});
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[5]
					{
						GoodDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						ChaosDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						TrickeryDomainAllowed.ToReference<BlueprintUnitFactReference>(),
						LuckDomainAllowed.ToReference<BlueprintUnitFactReference>(),
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
					c.m_Feature = ShortswordProficiency.ToReference<BlueprintFeatureReference>();
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
					c.m_BasicItems = new BlueprintItemReference[1] { ShortswordPlus1.ToReference<BlueprintItemReference>() };
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
