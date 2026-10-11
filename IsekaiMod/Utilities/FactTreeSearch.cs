using System;
using System.Collections.Generic;

namespace IsekaiMod.Utilities;

internal static class FactTreeSearch
{
	internal static T FirstMatch<T>(T root, Func<T, IEnumerable<T>> children, Func<T, bool> match, int maxDepth = 5) where T : class
	{
		if (root == null || children == null || match == null)
		{
			return null;
		}
		List<KeyValuePair<T, int>> list = new List<KeyValuePair<T, int>>
		{
			new KeyValuePair<T, int>(root, 0)
		};
		HashSet<T> hashSet = new HashSet<T>();
		hashSet.Add(root);
		for (int i = 0; i < list.Count; i++)
		{
			KeyValuePair<T, int> keyValuePair = list[i];
			if (match(keyValuePair.Key))
			{
				return keyValuePair.Key;
			}
			if (keyValuePair.Value >= maxDepth)
			{
				continue;
			}
			IEnumerable<T> enumerable;
			try
			{
				enumerable = children(keyValuePair.Key);
			}
			catch
			{
				continue;
			}
			if (enumerable == null)
			{
				continue;
			}
			foreach (T item in enumerable)
			{
				if (item != null && hashSet.Add(item))
				{
					list.Add(new KeyValuePair<T, int>(item, keyValuePair.Value + 1));
				}
			}
		}
		return null;
	}
}
