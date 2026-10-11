using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Backgrounds;

internal class Rationalist
{
	public static void Add()
	{
		IsekaiBackgroundSelection.AddToSelection(Helpers.CreateBlueprint(Main.IsekaiContext, "BackgroundRationalist", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Rationalist");
			bp.SetBackgroundDescription(Main.IsekaiContext, "Convinced that magic is nothing but a trick of the mind, the Rationalist simply refuses to believe in it. Unbeliever: she is immune to any {g|Encyclopedia:Spell}spell{/g} or {g|Encyclopedia:Special_Abilities}spell-like ability{/g} that allows {g|Encyclopedia:Spell_Resistance}spell resistance{/g}, and to spell areas created by enemies. This works on friendly magic too: heals and buffs from her allies that allow spell resistance fail on her. She refuses magic entirely: she cannot cast {g|Encyclopedia:Spell}spells{/g}, use {g|Encyclopedia:Special_Abilities}spell-like abilities{/g} (including her own class abilities such as Instakill, Grasp Heart, Create Death Knight and Supreme Pre-Buff Routine). Supernatural abilities such as Super Buff are untouched, and scrolls and wands still work for her unless using one would call for a {g|Encyclopedia:Use_Magic_Device}Use Magic Device{/g} check.");
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
