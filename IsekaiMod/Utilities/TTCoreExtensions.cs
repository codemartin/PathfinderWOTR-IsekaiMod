using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using IsekaiMod.Content.Classes.IsekaiProtagonist;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Utilities
{
	internal class TTCoreExtensions
	{
		private static readonly Dictionary<BlueprintSpellList, HashSet<BlueprintGuid>> _spellListCache = new Dictionary<BlueprintSpellList, HashSet<BlueprintGuid>>();

		public static void RegisterClass(BlueprintCharacterClass classToRegister)
		{
			if (classToRegister != null)
			{
				if (ContainsClass(ClassTools.Classes.AllClasses, classToRegister))
				{
					Main.IsekaiContext.Logger.LogWarning("class already registered= " + classToRegister.name + " gui id=" + classToRegister.AssetGuid.m_Guid.ToString("N"));
				}
				else if (BlueprintRoot.Instance?.Progression != null)
				{
					BlueprintRoot.Instance.Progression.m_CharacterClasses = ClassTools.ClassReferences.AllClasses.AddToArray(classToRegister.ToReference<BlueprintCharacterClassReference>());
				}
			}
		}

		public static void RegisterSpell(BlueprintSpellList list, BlueprintAbility spell, int level)
		{
			if (list != null && spell != null && !ContainsSpell(list, spell))
			{
				spell.AddToSpellList(list, level);
				if (_spellListCache.TryGetValue(list, out var value))
				{
					value.Add(spell.AssetGuid);
				}
			}
		}

		public static void RegisterForMythicSpellbook(BlueprintFeatureSelectMythicSpellbook mythicSpellbook, BlueprintSpellbook spellBook)
		{
			if (mythicSpellbook != null && spellBook != null)
			{
				if (mythicSpellbook.m_AllowedSpellbooks != null && ContainsSpellbook(mythicSpellbook.m_AllowedSpellbooks, spellBook))
				{
					Main.IsekaiContext.Logger.LogWarning("spellbook already registered= " + spellBook.name + " gui id=" + spellBook.AssetGuid.m_Guid.ToString("N") + " for mythic= " + mythicSpellbook.Name);
				}
				else
				{
					mythicSpellbook.m_AllowedSpellbooks = (mythicSpellbook.m_AllowedSpellbooks ?? new BlueprintSpellbookReference[0]).AddToArray(spellBook.ToReference<BlueprintSpellbookReference>());
				}
			}
		}

		private static bool ContainsSpell(BlueprintSpellList list, BlueprintAbility spell)
		{
			if (list == null || spell == null)
			{
				return false;
			}
			if (!_spellListCache.TryGetValue(list, out var value))
			{
				value = new HashSet<BlueprintGuid>();
				if (list.SpellsByLevel != null)
				{
					SpellLevelList[] spellsByLevel = list.SpellsByLevel;
					foreach (SpellLevelList spellLevelList in spellsByLevel)
					{
						if (spellLevelList?.Spells == null)
						{
							continue;
						}
						foreach (BlueprintAbility spell2 in spellLevelList.Spells)
						{
							if (spell2 != null)
							{
								value.Add(spell2.AssetGuid);
							}
						}
					}
				}
				_spellListCache[list] = value;
			}
			return value.Contains(spell.AssetGuid);
		}

		private static bool ContainsClass(BlueprintCharacterClass[] array, BlueprintCharacterClass classToCheck)
		{
			if (array == null || classToCheck == null)
			{
				return false;
			}
			foreach (BlueprintCharacterClass blueprintCharacterClass in array)
			{
				if (blueprintCharacterClass != null && blueprintCharacterClass.AssetGuid == classToCheck.AssetGuid)
				{
					return true;
				}
			}
			return false;
		}

		private static bool ContainsSpellbook(BlueprintSpellbookReference[] array, BlueprintSpellbook classToCheck)
		{
			if (array == null || classToCheck == null)
			{
				return false;
			}
			BlueprintGuid assetGuid = classToCheck.AssetGuid;
			foreach (BlueprintSpellbookReference blueprintSpellbookReference in array)
			{
				if (blueprintSpellbookReference != null)
				{
					if (((BlueprintReferenceBase)blueprintSpellbookReference).deserializedGuid == assetGuid)
					{
						return true;
					}
				}
				else
				{
					Main.IsekaiContext.Logger.LogWarning("prestige class spellbook array contained null value");
				}
			}
			return false;
		}

		public static BlueprintCheck CreateCheck(string name, Action<BlueprintCheck> init = null)
		{
			BlueprintCheck blueprintCheck = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintCheck bp)
			{
				bp.ShowOnce = false;
				bp.ShowOnceCurrentDialog = false;
				bp.DCModifiers = new DCModifier[0];
				bp.Conditions = ActionFlow.EmptyCondition();
				bp.Experience = DialogExperience.NormalExperience;
			});
			init?.Invoke(blueprintCheck);
			return blueprintCheck;
		}

		public static BlueprintAnswer CreateAnswer(string name, Action<BlueprintAnswer> init = null)
		{
			BlueprintAnswer blueprintAnswer = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintAnswer bp)
			{
				bp.NextCue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference>(),
					Strategy = Strategy.First
				};
				bp.ShowOnce = false;
				bp.ShowOnceCurrentDialog = false;
				bp.ShowCheck = new ShowCheck
				{
					Type = StatType.Unknown,
					DC = 0
				};
				bp.Experience = DialogExperience.NoExperience;
				bp.DebugMode = false;
				bp.CharacterSelection = new CharacterSelection
				{
					SelectionType = CharacterSelection.Type.Clear,
					ComparisonStats = new StatType[0]
				};
				if (Main.IsekaiContext.AddedContent.Isekai.IsEnabled("Isekai Protagonist"))
				{
					bp.ShowConditions = ActionFlow.IfAll(new PlayerSignificantClassIs
					{
						Not = false,
						CheckGroup = false,
						m_CharacterClass = IsekaiProtagonistClass.GetReference()
					});
				}
				else
				{
					bp.ShowConditions = ActionFlow.IfAll();
				}
				bp.SelectConditions = ActionFlow.EmptyCondition();
				bp.RequireValidCue = false;
				bp.AddToHistory = true;
				bp.OnSelect = ActionFlow.DoNothing();
				bp.FakeChecks = new CheckData[0];
				bp.AlignmentShift = new AlignmentShift
				{
					Direction = AlignmentShiftDirection.TrueNeutral,
					Value = 0,
					Description = new LocalizedString()
				};
			});
			init?.Invoke(blueprintAnswer);
			return blueprintAnswer;
		}

		public static BlueprintCue CreateCue(string name, Action<BlueprintCue> init = null)
		{
			BlueprintCue blueprintCue = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintCue bp)
			{
				bp.Speaker = new DialogSpeaker
				{
					m_Blueprint = null,
					MoveCamera = true
				};
				bp.ShowOnce = false;
				bp.ShowOnceCurrentDialog = false;
				bp.Conditions = ActionFlow.EmptyCondition();
				bp.Experience = DialogExperience.NoExperience;
				bp.TurnSpeaker = true;
				bp.Animation = DialogAnimation.None;
				bp.OnShow = ActionFlow.DoNothing();
				bp.OnStop = ActionFlow.DoNothing();
				bp.AlignmentShift = new AlignmentShift
				{
					Direction = AlignmentShiftDirection.TrueNeutral,
					Value = 0,
					Description = new LocalizedString()
				};
				bp.Answers = new List<BlueprintAnswerBaseReference>();
				bp.Continue = new CueSelection
				{
					Cues = new List<BlueprintCueBaseReference>(),
					Strategy = Strategy.First
				};
			});
			init?.Invoke(blueprintCue);
			return blueprintCue;
		}

		public static BlueprintBuff CreateBuff(string name, Action<BlueprintBuff> init = null)
		{
			BlueprintBuff blueprintBuff = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintBuff bp)
			{
				bp.FxOnStart = new PrefabLink();
				bp.FxOnRemove = new PrefabLink();
			});
			init?.Invoke(blueprintBuff);
			return blueprintBuff;
		}

		public static BlueprintActivatableAbility CreateActivatableAbility(string name, Action<BlueprintActivatableAbility> init = null)
		{
			BlueprintActivatableAbility blueprintActivatableAbility = Helpers.CreateBlueprint(Main.IsekaiContext, name, delegate(BlueprintActivatableAbility bp)
			{
				bp.IsOnByDefault = true;
				bp.DeactivateImmediately = true;
				bp.ActivationType = AbilityActivationType.Immediately;
			});
			init?.Invoke(blueprintActivatableAbility);
			return blueprintActivatableAbility;
		}

		public static BlueprintFeature CreateToggleBuffFeature(string name, string description, Sprite icon, Action<BlueprintBuff> buffEffect = null)
		{
			string text = string.Concat(name.Select((char x) => (!char.IsUpper(x)) ? x.ToString() : (" " + x))).TrimStart(' ');
			return CreateToggleBuffFeature(name, text, description, text, description, icon, buffEffect);
		}

		public static BlueprintFeature CreateToggleBuffFeature(string name, string displayName, string description, Sprite icon, Action<BlueprintBuff> buffEffect = null)
		{
			return CreateToggleBuffFeature(name, displayName, description, displayName, description, icon, buffEffect);
		}

		public static BlueprintFeature CreateToggleBuffFeature(string name, string displayName, string description, string displayNameBuff, string descriptionBuff, Sprite icon, Action<BlueprintBuff> buffEffect = null)
		{
			LocalizedString displayDesc = Helpers.CreateString(Main.IsekaiContext, name + ".Description", description);
			LocalizedString buffDesc = Helpers.CreateString(Main.IsekaiContext, name + "Buff.Description", descriptionBuff);
			BlueprintBuff buff = CreateBuff(name + "Buff", delegate(BlueprintBuff bp)
			{
				bp.SetName(Main.IsekaiContext, displayNameBuff);
				bp.SetDescription(buffDesc);
				((BlueprintUnitFact)bp).m_Icon = icon;
				bp.IsClassFeature = true;
			});
			buffEffect?.Invoke(buff);
			BlueprintActivatableAbility ability = CreateActivatableAbility(name + "Ability", delegate(BlueprintActivatableAbility bp)
			{
				bp.SetName(Main.IsekaiContext, displayName);
				bp.SetDescription(displayDesc);
				((BlueprintUnitFact)bp).m_Icon = icon;
				bp.m_Buff = buff.ToReference<BlueprintBuffReference>();
			});
			return Helpers.CreateBlueprint(Main.IsekaiContext, name + "Feature", delegate(BlueprintFeature bp)
			{
				bp.SetName(Main.IsekaiContext, displayName);
				bp.SetDescription(displayDesc);
				((BlueprintUnitFact)bp).m_Icon = icon;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { ability.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}

		public static BlueprintFeature CreateToggleAuraBuffFeature(string name, string description, string descriptionBuff, Sprite icon, BlueprintAbilityAreaEffect.TargetType targetType, Feet auraSize, bool affectEnemies = false, Action<BlueprintBuff> buffEffect = null)
		{
			string text = string.Concat(name.Select((char x) => (!char.IsUpper(x)) ? x.ToString() : (" " + x))).TrimStart(' ');
			LocalizedString displayName = Helpers.CreateString(Main.IsekaiContext, name + ".Name", text);
			LocalizedString displayDesc = Helpers.CreateString(Main.IsekaiContext, name + ".Description", description);
			LocalizedString displayDescBuff = Helpers.CreateString(Main.IsekaiContext, name + "Buff.Description", descriptionBuff);
			BlueprintBuff buff = CreateBuff(name + "Buff", delegate(BlueprintBuff bp)
			{
				bp.SetName(displayName);
				bp.SetDescription(displayDescBuff);
				bp.IsClassFeature = true;
				((BlueprintUnitFact)bp).m_Icon = icon;
			});
			buffEffect?.Invoke(buff);
			return CreateToggleAuraFeature(name, displayName, displayDesc, icon, delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.m_TargetType = targetType;
				bp.Size = auraSize;
				bp.AffectEnemies = affectEnemies;
				bp.AddUnconditionalAuraEffect(buff.ToReference<BlueprintBuffReference>());
			});
		}

		public static BlueprintFeature CreateToggleAuraFeature(string name, LocalizedString displayName, LocalizedString displayDesc, Sprite icon, Action<BlueprintAbilityAreaEffect> areaEffect = null)
		{
			BlueprintAbilityAreaEffect area = Helpers.CreateBlueprint(Main.IsekaiContext, name + "Area", delegate(BlueprintAbilityAreaEffect bp)
			{
				bp.Shape = AreaEffectShape.Cylinder;
				bp.Fx = new PrefabLink();
				bp.AggroEnemies = false;
			});
			areaEffect?.Invoke(area);
			BlueprintBuff areaBuff = CreateBuff(name + "AreaBuff", delegate(BlueprintBuff bp)
			{
				bp.SetName(displayName);
				bp.SetDescription(displayDesc);
				((BlueprintUnitFact)bp).m_Icon = icon;
				bp.IsClassFeature = true;
				bp.m_Flags = BlueprintBuff.Flags.HiddenInUi;
				bp.AddComponent(delegate(AddAreaEffect c)
				{
					c.m_AreaEffect = area.ToReference<BlueprintAbilityAreaEffectReference>();
				});
			});
			BlueprintActivatableAbility ability = CreateActivatableAbility(name + "Ability", delegate(BlueprintActivatableAbility bp)
			{
				bp.SetName(displayName);
				bp.SetDescription(displayDesc);
				((BlueprintUnitFact)bp).m_Icon = icon;
				bp.m_Buff = areaBuff.ToReference<BlueprintBuffReference>();
				bp.DoNotTurnOffOnRest = true;
			});
			return Helpers.CreateBlueprint(Main.IsekaiContext, name + "Feature", delegate(BlueprintFeature bp)
			{
				bp.SetName(displayName);
				bp.SetDescription(displayDesc);
				((BlueprintUnitFact)bp).m_Icon = icon;
				bp.AddComponent(delegate(AddFacts c)
				{
					c.m_Facts = new BlueprintUnitFactReference[1] { ability.ToReference<BlueprintUnitFactReference>() };
				});
			});
		}
	}
}
