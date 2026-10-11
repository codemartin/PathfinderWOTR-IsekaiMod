using System;
using System.IO;
using Newtonsoft.Json;
using TabletopTweaks.Core.Localization;

namespace IsekaiMod.ModLogic;

internal static class LocalizationPackRepair
{
	internal enum Outcome
	{
		Kept,
		RestoredMissing,
		RestoredBroken,
		NoEmbeddedCopy
	}

	internal static bool HasStrings(MultiLocalizationPack pack)
	{
		if (pack?.Strings != null)
		{
			return pack.Strings.Count > 0;
		}
		return false;
	}

	internal static MultiLocalizationPack Read(Stream stream)
	{
		if (stream == null)
		{
			return null;
		}
		try
		{
			using StreamReader streamReader = new StreamReader(stream);
			return JsonConvert.DeserializeObject<MultiLocalizationPack>(streamReader.ReadToEnd());
		}
		catch
		{
			return null;
		}
	}

	internal static Outcome RestoreIfMissing(string loosePath, Func<Stream> embedded)
	{
		FileInfo fileInfo = new FileInfo(loosePath);
		if (fileInfo.Exists && fileInfo.Length > 0)
		{
			return Outcome.Kept;
		}
		return Restore(loosePath, embedded);
	}

	internal static Outcome Restore(string loosePath, Func<Stream> embedded)
	{
		bool flag = File.Exists(loosePath) && new FileInfo(loosePath).Length > 0;
		byte[] bytes;
		using (Stream stream = embedded?.Invoke())
		{
			if (stream == null)
			{
				return Outcome.NoEmbeddedCopy;
			}
			using MemoryStream memoryStream = new MemoryStream();
			stream.CopyTo(memoryStream);
			bytes = memoryStream.ToArray();
		}
		string directoryName = Path.GetDirectoryName(loosePath);
		if (!string.IsNullOrEmpty(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		if (flag)
		{
			File.Copy(loosePath, Path.Combine(directoryName ?? "", "BROKEN_" + Path.GetFileName(loosePath)), overwrite: true);
		}
		File.WriteAllBytes(loosePath, bytes);
		if (!flag)
		{
			return Outcome.RestoredMissing;
		}
		return Outcome.RestoredBroken;
	}

	internal static string Describe(Outcome outcome, string loosePath)
	{
		return outcome switch
		{
			Outcome.RestoredMissing => loosePath + " was missing; it was restored from the copy inside IsekaiMod.dll.", 
			Outcome.RestoredBroken => loosePath + " could not be read or held no text; it was kept as BROKEN_" + Path.GetFileName(loosePath) + " and restored from the copy inside IsekaiMod.dll.", 
			Outcome.NoEmbeddedCopy => loosePath + " is missing or broken and IsekaiMod.dll carries no copy to restore it from.", 
			_ => loosePath + " is in place.", 
		};
	}
}
