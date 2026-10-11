using System;
using System.Collections.Generic;
using System.Linq;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Content.Guardians;

internal static class GuardianRoles
{
	private sealed class SaveSync : IAreaHandler, ISubscriber, IGlobalSubscriber
	{
		public void OnAreaDidLoad()
		{
			try
			{
				EntityPool<UnitEntityData> entityPool = Game.Instance?.State?.Units;
				if (entityPool == null)
				{
					return;
				}
				foreach (UnitEntityData item in entityPool.ToList())
				{
					Sync(item);
				}
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("GuardianRoles.SaveSync: " + ex);
			}
		}

		public void OnAreaBeginUnloading()
		{
		}
	}

	private static bool Added = false;

	internal const string SlotFeatureName = "GuardianSlotFeature";

	internal static readonly LockEquipmentSlot.SlotType[] NaturalHandSlots = new LockEquipmentSlot.SlotType[8]
	{
		LockEquipmentSlot.SlotType.Weapon1,
		LockEquipmentSlot.SlotType.Weapon2,
		LockEquipmentSlot.SlotType.Weapon3,
		LockEquipmentSlot.SlotType.Weapon4,
		LockEquipmentSlot.SlotType.Weapon5,
		LockEquipmentSlot.SlotType.Weapon6,
		LockEquipmentSlot.SlotType.Weapon7,
		LockEquipmentSlot.SlotType.Weapon8
	};

	internal static readonly int[] BonusFeatLevels = new int[6] { 1, 4, 8, 12, 16, 20 };

	internal static readonly GuardianProfile[] Profiles = new GuardianProfile[13]
	{
		new GuardianProfile
		{
			UnitName = "GuardianAngelCompanionUnit",
			TemplateGuid = "8f3bd0ecea704277a9f2b09296a7b01e",
			Rig = GuardianRig.Monster,
			Role = GuardianRole.Tank
		},
		new GuardianProfile
		{
			UnitName = "ShinigamiCompanionUnit",
			TemplateGuid = "bc6b0477fba49e54e84e2c7ba3cf2144",
			Rig = GuardianRig.Monster,
			Role = GuardianRole.Striker
		},
		new GuardianProfile
		{
			UnitName = "LoyalDemonCompanionUnit",
			TemplateGuid = "a1fe1265b9baad340b6ba02ab4d95a87",
			FallbackTemplateGuid = "2db556136eac2544fa9744314c2a5713",
			Rig = GuardianRig.Humanoid,
			Role = GuardianRole.Striker
		},
		new GuardianProfile
		{
			UnitName = "AstralDevourerCompanionUnit",
			TemplateGuid = "eedec6f78f5a4deb9b58666627faf9de",
			FallbackTemplateGuid = "bc6b0477fba49e54e84e2c7ba3cf2144",
			Rig = GuardianRig.Monster,
			Role = GuardianRole.Tank
		},
		new GuardianProfile
		{
			UnitName = "HavocDragonCompanionUnit",
			TemplateGuid = "4f2ed74aea457fb409ba56872dbc5552",
			Rig = GuardianRig.Monster,
			Role = GuardianRole.Controller
		},
		new GuardianProfile
		{
			UnitName = "TempestStarWolfCompanionUnit",
			TemplateGuid = "eab864d9ca3415644a792792fd81bf87",
			Rig = GuardianRig.Monster,
			Role = GuardianRole.Controller
		},
		new GuardianProfile
		{
			UnitName = "ShadowMarshallCompanionUnit",
			TemplateGuid = "7121303d0f344a5abb3b43b0c9cef8e4",
			FallbackTemplateGuid = "7237a32613fe55e479d1141682f2bbd4",
			Rig = GuardianRig.Monster,
			Role = GuardianRole.Tank
		},
		new GuardianProfile
		{
			UnitName = "OverlordGuardianCompanionUnit",
			TemplateGuid = "b576f3eb0aa94af44a985f51eda9db7b",
			FallbackTemplateGuid = "2db556136eac2544fa9744314c2a5713",
			Rig = GuardianRig.Humanoid,
			Role = GuardianRole.Tank
		},
		new GuardianProfile
		{
			UnitName = "DivineHeraldCompanionUnit",
			TemplateGuid = "8f3bd0ecea704277a9f2b09296a7b01e",
			Rig = GuardianRig.Monster,
			Role = GuardianRole.Support
		},
		new GuardianProfile
		{
			UnitName = "ChronoSpriteCompanionUnit",
			TemplateGuid = "09fa46bc6d1cb774fb6dfefada332b9f",
			Rig = GuardianRig.Monster,
			Role = GuardianRole.Support
		},
		new GuardianProfile
		{
			UnitName = "EnigmaticCoConspiratorCompanionUnit",
			TemplateGuid = "030bffa7543f0a04ba7d60d0558a8d36",
			FallbackTemplateGuid = "b576f3eb0aa94af44a985f51eda9db7b",
			Rig = GuardianRig.Humanoid,
			Role = GuardianRole.Controller
		},
		new GuardianProfile
		{
			UnitName = "ManifestedMartialSpiritCompanionUnit",
			TemplateGuid = "b95b40741207418294a72bdfc409c9c1",
			FallbackTemplateGuid = "bc6b0477fba49e54e84e2c7ba3cf2144",
			Rig = GuardianRig.Monster,
			Role = GuardianRole.Striker
		},
		new GuardianProfile
		{
			UnitName = "TwinAvatarCompanionUnit",
			TemplateGuid = "00f6e955cf970cb449958bd95d3ad2b7",
			FallbackTemplateGuid = "b3bc5994e94534d4c86613e649fe1913",
			Rig = GuardianRig.Humanoid,
			Role = GuardianRole.Striker
		}
	};

