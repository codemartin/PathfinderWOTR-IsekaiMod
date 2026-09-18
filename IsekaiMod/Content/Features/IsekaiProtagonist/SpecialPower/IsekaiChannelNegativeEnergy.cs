using IsekaiMod.Content.Classes.IsekaiProtagonist;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.SpecialPower
{
	internal class IsekaiChannelNegativeEnergy
	{
		private static readonly BlueprintFeature ExtraChannel = BlueprintTools.GetBlueprint<BlueprintFeature>("cd9f19775bd9d3343a31a065e93f0c47");

		private static readonly BlueprintFeature SelectiveChannel = BlueprintTools.GetBlueprint<BlueprintFeature>("fd30c69417b434d47b6b03b9c1f568ff");

		private static readonly BlueprintAbility ChannelNegativeEnergy = BlueprintTools.GetBlueprint<BlueprintAbility>("89df18039ef22174b81052e2e419c728");

		private static readonly BlueprintAbility ChannelNegativeHeal = BlueprintTools.GetBlueprint<BlueprintAbility>("9be3aa47a13d5654cbcb8dbd40c325f2");

		private static readonly BlueprintUnitFact ChannelEnergyFact = BlueprintTools.GetBlueprint<BlueprintUnitFact>("93f062bc0bf70e84ebae436e325e30e8");

		public static void Add()
		{
			BlueprintAbility HarmAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiChannelNegativeEnergyAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Channel Negative Energy - Damage Living");
				bp.SetDescription(Main.IsekaiContext, "Channeling negative energy causes a burst that damages every living creature in a 30-foot radius centered on the caster. The amount of damage inflicted is equal to 1d6 points of damage plus 1d6 points of damage for every two Isekai Protagonist levels beyond 1st (2d6 at 3rd, 3d6 at 5th, and so on). Creatures that take damage from channeled energy receive a Will save to halve the damage. The DC of this save is equal to 10 + 1/2 the Isekai Protagonist level + Charisma modifier.");
				((BlueprintUnitFact)bp).m_Icon = ChannelNegativeEnergy.Icon;
				bp.LocalizedDuration = StaticReferences.Strings.Null;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
				bp.AvailableMetamagic = ChannelNegativeEnergy.AvailableMetamagic;
				bp.Range = AbilityRange.Personal;
				bp.Type = AbilityType.Special;
				bp.CanTargetEnemies = true;
				bp.CanTargetFriends = true;
				bp.EffectOnAlly = AbilityEffectOnUnit.Harmful;
				bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.ResourceAssetIds = ChannelNegativeEnergy.ResourceAssetIds;
				((BlueprintScriptableObject)bp).Components = new BlueprintComponent[0];
			});
			BlueprintAbility HealAbility = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiChannelNegativeHealAbility", delegate(BlueprintAbility bp)
			{
				bp.SetName(Main.IsekaiContext, "Channel Negative Energy - Heal Undead");
				bp.SetDescription(Main.IsekaiContext, "Channeling negative energy causes a burst that heals every undead creature in a 30-foot radius centered on the caster. The amount of damage healed is equal to 1d6 points of damage plus 1d6 points of damage for every two Isekai Protagonist levels beyond 1st (2d6 at 3rd, 3d6 at 5th, and so on).");
				((BlueprintUnitFact)bp).m_Icon = ChannelNegativeHeal.Icon;
				bp.LocalizedDuration = StaticReferences.Strings.Null;
				bp.LocalizedSavingThrow = StaticReferences.Strings.Null;
				bp.AvailableMetamagic = ChannelNegativeHeal.AvailableMetamagic;
				bp.Range = AbilityRange.Personal;
				bp.Type = AbilityType.Special;
				bp.CanTargetEnemies = true;
				bp.CanTargetFriends = true;
				bp.EffectOnAlly = AbilityEffectOnUnit.Helpful;
				bp.EffectOnEnemy = AbilityEffectOnUnit.Helpful;
				bp.Animation = UnitAnimationActionCastSpell.CastAnimationStyle.Omni;
				bp.ActionType = UnitCommand.CommandType.Standard;
				bp.ResourceAssetIds = ChannelNegativeHeal.ResourceAssetIds;
				((BlueprintScriptableObject)bp).Components = new BlueprintComponent[0];
			});
			AddChannelEnergyPatchedComponents(HarmAbility, ((BlueprintScriptableObject)ChannelNegativeEnergy).Components);
			AddChannelEnergyPatchedComponents(HealAbility, ((BlueprintScriptableObject)ChannelNegativeHeal).Components);
			BlueprintFeature feature = Helpers.CreateBlueprint(Main.IsekaiContext, "IsekaiChannelNegativeEnergyFeature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, "Channel Negative Energy");
				bp.SetDescription(Main.IsekaiContext, "You gain the supernatural ability to channel negative energy like a cleric. You use your Isekai Protagonist level as your effective cleric level when channeling negative energy. You can channel energy a number of times per day equal to 3 + your Charisma modifier.");
				((BlueprintUnitFact)bp).m_Icon = ChannelNegativeEnergy.Icon;
				bp.Groups = new FeatureGroup[0];
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[3]
					{
						ChannelEnergyFact.ToReference<BlueprintUnitFactReference>(),
						HarmAbility.ToReference<BlueprintUnitFactReference>(),
						HealAbility.ToReference<BlueprintUnitFactReference>()
					};
				});
			});
			SelectiveChannel.AddPrerequisiteFeature(feature, Prerequisite.GroupType.Any);
			ExtraChannel.AddPrerequisiteFeature(feature, Prerequisite.GroupType.Any);
			SpecialPowerSelection.AddToAuthoritySelection(feature);
		}

		private static void AddChannelEnergyPatchedComponents(BlueprintAbility ability, BlueprintComponent[] components)
		{
			foreach (BlueprintComponent blueprintComponent in components)
			{
				if (blueprintComponent is ContextRankConfig contextRankConfig && (contextRankConfig.m_BaseValueType == ContextRankBaseValueType.ClassLevel || contextRankConfig.m_BaseValueType == ContextRankBaseValueType.SummClassLevelWithArchetype))
				{
					ability.AddComponent(delegate(ContextRankConfig c)
					{
						c.m_Type = AbilityRankType.Default;
						c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
						c.m_Progression = ContextRankProgression.OnePlusDiv2;
						c.m_Class = new BlueprintCharacterClassReference[1] { IsekaiProtagonistClass.GetReference() };
					});
				}
				else
				{
					ability.AddComponent(blueprintComponent);
				}
			}
		}
	}
}
