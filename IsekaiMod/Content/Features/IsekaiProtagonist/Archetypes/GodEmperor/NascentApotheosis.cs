using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor
{
	internal class NascentApotheosis
	{
		public static void Add()
		{
			Sprite Icon_Serenity = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("d316d3d94d20c674db2c24d7de96f6a7")).m_Icon;
			Helpers.CreateBlueprint(Main.IsekaiContext, "NascentApotheosis", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Nascent Apotheosis");
				bp.SetDescription(Main.IsekaiContext, "The God Emperor gains an inherent bonus to all attributes equal to 1/2 their character level and {g|Encyclopedia:Damage_Reduction}DR{/g}/- and spell resistance equal to their character level.\nAs they increase their level, they gain more immunities.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Serenity;
				bp.ReapplyOnLevelUp = true;
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Strength;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Dexterity;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Constitution;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Intelligence;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Wisdom;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Charisma;
					c.Value = Values.CreateContextRankValue(AbilityRankType.StatBonus);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.StatBonus;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
					c.m_Progression = ContextRankProgression.Div2;
				});
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
				});
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
				});
				bp.AddComponent(delegate(ContextRankConfig c)
				{
					c.m_Type = AbilityRankType.Default;
					c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 1;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "BlindImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 1;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "DazzledImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 2;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "ShakenImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 2;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "SickenedImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 3;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "SlowImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 3;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "EntangledImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 4;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "StaggeredImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 4;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "FrightenedImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 5;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "PoisonImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 5;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "DiseaseImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 6;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "BleedImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 6;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "StunImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 7;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "ConfusionImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 7;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "SleepImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 8;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "MovementImpairingImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 8;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "CoweringImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 9;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "FearImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 9;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "StatDamageNegativeLevelImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 10;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "SneakAttackImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 10;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "CriticalHitImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 11;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "CharmImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 11;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "CurseImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 12;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "CompulsionImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 12;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "MindAffectingImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 13;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "DazedImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 13;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "NauseatedImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 14;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "HexImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 14;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "DeathImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 15;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "ParalyzedImmunity");
				});
				bp.AddComponent(delegate(AddFeatureOnClassLevel c)
				{
					c.m_Class = IsekaiProtagonistClass.GetReference();
					c.Level = 15;
					c.m_Feature = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(Main.IsekaiContext, "PetrifiedImmunity");
				});
			});
		}
	}
}
