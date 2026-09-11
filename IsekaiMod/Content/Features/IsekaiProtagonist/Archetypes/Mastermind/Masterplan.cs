using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.Archetypes.Mastermind
{
	internal class Masterplan
	{
		private const string Name = "Masterplan";

		private static readonly Sprite Icon_Masterplan = AssetLoader.LoadInternal(Main.IsekaiContext, "Features", "ICON_MASTERPLAN.png");

		public static void Add()
		{
			BlueprintBuff MasterplanBuff = TTCoreExtensions.CreateBuff("MasterplanBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Masterplan");
				bp.SetDescription(Main.IsekaiContext, "This character cannot cast spells or use magic items.");
				bp.IsClassFeature = true;
				((BlueprintUnitFact)bp).m_Icon = Icon_Masterplan;
				bp.AddComponent(delegate(ForbidSpellCasting c)
				{
					c.ForbidMagicItems = true;
				});
			});
			BlueprintAbilityAreaEffect MasterplanArea = Helpers.CreateBlueprint(Main.IsekaiContext, "MasterplanArea", delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Enemy;
				bp.SpellResistance = false;
				bp.AggroEnemies = false;
				bp.AffectEnemies = true;
				bp.Shape = AreaEffectShape.Cylinder;
				bp.Size = new Feet(120f);
				bp.Fx = new PrefabLink();
				bp.AddUnconditionalAuraEffect(MasterplanBuff.ToReference<BlueprintBuffReference>());
			});
			BlueprintBuff MasterplanAreaBuff = TTCoreExtensions.CreateBuff("MasterplanAreaBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, "Masterplan");
				bp.SetDescription(Main.IsekaiContext, "Enemies within 120 feet cannot cast spells or use magic items.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Masterplan;
				bp.IsClassFeature = true;
				bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
				bp.AddComponent(delegate(AddAreaEffect c)
				{
					c.m_AreaEffect = MasterplanArea.ToReference<BlueprintAbilityAreaEffectReference>();
				});
			});
			Helpers.CreateBlueprint(Main.IsekaiContext, "MasterplanFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Masterplan");
				bp.SetDescription(Main.IsekaiContext, "Your spells ignore spell resistance and spell immunity. Enemies within 120 feet cannot cast spells or use magic items.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Masterplan;
				bp.Ranks = 1;
				bp.AddComponent(delegate(IgnoreSpellImmunity c)
				{
					c.SpellDescriptor = SpellDescriptor.None;
				});
				bp.AddComponent(delegate(IgnoreSpellResistanceForSpells c)
				{
					c.m_AbilityList = new BlueprintAbilityReference[0];
					c.AllSpells = true;
				});
				bp.AddComponent(delegate(AuraFeatureComponent c)
				{
					c.m_Buff = MasterplanAreaBuff.ToReference<BlueprintBuffReference>();
				});
			});
		}
	}
}
