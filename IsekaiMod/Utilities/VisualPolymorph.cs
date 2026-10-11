using System.Reflection;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities;

internal static class VisualPolymorph
{
	internal const string NaturalSpellGuid = "c806103e27cce6f429e5bf47067966cf";

	private static readonly string[] VisualFields = new string[7] { "m_Prefab", "m_PrefabFemale", "m_SpecialDollType", "m_EnterTransition", "m_ExitTransition", "m_TransitionExternal", "m_SilentCaster" };

	internal static bool SetModel(BlueprintBuff buff, BlueprintUnit modelSource)
	{
		Polymorph polymorph = buff?.GetComponent<Polymorph>();
		UnitViewLink unitViewLink = modelSource?.Prefab;
		if (polymorph == null || unitViewLink == null || string.IsNullOrEmpty(unitViewLink.AssetId))
		{
			return false;
		}
		Set(polymorph, "m_Prefab", unitViewLink);
		Set(polymorph, "m_PrefabFemale", unitViewLink);
		return true;
	}

	internal static bool SetFemaleModel(BlueprintBuff buff, UnitViewLink female)
	{
		Polymorph polymorph = buff?.GetComponent<Polymorph>();
		if (polymorph == null || female == null || string.IsNullOrEmpty(female.AssetId))
		{
			return false;
		}
		Set(polymorph, "m_PrefabFemale", female);
		return true;
	}

	private static void Set(Polymorph target, string field, object value)
	{
		AccessTools.Field(typeof(Polymorph), field)?.SetValue(target, value);
	}

	internal static BlueprintBuff CreateBodyBuff(string name, BlueprintBuff source)
	{
		Polymorph sourcePolymorph = source?.GetComponent<Polymorph>();
		if (sourcePolymorph == null)
		{
			return null;
		}
		return Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintBuff bp)
		{
			bp.SetName(StaticReferences.Strings.Null);
			bp.SetDescription(StaticReferences.Strings.Null);
			bp.IsClassFeature = true;
			bp.m_Flags = BlueprintBuff.Flags.HiddenInUi | BlueprintBuff.Flags.StayOnDeath;
			bp.AddComponent(delegate(Polymorph c)
			{
				string[] visualFields = VisualFields;
				foreach (string name2 in visualFields)
				{
					FieldInfo fieldInfo = AccessTools.Field(typeof(Polymorph), name2);
					fieldInfo?.SetValue(c, fieldInfo.GetValue(sourcePolymorph));
				}
				c.Size = sourcePolymorph.Size;
				c.UseSizeAsBaseForDamage = false;
				c.StrengthBonus = 0;
				c.DexterityBonus = 0;
				c.ConstitutionBonus = 0;
				c.NaturalArmor = 0;
				c.AllowDamageTransfer = false;
				Set(c, "m_KeepSlots", true);
				Set(c, "m_Race", null);
				Set(c, "m_Portrait", null);
				Set(c, "m_ReplaceUnitForInspection", null);
				Set(c, "m_MainHand", null);
				Set(c, "m_OffHand", null);
				Set(c, "m_Facts", new BlueprintUnitFactReference[0]);
				Set(c, "m_AdditionalLimbs", new BlueprintItemWeaponReference[0]);
				Set(c, "m_SecondaryAdditionalLimbs", new BlueprintItemWeaponReference[0]);
			});
			BlueprintFeature naturalSpell = BlueprintTools.GetBlueprint<BlueprintFeature>("c806103e27cce6f429e5bf47067966cf");
			if (naturalSpell != null)
			{
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { naturalSpell.ToReference<BlueprintUnitFactReference>() };
				});
			}
		});
	}
}
