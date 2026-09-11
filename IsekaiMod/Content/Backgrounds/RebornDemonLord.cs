using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds
{
	internal class RebornDemonLord
	{
		public static void Add()
		{
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundRebornDemonLord", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Reborn Demon Lord");
				bp.SetBackgroundDescription(Main.IsekaiContext, "The Reborn Demon Lord has a +2 bonus to strength and electricity resistance 20.");
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Trait;
					c.Stat = StatType.Strength;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddDamageResistanceEnergy c)
				{
					c.Type = DamageEnergyType.Electricity;
					c.Value = 20;
				});
			}));
		}
	}
}
