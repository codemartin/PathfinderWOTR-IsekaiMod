using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Dialogue
{
	internal class IsekaiFinnean
	{
		public static BlueprintBuff FinneanAwakenedEdgeBuff;

		public static void Add()
		{
			CreateBuffs();
		}

		public static void CreateBuffs()
		{
			if (FinneanAwakenedEdgeBuff != null)
			{
				return;
			}
			Sprite iconCoin = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_COSMIC_COIN.png");
			FinneanAwakenedEdgeBuff = TTCoreExtensions.CreateBuff("FinneanAwakenedEdgeBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Finnean's Awakened Resonance");
				bp.SetDescription(Main.IsekaiContext, "Resonating with the soul of an Otherworlder, Finnean hums with pure spectral luminescence. Grants a +1 competence bonus to attack rolls and weapon damage, and causes weapon attacks to strike incorporeal foes as though possessing Ghost Touch.");
				((BlueprintUnitFact)bp).m_Icon = iconCoin;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 1;
				});
				bp.AddComponent(delegate(AddOutgoingPhysicalDamageProperty c)
				{
					c.AffectAnyPhysicalDamage = true;
					c.AddReality = true;
					c.Reality = DamageRealityType.Ghost;
				});
			});
		}
	}
}
