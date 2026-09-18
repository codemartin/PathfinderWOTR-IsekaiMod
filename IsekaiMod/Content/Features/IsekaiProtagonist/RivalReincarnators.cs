using System;
using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	public class RivalReincarnators : IUnitHandler, ISubscriber, IGlobalSubscriber, IUnitSpawnHandler, IUnitCombatHandler
	{
		private static bool _initialized;

		private static RivalReincarnators _instance;

		private static BlueprintBuff _rivalBuff;

		public static RivalReincarnators Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new RivalReincarnators();
				}
				return _instance;
			}
		}

		public static void Add()
		{
			Sprite Icon_Rival = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("247a4068296e8be42890143f451b4b45"))?.m_Icon;
			_rivalBuff = TTCoreExtensions.CreateBuff("RivalReincarnatorBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Rival Reincarnator");
				bp.SetDescription(Main.IsekaiContext, "A rival reincarnated soul from beyond the astral plane. Equipped with cheat abilities and anomalous plot resistance, they drop 250 Cosmic Coins upon defeat.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Rival;
				bp.m_Flags = BlueprintBuff.Flags.StayOnDeath;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddConcealment c)
				{
					c.Concealment = Concealment.Partial;
				});
				bp.AddComponent(delegate(AddSpellResistance c)
				{
					c.Value = new ContextValue
					{
						ValueType = ContextValueType.Simple,
						Value = 8
					};
				});
			});
			Init();
		}

		public static void Init()
		{
			if (!_initialized)
			{
				_initialized = true;
				EventBus.Subscribe(Instance);
			}
		}

		public void HandleUnitDeath(UnitEntityData unit)
		{
			OnUnitDie(unit);
		}

		public void HandleUnitDestroyed(UnitEntityData unit)
		{
			OnUnitDie(unit);
		}

		public void HandleUnitSpawned(UnitEntityData unit)
		{
		}

		public void OnUnitDie(UnitEntityData unit)
		{
			try
			{
				if (unit == null)
				{
					return;
				}
				if (_rivalBuff == null)
				{
					_rivalBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "RivalReincarnatorBuff");
				}
				if (_rivalBuff != null)
				{
					UnitDescriptor descriptor = unit.Descriptor;
					if (descriptor != null && descriptor.HasFact(_rivalBuff))
					{
						DivineTokens.AddCoins(250, "Rival Reincarnator Defeat");
						Main.IsekaiContext.Logger.Log("[Rival Reincarnator] Defeated rival " + unit.CharacterName + "! Awarded 250 Cosmic Coins.");
					}
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in RivalReincarnators.OnUnitDie: " + ex);
			}
		}

		public void HandleUnitJoinCombat(UnitEntityData unit)
		{
			try
			{
				UnitEntityData unitEntityData = BlueprintSafetyExtensions.SafeGetMainCharacter();
				if (unitEntityData == null || unit == null || !unit.IsEnemy(unitEntityData) || !Main.IsekaiContext.AddedContent.EnableRivalReincarnators)
				{
					return;
				}
				int num = unit.Blueprint?.CR ?? 0;
				int num2 = unit.Progression?.CharacterLevel ?? 0;
				if (num >= 12 || num2 >= 12)
				{
					if (_rivalBuff == null)
					{
						_rivalBuff = BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.IsekaiContext, "RivalReincarnatorBuff");
					}
					if (_rivalBuff != null && unit.Descriptor != null && !unit.Descriptor.HasFact(_rivalBuff) && !string.IsNullOrEmpty(unit.UniqueId) && (uint)unit.UniqueId.GetHashCode() % 5u == 0)
					{
						unit.Descriptor.AddFact(_rivalBuff);
					}
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Error in RivalReincarnators.HandleUnitJoinCombat: " + ex);
			}
		}

		public void HandleUnitLeaveCombat(UnitEntityData unit)
		{
		}
	}
}
