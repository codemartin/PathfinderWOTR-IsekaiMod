using System;
using System.Linq;
using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Class.Kineticist;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using Owlcat.QA.Validation;
using UnityEngine;

namespace IsekaiMod.Components
{
	[TypeId("bd8d467121614c95a058bf66b6dcd1fa")]
	internal class ContextCalculateAbilityParamsBasedOnClasses : ContextAbilityParamsCalculator
	{
		public bool UseKineticistMainStat;

		[HideIf("UseKineticistMainStat")]
		public StatType StatType = StatType.Charisma;

		[SerializeField]
		public BlueprintCharacterClassReference[] m_CharacterClasses;

		public BlueprintCharacterClass[] CharacterClasses
		{
			get
			{
				if (m_CharacterClasses == null)
				{
					return null;
				}
				return m_CharacterClasses.Select((BlueprintCharacterClassReference bp) => bp.Get()).ToArray();
			}
		}

		public int GetMaxClassLevel([NotNull] UnitEntityData caster)
		{
			int num = 0;
			if (CharacterClasses == null || caster?.Descriptor?.Progression == null)
			{
				return 0;
			}
			BlueprintCharacterClass[] characterClasses = CharacterClasses;
			foreach (BlueprintCharacterClass blueprintCharacterClass in characterClasses)
			{
				if (blueprintCharacterClass != null)
				{
					num = Math.Max(num, caster.Descriptor.Progression.GetClassLevel(blueprintCharacterClass));
				}
			}
			return num;
		}

		public override AbilityParams Calculate(MechanicsContext context)
		{
			if (context == null)
			{
				return null;
			}
			UnitEntityData maybeCaster = context.MaybeCaster;
			if (maybeCaster == null)
			{
				PFLog.Default.Error(this, "Caster is missing");
				return context.Params;
			}
			BlueprintScriptableObject associatedBlueprint = context.AssociatedBlueprint;
			UnitEntityData caster = maybeCaster;
			return Calculate(context, associatedBlueprint, caster, context.SourceAbilityContext?.Ability);
		}

		public AbilityParams Calculate([NotNull] AbilityData ability)
		{
			if (ability == null)
			{
				return null;
			}
			return Calculate(null, ability.Blueprint, ability.Caster, ability);
		}

		public AbilityParams Calculate([CanBeNull] MechanicsContext context, [NotNull] BlueprintScriptableObject blueprint, [NotNull] UnitEntityData caster, [CanBeNull] AbilityData ability)
		{
			if (caster == null)
			{
				return context?.Params;
			}
			StatType value = StatType;
			if (UseKineticistMainStat)
			{
				UnitPartKineticist unitPartKineticist = caster?.Get<UnitPartKineticist>();
				if (unitPartKineticist == null)
				{
					PFLog.Default.Error(blueprint, $"Caster is not kineticist: {caster} ({blueprint.NameSafe()})");
				}
				value = unitPartKineticist?.MainStatType ?? StatType;
			}
			RuleCalculateAbilityParams ruleCalculateAbilityParams = ((ability != null) ? new RuleCalculateAbilityParams(caster, ability) : new RuleCalculateAbilityParams(caster, blueprint, null));
			ruleCalculateAbilityParams.ReplaceStat = value;
			int maxClassLevel = GetMaxClassLevel(caster);
			ruleCalculateAbilityParams.ReplaceCasterLevel = maxClassLevel;
			ruleCalculateAbilityParams.ReplaceSpellLevel = maxClassLevel / 2;
			if (context != null)
			{
				return context.TriggerRule(ruleCalculateAbilityParams).Result;
			}
			return Rulebook.Trigger(ruleCalculateAbilityParams).Result;
		}

		public override void ApplyValidation(ValidationContext context, int parentIndex)
		{
			base.ApplyValidation(context, parentIndex);
			if (!StatType.IsAttribute() && StatType != StatType.BaseAttackBonus)
			{
				string errorFormat = string.Join(", ", StatTypeHelper.Attributes.Select(delegate(StatType s)
				{
					StatType statType = s;
					return statType.ToString();
				}));
				context.AddError(ErrorLevel.Unprioritized, "StatType must be Base Attack Bonus or an attribute: {0}", errorFormat);
			}
		}
	}
}
