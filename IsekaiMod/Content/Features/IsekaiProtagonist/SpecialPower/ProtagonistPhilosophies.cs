using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal static class ProtagonistPhilosophies
	{
		private static readonly Sprite Icon_Hero = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("e45ab30f49215054e83b4ea12165409f"))?.m_Icon;

		private static readonly Sprite Icon_Dark = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("9eda82a1f78558747a03c17e0e9a1a68"))?.m_Icon ?? Icon_Hero;

		private static readonly Sprite Icon_System = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("faf473e3a977fd4428cd3f1a526346d2"))?.m_Icon ?? Icon_Hero;

		private static readonly Sprite Icon_Trickster = ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintBuff>("03464790f40c3c24aa684b57155f3280"))?.m_Icon ?? Icon_Hero;

		public static BlueprintFeature PhilosophyShonenIdealistFeature;

		public static BlueprintFeature PhilosophyRuthlessPragmatistFeature;

		public static BlueprintFeature PhilosophySystemsExploiterFeature;

		public static BlueprintFeature PhilosophyChaoticWildcardFeature;

		public static void Add()
		{
			PhilosophyShonenIdealistFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "PhilosophyShonenIdealistFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Protagonist Philosophy: Shonen Idealist");
				bp.SetDescription(Main.IsekaiContext, "You believe passionately in boundless growth, second chances, and the power of fellowship. Whenever your party engages in combat, you inspire unshakeable grit: you and all allies within 30 feet gain a +3 morale bonus to attack rolls, weapon damage, and saving throws, and Fast Healing 3.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Hero;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.AdditionalDamage;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveFortitude;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveReflex;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = StatType.SaveWill;
					c.Value = 3;
				});
				bp.AddComponent(delegate(AddEffectFastHealing c)
				{
					c.Heal = 3;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 9;
				});
			});
			SpecialPowerSelection.AddToAuthoritySelection(PhilosophyShonenIdealistFeature);
			PhilosophyRuthlessPragmatistFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "PhilosophyRuthlessPragmatistFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Protagonist Philosophy: Ruthless Pragmatist");
				bp.SetDescription(Main.IsekaiContext, "Morality is a luxury for the victorious. You exploit every tactical opening without hesitation: you deal an additional +2d6 Sneak Attack damage, gain a +2 bonus on critical confirmation rolls, and your attacks ignore concealment.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Dark;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Insight;
					c.Stat = StatType.SneakAttack;
					c.Value = 2;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Competence;
					c.Stat = StatType.AdditionalAttackBonus;
					c.Value = 2;
				});
				bp.AddComponent<IgnoreConcealment>();
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 9;
				});
			});
			SpecialPowerSelection.AddToAuthoritySelection(PhilosophyRuthlessPragmatistFeature);
			PhilosophySystemsExploiterFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "PhilosophySystemsExploiterFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Protagonist Philosophy: Systems Exploiter");
				bp.SetDescription(Main.IsekaiContext, "You view Golarion's planar mechanics as an exploitable game system. You gain a +2 bonus to the DC of all spells and abilities, a +4 bonus on caster level checks to overcome spell resistance, and +1 extra attack per round during full attacks.");
				((BlueprintUnitFact)bp).m_Icon = Icon_System;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(IncreaseAllSpellsDC c)
				{
					c.Value = 2;
					c.Descriptor = ModifierDescriptor.UntypedStackable;
				});
				bp.AddComponent(delegate(SpellPenetrationBonus c)
				{
					c.Value = 4;
				});
				bp.AddComponent(delegate(BuffExtraAttack c)
				{
					c.Number = 1;
					c.Haste = true;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 9;
				});
			});
			SpecialPowerSelection.AddToAuthoritySelection(PhilosophySystemsExploiterFeature);
			PhilosophyChaoticWildcardFeature = Helpers.CreateBlueprint(Main.IsekaiContext, "PhilosophyChaoticWildcardFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Protagonist Philosophy: Chaotic Wildcard");
				bp.SetDescription(Main.IsekaiContext, "Your actions defy logic, probability, and narrative structure. You gain a permanent +4 luck bonus to Armor Class, a +4 luck bonus to all saving throws, and a 20% miss chance against all incoming attacks as probability ripples around you.");
				((BlueprintUnitFact)bp).m_Icon = Icon_Trickster;
				bp.IsClassFeature = true;
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.AC;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveFortitude;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveReflex;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Luck;
					c.Stat = StatType.SaveWill;
					c.Value = 4;
				});
				bp.AddComponent(delegate(AddConcealment c)
				{
					c.Concealment = Concealment.Partial;
					c.Descriptor = ConcealmentDescriptor.Blur;
				});
				bp.AddComponent(delegate(PrerequisiteCharacterLevel c)
				{
					c.Level = 9;
				});
			});
			SpecialPowerSelection.AddToAuthoritySelection(PhilosophyChaoticWildcardFeature);
			Action<BlueprintFeature, BlueprintFeature> action = delegate(BlueprintFeature target, BlueprintFeature excluded)
			{
				target.AddComponent(delegate(PrerequisiteNoFeature c)
				{
					c.m_Feature = excluded.ToReference<BlueprintFeatureReference>();
					c.Group = Prerequisite.GroupType.All;
				});
			};
			action(PhilosophyShonenIdealistFeature, PhilosophyRuthlessPragmatistFeature);
			action(PhilosophyShonenIdealistFeature, PhilosophySystemsExploiterFeature);
			action(PhilosophyShonenIdealistFeature, PhilosophyChaoticWildcardFeature);
			action(PhilosophyRuthlessPragmatistFeature, PhilosophyShonenIdealistFeature);
			action(PhilosophyRuthlessPragmatistFeature, PhilosophySystemsExploiterFeature);
			action(PhilosophyRuthlessPragmatistFeature, PhilosophyChaoticWildcardFeature);
			action(PhilosophySystemsExploiterFeature, PhilosophyShonenIdealistFeature);
			action(PhilosophySystemsExploiterFeature, PhilosophyRuthlessPragmatistFeature);
			action(PhilosophySystemsExploiterFeature, PhilosophyChaoticWildcardFeature);
			action(PhilosophyChaoticWildcardFeature, PhilosophyShonenIdealistFeature);
			action(PhilosophyChaoticWildcardFeature, PhilosophyRuthlessPragmatistFeature);
			action(PhilosophyChaoticWildcardFeature, PhilosophySystemsExploiterFeature);
		}
	}
}
