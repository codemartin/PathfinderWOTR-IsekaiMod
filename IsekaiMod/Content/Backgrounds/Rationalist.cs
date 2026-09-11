using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds
{
	internal class Rationalist
	{
		public static void Add()
		{
			IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundRationalist", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Rationalist");
				bp.SetBackgroundDescription(Main.IsekaiContext, "The Rationalist is immune to any {g|Encyclopedia:Spell}spell{/g} or {g|Encyclopedia:Special_Abilities}spell-like ability{/g} that allows {g|Encyclopedia:Spell_Resistance}spell resistance{/g} but cannot cast any spells.");
				bp.AddComponent<AddSpellImmunity>();
				bp.AddComponent(delegate(AreaEffectImmunity c)
				{
					c.m_CasterType = TargetType.Enemy;
					c.m_SpecificAreaEffects = false;
				});
				bp.AddComponent(delegate(AddCondition c)
				{
					c.Condition = UnitCondition.SpellcastingForbidden;
				});
			}));
		}
	}
}
