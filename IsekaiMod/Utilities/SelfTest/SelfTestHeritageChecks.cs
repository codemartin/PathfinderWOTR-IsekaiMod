using System.Collections.Generic;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;

namespace IsekaiMod.Utilities.SelfTest;

internal static class SelfTestHeritageChecks
{
	private const string HumanRaceGuid = "0a5d473ead98b0646b94495af250fdc4";

	internal static SelfTestChecks.Outcome HumanHeritageDefault()
	{
		List<string> list = new List<string>();
		BlueprintFeatureSelection selection = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.IsekaiContext, "IsekaiHumanHeritageSelection");
		BlueprintFeature modBlueprint = BlueprintTools.GetModBlueprint<BlueprintFeature>(Main.IsekaiContext, "IsekaiHumanHeritageNoFeat");
		if (selection == null || modBlueprint == null)
		{
			return SelfTestChecks.Outcome.Of(passed: false, "IsekaiHumanHeritageSelection or IsekaiHumanHeritageNoFeat was not created.");
		}
		BlueprintRace blueprint = BlueprintTools.GetBlueprint<BlueprintRace>("0a5d473ead98b0646b94495af250fdc4");
		bool num = blueprint != null && blueprint.m_Features?.Any((BlueprintFeatureBaseReference f) => f != null && ((BlueprintReferenceBase)f).deserializedGuid == selection.AssetGuid) == true;
		List<IFeatureSelectionItem> list2 = selection.Items.ToList();
		BlueprintFeature blueprintFeature = list2.FirstOrDefault() as BlueprintFeature;
		list.Add("Options, in the order the engine offers them: " + string.Join(", ", list2.Select((IFeatureSelectionItem i) => (i as BlueprintFeature)?.name ?? "(unnamed)")));
		if (!num)
		{
			return SelfTestChecks.Outcome.Of(passed: true, "The Isekai human heritages are not offered in this setup (the human race does not carry the selection); nothing to auto-pick.", list);
		}
		bool flag = blueprintFeature != null && blueprintFeature.AssetGuid == modBlueprint.AssetGuid;
		string summary = (flag ? $"The human race carries IsekaiHumanHeritageSelection and its first option, the one every auto-levelled human takes, is '{modBlueprint.name}' ({list2.Count} options)." : ("The first option of IsekaiHumanHeritageSelection is '" + (blueprintFeature?.name ?? "nothing") + "', not '" + modBlueprint.name + "': every companion, pregen, mercenary and human NPC levelled by the game takes it."));
		return SelfTestChecks.Outcome.Of(flag, summary, list);
	}
}
