using IsekaiMod.Components;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Content.Features.IsekaiProtagonist.OverpoweredAbility;

internal static class PortedImmunities
{
	internal const SpellDescriptor FearDescriptors = SpellDescriptor.Fear | SpellDescriptor.Shaken | SpellDescriptor.Frightened;

	internal static void AddFearImmunity(BlueprintScriptableObject bp)
	{
		bp.AddComponent(delegate(AddConditionImmunity c)
		{
			c.Condition = UnitCondition.Shaken;
		});
		bp.AddComponent(delegate(AddConditionImmunity c)
		{
			c.Condition = UnitCondition.Frightened;
		});
		AddHostileImmunity(bp, SpellDescriptor.Fear | SpellDescriptor.Shaken | SpellDescriptor.Frightened);
	}

	internal static void AddMindAffectingImmunity(BlueprintScriptableObject bp, SpellDescriptor extra = SpellDescriptor.None)
	{
		AddHostileImmunity(bp, MindAffectingSet(extra));
	}

	internal static SpellDescriptor MindAffectingSet(SpellDescriptor extra)
	{
		return SpellDescriptor.MindAffecting | SpellDescriptor.Fear | SpellDescriptor.Compulsion | SpellDescriptor.Charm | SpellDescriptor.Daze | SpellDescriptor.Shaken | SpellDescriptor.Frightened | SpellDescriptor.Confusion | SpellDescriptor.Sleep | SpellDescriptor.NegativeEmotion | extra;
	}

	internal static SpellDescriptor SpellLevel(SpellDescriptor descriptor)
	{
		return descriptor & ~(SpellDescriptor.MindAffecting | SpellDescriptor.Emotion);
	}

	internal static void AddHostileImmunity(BlueprintScriptableObject bp, SpellDescriptor descriptor)
	{
		bp.AddComponent(delegate(HostileBuffDescriptorImmunity c)
		{
			c.Descriptor = descriptor;
		});
		SpellDescriptor spellLevel = SpellLevel(descriptor);
		if (spellLevel != SpellDescriptor.None)
		{
			bp.AddComponent(delegate(SpellImmunityToSpellDescriptor c)
			{
				c.Descriptor = spellLevel;
			});
		}
	}
}
