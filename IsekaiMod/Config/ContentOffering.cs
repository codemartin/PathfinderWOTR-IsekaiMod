using System.Collections.Generic;
using IsekaiMod.Components;
using IsekaiMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using TabletopTweaks.Core.Config;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Config;

public static class ContentOffering
{
	private static readonly List<(BlueprintFeatureSelection selection, BlueprintFeature feature)> s_DeferredHeritageOffers = new List<(BlueprintFeatureSelection, BlueprintFeature)>();

	private static bool s_HeritageOffersFlushed;

	public static string SettingKey(OfferedContent content)
	{
		return content switch
		{
			OfferedContent.IsekaiProtagonist => "Isekai Protagonist", 
			OfferedContent.IsekaiDialogue => "Isekai Dialogue", 
			OfferedContent.IsekaiHeritages => "Isekai Heritages", 
			OfferedContent.IsekaiBackgrounds => "Isekai Backgrounds", 
			OfferedContent.IsekaiDeities => "Isekai Deities", 
			OfferedContent.ExceptionalFeats => "Exceptional Feats", 
			OfferedContent.MythicClassFeature => "Mythic Class Feature", 
			_ => null, 
		};
	}

	public static bool InOtherGroup(OfferedContent content)
	{
		if (content != OfferedContent.ExceptionalFeats)
		{
			return content == OfferedContent.MythicClassFeature;
		}
		return true;
	}

	public static bool IsOffered(AddedContent settings, OfferedContent content)
	{
		if (settings == null)
		{
			return true;
		}
		SettingGroup settingGroup = (InOtherGroup(content) ? settings.Other : settings.Isekai);
		string text = SettingKey(content);
		if (settingGroup == null || text == null)
		{
			return true;
		}
		if (settingGroup.Settings == null || !settingGroup.Settings.TryGetValue(text, out var value) || value == null)
		{
			return !settingGroup.DisableAll;
		}
		if (value.Enabled)
		{
			return !settingGroup.DisableAll;
		}
		return false;
	}

	public static bool IsOffered(OfferedContent content)
	{
		return IsOffered(Main.IsekaiContext?.AddedContent, content);
	}

	public static bool MayTakeClassLevel(bool offered, int existingClassLevels)
	{
		if (!offered)
		{
			return existingClassLevels > 0;
		}
		return true;
	}

	public static bool ShouldRestrictExceptionalFeats(bool protagonistOffered, bool restrictSetting)
	{
		return protagonistOffered & restrictSetting;
	}

	public static bool ShouldOfferBlessingOfTheMythic(bool mythicClassFeatureOffered, bool protagonistOffered)
	{
		return mythicClassFeatureOffered & protagonistOffered;
	}

	public static void RegisterClass(BlueprintCharacterClass characterClass, OfferedContent content)
	{
		if (characterClass != null)
		{
			characterClass.AddComponent(delegate(PrerequisiteContentOffered c)
			{
				c.Content = content;
				c.HideInUI = true;
			});
			if (!IsOffered(content))
			{
				characterClass.HideIfRestricted = true;
			}
			TTCoreExtensions.RegisterClass(characterClass);
		}
	}

	public static void OfferInSelection(BlueprintFeatureSelection selection, BlueprintFeature feature, OfferedContent content)
	{
		if (selection != null && feature != null && IsOffered(content))
		{
			if (content == OfferedContent.IsekaiHeritages && !s_HeritageOffersFlushed)
			{
				s_DeferredHeritageOffers.Add((selection, feature));
			}
			else
			{
				selection.AddToSelection(feature);
			}
		}
	}

	public static int FlushHeritageOffers()
	{
		s_HeritageOffersFlushed = true;
		int num = 0;
		foreach (var (selection, feature) in s_DeferredHeritageOffers)
		{
			selection.AddToSelection(feature);
			num++;
		}
		s_DeferredHeritageOffers.Clear();
		return num;
	}

	public static void ShowOnlyWhenOffered(BlueprintAnswer answer, OfferedContent content)
	{
		if (answer != null)
		{
			ConditionContentOffered conditionContentOffered = new ConditionContentOffered
			{
				Content = content
			};
			if (answer.ShowConditions == null)
			{
				answer.ShowConditions = ActionFlow.IfAll(conditionContentOffered);
				return;
			}
			if (answer.ShowConditions.Operation == Operation.And)
			{
				answer.ShowConditions.Conditions = (answer.ShowConditions.Conditions ?? new Condition[0]).AppendToArray(conditionContentOffered);
				return;
			}
			ConditionsChecker showConditions = answer.ShowConditions;
			answer.ShowConditions = ActionFlow.IfAll(new OrAndLogic
			{
				ConditionsChecker = showConditions
			}, conditionContentOffered);
		}
	}
}
