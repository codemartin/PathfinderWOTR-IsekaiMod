using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Narrative.Actions
{
	public static class NarrativeCustomActions
	{
		private static bool _initialized;

		public static BlueprintBuff SilverDragonAegisBuff;

		public static BlueprintBuff MindOfTheOtherworlderBuff;

		public static BlueprintBuff StitchWardBuff;

		public static BlueprintBuff EmberSparkBuff;

		public static BlueprintFeature LarielsVowFeature;

		public static BlueprintFeature ResonantMythicSparkFeature;

		public static BlueprintItem ItemSilverDragonShard;

		public static BlueprintItemEquipmentUsable ItemTearOfSilverDragon;

		public static BlueprintItem ItemLarielFeather;

		public static BlueprintItem ItemThieflingCharm;

		public static BlueprintItem ItemRoyalMendevianSignet;

		public static BlueprintItem ItemHellknightOfficerRegalia;

		public static BlueprintItemEquipmentUsable ItemVescavorPheromoneFlask;

		public static BlueprintBuff CrusaderLiberatorBuff;

		public static BlueprintFeature SwordOfValorResonantFeature;

		public static BlueprintItemWeapon ItemGwermAncestralRapier;

		public static BlueprintItemEquipmentRing ItemRingOfTheAncientChronicler;

		public static BlueprintItem ItemThieflingMasterLockpicks;

		public static BlueprintItem ItemNenioInscribedJournal;

		public static BlueprintBuff TavernDefianceMoraleBuff;

		public static void InitBuffsAndItems()
		{
			if (!_initialized)
			{
				_initialized = true;
				SilverDragonAegisBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "SilverDragonAegisBuff");
				MindOfTheOtherworlderBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "MindOfTheOtherworlderBuff");
				StitchWardBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "StitchWardBuff");
				EmberSparkBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "EmberSparkBuff");
				LarielsVowFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "LarielsVowFeature");
				ResonantMythicSparkFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "ResonantMythicSparkFeature");
				ItemSilverDragonShard = BlueprintTools.GetModBlueprint<BlueprintItem>(Main.IsekaiContext, "ItemSilverDragonShard");
				ItemTearOfSilverDragon = BlueprintTools.GetModBlueprint<BlueprintItemEquipmentUsable>(Main.IsekaiContext, "ItemTearOfSilverDragon");
				ItemLarielFeather = BlueprintTools.GetModBlueprint<BlueprintItem>(Main.IsekaiContext, "ItemLarielFeather");
				ItemThieflingCharm = BlueprintTools.GetModBlueprint<BlueprintItem>(Main.IsekaiContext, "ItemThieflingCharm");
				ItemRoyalMendevianSignet = BlueprintTools.GetModBlueprint<BlueprintItem>(Main.IsekaiContext, "ItemRoyalMendevianSignet");
				ItemHellknightOfficerRegalia = BlueprintTools.GetModBlueprint<BlueprintItem>(Main.IsekaiContext, "ItemHellknightOfficerRegalia");
				ItemVescavorPheromoneFlask = BlueprintTools.GetModBlueprint<BlueprintItemEquipmentUsable>(Main.IsekaiContext, "ItemVescavorPheromoneFlask");
				CrusaderLiberatorBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "CrusaderLiberatorBuff");
				SwordOfValorResonantFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "SwordOfValorResonantFeature");
				ItemGwermAncestralRapier = BlueprintTools.GetModBlueprint<BlueprintItemWeapon>(Main.IsekaiContext, "ItemGwermAncestralRapier");
				ItemRingOfTheAncientChronicler = BlueprintTools.GetModBlueprint<BlueprintItemEquipmentRing>(Main.IsekaiContext, "ItemRingOfTheAncientChronicler");
				ItemThieflingMasterLockpicks = BlueprintTools.GetModBlueprint<BlueprintItem>(Main.IsekaiContext, "ItemThieflingMasterLockpicks");
				ItemNenioInscribedJournal = BlueprintTools.GetModBlueprint<BlueprintItem>(Main.IsekaiContext, "ItemNenioInscribedJournal");
				TavernDefianceMoraleBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "TavernDefianceMoraleBuff");
			}
		}
	}
}
