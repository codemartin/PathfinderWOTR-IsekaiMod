using System;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Quests
{
	public static class MythicQuestSynergies
	{
		private static bool Added = false;

		private static readonly Sprite Icon_Mythic = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9a56368c28795544fbeb43fe70e1a40d"))?.m_Icon;

		public static BlueprintFeature MythicResonanceBoonFeature { get; private set; }

		public static BlueprintBuff MythicResonanceAct3Buff { get; private set; }

		public static BlueprintBuff MythicResonanceAct4Buff { get; private set; }

		public static BlueprintBuff MythicResonanceAct5Buff { get; private set; }

		public static void Add()
		{
			if (Added)
			{
				return;
			}
			Added = true;
			MythicResonanceAct3Buff = TTCoreExtensions.CreateBuff("MythicResonanceAct3Buff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Mythic Resonance: Planar Anchorage (Act 3)");
				bp.SetDescription(Main.IsekaiContext, "Your otherworldly soul resonates with Golarion's ley lines as the crusader flag is planted at Drezen. Grants a +2 inherent bonus to all ability scores and a +10 ft bonus to base movement speed.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Mythic;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Strength;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Dexterity;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Constitution;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Intelligence;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Wisdom;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Inherent;
					c.Stat = StatType.Charisma;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.Speed;
					c.Value = 10;
				});
			});
			MythicResonanceAct4Buff = TTCoreExtensions.CreateBuff("MythicResonanceAct4Buff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Mythic Resonance: Abyssal Inversion (Act 4)");
				bp.SetDescription(Main.IsekaiContext, "Treading through the chaotic depths of Alushinyrra, your protagonist aura refuses planar suppression. Increases the DC of all your spells by +2, and all your attacks bypass DR/cold iron and DR/good.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Mythic;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 2;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Enhancement;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
			});
			MythicResonanceAct5Buff = TTCoreExtensions.CreateBuff("MythicResonanceAct5Buff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Mythic Resonance: Transcendent Sovereignty (Act 5)");
				bp.SetDescription(Main.IsekaiContext, "Standing at the threshold of cosmic ascendance, your power rivals the demigods. You gain a +2 bonus to Base Attack Bonus, +4 to all saving throws, and +20% maximum hit points.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Mythic;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.UntypedStackable;
					c.Stat = StatType.BaseAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Resistance;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
			});
			MythicResonanceBoonFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "MythicResonanceBoonFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Mythic Resonance Boon");
				bp.SetDescription(Main.IsekaiContext, "Your otherworlder soul establishes an unbreakable resonance with mythic tiers. Regardless of class respecs or shifting mythic alignments, this planar resonance safely scales across Acts 3, 4, and 5.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Mythic;
				bp.IsClassFeature = true;
				bp.Ranks = 1;
			});
		}

		public static void CheckAndApplyResonance(UnitEntityData mainChar)
		{
			if (mainChar == null || mainChar.Descriptor == null || mainChar.Descriptor.Buffs == null)
			{
				return;
			}
			try
			{
				int num = Game.Instance?.Player?.Chapter ?? 1;
				if (num >= 3 && MythicResonanceAct3Buff != null && !mainChar.Descriptor.Buffs.HasFact(MythicResonanceAct3Buff))
				{
					mainChar.Descriptor.Buffs.AddBuff(MythicResonanceAct3Buff, mainChar, null);
				}
				if (num >= 4 && MythicResonanceAct4Buff != null && !mainChar.Descriptor.Buffs.HasFact(MythicResonanceAct4Buff))
				{
					mainChar.Descriptor.Buffs.AddBuff(MythicResonanceAct4Buff, mainChar, null);
				}
				if (num >= 5 && MythicResonanceAct5Buff != null && !mainChar.Descriptor.Buffs.HasFact(MythicResonanceAct5Buff))
				{
					mainChar.Descriptor.Buffs.AddBuff(MythicResonanceAct5Buff, mainChar, null);
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in CheckAndApplyResonance: " + ex);
			}
		}
	}
}