	private static readonly Dictionary<BlueprintGuid, GuardianProfile> s_ProfileByUnit = new Dictionary<BlueprintGuid, GuardianProfile>();

	private static BlueprintFeature s_SlotFeature;

	private static BlueprintFeature s_MythicBond;

	private static BlueprintFeature LightArmorProficiency => BlueprintTools.GetBlueprint<BlueprintFeature>("6d3728d4e9c9898458fe5e9532951132");

	private static BlueprintFeature MediumArmorProficiency => BlueprintTools.GetBlueprint<BlueprintFeature>("46f4fb320f35704488ba3d513397789d");

	private static BlueprintFeature HeavyArmorProficiency => BlueprintTools.GetBlueprint<BlueprintFeature>("1b0f68188dcc435429fb87a022239681");

	private static Sprite Icon_Tank => ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("1b0f68188dcc435429fb87a022239681"))?.m_Icon;

	private static Sprite Icon_Striker => ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("9972f33f977fc724c838e59641b2fca5"))?.m_Icon;

	private static Sprite Icon_Controller => ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("0f15c6f70d8fb2b49aa6cc24239cc5fa"))?.m_Icon;

	private static Sprite Icon_Support => ((BlueprintUnitFact)BlueprintTools.GetBlueprint<BlueprintFeature>("3a6db57fce75b0244a6a5819528ddf26"))?.m_Icon;

	internal static bool HoldsNaturalWeaponInHand(bool handsDisabled, int additionalLimbs, bool anyNaturalHandWeapon)
	{
		if (!handsDisabled)
		{
			return anyNaturalHandWeapon;
		}
		return additionalLimbs > 0;
	}

	private static bool HoldsNaturalWeaponInHand(BlueprintUnit unit)
	{
		BlueprintUnit.UnitBody unitBody = unit?.Body;
		if (unitBody == null)
		{
			return false;
		}
		bool flag = false;
		for (int i = 0; i < 4; i++)
		{
			if (flag)
			{
				break;
			}
			BlueprintItemWeapon obj = unitBody.GetHandEqupment(i, main: true) as BlueprintItemWeapon;
			flag = (obj != null && obj.IsNatural) || ((unitBody.GetHandEqupment(i, main: false) as BlueprintItemWeapon)?.IsNatural ?? false);
		}
		bool disableHands = unitBody.DisableHands;
		BlueprintItemWeaponReference[] additionalLimbs = unitBody.m_AdditionalLimbs;
		return HoldsNaturalWeaponInHand(disableHands, (additionalLimbs != null) ? additionalLimbs.Length : 0, flag);
	}

	internal static bool LocksGearSlots(GuardianRig rig)
	{
		return rig == GuardianRig.Monster;
	}

	internal static string RoleFeatureName(GuardianRole role)
	{
		return "GuardianRole" + role.ToString() + "Feature";
	}

	internal static int RoleBonus(int guardianLevel)
	{
		return 1 + Math.Max(guardianLevel / 5, 0);
	}

	internal static int SupportAuraBonus(int guardianLevel)
	{
		return 1 + Math.Max(guardianLevel / 10, 0);
	}

	public static void Add()
	{
		if (Added)
		{
			return;
		}
		Added = true;
		BlueprintCharacterClassReference reference = GuardianCompanionClass.GetReference();
		Dictionary<GuardianRole, BlueprintFeature> dictionary = new Dictionary<GuardianRole, BlueprintFeature>
		{
			{
				GuardianRole.Tank,
				CreateTank(reference)
			},
			{
				GuardianRole.Striker,
				CreateStriker(reference)
			},
			{
				GuardianRole.Controller,
				CreateController(reference)
			},
			{
				GuardianRole.Support,
				CreateSupport(reference)
			}
		};
		s_SlotFeature = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GuardianSlotFeature");
		s_MythicBond = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GuardianMythicBondFeature");
		BlueprintUnitFactReference slotRef = s_SlotFeature?.ToReference<BlueprintUnitFactReference>();
		BlueprintUnitFactReference handSlotRef = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "GuardianNaturalHandSlotFeature")?.ToReference<BlueprintUnitFactReference>();
		GuardianProfile[] profiles = Profiles;
		foreach (GuardianProfile guardianProfile in profiles)
		{
			BlueprintUnit modBlueprint = BlueprintTools.GetModBlueprint<BlueprintUnit>(Main.IsekaiContext, guardianProfile.UnitName);
			if (modBlueprint == null)
			{
				Main.IsekaiContext.Logger.LogWarning("GuardianRoles: unit " + guardianProfile.UnitName + " not found, skipping.");
				continue;
			}
			s_ProfileByUnit[modBlueprint.AssetGuid] = guardianProfile;
			try
			{
				EyePortraitInjector.RegisterCropFallback(modBlueprint.PortraitSafe?.Data);
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("GuardianRoles: could not register " + guardianProfile.UnitName + "'s pet eye-strip: " + ex.Message);
			}
			List<BlueprintUnitFactReference> list = (modBlueprint.m_AddFacts ?? new BlueprintUnitFactReference[0]).Where((BlueprintUnitFactReference f) => f != null && (slotRef == null || !(f.Guid == slotRef.Guid))).ToList();
			if (LocksGearSlots(guardianProfile.Rig))
			{
				if (slotRef != null)
				{
					list.Insert(0, slotRef);
				}
			}
			else
			{
				BlueprintFeature[] array = new BlueprintFeature[3] { LightArmorProficiency, MediumArmorProficiency, HeavyArmorProficiency };
				foreach (BlueprintFeature blueprintFeature in array)
				{
					if (blueprintFeature != null)
					{
						BlueprintUnitFactReference profRef = blueprintFeature.ToReference<BlueprintUnitFactReference>();
						if (!list.Any((BlueprintUnitFactReference f) => f.Guid == profRef.Guid))
						{
							list.Add(profRef);
						}
					}
				}
			}
			if (handSlotRef != null)
			{
				list.RemoveAll((BlueprintUnitFactReference f) => f.Guid == handSlotRef.Guid);
				if (HoldsNaturalWeaponInHand(modBlueprint))
				{
					list.Insert(0, handSlotRef);
				}
			}
			list.Add(dictionary[guardianProfile.Role].ToReference<BlueprintUnitFactReference>());
			modBlueprint.m_AddFacts = list.ToArray();
		}
	}

	public static void Init()
	{
		EventBus.Subscribe(new SaveSync());
	}

	internal static void Sync(UnitEntityData unit)
	{
		if (unit?.Blueprint == null || !s_ProfileByUnit.TryGetValue(unit.Blueprint.AssetGuid, out var value))
		{
			return;
		}
		if (!LocksGearSlots(value.Rig) && s_SlotFeature != null && unit.HasFact(s_SlotFeature))
		{
			unit.RemoveFact(s_SlotFeature);
		}
		GuardianBodySync.Apply(unit);
		foreach (BlueprintUnitFact item in unit.Blueprint.AddFacts.EmptyIfNull())
		{
			if (item != null && item != s_SlotFeature && !unit.HasFact(item))
			{
				GuardianBodySync.ClearSlotsBeforeLock(unit, item);
				unit.AddFact(item);
			}
		}
		if (s_MythicBond != null && !unit.HasFact(s_MythicBond))
		{
			unit.AddFact(s_MythicBond);
		}
	}

	private static ContextRankConfig LevelRank(BlueprintCharacterClassReference guardianClass, int step)
	{
		return Helpers.Create(delegate(ContextRankConfig c)
		{
			c.m_Type = AbilityRankType.Default;
			c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
			c.m_Class = new BlueprintCharacterClassReference[1] { guardianClass };
			c.m_Progression = ContextRankProgression.OnePlusDivStep;
			c.m_StepLevel = step;
		});
	}

	private static BlueprintFeature CreateTank(BlueprintCharacterClassReference guardianClass)
	{
		return Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianRoleTankFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Guardian Role: Bulwark");
			bp.SetDescription(Main.IsekaiContext, "This guardian is a tank: it stands in front and takes the hits. It gains a dodge bonus to AC and a bonus to CMD of +1, plus 1 per five guardian levels (+5 at level 20).");
			((BlueprintUnitFact)bp).m_Icon = Icon_Tank;
			bp.IsClassFeature = true;
			bp.ReapplyOnLevelUp = true;
			bp.AddComponent(LevelRank(guardianClass, 5));
			bp.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Dodge;
				c.Stat = StatType.AC;
				c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
			});
			bp.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalCMD;
				c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
			});
		});
	}

	private static BlueprintFeature CreateStriker(BlueprintCharacterClassReference guardianClass)
	{
		return Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianRoleStrikerFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Guardian Role: Striker");
			bp.SetDescription(Main.IsekaiContext, "This guardian is a striker: it exists to bring enemies down. It gains a competence bonus on attack and damage rolls of +1, plus 1 per five guardian levels (+5 at level 20).");
			((BlueprintUnitFact)bp).m_Icon = Icon_Striker;
			bp.IsClassFeature = true;
			bp.ReapplyOnLevelUp = true;
			bp.AddComponent(LevelRank(guardianClass, 5));
			bp.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.AdditionalAttackBonus;
				c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
			});
			bp.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.Competence;
				c.Stat = StatType.AdditionalDamage;
				c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
			});
		});
	}

	private static BlueprintFeature CreateController(BlueprintCharacterClassReference guardianClass)
	{
		return Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianRoleControllerFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Guardian Role: Controller");
			bp.SetDescription(Main.IsekaiContext, "This guardian is a controller: it trips, charms and hinders so the party can finish the fight. It gains a bonus to CMB and to the save DCs of its abilities of +1, plus 1 per five guardian levels (+5 at level 20).");
			((BlueprintUnitFact)bp).m_Icon = Icon_Controller;
			bp.IsClassFeature = true;
			bp.ReapplyOnLevelUp = true;
			bp.AddComponent(LevelRank(guardianClass, 5));
			bp.AddComponent(delegate(AddContextStatBonus c)
			{
				c.Descriptor = ModifierDescriptor.UntypedStackable;
				c.Stat = StatType.AdditionalCMB;
				c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
			});
			bp.AddComponent(delegate(IncreaseAllSpellsDC c)
			{
				c.SpellsOnly = false;
				c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
			});
		});
	}

	private static BlueprintFeature CreateSupport(BlueprintCharacterClassReference guardianClass)
	{
		BlueprintBuff auraBuff = TTCoreExtensions.CreateBuff("GuardianRoleSupportBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Guardian's Ward");
			bp.SetDescription(Main.IsekaiContext, "A guardian's ward: a morale bonus on attack rolls and saving throws of +1, +2 once the guardian reaches level 10 and +3 at level 20.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Support;
			bp.AddComponent(LevelRank(guardianClass, 10));
			StatType[] array = new StatType[4]
			{
				StatType.AdditionalAttackBonus,
				StatType.SaveFortitude,
				StatType.SaveReflex,
				StatType.SaveWill
			};
			foreach (StatType stat in array)
			{
				bp.AddComponent(delegate(AddContextStatBonus c)
				{
					c.Descriptor = ModifierDescriptor.Morale;
					c.Stat = stat;
					c.Value = Values.CreateContextRankValue(AbilityRankType.Default);
				});
			}
		});
		BlueprintAbilityAreaEffect auraArea = Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianRoleSupportArea", delegate(BlueprintAbilityAreaEffect bp)
		{
			bp.m_TargetType = BlueprintAbilityAreaEffect.TargetType.Ally;
			bp.SpellResistance = false;
			bp.AggroEnemies = false;
			bp.AffectEnemies = false;
			bp.Shape = AreaEffectShape.Cylinder;
			bp.Size = new Feet(30f);
			bp.Fx = new PrefabLink();
			bp.AddUnconditionalAuraEffect(auraBuff.ToReference<BlueprintBuffReference>());
		});
		BlueprintBuff auraAreaBuff = TTCoreExtensions.CreateBuff("GuardianRoleSupportAreaBuff", delegate(BlueprintBuff bp)
		{
			bp.SetName(Main.IsekaiContext, "Guardian's Ward Aura");
			bp.SetDescription(Main.IsekaiContext, "Allies within 30 feet of the guardian gain a morale bonus on attack rolls and saving throws.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Support;
			bp.IsClassFeature = true;
			bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
			bp.AddComponent(delegate(AddAreaEffect c)
			{
				c.m_AreaEffect = auraArea.ToReference<BlueprintAbilityAreaEffectReference>();
			});
		});
		return Helpers.CreateBlueprint(Main.IsekaiContext, "GuardianRoleSupportFeature", delegate(BlueprintFeature bp)
		{
			bp.SetName(Main.IsekaiContext, "Guardian Role: Support");
			bp.SetDescription(Main.IsekaiContext, "This guardian is a support: it keeps the party standing. Allies within 30 feet of it, the guardian included, gain a morale bonus on attack rolls and saving throws of +1, +2 once the guardian reaches level 10 and +3 at level 20.");
			((BlueprintUnitFact)bp).m_Icon = Icon_Support;
			bp.IsClassFeature = true;
			bp.AddComponent(delegate(AuraFeatureComponent c)
			{
				c.m_Buff = auraAreaBuff.ToReference<BlueprintBuffReference>();
			});
		});
	}
}
