using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.GodEmperor
{
	internal class Godhood
	{
		public static void Add()
		{
			Sprite Icon_Godhood = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_GODHOOD.png");
			Helpers.CreateBlueprint(Main.IsekaiContext, "Godhood", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Godhood");
				bp.SetDescription(Main.IsekaiContext, "At 20th level, you surpass the pinnacle of this physical reality. You are immune to all spells and {g|Encyclopedia:Physical_Damage}physical damage{/g}. Your attacks ignore concealment and damage reduction. Your spells ignore spell resistance and spell immunity.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Godhood;
				bp.AddComponent<AddSpellImmunity>();
				bp.AddComponent(delegate(AreaEffectImmunity c)
				{
					c.m_CasterType = TargetType.Enemy;
					c.m_SpecificAreaEffects = false;
				});
				bp.AddComponent<AddPhysicalImmunity>();
				bp.AddComponent<IgnoreConcealment>();
				bp.AddComponent<IgnoreDamageReductionOnAttack>();
				bp.AddComponent(delegate(IgnoreSpellImmunity c)
				{
					c.SpellDescriptor = SpellDescriptor.None;
				});
				bp.AddComponent(delegate(IgnoreSpellResistanceForSpells c)
				{
					c.m_AbilityList = new BlueprintAbilityReference[0];
					c.AllSpells = true;
				});
			});
		}
	}
}
