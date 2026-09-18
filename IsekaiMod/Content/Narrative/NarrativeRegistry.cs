using System;
using System.Collections.Generic;
using System.Linq;

namespace IsekaiMod.Content.Narrative
{
	public static class NarrativeRegistry
	{
		private static readonly Dictionary<string, NarrativeScene> _scenes = new Dictionary<string, NarrativeScene>(StringComparer.OrdinalIgnoreCase);

		private static readonly Dictionary<string, NarrativeOption> _options = new Dictionary<string, NarrativeOption>(StringComparer.OrdinalIgnoreCase);

		public static void RegisterScene(NarrativeScene scene)
		{
			if (scene == null)
			{
				throw new ArgumentNullException("scene");
			}
			if (_scenes.ContainsKey(scene.SceneId))
			{
				throw new InvalidOperationException("Duplicate NarrativeScene ID: '" + scene.SceneId + "'");
			}
			_scenes[scene.SceneId] = scene;
			foreach (NarrativeOption option in scene.Options)
			{
				if (_options.ContainsKey(option.Id))
				{
					throw new InvalidOperationException("Duplicate NarrativeOption ID: '" + option.Id + "' in scene '" + scene.SceneId + "'");
				}
				_options[option.Id] = option;
			}
		}

		public static IReadOnlyCollection<NarrativeScene> GetAllScenes()
		{
			return _scenes.Values;
		}

		public static IReadOnlyCollection<NarrativeOption> GetAllOptions()
		{
			return _options.Values;
		}

		public static NarrativeScene GetScene(string sceneId)
		{
			_scenes.TryGetValue(sceneId, out var value);
			return value;
		}

		public static NarrativeOption GetOption(string optionId)
		{
			_options.TryGetValue(optionId, out var value);
			return value;
		}

		public static IEnumerable<NarrativeScene> GetScenesByAct(string act)
		{
			return _scenes.Values.Where((NarrativeScene s) => string.Equals(s.Act, act, StringComparison.OrdinalIgnoreCase));
		}

		public static IEnumerable<NarrativeOption> GetOptionsForArchetype(string proficiencyFactName)
		{
			return _options.Values.Where((NarrativeOption o) => string.Equals(o.RequiredProficiency, proficiencyFactName, StringComparison.OrdinalIgnoreCase));
		}

		public static IEnumerable<NarrativeOption> GetThirdOptions()
		{
			return _options.Values.Where((NarrativeOption o) => o.Type == OptionType.ThirdOption);
		}

		public static void Clear()
		{
			_scenes.Clear();
			_options.Clear();
		}
	}
}
