using System;
using System.Collections.Generic;

namespace IsekaiMod.Utilities;

internal sealed class DeferredOffers
{
	private readonly List<Action> pending = new List<Action>();

	internal int PendingCount => pending.Count;

	internal void Offer(Action addToSelections, bool offeredNow)
	{
		if (addToSelections != null)
		{
			if (offeredNow)
			{
				addToSelections();
			}
			else
			{
				pending.Add(addToSelections);
			}
		}
	}

	internal int Release(bool offeredNow, Action<Exception> onError = null)
	{
		if (!offeredNow || pending.Count == 0)
		{
			return 0;
		}
		Action[] array = pending.ToArray();
		pending.Clear();
		int num = 0;
		Action[] array2 = array;
		foreach (Action action in array2)
		{
			try
			{
				action();
				num++;
			}
			catch (Exception obj)
			{
				onError?.Invoke(obj);
			}
		}
		return num;
	}
}
