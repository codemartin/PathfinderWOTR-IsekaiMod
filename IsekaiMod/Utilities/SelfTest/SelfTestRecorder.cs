using System;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestRecorder
{
	internal sealed class CreatedBlueprint
	{
		public string Name;

		public BlueprintGuid Guid;

		public SimpleBlueprint Blueprint;
	}

	private static readonly object Gate = new object();

	private static readonly List<CreatedBlueprint> Created = new List<CreatedBlueprint>();

	private static readonly List<string> Collisions = new List<string>();

	private static readonly List<string> UnityExceptions = new List<string>();

	private static int unityExceptionsAtMainMenu = -1;

	internal static List<CreatedBlueprint> CreatedBlueprints()
	{
		lock (Gate)
		{
			return new List<CreatedBlueprint>(Created);
		}
	}

	internal static List<string> IdCollisions()
	{
		lock (Gate)
		{
			return new List<string>(Collisions);
		}
	}

	internal static void LoggedUnityExceptions(out List<string> startup, out List<string> duringChecks)
	{
		lock (Gate)
		{
			int count = ((unityExceptionsAtMainMenu < 0) ? UnityExceptions.Count : Math.Min(unityExceptionsAtMainMenu, UnityExceptions.Count));
			startup = UnityExceptions.Take(count).ToList();
			duringChecks = UnityExceptions.Skip(count).ToList();
		}
	}

	internal static void MarkMainMenu()
	{
		lock (Gate)
		{
			if (unityExceptionsAtMainMenu < 0)
			{
				unityExceptionsAtMainMenu = UnityExceptions.Count;
			}
		}
	}

	internal static void AddCreated(string name, BlueprintGuid guid, SimpleBlueprint blueprint)
	{
		lock (Gate)
		{
			Created.Add(new CreatedBlueprint
			{
				Name = name,
				Guid = guid,
				Blueprint = blueprint
			});
		}
	}

	internal static void AddCollision(string text)
	{
		lock (Gate)
		{
			Collisions.Add(text);
		}
	}

	internal static void AddUnityException(string text)
	{
		lock (Gate)
		{
			if (UnityExceptions.Count < 500)
			{
				UnityExceptions.Add(text);
			}
		}
	}
}
