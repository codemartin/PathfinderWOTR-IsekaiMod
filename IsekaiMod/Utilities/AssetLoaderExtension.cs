using System;
using System.IO;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.ResourceManagement;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace IsekaiMod.Utilities
{
	internal class AssetLoaderExtension : AssetLoader
	{
		public static Sprite LoadPortrait(string imagePath, Vector2Int size)
		{
			if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
			{
				return null;
			}
			try
			{
				byte[] data = File.ReadAllBytes(imagePath);
				Texture2D obj = new Texture2D(size.x, size.y, TextureFormat.RGBA32, mipChain: false)
				{
					mipMapBias = 15f
				};
				obj.LoadImage(data);
				return Sprite.Create(obj, new Rect(0f, 0f, size.x, size.y), new Vector2(0f, 0f));
			}
			catch (Exception ex)
			{
				Main.IsekaiContext.Logger.LogError("Failed to load portrait at " + imagePath + ": " + ex.Message);
				return null;
			}
		}

		public static PortraitData LoadPortraitData(string folder)
		{
			string path = Path.Combine(Main.IsekaiContext.ModEntry.Path, "Assets", "Portraits", folder);
			string path2 = Path.Combine(path, "Small.png");
			string path3 = Path.Combine(path, "Medium.png");
			string path4 = Path.Combine(path, "FullLength.png");
			CustomPortraitHandle smallPortraitHandle = CreateCustomPortraitHandle(path2, PortraitType.SmallPortrait, new Vector2Int(185, 242));
			CustomPortraitHandle halfPortraitHandle = CreateCustomPortraitHandle(path3, PortraitType.HalfLengthPortrait, new Vector2Int(330, 432));
			CustomPortraitHandle fullPortraitHandle = CreateCustomPortraitHandle(path4, PortraitType.FullLengthPortrait, new Vector2Int(692, 1024));
			return new PortraitData(folder)
			{
				SmallPortraitHandle = smallPortraitHandle,
				HalfPortraitHandle = halfPortraitHandle,
				FullPortraitHandle = fullPortraitHandle
			};
		}

		private static CustomPortraitHandle CreateCustomPortraitHandle(string path, PortraitType type, Vector2Int size)
		{
			ResourceStorage<Sprite> storage = ((CustomPortraitsManager.Instance != null) ? CustomPortraitsManager.Instance.Storage : null);
			return new CustomPortraitHandle(path, type, storage)
			{
				Request = new SpriteLoadingRequest(path)
				{
					Resource = LoadPortrait(path, size)
				}
			};
		}
	}
}
