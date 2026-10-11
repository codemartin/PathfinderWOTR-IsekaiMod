using System;
using System.IO;
using IsekaiMod.Config;
using Kingmaker.PubSubSystem;
using TabletopTweaks.Core.Localization;
using TabletopTweaks.Core.ModLogic;
using UnityModManagerNet;

namespace IsekaiMod.ModLogic;

internal class ModContextTTTBase : ModContextBase
{
	public AddedContent AddedContent;

	public ModContextTTTBase(UnityModManager.ModEntry ModEntry)
		: base(ModEntry)
	{
		LoadAllSettings();
	}

	public override void LoadAllSettings()
	{
		LoadSettings("AddedContent.json", "IsekaiMod.Config", ref AddedContent);
		LoadBlueprints("IsekaiMod.Config", this);
		LoadLocalization("IsekaiMod.Localization");
	}

	public override void LoadLocalization(string classPath)
	{
		string resourceName = classPath + "." + LocalizationFile;
		string loosePath = Path.Combine(LocalizationFolder, LocalizationFile);
		Func<Stream> embedded = () => typeof(ModContextTTTBase).Assembly.GetManifestResourceStream(resourceName);
		Report(() => LocalizationPackRepair.RestoreIfMissing(loosePath, embedded), loosePath);
		try
		{
			base.LoadLocalization(classPath);
			if (LocalizationPackRepair.HasStrings(ModLocalizationPack))
			{
				return;
			}
			Logger.LogError("[Localization] TabletopTweaks-Core loaded an empty LocalizationPack.json; using the copy inside IsekaiMod.dll.");
			try
			{
				EventBus.Unsubscribe(ModLocalizationPack);
			}
			catch
			{
			}
		}
		catch (Exception ex)
		{
			Logger.LogError("[Localization] TabletopTweaks-Core could not load LocalizationPack.json (" + ex.GetType().Name + ": " + ex.Message + "); using the copy inside IsekaiMod.dll.");
		}
		Report(() => LocalizationPackRepair.Restore(loosePath, embedded), loosePath);
		MultiLocalizationPack multiLocalizationPack = null;
		try
		{
			using Stream stream = embedded();
			multiLocalizationPack = LocalizationPackRepair.Read(stream);
		}
		catch (Exception ex2)
		{
			Logger.LogError("[Localization] The copy inside IsekaiMod.dll could not be read either: " + ex2.Message);
		}
		ModLocalizationPack = multiLocalizationPack ?? new MultiLocalizationPack();
		ModLocalizationPack.Context = this;
		EventBus.Subscribe(ModLocalizationPack);
	}

	private void Report(Func<LocalizationPackRepair.Outcome> repair, string loosePath)
	{
		try
		{
			LocalizationPackRepair.Outcome outcome = repair();
			if (outcome != LocalizationPackRepair.Outcome.Kept)
			{
				Logger.LogError("[Localization] " + LocalizationPackRepair.Describe(outcome, loosePath));
			}
		}
		catch (Exception ex)
		{
			Logger.LogError("[Localization] Could not repair " + loosePath + ": " + ex.Message);
		}
	}

	public override void AfterBlueprintCachePatches()
	{
		base.AfterBlueprintCachePatches();
		if (Debug)
		{
			Blueprints.RemoveUnused();
			SaveSettings(BlueprintsFile, Blueprints);
			ModLocalizationPack.RemoveUnused();
			SaveLocalization(ModLocalizationPack);
		}
	}

	public override void SaveAllSettings()
	{
		base.SaveAllSettings();
		SaveSettings("AddedContent.json", AddedContent);
	}
}
