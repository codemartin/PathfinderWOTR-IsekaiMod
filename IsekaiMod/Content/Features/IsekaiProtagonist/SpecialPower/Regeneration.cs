using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Buffs.Components;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class Regeneration
	{
		public static void Add()
		{
			Sprite Icon_Regeneration = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_REGENERATION.png");
			SpecialPowerSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "RegenerationFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Regeneration");
				bp.SetDescription(Main.IsekaiContext, "You regain 10 hit points per round and cannot die while regeneration is functioning. Regeneration is disabled for 1 round when you are hit with an acid or fire attack. ");
				((BlueprintUnitFact)bp).m_Icon = Icon_Regeneration;
				bp.AddComponent(delegate(AddEffectRegeneration c)
				{
					c.Heal = 10;
					c.Unremovable = false;
					c.CancelByMagicWeapon = false;
					c.CancelDamageEnergyTypes = new DamageEnergyType[2]
					{
						DamageEnergyType.Acid,
						DamageEnergyType.Fire
					};
					c.CancelDamageAlignmentTypes = new DamageAlignment[0];
					c.CancelDamageMaterials = new PhysicalDamageMaterial[0];
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 5;
				});
			}));
		}
	}
}
