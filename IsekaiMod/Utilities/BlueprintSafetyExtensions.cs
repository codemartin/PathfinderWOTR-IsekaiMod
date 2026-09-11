using System;
using System.Collections.Generic;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Localization;

namespace IsekaiMod.Utilities
{
	public static class BlueprintSafetyExtensions
	{
		public static string SafeGetName(this BlueprintUnitFact fact, string fallback = "")
		{
			if (fact == null)
			{
				return fallback;
			}
			try
			{
				if (fact.m_DisplayName != null)
				{
					LocalizationPack currentPack = LocalizationManager.CurrentPack;
					if (currentPack != null)
					{
						string text = fact.m_DisplayName.LoadString(currentPack, LocalizationManager.CurrentLocale);
						if (!string.IsNullOrEmpty(text))
						{
							return text;
						}
					}
				}
				return fact.Name ?? fact.name ?? fallback;
			}
			catch
			{
				return fact.name ?? fallback;
			}
		}

		public static string SafeGetDescription(this BlueprintUnitFact fact, string fallback = "")
		{
			if (fact == null)
			{
				return fallback;
			}
			try
			{
				if (fact.m_Description != null)
				{
					LocalizationPack currentPack = LocalizationManager.CurrentPack;
					if (currentPack != null)
					{
						string text = fact.m_Description.LoadString(currentPack, LocalizationManager.CurrentLocale);
						if (!string.IsNullOrEmpty(text))
						{
							return text;
						}
					}
				}
				return fact.Description ?? fallback;
			}
			catch
			{
				return fallback;
			}
		}

		public static IEnumerable<T> SafeResolved<T>(this IEnumerable<BlueprintReference<T>> refs) where T : SimpleBlueprint
		{
			if (refs == null)
			{
				yield break;
			}
			foreach (BlueprintReference<T> @ref in refs)
			{
				if (@ref != null)
				{
					T val = null;
					try
					{
						val = @ref.Get();
					}
					catch
					{
					}
					if (val != null)
					{
						yield return val;
					}
				}
			}
		}

		public static T[] EmptyIfNull<T>(this T[] array)
		{
			return array ?? Array.Empty<T>();
		}

		public static UnitEntityData SafeGetMainCharacter(this Player player)
		{
			try
			{
				return player?.MainCharacter.Value;
			}
			catch
			{
				return null;
			}
		}

		public static UnitEntityData SafeGetMainCharacter()
		{
			try
			{
				return (Game.Instance?.Player).SafeGetMainCharacter();
			}
			catch
			{
				return null;
			}
		}

		public static T SafeGetBlueprint<T>(string guidStr) where T : BlueprintScriptableObject
		{
			if (string.IsNullOrEmpty(guidStr))
			{
				return null;
			}
			try
			{
				return ResourcesLibrary.TryGetBlueprint<T>(BlueprintGuid.Parse(guidStr));
			}
			catch
			{
				return null;
			}
		}
	}
}
