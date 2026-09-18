using System;
using System.Collections.Generic;
using System.Linq;

namespace IsekaiMod.Content.Constellations
{
	public static class ConstellationEventHistory
	{
		private static readonly List<ConstellationMessageEvent> _events = new List<ConstellationMessageEvent>();

		private static readonly object _lock = new object();

		public const int MaxStoredEvents = 500;

		public static void RecordEvent(string message, string sponsor = null, ConstellationCategory category = ConstellationCategory.Subclass, int coins = 0, string sceneContext = null)
		{
			if (string.IsNullOrEmpty(message))
			{
				return;
			}
			lock (_lock)
			{
				_events.Add(new ConstellationMessageEvent(message, sponsor, category, coins, sceneContext));
				if (_events.Count > 500)
				{
					_events.RemoveAt(0);
				}
			}
		}

		public static List<ConstellationMessageEvent> GetEvents(string sponsorFilter = null, ConstellationCategory categoryFilter = ConstellationCategory.All, string searchFilter = null)
		{
			lock (_lock)
			{
				IEnumerable<ConstellationMessageEvent> source = _events;
				if (!string.IsNullOrEmpty(sponsorFilter) && sponsorFilter != "All Sponsors")
				{
					source = source.Where((ConstellationMessageEvent e) => e.Sponsor != null && e.Sponsor.IndexOf(sponsorFilter, StringComparison.OrdinalIgnoreCase) >= 0);
				}
				if (categoryFilter != ConstellationCategory.All)
				{
					source = source.Where((ConstellationMessageEvent e) => e.Category == categoryFilter);
				}
				if (!string.IsNullOrEmpty(searchFilter))
				{
					source = source.Where((ConstellationMessageEvent e) => (e.Message != null && e.Message.IndexOf(searchFilter, StringComparison.OrdinalIgnoreCase) >= 0) || (e.Sponsor != null && e.Sponsor.IndexOf(searchFilter, StringComparison.OrdinalIgnoreCase) >= 0) || (e.SceneContext != null && e.SceneContext.IndexOf(searchFilter, StringComparison.OrdinalIgnoreCase) >= 0));
				}
				return source.ToList();
			}
		}

		public static int GetTotalCount()
		{
			lock (_lock)
			{
				return _events.Count;
			}
		}

		public static void Clear()
		{
			lock (_lock)
			{
				_events.Clear();
			}
		}
	}
}
