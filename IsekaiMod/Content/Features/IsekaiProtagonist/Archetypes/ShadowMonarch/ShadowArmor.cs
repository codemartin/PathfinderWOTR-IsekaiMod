using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.ShadowMonarch
{
	internal class ShadowArmor
	{
		private static readonly Sprite Icon_ShadowArmor = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintAbility>("ad5ed5ea4ec52334a94e975a64dad336"))?.m_Icon;

		public static void Add()
		{
			Helpers.CreateBlueprint(Main.IsekaiContext, "ShadowArmorFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Monarch's Shadow Cloak");
				bp.SetDescription(Main.IsekaiContext, "At 7th level, living shadows swirl around the Monarch, deflecting blade strikes and confusing enemy vision. Grants Damage Reduction 10/Silver and a permanent 20% concealment miss chance against physical attacks.");
				((BlueprintUnitFact)bp).m_Icon = Icon_ShadowArmor;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddDamageResistancePhysical c)
				{
					c.Value = 10;
					c.BypassedByMaterial = true;
					c.Material = PhysicalDamageMaterial.Silver;
				});
				bp.AddComponent(delegate(AddConcealment c)
				{
					c.Descriptor = ConcealmentDescriptor.Displacement;
					c.Concealment = Concealment.Partial;
				});
			});
		}
	}
}
