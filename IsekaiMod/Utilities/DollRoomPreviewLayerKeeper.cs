using System.Collections.Generic;
using UnityEngine;

namespace IsekaiMod.Utilities;

internal sealed class DollRoomPreviewLayerKeeper : MonoBehaviour
{
	internal string FormName;

	private static readonly List<Renderer> s_Renderers = new List<Renderer>();

	private void LateUpdate()
	{
		GetComponentsInChildren(includeInactive: true, s_Renderers);
		foreach (Renderer s_Renderer in s_Renderers)
		{
			if (s_Renderer != null && s_Renderer.gameObject.layer != 15)
			{
				s_Renderer.gameObject.layer = 15;
			}
		}
		s_Renderers.Clear();
	}
}
