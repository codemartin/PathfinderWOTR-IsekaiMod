using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.View;
using UnityEngine;

namespace IsekaiMod.Content.Guardians
{
	[TypeId("b6c33cda4b124806a382103fca500271")]
	[ComponentName("Hides skull mesh for Chrono Sprite fairy")]
	[AllowMultipleComponents]
	[AllowedOn(typeof(BlueprintUnitFact), false)]
	public class ChronoSpriteFairyGlowComponent : UnitFactComponentDelegate, IUnitViewAttachedHandler, ISubscriber, IUnitSubscriber, IUnitSpawnHandler, IGlobalSubscriber
	{
		public void HandleUnitViewAttached()
		{
			HideSkull();
		}

		public void HandleUnitSpawned(UnitEntityData unit)
		{
			if (unit == base.Owner)
			{
				HideSkull();
			}
		}

		public override void OnActivate()
		{
			HideSkull();
		}

		public override void OnTurnOn()
		{
			HideSkull();
		}

		private void HideSkull()
		{
			try
			{
				UnitEntityView unitEntityView = base.Owner?.View;
				if (unitEntityView == null)
				{
					return;
				}
				Renderer[] componentsInChildren = unitEntityView.GetComponentsInChildren<Renderer>(includeInactive: true);
				foreach (Renderer renderer in componentsInChildren)
				{
					if (renderer is SkinnedMeshRenderer || renderer is MeshRenderer)
					{
						renderer.enabled = false;
						if (renderer.gameObject != unitEntityView.gameObject)
						{
							renderer.gameObject.SetActive(value: false);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Main.Log(ex.ToString());
			}
		}
	}
}
